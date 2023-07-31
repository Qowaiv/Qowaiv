namespace Qowaiv.UnitTests.Globalization;

public class CountryToCurrencyTest
{
    private static readonly Country[] CountriesWithoutCurrency = new[] { Country.AQ };
    private static IEnumerable<Country> GetCountriesWithCurrency() => Country.All.Where(c => !CountriesWithoutCurrency.Contains(c));

    [TestCaseSource(nameof(GetCountriesWithCurrency))]
    public void GetCurrency_ExistOnStartDate(Country country)
    {
        Assert.AreNotEqual(Currency.Empty, country.GetCurrency(country.StartDate), "{0} ({0:f}) on {1:yyyy-MM-dd}", country, country.StartDate);
    }

    [TestCaseSource(nameof(GetCountriesWithCurrency))]
    public void GetCurrency_ExistOnEndDate(Country country)
    {
        var test = country.EndDate ?? Clock.Today();
        Assert.AreNotEqual(Currency.Empty, country.GetCurrency(test), "{0} ({0:f}) on {1:yyyy-MM-dd}", country, test);
    }
}
