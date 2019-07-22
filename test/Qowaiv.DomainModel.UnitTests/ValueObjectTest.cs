using NUnit.Framework;
using Qowaiv.DomainModel.UnitTests.Models;
using Qowaiv.Globalization;

namespace Qowaiv.DomainModel.UnitTests
{
    public class ValueObjectTest
    {
        [Test]
        public void Equals_TwoIdenticalInstances_AreEqual()
        {
            var act = new AddressValueObject("Downingstreet", 10, PostalCode.Parse("SW1A 2AA"), Country.GB);
            var exp = new AddressValueObject("Downingstreet", 10, PostalCode.Parse("SW1A 2AA"), Country.GB);

            Assert.AreNotSame(exp, act);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Equals_TwoIDifferentInstances_AreEqual()
        {
            var act = new AddressValueObject("Downingstreet", 10, PostalCode.Parse("SW1A 2AA"), Country.GB);
            var exp = new AddressValueObject("Downingstreet", 17, PostalCode.Parse("SW1A 2AA"), Country.GB);

            Assert.AreNotEqual(exp, act);
        }

        [Test]
        public void Equals_SameInstances_AreEqual()
        {
            var act = new AddressValueObject("Downingstreet", 10, PostalCode.Parse("SW1A 2AA"), Country.GB);

            Assert.IsTrue(act.Equals(act));
        }

        [Test]
        public void Equlity_SameInstances_IsTrue()
        {
            var left = new AddressValueObject("Downingstreet", 10, PostalCode.Parse("SW1A 2AA"), Country.GB);
            var right = new AddressValueObject("Downingstreet", 10, PostalCode.Parse("SW1A 2AA"), Country.GB);

            Assert.IsTrue(left == right);
        }

        [Test]
        public void Inequality_SameInstances_IsFalse()
        {
            var left = new AddressValueObject("Downingstreet", 10, PostalCode.Parse("SW1A 2AA"), Country.GB);
            var right = new AddressValueObject("Downingstreet", 10, PostalCode.Parse("SW1A 2AA"), Country.GB);

            Assert.IsFalse(left != right);
        }

        [Test]
        public void GetHashCode_TwoIdenticalInstances_AreEqual()
        {
            var act = new AddressValueObject("Downingstreet", 10, PostalCode.Parse("SW1A 2AA"), Country.GB).GetHashCode();
            var exp = new AddressValueObject("Downingstreet", 10, PostalCode.Parse("SW1A 2AA"), Country.GB).GetHashCode();

            Assert.AreEqual(exp, act);
        }
    }
}
