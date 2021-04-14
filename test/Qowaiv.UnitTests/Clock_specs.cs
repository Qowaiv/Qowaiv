using NUnit.Framework;
using Qowaiv;
using Qowaiv.UnitTests;
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

    public class For_current_thread_and_scope
    {
        [Test]
        public void UtcNow_can_be_set()
        {
            using (Clock.SetTimeForCurrentThread(()=> Svo.DateTime))
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
}
