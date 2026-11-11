#if NET10_0_OR_GREATER

using Qowaiv.TestTools.Numerics;

namespace Percentage_specs;

public class Defines
{
    [Test]
    [Arguments("-34%", "30%", "-4%")]
    [Arguments("34%", "30%", "4%")]
    [Arguments("34%", "2%", "0%")]
    public void modulo(Percentage p, Percentage mod, Percentage remainder)
        => (p % mod).Should().Be(remainder);
}

public class Percentage_as_INumber
{
    [Test]
    public void radix_is_10()
        => Number.Radix<Percentage>().Should().Be(10);

    [Test]
    public void Additive_identity_Is_1_percent()
    => Number.AdditiveIdentity<Percentage>().Should().Be(1m.Percent());

    [Test]
    public void Multiplicative_identity_is_100_percent()
        => Number.MultiplicativeIdentity<Percentage>().Should().Be(100m.Percent());

    [Test]
    [CombinedDataSources]
    public void Is_canonical_equal_to_decimal([Random(Min, Max, Count)] double d)
    {
        var n = d.ToDecimal();
        Number.IsCanonical((Percentage)n).Should().Be(Number.IsCanonical(n));
    }

    [Test]
    [CombinedDataSources]
    public void Abs_equal_to_percentage_Abs([Random(Min, Max, Count)] double d)
    {
        var p = (Percentage)d;
        Number.Abs(p).Should().Be(p.Abs());
    }

    [Test]
    [CombinedDataSources]
    public void is_never_a_complex_number([Random(Min, Max, Count)] double d)
        => Number.IsComplexNumber((Percentage)d).Should().BeFalse();

    [Test]
    [Arguments("4%", true)]
    [Arguments("9%", true)]
    [Arguments("1%", true)]
    [Arguments("-1%", true)]
    [Arguments("-2%", true)]
    [Arguments("3.3%", false)]
    [Arguments("-4.4%", false)]
    public void is_integer(Percentage p, bool isEvenInteger)
        => Number.IsInteger(p).Should().Be(isEvenInteger);

    [Test]
    [Arguments("4%", true)]
    [Arguments("8%", true)]
    [Arguments("0%", true)]
    [Arguments("-2%", true)]
    [Arguments("-1%", false)]
    [Arguments("2.2%", false)]
    public void is_even_integer(Percentage p, bool isEvenInteger)
        => Number.IsEvenInteger(p).Should().Be(isEvenInteger);

    [Test]
    [Arguments("5%", true)]
    [Arguments("9%", true)]
    [Arguments("1%", true)]
    [Arguments("-1%", true)]
    [Arguments("-2%", false)]
    [Arguments("3.3%", false)]
    public void is_odd_integer(Percentage p, bool isEvenInteger)
        => Number.IsOddInteger(p).Should().Be(isEvenInteger);

    [Test]
    [CombinedDataSources]
    public void is_always_real([Random(Min, Max, Count)] double d)
       => Number.IsRealNumber((Percentage)d).Should().BeTrue();

    [Test]
    [CombinedDataSources]
    public void is_never_complex([Random(Min, Max, Count)] double d)
       => Number.IsComplexNumber((Percentage)d).Should().BeFalse();

    [Test]
    [CombinedDataSources]
    public void is_never_imaginary([Random(Min, Max, Count)] double d)
        => Number.IsImaginaryNumber((Percentage)d).Should().BeFalse();

    [Test]
    [CombinedDataSources]
    public void is_always_finite([Random(Min, Max, Count)] double d)
        => Number.IsFinite((Percentage)d).Should().BeTrue();

    [Test]
    [CombinedDataSources]
    public void is_never_infinite([Random(Min, Max, Count)] double d)
    {
        Number.IsInfinity((Percentage)d).Should().BeFalse();
        Number.IsNegativeInfinity((Percentage)d).Should().BeFalse();
        Number.IsPositiveInfinity((Percentage)d).Should().BeFalse();
    }

    [Test]
    [CombinedDataSources]
    public void is_never_NaN([Random(Min, Max, Count)] double d)
        => Number.IsNaN((Percentage)d).Should().BeFalse();

    [Test]
    [CombinedDataSources]
    public void is_negative_equal_to_decimal([Random(Min, Max, Count)] double d)
    {
        var n = d.ToDecimal();
        Number.IsNegative((Percentage)n).Should().Be(Number.IsNegative(n));
    }

    [Test]
    public void zero_is_positive_and_not_negative()
    {
        Number.IsPositive(Percentage.Zero).Should().BeTrue();
        Number.IsNegative(Percentage.Zero).Should().BeFalse();

        Number.IsPositive(0m).Should().BeTrue();
        Number.IsNegative(0m).Should().BeFalse();
    }

    [Test]
    [CombinedDataSources]
    public void is_zero_is_false_for_all_but_zero([Random(Min, Max, Count)] double d)
    {
        var n = d.ToDecimal();
        Number.IsZero((Percentage)n).Should().BeFalse();
    }

    [Test]
    [CombinedDataSources]
    public void is_positive_equal_to_decimal([Random(Min, Max, Count)] double d)
    {
        var n = d.ToDecimal();
        Number.IsPositive((Percentage)n).Should().Be(Number.IsPositive(n));
    }

    [Test]
    public void Is_not_Normal_when_zero()
        => Number.IsNormal(Percentage.Zero).Should().BeFalse();

    [Test]
    [CombinedDataSources]
    public void Is_never_subnormal([Random(Min, Max, Count)] double d)
        => Number.IsSubnormal((Percentage)d).Should().BeFalse();

    [Test]
    [CombinedDataSources]
    public void Is_normal_when_not_zero([Random(Min, Max, Count)] double d)
    {
        var n = d.ToDecimal();
        Number.IsNormal((Percentage)n).Should().BeTrue();
    }

    [Test]
    [CombinedDataSources]
    public void max_maginiute_equal_to_decimal([Random(Min, Max, 3)] double x, [Random(Min, Max, 3)] double y)
    {
        var x_ = x.ToDecimal();
        var y_ = y.ToDecimal();

        Number.MaxMagnitude((Percentage)x_, (Percentage)y_).Should().Be((Percentage)Number.MaxMagnitude(x_, y_));
        Number.MaxMagnitudeNumber((Percentage)x_, (Percentage)y_).Should().Be((Percentage)Number.MaxMagnitudeNumber(x_, y_));
    }

    [Test]
    [CombinedDataSources]
    public void min_maginiute_equal_to_decimal([Random(Min, Max, 3)] double x, [Random(Min, Max, 3)] double y)
    {
        var x_ = x.ToDecimal();
        var y_ = y.ToDecimal();

        Number.MinMagnitude((Percentage)x_, (Percentage)y_).Should().Be((Percentage)Number.MinMagnitude(x_, y_));
        Number.MinMagnitudeNumber((Percentage)x_, (Percentage)y_).Should().Be((Percentage)Number.MinMagnitudeNumber(x_, y_));
    }

    private const double Min = -7922816251426433759354395.0335d;
    private const double Max = +7922816251426433759354395.0335d;
    private const int Count = 8;
}
#endif
