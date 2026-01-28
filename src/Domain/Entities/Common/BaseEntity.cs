namespace Domain.Entities.Common;

using System;
using System.ComponentModel.DataAnnotations;



public abstract class BaseEntity : BaseEntity<int>;
public abstract class BaseEntity<T>
{
    //TODO: Should this be 'required', or set as required in the EF configuration?
    public T Id { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
    public bool IsActive { get; set; }

    //TODO: Test this Version, and should this be decorated with [Timestamp]?
    //Should this be 'required', or set as required in the EF configuration?
    [Timestamp]
    public byte[] Version { get; set; }

}
