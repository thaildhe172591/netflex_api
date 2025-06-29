using Microsoft.AspNetCore.Mvc;
using Netflex.Application.UseCases.V1.Genres.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Genres;

public record UpdateGenreRequest(string Name);
public class UpdateGenreEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/genres/{id}", async (long id, [FromBody] UpdateGenreRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateGenreCommand>() with { Id = id };
            await sender.Send(command);
            return Results.Ok();
        })
        // .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(UpdateGenreEndpoint));
    }
}