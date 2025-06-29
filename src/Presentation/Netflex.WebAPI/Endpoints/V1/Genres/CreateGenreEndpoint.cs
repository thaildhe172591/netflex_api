using Netflex.Application.UseCases.V1.Genres.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Genres;

public record CreateGenreRequest(string Name);
public record CreateGenreResponse(long Id);

public class CreateGenreEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/genres", async (CreateGenreRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateGenreCommand>();
            var result = await sender.Send(command);
            return Results.Ok(result.Adapt<CreateGenreResponse>());
        })
        // .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(CreateGenreEndpoint));
    }
}