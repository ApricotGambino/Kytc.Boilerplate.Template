// EndpointRouteBuilderExtensions.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Kernel.Api.Configurations.MinimalApiConfigurations.EndpointFilters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Scalar.AspNetCore;

namespace Kernel.Api.Configurations.MinimalApiConfigurations;

public static class EndpointRouteBuilderExtensions
{
    public static RouteHandlerBuilder MapGet(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern = "")
    {
        ArgumentNullException.ThrowIfNull(handler);

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
