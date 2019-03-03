using Qowaiv.ComponentModel.Messages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Qowaiv.ComponentModel.DataAnnotations
{
    /// <summary>Specifies that a data field value is immutable.</summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ImmutableAttribute : ValidationAttribute
    {
        /// <summary>Creates a new instance of an <see cref="ImmutableAttribute"/>.</summary>
        public ImmutableAttribute()
            : base(() => QowaivComponentModelMessages.ImmutableAttribute_ErrorMessage) { }

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

            var current = GetCurrent(validationContext);

            return IsDefaultValue(current) || current.Equals(value)
                ? ValidationMessage.None
                : ValidationMessage.Error(FormatErrorMessage(validationContext.MemberName), validationContext.MemberName);
        }

        /// <summary>Gets the current value of the involved member (property or field).</summary>
        private object GetCurrent(ValidationContext validationContext)
        {
            var member = validationContext.MemberName;
            var type = validationContext.ObjectType;
            var instance = validationContext.ObjectInstance;

            var property = type.GetProperty(member, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (property != null)
            {
                return property.GetValue(instance);
            }

            var field = type.GetField(member, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                return field.GetValue(instance);
            }

            throw new ArgumentException(string.Format(QowaivComponentModelMessages.ArgumentException_MemberCouldNotBeResolvedForType, member, type), nameof(validationContext));
        }

        /// <summary>Returns true if the value equals null or the default of its type.</summary>
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
