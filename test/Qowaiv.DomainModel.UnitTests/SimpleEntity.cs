using Qowaiv.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.DomainModel.UnitTests
{
    internal class SimpleEntity : Entity<Guid>
    {
        public SimpleEntity() { }
        public SimpleEntity(Guid id) : base(id) { }

        [Mandatory, MaxLength(3), Display(Name = "Full name")]
        public string FullName
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
    }
}
