using System.Security.Claims;
using Netflex.Application.Common.Exceptions;
using Netflex.Application.UseCases.V1.Reviews.Queries;
namespace Netflex.WebAPI.Endpoints.V1.Users;

public record GetReviewRequest(string TargetId, string TargetType);

public class GetReviewEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/review", async ([AsParameters] GetReviewRequest request, ISender sender, HttpContext context) =>
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UserNotFoundException();
            var query = request.Adapt<GetReviewQuery>() with { UserId = userId };
            return Results.Ok(await sender.Send(query));
        })
        .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(GetReviewEndpoint));
    }
}