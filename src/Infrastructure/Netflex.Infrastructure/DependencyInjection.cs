using Mapster;
using Microsoft.AspNetCore.Http;
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

        services.Configure<JwtSettings>(
            configuration.GetSection(nameof(JwtSettings)));

        services.Configure<RefreshSettings>(
            configuration.GetSection(nameof(RefreshSettings)));

        services.Configure<GoogleSettings>(
            configuration.GetSection(nameof(GoogleSettings)));

        services.AddHttpClient();
        services.AddStackExchangeRedisCache(options =>
            options.Configuration = cacheConnection);

        services.AddSingleton<ISlugGenerator, SlugGenerator>();

        services.AddScoped<IEmailService, EmailService>()
            .AddScoped<IOtpGenerator, OtpGenerator>()
            .AddScoped<IRefreshOptions, RefreshOptions>()
            .AddScoped<IJwtTokenService, JwtTokenService>();

        services.AddScoped<ISocialService, GoogleService>()
            .AddScoped<ISocialServiceFactory, SocialServiceFactory>();

        services.AddScoped<ICloudStorage, CloudStorage>();

        TypeAdapterConfig<IFormFile?, IFileResource?>.ForType()
            .MapWith(src => FormFileAdapter.From(src));

        return services;
    }
}
