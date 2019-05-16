using Qowaiv.DomainModel.EventSourcing;

namespace Qowaiv.DomainModel.UnitTests.Models
{
    public sealed class SimpleEventSourcedRoot : EventSourcedAggregateRoot<SimpleEventSourcedRoot>
    {
        public bool Initialized
        {
            get => GetProperty<bool>();
            private set => SetProperty(value);
        }

        internal void Apply(SimpleInitEvent @event)
        {
            Initialized = true;
        }
    }
    public class SimpleInitEvent{}

}
