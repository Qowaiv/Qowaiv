using System;

namespace Qowaiv.DomainModel.UnitTests.Models
{
    public class EmptyEntity : AggregateRoot<EmptyEntity>
    {
        public EmptyEntity() { }

        public EmptyEntity(Guid id) => Id = id;
    }
}
