namespace DateTime_specs;

public class Can_be_adjusted_with
{
    [Test]
    public void Date_span_with_months_first()
        => new DateTime(2017, 06, 11).Add(new DateSpan(2, 20)).Should().Be(new DateTime(2017, 08, 31));

    [Test]
    public void Date_span_with_days_first()
        => new DateTime(2017, 06, 11).Add(new DateSpan(2, 20), DateSpanSettings.DaysFirst).Should().Be(new DateTime(2017, 09, 01));

    [Test]
    public void Month_span()
        => new DateTime(2017, 06, 11).Add(MonthSpan.FromMonths(3)).Should().Be(new DateTime(2017, 09, 11));
}

public class Can_not_be_adjusted_with
{
    [TestCase(DateSpanSettings.WithoutMonths)]
    [TestCase(DateSpanSettings.DaysFirst | DateSpanSettings.MixedSigns)]
    public void Date_span_with(DateSpanSettings settings)
        => new DateTime(2017, 06, 11).Invoking(d => d.Add(new DateSpan(2, 20), settings))
        .Should().Throw<ArgumentOutOfRangeException>().WithMessage("Adding a date span only supports 'Default' and 'DaysFirst'.*");
}
