using Qowaiv.Validation.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.DomainModel.UnitTests.Models
{
    public class EntityWithCaculatedProperty : AggregateRoot<EntityWithCaculatedProperty>
    {
        public EntityWithCaculatedProperty()
            : base(Guid.NewGuid(), new AnnotatedModelValidator<EntityWithCaculatedProperty>()) { }

        public decimal Value
        {
            get => GetProperty<decimal>();
            set => SetProperty(value);
        }
        public int Repertitions
        {
            get => GetProperty<int>();
            set => SetProperty(value);
        }

        [Range(15, 150)]
        public decimal Total => Value * Repertitions;
    }
}
