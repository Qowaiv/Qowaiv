namespace Financial.BIC_specs;

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(BusinessIdentifierCode).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From<string>(null).To<BusinessIdentifierCode>().Should().Be(default);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From(string.Empty).To<BusinessIdentifierCode>().Should().Be(default);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From("AEGONL2UXXX").To<BusinessIdentifierCode>().Should().Be(Svo.BusinessIdentifierCode);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.ToString().From(Svo.BusinessIdentifierCode).Should().Be("AEGONL2UXXX");
        }
    }
}
