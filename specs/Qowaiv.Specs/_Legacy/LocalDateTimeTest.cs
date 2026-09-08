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
    public void MinValue_None_EqualsDefault() => LocalDateTime.MinValue.Should().Be(default);

    #endregion

    #region Methods

    [Test]
    public void Increment_None_AreEqual()
    {
        var act = TestStruct;
        act++;
        var exp = new LocalDateTime(1988, 06, 14, 22, 10, 05, 001);
        act.Should().Be(exp);
    }
    [Test]
    public void Decrement_None_AreEqual()
    {
        var act = TestStruct;
        act--;
        var exp = new LocalDateTime(1988, 06, 12, 22, 10, 05, 001);
        act.Should().Be(exp);
    }

    [Test]
    public void Plus_TimeSpan_AreEqual()
    {
        var act = TestStruct + new TimeSpan(25, 30, 15);
        var exp = new LocalDateTime(1988, 06, 14, 23, 40, 20, 001);
        act.Should().Be(exp);
    }
    [Test]
    public void Min_TimeSpan_AreEqual()
    {
        var act = TestStruct - new TimeSpan(25, 30, 15);
        var exp = new LocalDateTime(1988, 06, 12, 20, 39, 50, 001);
        act.Should().Be(exp);
    }

    [Test]
    public void Min_LocalDateTimeTime_AreEqual()
    {
        var act = TestStruct - new LocalDateTime(1988, 06, 11, 20, 10, 05);
        var exp = new TimeSpan(2, 02, 00, 00, 001);
        act.Should().Be(exp);
    }

    [Test]
    public void AddTicks_4000001700000_AreEqual()
    {
        var act = TestStruct.AddTicks(4000001700000L);
        var exp = new LocalDateTime(1988, 06, 18, 13, 16, 45, 171);

        act.Should().Be(exp);
    }

    [Test]
    public void AddMilliseconds_around3Days_AreEqual()
    {
        var act = TestStruct.AddMilliseconds(3 * 24 * 60 * 60 * 1003);
        var exp = new LocalDateTime(1988, 06, 16, 22, 23, 02, 601);

        act.Should().Be(exp);
    }
    [Test]
    public void AddSeconds_around3Days_AreEqual()
    {
        var act = TestStruct.AddSeconds(3 * 24 * 60 * 64);
        var exp = new LocalDateTime(1988, 06, 17, 02, 58, 05, 001);

        act.Should().Be(exp);
    }
    [Test]
    public void AddMinutes_2280_AreEqual()
    {
        var act = TestStruct.AddMinutes(2 * 24 * 60);
        var exp = new LocalDateTime(1988, 06, 15, 22, 10, 05, 001);

        act.Should().Be(exp);
    }

    [Test]
    public void AddHours_41_AreEqual()
    {
        var act = TestStruct.AddHours(41);
        var exp = new LocalDateTime(1988, 06, 15, 15, 10, 05, 001);

        act.Should().Be(exp);
    }

    [Test]
    public void AddMonths_12_AreEqual()
    {
        var act = TestStruct.AddMonths(12);
        var exp = new LocalDateTime(1989, 06, 13, 22, 10, 05, 001);

        act.Should().Be(exp);
    }

    [Test]
    public void Add_1Year_AreEqual()
    {
        var act = TestStruct + MonthSpan.FromYears(1);
        var exp = new LocalDateTime(1989, 06, 13, 22, 10, 05, 001);

        act.Should().Be(exp);
    }

    [Test]
    public void Subtract_1Month_AreEqual()
    {
        var act = TestStruct - MonthSpan.FromMonths(1);
        var exp = new LocalDateTime(1988, 05, 13, 22, 10, 05, 001);

        act.Should().Be(exp);
    }

    [Test]
    public void AddYears_Min12_AreEqual()
    {
        var act = TestStruct.AddYears(-12);
        var exp = new LocalDateTime(1976, 06, 13, 22, 10, 05, 001);

        act.Should().Be(exp);
    }

    #endregion
}
