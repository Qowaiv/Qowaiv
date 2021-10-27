using NUnit.Framework;
using System.Diagnostics;

namespace Qowaiv.TestTools.UnitTests
{
    public class DebuggerDisplayAssertTest
    {
        [Test]
        public void HasDebuggerDisplay_ViaParent_Successful()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(WithDebuggerDisplayChild));
        }

        [Test]
        public void HasDebuggerDisplay_Directly_Successful()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(WithDebuggerDisplay));
        }

        [Test]
        public void HasResult_ViaParent_StringValue()
        {
            DebuggerDisplayAssert.HasResult("WithDebuggerDisplayChild", new WithDebuggerDisplayChild());
        }

        [Test]
        public void HasResulty_Directly_StringValue()
        {
            DebuggerDisplayAssert.HasResult("WithDebuggerDisplay", new WithDebuggerDisplay());
        }
    }

    internal class WithDebuggerDisplayChild : WithDebuggerDisplay { }

    [DebuggerDisplay("{DebuggerDisplay}")]
    internal class WithDebuggerDisplay
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => GetType().Name;
    }

}
