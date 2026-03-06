// AuditableEntityInterceptor.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

using FluentValidation;
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
    TimeProvider dateTime) : SaveChangesInterceptor
{

    //TODO: Write tests for these, because we're using TimeProvider, we can actually test these around time.
    private readonly TimeProvider _dateTime = dateTime;

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ValidateAndUpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        ValidateAndUpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ValidateAndUpdateEntities(DbContext? context)
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
            ValidateWithFluentValidation(entry.Entity);

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

    /// <summary>
    /// Creates an instance of the fluentvalidation validator if found for the given entity, and if the entity is not
    /// valid based on the rules, will throw a <see cref="FluentValidation.ValidationException"/> .
    /// </summary>
    /// <remarks>
    /// This method is not a replacement for model validation for incoming API requests, this is a last chance moment to
    /// validate the actual entity is considered valid prior to being saved into the database.
    /// </remarks>
    /// <param name="entity"></param>
    /// <exception cref="FluentValidation.ValidationException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private static void ValidateWithFluentValidation(BaseEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var abstractValidatorType = typeof(AbstractValidator<>);

        var entityType = entity.GetType();

        var genericAbstractValidatorType = abstractValidatorType.MakeGenericType(entityType);

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        var validatorType = assemblies.SelectMany(s => s.GetTypes().Where(t => t.IsSubclassOf(genericAbstractValidatorType))).FirstOrDefault();

        if (validatorType != null)
        {
            //There may or may not be a fluentvalidation configuration for the entity we're working with.

            var validatorInstance = Activator.CreateInstance(validatorType) as IValidator;

            if (validatorInstance != null)
            {
                var validationContext = new ValidationContext<object>(entity);
                var results = validatorInstance.Validate(validationContext);
                if (!results.IsValid)
                {
                    throw new FluentValidation.ValidationException(results.Errors);
                }
            }
            else
            {
                //NOTE: I'm not sure what exactly would or could cause this, but if this happens, we probably need to stop
                //any inserts because at very least, our validation won't work.
                throw new InvalidOperationException($"Tried to create an instance of {validatorType.Name}, but could not.");
            }
        }
    }

    /// <summary>
    /// This checks if an entity has changed any entities it has access to.
    /// </summary>
    /// <param name="entry"></param>
    /// <returns></returns>
    private static bool HasChangedOwnedEntities(EntityEntry entry)
    {
        return entry.References.Any(r =>
                r.TargetEntry?.Metadata.IsOwned() == true &&
                (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
    }
}
