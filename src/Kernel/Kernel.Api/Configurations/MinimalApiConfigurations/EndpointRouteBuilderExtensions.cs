// EndpointRouteBuilderExtensions.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Scalar.AspNetCore;

namespace Kernel.Api.Configurations.MinimalApiConfigurations;

public static class EndpointRouteBuilderExtensions
{
    public static RouteHandlerBuilder MapGet(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern = "")
    {
        //Guard.Against.AnonymousMethod(handler);

        return builder.MapGet(handler.Method.Name, handler)
            .WithDescription("Test Description") //TODO: update this

        .WithName(handler.Method.Name)
        //.AddEndpointFilter(async (context, next) =>
        //{
        //    var result = await next(context);
        //    //if (context.HttpContext.StatusCode == StatusCodes.Status400BadRequest)
        //    //{
        //    //    return Results.ProblemDetails(..);
        //    //}
        //    return result;
        //});
        ;

        //return builder.MapGet(pattern, handler)
        //.WithName(handler.Method.Name);
    }

    public static RouteHandlerBuilder MapPost(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern = "")
    {
        var thing = handler.Method.GetParameters().Single();
        var thing2 = thing.GetType();
        dynamic v2 = thing2.GetType();

        var abstractValidatorType = typeof(ValidationFilter<>);

        var entityType = thing2.GetType();

        var genericAbstractValidatorType = abstractValidatorType.MakeGenericType(entityType);

        return builder.MapPost(handler.Method.Name, handler)
            .WithDescription("Test Description") //TODO: update this

        .WithName(handler.Method.Name)
        //.AddEndpointFilter<ValidationFilter<ExampleEntity>>();
        //.AddEndpointFilter < ValidationFilter < typeof(v2) >> ();
        //.AddEndpointFilter()
        //.AddEndpointFilter<genericAbstractValidatorType>();
        .AddEndpointFilter(async (invocationContext, next) =>
        {
            //var color = invocationContext.GetArgument<string>(0);

            //if (color == "Red")
            //{
            //    return Results.Problem("Red not allowed!");
            //}

            var thing = invocationContext.GetArgument<object>(0);
            var results = ValidateWithFluentValidation(thing);
            if (!results.IsValid)
            {
                return Results.ValidationProblem(results.ToDictionary());
            }
            return await next(invocationContext);
        });

        //Guard.Against.AnonymousMethod(handler);

        //return builder.MapPost(pattern, handler)
        //    .WithName(handler.Method.Name);
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

    ////TODO: Move this somewhere it should be if it works.
    //internal class ValidationFilter<T> : IEndpointFilter
    //    where T : class
    //{
    //    private readonly IValidator<T> _validator;

    //    public ValidationFilter(IValidator<T> validator)
    //    {
    //        _validator = validator;
    //    }

    //    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    //    {
    //        // 👇 this can be more complicated
    //        if (context.Arguments.FirstOrDefault(a => a is T) is not T model)
    //        {
    //            throw new InvalidOperationException("Model is null");
    //        }

    //        var validationResult = await _validator.ValidateAsync(model);

    //        if (!validationResult.IsValid)
    //        {
    //            return Results.ValidationProblem(validationResult.ToDictionary());
    //        }

    //        return await next(context);
    //    }
    //}
    //TODO: Move this somewhere it should be if it works.
    internal class ValidationFilter<T> : IEndpointFilter
    {
        private readonly object _objectToValidate;

        public ValidationFilter(T objectToValidate)
        {
            _objectToValidate = objectToValidate;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            //// 👇 this can be more complicated
            //if (context.Arguments.FirstOrDefault(a => a is T) is not T model)
            //{
            //    throw new InvalidOperationException("Model is null");
            //}

            var validationResult = ValidateWithFluentValidation(_objectToValidate);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            return await next(context);
        }


    }

    private static FluentValidation.Results.ValidationResult ValidateWithFluentValidation(object objectToValidate)
    {
        ArgumentNullException.ThrowIfNull(objectToValidate);

        var abstractValidatorType = typeof(AbstractValidator<>);

        var entityType = objectToValidate.GetType();

        var genericAbstractValidatorType = abstractValidatorType.MakeGenericType(entityType);

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        var validatorType = assemblies.SelectMany(s => s.GetTypes().Where(t => t.IsSubclassOf(genericAbstractValidatorType))).FirstOrDefault();

        if (validatorType != null)
        {
            //There may or may not be a fluentvalidation configuration for the entity we're working with.

            var validatorInstance = Activator.CreateInstance(validatorType) as IValidator;

            if (validatorInstance != null)
            {
                var validationContext = new ValidationContext<object>(objectToValidate);
                var results = validatorInstance.Validate(validationContext);
                return results;
            }
            else
            {
                //NOTE: I'm not sure what exactly would or could cause this, but if this happens, we probably need to stop
                //any inserts because at very least, our validation won't work.
                throw new InvalidOperationException($"Tried to create an instance of {validatorType.Name}, but could not.");
            }
        }

        return null;
    }


}
