// ValidationHelper.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

using System.Reflection;
using FluentValidation;
using Kernel.Data.Entities;

namespace Kernel.Data.Helpers
{
    public static class ValidationHelper
    {
        /// <summary>
        /// Tries to create an instance of the FluentValidator if found for the provided object and validates.
        /// </summary>
        /// <param name="objectToValidate">
        /// An object that may have a FluentValidation validator configured for it.
        /// </param>
        /// <param name="validationResult">A validation result if a FluentValidator was found.</param>
        /// <returns>
        /// <para> <see cref="true"/> if the validation result was successful, or if no validator was found for the
        /// object. </para> <para> <see cref="false"/> if the validation result was not valid. </para>
        /// </returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static bool TryValidateWithFluentValidation(object objectToValidate, out FluentValidation.Results.ValidationResult? validationResult)
        {
            ArgumentNullException.ThrowIfNull(objectToValidate);

            Type? validatorType;
            validationResult = null;
            var abstractValidatorType = typeof(AbstractValidator<>);
            var objectType = objectToValidate.GetType();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            //First, try to get a validator by the object being presented, then look for a validator by the EntityFieldsAttribute interface.
            if (TryGetValidatorByObject(objectType, assemblies, abstractValidatorType, out validatorType)
                || TryGetValidatorByEntityFieldsInterface(objectType, assemblies, abstractValidatorType, out validatorType))
            {
                if (validatorType != null && Activator.CreateInstance(validatorType) is IValidator validatorInstance)
                {
                    var validationContext = new ValidationContext<object>(objectToValidate);
                    validationResult = validatorInstance.Validate(validationContext);
                    return validationResult.IsValid;
                }
                else
                {
                    //NOTE: I'm not sure what exactly would or could cause this, but if this happens, we probably need to stop
                    //any inserts because at very least, our validation won't work.
                    throw new InvalidOperationException($"Tried to create a validator instance of {validatorType?.Name} for {objectType.Name}, but could not.");
                }
            }

            //No Validator was found, so we'll assume there's no validation to do.
            return true;
        }

        /// <summary>
        /// Will attempt to get the validator by searching for validations that use the type of the object provided.
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="assemblies"></param>
        /// <param name="abstractValidatorType"></param>
        /// <param name="validator"></param>
        /// <returns></returns>
        private static bool TryGetValidatorByObject(Type objectType, Assembly[] assemblies, Type abstractValidatorType, out Type? validator)
        {
            validator = null;
            var genericAbstractValidatorType = abstractValidatorType.MakeGenericType(objectType);
            var validatorType = assemblies.SelectMany(s => s.GetTypes().Where(t => t.IsSubclassOf(genericAbstractValidatorType))).FirstOrDefault();

            if (validatorType != null)
            {
                validator = validatorType;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Will attempt to get the validator by searching the object's interfaces for an attribute of
        /// <see cref="EntityFieldsAttribute"/>
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="assemblies"></param>
        /// <param name="abstractValidatorType"></param>
        /// <param name="validator"></param>
        /// <returns></returns>
        private static bool TryGetValidatorByEntityFieldsInterface(Type objectType, Assembly[] assemblies, Type abstractValidatorType, out Type? validator)
        {
            validator = null;
            var entityFieldInterface = objectType.GetInterfaces().FirstOrDefault(p => p.GetCustomAttribute<EntityFieldsAttribute>() != null);
            if (entityFieldInterface != null)
            {
                var genericAbstractValidatorType = abstractValidatorType.MakeGenericType(entityFieldInterface);
                var validatorType = assemblies.SelectMany(s => s.GetTypes().Where(t => t.IsSubclassOf(genericAbstractValidatorType))).FirstOrDefault();

                if (validatorType != null)
                {
                    validator = validatorType;
                    return true;
                }
            }
            return false;
        }
    }
}
