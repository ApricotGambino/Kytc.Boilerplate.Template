namespace Domain.Entities.Common;
using System;
using System.ComponentModel.DataAnnotations;



public abstract class BaseEntity : BaseEntity<int> { }
public abstract class BaseEntity<T>
{
    public required T Id { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
    public bool IsActive { get; set; }
    [Timestamp]
    public byte[] Version { get; set; }

}
