using NUnit.Framework;

namespace Qowaiv.DomainModel.UnitTests
{
    public class QowaivHashTest
    {
        [Test]
        public void Hash_WithoutSwift()
        {
            var act = QowaivHash.Hash(true);
            var exp = 1;
            Assert.AreEqual(exp, act);
        }

        [TestCase(17, 17, 0)]
        [TestCase(34, 17, 1)]
        [TestCase(68, 17, 2)]
        [TestCase(17, 17, 32)]
        [TestCase(+1073741828, 17, 30)]
        [TestCase(-2147483640, 17, 31)]
        public void Hash(int exp, int value, int shift)
        {
            var act = QowaivHash.Hash(value, shift);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void HashObjectWithSwift_Null_IsZero()
        {
            var act = QowaivHash.HashObject<object>(null, 1);
            var exp = 0;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void HashObject_Null_IsZero()
        {
            var act = QowaivHash.HashObject<object>(null);
            var exp = 0;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void HashObject_NotNull()
        {
            var act = QowaivHash.HashObject(new HashableObject(17));
            var exp = 17;
            Assert.AreEqual(exp, act);
        }

        [TestCase(17, 17, 0)]
        [TestCase(34, 17, 1)]
        [TestCase(68, 17, 2)]
        [TestCase(17, 17, 32)]
        [TestCase(+1073741828, 17, 30)]
        [TestCase(-2147483640, 17, 31)]
        public void HashObject(int exp, int value, int shift)
        {
            var act = QowaivHash.HashObject(new HashableObject(value), shift);
            Assert.AreEqual(exp, act);
        }
    }

    internal sealed class HashableObject
    {
        private readonly int _hash;
        public HashableObject(int hash) => _hash = hash;
        public override int GetHashCode() => _hash;
        public override bool Equals(object obj) => obj is HashableObject other && other._hash == _hash;
    }
}
