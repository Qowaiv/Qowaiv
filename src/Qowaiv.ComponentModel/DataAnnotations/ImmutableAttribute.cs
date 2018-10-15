using Qowaiv.ComponentModel.Messages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Qowaiv.ComponentModel.DataAnnotations
{
    public sealed class ImmutableAttribute : ValidationAttribute
    {
        public ImmutableAttribute() { }

        public ImmutableAttribute(Func<string> errorMessageAccessor)
            : base(errorMessageAccessor) { }

        public ImmutableAttribute(string errorMessage)
            : base(errorMessage) { }

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
                : ValidationMessage.Error("", validationContext.MemberName);
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
