using System.Security.Claims;
using Netflex.Domain.Entities.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Netflex.Persistence.Interceptors;

public class UserTrackingEntityInterceptor(IHttpContextAccessor accessor)
        : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _accessor = accessor;
    public string LoggedInUserId
       => _accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? dbContext)
    {
        if (dbContext == null) return;
        var userId = LoggedInUserId;
        foreach (var entry in dbContext.ChangeTracker.Entries<IUserTracking>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = userId;
            }

            if (entry.State == EntityState.Added
                || entry.State == EntityState.Modified
                || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModifiedBy = userId;
            }
        }
    }
}
