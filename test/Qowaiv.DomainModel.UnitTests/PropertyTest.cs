using NUnit.Framework;
using Qowaiv.ComponentModel.DataAnnotations;
using Qowaiv.DomainModel.UnitTests.Models;
using Qowaiv.Globalization;
using System;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.DomainModel.UnitTests
{
    public class PropertyTest
    {
        [Test]
        public void DefaultValue_SomeModel_Initialized()
        {
            var entity = new EntityWithDefaultValue(17);
            Assert.AreEqual(Country.IS, entity.Country);
        }

        [Test]
        public void GuardType_WrongType_Throws()
        {
            var entity = new PropertyTestEntity();

            Assert.Throws<ArgumentException>(() =>
            {
                entity.WrongTypeProperty = 12;    
            });
        }

        [Test]
        public void GuardType_SameValueButMandatory_Throws()
        {
            var entity = new PropertyTestEntity();

            Assert.Throws<ValidationException>(() =>
            {
                entity.RequiredProperty = 0;
            });
        }

        [Test]
        public void ToString_PropertyWithDisplayAttribute_SomeInformativeText()
        {
            var entity = new PropertyTestEntity
            {
                DisplayProperty = "Hello World!"
            };

            Assert.AreEqual("Display property, Value: Hello World!", entity.ToStringAccessor());
        }

        private class PropertyTestEntity : Entity<int>
        {
            public int WrongTypeProperty
            {
                get => GetProperty<int>();
                // Sets the value using a string to trigger the validation.
                set => SetProperty(value.ToString());
            }

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

            public string ToStringAccessor()
            {
                return Properties[nameof(DisplayProperty)].ToString();
            }
        }
    }
}
