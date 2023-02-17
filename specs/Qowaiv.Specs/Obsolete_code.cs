namespace Obsolete_code;

[Obsolete("Will be dropped when the next major version is released.")]
public class Will_be_dropped
{
    [Test]
    public void Date_Today() => Date.Today.Should().Be(Clock.Today());

    [Test]
    public void Date_Yesterday() => Date.Yesterday.Should().Be(Clock.Yesterday());

    [Test]
    public void Date_Tomorrow() => Date.Tomorrow.Should().Be(Clock.Tomorrow());

    [Test]
    public void Amount_Addition_with_decimal() => (50.Amount() + 49m).Should().Be(99.Amount());

    [Test]
    public void Amount_Subtraction_with_decimal() => (90.Amount() - 20m).Should().Be(70.Amount());

    [Test]
    public void Percentage_Implicit_cast_from_decimal()
    {
        Percentage casted = 0.1751m;
        casted.Should().Be(Svo.Percentage);
    }

    [Test]
    public void Clock_SetTimeForCurrentThread()
    {
        using (Clock.SetTimeForCurrentThread(() => Svo.DateTime))
        {
            Clock.UtcNow().Should().Be(Svo.DateTime);
        }
    }

    [Test]
    public void Clock_SetTimeZoneForCurrentThread()
    {
        using (Clock.SetTimeZoneForCurrentThread(Svo.TimeZone))
        {
            Clock.TimeZone.Should().Be(Svo.TimeZone);
        }
    }

    [Test]
    public void Clock_SetTimeAndTimeZoneForCurrentThread()
    {
        using (Clock.SetTimeAndTimeZoneForCurrentThread(() => Svo.DateTime, Svo.TimeZone))
        {
            Clock.UtcNow().Should().Be(Svo.DateTime);
            Clock.TimeZone.Should().Be(Svo.TimeZone);
        }
    }
}

[Obsolete("Will become private when the next major version is released.")]
public class Will_become_private { }

public class Will_seal
{
    [Test]
    public void _2_types()
    {
        var decorated = typeof(Qowaiv.Date).Assembly.GetTypes().Concat(
        typeof(Qowaiv.Data.SvoParameter).Assembly.GetTypes())
        .Where(tp => tp.GetCustomAttributes<WillBeSealedAttribute>().Any())
        .OrderBy(tp => tp.FullName);

        decorated.Should().BeEquivalentTo(new[]
        {
            typeof(Qowaiv.Conversion.Security.Cryptography.CryptographicSeedTypeConverter),
            typeof(Qowaiv.Conversion.Security.SecretTypeConverter),
        });
    }
}
