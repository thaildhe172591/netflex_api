using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Netflex.Application.Common.Exceptions;
using Netflex.Application.UseCases.V1.Users.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Users;

public record ChangePasswordRequest(string OldPassword, string NewPassword);

public class ChangePasswordEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/change-password", async (ChangePasswordRequest request, ISender sender, HttpContext context) =>
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UserNotFoundException();
            var sessionId = context.User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value
                ?? throw new SessionNotFoundException();

            await sender.Send(new ChangePasswordCommand(userId, sessionId, request.OldPassword, request.NewPassword));
            return Results.Ok();
        })
        .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(ChangePasswordEndpoint));
    }
}