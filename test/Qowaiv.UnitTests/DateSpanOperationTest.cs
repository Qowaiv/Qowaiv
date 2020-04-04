using NUnit.Framework;
using System;

namespace Qowaiv.UnitTests
{
    public class DateSpanOperationTest
    {
        [Test]
        public void Add_DateSpan_ShouldAddDaysSecond()
        {
            var date = new DateTime(1999, 01, 30);
            var span = DateSpan.FromMonths(1);
            Assert.AreEqual(new DateTime(1999,02,28), date.Add(span));
        }

        [TestCase("1999-01-30", "1999-02-28", 1, +0, false)]
        [TestCase("2000-01-30", "2000-02-29", 1, +0, false)]
        [TestCase("1999-01-30", "1999-02-27", 1, -1, false)]
        [TestCase("1999-01-30", "1999-02-28", 1, -1, true)]
        public void Add_DateSpan(DateTime dateTime, DateTime expected,  int months, int days, bool daysFirst)
        {
            var span = new DateSpan(months, days);

            var date = (Date)dateTime;
            var local = (LocalDateTime)dateTime;

            Assert.AreEqual(expected, dateTime.Add(span, daysFirst));
            Assert.AreEqual((Date)expected, date.Add(span, daysFirst));
            Assert.AreEqual((LocalDateTime)expected, local.Add(span, daysFirst));
        }
    }
}
