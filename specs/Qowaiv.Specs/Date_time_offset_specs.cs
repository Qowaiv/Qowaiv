using FluentAssertions.Extensions;

namespace Date_time_offset_specs;

public class Can_be_adjusted_with
{
    [Test]
    public void Date_span_with_months_first()
        => 11.June(2017).At(06, 15).WithOffset(TimeSpan.FromHours(+2)).Add(new DateSpan(2, 20))
        .Should().Be(31.August(2017).At(06, 15).WithOffset(TimeSpan.FromHours(+2)));

    [Test]
    public void Date_span_with_days_first()
        => 11.June(2017).At(06, 15).WithOffset(TimeSpan.FromHours(+2)).Add(new DateSpan(2, 20), DateSpanSettings.DaysFirst)
        .Should().Be(01.September(2017).At(06, 15).WithOffset(TimeSpan.FromHours(+2)));

    [Test]
    public void Month_span()
        => 11.June(2017).At(06, 15).WithOffset(TimeSpan.FromHours(+2)).Add(MonthSpan.FromMonths(3))
        .Should().Be(11.September(2017).At(06, 15).WithOffset(TimeSpan.FromHours(+2)));
}

public class With_local
{
    [Test]
    public void represents_a_local_date_time()
    {
        11.June(2017).At(06, 15).WithOffset(TimeSpan.FromHours(+2)).ToLocal()
            .Should().Be(new LocalDateTime(2017, 06, 11, 06, 15));
    }
}

public class Can_not_be_adjusted_with
{
    [TestCase(DateSpanSettings.WithoutMonths)]
    [TestCase(DateSpanSettings.DaysFirst | DateSpanSettings.MixedSigns)]
    public void Date_span_with(DateSpanSettings settings)
        => 11.June(2017).At(06, 15).WithOffset(TimeSpan.FromHours(+2)).Invoking(d => d.Add(new DateSpan(2, 20), settings))
        .Should().Throw<ArgumentOutOfRangeException>().WithMessage("Adding a date span only supports 'Default' and 'DaysFirst'.*");
}

public class Can_be_related_to
{
    [Test]
    public void matching_month()
        => 11.June(2017).At(06, 15).WithOffset(TimeSpan.FromHours(+2)).IsIn(Month.June).Should().BeTrue();

    [Test]
    public void none_matching_month()
       => 11.June(2017).At(06, 15).WithOffset(TimeSpan.FromHours(+2)).IsIn(Month.February).Should().BeFalse();

    [Test]
    public void matching_year()
        => 11.June(2017).At(06, 15).WithOffset(TimeSpan.FromHours(+2)).IsIn(2017.CE()).Should().BeTrue();

    [Test]
    public void none_matching_year()
       => 11.June(2017).At(06, 15).WithOffset(TimeSpan.FromHours(+2)).IsIn(2018.CE()).Should().BeFalse();
}

public class Can_not_be_related_to
{
    [Test]
    public void month_empty()
        => 11.June(2017).At(06, 15).WithOffset(TimeSpan.FromHours(+2)).IsIn(Month.Empty).Should().BeFalse();

    [Test]
    public void month_unknown()
       => 11.June(2017).At(06, 15).WithOffset(TimeSpan.FromHours(+2)).IsIn(Month.Unknown).Should().BeFalse();

    [Test]
    public void year_empty()
        => 11.June(2017).At(06, 15).WithOffset(TimeSpan.FromHours(+2)).IsIn(Year.Empty).Should().BeFalse();

    [Test]
    public void year_unknown()
       => 11.June(2017).At(06, 15).WithOffset(TimeSpan.FromHours(+2)).IsIn(Year.Unknown).Should().BeFalse();
}
