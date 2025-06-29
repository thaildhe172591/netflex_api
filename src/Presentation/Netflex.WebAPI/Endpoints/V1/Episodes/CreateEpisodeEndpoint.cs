using Microsoft.AspNetCore.Mvc;
using Netflex.Application.UseCases.V1.Episodes.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Episodes;

public record CreateEpisodeRequest
{
    public required string Name { get; init; }
    public required int EpisodeNumber { get; init; }
    public required long SeriesId { get; init; }
    public string? Overview { get; init; }
    public IFormFile? Video { get; init; }
    public TimeSpan? Runtime { get; init; }
    public DateTime? AirDate { get; init; }
    public ICollection<long>? Actors { get; init; }
}
public record CreateEpisodeResponse(long Id);

public class CreateEpisodeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/episodes", async ([FromForm] CreateEpisodeRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateEpisodeCommand>();
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        // .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(CreateEpisodeEndpoint));
    }
}