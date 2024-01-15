namespace Globalization.Country_to_currency_specs;

internal class Get_Currency
{
    private static readonly Country[] WithoutCurrency = [Country.AQ];
    private static IEnumerable<Country> WithCurrency => Country.GetExisting().Except(WithoutCurrency);

    [TestCaseSource(nameof(WithCurrency))]
    public void Exists_on_start_date(Country country)
        => country.GetCurrency(country.StartDate)
            .Should().NotBe(Currency.Empty, because: "{0} ({0:f}) on {1:yyyy-MM-dd}", country, country.StartDate);

    [TestCaseSource(nameof(WithCurrency))]
    public void Exist_on_end_date(Country country)
    {
        var reference = country.EndDate ?? Clock.Today();
        country.GetCurrency(reference)
            .Should().NotBe(Currency.Empty, because: "{0} ({0:f}) on {1:yyyy-MM-dd}", country, reference);
    }

    [Test]
    public void based_on_date()
        => Country.NL.GetCurrency(Svo.Date).Should().Be(Currency.EUR);

#if NET6_0_OR_GREATER
    [Test]
    public void based_on_date_only()
        => Country.NL.GetCurrency(Svo.DateOnly).Should().Be(Currency.EUR);
#endif
}
