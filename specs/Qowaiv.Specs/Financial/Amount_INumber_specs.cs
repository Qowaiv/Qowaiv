#if NET8_0_OR_GREATER

using Qowaiv.TestTools.Numerics;

namespace Financial.Amount_specs;

public class Defines
{
    [TestCase(-34, 30, -4)]
    [TestCase(+34, 30, +4)]
    [TestCase(+34, 02, +0)]
    public void modulo(Amount p, Amount mod, Amount remainder)
        => (p % mod).Should().Be(remainder);
}

public class Amount_as_INumber
{
    [Test]
    public void radixis_10()
        => Number.Radix<Amount>().Should().Be(10);

    [Test]
    public void additive_identityIs_1()
    => Number.AdditiveIdentity<Amount>().Should().Be(1.Amount());

    [Test]
    public void multiplicative_identityis_1()
        => Number.MultiplicativeIdentity<Amount>().Should().Be(1.Amount());

    [Test]
    public void is_canonical_equal_to_decimal([Random(Min, Max, Count)] decimal d)
    {
        Number.IsCanonical(d.Amount()).Should().Be(Number.IsCanonical(d));
    }

    [Test]
    public void abs_equal_to_Amount_Abs([Random(Min, Max, Count)] decimal d)
    {
        Number.Abs(d.Amount()).Should().Be(d.Amount().Abs());
    }

    [Test]
    public void is_never_a_complexnumber([Random(Min, Max, Count)] decimal d)
        => Number.IsComplexNumber(d.Amount()).Should().BeFalse();

    [TestCase(4, true)]
    [TestCase(9, true)]
    [TestCase(1, true)]
    [TestCase(-1, true)]
    [TestCase(-2, true)]
    [TestCase(3.3, false)]
    [TestCase(-4.4, false)]
    public void is_integer(Amount p, bool isEvenInteger)
        => Number.IsInteger(p).Should().Be(isEvenInteger);

    [TestCase(4, true)]
    [TestCase(8, true)]
    [TestCase(0, true)]
    [TestCase(-2, true)]
    [TestCase(-1, false)]
    [TestCase(2.2, false)]
    public void is_even_integer(Amount p, bool isEvenInteger)
        => Number.IsEvenInteger(p).Should().Be(isEvenInteger);

    [TestCase(5, true)]
    [TestCase(9, true)]
    [TestCase(1, true)]
    [TestCase(-1, true)]
    [TestCase(-2, false)]
    [TestCase(3.3, false)]
    public void is_odd_integer(Amount p, bool isEvenInteger)
        => Number.IsOddInteger(p).Should().Be(isEvenInteger);

    [Test]
    public void is_always_real([Random(Min, Max, Count)] decimal d)
       => Number.IsRealNumber(d.Amount()).Should().BeTrue();

    [Test]
    public void is_never_complex([Random(Min, Max, Count)] decimal d)
       => Number.IsComplexNumber(d.Amount()).Should().BeFalse();

    [Test]
    public void is_never_imaginary([Random(Min, Max, Count)] decimal d)
        => Number.IsImaginaryNumber(d.Amount()).Should().BeFalse();

    [Test]
    public void is_always_finite([Random(Min, Max, Count)] decimal d)
        => Number.IsFinite(d.Amount()).Should().BeTrue();

    [Test]
    public void is_never_infinite([Random(Min, Max, Count)] decimal d)
    {
        Number.IsInfinity(d.Amount()).Should().BeFalse();
        Number.IsNegativeInfinity(d.Amount()).Should().BeFalse();
        Number.IsPositiveInfinity(d.Amount()).Should().BeFalse();
    }

    [Test]
    public void is_never_NaN([Random(Min, Max, Count)] decimal d)
        => Number.IsNaN(d.Amount()).Should().BeFalse();

    [Test]
    public void is_negative_equal_to_decimal([Random(Min, Max, Count)] decimal d)
    {
        Number.IsNegative(d.Amount()).Should().Be(Number.IsNegative(d));
    }

    [Test]
    public void zero_is_positive_and_not_negative()
    {
        Number.IsPositive(Amount.Zero).Should().BeTrue();
        Number.IsNegative(Amount.Zero).Should().BeFalse();

        Number.IsPositive(0m).Should().BeTrue();
        Number.IsNegative(0m).Should().BeFalse();
    }

    [Test]
    public void is_zero_is_false_for_all_but_zero([Random(Min, Max, Count)] decimal d)
    {
        Number.IsZero(d.Amount()).Should().BeFalse();
    }

    [Test]
    public void is_positive_equal_to_decimal([Random(Min, Max, Count)] decimal d)
    {
        Number.IsPositive(d.Amount()).Should().Be(Number.IsPositive(d.Amount()));
    }

    [Test]
    public void is_not_Normal_when_zero()
        => Number.IsNormal(Amount.Zero).Should().BeFalse();

    [Test]
    public void Is_never_subnormal([Random(Min, Max, Count)] decimal d)
        => Number.IsSubnormal(d.Amount()).Should().BeFalse();

    [Test]
    public void is_normal_when_not_zero([Random(Min, Max, Count)] decimal d)
    {
        Number.IsNormal(d.Amount()).Should().BeTrue();
    }

    [Test]
    public void max_magnitude_equal_to_decimal([Random(Min, Max, 3)] decimal x, [Random(Min, Max, 3)] decimal y)
    {
        Number.MaxMagnitude(x.Amount(), y.Amount()).Should().Be(Number.MaxMagnitude(x, y).Amount());
        Number.MaxMagnitudeNumber(x.Amount(), y.Amount()).Should().Be(Number.MaxMagnitudeNumber(x, y).Amount());
    }

    [Test]
    public void min_maginiute_equal_to_decimal([Random(Min, Max, 3)] decimal x, [Random(Min, Max, 3)] decimal y)
    {
        Number.MinMagnitude(x.Amount(), y.Amount()).Should().Be(Number.MinMagnitude(x, y).Amount());
        Number.MinMagnitudeNumber(x.Amount(), y.Amount()).Should().Be((Amount)Number.MinMagnitudeNumber(x, y));
    }

    [Test]
    public void multiplication_is_supported_via_explicit_contract_only([Random(-100_000, +100_000, 3)] decimal x, [Random(-100_000, +100_000, 3)] decimal y)
    {
        Number.Multiply(x.Amount(), y.Amount()).Should().Be((x * y).Amount());
    }

    [Test]
    public void division_is_supported_via_explicit_contract_only([Random(Min, Max, 3)] decimal x, [Random(Min, Max, 3)] decimal y)
    {
        Number.Divide(x.Amount(), y.Amount()).Should().Be((x / y).Amount());
    }

    private const double Min = -79228162514264337593543950335d;
    private const double Max = +79228162514264337593543950335d;
    private const int Count = 8;
}
#endif
