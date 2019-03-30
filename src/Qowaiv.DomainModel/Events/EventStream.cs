using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Qowaiv.DomainModel.Events
{
    /// <summary>An event stream.</summary>
    public class EventStream : IEnumerable<IEvent>
    {
        private readonly List<IEvent> events = new List<IEvent>();

        /// <summary>Gets the identifier of the related aggregate root.</summary>
        public Guid AggregateId => Version == 0 ? Guid.Empty : events[0].Id;

        /// <summary>The version of the event stream.</summary>
        /// <remarks>
        /// Equal to the number of events in the stream.
        /// </remarks>
        public int Version => events.Count;

        public int CommittedVersion { get; private set; }

        /// <summary>Gets an event, based on its version.</summary>
        public IEvent this[int version]
        {
            get
            {
                if(version < 1 || version > Version)
                {
                    throw new ArgumentOutOfRangeException(nameof(version), "");
                }
                var @event = events[version - 1];
                @event.Version = version;
                @event.Id = AggregateId;
                return @event;
            }
        }

        /// <summary>Adds an event that is already committed.</summary>
        public void AddCommitted(IEnumerable<IEvent> @events)
        {
            lock (locker)
            {
                if (Version != 0) { throw new InvalidOperationException(QowaivDomainModelMessages.InvalidOperationException_NotEmptyEventStream); }

                this.events.AddRange(EventGuard.LoadFromHistory(events, nameof(events)));
            }
        }

        /// <summary>Adds an event that is uncommitted.</summary>
        public void AddUncommited(IEvent @event) => Add(@event, false);

        private void Add(IEvent @event, bool isCommited)
        {
            Guard.NotNull(@event, nameof(@event));
            
            lock(locker)
            {
                if (isCommited)
                {
                    if (@event.Version != Version + 1)
                    {
                        throw new ArgumentException(string.Format(QowaivDomainModelMessages.ArgumenException_VersionNotSuccessive, @event.Version, Version), nameof(@event));
                    }
                    if (Version > 0 && (@event.Id == Guid.Empty || @event.Id != AggregateId))
                    {
                        throw new ArgumentException(QowaivDomainModelMessages.ArgumentException_InvalidEventId, nameof(@event));
                    }
                }
                else
                {
                    @event.Version = Version + 1;
                }
                events.Add(@event);
            }
        }

        public void MarkAllAsCommitted() => CommittedVersion = Version;

        public IEnumerable<IEvent> GetUncommitted() => this.Skip(CommittedVersion);

        /// <summary>Gets the lock object to lock the event stream.</summary>
        public object Lock() => locker;

        #region IEnumerable

        private IEnumerable<IEvent> GetAll()
        {
            var aggregateId = AggregateId;
            var version = 0;
            var count = events.Count;

            while(version < count)
            {
                // be on the safe side:
                // set (aggregate) id
                // set version
                var @event = events[version++];
                @event.Version = version;
                @event.Id = aggregateId;
                yield return @event;
            }
        }

        /// <inheritdoc />
        public IEnumerator<IEvent> GetEnumerator() => GetAll().GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        private readonly object locker = new object();
    }
}
