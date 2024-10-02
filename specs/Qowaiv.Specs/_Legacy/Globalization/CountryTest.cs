namespace Qowaiv.UnitTests.Globalization;

public class CountryTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly Country TestStruct = Country.VA;

    /// <summary>Country.Empty should be equal to the default of Country.</summary>
    [Test]
    public void Empty_None_EqualsDefault()
    {
        Country.Empty.Should().Be(default);
    }

    #region Current

    [Test]
    public void Current_CurrentCultureDeDE_Germany()
    {
        using (TestCultures.de_DE.Scoped())
        {
            var act = Country.Current;
            var exp = Country.DE;

            act.Should().Be(exp);
        }
    }

    [Test]
    public void Current_CurrentCultureEsEC_Ecuador()
    {
        using (TestCultures.es_EC.Scoped())
        {
            var act = Country.Current;
            var exp = Country.EC;

            act.Should().Be(exp);
        }
    }

    [Test]
    public void Current_CurrentCultureEn_Empty()
    {
        using (TestCultures.en.Scoped())
        {
            var act = Country.Current;
            var exp = Country.Empty;

            act.Should().Be(exp);
        }
    }

    #endregion

    #region Country IsEmpty tests

    /// <summary>Country.IsEmpty() should true for the default of Country.</summary>
    [Test]
    public void IsEmpty_Default_IsTrue()
    {
        default(Country).IsEmpty().Should().BeTrue();
    }

    /// <summary>Country.IsEmpty() should false for the TestStruct.</summary>
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
        Country.TryParse(Nil.String, out Country val).Should().BeTrue();
        val.Should().Be(default);
    }

    /// <summary>TryParse string.Empty should be valid.</summary>
    [Test]
    public void TryParse_StringEmpty_IsValid()
    {
        string str = string.Empty;
        Country.TryParse(str, out Country val).Should().BeTrue();
        val.Should().Be(default);
    }

    /// <summary>TryParse "?" should be valid and the result should be Country.Unknown.</summary>
    [Test]
    public void TryParse_question_mark_IsValid()
    {
        string str = "?";
        Country.TryParse(str, out Country val).Should().BeTrue();
        val.IsUnknown().Should().BeTrue();
    }

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_NullCultureStringValue_IsValid()
    {
        string str = "VA";
        Country.TryParse(str, null, out Country val).Should().BeTrue();
        Assert.AreEqual(str, val.ToString(), "Value");
    }

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_StringValue_IsValid()
    {
        string str = "VA";
        Country.TryParse(str, out Country val).Should().BeTrue();
        Assert.AreEqual(str, val.ToString(), "Value");
    }

    /// <summary>TryParse with specified string value should be invalid.</summary>
    [Test]
    public void TryParse_StringValue_IsNotValid()
    {
        string str = "string";
        Country.TryParse(str, out Country val).Should().BeFalse();
        val.Should().Be(default);
    }

    [Test]
    public void Parse_InvalidInput_ThrowsFormatException()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Assert.Catch<FormatException>
            (() =>
            {
                Country.Parse("InvalidInput");
            },
            "Not a valid country");
        }
    }

    [Test]
    public void TryParse_TestStructInput_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var exp = TestStruct;
            var act = Country.TryParse(exp.ToString());

            act.Should().Be(exp);
        }
    }

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => Country.TryParse("invalid input").Should().BeNull();

    #endregion

    #region Create tests

    [Test]
    public void Create_RegionInfoNull_Empty()
    {
        var exp = Country.Empty;
        var act = Country.Create((RegionInfo?)null);
        act.Should().Be(exp);
    }

    [Test]
    public void Create_CultureInfoNull_Empty()
    {
        var exp = Country.Empty;
        var act = Country.Create((CultureInfo?)null);
        act.Should().Be(exp);
    }

    [Test]
    public void Create_NL_NL()
    {
        var exp = Country.NL;
        var act = Country.Create(new RegionInfo("NL"));
        act.Should().Be(exp);
    }

    [Test]
    public void Create_CultureInfoInvariant_Empty()
    {
        var exp = Country.Empty;
        var act = Country.Create(CultureInfo.InvariantCulture);
        act.Should().Be(exp);
    }

    [Test]
    public void Create_CultureInfoEs_Empty()
    {
        var exp = Country.Empty;
        var act = Country.Create(new CultureInfo("es"));
        act.Should().Be(exp);
    }

    [Test]
    public void Create_CultureInfoEsEC_Empty()
    {
        var exp = Country.EC;
        var act = Country.Create(new CultureInfo("es-EC"));
        act.Should().Be(exp);
    }

    [Test]
    public void Create_CS_CSXX()
    {
        var cs = new RegionInfo("CS");
        var country = Country.Create(cs);
        country.Should().Be(Country.CSXX);
    }

    #endregion

    #region (XML) (De)serialization tests

    [Test]
    public void DataContractSerializeDeserialize_TestStruct_AreEqual()
    {
        var input = CountryTest.TestStruct;
        var exp = CountryTest.TestStruct;
        var act = SerializeDeserialize.DataContract(input);
        act.Should().Be(exp);
    }

    [Test]
    public void XmlSerialize_TestStruct_AreEqual()
    {
        var act = Serialize.Xml(TestStruct);
        var exp = "VA";
        act.Should().Be(exp);
    }

    [Test]
    public void XmlDeserialize_XmlString_AreEqual()
    {
        var act = Deserialize.Xml<Country>("VA");
        act.Should().Be(TestStruct);
    }

#if NET8_0_OR_GREATER
#else
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_CountrySerializeObject_AreEqual()
    {
        var input = new CountrySerializeObject
        {
            Id = 17,
            Obj = CountryTest.TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new CountrySerializeObject
        {
            Id = 17,
            Obj = CountryTest.TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var act = SerializeDeserialize.Binary(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }
#endif

    [Test]
    public void XmlSerializeDeserialize_CountrySerializeObject_AreEqual()
    {
        var input = new CountrySerializeObject
        {
            Id = 17,
            Obj = CountryTest.TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new CountrySerializeObject
        {
            Id = 17,
            Obj = CountryTest.TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var act = SerializeDeserialize.Xml(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }
    [Test]
    public void DataContractSerializeDeserialize_CountrySerializeObject_AreEqual()
    {
        var input = new CountrySerializeObject
        {
            Id = 17,
            Obj = CountryTest.TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new CountrySerializeObject
        {
            Id = 17,
            Obj = CountryTest.TestStruct,
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
        var input = new CountrySerializeObject
        {
            Id = 17,
            Obj = Country.Empty,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new CountrySerializeObject
        {
            Id = 17,
            Obj = Country.Empty,
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
        var input = new CountrySerializeObject
        {
            Id = 17,
            Obj = Country.Empty,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new CountrySerializeObject
        {
            Id = 17,
            Obj = Country.Empty,
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
        var act = TestStruct.ToString("e (3)", FormatProvider.CustomFormatter);
        var exp = "Unit Test Formatter, value: 'Holy See (VAT)', format: 'e (3)'";

        act.Should().Be(exp);
    }

    [Test]
    public void ToString_Empty_IsStringEmpty()
    {
        var act = Country.Empty.ToString();
        var exp = "";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString_Unknown_question_mark()
    {
        var act = Country.Unknown.ToString();
        var exp = "?";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString2_NZ_AreEqual()
    {
        var exp = "MZ";
        var act = Country.MZ.ToString("2");
        act.Should().Be(exp);
    }
    [Test]
    public void ToString3_MZ_AreEqual()
    {
        var exp = "MOZ";
        var act = Country.MZ.ToString("3", new CultureInfo("ja-JP"));
        act.Should().Be(exp);
    }
    [Test]
    public void ToString0_MZ_AreEqual()
    {
        var exp = "508";
        var act = Country.MZ.ToString("0", new CultureInfo("ja-JP"));
        act.Should().Be(exp);
    }

    [Test]
    public void ToStringN_CSHH_AreEqual()
    {
        var exp = "CSHH";
        var act = Country.CSHH.ToString("n", new CultureInfo("ja-JP"));
        act.Should().Be(exp);
    }

    [Test]
    public void ToStringE_MZ_AreEqual()
    {
        var exp = "Mozambique";
        var act = Country.MZ.ToString("e", new CultureInfo("ja-JP"));
        act.Should().Be(exp);
    }

    [Test]
    public void ToStringF_MZ_AreEqual()
    {
        var exp = "モザンビーク";
        var act = Country.MZ.ToString("f", new CultureInfo("ja-JP"));
        act.Should().Be(exp);
    }

    #endregion

    #region IEquatable tests

    [Test]
    public void Equals_EmptyEmpty_IsTrue()
    {
        Country.Empty.Equals(Country.Empty).Should().BeTrue();
    }

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        using (TestCultures.nl_NL.Scoped())
        {
            var l = Country.Parse("België");
            var r = Country.Parse("belgie");

            l.Equals(r).Should().BeTrue();
        }
    }

    [Test]
    public void Equals_TestStructTestStruct_IsTrue()
    {
        CountryTest.TestStruct.Equals(CountryTest.TestStruct).Should().BeTrue();
    }

    [Test]
    public void Equals_TestStructEmpty_IsFalse()
    {
        CountryTest.TestStruct.Equals(Country.Empty).Should().BeFalse();
    }

    [Test]
    public void Equals_EmptyTestStruct_IsFalse()
    {
        Country.Empty.Equals(CountryTest.TestStruct).Should().BeFalse();
    }

    [Test]
    public void Equals_TestStructObjectTestStruct_IsTrue()
    {
        CountryTest.TestStruct.Equals((object)CountryTest.TestStruct).Should().BeTrue();
    }

    [Test]
    public void Equals_TestStructNull_IsFalse()
    {
        CountryTest.TestStruct.Equals(Nil.Object).Should().BeFalse();
    }

    [Test]
    public void Equals_TestStructObject_IsFalse()
    {
        CountryTest.TestStruct.Equals(new object()).Should().BeFalse();
    }

    [Test]
    public void OperatorIs_TestStructTestStruct_IsTrue()
    {
        var l = CountryTest.TestStruct;
        var r = CountryTest.TestStruct;
        (l == r).Should().BeTrue();
    }

    [Test]
    public void OperatorIsNot_TestStructTestStruct_IsFalse()
    {
        var l = CountryTest.TestStruct;
        var r = CountryTest.TestStruct;
        (l != r).Should().BeFalse();
    }

    #endregion

    #region IComparable tests

    /// <summary>Orders a list of Countrys ascending.</summary>
    [Test]
    public void OrderBy_Country_AreEqual()
    {
        var item0 = Country.AE;
        var item1 = Country.BE;
        var item2 = Country.CU;
        var item3 = Country.DO;

        var inp = new List<Country> { Country.Empty, item3, item2, item0, item1, Country.Empty };
        var exp = new List<Country> { Country.Empty, Country.Empty, item0, item1, item2, item3 };
        var act = inp.OrderBy(item => item).ToList();

        act.Should().BeEquivalentTo(exp);
    }

    /// <summary>Orders a list of Countrys descending.</summary>
    [Test]
    public void OrderByDescending_Country_AreEqual()
    {
        var item0 = Country.AE;
        var item1 = Country.BE;
        var item2 = Country.CU;
        var item3 = Country.DO;

        var inp = new List<Country> { Country.Empty, item3, item2, item0, item1, Country.Empty };
        var exp = new List<Country> { item3, item2, item1, item0, Country.Empty, Country.Empty };
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

    #region Casting tests

    [Test]
    public void Implicit_RegionInfoToCountry_AreEqual()
    {
        Country exp = Country.NL;
        Country act = new RegionInfo("NL");

        act.Should().Be(exp);
    }
    [Test]
    public void Explicit_CountryToRegionInfo_AreEqual()
    {
        var exp = new RegionInfo("NL");
        var act = (RegionInfo)Country.NL;

        act.Should().Be(exp);
    }

    #endregion

    #region Properties

    [Test]
    public void CallingCode_Empty_AreEqual()
    {
        var exp = "";
        var act = Country.Empty.CallingCode;
        act.Should().Be(exp);
    }
    [Test]
    public void CallingCode_Unknown_AreEqual()
    {
        var exp = "";
        var act = Country.Unknown.CallingCode;
        act.Should().Be(exp);
    }
    [Test]
    public void CallingCode_TestStruct_AreEqual()
    {
        var exp = "+379";
        var act = TestStruct.CallingCode;
        act.Should().Be(exp);
    }

    [Test]
    public void Name_Empty_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var exp = "";
            var act = Country.Empty.Name;
            act.Should().Be(exp);
        }
    }
    [Test]
    public void Name_Unknown_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var exp = "?";
            var act = Country.Unknown.Name;
            act.Should().Be(exp);
        }
    }
    [Test]
    public void Name_TestStruct_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var exp = "VA";
            var act = TestStruct.Name;
            act.Should().Be(exp);
        }
    }

    [Test]
    public void IsoNumericCode_Empty_AreEqual()
    {
        var exp = 0;
        var act = Country.Empty.IsoNumericCode;
        act.Should().Be(exp);
    }
    [Test]
    public void IsoNumericCode_Unknown_AreEqual()
    {
        var exp = 999;
        var act = Country.Unknown.IsoNumericCode;
        act.Should().Be(exp);
    }
    [Test]
    public void IsoNumericCode_TestStruct_AreEqual()
    {
        var exp = 336;
        var act = TestStruct.IsoNumericCode;
        act.Should().Be(exp);
    }

    #endregion

    #region Methods

    [Test]
    public void IsEmptyOrNotKnown_Empty_IsTrue()
    {
        Country.Empty.IsEmptyOrUnknown().Should().BeTrue();
    }

    [Test]
    public void IsEmptyOrNotKnown_NotKnown_IsTrue()
    {
        Country.Unknown.IsEmptyOrUnknown().Should().BeTrue();
    }

    [Test]
    public void IsEmptyOrNotKnown_TestStruct_IsFalse()
    {
        TestStruct.IsEmptyOrUnknown().Should().BeFalse();
    }

    /// <remarks>
    /// On 1980, Burkina Faso did not yet exist.
    /// </remarks>
    [Test]
    public void GetCurrency_BF1980_Empty()
    {
        var currency = Country.BF.GetCurrency(new Date(1980, 01, 01));
        currency.Should().Be(Currency.Empty);
    }

    [Test]
    public void GetCurrency_NL2001_NLG()
    {
        var act = Country.NL.GetCurrency(new Date(2001, 12, 31));
        var exp = Currency.NLG;

        exp.Should().Be(act);
    }

    [Test]
    public void GetCurrency_NLToday_EUR()
    {
        var act = Country.NL.GetCurrency(Clock.Today());
        var exp = Currency.EUR;

        exp.Should().Be(act);
    }

    #endregion
}

[Serializable]
public class CountrySerializeObject
{
    public int Id { get; set; }
    public Country Obj { get; set; }
    public DateTime Date { get; set; }
}
