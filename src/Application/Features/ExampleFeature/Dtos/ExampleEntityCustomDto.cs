using Data.Entities.ExampleSchema;
using GeneratedDtos;
using Kernel.Data.Mapping;

namespace Application.Features.ExampleFeature.Dtos;

public record ExampleEntityCustomDto : ExampleEntityResponse, IMap<ExampleEntity, ExampleEntityCustomDto>
{
    public required string ACompletelyNewValueNotFromTheEntity { get; set; }
    public string AStringJoinedWithANumber
    {
        get
        {
            return $"{AString}-{ANumber}";
        }
    }

    public static new ExampleEntityCustomDto MapFrom(ExampleEntity entity)
    {
        return new ExampleEntityCustomDto()
        {
            Id = entity.Id,
            AString = entity.AString,
            AStringWithNumbers = entity.AStringWithNumbers,
            ANumber = entity.ANumber,
            ABool = entity.ABool,
            ADateTimeOffset = entity.ADateTimeOffset,
            AFutureDate = entity.AFutureDate,
            ACompletelyNewValueNotFromTheEntity = "This value came from the interface implemented method."
        };
    }

    public static ExampleEntityCustomDto ComplexMapFrom(ExampleEntity entity, int isEven)
    {
        var customDto = MapFrom(entity);
        customDto.ACompletelyNewValueNotFromTheEntity = isEven % 2 == 0 ? "Is Even" : "Is Odd";
        return customDto;
    }
}
