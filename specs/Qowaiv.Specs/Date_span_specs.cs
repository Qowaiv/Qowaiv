using AwesomeAssertions.Extensions;

namespace Date_span_specs;

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.DateSpan.CompareTo(Nil.Object).Should().Be(1);
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
#if NET6_0_OR_GREATER
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

#if NET8_0_OR_GREATER
#else
public class Supports_binary_serialization
{
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void using_BinaryFormatter()
        => SerializeDeserialize.Binary(Svo.DateSpan).Should().Be(Svo.DateSpan);

    [Test]
    public void storing_string_in_SerializationInfo()
        => Serialize.GetInfo(Svo.DateSpan).GetUInt64("Value").Should().Be(532575944699UL);
}
#endif

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

#if NET6_0_OR_GREATER

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

#if NET6_0_OR_GREATER
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
