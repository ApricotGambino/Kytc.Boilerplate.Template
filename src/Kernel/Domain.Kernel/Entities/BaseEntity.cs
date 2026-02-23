// BaseEntity.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

namespace KernelData.Entities;
//NOTE:
//For Entities, we never initialize navigation properties.
//This can cause unexpected results, and is discussed here: https://learn.microsoft.com/en-us/ef/core/modeling/relationships/navigations
//You'll probably get pinged by CS618 as a warning by this, so you can initalize using `null!` which is a 'forgiving nullability operator'
//It's actually used for exactly this use-case here: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/nullable-warnings#nonnullable-reference-not-initialized
//They just don't mention WHY you should use it.
//Also no need for `virtual`, as they are eager loaded, never lazy, so we'll .include() if we want them.

//Also, you can read more about EF relationships here:
//https://learn.microsoft.com/en-us/ef/core/modeling/relationships

/// <summary>
/// <inheritdoc/> The <see cref="BaseEntity"/> with a primary key of type <see cref="int"/>
/// </summary>
public abstract class BaseEntity : BaseEntity<int>;

/// <summary>
/// This is the base class in which all entities must inherit from.
/// </summary>
/// <typeparam name="TId">TId is the type used for the Id of the entity.</typeparam>
public abstract class BaseEntity<TId> where TId : struct, IEquatable<TId>
{
    /// <summary>
    /// The Primary Key (PK) of the entity.
    /// </summary>
    /// <remarks>
    /// Entity Framework will assign a value based on the PK generation strategy of the underlying database provider. is
    /// a great candidate to filter or order on cronologically.
    /// </remarks>
    public TId Id { get; set; } = default!;

    /// <summary>
    /// An audit value that represents the creation date of the record.
    /// </summary>
    /// <remarks><see cref="DateTimeOffset"/> is used to help avoid timezone issues</remarks>
    /// <remarks>
    /// Do not use or rely on this property for anything other than the date this record was created.
    /// </remarks>
    public DateTimeOffset CreatedDateTimeOffset { get; set; }

    /// <summary>
    /// An audit value that represents the date this record was updated. if the value is null the record has never been
    /// updated.
    /// </summary>
    /// <remarks><see cref="DateTimeOffset"/> is used to help avoid timezone issues</remarks>
    public DateTimeOffset? UpdatedDateTimeOffset { get; set; }

    /// <summary>
    /// An audit value that represents the Id of the user who created this record
    /// </summary>
    public Guid CreatedBy { get; set; }

    /// <summary>
    /// An audit value that represents the Id of the user who updated this record, if this record is null, the record
    /// has never been updated.
    /// </summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>
    /// Represents if the record is soft deleted, if the record is soft deleted, then consider it as deleted.
    /// </summary>
    /// <remarks>
    /// Queries against the database should not consider soft deleted records in results unless specifically requested.
    /// </remarks>
    public bool IsSoftDeleted { get; set; } = false;

    /// <summary>
    /// Database providers can choose to interpret this in different way, but it is commonly used to indicate some form
    /// of automatic row-versioning as used for optimistic concurrency detection.
    /// </summary>
    public byte[] Version { get; set; } = null!;
}
