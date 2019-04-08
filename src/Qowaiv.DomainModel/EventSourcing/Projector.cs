using Qowaiv.DomainModel.Dynamic;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Qowaiv.DomainModel.EventSourcing
{
    /// <summary>Represent a projector base class.</summary>
    public abstract class Projector
    {
        /// <summary>Creates a new instance of a <see cref="Projector"/>.</summary>
        protected Projector()
        {
            _dynamic = new DynamicApplyEventObject(this);
            _supportedEventTypes = new HashSet<Type>(_dynamic.SupportedEventTypes);
        }

        /// <summary>Applies the projection defined for the event (type).</summary>
        /// <param name="event">
        /// The event that should be applied on the projection.
        /// </param>
        public void ApplyProjection(object @event)
        {
            Guard.NotNull(@event, nameof(@event));

            if (_supportedEventTypes.Contains(@event.GetType()))
            {
                AsDynamic().Apply(@event);
            }
        }

        /// <summary>Represents the projector as a dynamic.</summary>
        private dynamic AsDynamic() => _dynamic;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DynamicApplyEventObject _dynamic;

        private readonly HashSet<Type> _supportedEventTypes;
    }
}
