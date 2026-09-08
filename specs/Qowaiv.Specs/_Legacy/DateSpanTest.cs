namespace Qowaiv.UnitTests;

/// <summary>Tests the date span SVO.</summary>
public class DateSpanTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly DateSpan TestStruct = new(10, 3, -5);

    #region date span const tests

    /// <summary>DateSpan.Zero should be equal to the default of date span.</summary>
    [Test]
    public void Zero_None_EqualsDefault() => DateSpan.Zero.Should().Be(default);

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
