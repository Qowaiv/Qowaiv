namespace Obsolete_code;

[Obsolete("Will be dropped when the next major version is released.")]
public class Will_be_dropped
{
    [Test]
    public void Clock_Yesterday_TimeZone()
    {
        using (Clock.SetTimeAndTimeZoneForCurrentContext(() => Svo.DateTime, Svo.TimeZone))
        {
            Clock.Yesterday(TestTimeZones.AlaskanStandardTime).Should().Be(new Date(2017, 06, 09));
        }
    }

    [Test]
    public void Clock_Yesterday()
    {
        using (Clock.SetTimeAndTimeZoneForCurrentContext(() => Svo.DateTime, Svo.TimeZone))
        {
            Clock.Yesterday().Should().Be(new Date(2017, 06, 10));
        }
    }

    [Test]
    public void Clock_Tomorrow_TimeZone()
    {
        using (Clock.SetTimeAndTimeZoneForCurrentContext(() => Svo.DateTime, Svo.TimeZone))
        {
            Clock.Tomorrow(TestTimeZones.AlaskanStandardTime).Should().Be(new Date(2017, 06, 11));
        }
    }

    [Test]
    public void Clock_Tomorrow()
    {
        using (Clock.SetTimeAndTimeZoneForCurrentContext(() => Svo.DateTime, Svo.TimeZone))
        {
            Clock.Tomorrow().Should().Be(new Date(2017, 06, 12));
        }
    }
}

[Obsolete("Will become private when the next major version is released.")]
public class Will_become_private { }

public class Will_seal
{
    [Test]
    public void _0_types()
    {
        var decorated = typeof(Qowaiv.Date).Assembly.GetTypes().Concat(
        typeof(Qowaiv.Data.SvoParameter).Assembly.GetTypes())
        .Where(tp => tp.GetCustomAttributes<WillBeSealedAttribute>().Any())
        .OrderBy(tp => tp.FullName);

        decorated.Should().BeEmpty();
    }
}
