using Qowaiv;
using Qowaiv.Validation.DataAnnotations;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>Extensions on <see cref="ValidationContext"/>.</summary>
    public static class ValidationContextExtensions
    {
        /// <summary>Returns the service that provides custom validation.</summary>
        public static T GetSevice<T>(this ValidationContext validationContext)
        {
            Guard.NotNull(validationContext, nameof(validationContext));
            return (T)validationContext.GetService(typeof(T));
        }

        /// <summary>Gets a validation context for a property.</summary>
        public static ValidationContext ForProperty(this ValidationContext validationContext, AnnotatedProperty property)
        {
            Guard.NotNull(validationContext, nameof(validationContext));
            Guard.NotNull(property, nameof(property));
            return new ValidationContext(validationContext.ObjectInstance, validationContext, validationContext.Items)
            {
                MemberName = property.Name,
                DisplayName = property.DisplayAttribute.Name,
            };
        }
    }
}
