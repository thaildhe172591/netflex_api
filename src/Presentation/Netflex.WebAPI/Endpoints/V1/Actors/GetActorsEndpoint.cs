using Netflex.Application.UseCases.V1.Actors.Queries;
using Netflex.Shared.Pagination;

namespace Netflex.WebAPI.Endpoints.V1.Actors;

public record GetActorsRequest(
    string? Search,
    string? SortBy,
    int PageIndex = 1,
    int PageSize = 10
) : PaginationRequest(PageIndex, PageSize);

public class GetActorsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/actors", async ([AsParameters] GetActorsRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetActorsQuery>();
            var result = await sender.Send(request.Adapt<GetActorsQuery>());
            return Results.Ok(result);
        })
        // .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(GetActorsEndpoint));
    }
}