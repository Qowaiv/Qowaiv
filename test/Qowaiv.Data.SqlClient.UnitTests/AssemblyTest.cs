using NUnit.Framework;
using Qowaiv.TestTools;
using System;

namespace Qowaiv.Sql.UnitTests
{
    [TestFixture]
    public class AssemblyTest : AssemblyTestBase
    {
        protected override Type AssemblyType => typeof(Timestamp);

        protected override Type[] ExpectedSvos => new Type[] 
        {
            typeof(Timestamp),
        };
    }
}
