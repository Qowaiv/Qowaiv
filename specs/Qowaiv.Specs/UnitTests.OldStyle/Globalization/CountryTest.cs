namespace Qowaiv.UnitTests.Globalization;

public class CountryTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly Country TestStruct = Country.VA;

    /// <summary>Country.Empty should be equal to the default of Country.</summary>
    [Test]
    public void Empty_None_EqualsDefault()
    {
        Assert.AreEqual(default(Country), Country.Empty);
    }

    #region Current

    [Test]
    public void Current_CurrentCultureDeDE_Germany()
    {
        using (TestCultures.De_DE.Scoped())
        {
            var act = Country.Current;
            var exp = Country.DE;

            Assert.AreEqual(exp, act);
        }
    }

    [Test]
    public void Current_CurrentCultureEsEC_Ecuador()
    {
        using (TestCultures.Es_EC.Scoped())
        {
            var act = Country.Current;
            var exp = Country.EC;

            Assert.AreEqual(exp, act);
        }
    }

    [Test]
    public void Current_CurrentCultureEn_Empty()
    {
        using (TestCultures.En.Scoped())
        {
            var act = Country.Current;
            var exp = Country.Empty;

            Assert.AreEqual(exp, act);
        }
    }

    #endregion

    #region Country IsEmpty tests

    /// <summary>Country.IsEmpty() should true for the default of Country.</summary>
    [Test]
    public void IsEmpty_Default_IsTrue()
    {
        Assert.IsTrue(default(Country).IsEmpty());
    }

    /// <summary>Country.IsEmpty() should false for the TestStruct.</summary>
    [Test]
    public void IsEmpty_Default_IsFalse()
    {
        Assert.IsFalse(TestStruct.IsEmpty());
    }

    #endregion

    #region TryParse tests

    /// <summary>TryParse null should be valid.</summary>
    [Test]
    public void TryParse_Null_IsValid()
    {
        string str = null;
        Assert.IsTrue(Country.TryParse(str, out Country val), "Valid");
        Assert.AreEqual(string.Empty, val.ToString(), "Value");
    }

    /// <summary>TryParse string.Empty should be valid.</summary>
    [Test]
    public void TryParse_StringEmpty_IsValid()
    {
        string str = string.Empty;
        Assert.IsTrue(Country.TryParse(str, out Country val), "Valid");
        Assert.AreEqual(string.Empty, val.ToString(), "Value");
    }

    /// <summary>TryParse "?" should be valid and the result should be Country.Unknown.</summary>
    [Test]
    public void TryParse_Questionmark_IsValid()
    {
        string str = "?";
        Assert.IsTrue(Country.TryParse(str, out Country val), "Valid");
        Assert.IsTrue(val.IsUnknown(), "Value");
    }

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_NullCultureStringValue_IsValid()
    {
        string str = "VA";
        Assert.IsTrue(Country.TryParse(str, null, out Country val), "Valid");
        Assert.AreEqual(str, val.ToString(), "Value");
    }

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_StringValue_IsValid()
    {
        string str = "VA";
        Assert.IsTrue(Country.TryParse(str, out Country val), "Valid");
        Assert.AreEqual(str, val.ToString(), "Value");
    }

    /// <summary>TryParse with specified string value should be invalid.</summary>
    [Test]
    public void TryParse_StringValue_IsNotValid()
    {
        string str = "string";
        Assert.IsFalse(Country.TryParse(str, out Country val), "Valid");
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
                Country.Parse("InvalidInput");
            },
            "Not a valid country");
        }
    }

    [Test]
    public void TryParse_TestStructInput_AreEqual()
    {
        using (TestCultures.En_GB.Scoped())
        {
            var exp = TestStruct;
            var act = Country.TryParse(exp.ToString());

            Assert.AreEqual(exp, act);
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
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void Create_CultureInfoNull_Empty()
    {
        var exp = Country.Empty;
        var act = Country.Create((CultureInfo?)null);
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void Create_NL_NL()
    {
        var exp = Country.NL;
        var act = Country.Create(new RegionInfo("NL"));
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void Create_CultureInfoInvariant_Empty()
    {
        var exp = Country.Empty;
        var act = Country.Create(CultureInfo.InvariantCulture);
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void Create_CultureInfoEs_Empty()
    {
        var exp = Country.Empty;
        var act = Country.Create(new CultureInfo("es"));
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void Create_CultureInfoEsEC_Empty()
    {
        var exp = Country.EC;
        var act = Country.Create(new CultureInfo("es-EC"));
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void Create_CS_CSXX()
    {
        var cs = new RegionInfo("CS");
        var country = Country.Create(cs);
        Assert.AreEqual(Country.CSXX, country);
    }

    [TestCaseSource(typeof(Country), nameof(Country.All))]
    public void RegionInfoExists(Country country)
    {
        // As the regions available depend on the environment running, we can't
        // predict the outcome.
        Assert.IsTrue(new[] { true, false }.Contains(country.RegionInfoExists));
    }

    #endregion

    #region (XML) (De)serialization tests

    [Test]
    public void DataContractSerializeDeserialize_TestStruct_AreEqual()
    {
        var input = CountryTest.TestStruct;
        var exp = CountryTest.TestStruct;
        var act = SerializeDeserialize.DataContract(input);
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void XmlSerialize_TestStruct_AreEqual()
    {
        var act = Serialize.Xml(TestStruct);
        var exp = "VA";
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void XmlDeserialize_XmlString_AreEqual()
    {
        var act =Deserialize.Xml<Country>("VA");
        Assert.AreEqual(TestStruct, act);
    }


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
        Assert.IsNull(obj.GetSchema());
    }

    #endregion

    #region IFormattable / ToString tests

    [Test]
    public void ToString_CustomFormatter_SupportsCustomFormatting()
    {
        var act = TestStruct.ToString("e (3)", FormatProvider.CustomFormatter);
        var exp = "Unit Test Formatter, value: 'Holy See (VAT)', format: 'e (3)'";

        Assert.AreEqual(exp, act);
    }

    [Test]
    public void ToString_Empty_IsStringEmpty()
    {
        var act = Country.Empty.ToString();
        var exp = "";
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void ToString_Unknown_Questionmark()
    {
        var act = Country.Unknown.ToString();
        var exp = "?";
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void ToString2_NZ_AreEqual()
    {
        var exp = "MZ";
        var act = Country.MZ.ToString("2");
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void ToString3_MZ_AreEqual()
    {
        var exp = "MOZ";
        var act = Country.MZ.ToString("3", new CultureInfo("ja-JP"));
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void ToString0_MZ_AreEqual()
    {
        var exp = "508";
        var act = Country.MZ.ToString("0", new CultureInfo("ja-JP"));
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void ToStringN_CSHH_AreEqual()
    {
        var exp = "CSHH";
        var act = Country.CSHH.ToString("n", new CultureInfo("ja-JP"));
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void ToStringE_MZ_AreEqual()
    {
        var exp = "Mozambique";
        var act = Country.MZ.ToString("e", new CultureInfo("ja-JP"));
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void ToStringF_MZ_AreEqual()
    {
        var exp = "モザンビーク";
        var act = Country.MZ.ToString("f", new CultureInfo("ja-JP"));
        Assert.AreEqual(exp, act);
    }

    #endregion

    #region IEquatable tests

    /// <summary>GetHash should not fail for Country.Empty.</summary>
    [Test]
    public void GetHash_Empty_Hash()
    {
        Assert.AreEqual(0, Country.Empty.GetHashCode());
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
        Assert.IsTrue(Country.Empty.Equals(Country.Empty));
    }

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        using (TestCultures.Nl_NL.Scoped())
        {
            var l = Country.Parse("België");
            var r = Country.Parse("belgie");

            Assert.IsTrue(l.Equals(r));
        }
    }

    [Test]
    public void Equals_TestStructTestStruct_IsTrue()
    {
        Assert.IsTrue(CountryTest.TestStruct.Equals(CountryTest.TestStruct));
    }

    [Test]
    public void Equals_TestStructEmpty_IsFalse()
    {
        Assert.IsFalse(CountryTest.TestStruct.Equals(Country.Empty));
    }

    [Test]
    public void Equals_EmptyTestStruct_IsFalse()
    {
        Assert.IsFalse(Country.Empty.Equals(CountryTest.TestStruct));
    }

    [Test]
    public void Equals_TestStructObjectTestStruct_IsTrue()
    {
        Assert.IsTrue(CountryTest.TestStruct.Equals((object)CountryTest.TestStruct));
    }

    [Test]
    public void Equals_TestStructNull_IsFalse()
    {
        Assert.IsFalse(CountryTest.TestStruct.Equals(Nil.Object));
    }

    [Test]
    public void Equals_TestStructObject_IsFalse()
    {
        Assert.IsFalse(CountryTest.TestStruct.Equals(new object()));
    }

    [Test]
    public void OperatorIs_TestStructTestStruct_IsTrue()
    {
        var l = CountryTest.TestStruct;
        var r = CountryTest.TestStruct;
        Assert.IsTrue(l == r);
    }

    [Test]
    public void OperatorIsNot_TestStructTestStruct_IsFalse()
    {
        var l = CountryTest.TestStruct;
        var r = CountryTest.TestStruct;
        Assert.IsFalse(l != r);
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

        CollectionAssert.AreEqual(exp, act);
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

    /// <summary>Compare with null should return 1.</summary>
    [Test]
    public void CompareTo_null_1()
    {
        object @null = null;
        Assert.AreEqual(1, TestStruct.CompareTo(@null));
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

        Assert.AreEqual(exp, act);
    }
    [Test]
    public void Explicit_CountryToRegionInfo_AreEqual()
    {
        var exp = new RegionInfo("NL");
        var act = (RegionInfo)Country.NL;

        Assert.AreEqual(exp, act);
    }

    #endregion

    #region Properties

    [Test]
    public void CallingCode_Empty_AreEqual()
    {
        var exp = "";
        var act = Country.Empty.CallingCode;
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void CallingCode_Unknown_AreEqual()
    {
        var exp = "";
        var act = Country.Unknown.CallingCode;
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void CallingCode_TestStruct_AreEqual()
    {
        var exp = "+379";
        var act = TestStruct.CallingCode;
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void Name_Empty_AreEqual()
    {
        using (TestCultures.En_GB.Scoped())
        {
            var exp = "";
            var act = Country.Empty.Name;
            Assert.AreEqual(exp, act);
        }
    }
    [Test]
    public void Name_Unknown_AreEqual()
    {
        using (TestCultures.En_GB.Scoped())
        {
            var exp = "?";
            var act = Country.Unknown.Name;
            Assert.AreEqual(exp, act);
        }
    }
    [Test]
    public void Name_TestStruct_AreEqual()
    {
        using (TestCultures.En_GB.Scoped())
        {
            var exp = "VA";
            var act = TestStruct.Name;
            Assert.AreEqual(exp, act);
        }
    }

    [Test]
    public void IsoNumericCode_Empty_AreEqual()
    {
        var exp = 0;
        var act = Country.Empty.IsoNumericCode;
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void IsoNumericCode_Unknown_AreEqual()
    {
        var exp = 999;
        var act = Country.Unknown.IsoNumericCode;
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void IsoNumericCode_TestStruct_AreEqual()
    {
        var exp = 336;
        var act = TestStruct.IsoNumericCode;
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void StartDate_Empty_AreEqual()
    {
        var exp = Date.MinValue;
        var act = Country.Empty.StartDate;
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void StartDate_Unknown_AreEqual()
    {
        var exp = Date.MinValue;
        var act = Country.Unknown.StartDate;
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void StartDate_TestStruct_AreEqual()
    {
        var exp = new Date(1974, 01, 01);
        var act = TestStruct.StartDate;
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void StartDate_CZ_AreEqual()
    {
        var exp = new Date(1993, 01, 01);
        var act = Country.CZ.StartDate;
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void EndDate_Empty_AreEqual()
    {
        Country.Empty.EndDate.Should().BeNull();
    }

    [Test]
    public void EndDate_Unknown_AreEqual()
    {
        Country.Unknown.EndDate.Should().BeNull();
    }

    [Test]
    public void EndDate_TestStruct_AreEqual()
    {
        TestStruct.EndDate.Should().BeNull();
    }

    [Test]
    public void EndDate_CZ_AreEqual()
    {
        DateTime? exp = null;
        var act = Country.CZ.EndDate;
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void EndDate_CSHH_AreEqual()
    {
        var exp = new Date(1992, 12, 31);
        var act = Country.CSHH.EndDate;
        Assert.AreEqual(exp, act);
    }

    #endregion

    #region Methods

    [Test]
    public void IsEmptyOrNotKnown_Empty_IsTrue()
    {
        Assert.IsTrue(Country.Empty.IsEmptyOrUnknown());
    }

    [Test]
    public void IsEmptyOrNotKnown_NotKnown_IsTrue()
    {
        Assert.IsTrue(Country.Unknown.IsEmptyOrUnknown());
    }

    [Test]
    public void IsEmptyOrNotKnown_TestStruct_IsFalse()
    {
        Assert.IsFalse(TestStruct.IsEmptyOrUnknown());
    }

    [Test]
    public void ExistsOnDate_SerbiaAndMontenegro1992_IsFalse()
    {
        Assert.IsFalse(Country.CSXX.ExistsOnDate(new Date(1992, 12, 31)));
    }
    [Test]
    public void ExistsOnDate_SerbiaAndMontenegro1993_IsTrue()
    {
        Assert.IsTrue(Country.CSXX.ExistsOnDate(new Date(1993, 01, 01)));
    }
    [Test]
    public void ExistsOnDate_SerbiaAndMontenegro2012_IsFalse()
    {
        Assert.IsFalse(Country.CSXX.ExistsOnDate(new Date(2012, 01, 01)));
    }

    /// <remarks>
    /// On 1980, Burkina Faso did not yet exist.
    /// </remarks>
    [Test]
    public void GetCurrency_BF1980_Empty()
    {
        var currency = Country.BF.GetCurrency(new Date(1980, 01, 01));
        Assert.AreEqual(Currency.Empty, currency);
    }

    [Test]
    public void GetCurrency_NL2001_NLG()
    {
        var act = Country.NL.GetCurrency(new Date(2001, 12, 31));
        var exp = Currency.NLG;

        Assert.AreEqual(act, exp);
    }

    [Test]
    public void GetCurrency_NLToday_EUR()
    {
        var act = Country.NL.GetCurrency(Clock.Today());
        var exp = Currency.EUR;

        Assert.AreEqual(act, exp);
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
