using Netflex.Application.UseCases.V1.Users.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Users;

public record CreateUserRequest(string Email, string Password);
public class CreateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/users", async (CreateUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateUserCommand>();
            await sender.Send(command);
            return Results.Created();
        })
        .MapToApiVersion(1)
        .WithName(nameof(CreateUserEndpoint));
    }
}
