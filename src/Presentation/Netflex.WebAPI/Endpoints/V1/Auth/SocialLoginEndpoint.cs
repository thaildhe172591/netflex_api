using Netflex.Application.UseCases.V1.Auth.Queries;

namespace Netflex.WebAPI.Endpoints.V1.Auth;

public record SocialLoginRequest(string LoginProvider, string RedirectUrl);
public record SocialLoginResponse(string LoginUrl);
public class SocialLoginEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/auth/{loginProvider}", async (
            [AsParameters] SocialLoginRequest request, ISender sender) =>
        {
            var result = await sender.Send(new GetSocialLoginQuery(request.LoginProvider, request.RedirectUrl));
            return Results.Ok(new SocialLoginResponse(result.LoginUrl));
        })
        .MapToApiVersion(1)
        .WithName(nameof(SocialLoginEndpoints));
    }
}