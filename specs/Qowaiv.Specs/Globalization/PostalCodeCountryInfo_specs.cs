namespace Globalization.PostalCodeCountryInfo_specs;

public class GetInstance
{
    [TestCase("?")]
    [TestCase("ZRCD")]
    public void Returns_default_instance_for_unsupported(Country country)
        => PostalCodeCountryInfo.GetInstance(country)
        .HasPostalCode.Should().BeFalse();
}
