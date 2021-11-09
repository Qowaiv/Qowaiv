namespace Identifiers.Id_for_Int32_specs;

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(Int32Id).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From<string>(null).To<Int32Id>().Should().Be(Int32Id.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From(string.Empty).To<Int32Id>().Should().Be(Int32Id.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From("PREFIX17").To<Int32Id>().Should().Be(Svo.Int32Id);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.ToString().From(Svo.Int32Id).Should().Be("17");
        }
    }

    [Test]
    public void from_int()
        => Converting.From(17).To<Int32Id>().Should().Be(Svo.Int32Id);

    [Test]
    public void to_int()
        => Converting.To<int>().From(Svo.Int32Id).Should().Be(17);
}

