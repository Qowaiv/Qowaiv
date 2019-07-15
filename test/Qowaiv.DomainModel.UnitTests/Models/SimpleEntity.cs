using Qowaiv.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.DomainModel.UnitTests.Models
{
    internal class SimpleEntity : AggregateRoot<SimpleEntity>
    {
        [Mandatory, MaxLength(3), Display(Name = "Full name")]
        public string FullName
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
    }
}
