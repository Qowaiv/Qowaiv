using NUnit.Framework;
using Qowaiv.TestTools;

namespace Qowaiv.Sql.UnitTests
{
    public class AssemblyTest
    {
        [Test]
        public void ContainsSvos_None()
        {
            AssemblyAssert.ContainsSvos<Timestamp>(typeof(Timestamp));
        }
    }
}
