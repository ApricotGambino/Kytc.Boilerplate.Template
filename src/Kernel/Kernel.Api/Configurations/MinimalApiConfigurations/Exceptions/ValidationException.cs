// ValidationException.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

//using FluentValidation.Results;

//namespace Kernel.Api.Configurations.MinimalApiConfigurations.Exceptions;

//public class ValidationException : Exception
//{
//    public ValidationException()
//        : base("One or more validation failures have occurred.")
//    {
//        Errors = new Dictionary<string, string[]>();
//    }

//    public ValidationException(IEnumerable<ValidationFailure> failures)
//        : this()
//    {
//        Errors = failures
//            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
//            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
//    }

//    public IDictionary<string, string[]> Errors { get; }
//}
