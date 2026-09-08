namespace Qowaiv.UnitTests.Financial;

/// <summary>Tests the BIC SVO.</summary>
public class BusinessIdentifierCodeTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly BusinessIdentifierCode TestStruct = BusinessIdentifierCode.Parse("AEGONL2UXXX");

    #region BIC const tests

    /// <summary>BusinessIdentifierCode.Empty should be equal to the default of BIC.</summary>
    [Test]
    public void Empty_None_EqualsDefault() => BusinessIdentifierCode.Empty.Should().Be(default);

    #endregion

    #region BIC IsEmpty tests

    /// <summary>BusinessIdentifierCode.IsEmpty() should be true for the default of BIC.</summary>
    [Test]
    public void IsEmpty_Default_IsTrue() => default(BusinessIdentifierCode).IsEmpty().Should().BeTrue();
    /// <summary>BusinessIdentifierCode.IsEmpty() should be false for BusinessIdentifierCode.Unknown.</summary>
    [Test]
    public void IsEmpty_Unknown_IsFalse() => BusinessIdentifierCode.Unknown.IsEmpty().Should().BeFalse();
    /// <summary>BusinessIdentifierCode.IsEmpty() should be false for the TestStruct.</summary>
    [Test]
    public void IsEmpty_TestStruct_IsFalse() => TestStruct.IsEmpty().Should().BeFalse();

    /// <summary>BusinessIdentifierCode.IsUnknown() should be false for the default of BIC.</summary>
    [Test]
    public void IsUnknown_Default_IsFalse() => default(BusinessIdentifierCode).IsUnknown().Should().BeFalse();
    /// <summary>BusinessIdentifierCode.IsUnknown() should be true for BusinessIdentifierCode.Unknown.</summary>
    [Test]
    public void IsUnknown_Unknown_IsTrue() => BusinessIdentifierCode.Unknown.IsUnknown().Should().BeTrue();
    /// <summary>BusinessIdentifierCode.IsUnknown() should be false for the TestStruct.</summary>
    [Test]
    public void IsUnknown_TestStruct_IsFalse() => TestStruct.IsUnknown().Should().BeFalse();

    /// <summary>BusinessIdentifierCode.IsEmptyOrUnknown() should be true for the default of BIC.</summary>
    [Test]
    public void IsEmptyOrUnknown_Default_IsFalse() => default(BusinessIdentifierCode).IsEmptyOrUnknown().Should().BeTrue();
    /// <summary>BusinessIdentifierCode.IsEmptyOrUnknown() should be true for BusinessIdentifierCode.Unknown.</summary>
    [Test]
    public void IsEmptyOrUnknown_Unknown_IsTrue() => BusinessIdentifierCode.Unknown.IsEmptyOrUnknown().Should().BeTrue();
    /// <summary>BusinessIdentifierCode.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
    [Test]
    public void IsEmptyOrUnknown_TestStruct_IsFalse() => TestStruct.IsEmptyOrUnknown().Should().BeFalse();

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
        Should.BeEqual(str, val.ToString(), "Value");
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

    [Test]
    public void Equals_EmptyEmpty_IsTrue() => BusinessIdentifierCode.Empty.Equals(BusinessIdentifierCode.Empty).Should().BeTrue();

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        var l = BusinessIdentifierCode.Parse("AEGONL2UXXX", CultureInfo.InvariantCulture);
        var r = BusinessIdentifierCode.Parse("AEgonL2Uxxx", CultureInfo.InvariantCulture);

        l.Equals(r).Should().BeTrue();
    }

    [Test]
    public void Equals_TestStructTestStruct_IsTrue() => TestStruct.Equals(TestStruct).Should().BeTrue();

    [Test]
    public void Equals_TestStructEmpty_IsFalse() => TestStruct.Equals(BusinessIdentifierCode.Empty).Should().BeFalse();

    [Test]
    public void Equals_EmptyTestStruct_IsFalse() => BusinessIdentifierCode.Empty.Equals(TestStruct).Should().BeFalse();

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
