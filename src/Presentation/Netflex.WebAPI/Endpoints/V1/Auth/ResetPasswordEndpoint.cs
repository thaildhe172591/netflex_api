using Netflex.Application.UseCases.V1.Users.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Auth;

public record ResetPasswordRequest(string Email, string Otp, string NewPassword);

public class ResetPasswordEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/reset-password", async (ResetPasswordRequest request, ISender sender) =>
        {
            await sender.Send(new ResetPasswordCommand(request.Email, request.Otp, request.NewPassword));
            return Results.Ok();
        })
        .MapToApiVersion(1)
        .WithName(nameof(ResetPasswordEndpoint));
    }
}