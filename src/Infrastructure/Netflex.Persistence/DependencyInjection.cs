using Netflex.Application.Interfaces.Repositories;
using Netflex.Persistence.Interceptors;
using Netflex.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using Npgsql;

namespace Netflex.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices
        (this IServiceCollection services, string databaseConnection)
    {
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddHttpContextAccessor();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(databaseConnection, o =>
                o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                    .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IDbConnection>(sp => new NpgsqlConnection(databaseConnection));

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork))
            .AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>))
            .AddTransient<IUserRepository, UserRepository>();

        services.Decorate<IUserRepository, CachedUserRepository>();

        return services;
    }
}
