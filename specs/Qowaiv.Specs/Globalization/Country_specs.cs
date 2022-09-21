namespace Globalization.Country_specs;

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(Country).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From<string>(null).To<Country>().Should().Be(default);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From(string.Empty).To<Country>().Should().Be(default);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From("VA").To<Country>().Should().Be(Svo.Country);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.ToString().From(Svo.Country).Should().Be("VA");
        }
    }
}

public class Supports_JSON_serialization
{
    [TestCase("NL", "Netherlands")]
    [TestCase("NL", "nl")]
    [TestCase("AF", 4L)]
    [TestCase("BG", 100L)]
    public void System_Text_JSON_deserialization(Country svo, object json)
        => JsonTester.Read_System_Text_JSON<Country>(json).Should().Be(svo);

    [TestCase("NL", "NL")]
    public void System_Text_JSON_serialization(object json, Country svo)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);

    [TestCase("NL", "Netherlands")]
    [TestCase("NL", "nl")]
    [TestCase("AF", 4L)]
    [TestCase("BG", 100L)]
    public void convention_based_deserialization(Country svo, object json)
        => JsonTester.Read<Country>(json).Should().Be(svo);

    [TestCase("NL", "NL")]
    public void convention_based_serialization(object json, Country svo)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("2017-06-11", typeof(FormatException))]
    [TestCase(true, typeof(InvalidOperationException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
    {
        var exception = Assert.Catch(() => JsonTester.Read<Country>(json));
        Assert.IsInstanceOf(exceptionType, exception);
    }
}
