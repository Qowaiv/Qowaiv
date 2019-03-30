using System;
using System.Runtime.Serialization;

namespace Qowaiv.DomainModel.Events
{
    /// <summary>The exception that is thrown when an event type is not supported for a specific <see cref="AggregateRoot{TAggrgate}"/>.</summary>
    [Serializable]
    public class EventTypeNotSupportedException : NotSupportedException
    {
        /// <summary>Creates a new instance of an <see cref="EventTypeNotSupportedException"/>.</summary>
        public EventTypeNotSupportedException(Type eventType, Type aggragateType)
            : this(GetMessage(eventType, aggragateType))
        {
            EventType = eventType;
            AggregateType = aggragateType;
        }

        private static string GetMessage(Type eventType, Type aggragateType)
        {
            return string.Format(
                QowaivDomainModelMessages.EventTypeNotSupportedException, 
                eventType?.ToString() ?? "{null}", 
                aggragateType ?? typeof(AggregateRoot<>));
        }

        /// <summary>Creates a new instance of an <see cref="EventTypeNotSupportedException"/>.</summary>
        public EventTypeNotSupportedException() { }

        /// <summary>Creates a new instance of an <see cref="EventTypeNotSupportedException"/>.</summary>
        public EventTypeNotSupportedException(string message) : base(message) { }

        /// <summary>Creates a new instance of an <see cref="EventTypeNotSupportedException"/>.</summary>
        public EventTypeNotSupportedException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>Deserializes an <see cref="EventTypeNotSupportedException"/></summary>
        protected EventTypeNotSupportedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            EventType = (Type)info.GetValue(nameof(EventType), typeof(Type));
            AggregateType = (Type)info.GetValue(nameof(AggregateType), typeof(Type));
        }

        /// <summary>The event type that is not supported.</summary>
        public Type EventType { get; }

        /// <summary>The aggregate for which the event type is not supported.</summary>
        public Type AggregateType { get; }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(EventType), EventType);
            info.AddValue(nameof(AggregateType), AggregateType);
        }
    }
}
