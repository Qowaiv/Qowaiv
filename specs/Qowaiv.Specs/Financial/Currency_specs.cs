namespace Financial.Currency_specs;

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(Currency).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.FromNull<string>().To<Currency>().Should().Be(Currency.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From(string.Empty).To<Currency>().Should().Be(Currency.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From("EUR").To<Currency>().Should().Be(Svo.Currency);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.ToString().From(Svo.Currency).Should().Be("EUR");
        }
    }
}

public class Supports_JSON_serialization
{
#if NET6_0_OR_GREATER
    [TestCase(null, null)]
    [TestCase(978L, "EUR")]
    [TestCase(978d, "EUR")]
    [TestCase("EUR", "EUR")]
    public void System_Text_JSON_deserialization(object json, Currency svo)
        => JsonTester.Read_System_Text_JSON<Currency>(json).Should().Be(svo);

    [TestCase(null, null)]
    [TestCase("EUR", "EUR")]
    public void System_Text_JSON_serialization(Currency svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [TestCase(978L, "EUR")]
    [TestCase("EUR", "EUR")]
    public void convention_based_deserialization(object json, Currency svo)
       => JsonTester.Read<Currency>(json).Should().Be(svo);

    [TestCase(null, null)]
    [TestCase("EUR", "EUR")]
    public void convention_based_serialization(Currency svo, object json)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("2017-06-11", typeof(FormatException))]
    [TestCase(true, typeof(InvalidOperationException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
    {
        var exception = Assert.Catch(() => JsonTester.Read<Currency>(json));
        Assert.IsInstanceOf(exceptionType, exception);
    }
}
