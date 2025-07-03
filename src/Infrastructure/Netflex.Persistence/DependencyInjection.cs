using Netflex.Application.Interfaces.Repositories;
using Netflex.Persistence.Interceptors;
using Netflex.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using Npgsql;
using Microsoft.Extensions.Configuration;
using Netflex.Application.Common.Exceptions;
using Netflex.Application.Interfaces.Repositories.ReadOnly;
using Netflex.Persistence.Repositories.ReadOnly;

namespace Netflex.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        var databaseConnection = configuration.GetConnectionString("Database")
            ?? throw new NotConfiguredException("Database");

        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DateTrackingEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, UserTrackingEntityInterceptor>();

        services.AddHttpContextAccessor();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(databaseConnection, o =>
                o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                    .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IDbConnection>(sp => new NpgsqlConnection(databaseConnection));
        services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork))
            .AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>))
            .AddTransient<IUserRepository, UserRepository>();

        services.AddTransient<IUserReadOnlyRepository, UserReadOnlyRepository>()
            .AddTransient<IKeywordReadOnlyRepository, KeywordReadOnlyRepository>()
            .AddTransient<IGenreReadOnlyRepository, GenreReadOnlyRepository>()
            .AddTransient<IMovieReadOnlyRepository, MovieReadOnlyRepository>()
            .AddTransient<ISerieReadOnlyRepository, SerieReadOnlyRepository>()
            .AddTransient<IActorReadOnlyRepository, ActorReadOnlyRepository>()
            .AddTransient<IEpisodeReadOnlyRepository, EpisodeReadOnlyRepository>();

        services.Decorate<IUserRepository, CachedUserRepository>();

        return services;
    }
}
