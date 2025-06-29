using Netflex.Domain.ValueObjects;

namespace Netflex.Application.UseCases.V1.Auth.Queries;

public record GetSocialLoginResult(string LoginUrl);
public record GetSocialLoginQuery(string LoginProvider, string RedirectUrl)
    : IQuery<GetSocialLoginResult>;

public class GetSocialLoginHandler(ISocialServiceFactory socialServiceFactory)
    : IQueryHandler<GetSocialLoginQuery, GetSocialLoginResult>
{
    private readonly ISocialServiceFactory _socialServiceFactory = socialServiceFactory;
    public async Task<GetSocialLoginResult> Handle(GetSocialLoginQuery request,
        CancellationToken cancellationToken)
    {
        var service = _socialServiceFactory.GetByProvider(LoginProvider.Of(request.LoginProvider))
            ?? throw new NotSupportedLoginProviderException();

        var loginUrl = await service.GetLoginUrlAsync(request.RedirectUrl);
        return new GetSocialLoginResult(loginUrl);
    }
}