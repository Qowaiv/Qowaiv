using NUnit.Framework;
using Qowaiv.CodeGenerator.Xml;
using Qowaiv.UnitTests.TestTools;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests.SvoGenerator.Xml
{
    [TestFixture]
    public class XResourceFileHeaderTest
    {
        [Test]
        public void Ctor_NullName_ThrowsArgumentNullException()
        {
            ExceptionAssert.ExpectArgumentNullException(() =>
            {
                new XResourceFileHeader(null, null);
            },
            "name");
        }

        [Test]
        public void DebugToString_Params_HasResult()
        {
            var val = new XResourceFileHeader("KEY0", "VALUE0");

            DebuggerDisplayAssert.HasResult("Header, Name: KEY0, Value: 'VALUE0'", val);
        }

        [Test]
        public void GetSchema_None_IsNull()
        {
            IXmlSerializable act = new XResourceFileHeader("KEY0", "VALUE0");
            Assert.IsNull(act.GetSchema());
        }
    }
}
