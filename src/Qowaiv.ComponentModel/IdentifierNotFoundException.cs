using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Qowaiv.ComponentModel
{
    /// <summary>An exception that can be thrown when an model lacks an identifier.</summary>
    [Serializable]
    public class IdentifierNotFoundException : ValidationException
    {
        /// <summary>Creates a new instance of an <see cref="IdentifierNotFoundException"/>.</summary>
        public IdentifierNotFoundException() : this(QowaivComponentModelMessages.IdentifierNotFoundException) { }

        /// <summary>Creates a new instance of an <see cref="IdentifierNotFoundException"/>.</summary>
        public IdentifierNotFoundException(string message) : base(message) { }

        /// <summary>Creates a new instance of an <see cref="IdentifierNotFoundException"/>.</summary>
        public IdentifierNotFoundException(string message, Exception inner) : base(message, inner) { }

        /// <summary>Creates a new instance of an <see cref="IdentifierNotFoundException"/>.</summary>
        protected IdentifierNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
