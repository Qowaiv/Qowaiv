using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qowaiv.ComponentModel.Messages
{
    /// <summary>Represents a validation error message.</summary>
    [Serializable]
    public class ValidationErrorMessage : ValidationMessage
    {
        /// <summary>Creates a new instance of a <see cref="ValidationErrorMessage"/>.</summary>
        /// <param name="message">
        /// The validation message.
        /// </param>
        /// <param name="memberNames">
        /// The involved members.
        /// </param>
        internal ValidationErrorMessage(string message, IEnumerable<string> memberNames)
            : base(message, memberNames) { }

        /// <summary>Creates a new instance of <see cref="ValidationErrorMessage"/>.</summary>
        protected ValidationErrorMessage(SerializationInfo info, StreamingContext context) :
            base(info, context){ }

        /// <summary>Gets the Severity (Error) of the message.</summary>
        public sealed override ValidationSeverity Severity => ValidationSeverity.Error;
    }
}
