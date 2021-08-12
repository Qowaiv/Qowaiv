using NUnit.Framework;
using Qowaiv.Text;
using System;
using System.Linq;

namespace Text.Base64_specs
{
    public class ToString
    {
        [Test]
        public void Null_StringEmpty()
        {
            Assert.AreEqual(string.Empty, Base64.ToString(null));
        }

        [Test]
        public void EmptyArray_StringEmpty()
        {
            Assert.AreEqual(string.Empty, Array.Empty<byte>());
        }

        [TestCase("Aao=", 1, 170)]
        [TestCase("Cxct", 11, 23, 45)]
        [TestCase("Qowaig==", 66, 140, 26, 138)]
        public void bytes(string expected, params int[] data)
        {
            var bytes = data.Select(v => (byte)v).ToArray();

            var actualString = Base64.ToString(bytes);
            Assert.AreEqual(expected, actualString);
        }
    }

    public class TryGetBytes_from
    {
        [Test]
        public void Null__returns_EmptyArray()
        {
            Assert.IsTrue(Base64.TryGetBytes(null, out var bytes));
            CollectionAssert.AreEqual(Array.Empty<byte>(), bytes);
        }

        [Test]
        public void StringEmpty_returns_EmptyArray()
        {
            Assert.IsTrue(Base64.TryGetBytes(string.Empty, out var bytes));
            CollectionAssert.AreEqual(Array.Empty<byte>(), bytes);
        }

        [TestCase("Aap=", 1, 170)]
        [TestCase("Cxct", 11, 23, 45)]
        [TestCase("Qowaiv==", 66, 140, 26, 138)]
        public void String_returns(string str, params int[] expected)
        {
            var bytes = expected.Select(v => (byte)v).ToArray();
            Assert.IsTrue(Base64.TryGetBytes(str, out byte[] actualBytes));
            CollectionAssert.AreEqual(bytes, actualBytes);
        }

        [Test]
        public void Not_support_chars_returns_false_with_EmptyArray()
        {
            Assert.IsFalse(Base64.TryGetBytes("ABC}", out byte[] bytes));
            Assert.AreEqual(Array.Empty<byte>(), bytes);
        }
    }
}
