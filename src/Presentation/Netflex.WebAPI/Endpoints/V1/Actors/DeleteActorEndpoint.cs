using Netflex.Application.UseCases.V1.Actors.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Actors;

public record DeleteActorRequest(long Id);

public class DeleteActorEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/actors/{id}", async ([AsParameters] DeleteActorRequest request, ISender sender) =>
        {
            var command = request.Adapt<DeleteActorCommand>();
            await sender.Send(command);
            return Results.Ok();
        })
        // .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(DeleteActorEndpoint));
    }
}