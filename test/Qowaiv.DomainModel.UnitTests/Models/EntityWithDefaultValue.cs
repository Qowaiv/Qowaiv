using Qowaiv.Globalization;
using System.ComponentModel;

namespace Qowaiv.DomainModel.UnitTests.Models
{
    public class EntityWithDefaultValue : Entity<EntityWithDefaultValue>
    {
        [DefaultValue("is")]
        public Country Country
        {
            get => GetProperty<Country>();
            set => SetProperty(value);
        }
    }
}
