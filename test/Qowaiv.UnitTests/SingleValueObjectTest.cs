using NUnit.Framework;
using Qowaiv.Formatting;
using Qowaiv.Mathematics;
using Qowaiv.Reflection;
using Qowaiv.TestTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Qowaiv.UnitTests
{
    public class SingleValueObjectTest
    {
        internal static readonly Type[] NoneSvos = new [] { typeof(FormattingArguments) };

        public static IEnumerable<Type> SvoTypes
        {
            get => typeof(SingleValueObjectAttribute).Assembly
                .GetTypes()
                .Where(tp => tp.IsValueType && tp.IsPublic && !tp.IsEnum && !tp.IsAbstract)
                .Except(NoneSvos);
        }

        [TestCaseSource(nameof(SvoTypes))]
        public void IsSingleValueObject(Type svoType)
        {
            Assert.IsTrue(QowaivType.IsSingleValueObject(svoType));
        }

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
        public void ImplementsInterfaces(Type svo)
        {
            SvoAssert.ImplementsInterfaces(svo);
        }

        [TestCaseSource(nameof(SvoTypes))]
        public void AttributesMatches(Type svo)
        {
            SvoAssert.AttributesMatches(svo);
        }

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
