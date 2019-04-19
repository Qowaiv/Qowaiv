using NUnit.Framework;
using Qowaiv.ComponentModel.DataAnnotations;
using System;

namespace Qowaiv.ComponentModel.UnitTests.DataAnnotations
{
    public class DefinedEnumValuesOnlyAttributeTest
    {
        [Test]
        public void IsValid_null_IsTrue()
        {
            var attribute = new DefinedEnumValuesOnlyAttribute();
            Assert.IsTrue(attribute.IsValid(null));
        }

        [Test]
        public void IsValid_NotAnEnum_Throws()
        {
            var attribute = new DefinedEnumValuesOnlyAttribute();
            Assert.Throws<ArgumentException>(() => attribute.IsValid(1));
        }

        [Test]
        public void IsValid_FlagsEnumSingle_IsTrue()
        {
            var attribute = new DefinedEnumValuesOnlyAttribute();
            Assert.IsTrue(attribute.IsValid(Flag.UnionJack));
        }

        [Test]
        public void IsValid_FlagsEnumMixed_IsTrue()
        {
            var attribute = new DefinedEnumValuesOnlyAttribute { AllowUndefinedFlagCombinations = false };
            Assert.IsTrue(attribute.IsValid(Flag.American));
        }

        [Test]
        public void IsValid_FlagsEnumMixedAllowUndefinedFlagCombinations_IsTrue()
        {
            var attribute = new DefinedEnumValuesOnlyAttribute { AllowUndefinedFlagCombinations = true };
            Assert.IsTrue(attribute.IsValid(Flag.UnionJack | Flag.StarsAndStripes));
        }

        [Test]
        public void IsValid_FlagsEnumMixed_IsFalse()
        {
            var attribute = new DefinedEnumValuesOnlyAttribute { AllowUndefinedFlagCombinations = false };
            Assert.IsFalse(attribute.IsValid(Flag.UnionJack | Flag.StarsAndStripes));
        }

        [Test]
        public void IsValid_FlagsRandomIntValue_IsFalse()
        {
            var attribute = new DefinedEnumValuesOnlyAttribute();
            Assert.IsFalse(attribute.IsValid((Flag)666));
        }

        [Test]
        public void IsValid_EnumDefined_IsTrue()
        {
            var attribute = new DefinedEnumValuesOnlyAttribute();
            Assert.IsTrue(attribute.IsValid(Number.Zero));
        }

        [Test]
        public void IsValid_EnumNotDefined_IsFalse()
        {
            var attribute = new DefinedEnumValuesOnlyAttribute();
            Assert.IsFalse(attribute.IsValid((Number)17));
        }


        [Flags]
        public enum Flag
        {
            UnionJack = 1,
            StarsAndStripes = 2,
            MapleLeaf = 4,
            American = StarsAndStripes | MapleLeaf,
        }

        public enum Number
        {
            Zero,
            One,
        }
    }
}
