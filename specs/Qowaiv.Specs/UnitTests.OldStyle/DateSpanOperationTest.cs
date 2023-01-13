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

        
    }
}
