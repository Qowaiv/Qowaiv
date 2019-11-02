using NUnit.Framework;
using System.Diagnostics;

namespace Qowaiv.TestTools.UnitTests
{
    public class DebuggerDisplayAssertTest
    {
        [Test]
        public void HasDebuggerDisplay_ViaParent_Successful()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(WithDubuggerDisplayChild));
        }

        [Test]
        public void HasDebuggerDisplay_Directly_Successful()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(WithDebuggerDisplay));
        }

        [Test]
        public void HasResult_ViaParent_StringValue()
        {
            DebuggerDisplayAssert.HasResult("WithDubuggerDisplayChild", new WithDubuggerDisplayChild());
        }

        [Test]
        public void HasResulty_Directly_StringValue()
        {
            DebuggerDisplayAssert.HasResult("WithDebuggerDisplay", new WithDebuggerDisplay());
        }
    }

    internal class WithDubuggerDisplayChild : WithDebuggerDisplay { }

    [DebuggerDisplay("{DebuggerDisplay}")]
    internal class WithDebuggerDisplay
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => GetType().Name;
    }

}
