﻿namespace Qowaiv.UnitTests.Sql;

/// <summary>Tests the timestamp SVO.</summary>
public class TimestampTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly Timestamp TestStruct = 123456789L;

    #region TryParse tests

    [Test]
    public void TryParse_0x00000000075BCD15_IsValid()
    {
        string str = "0x00000000075BCD15";

        Timestamp.TryParse(str, out Timestamp val).Should().BeTrue();
        Assert.AreEqual(TestStruct, val, "Value");
    }

    [Test]
    public void TryParse_123456789_IsValid()
    {
        string str = "123456789";

        Timestamp.TryParse(str, out Timestamp val).Should().BeTrue();
        Assert.AreEqual(TestStruct, val, "Value");
    }

    /// <summary>TryParse with specified string value should be invalid.</summary>
    [Test]
    public void TryParse_0xInvalidTimeStamp_IsNotValid()
    {
        string str = "0xInvalidTimeStamp";

        Timestamp.TryParse(str, out Timestamp val).Should().BeFalse();
        Assert.AreEqual(Timestamp.MinValue, val, "Value");
    }

    [Test]
    public void Parse_InvalidInput_ThrowsFormatException()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Assert.Catch<FormatException>
            (() =>
            {
                Timestamp.Parse("InvalidInput");
            },
            "Not a valid SQL timestamp");
        }
    }

    [Test]
    public void TryParse_TestStructInput_AreEqual()
    {
        using (TestCultures.En_GB.Scoped())
        {
            var exp = TestStruct;
            var act = Timestamp.TryParse(exp.ToString());

            act.Should().Be(exp);
        }
    }

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => Timestamp.TryParse("invalid input").Should().BeNull();

    #endregion

    #region (XML) (De)serialization tests

#if NET8_0_OR_GREATER
#else
    [Test]
    public void GetObjectData_SerializationInfo_AreEqual()
    {
        ISerializable obj = TestStruct;
        var info = new SerializationInfo(typeof(Timestamp), new FormatterConverter());
        obj.GetObjectData(info, default);

        info.GetInt64("Value").Should().Be((long)123456789);
    }

    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_TestStruct_AreEqual()
        => SerializeDeserialize.Binary(TestStruct).Should().Be(TestStruct);
#endif

    [Test]
    public void DataContractSerializeDeserialize_TestStruct_AreEqual()
        => SerializeDeserialize.DataContract(TestStruct).Should().Be(TestStruct);

    [Test]
    public void XmlSerialize_TestStruct_AreEqual()
    {
        var act = Serialize.Xml(TestStruct);
        var exp = "0x00000000075BCD15";
        act.Should().Be(exp);
    }

    [Test]
    public void XmlDeserialize_XmlString_AreEqual()
    {
        var act = Deserialize.Xml<Timestamp>("0x00000000075BCD15");
        act.Should().Be(TestStruct);
    }

#if NET8_0_OR_GREATER
#else
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_TimestampSerializeObject_AreEqual()
    {
        var input = new TimestampSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new TimestampSerializeObject
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
    public void XmlSerializeDeserialize_TimestampSerializeObject_AreEqual()
    {
        var input = new TimestampSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new TimestampSerializeObject
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
    public void DataContractSerializeDeserialize_TimestampSerializeObject_AreEqual()
    {
        var input = new TimestampSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new TimestampSerializeObject
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
    public void SerializeDeserialize_Default_AreEqual()
    {
        var input = new TimestampSerializeObject
        {
            Id = 17,
            Obj = default,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new TimestampSerializeObject
        {
            Id = 17,
            Obj = default,
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
        var input = new TimestampSerializeObject
        {
            Id = 17,
            Obj = Timestamp.MinValue,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new TimestampSerializeObject
        {
            Id = 17,
            Obj = Timestamp.MinValue,
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
    public void ToString_MinValue_StringEmpty()
    {
        var act = Timestamp.MinValue.ToString();
        var exp = "0x0000000000000000";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString_MaxValue_QuestionMark()
    {
        var act = Timestamp.MaxValue.ToString();
        var exp = "0xFFFFFFFFFFFFFFFF";
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
        var exp = "0x00000000075BCD15";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString_ValueDutchBelgium_AreEqual()
    {
        using (TestCultures.Nl_BE.Scoped())
        {
            var act = Timestamp.Parse("1600").ToString();
            var exp = "0x0000000000000640";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void ToString_FormatValueDutchBelgium_AreEqual()
    {
        using (TestCultures.Nl_BE.Scoped())
        {
            var act = Timestamp.Parse("800").ToString("0000");
            var exp = "0800";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void ToString_FormatValueEnglishGreatBritain_AreEqual()
    {
        using (TestCultures.En_GB.Scoped())
        {
            var act = Timestamp.Parse("800").ToString("0000");
            var exp = "0800";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void ToString_FormatValueSpanishEcuador_AreEqual()
    {
        var act = Timestamp.Parse("1700").ToString("00000.0", new CultureInfo("es-EC"));
        var exp = "01700,0";
        act.Should().Be(exp);
    }

    #endregion

    #region IEquatable tests

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        var l = Timestamp.Parse("0x75bcd15", CultureInfo.InvariantCulture);
        var r = Timestamp.Parse("0x00000000075BCD15", CultureInfo.InvariantCulture);

        l.Equals(r).Should().BeTrue();
    }

    #endregion

    #region IComparable tests

    /// <summary>Orders a list of timestamps ascending.</summary>
    [Test]
    public void OrderBy_Timestamp_AreEqual()
    {
        Timestamp item0 = 3245;
        Timestamp item1 = 13245;
        Timestamp item2 = 132456;
        Timestamp item3 = 1324589;

        var inp = new List<Timestamp> { Timestamp.MinValue, item3, item2, item0, item1, Timestamp.MinValue };
        var exp = new List<Timestamp> { Timestamp.MinValue, Timestamp.MinValue, item0, item1, item2, item3 };
        var act = inp.OrderBy(item => item).ToList();

        act.Should().BeEquivalentTo(exp);
    }

    /// <summary>Orders a list of timestamps descending.</summary>
    [Test]
    public void OrderByDescending_Timestamp_AreEqual()
    {
        Timestamp item0 = 3245;
        Timestamp item1 = 13245;
        Timestamp item2 = 132456;
        Timestamp item3 = 1324589;

        var inp = new List<Timestamp> { Timestamp.MinValue, item3, item2, item0, item1, Timestamp.MinValue };
        var exp = new List<Timestamp> { item3, item2, item1, item0, Timestamp.MinValue, Timestamp.MinValue };
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
        Timestamp l = 17;
        Timestamp r = 19;

        (l < r).Should().BeTrue();
    }
    [Test]
    public void GreaterThan_21LT19_IsTrue()
    {
        Timestamp l = 21;
        Timestamp r = 19;

        (l > r).Should().BeTrue();
    }

    [Test]
    public void LessThanOrEqual_17LT19_IsTrue()
    {
        Timestamp l = 17;
        Timestamp r = 19;

        (l <= r).Should().BeTrue();
    }
    [Test]
    public void GreaterThanOrEqual_21LT19_IsTrue()
    {
        Timestamp l = 21;
        Timestamp r = 19;

        (l >= r).Should().BeTrue();
    }

    [Test]
    public void LessThanOrEqual_17LT17_IsTrue()
    {
        Timestamp l = 17;
        Timestamp r = 17;

        (l <= r).Should().BeTrue();
    }
    [Test]
    public void GreaterThanOrEqual_21LT21_IsTrue()
    {
        Timestamp l = 21;
        Timestamp r = 21;

        (l >= r).Should().BeTrue();
    }
    #endregion

    #region Methods

    [Test]
    public void ToByteArray_TestStruct_()
    {
        var act = TestStruct.ToByteArray();
        var exp = new byte[] { 21, 205, 91, 7, 0, 0, 0, 0 };

        act.Should().BeEquivalentTo(exp);
    }

    #endregion

    #region Casting tests

    [Test]
    public void Explicit_ByteArrayToTimestamp_AreEqual()
    {
        var exp = TestStruct;
        var act = (Timestamp)new byte[] { 21, 205, 91, 7, 0, 0, 0, 0 };

        act.Should().Be(exp);
    }
    [Test]
    public void Explicit_TimestampToByteArray_AreEqual()
    {
        var exp = new byte[] { 21, 205, 91, 7, 0, 0, 0, 0 };
        var act = (byte[])TestStruct;

        act.Should().BeEquivalentTo(exp);
    }

    [Test]
    public void Explicit_Int64ToTimestamp_AreEqual()
    {
        var exp = TestStruct;
        var act = (Timestamp)123456789L;

        act.Should().Be(exp);
    }
    [Test]
    public void Explicit_TimestampToInt64_AreEqual()
    {
        var exp = 123456789L;
        var act = (long)TestStruct;

        act.Should().Be(exp);
    }

    [Test]
    public void Explicit_UInt64ToTimestamp_AreEqual()
    {
        var exp = TestStruct;
        var act = (Timestamp)123456789UL;

        act.Should().Be(exp);
    }
    [Test]
    public void Explicit_TimestampToUInt64_AreEqual()
    {
        var exp = 123456789UL;
        var act = (ulong)TestStruct;

        act.Should().Be(exp);
    }

    #endregion
}

[Serializable]
public class TimestampSerializeObject
{
    public int Id { get; set; }
    public Timestamp Obj { get; set; }
    public DateTime Date { get; set; }
}
