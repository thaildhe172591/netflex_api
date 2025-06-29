using Netflex.Shared.Exceptions.Handler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Netflex.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Netflex.Infrastructure.Settings;

namespace Netflex.WebAPI;

public static class DependencyInjection
{
    public const string API_PREFIX = "/api/v{version:apiVersion}";
    public static IServiceCollection AddApiServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var databaseConnection = configuration.GetConnectionString("Database")
            ?? throw new NotConfiguredException("Database");
        var cacheConnection = configuration.GetConnectionString("Cache")
            ?? throw new NotConfiguredException("Cache");

        services.AddHealthChecks()
            .AddNpgSql(databaseConnection)
            .AddRedis(cacheConnection);

        services.AddCarter();

        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddApiVersioning(options => { options.ReportApiVersions = true; })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        services.AddAuthentication(configuration);
        services.AddAuthorization();

        var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>()
            ?? throw new NotConfiguredException("AllowedOrigins");
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.UseAuthorization();
        app.UseExceptionHandler(options => { });
        app.UseAccessTokenValidation();
        app.UseHealthChecks("/health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        app.MapGroup(API_PREFIX)
            .DisableAntiforgery()
            .WithApiVersionSet(versionSet)
            .MapCarter();

        app.UseCors();

        return app;
    }

    private static IServiceCollection AddAuthentication(
        this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>()
            ?? throw new NotConfiguredException(nameof(JwtSettings));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Key)),
                };
            });
        return services;
    }
}