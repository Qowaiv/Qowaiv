namespace Qowaiv.UnitTests.Financial;

/// <summary>Tests the currency SVO.</summary>
public class CurrencyTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly Currency TestStruct = Currency.EUR;

    #region currency const tests

    /// <summary>Currency.Empty should be equal to the default of currency.</summary>
    [Test]
    public void Empty_None_EqualsDefault()
    {
        Currency.Empty.Should().Be(default);
    }

    #endregion

    #region Current

    [Test]
    public void Current_CurrentCultureNlNL_Germany()
    {
        using (TestCultures.nl_NL.Scoped())
        {
            var act = Currency.Current;
            var exp = Currency.EUR;

            act.Should().Be(exp);
        }
    }

    [Test]
    public void Current_CurrentCultureEsEC_Ecuador()
    {
        using (TestCultures.es_EC.Scoped())
        {
            var act = Currency.Current;
            var exp = Currency.USD;

            act.Should().Be(exp);
        }
    }

    [Test]
    public void Current_CurrentCultureEn_Empty()
    {
        using (TestCultures.en.Scoped())
        {
            var act = Currency.Current;
            var exp = Currency.Empty;

            act.Should().Be(exp);
        }
    }

    #endregion

    #region currency IsEmpty tests

    /// <summary>Currency.IsEmpty() should be true for the default of currency.</summary>
    [Test]
    public void IsEmpty_Default_IsTrue()
    {
        default(Currency).IsEmpty().Should().BeTrue();
    }
    /// <summary>Currency.IsEmpty() should be false for Currency.Unknown.</summary>
    [Test]
    public void IsEmpty_Unknown_IsFalse()
    {
        Currency.Unknown.IsEmpty().Should().BeFalse();
    }
    /// <summary>Currency.IsEmpty() should be false for the TestStruct.</summary>
    [Test]
    public void IsEmpty_TestStruct_IsFalse()
    {
        TestStruct.IsEmpty().Should().BeFalse();
    }

    /// <summary>Currency.IsUnknown() should be false for the default of currency.</summary>
    [Test]
    public void IsUnknown_Default_IsFalse()
    {
        default(Currency).IsUnknown().Should().BeFalse();
    }
    /// <summary>Currency.IsUnknown() should be true for Currency.Unknown.</summary>
    [Test]
    public void IsUnknown_Unknown_IsTrue()
    {
        Currency.Unknown.IsUnknown().Should().BeTrue();
    }
    /// <summary>Currency.IsUnknown() should be false for the TestStruct.</summary>
    [Test]
    public void IsUnknown_TestStruct_IsFalse()
    {
        TestStruct.IsUnknown().Should().BeFalse();
    }

    /// <summary>Currency.IsEmptyOrUnknown() should be true for the default of currency.</summary>
    [Test]
    public void IsEmptyOrUnknown_Default_IsFalse()
    {
        default(Currency).IsEmptyOrUnknown().Should().BeTrue();
    }
    /// <summary>Currency.IsEmptyOrUnknown() should be true for Currency.Unknown.</summary>
    [Test]
    public void IsEmptyOrUnknown_Unknown_IsTrue()
    {
        Currency.Unknown.IsEmptyOrUnknown().Should().BeTrue();
    }
    /// <summary>Currency.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
    [Test]
    public void IsEmptyOrUnknown_TestStruct_IsFalse()
    {
        TestStruct.IsEmptyOrUnknown().Should().BeFalse();
    }

    #endregion

    #region TryParse tests

    /// <summary>TryParse null should be valid.</summary>
    [Test]
    public void TryParse_Null_IsValid()
    {
        Currency.TryParse(Nil.String, out Currency val).Should().BeTrue();
        val.Should().Be(default);
    }

    /// <summary>TryParse string.Empty should be valid.</summary>
    [Test]
    public void TryParse_StringEmpty_IsValid()
    {
        string str = string.Empty;
        Currency.TryParse(str, out Currency val).Should().BeTrue();
        val.Should().Be(default);
    }

    /// <summary>TryParse "?" should be valid and the result should be Currency.Unknown.</summary>
    [Test]
    public void TryParse_question_mark_IsValid()
    {
        string str = "?";
        Currency.TryParse(str, out Currency val).Should().BeTrue();
        val.IsUnknown().Should().BeTrue();
    }

    /// <summary>TryParse "¤" should be valid and the result should be Currency.Unknown.</summary>
    [Test]
    public void TryParse_UnknownCurrencySymbol_IsValid()
    {
        string str = "¤";
        Currency.TryParse(str, out Currency val).Should().BeTrue();
        val.IsUnknown().Should().BeTrue();
    }

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_StringValue_IsValid()
    {
        string str = "USD";
        Currency.TryParse(str, out Currency val).Should().BeTrue();
        Should.BeEqual(str, val.ToString(), "Value");
    }

    /// <summary>TryParse with specified string value should be invalid.</summary>
    [Test]
    public void TryParse_StringValue_IsNotValid()
    {
        string str = "string";
        Currency.TryParse(str, out Currency val).Should().BeFalse();
        val.Should().Be(default);
    }

    [Test]
    public void Parse_Unknown_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var act = Currency.Parse("?");
            var exp = Currency.Unknown;
            act.Should().Be(exp);
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
                Currency.Parse("InvalidInput");
            },
            "Not a valid currency");
        }
    }

    [Test]
    public void TryParse_TestStructInput_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var exp = TestStruct;
            var act = Currency.TryParse(exp.ToString());

            act.Should().Be(exp);
        }
    }

    [Test]
    public void from_invalid_as_null_with_TryParse()
       => Currency.TryParse("invalid input").Should().BeNull();

    [Test]
    public void Parse_EuroSign_EUR()
    {
        var act = Currency.Parse("€");
        var exp = Currency.EUR;
        act.Should().Be(exp);
    }

    #endregion

    #region (XML) (De)serialization tests

    [Test]
    public void DataContractSerializeDeserialize_TestStruct_AreEqual()
    {
        var input = CurrencyTest.TestStruct;
        var exp = CurrencyTest.TestStruct;
        var act = SerializeDeserialize.DataContract(input);
        act.Should().Be(exp);
    }

    [Test]
    public void XmlSerialize_TestStruct_AreEqual()
    {
        var act = Serialize.Xml(TestStruct);
        var exp = "EUR";
        act.Should().Be(exp);
    }

    [Test]
    public void XmlDeserialize_XmlString_AreEqual()
    {
        var act = Deserialize.Xml<Currency>("EUR");
        act.Should().Be(TestStruct);
    }

    [Test]
    public void XmlSerializeDeserialize_CurrencySerializeObject_AreEqual()
    {
        var input = new CurrencySerializeObject
        {
            Id = 17,
            Obj = CurrencyTest.TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new CurrencySerializeObject
        {
            Id = 17,
            Obj = CurrencyTest.TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var act = SerializeDeserialize.Xml(input);
        Should.BeEqual(exp.Id, act.Id, "Id");
        Should.BeEqual(exp.Obj, act.Obj, "Obj");
        Should.BeEqual(exp.Date, act.Date, "Date");
    }
    [Test]
    public void DataContractSerializeDeserialize_CurrencySerializeObject_AreEqual()
    {
        var input = new CurrencySerializeObject
        {
            Id = 17,
            Obj = CurrencyTest.TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new CurrencySerializeObject
        {
            Id = 17,
            Obj = CurrencyTest.TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var act = SerializeDeserialize.DataContract(input);
        Should.BeEqual(exp.Id, act.Id, "Id");
        Should.BeEqual(exp.Obj, act.Obj, "Obj");
        Should.BeEqual(exp.Date, act.Date, "Date");
    }

    [Test]
    public void XmlSerializeDeserialize_Empty_AreEqual()
    {
        var input = new CurrencySerializeObject
        {
            Id = 17,
            Obj = Currency.Empty,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new CurrencySerializeObject
        {
            Id = 17,
            Obj = Currency.Empty,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var act = SerializeDeserialize.Xml(input);
        Should.BeEqual(exp.Id, act.Id, "Id");
        Should.BeEqual(exp.Obj, act.Obj, "Obj");
        Should.BeEqual(exp.Date, act.Date, "Date");
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
        var act = Currency.Empty.ToString();
        var exp = "";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString_Unknown_QuestionMark()
    {
        var act = Currency.Unknown.ToString();
        var exp = "?";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString_CustomFormatter_SupportsCustomFormatting()
    {
        var act = TestStruct.ToString("$: e", FormatProvider.CustomFormatter);
        var exp = "Unit Test Formatter, value: '€: Euro', format: '$: e'";

        act.Should().Be(exp);
    }
    [Test]
    public void ToString_TestStruct_ComplexPattern()
    {
        var act = TestStruct.ToString(string.Empty);
        var exp = "EUR";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString_ValueDutchBelgium_AreEqual()
    {
        using (TestCultures.nl_BE.Scoped())
        {
            var act = Currency.Parse("Amerikaanse dollar").ToString();
            var exp = "USD";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void ToString_ValueEnglishGreatBritain_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var act = Currency.Parse("pound sterling").ToString("f");
            var exp = "Pound sterling";
            act.Should().Be(exp);
        }
    }
    #endregion

    #region IFormatProvider

    [Test]
    public void FormatAmount_BYR_NAf12Dot34()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Amount number = (Amount)1200.34m;
            var act = number.ToString("C", Currency.BYR);
            var exp = "BYR1,200";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void FormatAmount_ANG_NAf12Dot34()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Amount number = (Amount)12.34m;
            var act = number.ToString("C", Currency.ANG);
            var exp = "NAf.12.34";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void FormatDecimal_ANG_ANG12Dot34()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var number = 12.34m;
            var act = number.ToString("C", Currency.ANG);
            var exp = "NAf.12.34";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void FormatDecimal_TND_TND12Dot340()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var number = 12.34m;
            var act = number.ToString("C", Currency.TND);
            var exp = "TND12.340";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void FormatDecimal_EUR_E12Dot34()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var number = 12.34m;
            var act = number.ToString("C", Currency.EUR);
            var exp = "€12.34";

            act.Should().Be(exp);
        }
    }

    [Test]
    public void FormatDouble_EUR_E12Dot34()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var number = 12.34;
            var act = number.ToString("C", Currency.EUR);
            var exp = "€12.34";

            act.Should().Be(exp);
        }
    }

    #endregion

    #region IEquatable tests

    [Test]
    public void Equals_EmptyEmpty_IsTrue()
    {
        Currency.Empty.Equals(Currency.Empty).Should().BeTrue();
    }

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        var l = Currency.Parse("eur", CultureInfo.InvariantCulture);
        var r = Currency.Parse("EUR", CultureInfo.InvariantCulture);

        l.Equals(r).Should().BeTrue();
    }

    [Test]
    public void Equals_TestStructTestStruct_IsTrue()
    {
        CurrencyTest.TestStruct.Equals(CurrencyTest.TestStruct).Should().BeTrue();
    }

    [Test]
    public void Equals_TestStructEmpty_IsFalse()
    {
        CurrencyTest.TestStruct.Equals(Currency.Empty).Should().BeFalse();
    }

    [Test]
    public void Equals_EmptyTestStruct_IsFalse()
    {
        Currency.Empty.Equals(CurrencyTest.TestStruct).Should().BeFalse();
    }

    [Test]
    public void Equals_TestStructObjectTestStruct_IsTrue()
    {
        CurrencyTest.TestStruct.Equals((object)CurrencyTest.TestStruct).Should().BeTrue();
    }

    [Test]
    public void Equals_TestStructNull_IsFalse()
    {
        CurrencyTest.TestStruct.Equals(null).Should().BeFalse();
    }

    [Test]
    public void Equals_TestStructObject_IsFalse()
    {
        CurrencyTest.TestStruct.Equals(new object()).Should().BeFalse();
    }

    [Test]
    public void OperatorIs_TestStructTestStruct_IsTrue()
    {
        var l = CurrencyTest.TestStruct;
        var r = CurrencyTest.TestStruct;
        (l == r).Should().BeTrue();
    }

    [Test]
    public void OperatorIsNot_TestStructTestStruct_IsFalse()
    {
        var l = CurrencyTest.TestStruct;
        var r = CurrencyTest.TestStruct;
        (l != r).Should().BeFalse();
    }

    #endregion

    #region IComparable tests

    /// <summary>Orders a list of currencys ascending.</summary>
    [Test]
    public void OrderBy_Currency_AreEqual()
    {
        var item0 = Currency.AED;
        var item1 = Currency.BAM;
        var item2 = Currency.CAD;
        var item3 = Currency.EUR;

        var inp = new List<Currency> { Currency.Empty, item3, item2, item0, item1, Currency.Empty };
        var exp = new List<Currency> { Currency.Empty, Currency.Empty, item0, item1, item2, item3 };
        var act = inp.OrderBy(item => item).ToList();

        act.Should().BeEquivalentTo(exp);
    }

    /// <summary>Orders a list of currencys descending.</summary>
    [Test]
    public void OrderByDescending_Currency_AreEqual()
    {
        var item0 = Currency.AED;
        var item1 = Currency.BAM;
        var item2 = Currency.CAD;
        var item3 = Currency.EUR;

        var inp = new List<Currency> { Currency.Empty, item3, item2, item0, item1, Currency.Empty };
        var exp = new List<Currency> { item3, item2, item1, item0, Currency.Empty, Currency.Empty };
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
    #endregion

    #region Properties

    [Test]
    public void Symbol_AZN_Special()
    {
        var act = Currency.AZN.Symbol;
        var exp = "₼";
        act.Should().Be(exp);
    }

    #endregion
}

[Serializable]
public class CurrencySerializeObject
{
    public int Id { get; set; }
    public Currency Obj { get; set; }
    public DateTime Date { get; set; }
}
