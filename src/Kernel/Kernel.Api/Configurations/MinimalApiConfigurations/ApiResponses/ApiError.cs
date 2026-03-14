// ApiError.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

using System.Net;
using Kernel.Infrastructure.Extensions.String;

namespace Kernel.Api.Configurations.MinimalApiConfigurations.ApiResponses;

//TODO: Document this.
public class ApiError
{
    public ApiError(string title, IDictionary<string, string[]> messages, string traceIdentifier, HttpStatusCode httpStatusCode)
    {
        Title = title.ToPresentedFormat();
        TraceIdentifier = traceIdentifier;
        HttpStatusCode = httpStatusCode;

        //NOTE: I couldn't figure out how to format this dictonary in a quick one-liner, if you can and it's readable, please do so.
        var formattedMessages = new Dictionary<string, string[]>();
        foreach (var key in messages.Keys)
        {
            List<string> keyValues = new List<string>();
            foreach (var value in messages[key])
            {
                keyValues.Add(value.ToPresentedFormat());
            }

            //Keys should not end in a period.
            formattedMessages.Add(key.TrimAndReduce().RemoveAllPeriodsFromEndIfFound(), keyValues.ToArray());
        }

        Messages = formattedMessages;
    }

    public ApiError(string title, string message, string traceIdentifier, HttpStatusCode httpStatusCode)
    {
        Title = title.ToPresentedFormat();
        TraceIdentifier = traceIdentifier;
        HttpStatusCode = httpStatusCode;
        Messages = new Dictionary<string, string[]>() { { Title, [message.ToPresentedFormat()] } };
    }

    public string Title { get; }
    public string TraceIdentifier { get; }
    public HttpStatusCode HttpStatusCode { get; }
    public IDictionary<string, string[]> Messages { get; }

    public static ApiError None => new(string.Empty, string.Empty, string.Empty, HttpStatusCode.OK);
}
