using System;

namespace Qowaiv.DomainModel.Events
{
    /// <summary>A basic implementation of an <see cref="IEvent"/>.</summary>
    public abstract class EventBase : IEvent
    {
        /// <inheritdoc />
        public virtual Guid Id { get; set; }

        /// <inheritdoc />
        public virtual int Version { get; set; }
    }
}
