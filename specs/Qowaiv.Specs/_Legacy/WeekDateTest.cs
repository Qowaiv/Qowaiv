namespace Qowaiv.UnitTests;

/// <summary>Tests the week date SVO.</summary>
public class WeekDateTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly WeekDate TestStruct = new(1997, 14, 6);

    #region week date const tests

    /// <summary>WeekDate.MinValue should be equal to the default of week date.</summary>
    [Test]
    public void MinValue_None_EqualsDefault() => WeekDate.MinValue.Should().Be(default);

    #endregion

    #region Properties

    [Test]
    public void Date_TestStruct_AreEqual()
    {
        var exp = new Date(1997, 04, 05);
        var act = TestStruct.Date;

        act.Should().Be(exp);
    }

    [Test]
    public void Year_MinValue_AreEqual()
    {
        var exp = WeekDate.MinValue.Year;
        var act = 1;

        act.Should().Be(exp);
    }

    [Test]
    public void Year_MaxValue_AreEqual()
    {
        var exp = WeekDate.MaxValue.Year;
        var act = 9999;

        act.Should().Be(exp);
    }

    [Test]
    public void Year_Y2010W52D7_AreEqual()
    {
        var date = new WeekDate(2010, 52, 7);
        var exp = 2010;
        var act = date.Year;

        act.Should().Be(exp);
    }

    [Test]
    public void Year_Y2020W01D1_AreEqual()
    {
        var date = new WeekDate(2020, 01, 1);
        var exp = 2020;
        var act = date.Year;

        act.Should().Be(exp);
    }




    [Test]
    public void Day_TestStruct_AreEqual()
    {
        var exp = 6;
        var act = TestStruct.Day;

        act.Should().Be(exp);
    }

    [Test]
    public void Day_Sunday_AreEqual()
    {
        var date = new WeekDate(1990, 40, 7);
        var exp = 7;
        var act = date.Day;

        act.Should().Be(exp);
    }

    [Test]
    public void DayOfYear_TestStruct_AreEqual()
    {
        var exp = 96;
        var act = TestStruct.DayOfYear;

        act.Should().Be(exp);
    }



    #endregion
}
