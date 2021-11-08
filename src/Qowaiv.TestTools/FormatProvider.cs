using System;
using System.Diagnostics.Contracts;

namespace Qowaiv.TestTools
{
    /// <summary>Helper class for getting <see cref="IFormatProvider"/>s.</summary>
    public static class FormatProvider
    {
        /// <summary>Gets an empty <see cref="IFormatProvider"/>.</summary>
        /// <remarks>
        /// <see cref="IFormatProvider.GetFormat(Type)"/> always returns null.
        /// </remarks>
        public static readonly IFormatProvider Empty = new EmptyFormatProvider();

        /// <summary>Represents a format provider, that contains no format types.</summary>
        private class EmptyFormatProvider : IFormatProvider
        {
            /// <summary>Always returns null.</summary>
            [Pure]
            public object GetFormat(Type formatType) => null;
        }
    }
}
