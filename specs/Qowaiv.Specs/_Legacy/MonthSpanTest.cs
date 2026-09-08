namespace Qowaiv.UnitTests;

/// <summary>Tests the month span SVO.</summary>
public class MonthSpanTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly MonthSpan TestStruct = MonthSpan.FromMonths(69);

    /// <summary>MonthSpan.Zero should be equal to the default of month span.</summary>
    [Test]
    public void Zero_EqualsDefault() => MonthSpan.Zero.Should().Be(default);

    /// <summary>TryParse null should be valid.</summary>
    [Test]
    public void TryParse_Null_IsValid()
    {
        MonthSpan.TryParse(null, out var val).Should().BeTrue();
        val.Should().Be(default);
    }

    /// <summary>TryParse string.Empty should be valid.</summary>
    [Test]
    public void TryParse_StringEmpty_IsValid()
    {
        MonthSpan.TryParse(string.Empty, out var val).Should().BeTrue();
        val.Should().Be(default);
    }

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_StringValue_IsValid()
    {
        string str = "0Y+0M";
        MonthSpan.TryParse(str, out var val).Should().BeTrue();
        val.ToString().Should().Be(str);
    }

    /// <summary>TryParse with specified string value should be invalid.</summary>
    [Test]
    public void TryParse_StringValue_IsNotValid()
    {
        string str = "5Y#9M";
        MonthSpan.TryParse(str, out var val).Should().BeFalse();
        val.Should().Be(default);
    }

    [Test]
    public void TryParse_TestStructInput_AreEqual()
    {
        using (new CultureInfoScope("en-GB"))
        {
            var exp = TestStruct;
            var act = MonthSpan.TryParse(exp.ToString());
            act.Should().Be(exp);
        }
    }

    [Test]
    public void from_invalid_as_null_with_TryParse()
       => MonthSpan.TryParse("invalid input").Should().BeNull();

    [Test]
    public void Constructor_5Years9Months_69Months()
    {
        var ctor = new MonthSpan(years: 5, months: 9);
        ctor.Should().Be(MonthSpan.FromMonths(69));
    }

    [Test]
    public void From_years_3_36M()
    {
        var span = MonthSpan.FromYears(3);
        span.Should().Be(MonthSpan.FromMonths(36));
    }

    [Test]
    public void ToString_Zero_StringEmpty()
    {
        var act = MonthSpan.Zero.ToString();
        var exp = "0Y+0M";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString_CustomFormatter_SupportsCustomFormatting()
    {
        var act = TestStruct.ToString("0.00", FormatProvider.CustomFormatter);
        var exp = "Unit Test Formatter, value: '69.00', format: '0.00'";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString_FormatValueSpanishEcuador_AreEqual()
    {
        var act = MonthSpan.Parse("1700").ToString("00000.0", new CultureInfo("es-EC"));
        var exp = "01700,0";
        act.Should().Be(exp);
    }

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        var l = MonthSpan.Parse("69", CultureInfo.InvariantCulture);
        var r = MonthSpan.Parse("5Y+9M", CultureInfo.InvariantCulture);
        l.Equals(r).Should().BeTrue();
    }

    [Test]
    public void Plus_TestStruct_Unchanged()
    {
        var plus = +TestStruct;
        plus.Should().Be(TestStruct);
    }

    [Test]
    public void Negated_TestStruct_Min69Months()
    {
        var negated = -TestStruct;
        Should.BeEqual(MonthSpan.FromMonths(-69), negated);
    }

    [Test]
    public void Add_1Year7Months_19Months()
    {
        var added = MonthSpan.FromYears(1) + MonthSpan.FromMonths(7);
        added.Should().Be(MonthSpan.FromMonths(19));
    }

    [Test]
    public void Subtract_19Months6Months_13Months()
    {
        var subtracted = MonthSpan.FromMonths(19) - MonthSpan.FromMonths(6);
        subtracted.Should().Be(MonthSpan.FromMonths(13));
    }

    [Test]
    public void Explicit_Int32ToMonthSpan_AreEqual()
    {
        var exp = TestStruct;
        var act = (MonthSpan)69;
        act.Should().Be(exp);
    }

    [Test]
    public void Explicit_MonthSpanToInt32_AreEqual()
    {
        var exp = 69;
        var act = (int)TestStruct;
        act.Should().Be(exp);
    }
}
