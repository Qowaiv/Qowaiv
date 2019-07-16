using Qowaiv.Validation.Abstractions;
using Qowaiv.Validation.DataAnnotations;
using System;

namespace Qowaiv.DomainModel.UnitTests.Models
{
    public class EntityWithInitScope : AggregateRoot<EntityWithInitScope>
    {
        public EntityWithInitScope(Guid id)
            : base(id, new AnnotatedModelValidator<EntityWithInitScope>()) { }

        [Mandatory]
        public string Name
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        [Mandatory]
        public Date StartDate
        {
            get => GetProperty<Date>();
            set => SetProperty(value);
        }

        public static Result<EntityWithInitScope> FromData(Guid id, string name, Date startDate)
        {
            var entity = new EntityWithInitScope(id);

            return entity.TrackChanges((e) =>
            {
                e.Name = name;
                e.StartDate = startDate;
            });
        }
    }
}
