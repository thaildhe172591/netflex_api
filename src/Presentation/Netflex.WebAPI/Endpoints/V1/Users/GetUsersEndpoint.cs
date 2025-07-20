using Netflex.Application.UseCases.V1.Users.Queries;
using Netflex.Shared.Pagination;

namespace Netflex.WebAPI.Endpoints.V1.Users;

public record GetUsersRequest(
    string? Search,
    string? Role,
    bool? IsConfirmed,
    string? SortBy,
    int PageIndex = 1,
    int PageSize = 10
) : PaginationRequest(PageIndex, PageSize);

public class GetUsersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users", async ([AsParameters] GetUsersRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetUsersQuery>();
            query = query with
            {
                Roles = request.Role?.Split(',')
                    .Select(x => x.Trim())
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToArray() ?? []
            };
            var result = await sender.Send(query);
            return Results.Ok(result);
        })
        // .RequireAuthorization("Admin")
        .MapToApiVersion(1)
        .WithName(nameof(GetUsersEndpoint));
    }
}
