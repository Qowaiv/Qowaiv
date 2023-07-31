namespace Qowaiv.UnitTests;

public class DateSpanOperationTest
{
    [Test]
    public void Add_DateSpan_ShouldAddDaysSecond()
    {
        var date = new DateTime(1999, 01, 30, 00, 00, 00, 000, DateTimeKind.Utc);
        var span = DateSpan.FromMonths(1);
        date.Add(span).Should().Be(new DateTime(1999, 02, 28, 00, 00, 000, DateTimeKind.Utc));
    }
}
