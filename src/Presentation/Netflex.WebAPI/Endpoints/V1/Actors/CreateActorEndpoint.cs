using Microsoft.AspNetCore.Mvc;
using Netflex.Application.UseCases.V1.Actors.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Actors;

public record CreateActorRequest
{
    public required string Name { get; init; }
    public bool Gender { get; init; }
    public IFormFile? Image { get; init; }
    public DateTime? BirthDate { get; init; }
    public string? Biography { get; init; }
}

public record CreateActorResponse(long Id);

public class CreateActorEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/actors", async ([FromForm] CreateActorRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateActorCommand>();
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        // .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(CreateActorEndpoint));
    }
}