using Qowaiv.Reflection;
using System;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.Validation.DataAnnotations
{
    /// <summary>Specifies that a field is mandatory (for value types the default value is not allowed).</summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class MandatoryAttribute : RequiredAttribute
    {
        /// <summary>Gets or sets a value that indicates whether an empty string is allowed.</summary>
        public bool AllowUnknownValue { get; set; }

        /// <inheritdoc />
        public override bool RequiresValidationContext => true;

        /// <inheritdoc />
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Guard.NotNull(validationContext, nameof(validationContext));

            if (IsValid(value, GetMemberType(validationContext)))
            {
                return ValidationResult.Success;
            }

            var memberNames = validationContext.MemberName != null ? new[] { validationContext.MemberName } : null;
            return ValidationMessage.Error(FormatErrorMessage(validationContext.DisplayName), memberNames);
        }

        /// <summary>Gets the type of the field/property.</summary>
        /// <remarks>
        /// Because the values of the member are boxed, this is (unfortunately)
        /// the only way to determine if the provided value is a nullable type,
        /// or not.
        /// </remarks>
        private Type GetMemberType(ValidationContext context)
        {
            if (string.IsNullOrEmpty(context.MemberName))
            {
                return null;
            }
            return context.ObjectType.GetProperty(context.MemberName)?.PropertyType
                ?? context.ObjectType.GetField(context.MemberName)?.FieldType;
        }

        /// <summary>Returns true if the value is not null and value types are
        /// not equal to their default value, otherwise false.
        /// </summary>
        /// <remarks>
        /// The unknown value is expected to be static field or property of the type with the name "Unknown".
        /// </remarks>
        public override bool IsValid(object value) => IsValid(value, null);

        private bool IsValid(object value, Type memberType)
        {
            if (value != null)
            {
                var type = memberType ?? value.GetType();
                var underlyingType = QowaivType.GetNotNullableType(type);

                if (!AllowUnknownValue && value.Equals(Unknown.Value(underlyingType)))
                {
                    return false;
                }
                if (type.IsValueType)
                {
                    return !value.Equals(Activator.CreateInstance(type));
                }
            }
            return base.IsValid(value);
        }
    }
}
