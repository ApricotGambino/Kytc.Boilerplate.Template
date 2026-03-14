// ValidationException.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

namespace Kernel.Api.Exceptions.ExceptionTypes;
//TODO: Document this

public class ValidationException : Exception
{
    public IDictionary<string, string[]> ValidationErrors { get; }
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        ValidationErrors = new Dictionary<string, string[]>();
    }

    public ValidationException(IDictionary<string, string[]> validationErrors)
        : this()
    {
        ValidationErrors = validationErrors;
    }

    public ValidationException(string? message) : base(message)
    {
        ValidationErrors = new Dictionary<string, string[]>();
    }

    public ValidationException(string? message, Exception? innerException) : base(message, innerException)
    {
        ValidationErrors = new Dictionary<string, string[]>();
    }
}
