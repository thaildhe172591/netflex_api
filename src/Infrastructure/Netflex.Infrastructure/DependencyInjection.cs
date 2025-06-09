using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Netflex.Application.Exceptions;
using Netflex.Application.Interfaces;

namespace Netflex.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        var cacheConnection = configuration.GetConnectionString("Cache")
            ?? throw new NotConfiguredException("Cache");

        services.Configure<JwtConfig>(
            configuration.GetSection(nameof(JwtConfig)));

        services.Configure<RefreshConfig>(
            configuration.GetSection(nameof(RefreshConfig)));

        services.AddHttpClient();
        services.AddStackExchangeRedisCache(options =>
            options.Configuration = cacheConnection);

        services.AddSingleton<ISlugGenerator, SlugGenerator>();

        services.AddScoped<IEmailService, EmailService>()
            .AddScoped<IOTPGenerator, OTPGenerator>()
            .AddScoped<ITemplateService, TemplateService>()
            .AddScoped<IRefreshTokenService, RefreshTokenService>()
            .AddScoped<IJwtTokenService, JwtTokenService>();

        services.AddScoped<ISocialLoginService, GoogleLoginService>()
            .AddScoped<ISocialLoginServiceFactory, SocialLoginServiceFactory>();

        return services;
    }
}
