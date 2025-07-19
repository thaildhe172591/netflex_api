using System.Security.Claims;
using Netflex.Application.Common.Exceptions;
using Netflex.Application.UseCases.V1.Notifications.Queries;
using Netflex.Shared.Pagination;

namespace Netflex.WebAPI.Endpoints.V1.Users;

public record GetNotificationsRequest(string? Search, string? SortBy, int PageIndex = 1, int PageSize = 10)
    : PaginationRequest(PageIndex, PageSize);

public class GetMyNotificationsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/notifications", async ([AsParameters] GetNotificationsRequest request, ISender sender, HttpContext context) =>
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UserNotFoundException();

            var result = await sender.Send(request.Adapt<GetNotificationsQuery>() with { UserId = userId });
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(GetMyNotificationsEndpoint));
    }
}