using Qowaiv.Globalization;
using System;
using System.ComponentModel;

namespace Qowaiv.DomainModel.UnitTests.Models
{
    public class EntityWithDefaultValue : AggregateRoot<EntityWithDefaultValue>
    {
        public EntityWithDefaultValue() : base(Guid.NewGuid()) { }

        [DefaultValue("is")]
        public Country Country
        {
            get => GetProperty<Country>();
            set => SetProperty(value);
        }
    }
}
