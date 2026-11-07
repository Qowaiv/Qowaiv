namespace Mathematics.DecimalMath_specs;

public class Constants
{
    [Test]
    public void Epsilon() => DecimalMath.Epsilon.Should().Be(new decimal(1, 0, 0, isNegative: false, scale: 28));

    [Test]
    public void E() => DecimalMath.E.Should().Be(2.7182818284590452353602874714m);

    [Test]
    public void Pi() => DecimalMath.Pi.Should().Be(3.1415926535897932384626433833m);
}

public class Abs
{
    [Test]
    [Arguments(0, 0)]
    [Arguments(+1.5, 1.5)]
    [Arguments(-1.5, 1.5)]
    public void returns_absolute_value(decimal value, decimal absolute)
        => DecimalMath.Abs(value).Should().Be(absolute);
}

public class Rounds
{
    [Test]
    [Arguments("+24.5", "+24")]
    [Arguments("-24.5", "-24")]
    public void To_even_by_default(decimal value, decimal rounded)
        => DecimalMath.Round(value).Should().Be(rounded);

    /// <remarks>Use strings as doubles lack precision.</remarks>
    [Test]
    [Arguments("123456789123456789.1234567890", +10)]
    [Arguments("123456789123456789.123456789", +9)]
    [Arguments("123456789123456789.12345679", +8)]
    [Arguments("123456789123456789.1234568", +7)]
    [Arguments("123456789123456789.123457", +6)]
    [Arguments("123456789123456789.12346", +5)]
    [Arguments("123456789123456789.1235", +4)]
    [Arguments("123456789123456789.123", +3)]
    [Arguments("123456789123456789.12", +2)]
    [Arguments("123456789123456789.1", +1)]
    [Arguments("123456789123456789", +0)]
    [Arguments("123456789123456790", -1)]
    [Arguments("123456789123456800", -2)]
    [Arguments("123456789123457000", -3)]
    [Arguments("123456789123460000", -4)]
    [Arguments("123456789123500000", -5)]
    [Arguments("123456789123000000", -6)]
    [Arguments("123456789120000000", -7)]
    [Arguments("123456789100000000", -8)]
    [Arguments("123456789000000000", -9)]
    [Arguments("123456790000000000", -10)]
    [Arguments("123456800000000000", -11)]
    [Arguments("123457000000000000", -12)]
    [Arguments("123460000000000000", -13)]
    [Arguments("123500000000000000", -14)]
    [Arguments("123000000000000000", -15)]
    [Arguments("120000000000000000", -16)]
    [Arguments("100000000000000000", -17)]
    [Arguments(0, -18)]
    public void Takes_digits_into_account(decimal exp, int digits)
    {
        var act = DecimalMath.Round(123456789123456789.123456789m, digits, DecimalRounding.AwayFromZero);
        act.Should().Be(exp);
    }

    // Halfway/nearest rounding
    [Test]
    [Arguments(-26, -25.5, DecimalRounding.AwayFromZero)]
    [Arguments(+26, +25.5, DecimalRounding.AwayFromZero)]
    [Arguments(-25, -25.5, DecimalRounding.TowardsZero)]
    [Arguments(+25, +25.5, DecimalRounding.TowardsZero)]
    [Arguments(+26, +25.5, DecimalRounding.ToEven)]
    [Arguments(+24, +24.5, DecimalRounding.ToEven)]
    [Arguments(+25, +25.5, DecimalRounding.ToOdd)]
    [Arguments(+25, +24.5, DecimalRounding.ToOdd)]
    [Arguments(-25, -25.5, DecimalRounding.Up)]
    [Arguments(+26, +25.5, DecimalRounding.Up)]
    [Arguments(-26, -25.5, DecimalRounding.Down)]
    [Arguments(+25, +25.5, DecimalRounding.Down)]

    // Up
    [Arguments(-26, -25.6, DecimalRounding.AwayFromZero)]
    [Arguments(+26, +25.6, DecimalRounding.AwayFromZero)]
    [Arguments(-26, -25.6, DecimalRounding.TowardsZero)]
    [Arguments(+26, +25.6, DecimalRounding.TowardsZero)]
    [Arguments(+26, +25.6, DecimalRounding.ToEven)]
    [Arguments(+25, +24.6, DecimalRounding.ToEven)]
    [Arguments(+26, +25.6, DecimalRounding.ToOdd)]
    [Arguments(+25, +24.6, DecimalRounding.ToOdd)]
    [Arguments(-26, -25.6, DecimalRounding.Up)]
    [Arguments(+26, +25.6, DecimalRounding.Up)]
    [Arguments(-26, -25.6, DecimalRounding.Down)]
    [Arguments(+26, +25.6, DecimalRounding.Down)]
    [Arguments(+26, +25.6, DecimalRounding.RandomTieBreaking)]
    [Arguments(-26, -25.6, DecimalRounding.RandomTieBreaking)]

    // Down
    [Arguments(-25, -25.1, DecimalRounding.AwayFromZero)]
    [Arguments(+25, +25.1, DecimalRounding.AwayFromZero)]
    [Arguments(-25, -25.1, DecimalRounding.TowardsZero)]
    [Arguments(+25, +25.1, DecimalRounding.TowardsZero)]
    [Arguments(+25, +25.1, DecimalRounding.ToEven)]
    [Arguments(+24, +24.1, DecimalRounding.ToEven)]
    [Arguments(+25, +25.1, DecimalRounding.ToOdd)]
    [Arguments(+24, +24.1, DecimalRounding.ToOdd)]
    [Arguments(-25, -25.1, DecimalRounding.Up)]
    [Arguments(+25, +25.1, DecimalRounding.Up)]
    [Arguments(-25, -25.1, DecimalRounding.Down)]
    [Arguments(+25, +25.1, DecimalRounding.Down)]
    [Arguments(+25, +25.1, DecimalRounding.RandomTieBreaking)]
    [Arguments(-25, -25.1, DecimalRounding.RandomTieBreaking)]

    // Direct rounding
    [Arguments(-26, -25.1, DecimalRounding.DirectAwayFromZero)]
    [Arguments(+26, +25.1, DecimalRounding.DirectAwayFromZero)]
    [Arguments(-25, -25.1, DecimalRounding.DirectTowardsZero)]
    [Arguments(+25, +25.1, DecimalRounding.DirectTowardsZero)]
    [Arguments(-25, -25.1, DecimalRounding.Ceiling)]
    [Arguments(+26, +25.1, DecimalRounding.Ceiling)]
    [Arguments(-26, -25.1, DecimalRounding.Floor)]
    [Arguments(+25, +25.1, DecimalRounding.Floor)]
    [Arguments(-25, -25.1, DecimalRounding.Truncate)]
    [Arguments(+25, +25.1, DecimalRounding.Truncate)]
    public void Takes_strategy_into_account(decimal rounded, decimal value, DecimalRounding mode)
        => DecimalMath.Round(value, 0, mode).Should().Be(rounded);

    [Test]
    [Arguments(17.1)]
    [Arguments(17.3)]
    [Arguments(17.6)]
    [Arguments(17.8)]
    public void Stochastic_within_margin(decimal value)
    {
        var runs = 10_000;
        var sum = 0m;

        for (var i = 0; i < runs; i++)
        {
            var rounded = DecimalMath.Round(value, 0, DecimalRounding.StochasticRounding);
            (rounded == 17 || rounded == 18).Should().BeTrue();
            sum += rounded;
        }

        var avg = sum / runs;
        avg.Should().BeApproximately(value, 0.05m);
    }

    [Test]
    public void Tie_breaking_within_margin()
    {
        var value = 17.5m;

        var runs = 100_000;
        var sum = 0m;

        for (var i = 0; i < runs; i++)
        {
            var rounded = DecimalMath.Round(value, 0, DecimalRounding.RandomTieBreaking);
            (rounded == 17 || rounded == 18).Should().BeTrue();
            sum += rounded;
        }

        var avg = sum / runs;
        avg.Should().BeApproximately(value, 0.05m);
    }

    [Test]
    public void Supports_non_leading_decimal_zeros()
    {
        // initializes a decimal with scale of 4, instead of 0.
        var value = decimal.Parse("1000.0000", CultureInfo.InvariantCulture);
        var rounded = DecimalMath.Round(value, -2);
        rounded.Should().Be(1000m);
    }

    [Test]
    public void Round_ALotOf3s_WithoutIssues()
    {
        var value = 9_876_543_210m + 1m / 3m;
        var rounded = DecimalMath.Round(value, -9);
        var expected = 10_000_000_000m;

        rounded.Should().Be(expected);
    }
}

public class Rounds_to_multple_of
{
    [Test]
    [Arguments("+24.5", "+24")]
    [Arguments("-24.5", "-24")]
    public void To_even_by_default_for_positive(decimal value, decimal rounded)
        => DecimalMath.RoundToMultiple(value, 1).Should().Be(rounded);

    [Test]
    [Arguments(150.0, 125.0, 50, DecimalRounding.AwayFromZero)]
    [Arguments(125.0, 123.0, 5, DecimalRounding.AwayFromZero)]
    [Arguments(123.25, 123.3085, 0.25, DecimalRounding.AwayFromZero)]
    [Arguments(666, 666, 3, DecimalRounding.AwayFromZero)]
    public void Supports_rouding_stratgies(decimal rounded, decimal value, decimal multipleOf, DecimalRounding mode)
        => DecimalMath.RoundToMultiple(value, multipleOf, mode).Should().Be(rounded);
}

public class Signs
{
    [Test]
    [Arguments(-1, -1.5)]
    [Arguments(+0, 0)]
    [Arguments(+1, +1.5)]
    public void returns_the_sign(int sign, decimal value)
        => DecimalMath.Sign(value).Should().Be(sign);
}
