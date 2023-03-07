﻿using System.Threading;

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
///     using(Clock.SetTimeForCurrentContext(() => new DateTime(2017, 06, 11))
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
        var utcNow = (localContextUtcNow.Value ?? globalUtcNow).Invoke();
        return utcNow.Kind == DateTimeKind.Utc
            ? utcNow
            : new DateTime(utcNow.Ticks, DateTimeKind.Utc);
    }

    /// <summary>Gets the time zone of the <see cref="Clock"/>.</summary>
    public static TimeZoneInfo TimeZone => localContextTimeZone.Value ?? globalTimeZone;

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
    /// For test purposes use <see cref="SetLocalContextUtcNow(Func{DateTime})"/>.
    /// </remarks>
    public static void SetTime(Func<DateTime> time) => globalUtcNow = Guard.NotNull(time, nameof(time));

    /// <summary>Sets the <see cref="TimeZoneInfo"/> function globally (for the full Application Domain).</summary>
    /// <remarks>
    /// For test purposes use <see cref="SetTimeZoneForCurrentContext(TimeZoneInfo)"/>.
    /// </remarks>
    public static void SetTimeZone(TimeZoneInfo timeZone) => globalTimeZone = Guard.NotNull(timeZone, nameof(timeZone));

    /// <summary>Sets the <see cref="DateTime"/> function for current (execution) context only.</summary>
    public static IDisposable SetTimeForCurrentContext(Func<DateTime> time) => new TimeScope(time);

    /// <summary>Sets the <see cref="TimeZoneInfo"/> for current (execution) context only.</summary>
    public static IDisposable SetTimeZoneForCurrentContext(TimeZoneInfo timeZone) => new TimeZoneScope(timeZone);

    /// <summary>Sets the <see cref="DateTime"/> function and <see cref="TimeZoneInfo"/> for current (execution) context only.</summary>
    public static IDisposable SetTimeAndTimeZoneForCurrentContext(Func<DateTime> time, TimeZoneInfo timeZone) => new ClockScope(time, timeZone);

    /// <summary>Sets the <see cref="DateTime"/> function for current thread only.</summary>
    [Obsolete("Use SetTimeForCurrentContext(time) instead.")]
    public static IDisposable SetTimeForCurrentThread(Func<DateTime> time) => SetTimeForCurrentContext(time);

    /// <summary>Sets the <see cref="TimeZoneInfo"/> for current thread only.</summary>
    [Obsolete("Use SetTimeZoneForCurrentContext(timeZone) instead.")]
    public static IDisposable SetTimeZoneForCurrentThread(TimeZoneInfo timeZone) => SetTimeZoneForCurrentContext(timeZone);

    /// <summary>Sets the <see cref="DateTime"/> function and <see cref="TimeZoneInfo"/> for current thread only.</summary>
    [Obsolete("Use SetTimeAndTimeZoneForCurrentContext(time, timeZone) instead.")]
    public static IDisposable SetTimeAndTimeZoneForCurrentThread(Func<DateTime> time, TimeZoneInfo timeZone) => SetTimeAndTimeZoneForCurrentContext(time, timeZone);

    private static void SetLocalContextUtcNow(Func<DateTime>? time) => localContextUtcNow.Value = time;

    private static void SetLocalContextTimeZone(TimeZoneInfo? timeZone) => localContextTimeZone.Value = timeZone;

#pragma warning disable S6354 // Use a testable (date) time provider instead
    // This is the testable time provider.
    private static Func<DateTime> globalUtcNow = () => DateTime.UtcNow;
#pragma warning restore S6354 // Use a testable (date) time provider instead
    private static TimeZoneInfo globalTimeZone = TimeZoneInfo.Local;

    private static readonly AsyncLocal<Func<DateTime>?> localContextUtcNow = new();

    private static readonly AsyncLocal<TimeZoneInfo?> localContextTimeZone = new();

    /// <summary>Class to scope a time function.</summary>
    private sealed class TimeScope : IDisposable
    {
        public TimeScope(Func<DateTime> time)
        {
            _func = localContextUtcNow.Value;
            SetLocalContextUtcNow(Guard.NotNull(time, nameof(time)));
        }

        private readonly Func<DateTime>? _func;

        public void Dispose() => SetLocalContextUtcNow(_func);
    }

    /// <summary>Class to scope a time zone.</summary>
    private sealed class TimeZoneScope : IDisposable
    {
        public TimeZoneScope(TimeZoneInfo timeZone)
        {
            _zone = localContextTimeZone.Value;
            SetLocalContextTimeZone(Guard.NotNull(timeZone, nameof(timeZone)));
        }

        private readonly TimeZoneInfo? _zone;

        public void Dispose() => SetLocalContextTimeZone(_zone);
    }

    /// <summary>Class to scope time and a time zone.</summary>
    private sealed class ClockScope : IDisposable
    {
        public ClockScope(Func<DateTime> time, TimeZoneInfo timeZone)
        {
            _func = localContextUtcNow.Value;
            _zone = localContextTimeZone.Value;
            SetLocalContextUtcNow(Guard.NotNull(time, nameof(time)));
            SetLocalContextTimeZone(Guard.NotNull(timeZone, nameof(timeZone)));
        }

        private readonly Func<DateTime>? _func;

        private readonly TimeZoneInfo? _zone;

        public void Dispose()
        {
            SetLocalContextUtcNow(_func);
            SetLocalContextTimeZone(_zone);
        }
    }
}
