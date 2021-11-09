namespace DateSpan_specs;

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(DateSpan).Should().HaveTypeConverterDefined();

    [Test]
    public void from_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From("10Y+3M-5D").To<DateSpan>().Should().Be(Svo.DateSpan);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.ToString().From(Svo.DateSpan).Should().Be("10Y+3M-5D");
        }
    }
}
