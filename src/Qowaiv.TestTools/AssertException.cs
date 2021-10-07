using System;
using System.Runtime.Serialization;

namespace Qowaiv.TestTools
{
    /// <summary>Assert exception.</summary>
    /// <remarks>
    /// Exists to be independent to external test frameworks.
    /// </remarks>
    [Serializable]
    public class AssertException : Exception
    {
        /// <inheritdoc />
        public AssertException() : this("Assertion failed.") { }

        /// <inheritdoc />
        public AssertException(string message) : base(message) { }

        /// <inheritdoc />
        public AssertException(string message, Exception innerException) : base(message, innerException) { }

        /// <inheritdoc />
        protected AssertException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
