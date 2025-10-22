using AwesomeAssertions.Extensions;

namespace YearMonth_specs;

public class With_domain_logic
{
    [Test]
    public void with_Year_property() => Svo.YearMonth.Year.Should().Be(2017);


    [Test]
    public void with_Month_property() => Svo.YearMonth.Month.Should().Be(06);
}

public class Has_constant
{
    [Test]
    public void Min_value_represent_0001_January()
        => YearMonth.MinValue.Should().Be(new(year: 0001, month: 01));
    [Test]
    public void Max_value_represent_9999_december()
        => YearMonth.MaxValue.Should().Be(new(year: 9999, month: 12));
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.YearMonth.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.YearMonth.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.YearMonth.Equals(new YearMonth(2000, 09)).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.YearMonth.Equals(YearMonth.Parse("2017-06")).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (Svo.YearMonth == YearMonth.Parse("2017-06")).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (Svo.YearMonth == new YearMonth(2000, 09)).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (Svo.YearMonth != YearMonth.Parse("2017-06")).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (Svo.YearMonth != new YearMonth(2000, 09)).Should().BeTrue();

    [TestCase("2000-09", 665643119)]
    [TestCase("2017-06", 665643862)]
    public void hash_code_is_value_based(YearMonth svo, int hash)
    {
        using (Hash.WithoutRandomizer())
        {
            svo.GetHashCode().Should().Be(hash);
        }
    }
}
public class Can_be_adjusted_with
{
    [Test]
    public void Month_span()
        => (Svo.YearMonth + MonthSpan.FromMonths(7)).Should().Be(new YearMonth(2018, 01));

    [Test]
    public void Months()
        => Svo.YearMonth.AddMonths(7).Should().Be(new YearMonth(2018, 01));

    [Test]
    public void Years()
        => Svo.YearMonth.AddYears(7).Should().Be(new YearMonth(2024, 06));

    [Test]
    public void unit()
    {
        var date = Svo.YearMonth;
        date++;
        date.Should().Be(new YearMonth(2017, 07));
    }
}

public class Cannot_be_adjusted_with
{
    [Test]
    public void too_big_or_small_Month_span()
        => MonthSpan.FromYears(1).Invoking(YearMonth.MaxValue.Add)
        .Should().Throw<ArgumentOutOfRangeException>()
        .WithMessage("Year, and Month parameters describe an un-representable year-month.*");

    [Test]
    public void too_big_or_small_Months()
        => 17.Invoking(YearMonth.MaxValue.AddMonths)
        .Should().Throw<ArgumentOutOfRangeException>()
        .WithMessage("Year, and Month parameters describe an un-representable year-month.*");

    [Test]
    public void too_big_or_small_Years()
        => 17.Invoking(YearMonth.MaxValue.AddYears)
        .Should().Throw<ArgumentOutOfRangeException>()
        .WithMessage("Year, and Month parameters describe an un-representable year-month.*");
}

public class Can_be_subtracted_by
{
    [Test]
    public void Month_span()
        => (Svo.YearMonth - MonthSpan.FromMonths(9)).Should().Be(new YearMonth(2016, 09));

    [Test]
    public void Year_month()
        => (new YearMonth(2024, 08) - Svo.YearMonth).Should().Be(new MonthSpan(years: 7, months: 02));

    [Test]
    public void unit()
    {
        var date = Svo.YearMonth;
        date--;
        date.Should().Be(new YearMonth(2017, 05));
    }
}

public class Can_be_related_to
{
    [Test]
    public void matching_month()
        => Svo.YearMonth.IsIn(Month.June).Should().BeTrue();

    [Test]
    public void non_matching_month()
       => Svo.YearMonth.IsIn(Month.February).Should().BeFalse();

    [Test]
    public void matching_year()
        => Svo.YearMonth.IsIn(2017.CE()).Should().BeTrue();

    [Test]
    public void non_matching_year()
       => Svo.YearMonth.IsIn(2018.CE()).Should().BeFalse();
}

public class Can_not_be_related_to
{
    [Test]
    public void month_empty()
        => Svo.YearMonth.IsIn(Month.Empty).Should().BeFalse();

    [Test]
    public void month_unknown()
       => Svo.YearMonth.IsIn(Month.Unknown).Should().BeFalse();

    [Test]
    public void year_empty()
        => Svo.YearMonth.IsIn(Year.Empty).Should().BeFalse();

    [Test]
    public void year_unknown()
       => Svo.YearMonth.IsIn(Year.Unknown).Should().BeFalse();
}

public class Can_not_be_created
{
    [TestCase(0)]
    [TestCase(10000)]
    public void for_year_before_1_or_after_9999(int year)
        => year.Invoking(y => new YearMonth(y, 06))
        .Should().Throw<ArgumentOutOfRangeException>()
        .WithMessage("Year, and Month parameters describe an un-representable year-month.*")
        .And.ParamName.Should().Be("year");

    [TestCase(0)]
    [TestCase(13)]
    public void for_month_smaller_than_1_or_bigger_than_12(int month)
        => month.Invoking(m => new YearMonth(2017, m))
        .Should().Throw<ArgumentOutOfRangeException>()
        .WithMessage("Year, and Month parameters describe an un-representable year-month.*")
        .And.ParamName.Should().Be("month");
}

public class Can_not_be_parsed
{
    [Test]
    public void from_null()
        => YearMonth.TryParse(null).Should().BeNull();

    [Test]
    public void from_empty_string()
        => YearMonth.TryParse(string.Empty, CultureInfo.InvariantCulture).Should().BeNull();

    [Test]
    public void from_invalid_input()
        => YearMonth.TryParse("invalid input").Should().BeNull();
}

public class Can_be_parsed
{
    [TestCase("en", "2017-06")]
    public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
    {
        using (culture.Scoped())
        {
            YearMonth.Parse(input).Should().Be(Svo.YearMonth);
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_throws_on_Parse()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Func<YearMonth> parse = () => YearMonth.Parse("invalid input");
            parse.Should().Throw<FormatException>()
                .WithMessage("Not a valid year-month.");
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_return_false_on_TryParse()
        => (YearMonth.TryParse("invalid input", out _)).Should().BeFalse();

    [Test]
    public void with_TryParse_returns_SVO()
        => YearMonth.TryParse("2017-06").Should().Be(Svo.YearMonth);

    [Test]
    public void with_TryParse_with_culture_returns_SVO()
       => YearMonth.TryParse("2017-06", TestCultures.en_GB).Should().Be(Svo.YearMonth);
}

public class Has_custom_formatting
{
    [Test]
    public void _default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.YearMonth.ToString().Should().Be("2017-06");
        }
    }

    [Test]
    public void with_null_format_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.YearMonth.ToString().Should().Be(Svo.YearMonth.ToString(default(string)));
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.YearMonth.ToString().Should().Be(Svo.YearMonth.ToString(string.Empty));
        }
    }

    [Test]
    public void with_empty_format_provider()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Svo.YearMonth.ToString(FormatProvider.Empty).Should().Be("2017-06");
        }
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.YearMonth.ToString("yyyy MMM", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: '2017 Jun', format: 'yyyy MMM'");
    }

    [TestCase("en-GB", "yyyy MMMM", "2017-06", "2017 June")]
    [TestCase("nl-BE", "MMMM yyyy", "2017-06", "juni 2017")]
    public void culture_dependent(CultureInfo culture, string format, YearMonth svo, string formatted)
    {
        using (culture.Scoped())
        {
            svo.ToString(format).Should().Be(formatted);
        }
    }

    [Test]
    public void with_current_thread_culture_as_default()
    {
        using (new CultureInfoScope(
            culture: TestCultures.nl_NL,
            cultureUI: TestCultures.en_GB))
        {
            Svo.YearMonth.ToString(provider: null).Should().Be("2017-06");
        }
    }
}

public class Is_comparable
{
    [Test]
    public void to_null() => Svo.YearMonth.CompareTo(null).Should().Be(1);

    [Test]
    public void to_YearMonth_as_object()
    {
        object obj = Svo.YearMonth;
        Svo.YearMonth.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_YearMonth_only()
        => new object().Invoking(Svo.YearMonth.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new[]
        {
            YearMonth.MinValue,
            YearMonth.MinValue,
            new YearMonth(2017, 05),
            new YearMonth(2017, 06),
            new YearMonth(2017, 07),
            YearMonth.MaxValue,
        };

        var list = new List<YearMonth> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }

    [Test]
    public void by_operators_for_different_values()
    {
        var smaller = new YearMonth(2017, 06);
        var bigger = new YearMonth(2017, 07);

        (smaller < bigger).Should().BeTrue();
        (smaller <= bigger).Should().BeTrue();
        (smaller > bigger).Should().BeFalse();
        (smaller >= bigger).Should().BeFalse();
    }

    [Test]
    public void by_operators_for_equal_values()
    {
        var left = new YearMonth(2017, 06);
        var right = new YearMonth(2017, 06);

        (left < right).Should().BeFalse();
        (left <= right).Should().BeTrue();
        (left > right).Should().BeFalse();
        (left >= right).Should().BeTrue();
    }
}

public class Casts
{
    [Test]
    public void explicitly_from_Date()
        => ((YearMonth)Svo.Date).Should().Be(Svo.YearMonth);

    [Test]
    public void explicitly_from_DateTime()
        => ((YearMonth)Svo.DateTime).Should().Be(Svo.YearMonth);

    [Test]
    public void explicitly_from_LocalDateTime()
        => ((YearMonth)Svo.LocalDateTime).Should().Be(Svo.YearMonth);

#if NET8_0_OR_GREATER
    [Test]
    public void explicitly_from_DateOnly()
        => ((YearMonth)Svo.DateOnly).Should().Be(Svo.YearMonth);
#endif

    [Test]
    public void explicitly_to_Date()
        => ((Date)Svo.YearMonth).Should().Be(new Date(2017, 06, 01));

    [Test]
    public void explicitly_to_DateTime()
        => ((DateTime)Svo.YearMonth).Should().Be(01.June(2017));

    [Test]
    public void explicitly_to_LocalDateTime()
        => ((LocalDateTime)Svo.YearMonth).Should().Be(new LocalDateTime(2017, 06, 01));

#if NET8_0_OR_GREATER
    [Test]
    public void explicitly_to_DateOnly()
        => ((DateOnly)Svo.YearMonth).Should().Be(new DateOnly(2017, 06, 01));
#endif
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(YearMonth).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<YearMonth>().Should().Be(default);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("2017-06").To<YearMonth>().Should().Be(Svo.YearMonth);
        }
    }

    [Test]
    public void from_DateTime()
        => Converting.From(Svo.DateTime).To<YearMonth>().Should().Be(Svo.YearMonth);

#if NET8_0_OR_GREATER
    [Test]
    public void from_DateOnly()
        => Converting.From(Svo.DateOnly).To<YearMonth>().Should().Be(Svo.YearMonth);
#endif

    [Test]
    public void from_DateTimeOffset()
        => Converting.From(Svo.DateTimeOffset).To<YearMonth>().Should().Be(Svo.YearMonth);

    [Test]
    public void from_LocalDateTime()
        => Converting.From(Svo.LocalDateTime).To<YearMonth>().Should().Be(Svo.YearMonth);

    [Test]
    public void from_Date()
        => Converting.From(Svo.Date).To<YearMonth>().Should().Be(Svo.YearMonth);

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.YearMonth).Should().Be("2017-06");
        }
    }

    [Test]
    public void to_DateTime()
        => Converting.To<DateTime>().From(Svo.YearMonth).Should().Be(01.June(2017));


#if NET8_0_OR_GREATER
    [Test]
    public void to_DateOnly()
        => Converting.To<DateOnly>().From(Svo.YearMonth).Should().Be(new DateOnly(2017, 06, 01));
#endif

    [Test]
    public void to_DateTimeOffset()
        => Converting.To<DateTimeOffset>().From(Svo.YearMonth).Should().Be(01.June(2017).WithOffset(TimeSpan.Zero));

    [Test]
    public void to_LocalDateTime()
        => Converting.To<LocalDateTime>().From(Svo.YearMonth).Should().Be(new LocalDateTime(2017, 06, 01));

    [Test]
    public void to_Date()
        => Converting.To<Date>().From(Svo.YearMonth).Should().Be(new Date(2017, 06, 01));
}

public class Supports_JSON_serialization
{
#if NET8_0_OR_GREATER
    [TestCase("2017-06", "2017-06")]
    public void System_Text_JSON_deserialization(object json, YearMonth svo)
        => JsonTester.Read_System_Text_JSON<YearMonth>(json).Should().Be(svo);

    [TestCase("2017-06", "2017-06")]
    public void System_Text_JSON_serialization(YearMonth svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif

    [TestCase("2017-06", "2017-06")]
    public void convention_based_deserialization(YearMonth svo, object json)
        => JsonTester.Read<YearMonth>(json).Should().Be(svo);

    [TestCase("2017-06", "2017-06")]
    public void convention_based_serialization(object json, YearMonth svo)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("2017.15", typeof(FormatException))]
    [TestCase(5L, typeof(InvalidOperationException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
    {
        Func<YearMonth> read = () => JsonTester.Read<YearMonth>(json);
        read.Should().Throw<Exception>().Subject.Single().Should().BeOfType(exceptionType);
    }
}

public class Supports_XML_serialization
{
    [Test]
    public void using_XmlSerializer_to_serialize()
    {
        var xml = Serialize.Xml(Svo.YearMonth);
        xml.Should().Be("2017-06");
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<YearMonth>("2017-06");
        svo.Should().Be(Svo.YearMonth);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.YearMonth);
        Svo.YearMonth.Should().Be(round_tripped);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.YearMonth);
        var round_tripped = SerializeDeserialize.Xml(structure);
        structure.Should().Be(round_tripped);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.YearMonth;
        obj.GetSchema().Should().BeNull();
    }
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
       => OpenApiDataType.FromType(typeof(YearMonth))
       .Should().Be(new OpenApiDataType(
           dataType: typeof(YearMonth),
           description: "Date notation with month precision.",
           example: "2017-06",
           type: "string",
           format: "year-month",
           pattern: "[0-9]{4}-(0?[1-9]|1[0-2])"));

    [TestCase("2017-6")]
    [TestCase("2017-06")]
    [TestCase("1900-10")]
    [TestCase("1900-11")]
    [TestCase("1979-12")]
    public void pattern_matches(string input)
        => OpenApiDataType.FromType(typeof(YearMonth))!.Matches(input).Should().BeTrue();

    [TestCase("1900-00")]
    [TestCase("1979-13")]
    public void pattern_does_not_match(string input)
        => OpenApiDataType.FromType(typeof(YearMonth))!.Matches(input).Should().BeFalse();
}

public class Debugger
{
    [TestCase("2017-06", "2017-06")]
    public void has_custom_display(object display, YearMonth svo)
        => svo.Should().HaveDebuggerDisplay(display);
}

public class Can_be_deconstructed
{
    [Test]
    public void in_year_and_month_part()
    {
        var (year, month) = Svo.YearMonth;
        year.Should().Be(2017);
        month.Should().Be(6);
    }
}
