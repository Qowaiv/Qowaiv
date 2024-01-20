namespace Qowaiv.UnitTests.Financial;

/// <summary>Tests the BIC SVO.</summary>
public class BusinessIdentifierCodeTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly BusinessIdentifierCode TestStruct = BusinessIdentifierCode.Parse("AEGONL2UXXX");

    #region BIC const tests

    /// <summary>BusinessIdentifierCode.Empty should be equal to the default of BIC.</summary>
    [Test]
    public void Empty_None_EqualsDefault()
    {
        BusinessIdentifierCode.Empty.Should().Be(default);
    }

    #endregion

    #region BIC IsEmpty tests

    /// <summary>BusinessIdentifierCode.IsEmpty() should be true for the default of BIC.</summary>
    [Test]
    public void IsEmpty_Default_IsTrue()
    {
        default(BusinessIdentifierCode).IsEmpty().Should().BeTrue();
    }
    /// <summary>BusinessIdentifierCode.IsEmpty() should be false for BusinessIdentifierCode.Unknown.</summary>
    [Test]
    public void IsEmpty_Unknown_IsFalse()
    {
        BusinessIdentifierCode.Unknown.IsEmpty().Should().BeFalse();
    }
    /// <summary>BusinessIdentifierCode.IsEmpty() should be false for the TestStruct.</summary>
    [Test]
    public void IsEmpty_TestStruct_IsFalse()
    {
        TestStruct.IsEmpty().Should().BeFalse();
    }

    /// <summary>BusinessIdentifierCode.IsUnknown() should be false for the default of BIC.</summary>
    [Test]
    public void IsUnknown_Default_IsFalse()
    {
        default(BusinessIdentifierCode).IsUnknown().Should().BeFalse();
    }
    /// <summary>BusinessIdentifierCode.IsUnknown() should be true for BusinessIdentifierCode.Unknown.</summary>
    [Test]
    public void IsUnknown_Unknown_IsTrue()
    {
        BusinessIdentifierCode.Unknown.IsUnknown().Should().BeTrue();
    }
    /// <summary>BusinessIdentifierCode.IsUnknown() should be false for the TestStruct.</summary>
    [Test]
    public void IsUnknown_TestStruct_IsFalse()
    {
        TestStruct.IsUnknown().Should().BeFalse();
    }

    /// <summary>BusinessIdentifierCode.IsEmptyOrUnknown() should be true for the default of BIC.</summary>
    [Test]
    public void IsEmptyOrUnknown_Default_IsFalse()
    {
        default(BusinessIdentifierCode).IsEmptyOrUnknown().Should().BeTrue();
    }
    /// <summary>BusinessIdentifierCode.IsEmptyOrUnknown() should be true for BusinessIdentifierCode.Unknown.</summary>
    [Test]
    public void IsEmptyOrUnknown_Unknown_IsTrue()
    {
        BusinessIdentifierCode.Unknown.IsEmptyOrUnknown().Should().BeTrue();
    }
    /// <summary>BusinessIdentifierCode.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
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
        BusinessIdentifierCode.TryParse(Nil.String, out BusinessIdentifierCode val).Should().BeTrue();
        val.Should().Be(default);
    }

    /// <summary>TryParse string.Empty should be valid.</summary>
    [Test]
    public void TryParse_StringEmpty_IsValid()
    {
        string str = string.Empty;
        BusinessIdentifierCode.TryParse(str, out BusinessIdentifierCode val).Should().BeTrue();
        val.Should().Be(default);
    }

    /// <summary>TryParse "?" should be valid and the result should be BusinessIdentifierCode.Unknown.</summary>
    [Test]
    public void TryParse_question_mark_IsValid()
    {
        string str = "?";
        BusinessIdentifierCode.TryParse(str, out BusinessIdentifierCode val).Should().BeTrue();
        val.IsUnknown().Should().BeTrue();
    }

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_StringValue_IsValid()
    {
        string str = "AEGONL2UXXX";
        BusinessIdentifierCode.TryParse(str, out BusinessIdentifierCode val).Should().BeTrue();
        Assert.AreEqual(str, val.ToString(), "Value");
    }

    /// <summary>TryParse with specified string value should be invalid.</summary>
    [Test]
    public void TryParse_StringValue_IsNotValid()
    {
        string str = "string";
        BusinessIdentifierCode.TryParse(str, out BusinessIdentifierCode val).Should().BeFalse();
        val.Should().Be(default);
    }

    [Test]
    public void Parse_Unknown_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var act = BusinessIdentifierCode.Parse("?");
            var exp = BusinessIdentifierCode.Unknown;
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
                BusinessIdentifierCode.Parse("InvalidInput");
            },
            "Not a valid BIC");
        }
    }

    [Test]
    public void TryParse_TestStructInput_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var exp = TestStruct;
            var act = BusinessIdentifierCode.TryParse(exp.ToString());

            act.Should().Be(exp);
        }
    }

    [Test]
    public void from_invalid_as_null_with_TryParse()
       => BusinessIdentifierCode.TryParse("invalid input").Should().BeNull();

    #endregion

    #region (XML) (De)serialization tests

#if NET8_0_OR_GREATER
#else
    [Test]
    public void GetObjectData_SerializationInfo_AreEqual()
    {
        ISerializable obj = TestStruct;
        var info = new SerializationInfo(typeof(BusinessIdentifierCode), new FormatterConverter());
        obj.GetObjectData(info, default);

        info.GetString("Value").Should().Be(TestStruct.ToString());
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
        var exp = "AEGONL2UXXX";
        act.Should().Be(exp);
    }

    [Test]
    public void XmlDeserialize_XmlString_AreEqual()
    {
        var act = Deserialize.Xml<BusinessIdentifierCode>("AEGONL2UXXX");
        act.Should().Be(TestStruct);
    }

#if NET8_0_OR_GREATER
#else
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_BusinessIdentifierCodeSerializeObject_AreEqual()
    {
        var input = new BusinessIdentifierCodeSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new BusinessIdentifierCodeSerializeObject
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
    public void XmlSerializeDeserialize_BusinessIdentifierCodeSerializeObject_AreEqual()
    {
        var input = new BusinessIdentifierCodeSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new BusinessIdentifierCodeSerializeObject
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
    public void DataContractSerializeDeserialize_BusinessIdentifierCodeSerializeObject_AreEqual()
    {
        var input = new BusinessIdentifierCodeSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new BusinessIdentifierCodeSerializeObject
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
    public void SerializeDeserialize_Empty_AreEqual()
    {
        var input = new BusinessIdentifierCodeSerializeObject
        {
            Id = 17,
            Obj = BusinessIdentifierCode.Empty,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new BusinessIdentifierCodeSerializeObject
        {
            Id = 17,
            Obj = BusinessIdentifierCode.Empty,
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
        var input = new BusinessIdentifierCodeSerializeObject
        {
            Id = 17,
            Obj = BusinessIdentifierCode.Empty,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new BusinessIdentifierCodeSerializeObject
        {
            Id = 17,
            Obj = BusinessIdentifierCode.Empty,
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
        var act = BusinessIdentifierCode.Empty.ToString();
        var exp = "";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString_Unknown_QuestionMark()
    {
        var act = BusinessIdentifierCode.Unknown.ToString();
        var exp = "?";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString_CustomFormatter_SupportsCustomFormatting()
    {
        var act = TestStruct.ToString("Unit Test Format", FormatProvider.CustomFormatter);
        var exp = "Unit Test Formatter, value: 'AEGONL2UXXX', format: 'Unit Test Format'";

        act.Should().Be(exp);
    }
    [Test]
    public void ToString_TestStruct_ComplexPattern()
    {
        var act = TestStruct.ToString(string.Empty);
        var exp = "AEGONL2UXXX";
        act.Should().Be(exp);
    }

    #endregion

    #region IEquatable tests

    /// <summary>GetHash should not fail for BusinessIdentifierCode.Empty.</summary>
    [Test]
    public void GetHash_Empty_Hash()
    {
        BusinessIdentifierCode.Empty.GetHashCode().Should().Be(0);
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
        BusinessIdentifierCode.Empty.Equals(BusinessIdentifierCode.Empty).Should().BeTrue();
    }

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        var l = BusinessIdentifierCode.Parse("AEGONL2UXXX", CultureInfo.InvariantCulture);
        var r = BusinessIdentifierCode.Parse("AEgonL2Uxxx", CultureInfo.InvariantCulture);

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
        TestStruct.Equals(BusinessIdentifierCode.Empty).Should().BeFalse();
    }

    [Test]
    public void Equals_EmptyTestStruct_IsFalse()
    {
        BusinessIdentifierCode.Empty.Equals(TestStruct).Should().BeFalse();
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

    /// <summary>Orders a list of BICs ascending.</summary>
    [Test]
    public void OrderBy_BusinessIdentifierCode_AreEqual()
    {
        var item0 = BusinessIdentifierCode.Parse("AEGONL2UXXX");
        var item1 = BusinessIdentifierCode.Parse("CEBUNL2U");
        var item2 = BusinessIdentifierCode.Parse("DSSBNL22");
        var item3 = BusinessIdentifierCode.Parse("FTSBNL2R");

        var inp = new List<BusinessIdentifierCode> { BusinessIdentifierCode.Empty, item3, item2, item0, item1, BusinessIdentifierCode.Empty };
        var exp = new List<BusinessIdentifierCode> { BusinessIdentifierCode.Empty, BusinessIdentifierCode.Empty, item0, item1, item2, item3 };
        var act = inp.OrderBy(item => item).ToList();

        act.Should().BeEquivalentTo(exp);
    }

    /// <summary>Orders a list of BICs descending.</summary>
    [Test]
    public void OrderByDescending_BusinessIdentifierCode_AreEqual()
    {
        var item0 = BusinessIdentifierCode.Parse("AEGONL2UXXX");
        var item1 = BusinessIdentifierCode.Parse("CEBUNL2U");
        var item2 = BusinessIdentifierCode.Parse("DSSBNL22");
        var item3 = BusinessIdentifierCode.Parse("FTSBNL2R");

        var inp = new List<BusinessIdentifierCode> { BusinessIdentifierCode.Empty, item3, item2, item0, item1, BusinessIdentifierCode.Empty };
        var exp = new List<BusinessIdentifierCode> { item3, item2, item1, item0, BusinessIdentifierCode.Empty, BusinessIdentifierCode.Empty };
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
        var act = BusinessIdentifierCode.Empty.Length;
        act.Should().Be(exp);
    }
    [Test]
    public void Length_Unknown_0()
    {
        var exp = 0;
        var act = BusinessIdentifierCode.Unknown.Length;
        act.Should().Be(exp);
    }
    [Test]
    public void Length_TestStruct_IntValue()
    {
        var exp = 11;
        var act = TestStruct.Length;
        act.Should().Be(exp);
    }

    [Test]
    public void BusinessCode_DefaultValue_StringEmpty()
    {
        var exp = "";
        var act = BusinessIdentifierCode.Empty.Business;
        act.Should().Be(exp);
    }
    [Test]
    public void BusinessCode_Unknown_StringEmpty()
    {
        var exp = "";
        var act = BusinessIdentifierCode.Unknown.Business;
        act.Should().Be(exp);
    }
    [Test]
    public void BusinessCode_has_length_of_four()
        => TestStruct.Business.Should().Be("AEGO");

    [Test]
    public void Country_DefaultValue_CountryEmpty()
    {
        var exp = Country.Empty;
        var act = BusinessIdentifierCode.Empty.Country;
        act.Should().Be(exp);
    }
    [Test]
    public void Country_Unknown_CountryUnknown()
    {
        var exp = Country.Unknown;
        var act = BusinessIdentifierCode.Unknown.Country;
        act.Should().Be(exp);
    }
    [Test]
    public void Country_TestStruct_NL()
    {
        var exp = Country.NL;
        var act = TestStruct.Country;
        act.Should().Be(exp);
    }

    [Test]
    public void LocationCode_DefaultValue_StringEmpty()
    {
        var exp = "";
        var act = BusinessIdentifierCode.Empty.Location;
        act.Should().Be(exp);
    }
    [Test]
    public void LocationCode_Unknown_StringEmpty()
    {
        var exp = "";
        var act = BusinessIdentifierCode.Unknown.Location;
        act.Should().Be(exp);
    }
    [Test]
    public void LocationCode_TestStruct_NL()
    {
        var exp = "2U";
        var act = TestStruct.Location;
        act.Should().Be(exp);
    }

    [Test]
    public void BranchCode_DefaultValue_StringEmpty()
    {
        var exp = "";
        var act = BusinessIdentifierCode.Empty.Branch;
        act.Should().Be(exp);
    }
    [Test]
    public void BranchCode_Unknown_StringEmpty()
    {
        var exp = "";
        var act = BusinessIdentifierCode.Unknown.Branch;
        act.Should().Be(exp);
    }
    [Test]
    public void BranchCode_TestStruct_NL()
    {
        var exp = "XXX";
        var act = TestStruct.Branch;
        act.Should().Be(exp);
    }
    [Test]
    public void BranchCode_empty_for_BIC_without_one()
    {
        var exp = "";
        var act = BusinessIdentifierCode.Parse("AEGONL2U").Branch;
        act.Should().Be(exp);
    }

    #endregion
}

[Serializable]
public class BusinessIdentifierCodeSerializeObject
{
    public int Id { get; set; }
    public BusinessIdentifierCode Obj { get; set; }
    public DateTime Date { get; set; }
}
