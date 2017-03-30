using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Qowaiv.CodeGenerator.Xml;
using Qowaiv.UnitTests.TestTools;
using System.Collections.Generic;

namespace Qowaiv.UnitTests.CodeGenerator.Xml
{
    [TestClass]
    public class XWorkbookTest
    {
        [TestMethod]
        public void Load_NullFileThrowArgumentNullException()
        {
            ExceptionAssert.ExpectArgumentNullException(() =>
            {
                XWorkbook.Load((FileInfo)null);
            },
            "file");
        }

        [TestMethod]
        public void Load_NullStream_ThrowArgumentNullException()
        {
            ExceptionAssert.ExpectArgumentNullException(() =>
            {
                XWorkbook.Load((Stream)null);
            },
            "stream");
        }

        [TestMethod]
        public void Save_NullFile_ThrowArgumentNullException()
        {
            ExceptionAssert.ExpectArgumentNullException(() =>
            {
                new XWorkbook().Save((FileInfo)null);
            },
            "file");
        }

        [TestMethod]
        public void Save_NullStream_ThrowArgumentNullException()
        {
            ExceptionAssert.ExpectArgumentNullException(() =>
            {
                new XWorkbook().Save((Stream)null);
            },
            "stream");
        }


        [TestMethod]
        public void SaveLoad_Data_AreEqual()
        {
            var file = new FileInfo("SaveLoad_Data_AreEqual.xml");

            var workbook = new XWorkbook()
            {
                ExcelWorkbook = new XExcelWorkbook()
                {
                },
                Worksheets = new List<XWorksheet>()
                {
                    new XWorksheet()
                    {
                        Name = "Tab1",
                        Table = new XTable()
                        {
                                
                        },
                    },
                },
            };
            
            workbook.Save(file);

            var act = XWorkbook.Load(file);
        }
    }
}
