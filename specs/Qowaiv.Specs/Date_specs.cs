namespace Date_specs;

public class Is_invalid
{
    [Test]
    public void for_empty_string() => Date.TryParse(string.Empty).Should().BeNull();
    [Test]
    public void for_null() => Date.TryParse(Nil.String).Should().BeNull();
}

public class Has_constant
{
    [Test]
    public void MinValue_represent_0001_01_01() => Date.MinValue.Should().Be(new Date(0001, 01, 01));

    [Test]
    public void MaxValue_represents_9999_12_13() => Date.MaxValue.Should().Be(new Date(9999, 12, 31));
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.Date.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.Date.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.Date.Equals(Date.MinValue).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.Date.Equals(new Date(2017, 06, 11)).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (new Date(2017, 06, 11) == Svo.Date).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (new Date(2017, 06, 11) == Date.MinValue).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (new Date(2017, 06, 11) != Svo.Date).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (new Date(2017, 06, 11) != Date.MinValue).Should().BeTrue();

    [TestCase("0001-01-01", 0)]
    [TestCase("2017-06-11", -489585265)]
    public void hash_code_is_value_based(Date svo, int hash)
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
    public void Date_span_with_months_first()
        => new Date(2017, 06, 11).Add(new DateSpan(2, 20)).Should().Be(new Date(2017, 08, 31));

    [Test]
    public void Date_span_with_days_first()
        => new Date(2017, 06, 11).Add(new DateSpan(2, 20), DateSpanSettings.DaysFirst).Should().Be(new Date(2017, 09, 01));

    [Test]
    public void Month_span()
        => new Date(2017, 06, 11).Add(MonthSpan.FromMonths(3)).Should().Be(new Date(2017, 09, 11));
}

public class Can_not_be_adjusted_with
{
    [TestCase(DateSpanSettings.WithoutMonths)]
    [TestCase(DateSpanSettings.DaysFirst | DateSpanSettings.MixedSigns)]
    public void Date_span_with(DateSpanSettings settings)
        => new Date(2017, 06, 11).Invoking(d => d.Add(new DateSpan(2, 20), settings))
        .Should().Throw<ArgumentOutOfRangeException>().WithMessage("Adding a date span only supports 'Default' and 'DaysFirst'.*");
}

public class Can_be_related_to
{
    [Test]
    public void matching_month()
        => new Date(2017, 06, 11).IsIn(Month.June).Should().BeTrue();

    [Test]
    public void none_matching_month()
       => new Date(2017, 06, 11).IsIn(Month.February).Should().BeFalse();

    [Test]
    public void matching_year()
        => new Date(2017, 06, 11).IsIn(2017.CE()).Should().BeTrue();

    [Test]
    public void none_matching_year()
       => new Date(2017, 06, 11).IsIn(2018.CE()).Should().BeFalse();
}

public class Can_not_be_related_to
{
    [Test]
    public void month_empty()
        => new Date(2017, 06, 11).IsIn(Month.Empty).Should().BeFalse();

    [Test]
    public void month_unknown()
       => new Date(2017, 06, 11).IsIn(Month.Unknown).Should().BeFalse();

    [Test]
    public void year_empty()
        => new Date(2017, 06, 11).IsIn(Year.Empty).Should().BeFalse();

    [Test]
    public void year_unknown()
       => new Date(2017, 06, 11).IsIn(Year.Unknown).Should().BeFalse();
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.Date.CompareTo(Nil.Object).Should().Be(1);
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(Date).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<Date>().Should().Be(default);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("2017-06-11").To<Date>().Should().Be(Svo.Date);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.Date).Should().Be("11/06/2017");
        }
    }

    [Test]
    public void from_DateTime()
        => Converting.From(new DateTime(2017, 06, 11, 00, 00, 000, DateTimeKind.Local)).To<Date>().Should().Be(Svo.Date);

    [Test]
    public void from_DateTimeOffset()
        => Converting.From(new DateTimeOffset(2017, 06, 11, 00, 00, 00, TimeSpan.Zero)).To<Date>().Should().Be(Svo.Date);

    [Test]
    public void from_LocalDateTime()
        => Converting.From(new LocalDateTime(2017, 06, 11)).To<Date>().Should().Be(Svo.Date);

    [Test]
    public void from_WeekDate()
        => Converting.From(new WeekDate(2017, 23, 7)).To<Date>().Should().Be(Svo.Date);

    [Test]
    public void to_DateTime()
        => Converting.To<DateTime>().From(Svo.Date).Should().Be(new DateTime(2017, 06, 11, 00, 00, 000, DateTimeKind.Local));

    [Test]
    public void to_DateTimeOffset()
        => Converting.To<DateTimeOffset>().From(Svo.Date).Should().Be(new DateTimeOffset(2017, 06, 11, 00, 00, 00, TimeSpan.Zero));

    [Test]
    public void to_LocalDateTime()
        => Converting.To<LocalDateTime>().From(Svo.Date).Should().Be(new LocalDateTime(2017, 06, 11));

    [Test]
    public void to_WeekDate()
        => Converting.To<WeekDate>().From(Svo.Date).Should().Be(new WeekDate(2017, 23, 7));
}

public class Supports_JSON_serialization
{
#if NET6_0_OR_GREATER
    [TestCase("2012-04-23", "2012-04-23")]
    [TestCase("2012-04-23T18:25:43.511Z", "2012-04-23")]
    [TestCase("2012-04-23T10:25:43-05:00", "2012-04-23")]
    [TestCase(636_327_360_000_000_000L, "2017-06-11")]
    public void System_Text_JSON_deserialization(object json, Date svo)
        => JsonTester.Read_System_Text_JSON<Date>(json).Should().Be(svo);

    [TestCase("2012-04-23", "2012-04-23")]
    public void System_Text_JSON_serialization(Date svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [TestCase("2012-04-23", "2012-04-23")]
    [TestCase("2012-04-23T18:25:43.511Z", "2012-04-23")]
    [TestCase("2012-04-23T10:25:43-05:00", "2012-04-23")]
    public void convention_based_deserialization(object json, Date svo)
      => JsonTester.Read<Date>(json).Should().Be(svo);
  
    [TestCase("2012-04-23", "2012-04-23")]
    public void convention_based_serialization(Date svo, object json)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("yyyy-06-11", typeof(FormatException))]
    [TestCase(true, typeof(InvalidOperationException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
        => json
            .Invoking(JsonTester.Read<Date>)
            .Should().Throw<Exception>()
            .And.Should().BeOfType(exceptionType);
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
       => OpenApiDataType.FromType(typeof(Date))
       .Should().Be(new OpenApiDataType(
           dataType: typeof(Date),
           description: "Full-date notation as defined by RFC 3339, section 5.6.",
           example: "2017-06-10",
           type: "string",
           format: "date"));
}

#if NET6_0_OR_GREATER
public class Casts
{
    [Test]
    public void explicitly_from_DateOnly()
    {
        var casted = (Date)Svo.DateOnly;
        casted.Should().Be(Svo.Date);
    }

    [Test]
    public void implicitly_to_DateOnly()
    {
        DateOnly casted = Svo.Date;
        casted.Should().Be(Svo.DateOnly);
    }
}
#endif
