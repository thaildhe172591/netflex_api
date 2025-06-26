using Netflex.Application.UseCases.V1.Movies.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Movies;

public record CreateMovieRequest(
    string Title,
    string? Overview,
    string? PosterPath,
    string? BackdropPath,
    string? VideoUrl,
    string? CountryIso,
    DateTime? ReleaseDate,
    ICollection<long>? ActorIds,
    ICollection<long>? KeywordIds,
    ICollection<long>? GenreIds
);
public record CreateMovieResponse(long Id);

public class CreateMovieEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/movies", async (CreateMovieRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateMovieCommand>();

            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        // .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(CreateMovieEndpoint));
    }
}