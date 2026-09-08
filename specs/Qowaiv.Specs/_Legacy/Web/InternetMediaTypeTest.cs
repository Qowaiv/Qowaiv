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
    public void Empty_None_EqualsDefault() => InternetMediaType.Empty.Should().Be(default);

    #endregion

    #region internet media type IsEmpty tests

    /// <summary>InternetMediaType.IsEmpty() should be true for the default of internet media type.</summary>
    [Test]
    public void IsEmpty_Default_IsTrue() => default(InternetMediaType).IsEmpty().Should().BeTrue();
    /// <summary>InternetMediaType.IsEmpty() should be false for InternetMediaType.Unknown.</summary>
    [Test]
    public void IsEmpty_Unknown_IsFalse() => InternetMediaType.Unknown.IsEmpty().Should().BeFalse();
    /// <summary>InternetMediaType.IsEmpty() should be false for the TestStruct.</summary>
    [Test]
    public void IsEmpty_TestStruct_IsFalse() => TestStruct.IsEmpty().Should().BeFalse();

    /// <summary>InternetMediaType.IsUnknown() should be false for the default of internet media type.</summary>
    [Test]
    public void IsUnknown_Default_IsFalse() => default(InternetMediaType).IsUnknown().Should().BeFalse();
    /// <summary>InternetMediaType.IsUnknown() should be true for InternetMediaType.Unknown.</summary>
    [Test]
    public void IsUnknown_Unknown_IsTrue() => InternetMediaType.Unknown.IsUnknown().Should().BeTrue();
    /// <summary>InternetMediaType.IsUnknown() should be false for the TestStruct.</summary>
    [Test]
    public void IsUnknown_TestStruct_IsFalse() => TestStruct.IsUnknown().Should().BeFalse();

    /// <summary>InternetMediaType.IsEmptyOrUnknown() should be true for the default of internet media type.</summary>
    [Test]
    public void IsEmptyOrUnknown_Default_IsFalse() => default(InternetMediaType).IsEmptyOrUnknown().Should().BeTrue();
    /// <summary>InternetMediaType.IsEmptyOrUnknown() should be true for InternetMediaType.Unknown.</summary>
    [Test]
    public void IsEmptyOrUnknown_Unknown_IsTrue() => InternetMediaType.Unknown.IsEmptyOrUnknown().Should().BeTrue();
    /// <summary>InternetMediaType.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
    [Test]
    public void IsEmptyOrUnknown_TestStruct_IsFalse() => TestStruct.IsEmptyOrUnknown().Should().BeFalse();

    #endregion

    #region IEquatable tests

    [Test]
    public void Equals_EmptyEmpty_IsTrue() => InternetMediaType.Empty.Equals(InternetMediaType.Empty).Should().BeTrue();

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        var l = InternetMediaType.Parse("application/x-chess-pgn");
        var r = InternetMediaType.Parse("application/X-chess-PGN");

        l.Equals(r).Should().BeTrue();
    }

    [Test]
    public void Equals_TestStructTestStruct_IsTrue() => TestStruct.Equals(TestStruct).Should().BeTrue();

    [Test]
    public void Equals_TestStructEmpty_IsFalse() => TestStruct.Equals(InternetMediaType.Empty).Should().BeFalse();

    [Test]
    public void Equals_EmptyTestStruct_IsFalse() => InternetMediaType.Empty.Equals(TestStruct).Should().BeFalse();

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
