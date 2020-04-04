#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.TestTools.Globalization;
using Qowaiv.Text;
using System;
using System.Linq;

namespace Qowaiv.UnitTests.Text
{
    [TestFixture]
    public class Base32Test
    {
        [TestCase("")]
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
        public void ToString_Data(string str, params int[] vals)
        {
            var bytes = vals.Select(v => (byte)v).ToArray();

            Assert.IsTrue(Base32.TryGetBytes(str, out byte[] actualBytes));
            CollectionAssert.AreEqual(bytes, actualBytes);

            var actualString = Base32.ToString(bytes);
            Assert.AreEqual(str, actualString);
        }

        [Test]
        public void ToString_LowerCase()
        {
            var bytes = new byte[] { 153, 201, 010, 032, 074, 012, 093, 102, 149, 215, 077, 024, 025, 058, 164, 140, 206, 069, 131, 056, 027, 141, 173, 243, 190, 255 };
            var str = Base32.ToString(bytes, true);
            Assert.AreEqual("thequickbrownfoxjumbsoverthelazydog2345674", str);
        }

        [Test]
        public void TryGetBytes_LowerCaseEqualsUpperCase()
        {
            var str = "thequickbrownfoxjumbsoverthelazydog2345674";
            var lowercase = Base32.GetBytes(str);
            var uppercase = Base32.GetBytes(str.ToUpperInvariant());

            Assert.AreEqual(uppercase, lowercase);
        }

        [Test]
        public void ToString_Null_StringEmpty()
        {
            var act = Base32.ToString(null);
            Assert.AreEqual(string.Empty, act);
        }
        [Test]
        public void ToString_EmptyArray_StringEmpty()
        {
            var act = Base32.ToString(new byte[0]);
            Assert.AreEqual(string.Empty, act);
        }

        [Test]
        public void GetBytes_Q0waiv_ThrowsFormatException()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var act = Assert.Throws<FormatException>(() =>
                {
                    Base32.GetBytes("Q0waiv");
                });
                Assert.AreEqual("Not a valid Base32 string", act.Message);
            }
        }

        [Test]
        public void GetBytes_Null_EmptyArray()
        {
            var act = Base32.GetBytes(null);
            var exp = new byte[0];
            CollectionAssert.AreEqual(exp, act);
        }

        [Test]
        public void TryGetBytes_NotSupportChars_FalseAndEmptyArray()
        {
            Assert.IsFalse(Base32.TryGetBytes("ABC}", out byte[] bytes));
            Assert.AreEqual(new byte[0], bytes);
        }

        [Test]
        public void Generate_CharsLookup()
        {
            var str = "abcdefghijklmnopqrstuvwxyz234567";

            var max = (int)'z';
            var bytes = new byte[max + 1];

            for(var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.MaxValue;
            }

            foreach(var ch in str)
            {
                var val = (byte)str.IndexOf(ch);
                bytes[ch] = val;
                bytes[char.ToUpperInvariant(ch)] = val;
            }

            var values = string.Join(",", bytes);

            Console.WriteLine(values);

            var expected = "255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,26,27,28,29,30,31,255,255,255,255,255,255,255,255,255,0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,255,255,255,255,255,255,0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25";

            Assert.AreEqual(expected, values);
        }
    }
}
