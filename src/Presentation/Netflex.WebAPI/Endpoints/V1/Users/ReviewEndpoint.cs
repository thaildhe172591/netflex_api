using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Netflex.Application.Common.Exceptions;
using Netflex.Application.UseCases.V1.Reviews.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Users;

public record CreateOrUpdateReviewRequest(string UserId, string TargetId, string TargetType, int Rating);

public class ReviewEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/users/review", async ([FromBody] CreateOrUpdateReviewRequest request, ISender sender, HttpContext context) =>
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UserNotFoundException();
            var command = request.Adapt<CreateOrUpdateReviewCommand>() with { UserId = userId };
            await sender.Send(command);
            return Results.NoContent();
        })
        .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(ReviewEndpoint));
    }
}