using NUnit.Framework;
using Qowaiv.Reflection;
using System;
using System.Collections.Generic;

namespace Qowaiv.UnitTests.Reflection
{
    public class QowaivTypeTest
    {
        [TestCase(typeof(decimal))]
        [TestCase(typeof(double))]
        [TestCase(typeof(int))]
        [TestCase(typeof(sbyte))]
        [TestCase(typeof(ulong))]
        public void IsNumeric(Type type)
        {
            Assert.IsTrue(QowaivType.IsNumeric(type));
        }

        [TestCase(typeof(object))]
        [TestCase(typeof(string))]
        [TestCase(typeof(QowaivTypeTest))]
        public void IsNotNumeric(Type type)
        {
            Assert.IsFalse(QowaivType.IsNumeric(type));
        }

        [TestCase(typeof(int), typeof(int))]
        [TestCase(typeof(int), typeof(int?))]
        [TestCase(typeof(string), typeof(string))]
        public void GetNotNullableType(Type expected, Type type)
        {
            Assert.AreEqual(expected, QowaivType.GetNotNullableType(type));
        }

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
            var formatted = QowaivType.ToCSharpString(type);
            Assert.AreEqual(expected, formatted);
        }

        [Test]
        public void ToCSharpString_GenericArgument()
        {
            var generic = typeof(GenericOf).GetMethod(nameof(GenericOf.Default)).ReturnType;
            var formatted = QowaivType.ToCSharpString(generic);
            Assert.AreEqual("QowaivTypeTest.GenericOf.TModel", formatted);
        }

        internal class NestedTest { }

        internal class GenericOf
        {
            public TModel Default<TModel>() => default;
        }
    }
}
