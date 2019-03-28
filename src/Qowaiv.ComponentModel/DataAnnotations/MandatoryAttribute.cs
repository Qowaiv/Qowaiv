using System;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.DataAnnotations
{
    /// <summary>Specifies that a field is mandatory (for value types the default is rejected).</summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class MandatoryAttribute : RequiredAttribute
    {
        /// <summary>Gets or sets a value that indicates whether an empty string is allowed.</summary>
        public bool AllowUnknownValue { get; set; }

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
                if (type.IsValueType)
                {
                    return !value.Equals(Activator.CreateInstance(value.GetType()));
                }
            }
            return base.IsValid(value);
        }
    }
}
