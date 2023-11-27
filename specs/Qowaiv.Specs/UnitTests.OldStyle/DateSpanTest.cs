namespace Qowaiv.UnitTests;

/// <summary>Tests the date span SVO.</summary>
public class DateSpanTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly DateSpan TestStruct = new(10, 3, -5);
    public static readonly DateSpan Smaller = new(10, 3, -5);
    public static readonly DateSpan Bigger = new(10, 3, +02);

    #region date span const tests

    /// <summary>DateSpan.Zero should be equal to the default of date span.</summary>
    [Test]
    public void Zero_None_EqualsDefault()
    {
        DateSpan.Zero.Should().Be(default(DateSpan));
    }

    [Test]
    public void MaxValue_EqualsDateMaxDateMin()
    {
        var max = DateSpan.Subtract(Date.MaxValue, Date.MinValue);
        max.Should().Be(DateSpan.MaxValue);
    }

    [Test]
    public void MinValue_EqualsDateMinDateMax()
    {
        var min = DateSpan.Subtract(Date.MinValue, Date.MaxValue);
        min.Should().Be(DateSpan.MinValue);
    }

    #endregion

    #region TryParse tests

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_StringValue_IsValid()
    {
        string str = "5Y+3M+2D";
        DateSpan.TryParse(str, out DateSpan val).Should().BeTrue();
        Assert.AreEqual(str, val.ToString(), "Value");
    }

    [Test]
    public void Parse_InvalidInput_ThrowsFormatException()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Assert.Catch<FormatException>
            (() =>
            {
                DateSpan.Parse("InvalidInput");
            },
            "Not a valid date span");
        }
    }

    [Test]
    public void TryParse_TestStructInput_AreEqual()
    {
        using (TestCultures.En_GB.Scoped())
        {
            var exp = TestStruct;
            var act = DateSpan.TryParse(exp.ToString());

            act.Should().Be(exp);
        }
    }

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => DateSpan.TryParse("invalid input").Should().BeNull();

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
    public void XmlSerializeDeserialize_TestStruct_AreEqual()
    {
        var input = TestStruct;
        var exp = TestStruct;
        var act = SerializeDeserialize.Xml(input);
        act.Should().Be(exp);
    }

#if NET8_0_OR_GREATER
#else
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_DateSpanSerializeObject_AreEqual()
    {
        var input = new DateSpanSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new DateSpanSerializeObject
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
    public void XmlSerializeDeserialize_DateSpanSerializeObject_AreEqual()
    {
        var input = new DateSpanSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new DateSpanSerializeObject
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
    public void DataContractSerializeDeserialize_DateSpanSerializeObject_AreEqual()
    {
        var input = new DateSpanSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new DateSpanSerializeObject
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
        var input = new DateSpanSerializeObject
        {
            Id = 17,
            Obj = default,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new DateSpanSerializeObject
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
    public void XmlSerializeDeserialize_Default_AreEqual()
    {
        var input = new DateSpanSerializeObject
        {
            Id = 17,
            Obj = default,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new DateSpanSerializeObject
        {
            Id = 17,
            Obj = default,
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

    #region IFormattable / Tostring tests

    [Test]
    public void ToString_Zero_0Y0M0D()
    {
        var act = DateSpan.Zero.ToString();
        var exp = "0Y+0M+0D";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString_CustomFormatter_SupportsCustomFormatting()
    {
        var act = TestStruct.ToString("Unit Test Format", FormatProvider.CustomFormatter);
        var exp = "Unit Test Formatter, value: '10Y+3M-5D', format: 'Unit Test Format'";

        act.Should().Be(exp);
    }

    [TestCase("0Y+0M+0D", 0, 0)]
    [TestCase("0Y+0M+1D", 0, 1)]
    [TestCase("0Y+1M+0D", 1, 0)]
    [TestCase("1Y+0M+0D", 12, 0)]
    [TestCase("1Y+0M+1D", 12, 1)]
    [TestCase("1Y+0M+1D", 12, 1)]
    [TestCase("1Y+1M+1D", 13, 1)]
    [TestCase("0Y+0M-11D", 0, -11)]
    [TestCase("0Y+1M-12D", +1, -12)]
    [TestCase("0Y-1M-12D", -1, -12)]
    [TestCase("-1Y-1M+1D", -13, 1)]
    public void ToString_Invariant(string expected, int months, int days)
    {
        using (CultureInfoScope.NewInvariant())
        {
            var span = new DateSpan(months, days);
            span.ToString().Should().Be(expected);
        }
    }

    #endregion

    #region IEquatable tests

    /// <summary>GetHash should not fail for DateSpan.Zero.</summary>
    [Test]
    public void GetHash_Zero_Hash()
    {
        DateSpan.Zero.GetHashCode().Should().Be(0);
    }

    /// <summary>GetHash should not fail for the test struct.</summary>
    [Test]
    public void GetHash_TestStruct_Hash()
    {
        Assert.AreNotEqual(0, TestStruct.GetHashCode());
    }

    [Test]
    public void Equals_ZeroZero_IsTrue()
    {
        DateSpan.Zero.Equals(DateSpan.Zero).Should().BeTrue();
    }

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        var l = DateSpan.Parse("3Y-0M+3D", CultureInfo.InvariantCulture);
        var r = DateSpan.Parse("-0y+36m+3d", CultureInfo.InvariantCulture);

        l.Equals(r).Should().BeTrue();
    }

    [Test]
    public void Equals_TestStructTestStruct_IsTrue()
    {
        TestStruct.Equals(TestStruct).Should().BeTrue();
    }

    [Test]
    public void Equals_TestStructZero_IsFalse()
    {
        TestStruct.Equals(DateSpan.Zero).Should().BeFalse();
    }

    [Test]
    public void Equals_ZeroTestStruct_IsFalse()
    {
        DateSpan.Zero.Equals(TestStruct).Should().BeFalse();
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

    [Test]
    public void DaysPerMonth_ShouldMatchTheCalculatedValue()
    {
        var date = new Date(2000, 01, 01) - Date.MinValue;
        double daysPerMonth = date.TotalDays / 2000 / 12;
        var DateSpan_DaysPerMonth = (double)typeof(DateSpan).GetField("DaysPerMonth", (BindingFlags)int.MaxValue)!.GetValue(null)!;
        DateSpan_DaysPerMonth.Should().BeApproximately(daysPerMonth, precision: 0.000000001);
    }

    /// <summary>Orders a list of date spans ascending.</summary>
    [Test]
    public void OrderBy_DateSpan_AreEqual()
    {
        var item0 = new DateSpan(0, 00, -1);
        var item1 = new DateSpan(1, +2, 0);
        var item2 = new DateSpan(0, 00, +500);
        var item3 = new DateSpan(4, 00, -40);

        var inp = new List<DateSpan> { DateSpan.Zero, item3, item2, item0, item1, DateSpan.Zero };
        var exp = new List<DateSpan> { item0, DateSpan.Zero, DateSpan.Zero, item1, item2, item3 };
        var act = inp.OrderBy(item => item).ToList();

        CollectionAssert.AreEqual(exp, act);
    }

    /// <summary>Orders a list of date spans descending.</summary>
    [Test]
    public void OrderByDescending_DateSpan_AreEqual()
    {
        var item0 = new DateSpan(0, 00, -1);
        var item1 = new DateSpan(1, +2, 0);
        var item2 = new DateSpan(0, 00, +500);
        var item3 = new DateSpan(4, 00, -40);

        var inp = new List<DateSpan> { DateSpan.Zero, item3, item2, item0, item1, DateSpan.Zero };
        var exp = new List<DateSpan> { item3, item2, item1, DateSpan.Zero, DateSpan.Zero, item0 };
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
    public void Smaller_LessThan_Bigger_IsTrue()
    {
        (Smaller < Bigger).Should().BeTrue();
    }
    [Test]
    public void Bigger_GreaterThan_Smaller_IsTrue()
    {
        (Bigger > Smaller).Should().BeTrue();
    }

    [Test]
    public void Smaller_LessThanOrEqual_Bigger_IsTrue()
    {
        (Smaller <= Bigger).Should().BeTrue();
    }
    [Test]
    public void Bigger_GreaterThanOrEqual_Smaller_IsTrue()
    {
       (Bigger >= Smaller).Should().BeTrue();
    }

    [Test]
    public void Smaller_LessThanOrEqual_Smaller_IsTrue()
    {
        var left = Smaller;
        var right = Smaller;
        (left <= right).Should().BeTrue();
    }

    [Test]
    public void Smaller_GreaterThanOrEqual_Smaller_IsTrue()
    {
        var left = Smaller;
        var right = Smaller;
        (left >= right).Should().BeTrue();
    }

    #endregion

    #region Properties

    [TestCase(1, 2, +3)]
    [TestCase(0, 0, +3)]
    [TestCase(9, 6, +0)]
    [TestCase(9, 6, -1)]
    [TestCase(-9, -6, -1)]
    public void Days(int years, int months, int days)
    {
        var span = new DateSpan(years, months, days);
        span.Days.Should().Be(days);
    }

    [TestCase(1, 2, +3)]
    [TestCase(0, 0, +3)]
    [TestCase(9, 6, +0)]
    [TestCase(9, 6, -1)]
    [TestCase(-9, -6, -1)]
    public void Months(int years, int months, int days)
    {
        var span = new DateSpan(years, months, days);
        span.Months.Should().Be(months);
    }

    [TestCase(1, 2, +3)]
    [TestCase(0, 0, +3)]
    [TestCase(9, 6, +0)]
    [TestCase(9, 6, -1)]
    [TestCase(-9, -6, -1)]
    public void Years(int years, int months, int days)
    {
        var span = new DateSpan(years, months, days);
        span.Years.Should().Be(years);
    }

    [TestCase(014, 1, 2, +3)]
    [TestCase(012, 1, 0, +3)]
    [TestCase(117, 9, 9, +0)]
    [TestCase(006, 0, 6, -1)]
    [TestCase(-19, -1, -7, -1)]
    public void TotalMonths(int total, int years, int months, int days)
    {
        var span = new DateSpan(years, months, days);
        span.TotalMonths.Should().Be(total);
    }

    #endregion

    #region Operations

    [Test]
    public void Mutate_Overflows()
    {
        var x = Assert.Catch<OverflowException>(() => DateSpan.MaxValue.AddDays(1));
        x.Message.Should().Be("DateSpan overflowed because the resulting duration is too long.");
    }

    #endregion

    [Test]
    public void Ctor_OutOfRange_Throws()
    {
        var x = Assert.Catch<ArgumentOutOfRangeException>(() => new DateSpan(int.MaxValue, int.MaxValue));
        Assert.AreEqual("The specified years, months and days results in an un-representable DateSpan.", x.Message);
    }

    [Test]
    public void FromDays_4_4Days()
    {
        var span = DateSpan.FromDays(4);
        var exp = new DateSpan(0, 4);

        span.Should().Be(exp);
    }

    [Test]
    public void FromMonths_17_17Months()
    {
        var span = DateSpan.FromMonths(17);
        var exp = new DateSpan(17, 0);

        span.Should().Be(exp);
    }

    [Test]
    public void FromYears_17_17Years()
    {
        var span = DateSpan.FromYears(17);
        var exp = new DateSpan(17, 0, 0);

        span.Should().Be(exp);
    }
}

[Serializable]
public class DateSpanSerializeObject
{
    public int Id { get; set; }
    public DateSpan Obj { get; set; }
    public DateTime Date { get; set; }
}
