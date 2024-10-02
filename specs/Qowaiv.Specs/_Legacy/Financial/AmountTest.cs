namespace Qowaiv.Financial.UnitTests;

/// <summary>Tests the Amount SVO.</summary>
public class AmountTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly Amount TestStruct = (Amount)42.17m;

    private static NumberFormatInfo GetCustomNumberFormatInfo()
    {
        var info = new NumberFormatInfo
        {
            CurrencyGroupSeparator = "#",
            CurrencyDecimalSeparator = "*",
        };
        return info;
    }

    #region Amount const tests

    /// <summary>Amount.Zero should be equal to the default of Amount.</summary>
    [Test]
    public void Zero_None_EqualsDefault()
    {
        Amount.Zero.Should().Be(default);
    }

    #endregion

    #region TryParse tests

    /// <summary>TryParse null should be valid.</summary>
    [Test]
    public void TryParse_Null_IsInvalid() => Amount.TryParse(Nil.String, out _).Should().BeFalse();

    /// <summary>TryParse string.Empty should be valid.</summary>
    [Test]
    public void TryParse_StringEmpty_IsInvalid() => Amount.TryParse(string.Empty, out _).Should().BeFalse();

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_StringValue_IsValid()
    {
        using (CultureInfoScope.NewInvariant())
        {
            string str = "14.1804";
            Amount.TryParse(str, out Amount val).Should().BeTrue();
            Assert.AreEqual(str, val.ToString(), "Value");
        }
    }

    [Test]
    public void Parse_InvalidInput_ThrowsFormatException()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Assert.Catch<FormatException>
            (() =>
            {
                Amount.Parse("InvalidInput");
            },
            "Not a valid amount");
        }
    }

    [Test]
    public void TryParse_TestStructInput_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var exp = TestStruct;
            var act = Amount.TryParse(exp.ToString());

            act.Should().Be(exp);
        }
    }

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => Amount.TryParse("invalid input").Should().BeNull();

    [Test]
    public void Parse_CustomFormatProvider_ValidParsing()
    {
        Amount act = Amount.Parse("5#123*34", GetCustomNumberFormatInfo());
        Amount exp = (Amount)5123.34;

        act.Should().Be(exp);
    }

    #endregion

    #region (XML) (De)serialization tests

#if NET8_0_OR_GREATER
#else
    [Test]
    public void GetObjectData_SerializationInfo_AreEqual()
    {
        ISerializable obj = TestStruct;
        var info = new SerializationInfo(typeof(Amount), new FormatterConverter());
        obj.GetObjectData(info, default);

        info.GetDecimal("Value").Should().Be(42.17m);
    }

    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_TestStruct_AreEqual()
    {
        var input = TestStruct;
        var exp = TestStruct;
        var act = SerializeDeserialize.Binary(input);
        act.Should().Be(exp);
    }
#endif

    [Test]
    public void DataContractSerializeDeserialize_TestStruct_AreEqual()
    {
        var input = TestStruct;
        var exp = TestStruct;
        var act = SerializeDeserialize.DataContract(input);
        act.Should().Be(exp);
    }

    [Test]
    public void XmlSerialize_TestStruct_AreEqual()
    {
        var act = Serialize.Xml(TestStruct);
        var exp = "42.17";
        act.Should().Be(exp);
    }

    [Test]
    public void XmlDeserialize_XmlString_AreEqual()
    {
        var act = Deserialize.Xml<Amount>("42.17");
        act.Should().Be(TestStruct);
    }

#if NET8_0_OR_GREATER
#else
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_AmountSerializeObject_AreEqual()
    {
        var input = new AmountSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new AmountSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var act = SerializeDeserialize.Binary(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }
#endif

    [Test]
    public void XmlSerializeDeserialize_AmountSerializeObject_AreEqual()
    {
        var input = new AmountSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new AmountSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var act = SerializeDeserialize.Xml(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }
    [Test]
    public void DataContractSerializeDeserialize_AmountSerializeObject_AreEqual()
    {
        var input = new AmountSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new AmountSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var act = SerializeDeserialize.DataContract(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }

#if NET8_0_OR_GREATER
#else
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_Default_AreEqual()
    {
        var input = new AmountSerializeObject
        {
            Id = 17,
            Obj = default,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new AmountSerializeObject
        {
            Id = 17,
            Obj = default,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var act = SerializeDeserialize.Binary(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }
#endif

    [Test]
    public void XmlSerializeDeserialize_Empty_AreEqual()
    {
        var input = new AmountSerializeObject
        {
            Id = 17,
            Obj = Amount.Zero,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new AmountSerializeObject
        {
            Id = 17,
            Obj = Amount.Zero,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var act = SerializeDeserialize.Xml(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }

    [Test]
    public void GetSchema_None_IsNull()
    {
        IXmlSerializable obj = TestStruct;
        obj.GetSchema().Should().BeNull();
    }

    #endregion

    #region IFormattable / ToString tests

    [Test]
    public void ToString_CustomFormatter_SupportsCustomFormatting()
    {
        var act = TestStruct.ToString("#.0", FormatProvider.CustomFormatter);
        var exp = "Unit Test Formatter, value: '42.2', format: '#.0'";

        act.Should().Be(exp);
    }
    [Test]
    public void ToString_TestStruct_ComplexPattern()
    {
        var act = TestStruct.ToString(string.Empty, CultureInfo.InvariantCulture);
        var exp = "42.17";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString_ValueDutchBelgium_AreEqual()
    {
        using (TestCultures.nl_BE.Scoped())
        {
            var act = Amount.Parse("1600,1").ToString();
            var exp = "1600,1";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void ToString_ValueEnglishGreatBritain_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var act = Amount.Parse("1600.1").ToString();
            var exp = "1600.1";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void ToString_FormatValueDutchBelgium_AreEqual()
    {
        using (TestCultures.nl_BE.Scoped())
        {
            var act = Amount.Parse("800").ToString("0000");
            var exp = "0800";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void ToString_FormatValueEnglishGreatBritain_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var act = Amount.Parse("800").ToString("0000");
            var exp = "0800";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void ToString_FormatValueSpanishEcuador_AreEqual()
    {
        var act = Amount.Parse("1700").ToString("00000.0", new CultureInfo("es-EC"));
        var exp = "01700,0";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString_FormatCurrencyFrFr_170Comma42Euro()
    {
        Amount amount = (Amount)170.42;
        var act = amount.ToString("C", new CultureInfo("fr-FR"));
        var exp = "170,42 €";
        act.Should().Be(exp);
    }


    [Test]
    public void ToString_CustomFormatProvider_Formatted()
    {
        Amount amount = (Amount)12345678.235m;
        var act = amount.ToString("#,##0.0000", GetCustomNumberFormatInfo());
        var exp = "12#345#678*2350";
        act.Should().Be(exp);
    }

    #endregion

    #region IEquatable tests

    /// <summary>GetHash should not fail for Amount.Zero.</summary>
    [Test]
    public void GetHash_Zero_Hash()
    {
        Amount.Zero.GetHashCode().Should().Be(0);
    }

    /// <summary>GetHash should not fail for the test struct.</summary>
    [Test]
    public void GetHash_SameValue_SameHash()
    {
        var hash0 = ((Amount)451).GetHashCode();
        var hash1 = ((Amount)451).GetHashCode();
        hash0.Should().Be(hash1);
    }

    [Test]
    public void Equals_EmptyEmpty_IsTrue()
    {
        Amount.Zero.Equals(Amount.Zero).Should().BeTrue();
    }

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
    public void Equals_TestStructTestStruct_IsTrue()
    {
        TestStruct.Equals(TestStruct).Should().BeTrue();
    }

    [Test]
    public void Equals_TestStructEmpty_IsFalse()
    {
        TestStruct.Equals(Amount.Zero).Should().BeFalse();
    }

    [Test]
    public void Equals_EmptyTestStruct_IsFalse()
    {
        Amount.Zero.Equals(TestStruct).Should().BeFalse();
    }

    [Test]
    public void Equals_TestStructObjectTestStruct_IsTrue()
    {
        TestStruct.Equals((object)TestStruct).Should().BeTrue();
    }

    [Test]
    public void Equals_TestStructNull_IsFalse()
    {
        TestStruct.Equals(null).Should().BeFalse();
    }

    [Test]
    public void Equals_TestStructObject_IsFalse()
    {
        TestStruct.Equals(new object()).Should().BeFalse();
    }

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

    #region IComparable tests

    /// <summary>Orders a list of Amounts ascending.</summary>
    [Test]
    public void OrderBy_Amount_AreEqual()
    {
        Amount item0 = (Amount)0.23;
        Amount item1 = (Amount)1.24;
        Amount item2 = (Amount)2.27;
        Amount item3 = (Amount)1300;

        var inp = new List<Amount> { Amount.Zero, item3, item2, item0, item1, Amount.Zero };
        var exp = new List<Amount> { Amount.Zero, Amount.Zero, item0, item1, item2, item3 };
        var act = inp.OrderBy(item => item).ToList();

        act.Should().BeEquivalentTo(exp);
    }

    /// <summary>Orders a list of Amounts descending.</summary>
    [Test]
    public void OrderByDescending_Amount_AreEqual()
    {
        Amount item0 = (Amount)0.23;
        Amount item1 = (Amount)1.24;
        Amount item2 = (Amount)2.27;
        Amount item3 = (Amount)1300;

        var inp = new List<Amount> { Amount.Zero, item3, item2, item0, item1, Amount.Zero };
        var exp = new List<Amount> { item3, item2, item1, item0, Amount.Zero, Amount.Zero };
        var act = inp.OrderByDescending(item => item).ToList();

        act.Should().BeEquivalentTo(exp);
    }

    /// <summary>Compare with a to object casted instance should be fine.</summary>
    [Test]
    public void CompareTo_ObjectTestStruct_0()
    {
        object other = TestStruct;

        var exp = 0;
        var act = TestStruct.CompareTo(other);

        act.Should().Be(exp);
    }

    /// <summary>Compare with a random object should throw an exception.</summary>
    [Test]
    public void CompareTo_newObject_ThrowsArgumentException()
    {
        Func<int> compare = () => TestStruct.CompareTo(new object());
        compare.Should().Throw<ArgumentException>();
    }

    [Test]
    public void LessThan_17LT19_IsTrue()
    {
        Amount l = (Amount)17;
        Amount r = (Amount)19;

        (l < r).Should().BeTrue();
    }
    [Test]
    public void GreaterThan_21LT19_IsTrue()
    {
        Amount l = (Amount)21;
        Amount r = (Amount)19;

        (l > r).Should().BeTrue();
    }

    [Test]
    public void LessThanOrEqual_17LT19_IsTrue()
    {
        Amount l = (Amount)17;
        Amount r = (Amount)19;

        (l <= r).Should().BeTrue();
    }
    [Test]
    public void GreaterThanOrEqual_21LT19_IsTrue()
    {
        Amount l = (Amount)21;
        Amount r = (Amount)19;

        (l >= r).Should().BeTrue();
    }

    [Test]
    public void LessThanOrEqual_17LT17_IsTrue()
    {
        Amount l = (Amount)17;
        Amount r = (Amount)17;

        (l <= r).Should().BeTrue();
    }
    [Test]
    public void GreaterThanOrEqual_21LT21_IsTrue()
    {
        Amount l = (Amount)21;
        Amount r = (Amount)21;

        (l >= r).Should().BeTrue();
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
        Assert.AreEqual(TestStruct, amount + other);
    }

    [Test]
    public void Add_SomePercentage_Added()
    {
        Amount amount = (Amount)40.00;
        var p = 10.Percent();
        Assert.AreEqual((Amount)44.00, amount + p);
    }

    [Test]
    public void Subtract_SomeAmount_Subtracted()
    {
        Amount amount = (Amount)43.20;
        Amount other = (Amount)1.03;
        Assert.AreEqual(TestStruct, amount - other);
    }

    [Test]
    public void Subtract_SomePercentage_Subtracted()
    {
        Amount amount = (Amount)40.00;
        var p = 25.Percent();
        Assert.AreEqual((Amount)30.00, amount - p);
    }

    [Test]
    public void Multiply_Percentage()
    {
        Amount amount = (Amount)100.40m;
        var p = 50.Percent();
        Amount expected = (Amount)50.20m;
        Assert.AreEqual(expected, amount * p);
    }

    [Test]
    public void Multiply_Float()
    {
        Amount amount = (Amount)100.40m;
        float p = 0.5F;
        Amount expected = (Amount)50.20m;
        Assert.AreEqual(expected, amount * p);
    }

    [Test]
    public void Multiply_Double()
    {
        Amount amount = (Amount)100.40m;
        double p = 0.5;
        Amount expected = (Amount)50.20m;
        Assert.AreEqual(expected, amount * p);
    }

    [Test]
    public void Multiply_Decimal()
    {
        Amount amount = (Amount)100.40m;
        var p = 0.5m;
        Amount expected = (Amount)50.20m;
        Assert.AreEqual(expected, amount * p);
    }

    [Test]
    public void Multiply_Short()
    {
        Amount amount = (Amount)100.40m;
        short f = 2;
        Amount expected = (Amount)200.80m;
        Assert.AreEqual(expected, amount * f);
    }

    [Test]
    public void Multiply_Int()
    {
        Amount amount = (Amount)100.40m;
        int f = 2;
        Amount expected = (Amount)200.80m;
        Assert.AreEqual(expected, amount * f);
    }

    [Test]
    public void Multiply_Long()
    {
        Amount amount = (Amount)100.40m;
        long f = 2;
        Amount expected = (Amount)200.80m;
        Assert.AreEqual(expected, amount * f);
    }

    [Test]
    public void Multiply_UShort()
    {
        Amount amount = (Amount)100.40m;
        ushort f = 2;
        Amount expected = (Amount)200.80m;
        Assert.AreEqual(expected, amount * f);
    }

    [Test]
    public void Multiply_UInt()
    {
        Amount amount = (Amount)100.40m;
        uint f = 2;
        Amount expected = (Amount)200.80m;
        Assert.AreEqual(expected, amount * f);
    }

    [Test]
    public void Multiply_ULong()
    {
        Amount amount = (Amount)100.40m;
        ulong f = 2;
        Amount expected = (Amount)200.80m;
        Assert.AreEqual(expected, amount * f);
    }

    [Test]
    public void Divide_Percentage()
    {
        Amount amount = (Amount)100.40m;
        var p = 50.Percent();
        Amount expected = (Amount)200.80m;
        Assert.AreEqual(expected, amount / p);
    }

    [Test]
    public void Divide_Float()
    {
        Amount amount = (Amount)100.40m;
        float p = 0.5F;
        Amount expected = (Amount)200.80m;
        Assert.AreEqual(expected, amount / p);
    }

    [Test]
    public void Divide_Double()
    {
        Amount amount = (Amount)100.40m;
        double p = 0.5;
        Amount expected = (Amount)200.80m;
        Assert.AreEqual(expected, amount / p);
    }

    [Test]
    public void Divide_Decimal()
    {
        Amount amount = (Amount)100.40m;
        var p = 0.5m;
        Amount expected = (Amount)200.80m;
        Assert.AreEqual(expected, amount / p);
    }

    [Test]
    public void Divide_Short()
    {
        Amount amount = (Amount)100.40m;
        short f = 2;
        Amount expected = (Amount)50.20m;
        Assert.AreEqual(expected, amount / f);
    }

    [Test]
    public void Divide_Int()
    {
        Amount amount = (Amount)100.40m;
        int f = 2;
        Amount expected = (Amount)50.20m;
        Assert.AreEqual(expected, amount / f);
    }

    [Test]
    public void Divide_Long()
    {
        Amount amount = (Amount)100.40m;
        long f = 2;
        Amount expected = (Amount)50.20m;
        Assert.AreEqual(expected, amount / f);
    }

    [Test]
    public void Divide_UShort()
    {
        Amount amount = (Amount)100.40m;
        ushort f = 2;
        Amount expected = (Amount)50.20m;
        Assert.AreEqual(expected, amount / f);
    }

    [Test]
    public void Divide_UInt()
    {
        Amount amount = (Amount)100.40m;
        uint f = 2;
        Amount expected = (Amount)50.20m;
        Assert.AreEqual(expected, amount / f);
    }

    [Test]
    public void Divide_ULong()
    {
        Amount amount = (Amount)100.40m;
        ulong f = 2;
        Amount expected = (Amount)50.20m;
        Assert.AreEqual(expected, amount / f);
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

[Serializable]
public class AmountSerializeObject
{
    public int Id { get; set; }
    public Amount Obj { get; set; }
    public DateTime Date { get; set; }
}
