using Qowaiv.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Qowaiv.DomainModel.EventSourcing
{
    /// <summary>An event stream.</summary>
    [DebuggerDisplay("{DebuggerDisplay}"), DebuggerTypeProxy(typeof(CollectionDebugView))]
    public class EventStream : IEnumerable<EventMessage>
    {
        /// <summary>Underlying list.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<EventMessage> messages = new List<EventMessage>();

        /// <summary>Creates a new instance of an <see cref="EventStream"/>.</summary>
        public EventStream() : this(Guid.NewGuid()) { }

        /// <summary>Creates a new instance of an <see cref="EventStream"/>.</summary>
        public EventStream(Guid aggregateId) => AggregateId = Guard.NotEmpty(aggregateId, nameof(aggregateId));

        /// <summary>Gets the identifier of the aggregate linked to the event stream.</summary>
        public Guid AggregateId { get; }
        
        /// <summary>The version of the event stream.</summary>
        /// <remarks>
        /// Equal to the number of events in the stream.
        /// </remarks>
        public int Version => messages.Count;

        /// <summary>Gets the committed version of the event stream.</summary>
        public int CommittedVersion { get; private set; }

        /// <summary>Gets a lock object to lock the event stream.</summary>
        public object Lock() => locker;
        private readonly object locker = new object();

        /// <summary>Gets an event, based on its version.</summary>
        public EventMessage this[int version]
        {
            get
            {
                if(version < 1 || version > Version)
                {
                    throw new ArgumentOutOfRangeException(nameof(version), string.Format(
                        QowaivDomainModelMessages.ArgumentOutOfRangeException_InvalidVersion, 
                        version, AggregateId));
                }
                return messages[version - 1];

            }
        }

        /// <summary>Adds an event to the event stream.</summary>
        public void Add(object @event)
        {
            lock (locker)
            {
                var info = new EventInfo(Version + 1, AggregateId, Clock.UtcNow());
                var versioned = new EventMessage(info, @event);
                messages.Add(versioned);
            }
        }

        /// <summary>Gets the uncommitted events of the event stream.</summary>
        public IEnumerable<EventMessage> GetUncommitted() => this.Skip(CommittedVersion);

        /// <summary>Marks all events as being committed.</summary>
        public void MarkAllAsCommitted() => CommittedVersion = Version;

        /// <summary>Returns a <see cref="string"/> that represents the event stream for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal string DebuggerDisplay
        {
            get => Version == CommittedVersion
                ? $"Version: {Version}"
                : $"Version: {Version} (Committed: {CommittedVersion})";
        }

        #region IEnumerable

        /// <inheritdoc />
        public IEnumerator<EventMessage> GetEnumerator() => messages.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        /// <summary>Initializes the event stream with an initial set of events.</summary>
        public static EventStream FromMessages(IEnumerable<EventMessage> events)
        {
            var eventArray = Guard.HasAny(events?.ToArray(), nameof(events));
            var first = eventArray[0].Info;

            for (var i = 0; i < eventArray.Length; i++)
            {
                var info = eventArray[i].Info;

                if (info.AggregateId != first.AggregateId)
                {
                    throw new ArgumentException(QowaivDomainModelMessages.ArgumentException_MultipleAggregates, nameof(events));
                }
                if (info.Version != i + 1)
                {
                    throw new EventsOutOfOrderException(nameof(events));
                }
            }
            var stream = new EventStream(first.AggregateId);
            stream.messages.AddRange(eventArray);
            stream.MarkAllAsCommitted();
            return stream;
        }
    }
}
