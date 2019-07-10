using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Qowaiv.Validation.DataAnnotations
{
    /// <summary>Compares objects just by reference.</summary>
    /// <remarks>
    /// We don't want to be cut of in the nested validation due to 
    /// <see cref="object.GetHashCode()"/> and <see cref="object.Equals(object)"/>
    /// overrides.
    /// </remarks>
    public class ReferenceComparer : IEqualityComparer<object>
    {
        /// <summary>Gets the singleton instance.</summary>
        public static readonly IEqualityComparer<object> Instance = new ReferenceComparer();

        /// <summary>No public accessor.</summary>
        private ReferenceComparer() { }

        /// <inheritdoc />
        public new bool Equals(object x, object y) => ReferenceEquals(x, y);

        /// <inheritdoc />
        public int GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
    }
}
