using Qowaiv.Globalization;
using System.ComponentModel;

namespace Qowaiv.DomainModel.UnitTests.Models
{
    public class EntityWithDefaultValue : Entity<int>
    {
        public EntityWithDefaultValue(int id) : base(id) { }

        [DefaultValue("is")]
        public Country Country
        {
            get => GetProperty<Country>();
            set => SetProperty(value);
        }
    }
}
