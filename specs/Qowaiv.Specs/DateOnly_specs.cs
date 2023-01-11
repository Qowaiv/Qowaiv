#if NET6_0_OR_GREATER

namespace DateOnly_specs;

public class Can_be_related_to
{
    [Test]
    public void matching_month()
        => new DateOnly(2017, 06, 11).InMonth(Month.June).Should().BeTrue();

    [Test]
    public void none_matching_month()
       => new DateOnly(2017, 06, 11).InMonth(Month.February).Should().BeFalse();

    [Test]
    public void matching_year()
        => new DateOnly(2017, 06, 11).InYear(2017.CE()).Should().BeTrue();

    [Test]
    public void none_matching_year()
       => new DateOnly(2017, 06, 11).InYear(2018.CE()).Should().BeFalse();
}

public class Can_not_be_related_to
{
    [Test]
    public void month_empty()
        => new DateOnly(2017, 06, 11).InMonth(Month.Empty).Should().BeFalse();

    [Test]
    public void month_unknown()
       => new DateOnly(2017, 06, 11).InMonth(Month.Unknown).Should().BeFalse();

    [Test]
    public void year_empty()
        => new DateOnly(2017, 06, 11).InYear(Year.Empty).Should().BeFalse();

    [Test]
    public void year_unknown()
       => new DateOnly(2017, 06, 11).InYear(Year.Unknown).Should().BeFalse();
}

public class Can_be_adjusted_with
{
    [Test]
    public void Date_span_with_months_first()
        => new DateOnly(2017, 06, 11).Add(new DateSpan(2, 20)).Should().Be(new DateOnly(2017, 08, 31));

    [Test]
    public void Date_span_with_days_first()
        => new DateOnly(2017, 06, 11).Add(new DateSpan(2, 20), true).Should().Be(new DateOnly(2017, 09, 01));

    [Test]
    public void Month_span()
        => new DateOnly(2017, 06, 11).Add(MonthSpan.FromMonths(3)).Should().Be(new DateOnly(2017, 09, 11));
}

#endif
