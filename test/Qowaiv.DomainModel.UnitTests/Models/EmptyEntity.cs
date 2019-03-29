using System;

namespace Qowaiv.DomainModel.UnitTests.Models
{
    public class EmptyEntity : Entity<EmptyEntity>
    {
        public EmptyEntity() { }

        public EmptyEntity(Guid id) => Id = id;
    }
}
