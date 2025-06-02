using Netflex.Application.UseCases.V1.Auth.Queries;

namespace Netflex.WebAPI.Endpoints.V1.Auth;

public record SocialLoginRequest(string RedirectUrl);
public class SocialLoginEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/auth/{loginProvider}", async (string loginProvider,
            [AsParameters] SocialLoginRequest request, ISender sender) =>
        {
            var result = await sender.Send(new GetSocialLoginQuery(loginProvider, request.RedirectUrl));
            return Results.Redirect(result.LoginUrl);
        })
        .MapToApiVersion(1)
        .WithName(nameof(SocialLoginEndpoints));
    }
}