using System;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.DataAnnotations
{
    /// <summary>Specifies that a field is mandatory (for value types the default is rejected).</summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class MandatoryAttribute : RequiredAttribute
    {
        /// <summary>Returns true if the value is not null and value types are
        /// not equal to their default value, otherwise false.
        /// </summary>
        public override bool IsValid(object value)
        {
            if (value != null && value.GetType().IsValueType)
            {
                return !value.Equals(Activator.CreateInstance(value.GetType()));
            }
            return base.IsValid(value);
        }
    }
}
