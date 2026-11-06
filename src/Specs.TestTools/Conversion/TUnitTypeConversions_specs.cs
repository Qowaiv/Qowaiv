using Qowaiv.TestTools.Conversion;

namespace TestTools.Conversion.TUnitTypeConversions_specs;

public class Scans
{
    [Test]
    public void Assemblies()
    {
        var conversions = TUnitTypeConversions.Scan(GetType().Assembly);
        conversions.Should().Contain(new TypeConversion(typeof(string), typeof(CultureInfo)));
    }

    /// <summary>Test exists to ensure that <see cref="Assemblies()"/> has a useful assert.</summary>
    [Test]
    [Arguments("nl")]
    public void Converts(CultureInfo culture)
        => culture.Name.Should().Be("nl");
}
