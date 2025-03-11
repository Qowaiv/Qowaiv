namespace Qowaiv.UnitTests;

/// <summary>Tests the house number SVO.</summary>
[TestFixture]
public class HouseNumberTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly HouseNumber TestStruct = 123456789L;

    #region house number const tests

    /// <summary>HouseNumber.Empty should be equal to the default of house number.</summary>
    [Test]
    public void Empty_None_EqualsDefault()
    {
        HouseNumber.Empty.Should().Be(default);
    }

    [Test]
    public void MinValue_None_1()
    {
        HouseNumber.MinValue.Should().Be(HouseNumber.Create(1));
    }

    [Test]
    public void MaxValue_None_999999999()
    {
        HouseNumber.MaxValue.Should().Be(HouseNumber.Create(999999999));
    }

    #endregion

    #region house number IsEmpty tests

    /// <summary>HouseNumber.IsEmpty() should be true for the default of house number.</summary>
    [Test]
    public void IsEmpty_Default_IsTrue()
    {
        default(HouseNumber).IsEmpty().Should().BeTrue();
    }
    /// <summary>HouseNumber.IsEmpty() should be false for HouseNumber.Unknown.</summary>
    [Test]
    public void IsEmpty_Unknown_IsFalse()
    {
        HouseNumber.Unknown.IsEmpty().Should().BeFalse();
    }
    /// <summary>HouseNumber.IsEmpty() should be false for the TestStruct.</summary>
    [Test]
    public void IsEmpty_TestStruct_IsFalse()
    {
        TestStruct.IsEmpty().Should().BeFalse();
    }

    /// <summary>HouseNumber.IsUnknown() should be false for the default of house number.</summary>
    [Test]
    public void IsUnknown_Default_IsFalse()
    {
        default(HouseNumber).IsUnknown().Should().BeFalse();
    }
    /// <summary>HouseNumber.IsUnknown() should be true for HouseNumber.Unknown.</summary>
    [Test]
    public void IsUnknown_Unknown_IsTrue()
    {
        HouseNumber.Unknown.IsUnknown().Should().BeTrue();
    }
    /// <summary>HouseNumber.IsUnknown() should be false for the TestStruct.</summary>
    [Test]
    public void IsUnknown_TestStruct_IsFalse()
    {
        TestStruct.IsUnknown().Should().BeFalse();
    }

    /// <summary>HouseNumber.IsEmptyOrUnknown() should be true for the default of house number.</summary>
    [Test]
    public void IsEmptyOrUnknown_Default_IsFalse()
    {
        default(HouseNumber).IsEmptyOrUnknown().Should().BeTrue();
    }
    /// <summary>HouseNumber.IsEmptyOrUnknown() should be true for HouseNumber.Unknown.</summary>
    [Test]
    public void IsEmptyOrUnknown_Unknown_IsTrue()
    {
        HouseNumber.Unknown.IsEmptyOrUnknown().Should().BeTrue();
    }
    /// <summary>HouseNumber.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
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
        HouseNumber.TryParse(Nil.String, out HouseNumber val).Should().BeTrue();
        val.Should().Be(default);
    }

    /// <summary>TryParse string.Empty should be valid.</summary>
    [Test]
    public void TryParse_StringEmpty_IsValid()
    {
        string str = string.Empty;

        HouseNumber.TryParse(str, out HouseNumber val).Should().BeTrue();
        val.Should().Be(default);
    }

    /// <summary>TryParse "?" should be valid and the result should be HouseNumber.Unknown.</summary>
    [Test]
    public void TryParse_question_mark_IsValid()
    {
        string str = "?";

        HouseNumber.TryParse(str, out HouseNumber val).Should().BeTrue();
        val.IsUnknown().Should().BeTrue();
    }

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_StringValue_IsValid()
    {
        string str = "123";

        HouseNumber.TryParse(str, out HouseNumber val).Should().BeTrue();
        Should.BeEqual(str, val.ToString(), "Value");
    }

    /// <summary>TryParse with specified string value should be invalid.</summary>
    [Test]
    public void TryParse_StringValue_IsNotValid()
    {
        string str = "string";

        HouseNumber.TryParse(str, out HouseNumber val).Should().BeFalse();
        val.Should().Be(default);
    }

    [Test]
    public void Parse_Unknown_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var act = HouseNumber.Parse("?");
            var exp = HouseNumber.Unknown;
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
                HouseNumber.Parse("InvalidInput");
            },
            "Not a valid house number");
        }
    }

    [Test]
    public void TryParse_TestStructInput_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var exp = TestStruct;
            var act = HouseNumber.TryParse(exp.ToString());

            act.Should().Be(exp);
        }
    }

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => HouseNumber.TryParse("invalid input").Should().BeNull();

    #endregion

    #region TryCreate tests

    [Test]
    public void TryCreate_Null_IsEmpty()
    {
        HouseNumber exp = HouseNumber.Empty;
        HouseNumber.TryCreate(null, out HouseNumber act).Should().BeTrue();
        act.Should().Be(exp);
    }
    [Test]
    public void TryCreate_Int32MinValue_IsEmpty()
    {
        HouseNumber exp = HouseNumber.Empty;
        HouseNumber.TryCreate(int.MinValue, out HouseNumber act).Should().BeFalse();
        act.Should().Be(exp);
    }

    [Test]
    public void TryCreate_Int32MinValue_AreEqual()
    {
        var exp = HouseNumber.Empty;
        var act = HouseNumber.TryCreate(int.MinValue);
        act.Should().Be(exp);
    }
    [Test]
    public void TryCreate_Value_AreEqual()
    {
        var exp = TestStruct;
        var act = HouseNumber.TryCreate(123456789);
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
        var info = new SerializationInfo(typeof(HouseNumber), new System.Runtime.Serialization.FormatterConverter());
        obj.GetObjectData(info, default);

        info.GetInt32("Value").Should().Be(123456789);
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
        var exp = "123456789";
        act.Should().Be(exp);
    }

    [Test]
    public void XmlDeserialize_XmlString_AreEqual()
    {
        var act = Deserialize.Xml<HouseNumber>("123456789");
        act.Should().Be(TestStruct);
    }

#if NET8_0_OR_GREATER
#else
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_HouseNumberSerializeObject_AreEqual()
    {
        var input = new HouseNumberSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new HouseNumberSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var act = SerializeDeserialize.Binary(input);
        Should.BeEqual(exp.Id, act.Id, "Id");
        Should.BeEqual(exp.Obj, act.Obj, "Obj");
        Should.BeEqual(exp.Date, act.Date, "Date");
    }
#endif

    [Test]
    public void XmlSerializeDeserialize_HouseNumberSerializeObject_AreEqual()
    {
        var input = new HouseNumberSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new HouseNumberSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var act = SerializeDeserialize.Xml(input);
        Should.BeEqual(exp.Id, act.Id, "Id");
        Should.BeEqual(exp.Obj, act.Obj, "Obj");
        Should.BeEqual(exp.Date, act.Date, "Date");
    }
    [Test]
    public void DataContractSerializeDeserialize_HouseNumberSerializeObject_AreEqual()
    {
        var input = new HouseNumberSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new HouseNumberSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var act = SerializeDeserialize.DataContract(input);
        Should.BeEqual(exp.Id, act.Id, "Id");
        Should.BeEqual(exp.Obj, act.Obj, "Obj");
        Should.BeEqual(exp.Date, act.Date, "Date");
    }

#if NET8_0_OR_GREATER
#else
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_Empty_AreEqual()
    {
        var input = new HouseNumberSerializeObject
        {
            Id = 17,
            Obj = HouseNumber.Empty,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new HouseNumberSerializeObject
        {
            Id = 17,
            Obj = HouseNumber.Empty,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var act = SerializeDeserialize.Binary(input);
        Should.BeEqual(exp.Id, act.Id, "Id");
        Should.BeEqual(exp.Obj, act.Obj, "Obj");
        Should.BeEqual(exp.Date, act.Date, "Date");
    }
#endif

    [Test]
    public void XmlSerializeDeserialize_Empty_AreEqual()
    {
        var input = new HouseNumberSerializeObject
        {
            Id = 17,
            Obj = HouseNumber.Empty,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new HouseNumberSerializeObject
        {
            Id = 17,
            Obj = HouseNumber.Empty,
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

    #region IFormattable / Tostring tests

    [Test]
    public void ToString_Empty_IsStringEmpty()
    {
        var act = HouseNumber.Empty.ToString();
        var exp = "";
        act.Should().Be(exp);
    }
    [Test]
    public void ToString_Unknown_QuestionMark()
    {
        var act = HouseNumber.Unknown.ToString();
        var exp = "?";
        act.Should().Be(exp);
    }
    [Test]
    public void ToString_CustomFormatter_SupportsCustomFormatting()
    {
        var act = TestStruct.ToString("#,##0", FormatProvider.CustomFormatter);
        var exp = "Unit Test Formatter, value: '123,456,789', format: '#,##0'";

        act.Should().Be(exp);
    }
    [Test]
    public void ToString_TestStruct_ComplexPattern()
    {
        var act = TestStruct.ToString(string.Empty);
        var exp = "123456789";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString_FormatValueDutchBelgium_AreEqual()
    {
        using (TestCultures.nl_BE.Scoped())
        {
            var act = HouseNumber.Parse("800").ToString("0000");
            var exp = "0800";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void ToString_FormatValueEnglishGreatBritain_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var act = HouseNumber.Parse("800").ToString("0000");
            var exp = "0800";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void ToString_FormatValueSpanishEcuador_AreEqual()
    {
        var act = HouseNumber.Parse("1700").ToString("00000.0", new CultureInfo("es-EC"));
        var exp = "01700,0";
        act.Should().Be(exp);
    }

    #endregion

    #region IComparable tests

    /// <summary>Orders a list of house numbers ascending.</summary>
    [Test]
    public void OrderBy_HouseNumber_AreEqual()
    {
        HouseNumber item0 = 1;
        HouseNumber item1 = 12;
        HouseNumber item2 = 123;
        HouseNumber item3 = 1234;

        var inp = new List<HouseNumber> { HouseNumber.Empty, item3, item2, item0, item1, HouseNumber.Empty };
        var exp = new List<HouseNumber> { HouseNumber.Empty, HouseNumber.Empty, item0, item1, item2, item3 };
        var act = inp.OrderBy(item => item).ToList();

        act.Should().BeEquivalentTo(exp);
    }

    /// <summary>Orders a list of house numbers descending.</summary>
    [Test]
    public void OrderByDescending_HouseNumber_AreEqual()
    {
        HouseNumber item0 = 1;
        HouseNumber item1 = 12;
        HouseNumber item2 = 123;
        HouseNumber item3 = 1234;

        var inp = new List<HouseNumber> { HouseNumber.Empty, item3, item2, item0, item1, HouseNumber.Empty };
        var exp = new List<HouseNumber> { item3, item2, item1, item0, HouseNumber.Empty, HouseNumber.Empty };
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
        HouseNumber l = 17;
        HouseNumber r = 19;

        (l < r).Should().BeTrue();
    }
    [Test]
    public void GreaterThan_21LT19_IsTrue()
    {
        HouseNumber l = 21;
        HouseNumber r = 19;

        (l > r).Should().BeTrue();
    }

    [Test]
    public void LessThanOrEqual_17LT19_IsTrue()
    {
        HouseNumber l = 17;
        HouseNumber r = 19;

        (l <= r).Should().BeTrue();
    }
    [Test]
    public void GreaterThanOrEqual_21LT19_IsTrue()
    {
        HouseNumber l = 21;
        HouseNumber r = 19;

        (l >= r).Should().BeTrue();
    }

    [Test]
    public void LessThanOrEqual_17LT17_IsTrue()
    {
        HouseNumber l = 17;
        HouseNumber r = 17;

        (l <= r).Should().BeTrue();
    }
    [Test]
    public void GreaterThanOrEqual_21LT21_IsTrue()
    {
        HouseNumber l = 21;
        HouseNumber r = 21;

        (l >= r).Should().BeTrue();
    }
    #endregion

    #region Casting tests

    [Test]
    public void Explicit_Int32ToHouseNumber_AreEqual()
    {
        var exp = TestStruct;
        var act = (HouseNumber)123456789;

        act.Should().Be(exp);
    }
    [Test]
    public void Explicit_HouseNumberToInt32_AreEqual()
    {
        var exp = 123456789;
        var act = (int)TestStruct;

        act.Should().Be(exp);
    }
    #endregion

    #region Properties

    [Test]
    public void IsOdd_Empty_IsFalse()
    {
        HouseNumber.Empty.IsOdd.Should().BeFalse();
    }

    [Test]
    public void IsOdd_Unknown_IsFalse()
    {
        HouseNumber.Unknown.IsOdd.Should().BeFalse();
    }

    [Test]
    public void IsOdd_TestStruct_IsTrue()
    {
        TestStruct.IsOdd.Should().BeTrue();
    }

    [Test]
    public void IsEven_Empty_IsFalse()
    {
        HouseNumber.Empty.IsEven.Should().BeFalse();
    }

    [Test]
    public void IsEven_Unknown_IsFalse()
    {
        HouseNumber.Unknown.IsEven.Should().BeFalse();
    }

    [Test]
    public void IsEven_TestStruct_IsFalse()
    {
        TestStruct.IsEven.Should().BeFalse();
    }

    [Test]
    public void IsEven_1234_IsTrue()
    {
        HouseNumber.Create(1234).IsEven.Should().BeTrue();
    }


    [Test]
    public void Length_Empty_0()
    {
        var act = HouseNumber.Empty.Length;
        var exp = 0;
        act.Should().Be(exp);
    }

    [Test]
    public void Length_Unknown_0()
    {
        var act = HouseNumber.Unknown.Length;
        var exp = 0;
        act.Should().Be(exp);
    }

    [Test]
    public void Length_TestStruct_9()
    {
        var act = TestStruct.Length;
        var exp = 9;
        act.Should().Be(exp);
    }

    [Test]
    public void Length_1234_4()
    {
        var act = HouseNumber.Create(1234).Length;
        var exp = 4;
        act.Should().Be(exp);
    }
    #endregion
}

[Serializable]
public class HouseNumberSerializeObject
{
    public int Id { get; set; }
    public HouseNumber Obj { get; set; }
    public DateTime Date { get; set; }
}
