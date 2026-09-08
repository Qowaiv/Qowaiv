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
    public void MinValue_None_EqualsDefault() => Date.MinValue.Should().Be(default);

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
    public void AddYears_Min12_AreEqual()
    {
        var act = TestStruct.AddYears(-12);
        var exp = new Date(1958, 02, 14);

        act.Should().Be(exp);
    }

    #endregion
}
