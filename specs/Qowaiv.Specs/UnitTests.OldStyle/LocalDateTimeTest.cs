namespace Qowaiv.UnitTests;

/// <summary>Tests the local date time SVO.</summary>
public class LocalDateTimeTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly LocalDateTime TestStruct = new(1988, 06, 13, 22, 10, 05, 001);

    /// <summary>The test instance for most tests.</summary>
    public static readonly LocalDateTime TestStructNoMilliseconds = new(2001, 07, 30, 21, 55, 08);

    #region local date time const tests

    /// <summary>LocalDateTime.MinValue should be equal to the default of local date time.</summary>
    [Test]
    public void MinValue_None_EqualsDefault()
    {
        Assert.AreEqual(default(LocalDateTime), LocalDateTime.MinValue);
    }

    #endregion

    #region TryParse tests

    /// <summary>TryParse null should be valid.</summary>
    [Test]
    public void TryParse_Null_IsNotValid()
    {
        string str = null;
        Assert.IsFalse(LocalDateTime.TryParse(str, out _), "Valid");
    }

    /// <summary>TryParse string.MinValue should be valid.</summary>
    [Test]
    public void TryParse_StringMinValue_IsNotValid()
    {
        string str = string.Empty;
        Assert.IsFalse(LocalDateTime.TryParse(str, out _), "Valid");
    }

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_StringValue_IsValid()
    {
        using (new CultureInfoScope(TestCultures.Nl_NL))
        {
            string str = "26-4-2015 17:07:13";
            Assert.IsTrue(LocalDateTime.TryParse(str, out LocalDateTime val), "Valid");
            Assert.AreEqual(new LocalDateTime(2015, 04, 26, 17, 07, 13, 000), val, "Value");
        }
    }

    /// <summary>TryParse with specified string value should be invalid.</summary>
    [Test]
    public void TryParse_StringValue_IsNotValid()
    {
        string str = "invalid format";
        Assert.IsFalse(LocalDateTime.TryParse(str, out _), "Valid");
    }

    [Test]
    public void Parse_InvalidInput_ThrowsFormatException()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Assert.Catch<FormatException>
            (() =>
            {
                LocalDateTime.Parse("InvalidInput");
            },
            "Not a valid date");
        }
    }

    [Test]
    public void TryParse_TestStructInput_AreEqual()
    {
        using (TestCultures.En_GB.Scoped())
        {
            var exp = TestStructNoMilliseconds;
            var act = LocalDateTime.TryParse(exp.ToString());

            Assert.AreEqual(exp, act);
        }
    }

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => LocalDateTime.TryParse("invalid input").Should().BeNull();

    #endregion

    #region (XML) (De)serialization tests

    [Test]
    public void GetObjectData_SerializationInfo_AreEqual()
    {
        ISerializable obj = TestStruct;
        var info = new SerializationInfo(typeof(LocalDateTime), new System.Runtime.Serialization.FormatterConverter());
        obj.GetObjectData(info, default);

        info.GetDateTime("Value").Should().Be(new DateTime(1988, 06, 13, 22, 10, 05, 001, DateTimeKind.Local));
    }

    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_TestStruct_AreEqual()
    {
        var input = LocalDateTimeTest.TestStructNoMilliseconds;
        var exp = LocalDateTimeTest.TestStructNoMilliseconds;
        var act = SerializeDeserialize.Binary(input);
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void DataContractSerializeDeserialize_TestStruct_AreEqual()
    {
        var input = LocalDateTimeTest.TestStructNoMilliseconds;
        var exp = LocalDateTimeTest.TestStructNoMilliseconds;
        var act = SerializeDeserialize.DataContract(input);
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void XmlSerialize_TestStruct_AreEqual()
    {
        var act = Serialize.Xml(TestStruct);
        var exp = "1988-06-13 22:10:05.001";
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void XmlDeserialize_XmlString_AreEqual()
    {
        var act =Deserialize.Xml<LocalDateTime>("1988-06-13 22:10:05.001");
        Assert.AreEqual(TestStruct, act);
    }

    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_LocalDateTimeSerializeObject_AreEqual()
    {
        var input = new LocalDateTimeSerializeObject
        {
            Id = 17,
            Obj = LocalDateTimeTest.TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new LocalDateTimeSerializeObject
        {
            Id = 17,
            Obj = LocalDateTimeTest.TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var act = SerializeDeserialize.Binary(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }
    [Test]
    public void XmlSerializeDeserialize_LocalDateTimeSerializeObject_AreEqual()
    {
        var input = new LocalDateTimeSerializeObject
        {
            Id = 17,
            Obj = LocalDateTimeTest.TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new LocalDateTimeSerializeObject
        {
            Id = 17,
            Obj = LocalDateTimeTest.TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var act = SerializeDeserialize.Xml(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }
    [Test]
    public void DataContractSerializeDeserialize_LocalDateTimeSerializeObject_AreEqual()
    {
        var input = new LocalDateTimeSerializeObject
        {
            Id = 17,
            Obj = LocalDateTimeTest.TestStructNoMilliseconds,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new LocalDateTimeSerializeObject
        {
            Id = 17,
            Obj = LocalDateTimeTest.TestStructNoMilliseconds,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var act = SerializeDeserialize.DataContract(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }

    [Test]
    public void XmlSerializeDeserialize_MinValue_AreEqual()
    {
        var input = new LocalDateTimeSerializeObject
        {
            Id = 17,
            Obj = LocalDateTime.MinValue,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new LocalDateTimeSerializeObject
        {
            Id = 17,
            Obj = LocalDateTime.MinValue,
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
        var act = TestStruct.ToString("M:d & h:m", FormatProvider.CustomFormatter);
        var exp = "Unit Test Formatter, value: '6:13 & 10:10', format: 'M:d & h:m'";

        Assert.AreEqual(exp, act);
    }

    [Test]
    public void ToString_TestStruct_ComplexPattern()
    {
        var act = TestStruct.ToString(@"yyyy-MM-dd\THH:mm:ss.FFFFFFF");
        var exp = "1988-06-13T22:10:05.001";
        Assert.AreEqual(exp, act);
    }

    #endregion

    #region IEquatable tests

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        using (TestCultures.En_GB.Scoped())
        {
            var l = LocalDateTime.Parse("14 february 2010", CultureInfo.InvariantCulture);
            var r = LocalDateTime.Parse("2010-02-14", CultureInfo.InvariantCulture);

            Assert.IsTrue(l.Equals(r));
        }
    }

    #endregion

    #region IComparable tests

    /// <summary>Orders a list of local date times ascending.</summary>
    [Test]
    public void OrderBy_LocalDateTime_AreEqual()
    {
        var item0 = new LocalDateTime(1900, 10, 01, 22, 10, 16);
        var item1 = new LocalDateTime(1963, 08, 23, 23, 59, 15);
        var item2 = new LocalDateTime(1999, 12, 05, 04, 13, 14);
        var item3 = new LocalDateTime(2010, 07, 13, 00, 44, 13);

        var inp = new List<LocalDateTime> { LocalDateTime.MinValue, item3, item2, item0, item1, LocalDateTime.MinValue };
        var exp = new List<LocalDateTime> { LocalDateTime.MinValue, LocalDateTime.MinValue, item0, item1, item2, item3 };
        var act = inp.OrderBy(item => item).ToList();

        CollectionAssert.AreEqual(exp, act);
    }

    /// <summary>Orders a list of local date times descending.</summary>
    [Test]
    public void OrderByDescending_LocalDateTime_AreEqual()
    {
        var item0 = new LocalDateTime(1900, 10, 01, 22, 10, 16);
        var item1 = new LocalDateTime(1963, 08, 23, 23, 59, 15);
        var item2 = new LocalDateTime(1999, 12, 05, 04, 13, 14);
        var item3 = new LocalDateTime(2010, 07, 13, 00, 44, 13);

        var inp = new List<LocalDateTime> { LocalDateTime.MinValue, item3, item2, item0, item1, LocalDateTime.MinValue };
        var exp = new List<LocalDateTime> { item3, item2, item1, item0, LocalDateTime.MinValue, LocalDateTime.MinValue };
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
    #endregion

    #region Methods

    [Test]
    public void Increment_None_AreEqual()
    {
        var act = TestStruct;
        act++;
        var exp = new LocalDateTime(1988, 06, 14, 22, 10, 05, 001);
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void Decrement_None_AreEqual()
    {
        var act = TestStruct;
        act--;
        var exp = new LocalDateTime(1988, 06, 12, 22, 10, 05, 001);
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void Plus_TimeSpan_AreEqual()
    {
        var act = TestStruct + new TimeSpan(25, 30, 15);
        var exp = new LocalDateTime(1988, 06, 14, 23, 40, 20, 001);
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void Min_TimeSpan_AreEqual()
    {
        var act = TestStruct - new TimeSpan(25, 30, 15);
        var exp = new LocalDateTime(1988, 06, 12, 20, 39, 50, 001);
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void Min_LocalDateTimeTime_AreEqual()
    {
        var act = TestStruct - new LocalDateTime(1988, 06, 11, 20, 10, 05);
        var exp = new TimeSpan(2, 02, 00, 00, 001);
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void AddTicks_4000001700000_AreEqual()
    {
        var act = TestStruct.AddTicks(4000001700000L);
        var exp = new LocalDateTime(1988, 06, 18, 13, 16, 45, 171);

        Assert.AreEqual(exp, act);
    }

    [Test]
    public void AddMilliseconds_around3Days_AreEqual()
    {
        var act = TestStruct.AddMilliseconds(3 * 24 * 60 * 60 * 1003);
        var exp = new LocalDateTime(1988, 06, 16, 22, 23, 02, 601);

        Assert.AreEqual(exp, act);
    }
    [Test]
    public void AddSeconds_around3Days_AreEqual()
    {
        var act = TestStruct.AddSeconds(3 * 24 * 60 * 64);
        var exp = new LocalDateTime(1988, 06, 17, 02, 58, 05, 001);

        Assert.AreEqual(exp, act);
    }
    [Test]
    public void AddMinutes_2280_AreEqual()
    {
        var act = TestStruct.AddMinutes(2 * 24 * 60);
        var exp = new LocalDateTime(1988, 06, 15, 22, 10, 05, 001);

        Assert.AreEqual(exp, act);
    }

    [Test]
    public void AddHours_41_AreEqual()
    {
        var act = TestStruct.AddHours(41);
        var exp = new LocalDateTime(1988, 06, 15, 15, 10, 05, 001);

        Assert.AreEqual(exp, act);
    }

    [Test]
    public void AddMonths_12_AreEqual()
    {
        var act = TestStruct.AddMonths(12);
        var exp = new LocalDateTime(1989, 06, 13, 22, 10, 05, 001);

        Assert.AreEqual(exp, act);
    }

    [Test]
    public void Add_1Year_AreEqual()
    {
        var act = TestStruct + MonthSpan.FromYears(1);
        var exp = new LocalDateTime(1989, 06, 13, 22, 10, 05, 001);

        Assert.AreEqual(exp, act);
    }

    [Test]
    public void Subtract_1Month_AreEqual()
    {
        var act = TestStruct - MonthSpan.FromMonths(1);
        var exp = new LocalDateTime(1988, 05, 13, 22, 10, 05, 001);

        Assert.AreEqual(exp, act);
    }

    [Test]
    public void AddYears_Min12_AreEqual()
    {
        var act = TestStruct.AddYears(-12);
        var exp = new LocalDateTime(1976, 06, 13, 22, 10, 05, 001);

        Assert.AreEqual(exp, act);
    }

    #endregion
}

[Serializable]
public class LocalDateTimeSerializeObject
{
    public int Id { get; set; }
    public LocalDateTime Obj { get; set; }
    public DateTime Date { get; set; }
}
