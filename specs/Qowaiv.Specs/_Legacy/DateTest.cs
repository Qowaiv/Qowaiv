namespace Qowaiv.UnitTests;

/// <summary>Tests the Date SVO.</summary>
[TestFixture]
public class DateTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly Date TestStruct = new(1970, 02, 14);

    #region Date const tests

    /// <summary>Date.Empty should be equal to the default of Date.</summary>
    [Test]
    public void MinValue_None_EqualsDefault()
    {
        Date.MinValue.Should().Be(default);
    }

    #endregion

    #region Constructor Tests

    [Test]
    public void Ctor_621393984000000017_AreEqual()
    {
        var act = new Date(621393984000000017L);
        var exp = TestStruct;

        act.Should().Be(exp);
    }

    #endregion

    #region TryParse tests

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_StringValue_IsValid()
    {
        string str = "1983-05-02";

        Date.TryParse(str, out Date val).Should().BeTrue();
        Should.BeEqual(new Date(1983, 05, 02), val, "Value");
    }

    /// <summary>TryParse with specified string value should be invalid.</summary>
    [Test]
    public void TryParse_NotADate_IsNotValid()
    {
        using (new CultureInfoScope(TestCultures.nl_NL))
        {
            string str = "not a date";

            Date.TryParse(str, out Date val).Should().BeFalse();
            Should.BeEqual(Date.MinValue, val, "Value");
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
                Date.Parse("InvalidInput");
            },
            "Not a valid date");
        }
    }

    [Test]
    public void TryParse_TestStructInput_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var exp = TestStruct;
            var act = Date.TryParse(exp.ToString());

            act.Should().Be(exp);
        }
    }

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => Date.TryParse("invalid input").Should().BeNull();

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
        var exp = "1970-02-14";
        act.Should().Be(exp);
    }

    [Test]
    public void XmlDeserialize_XmlString_AreEqual()
    {
        var act = Deserialize.Xml<Date>("1970-02-14");
        act.Should().Be(TestStruct);
    }

    [Test]
    public void XmlSerializeDeserialize_DateSerializeObject_AreEqual()
    {
        var input = new DateSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new DateSerializeObject
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
    public void DataContractSerializeDeserialize_DateSerializeObject_AreEqual()
    {
        var input = new DateSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new DateSerializeObject
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
        var input = new DateSerializeObject
        {
            Id = 17,
            Obj = Date.MinValue,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new DateSerializeObject
        {
            Id = 17,
            Obj = Date.MinValue,
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
    public void ToString_CustomFormatter_SupportsCustomFormatting()
    {
        var act = TestStruct.ToString("d_M_yy", FormatProvider.CustomFormatter);
        var exp = "Unit Test Formatter, value: '14_2_70', format: 'd_M_yy'";

        act.Should().Be(exp);
    }
    [Test]
    public void ToString_TestStruct_ComplexPattern()
    {
        using (TestCultures.nl_BE.Scoped())
        {
            var act = TestStruct.ToString(string.Empty);
            var exp = "14/02/1970";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void ToString_FormatValueEnglishGreatBritain_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var act = new Date(1988, 08, 08).ToString("yy-M-d");
            var exp = "88-8-8";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void ToString_FormatValueSpanishSpain_AreEqual()
    {
        var act = new Date(1988, 08, 08).ToString("d", new CultureInfo("es-EC"));
        var exp = "8/8/1988";
        act.Should().Be(exp);
    }

    #endregion

    #region IEquatable tests

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        var l = Date.Parse("1970-02-14", CultureInfo.InvariantCulture);
        var r = Date.Parse("14 february 1970", CultureInfo.InvariantCulture);

        l.Equals(r).Should().BeTrue();
    }

    #endregion

    #region IComparable tests

    /// <summary>Orders a list of Dates ascending.</summary>
    [Test]
    public void OrderBy_Date_AreEqual()
    {
        var item0 = Date.Parse("1970-01-03");
        var item1 = Date.Parse("1970-02-01");
        var item2 = Date.Parse("1970-03-28");
        var item3 = Date.Parse("1970-04-12");

        var inp = new List<Date> { Date.MinValue, item3, item2, item0, item1, Date.MinValue };
        var exp = new List<Date> { Date.MinValue, Date.MinValue, item0, item1, item2, item3 };
        var act = inp.OrderBy(item => item).ToList();

        act.Should().BeEquivalentTo(exp);
    }

    /// <summary>Orders a list of Dates descending.</summary>
    [Test]
    public void OrderByDescending_Date_AreEqual()
    {
        var item0 = Date.Parse("1970-01-03");
        var item1 = Date.Parse("1970-02-01");
        var item2 = Date.Parse("1970-03-28");
        var item3 = Date.Parse("1970-04-12");

        var inp = new List<Date> { Date.MinValue, item3, item2, item0, item1, Date.MinValue };
        var exp = new List<Date> { item3, item2, item1, item0, Date.MinValue, Date.MinValue };
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
        Date l = new(1990, 10, 17);
        Date r = new(1990, 10, 19);

        (l < r).Should().BeTrue();
    }
    [Test]
    public void GreaterThan_21LT19_IsTrue()
    {
        Date l = new(1990, 10, 21);
        Date r = new(1990, 10, 19);

        (l > r).Should().BeTrue();
    }

    [Test]
    public void LessThanOrEqual_17LT19_IsTrue()
    {
        Date l = new(1990, 10, 17);
        Date r = new(1990, 10, 19);

        (l <= r).Should().BeTrue();
    }
    [Test]
    public void GreaterThanOrEqual_21LT19_IsTrue()
    {
        Date l = new(1990, 10, 21);
        Date r = new(1990, 10, 19);

        (l >= r).Should().BeTrue();
    }

    [Test]
    public void LessThanOrEqual_17LT17_IsTrue()
    {
        Date l = new(1990, 10, 17);
        Date r = new(1990, 10, 17);

        (l <= r).Should().BeTrue();
    }
    [Test]
    public void GreaterThanOrEqual_21LT21_IsTrue()
    {
        Date l = new(1990, 10, 21);
        Date r = new(1990, 10, 21);

        (l >= r).Should().BeTrue();
    }
    #endregion

    #region Casting tests

    [Test]
    public void Implicit_WeekDateToDate_AreEqual()
    {
        Date exp = new WeekDate(1970, 07, 6);
        Date act = TestStruct;

        act.Should().Be(exp);
    }
    [Test]
    public void Implicit_DateToWeekDate_AreEqual()
    {
        WeekDate exp = TestStruct;
        WeekDate act = new(1970, 07, 6);

        act.Should().Be(exp);
    }

    [Test]
    public void Explicit_DateTimeToDate_AreEqual()
    {
        Date exp = (Date)new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local);
        Date act = TestStruct;

        act.Should().Be(exp);
    }
    [Test]
    public void Implicit_DateToDateTime_AreEqual()
    {
        DateTime exp = TestStruct;
        DateTime act = new(1970, 02, 14, 00, 00, 000, DateTimeKind.Local);

        act.Should().Be(exp);
    }

    #endregion

    #region Properties

    [Test]
    public void Year_TestStruct_AreEqual()
    {
        var act = TestStruct.Year;
        var exp = 1970;
        act.Should().Be(exp);
    }
    [Test]
    public void Month_TestStruct_AreEqual()
    {
        var act = TestStruct.Month;
        var exp = 2;
        act.Should().Be(exp);
    }
    [Test]
    public void Day_TestStruct_AreEqual()
    {
        var act = TestStruct.Day;
        var exp = 14;
        act.Should().Be(exp);
    }
    [Test]
    public void DayOfWeek_TestStruct_AreEqual()
    {
        var act = TestStruct.DayOfWeek;
        var exp = DayOfWeek.Saturday;
        act.Should().Be(exp);
    }
    [Test]
    public void DayOfYear_TestStruct_AreEqual()
    {
        var act = TestStruct.DayOfYear;
        var exp = 45;
        act.Should().Be(exp);
    }

    #endregion

    #region Methods

    [Test]
    public void Increment_None_AreEqual()
    {
        var act = TestStruct;
        act++;
        var exp = new Date(1970, 02, 15);
        act.Should().Be(exp);
    }
    [Test]
    public void Decrement_None_AreEqual()
    {
        var act = TestStruct;
        act--;
        var exp = new Date(1970, 02, 13);
        act.Should().Be(exp);
    }

    [Test]
    public void Plus_TimeSpan_AreEqual()
    {
        var act = TestStruct + new TimeSpan(25, 30, 15);
        var exp = new Date(1970, 02, 15);
        act.Should().Be(exp);
    }
    [Test]
    public void Min_TimeSpan_AreEqual()
    {
        var act = TestStruct - new TimeSpan(25, 30, 15);
        var exp = new Date(1970, 02, 12);
        act.Should().Be(exp);
    }

    [Test]
    public void Min_DateTime_AreEqual()
    {
        var act = TestStruct - new Date(1970, 02, 12);
        var exp = TimeSpan.FromDays(2);
        act.Should().Be(exp);
    }

    [Test]
    public void AddTicks_4000000000017_AreEqual()
    {
        var act = TestStruct.AddTicks(4000000000017L);
        var exp = new Date(1970, 02, 18);

        act.Should().Be(exp);
    }

    [Test]
    public void AddMilliseconds_around3Days_AreEqual()
    {
        var act = TestStruct.AddMilliseconds(3 * 24 * 60 * 60 * 1003);
        var exp = new Date(1970, 02, 17);

        act.Should().Be(exp);
    }
    [Test]
    public void AddSeconds_around3Days_AreEqual()
    {
        var act = TestStruct.AddSeconds(3 * 24 * 60 * 64);
        var exp = new Date(1970, 02, 17);

        act.Should().Be(exp);
    }
    [Test]
    public void AddMinutes_2280_AreEqual()
    {
        var act = TestStruct.AddMinutes(2 * 24 * 60);
        var exp = new Date(1970, 02, 16);

        act.Should().Be(exp);
    }

    [Test]
    public void AddHours_41_AreEqual()
    {
        var act = TestStruct.AddHours(41);
        var exp = new Date(1970, 02, 15);

        act.Should().Be(exp);
    }

    [Test]
    public void AddMonths_12_AreEqual()
    {
        var act = TestStruct.AddMonths(12);
        var exp = new Date(1971, 02, 14);

        act.Should().Be(exp);
    }

    [Test]
    public void Add_12Months_AreEqual()
    {
        var added = new Date(1970, 02, 14) + MonthSpan.FromMonths(12);
        Should.BeEqual(new Date(1971, 02, 14), added);
    }

    [Test]
    public void Subtract_3Months_AreEqual()
    {
        var subtracted = new Date(1971, 02, 14) - MonthSpan.FromMonths(3);
        Should.BeEqual(new Date(1970, 11, 14), subtracted);
    }

    [Test]
    public void AddYears_Min12_AreEqual()
    {
        var act = TestStruct.AddYears(-12);
        var exp = new Date(1958, 02, 14);

        act.Should().Be(exp);
    }

    #endregion
}

[Serializable]
public class DateSerializeObject
{
    public int Id { get; set; }
    public Date Obj { get; set; }
    public DateTime Date { get; set; }
}
