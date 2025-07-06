using FluentValidation;
using Netflex.Domain.Entities;
using Netflex.Domain.ValueObjects;


namespace Netflex.Application.UseCases.V1.Auth.Commands;

public record SocialLoginCallbackResult(string AccessToken, string RefreshToken);
public record SocialLoginCallbackCommand(string LoginProvider, string Code, string RedirectUrl,
    string DeviceId, string? IpAddress = default, string? UserAgent = default)
    : ICommand<SocialLoginCallbackResult>;
public class SocialLoginCallbackCommandValidator
    : AbstractValidator<SocialLoginCallbackCommand>
{
    public SocialLoginCallbackCommandValidator()
    {
        RuleFor(x => x.LoginProvider).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.RedirectUrl).NotEmpty();
        RuleFor(x => x.DeviceId).NotEmpty();
    }
}
public class SocialLoginCallbackHandler(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService,
    IRefreshOptions refreshOptions, ISocialServiceFactory socialServiceFactory)
    : ICommandHandler<SocialLoginCallbackCommand, SocialLoginCallbackResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ISocialServiceFactory _socialServiceFactory = socialServiceFactory;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly IRefreshOptions _refreshOptions = refreshOptions;
    public async Task<SocialLoginCallbackResult> Handle(SocialLoginCallbackCommand request,
        CancellationToken cancellationToken)
    {
        var service = _socialServiceFactory.GetByProvider(LoginProvider.Of(request.LoginProvider))
            ?? throw new NotSupportedLoginProviderException();

        var info = await service.FetchUserInfoAsync(request.Code, request.RedirectUrl);

        var userRepository = _unitOfWork.Repository<User>();
        var userLoginRepository = _unitOfWork.Repository<UserLogin>();

        var userLogin = await userLoginRepository.GetAsync(ul =>
                ul.ProviderKey == info.Id
                    && ul.LoginProvider == LoginProvider.Of(request.LoginProvider), cancellationToken: cancellationToken);

        var userId = userLogin?.UserId ?? Guid.NewGuid().ToString();

        //STEP 2: Check user login is exist or not. if user login is not, create new user and new user login. 
        if (userLogin is null)
        {
            var userExists = await userRepository.ExistsAsync(u => u.Email == Email.Of(info.Email), cancellationToken);
            if (userExists) throw new EmailAlreadyExistsException();

            var newUser = User.Create(userId, Email.Of(info.Email));
            await userRepository.AddAsync(newUser, cancellationToken);

            userLogin = UserLogin.Create(userId, LoginProvider.Of(request.LoginProvider), info.Id);
            await userLoginRepository.AddAsync(userLogin, cancellationToken);

            await _unitOfWork.CommitAsync();
        }

        //STEP 3: If session is existed, update session with new refresh token
        var refreshToken = Guid.NewGuid().ToString();

        var userSessionRepository = _unitOfWork.Repository<UserSession>();
        var userSession = await userSessionRepository.GetAsync(x => x.UserId == userId
            && x.DeviceId == request.DeviceId && !x.IsRevoked, cancellationToken: cancellationToken);

        var expiresAt = DateTime.UtcNow.AddDays(_refreshOptions.ExpiresInDays);

        if (userSession is not null)
        {
            userSession.RefreshHash = HashString.Of(refreshToken);
            userSession.ExpiresAt = expiresAt;
            userSessionRepository.Update(userSession);
        }
        else
        {
            userSession = UserSession.Create(Guid.NewGuid().ToString(), request.DeviceId, HashString.Of(refreshToken),
                userId, request.UserAgent, request.IpAddress, expiresAt
            );
            await userSessionRepository.AddAsync(userSession, cancellationToken);
        }

        var user = await _unitOfWork.Repository<User>()
            .GetAsync(x => x.Id == userSession.UserId,
                includeProperties: string.Join(",", [nameof(User.Roles), nameof(User.Permissions)]),
                cancellationToken: cancellationToken)
            ?? throw new UserNotFoundException();

        var accessToken = _jwtTokenService.GenerateJwt(user, userSession.Id);

        await _unitOfWork.CommitAsync();

        return new SocialLoginCallbackResult(accessToken, refreshToken);
    }
}