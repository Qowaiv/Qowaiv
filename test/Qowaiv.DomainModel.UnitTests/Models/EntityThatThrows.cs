using Qowaiv.Validation.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.DomainModel.UnitTests.Models
{
    public class EntityThatThrows : AggregateRoot<EntityThatThrows>, IValidatableObject
    {
        public EntityThatThrows() : base(new AnnotatedModelValidator<EntityThatThrows>())
        {
            Initialize(() =>
            {
                Value = 17;
            });
        }

        public int Value
        {
            get => GetProperty<int>();
            set => SetProperty(value);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new DivideByZeroException();
        }
    }
}
