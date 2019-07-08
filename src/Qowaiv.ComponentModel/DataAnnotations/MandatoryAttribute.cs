using System;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.DataAnnotations
{
    /// <summary>Specifies that a field is mandatory (for value types the default value is not allowed).</summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class MandatoryAttribute : RequiredAttribute
    {
        /// <summary>Gets or sets a value that indicates whether an empty string is allowed.</summary>
        public bool AllowUnknownValue { get; set; }

        /// <summary>Gets or sets that the type of the attribute is a <see cref="Nullable{T}"/>.</summary>
        /// <remarks>
        /// Because the argument provided as an <see cref="object"/> (and
        /// therefor boxed), it is not possible to get the right <see cref="Type"/>
        /// of an set <see cref="Nullable{T}"/>, as the boxing removes the 
        /// <see cref="Nullable{T}"/> wrapper.
        /// 
        /// To prevent a mixture of <see cref="MandatoryAttribute"/> and
        /// <see cref="RequiredAttribute"/>, this option can be flagged to
        /// prevent set nullables with the default value to be marked as
        /// invalid.
        /// </remarks>
        public bool IsNullable { get; set; }

        /// <summary>Returns true if the value is not null and value types are
        /// not equal to their default value, otherwise false.
        /// </summary>
        /// <remarks>
        /// The unknown value is expected to be static field or property of the type with the name "Unknown".
        /// </remarks>
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                var type = value.GetType();

                if (!AllowUnknownValue && value.Equals(Unknown.Value(type)))
                {
                    return false;
                }
                if (type.IsValueType && !IsNullable)
                {
                    return !value.Equals(Activator.CreateInstance(value.GetType()));
                }
            }
            return base.IsValid(value);
        }
    }
}
