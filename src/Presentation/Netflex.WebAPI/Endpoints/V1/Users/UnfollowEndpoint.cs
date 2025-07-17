using System.Security.Claims;
using Netflex.Application.Common.Exceptions;
using Netflex.Application.UseCases.V1.Users.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Users;

public record UnfollowRequest(string TargetId, string TargetType);

public class UnfollowEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/users/unfollow", async (ChangePasswordRequest request, ISender sender, HttpContext context) =>
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UserNotFoundException();

            await sender.Send(request.Adapt<UnfollowCommand>() with { UserId = userId });
            return Results.Ok();
        })
        .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(UnfollowEndpoint));
    }
}