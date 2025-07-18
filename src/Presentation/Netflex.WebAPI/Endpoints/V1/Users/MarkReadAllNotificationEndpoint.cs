using Netflex.Application.UseCases.V1.Notifications.Commands;
using System.Security.Claims;
using Netflex.Application.Common.Exceptions;

namespace Netflex.WebAPI.Endpoints.V1.Users;

public class MarkReadAllNotificationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("users/read-all", async (ISender sender, HttpContext context) =>
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UserNotFoundException();
            var command = new MarkReadAllNotificationCommand(userId);
            await sender.Send(command);
            return Results.Ok();
        })
        .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(MarkReadAllNotificationEndpoint));
    }
}
