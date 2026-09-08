namespace Qowaiv.UnitTests.Statistics;

/// <summary>Tests the Elo SVO.</summary>
public class EloTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly Elo TestStruct = 1732.4;

    #region Elo const tests

    /// <summary>Elo.Zero should be equal to the default of Elo.</summary>
    [Test]
    public void Zero_None_EqualsDefault() => Elo.Zero.Should().Be(default);
    [Test]
    public void MinValue_None_DoubleMinValue() => Elo.MinValue.Should().Be((Elo)double.MinValue);
    [Test]
    public void MaxValue_None_DoubleMaxValue() => Elo.MaxValue.Should().Be((Elo)double.MaxValue);

    #endregion

    #region IEquatable tests

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        var l = Elo.Parse("1600", CultureInfo.InvariantCulture);
        var r = Elo.Parse("1,600.00*", CultureInfo.InvariantCulture);

        l.Equals(r).Should().BeTrue();
    }

    #endregion

    #region Casting tests

    [Test]
    public void Implicit_DoubleToElo_AreEqual()
    {
        Elo exp = Elo.Create(1600.1);
        Elo act = 1600.1;

        act.Should().Be(exp);
    }
    [Test]
    public void Explicit_EloToDouble_AreEqual()
    {
        var exp = 1600.1;
        var act = (double)Elo.Create(1600.1);

        act.Should().Be(exp);
    }

    [Test]
    public void Implicit_DecimalToElo_AreEqual()
    {
        Elo exp = Elo.Create(1600.1);
        Elo act = 1600.1m;

        act.Should().Be(exp);
    }
    [Test]
    public void Explicit_EloToDecimal_AreEqual()
    {
        var exp = 1600.1m;
        var act = (decimal)Elo.Create(1600.1);

        act.Should().Be(exp);
    }


    [Test]
    public void Implicit_Int32ToElo_AreEqual()
    {
        Elo exp = Elo.Create(1600);
        Elo act = 1600;

        act.Should().Be(exp);
    }
    [Test]
    public void Explicit_EloToInt32_AreEqual()
    {
        var exp = 1600;
        var act = (int)Elo.Create(1600);

        act.Should().Be(exp);
    }

    #endregion

    #region Properties
    #endregion

    #region Operators

    [Test]
    public void Add_1600Add100_1700()
    {
        Elo l = 1600;
        Elo r = 100;

        Elo act = l + r;
        Elo exp = 1700;

        act.Should().Be(exp);
    }

    [Test]
    public void Subtract_1600Subtract100_1500()
    {
        Elo l = 1600;
        Elo r = 100;

        Elo act = l - r;
        Elo exp = 1500;

        act.Should().Be(exp);
    }

    [Test]
    public void Divide_1600Subtract100_800()
    {
        Elo act = 1600m;

        act /= 2.0;

        Elo exp = 800;

        act.Should().Be(exp);
    }

    [Test]
    public void Multiply_3200()
    {
        Elo act = 1600m;

        act *= 2.0;

        Elo exp = 3200;

        act.Should().Be(exp);
    }

    [Test]
    public void Increment_1600_1601()
    {
        Elo act = 1600;
        act++;

        Elo exp = 1601;

        act.Should().Be(exp);
    }

    [Test]
    public void Decrement_1600_1599()
    {
        Elo act = 1600;
        act--;

        Elo exp = 1599;

        act.Should().Be(exp);
    }

    [Test]
    public void Negate_Min1600_1600()
    {
        Elo act = -1600;

        act = -act;

        Elo exp = 1600;

        act.Should().Be(exp);
    }

    [Test]
    public void Plus_1600_1600()
    {
        Elo act = 1600;

        act = +act;

        Elo exp = 1600;

        act.Should().Be(exp);
    }
    #endregion
}
