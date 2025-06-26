using Netflex.Application.UseCases.V1.Movies.Queries;
using Netflex.Shared.Pagination;

namespace Netflex.WebAPI.Endpoints.V1.Movies;

public record GetMoviesRequest(
    string? Search,
    string? Genres,
    string? Keywords,
    string? Actors,
    string? SortBy,
    int PageIndex = 0,
    int PageSize = 10
) : PaginationRequest(PageIndex, PageSize);

public class GetMoviesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/movies", async ([AsParameters] GetMoviesRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetMoviesQuery>();
            query = query with
            {
                GenreIds = request.Genres?.Split(',')
                  .Select(x => long.TryParse(x.Trim(), out var id) ? id : 0)
                  .Where(id => id != 0) ?? [],

                KeywordIds = request.Keywords?.Split(',')
                  .Select(x => long.TryParse(x.Trim(), out var id) ? id : 0)
                  .Where(id => id != 0) ?? [],

                ActorIds = request.Actors?.Split(',')
                 .Select(x => long.TryParse(x.Trim(), out var id) ? id : 0)
                 .Where(id => id != 0) ?? []

            };
            var result = await sender.Send(request.Adapt<GetMoviesQuery>());
            return Results.Ok(result);
        })
        .MapToApiVersion(1)
        .WithName(nameof(GetMoviesEndpoint));
    }
}