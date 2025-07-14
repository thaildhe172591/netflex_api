using Netflex.Application.UseCases.V1.Actors.Queries;

namespace Netflex.WebAPI.Endpoints.V1.Actors;

public class GetActorEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/actors/{id:long}", async (long id, ISender sender) =>
        {
            var query = new GetActorQuery(id);
            var result = await sender.Send(query);
            return Results.Ok(result);
        })
        // .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(GetActorEndpoint));
    }
}