﻿using NUnit.Framework;
using Qowaiv.Reflection;
using System;
using System.Collections.Generic;

namespace Qowaiv.UnitTests.Reflection
{
    public class QowaivTypeTest
    {
        [Test]
        public void IsNullOrDefaultValue_Null()
        {
            Assert.IsTrue(QowaivType.IsNullOrDefaultValue(null));
        }

        [Test]
        public void IsNullOrDefaultValue_HouseNumberEmpty()
        {
            Assert.IsTrue(QowaivType.IsNullOrDefaultValue(HouseNumber.Empty));
        }

        [Test]
        public void IsNotNullOrDefaultValue_17()
        {
            Assert.IsFalse(QowaivType.IsNullOrDefaultValue(17));
        }

        [Test]
        public void IsNotNullOrDefaultValue_SomeObject()
        {
            Assert.IsFalse(QowaivType.IsNullOrDefaultValue(new QowaivTypeTest()));
        }

        [TestCase(typeof(string), "string")]
        [TestCase(typeof(byte[]), "byte[]")]
        [TestCase(typeof(Guid), "System.Guid")]
        [TestCase(typeof(Dictionary<string, Action>), "System.Collections.Generic.Dictionary<string, System.Action>")]
        [TestCase(typeof(long?), "long?")]
        [TestCase(typeof(Dictionary<,>), "System.Collections.Generic.Dictionary<,>")]
        [TestCase(typeof(Nullable<>), "System.Nullable<>")]
        [TestCase(typeof(Dictionary<object, List<int?>>[]), "System.Collections.Generic.Dictionary<object, System.Collections.Generic.List<int?>>[]")]
        [TestCase(typeof(NestedTest), "Qowaiv.UnitTests.Reflection.QowaivTypeTest.NestedTest")]
        public void ToCSharpStringWithNamespace(Type type, string expected)
        {
            var formatted = QowaivType.ToCSharpString(type, true);
            Assert.AreEqual(expected, formatted);
        }

        [TestCase(typeof(string), "string")]
        [TestCase(typeof(byte[]), "byte[]")]
        [TestCase(typeof(int[][]), "int[][]")]
        [TestCase(typeof(int[,]), "int[,]")]
        [TestCase(typeof(int[,,]), "int[,,]")]
        [TestCase(typeof(Guid), "Guid")]
        [TestCase(typeof(Dictionary<string, Action>), "Dictionary<string, Action>")]
        [TestCase(typeof(long?), "long?")]
        [TestCase(typeof(Dictionary<,>), "Dictionary<,>")]
        [TestCase(typeof(Nullable<>), "Nullable<>")]
        [TestCase(typeof(Dictionary<object, List<int?>>[]), "Dictionary<object, List<int?>>[]")]
        [TestCase(typeof(NestedTest), "QowaivTypeTest.NestedTest")]
        public void ToCSharpStringWithoutNamespace(Type type, string expected)
        {
            var formatted = QowaivType.ToCSharpString(type, false);
            Assert.AreEqual(expected, formatted);
        }

        internal class NestedTest { }
    }
}
