using System;

namespace Qowaiv
{
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
        public static DateTime UtcNow()
        {
            var utcNow = (threadUtcNow ?? globalUtcNow).Invoke();
            return utcNow.Kind == DateTimeKind.Utc
                ? utcNow
                : new DateTime(utcNow.Ticks, DateTimeKind.Utc);
        }

        /// <summary>Gets the time zone of the <see cref="Clock"/>.</summary>
        public static TimeZoneInfo TimeZone => threadTimeZone ?? globalTimeZone;

        /// <summary>Gets the current <see cref="LocalDateTime"/>.</summary>
        public static LocalDateTime Now() => Now(TimeZone);

        /// <summary>Gets the current <see cref="LocalDateTime"/> for the specified time zone.</summary>
        /// <param name="timeZone">
        /// The specified time zone.
        /// </param>
        public static LocalDateTime Now(TimeZoneInfo timeZone) => TimeZoneInfo.ConvertTimeFromUtc(UtcNow(), Guard.NotNull(timeZone, nameof(timeZone)));

        /// <summary>Gets the current <see cref="DateTimeOffset"/>.</summary>
        public static DateTimeOffset NowWithOffset() => NowWithOffset(TimeZone);

        /// <summary>Gets the current <see cref="DateTimeOffset"/> for the specified time zone.</summary>
        /// <param name="timeZone">
        /// The specified time zone.
        /// </param>
        public static DateTimeOffset NowWithOffset(TimeZoneInfo timeZone)
        {
            Guard.NotNull(timeZone, nameof(timeZone));
            var utcNow = UtcNow();
            var now = TimeZoneInfo.ConvertTimeFromUtc(utcNow, timeZone);
            return new DateTimeOffset(now.Ticks, now - utcNow);
        }

        /// <summary>Gets the yesterday for the local <see cref="DateTime"/>.</summary>
        public static Date Yesterday() => Yesterday(TimeZone);

        /// <summary>Gets the yesterday for the specified time zone.</summary>
        /// <param name="timeZone">
        /// The specified time zone.
        /// </param>
        public static Date Yesterday(TimeZoneInfo timeZone) => Today(timeZone).AddDays(-1);

        /// <summary>Gets the today for the local <see cref="DateTime"/>.</summary>
        public static Date Today() => Today(TimeZone);

        /// <summary>Gets the today for the specified time zone.</summary>
        /// <param name="timeZone">
        /// The specified time zone.
        /// </param>
        public static Date Today(TimeZoneInfo timeZone)
        {
            Guard.NotNull(timeZone, nameof(timeZone));
            return (Date)TimeZoneInfo.ConvertTimeFromUtc(UtcNow(), timeZone);
        }

        /// <summary>Gets the tomorrow for the local <see cref="DateTime"/>.</summary>
        public static Date Tomorrow() => Tomorrow(TimeZone);
        /// <summary>Gets the tomorrow for the specified time zone.</summary>
        /// <param name="timeZone">
        /// The specified time zone.
        /// </param>
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

        private static void SetThreadUtcNow(Func<DateTime> time) => threadUtcNow = time;
        private static void SetThreadTimeZone(TimeZoneInfo timeZone) => threadTimeZone = timeZone;

        private static Func<DateTime> globalUtcNow = () => DateTime.UtcNow;
        private static TimeZoneInfo globalTimeZone = TimeZoneInfo.Local;

        [ThreadStatic]
        private static Func<DateTime> threadUtcNow;
        [ThreadStatic]
        private static TimeZoneInfo threadTimeZone;

        /// <summary>Class to scope a time function.</summary>
        private sealed class TimeScope : IDisposable
        {
            public TimeScope(Func<DateTime> time)
            {
                Guard.NotNull(time, nameof(time));
                _func = threadUtcNow;
                SetThreadUtcNow(time);
            }
            private readonly Func<DateTime> _func;
            public void Dispose() => SetThreadUtcNow(_func);
        }

        /// <summary>Class to scope a time zone.</summary>
        private sealed class TimeZoneScope : IDisposable
        {
            public TimeZoneScope(TimeZoneInfo timeZone)
            {
                Guard.NotNull(timeZone, nameof(timeZone));
                _zone = threadTimeZone;
                SetThreadTimeZone(timeZone);
            }
            private readonly TimeZoneInfo _zone;

            public void Dispose() => SetThreadTimeZone(_zone);
        }

        /// <summary>Class to scope time and a time zone.</summary>
        private sealed class ClockScope : IDisposable
        {
            public ClockScope(Func<DateTime> time, TimeZoneInfo timeZone)
            {
                Guard.NotNull(time, nameof(time));
                Guard.NotNull(timeZone, nameof(timeZone));
                _func = threadUtcNow;
                _zone = threadTimeZone;
                SetThreadUtcNow(time);
                SetThreadTimeZone(timeZone);
            }
            private readonly Func<DateTime> _func;
            private readonly TimeZoneInfo _zone;

            public void Dispose()
            {
                SetThreadUtcNow(_func);
                SetThreadTimeZone(_zone);
            }
        }
        #endregion
    }
}
