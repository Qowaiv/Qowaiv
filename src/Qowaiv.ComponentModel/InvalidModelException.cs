using Qowaiv.ComponentModel.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Qowaiv.ComponentModel
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
        public InvalidModelException(string message, Type modelType, Exception innerException, IEnumerable<ValidationResult> messages)
            : this(message, innerException)
        {
            var errors = new List<ValidationMessage>();

            if(messages != null)
            {
                foreach(var error in messages.Where(e => e.GetSeverity() >= ValidationSeverity.Error))
                {
                    // As validationResult is not ISerializable and ValidationMessage is.
                    var serializable = error as ValidationMessage
                        ?? ValidationMessage.Error(error.ErrorMessage, error.MemberNames.ToArray());

                    errors.Add(serializable);
                }
            }
            ModelType = modelType;
            Errors = new ReadOnlyCollection<ValidationMessage>(errors);
        }

        /// <summary>Creates a new instance of an <see cref="InvalidModelException"/>.</summary>
        protected InvalidModelException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ModelType = info.GetValue(nameof(ModelType), typeof(Type)) as Type;
            var errors = info.GetValue(nameof(Errors), typeof(ValidationMessage[])) as ValidationMessage[];
            Errors = new ReadOnlyCollection<ValidationMessage>(errors ?? new ValidationMessage[0]);
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(Errors), Errors.ToArray());
            info.AddValue(nameof(ModelType), ModelType);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var sb = new StringBuilder().AppendLine(base.ToString());
            foreach(var error in Errors)
            {
                sb.Append(error.ErrorMessage);
                if (error.MemberNames.Any())
                {
                    sb.Append(" (")
                        .Append(string.Join(", ", error.MemberNames))
                        .AppendLine(")");
                }
                else
                {
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }

        /// <summary>The related validation error(s).</summary>
        public IReadOnlyList<ValidationMessage> Errors { get; } = new ReadOnlyCollection<ValidationMessage>(new ValidationMessage[0]);

        /// <summary>The type of the invalid model.</summary>
        public Type ModelType { get; }

        /// <summary>Creates an <see cref="InvalidModelException"/> for the model.</summary>
        public static InvalidModelException For<T>(IEnumerable<ValidationResult> messages)
        {
            return new InvalidModelException(string.Format(QowaivComponentModelMessages.InvalidModelException, typeof(T)), typeof(T), null, messages);
        }
    }
}
