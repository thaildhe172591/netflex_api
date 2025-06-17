using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Netflex.Application.Interfaces;
using Netflex.Application.Interfaces.Repositories;
using Netflex.WebAPI.Middleware.Attributes;
using Netflex.WebAPI.Middleware.Exceptions;

namespace Netflex.WebAPI.Middleware;

public class AccessTokenValidationMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, IJwtTokenService tokenService,
        IUserRepository userRepository)
    {
        if (IsEndpointAuthorized(context) && context.User?.Identity?.IsAuthenticated == true)
        {
            var accessJti = context.User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var version = context.User.FindFirst(CustomClaimNames.Version)?.Value ?? "0";

            if (!string.IsNullOrEmpty(accessJti))
            {
                var isRevoked = await tokenService.IsRevokedAsync(accessJti);
                if (isRevoked) throw new AccessTokenRevokedExceptions();
            }

            if (RequireEmailVerified(context))
            {
                var email = context.User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
                var emailVerified = context.User.FindFirst(JwtRegisteredClaimNames.EmailVerified)?.Value;
                if (!bool.TryParse(emailVerified, out var isEmailVerified) || !isEmailVerified)
                {
                    throw new EmailNotVerifiedException(email);
                }
            }

            if (!string.IsNullOrEmpty(userId))
            {
                var current = await userRepository.GetVersionByIdAsync(userId);
                if (current != int.Parse(version))
                    throw new ObsoleteAccessTokenException();
            }
        }

        await _next(context);
    }


    private static bool IsEndpointAuthorized(HttpContext context)
    {
        var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
        if (endpoint == null) return false;

        return endpoint.Metadata.GetMetadata<AuthorizeAttribute>() != null;
    }

    private static bool RequireEmailVerified(HttpContext context)
    {
        var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
        return endpoint?.Metadata.OfType<RequireEmailVerifiedAttribute>().Any() == true;
    }
}

public static class Extentions
{
    public static TBuilder RequireEmailVerified<TBuilder>(this TBuilder builder)
        where TBuilder : IEndpointConventionBuilder
    {
        builder.Add(endpointBuilder =>
            endpointBuilder.Metadata.Add(new RequireEmailVerifiedAttribute())
        );

        return builder;
    }

    public static IApplicationBuilder UseAccessTokenValidation(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<AccessTokenValidationMiddleware>();
        return builder;
    }
}