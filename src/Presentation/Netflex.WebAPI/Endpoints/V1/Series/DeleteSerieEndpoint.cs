using Netflex.Application.UseCases.V1.Series.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Series;

public record DeleteSerieRequest(long Id);

public class DeleteSerieEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/series/{id}", async ([AsParameters] DeleteSerieRequest request, ISender sender) =>
        {
            var command = request.Adapt<DeleteSerieCommand>();
            await sender.Send(command);
            return Results.NoContent();
        })
        // .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(DeleteSerieEndpoint));
    }
}