using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qowaiv.CodeGenerator.Generators;
using System.IO;
using System.Linq;
using Qowaiv.CodeGenerator.Xml;
using ExcelLibrary.SpreadSheet;
using System.Collections.Generic;

namespace Qowaiv.UnitTests.CodeGenerator.Generators
{
    [TestClass]
    public class QowaivCodeGeneratorTest
    {
        [TestMethod]
        public void Generate_Dir_Successful()
        {
            var gen = new QowaivCodeGenerator();
            gen.Generate(new DirectoryInfo(@"c:\temp\Qowaiv"));
        }
    }
}
