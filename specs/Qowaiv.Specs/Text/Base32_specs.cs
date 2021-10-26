﻿using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.TestTools.Globalization;
using Qowaiv.Text;
using System;
using System.Linq;

namespace Text.Base32_specs
{
    public class ToString
    {
        [Test]
        public void Null_StringEmpty()
        {
            Assert.AreEqual(string.Empty, Base32.ToString(null));
        }

        [Test]
        public void EmptyArray_StringEmpty()
        {
            Assert.AreEqual(string.Empty, Array.Empty<byte>());
        }

        [TestCase("BQ", 12)]
        [TestCase("BQRA", 12, 34)]
        [TestCase("QOWAI", 131, 172, 4)]
        [TestCase("BQRDQTQ", 12, 34, 56, 78)]
        [TestCase("BQRDQTS2", 12, 34, 56, 78, 90)]
        [TestCase("BQRDQTS2N4", 12, 34, 56, 78, 90, 111)]
        [TestCase("BQRDQTS2N55Q", 12, 34, 56, 78, 90, 111, 123)]
        [TestCase("BQRDQTS2N55YM", 12, 34, 56, 78, 90, 111, 123, 134)]
        [TestCase("BQRDQTS2N55YNEI", 12, 34, 56, 78, 90, 111, 123, 134, 145)]
        [TestCase("BQRDQTS2N55YNEM4", 12, 34, 56, 78, 90, 111, 123, 134, 145, 156)]
        [TestCase("BQRDQTS2N55YNEM4U4", 12, 34, 56, 78, 90, 111, 123, 134, 145, 156, 167)]
        [TestCase("BQRDQTS2N55YNEM4U6ZA", 12, 34, 56, 78, 90, 111, 123, 134, 145, 156, 167, 178)]
        [TestCase("THEQUICKBROWNFOXJUMBSOVERTHELAZYDOG2345674", 153, 201, 010, 032, 074, 012, 093, 102, 149, 215, 077, 024, 025, 058, 164, 140, 206, 069, 131, 056, 027, 141, 173, 243, 190, 255)]
        public void bytes(string expected, params int[] data)
        {
            var bytes = data.Select(v => (byte)v).ToArray();

            var actualString = Base32.ToString(bytes);
            Assert.AreEqual(expected, actualString);
        }

        [Test]
        public void LowerCase()
        {
            var bytes = new byte[] { 153, 201, 010, 032, 074, 012, 093, 102, 149, 215, 077, 024, 025, 058, 164, 140, 206, 069, 131, 056, 027, 141, 173, 243, 190, 255 };
            var str = Base32.ToString(bytes, true);
            Assert.AreEqual("thequickbrownfoxjumbsoverthelazydog2345674", str);
        }
    }

    public class TryGetBytes_from
    {
        [TestCase("BQ", 12)]
        [TestCase("BQRA", 12, 34)]
        [TestCase("QOWAI", 131, 172, 4)]
        [TestCase("BQRDQTQ", 12, 34, 56, 78)]
        [TestCase("BQRDQTS2", 12, 34, 56, 78, 90)]
        [TestCase("BQRDQTS2N4", 12, 34, 56, 78, 90, 111)]
        [TestCase("BQRDQTS2N55Q", 12, 34, 56, 78, 90, 111, 123)]
        [TestCase("BQRDQTS2N55YM", 12, 34, 56, 78, 90, 111, 123, 134)]
        [TestCase("BQRDQTS2N55YNEI", 12, 34, 56, 78, 90, 111, 123, 134, 145)]
        [TestCase("BQRDQTS2N55YNEM4", 12, 34, 56, 78, 90, 111, 123, 134, 145, 156)]
        [TestCase("BQRDQTS2N55YNEM4U4", 12, 34, 56, 78, 90, 111, 123, 134, 145, 156, 167)]
        [TestCase("BQRDQTS2N55YNEM4U6ZA", 12, 34, 56, 78, 90, 111, 123, 134, 145, 156, 167, 178)]
        [TestCase("THEQUICKBROWNFOXJUMBSOVERTHELAZYDOG2345674", 153, 201, 010, 032, 074, 012, 093, 102, 149, 215, 077, 024, 025, 058, 164, 140, 206, 069, 131, 056, 027, 141, 173, 243, 190, 255)]
        public void String_returns(string str, params int[] expected)
        {
            var bytes = expected.Select(v => (byte)v).ToArray();
            Assert.IsTrue(Base32.TryGetBytes(str, out byte[] actualBytes));
            CollectionAssert.AreEqual(bytes, actualBytes);
        }

        [Test]
        public void Not_support_chars_returns_false_with_EmptyArray()
        {
            Assert.IsFalse(Base32.TryGetBytes("ABC}", out byte[] bytes));
            Assert.AreEqual(Array.Empty<byte>(), bytes);
        }
    }
    public class GetBytes
    {
        [Test]
        public void Null__returns_EmptyArray()
        {
            CollectionAssert.AreEqual(Array.Empty<byte>(), Base32.GetBytes(null));
        }

        [Test]
        public void StringEmpty_returns_EmptyArray()
        {
            CollectionAssert.AreEqual(Array.Empty<byte>(), Base32.GetBytes(string.Empty));
        }

        [Test]
        public void LowerCase_equals_UpperCase()
        {
            var str = "thequickbrownfoxjumbsoverthelazydog2345674";
            var lowercase = Base32.GetBytes(str);
            var uppercase = Base32.GetBytes(str.ToUpperInvariant());
            Assert.AreEqual(uppercase, lowercase);
        }

        [Test]
        public void Throws_on_invalid_input()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var act = Assert.Catch<FormatException>(() =>
                {
                    Base32.GetBytes("Q0waiv");
                });
                Assert.AreEqual("Not a valid Base32 string", act.Message);
            }
        }
    }
}
