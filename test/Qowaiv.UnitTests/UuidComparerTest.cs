using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Qowaiv.UnitTests
{
    public class UuidComparerTest
    {
        /// <summary>Proves that the<paramref name="index0"/> has an higher priority than the paired
        /// <paramref name="index1"/>.
        /// </summary>
        [TestCase(10, 11)]
        [TestCase(11, 12)]
        [TestCase(12, 13)]
        [TestCase(13, 14)]
        [TestCase(14, 15)]
        [TestCase(15, 08)]
        [TestCase(08, 09)]
        [TestCase(09, 06)]
        [TestCase(06, 07)]
        [TestCase(07, 04)]
        [TestCase(04, 05)]
        [TestCase(05, 00)]
        [TestCase(00, 01)]
        [TestCase(01, 02)]
        [TestCase(02, 03)]
        public void Compare_SqlServer(int index0, int index1)
        {
            var l = Simple(index0, index1, 1, 2);
            var r = Simple(index0, index1, 2, 1);

            var compare = UuidComparer.SqlServer.Compare(l, r);
            Assert.AreEqual(-1, compare);
        }

        private static Uuid Simple(int i0, int i1, byte v0, byte v1)
        {
            var bytes = new byte[16];
            bytes[i0] = v0;
            bytes[i1] = v1;
            return new Guid(bytes);
        }


        [Test]
        public void Compare_Default_IsOrderComarerGuidDefault()
        {
            var uuids = new List<Guid>();

            for (var i = 0; i < 1000; i++)
            {
                uuids.Add(Uuid.NewUuid());
            }
            uuids.Sort(UuidComparer.Default);

            CollectionAssert.IsOrdered(uuids, Comparer<Guid>.Default);
        }
    }
}
