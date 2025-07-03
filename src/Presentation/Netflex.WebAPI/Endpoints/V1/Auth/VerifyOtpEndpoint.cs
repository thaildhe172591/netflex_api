using Netflex.Application.Common.Exceptions;
using Netflex.Application.Interfaces;

namespace Netflex.WebAPI.Endpoints.V1.Auth;

public record VerifyOtpRequest(string Email, string Otp);

public class VerifyOtpEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/auth/otp", async ([AsParameters] VerifyOtpRequest request, IOtpGenerator otpGenerator) =>
        {
            var isValid = await otpGenerator.VerifyOtpAsync(request.Email, request.Otp);
            if (!isValid) throw new InvalidOtpException();
            return Results.Ok(isValid);
        })
        .MapToApiVersion(1)
        .WithName(nameof(VerifyOtpEndpoint));
    }
}