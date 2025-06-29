using Microsoft.AspNetCore.Mvc;
using Netflex.Application.UseCases.V1.Movies.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Movies;

public record CreateMovieRequest
{
    public required string Title { get; init; }
    public string? Overview { get; init; }
    public IFormFile? Poster { get; init; }
    public IFormFile? Backdrop { get; init; }
    public IFormFile? Video { get; init; }
    public string? CountryIso { get; init; }
    public TimeSpan? RunTime { get; init; }
    public DateTime? ReleaseDate { get; init; }
    public ICollection<long>? Actors { get; init; }
    public ICollection<long>? Keywords { get; init; }
    public ICollection<long>? Genres { get; init; }
}
public record CreateMovieResponse(long Id);

public class CreateMovieEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/movies", async ([FromForm] CreateMovieRequest request, ISender sender) =>
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