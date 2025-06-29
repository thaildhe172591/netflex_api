using Netflex.Application.UseCases.V1.Episodes.Queries;

namespace Netflex.WebAPI.Endpoints.V1.Episodes;

public record GetEpisodeRequest(long Id);
public class GetEpisodeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/episodes/{id}", async ([AsParameters] GetEpisodeRequest request, ISender sender) =>
        {
            var result = await sender.Send(request.Adapt<GetEpisodeQuery>());
            return Results.Ok(result);
        })
        // .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(GetEpisodeEndpoint));
    }
}