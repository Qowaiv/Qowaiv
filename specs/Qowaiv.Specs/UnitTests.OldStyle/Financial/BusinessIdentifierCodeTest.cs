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
        Assert.AreEqual(default(BusinessIdentifierCode), BusinessIdentifierCode.Empty);
    }

    #endregion

    #region BIC IsEmpty tests

    /// <summary>BusinessIdentifierCode.IsEmpty() should be true for the default of BIC.</summary>
    [Test]
    public void IsEmpty_Default_IsTrue()
    {
        Assert.IsTrue(default(BusinessIdentifierCode).IsEmpty());
    }
    /// <summary>BusinessIdentifierCode.IsEmpty() should be false for BusinessIdentifierCode.Unknown.</summary>
    [Test]
    public void IsEmpty_Unknown_IsFalse()
    {
        Assert.IsFalse(BusinessIdentifierCode.Unknown.IsEmpty());
    }
    /// <summary>BusinessIdentifierCode.IsEmpty() should be false for the TestStruct.</summary>
    [Test]
    public void IsEmpty_TestStruct_IsFalse()
    {
        Assert.IsFalse(TestStruct.IsEmpty());
    }

    /// <summary>BusinessIdentifierCode.IsUnknown() should be false for the default of BIC.</summary>
    [Test]
    public void IsUnknown_Default_IsFalse()
    {
        Assert.IsFalse(default(BusinessIdentifierCode).IsUnknown());
    }
    /// <summary>BusinessIdentifierCode.IsUnknown() should be true for BusinessIdentifierCode.Unknown.</summary>
    [Test]
    public void IsUnknown_Unknown_IsTrue()
    {
        Assert.IsTrue(BusinessIdentifierCode.Unknown.IsUnknown());
    }
    /// <summary>BusinessIdentifierCode.IsUnknown() should be false for the TestStruct.</summary>
    [Test]
    public void IsUnknown_TestStruct_IsFalse()
    {
        Assert.IsFalse(TestStruct.IsUnknown());
    }

    /// <summary>BusinessIdentifierCode.IsEmptyOrUnknown() should be true for the default of BIC.</summary>
    [Test]
    public void IsEmptyOrUnknown_Default_IsFalse()
    {
        Assert.IsTrue(default(BusinessIdentifierCode).IsEmptyOrUnknown());
    }
    /// <summary>BusinessIdentifierCode.IsEmptyOrUnknown() should be true for BusinessIdentifierCode.Unknown.</summary>
    [Test]
    public void IsEmptyOrUnknown_Unknown_IsTrue()
    {
        Assert.IsTrue(BusinessIdentifierCode.Unknown.IsEmptyOrUnknown());
    }
    /// <summary>BusinessIdentifierCode.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
    [Test]
    public void IsEmptyOrUnknown_TestStruct_IsFalse()
    {
        Assert.IsFalse(TestStruct.IsEmptyOrUnknown());
    }

    #endregion

    #region TryParse tests

    /// <summary>TryParse null should be valid.</summary>
    [Test]
    public void TryParse_Null_IsValid()
    {
        Assert.IsTrue(BusinessIdentifierCode.TryParse(Nil.String, out BusinessIdentifierCode val), "Valid");
        Assert.AreEqual(string.Empty, val.ToString(), "Value");
    }

    /// <summary>TryParse string.Empty should be valid.</summary>
    [Test]
    public void TryParse_StringEmpty_IsValid()
    {
        string str = string.Empty;
        Assert.IsTrue(BusinessIdentifierCode.TryParse(str, out BusinessIdentifierCode val), "Valid");
        Assert.AreEqual(string.Empty, val.ToString(), "Value");
    }

    /// <summary>TryParse "?" should be valid and the result should be BusinessIdentifierCode.Unknown.</summary>
    [Test]
    public void TryParse_question_mark_IsValid()
    {
        string str = "?";
        Assert.IsTrue(BusinessIdentifierCode.TryParse(str, out BusinessIdentifierCode val), "Valid");
        Assert.IsTrue(val.IsUnknown(), "Value");
    }

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_StringValue_IsValid()
    {
        string str = "AEGONL2UXXX";
        Assert.IsTrue(BusinessIdentifierCode.TryParse(str, out BusinessIdentifierCode val), "Valid");
        Assert.AreEqual(str, val.ToString(), "Value");
    }

    /// <summary>TryParse with specified string value should be invalid.</summary>
    [Test]
    public void TryParse_StringValue_IsNotValid()
    {
        string str = "string";
        Assert.IsFalse(BusinessIdentifierCode.TryParse(str, out BusinessIdentifierCode val), "Valid");
        Assert.AreEqual(string.Empty, val.ToString(), "Value");
    }

    [Test]
    public void Parse_Unknown_AreEqual()
    {
        using (TestCultures.En_GB.Scoped())
        {
            var act = BusinessIdentifierCode.Parse("?");
            var exp = BusinessIdentifierCode.Unknown;
            Assert.AreEqual(exp, act);
        }
    }

    [Test]
    public void Parse_InvalidInput_ThrowsFormatException()
    {
        using (TestCultures.En_GB.Scoped())
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
        using (TestCultures.En_GB.Scoped())
        {
            var exp = TestStruct;
            var act = BusinessIdentifierCode.TryParse(exp.ToString());

            Assert.AreEqual(exp, act);
        }
    }

    [Test]
    public void from_invalid_as_null_with_TryParse()
       => BusinessIdentifierCode.TryParse("invalid input").Should().BeNull();

    #endregion

    #region (XML) (De)serialization tests

    [Test]
    public void GetObjectData_SerializationInfo_AreEqual()
    {
        ISerializable obj = TestStruct;
        var info = new SerializationInfo(typeof(BusinessIdentifierCode), new FormatterConverter());
        obj.GetObjectData(info, default);

        Assert.AreEqual(TestStruct.ToString(), info.GetString("Value"));
    }

#if NET8_0_OR_GREATER
#else
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_TestStruct_AreEqual()
    {
        var input = TestStruct;
        var exp = TestStruct;
        var act = SerializeDeserialize.Binary(input);
        Assert.AreEqual(exp, act);
    }
#endif

    [Test]
    public void DataContractSerializeDeserialize_TestStruct_AreEqual()
    {
        var input = TestStruct;
        var exp = TestStruct;
        var act = SerializeDeserialize.DataContract(input);
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void XmlSerialize_TestStruct_AreEqual()
    {
        var act = Serialize.Xml(TestStruct);
        var exp = "AEGONL2UXXX";
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void XmlDeserialize_XmlString_AreEqual()
    {
        var act = Deserialize.Xml<BusinessIdentifierCode>("AEGONL2UXXX");
        Assert.AreEqual(TestStruct, act);
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
        Assert.IsNull(obj.GetSchema());
    }

    #endregion

    #region IFormattable / ToString tests

    [Test]
    public void ToString_Empty_StringEmpty()
    {
        var act = BusinessIdentifierCode.Empty.ToString();
        var exp = "";
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void ToString_Unknown_QuestionMark()
    {
        var act = BusinessIdentifierCode.Unknown.ToString();
        var exp = "?";
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void ToString_CustomFormatter_SupportsCustomFormatting()
    {
        var act = TestStruct.ToString("Unit Test Format", FormatProvider.CustomFormatter);
        var exp = "Unit Test Formatter, value: 'AEGONL2UXXX', format: 'Unit Test Format'";

        Assert.AreEqual(exp, act);
    }
    [Test]
    public void ToString_TestStruct_ComplexPattern()
    {
        var act = TestStruct.ToString(string.Empty);
        var exp = "AEGONL2UXXX";
        Assert.AreEqual(exp, act);
    }

    #endregion

    #region IEquatable tests

    /// <summary>GetHash should not fail for BusinessIdentifierCode.Empty.</summary>
    [Test]
    public void GetHash_Empty_Hash()
    {
        Assert.AreEqual(0, BusinessIdentifierCode.Empty.GetHashCode());
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
        Assert.IsTrue(BusinessIdentifierCode.Empty.Equals(BusinessIdentifierCode.Empty));
    }

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        var l = BusinessIdentifierCode.Parse("AEGONL2UXXX", CultureInfo.InvariantCulture);
        var r = BusinessIdentifierCode.Parse("AEgonL2Uxxx", CultureInfo.InvariantCulture);

        Assert.IsTrue(l.Equals(r));
    }

    [Test]
    public void Equals_TestStructTestStruct_IsTrue()
    {
        Assert.IsTrue(TestStruct.Equals(TestStruct));
    }

    [Test]
    public void Equals_TestStructEmpty_IsFalse()
    {
        Assert.IsFalse(TestStruct.Equals(BusinessIdentifierCode.Empty));
    }

    [Test]
    public void Equals_EmptyTestStruct_IsFalse()
    {
        Assert.IsFalse(BusinessIdentifierCode.Empty.Equals(TestStruct));
    }

    [Test]
    public void Equals_TestStructObjectTestStruct_IsTrue()
    {
        Assert.IsTrue(TestStruct.Equals((object)TestStruct));
    }

    [Test]
    public void Equals_TestStructNull_IsFalse()
    {
        Assert.IsFalse(TestStruct.Equals(null));
    }

    [Test]
    public void Equals_TestStructObject_IsFalse()
    {
        Assert.IsFalse(TestStruct.Equals(new object()));
    }

    [Test]
    public void OperatorIs_TestStructTestStruct_IsTrue()
    {
        var l = TestStruct;
        var r = TestStruct;
        Assert.IsTrue(l == r);
    }

    [Test]
    public void OperatorIsNot_TestStructTestStruct_IsFalse()
    {
        var l = TestStruct;
        var r = TestStruct;
        Assert.IsFalse(l != r);
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

        CollectionAssert.AreEqual(exp, act);
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

        CollectionAssert.AreEqual(exp, act);
    }

    /// <summary>Compare with a to object casted instance should be fine.</summary>
    [Test]
    public void CompareTo_ObjectTestStruct_0()
    {
        object other = TestStruct;

        var exp = 0;
        var act = TestStruct.CompareTo(other);

        Assert.AreEqual(exp, act);
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
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void Length_Unknown_0()
    {
        var exp = 0;
        var act = BusinessIdentifierCode.Unknown.Length;
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void Length_TestStruct_IntValue()
    {
        var exp = 11;
        var act = TestStruct.Length;
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void BusinessCode_DefaultValue_StringEmpty()
    {
        var exp = "";
        var act = BusinessIdentifierCode.Empty.Business;
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void BusinessCode_Unknown_StringEmpty()
    {
        var exp = "";
        var act = BusinessIdentifierCode.Unknown.Business;
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void BusinessCode_has_length_of_four()
        => TestStruct.Business.Should().Be("AEGO");

    [Test]
    public void Country_DefaultValue_CountryEmpty()
    {
        var exp = Country.Empty;
        var act = BusinessIdentifierCode.Empty.Country;
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void Country_Unknown_CountryUnknown()
    {
        var exp = Country.Unknown;
        var act = BusinessIdentifierCode.Unknown.Country;
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void Country_TestStruct_NL()
    {
        var exp = Country.NL;
        var act = TestStruct.Country;
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void LocationCode_DefaultValue_StringEmpty()
    {
        var exp = "";
        var act = BusinessIdentifierCode.Empty.Location;
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void LocationCode_Unknown_StringEmpty()
    {
        var exp = "";
        var act = BusinessIdentifierCode.Unknown.Location;
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void LocationCode_TestStruct_NL()
    {
        var exp = "2U";
        var act = TestStruct.Location;
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void BranchCode_DefaultValue_StringEmpty()
    {
        var exp = "";
        var act = BusinessIdentifierCode.Empty.Branch;
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void BranchCode_Unknown_StringEmpty()
    {
        var exp = "";
        var act = BusinessIdentifierCode.Unknown.Branch;
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void BranchCode_TestStruct_NL()
    {
        var exp = "XXX";
        var act = TestStruct.Branch;
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void BranchCode_empty_for_BIC_without_one()
    {
        var exp = "";
        var act = BusinessIdentifierCode.Parse("AEGONL2U").Branch;
        Assert.AreEqual(exp, act);
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
