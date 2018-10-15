using Qowaiv.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.DomainModel.UnitTests
{
    internal class SimpleEntity : Entity<Guid>
    {
        public void NewId(Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
        }

        [Mandatory, MaxLength(3), Display(Name = "Full name")]
        public string FullName
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
    }
}
