using System;

namespace Qowaiv.DomainModel.UnitTests.Models
{
    public class EmptyEntity : AggregateRoot<EmptyEntity>
    {
        public EmptyEntity(): this(Guid.NewGuid()) { }

        public EmptyEntity(Guid id) : base(id) { }
    }
}
