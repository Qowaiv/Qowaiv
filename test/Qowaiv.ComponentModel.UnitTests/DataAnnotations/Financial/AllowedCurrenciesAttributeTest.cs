using NUnit.Framework;
using Qowaiv.ComponentModel.DataAnnotations.Financial;
using Qowaiv.Financial;
using System;

namespace Qowaiv.ComponentModel.UnitTests.DataAnnotations.Financial
{
    public class AllowedCurrenciesAttributeTest
    {
        [Test]
        public void Ctor_WithCommaSeperatedList_Parsed()
        {
            var attribute = new AllowedCurrenciesAttribute("EUR, USD");
            Assert.AreEqual(new[] { Currency.EUR, Currency.USD }, attribute.AllowedCurrencies);
        }

        [Test]
        public void Ctor_None_Throws()
        {
            Assert.Throws<ArgumentException>(() => new AllowedCurrenciesAttribute());
        }

        [Test]
        public void IsValid_Null_IsTrue()
        {
            var attribute = new AllowedCurrenciesAttribute("EUR");
            Assert.IsTrue(attribute.IsValid(null));
        }

        [Test]
        public void IsValid_NotMoney_Throws()
        {
            var attribute = new AllowedCurrenciesAttribute("EUR");
            Assert.Throws<ArgumentException>(() => attribute.IsValid(45));
        }

        [Test]
        public void IsValid_NullableMoney_ÍsValid()
        {
            var attribute = new AllowedCurrenciesAttribute("EUR");
            Money? value = 1234 + Currency.EUR;
            Assert.True(attribute.IsValid(value));
        }

        [Test]
        public void IsValid_AnyCurrencyOfMultiple_True()
        {
            var attribute = new AllowedCurrenciesAttribute("USD", "EUR", "GBP");
            Assert.IsTrue(attribute.IsValid(1234 + Currency.GBP));
        }

        [Test]
        public void IsValid_WrongCurrencyOfMultiple_False()
        {
            var attribute = new AllowedCurrenciesAttribute("USD", "EUR");
            Assert.False(attribute.IsValid(1234 + Currency.GBP));
        }

        [Test]
        public void IsValid_WrongCurrency_False()
        {
            var attribute = new AllowedCurrenciesAttribute("USD");
            Assert.False(attribute.IsValid(1234 + Currency.EUR));
        }
    }
}
