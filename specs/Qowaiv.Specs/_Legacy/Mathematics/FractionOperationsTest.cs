namespace Qowaiv.UnitTests.Mathematics;

public class FractionOperationsTest
{
    [TestCase("0", 0)]
    [TestCase("-1/4", -1)]
    [TestCase("+1/4", +1)]
    public void Sign(Fraction fraction, int sign) => fraction.Sign().Should().Be(sign);

    [TestCase("0", "0")]
    [TestCase("17/3", "17/3")]
    [TestCase("-7/3", "+7/3")]
    public void Abs(Fraction fraction, Fraction absolute) => fraction.Abs().Should().Be(absolute);

    [TestCase("0", "0")]
    [TestCase("17/3", "17/3")]
    [TestCase("-7/3", "-7/3")]
    public void Plus(Fraction fraction, Fraction expected)
    {
        Assert.AreEqual(expected, +fraction);
    }

    [TestCase("0", "0")]
    [TestCase("-7/3", "+7/3")]
    [TestCase("+7/3", "-7/3")]
    public void Negate(Fraction fraction, Fraction expected)
    {
        Assert.AreEqual(expected, -fraction);
    }

    [Test]
    public void Inverse_Zero_Throws()
    {
        Assert.Throws<DivideByZeroException>(() => Fraction.Zero.Inverse());
    }

    [TestCase("+1/4", "4/1")]
    [TestCase("-2/3", "-3/2")]
    public void Inverse(Fraction faction, Fraction expected)
    {
        faction.Inverse().Should().Be(expected);
    }

    [Test]
    public void Multiply_long()
    {
        var fraction = 19.DividedBy(72);
        var actual = fraction * 48L;
        var expected = 38.DividedBy(3);
        actual.Should().Be(expected);
    }

    [Test]
    public void Multiply_int()
    {
        var fraction = 19.DividedBy(72);
        var actual = fraction * 48;
        var expected = 38.DividedBy(3);
        actual.Should().Be(expected);
    }

    [TestCase("1/3", "1/4", "1/12")]
    [TestCase("-1/3", "-1/4", "1/12")]
    [TestCase("1/4", "4/7", "1/7")]
    [TestCase("2/5", "11/16", "11/40")]
    [TestCase("-2/5", "4", "-8/5")]
    [TestCase("2/3", "-8/9", "-16/27")]
    public void Multiply(Fraction left, Fraction right, Fraction expected)
    {
        Assert.AreEqual(expected, left * right);
    }

    [Test]
    public void Divide_long()
    {
        var fraction = 19.DividedBy(72);
        var actual = fraction / 48L;
        var expected = 19.DividedBy(3456);
        actual.Should().Be(expected);
    }

    [Test]
    public void Divide_int()
    {
        var fraction = 19.DividedBy(72);
        var actual = fraction / 48;
        var expected = 19.DividedBy(3456);
        actual.Should().Be(expected);
    }

    [TestCase("1/3", "1/4", "4/3")]
    [TestCase("-1/3", "-1/4", "4/3")]
    [TestCase("1/4", "4/7", "7/16")]
    [TestCase("2/5", "11/16", "32/55")]
    [TestCase("-2/5", "4", "-1/10")]
    [TestCase("2/3", "-8/9", "-3/4")]
    public void Divide(Fraction left, Fraction right, Fraction expected)
    {
        Assert.AreEqual(expected, left / right);
    }

    [Test]
    public void Add_WithPotentialOverflow_ShouldNotOverflow()
    {
        var l = 1.DividedBy(4_000_000_000L);
        var r = 1.DividedBy(8_000_000_000L);

        var actual = l + r;
        var expected = 3.DividedBy(8_000_000_000L);

        actual.Should().Be(expected);
    }

    [Test]
    public void Add_FractionPlusLong()
    {
        var fraction = 1.DividedBy(3);
        var actual = fraction + 2L;
        var expected = 7.DividedBy(3);
        actual.Should().Be(expected);
    }

    [Test]
    public void Add_LongPlusFraction()
    {
        var fraction = 1.DividedBy(3);
        var actual = 2L + fraction;
        var expected = 7.DividedBy(3);
        actual.Should().Be(expected);
    }

    [Test]
    public void Add_FractionPlusInt()
    {
        var fraction = 1.DividedBy(3);
        var actual = fraction + 2;
        var expected = 7.DividedBy(3);
        actual.Should().Be(expected);
    }

    [Test]
    public void Add_IntPlusFraction()
    {
        var fraction = 1.DividedBy(3);
        var actual = 2 + fraction;
        var expected = 7.DividedBy(3);
        actual.Should().Be(expected);
    }

    [TestCase("1/4", "0", "1/4")]
    [TestCase("0", "1/4", "1/4")]
    [TestCase("1/3", "1/4", "7/12")]
    [TestCase("1/4", "1/3", "7/12")]
    [TestCase("1/4", "1/12", "1/3")]
    [TestCase("-1/4", "-1/12", "-1/3")]
    [TestCase("-1/4", "1/12", "-1/6")]
    [TestCase("1/5", "2/5", "3/5")]
    public void Add(Fraction left, Fraction right, Fraction expected)
    {
        Assert.AreEqual(expected, left + right);
    }

    [Test]
    public void Subtract_FractionMinLong()
    {
        var fraction = 1.DividedBy(3);
        var actual = fraction - 2L;
        var expected = -5.DividedBy(3);
        actual.Should().Be(expected);
    }

    [Test]
    public void Subtract_LongMinFraction()
    {
        var fraction = 1.DividedBy(3);
        var actual = 2L - fraction;
        var expected = 5.DividedBy(3);
        actual.Should().Be(expected);
    }

    [Test]
    public void Subtract_FractionMinInt()
    {
        var fraction = 1.DividedBy(3);
        var actual = fraction - 2;
        var expected = -5.DividedBy(3);
        actual.Should().Be(expected);
    }

    [Test]
    public void Subtract_IntMinFraction()
    {
        var fraction = 1.DividedBy(3);
        var actual = 2 - fraction;
        var expected = 5.DividedBy(3);
        actual.Should().Be(expected);
    }

    [TestCase("1/4", "0", "1/4")]
    [TestCase("0", "1/4", "-1/4")]
    [TestCase("1/3", "1/4", "1/12")]
    [TestCase("1/4", "1/3", "-1/12")]
    [TestCase("1/4", "1/12", "1/6")]
    [TestCase("-1/4", "-1/12", "-1/6")]
    [TestCase("-1/4", "1/12", "-1/3")]
    public void Subtract(Fraction left, Fraction right, Fraction expected)
    {
        Assert.AreEqual(expected, left - right);
    }
}
