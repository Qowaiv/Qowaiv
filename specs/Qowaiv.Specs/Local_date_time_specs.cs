namespace Local_DateTime_specs;

public class Has_constant
{
    [Test]
    public void MinValue_is_0001Y_01M_01D() => LocalDateTime.MinValue.Should().Be(new(0001, 01, 01));

    [Test]
    public void MaxValue_equal_to_9999Y_12M_31D() => LocalDateTime.MaxValue.Should().Be(new(DateTime.MaxValue.Ticks));
}

public class Is_invalid
{
    [Test]
    public void for_empty_string() => LocalDateTime.TryParse(string.Empty).Should().BeNull();

    [Test]
    public void for_null() => LocalDateTime.TryParse(Nil.String).Should().BeNull();

    [Test]
    public void for_garbage()
        => DateSpan.TryParse("not a date").Should().BeNull();
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.LocalDateTime.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.LocalDateTime.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.LocalDateTime.Equals(LocalDateTime.MinValue).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.LocalDateTime.Equals(new LocalDateTime(2017, 06, 11, 06, 15, 00)).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (new LocalDateTime(2017, 06, 11, 06, 15, 00) == Svo.LocalDateTime).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (new LocalDateTime(2017, 06, 11, 06, 15, 00) == LocalDateTime.MinValue).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (new LocalDateTime(2017, 06, 11, 06, 15, 00) != Svo.LocalDateTime).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (new LocalDateTime(2017, 06, 11, 06, 15, 00) != LocalDateTime.MinValue).Should().BeTrue();

    [TestCase("0001-01-01", 0)]
    [TestCase("2017-06-11 06:15", 533532482)]
    public void hash_code_is_value_based(LocalDateTime svo, int hash)
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
        => new LocalDateTime(2017, 06, 11).Add(new DateSpan(2, 20)).Should().Be(new LocalDateTime(2017, 08, 31));

    [Test]
    public void Date_span_with_days_first()
        => new DateTime(2017, 06, 11, 00, 00, 000, DateTimeKind.Local).Add(new DateSpan(2, 20), DateSpanSettings.DaysFirst).Should().Be(new LocalDateTime(2017, 09, 01));

    [Test]
    public void Month_span()
        => new LocalDateTime(2017, 06, 11).Add(MonthSpan.FromMonths(3)).Should().Be(new LocalDateTime(2017, 09, 11));
}

public class Can_not_be_adjusted_with
{
    [TestCase(DateSpanSettings.WithoutMonths)]
    [TestCase(DateSpanSettings.DaysFirst | DateSpanSettings.MixedSigns)]
    public void Date_span_with(DateSpanSettings settings)
        => new LocalDateTime(2017, 06, 11).Invoking(d => d.Add(new DateSpan(2, 20), settings))
        .Should().Throw<ArgumentOutOfRangeException>().WithMessage("Adding a date span only supports 'Default' and 'DaysFirst'.*");
}

public class Can_be_related_to
{
    [Test]
    public void matching_month()
        => new LocalDateTime(2017, 06, 11).IsIn(Month.June).Should().BeTrue();

    [Test]
    public void non_matching_month()
       => new LocalDateTime(2017, 06, 11).IsIn(Month.February).Should().BeFalse();

    [Test]
    public void matching_year()
        => new LocalDateTime(2017, 06, 11).IsIn(2017.CE()).Should().BeTrue();

    [Test]
    public void non_matching_year()
       => new LocalDateTime(2017, 06, 11).IsIn(2018.CE()).Should().BeFalse();
}

public class Can_not_be_related_to
{
    [Test]
    public void month_empty()
        => new LocalDateTime(2017, 06, 11).IsIn(Month.Empty).Should().BeFalse();

    [Test]
    public void month_unknown()
       => new LocalDateTime(2017, 06, 11).IsIn(Month.Unknown).Should().BeFalse();

    [Test]
    public void year_empty()
        => new LocalDateTime(2017, 06, 11).IsIn(Year.Empty).Should().BeFalse();

    [Test]
    public void year_unknown()
       => new LocalDateTime(2017, 06, 11).IsIn(Year.Unknown).Should().BeFalse();
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.LocalDateTime.CompareTo(Nil.Object).Should().Be(1);
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
            Converting.FromNull<string>().To<LocalDateTime>().Should().Be(default);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("2017-06-11 06:15:00").To<LocalDateTime>().Should().Be(Svo.LocalDateTime);
        }
    }

    [Test]
    public void from_DateTime()
        => Converting.From(Svo.DateTime).To<LocalDateTime>().Should().Be(Svo.LocalDateTime);

#if NET8_0_OR_GREATER
    [Test]
    public void from_DateOnly()
        => Converting.From(Svo.DateOnly).To<LocalDateTime>().Should().Be(new LocalDateTime(2017, 06, 11));
#endif

    [Test]
    public void from_DateTimeOffset()
        => Converting.From(Svo.DateTimeOffset).To<LocalDateTime>().Should().Be(Svo.LocalDateTime);

    [Test]
    public void from_Date()
        => Converting.From(Svo.Date).To<LocalDateTime>().Should().Be(new LocalDateTime(2017, 06, 11));

    [Test]
    public void from_WeekDate()
        => Converting.From(Svo.WeekDate).To<LocalDateTime>().Should().Be(new LocalDateTime(2017, 06, 11));

    [Test]
    public void from_year_month()
        => Converting.From(Svo.YearMonth).To<LocalDateTime>().Should().Be(new LocalDateTime(2017, 06, 01));

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.LocalDateTime).Should().Be("11/06/2017 06:15:00");
        }
    }


    [Test]
    public void to_DateTime()
        => Converting.To<DateTime>().From(Svo.LocalDateTime).Should().Be(Svo.DateTime);

#if NET8_0_OR_GREATER
    [Test]
    public void to_DateOnly()
        => Converting.To<DateOnly>().From(Svo.LocalDateTime).Should().Be(Svo.DateOnly);
#endif

    [Test]
    public void to_DateTimeOffset()
        => Converting.To<DateTimeOffset>().From(Svo.LocalDateTime).Should().Be(Svo.DateTimeOffset);

    [Test]
    public void to_Date()
        => Converting.To<Date>().From(Svo.LocalDateTime).Should().Be(Svo.Date);

    [Test]
    public void to_WeekDate()
        => Converting.To<WeekDate>().From(Svo.LocalDateTime).Should().Be(Svo.WeekDate);

    [Test]
    public void to_year_month()
        => Converting.To<YearMonth>().From(Svo.LocalDateTime).Should().Be(Svo.YearMonth);
}

public class Supports_JSON_serialization
{
#if NET8_0_OR_GREATER
    [TestCase(627178398050010000L, "1988-06-13 22:10:05.001")]
    [TestCase("1988-06-13 22:10:05.001", "1988-06-13 22:10:05.001")]
    public void System_Text_JSON_deserialization(object json, LocalDateTime svo)
        => JsonTester.Read_System_Text_JSON<LocalDateTime>(json).Should().Be(svo);

    [TestCase("1988-06-13 22:10:05.001", "1988-06-13 22:10:05.001")]
    public void System_Text_JSON_serialization(LocalDateTime svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [TestCase(627178398050010000L, "1988-06-13 22:10:05.001")]
    [TestCase("1988-06-13 22:10:05.001", "1988-06-13 22:10:05.001")]
    public void convention_based_deserialization(object json, LocalDateTime svo)
      => JsonTester.Read<LocalDateTime>(json).Should().Be(svo);

    [TestCase("1988-06-13 22:10:05.001", "1988-06-13 22:10:05.001")]
    public void convention_based_serialization(LocalDateTime svo, object json)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("yyyy-06-11", typeof(FormatException))]
    [TestCase(true, typeof(InvalidOperationException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
        => json
            .Invoking(JsonTester.Read<LocalDateTime>)
            .Should().Throw<Exception>()
            .And.Should().BeOfType(exceptionType);
}

public class Casts
{
#if NET8_0_OR_GREATER
    [Test]
    public void explicitly_from_DateOnly()
    {
        var casted = (LocalDateTime)Svo.DateOnly;
        casted.Should().Be(new(2017, 06, 11));
    }

    [Test]
    public void explicitly_to_DateOnly()
    {
        DateOnly casted = (DateOnly)Svo.LocalDateTime;
        casted.Should().Be(Svo.DateOnly);
    }
#endif

    [Test]
    public void explicitly_from_Date()
    {
        var casted = (LocalDateTime)Svo.Date;
        casted.Should().Be(new(2017, 06, 11));
    }

    [Test]
    public void explicitly_to_Date()
    {
        var casted = (Date)Svo.LocalDateTime;
        casted.Should().Be(Svo.Date);
    }
}

