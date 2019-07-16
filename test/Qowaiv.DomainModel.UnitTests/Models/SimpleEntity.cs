using Qowaiv.Validation.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.DomainModel.UnitTests.Models
{
    internal class SimpleEntity : AggregateRoot<SimpleEntity>
    {
        public SimpleEntity() : base(new AnnotatedModelValidator<SimpleEntity>()) { }

        [Mandatory, MaxLength(3), Display(Name = "Full name")]
        public string FullName
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
    }
}
