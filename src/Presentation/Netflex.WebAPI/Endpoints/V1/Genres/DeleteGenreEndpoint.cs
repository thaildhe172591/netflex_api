using Netflex.Application.UseCases.V1.Genres.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Genres;

public record DeleteGenreRequest(long Id);
public class DeleteGenreEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/genres/{id}", async ([AsParameters] DeleteGenreRequest request, ISender sender) =>
        {
            var command = request.Adapt<DeleteGenreCommand>();
            await sender.Send(command);
            return Results.Ok();
        })
        // .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(DeleteGenreEndpoint));
    }
}