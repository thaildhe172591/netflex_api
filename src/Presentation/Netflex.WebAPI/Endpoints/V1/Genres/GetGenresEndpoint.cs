using Netflex.Application.UseCases.V1.Genres.Queries;
using Netflex.Shared.Pagination;

namespace Netflex.WebAPI.Endpoints.V1.Genres;

public record GetGenresRequest(string? Search, string? SortBy, int PageIndex = 1, int PageSize = 10)
    : PaginationRequest(PageIndex, PageSize);

public class GetGenresEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/genres", async ([AsParameters] GetGenresRequest request, ISender sender) =>
        {
            var result = await sender.Send(request.Adapt<GetGenresQuery>());
            return Results.Ok(result);
        })
        .MapToApiVersion(1)
        .WithName(nameof(GetGenresEndpoint));
    }
}