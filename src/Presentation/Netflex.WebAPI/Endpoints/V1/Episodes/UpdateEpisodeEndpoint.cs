using Microsoft.AspNetCore.Mvc;
using Netflex.Application.UseCases.V1.Episodes.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Episodes;

public record UpdateEpisodeRequest
{
    public string? Name { get; init; }
    public int? EpisodeNumber { get; init; }
    public long? SeriesId { get; init; }
    public string? Overview { get; init; }
    public IFormFile? Video { get; init; }
    public TimeSpan? Runtime { get; init; }
    public DateTime? AirDate { get; init; }
    public ICollection<long>? Actors { get; init; }
}

public class UpdateEpisodeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/episodes/{id}", async (long id, [FromForm] UpdateEpisodeRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateEpisodeCommand>() with { Id = id };
            await sender.Send(command);
            return Results.Ok();
        })
        // .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(UpdateEpisodeEndpoint));
    }
}