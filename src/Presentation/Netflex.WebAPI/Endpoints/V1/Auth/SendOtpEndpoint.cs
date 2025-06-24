using Netflex.Application.UseCases.V1.Auth.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Auth;

public record SendOtpRequest(string Email);

public class SendOtpEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/otp", async (SendOtpRequest request, ISender sender) =>
        {
            await sender.Send(new SendOtpCommand(request.Email));
            return Results.Ok();
        })
        .MapToApiVersion(1)
        .WithName(nameof(SendOtpEndpoint));
    }
}