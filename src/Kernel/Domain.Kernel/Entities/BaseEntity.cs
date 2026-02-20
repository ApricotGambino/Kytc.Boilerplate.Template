namespace KernelData.Entities;

using System;
using System.ComponentModel.DataAnnotations;

//NOTE: Navigation properties are intentionally nullable, because
//https://learn.microsoft.com/en-us/ef/core/modeling/relationships/navigations
//tells us that if we initialize them, we're going to have unexpected results, and because we don't want to
//supress every warning of uninitialized objects CS618, we make them nullable.
//Also, they are eager loaded, never lazy, so we'll .include() if we want them.

//Also, you can read more about EF relationships here:
//https://learn.microsoft.com/en-us/ef/core/modeling/relationships

public abstract class BaseEntity : BaseEntity<int>;

/// <summary>
/// This is the base class in which all entities must inherit from.
/// </summary>
/// <typeparam name="TId">TId is the type used for the Id of the entity.</typeparam>
public abstract class BaseEntity<TId> where TId : struct, IEquatable<TId>
{

    //TODO: Should write a test to scan all entities to ensure they both inherit from BaseEntity, and also don't contain DateTime

    //TODO: Should this be 'required', or set as required in the EF configuration?
    public TId Id { get; set; }

    /// <summary>
    /// CreatedDate is an audit field, and is automatically generated. If you absolutely need to set it, use the SetCreatedDate() method.
    /// </summary>

    /// <summary>
    /// All entities will have a CreatedDate, but that should represent the date in which the entity was created.
    /// If you have the need for something similar, but something that doesn't represent the actual creation date of the database record,
    /// then consider creating that property separately, with a different name.
    /// This distinction can be illustrated by the fact that CreatedDate and the ID of the record are always sequential, and almost never should
    /// be anything other than that.
    /// </summary>
    public DateTimeOffset CreatedDateTimeOffset { get; private set; } = DateTimeOffset.Now;
    public DateTimeOffset? UpdatedDateTimeOffset { get; set; }



    public Guid? UpdatedBy { get; set; }
    public Guid CreatedBy { get; set; }
    public bool IsActive { get; set; }

    //TODO: Test this Version, and should this be decorated with [Timestamp]?
    //Should this be 'required', or set as required in the EF configuration?
    [Timestamp]
    public byte[] Version { get; set; }

    public void SetCreatedDate(DateTimeOffset createdDate)
    {
        CreatedDateTimeOffset = createdDate;
    }

}
