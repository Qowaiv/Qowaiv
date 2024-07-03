﻿namespace Decimal_round_specs;

public class Rounds
{
    private readonly decimal[] Unrounded = new decimal[1000];

    [Test]
    public void X()
    {
        var rnd = new Random();
        for (var i = 0; i < 1000; i++)
        {
            decimal n = rnd.Next(0, 100) * rnd.Next(5, 10);
            var d = rnd.Next(3, 9) * rnd.Next(1, 3);
            var f = 0.1m;
            for (var p = 0; p < d; p++)
            {
                n += f * rnd.Next(0, 10);
                f *= 0.1m;
            }
            Unrounded[i] = n;
        }
    }

    [Test]
    public void To_even_by_default_for_positive()
    {
        var rounded = 24.5m.Round();
        rounded.Should().Be(24m);
    }

    [Test]
    public void To_even_by_default_for_negative()
    {
        var rounded = -24.5m.Round();
        rounded.Should().Be(-24m);
    }

    /// <remarks>Use strings as doubles lack precision.</remarks>
    [TestCase("123456789.1234567890", +10)]
    [TestCase("123456789.123456789", +9)]
    [TestCase("123456789.12345679", +8)]
    [TestCase("123456789.1234568", +7)]
    [TestCase("123456789.123457", +6)]
    [TestCase("123456789.12346", +5)]
    [TestCase("123456789.1235", +4)]
    [TestCase("123456789.123", +3)]
    [TestCase("123456789.12", +2)]
    [TestCase("123456789.1", +1)]
    [TestCase("123456789", +0)]
    [TestCase("123456790", -1)]
    [TestCase("123456800", -2)]
    [TestCase("123457000", -3)]
    [TestCase("123460000", -4)]
    [TestCase("123500000", -5)]
    [TestCase("123000000", -6)]
    [TestCase("120000000", -7)]
    [TestCase("100000000", -8)]
    [TestCase(0, -9)]
    public void Takes_digits_into_account(decimal exp, int digits)
    {
        var act = 123456789.123456789m.Round(digits, DecimalRounding.AwayFromZero);
        act.Should().Be(exp);
    }

    // Halfway/nearest rounding
    [TestCase(-26, -25.5, DecimalRounding.AwayFromZero)]
    [TestCase(+26, +25.5, DecimalRounding.AwayFromZero)]
    [TestCase(-25, -25.5, DecimalRounding.TowardsZero)]
    [TestCase(+25, +25.5, DecimalRounding.TowardsZero)]
    [TestCase(+26, +25.5, DecimalRounding.ToEven)]
    [TestCase(+24, +24.5, DecimalRounding.ToEven)]
    [TestCase(+25, +25.5, DecimalRounding.ToOdd)]
    [TestCase(+25, +24.5, DecimalRounding.ToOdd)]
    [TestCase(-25, -25.5, DecimalRounding.Up)]
    [TestCase(+26, +25.5, DecimalRounding.Up)]
    [TestCase(-26, -25.5, DecimalRounding.Down)]
    [TestCase(+25, +25.5, DecimalRounding.Down)]
    // Direct rounding
    [TestCase(-26, -25.1, DecimalRounding.DirectAwayFromZero)]
    [TestCase(+26, +25.1, DecimalRounding.DirectAwayFromZero)]
    [TestCase(-25, -25.1, DecimalRounding.DirectTowardsZero)]
    [TestCase(+25, +25.1, DecimalRounding.DirectTowardsZero)]
    [TestCase(-25, -25.1, DecimalRounding.Ceiling)]
    [TestCase(+26, +25.1, DecimalRounding.Ceiling)]
    [TestCase(-26, -25.1, DecimalRounding.Floor)]
    [TestCase(+25, +25.1, DecimalRounding.Floor)]
    [TestCase(-25, -25.1, DecimalRounding.Truncate)]
    [TestCase(+25, +25.1, DecimalRounding.Truncate)]
    public void Takes_strategy_into_account(decimal rounded, decimal value, DecimalRounding mode)
    {
        var act = value.Round(0, mode);
        act.Should().Be(rounded);
    }

    [TestCase(17.1)]
    [TestCase(17.3)]
    [TestCase(17.6)]
    [TestCase(17.8)]
    public void Stochastic_within_margin(decimal value)
    {
        var runs = 10_000;
        var sum = 0m;

        for (var i = 0; i < runs; i++)
        {
            var rounded = value.Round(0, DecimalRounding.StochasticRounding);
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
            var rounded = value.Round(0, DecimalRounding.RandomTieBreaking);
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
        var rounded = value.Round(-2);
        rounded.Should().Be(1000m);
    }

    [Test]
    public void Round_ALotOf3s_WithoutIssues()
    {
        var value = 9_876_543_210m + 1m / 3m;
        var rounded = value.Round(-9);
        var expected = 10_000_000_000m;

        rounded.Should().Be(expected);
    }
}

public class Rounds_to_multiple
{
    [Test]
    public void To_even_by_default_for_positive()
    {
        var rounded = 24.5m.RoundToMultiple(1m);
        rounded.Should().Be(24m);
    }

    [Test]
    public void To_even_by_default_for_negative()
    {
        var rounded = -26.5m.RoundToMultiple(1m);
        rounded.Should().Be(-26m);
    }

    [TestCase(150.0, 125.0, 50, DecimalRounding.AwayFromZero)]
    [TestCase(125.0, 123.0, 5, DecimalRounding.AwayFromZero)]
    [TestCase(123.25, 123.3085, 0.25, DecimalRounding.AwayFromZero)]
    [TestCase(666, 666, 3, DecimalRounding.AwayFromZero)]
    public void Supports_rouding_stratgies(decimal rounded, decimal value, decimal factor, DecimalRounding mode)
    {
        var act = value.RoundToMultiple(factor, mode);
        act.Should().Be(rounded);
    }
}
