using GeneratedDtos;
using Kernel.Data.Entities;

namespace Application.Features.ExampleFeature.Dtos;

public record ExampleEntityGeneratedFullDto() : ExampleEntityResponse, IBaseEntityAuditProperties
{
    public DateTimeOffset CreatedDateTimeOffset { get; set; }
    public DateTimeOffset? UpdatedDateTimeOffset { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsSoftDeleted { get; set; }
}
