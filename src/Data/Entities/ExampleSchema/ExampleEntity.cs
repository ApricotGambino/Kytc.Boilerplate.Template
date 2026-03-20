using CodeGenerator;
using FluentValidation;
using Kernel.Data.Entities;

namespace Data.Entities.ExampleSchema;

//NOTE: This example entity showcases how to create your own entity, and how that flows all the way up through
//the service layer, into an endpoint. You're welcome to remove this whenever you feel you have a grasp on how to do things
//on your own.
//Unless you're certain of what you're doing, you shouldn't modify or add anything to the Kernel section of the source code.
//Which is why following this ExampleEntity is going to represent what you need to do for your application.

//NOTE: Don't use DataAnnotations (The baked in .net validation solution)
//They seem easy to use, because they are, but they don't work (or at least without some SEVERE changes that require
//pramga statements to silence 'internal usage' errors at the time of writing.
//They work out of the box if you're using them to validate on savechanges(), but not for MinimalApi
//validations, even though Microsoft says they should work, they just don't.  Don't use them, use FluentValidation

//TODO: Show all the different types of analyzer errors that can happen.

[DomainEntityFields]
public interface IExampleEntityFields
{
    /// <summary>
    /// This is just a string, it can be null, or string.Empty, this will make DB column of (nvarchar(max), null)
    /// </summary>
    public string? AString { get; set; }
    /// <summary>
    /// This is a string that we assume to have numbers. because it is denoted as <see cref="required"/>, it will
    /// create a DB column of (nvarchar(max, not null)
    /// </summary>
    public string AStringWithNumbers { get; set; }
    public int ANumber { get; set; }
    public bool ABool { get; set; }
    public DateTimeOffset ADateTimeOffset { get; set; }
    /// <summary>
    /// This is a nullable <see cref="DateTimeOffset"/> , using FluentValidation, we'll indicate that this can either be
    /// null, or if it has a value, it needs to not be 'in the past'
    /// </summary>
    public DateTimeOffset? AFutureDate { get; set; }
}

[DomainEntity]
public class ExampleEntity : BaseEntity, IExampleEntityFields
{
    public string? AString { get; set; }
    public required string AStringWithNumbers { get; set; }
    public int ANumber { get; set; }
    public bool ABool { get; set; }
    public DateTimeOffset ADateTimeOffset { get; set; }
    public DateTimeOffset? AFutureDate { get; set; }
}

public class ExampleEntityValidator : AbstractValidator<IExampleEntityFields>
{
    public ExampleEntityValidator()
    {
        //This will validate the AStringWithNumbers is no greater than 10 characters long, and must contain a number.
        RuleFor(x => x.AStringWithNumbers).NotEmpty().MaximumLength(10).Matches("\\d")
            .WithMessage($"{nameof(ExampleEntity.AStringWithNumbers)} must contain at least one number.");

        //This will check if the date is greater than or equal to today if it is not null.
        RuleFor(x => x.AFutureDate).GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage($"{nameof(ExampleEntity.AFutureDate)} must be on {DateTime.Today.ToShortDateString()} or later.")
            .When(p => p.AFutureDate != null);
    }
}
