namespace Week_date_specs;

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
    public void for_date_above_week_date_max()
    {
        Func<WeekDate> create = () => new WeekDate(9999, 52, 6);
        create.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("Year, Week, and Day parameters describe an un-representable Date.");
    }

    [Test]
    public void for_date_above_week_date_max_with_invalid_week_number()
    {
        Func<WeekDate> create = () => new WeekDate(9999, 53, 6);
        create.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("Year, Week, and Day parameters describe an un-representable Date.");
    }
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.WeekDate.CompareTo(Nil.Object).Should().Be(1);
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(WeekDate).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.FromNull<string>().To<WeekDate>().Should().Be(default);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From("2017-W23-7").To<WeekDate>().Should().Be(Svo.WeekDate);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.ToString().From(Svo.WeekDate).Should().Be("2017-W23-7");
        }
    }

    [Test]
    public void from_Date()
        => Converting.From(new Date(2017, 06, 11)).To<WeekDate>().Should().Be(Svo.WeekDate);

    [Test]
    public void from_DateTime()
        => Converting.From(new DateTime(2017, 06, 11, 00, 00, 000, DateTimeKind.Local)).To<WeekDate>().Should().Be(Svo.WeekDate);

    [Test]
    public void from_DateTimeOffset()
        => Converting.From(new DateTimeOffset(2017, 06, 11, 00, 00, 00, TimeSpan.Zero)).To<WeekDate>().Should().Be(Svo.WeekDate);

    [Test]
    public void from_LocalDateTime()
        => Converting.From(new LocalDateTime(2017, 06, 11)).To<WeekDate>().Should().Be(Svo.WeekDate);

    [Test]
    public void to_Date()
        => Converting.To<Date>().From(Svo.WeekDate).Should().Be(new Date(2017, 06, 11));

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
#if NET6_0_OR_GREATER
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
    {
        var exception = Assert.Catch(() => JsonTester.Read<WeekDate>(json));
        Assert.IsInstanceOf(exceptionType, exception);
    }
}
public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
       => Qowaiv.OpenApi.OpenApiDataType.FromType(typeof(WeekDate))
       .Should().Be(new Qowaiv.OpenApi.OpenApiDataType(
           dataType: typeof(WeekDate),
           description: "Full-date notation as defined by ISO 8601.",
           example: "1997-W14-6",
           type: "string",
           format: "date-weekbased"));
}
