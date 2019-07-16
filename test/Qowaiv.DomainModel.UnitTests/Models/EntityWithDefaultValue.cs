using Qowaiv.Globalization;
using Qowaiv.Validation.DataAnnotations;

namespace Qowaiv.DomainModel.UnitTests.Models
{
    public class EntityWithDefaultValue : AggregateRoot<EntityWithDefaultValue>
    {
        public EntityWithDefaultValue() : base(new AnnotatedModelValidator<EntityWithDefaultValue>()) 
        {
            Initialize(() => Country = Country.IS);
        }

        /// <summary>Initial value is not allowed.</summary>
        [ForbiddenValues("IS")]
        public Country Country
        {
            get => GetProperty<Country>();
            set => SetProperty(value);
        }
    }
}
