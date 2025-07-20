
using Netflex.Application.UseCases.V1.Users.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Users;

public record AssignRoleRequest(string UserId, string RoleName);

public class AssignRoleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/users/roles", async (AssignRoleRequest request, ISender sender) =>
        {
            await sender.Send(request.Adapt<AssignRoleCommand>());
            return Results.Ok();
        })
        // .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(AssignRoleEndpoint));
    }
}