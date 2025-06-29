using Microsoft.AspNetCore.Mvc;
using Netflex.Application.UseCases.V1.Actors.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Actors;

public record UpdateActorRequest
{
    public string? Name { get; init; }
    public IFormFile? Image { get; init; }
    public bool? Gender { get; init; }
    public DateTime? BirthDate { get; init; }
    public string? Biography { get; init; }
}

public class UpdateActorEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/actors/{id}", async (long id, [FromForm] UpdateActorRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateActorCommand>() with { Id = id };
            await sender.Send(command);
            return Results.Ok();
        })
        // .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(UpdateActorEndpoint));
    }
}