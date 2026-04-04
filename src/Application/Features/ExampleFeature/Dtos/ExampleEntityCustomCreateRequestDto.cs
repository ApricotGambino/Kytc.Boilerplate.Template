using Data.Entities.ExampleSchema;
using FluentValidation;

namespace Application.Features.ExampleFeature.Dtos;

public record ExampleEntityCustomCreateRequestDto() : IExampleEntityFields
{
    public required string? AString { get; set; }
    public required string AStringWithNumbers { get; set; }
    public required int ANumber { get; set; }
    public required bool ABool { get; set; }
    public required DateTimeOffset ADateTimeOffset { get; set; }
    public required DateTimeOffset? AFutureDate { get; set; }

    public required string ACompletelyNewValueNotFromTheEntity { get; set; }
}

public class ExampleEntityCustomCreateValidator : AbstractValidator<ExampleEntityCustomCreateRequestDto>
{
    public ExampleEntityCustomCreateValidator()
    {
        //Include the 'normal' validations from the actual entity.
        Include(new ExampleEntityValidator());
        //This will validate the AStringWithNumbers is no greater than 10 characters long, and must contain a number.
        RuleFor(x => x.ACompletelyNewValueNotFromTheEntity).NotEmpty().MaximumLength(2).Must(p => p.Contains("#"))
            .WithMessage($"{nameof(ExampleEntityCustomCreateRequestDto.ACompletelyNewValueNotFromTheEntity)} must contain a '#' symbol.");
    }
}
