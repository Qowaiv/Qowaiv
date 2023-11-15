﻿namespace Qowaiv.UnitTests;

/// <summary>Tests the month span SVO.</summary>
public class MonthSpanTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly MonthSpan TestStruct = MonthSpan.FromMonths(69);

    /// <summary>MonthSpan.Zero should be equal to the default of month span.</summary>
    [Test]
    public void Zero_EqualsDefault()
    {
        Assert.AreEqual(default(MonthSpan), MonthSpan.Zero);
    }

    /// <summary>TryParse null should be valid.</summary>
    [Test]
    public void TryParse_Null_IsValid()
    {
        Assert.IsTrue(MonthSpan.TryParse(null, out var val));
        Assert.AreEqual(default(MonthSpan), val);
    }

    /// <summary>TryParse string.Empty should be valid.</summary>
    [Test]
    public void TryParse_StringEmpty_IsValid()
    {
        Assert.IsTrue(MonthSpan.TryParse(string.Empty, out var val));
        Assert.AreEqual(default(MonthSpan), val);
    }

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_StringValue_IsValid()
    {
        string str = "0Y+0M";
        Assert.IsTrue(MonthSpan.TryParse(str, out var val));
        Assert.AreEqual(str, val.ToString());
    }

    /// <summary>TryParse with specified string value should be invalid.</summary>
    [Test]
    public void TryParse_StringValue_IsNotValid()
    {
        string str = "5Y#9M";
        Assert.IsFalse(MonthSpan.TryParse(str, out var val));
        Assert.AreEqual(default(MonthSpan), val);
    }

    [Test]
    public void Parse_InvalidInput_ThrowsFormatException()
    {
        using (new CultureInfoScope("en-GB"))
        {
            Assert.Catch<FormatException>(() =>
            {
                MonthSpan.Parse("InvalidInput");
            }
            , "Not a valid month span");
        }
    }

    [Test]
    public void TryParse_TestStructInput_AreEqual()
    {
        using (new CultureInfoScope("en-GB"))
        {
            var exp = TestStruct;
            var act = MonthSpan.TryParse(exp.ToString());
            Assert.AreEqual(exp, act);
        }
    }

    [Test]
    public void from_invalid_as_null_with_TryParse()
       => MonthSpan.TryParse("invalid input").Should().BeNull();

    [Test]
    public void FromYears_20k_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => MonthSpan.FromYears(20_000));
    }

    [Test]
    public void FromMonths_200k_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => MonthSpan.FromMonths(200_000));
    }

    [Test]
    public void Constructor_OutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new MonthSpan(years: 9800, months: 5000));
    }

    [Test]
    public void Constructor_5Years9Months_69Months()
    {
        var ctor = new MonthSpan(years: 5, months: 9);
        Assert.AreEqual(MonthSpan.FromMonths(69), ctor);
    }

#if NET8_0_OR_GREATER
#else
    [Test]
    public void GetObjectData_NulSerializationInfo_Throws()
    {
        ISerializable obj = TestStruct;
        Assert.Catch<ArgumentNullException>(() => obj.GetObjectData(null!, default));
    }

    [Test]
    public void GetObjectData_SerializationInfo_AreEqual()
    {
        ISerializable obj = TestStruct;
        var info = new SerializationInfo(typeof(MonthSpan), new FormatterConverter());
        obj.GetObjectData(info, default);
        Assert.AreEqual(69, info.GetValue("Value", typeof(int)));
    }

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
        var exp = "5Y+9M";
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void XmlDeserialize_XmlString_AreEqual()
    {
        var act = Deserialize.Xml<MonthSpan>("69");
        Assert.AreEqual(TestStruct, act);
    }

#if NET8_0_OR_GREATER
#else
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_MonthSpanSerializeObject_AreEqual()
    {
        var input = new MonthSpanSerializeObject { Id = 17, Obj = TestStruct, Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local), };
        var exp = new MonthSpanSerializeObject { Id = 17, Obj = TestStruct, Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local), };
        var act = SerializeDeserialize.Binary(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }
#endif

    [Test]
    public void XmlSerializeDeserialize_MonthSpanSerializeObject_AreEqual()
    {
        var input = new MonthSpanSerializeObject { Id = 17, Obj = TestStruct, Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local), };
        var exp = new MonthSpanSerializeObject { Id = 17, Obj = TestStruct, Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local), };
        var act = SerializeDeserialize.Xml(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }

    [Test]
    public void DataContractSerializeDeserialize_MonthSpanSerializeObject_AreEqual()
    {
        var input = new MonthSpanSerializeObject { Id = 17, Obj = TestStruct, Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local), };
        var exp = new MonthSpanSerializeObject { Id = 17, Obj = TestStruct, Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local), };
        var act = SerializeDeserialize.DataContract(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }

#if NET8_0_OR_GREATER
#else
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_Default_AreEqual()
    {
        var input = new MonthSpanSerializeObject { Id = 17, Obj = default, Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local), };
        var exp = new MonthSpanSerializeObject { Id = 17, Obj = default, Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local), };
        var act = SerializeDeserialize.Binary(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }
#endif

    [Test]
    public void XmlSerializeDeserialize_Default_AreEqual()
    {
        var input = new MonthSpanSerializeObject { Id = 17, Obj = default, Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local), };
        var exp = new MonthSpanSerializeObject { Id = 17, Obj = default, Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local), };
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

    [Test]
    public void From_years_3_36M()
    {
        var span = MonthSpan.FromYears(3);
        Assert.AreEqual(MonthSpan.FromMonths(36), span);
    }

    [Test]
    public void ToString_Zero_StringEmpty()
    {
        var act = MonthSpan.Zero.ToString();
        var exp = "0Y+0M";
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void ToString_CustomFormatter_SupportsCustomFormatting()
    {
        var act = TestStruct.ToString("0.00", FormatProvider.CustomFormatter);
        var exp = "Unit Test Formatter, value: '69.00', format: '0.00'";
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void ToString_FormatValueSpanishEcuador_AreEqual()
    {
        var act = MonthSpan.Parse("1700").ToString("00000.0", new CultureInfo("es-EC"));
        var exp = "01700,0";
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        var l = MonthSpan.Parse("69", CultureInfo.InvariantCulture);
        var r = MonthSpan.Parse("5Y+9M", CultureInfo.InvariantCulture);
        Assert.IsTrue(l.Equals(r));
    }

    [Test]
    public void Plus_TestStruct_Unchanged()
    {
        var plus = +TestStruct;
        Assert.AreEqual(TestStruct, plus);
    }

    [Test]
    public void Negated_TestStruct_Min69Months()
    {
        var negated = -TestStruct;
        Assert.AreEqual(MonthSpan.FromMonths(-69), negated);
    }

    [Test]
    public void Add_1Year7Months_19Months()
    {
        var added = MonthSpan.FromYears(1) + MonthSpan.FromMonths(7);
        Assert.AreEqual(MonthSpan.FromMonths(19), added);
    }

    [Test]
    public void Add_2MonthsToDateTime_2MonthsLater()
    {
        var added = new DateTime(1979, 12, 31, 00, 00, 000, DateTimeKind.Local) + MonthSpan.FromMonths(2);
        added.Should().Be(new DateTime(1980, 02, 29, 00, 00, 000, DateTimeKind.Local));
    }

    [Test]
    public void Subtract_19Months6Months_13Months()
    {
        var subtracted = MonthSpan.FromMonths(19) - MonthSpan.FromMonths(6);
        Assert.AreEqual(MonthSpan.FromMonths(13), subtracted);
    }

    [Test]
    public void Subtract_9MonthsFromDateTime_9MonthsEarlier()
    {
        var added = new DateTime(2017, 06, 11, 00, 00, 000, DateTimeKind.Local) - MonthSpan.FromMonths(9);
        added.Should().Be(new DateTime(2016, 09, 11, 00, 00, 000, DateTimeKind.Local));
    }

    [TestCase(0, "2020-04-30", "2020-04-01")]
    [TestCase(1, "2020-04-30", "2020-03-31")]
    [TestCase(11, "2020-01-01", "2019-01-02")]
    [TestCase(9, "2020-01-01", "2019-03-13")]
    [TestCase(10, "2020-01-01", "2019-03-01")]
    [TestCase(-1, "2020-01-01", "2020-02-20")]
    public void Subtract_TwoDates(MonthSpan expected, Date d1, Date d2)
    {
        var delta = MonthSpan.Subtract(d1, d2);
        Assert.AreEqual(expected, delta);
    }

    /// <summary>Orders a list of month spans ascending.</summary>
    [Test]
    public void OrderBy_MonthSpan_AreEqual()
    {
        var item0 = MonthSpan.FromMonths(1);
        var item1 = MonthSpan.FromMonths(12);
        var item2 = MonthSpan.FromMonths(13);
        var item3 = MonthSpan.FromMonths(145);
        var inp = new List<MonthSpan> { MonthSpan.Zero, item3, item2, item0, item1, MonthSpan.Zero };
        var exp = new List<MonthSpan> { MonthSpan.Zero, MonthSpan.Zero, item0, item1, item2, item3 };
        var act = inp.OrderBy(item => item).ToList();
        CollectionAssert.AreEqual(exp, act);
    }

    /// <summary>Orders a list of month spans descending.</summary>
    [Test]
    public void OrderByDescending_MonthSpan_AreEqual()
    {
        var item0 = MonthSpan.FromMonths(1);
        var item1 = MonthSpan.FromMonths(12);
        var item2 = MonthSpan.FromMonths(13);
        var item3 = MonthSpan.FromMonths(145);
        var inp = new List<MonthSpan> { MonthSpan.Zero, item3, item2, item0, item1, MonthSpan.Zero };
        var exp = new List<MonthSpan> { item3, item2, item1, item0, MonthSpan.Zero, MonthSpan.Zero };
        var act = inp.OrderByDescending(item => item).ToList();
        CollectionAssert.AreEqual(exp, act);
    }

    [Test]
    public void Explicit_Int32ToMonthSpan_AreEqual()
    {
        var exp = TestStruct;
        var act = (MonthSpan)69;
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void Explicit_MonthSpanToInt32_AreEqual()
    {
        var exp = 69;
        var act = (int)TestStruct;
        Assert.AreEqual(exp, act);
    }
}

[Serializable]
public class MonthSpanSerializeObject
{
    public int Id { get; set; }
    public MonthSpan Obj { get; set; }
    public DateTime Date { get; set; }
}
