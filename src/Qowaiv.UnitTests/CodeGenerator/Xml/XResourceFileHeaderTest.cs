using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qowaiv.CodeGenerator.Xml;
using Qowaiv.UnitTests.TestTools;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests.SvoGenerator.Xml
{
    [TestClass]
    public class XResourceFileHeaderTest
    {
        [TestMethod]
        public void Ctor_NullName_ThrowsArgumentNullException()
        {
            ExceptionAssert.ExpectArgumentNullException(() =>
            {
                new XResourceFileHeader(null, null);
            },
            "name");
        }

        [TestMethod]
        public void DebugToString_Params_HasResult()
        {
            var val = new XResourceFileHeader("KEY0", "VALUE0");

            DebuggerDisplayAssert.HasResult("Header, Name: KEY0, Value: 'VALUE0'", val);
        }

        [TestMethod]
        public void GetSchema_None_IsNull()
        {
            IXmlSerializable act = new XResourceFileHeader("KEY0", "VALUE0");
            Assert.IsNull(act.GetSchema());
        }
    }
}
