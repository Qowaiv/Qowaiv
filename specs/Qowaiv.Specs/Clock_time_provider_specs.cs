#if NET8_0_OR_GREATER

using FluentAssertions.Extensions;

namespace Clock_specs;

public class Time_provider
{
    [Test]
    public void reflects_time_of_Qowaiv_Clock()
    {
        var provider = Clock.TimeProvider;

        using (Clock.SetTimeForCurrentContext(() => 11.June(2017).At(06, 15).AsUtc()))
        {
            provider.GetUtcNow().Should().Be(new DateTimeOffset(2017, 06, 11, 06, 15, 00, TimeSpan.Zero));
        }
    }

    [Test]
    public void reflects_time_zone_of_Qowaiv_Clock()
    {
        var provider = Clock.TimeProvider;

        using (Clock.SetTimeZoneForCurrentContext(TestTimeZones.LeidenTime))
        {
            provider.LocalTimeZone.Should().Be(TestTimeZones.LeidenTime);
        }
    }

    [Test]
    public void reflects_timestamp_of_Qowaiv_Clock()
    {
        var provider = Clock.TimeProvider;

        using (Clock.SetTimeForCurrentContext(() => 11.June(2017).At(06, 15).AsUtc()))
        {
            provider.GetTimestamp().Should().Be(11.June(2017).At(06, 15).AsUtc().Ticks);
        }
    }
}

#endif
