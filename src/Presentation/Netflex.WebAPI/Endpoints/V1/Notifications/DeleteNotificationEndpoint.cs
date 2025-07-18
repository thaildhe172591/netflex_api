using Netflex.Application.UseCases.V1.Notifications.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Notifications;

public record DeleteNotificationRequest(long Id);

public class DeleteNotificationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/notifications/{id}", async ([AsParameters] DeleteNotificationRequest request, ISender sender) =>
        {
            var command = request.Adapt<DeleteNotificationCommand>();
            await sender.Send(command);
            return Results.Ok();
        })
        // .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(DeleteNotificationEndpoint));
    }
}
