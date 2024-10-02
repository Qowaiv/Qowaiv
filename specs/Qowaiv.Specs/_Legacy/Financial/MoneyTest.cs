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
    public void Zero_None_EqualsDefault()
    {
        Money.Zero.Should().Be(default);
    }

    #endregion

    #region TryParse tests

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_StringValue_IsValid()
    {
        using (TestCultures.nl_NL.Scoped())
        {
            Money exp = 42.17 + Currency.EUR;
            Money.TryParse("€42,17", out Money act).Should().BeTrue();
            Assert.AreEqual(exp, act, "Value");
        }
    }

    /// <summary>TryParse with specified string value should be invalid.</summary>
    [Test]
    public void TryParse_StringValue_IsNotValid()
    {
        string str = "string";
        Money.TryParse(str, out Money val).Should().BeFalse();
        Assert.AreEqual(Money.Zero, val, "Value");
    }

    [Test]
    public void Parse_InvalidInput_ThrowsFormatException()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Assert.Catch<FormatException>
            (() =>
            {
                Money.Parse("InvalidInput");
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
            var act = Money.TryParse("€42.17");

            act.Should().Be(exp);
        }
    }

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => Money.TryParse("invalid input").Should().BeNull();

    [Test]
    public void Parse_EuroSpace12_Parsed()
    {
        using (TestCultures.fr_FR.Scoped())
        {
            var exp = 12 + Currency.EUR;
            var act = Money.Parse("€ 12");
            act.Should().Be(exp);
        }
    }

    [Test]
    public void Parse_Min12Comma765SpaceEuro_Parsed()
    {
        using (TestCultures.fr_FR.Scoped())
        {
            var exp = -12.765 + Currency.EUR;
            var act = Money.Parse("-12,765 €");
            act.Should().Be(exp);
        }
    }

    #endregion

    #region (XML) (De)serialization tests

#if NET8_0_OR_GREATER
#else
    [Test]
    public void GetObjectData_SerializationInfo_AreEqual()
    {
        ISerializable obj = TestStruct;
        var info = new SerializationInfo(typeof(Money), new FormatterConverter());
        obj.GetObjectData(info, default);

        info.GetDecimal("Value").Should().Be(42.17m);
        info.GetString("Currency").Should().Be("EUR");
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
        var exp = "EUR42.17";
        act.Should().Be(exp);
    }

    [Test]
    public void XmlDeserialize_XmlString_AreEqual()
    {
        var act = Deserialize.Xml<Money>("EUR42.17");
        act.Should().Be(TestStruct);
    }

#if NET8_0_OR_GREATER
#else
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_MoneySerializeObject_AreEqual()
    {
        var input = new MoneySerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new MoneySerializeObject
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
    public void XmlSerializeDeserialize_MoneySerializeObject_AreEqual()
    {
        var input = new MoneySerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new MoneySerializeObject
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
    public void DataContractSerializeDeserialize_MoneySerializeObject_AreEqual()
    {
        var input = new MoneySerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new MoneySerializeObject
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
        var input = new MoneySerializeObject
        {
            Id = 17,
            Obj = default,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new MoneySerializeObject
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
        var input = new MoneySerializeObject
        {
            Id = 17,
            Obj = Money.Zero,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new MoneySerializeObject
        {
            Id = 17,
            Obj = Money.Zero,
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
    public void ToString_Empty_StringEmpty()
    {
        using (CultureInfoScope.NewInvariant())
        {
            var act = Money.Zero.ToString();
            var exp = "0";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void ToString_CustomFormatter_SupportsCustomFormatting()
    {
        var act = TestStruct.ToString("0.0", FormatProvider.CustomFormatter);
        var exp = "Unit Test Formatter, value: '42.2', format: '0.0'";

        act.Should().Be(exp);
    }

    [Test]
    public void ToString_TestStruct_ComplexPattern()
    {
        using (TestCultures.nl_BE.Scoped())
        {
            var act = TestStruct.ToString("0.00");
            var exp = "42,17";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void ToString_ValueDutchBelgium_AreEqual()
    {
        using (TestCultures.nl_BE.Scoped())
        {
            // 3: n $
            CultureInfo.CurrentCulture.NumberFormat.CurrencyPositivePattern = 3;
            var act = Money.Parse("ALL 1600,1").ToString();
            var exp = "1.600,10 Lekë";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void ToString_ValueEnglishGreatBritain_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var act = Money.Parse("EUR 1600.1").ToString();
            var exp = "€1,600.10";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void ToString_AED_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var act = Money.Parse("AED 1600.1").ToString();
            var exp = "AED1,600.10";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void ToString_FormatValueDutchBelgium_AreEqual()
    {
        using (TestCultures.nl_BE.Scoped())
        {
            var act = Money.Parse("800").ToString("0000");
            var exp = "0800";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void ToString_FormatValueEnglishGreatBritain_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var act = Money.Parse("800").ToString("0000");
            var exp = "0800";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void ToString_FormatValueSpanishEcuador_AreEqual()
    {
        var act = Money.Parse("1700").ToString("00000.0", new CultureInfo("es-EC"));
        var exp = "01700,0";
        act.Should().Be(exp);
    }

    #endregion

    #region IEquatable tests

    /// <summary>GetHash should not fail for Money.Empty.</summary>
    [Test]
    public void GetHash_Empty_Hash()
    {
        Money.Zero.GetHashCode().Should().Be(0);
    }

    /// <summary>GetHash should not fail for the test struct.</summary>
    [Test]
    public void GetHash_TestStruct_NotZero()
    {
        Assert.NotZero(TestStruct.GetHashCode());
    }

    [Test]
    public void Equals_EmptyEmpty_IsTrue()
    {
        Money.Zero.Equals(Money.Zero).Should().BeTrue();
    }

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        var l = Money.Parse("€1,234,456.789", CultureInfo.InvariantCulture);
        var r = Money.Parse("EUR1234456.789", CultureInfo.InvariantCulture);

        l.Equals(r).Should().BeTrue();
    }

    [Test]
    public void Equals_TestStructTestStruct_IsTrue()
    {
        TestStruct.Equals(TestStruct).Should().BeTrue();
    }

    [Test]
    public void Equals_TestStructEmpty_IsFalse()
    {
        TestStruct.Equals(Money.Zero).Should().BeFalse();
    }

    [Test]
    public void Equals_EmptyTestStruct_IsFalse()
    {
        Money.Zero.Equals(TestStruct).Should().BeFalse();
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

    /// <summary>Orders a list of Moneys ascending.</summary>
    [Test]
    public void OrderBy_Money_AreEqual()
    {
        var item0 = 124.40 + Currency.EUR;
        var item1 = 124.41 + Currency.EUR;
        var item2 = 124.42 + Currency.EUR;
        var item3 = 124.39 + Currency.GBP;

        var inp = new List<Money> { Money.Zero, item3, item2, item0, item1, Money.Zero };
        var exp = new List<Money> { Money.Zero, Money.Zero, item0, item1, item2, item3 };
        var act = inp.OrderBy(item => item).ToList();

        act.Should().BeEquivalentTo(exp);
    }

    /// <summary>Orders a list of Moneys descending.</summary>
    [Test]
    public void OrderByDescending_Money_AreEqual()
    {
        var item0 = 124.40 + Currency.EUR;
        var item1 = 124.41 + Currency.EUR;
        var item2 = 124.42 + Currency.EUR;
        var item3 = 124.39 + Currency.GBP;

        var inp = new List<Money> { Money.Zero, item3, item2, item0, item1, Money.Zero };
        var exp = new List<Money> { item3, item2, item1, item0, Money.Zero, Money.Zero };
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
        var l = 17 + Currency.DKK;
        var r = 19 + Currency.DKK;

        (l < r).Should().BeTrue();
    }
    [Test]
    public void GreaterThan_21LT19_IsTrue()
    {
        var l = 21 + Currency.DKK;
        var r = 19 + Currency.DKK;

        (l > r).Should().BeTrue();
    }

    [Test]
    public void LessThanOrEqual_17LT19_IsTrue()
    {
        var l = 17 + Currency.DKK;
        var r = 19 + Currency.DKK;

        (l <= r).Should().BeTrue();
    }
    [Test]
    public void GreaterThanOrEqual_21LT19_IsTrue()
    {
        var l = 21 + Currency.DKK;
        var r = 19 + Currency.DKK;

        (l >= r).Should().BeTrue();
    }

    [Test]
    public void LessThanOrEqual_17LT17_IsTrue()
    {
        var l = 17 + Currency.DKK;
        var r = 17 + Currency.DKK;

        (l <= r).Should().BeTrue();
    }
    [Test]
    public void GreaterThanOrEqual_21LT21_IsTrue()
    {
        var l = 21 + Currency.DKK;
        var r = 21 + Currency.DKK;

        (l >= r).Should().BeTrue();
    }
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
        Assert.AreEqual(expected + Currency.USD, abs);
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

        Assert.AreEqual(a, l + r);
    }

    [Test]
    public void Add_25Percent_Added()
    {
        var l = 16 + Currency.EUR;
        var p = 25.Percent();
        var a = 20 + Currency.EUR;

        Assert.AreEqual(a, l + p);
    }

    [Test]
    public void Subtract_SameCurrency_Subtracted()
    {
        var l = 69 + Currency.EUR;
        var r = 27 + Currency.EUR;
        var a = 42 + Currency.EUR;

        Assert.AreEqual(a, l - r);
    }

    [Test]
    public void Subtract_25Percent_Subtracted()
    {
        var l = 16 + Currency.EUR;
        var p = 25.Percent();
        var a = 12 + Currency.EUR;

        Assert.AreEqual(a, l - p);
    }

    [Test]
    public void Multiply_Percentage()
    {
        var money = 100.40m + Currency.USD;
        var p = 50.Percent();
        var expected = 50.20m + Currency.USD;
        Assert.AreEqual(expected, money * p);
    }

    [Test]
    public void Multiply_Float()
    {
        var money = 100.40m + Currency.USD;
        float p = 0.5F;
        var expected = 50.20m + Currency.USD;
        Assert.AreEqual(expected, money * p);
    }

    [Test]
    public void Multiply_Double()
    {
        var money = 100.40m + Currency.USD;
        double p = 0.5;
        var expected = 50.20m + Currency.USD;
        Assert.AreEqual(expected, money * p);
    }

    [Test]
    public void Multiply_Decimal()
    {
        var money = 100.40m + Currency.USD;
        var p = 0.5m;
        var expected = 50.20m + Currency.USD;
        Assert.AreEqual(expected, money * p);
    }

    [Test]
    public void Multiply_Short()
    {
        var money = 100.40m + Currency.USD;
        short f = 2;
        var expected = 200.8m + Currency.USD;
        Assert.AreEqual(expected, money * f);
    }

    [Test]
    public void Multiply_Int()
    {
        var money = 100.40m + Currency.USD;
        int f = 2;
        var expected = 200.8m + Currency.USD;
        Assert.AreEqual(expected, money * f);
    }

    [Test]
    public void Multiply_Long()
    {
        var money = 100.40m + Currency.USD;
        long f = 2;
        var expected = 200.8m + Currency.USD;
        Assert.AreEqual(expected, money * f);
    }

    [Test]
    public void Multiply_UShort()
    {
        var money = 100.40m + Currency.USD;
        ushort f = 2;
        var expected = 200.8m + Currency.USD;
        Assert.AreEqual(expected, money * f);
    }

    [Test]
    public void Multiply_UInt()
    {
        var money = 100.40m + Currency.USD;
        uint f = 2;
        var expected = 200.8m + Currency.USD;
        Assert.AreEqual(expected, money * f);
    }

    [Test]
    public void Multiply_ULong()
    {
        var money = 100.40m + Currency.USD;
        ulong f = 2;
        var expected = 200.8m + Currency.USD;
        Assert.AreEqual(expected, money * f);
    }

    [Test]
    public void Divide_Percentage()
    {
        var money = 100.40m + Currency.USD;
        var p = 50.Percent();
        var expected = 200.8m + Currency.USD;
        Assert.AreEqual(expected, money / p);
    }

    [Test]
    public void Divide_Float()
    {
        var money = 100.40m + Currency.USD;
        float p = 0.5F;
        var expected = 200.8m + Currency.USD;
        Assert.AreEqual(expected, money / p);
    }

    [Test]
    public void Divide_Double()
    {
        var money = 100.40m + Currency.USD;
        double p = 0.5;
        var expected = 200.8m + Currency.USD;
        Assert.AreEqual(expected, money / p);
    }

    [Test]
    public void Divide_Decimal()
    {
        var money = 100.40m + Currency.USD;
        var p = 0.5m;
        var expected = 200.8m + Currency.USD;
        Assert.AreEqual(expected, money / p);
    }

    [Test]
    public void Divide_Short()
    {
        var money = 100.40m + Currency.USD;
        short f = 2;
        var expected = 50.20m + Currency.USD;
        Assert.AreEqual(expected, money / f);
    }

    [Test]
    public void Divide_Int()
    {
        var money = 100.40m + Currency.USD;
        int f = 2;
        var expected = 50.20m + Currency.USD;
        Assert.AreEqual(expected, money / f);
    }

    [Test]
    public void Divide_Long()
    {
        var money = 100.40m + Currency.USD;
        long f = 2;
        var expected = 50.20m + Currency.USD;
        Assert.AreEqual(expected, money / f);
    }

    [Test]
    public void Divide_UShort()
    {
        var money = 100.40m + Currency.USD;
        ushort f = 2;
        var expected = 50.20m + Currency.USD;
        Assert.AreEqual(expected, money / f);
    }

    [Test]
    public void Divide_UInt()
    {
        var money = 100.40m + Currency.USD;
        uint f = 2;
        var expected = 50.20m + Currency.USD;
        Assert.AreEqual(expected, money / f);
    }

    [Test]
    public void Divide_ULong()
    {
        var money = 100.40m + Currency.USD;
        ulong f = 2;
        var expected = 50.20m + Currency.USD;
        Assert.AreEqual(expected, money / f);
    }

    [Test]
    public void Round_EUR_2Digits()
    {
        var money = 123.4567m + Currency.EUR;
        var rounded = money.Round();
        Assert.AreEqual(123.46m + Currency.EUR, rounded);
    }

    [Test]
    public void Round_1Digit()
    {
        var money = 123.4567m + Currency.EUR;
        var rounded = money.Round(1);
        Assert.AreEqual(123.5m + Currency.EUR, rounded);
    }

    [Test]
    public void RoundToMultiple_0d25()
    {
        var money = 123.6567m + Currency.EUR;
        var rounded = money.RoundToMultiple(0.25m);
        Assert.AreEqual(123.75m + Currency.EUR, rounded);
    }

    #endregion
}

[Serializable]
public class MoneySerializeObject
{
    public int Id { get; set; }
    public Money Obj { get; set; }
    public DateTime Date { get; set; }
}
