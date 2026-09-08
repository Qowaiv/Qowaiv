using AwesomeAssertions.Extensions;

namespace Date_span_specs;

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.DateSpan.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_DateSpan_as_object()
    {
        object obj = Svo.DateSpan;
        Svo.DateSpan.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_DateSpan_only()
        => new object().Invoking(Svo.DateSpan.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new[]
        {
            new DateSpan(0, 0, -1),
            DateSpan.Zero,
            DateSpan.Zero,
            new DateSpan(1, 2, 0),
            new DateSpan(0, 0, 500),
            new DateSpan(4, 0, -40),
        };

        var list = new List<DateSpan> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();

        list.Should().BeEquivalentTo(sorted);
    }

    [Test]
    public void by_operators_for_different_values()
    {
        DateSpan smaller = new(10, 3, -5);
        DateSpan bigger = new(10, 3, 2);

        (smaller < bigger).Should().BeTrue();
        (smaller <= bigger).Should().BeTrue();
        (smaller > bigger).Should().BeFalse();
        (smaller >= bigger).Should().BeFalse();
    }

    [Test]
    public void by_operators_for_equal_values()
    {
        DateSpan left = new(10, 3, -5);
        DateSpan right = new(10, 3, -5);

        (left < right).Should().BeFalse();
        (left <= right).Should().BeTrue();
        (left > right).Should().BeFalse();
        (left >= right).Should().BeTrue();
    }
}

public class Can_be_parsed
{
    [Test]
    public void from_date_span_string()
        => DateSpan.Parse("5Y+3M+2D", CultureInfo.InvariantCulture).Should().Be(new DateSpan(5, 3, 2));

    [TestCase("en-GB", "10Y+3M-5D")]
    [TestCase("es-EC", "10Y+3M-5D")]
    public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
    {
        using (culture.Scoped())
        {
            DateSpan.Parse(input).Should().Be(Svo.DateSpan);
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_throws_on_Parse()
    {
        using (TestCultures.en_GB.Scoped())
        {
            "invalid input".Invoking(DateSpan.Parse)
                .Should().Throw<FormatException>()
                .WithMessage("Not a valid date span");
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_return_false_on_TryParse()
        => DateSpan.TryParse("invalid input", out _).Should().BeFalse();

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => DateSpan.TryParse("invalid input").Should().BeNull();

    [Test]
    public void with_TryParse_returns_SVO()
    {
        using (TestCultures.en_GB.Scoped())
        {
            DateSpan.TryParse(Svo.DateSpan.ToString()).Should().Be(Svo.DateSpan);
        }
    }
}

public class Has_custom_formatting
{
    [Test]
    public void _default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.DateSpan.ToString().Should().Be("10Y+3M-5D");
        }
    }

    [Test]
    public void with_null_format_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.DateSpan.ToString(default(string)).Should().Be(Svo.DateSpan.ToString());
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.DateSpan.ToString(string.Empty).Should().Be(Svo.DateSpan.ToString());
        }
    }

    [Test]
    public void default_value_is_represented_as_0Y0M0D()
    {
        using (TestCultures.en_GB.Scoped())
        {
            default(DateSpan).ToString().Should().Be("0Y+0M+0D");
        }
    }

    [Test]
    public void with_empty_format_provider()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Svo.DateSpan.ToString(FormatProvider.Empty).Should().Be("10Y+3M-5D");
        }
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.DateSpan.ToString("Unit Test Format", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: '10Y+3M-5D', format: 'Unit Test Format'");
    }

    [TestCase("0Y+0M+0D", 0, 0)]
    [TestCase("0Y+0M+1D", 0, 1)]
    [TestCase("0Y+1M+0D", 1, 0)]
    [TestCase("1Y+0M+0D", 12, 0)]
    [TestCase("1Y+0M+1D", 12, 1)]
    [TestCase("1Y+1M+1D", 13, 1)]
    [TestCase("0Y+0M-11D", 0, -11)]
    [TestCase("0Y+1M-12D", +1, -12)]
    [TestCase("0Y-1M-12D", -1, -12)]
    [TestCase("-1Y-1M+1D", -13, 1)]
    public void format_dependent(string formatted, int months, int days)
    {
        using (CultureInfoScope.NewInvariant())
        {
            new DateSpan(months, days).ToString().Should().Be(formatted);
        }
    }

    [Test]
    public void with_current_thread_culture_as_default()
    {
        using (new CultureInfoScope(culture: TestCultures.nl_NL, cultureUI: TestCultures.en_GB))
        {
            Svo.DateSpan.ToString(provider: null).Should().Be("10Y+3M-5D");
        }
    }
}

public class Is_valid
{
    [TestCase("23Y+0M+0D", "Without starting sign")]
    [TestCase("+9998Y+0M+0D", "A lot of years")]
    [TestCase("-9998Y+0M+0D", "A lot of years")]
    [TestCase("0Y+100000M+1D", "A lot of months")]
    [TestCase("0Y-100000M+1D", "A lot of months")]
    [TestCase("0Y+0M+3650000D", "A lot of days")]
    [TestCase("0Y+0M-3650000D", "A lot of days")]
    public void For(string str, string because)
        => DateSpan.TryParse(str).Should().NotBeNull(because);
}

public class Is_invalid
{
    [Test]
    public void for_empty_string() => DateSpan.TryParse(string.Empty).Should().BeNull();

    [Test]
    public void for_null() => DateSpan.TryParse(Nil.String).Should().BeNull();

    [TestCase("+9999Y+0M+0D", "Years out of reach")]
    [TestCase("-9999Y+0M+0D", "Years out of reach")]
    [TestCase("0Y+0M+4650000D", "Days out of reach")]
    [TestCase("0Y+0M-4650000D", "Days out of reach")]
    [TestCase("Not a date span", "Garbage")]
    public void For(string str, string because)
        => DateSpan.TryParse(str).Should().BeNull(because);
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(DateSpan).Should().HaveTypeConverterDefined();

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("10Y+3M-5D").To<DateSpan>().Should().Be(Svo.DateSpan);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.DateSpan).Should().Be("10Y+3M-5D");
        }
    }
}

public class Supports_JSON_serialization
{
#if NET8_0_OR_GREATER
    [TestCase(null, "0Y+0M+0D")]
    [TestCase(0L, "0Y+0M+0D")]
    [TestCase("0Y+0M+0D", "0Y+0M+0D")]
    [TestCase("1Y+8M+3D", "1Y+8M+3D")]
    public void System_Text_JSON_deserialization(object json, DateSpan svo)
        => JsonTester.Read_System_Text_JSON<DateSpan>(json).Should().Be(svo);

    [TestCase("1Y+8M+3D", "1Y+8M+3D")]
    public void System_Text_JSON_serialization(DateSpan svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [TestCase(0L, "0Y+0M+0D")]
    [TestCase("0Y+0M+0D", "0Y+0M+0D")]
    [TestCase("1Y+8M+3D", "1Y+8M+3D")]
    public void convention_based_deserialization(object json, DateSpan svo)
        => JsonTester.Read<DateSpan>(json).Should().Be(svo);

    [TestCase("1Y+8M+3D", "1Y+8M+3D")]
    public void convention_based_serialization(DateSpan svo, object json)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("2017-06-11", typeof(FormatException))]
    [TestCase(true, typeof(InvalidOperationException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
        => json
            .Invoking(JsonTester.Read<DateSpan>)
            .Should().Throw<Exception>()
            .And.Should().BeOfType(exceptionType);
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
       => OpenApiDataType.FromType(typeof(DateSpan))
       .Should().Be(new OpenApiDataType(
           dataType: typeof(DateSpan),
           description: "Date span, specified in years, months and days.",
           example: "1Y+10M+16D",
           type: "string",
           format: "date-span",
           pattern: @"[+-]?[0-9]+Y[+-][0-9]+M[+-][0-9]+D"));
}

public class Supports_XML_serialization
{
    [Test]
    public void using_XmlSerializer_to_serialize()
    {
        var xml = Serialize.Xml(Svo.DateSpan);
        xml.Should().Be("10Y+3M-5D");
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<DateSpan>("10Y+3M-5D");
        svo.Should().Be(Svo.DateSpan);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.DateSpan);
        Svo.DateSpan.Should().Be(round_tripped);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.DateSpan);
        var round_tripped = SerializeDeserialize.Xml(structure);
        structure.Should().Be(round_tripped);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.DateSpan;
        obj.GetSchema().Should().BeNull();
    }
}

public class Can_be_operated
{
    [Test]
    public void negate_negates_values()
    {
        var negated = -Svo.DateSpan;
        negated.Should().Be(new DateSpan(-10, -3, +5));
    }

    [Test]
    public void plus_does_nothing()
    {
        var plussed = +Svo.DateSpan;
        plussed.Should().Be(Svo.DateSpan);
    }
}

public class Can_be_added_to
{
    [Test]
    public void Date_times()
    {
        var date = 30.January(1999).At(13, 42).AsUtc();
        date.Add(DateSpan.FromMonths(1)).Should().Be(28.February(1999).At(13, 42).AsUtc());
    }
}

public class Can_create
{
    [Test]
    public void Age_form_Date_without_months()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2019, 10, 10)))
        {
            var age = DateSpan.Age(new Date(2017, 06, 11));
            age.Should().Be(new DateSpan(years: 2, months: 0, days: 121));
        }
    }

#if NET8_0_OR_GREATER

    [Test]
    public void Age_form_DateOnly_without_months()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2019, 10, 10)))
        {
            var age = DateSpan.Age(new DateOnly(2017, 06, 11));
            age.Should().Be(new DateSpan(years: 2, months: 0, days: 121));
        }
    }

    [Test]
    public void Age_for_reference_Date()
    {
        var age = DateSpan.Age(new DateOnly(2017, 06, 11), new DateOnly(2023, 11, 17));
        age.Should().Be(new DateSpan(years: 6, months: 0, days: 159));
    }

#endif
}

public class Can_add
{
    [Test]
    public void two_DateSpans()
    {
        var l = new DateSpan(12, 3, 4);
        var r = new DateSpan(-2, 2, 7);
        (l + r).Should().Be(new DateSpan(10, 5, 11));
    }

    [Test]
    public void days_to_DateSpan()
    {
        var span = new DateSpan(12, 3, 4);
        span.AddDays(17).Should().Be(new DateSpan(12, 3, 21));
    }

    [Test]
    public void months_to_DateSpan()
    {
        var span = new DateSpan(12, 3, 4);
        span.AddMonths(17).Should().Be(new DateSpan(12, 20, 4));
    }

    [Test]
    public void years_to_DateSpan()
    {
        var span = new DateSpan(12, 3, 4);
        span.AddYears(17).Should().Be(new DateSpan(29, 3, 4));
    }
}

public class Can_subtract
{
    [Test]
    public void two_DateSpans()
    {
        var l = new DateSpan(12, 3, 4);
        var r = new DateSpan(-2, 2, 7);
        (l - r).Should().Be(new DateSpan(14, 1, -3));
    }

    [Test]
    public void two_Dates()
        => DateSpan.Subtract(new Date(2023, 11, 17), Svo.Date)
        .Should().Be(new DateSpan(years: 6, months: 5, days: 6));

    [TestCase(+0, +364, "2018-06-10", "2017-06-11", DateSpanSettings.DaysOnly)]
    [TestCase(+0, -364, "2017-06-11", "2018-06-10", DateSpanSettings.DaysOnly)]
    [TestCase(+11, +30, "2018-06-10", "2017-06-11", DateSpanSettings.Default)]
    [TestCase(+12, -01, "2018-06-10", "2017-06-11", DateSpanSettings.MixedSigns)]
    [TestCase(+15, +14, "2018-06-10", "2017-02-27", DateSpanSettings.Default)]
    [TestCase(+15, +11, "2018-06-10", "2017-02-27", DateSpanSettings.DaysFirst)]
    [TestCase(+24, +119, "2019-10-08", "2017-06-11", DateSpanSettings.WithoutMonths)]
    [TestCase(+36, +120, "2020-10-08", "2017-06-11", DateSpanSettings.WithoutMonths)]
    [TestCase(+12, +331, "2019-05-08", "2017-06-11", DateSpanSettings.WithoutMonths)]
    [TestCase(+24, +332, "2020-05-08", "2017-06-11", DateSpanSettings.WithoutMonths)]
    [TestCase(-11, -30, "2017-06-11", "2018-06-10", DateSpanSettings.Default)]
    [TestCase(-12, +01, "2017-06-11", "2018-06-10", DateSpanSettings.MixedSigns)]
    public void two_Dates_based_on_settings(int months, int days, Date d1, Date d2, DateSpanSettings settings)
    {
        var expected = new DateSpan(0, months, days);
        DateSpan.Subtract(d1, d2, settings).Should().Be(expected);
    }

#if NET8_0_OR_GREATER
    [Test]
    public void two_DateOnlys()
        => DateSpan.Subtract(new DateOnly(2023, 11, 17), Svo.DateOnly)
        .Should().Be(new DateSpan(years: 6, months: 5, days: 6));

    [TestCase(+0, +364, "2018-06-10", "2017-06-11", DateSpanSettings.DaysOnly)]
    [TestCase(+0, -364, "2017-06-11", "2018-06-10", DateSpanSettings.DaysOnly)]
    [TestCase(+11, +30, "2018-06-10", "2017-06-11", DateSpanSettings.Default)]
    [TestCase(+12, -01, "2018-06-10", "2017-06-11", DateSpanSettings.MixedSigns)]
    [TestCase(+15, +14, "2018-06-10", "2017-02-27", DateSpanSettings.Default)]
    [TestCase(+15, +11, "2018-06-10", "2017-02-27", DateSpanSettings.DaysFirst)]
    [TestCase(+24, +119, "2019-10-08", "2017-06-11", DateSpanSettings.WithoutMonths)]
    [TestCase(+36, +120, "2020-10-08", "2017-06-11", DateSpanSettings.WithoutMonths)]
    [TestCase(+12, +331, "2019-05-08", "2017-06-11", DateSpanSettings.WithoutMonths)]
    [TestCase(+24, +332, "2020-05-08", "2017-06-11", DateSpanSettings.WithoutMonths)]
    [TestCase(-11, -30, "2017-06-11", "2018-06-10", DateSpanSettings.Default)]
    [TestCase(-12, +01, "2017-06-11", "2018-06-10", DateSpanSettings.MixedSigns)]
    public void two_DateOnlys_based_on_settings(int months, int days, Date d1, Date d2, DateSpanSettings settings)
    {
        var expected = new DateSpan(0, months, days);
        DateSpan.Subtract((DateOnly)d1, (DateOnly)d2, settings).Should().Be(expected);
    }
#endif
}

public class Throws_when
{
    [Test]
    public void Mutatation_overflows()
        => 1.Invoking(DateSpan.MaxValue.AddDays)
            .Should().Throw<OverflowException>()
            .WithMessage("DateSpan overflowed because the resulting duration is too long.");

    [Test]
    public void Ctor_arguments_are_out_of_range()
        => int.MaxValue.Invoking(n => new DateSpan(n, n))
            .Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("The specified years, months and days results in an un-representable DateSpan.");
}

public class Can_be_deconstructed
{
    [Test]
    public void in_years_months_and_days_part()
    {
        var (years, months, days) = Svo.DateSpan;
        years.Should().Be(10);
        months.Should().Be(3);
        days.Should().Be(-5);
    }
}
