using Netflex.Application.UseCases.V1.Auth.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Auth;

public record SocialLoginCallbackRequest(string Code, string RedirectUrl, string DeviceId);
public record SocialLoginCallbackResponse(string AccessToken, string RefreshToken);
public class SocialLoginCallbackEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/{loginProvider}/callback", async (string loginProvider,
            SocialLoginCallbackRequest request, ISender sender, HttpContext context) =>
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();
            var userAgent = context.Request.Headers.UserAgent.ToString();

            var result = await sender.Send(new SocialLoginCallbackCommand(
                loginProvider, request.Code, request.RedirectUrl,
                request.DeviceId, ipAddress, userAgent));
            return Results.Ok(result.Adapt<SocialLoginCallbackResponse>());
        })
        .MapToApiVersion(1)
        .WithName(nameof(SocialLoginCallbackEndpoints));
    }
}