using Qowaiv.Specs.TestTools;

namespace Mathematics.Fraction_specs;

public class Defines
{
    [TestCase("0", "0")]
    [TestCase("17/3", "17/3")]
    [TestCase("-7/3", "-7/3")]
    public void plus(Fraction fraction, Fraction negated) => (+fraction).Should().Be(negated);

    [TestCase("0", "0")]
    [TestCase("-7/3", "+7/3")]
    [TestCase("+7/3", "-7/3")]
    public void negation(Fraction fraction, Fraction negated) => (-fraction).Should().Be(negated);

    [Test]
    public void increment()
    {
        var fraction = Svo.Fraction;
        fraction++;
        fraction.Should().Be(-52.DividedBy(17));
    }

    [Test]
    public void decrement()
    {
        var fraction = Svo.Fraction;
        fraction--;
        fraction.Should().Be(-86.DividedBy(17));
    }

    public class Multiplication
    {
        [TestCase("1/3", "0", "0")]
        [TestCase("0", "5/7", "0")]
        [TestCase("1/3", "1/4", "1/12")]
        [TestCase("-1/3", "-1/4", "1/12")]
        [TestCase("1/4", "4/7", "1/7")]
        [TestCase("2/5", "11/16", "11/40")]
        [TestCase("-2/5", "4", "-8/5")]
        [TestCase("2/3", "-8/9", "-16/27")]
        public void between_fractions(Fraction left, Fraction right, Fraction product)
            => (left * right).Should().Be(product);

        [Test]
        public void between_fractions_and_longs()
           => (1.DividedBy(3) * 4L).Should().Be(4.DividedBy(3));

        [Test]
        public void between_fractions_and_ints()
           => (1.DividedBy(3) * 4).Should().Be(4.DividedBy(3));


        [Test]
        public void between_longs_and_fractions()
            => (4L * 1.DividedBy(3)).Should().Be(4.DividedBy(3));

        [Test]
        public void between_ints_and_fractions()
           => (4 * 1.DividedBy(3)).Should().Be(4.DividedBy(3));
    }

    public class Division
    {
        [TestCase("0", "1/3", "0")]
        [TestCase("1/3", "1/4", "4/3")]
        [TestCase("-1/3", "-1/4", "4/3")]
        [TestCase("1/4", "4/7", "7/16")]
        [TestCase("2/5", "11/16", "32/55")]
        [TestCase("-2/5", "4", "-1/10")]
        [TestCase("2/3", "-8/9", "-3/4")]
        public void between_Fractions(Fraction left, Fraction right, Fraction division)
            => (left / right).Should().Be(division);

        [Test]
        public void between_fractions([RandomFraction(3)] Fraction left, [RandomFraction(3, false)] Fraction right)
        {
            var division = ((decimal)left) / ((decimal)right);
            (left / right).Should().Be(division.Fraction());
        }

        [Test]
        public void between_fractions_and_longs()
          => (1.DividedBy(3) / 4L).Should().Be(1.DividedBy(12));

        [Test]
        public void between_fractions_and_ints()
           => (1.DividedBy(3) / 4).Should().Be(1.DividedBy(12));


        [Test]
        public void between_longs_and_fractions()
            => (5L / 2.DividedBy(3)).Should().Be(15.DividedBy(2));

        [Test]
        public void between_ints_and_fractions()
           => (5 / 2.DividedBy(3)).Should().Be(15.DividedBy(2));
    }

    public class Addition
    {
        [TestCase("1/4", "0", "1/4")]
        [TestCase("0", "1/4", "1/4")]
        [TestCase("5/7", "0", "5/7")]
        [TestCase("1/3", "1/4", "7/12")]
        [TestCase("1/4", "1/3", "7/12")]
        [TestCase("1/4", "1/12", "1/3")]
        [TestCase("-1/4", "-1/12", "-1/3")]
        [TestCase("-1/4", "1/12", "-1/6")]
        [TestCase("1/5", "2/5", "3/5")]
        [TestCase("8/3", "1/2", "19/6")]
        public void between_fractions(Fraction left, Fraction right, Fraction addition)
            => (left + right).Should().Be(addition);

        [Test]
        public void between_fractions([RandomFraction(3)] Fraction left, [RandomFraction(3)] Fraction right)
        {
            var sum = ((decimal)left) + ((decimal)right);
            (left + right).Should().Be(sum.Fraction());
        }

        [Test]
        public void between_fractions_and_longs()
            => (1.DividedBy(3) + 4L).Should().Be(13.DividedBy(3));

        [Test]
        public void between_fractions_and_ints()
           => (1.DividedBy(3) + 4).Should().Be(13.DividedBy(3));


        [Test]
        public void between_longs_and_fractions()
            => (4L + 1.DividedBy(3)).Should().Be(13.DividedBy(3));

        [Test]
        public void between_ints_and_fractions()
           => (4 + 1.DividedBy(3)).Should().Be(13.DividedBy(3));
    }

    public class Subtraction
    {
        [TestCase("1/4", "0", "1/4")]
        [TestCase("0", "1/4", "-1/4")]
        [TestCase("1/3", "1/4", "1/12")]
        [TestCase("1/4", "1/3", "-1/12")]
        [TestCase("1/4", "1/12", "1/6")]
        [TestCase("-1/4", "-1/12", "-1/6")]
        [TestCase("-1/4", "1/12", "-1/3")]
        public void between_fractions(Fraction left, Fraction right, Fraction subtraction)
            => (left - right).Should().Be(subtraction);

        [Test]
        public void between_fractions([RandomFraction(3)] Fraction left, [RandomFraction(3)] Fraction right)
        {
            var subtraction = ((decimal)left) - ((decimal)right);
            (left - right).Should().Be(subtraction.Fraction());
        }

        [Test]
        public void between_fractions_and_longs()
          => (1.DividedBy(3) - 4L).Should().Be(-11.DividedBy(3));

        [Test]
        public void between_fractions_and_ints()
           => (1.DividedBy(3) - 4).Should().Be(-11.DividedBy(3));

        [Test]
        public void between_longs_and_fractions()
            => (4L - 1.DividedBy(3)).Should().Be(11.DividedBy(3));

        [Test]
        public void between_ints_and_fractions()
           => (4 - 1.DividedBy(3)).Should().Be(11.DividedBy(3));
    }

    public class Modulation
    {
        [TestCase("5/4", "1/1", "1/4")]
        [TestCase("-5/4", "1/1", "-1/4")]
        [TestCase("5/3", "2/3", "1/3")]
        public void between_fractions(Fraction fraction, Fraction divider, Fraction remainder)
            => (fraction % divider).Should().Be(remainder);

        [Test]
        public void between_fractions([RandomFraction(3)] Fraction fraction, [RandomFraction(3, false)] Fraction divider)
        {
            var modulo = ((decimal)fraction) % ((decimal)divider);
            (fraction % divider).Should().Be(modulo.Fraction());
        }

        [Test]
        public void between_fractions_and_longs()
          => (1.DividedBy(3) % 4L).Should().Be(1.DividedBy(3));

        [Test]
        public void between_fractions_and_ints()
           => (1.DividedBy(3) % 4).Should().Be(1.DividedBy(3));

        [Test]
        public void between_longs_and_fractions()
            => (4L % 3.DividedBy(1)).Should().Be(1.DividedBy(1));

        [Test]
        public void between_ints_and_fractions()
           => (4 % 3.DividedBy(1)).Should().Be(1.DividedBy(1));
    }

    [TestCase("0", "0")]
    [TestCase("17/3", "17/3")]
    [TestCase("-7/3", "+7/3")]
    public void abs(Fraction fraction, Fraction absolute)
        => fraction.Abs().Should().Be(absolute);

    [TestCase("+1/4", "4/1")]
    [TestCase("-2/3", "-3/2")]
    public void inverse(Fraction faction, Fraction inverse)
        => faction.Inverse().Should().Be(inverse);

    [TestCase("0", 0)]
    [TestCase("-1/4", -1)]
    [TestCase("+1/4", +1)]
    public void sign(Fraction fraction, int sign)
        => fraction.Sign().Should().Be(sign);

    [TestCase("-69/17", 1)]
    [TestCase("+69/17", 1)]
    [TestCase("0", 0)]
    public void remainder_as_positive_value(Fraction fraction, int remainder)
        => fraction.Remainder.Should().Be(remainder);

    [TestCase("-69/17", -4)]
    [TestCase("+69/17", +4)]
    [TestCase("0", 0)]
    public void whole(Fraction fraction, int remainder)
        => fraction.Whole.Should().Be(remainder);
}

public class Has_constant
{
    [Test]
    public void Zero_represent_0_divided_by_1()
        => Fraction.Zero.Should().BeEquivalentTo(new
        {
            Numerator = 0L,
            Denominator = 1L,
        });

    [Test]
    public void Zero_equal_to_default()
       => Fraction.Zero.Should().Be(default);

    [Test]
    public void One_represent_0_divided_by_1()
        => Fraction.One.Should().BeEquivalentTo(new
        {
            Numerator = 1L,
            Denominator = 1L,
        });

    [Test]
    public void Epsilon_represent_0_divided_by_1()
        => Fraction.Epsilon.Should().BeEquivalentTo(new
        {
            Numerator = 1L,
            Denominator = long.MaxValue,
        });

    [Test]
    public void MinValue_represent_0_divided_by_1()
        => Fraction.MinValue.Should().BeEquivalentTo(new
        {
            Numerator = -long.MaxValue,
            Denominator = 1L,
        });

    [Test]
    public void MaxValue_represent_0_divided_by_1()
        => Fraction.MaxValue.Should().BeEquivalentTo(new
        {
            Numerator = long.MaxValue,
            Denominator = 1L,
        });
}

public class Prevents_overflow
{
    [Test]
    public void on_additions()
    {
        var l = 1.DividedBy(4_000_000_000L);
        var r = 1.DividedBy(8_000_000_000L);
        (l + r).Should().Be(3.DividedBy(8_000_000_000L));
    }

    [Test]
    public void on_multiplications()
    {
        var l = 1.DividedBy(4_000_000_000L);
        var r = 8_000_000_000L.DividedBy(3);
        (l * r).Should().Be(2.DividedBy(3));
    }
}

public class Does_not_define
{
    [Test]
    public void inverse_on_zero()
        => Fraction.Zero.Invoking(f => f.Inverse()).Should().Throw<DivideByZeroException>();
}

public class Throws_when
{
    [Test]
    public void multiplication_can_not_be_represented_by_a_long()
    {
        var x = (long.MaxValue - 3).DividedBy(1);
        var y = (long.MaxValue - 4).DividedBy(1);

        x.Invoking(_ => x * y)
            .Should().Throw<OverflowException>()
            .WithMessage("Arithmetic operation resulted in an overflow.*");
    }

    [Test]
    public void division_can_not_be_represented_by_a_long()
    {
        var x = (long.MaxValue - 3).DividedBy(1);
        var y = (long.MaxValue - 4).DividedBy(long.MaxValue - 7);

        x.Invoking(_ => x / y)
            .Should().Throw<OverflowException>()
            .WithMessage("Arithmetic operation resulted in an overflow.*");
    }

    [Test]
    public void addition_can_not_be_represented_by_a_long()
    {
        var x = 17.DividedBy(long.MaxValue - 3);
        var y = 13.DividedBy(long.MaxValue - 4);

        x.Invoking(_ => x + y)
            .Should().Throw<OverflowException>()
            .WithMessage("Arithmetic operation resulted in an overflow.*");
    }

    [Test]
    public void subtraction_can_not_be_represented_by_a_long()
    {
        var x = 17.DividedBy(long.MaxValue - 3);
        var y = 13.DividedBy(long.MaxValue - 4);

        x.Invoking(_ => x - y)
            .Should().Throw<OverflowException>()
            .WithMessage("Arithmetic operation resulted in an overflow.*");
    }

    [TestCase(-19223372036854775809.0)]
    [TestCase(+19223372036854775809.0)]
    public void double_can_not_be_casted_to_fraction(double dbl)
        => dbl.Invoking(d => (Fraction)d).Should().Throw<OverflowException>()
        .WithMessage("Value was either too large or too small for a Fraction.");

    [TestCase(-9223372036854775808.0)]
    [TestCase(+9223372036854775808.0)]
    public void decimal_can_not_be_casted_to_fraction(decimal dec)
        => dec.Invoking(d => (Fraction)d).Should().Throw<OverflowException>()
        .WithMessage("Value was either too large or too small for a Fraction.");

    [TestCase(-9223372036854775808.0)]
    [TestCase(+9223372036854775808.0)]
    public void fraction_can_no_be_created_form_double(double dbl)
        => dbl.Invoking(Fraction.Create).Should().Throw<ArgumentOutOfRangeException>()
        .WithMessage("Value was either too large or too small for a Fraction. *");

    [TestCase(-9223372036854775808.0)]
    [TestCase(+9223372036854775808.0)]
    public void fraction_can_no_be_created_form_decimal(decimal dec)
        => dec.Invoking(Fraction.Create).Should().Throw<ArgumentOutOfRangeException>()
        .WithMessage("Value was either too large or too small for a Fraction. *");

    [Test]
    public void invalid_input_is_parsed()
     => "NaN".Invoking(Fraction.Parse)
     .Should().Throw<FormatException>()
     .WithMessage("Not a valid fraction");
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.Fraction.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.Fraction.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.Fraction.Equals(17.DividedBy(42)).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.Fraction.Equals(-69.DividedBy(17)).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (Svo.Fraction == -69.DividedBy(17)).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (Svo.Fraction == 17.DividedBy(42)).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (Svo.Fraction != -69.DividedBy(17)).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (Svo.Fraction != 17.DividedBy(42)).Should().BeTrue();

    [TestCase(0, 0)]
    [TestCase("17/42", 490960136)]
    public void hash_code_is_value_based(Fraction svo, int hash)
    {
        using (Hash.WithoutRandomizer())
        {
            svo.GetHashCode().Should().Be(hash);
        }
    }
}
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

    [Test]
    public void without_specifying_format_provider()
    {
        Fraction.TryParse("-69/17", out var fraction).Should().BeTrue();
        fraction.Should().Be(Svo.Fraction);
    }

    [Test]
    public void using_pure_try_parse() => Fraction.TryParse("-69/17").Should().Be(Svo.Fraction);
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

    [TestCase("NaN", "NaN")]
    [TestCase("-Infinity", "-Infinity")]
    [TestCase("+Infinity", "+Infinity")]
    [TestCase("0xFF", "Hexa-decimal")]
    [TestCase("15/", "Ends with an operator")]
    [TestCase("1//4", "Two division operators")]
    [TestCase("1/½", "Vulgar with division operator")]
    [TestCase("½1", "Vulgar not at the end")]
    [TestCase("²3/₇", "Normal and superscript mixed")]
    [TestCase("²/₇3", "Normal and subscript mixed")]
    [TestCase("²/3₇", "Normal and subscript mixed")]
    [TestCase("₇/3", "Subscript first")]
    [TestCase("92233720368547758 17/32464364", "Numerator overflow")]
    [TestCase("9223372036854775808", "Long.MaxValue + 1")]
    [TestCase("-9223372036854775808", "Long.MinValue")]
    [TestCase("-9223372036854775809", "Long.MinValue - 1")]
    public void non_fractional_strings(string str, string because)
        => Fraction.TryParse(str).Should().BeNull(because);

    [Test]
    public void retrieving_null_for_invalid_input() => Fraction.TryParse("invalid").Should().BeNull();
}

public class Can_be_created
{
    [TestCase("0/1", 0, 8, "Should set zero")]
    [TestCase("1/4", 2, 8, "Should reduce")]
    [TestCase("-1/4", -2, 8, "Should reduce")]
    [TestCase("1/4", 3, 12, "Should reduce")]
    [TestCase("-1/4", -3, 12, "Should reduce")]
    [TestCase("3/7", -3, -7, "Should have no signs")]
    [TestCase("-3/7", 3, -7, "Should have no sign on denominator")]
    [TestCase("-3/7", -3, 7, "Should have no sign on denominator")]
    public void with_constructor(Fraction fraction, long numerator, long denominator, string because)
        => new Fraction(numerator, denominator).Should().Be(fraction, because);

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

        foreach (var fraction in Enumerable.Range(0, runs).Select(i => rnd.Next(int.MinValue, int.MaxValue).DividedBy(rnd.Next(3, int.MaxValue))))
        {
            var created = Fraction.Create((decimal)fraction);
            if (created != fraction)
            {
                failures.Add(fraction);
            }
        }
        failures.Should().BeEmpty();
    }

    [Test]
    public void applying_greatest_common_divisor()
        => 60.DividedBy(420).Should().BeEquivalentTo(new
        {
            Numerator = 1L,
            Denominator = 7L,
        });
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

public class Can_be_casted_explicit
{
    [TestCase(0, 0)]
    [TestCase("-69/17", -69 / 17)]
    public void to_Int32(Fraction fraction, int casted) => ((int)fraction).Should().Be(casted);

    [TestCase(0, 0)]
    [TestCase("-69/17", -69 / 17)]
    public void to_Int64(Fraction fraction, long casted) => ((long)fraction).Should().Be(casted);

    [TestCase(0, 0)]
    [TestCase("-69/17", -69.0 / 17.0)]
    public void to_double(Fraction fraction, double casted) => ((double)fraction).Should().Be(casted);

    [TestCase(0, 0)]
    [TestCase(-84, -84)]
    public void to_decimal(Fraction fraction, decimal casted) => ((decimal)fraction).Should().Be(casted);

    [Test]
    public void to_percent() => ((Percentage)1.DividedBy(4)).Should().Be(25.Percent());

    [Test]
    public void from_Int32() => ((Fraction)42).Should().Be(42.DividedBy(1));

    [Test]
    public void from_Int64() => ((Fraction)42L).Should().Be(42.DividedBy(1));

    [Test]
    public void from_double() => ((Fraction)(-1.0 / 16.0)).Should().Be(-1.DividedBy(16));

    [Test]
    public void from_decimal() => ((Fraction)(-69m / 17m)).Should().Be(Svo.Fraction);

    [Test]
    public void from_percent() => ((Fraction)25.Percent()).Should().Be(1.DividedBy(4));
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.Fraction.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_Fraction_as_object()
    {
        object obj = Svo.Fraction;
        Svo.Fraction.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_Fraction_only()
        => new object().Invoking(Svo.Fraction.CompareTo).Should().Throw<ArgumentException>();

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

public class Has_custom_formatting
{
    [Test]
    public void _default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Fraction.ToString().Should().Be("-69/17");
        }
    }

    [Test]
    public void with_null_format_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Fraction.ToString().Should().Be(Svo.Fraction.ToString(default(string)));
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Fraction.ToString().Should().Be(Svo.Fraction.ToString(string.Empty));
        }
    }

    [Test]
    public void default_value_is_represented_as_zero()
        => default(Fraction).ToString().Should().Be("0/1");

    [Test]
    public void with_empty_format_provider()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Svo.Fraction.ToString(FormatProvider.Empty).Should().Be("-69/17");
        }
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.Fraction.ToString("[0]super⁄sub", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: '-4¹⁄₁₇', format: '[0]super⁄sub'");
    }

    [TestCase(null, /*............*/ "-2/7", "-2/7")]
    [TestCase("", /*..............*/ "-2/7", "-2/7")]
    [TestCase("0:0", /*...........*/ "-2:7", "-2/7")]
    [TestCase("0÷0", /*...........*/ "4÷3", "4/3")]
    [TestCase("[0]0/0", /*........*/ "1 1/3", "4/3")]
    [TestCase("[0]0/0", /*........*/ "-1 1/3", "-4/3")]
    [TestCase("[0 ]0/0",/*........*/ "-1 1/3", "-4/3")]
    [TestCase("#.00", /*..........*/ ".33", "1/3")]
    [TestCase("[0]super⁄sub", /*..*/ "5¹¹⁄₁₂", "71/12")]
    [TestCase("[0]super⁄0", /*....*/ "5¹¹⁄12", "71/12")]
    [TestCase("[0] 0⁄sub", /*.....*/ "5 11⁄₁₂", "71/12")]
    [TestCase("[0]super⁄sub", /*..*/ "-3¹⁄₂", "-7/2")]
    [TestCase("[0 ]super⁄sub", /*.*/ "-3 ¹⁄₂", "-7/2")]
    [TestCase("[#]super⁄sub", /*..*/ "-¹⁄₂", "-1/2")]
    [TestCase("[0]super⁄sub", /*..*/ "-0¹⁄₂", "-1/2")]
    [TestCase("super⁄sub", /*.....*/ "⁷¹⁄₁₂", "71/12")]
    [TestCase("super⁄sub", /*.....*/ "-⁷⁄₂", "-7/2")]
    public void for_format(string format, string formatted, Fraction fraction)
        => fraction.ToString(format, CultureInfo.InvariantCulture).Should().Be(formatted);

    [Test]
    public void that_throws_for_invalid_formats()
        => "/invalid".Invoking(Svo.Fraction.ToString).Should().Throw<FormatException>();
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
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("-69/17").To<Fraction>().Should().Be(Svo.Fraction);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.Fraction).Should().Be("-69/17");
        }
    }
}

public class Supports_JSON_serialization
{
#if NET6_0_OR_GREATER
    [TestCase(4L, "4/1")]
    [TestCase(0.25d, "1/4")]
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
        => json
            .Invoking(JsonTester.Read<Fraction>)
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
        var round_tripped = SerializeDeserialize.Binary(Svo.Fraction);
        round_tripped.Should().Be(Svo.Fraction);
    }

    [Test]
    public void storing_values_in_SerializationInfo()
    {
        var info = Serialize.GetInfo(Svo.Fraction);
        info.GetInt64("numerator").Should().Be(-69);
        info.GetInt64("denominator").Should().Be(17);
    }
}
#endif

public class Supports_XML_serialization
{
    [Test]
    public void using_XmlSerializer_to_serialize()
    {
        var xml = Serialize.Xml(Svo.Fraction);
        xml.Should().Be("-69/17");
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<Fraction>("-69/17");
        svo.Should().Be(Svo.Fraction);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.Fraction);
        Svo.Fraction.Should().Be(round_tripped);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.Fraction);
        var round_tripped = SerializeDeserialize.Xml(structure);
        structure.Should().Be(round_tripped);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.Fraction;
        obj.GetSchema().Should().BeNull();
    }
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
        => OpenApiDataType.FromType(typeof(Fraction))
        .Should().Be(new OpenApiDataType(
            dataType: typeof(Fraction),
            description: "Faction",
            type: "string",
            format: "faction",
            pattern: "-?[0-9]+(/[0-9]+)?",
            example: "13/42"));
}

public class Debugger
{
    [TestCase("⁰⁄₁ = 0", "0")]
    [TestCase("-⁴²⁄₁₇ = -2.47058824", "-42/17")]
    public void has_custom_display(object display, Fraction svo)
        => svo.Should().HaveDebuggerDisplay(display);
}

public class Can_be_deconstructed
{
    [Test]
    public void in_numerator_and_denominator_part()
    {
        var (numerator, denominator) = Svo.Fraction;
        numerator.Should().Be(-69);
        denominator.Should().Be(17);
    }
}
