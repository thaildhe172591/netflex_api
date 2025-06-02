using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Netflex.Application.Exceptions;
using Netflex.Application.UseCases.V1.Auth.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Auth;

public class LogoutEndpoint : ICarterModule
{
    public record LogoutRequest(string DeviceId);

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/logout", async (LogoutRequest request, ISender sender, HttpContext context) =>
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new NotAuthenticatedException();

            var accessJti = context.User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value
                ?? throw new NotAuthenticatedException();

            await sender.Send(new LogoutCommand(accessJti, userId, request.DeviceId));
            return Results.Ok();
        })
        .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(LogoutEndpoint));
    }
}
