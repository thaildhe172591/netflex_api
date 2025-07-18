using Netflex.Application.UseCases.V1.Notifications.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Netflex.WebAPI.Endpoints.V1.Notifications;

public record CreateNotificationRequest(string Title, string? Content, IEnumerable<string> UserId);

public class CreateNotificationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/notifications", async ([FromBody] CreateNotificationRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateNotificationCommand>();
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        // .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(CreateNotificationEndpoint));
    }
}
