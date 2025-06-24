using System.IdentityModel.Tokens.Jwt;
using Netflex.Application.Interfaces;
using Netflex.Application.UseCases.V1.Auth.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Auth;

public record ConfirmEmailRequest(string Email, string Otp);
public class ConfirmEmailEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/confirm-email", async (ConfirmEmailRequest request, ISender sender,
            HttpContext context, IJwtTokenService tokenService) =>
        {
            await sender.Send(new ConfirmEmailCommand(request.Email, request.Otp));
            var accessJti = context.User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            if (accessJti is not null) await tokenService.AddToBlacklistAsync(accessJti);
            return Results.Ok();
        })
        .MapToApiVersion(1)
        .WithName(nameof(ConfirmEmailEndpoint));
    }
}