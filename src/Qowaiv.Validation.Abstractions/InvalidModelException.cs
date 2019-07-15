using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Qowaiv.Validation.Abstractions
{
    /// <summary>Represents an exception that is shown once tried to access the invalid model of a <see cref="Result{T}"/>.</summary>
    [Serializable]
    public class InvalidModelException : InvalidOperationException
    {
        /// <summary>Creates a new instance of an <see cref="InvalidModelException"/>.</summary>
        public InvalidModelException() { }

        /// <summary>Creates a new instance of an <see cref="InvalidModelException"/>.</summary>
        public InvalidModelException(string message)
            : base(message) { }

        /// <summary>Creates a new instance of an <see cref="InvalidModelException"/>.</summary>
        public InvalidModelException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>Creates a new instance of an <see cref="InvalidModelException"/>.</summary>
        public InvalidModelException(string message, Exception innerException, IEnumerable<IValidationMessage> messages)
            :this(message, innerException)
        {
            Errors = new ReadOnlyCollection<IValidationMessage>(messages.Where(e => e.Severity >= ValidationSeverity.Error).ToArray());
        }

        /// <summary>Creates a new instance of an <see cref="InvalidModelException"/>.</summary>
        protected InvalidModelException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            var errors = info.GetValue(nameof(Errors), typeof(IValidationMessage[])) as IValidationMessage[];
            Errors = new ReadOnlyCollection<IValidationMessage>(errors);
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(Errors), Errors.ToArray());
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var sb = new StringBuilder().AppendLine(base.ToString());
            foreach(var error in Errors)
            {
                sb.Append(error.Message);
                if (!string.IsNullOrEmpty(error.PropertyName))
                {
                    sb.Append(" (").Append(error.PropertyName).AppendLine(")");
                }
                else
                {
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }

        /// <summary>The related validation error(s).</summary>
        public IReadOnlyList<IValidationMessage> Errors { get; } = new ReadOnlyCollection<IValidationMessage>(new IValidationMessage[0]);

        /// <summary>Creates an <see cref="InvalidModelException"/> for the model.</summary>
        public static InvalidModelException For<T>(IEnumerable<IValidationMessage> messages)
        {
            return new InvalidModelException(string.Format(QowaivValidationMessages.InvalidModelException, typeof(T)), null, messages);
        }
    }
}
