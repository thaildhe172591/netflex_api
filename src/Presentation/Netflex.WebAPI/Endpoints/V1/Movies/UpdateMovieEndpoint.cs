using Microsoft.AspNetCore.Mvc;
using Netflex.Application.UseCases.V1.Movies.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Movies;

public record UpdateMovieRequest
{
    public string? Title { get; init; }
    public string? Overview { get; init; }
    public IFormFile? Poster { get; init; }
    public IFormFile? Backdrop { get; init; }
    public IFormFile? Video { get; init; }
    public string? CountryIso { get; init; }
    public int? Runtime { get; init; }
    public DateTime? ReleaseDate { get; init; }
    public ICollection<long>? Actors { get; init; }
    public ICollection<long>? Keywords { get; init; }
    public ICollection<long>? Genres { get; init; }
}

public class UpdateMovieEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/movies/{id}", async (long id, [FromForm] UpdateMovieRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateMovieCommand>() with { Id = id };
            await sender.Send(command);
            return Results.Ok();
        })
        // .RequireAuthorization("EmailVerified")
        .MapToApiVersion(1)
        .WithName(nameof(UpdateMovieEndpoint));
    }
}