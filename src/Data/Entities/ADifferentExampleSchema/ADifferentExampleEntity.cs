using CodeGenerator;
using Data.Entities.ExampleSchema;
using Kernel.Data.Entities;

namespace Data.Entities.ADifferentExampleSchema;

//TODO: Remove this?

[DomainEntityFields]
public interface IADifferentExampleEntityFields
{
    /// <summary>
    /// This is just a string, it can be null, or string.Empty, this will make DB column of (nvarchar(max), null)
    /// </summary>
    public string Name { get; set; }
    public int ExampleEntityId { get; set; }


    public ExampleEntity ExampleEntity { get; set; }

}

[DomainEntity]
public class ADifferentExampleEntity : BaseEntity, IADifferentExampleEntityFields
{
    //TODO: Write an analyzer to force null or required on strings.
    public required string Name { get; set; }
    public int ExampleEntityId { get; set; }

    //TODO: Write an analyzer to force = null!, explain why.
    public ExampleEntity ExampleEntity { get; set; } = null!;
}

//public class ExampleEntityValidator : AbstractValidator<IExampleEntityFields>
//{
//    public ExampleEntityValidator()
//    {
//        //This will validate the AStringWithNumbers is no greater than 10 characters long, and must contain a number.
//        RuleFor(x => x.AStringWithNumbers).NotEmpty().MaximumLength(10).Matches("\\d")
//            .WithMessage($"{nameof(ExampleEntity.AStringWithNumbers)} must contain at least one number.");

//        //This will check if the date is greater than or equal to today if it is not null.
//        RuleFor(x => x.AFutureDate).GreaterThanOrEqualTo(DateTime.Today)
//            .WithMessage($"{nameof(ExampleEntity.AFutureDate)} must be on {DateTime.Today.ToShortDateString()} or later.")
//            .When(p => p.AFutureDate != null);
//    }
//}

//[DomainEntity]
//public class TestThing : BaseEntity, ITestThingFields
//{
//    public string? Name { get; set; }
//}

//[DomainEntityFields]
//public interface ITestThingFields
//{
//    /// <summary>
//    /// Hi
//    /// </summary>
//    public string? Name { get; set; }
//}

//public class Thing
//{
//    public TestService
//}

