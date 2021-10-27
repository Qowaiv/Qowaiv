using FluentAssertions;
using NUnit.Framework;
using Qowaiv.Mathematics;
using Qowaiv.Reflection;
using Qowaiv.TestTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests
{
    public class SingleValueObjectTest
    {
        private static IEnumerable<Type> SvoTypes => SVO_specs.All.Types;

        [TestCaseSource(nameof(SvoTypes))]
        public void IsSingleValueObject(Type svoType)
            => svoType.GetCustomAttribute<SingleValueObjectAttribute>().Should().NotBeNull();

        [TestCaseSource(nameof(SvoTypes))]
        public void ParseMatches(Type svo)
        {
            SvoAssert.ParseMatches(svo, svo.GetCustomAttribute<SingleValueObjectAttribute>());
        }

        [TestCaseSource(nameof(SvoTypes))]
        public void TryParseMatches(Type svo)
        {
            SvoAssert.TryParseMatches(svo, svo.GetCustomAttribute<SingleValueObjectAttribute>());
        }

        [TestCaseSource(nameof(SvoTypes))]
        public void IsValidMatches(Type svo)
        {
            SvoAssert.IsValidMatches(svo, svo.GetCustomAttribute<SingleValueObjectAttribute>());
        }

        [TestCaseSource(nameof(SvoTypes))]
        public void EmptyAndUnknownMatches(Type svo)
        {
            SvoAssert.EmptyAndUnknownMatches(svo, svo.GetCustomAttribute<SingleValueObjectAttribute>());
        }

        [TestCaseSource(nameof(SvoTypes))]
        public void ImplementsISerializable(Type svo) => TypeAssert.ImplementsInterface(svo, typeof(ISerializable));

        [TestCaseSource(nameof(SvoTypes))]
        public void ImplementsIXmlSerializable(Type svo) => TypeAssert.ImplementsInterface(svo, typeof(IXmlSerializable));

        [TestCaseSource(nameof(SvoTypes))]
        public void ImplementsIFormattable(Type svo) => TypeAssert.ImplementsInterface(svo, typeof(IFormattable));

        [TestCaseSource(nameof(SvoTypes))]
        public void ImplementsIComparable(Type svo) => TypeAssert.ImplementsInterface(svo, typeof(IComparable));

        [TestCaseSource(nameof(SvoTypes))]
        public void ImplementsIComparableOfT(Type svo) => TypeAssert.ImplementsInterface(svo, typeof(IComparable<>).MakeGenericType(svo));

        [TestCaseSource(nameof(SvoTypes))]
        public void HasSerializableAttribute(Type svo) => TypeAssert.HasAttribute(svo, typeof(SerializableAttribute));

        [TestCaseSource(nameof(SvoTypes))]
        public void HasTypeConverterAttribute(Type svo) => TypeAssert.HasAttribute(svo, typeof(TypeConverterAttribute));

        [TestCaseSource(nameof(SvoTypes))]
        public void HasDebuggerDisplayAttribute(Type svo) => TypeAssert.HasAttribute(svo, typeof(DebuggerDisplayAttribute));

        [TestCaseSource(nameof(SvoTypes))]
        public void UnderlyingTypeMatches(Type svo)
        {
            // does not have an m_Value.
            if (typeof(Fraction) == svo) { return; }

            SvoAssert.UnderlyingTypeMatches(svo, svo.GetCustomAttribute<SingleValueObjectAttribute>());
        }

        [Test]
        public void Ctor_Params_AreEqual()
        {
            var act = new SingleValueObjectAttribute(SingleValueStaticOptions.All, typeof(string));

            Assert.AreEqual(SingleValueStaticOptions.All, act.StaticOptions, "act.StaticOptions");
            Assert.AreEqual(typeof(string), act.UnderlyingType, "act.UnderlyingType");
        }
    }
}
