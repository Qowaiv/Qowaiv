#if NET8_0_OR_GREATER

using Qowaiv.Specs.TestTools;
using Qowaiv.TestTools.Numerics;

namespace Mathematics.Fraction_specs;

public class Fraction_as_INumber
{
    [Test]
    public void radixis_10()
        => Number.Radix<Fraction>().Should().Be(10);

    [Test]
    public void Additive_identityIs_1()
    => Number.AdditiveIdentity<Fraction>().Should().Be(Fraction.One);

    [Test]
    public void Multiplicative_identityis_1()
        => Number.MultiplicativeIdentity<Fraction>().Should().Be(Fraction.One);

    [Test]
    public void Is_always_canonical([RandomFraction(Count)] Fraction fraction)
        => Number.IsCanonical(fraction).Should().BeTrue();

    [Test]
    public void Abs_equal_to_Fraction_Abs([RandomFraction(Count)] Fraction fraction)
        => Number.Abs(fraction).Should().Be(Number.Abs((decimal)fraction).Fraction());

    [Test]
    public void is_never_a_complexnumber([RandomFraction(Count)] Fraction fraction)
        => Number.IsComplexNumber(fraction).Should().BeFalse();

    [TestCase(4, true)]
    [TestCase(9, true)]
    [TestCase(1, true)]
    [TestCase(-1, true)]
    [TestCase(-2, true)]
    [TestCase("3/5", false)]
    [TestCase("-3/4", false)]
    public void is_integer(Fraction fraction, bool isEvenInteger)
        => Number.IsInteger(fraction).Should().Be(isEvenInteger);

    [TestCase(4, true)]
    [TestCase(8, true)]
    [TestCase(0, true)]
    [TestCase(-2, true)]
    [TestCase(-1, false)]
    [TestCase("3/5", false)]
    [TestCase("-3/4", false)]
    public void is_even_integer(Fraction fraction, bool isEvenInteger)
        => Number.IsEvenInteger(fraction).Should().Be(isEvenInteger);

    [TestCase(5, true)]
    [TestCase(9, true)]
    [TestCase(1, true)]
    [TestCase(-1, true)]
    [TestCase(-2, false)]
    [TestCase("3/5", false)]
    [TestCase("-3/4", false)]
    public void is_odd_integer(Fraction p, bool isEvenInteger)
        => Number.IsOddInteger(p).Should().Be(isEvenInteger);

    [Test]
    public void is_always_real([RandomFraction(Count)] Fraction fraction)
       => Number.IsRealNumber(fraction).Should().BeTrue();

    [Test]
    public void is_never_complex([RandomFraction(Count)] Fraction fraction)
       => Number.IsComplexNumber(fraction).Should().BeFalse();

    [Test]
    public void is_never_imaginary([RandomFraction(Count)] Fraction fraction)
        => Number.IsImaginaryNumber(fraction).Should().BeFalse();

    [Test]
    public void is_always_finite([RandomFraction(Count)] Fraction fraction)
        => Number.IsFinite(fraction).Should().BeTrue();

    [Test]
    public void is_never_infinite([RandomFraction(Count)] Fraction fraction)
    {
        Number.IsInfinity(fraction).Should().BeFalse();
        Number.IsNegativeInfinity(fraction).Should().BeFalse();
        Number.IsPositiveInfinity(fraction).Should().BeFalse();
    }

    [Test]
    public void is_never_NaN([RandomFraction(Count)] Fraction fraction)
        => Number.IsNaN(fraction).Should().BeFalse();

    [Test]
    public void is_negative_equal_to_decimal([RandomFraction(Count)] Fraction fraction)
        => Number.IsNegative(fraction).Should().Be(Number.IsNegative((decimal)fraction));

    [Test]
    public void zero_is_positive_and_not_negative()
    {
        Number.IsPositive(Fraction.Zero).Should().BeTrue();
        Number.IsNegative(Fraction.Zero).Should().BeFalse();

        Number.IsPositive(0m).Should().BeTrue();
        Number.IsNegative(0m).Should().BeFalse();
    }

    [Test]
    public void is_zero_is_false_for_all_but_zero([RandomFraction(Count)] Fraction fraction)
        => Number.IsZero(fraction).Should().BeFalse();

    [Test]
    public void is_positive_equal_to_decimal([RandomFraction(Count)] Fraction fraction)
        => Number.IsPositive(fraction).Should().Be(Number.IsPositive((decimal)fraction));

    [Test]
    public void Is_always_normal([RandomFraction(Count)] Fraction fraction)
        => Number.IsNormal(fraction).Should().BeTrue();

    [Test]
    public void Is_never_subnormal([RandomFraction(Count)] Fraction fraction)
        => Number.IsSubnormal(fraction).Should().BeFalse();

    [Test]
    public void max_magnitude_equal_to_decimal([RandomFraction(3)] Fraction x, [RandomFraction(3)] Fraction y)
    {
        var x_ = (decimal)x.Numerator / x.Denominator;
        var y_ = (decimal)y.Numerator / y.Denominator;

        Number.MaxMagnitude(x, y).Should().Be(Number.MaxMagnitude(x_, y_).Fraction());
        Number.MaxMagnitudeNumber(x, y).Should().Be(Number.MaxMagnitudeNumber(x_, y_).Fraction());
    }

    [Test]
    public void min_maginiute_equal_to_decimal([RandomFraction(3)] Fraction x, [RandomFraction(3)] Fraction y)
    {
        var x_ = (decimal)x.Numerator / x.Denominator;
        var y_ = (decimal)y.Numerator / y.Denominator;

        Number.MinMagnitude(x, y).Should().Be(Number.MinMagnitude(x_, y_).Fraction());
        Number.MinMagnitudeNumber(x, y).Should().Be(Number.MinMagnitudeNumber(x_, y_).Fraction());
    }

    private const int Count = 8;
}
#endif
