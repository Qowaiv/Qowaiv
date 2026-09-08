namespace Qowaiv.UnitTests.Financial;

/// <summary>Tests the Money SVO.</summary>
[TestFixture]
public class MoneyTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly Money TestStruct = 42.17 + Currency.EUR;

    #region Money const tests

    /// <summary>Money.Empty should be equal to the default of Money.</summary>
    [Test]
    public void Zero_None_EqualsDefault() => Money.Zero.Should().Be(default);

    #endregion

    #region Properties

    [Test]
    public void Currency_Set()
    {
        var money = 42 + Currency.EUR;
        money.Currency.Should().Be(Currency.EUR);
    }

    [Test]
    public void Amount_Set()
    {
        var money = 42 + Currency.EUR;
        money.Amount.Should().Be((Amount)42);
    }

    #endregion

    #region Operations

    [TestCase(-1, "EUR -1000")]
    [TestCase(0, "EUR 0")]
    [TestCase(+1, "EUR 100")]
    public void Sign(int expected, Money value)
    {
        var actual = value.Sign();
        actual.Should().Be(expected);
    }

    [TestCase(23.1234, -23.1234)]
    [TestCase(23.1234, +23.1234)]
    public void Abs(decimal expected, decimal value)
    {
        var money = value + Currency.USD;
        var abs = money.Abs();
        Should.BeEqual(expected + Currency.USD, abs);
    }

    [Test]
    public void Plus_TestStruct_SameValue()
    {
        var act = +TestStruct;
        act.Should().Be(TestStruct);
    }

    [Test]
    public void Negates_TestStruct_NegativeValue()
    {
        var act = -TestStruct;
        var exp = -42.17 + Currency.EUR;
        act.Should().Be(exp);
    }

    [Test]
    public void Increase_TestStruct_Plus1()
    {
        var act = TestStruct;
        act++;
        var exp = 43.17 + Currency.EUR;
        act.Should().Be(exp);
    }

    [Test]
    public void Decrease_TestStruct_Minus1()
    {
        var act = TestStruct;
        act--;
        var exp = 41.17 + Currency.EUR;
        act.Should().Be(exp);
    }

    [Test]
    public void Add_SameCurrency_Added()
    {
        var l = 16 + Currency.EUR;
        var r = 26 + Currency.EUR;
        var a = 42 + Currency.EUR;

        Should.BeEqual(a, l + r);
    }

    [Test]
    public void Add_25Percent_Added()
    {
        var l = 16 + Currency.EUR;
        var p = 25.Percent();
        var a = 20 + Currency.EUR;

        Should.BeEqual(a, l + p);
    }

    [Test]
    public void Subtract_SameCurrency_Subtracted()
    {
        var l = 69 + Currency.EUR;
        var r = 27 + Currency.EUR;
        var a = 42 + Currency.EUR;

        Should.BeEqual(a, l - r);
    }

    [Test]
    public void Subtract_25Percent_Subtracted()
    {
        var l = 16 + Currency.EUR;
        var p = 25.Percent();
        var a = 12 + Currency.EUR;

        Should.BeEqual(a, l - p);
    }

    [Test]
    public void Multiply_Percentage()
    {
        var money = 100.40m + Currency.USD;
        var p = 50.Percent();
        var expected = 50.20m + Currency.USD;
        Should.BeEqual(expected, money * p);
    }

    [Test]
    public void Multiply_Float()
    {
        var money = 100.40m + Currency.USD;
        float p = 0.5F;
        var expected = 50.20m + Currency.USD;
        Should.BeEqual(expected, money * p);
    }

    [Test]
    public void Multiply_Double()
    {
        var money = 100.40m + Currency.USD;
        double p = 0.5;
        var expected = 50.20m + Currency.USD;
        Should.BeEqual(expected, money * p);
    }

    [Test]
    public void Multiply_Decimal()
    {
        var money = 100.40m + Currency.USD;
        var p = 0.5m;
        var expected = 50.20m + Currency.USD;
        Should.BeEqual(expected, money * p);
    }

    [Test]
    public void Multiply_Short()
    {
        var money = 100.40m + Currency.USD;
        short f = 2;
        var expected = 200.8m + Currency.USD;
        Should.BeEqual(expected, money * f);
    }

    [Test]
    public void Multiply_Int()
    {
        var money = 100.40m + Currency.USD;
        int f = 2;
        var expected = 200.8m + Currency.USD;
        Should.BeEqual(expected, money * f);
    }

    [Test]
    public void Multiply_Long()
    {
        var money = 100.40m + Currency.USD;
        long f = 2;
        var expected = 200.8m + Currency.USD;
        Should.BeEqual(expected, money * f);
    }

    [Test]
    public void Multiply_UShort()
    {
        var money = 100.40m + Currency.USD;
        ushort f = 2;
        var expected = 200.8m + Currency.USD;
        Should.BeEqual(expected, money * f);
    }

    [Test]
    public void Multiply_UInt()
    {
        var money = 100.40m + Currency.USD;
        uint f = 2;
        var expected = 200.8m + Currency.USD;
        Should.BeEqual(expected, money * f);
    }

    [Test]
    public void Multiply_ULong()
    {
        var money = 100.40m + Currency.USD;
        ulong f = 2;
        var expected = 200.8m + Currency.USD;
        Should.BeEqual(expected, money * f);
    }

    [Test]
    public void Divide_Percentage()
    {
        var money = 100.40m + Currency.USD;
        var p = 50.Percent();
        var expected = 200.8m + Currency.USD;
        Should.BeEqual(expected, money / p);
    }

    [Test]
    public void Divide_Float()
    {
        var money = 100.40m + Currency.USD;
        float p = 0.5F;
        var expected = 200.8m + Currency.USD;
        Should.BeEqual(expected, money / p);
    }

    [Test]
    public void Divide_Double()
    {
        var money = 100.40m + Currency.USD;
        double p = 0.5;
        var expected = 200.8m + Currency.USD;
        Should.BeEqual(expected, money / p);
    }

    [Test]
    public void Divide_Decimal()
    {
        var money = 100.40m + Currency.USD;
        var p = 0.5m;
        var expected = 200.8m + Currency.USD;
        Should.BeEqual(expected, money / p);
    }

    [Test]
    public void Divide_Short()
    {
        var money = 100.40m + Currency.USD;
        short f = 2;
        var expected = 50.20m + Currency.USD;
        Should.BeEqual(expected, money / f);
    }

    [Test]
    public void Divide_Int()
    {
        var money = 100.40m + Currency.USD;
        int f = 2;
        var expected = 50.20m + Currency.USD;
        Should.BeEqual(expected, money / f);
    }

    [Test]
    public void Divide_Long()
    {
        var money = 100.40m + Currency.USD;
        long f = 2;
        var expected = 50.20m + Currency.USD;
        Should.BeEqual(expected, money / f);
    }

    [Test]
    public void Divide_UShort()
    {
        var money = 100.40m + Currency.USD;
        ushort f = 2;
        var expected = 50.20m + Currency.USD;
        Should.BeEqual(expected, money / f);
    }

    [Test]
    public void Divide_UInt()
    {
        var money = 100.40m + Currency.USD;
        uint f = 2;
        var expected = 50.20m + Currency.USD;
        Should.BeEqual(expected, money / f);
    }

    [Test]
    public void Divide_ULong()
    {
        var money = 100.40m + Currency.USD;
        ulong f = 2;
        var expected = 50.20m + Currency.USD;
        Should.BeEqual(expected, money / f);
    }

    [Test]
    public void Round_EUR_2Digits()
    {
        var money = 123.4567m + Currency.EUR;
        var rounded = money.Round();
        Should.BeEqual(123.46m + Currency.EUR, rounded);
    }

    [Test]
    public void Round_1Digit()
    {
        var money = 123.4567m + Currency.EUR;
        var rounded = money.Round(1);
        Should.BeEqual(123.5m + Currency.EUR, rounded);
    }

    [Test]
    public void RoundToMultiple_0d25()
    {
        var money = 123.6567m + Currency.EUR;
        var rounded = money.RoundToMultiple(0.25m);
        Should.BeEqual(123.75m + Currency.EUR, rounded);
    }

    #endregion
}
