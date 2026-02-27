// AuditableEntityInterceptor.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

using Kernel.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Kernel.Infrastructure.Interceptors;
/// <summary>
/// This interceptor is responsible for enriching audit data when entities are saved
/// </summary>
public class AuditableEntityInterceptor(
    //IUser user,
    //TODO: Explain why we're using TimeProvider
    TimeProvider dateTime) : SaveChangesInterceptor
{
    private readonly TimeProvider _dateTime = dateTime;

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null)
        {
            return;
        }

        var trackedEntities = context.ChangeTracker.Entries<BaseEntity>()
                        .Where(p => p.State == EntityState.Added
                                || p.State == EntityState.Modified
                                || HasChangedOwnedEntities(p));

        foreach (var entry in trackedEntities)
        {
            var utcNow = _dateTime.GetUtcNow();
            if (entry.State == EntityState.Added)
            {
                //entry.Entity.CreatedBy = _user.Id;
                entry.Entity.CreatedDateTimeOffset = utcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                //entry.Entity.LastModifiedBy = _user.Id;
                entry.Entity.UpdatedDateTimeOffset = utcNow;
            }

        }
    }

    public static bool HasChangedOwnedEntities(EntityEntry entry)
    {
        return entry.References.Any(r =>
                r.TargetEntry?.Metadata.IsOwned() == true &&
                (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
    }
}
