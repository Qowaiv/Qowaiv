using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qowaiv.CodeGenerator.Xml;
using Qowaiv.UnitTests.TestTools;

namespace Qowaiv.UnitTests.SvoGenerator.Xml
{
    [TestClass]
    public class XResourceFileDataTest
    {
        [TestMethod]
        public void Ctor_NullName_ThrowsArgumentNullException()
        {
            ExceptionAssert.ExpectArgumentNullException(() =>
            {
                new XResourceFileData(null, null);
            },
            "name");
        }

        [TestMethod]
        public void DebugToString_NoComment_HasResult()
        {
            var val = new XResourceFileData("KEY0", "VALUE0");

            DebuggerDisplayAssert.HasResult("Data, Name: KEY0, Value: 'VALUE0'", val);
        }

        [TestMethod]
        public void DebugToString_WithComment_HasResult()
        {
            var val = new XResourceFileData("KEY1", "VALUE1", "With comment.");

            DebuggerDisplayAssert.HasResult("Data, Name: KEY1, Value: 'VALUE1', Comment: 'With comment.'", val);
        }
    }
}
