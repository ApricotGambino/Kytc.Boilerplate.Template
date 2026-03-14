// ValidationFilter.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

using Kernel.Api.Exceptions.ExceptionTypes;
using Microsoft.AspNetCore.Http;

namespace Kernel.Api.Configurations.MinimalApiConfigurations.EndpointFilters
{
    /// <summary>
    /// A filter for endpoints to attempt to validate a provided incoming request object.
    /// </summary>
    public class ValidationFilter : IEndpointFilter
    {
        public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var objectToValidate = context.GetArgument<object>(0);

            if (!Data.Helpers.ValidationHelper.TryValidateWithFluentValidation(objectToValidate, out var validationResult))
            {
                //If there are any validation issues, we'll conclude the exectuion chain early and return those errors.
                throw new ValidationException(validationResult!.ToDictionary());
            }

            return next(context);
        }
    }
}
