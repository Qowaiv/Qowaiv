using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Qowaiv.DomainModel.EventSourcing
{
    /// <summary>Thrown when a provided <see cref="EventStream"/> does not describe a (potential) full history of events.</summary>
    [Serializable]
    public class EventStreamNoFullHistoryException : ArgumentException
    {
        /// <summary>Creates a new instance of <see cref="EventStreamNoFullHistoryException"/>.</summary>
        public EventStreamNoFullHistoryException(string paramName)
            : base(QowaivDomainModelMessages.EventStreamNoFullHistoryException, paramName) { }

        /// <summary>Creates a new instance of <see cref="EventStreamNoFullHistoryException"/>.</summary>
        [ExcludeFromCodeCoverage/* Required Exception constructor for inheritance. */]
        public EventStreamNoFullHistoryException() { }

        /// <summary>Creates a new instance of <see cref="EventStreamNoFullHistoryException"/>.</summary>
        [ExcludeFromCodeCoverage/* Required Exception constructor for inheritance. */]
        public EventStreamNoFullHistoryException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>Creates a new instance of <see cref="EventStreamNoFullHistoryException"/>.</summary>
        [ExcludeFromCodeCoverage/* Required Exception constructor for inheritance. */]
        public EventStreamNoFullHistoryException(string message, string paramName, Exception innerException) : base(message, paramName, innerException) { }

        /// <summary>Creates a new instance of <see cref="EventStreamNoFullHistoryException"/>.</summary>
        [ExcludeFromCodeCoverage/* Required Exception constructor for inheritance. */]
        protected EventStreamNoFullHistoryException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
