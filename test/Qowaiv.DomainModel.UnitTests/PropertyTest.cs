using NUnit.Framework;
using Qowaiv.DomainModel.UnitTests.Models;
using Qowaiv.Globalization;
using Qowaiv.Validation.Abstractions;
using Qowaiv.Validation.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.DomainModel.UnitTests
{
    public class PropertyTest
    {
        [Test]
        public void DefaultValue_SomeModel_Initialized()
        {
            var entity = new EntityWithDefaultValue();
            Assert.AreEqual(Country.IS, entity.Country);
        }

        [Test]
        public void GuardType_SameValueButMandatory_Throws()
        {
            var entity = new PropertyTestEntity();

            Assert.Throws<InvalidModelException>(() =>
            {
                entity.RequiredProperty = 0;
            });
        }
        
        private class PropertyTestEntity : AggregateRoot<PropertyTestEntity>
        {
            public PropertyTestEntity() : base(new AnnotatedModelValidator<PropertyTestEntity>()) { }
            [Mandatory]
            public int RequiredProperty
            {
                get => GetProperty<int>();
                set => SetProperty(value);
            }

            [Display(Name = "Display property")]
            public string DisplayProperty
            {
                get => GetProperty<string>();
                set => SetProperty(value);
            }
        }
    }
}
