using System;
using System.Runtime.Serialization;

namespace Qowaiv.TestTools
{
    public class AssertException : Exception
    {
        public AssertException() : this("Assertion failed.") { }

        public AssertException(string message) : base(message) { }

        public AssertException(string message, Exception innerException) : base(message, innerException) { }

        protected AssertException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
