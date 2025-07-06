using Netflex.Domain.Entities;
using Netflex.Domain.ValueObjects;

namespace Netflex.Application.UseCases.V1.Auth.Commands;

public record RefreshCommand(string DeviceId, string RefreshToken)
    : ICommand<RefreshResult>;

public record RefreshResult(string AccessToken, string RefreshToken);

public class RefreshHandler(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService,
    IRefreshOptions refreshOptions)
        : ICommandHandler<RefreshCommand, RefreshResult>
{
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly IRefreshOptions _refreshOptions = refreshOptions;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<RefreshResult> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        //STEP 1: Get session by refresh token
        var sessions = await _unitOfWork.Repository<UserSession>()
            .GetAllAsync(x => x.DeviceId == request.DeviceId && !x.IsRevoked, cancellationToken: cancellationToken);

        var userSession = sessions.FirstOrDefault(s => s.RefreshHash.Verify(request.RefreshToken));
        if (userSession is null || !userSession.IsValid(DateTime.UtcNow))
            throw new InvalidRefreshTokenException();

        var user = await _unitOfWork.Repository<User>()
            .GetAsync(x => x.Id == userSession.UserId,
                includeProperties: [nameof(User.Roles), nameof(User.Permissions)],
                cancellationToken: cancellationToken)
            ?? throw new UserNotFoundException();

        //STEP 2: Create new access token and new refresh token
        var acccessToken = _jwtTokenService.GenerateJwt(user, userSession.Id);
        var refreshToken = Guid.NewGuid().ToString();

        userSession.RefreshHash = HashString.Of(refreshToken);
        userSession.ExpiresAt = DateTime.UtcNow.AddDays(_refreshOptions.ExpiresInDays);

        await _unitOfWork.CommitAsync();
        return new RefreshResult(acccessToken, refreshToken);
    }
}