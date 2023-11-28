namespace Qowaiv.UnitTests.Financial;

public class InternationalBankAccountNumberTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly InternationalBankAccountNumber TestStruct = InternationalBankAccountNumber.Parse("NL20 INGB 0001 2345 67");

    #region IBAN const tests

    /// <summary>InternationalBankAccountNumber.Empty should be equal to the default of IBAN.</summary>
    [Test]
    public void Empty_None_EqualsDefault()
    {
        InternationalBankAccountNumber.Empty.Should().Be(default(InternationalBankAccountNumber));
    }

    #endregion

    #region IBAN IsEmpty tests

    /// <summary>InternationalBankAccountNumber.IsEmpty() should true for the default of IBAN.</summary>
    [Test]
    public void IsEmpty_Default_IsTrue()
    {
        default(InternationalBankAccountNumber).IsEmpty().Should().BeTrue();
    }

    /// <summary>InternationalBankAccountNumber.IsEmpty() should false for the TestStruct.</summary>
    [Test]
    public void IsEmpty_Default_IsFalse()
    {
        TestStruct.IsEmpty().Should().BeFalse();
    }

    #endregion

    #region TryParse tests

    /// <summary>TryParse null should be valid.</summary>
    [Test]
    public void TryParse_Null_IsValid()
    {
        InternationalBankAccountNumber.TryParse(Nil.String, out var val).Should().BeTrue();
        Assert.AreEqual(string.Empty, val.ToString(), "Value");
    }

    /// <summary>TryParse string.Empty should be valid.</summary>
    [Test]
    public void TryParse_StringEmpty_IsValid()
    {
        string str = string.Empty;
        InternationalBankAccountNumber.TryParse(str, out var val).Should().BeTrue();
        Assert.AreEqual(string.Empty, val.ToString(), "Value");
    }

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_StringValue_IsValid()
    {
        string str = "NL20INGB0001234567";
        InternationalBankAccountNumber.TryParse(str, out var val).Should().BeTrue();
        Assert.AreEqual(str, val.ToString(), "Value");
    }

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_QuestionMark_IsValid()
    {
        string str = "?";
        InternationalBankAccountNumber.TryParse(str, out var val).Should().BeTrue();
        Assert.AreEqual(str, val.ToString(), "Value.ToString()");
        Assert.AreEqual(InternationalBankAccountNumber.Unknown, val, "Value");
    }

    /// <summary>TryParse with specified string value should be invalid.</summary>
    [Test]
    public void TryParse_StringValue_IsNotValid()
    {
        string str = "string";
        InternationalBankAccountNumber.TryParse(str, out var val).Should().BeFalse();
        Assert.AreEqual(string.Empty, val.ToString(), "Value");
    }

    [Test]
    public void Parse_InvalidInput_ThrowsFormatException()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Assert.Catch<FormatException>
            (() =>
            {
                InternationalBankAccountNumber.Parse("InvalidInput");
            },
            "Not a valid IBAN");
        }
    }

    [Test]
    public void TryParse_TestStructInput_AreEqual()
    {
        using (TestCultures.En_GB.Scoped())
        {
            var exp = TestStruct;
            var act = InternationalBankAccountNumber.TryParse(exp.ToString());

            act.Should().Be(exp);
        }
    }

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => InternationalBankAccountNumber.TryParse("invalid input").Should().BeNull();

    #endregion

    #region (XML) (De)serialization tests

#if NET8_0_OR_GREATER
#else
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_TestStruct_AreEqual()
    {
        var input = InternationalBankAccountNumberTest.TestStruct;
        var exp = InternationalBankAccountNumberTest.TestStruct;
        var act = SerializeDeserialize.Binary(input);
        act.Should().Be(exp);
    }
#endif

    [Test]
    public void DataContractSerializeDeserialize_TestStruct_AreEqual()
    {
        var input = InternationalBankAccountNumberTest.TestStruct;
        var exp = InternationalBankAccountNumberTest.TestStruct;
        var act = SerializeDeserialize.DataContract(input);
        act.Should().Be(exp);
    }

    [Test]
    public void XmlSerialize_TestStruct_AreEqual()
    {
        var act = Serialize.Xml(TestStruct);
        var exp = "NL20INGB0001234567";
        act.Should().Be(exp);
    }

    [Test]
    public void XmlDeserialize_XmlString_AreEqual()
    {
        var act = Deserialize.Xml<InternationalBankAccountNumber>("NL20INGB0001234567");
        act.Should().Be(TestStruct);
    }

#if NET8_0_OR_GREATER
#else
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_InternationalBankAccountNumberSerializeObject_AreEqual()
    {
        var input = new InternationalBankAccountNumberSerializeObject
        {
            Id = 17,
            Obj = InternationalBankAccountNumberTest.TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new InternationalBankAccountNumberSerializeObject
        {
            Id = 17,
            Obj = InternationalBankAccountNumberTest.TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var act = SerializeDeserialize.Binary(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }
#endif

    [Test]
    public void XmlSerializeDeserialize_InternationalBankAccountNumberSerializeObject_AreEqual()
    {
        var input = new InternationalBankAccountNumberSerializeObject
        {
            Id = 17,
            Obj = InternationalBankAccountNumberTest.TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new InternationalBankAccountNumberSerializeObject
        {
            Id = 17,
            Obj = InternationalBankAccountNumberTest.TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var act = SerializeDeserialize.Xml(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }
    [Test]
    public void DataContractSerializeDeserialize_InternationalBankAccountNumberSerializeObject_AreEqual()
    {
        var input = new InternationalBankAccountNumberSerializeObject
        {
            Id = 17,
            Obj = InternationalBankAccountNumberTest.TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new InternationalBankAccountNumberSerializeObject
        {
            Id = 17,
            Obj = InternationalBankAccountNumberTest.TestStruct,
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
    public void SerializeDeserialize_Empty_AreEqual()
    {
        var input = new InternationalBankAccountNumberSerializeObject
        {
            Id = 17,
            Obj = InternationalBankAccountNumber.Empty,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new InternationalBankAccountNumberSerializeObject
        {
            Id = 17,
            Obj = InternationalBankAccountNumber.Empty,
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
        var input = new InternationalBankAccountNumberSerializeObject
        {
            Id = 17,
            Obj = InternationalBankAccountNumber.Empty,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new InternationalBankAccountNumberSerializeObject
        {
            Id = 17,
            Obj = InternationalBankAccountNumber.Empty,
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
    public void ToString_Empty_IsStringEmpty()
    {
        var act = InternationalBankAccountNumber.Empty.ToString();
        var exp = "";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString_CustomFormatter_SupportsCustomFormatting()
    {
        var act = TestStruct.ToString("[f]", FormatProvider.CustomFormatter);
        var exp = "Unit Test Formatter, value: '[nl20 ingb 0001 2345 67]', format: '[f]'";

        act.Should().Be(exp);
    }

    [Test]
    public void ToString_Unknown_IsStringEmpty()
    {
        var act = InternationalBankAccountNumber.Unknown.ToString(string.Empty, null);
        var exp = "?";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString_TestStruct_AreEqual()
    {
        var act = TestStruct.ToString();
        var exp = "NL20INGB0001234567";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString_TestStructFormatULower_AreEqual()
    {
        var act = TestStruct.ToString("u");
        var exp = "nl20ingb0001234567";
        act.Should().Be(exp);
    }
    [Test]
    public void ToString_TestStructFormatUUpper_AreEqual()
    {
        var act = TestStruct.ToString("U");
        var exp = "NL20INGB0001234567";
        act.Should().Be(exp);
    }
    [Test]
    public void ToString_TestStructFormatFLower_AreEqual()
    {
        var act = TestStruct.ToString("f");
        var exp = "nl20 ingb 0001 2345 67";
        act.Should().Be(exp);
    }
    [Test]
    public void ToString_TestStructFormatFUpper_AreEqual()
    {
        var act = TestStruct.ToString("F");
        var exp = "NL20 INGB 0001 2345 67";
        act.Should().Be(exp);
    }
    [Test]
    public void ToString_EmptyFormatF_AreEqual()
    {
        var act = InternationalBankAccountNumber.Empty.ToString("F");
        var exp = "";
        act.Should().Be(exp);
    }
    [Test]
    public void ToString_UnknownFormatF_AreEqual()
    {
        var act = InternationalBankAccountNumber.Unknown.ToString("F");
        var exp = "?";
        act.Should().Be(exp);
    }

    #endregion

    #region IEquatable tests

    /// <summary>GetHash should not fail for InternationalBankAccountNumber.Empty.</summary>
    [Test]
    public void GetHash_Empty_Hash()
    {
        InternationalBankAccountNumber.Empty.GetHashCode().Should().Be(0);
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
        InternationalBankAccountNumber.Empty.Equals(InternationalBankAccountNumber.Empty).Should().BeTrue();
    }

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        var l = InternationalBankAccountNumber.Parse("NL20 INGB 0001 2345 67");
        var r = InternationalBankAccountNumber.Parse("nl20ingb0001234567");

        l.Equals(r).Should().BeTrue();
    }

    [Test]
    public void Equals_TestStructTestStruct_IsTrue()
    {
        InternationalBankAccountNumberTest.TestStruct.Equals(InternationalBankAccountNumberTest.TestStruct).Should().BeTrue();
    }

    [Test]
    public void Equals_TestStructEmpty_IsFalse()
    {
        InternationalBankAccountNumberTest.TestStruct.Equals(InternationalBankAccountNumber.Empty).Should().BeFalse();
    }

    [Test]
    public void Equals_EmptyTestStruct_IsFalse()
    {
        InternationalBankAccountNumber.Empty.Equals(InternationalBankAccountNumberTest.TestStruct).Should().BeFalse();
    }

    [Test]
    public void Equals_TestStructObjectTestStruct_IsTrue()
    {
        InternationalBankAccountNumberTest.TestStruct.Equals((object)InternationalBankAccountNumberTest.TestStruct).Should().BeTrue();
    }

    [Test]
    public void Equals_TestStructNull_IsFalse()
    {
        InternationalBankAccountNumberTest.TestStruct.Equals(null).Should().BeFalse();
    }

    [Test]
    public void Equals_TestStructObject_IsFalse()
    {
        InternationalBankAccountNumberTest.TestStruct.Equals(new object()).Should().BeFalse();
    }

    [Test]
    public void OperatorIs_TestStructTestStruct_IsTrue()
    {
        var l = InternationalBankAccountNumberTest.TestStruct;
        var r = InternationalBankAccountNumberTest.TestStruct;
        (l == r).Should().BeTrue();
    }

    [Test]
    public void OperatorIsNot_TestStructTestStruct_IsFalse()
    {
        var l = InternationalBankAccountNumberTest.TestStruct;
        var r = InternationalBankAccountNumberTest.TestStruct;
        (l != r).Should().BeFalse();
    }

    #endregion

    #region IComparable tests

    /// <summary>Orders a list of IBANs ascending.</summary>
    [Test]
    public void OrderBy_InternationalBankAccountNumber_AreEqual()
    {
        var item0 = InternationalBankAccountNumber.Parse("AE950210000000693123456");
        var item1 = InternationalBankAccountNumber.Parse("BH29BMAG1299123456BH00");
        var item2 = InternationalBankAccountNumber.Parse("CY17002001280000001200527600");
        var item3 = InternationalBankAccountNumber.Parse("DK5000400440116243");

        var inp = new List<InternationalBankAccountNumber> { InternationalBankAccountNumber.Empty, item3, item2, item0, item1, InternationalBankAccountNumber.Empty };
        var exp = new List<InternationalBankAccountNumber> { InternationalBankAccountNumber.Empty, InternationalBankAccountNumber.Empty, item0, item1, item2, item3 };
        var act = inp.OrderBy(item => item).ToList();

        act.Should().BeEquivalentTo(exp);
    }

    /// <summary>Orders a list of IBANs descending.</summary>
    [Test]
    public void OrderByDescending_InternationalBankAccountNumber_AreEqual()
    {
        var item0 = InternationalBankAccountNumber.Parse("AE950210000000693123456");
        var item1 = InternationalBankAccountNumber.Parse("BH29BMAG1299123456BH00");
        var item2 = InternationalBankAccountNumber.Parse("CY17002001280000001200527600");
        var item3 = InternationalBankAccountNumber.Parse("DK5000400440116243");

        var inp = new List<InternationalBankAccountNumber> { InternationalBankAccountNumber.Empty, item3, item2, item0, item1, InternationalBankAccountNumber.Empty };
        var exp = new List<InternationalBankAccountNumber> { item3, item2, item1, item0, InternationalBankAccountNumber.Empty, InternationalBankAccountNumber.Empty };
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
    public void Length_DefaultValue_0()
    {
        var exp = 0;
        var act = InternationalBankAccountNumber.Empty.Length;
        act.Should().Be(exp);
    }
    [Test]
    public void Length_Unknown_0()
    {
        var exp = 0;
        var act = InternationalBankAccountNumber.Unknown.Length;
        act.Should().Be(exp);
    }
    [Test]
    public void Length_TestStruct_IntValue()
    {
        var exp = 18;
        var act = TestStruct.Length;
        act.Should().Be(exp);
    }


    [Test]
    public void Country_Empty_Null()
    {
        var exp = Country.Empty;
        var act = InternationalBankAccountNumber.Empty.Country;
        act.Should().Be(exp);
    }
    [Test]
    public void Country_Unknown_Null()
    {
        var exp = Country.Unknown;
        var act = InternationalBankAccountNumber.Unknown.Country;
        act.Should().Be(exp);
    }
    [Test]
    public void Country_TestStruct_Null()
    {
        var exp = Country.NL;
        var act = TestStruct.Country;
        act.Should().Be(exp);
    }

    #endregion
}

[Serializable]
public class InternationalBankAccountNumberSerializeObject
{
    public int Id { get; set; }
    public InternationalBankAccountNumber Obj { get; set; }
    public DateTime Date { get; set; }
}
