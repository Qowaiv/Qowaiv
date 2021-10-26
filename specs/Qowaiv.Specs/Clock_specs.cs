using NUnit.Framework;
using Qowaiv;
using Qowaiv.Specs;
using Qowaiv.TestTools.Globalization;
using System;

namespace Clock_specs
{
    public class Default_behaviour
    {
        [Test]
        public void UtcNow_equals_DateTime_UtcNow()
            => Assert.That(Clock.UtcNow(), Is.EqualTo(DateTime.UtcNow).Within(TimeSpan.FromTicks(100)));

        [Test]
        public void TimeZone_equals_TimeZoneInfo_Local()
            => Assert.That(Clock.TimeZone, Is.EqualTo(TimeZoneInfo.Local));
        
        [Test]
        public void Now_equals_UTC_now_with_the_time_zone_offset()
        {
            using (Clock.SetTimeAndTimeZoneForCurrentThread(() => Svo.DateTime, Svo.TimeZone))
            {
                Assert.That(Clock.Now(), Is.EqualTo(new LocalDateTime(2017, 06, 11, 16, 15, 00)));
            }
        }

        [Test]
        public void NowWithOffset_equals_UTC_now_with_the_time_zone_offset()
        {
            using (Clock.SetTimeAndTimeZoneForCurrentThread(() => Svo.DateTime, Svo.TimeZone))
            {
                var date_time_offset = new DateTimeOffset(new DateTime(2017, 06, 11, 16, 15, 0, DateTimeKind.Unspecified), TimeSpan.FromHours(+10));
                Assert.That(Clock.NowWithOffset(), Is.EqualTo(date_time_offset));
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
            Assert.That(Clock.UtcNow(), Is.EqualTo(Svo.DateTime));
        }

        [Test]
        public void TimeZone_can_be_set()
        {
            Clock.SetTimeZone(Svo.TimeZone);
            Assert.That(Clock.TimeZone, Is.EqualTo(Svo.TimeZone));
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
            using (Clock.SetTimeForCurrentThread(() => new DateTime(2017, 06, 11, 06, 15, 0, kind)))
            {
                Assert.That(Clock.UtcNow().Kind, Is.EqualTo(DateTimeKind.Utc));
            }
        }
    }

    public class For_current_thread_and_scope
    {
        [Test]
        public void UtcNow_can_be_set()
        {
            using (Clock.SetTimeForCurrentThread(() => Svo.DateTime))
            {
                Assert.That(Clock.UtcNow(), Is.EqualTo(Svo.DateTime));
            }
            Assert.That(Clock.UtcNow(), Is.Not.EqualTo(Svo.DateTime));
        }

        [Test]
        public void TimeZone_can_be_set()
        {
            using (Clock.SetTimeZoneForCurrentThread(Svo.TimeZone))
            {
                Assert.That(Clock.TimeZone, Is.EqualTo(Svo.TimeZone));
            }
            Assert.Inconclusive("We can not guarantee that the zone we set for the scope is different from the zone we run the test.");
        }

        [Test]
        public void UtcNow_and_TimeZone_can_be_set_together()
        {
            using (Clock.SetTimeAndTimeZoneForCurrentThread(() => Svo.DateTime, Svo.TimeZone))
            {
                Assert.That(Clock.UtcNow(), Is.EqualTo(Svo.DateTime));
                Assert.That(Clock.TimeZone, Is.EqualTo(Svo.TimeZone));
            }
            Assert.That(Clock.UtcNow(), Is.Not.EqualTo(Svo.DateTime));
            Assert.Inconclusive("We can not guarantee that the zone we set for the scope is different from the zone we run the test.");
        }
    }

    public class Today
    {
        [Test]
        public void according_to_the_specified_time_zone()
        {
            using (Clock.SetTimeAndTimeZoneForCurrentThread(() => Svo.DateTime, Svo.TimeZone))
            {
                Assert.That(Clock.Today(TestTimeZones.AlaskanStandardTime), Is.EqualTo(new Date(2017, 06, 10)));
            }
        }

        [Test]
        public void according_to_the_current_time_zone_if_not_specified()
        {
            using (Clock.SetTimeAndTimeZoneForCurrentThread(() => Svo.DateTime, Svo.TimeZone))
            {
                Assert.That(Clock.Today(), Is.EqualTo(new Date(2017, 06, 11)));
            }
        }
    }

    public class Yesterday
    {
        [Test]
        public void according_to_the_specified_time_zone()
        {
            using (Clock.SetTimeAndTimeZoneForCurrentThread(() => Svo.DateTime, Svo.TimeZone))
            {
                Assert.That(Clock.Yesterday(TestTimeZones.AlaskanStandardTime), Is.EqualTo(new Date(2017, 06, 09)));
            }
        }

        [Test]
        public void according_to_the_current_time_zone_if_not_specified()
        {
            using (Clock.SetTimeAndTimeZoneForCurrentThread(() => Svo.DateTime, Svo.TimeZone))
            {
                Assert.That(Clock.Yesterday(), Is.EqualTo(new Date(2017, 06, 10)));
            }
        }
    }

    public class Tomorrow
    {
        [Test]
        public void according_to_the_specified_time_zone()
        {
            using (Clock.SetTimeAndTimeZoneForCurrentThread(() => Svo.DateTime, Svo.TimeZone))
            {
                Assert.That(Clock.Tomorrow(TestTimeZones.AlaskanStandardTime), Is.EqualTo(new Date(2017, 06, 11)));
            }
        }

        [Test]
        public void according_to_the_current_time_zone_if_not_specified()
        {
            using (Clock.SetTimeAndTimeZoneForCurrentThread(() => Svo.DateTime, Svo.TimeZone))
            {
                Assert.That(Clock.Tomorrow(), Is.EqualTo(new Date(2017, 06, 12)));
            }
        }
    }
}
