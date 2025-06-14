using Netflex.Domain.Entities;
using Netflex.Domain.ValueObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Netflex.Application.UseCases.V1.Auth.Commands;

public record SigninResult(string AccessToken, string RefreshToken);
public record SigninCommand(string Email, string Password, string DeviceId,
    string? IpAddress = default, string? UserAgent = default)
    : ICommand<SigninResult>;
public class SigninCommandValidator
    : AbstractValidator<SigninCommand>
{
    public SigninCommandValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).Length(6, 20);
    }
}

public class SigninHandler(IJwtTokenService jwtTokenService, IRefreshOptions refreshOptions,
    IUnitOfWork unitOfWork) : ICommandHandler<SigninCommand, SigninResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly IRefreshOptions _refreshOptions = refreshOptions;
    public async Task<SigninResult> Handle(SigninCommand request,
        CancellationToken cancellationToken)
    {
        //STEP 1: Check user credentials 
        var user = await _unitOfWork.Repository<Domain.Entities.User>()
            .GetAsync(u => u.Email == Email.Of(request.Email),
                q => q.Include(u => u.Roles).Include(u => u.Permissions),
                cancellationToken: cancellationToken)
            ?? throw new IncorrectEmailOrPasswordException();

        if (user.PasswordHash is null || !user.PasswordHash.Verify(request.Password))
            throw new IncorrectEmailOrPasswordException();

        //STEP 2: Create access token & refresh token
        var refreshToken = Guid.NewGuid().ToString();

        var userSessionRepository = _unitOfWork.Repository<UserSession>();
        var userSession = await userSessionRepository.GetAsync(x => x.UserId == user.Id
            && x.DeviceId == request.DeviceId && !x.IsRevoked, cancellationToken: cancellationToken);

        var expiresAt = DateTime.UtcNow.AddDays(_refreshOptions.ExpiresInDays);

        //STEP 3: If session is existed, update session with new refresh token
        if (userSession is not null)
        {
            userSession.RefreshHash = HashString.Of(refreshToken);
            userSession.ExpiresAt = expiresAt;
            userSessionRepository.Update(userSession);
        }
        else
        {
            userSession = UserSession.Create(Guid.NewGuid().ToString(), request.DeviceId, HashString.Of(refreshToken),
                user.Id, request.UserAgent, request.IpAddress, expiresAt
            );

            await userSessionRepository.AddAsync(userSession, cancellationToken);
        }

        var accessToken = _jwtTokenService.GenerateJwt(user, userSession.Id);

        await _unitOfWork.CommitAsync();
        return new SigninResult(accessToken, refreshToken);
    }
}