using System.Threading;

namespace Qowaiv;

/// <summary>Qowaiv Clock is lightweight solution for having changeable behaviour
/// on getting the current time of today, and derived logic.
/// </summary>
/// <example>
/// A typical call looks like:
/// <code>
/// DateTime now = Clock.UtcNow();
/// </code>
/// 
/// Controlling the behaviour in a test could look like:
/// 
/// <code>
/// [Test]
/// public void TestSomething()
/// {
///     using(Clock.SetTimeForCurrentThread(() => new DateTime(2017, 06, 11))
///     {
///         // test code.
///     }
/// }
/// </code>
/// </example>
public static class Clock
{
    /// <summary>Gets the UTC (Coordinated Universal Time) zone of the clock.</summary>
    /// <remarks>
    /// To be able to stub the clock, this simple class can be used. 
    /// 
    /// The core if this clock it UTC, see: https://en.wikipedia.org/wiki/Coordinated_Universal_Time)
    /// 
    /// To prevent unexpected behaviour, the result is always converted to
    /// <see cref="DateTimeKind.Utc"/> if needed.
    /// </remarks>
    [Pure]
    public static DateTime UtcNow()
    {
        var utcNow = (threadUtcNow.Value ?? globalUtcNow).Invoke();
        return utcNow.Kind == DateTimeKind.Utc
            ? utcNow
            : new DateTime(utcNow.Ticks, DateTimeKind.Utc);
    }

    /// <summary>Gets the time zone of the <see cref="Clock"/>.</summary>
    public static TimeZoneInfo TimeZone => threadTimeZone.Value ?? globalTimeZone;

    /// <summary>Gets the current <see cref="LocalDateTime"/>.</summary>
    [Pure]
    public static LocalDateTime Now() => Now(TimeZone);

    /// <summary>Gets the current <see cref="LocalDateTime"/> for the specified time zone.</summary>
    /// <param name="timeZone">
    /// The specified time zone.
    /// </param>
    [Pure]
    public static LocalDateTime Now(TimeZoneInfo timeZone) => TimeZoneInfo.ConvertTimeFromUtc(UtcNow(), Guard.NotNull(timeZone, nameof(timeZone)));

    /// <summary>Gets the current <see cref="DateTimeOffset"/>.</summary>
    [Pure]
    public static DateTimeOffset NowWithOffset() => NowWithOffset(TimeZone);

    /// <summary>Gets the current <see cref="DateTimeOffset"/> for the specified time zone.</summary>
    /// <param name="timeZone">
    /// The specified time zone.
    /// </param>
    [Pure]
    public static DateTimeOffset NowWithOffset(TimeZoneInfo timeZone)
    {
        Guard.NotNull(timeZone, nameof(timeZone));
        var utcNow = UtcNow();
        var now = TimeZoneInfo.ConvertTimeFromUtc(utcNow, timeZone);
        return new DateTimeOffset(now.Ticks, now - utcNow);
    }

    /// <summary>Gets the yesterday for the local <see cref="DateTime"/>.</summary>
    [Pure]
    public static Date Yesterday() => Yesterday(TimeZone);

    /// <summary>Gets the yesterday for the specified time zone.</summary>
    /// <param name="timeZone">
    /// The specified time zone.
    /// </param>
    [Pure]
    public static Date Yesterday(TimeZoneInfo timeZone) => Today(timeZone).AddDays(-1);

    /// <summary>Gets the today for the local <see cref="DateTime"/>.</summary>
    [Pure]
    public static Date Today() => Today(TimeZone);

    /// <summary>Gets the today for the specified time zone.</summary>
    /// <param name="timeZone">
    /// The specified time zone.
    /// </param>
    [Pure]
    public static Date Today(TimeZoneInfo timeZone)
    {
        Guard.NotNull(timeZone, nameof(timeZone));
        return (Date)TimeZoneInfo.ConvertTimeFromUtc(UtcNow(), timeZone);
    }

    /// <summary>Gets the tomorrow for the local <see cref="DateTime"/>.</summary>
    [Pure]
    public static Date Tomorrow() => Tomorrow(TimeZone);

    /// <summary>Gets the tomorrow for the specified time zone.</summary>
    /// <param name="timeZone">
    /// The specified time zone.
    /// </param>
    [Pure]
    public static Date Tomorrow(TimeZoneInfo timeZone) => Today(timeZone).AddDays(+1);

    /// <summary>Sets the <see cref="DateTime"/> function globally (for the full Application Domain).</summary>
    /// <remarks>
    /// For test purposes use <see cref="SetThreadUtcNow(Func{DateTime})"/>.
    /// </remarks>
    public static void SetTime(Func<DateTime> time) => globalUtcNow = Guard.NotNull(time, nameof(time));

    /// <summary>Sets the <see cref="TimeZoneInfo"/> function globally (for the full Application Domain).</summary>
    /// <remarks>
    /// For test purposes use <see cref="SetTimeZoneForCurrentThread(TimeZoneInfo)"/>.
    /// </remarks>
    public static void SetTimeZone(TimeZoneInfo timeZone) => globalTimeZone = Guard.NotNull(timeZone, nameof(timeZone));

    /// <summary>Sets the <see cref="DateTime"/> function for current thread only.</summary>
    public static IDisposable SetTimeForCurrentThread(Func<DateTime> time) => new TimeScope(time);

    /// <summary>Sets the <see cref="TimeZoneInfo"/> for current thread only.</summary>
    public static IDisposable SetTimeZoneForCurrentThread(TimeZoneInfo timeZone) => new TimeZoneScope(timeZone);

    /// <summary>Sets the <see cref="DateTime"/> function and <see cref="TimeZoneInfo"/> for current thread only.</summary>
    public static IDisposable SetTimeAndTimeZoneForCurrentThread(Func<DateTime> time, TimeZoneInfo timeZone) => new ClockScope(time, timeZone);

    #region private members

    private static void SetThreadUtcNow(Func<DateTime>? time) => threadUtcNow.Value = time;
    private static void SetThreadTimeZone(TimeZoneInfo? timeZone) => threadTimeZone.Value = timeZone;

#pragma warning disable QW0001 // Use a testable Time Provider
    // This is the testable time provider.
    private static Func<DateTime> globalUtcNow = () => DateTime.UtcNow;
#pragma warning restore QW0001 // Use a testable Time Provider
    private static TimeZoneInfo globalTimeZone = TimeZoneInfo.Local;

    static AsyncLocal<Func<DateTime>?> threadUtcNow = new();
    static AsyncLocal<TimeZoneInfo?> threadTimeZone = new();

    /// <summary>Class to scope a time function.</summary>
    private sealed class TimeScope : IDisposable
    {
        public TimeScope(Func<DateTime> time)
        {
            _func = threadUtcNow.Value;
            SetThreadUtcNow(Guard.NotNull(time, nameof(time)));
        }
        private readonly Func<DateTime>? _func;
        public void Dispose() => SetThreadUtcNow(_func);
    }

    /// <summary>Class to scope a time zone.</summary>
    private sealed class TimeZoneScope : IDisposable
    {
        public TimeZoneScope(TimeZoneInfo timeZone)
        {
            _zone = threadTimeZone.Value;
            SetThreadTimeZone(Guard.NotNull(timeZone, nameof(timeZone)));
        }
        private readonly TimeZoneInfo? _zone;

        public void Dispose() => SetThreadTimeZone(_zone);
    }

    /// <summary>Class to scope time and a time zone.</summary>
    private sealed class ClockScope : IDisposable
    {
        public ClockScope(Func<DateTime> time, TimeZoneInfo timeZone)
        {
            _func = threadUtcNow.Value;
            _zone = threadTimeZone.Value;
            SetThreadUtcNow(Guard.NotNull(time, nameof(time)));
            SetThreadTimeZone(Guard.NotNull(timeZone, nameof(timeZone)));
        }
        private readonly Func<DateTime>? _func;
        private readonly TimeZoneInfo? _zone;

        public void Dispose()
        {
            SetThreadUtcNow(_func);
            SetThreadTimeZone(_zone);
        }
    }
    #endregion
}
