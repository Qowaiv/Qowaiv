using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Qowaiv.DomainModel.Events
{
    /// <summary>An event stream.</summary>
    public class EventStream : IEnumerable<VersionedEvent>
    {
        private readonly List<VersionedEvent> collection = new List<VersionedEvent>();

        /// <summary>Gets the identifier of the aggregate linked to the event stream.</summary>
        public Guid AggregateId => Version == 0 ? Guid.Empty : collection[0].Info.AggregateId;
        
        /// <summary>The version of the event stream.</summary>
        /// <remarks>
        /// Equal to the number of events in the stream.
        /// </remarks>
        public int Version => collection.Count;

        /// <summary>Gets the committed version of the event stream.</summary>
        public int CommittedVersion { get; private set; }

        /// <summary>Gets an event, based on its version.</summary>
        public VersionedEvent this[int version]
        {
            get
            {
                if(version < 1 || version > Version)
                {
                    throw new ArgumentOutOfRangeException(nameof(version), "");
                }
                return collection[version - 1];

            }
        }

        /// <summary>Initializes the event stream with an initial set of events.</summary>
        public void Initialize(IEnumerable<VersionedEvent> events)
        {
            if (Version != 0)
            {
                throw new InvalidOperationException("Already initialized");
            }
            var eventArray = Guard.HasAny(events?.ToArray(), nameof(events));
            var first = eventArray[0].Info;

            for (var i = 0; i < eventArray.Length; i++)
            {
                var info = eventArray[i].Info;
                if (info.AggregateId == Guid.Empty || info.AggregateId != first.AggregateId)
                {
                    throw new ArgumentException();
                }
                if (info.Version != i + 1)
                {
                    throw new EventsOutOfOrderException(nameof(events));
                }
            }
            collection.AddRange(events);
            MarkAllAsCommitted();
        }

        /// <summary>Adds an event to the event stream.</summary>
        public void Add(IEvent @event)
        {
            var info = new EventInfo(Version + 1, AggregateId == Guid.Empty ? Guid.NewGuid() : AggregateId, Clock.UtcNow());
            var versioned = new VersionedEvent(info, @event);
            collection.Add(versioned);
        }

        /// <summary>Gets the uncommitted events of the event stream.</summary>
        public IEnumerable<VersionedEvent> GetUncommitted() => this.Skip(CommittedVersion);

        /// <summary>Marks all events as being committed.</summary>
        public void MarkAllAsCommitted() => CommittedVersion = Version;

        /// <summary>Rolls back to a specific version.</summary>
        /// <remarks>
        /// By doing so, the events with higher versions are removed.
        /// </remarks>
        public void Rollback(int version)
        {
            collection.RemoveRange(version, Version - version);

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
        public IEnumerator<VersionedEvent> GetEnumerator() => collection.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
       
    }
}
