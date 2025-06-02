using Netflex.Application.UseCases.V1.Auth.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Auth;

public record SendOTPRequest(string Email);

public class SendOTPEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/otp", async (SendOTPRequest request, ISender sender) =>
        {
            await sender.Send(new SendOTPCommand(request.Email));
            return Results.Ok();
        })
        .MapToApiVersion(1)
        .WithName(nameof(SendOTPEndpoint));
    }
}