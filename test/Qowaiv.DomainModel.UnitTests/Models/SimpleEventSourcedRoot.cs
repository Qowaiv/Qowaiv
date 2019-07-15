using System;
using Qowaiv.DomainModel.EventSourcing;
using Qowaiv.Validation.Abstractions;

namespace Qowaiv.DomainModel.UnitTests.Models
{
    public sealed class SimpleEventSourcedRoot : EventSourcedAggregateRoot<SimpleEventSourcedRoot>
    {
        public SimpleEventSourcedRoot() : base(Guid.NewGuid()) { }

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
