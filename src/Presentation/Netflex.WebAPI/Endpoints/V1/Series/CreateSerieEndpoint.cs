using Microsoft.AspNetCore.Mvc;
using Netflex.Application.UseCases.V1.Series.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Series;

public record CreateSerieRequest
{
    public required string Name { get; init; }
    public string? Overview { get; init; }
    public IFormFile? Poster { get; init; }
    public IFormFile? Backdrop { get; init; }
    public string? CountryIso { get; init; }
    public DateTime? FirstAirDate { get; init; }
    public DateTime? LastAirDate { get; init; }
    public ICollection<long>? Keywords { get; init; }
    public ICollection<long>? Genres { get; init; }
}

public class CreateSerieEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/series", async ([FromForm] CreateSerieRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateSerieCommand>();
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        // .RequireAuthorization("EmailVerified")
        .MapToApiVersion(1)
        .WithName(nameof(CreateSerieEndpoint));
    }
}