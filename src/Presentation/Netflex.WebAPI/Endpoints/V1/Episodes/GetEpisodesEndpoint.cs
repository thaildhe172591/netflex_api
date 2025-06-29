using Netflex.Application.UseCases.V1.Episodes.Queries;
using Netflex.Shared.Pagination;

namespace Netflex.WebAPI.Endpoints.V1.Episodes;

public record GetEpisodesRequest(
    string? Search,
    long? SeriesId,
    string? Actors,
    string? SortBy,
    int PageIndex = 1,
    int PageSize = 10
) : PaginationRequest(PageIndex, PageSize);

public class GetEpisodesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/episodes", async ([AsParameters] GetEpisodesRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetEpisodesQuery>() with
            {
                ActorIds = request.Actors?.Split(',')
                 .Select(x => long.TryParse(x.Trim(), out var id) ? id : 0)
                 .Where(id => id != 0).ToArray() ?? []
            };
            var result = await sender.Send(query);
            return Results.Ok(result);
        })
        .MapToApiVersion(1)
        .WithName(nameof(GetEpisodesEndpoint));
    }
}