using NUnit.Framework;
using System;

namespace Qowaiv.UnitTests
{
    public class ClockTest
    {
        private static readonly DateTime TestDateTime = new DateTime(2017, 06, 11, 06, 15, 00);
        private static readonly Func<DateTime> TestTimeFunction = () => TestDateTime;
        private static readonly TimeZoneInfo TestTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. Australia Standard Time");

        [Test]
        public void UtcNow_Default_EqualsSystemDateTimeNowUtc()
        {
            var act = Clock.UtcNow();
            var exp = DateTime.UtcNow;

            Assert.That(act, Is.EqualTo(exp).Within(TimeSpan.FromTicks(100)));
        }
        [Test]
        public void TimeZone_Default_TimeZoneInfoLocal()
        {
            var act = Clock.TimeZone;
            var exp = TimeZoneInfo.Local;

            Assert.AreEqual(exp, act);
        }

        /// <remarks>
        /// Just to hit the code path. We can't guarantee that stuff is cleaned up in time.
        /// </remarks>
        [Test]
        public void UtcNow_Globally_EqualsTestDateTime()
        {
            Clock.SetTime(() => DateTime.UtcNow);

            var act = Clock.UtcNow();
            var exp = Clock.UtcNow();

            Assert.That(act, Is.EqualTo(exp).Within(TimeSpan.FromTicks(100)));
        }
        /// <remarks>
        /// Just to hit the code path. We can't guarantee that stuff is cleaned up in time.
        /// </remarks>
        [Test]
        public void TimeZone_Globally_TimeZoneInfoLocal()
        {
            Clock.SetTimeZone(TimeZoneInfo.Local);

            var act = Clock.TimeZone;
            var exp = TimeZoneInfo.Local;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void UtcNow_CurrentThread_EqualsTestDateTime()
        {
            using (Clock.SetTimeForCurrentThread(TestTimeFunction))
            {
                var act = Clock.UtcNow();
                var exp = TestDateTime;
                Assert.AreEqual(exp, act);
            }
        }
        [Test]
        public void TimeZone_CurrentThread_EqualsTestDateTime()
        {
            using (Clock.SetTimeZoneForCurrentThread(TestTimeZone))
            {
                var act = Clock.TimeZone;
                var exp = TestTimeZone;
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void NowWithOffset_WestEuropeanWithoutDaylightSaving_Plus1()
        {
            var name = nameof(NowWithOffset_WestEuropeanWithoutDaylightSaving_Plus1);
            var timezone = TimeZoneInfo.CreateCustomTimeZone(name, TimeSpan.FromHours(+1), name, name);

            using (Clock.SetTimeAndTimeZoneForCurrentThread(() => new DateTime(2019, 02, 14), timezone))
            {
                var act = Clock.NowWithOffset();
                var exp = new DateTimeOffset(new LocalDateTime(2019, 02, 14, 1, 0, 0), TimeSpan.FromHours(+1));
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void Now_TestTimeZone_10HoursLaterThanUtc()
        {
            using (Clock.SetTimeAndTimeZoneForCurrentThread(TestTimeFunction, TestTimeZone))
            {
                var act = Clock.Now();
                var exp = new LocalDateTime(2017, 06, 11, 16, 15, 00);

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void Today_Australia_Y2017M06_D11()
        {
            using (Clock.SetTimeForCurrentThread(TestTimeFunction))
            {
                using (Clock.SetTimeZoneForCurrentThread(TestTimeZone))
                {
                    var act = Clock.Today();
                    var exp = new Date(2017, 06, 11);

                    Assert.AreEqual(exp, act);
                }
            }
        }

        [Test]
        public void Today_Alaska_Y2017M06_D10()
        {
            using (Clock.SetTimeForCurrentThread(TestTimeFunction))
            {
                var act = Clock.Today(TimeZoneInfo.FindSystemTimeZoneById("Alaskan Standard Time"));
                var exp = new Date(2017, 06, 10);

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void Yesterday_Y2017M06_D10()
        {
            using (Clock.SetTimeAndTimeZoneForCurrentThread(TestTimeFunction, TestTimeZone))
            {
                var act = Clock.Yesterday();
                var exp = new Date(2017, 06, 10);

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void Tomorrow_Y2017M06_D12()
        {
            using (Clock.SetTimeAndTimeZoneForCurrentThread(TestTimeFunction, TestTimeZone))
            {
                var act = Clock.Tomorrow();
                var exp = new Date(2017, 06, 12);

                Assert.AreEqual(exp, act);
            }
        }
    }
}
