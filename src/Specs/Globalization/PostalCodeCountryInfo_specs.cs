namespace Globalization.PostalCodeCountryInfo_specs;

public class GetInstance
{
    [Test]
    [Arguments("?")]
    [Arguments("ZRCD")]
    public void Returns_default_instance_for_unsupported(Country country)
        => PostalCodeCountryInfo.GetInstance(country)
        .HasPostalCode.Should().BeFalse();
}
