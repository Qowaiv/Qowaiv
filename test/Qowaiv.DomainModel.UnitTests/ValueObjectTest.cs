using NUnit.Framework;
using Qowaiv.ComponentModel;
using Qowaiv.DomainModel.UnitTests.Models;
using Qowaiv.Globalization;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.DomainModel.UnitTests
{
    public class ValueObjectTest
    {
        [Test]
        public void Ctor_WithoutError_Constructed()
        {
            var address = new AddressValueObject("Downingstreet", 10, PostalCode.Parse("SW1A 2AA"), Country.GB);
            Assert.NotNull(address);
        }
        [Test]
        public void Ctor_WithError_Throws()
        {
            Assert.Throws<InvalidModelException>(() =>
            {
                new AddressValueObject("Downingstreet", 10, PostalCode.Parse("SW1A 2AA"), Country.Empty);
            });
        }
    }
}
