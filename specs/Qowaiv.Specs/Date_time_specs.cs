﻿namespace DateTime_specs;

public class Can_be_adjusted_with
{
    [Test]
    public void Date_span_with_months_first()
        => new DateTime(2017, 06, 11).Add(new DateSpan(2, 20)).Should().Be(new DateTime(2017, 08, 31));

    [Test]
    public void Date_span_with_days_first()
        => new DateTime(2017, 06, 11).Add(new DateSpan(2, 20), DateSpanSettings.DaysOnly).Should().Be(new DateTime(2017, 09, 01));

    [Test]
    public void Month_span()
        => new DateTime(2017, 06, 11).Add(MonthSpan.FromMonths(3)).Should().Be(new DateTime(2017, 09, 11));
}
