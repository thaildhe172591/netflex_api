using Netflex.Application.UseCases.V1.Reports.Queries;
using Netflex.Shared.Pagination;

namespace Netflex.WebAPI.Endpoints.V1.Reports;

public record GetReportsRequest(string? Search, string? SortBy, int PageIndex = 1, int PageSize = 10)
    : PaginationRequest(PageIndex, PageSize);

public class GetReportsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/reports", async ([AsParameters] GetReportsRequest request, ISender sender) =>
        {
            var result = await sender.Send(request.Adapt<GetReportsQuery>());
            return Results.Ok(result);
        })
        // .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(GetReportsEndpoint));
    }
}