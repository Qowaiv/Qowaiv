using NUnit.Framework;
using Qowaiv.Statistics;
using System.Linq;

namespace Qowaiv.UnitTests.Statistics
{
    [TestFixture]
    public class EloExtensionsTests
    {
        [Test]
        public void Avarage_SmallCollection_1500()
        {
            var elos = new Elo[] { 1400, 1600 };
            Elo act = EloExtensions.Avarage(elos);
            Elo exp = 1500;

            Assert.AreEqual(exp, act);
        }
    }
}
