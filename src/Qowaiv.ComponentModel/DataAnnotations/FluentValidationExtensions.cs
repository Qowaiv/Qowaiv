using System.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.DataAnnotations
{
    /// <summary>Extensions that help writing fluent validations.</summary>
    /// <remarks>
    /// Could help in <see cref="IValidatableObject"/> scenarios where you only
    /// want to have validation on property groups if they are valid on property
    /// level.
    /// </remarks>
    public static class FluentValidationExtensions
    {
        /// <summary>Validates if the object is valid for a <see cref="MandatoryAttribute"/>.</summary>
        public static FluentValidationResult Mandatory(this object obj, bool allowUnkown = false, bool allowEmptyStrings = false)
        {
            return obj.IsValid(new MandatoryAttribute { AllowUnknownValue = allowUnkown, AllowEmptyStrings = allowEmptyStrings });
        }

        /// <summary>Validates if the object is valid for the specified <see cref="ValidationAttribute"/>.</summary>
        /// <param name="obj">
        /// The <see cref="object"/> to validate.
        /// </param>
        /// <param name="attribute">
        /// The attribute to use to validate the <see cref="object"/>.
        /// </param>
        /// <returns>
        /// <see cref="FluentValidationResult.False"/> if object is not valid for the attribute or already <see cref="FluentValidationResult.False"/>,
        /// otherwise a <see cref="FluentValidationResult"/> with the object value.
        /// </returns>
        public static FluentValidationResult IsValid(this object obj, ValidationAttribute attribute)
        {
            Guard.NotNull(attribute, nameof(attribute));

            var value = obj;

            if (obj is FluentValidationResult wrapper)
            {
                value = wrapper.Value;
                if (false.Equals(value))
                {
                    return wrapper;
                }
            }

            return attribute.IsValid(value)
                ? new FluentValidationResult(value)
                : FluentValidationResult.False;
        }
    }
}
