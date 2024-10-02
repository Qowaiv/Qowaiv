namespace Qowaiv.UnitTests.Web;

/// <summary>Tests the Internet media type SVO.</summary>
public class InternetMediaTypeTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly InternetMediaType TestStruct = InternetMediaType.Parse("application/x-chess-pgn");

    /// <summary>Represents text/html.</summary>
    public static readonly InternetMediaType TextHtml = InternetMediaType.Parse("text/html");

    /// <summary>Represents cooltalk (x-conference/x-cooltalk).</summary>
    public static readonly InternetMediaType XConferenceXCooltalk = InternetMediaType.Parse("x-conference/x-cooltalk");

    #region internet media type const tests

    /// <summary>InternetMediaType.Empty should be equal to the default of internet media type.</summary>
    [Test]
    public void Empty_None_EqualsDefault()
    {
        InternetMediaType.Empty.Should().Be(default);
    }

    #endregion

    #region internet media type IsEmpty tests

    /// <summary>InternetMediaType.IsEmpty() should be true for the default of internet media type.</summary>
    [Test]
    public void IsEmpty_Default_IsTrue()
    {
        default(InternetMediaType).IsEmpty().Should().BeTrue();
    }
    /// <summary>InternetMediaType.IsEmpty() should be false for InternetMediaType.Unknown.</summary>
    [Test]
    public void IsEmpty_Unknown_IsFalse()
    {
        InternetMediaType.Unknown.IsEmpty().Should().BeFalse();
    }
    /// <summary>InternetMediaType.IsEmpty() should be false for the TestStruct.</summary>
    [Test]
    public void IsEmpty_TestStruct_IsFalse()
    {
        TestStruct.IsEmpty().Should().BeFalse();
    }

    /// <summary>InternetMediaType.IsUnknown() should be false for the default of internet media type.</summary>
    [Test]
    public void IsUnknown_Default_IsFalse()
    {
        default(InternetMediaType).IsUnknown().Should().BeFalse();
    }
    /// <summary>InternetMediaType.IsUnknown() should be true for InternetMediaType.Unknown.</summary>
    [Test]
    public void IsUnknown_Unknown_IsTrue()
    {
        InternetMediaType.Unknown.IsUnknown().Should().BeTrue();
    }
    /// <summary>InternetMediaType.IsUnknown() should be false for the TestStruct.</summary>
    [Test]
    public void IsUnknown_TestStruct_IsFalse()
    {
        TestStruct.IsUnknown().Should().BeFalse();
    }

    /// <summary>InternetMediaType.IsEmptyOrUnknown() should be true for the default of internet media type.</summary>
    [Test]
    public void IsEmptyOrUnknown_Default_IsFalse()
    {
        default(InternetMediaType).IsEmptyOrUnknown().Should().BeTrue();
    }
    /// <summary>InternetMediaType.IsEmptyOrUnknown() should be true for InternetMediaType.Unknown.</summary>
    [Test]
    public void IsEmptyOrUnknown_Unknown_IsTrue()
    {
        InternetMediaType.Unknown.IsEmptyOrUnknown().Should().BeTrue();
    }
    /// <summary>InternetMediaType.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
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
        InternetMediaType.TryParse(Nil.String, out var val).Should().BeTrue();
        val.Should().Be(default);
    }

    /// <summary>TryParse string.Empty should be valid.</summary>
    [Test]
    public void TryParse_StringEmpty_IsValid()
    {

        string str = string.Empty;

        InternetMediaType.TryParse(str, out var val).Should().BeTrue();
        val.Should().Be(default);
    }

    /// <summary>TryParse "?" should be valid and the result should be InternetMediaType.Unknown.</summary>
    [Test]
    public void TryParse_question_mark_IsValid()
    {
        string str = "?";

        InternetMediaType.TryParse(str, out var val).Should().BeTrue();
        val.IsUnknown().Should().BeTrue();
    }

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_StringValue_IsValid()
    {
        string str = "application/atom+xml";

        InternetMediaType.TryParse(str, out var val).Should().BeTrue();
        Assert.AreEqual(str, val.ToString(), "Value");
    }

    /// <summary>TryParse with specified string value should be invalid.</summary>
    [Test]
    public void TryParse_StringValue_IsNotValid()
    {
        string str = "string";

        InternetMediaType.TryParse(str, out var val).Should().BeFalse();
        val.Should().Be(default);
    }

    [Test]
    public void Parse_Unknown_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var act = InternetMediaType.Parse("?");
            var exp = InternetMediaType.Unknown;
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
                InternetMediaType.Parse("InvalidInput");
            },
            "Not a valid internet media type");
        }
    }

    [Test]
    public void TryParse_TestStructInput_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var exp = TestStruct;
            var act = InternetMediaType.TryParse(exp.ToString());

            act.Should().Be(exp);
        }
    }

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => InternetMediaType.TryParse("invalid input").Should().BeNull();

    #endregion

    #region (XML) (De)serialization tests

#if NET8_0_OR_GREATER
#else
    [Test]
    public void GetObjectData_SerializationInfo_AreEqual()
    {
        ISerializable obj = TestStruct;
        var info = new SerializationInfo(typeof(InternetMediaType), new System.Runtime.Serialization.FormatterConverter());
        obj.GetObjectData(info, default);

        Assert.AreEqual("application/x-chess-pgn", info.GetString("Value"));
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
        var exp = "application/x-chess-pgn";
        act.Should().Be(exp);
    }

    [Test]
    public void XmlDeserialize_XmlString_AreEqual()
    {
        var act = Deserialize.Xml<InternetMediaType>("application/x-chess-pgn");
        act.Should().Be(TestStruct);
    }

#if NET8_0_OR_GREATER
#else
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_InternetMediaTypeSerializeObject_AreEqual()
    {
        var input = new InternetMediaTypeSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new InternetMediaTypeSerializeObject
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
    public void XmlSerializeDeserialize_InternetMediaTypeSerializeObject_AreEqual()
    {
        var input = new InternetMediaTypeSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new InternetMediaTypeSerializeObject
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
    public void DataContractSerializeDeserialize_InternetMediaTypeSerializeObject_AreEqual()
    {
        var input = new InternetMediaTypeSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new InternetMediaTypeSerializeObject
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
        var input = new InternetMediaTypeSerializeObject
        {
            Id = 17,
            Obj = InternetMediaType.Empty,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new InternetMediaTypeSerializeObject
        {
            Id = 17,
            Obj = InternetMediaType.Empty,
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
        var input = new InternetMediaTypeSerializeObject
        {
            Id = 17,
            Obj = InternetMediaType.Empty,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new InternetMediaTypeSerializeObject
        {
            Id = 17,
            Obj = InternetMediaType.Empty,
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
        var act = InternetMediaType.Empty.ToString();
        var exp = "";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString_Unknown_QuestionMark()
    {
        var act = InternetMediaType.Unknown.ToString();
        var exp = "application/octet-stream";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString_CustomFormatter_SupportsCustomFormatting()
    {
        var act = TestStruct.ToString("Unit Test Format", FormatProvider.CustomFormatter);
        var exp = "Unit Test Formatter, value: 'application/x-chess-pgn', format: 'Unit Test Format'";

        act.Should().Be(exp);
    }
    [Test]
    public void ToString_TestStruct_ComplexPattern()
    {
        var act = TestStruct.ToString(string.Empty);
        var exp = "application/x-chess-pgn";
        act.Should().Be(exp);
    }

    #endregion

    #region IEquatable tests

    /// <summary>GetHash should not fail for InternetMediaType.Empty.</summary>
    [Test]
    public void GetHash_Empty_0()
    {
        InternetMediaType.Empty.GetHashCode().Should().Be(0);
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
        InternetMediaType.Empty.Equals(InternetMediaType.Empty).Should().BeTrue();
    }

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        var l = InternetMediaType.Parse("application/x-chess-pgn");
        var r = InternetMediaType.Parse("application/X-chess-PGN");

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
        TestStruct.Equals(InternetMediaType.Empty).Should().BeFalse();
    }

    [Test]
    public void Equals_EmptyTestStruct_IsFalse()
    {
        InternetMediaType.Empty.Equals(TestStruct).Should().BeFalse();
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

    /// <summary>Orders a list of internet media types ascending.</summary>
    [Test]
    public void OrderBy_InternetMediaType_AreEqual()
    {
        var item0 = InternetMediaType.Parse("audio/mp3");
        var item1 = InternetMediaType.Parse("image/jpeg");
        var item2 = InternetMediaType.Parse("text/x-markdown");
        var item3 = InternetMediaType.Parse("video/quicktime");

        var inp = new List<InternetMediaType> { InternetMediaType.Empty, item3, item2, item0, item1, InternetMediaType.Empty };
        var exp = new List<InternetMediaType> { InternetMediaType.Empty, InternetMediaType.Empty, item0, item1, item2, item3 };
        var act = inp.OrderBy(item => item).ToList();

        act.Should().BeEquivalentTo(exp);
    }

    /// <summary>Orders a list of internet media types descending.</summary>
    [Test]
    public void OrderByDescending_InternetMediaType_AreEqual()
    {
        var item0 = InternetMediaType.Parse("audio/mp3");
        var item1 = InternetMediaType.Parse("image/jpeg");
        var item2 = InternetMediaType.Parse("text/x-markdown");
        var item3 = InternetMediaType.Parse("video/quicktime");

        var inp = new List<InternetMediaType> { InternetMediaType.Empty, item3, item2, item0, item1, InternetMediaType.Empty };
        var exp = new List<InternetMediaType> { item3, item2, item1, item0, InternetMediaType.Empty, InternetMediaType.Empty };
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
        var act = InternetMediaType.Empty.Length;
        act.Should().Be(exp);
    }
    [Test]
    public void Length_TestStruct_23()
    {
        var exp = 23;
        var act = TestStruct.Length;
        act.Should().Be(exp);
    }

    [Test]
    public void TopLevel_DefaultValue_0()
    {
        var exp = string.Empty;
        var act = InternetMediaType.Empty.TopLevel;
        act.Should().Be(exp);
    }
    [Test]
    public void TopLevel_TextHtml_Text()
    {
        var exp = "text";
        var act = TextHtml.TopLevel;
        act.Should().Be(exp);
    }
    [Test]
    public void TopLevel_XConferenceXCooltalk_XConference()
    {
        var exp = "x-conference";
        var act = XConferenceXCooltalk.TopLevel;
        act.Should().Be(exp);
    }
    [Test]
    public void TopLevel_TestStruct_Application()
    {
        var exp = "application";
        var act = TestStruct.TopLevel;
        act.Should().Be(exp);
    }

    [Test]
    public void TopLevelType_DefaultValue_0()
    {
        var exp = InternetMediaTopLevelType.None;
        var act = InternetMediaType.Empty.TopLevelType;
        act.Should().Be(exp);
    }
    [Test]
    public void TopLevelType_TextHtml_Text()
    {
        var exp = InternetMediaTopLevelType.Text;
        var act = TextHtml.TopLevelType;
        act.Should().Be(exp);
    }
    [Test]
    public void TopLevelType_XConferenceXCooltalk_Unregistered()
    {
        var exp = InternetMediaTopLevelType.Unregistered;
        var act = XConferenceXCooltalk.TopLevelType;
        act.Should().Be(exp);
    }
    [Test]
    public void TopLevelType_TestStruct_Application()
    {
        var exp = InternetMediaTopLevelType.Application;
        var act = TestStruct.TopLevelType;
        act.Should().Be(exp);
    }

    [Test]
    public void Subtype_DefaultValue_0()
    {
        var exp = string.Empty;
        var act = InternetMediaType.Empty.Subtype;
        act.Should().Be(exp);
    }
    [Test]
    public void Subtype_TextHtml_Html()
    {
        var exp = "html";
        var act = TextHtml.Subtype;
        act.Should().Be(exp);
    }
    [Test]
    public void Subtype_XConferenceXCooltalk_XCooltalk()
    {
        var exp = "x-cooltalk";
        var act = XConferenceXCooltalk.Subtype;
        act.Should().Be(exp);
    }
    [Test]
    public void Subtype_TestStruct_XChessPgn()
    {
        var exp = "x-chess-pgn";
        var act = TestStruct.Subtype;
        act.Should().Be(exp);
    }

    [Test]
    public void IsRegistered_DefaultValue_IsFalse()
    {
        var exp = false;
        var act = InternetMediaType.Empty.IsRegistered;
        act.Should().Be(exp);
    }
    [Test]
    public void IsRegistered_TextHtml_IsTrue()
    {
        var exp = true;
        var act = TextHtml.IsRegistered;
        act.Should().Be(exp);
    }
    [Test]
    public void IsRegistered_XConferenceXCooltalk_IsFalse()
    {
        var exp = false;
        var act = XConferenceXCooltalk.IsRegistered;
        act.Should().Be(exp);
    }
    [Test]
    public void IsRegistered_TestStruct_IsFalse()
    {
        var exp = false;
        var act = TestStruct.IsRegistered;
        act.Should().Be(exp);
    }
    [Test]
    public void IsRegistered_VideoSlashXDotTest_IsFalse()
    {
        var mime = InternetMediaType.Parse("video/x.test");
        var exp = false;
        var act = mime.IsRegistered;
        act.Should().Be(exp);
    }


    [Test]
    public void Suffix_DefaultValue_None()
    {
        var exp = InternetMediaSuffixType.None;
        var act = InternetMediaType.Empty.Suffix;
        act.Should().Be(exp);
    }

    [Test]
    public void Suffix_TestStruct_None()
    {
        var exp = InternetMediaSuffixType.None;
        var act = TestStruct.Suffix;
        act.Should().Be(exp);
    }
    [Test]
    public void Suffix_ApplicationAtomXml_Xml()
    {
        var mime = InternetMediaType.Parse("application/atom+xml");

        var exp = InternetMediaSuffixType.xml;
        var act = mime.Suffix;
        act.Should().Be(exp);
    }

    #endregion
}

[Serializable]
public class InternetMediaTypeSerializeObject
{
    public int Id { get; set; }
    public InternetMediaType Obj { get; set; }
    public DateTime Date { get; set; }
}
