// EndpointRouteBuilderExtensions.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Kernel.Api.Configurations.MinimalApiConfigurations.ApiResponses;
using Kernel.Api.Configurations.MinimalApiConfigurations.EndpointFilters;
using Kernel.Infrastructure.Extensions.Pagination;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Scalar.AspNetCore;

namespace Kernel.Api.Configurations.MinimalApiConfigurations;

//TODO: Document.

public static class EndpointRouteBuilderExtensions
{
    public static RouteHandlerBuilder MapGet(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern = "")
    {
        ArgumentNullException.ThrowIfNull(handler);

        var a = handler.Method.ReturnType;

        if (handler.Method.ReturnType.BaseType != typeof(Task))
        {
            throw new ArgumentException($"{handler.Method.Name} must be a Task.");
        }

        if (handler.Method.ReturnType.GenericTypeArguments[0].GetInterface(nameof(IResult)) == null)
        {
            throw new ArgumentException($"{handler.Method.Name} must have generic type that implments {nameof(IResult)}  Does this method return OK()?");
        }

        var apiResponseType = handler.Method.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].BaseType;
        if (apiResponseType == typeof(Object))
        {
            apiResponseType = handler.Method.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].UnderlyingSystemType;
        }
        if (apiResponseType != typeof(ApiResponse<>) && apiResponseType != typeof(ApiResponse))
        {
            throw new ArgumentException($"{handler.Method.Name} must return the standarized {nameof(ApiResponse)} object.");
        }

        if (handler.Method.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].GenericTypeArguments.Length > 0)
        {
            var dataReturnType = handler.Method.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].GenericTypeArguments[0];

            if (dataReturnType.IsGenericType && dataReturnType != typeof(PagedResults<>))
            {
                throw new ArgumentException($"{handler.Method.Name} must return either a single DTO object, or a collection of objects as a {nameof(PagedResults<>)}. Lists are not permitted as they do not sufficiently limit the results being returned to the client.");
            }

            //if (!dataReturnType.Name.ToLower().Contains("dto"))
            //{
            //    throw new ArgumentException($"{handler.Method.Name} is returning a {dataReturnType.Name} and must return an object that has 'dto' in the name, are you sure you're returning a DTO?");
            //}

            //if (dataReturnType == typeof(PagedResults<>))
            //{
            //    throw new ArgumentException($"{handler.Method.Name} must use  {nameof(PagedResults<>)}.");
            //}
        }


        var b = handler.Method.ReturnType.GetGenericArguments();


        return builder
            .MapGet(handler.Method.Name, handler)
            .WithName(handler.Method.Name);
    }

    public static RouteHandlerBuilder MapPost(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern = "")
    {
        ArgumentNullException.ThrowIfNull(handler);

        return builder
                .MapPost(handler.Method.Name, handler)
                .WithName(handler.Method.Name)
                .AddEndpointFilter<ValidationFilter>();
    }

    //public static RouteHandlerBuilder MapPut(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern)
    //{
    //    Guard.Against.AnonymousMethod(handler);

    //    return builder.MapPut(pattern, handler)
    //        .WithName(handler.Method.Name);
    //}

    //public static RouteHandlerBuilder MapDelete(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern)
    //{
    //    Guard.Against.AnonymousMethod(handler);

    //    return builder.MapDelete(pattern, handler)
    //        .WithName(handler.Method.Name);
    //}
}
