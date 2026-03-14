// ExceptionHandler.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

using System.Net;
using Kernel.Api.Configurations.MinimalApiConfigurations.ApiResponses;
using Kernel.Api.Exceptions.ExceptionTypes;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;


namespace Kernel.Api.Exceptions;

//TODO: Document this.
public class ExceptionHandler : IExceptionHandler
{
    private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;

    //TODO: Reinclude logger?
    //private readonly ILogger<ExceptionHandler> _logger;
    private readonly IOptions<BaseAppSettings> _appSettings;

    //public ExceptionHandler(ILogger<ExceptionHandler> logger, IOptions<BaseAppSettings> appSettings)
    public ExceptionHandler(IOptions<BaseAppSettings> appSettings)
    {
        // Register known exception types and handlers.
        _exceptionHandlers = new()
        {
            {typeof(BadHttpRequestException), HandleBadHttpRequestExceptionAsync },
            {typeof(ApiPresentedException), HandleApiPresentedExceptionAsync },
            {typeof(ValidationException), HandleValidationExceptionAsync }

            //{ typeof(ValidationException), HandleValidationExceptionAsync },
            //{ typeof(NotFoundException), HandleNotFoundExceptionAsync },
            //{ typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
            //{ typeof(ForbiddenAccessException), HandleForbiddenAccessException },
        };

        //_logger = logger;
        _appSettings = appSettings;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var exceptionType = exception.GetType();

        if (_exceptionHandlers.ContainsKey(exceptionType))
        {
            await _exceptionHandlers[exceptionType].Invoke(httpContext, exception);
        }
        else
        {
            await HandleUnexpectedExceptionAsync(httpContext, exception);

        }
        return true;
    }

    private static Task HandleBadHttpRequestExceptionAsync(HttpContext httpContext, Exception ex)
    {
        var exception = (BadHttpRequestException)ex;
        var error = new ApiError(title: "Invalid request format", message: exception.GetBaseException().Message, traceIdentifier: httpContext.TraceIdentifier, HttpStatusCode.BadRequest);
        return httpContext.Response.WriteAsJsonAsync(ApiResponse.Failure(error));
    }

    private Task HandleUnexpectedExceptionAsync(HttpContext httpContext, Exception ex)
    {
        //_logger.Log();
        var errorTitle = _appSettings.Value.EnableDetailedExceptionMessages ? $"{ex.Message}" : "An internal server error has occurred.";
        var errorMessage = _appSettings.Value.EnableDetailedExceptionMessages ? ex.ToString() : $"Please contact the system administrator to report this error and provide this TraceIdentifier: {httpContext.TraceIdentifier}";
        var error = new ApiError(title: errorTitle, message: errorMessage, traceIdentifier: httpContext.TraceIdentifier, HttpStatusCode.InternalServerError);
        return httpContext.Response.WriteAsJsonAsync(ApiResponse.Failure(error));
    }

    private static Task HandleApiPresentedExceptionAsync(HttpContext httpContext, Exception ex)
    {
        //_logger.Log();
        var exception = (ApiPresentedException)ex;
        var errorTitle = "An expected error has occurred.";
        var error = new ApiError(title: errorTitle, message: exception.Message, traceIdentifier: httpContext.TraceIdentifier, HttpStatusCode.OK);
        return httpContext.Response.WriteAsJsonAsync(ApiResponse.Failure(error));
    }
    private static Task HandleValidationExceptionAsync(HttpContext httpContext, Exception ex)
    {
        var exception = (ValidationException)ex;
        var errorTitle = "One or more validation failures have occurred.";
        var error = new ApiError(title: errorTitle, messages: exception.ValidationErrors, traceIdentifier: httpContext.TraceIdentifier, HttpStatusCode.OK);
        return httpContext.Response.WriteAsJsonAsync(ApiResponse.ValidationFailure(error));
    }




    //
    //public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    //{

    //    var exceptionType = exception.GetType();

    //    if (_exceptionHandlers.ContainsKey(exceptionType))
    //    {
    //        await _exceptionHandlers[exceptionType].Invoke(httpContext, exception);
    //        return true;
    //    }

    //    return false;


    //    //context.Response.StatusCode = 500;
    //    //context.Response.ContentType = "application/json";
    //    //var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

    //    //if (httpContext is not null)
    //    //{
    //    //    await context.Response.WriteAsJsonAsync(new
    //    //    {
    //    //        StatusCode = context.Response.StatusCode,
    //    //        Message = "Internal Server Error",
    //    //        MoreInfo = contextFeature.Error.Message
    //    //    });
    //    //}

    //    //return false;
    //}
}

//namespace Kernel.Api.Configurations.MinimalApiConfigurations.Exceptions
//{
//    public class ExceptionHandler : IExceptionHandler
//    {
//        private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;

//        public ExceptionHandler()
//        {
//            // Register known exception types and handlers.
//            _exceptionHandlers = new()
//            {
//                { typeof(ValidationException), HandleValidationExceptionAsync },
//                { typeof(NotFoundException), HandleNotFoundExceptionAsync },
//                { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
//                { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
//            };
//        }

//        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
//        {
//            var exceptionType = exception.GetType();

//            if (_exceptionHandlers.ContainsKey(exceptionType))
//            {
//                await _exceptionHandlers[exceptionType].Invoke(httpContext, exception);
//                return true;
//            }

//            return false;
//        }

//        private async Task HandleValidationExceptionAsync(HttpContext httpContext, Exception ex)
//        {
//            var exception = (ValidationException)ex;

//            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

//            await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails(exception.Errors)
//            {
//                Status = StatusCodes.Status400BadRequest,
//                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
//            });
//        }

//        private async Task HandleNotFoundExceptionAsync(HttpContext httpContext, Exception ex)
//        {
//            var exception = (NotFoundException)ex;

//            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

//            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails()
//            {
//                Status = StatusCodes.Status404NotFound,
//                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
//                Title = "The specified resource was not found.",
//                Detail = exception.Message
//            });
//        }

//        private async Task HandleUnauthorizedAccessException(HttpContext httpContext, Exception ex)
//        {
//            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

//            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
//            {
//                Status = StatusCodes.Status401Unauthorized,
//                Title = "Unauthorized",
//                Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
//            });
//        }

//        private async Task HandleForbiddenAccessException(HttpContext httpContext, Exception ex)
//        {
//            httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

//            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
//            {
//                Status = StatusCodes.Status403Forbidden,
//                Title = "Forbidden",
//                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
//            });
//        }
//    }
//}
