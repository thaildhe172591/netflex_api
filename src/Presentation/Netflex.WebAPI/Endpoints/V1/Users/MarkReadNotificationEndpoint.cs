using Netflex.Application.UseCases.V1.Notifications.Commands;
using System.Security.Claims;
using Netflex.Application.Common.Exceptions;

namespace Netflex.WebAPI.Endpoints.V1.Users;

public record MarkReadNotificationRequest(long NotificationId);

public class MarkReadNotificationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/users/read/{notificationId}", async ([AsParameters] MarkReadNotificationRequest request, ISender sender, HttpContext context) =>
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UserNotFoundException();
            var command = request.Adapt<MarkReadNotificationCommand>() with { UserId = userId };
            await sender.Send(command);
            return Results.Ok();
        })
        .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(MarkReadNotificationEndpoint));
    }
}
