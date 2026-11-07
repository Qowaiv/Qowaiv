using Qowaiv.Financial;
using Qowaiv.Humanizer;
using Qowaiv.TestTools.Conversion;

namespace TestTools.Conversion.TUnitTypeConversions_specs;

public class Scans
{
    [Test]
    public void Assemblies()
    {
        var conversions = TUnitTypeConversions.Scan(GetType().Assembly);
        conversions.Should().Contain(new TypeConversion(typeof(string), typeof(CultureInfo)))
            .And.Contain(new TypeConversion(typeof(string), typeof(Amount)));
    }

    /// <summary>Test exists to ensure that <see cref="Assemblies()"/> has a useful assert.</summary>
    [Test]
    [Arguments("nl")]
    public void Converts(CultureInfo culture)
        => culture.Name.Should().Be("nl");

    /// <summary>Test exists to ensure that <see cref="Assemblies()"/> has a useful assert.</summary>
    [Test]
    [Arguments("23.12")]
    public void Converts(Amount amount)
        => amount.Should().BeEquivalentTo(23.12m.Amount());
}
