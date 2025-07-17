using Netflex.Application.UseCases.V1.Notifications.Queries;
using Netflex.Shared.Pagination;

namespace Netflex.WebAPI.Endpoints.V1.Notifications;

public record GetNotificationsRequest(string? Search, string? SortBy, int PageIndex = 1, int PageSize = 10)
    : PaginationRequest(PageIndex, PageSize);

public class GetNotificationsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/notifications", async ([AsParameters] GetNotificationsRequest request, ISender sender) =>
        {
            var result = await sender.Send(request.Adapt<GetNotificationsQuery>());
            return Results.Ok(result);
        })
        // .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(GetNotificationsEndpoint));
    }
}