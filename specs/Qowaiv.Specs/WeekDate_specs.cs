namespace Week_Date_specs;

public class Is_invalid
{
    [Test]
    public void for_empty_string() => WeekDate.TryParse(string.Empty).Should().BeNull();

    [Test]
    public void for_null() => WeekDate.TryParse(Nil.String).Should().BeNull();

    [Test]
    public void for_garbage() => WeekDate.TryParse("Not a week date").Should().BeNull();
}

public class Can_be_parsed
{
    [Test]
    public void from_week_date_string()
        => WeekDate.Parse("1234-W50-6", CultureInfo.InvariantCulture).Should().Be(new WeekDate(1234, 50, 6));

    [TestCase("en-GB", "2017-W23-7")]
    [TestCase("nl-NL", "2017-W23-7")]
    public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
    {
        using (culture.Scoped())
        {
            WeekDate.Parse(input).Should().Be(Svo.WeekDate);
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_throws_on_Parse()
    {
        using (TestCultures.en_GB.Scoped())
        {
            "invalid input".Invoking(WeekDate.Parse)
                .Should().Throw<FormatException>()
                .WithMessage("Not a valid week date");
        }
    }

    [TestCase("0000-W21-7")]
    [TestCase("2000-W53-7")]
    public void from_valid_input_only_otherwise_return_false_on_TryParse(string input)
        => WeekDate.TryParse(input, out _).Should().BeFalse();

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => WeekDate.TryParse("invalid input").Should().BeNull();

    [Test]
    public void with_TryParse_returns_SVO()
        => WeekDate.TryParse("2017-W23-7").Should().Be(Svo.WeekDate);
}

public class Can_be_created
{
    [Test]
    public void from_Date()
        => WeekDate.Create(Svo.Date).Should().Be(Svo.WeekDate);

#if NET8_0_OR_GREATER
    [Test]
    public void from_DateOnly()
        => WeekDate.Create(Svo.DateOnly).Should().Be(Svo.WeekDate);
#endif
}
public class Can_not_be_created
{
    [TestCase(0)]
    [TestCase(10000)]
    public void for_years_below_1_or_above_9999(int year)
    {
        Func<WeekDate> create = () => new WeekDate(year, 10, 4);
        create.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("Year should be in range [1,9999].*");
    }

    [TestCase(0)]
    [TestCase(54)]
    public void for_weeks_below_1_or_above_53(int week)
    {
        Func<WeekDate> create = () => new WeekDate(1980, week, 4);
        create.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("Week should be in range [1,53].*");
    }

    [TestCase(0)]
    [TestCase(8)]
    public void for_days_below_1_or_above_7(int day)
    {
        Func<WeekDate> create = () => new WeekDate(1980, 10, day);
        create.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("Day should be in range [1,7].*");
    }

    [Test]
    public void for_Date_above_WeekDate_max()
    {
        Func<WeekDate> create = () => new WeekDate(9999, 52, 6);
        create.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("Year, Week, and Day parameters describe an un-representable Date.");
    }

    [Test]
    public void for_Date_above_WeekDate_max_with_invalid_week_number()
    {
        Func<WeekDate> create = () => new WeekDate(9999, 53, 6);
        create.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("Year, Week, and Day parameters describe an un-representable Date.");
    }
}

public class Has_custom_formatting
{
    [Test]
    public void _default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.WeekDate.ToString().Should().Be("2017-W23-7");
        }
    }

    [Test]
    public void with_null_format_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.WeekDate.ToString(default(string)).Should().Be(Svo.WeekDate.ToString());
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.WeekDate.ToString(string.Empty).Should().Be(Svo.WeekDate.ToString());
        }
    }

    [Test]
    public void default_value_is_represented_as_0001_W01_1()
    {
        using (TestCultures.en_GB.Scoped())
        {
            default(WeekDate).ToString().Should().Be("0001-W01-1");
        }
    }

    [Test]
    public void with_empty_format_provider()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Svo.WeekDate.ToString(FormatProvider.Empty).Should().Be("2017-W23-7");
        }
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = new WeekDate(1997, 14, 6).ToString("y#W", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: '1997#14', format: 'y#W'");
    }

    [TestCase("en-US", @"y-\WW-d", 1997, 14, 6, "1997-W14-6")]
    [TestCase("en-GB", "", 1997, 14, 6, "1997-W14-6")]
    [TestCase("en-GB", @"y-\WW-d", 1979, 3, 5, "1979-W3-5")]
    [TestCase("en-GB", @"y-\Ww-d", 1979, 3, 5, "1979-W03-5")]
    public void culture_dependent(CultureInfo culture, string format, int year, int week, int day, string formatted)
    {
        using (culture.Scoped())
        {
            new WeekDate(year, week, day).ToString(format).Should().Be(formatted);
        }
    }

    [Test]
    public void with_current_thread_culture_as_default()
    {
        using (new CultureInfoScope(culture: TestCultures.nl_NL, cultureUI: TestCultures.en_GB))
        {
            Svo.WeekDate.ToString(provider: null).Should().Be("2017-W23-7");
        }
    }
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.WeekDate.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_WeekDate_as_object()
    {
        object obj = Svo.WeekDate;
        Svo.WeekDate.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_WeekDate_only()
        => new object().Invoking(Svo.WeekDate.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new[]
        {
            WeekDate.MinValue,
            WeekDate.MinValue,
            WeekDate.Parse("2000-W01-3"),
            WeekDate.Parse("2000-W11-2"),
            WeekDate.Parse("2000-W21-1"),
            WeekDate.Parse("2000-W31-7"),
        };

        var list = new List<WeekDate> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();

        list.Should().BeEquivalentTo(sorted);
    }

    [Test]
    public void by_operators_for_different_values()
    {
        WeekDate smaller = new(1980, 17, 5);
        WeekDate bigger = new(1980, 19, 5);

        (smaller < bigger).Should().BeTrue();
        (smaller <= bigger).Should().BeTrue();
        (smaller > bigger).Should().BeFalse();
        (smaller >= bigger).Should().BeFalse();
    }

    [Test]
    public void by_operators_for_equal_values()
    {
        WeekDate left = new(1980, 17, 5);
        WeekDate right = new(1980, 17, 5);

        (left < right).Should().BeFalse();
        (left <= right).Should().BeTrue();
        (left > right).Should().BeFalse();
        (left >= right).Should().BeTrue();
    }
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(WeekDate).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<WeekDate>().Should().Be(default);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("2017-W23-7").To<WeekDate>().Should().Be(Svo.WeekDate);
        }
    }

    [Test]
    public void from_DateTime()
        => Converting.From(Svo.DateTime).To<WeekDate>().Should().Be(Svo.WeekDate);

#if NET8_0_OR_GREATER
    [Test]
    public void from_DateOnly()
        => Converting.From(Svo.DateOnly).To<WeekDate>().Should().Be(Svo.WeekDate);
#endif

    [Test]
    public void from_Date()
        => Converting.From(new Date(2017, 06, 11)).To<WeekDate>().Should().Be(Svo.WeekDate);

    [Test]
    public void from_DateTimeOffset()
        => Converting.From(new DateTimeOffset(2017, 06, 11, 00, 00, 00, TimeSpan.Zero)).To<WeekDate>().Should().Be(Svo.WeekDate);

    [Test]
    public void from_LocalDateTime()
        => Converting.From(new LocalDateTime(2017, 06, 11)).To<WeekDate>().Should().Be(Svo.WeekDate);

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.WeekDate).Should().Be("2017-W23-7");
        }
    }

#if NET8_0_OR_GREATER
    [Test]
    public void to_DateOnly()
       => Converting.To<DateOnly>().From(Svo.WeekDate).Should().Be(Svo.DateOnly);
#endif

    [Test]
    public void to_Date()
        => Converting.To<Date>().From(Svo.WeekDate).Should().Be(Svo.Date);

    [Test]
    public void to_DateTime()
        => Converting.To<DateTime>().From(Svo.WeekDate).Should().Be(new DateTime(2017, 06, 11, 00, 00, 000, DateTimeKind.Local));

    [Test]
    public void to_DateTimeOffset()
        => Converting.To<DateTimeOffset>().From(Svo.WeekDate).Should().Be(new DateTimeOffset(2017, 06, 11, 00, 00, 00, TimeSpan.Zero));

    [Test]
    public void to_LocalDateTime()
        => Converting.To<LocalDateTime>().From(Svo.WeekDate).Should().Be(new LocalDateTime(2017, 06, 11));
}

public class Supports_JSON_serialization
{
#if NET8_0_OR_GREATER
    [TestCase("1997-W14-6", "1997-W14-6")]
    public void System_Text_JSON_deserialization(object json, WeekDate svo)
        => JsonTester.Read_System_Text_JSON<WeekDate>(json).Should().Be(svo);

    [TestCase("1997-W14-6", "1997-W14-6")]
    public void System_Text_JSON_serialization(WeekDate svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif

    [TestCase("1997-W14-6", "1997-W14-6")]
    public void convention_based_deserialization(object json, WeekDate svo)
        => JsonTester.Read<WeekDate>(json).Should().Be(svo);

    [TestCase("1997-W14-6", "1997-W14-6")]
    public void convention_based_serialization(WeekDate svo, object json)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("yyyy-06-11", typeof(FormatException))]
    [TestCase(true, typeof(InvalidOperationException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
        => json
            .Invoking(JsonTester.Read<WeekDate>)
            .Should().Throw<Exception>()
            .And.Should().BeOfType(exceptionType);
}
public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
       => OpenApiDataType.FromType(typeof(WeekDate))
       .Should().Be(new OpenApiDataType(
           dataType: typeof(WeekDate),
           description: "Full-date notation as defined by ISO 8601.",
           example: "1997-W14-6",
           type: "string",
           format: "date-weekbased"));
}

public class Supports_XML_serialization
{
    [Test]
    public void using_XmlSerializer_to_serialize()
    {
        var xml = Serialize.Xml(Svo.WeekDate);
        xml.Should().Be("2017-W23-7");
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<WeekDate>("2017-W23-7");
        svo.Should().Be(Svo.WeekDate);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.WeekDate);
        Svo.WeekDate.Should().Be(round_tripped);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.WeekDate);
        var round_tripped = SerializeDeserialize.Xml(structure);
        structure.Should().Be(round_tripped);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.WeekDate;
        obj.GetSchema().Should().BeNull();
    }
}

#if NET8_0_OR_GREATER
public class Casts
{
    [Test]
    public void implicitly_from_DateOnly()
    {
        WeekDate casted = Svo.DateOnly;
        casted.Should().Be(Svo.WeekDate);
    }

    [Test]
    public void explicitly_from_DateTime()
    {
        var casted = (WeekDate)new DateTime(2017, 06, 11, 0, 0, 0, DateTimeKind.Local);
        casted.Should().Be(Svo.WeekDate);
    }

    [Test]
    public void implicitly_to_DateTime()
    {
        DateTime casted = Svo.WeekDate;
        casted.Should().Be(new DateTime(2017, 06, 11, 0, 0, 0, DateTimeKind.Local));
    }
}
#endif

public class Can_be_deconstructed
{
    [Test]
    public void in_year_week_and_day_part()
    {
        var (year, week, day) = Svo.WeekDate;
        year.Should().Be(2017);
        week.Should().Be(23);
        day.Should().Be(7);
    }
}
