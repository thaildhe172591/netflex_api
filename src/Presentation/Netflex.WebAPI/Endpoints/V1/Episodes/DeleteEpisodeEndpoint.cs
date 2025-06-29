using Netflex.Application.UseCases.V1.Episodes.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Episodes;

public record DeleteEpisodeRequest(long Id);

public class DeleteEpisodeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/episodes/{id}", async ([AsParameters] DeleteEpisodeRequest request, ISender sender) =>
        {
            var command = request.Adapt<DeleteEpisodeCommand>();
            await sender.Send(command);
            return Results.Ok();
        })
        // .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(DeleteEpisodeEndpoint));
    }
}