using System;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.Validation.DataAnnotations
{
    /// <summary>Null object pattern implementation for a <see cref="RequiredAttribute"/>.</summary>
    /// <remarks>
    /// See: https://en.wikipedia.org/wiki/Null_object_pattern
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    internal sealed class NotRequiredAttributeAttribute : RequiredAttribute
    {
        /// <summary>Gets a (singleton) <see cref="NotRequiredAttributeAttribute"/>.</summary>
        public static readonly NotRequiredAttributeAttribute Optional = new NotRequiredAttributeAttribute();

        /// <summary>Returns true as an <see cref="NotRequiredAttributeAttribute"/> is always valid.</summary>
        public override bool IsValid(object value) => true;
    }
}
