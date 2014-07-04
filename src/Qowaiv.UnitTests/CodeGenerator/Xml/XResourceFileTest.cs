using System;
using NUnit.Framework;
using Qowaiv.CodeGenerator.Xml;
using System.Collections.Generic;
using System.IO;
using Qowaiv.UnitTests.TestTools;

namespace Qowaiv.UnitTests.SvoGenerator.Xml
{
    [TestFixture]
    public class XResourceFileTest
    {
        [Test]
        public void Load_NullFileThrowArgumentNullException()
        {
            ExceptionAssert.ExpectArgumentNullException(() =>
            {
                XResourceFile.Load((FileInfo)null);
            },
            "file");
        }

        [Test]
        public void Load_NullStream_ThrowArgumentNullException()
        {
            ExceptionAssert.ExpectArgumentNullException(() =>
            {
                XResourceFile.Load((Stream)null);
            },
            "stream");
        }

        [Test]
        public void Save_NullFile_ThrowArgumentNullException()
        {
            ExceptionAssert.ExpectArgumentNullException(() =>
            {
                new XResourceFile().Save((FileInfo)null);
            },
            "file");
        }

        [Test]
        public void Save_NullStream_ThrowArgumentNullException()
        {
            ExceptionAssert.ExpectArgumentNullException(() =>
            {
                new XResourceFile().Save((Stream)null);
            },
            "stream");
        }


        [Test]
        public void SaveLoad_Data_AreEqual()
        {

            var file = new FileInfo("SaveLoad_Data_AreEqual.resx");

            var resourceFile = new XResourceFile(new List<XResourceFileData>()
            {
                new XResourceFileData("key0", "Some value 0"),
                new XResourceFileData("key1", "1.1", "comment 1.1"),
            });

            resourceFile.Save(file);

            var act = XResourceFile.Load(file);
        }
    }
}
