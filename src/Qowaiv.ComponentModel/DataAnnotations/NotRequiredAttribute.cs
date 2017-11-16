using System;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.DataAnnotations
{
    /// <summary>Null object pattern implementation for a <see cref="RequiredAttribute"/>.</summary>
    /// <remarks>
    /// See: https://en.wikipedia.org/wiki/Null_object_pattern
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    internal sealed class NotRequiredAttributeAttribute : RequiredAttribute
    {
        /// <summary>Returns true as an <see cref="NotRequiredAttributeAttribute"/> is always valid.</summary>
        public override bool IsValid(object value) => true;
    }
}
