namespace Kernel.Data.Entities;

using System;
using System.ComponentModel.DataAnnotations;

public abstract class BaseEntity : BaseEntity<int>;

/// <summary>
/// This is the base class in which all entities will inherit from.
/// </summary>
/// <typeparam name="TId"></typeparam>
public abstract class BaseEntity<TId> where TId : struct, IEquatable<TId>
{

    //TODO: Should write a test to scan all entities to ensure they both inherit from BaseEntity, and also don't contain DateTime
    //public DateTime test { get; set; }

    //TODO: Should this be 'required', or set as required in the EF configuration?
    public TId Id { get; set; }

    /// <summary>
    /// CreatedDate is an audit field, and is automatically generated. If you absolutely need to set it, use the SetCreatedDate() method. 
    /// </summary>    

    //NOTE: All entities will have a CreatedDate, but that should represent the date in which the entity was created. 
    //If you have the need for something similar, but something that doesn't represent the actual creation date of the database record,
    //then consider creating that property separately, with a different name.
    //This distinction can be illustrated by the fact that CreatedDate and the ID of the record are always sequential, and almost never should
    //be anything other than that. 
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
