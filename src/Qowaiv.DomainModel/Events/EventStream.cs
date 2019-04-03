using Qowaiv.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Qowaiv.DomainModel.Events
{
    /// <summary>An event stream.</summary>
    [DebuggerDisplay("{DebuggerDisplay}"), DebuggerTypeProxy(typeof(CollectionDebugView))]
    public class EventStream : IEnumerable<EventMessage>
    {
        private readonly List<EventMessage> messages = new List<EventMessage>();

        /// <summary>Gets the identifier of the aggregate linked to the event stream.</summary>
        public Guid AggregateId => Version == 0 ? Guid.Empty : messages[0].Info.AggregateId;
        
        /// <summary>The version of the event stream.</summary>
        /// <remarks>
        /// Equal to the number of events in the stream.
        /// </remarks>
        public int Version => messages.Count;

        /// <summary>Gets the committed version of the event stream.</summary>
        public int CommittedVersion { get; private set; }

        /// <summary>Gets an event, based on its version.</summary>
        public EventMessage this[int version]
        {
            get
            {
                if(Version == 0)
                {
                    throw new InvalidOperationException(QowaivDomainModelMessages.InvalidOperationException_NotInitializedEventStream);
                }
                if(version < 1 || version > Version)
                {
                    throw new ArgumentOutOfRangeException(nameof(version), string.Format(
                        QowaivDomainModelMessages.ArgumentOutOfRangeException_InvalidVersion, 
                        version, AggregateId));
                }
                return messages[version - 1];

            }
        }

        /// <summary>Initializes the event stream with an initial set of events.</summary>
        public void Initialize(IEnumerable<EventMessage> events)
        {
            if (Version != 0)
            {
                throw new InvalidOperationException(QowaivDomainModelMessages.InvalidOperationException_InitializedEventStream);
            }
            var eventArray = Guard.HasAny(events?.ToArray(), nameof(events));
            var first = eventArray[0].Info;

            for (var i = 0; i < eventArray.Length; i++)
            {
                var info = eventArray[i].Info;

                Guard.NotEmpty(info.AggregateId, $"events[{i}].AggregateId");

                if (info.AggregateId != first.AggregateId)
                {
                    throw new ArgumentException(QowaivDomainModelMessages.ArgumentException_MultipleAggregates, nameof(events));
                }
                if (info.Version != i + 1)
                {
                    throw new EventsOutOfOrderException(nameof(events));
                }
            }
            messages.AddRange(eventArray);
            MarkAllAsCommitted();
        }

        /// <summary>Adds an event to the event stream.</summary>
        public void Add(IEvent @event)
        {
            var info = new EventInfo(Version + 1, AggregateId == Guid.Empty ? Guid.NewGuid() : AggregateId, Clock.UtcNow());
            var versioned = new EventMessage(info, @event);
            messages.Add(versioned);
        }

        /// <summary>Gets the uncommitted events of the event stream.</summary>
        public IEnumerable<EventMessage> GetUncommitted() => this.Skip(CommittedVersion);

        /// <summary>Marks all events as being committed.</summary>
        public void MarkAllAsCommitted() => CommittedVersion = Version;

        /// <summary>Rolls back to a specific version.</summary>
        /// <remarks>
        /// By doing so, the events with higher versions are removed.
        /// </remarks>
        public void Rollback(int version)
        {
            messages.RemoveRange(version, Version - version);

            if (CommittedVersion >Version)
            {
                CommittedVersion = Version;
            }
        }

        /// <summary>Lock this to lock the event stream.</summary>
        public object Lock() => locker;
        private readonly object locker = new object();

        #region IEnumerable

        /// <inheritdoc />
        public IEnumerator<EventMessage> GetEnumerator() => messages.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
        
        /// <summary>Returns a <see cref="string"/> that represents the event stream for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal string DebuggerDisplay
        {
            get => Version == CommittedVersion
                ? $"Version: {Version}"
                : $"Version: {Version} (Committed: {CommittedVersion})";
        }
    }
}
