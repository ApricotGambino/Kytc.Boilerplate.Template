// ApiError.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

using System.Net;
using Kernel.Infrastructure.Extensions.String;

namespace Kernel.Api.Configurations.MinimalApiConfigurations.ApiResponses;

//TODO: Document this.
public class ApiError
{
    public ApiError(string title, string message, string traceIdentifier, HttpStatusCode httpStatusCode)
    {
        Title = title.ToPresentedFormat();
        Message = message.ToPresentedFormat();
        TraceIdentifier = traceIdentifier;
        HttpStatusCode = httpStatusCode;
    }

    public string Title { get; }
    public string Message { get; }
    public string TraceIdentifier { get; }
    public HttpStatusCode HttpStatusCode { get; }

    public static ApiError None => new(string.Empty, string.Empty, string.Empty, HttpStatusCode.OK);

    // TODO: Explain 'implicit' and 'operator'
    //public static implicit operator ApiError(string message) => new(message, HttpStatusCode.InternalServerError);

    //public static implicit operator string(ApiError error) => error.Message;
}
