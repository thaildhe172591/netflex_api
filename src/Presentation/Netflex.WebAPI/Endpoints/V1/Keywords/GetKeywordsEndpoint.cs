using Netflex.Application.UseCases.V1.Keywords.Queries;
using Netflex.Shared.Pagination;

namespace Netflex.WebAPI.Endpoints.V1.Keywords;

public record GetKeywordsRequest(string? Search, string? SortBy, int PageIndex = 0, int PageSize = 10)
    : PaginationRequest(PageIndex, PageSize);

public class GetKeywordsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/keywords", async ([AsParameters] GetKeywordsRequest request, ISender sender) =>
        {
            var result = await sender.Send(request.Adapt<GetKeywordsQuery>());
            return Results.Ok(result);
        })
        .MapToApiVersion(1)
        .WithName(nameof(GetKeywordsEndpoint));
    }
}