using Qowaiv.ComponentModel.Messages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Qowaiv.ComponentModel.DataAnnotations
{
    /// <summary>Specifies that a data field value is immutable.</summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class ImmutableAttribute : ValidationAttribute
    {
        /// <summary>Creates a new instance of an <see cref="ImmutableAttribute"/>.</summary>
        public ImmutableAttribute() { }

        /// <summary>Creates a new instance of an <see cref="ImmutableAttribute"/>.</summary>
        /// <param name="errorMessageAccessor">
        /// The function that enables access to validation resources.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// errorMessageAccessor is null.
        /// </exception>
        public ImmutableAttribute(Func<string> errorMessageAccessor)
            : base(errorMessageAccessor) { }

        /// <summary>Creates a new instance of an <see cref="ImmutableAttribute"/>.</summary>
        /// <param name="errorMessage">
        /// The error message to associate with a validation control.
        /// </param>
        public ImmutableAttribute(string errorMessage)
            : base(errorMessage) { }

        /// <inheritdoc />
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Guard.NotNull(validationContext, nameof(validationContext));
            if (IsDefaultValue(value))
            {
                throw new ArgumentException(QowaivMessages.ArgumentException_IsDefaultValue, nameof(value));
            }

            var property = validationContext.ObjectType.GetProperty(validationContext.MemberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (property is null)
            {
                throw new ArgumentException("Validation context contains unresolvable property.");
            }

            var current = property.GetValue(validationContext.ObjectInstance);

            return IsDefaultValue(current)
                ? ValidationMessage.None
                : ValidationMessage.Error(FormatErrorMessage(validationContext.MemberName), validationContext.MemberName);
        }

        private static bool IsDefaultValue(object value)
        {
            if (value is null)
            {
                return true;
            }
            
            var type = value.GetType();

            return type.IsValueType && Activator.CreateInstance(type).Equals(value);
        }
    }
}
