using Netflex.Application.UseCases.V1.Auth.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Auth;

public record SigninRequest(string Email, string Password, string DeviceId);
public record SigninResponse(string AccessToken, string RefreshToken);
public class SigninEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/signin", async (SigninRequest request, ISender sender, HttpContext context) =>
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();
            var userAgent = context.Request.Headers.UserAgent.ToString();

            var result = await sender.Send(new SigninCommand(
                request.Email, request.Password, request.DeviceId,
                ipAddress, userAgent
            ));
            return Results.Ok(result.Adapt<SigninResponse>());
        })
        .MapToApiVersion(1)
        .WithName(nameof(SigninEndpoint));
    }
}

