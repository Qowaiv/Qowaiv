﻿using System.Threading;

namespace Clock_specs;

public class Default_behavior
{
    [Test]
    public void UtcNow_equals_DateTime_UtcNow()
        => Clock.UtcNow().Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10));

    [Test]
    public void TimeZone_equals_TimeZoneInfo_Local()
        => Clock.TimeZone.Should().Be(TimeZoneInfo.Local);

    [Test]
    public void Now_equals_UTC_now_with_the_time_zone_offset()
    {
        using (Clock.SetTimeAndTimeZoneForCurrentContext(() => Svo.DateTime, TestTimeZones.EastAustraliaStandardTime))
        {
            Clock.Now().Should().Be(new LocalDateTime(2017, 06, 11, 16, 15, 00));
        }
    }

    [Test]
    public void NowWithOffset_equals_UTC_now_with_the_time_zone_offset()
    {
        using (Clock.SetTimeAndTimeZoneForCurrentContext(() => Svo.DateTime, Svo.TimeZone))
        {
            var date_time_offset = new DateTimeOffset(new DateTime(2017, 06, 11, 16, 15, 0, DateTimeKind.Unspecified), TimeSpan.FromHours(+10));
            Clock.NowWithOffset().Should().Be(date_time_offset);
        }
    }
}

[NonParallelizable]
public class Globally
{
    [Test]
    public void UtcNow_can_be_set()
    {
        Clock.SetTime(() => Svo.DateTime);
        Clock.UtcNow().Should().Be(Svo.DateTime);
    }

    [Test]
    public void TimeZone_can_be_set()
    {
        Clock.SetTimeZone(Svo.TimeZone);
        Clock.TimeZone.Should().Be(Svo.TimeZone);
    }

    [TearDown]
    public void TearDown()
    {
        Clock.SetTime(() => DateTime.UtcNow);
        Clock.SetTimeZone(TimeZoneInfo.Local);
    }
}

public class Date_time_kind
{
    [TestCase(DateTimeKind.Utc)]
    [TestCase(DateTimeKind.Local)]
    [TestCase(DateTimeKind.Unspecified)]
    public void UTC_for_UtcNow(DateTimeKind kind)
    {
        using (Clock.SetTimeForCurrentContext(() => new DateTime(2017, 06, 11, 06, 15, 0, kind)))
        {
            Clock.UtcNow().Kind.Should().Be(DateTimeKind.Utc);
        }
    }
}

public class For_current_execution_context_and_scope
{
    [Test]
    public void UtcNow_can_be_set()
    {
        using (Clock.SetTimeForCurrentContext(() => Svo.DateTime))
        {
            Clock.UtcNow().Should().Be(Svo.DateTime);
        }
        Clock.UtcNow().Should().NotBe(Svo.DateTime);
    }

    [Test]
    public void TimeZone_can_be_set()
    {
        using (Clock.SetTimeZoneForCurrentContext(TestTimeZones.LeidenTime))
        {
            Clock.TimeZone.Should().Be(TestTimeZones.LeidenTime);
        }
    }

    [Test]
    public void UtcNow_and_TimeZone_can_be_set_together()
    {
        using (Clock.SetTimeAndTimeZoneForCurrentContext(() => Svo.DateTime, Svo.TimeZone))
        {
            Clock.UtcNow().Should().Be(Svo.DateTime);
            Clock.TimeZone.Should().Be(Svo.TimeZone);
        }
        Clock.UtcNow().Should().NotBe(Svo.DateTime);
        Clock.TimeZone.Should().NotBe(Svo.TimeZone);
    }

    [Test]
    public async Task can_run_in_parallel_without_interference()
    {
        var tasks = Enumerable.Range(1900, 2000).Select(Test).ToArray();
        var executionContexts = await Task.WhenAll(tasks);
        executionContexts.Should().OnlyHaveUniqueItems();

        async Task<int> Test(int year)
        {
            using (Clock.SetTimeForCurrentContext(() => new DateTime(year, 06, 11, 16, 15, 00, 000, DateTimeKind.Local)))
            {
                await Task.Delay(10);
                Clock.UtcNow().Should().Be(new DateTime(year, 06, 11, 16, 15, 00, 000, DateTimeKind.Local));
                return ExecutionContext.Capture()?.GetHashCode() ?? 0;
            }
        }
    }
}

public class Today
{
    [Test]
    public void according_to_the_specified_time_zone()
    {
        using (Clock.SetTimeAndTimeZoneForCurrentContext(() => Svo.DateTime, Svo.TimeZone))
        {
            Clock.Today(TestTimeZones.AlaskanStandardTime).Should().Be(new Date(2017, 06, 10));
        }
    }

    [Test]
    public void according_to_the_current_time_zone_if_not_specified()
    {
        using (Clock.SetTimeAndTimeZoneForCurrentContext(() => Svo.DateTime, Svo.TimeZone))
        {
            Clock.Today().Should().Be(new Date(2017, 06, 11));
        }
    }
}

public class Yesterday
{
    [Test]
    public void according_to_the_specified_time_zone()
    {
        using (Clock.SetTimeAndTimeZoneForCurrentContext(() => Svo.DateTime, Svo.TimeZone))
        {
            Clock.Yesterday(TestTimeZones.AlaskanStandardTime).Should().Be(new Date(2017, 06, 09));
        }
    }

    [Test]
    public void according_to_the_current_time_zone_if_not_specified()
    {
        using (Clock.SetTimeAndTimeZoneForCurrentContext(() => Svo.DateTime, Svo.TimeZone))
        {
            Clock.Yesterday().Should().Be(new Date(2017, 06, 10));
        }
    }
}

public class Tomorrow
{
    [Test]
    public void according_to_the_specified_time_zone()
    {
        using (Clock.SetTimeAndTimeZoneForCurrentContext(() => Svo.DateTime, Svo.TimeZone))
        {
            Clock.Tomorrow(TestTimeZones.AlaskanStandardTime).Should().Be(new Date(2017, 06, 11));
        }
    }

    [Test]
    public void according_to_the_current_time_zone_if_not_specified()
    {
        using (Clock.SetTimeAndTimeZoneForCurrentContext(() => Svo.DateTime, Svo.TimeZone))
        {
            Clock.Tomorrow().Should().Be(new Date(2017, 06, 12));
        }
    }
}
