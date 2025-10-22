namespace Globalization.Country_specs;

public class With_domain_logic
{
    [TestCase(true, "VA")]
    [TestCase(true, "?")]
    [TestCase(false, "")]
    public void HasValue_is(bool result, Country svo) => svo.HasValue.Should().Be(result);

    [TestCase(true, "VA")]
    [TestCase(false, "?")]
    [TestCase(false, "")]
    public void IsKnown_is(bool result, Country svo) => svo.IsKnown.Should().Be(result);

    /// <remarks>
    /// As the regions available depend on the environment running, we can't
    /// predict the outcome.
    /// </remarks>
    [TestCaseSource(typeof(Country), nameof(Country.All))]
    public void RegionInfo_exists_indicates_counterpart_exists(Country country)
        => country.Invoking(c => c.RegionInfoExists).Should().NotThrow();
}

public class Display_name
{
    [Test]
    public void string_empty_for_empty_country()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Country.Empty.DisplayName.Should().Be(string.Empty);
        }
    }

    [Test]
    public void Unknown_for_unknown_country()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Country.Unknown.DisplayName.Should().Be("Unknown");
        }
    }

    [Test]
    public void Culture_dependent()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Country.DisplayName.Should().Be("Holy See");
        }
    }

    [Test]
    public void with_fallback_to_current_culture()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Svo.Country.GetDisplayName(null).Should().Be("Ciudad Del Vaticano");
        }
    }
}

public class Start_Date
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

public class End_Date
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

public class Exists
{
    [Test]
    public void for_country_exiting_on_Date()
        => Country.CSXX.ExistsOnDate(new Date(1993, 01, 01)).Should().BeTrue();

    [Test]
    public void not_for_country_not_exiting_on_Date()
        => Country.CSXX.ExistsOnDate(new Date(1992, 12, 31)).Should().BeFalse();

#if NET8_0_OR_GREATER
    [Test]
    public void for_country_exiting_on_DateOnly()
        => Country.CSXX.ExistsOnDate(new DateOnly(1993, 01, 01)).Should().BeTrue();

    [Test]
    public void not_for_country_not_exiting_on_DateOnly()
        => Country.CSXX.ExistsOnDate(new Date(1992, 12, 31)).Should().BeFalse();
#endif
}

public class Has_custom_formatting
{
    [Test]
    public void with_null_format_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Country.ToString().Should().Be(Svo.Country.ToString(default(string)));
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Country.ToString().Should().Be(Svo.Country.ToString(string.Empty));
        }
    }

    [Test]
    public void default_value_is_represented_as_string_empty()
        => default(Country).ToString().Should().BeEmpty();

    [Test]
    public void unknown_value_is_represented_as_unknown()
        => Country.Unknown.ToString().Should().Be("?");

    [Test]
    public void with_empty_format_provider()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Svo.Country.ToString(FormatProvider.Empty).Should().Be("VA");
        }
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.Country.ToString("2", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: 'VA', format: '2'");
    }

    [TestCase("MZ", "nl", "0", "508")]
    [TestCase("MZ", "nl", "2", "MZ")]
    [TestCase("MZ", "nl", "3", "MOZ")]
    [TestCase("MZ", "ja-JP", "e", "Mozambique")]
    [TestCase("MZ", "ja-JP", "f", "モザンビーク")]
    [TestCase("CSHH", "nl", "2", "CS")]
    [TestCase("CSHH", "nl", "n", "CSHH")]
    public void format_dependent(Country svo, CultureInfo culture, string format, string formatted)
        => svo.ToString(format, culture).Should().Be(formatted);
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.Country.CompareTo(Nil.Object).Should().Be(1);
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(Country).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<Country>().Should().Be(default);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<Country>().Should().Be(default);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("VA").To<Country>().Should().Be(Svo.Country);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.Country).Should().Be("VA");
        }
    }
}

public class Supports_JSON_serialization
{
#if NET8_0_OR_GREATER
    [TestCase("Netherlands", "NL")]
    [TestCase("nl", "NL")]
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
        => json
            .Invoking(JsonTester.Read<Country>)
            .Should().Throw<Exception>()
            .And.Should().BeOfType(exceptionType);
}

#if NET8_0_OR_GREATER
#else
public class Supports_binary_serialization
{
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void using_BinaryFormatter()
    {
        var round_tripped = SerializeDeserialize.Binary(Svo.Country);
        round_tripped.Should().Be(Svo.Country);
    }

    [Test]
    public void storing_value_in_SerializationInfo()
    {
        var info = Serialize.GetInfo(Svo.Country);
        info.GetString("Value").Should().Be("VA");
    }
}
#endif

public class Is_equal_by_value
{
    [TestCase("", 0)]
    [TestCase("NL", -1174190069)]
    public void hash_code_is_value_based(Country svo, int hash)
    {
        using (Hash.WithoutRandomizer())
        {
            svo.GetHashCode().Should().Be(hash);
        }
    }
}

public class Can_parse
{
    [Test]
    public void prefering_existing_over_former_countries()
        => Country.Parse("BQ").Should().Be(Country.BQ);

    [Test]
    public void former_countries_via_ISO_3166_3()
        => Country.Parse("BQAQ").Should().Be(Country.BQAQ);

    [Test]
    public void culture_specific()
        => Country.TryParse("モザンビーク", new CultureInfo("ja-JP")).Should().Be(Country.MZ);
}
