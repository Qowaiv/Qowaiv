namespace Globalization.Countries_specs;

public class All
{
    private static readonly Country[] all = [.. Country.All];

    [TestCaseSource(nameof(all))]
    public void Has_constant(Country country)
        => typeof(Country).GetField(country.Name, BindingFlags.Static|BindingFlags.Public)
        .Should().BeEquivalentTo(new
        {
            FieldType = typeof(Country),
        });

    [Test]
    public void _281_Countries()
        => Country.All.Should().HaveCount(281);

    [Test]
    public void _250_at_January_2016()
        => Country.GetExisting(new Date(2016, 01, 01)).Should().HaveCount(250);

    [Test]
    public void zero_at_1973()
        => Country.GetExisting(new Date(1973, 12, 31)).Should().BeEmpty(because: "before the ISO standard was introduced.");

#if NET6_0_OR_GREATER
    [Test]
    public void get_existing_by_DateOnly()
        => Country.GetExisting(new DateOnly(2016, 01, 01)).Should().HaveCount(250);
#endif
}
