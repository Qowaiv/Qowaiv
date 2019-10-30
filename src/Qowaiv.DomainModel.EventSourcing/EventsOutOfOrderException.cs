using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Qowaiv.DomainModel.EventSourcing
{
    /// <summary>Thrown when events are out of order/</summary>
    [Serializable]
    public class EventsOutOfOrderException : ArgumentOutOfRangeException
    {
        /// <summary>Creates a new instance of an <see cref="EventsOutOfOrderException"/>.</summary>
        public EventsOutOfOrderException(string paramName)
            : base(paramName, QowaivDomainModelMessages.EventsOutOfOrderException) { }

        /// <summary>Creates a new instance of an <see cref="EventsOutOfOrderException"/>.</summary>
        [ExcludeFromCodeCoverage/* Required Exception constructor for inheritance. */]
        public EventsOutOfOrderException() { }

        /// <summary>Creates a new instance of an <see cref="EventsOutOfOrderException"/>.</summary>
        [ExcludeFromCodeCoverage/* Required Exception constructor for inheritance. */]
        public EventsOutOfOrderException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>Creates a new instance of an <see cref="EventsOutOfOrderException"/>.</summary>
        [ExcludeFromCodeCoverage/* Required Exception constructor for inheritance. */]
        protected EventsOutOfOrderException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
