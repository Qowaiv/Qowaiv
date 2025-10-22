using AwesomeAssertions.Extensions;

namespace Month_span_specs;

public class Guards
{
    [TestCase(10_000)]
    [TestCase(20_000)]
    public void year_not_to_exceed_9999(int years)
        => years.Invoking(MonthSpan.FromYears)
        .Should().Throw<ArgumentOutOfRangeException>();

    [TestCase(10_000 * 12)]
    [TestCase(200_000)]
    public void months_not_to_exceed_9999(int months)
        => months.Invoking(MonthSpan.FromMonths)
        .Should().Throw<ArgumentOutOfRangeException>();

    [Test]
    public void ctor_arguments()
    {
        Func<MonthSpan> ctor = () => new MonthSpan(years: 9800, months: 5000);
        ctor.Should().Throw<ArgumentOutOfRangeException>();
    }
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.MonthSpan.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.MonthSpan.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.MonthSpan.Equals(MonthSpan.MinValue).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.MonthSpan.Equals(MonthSpan.FromMonths(69)).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (MonthSpan.FromMonths(69) == Svo.MonthSpan).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (MonthSpan.FromMonths(69) == MonthSpan.MinValue).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (MonthSpan.FromMonths(69) != Svo.MonthSpan).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (MonthSpan.FromMonths(69) != MonthSpan.MinValue).Should().BeTrue();

    [TestCase("0Y+0M", 0)]
    [TestCase("5Y+9M", 665630102)]
    public void hash_code_is_value_based(MonthSpan svo, int hash)
    {
        using (Hash.WithoutRandomizer())
        {
            svo.GetHashCode().Should().Be(hash);
        }
    }
}

public class Can_be_transformed
{
    [Test]
    public void negate() => (-Svo.MonthSpan).Should().Be(MonthSpan.FromMonths(-69));

    [Test]
    public void increment()
    {
        var span = Svo.MonthSpan;
        span++;
        span.Should().Be(MonthSpan.FromMonths(70));
    }
    [Test]
    public void decrement()
    {
        var span = Svo.MonthSpan;
        span--;
        span.Should().Be(MonthSpan.FromMonths(68));
    }

    [Test]
    public void multiply_by_int() => (Svo.MonthSpan * 3).Should().Be(MonthSpan.FromMonths(207));

    [Test]
    public void multiply_by_short() => (Svo.MonthSpan * (short)3).Should().Be(MonthSpan.FromMonths(207));

    [Test]
    public void multiply_by_double() => (Svo.MonthSpan * 0.608698).Should().Be(MonthSpan.FromMonths(42));

    [Test]
    public void multiply_by_decimal() => (Svo.MonthSpan * 0.608698m).Should().Be(MonthSpan.FromMonths(42));

    [Test]
    public void divide_by_int() => (Svo.MonthSpan / 3).Should().Be(MonthSpan.FromMonths(23));

    [Test]
    public void divide_by_short() => (Svo.MonthSpan / (short)3).Should().Be(MonthSpan.FromMonths(23));

    [Test]
    public void divide_by_double() => (Svo.MonthSpan / 4.0588).Should().Be(MonthSpan.FromMonths(17));

    [Test]
    public void divide_by_decimal() => (Svo.MonthSpan / 4.0588m).Should().Be(MonthSpan.FromMonths(17));
}

public class Can_modify
{
    [Test]
    public void DateTime_by_addition()
    {
        var added = Svo.DateTime + Svo.MonthSpan;
        added.Should().Be(11.March(2023).At(06, 15).AsUtc());
    }

    [Test]
    public void DateTime_by_subtraction()
    {
        var added = Svo.DateTime - Svo.MonthSpan;
        added.Should().Be(11.September(2011).At(06, 15).AsUtc());
    }

#if NET8_0_OR_GREATER
    [Test]
    public void DateOnly_by_addition()
    {
        var added = Svo.DateOnly + Svo.MonthSpan;
        added.Should().Be(new DateOnly(2023, 03, 11));
    }

    [Test]
    public void DateOnly_by_subtraction()
    {
        var added = Svo.DateOnly - Svo.MonthSpan;
        added.Should().Be(new DateOnly(2011, 09, 11));
    }
#endif
}

public class Can_subtract
{
    [TestCase("2020-04-30", "1710-02-01", "310Y+2M")]
    [TestCase("2020-04-30", "2020-04-01", 00)]
    [TestCase("2020-04-30", "2020-03-31", 01)]
    [TestCase("2020-01-01", "2019-01-02", 11)]
    [TestCase("2020-01-01", "2019-03-13", 09)]
    [TestCase("2020-01-01", "2019-03-01", 10)]
    [TestCase("2020-01-01", "2020-02-20", -1)]
    public void two_Dates(Date d1, Date d2, MonthSpan expected)
        => MonthSpan.Subtract(d1, d2).Should().Be(expected);

#if NET8_0_OR_GREATER
    [TestCase("2020-04-30", "1710-02-01", "310Y+2M")]
    [TestCase("2020-04-30", "2020-04-01", 00)]
    [TestCase("2020-04-30", "2020-03-31", 01)]
    [TestCase("2020-01-01", "2019-01-02", 11)]
    [TestCase("2020-01-01", "2019-03-13", 09)]
    [TestCase("2020-01-01", "2019-03-01", 10)]
    [TestCase("2020-01-01", "2020-02-20", -1)]
    public void two_DateOnlys(Date d1, Date d2, MonthSpan expected)
        => MonthSpan.Subtract((DateOnly)d1, (DateOnly)d2).Should().Be(expected);
#endif
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.MonthSpan.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_MonthSpan_as_object()
    {
        object obj = Svo.MonthSpan;
        Svo.MonthSpan.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_MonthSpan_only()
        => new object().Invoking(Svo.Month.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new[]
        {
            MonthSpan.FromMonths(-3),
            MonthSpan.Zero,
            MonthSpan.FromMonths(1),
            MonthSpan.FromMonths(12),
            MonthSpan.FromMonths(13),
            MonthSpan.FromMonths(145),
        };

        var list = new List<MonthSpan> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }

    [Test]
    public void by_operators_for_different_values()
    {
        var smaller = MonthSpan.FromMonths(17);
        var bigger = MonthSpan.FromMonths(42);

        (smaller < bigger).Should().BeTrue();
        (smaller <= bigger).Should().BeTrue();
        (smaller > bigger).Should().BeFalse();
        (smaller >= bigger).Should().BeFalse();
    }

    [Test]
    public void by_operators_for_equal_values()
    {
        var left = MonthSpan.FromMonths(17);
        var right = MonthSpan.FromMonths(17);

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
        => typeof(MonthSpan).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<MonthSpan>().Should().Be(MonthSpan.Zero);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<MonthSpan>().Should().Be(MonthSpan.Zero);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("5Y+9M").To<MonthSpan>().Should().Be(Svo.MonthSpan);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.MonthSpan).Should().Be("5Y+9M");
        }
    }

    [Test]
    public void from_int()
        => Converting.From(69).To<MonthSpan>().Should().Be(Svo.MonthSpan);

    [Test]
    public void to_int()
        => Converting.To<int>().From(Svo.MonthSpan).Should().Be(69);
}

public class Supports_JSON_serialization
{
#if NET8_0_OR_GREATER
    [TestCase(69d, "5Y+9M")]
    [TestCase(69L, "5Y+9M")]
    [TestCase("5Y+9M", "5Y+9M")]
    public void System_Text_JSON_deserialization(object json, MonthSpan svo)
        => JsonTester.Read_System_Text_JSON<MonthSpan>(json).Should().Be(svo);

    [TestCase("5Y+9M", "5Y+9M")]
    public void System_Text_JSON_serialization(MonthSpan svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [TestCase(69L, "5Y+9M")]
    [TestCase("5Y+9M", "5Y+9M")]
    public void convention_based_deserialization(object json, MonthSpan svo)
        => JsonTester.Read<MonthSpan>(json).Should().Be(svo);

    [TestCase("5Y+9M", "5Y+9M")]
    public void convention_based_serialization(MonthSpan svo, object json)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("2017-06-11", typeof(FormatException))]
    [TestCase(true, typeof(InvalidOperationException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
    {
        json.Invoking(JsonTester.Read<MonthSpan>)
            .Should().Throw<Exception>().Which.Should().BeOfType(exceptionType);
    }
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
       => OpenApiDataType.FromType(typeof(MonthSpan))
       .Should().Be(new OpenApiDataType(
           dataType: typeof(MonthSpan),
           description: "Month span, specified in years and months.",
           example: "1Y+10M",
           type: "string",
           format: "month-span",
           pattern: @"[+-]?[0-9]+Y[+-][0-9]+M"));
}

public class Can_be_deconstructed
{
    [Test]
    public void in_years_and_months_part()
    {
        var (years, months) = Svo.MonthSpan;
        years.Should().Be(5);
        months.Should().Be(9);
    }
}
