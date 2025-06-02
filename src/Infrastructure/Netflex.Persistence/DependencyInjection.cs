using Netflex.Application.Interfaces.Repositories;
using Netflex.Persistence.Interceptors;
using Netflex.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Netflex.Application.Exceptions;
using System.Data;
using Npgsql;

namespace Netflex.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database")
            ?? throw new NotConfiguredException("Database");

        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddHttpContextAccessor();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString, o =>
                o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                    .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IDbConnection>(sp => new NpgsqlConnection(connectionString));

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork))
            .AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>))
            .AddTransient<IUserRepository, UserRepository>();

        services.Decorate<IUserRepository, CachedUserRepository>();

        return services;
    }
}
