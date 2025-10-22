namespace Qowaiv.UnitTests;

/// <summary>Tests the week date SVO.</summary>
public class WeekDateTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly WeekDate TestStruct = new(1997, 14, 6);

    #region week date const tests

    /// <summary>WeekDate.MinValue should be equal to the default of week date.</summary>
    [Test]
    public void MinValue_None_EqualsDefault()
    {
        WeekDate.MinValue.Should().Be(default);
    }

    #endregion

    #region TryParse tests

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_StringValue_IsValid()
    {
        string str = "1234-W50-6";
        WeekDate.TryParse(str, out WeekDate val).Should().BeTrue();
        Should.BeEqual(str, val.ToString(), "Value");
    }

    [Test]
    public void Parse_InvalidInput_ThrowsFormatException()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Assert.Catch<FormatException>
            (() =>
            {
                WeekDate.Parse("InvalidInput");
            },
            "Not a valid week date");
        }
    }

    [Test]
    public void TryParse_TestStructInput_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var exp = TestStruct;
            var act = WeekDate.TryParse(exp.ToString());

            act.Should().Be(exp);
        }
    }

    [Test]
    public void TryParse_Y0000W21D7_DefaultValue()
    {
        WeekDate exp = default;
        WeekDate.TryParse("0000-W21-7", out WeekDate act).Should().BeFalse();
        act.Should().Be(exp);
    }

    [Test]
    public void TryParse_Y2000W53D7_DefaultValue()
    {
        WeekDate exp = default;
        WeekDate.TryParse("2000-W53-7", out WeekDate act).Should().BeFalse();
        act.Should().Be(exp);
    }

    #endregion

    #region (XML) (De)serialization tests

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
        var exp = "1997-W14-6";
        act.Should().Be(exp);
    }

    [Test]
    public void XmlDeserialize_XmlString_AreEqual()
    {
        var act = Deserialize.Xml<WeekDate>("1997-W14-6");
        act.Should().Be(TestStruct);
    }

    [Test]
    public void XmlSerializeDeserialize_WeekDateSerializeObject_AreEqual()
    {
        var input = new WeekDateSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new WeekDateSerializeObject
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
    public void DataContractSerializeDeserialize_WeekDateSerializeObject_AreEqual()
    {
        var input = new WeekDateSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new WeekDateSerializeObject
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

    [Test]
    public void XmlSerializeDeserialize_Empty_AreEqual()
    {
        var input = new WeekDateSerializeObject
        {
            Id = 17,
            Obj = WeekDate.MinValue,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new WeekDateSerializeObject
        {
            Id = 17,
            Obj = WeekDate.MinValue,
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
    public void ToString_CustomFormatter_SupportsCustomFormatting()
    {
        var act = TestStruct.ToString("y#W", FormatProvider.CustomFormatter);
        var exp = "Unit Test Formatter, value: '1997#14', format: 'y#W'";

        act.Should().Be(exp);
    }

    [Test]
    public void ToString_NullFormatProvider_FormattedString()
    {
        using (TestCultures.en_US.Scoped())
        {
            var act = TestStruct.ToString(@"y-\WW-d", null);
            var exp = "1997-W14-6";

            act.Should().Be(exp);
        }
    }

    [Test]
    public void ToString_TestStruct_ComplexPattern()
    {
        var act = TestStruct.ToString(string.Empty);
        var exp = "1997-W14-6";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString_Y1979W3D5FormatWUpper_ComplexPattern()
    {
        var act = new WeekDate(1979, 3, 5).ToString(@"y-\WW-d");
        var exp = "1979-W3-5";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString_Y1979W3D5FormatWLower_ComplexPattern()
    {
        var act = new WeekDate(1979, 3, 5).ToString(@"y-\Ww-d");
        var exp = "1979-W03-5";
        act.Should().Be(exp);
    }

    #endregion

    #region IEquatable tests

    /// <summary>GetHash should not fail for WeekDate.Empty.</summary>
    [Test]
    public void GetHash_Empty_Hash()
    {
        WeekDate.MinValue.GetHashCode().Should().Be(0);
    }

    /// <summary>GetHash should not fail for the test struct.</summary>
    [Test]
    public void GetHash_TestStruct_Hash()
    {
        TestStruct.GetHashCode().Should().Be(2027589483);
    }

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        var l = WeekDate.Parse("1997-14-6", CultureInfo.InvariantCulture);
        var r = WeekDate.Parse("1997-W14-6", CultureInfo.InvariantCulture);

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
        TestStruct.Equals(WeekDate.MinValue).Should().BeFalse();
    }

    [Test]
    public void Equals_EmptyTestStruct_IsFalse()
    {
        WeekDate.MinValue.Equals(TestStruct).Should().BeFalse();
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

    /// <summary>Orders a list of week dates ascending.</summary>
    [Test]
    public void OrderBy_WeekDate_AreEqual()
    {
        var item0 = WeekDate.Parse("2000-W01-3");
        var item1 = WeekDate.Parse("2000-W11-2");
        var item2 = WeekDate.Parse("2000-W21-1");
        var item3 = WeekDate.Parse("2000-W31-7");

        var inp = new List<WeekDate> { WeekDate.MinValue, item3, item2, item0, item1, WeekDate.MinValue };
        var exp = new List<WeekDate> { WeekDate.MinValue, WeekDate.MinValue, item0, item1, item2, item3 };
        var act = inp.OrderBy(item => item).ToList();

        act.Should().BeEquivalentTo(exp);
    }

    /// <summary>Orders a list of week dates descending.</summary>
    [Test]
    public void OrderByDescending_WeekDate_AreEqual()
    {
        var item0 = WeekDate.Parse("2000-W01-3");
        var item1 = WeekDate.Parse("2000-W11-2");
        var item2 = WeekDate.Parse("2000-W21-1");
        var item3 = WeekDate.Parse("2000-W31-7");

        var inp = new List<WeekDate> { WeekDate.MinValue, item3, item2, item0, item1, WeekDate.MinValue };
        var exp = new List<WeekDate> { item3, item2, item1, item0, WeekDate.MinValue, WeekDate.MinValue };
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
        WeekDate l = new(1980, 17, 5);
        WeekDate r = new(1980, 19, 5);

        (l < r).Should().BeTrue();
    }
    [Test]
    public void GreaterThan_21LT19_IsTrue()
    {
        WeekDate l = new(1980, 21, 5);
        WeekDate r = new(1980, 19, 5);

        (l > r).Should().BeTrue();
    }

    [Test]
    public void LessThanOrEqual_17LT19_IsTrue()
    {
        WeekDate l = new(1980, 17, 5);
        WeekDate r = new(1980, 19, 5);

        (l <= r).Should().BeTrue();
    }
    [Test]
    public void GreaterThanOrEqual_21LT19_IsTrue()
    {
        WeekDate l = new(1980, 21, 5);
        WeekDate r = new(1980, 19, 5);

        (l >= r).Should().BeTrue();
    }

    [Test]
    public void LessThanOrEqual_17LT17_IsTrue()
    {
        WeekDate l = new(1980, 17, 5);
        WeekDate r = new(1980, 17, 5);

        (l <= r).Should().BeTrue();
    }
    [Test]
    public void GreaterThanOrEqual_21LT21_IsTrue()
    {
        WeekDate l = new(1980, 21, 5);
        WeekDate r = new(1980, 21, 5);

        (l >= r).Should().BeTrue();
    }
    #endregion

    #region Casting tests

    [Test]
    public void Explicit_Int32ToWeekDate_AreEqual()
    {
        var exp = TestStruct;
        var act = (WeekDate)new DateTime(1997, 04, 05, 00, 00, 000, DateTimeKind.Local);

        act.Should().Be(exp);
    }
    [Test]
    public void Explicit_WeekDateToInt32_AreEqual()
    {
        DateTime exp = new(1997, 04, 05, 00, 00, 000, DateTimeKind.Local);
        DateTime act = TestStruct;

        act.Should().Be(exp);
    }
    #endregion

    #region Properties

    [Test]
    public void Date_TestStruct_AreEqual()
    {
        var exp = new Date(1997, 04, 05);
        var act = TestStruct.Date;

        act.Should().Be(exp);
    }

    [Test]
    public void Year_MinValue_AreEqual()
    {
        var exp = WeekDate.MinValue.Year;
        var act = 1;

        act.Should().Be(exp);
    }

    [Test]
    public void Year_MaxValue_AreEqual()
    {
        var exp = WeekDate.MaxValue.Year;
        var act = 9999;

        act.Should().Be(exp);
    }

    [Test]
    public void Year_Y2010W52D7_AreEqual()
    {
        var date = new WeekDate(2010, 52, 7);
        var exp = 2010;
        var act = date.Year;

        act.Should().Be(exp);
    }

    [Test]
    public void Year_Y2020W01D1_AreEqual()
    {
        var date = new WeekDate(2020, 01, 1);
        var exp = 2020;
        var act = date.Year;

        act.Should().Be(exp);
    }




    [Test]
    public void Day_TestStruct_AreEqual()
    {
        var exp = 6;
        var act = TestStruct.Day;

        act.Should().Be(exp);
    }

    [Test]
    public void Day_Sunday_AreEqual()
    {
        var date = new WeekDate(1990, 40, 7);
        var exp = 7;
        var act = date.Day;

        act.Should().Be(exp);
    }

    [Test]
    public void DayOfYear_TestStruct_AreEqual()
    {
        var exp = 96;
        var act = TestStruct.DayOfYear;

        act.Should().Be(exp);
    }



    #endregion
}

[Serializable]
public class WeekDateSerializeObject
{
    public int Id { get; set; }
    public WeekDate Obj { get; set; }
    public DateTime Date { get; set; }
}
