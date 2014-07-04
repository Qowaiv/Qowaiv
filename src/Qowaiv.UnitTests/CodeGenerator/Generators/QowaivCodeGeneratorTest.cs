using System;
using NUnit.Framework;
using Qowaiv.CodeGenerator.Generators;
using System.IO;
using System.Linq;
using Qowaiv.CodeGenerator.Xml;
using ExcelLibrary.SpreadSheet;
using System.Collections.Generic;

namespace Qowaiv.UnitTests.CodeGenerator.Generators
{
    [TestFixture]
    public class QowaivCodeGeneratorTest
    {
        [Test]
        public void Generate_Dir_Successful()
        {
            var gen = new QowaivCodeGenerator();
            gen.Generate(new DirectoryInfo(@"c:\temp\Qowaiv"));
        }
    }
}
