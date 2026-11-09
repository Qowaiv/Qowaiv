using Qowaiv.Validation.DataAnnotations.Reflection;

namespace System.ComponentModel.DataAnnotations;

/// <summary>Extensions on <see cref="ValidationAttribute"/>.</summary>
public static class QowaivValidationAttributeExtensions
{
    extension(ValidationAttribute attribute)
    {
        /// <summary>Replacement of <see cref="ValidationAttribute.GetValidationResult(object, ValidationContext)"/>.</summary>
        /// <remarks>
        /// The original function does not handle <see cref="ValidationMessage.None"/> correctly, as it has no error message.
        /// </remarks>
        [Pure]
        public ValidationMessage? GetValidationMessage(object? value, ValidationContext validationContext)
        {
            Guard.NotNull(attribute);

            var result = ValidationAttributes.IsValid(attribute, value, validationContext);
            return ValidationMessage.For(result);
        }

        /// <summary>Validates the attribute.</summary>
        /// <remarks>
        /// To support both custom error messages via <see cref="ValidationAttribute.ErrorMessage"/>
        /// and via the a custom resource, the latter should be reset when setting
        /// the first. However, when setting <see cref="ValidationAttribute.ErrorMessage"/>
        /// The <see cref="ValidationAttribute.ErrorMessageResourceName"/> is not
        /// reset, which would result in a runtime error. This construct is not
        /// ideal, but it is the only point where we can alter the default
        /// behavior.
        /// </remarks>
        [Pure]
        internal bool PostValidate(bool result)
        {
            if (!result && attribute.ErrorMessage is { Length: > 0 })
            {
                attribute.ErrorMessageResourceName = null;
                attribute.ErrorMessageResourceType = null;
            }
            return result;
        }
    }
}
