// BaseEntityConfiguration.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

//NOTE: Some of this code is from https://stackoverflow.com/questions/53275567/how-to-apply-common-configuration-to-all-entities-in-ef-core
//It's terribly confusing.

using System.Reflection;
using Kernel.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kernel.Data.EntityFramework;

public static class BaseEntityConfiguration
{
    static void Configure<TEntity, TId>(ModelBuilder modelBuilder)
        where TEntity : BaseEntity<TId>
        where TId : struct, IEquatable<TId>
    {
        modelBuilder.Entity<TEntity>(builder =>
        {
            builder.HasKey(nameof(BaseEntity.Id));
            builder.Property(e => e.Version).IsRowVersion();
        });
    }

    public static ModelBuilder ApplyBaseEntityConfiguration(this ModelBuilder modelBuilder)
    {
        var method = typeof(BaseEntityConfiguration).GetTypeInfo().DeclaredMethods
            .Single(m => m.Name == nameof(Configure));

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.ClrType.IsBaseEntity(out var T))
                method.MakeGenericMethod(entityType.ClrType, T).Invoke(null, new[] { modelBuilder });
        }

        return modelBuilder;
    }

    static bool IsBaseEntity(this Type type, out Type T)
    {
        for (var baseType = type.BaseType; baseType != null; baseType = baseType.BaseType)
        {
            if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(BaseEntity<>))
            {
                T = baseType.GetGenericArguments()[0];
                return true;
            }
        }
        T = null;
        return false;
    }
}
