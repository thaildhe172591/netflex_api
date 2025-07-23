using System.Security.Claims;
using Netflex.Application.Common.Exceptions;
using Netflex.Application.UseCases.V1.Users.Commands;
namespace Netflex.WebAPI.Endpoints.V1.Users;

public record GetFollowRequest(string TargetId, string TargetType);

public class GetFollowEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/follow", async ([AsParameters] GetFollowRequest request, ISender sender, HttpContext context) =>
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UserNotFoundException();
            var query = request.Adapt<GetFollowQuery>() with { UserId = userId };
            return Results.Ok(await sender.Send(query));
        })
        .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(GetFollowEndpoint));
    }
}