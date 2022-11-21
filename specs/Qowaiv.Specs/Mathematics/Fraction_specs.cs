namespace Mathematics.Fraction_specs;

public class Can_be_parsed
{
    [TestCase(17, 1, "17")]
    [TestCase(17, 1, "+17")]
    [TestCase(-12, 1, "-12")]
    public void from_integer(long numerator, long denominator, string num)
        => Fraction.Parse(num, CultureInfo.InvariantCulture).Should().Be(numerator.DividedBy(denominator));

    [TestCase(12_345, 1, "12,345")]
    [TestCase(-1, 4, "-0.25")]
    public void from_decimal(long numerator, long denominator, string dec)
        => Fraction.Parse(dec, CultureInfo.InvariantCulture).Should().Be(numerator.DividedBy(denominator));

    [TestCase(487, 1000, "48.70%")]
    [TestCase(487, 1000, "487.0‰")]
    public void from_percentage(long numerator, long denominator, string percentage)
        => Fraction.Parse(percentage, CultureInfo.InvariantCulture).Should().Be(numerator.DividedBy(denominator));

    [TestCase(+1, 2, "½")]
    [TestCase(-1, 2, "-½")]
    [TestCase(+3, 4, "¾")]
    [TestCase(11, 4, "2¾")]
    [TestCase(11, 4, "2 ¾")]
    public void from_vulgar(long numerator, long denominator, string vulgar)
        => Fraction.Parse(vulgar).Should().Be(numerator.DividedBy(denominator));

    [TestCase(9, 7, "1²/₇")]
    [TestCase(3, 7, "³/7")]
    [TestCase(9, 7, "1 ²/7")]
    [TestCase(23, 47, "²³/₄₇")]
    public void from_super_script_numerator(long numerator, long denominator, string superScript)
        => Fraction.Parse(superScript).Should().Be(numerator.DividedBy(denominator));

    [TestCase(-2, 7, "-²/₇")]
    [TestCase(+9, 7, "1 2/₇")]
    [TestCase(+3, 7, "3/₇")]
    [TestCase(23, 47, "23/₄₇")]
    public void from_sub_script_denominator(long numerator, long denominator, string subScript)
        => Fraction.Parse(subScript).Should().Be(numerator.DividedBy(denominator));
   
    [TestCase(1, 3, "+1/3")]
    [TestCase(-1, 3, "-1/3")]
    [TestCase(11, 43, "11/43")]
    [TestCase(4, 3, "1 1/3")]
    [TestCase(21, 2, "10 1/2")]
    public void from_fraction_strings(long numerator, long denominator, string str)
        => Fraction.Parse(str, CultureInfo.InvariantCulture).Should().Be(numerator.DividedBy(denominator));

    [TestCase("1/3")]
    [TestCase("1:3")]
    [TestCase("1÷3")]
    [TestCase("1⁄3")]
    [TestCase("1⁄3")]
    [TestCase("1⁄3")]
    [TestCase("1∕3")]
    public void from_multiple_bar_chars(string bar)
       => Fraction.Parse(bar, CultureInfo.InvariantCulture).Should().Be(1.DividedBy(3));
}

public class Can_not_be_parsed
{
    [Test]
    public void string_empty() => Fraction.TryParse(string.Empty).Should().BeNull();

    [Test]
    public void @null() => Fraction.TryParse(null).Should().BeNull();

    [TestCase("--3/7")]
    [TestCase("-+3/7")]
    [TestCase("+-3/7")]
    [TestCase("++3/7")]
    public void multiple_signs(string multiple) => Fraction.TryParse(multiple).Should().BeNull();

    [TestCase("3/+7")]
    public void plus_sign_denominator(string denominator) => Fraction.TryParse(denominator).Should().BeNull();

    [TestCase("3/0")]
    [TestCase("²/₀")]
    public void zero_denominator(string divideByZero) => Fraction.TryParse(divideByZero).Should().BeNull();
}

public class Can_be_created
{
    [TestCase(0, 1, "0")]
    [TestCase(00000003, 000000010, "0.3")]
    [TestCase(00000033, 000000100, "0.33")]
    [TestCase(00000333, 000001000, "0.333")]
    [TestCase(00003333, 000010000, "0.3333")]
    [TestCase(00033333, 000100000, "0.33333")]
    [TestCase(00333333, 001000000, "0.333333")]
    [TestCase(03333333, 010000000, "0.3333333")]
    [TestCase(33333333, 100000000, "0.33333333")]
    [TestCase(333333333, 1000000000, "0.333333333")]
    [TestCase(1, 3, "0.33333333333333333333333")]
    public void from_decimals(long numerator, long denominator, decimal number)
        => Fraction.Create(number).Should().Be(numerator.DividedBy(denominator));

    [TestCase(0, 1, 0.5, 0.6)]
    [TestCase(1, 1, 0.6, 0.5)]
    public void from_decimals_with_error(long numerator, long denominator, decimal number, decimal error)
        => Fraction.Create(number, error).Should().Be(numerator.DividedBy(denominator));
    
    [TestCase(100)]
    public void from_decimals_without_precision_loss(int runs)
    {
        var rnd = new Random();
        var failures = new List<Fraction>(runs);

        foreach(var fraction in Enumerable.Range(0, runs).Select(i => rnd.Next(int.MinValue, int.MaxValue).DividedBy(rnd.Next(3, int.MaxValue))))
        {
            var created = Fraction.Create((decimal)fraction);
            if (created != fraction)
            {
                failures.Add(fraction);
            }
        }
        failures.Should().BeEmpty();
    }
}

public class Can_not_be_created
{
    [TestCase(-10e18)]
    [TestCase(+10e18)]
    public void from_decimal_out_of_long_range(decimal dec)
    {
        Func<Fraction> create = () => Fraction.Create(dec);
        create.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestCase(1e-19)]
    [TestCase(+1.000001)]
    public void from_decimal_with_error_out_of_range(decimal error)
    {
        Func<Fraction> create = () => Fraction.Create(0, error);
        create.Should().Throw<ArgumentOutOfRangeException>();
    }
}

public class Is_comparable
{
    [Test]
    public void to_null() => Svo.Fraction.CompareTo(null).Should().Be(1);

    [Test]
    public void to_Fraction_as_object()
    {
        object obj = Svo.Fraction;
        Svo.Fraction.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_Fraction_only()
        => Assert.Throws<ArgumentException>(() => Svo.Fraction.CompareTo(new object()));

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new[]
        {
            (-1).DividedBy(12),
            Fraction.Zero,
            1.DividedBy(42),
            1.DividedBy(17),
            1.DividedBy(11),
            201.DividedBy(42),
        };

        var list = new List<Fraction> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }

    [Test]
    public void by_operators_for_different_values()
    {
        var smaller = 1.DividedBy(17);
        var bigger = 2.DividedBy(3);

        (smaller < bigger).Should().BeTrue();
        (smaller <= bigger).Should().BeTrue();
        (smaller > bigger).Should().BeFalse();
        (smaller >= bigger).Should().BeFalse();
    }

    [Test]
    public void by_operators_for_equal_values()
    {
        var left = 1.DividedBy(17);
        var right = 1.DividedBy(17);

        (left < right).Should().BeFalse();
        (left <= right).Should().BeTrue();
        (left > right).Should().BeFalse();
        (left >= right).Should().BeTrue();
    }
}

public class Has_humanizer_creators
{
    [Test]
    public void int_DividedBy_int()
        => 12.DividedBy(24).Should().Be(new Fraction(1, 2));

    [Test]
    public void long_DividedBy_long()
       => 12L.DividedBy(24L).Should().Be(new Fraction(1, 2));

    [Test]
    public void Fraction_from_double()
        => 0.5.Fraction().Should().Be(new Fraction(1, 2));

    [Test]
    public void Fraction_from_decimal()
        => 0.5m.Fraction().Should().Be(new Fraction(1, 2));
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(Fraction).Should().HaveTypeConverterDefined();

    [Test]
    public void from_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From("-69/17").To<Fraction>().Should().Be(Svo.Fraction);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.ToString().From(Svo.Fraction).Should().Be("-69/17");
        }
    }
}

public class Supports_JSON_serialization
{
#if NET6_0_OR_GREATER
    [TestCase(4L, "4/1")]
    [TestCase(3d, "3/1")]
    [TestCase("13%", "13/100")]
    [TestCase("14/42", "1/3")]
    public void System_Text_JSON_deserialization(object json, Fraction svo)
        => JsonTester.Read_System_Text_JSON<Fraction>(json).Should().Be(svo);

    [TestCase("1/3", "1/3")]
    [TestCase("4/3", "4/3")]
    public void System_Text_JSON_serialization(Fraction svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [TestCase(4L, "4/1")]
    [TestCase(3d, "3/1")]
    [TestCase("13%", "13/100")]
    [TestCase("14/42", "1/3")]
    public void convention_based_deserialization(object json, Fraction svo)
        => JsonTester.Read<Fraction>(json).Should().Be(svo);

    [TestCase("1/3", "1/3")]
    [TestCase("4/3", "4/3")]
    public void convention_based_serialization(Fraction svo, object json)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase(double.MaxValue, typeof(OverflowException))]
    [TestCase(double.MinValue, typeof(OverflowException))]
    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("2017-06-11", typeof(FormatException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
    {
        var exception = Assert.Catch(() => JsonTester.Read<Fraction>(json));
        Assert.IsInstanceOf(exceptionType, exception);
    }
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
        => Qowaiv.OpenApi.OpenApiDataType.FromType(typeof(Fraction))
        .Should().Be(new Qowaiv.OpenApi.OpenApiDataType(
            dataType: typeof(Fraction),
            description: "Faction",
            type: "string",
            format: "faction",
            pattern: "-?[0-9]+(/[0-9]+)?",
            example: "13/42"));
}
