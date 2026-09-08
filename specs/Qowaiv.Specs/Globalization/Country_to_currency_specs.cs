namespace Globalization.Country_to_currency_specs;

internal class Get_Currency
{
    private static readonly Country[] WithoutCurrency = [Country.AQ];
    private static IEnumerable<Country> WithCurrency => Country.GetExisting().Except(WithoutCurrency);

    [TestCaseSource(nameof(WithCurrency))]
    public void Exists_on_start_Date(Country country)
        => country.GetCurrency(country.StartDate)
            .Should().NotBe(Currency.Empty, because: "{0} ({0:f}) on {1:yyyy-MM-dd}", country, country.StartDate);

    [TestCaseSource(nameof(WithCurrency))]
    public void Exist_on_end_Date(Country country)
    {
        var reference = country.EndDate ?? Clock.Today();
        country.GetCurrency(reference)
            .Should().NotBe(Currency.Empty, because: "{0} ({0:f}) on {1:yyyy-MM-dd}", country, reference);
    }

    [Test]
    public void based_on_Date()
        => Country.NL.GetCurrency(Svo.Date).Should().Be(Currency.EUR);

    [Test]
    public void empty_for_former_country_on_start_date()
        => Country.BF.GetCurrency(new Date(1980, 01, 01)).Should().Be(Currency.Empty);

    [Test]
    public void NLG_for_NL_on_the_last_day_of_its_validity()
        => Country.NL.GetCurrency(new Date(2001, 12, 31)).Should().Be(Currency.NLG);

#if NET8_0_OR_GREATER
    [Test]
    public void based_on_DateOnly()
        => Country.NL.GetCurrency(Svo.DateOnly).Should().Be(Currency.EUR);
#endif
}
