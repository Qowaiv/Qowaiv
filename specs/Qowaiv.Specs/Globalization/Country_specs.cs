namespace Globalization.Country_specs;

public class Dipslay_name
{
    [Test]
    public void string_empty_for_empty_country()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Country.Empty.DisplayName.Should().Be(string.Empty);
        }
    }

    [Test]
    public void Unknown_for_unknown_country()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Country.Unknown.DisplayName.Should().Be("Unknown");
        }
    }

    [Test]
    public void Culture_dependent()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Svo.Country.DisplayName.Should().Be("Holy See");
        }
    }

    [Test]
    public void with_fallback_to_current_culture()
    {
        using (TestCultures.Es_EC.Scoped())
        {
            Svo.Country.GetDisplayName(null).Should().Be("Ciudad Del Vaticano");
        }
    }
}

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
            Converting.FromNull<string>().To<Country>().Should().Be(default);
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
#if NET6_0_OR_GREATER
    [TestCase("Netherlands", "NL")]
    [TestCase("nl", "NL")]
    [TestCase(4.00, "AF")]
    [TestCase(100L, "BG")]
    [TestCase(null, null)]
    [TestCase("?", "?")]
    public void System_Text_JSON_deserialization(object json, Country svo)
        => JsonTester.Read_System_Text_JSON<Country>(json).Should().Be(svo);

    [TestCase(null, null)]
    [TestCase("NL", "NL")]
    public void System_Text_JSON_serialization(Country svo, object json)
    => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [TestCase("Netherlands", "NL")]
    [TestCase("nl", "NL")]
    [TestCase(100L, "BG")]
    [TestCase("?", "?")]
    public void convention_based_deserialization(object json, Country svo)
        => JsonTester.Read<Country>(json).Should().Be(svo);

    [TestCase(null, null)]
    [TestCase("NL", "NL")]
    public void convention_based_serialization(Country svo, object json)
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

public class Supports_binary_serialization
{
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void using_BinaryFormatter()
    {
        var round_tripped = SerializeDeserialize.Binary(Svo.Country);
        Svo.Country.Should().Be(round_tripped);
    }

    [Test]
    public void storing_value_in_SerializationInfo()
    {
        var info = Serialize.GetInfo(Svo.Country);
        info.GetString("Value").Should().Be("VA");
    }
}
