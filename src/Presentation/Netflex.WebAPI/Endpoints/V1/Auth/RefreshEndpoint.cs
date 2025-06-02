using Netflex.Application.UseCases.V1.Auth.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Auth;

public record RefreshRequest(string DeviceId, string RefreshToken);
public record RefreshResult(string AccessToken, string RefreshToken);
public class RefreshEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/refresh", async (RefreshRequest request, ISender sender, HttpContext context) =>
        {
            var command = request.Adapt<RefreshCommand>();
            var result = await sender.Send(command);
            return Results.Ok(result.Adapt<RefreshResult>());
        })
        .MapToApiVersion(1)
        .WithName(nameof(RefreshEndpoint));
    }
}