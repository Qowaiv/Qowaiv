namespace Qowaiv.Financial.UnitTests;

/// <summary>Tests the Amount SVO.</summary>
public class AmountTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly Amount TestStruct = (Amount)42.17m;

    #region Amount const tests

    /// <summary>Amount.Zero should be equal to the default of Amount.</summary>
    [Test]
    public void Zero_None_EqualsDefault() => Amount.Zero.Should().Be(default);

    #endregion

    #region IEquatable tests

    /// <summary>GetHash should not fail for Amount.Zero.</summary>
    [Test]
    public void GetHash_Zero_Hash() => Amount.Zero.GetHashCode().Should().Be(0);

    /// <summary>GetHash should not fail for the test struct.</summary>
    [Test]
    public void GetHash_SameValue_SameHash()
    {
        var hash0 = ((Amount)451).GetHashCode();
        var hash1 = ((Amount)451).GetHashCode();
        hash0.Should().Be(hash1);
    }

    [Test]
    public void Equals_EmptyEmpty_IsTrue() => Amount.Zero.Equals(Amount.Zero).Should().BeTrue();

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        using (TestCultures.en_US.Scoped())
        {
            var l = Amount.Parse("$ 1,451.070");
            var r = Amount.Parse("1451.07");

            l.Equals(r).Should().BeTrue();
        }
    }

    [Test]
    public void Equals_TestStructTestStruct_IsTrue() => TestStruct.Equals(TestStruct).Should().BeTrue();

    [Test]
    public void Equals_TestStructEmpty_IsFalse() => TestStruct.Equals(Amount.Zero).Should().BeFalse();

    [Test]
    public void Equals_EmptyTestStruct_IsFalse() => Amount.Zero.Equals(TestStruct).Should().BeFalse();

    [Test]
    public void Equals_TestStructObjectTestStruct_IsTrue() => TestStruct.Equals((object)TestStruct).Should().BeTrue();

    [Test]
    public void Equals_TestStructNull_IsFalse() => TestStruct.Equals(null).Should().BeFalse();

    [Test]
    public void Equals_TestStructObject_IsFalse() => TestStruct.Equals(new object()).Should().BeFalse();

    [Test]
    public void OperatorIs_TestStructTestStruct_IsTrue()
    {
        var l = TestStruct;
        var r = TestStruct;
        (l == r).Should().BeTrue();
    }

    [Test]
    public void OperatorIsNot_TestStructTestStruct_IsFalse()
    {
        var l = TestStruct;
        var r = TestStruct;
        (l != r).Should().BeFalse();
    }

    #endregion

    [TestCase(-1, -1000)]
    [TestCase(0, 0)]
    [TestCase(+1, 1600)]
    public void Sign(int expected, Amount value)
    {
        var actual = value.Sign();
        actual.Should().Be(expected);
    }

    [TestCase(1234.01, -1234.01)]
    [TestCase(1234.01, +1234.01)]
    public void Abs(Amount expected, Amount value)
    {
        var abs = value.Abs();
        abs.Should().Be(expected);
    }

    [TestCase(-1234.01)]
    [TestCase(+1234.01)]
    public void Plus(Amount expected)
    {
        var plus = +expected;
        plus.Should().Be(expected);
    }

    [TestCase(+1234.01, -1234.01)]
    [TestCase(-1234.01, +1234.01)]
    public void Negate(Amount expected, Amount value)
    {
        var negated = -value;
        negated.Should().Be(expected);
    }

    [Test]
    public void Decrement_EqualsTestStruct()
    {
        Amount amount = (Amount)43.17;
        amount--;
        amount.Should().Be(TestStruct);
    }

    [Test]
    public void Increment_EqualsTestStruct()
    {
        Amount amount = (Amount)41.17;
        amount++;
        amount.Should().Be(TestStruct);
    }

    [Test]
    public void Add_SomeAmount_Added()
    {
        Amount amount = (Amount)40.10;
        Amount other = (Amount)2.07;
        Should.BeEqual(TestStruct, amount + other);
    }

    [Test]
    public void Add_SomePercentage_Added()
    {
        Amount amount = (Amount)40.00;
        var p = 10.Percent();
        Should.BeEqual((Amount)44.00, amount + p);
    }

    [Test]
    public void Subtract_SomeAmount_Subtracted()
    {
        Amount amount = (Amount)43.20;
        Amount other = (Amount)1.03;
        Should.BeEqual(TestStruct, amount - other);
    }

    [Test]
    public void Subtract_SomePercentage_Subtracted()
    {
        Amount amount = (Amount)40.00;
        var p = 25.Percent();
        Should.BeEqual((Amount)30.00, amount - p);
    }

    [Test]
    public void Multiply_Percentage()
    {
        Amount amount = (Amount)100.40m;
        var p = 50.Percent();
        Amount expected = (Amount)50.20m;
        Should.BeEqual(expected, amount * p);
    }

    [Test]
    public void Multiply_Float()
    {
        Amount amount = (Amount)100.40m;
        float p = 0.5F;
        Amount expected = (Amount)50.20m;
        Should.BeEqual(expected, amount * p);
    }

    [Test]
    public void Multiply_Double()
    {
        Amount amount = (Amount)100.40m;
        double p = 0.5;
        Amount expected = (Amount)50.20m;
        Should.BeEqual(expected, amount * p);
    }

    [Test]
    public void Multiply_Decimal()
    {
        Amount amount = (Amount)100.40m;
        var p = 0.5m;
        Amount expected = (Amount)50.20m;
        Should.BeEqual(expected, amount * p);
    }

    [Test]
    public void Multiply_Short()
    {
        Amount amount = (Amount)100.40m;
        short f = 2;
        Amount expected = (Amount)200.80m;
        Should.BeEqual(expected, amount * f);
    }

    [Test]
    public void Multiply_Int()
    {
        Amount amount = (Amount)100.40m;
        int f = 2;
        Amount expected = (Amount)200.80m;
        Should.BeEqual(expected, amount * f);
    }

    [Test]
    public void Multiply_Long()
    {
        Amount amount = (Amount)100.40m;
        long f = 2;
        Amount expected = (Amount)200.80m;
        Should.BeEqual(expected, amount * f);
    }

    [Test]
    public void Multiply_UShort()
    {
        Amount amount = (Amount)100.40m;
        ushort f = 2;
        Amount expected = (Amount)200.80m;
        Should.BeEqual(expected, amount * f);
    }

    [Test]
    public void Multiply_UInt()
    {
        Amount amount = (Amount)100.40m;
        uint f = 2;
        Amount expected = (Amount)200.80m;
        Should.BeEqual(expected, amount * f);
    }

    [Test]
    public void Multiply_ULong()
    {
        Amount amount = (Amount)100.40m;
        ulong f = 2;
        Amount expected = (Amount)200.80m;
        Should.BeEqual(expected, amount * f);
    }

    [Test]
    public void Divide_Percentage()
    {
        Amount amount = (Amount)100.40m;
        var p = 50.Percent();
        Amount expected = (Amount)200.80m;
        Should.BeEqual(expected, amount / p);
    }

    [Test]
    public void Divide_Float()
    {
        Amount amount = (Amount)100.40m;
        float p = 0.5F;
        Amount expected = (Amount)200.80m;
        Should.BeEqual(expected, amount / p);
    }

    [Test]
    public void Divide_Double()
    {
        Amount amount = (Amount)100.40m;
        double p = 0.5;
        Amount expected = (Amount)200.80m;
        Should.BeEqual(expected, amount / p);
    }

    [Test]
    public void Divide_Decimal()
    {
        Amount amount = (Amount)100.40m;
        var p = 0.5m;
        Amount expected = (Amount)200.80m;
        Should.BeEqual(expected, amount / p);
    }

    [Test]
    public void Divide_Short()
    {
        Amount amount = (Amount)100.40m;
        short f = 2;
        Amount expected = (Amount)50.20m;
        Should.BeEqual(expected, amount / f);
    }

    [Test]
    public void Divide_Int()
    {
        Amount amount = (Amount)100.40m;
        int f = 2;
        Amount expected = (Amount)50.20m;
        Should.BeEqual(expected, amount / f);
    }

    [Test]
    public void Divide_Long()
    {
        Amount amount = (Amount)100.40m;
        long f = 2;
        Amount expected = (Amount)50.20m;
        Should.BeEqual(expected, amount / f);
    }

    [Test]
    public void Divide_UShort()
    {
        Amount amount = (Amount)100.40m;
        ushort f = 2;
        Amount expected = (Amount)50.20m;
        Should.BeEqual(expected, amount / f);
    }

    [Test]
    public void Divide_UInt()
    {
        Amount amount = (Amount)100.40m;
        uint f = 2;
        Amount expected = (Amount)50.20m;
        Should.BeEqual(expected, amount / f);
    }

    [Test]
    public void Divide_ULong()
    {
        Amount amount = (Amount)100.40m;
        ulong f = 2;
        Amount expected = (Amount)50.20m;
        Should.BeEqual(expected, amount / f);
    }

    [Test]
    public void Round_NoDigits()
    {
        var amount = (Amount)123.4567m;
        var rounded = amount.Round();
        rounded.Should().Be((Amount)123m);
    }

    [Test]
    public void Round_1Digit()
    {
        var amount = (Amount)123.4567m;
        var rounded = amount.Round(1);
        rounded.Should().Be((Amount)123.5m);
    }

    [Test]
    public void RoundToMultiple_0d25()
    {
        var amount = (Amount)123.6567m;
        var rounded = amount.RoundToMultiple(0.25m);
        rounded.Should().Be((Amount)123.75m);
    }
}
