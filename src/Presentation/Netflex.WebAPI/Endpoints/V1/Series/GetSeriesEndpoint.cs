using Netflex.Application.UseCases.V1.Series.Queries;
using Netflex.Shared.Pagination;

namespace Netflex.WebAPI.Endpoints.V1.Series;

public record GetSeriesRequest(
    string? Search,
    string? Genres,
    string? Keywords,
    string? Country,
    int? Year,
    string? SortBy,
    string? FollowerId,
    int PageIndex = 1,
    int PageSize = 10
) : PaginationRequest(PageIndex, PageSize);

public class GetSeriesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/series", async ([AsParameters] GetSeriesRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetSeriesQuery>();
            query = query with
            {
                GenreIds = request.Genres?.Split(',')
                  .Select(x => long.TryParse(x.Trim(), out var id) ? id : 0)
                  .Where(id => id != 0).ToArray() ?? [],

                KeywordIds = request.Keywords?.Split(',')
                  .Select(x => long.TryParse(x.Trim(), out var id) ? id : 0)
                  .Where(id => id != 0).ToArray() ?? []
            };
            var result = await sender.Send(query);
            return Results.Ok(result);
        })
        .MapToApiVersion(1)
        .WithName(nameof(GetSeriesEndpoint));
    }
}