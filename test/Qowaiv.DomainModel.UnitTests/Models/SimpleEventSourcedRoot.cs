using Qowaiv.ComponentModel;
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

        public string Name
        {
            get => GetProperty<string>();
            private set => SetProperty(value);
        }

        public Result<SimpleEventSourcedRoot> SetName(UpdateNameEvent command) => ApplyChange(command);

        internal void Apply(UpdateNameEvent @event)
        {
            Name = @event.Name;
        }

        internal void Apply(SimpleInitEvent @event)
        {
            Initialized = true;
        }
    }

    public class UpdateNameEvent
    {
        public string Name { get; set; }
    }

    public class SimpleInitEvent { }
}
