using Netflex.Application.Exceptions;
using Netflex.Application.Interfaces;

namespace Netflex.WebAPI.Endpoints.V1.Auth;

public record VerifyOTPRequest(string Email, string OTP);

public class VerifyOTPEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/auth/otp", async ([AsParameters] VerifyOTPRequest request, IOTPGenerator otpGenerator) =>
        {
            var isValid = await otpGenerator.VerifyOTPAsync(request.Email, request.OTP);
            if (!isValid) throw new InvalidOTPException();
            return Results.Ok(isValid);
        })
        .MapToApiVersion(1)
        .WithName(nameof(VerifyOTPEndpoint));
    }
}