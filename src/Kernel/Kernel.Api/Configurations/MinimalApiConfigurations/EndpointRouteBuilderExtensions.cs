// EndpointRouteBuilderExtensions.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Kernel.Api.Configurations.MinimalApiConfigurations;

public static class EndpointRouteBuilderExtensions
{
    public static RouteHandlerBuilder MapGet(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern = "")
    {
        //Guard.Against.AnonymousMethod(handler);

        return builder.MapGet(pattern, handler)
        .WithName(handler.Method.Name);
    }

    //public static RouteHandlerBuilder MapPost(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern = "")
    //{
    //    Guard.Against.AnonymousMethod(handler);

    //    return builder.MapPost(pattern, handler)
    //        .WithName(handler.Method.Name);
    //}

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
