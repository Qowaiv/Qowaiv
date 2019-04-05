using NUnit.Framework;
using Qowaiv.ComponentModel.DataAnnotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Qowaiv.ComponentModel.UnitTests.DataAnnotations
{
    public class DistinctValuesAttributeTest
    {
        [Test]
        public void Ctor_TypeIsNotAEqualityComparer_Throws()
        {
            Assert.Throws<ArgumentException>(() => new DistinctValuesAttribute(typeof(string)));
        }

        [Test]
        public void IsValid_WithCustomComparerOfObject_IsTrue()
        {
            var attribute = new DistinctValuesAttribute(typeof(ObjectEqualityComparer));
            Assert.IsTrue(attribute.IsValid(new[] { 14, 42 }));
        }

        [Test]
        public void IsValid_WithCustomComparer_IsFalse()
        {
            var attribute = new DistinctValuesAttribute(typeof(NoSharedPrimeEqualityComparer));
            Assert.IsFalse(attribute.IsValid(new[] { 14, 42 }));
        }
        [Test]
        public void IsValid_WithoutCustomComparer_IsTrue()
        {
            var attribute = new DistinctValuesAttribute();
            Assert.IsTrue(attribute.IsValid(new[] { 14, 42 }));
        }

        [Test]
        public void IsValid_Null_IsTrue()
        {
            var attribute = new DistinctValuesAttribute();
            Assert.IsTrue(attribute.IsValid(null));
        }

        [Test]
        public void IsValid_EmptyString_IsTrue()
        {
            var attribute = new DistinctValuesAttribute();
            Assert.IsTrue(attribute.IsValid(""));
        }

        [Test]
        public void IsValid_NoEnumerable_Throws()
        {
            var attribute = new DistinctValuesAttribute();
            Assert.Throws<ArgumentException>(() => attribute.IsValid(7));
        }
    }

    internal class NoSharedPrimeEqualityComparer : IEqualityComparer
    {
        private static readonly int[] primes = { 2, 3, 5, 7, 11, 13, 17, 23, 29, 31 };

        bool IEqualityComparer.Equals(object x, object y)
        {
            int a = (int)x;
            int b = (int)y;
            return primes.Any(prime => a % prime == 0 && b % prime == 0);
        }

        public int GetHashCode(object obj) => 0;
    }

    internal class ObjectEqualityComparer : IEqualityComparer<object>
    {
        public new bool Equals(object x, object y) => object.Equals(x, y);
        public int GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
    }
}
