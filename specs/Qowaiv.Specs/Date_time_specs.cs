namespace Date_time_specs;

public class Can_be_adjusted_with
{
    [Test]
    public void Date_span_with_months_first()
        => new DateTime(2017, 06, 11, 00, 00, 000, DateTimeKind.Local).Add(new DateSpan(2, 20))
        .Should().Be(new DateTime(2017, 08, 31, 00, 00, 000, DateTimeKind.Local));

    [Test]
    public void Date_span_with_days_first()
        => new DateTime(2017, 06, 11, 00, 00, 000, DateTimeKind.Local).Add(new DateSpan(2, 20), DateSpanSettings.DaysFirst)
        .Should().Be(new DateTime(2017, 09, 01, 00, 00, 000, DateTimeKind.Local));

    [Test]
    public void Month_span()
        => new DateTime(2017, 06, 11, 00, 00, 000, DateTimeKind.Local).Add(MonthSpan.FromMonths(3))
        .Should().Be(new DateTime(2017, 09, 11, 00, 00, 000, DateTimeKind.Local));
}

public class Can_not_be_adjusted_with
{
    [TestCase(DateSpanSettings.WithoutMonths)]
    [TestCase(DateSpanSettings.DaysFirst | DateSpanSettings.MixedSigns)]
    public void Date_span_with(DateSpanSettings settings)
        => new DateTime(2017, 06, 11, 00, 00, 000, DateTimeKind.Local).Invoking(d => d.Add(new DateSpan(2, 20), settings))
        .Should().Throw<ArgumentOutOfRangeException>().WithMessage("Adding a date span only supports 'Default' and 'DaysFirst'.*");
}

public class Can_be_related_to
{
    [Test]
    public void matching_month()
        => new DateTime(2017, 06, 11, 00, 00, 000, DateTimeKind.Local).IsIn(Month.June).Should().BeTrue();

    [Test]
    public void non_matching_month()
       => new DateTime(2017, 06, 11, 00, 00, 000, DateTimeKind.Local).IsIn(Month.February).Should().BeFalse();

    [Test]
    public void matching_year()
        => new DateTime(2017, 06, 11, 00, 00, 000, DateTimeKind.Local).IsIn(2017.CE()).Should().BeTrue();

    [Test]
    public void non_matching_year()
       => new DateTime(2017, 06, 11, 00, 00, 000, DateTimeKind.Local).IsIn(2018.CE()).Should().BeFalse();
}

public class Can_not_be_related_to
{
    [Test]
    public void month_empty()
        => new DateTime(2017, 06, 11, 00, 00, 000, DateTimeKind.Local).IsIn(Month.Empty).Should().BeFalse();

    [Test]
    public void month_unknown()
       => new DateTime(2017, 06, 11, 00, 00, 000, DateTimeKind.Local).IsIn(Month.Unknown).Should().BeFalse();

    [Test]
    public void year_empty()
        => new DateTime(2017, 06, 11, 00, 00, 000, DateTimeKind.Local).IsIn(Year.Empty).Should().BeFalse();

    [Test]
    public void year_unknown()
       => new DateTime(2017, 06, 11, 00, 00, 000, DateTimeKind.Local).IsIn(Year.Unknown).Should().BeFalse();
}
