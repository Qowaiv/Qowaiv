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

    #region IEquatable tests

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        var l = Date.Parse("1970-02-14", CultureInfo.InvariantCulture);
        var r = Date.Parse("14 february 1970", CultureInfo.InvariantCulture);

        l.Equals(r).Should().BeTrue();
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
