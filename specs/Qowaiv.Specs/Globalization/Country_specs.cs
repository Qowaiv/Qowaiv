namespace Globalization.Country_specs;

public class Display_name
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

public class Start_date
{
    [Test]
    public void min_value_for_empty() => Country.Empty.StartDate.Should().Be(Date.MinValue);

    [Test]
    public void min_value_for_unknown() => Country.Unknown.StartDate.Should().Be(Date.MinValue);

    [Test]
    public void set_for_active_from_start() => Svo.Country.StartDate.Should().Be(new Date(1974, 01, 01));

    [Test]
    public void set_for_counties_established_after_ISO() => Country.CZ.StartDate.Should().Be(new Date(1993, 01, 01));
}

public class End_date
{
    [Test]
    public void null_for_empty() => Country.Empty.EndDate.Should().BeNull();

    [Test]
    public void null_for_unknown() => Country.Unknown.EndDate.Should().BeNull();

    [Test]
    public void null_for_active() => Svo.Country.EndDate.Should().BeNull();

    [Test]
    public void set_for_inactive() => Country.CSHH.EndDate.Should().Be(new Date(1992, 12, 31));
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
