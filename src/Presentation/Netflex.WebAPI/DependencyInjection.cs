using Netflex.Shared.Exceptions.Handler;
using Netflex.Shared.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Netflex.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Netflex.WebAPI.Middleware;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Netflex.Infrastructure.Services;

namespace Netflex.WebAPI;

public static class DependencyInjection
{
    public const string API_PREFIX = "/api/v{version:apiVersion}";
    public static IServiceCollection AddApiServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database")
            ?? throw new NotConfiguredException("Database");
        services.AddCarter();
        services.AddHealthChecks().AddNpgSql(connectionString);

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
        app.UseMiddleware<AccessTokenValidationMiddleware>();
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
            .WithApiVersionSet(versionSet)
            .AddEndpointFilter<CustomResponseFilter>()
            .MapCarter();

        app.UseCors();

        return app;
    }

    private static IServiceCollection AddAuthentication(
        this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfig = configuration.GetSection(nameof(JwtConfig)).Get<JwtConfig>()
            ?? throw new NotConfiguredException(nameof(JwtConfig));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtConfig.Key)),
                };
            });
        return services;
    }
}