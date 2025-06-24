using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
namespace Netflex.Persistence.Extensions;

public static class DatabaseExtension
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.MigrateAsync().GetAwaiter().GetResult();
        await SeedAsync(context);
    }

    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
                r.TargetEntry.Metadata.IsOwned() &&
                    (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified)
        );

    private static async Task SeedAsync(ApplicationDbContext context)
    {
        if (!context.Roles.Any())
        {
            context.Roles.Add(Role.Create(Guid.NewGuid().ToString(), "Admin"));
            context.Roles.Add(Role.Create(Guid.NewGuid().ToString(), "Moderator"));
            context.Roles.Add(Role.Create(Guid.NewGuid().ToString(), "User"));
            await context.SaveChangesAsync();
        }
    }


}