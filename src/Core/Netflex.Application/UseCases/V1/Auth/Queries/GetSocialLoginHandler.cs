using Netflex.Domain.ValueObjects;

namespace Netflex.Application.UseCases.V1.Auth.Queries;

public record GetSocialLoginResult(string LoginUrl);
public record GetSocialLoginQuery(string LoginProvider, string RedirectUrl)
    : IQuery<GetSocialLoginResult>;

public class GetSocialLoginHandler(ISocialLoginServiceFactory socialLoginServiceFactory)
    : IQueryHandler<GetSocialLoginQuery, GetSocialLoginResult>
{
    private readonly ISocialLoginServiceFactory _socialLoginServiceFactory = socialLoginServiceFactory;
    public async Task<GetSocialLoginResult> Handle(GetSocialLoginQuery request,
        CancellationToken cancellationToken)
    {
        var service = _socialLoginServiceFactory.GetByProvider(LoginProvider.Of(request.LoginProvider))
            ?? throw new NotSupportedLoginProviderException();

        var loginUrl = await service.GetLoginUrlAsync(request.RedirectUrl);
        return new GetSocialLoginResult(loginUrl);
    }
}