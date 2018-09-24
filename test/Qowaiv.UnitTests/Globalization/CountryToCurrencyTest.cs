using NUnit.Framework;
using Qowaiv.Financial;
using Qowaiv.Globalization;
using System;
using System.Linq;

namespace Qowaiv.UnitTests.Globalization
{
    [TestFixture]
    public class CountryToCurrencyTest
    {
        private static bool TestMode = false;

        private static readonly Country[] CountriesWithoutCurrency = new[] { Country.AQ };

        [Test]
        public void GetCurrency_AllCountries_CurrencyShouldExistOnStartDateAndEndDateOrToday()
        {
            foreach (var country in Country.All.Where(c => !CountriesWithoutCurrency.Contains(c)))
            {
                CurrencyExists(country, country.StartDate);
                CurrencyExists(country, country.EndDate);
            }
        }

        private void CurrencyExists(Country country, Date? mesurement)
        {
            var test = mesurement ?? Date.Today;

            if (TestMode)
            {
                if (Currency.Empty == country.GetCurrency(test))
                {
                    Console.WriteLine("{0} ({0:f}) on {1:yyyy-MM-dd}", country, test);
                }
            }
            else
            {
                Assert.AreNotEqual(Currency.Empty, country.GetCurrency(test), "{0} ({0:f}) on {1:yyyy-MM-dd}", country, test);
            }
        }
    }
}
