// ApiPresentedException.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

namespace Kernel.Api.Configurations.MinimalApiConfigurations.Exceptions;

//TODO: Document this
public class ApiPresentedException : Exception
{
    public ApiPresentedException()
        : base("An error has occurred.")
    {
    }

    public ApiPresentedException(string? message) : base(message)
    {
    }

    public ApiPresentedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
