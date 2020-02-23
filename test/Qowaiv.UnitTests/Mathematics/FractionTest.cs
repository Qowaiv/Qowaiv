using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.Mathematics;
using Qowaiv.TestTools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests.Mathematics
{
    /// <summary>Tests the fraction SVO.</summary>
    public class FractionTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly Fraction TestStruct = Fraction.Parse("-69/17");

        /// <summary>Fraction.Zero should be equal to the default of fraction.</summary>
        [Test]
        public void Zero_None_EqualsDefault()
        {
            Assert.AreEqual(default(Fraction), Fraction.Zero);
        }

        /// <summary>Fraction.IsZero() should be true for the default of fraction.</summary>
        [Test]
        public void IsZero_Default_IsTrue()
        {
            Assert.IsTrue(default(Fraction).IsZero());
        }

        /// <summary>Fraction.IsZero() should be false for the TestStruct.</summary>
        [Test]
        public void IsZero_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsZero());
        }

        [TestCase(17, 1, "17")]
        [TestCase(17, 1, "+17")]
        [TestCase(12, 1, "-12")]
        [TestCase(12_345, 1, "12,345")]
        [TestCase(1, 3, "1/3")]
        [TestCase(1, 3, "+1/3")]
        [TestCase(-1, 3, "-1/3")]
        public void Parse(long numerator, long denominator, string str)
        {
            var expected = new Fraction(numerator, denominator);
            var actual = Fraction.Parse(str, CultureInfo.InvariantCulture);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>TryParse null should be valid.</summary>
        [Test]
        public void TyrParse_Null_IsValid()
        {
            Assert.IsTrue(Fraction.TryParse(null, out var val));
            Assert.AreEqual(default(Fraction), val);
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TyrParse_StringEmpty_IsValid()
        {
            Assert.IsTrue(Fraction.TryParse(string.Empty, out var val));
            Assert.AreEqual(default(Fraction), val);
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TyrParse_StringValue_IsValid()
        {
            string str = "string";
            Assert.IsTrue(Fraction.TryParse(str, out var val));
            Assert.AreEqual(str, val.ToString());
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [Test]
        public void TyrParse_StringValue_IsNotValid()
        {
            string str = "invalid";
            Assert.IsFalse(Fraction.TryParse(str, out var val));
            Assert.AreEqual(default(Fraction), val);
        }

        [Test]
        public void Parse_InvalidInput_ThrowsFormatException()
        {
            using (new CultureInfoScope("en-GB"))
            {
                Assert.Catch<FormatException>(() =>
                {
                    Fraction.Parse("InvalidInput");
                }

                , "Not a valid fraction");
            }
        }

        [Test]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = TestStruct;
                var act = Fraction.TryParse(exp.ToString());
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void TryParse_InvalidInput_DefaultValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = default(Fraction);
                var act = Fraction.TryParse("InvalidInput");
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void Constructor_SerializationInfoIsNull_Throws()
        {
            Assert.Catch<ArgumentNullException>(() => SerializationTest.DeserializeUsingConstructor<Fraction>(null, default));
        }

        [Test]
        public void Constructor_InvalidSerializationInfo_Throws()
        {
            var info = new SerializationInfo(typeof(Fraction), new FormatterConverter());
            Assert.Catch<SerializationException>(() => SerializationTest.DeserializeUsingConstructor<Fraction>(info, default));
        }

        [Test]
        public void GetObjectData_NulSerializationInfo_Throws()
        {
            ISerializable obj = TestStruct;
            Assert.Catch<ArgumentNullException>(() => obj.GetObjectData(null, default));
        }

        [Test]
        public void GetObjectData_SerializationInfo_AreEqual()
        {
            ISerializable obj = TestStruct;
            var info = new SerializationInfo(typeof(Fraction), new FormatterConverter());
            obj.GetObjectData(info, default);
            Assert.AreEqual((long)2, info.GetValue("Value", typeof(long)));
        }

        [Test]
        public void SerializeDeserialize_TestStruct_AreEqual()
        {
            var input = TestStruct;
            var exp = TestStruct;
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void DataContractSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = TestStruct;
            var exp = TestStruct;
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void XmlSerialize_TestStruct_AreEqual()
        {
            var act = SerializationTest.XmlSerialize(TestStruct);
            var exp = "xmlstring";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void XmlDeserialize_XmlString_AreEqual()
        {
            var act = SerializationTest.XmlDeserialize<FractionSerializeObject>("xmlstring");
            Assert.AreEqual(TestStruct, act);
        }

        [Test]
        public void SerializeDeserialize_FractionSerializeObject_AreEqual()
        {
            var input = new FractionSerializeObject { Id = 17, Obj = TestStruct, Date = new DateTime(1970, 02, 14), };
            var exp = new FractionSerializeObject { Id = 17, Obj = TestStruct, Date = new DateTime(1970, 02, 14), };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void XmlSerializeDeserialize_FractionSerializeObject_AreEqual()
        {
            var input = new FractionSerializeObject { Id = 17, Obj = TestStruct, Date = new DateTime(1970, 02, 14), };
            var exp = new FractionSerializeObject { Id = 17, Obj = TestStruct, Date = new DateTime(1970, 02, 14), };
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void DataContractSerializeDeserialize_FractionSerializeObject_AreEqual()
        {
            var input = new FractionSerializeObject { Id = 17, Obj = TestStruct, Date = new DateTime(1970, 02, 14), };
            var exp = new FractionSerializeObject { Id = 17, Obj = TestStruct, Date = new DateTime(1970, 02, 14), };
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void SerializeDeserialize_Default_AreEqual()
        {
            var input = new FractionSerializeObject { Id = 17, Obj = default, Date = new DateTime(1970, 02, 14), };
            var exp = new FractionSerializeObject { Id = 17, Obj = default, Date = new DateTime(1970, 02, 14), };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void XmlSerializeDeserialize_Default_AreEqual()
        {
            var input = new FractionSerializeObject { Id = 17, Obj = default, Date = new DateTime(1970, 02, 14), };
            var exp = new FractionSerializeObject { Id = 17, Obj = default, Date = new DateTime(1970, 02, 14), };
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void GetSchema_None_IsNull()
        {
            IXmlSerializable obj = TestStruct;
            Assert.IsNull(obj.GetSchema());
        }

        [TestCase("Invalid input")]
        [TestCase("2017-06-11")]
        [TestCase(long.MinValue)]
        [TestCase(double.MinValue)]
        public void FromJson_Invalid_Throws(object json)
        {
            Assert.Catch<FormatException>(() => JsonTester.Read<Fraction>(json));
        }

        [TestCase("4/1", 4L)]
        [TestCase("3/1", 3.0)]
        [TestCase("1/3", "14/42")]
        public void FromJson(Fraction expected, object json)
        {
            var actual = JsonTester.Read<Fraction>(json);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_Zero_StringEmpty()
        {
            var act = Fraction.Zero.ToString();
            var exp = "0";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: 'Some Formatted Value', format: 'Unit Test Format'";
            Assert.AreEqual(exp, act);
        }

        [TestCase("en-US", "", "ComplexPattern", "ComplexPattern")]
        [TestCase("nl-BE", null, "1600,1", "1600,1")]
        [TestCase("en-GB", null, "1600.1", "1600.1")]
        [TestCase("nl-BE", "0000", "800", "0800")]
        [TestCase("en-GB", "0000", "800", "0800")]
        public void ToString_UsingCultureWithPattern(string culture, string format, string str, string expected)
        {
            using (new CultureInfoScope(culture))
            {
                var actual = Fraction.Parse(str).ToString(format);
                Assert.AreEqual(expected, actual);
            }
        }

        [Test]
        public void ToString_FormatValueSpanishEcuador_AreEqual()
        {
            var act = Fraction.Parse("1700").ToString("00000.0", new CultureInfo("es-EC"));
            var exp = "01700,0";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void DebuggerDisplay_DebugToString_HasAttribute()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(Fraction));
        }

        [Test]
        public void DebuggerDisplay_DefaultValue_String()
        {
            DebuggerDisplayAssert.HasResult("ComplexPattern", default(Fraction));
        }

        [Test]
        public void DebuggerDisplay_TestStruct_String()
        {
            DebuggerDisplayAssert.HasResult("ComplexPattern", TestStruct);
        }

        /// <summary>GetHash should not fail for Fraction.Zero.</summary>
        [Test]
        public void GetHash_Zero_Hash()
        {
            Assert.AreEqual(-1, Fraction.Zero.GetHashCode());
        }

        /// <summary>GetHash should not fail for the test struct.</summary>
        [Test]
        public void GetHash_TestStruct_Hash()
        {
            Assert.AreEqual(-1, TestStruct.GetHashCode());
        }

        [Test]
        public void Equals_EmptyEmpty_IsTrue()
        {
            Assert.IsTrue(Fraction.Zero.Equals(Fraction.Zero));
        }

        [Test]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = Fraction.Parse("formatted", CultureInfo.InvariantCulture);
            var r = Fraction.Parse("unformatted", CultureInfo.InvariantCulture);
            Assert.IsTrue(l.Equals(r));
        }

        [Test]
        public void Equals_TestStructTestStruct_IsTrue()
        {
            Assert.IsTrue(TestStruct.Equals(TestStruct));
        }

        [Test]
        public void Equals_TestStructZero_IsFalse()
        {
            Assert.IsFalse(TestStruct.Equals(Fraction.Zero));
        }

        [Test]
        public void Equals_ZeroTestStruct_IsFalse()
        {
            Assert.IsFalse(Fraction.Zero.Equals(TestStruct));
        }

        [Test]
        public void Equals_TestStructObjectTestStruct_IsTrue()
        {
            Assert.IsTrue(TestStruct.Equals((object)TestStruct));
        }

        [Test]
        public void Equals_TestStructNull_IsFalse()
        {
            Assert.IsFalse(TestStruct.Equals(null));
        }

        [Test]
        public void Equals_TestStructObject_IsFalse()
        {
            Assert.IsFalse(TestStruct.Equals(new object()));
        }

        [Test]
        public void OperatorIs_TestStructTestStruct_IsTrue()
        {
            var l = TestStruct;
            var r = TestStruct;
            Assert.IsTrue(l == r);
        }

        [Test]
        public void OperatorIsNot_TestStructTestStruct_IsFalse()
        {
            var l = TestStruct;
            var r = TestStruct;
            Assert.IsFalse(l != r);
        }

        /// <summary>Orders a list of fractions ascending.</summary>
        [Test]
        public void OrderBy_Fraction_AreEqual()
        {
            var item0 = Fraction.Parse("ComplexRegexPatternA");
            var item1 = Fraction.Parse("ComplexRegexPatternB");
            var item2 = Fraction.Parse("ComplexRegexPatternC");
            var item3 = Fraction.Parse("ComplexRegexPatternD");
            var inp = new List<Fraction> { Fraction.Zero, item3, item2, item0, item1, Fraction.Zero };
            var exp = new List<Fraction> { Fraction.Zero, Fraction.Zero, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();
            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of fractions descending.</summary>
        [Test]
        public void OrderByDescending_Fraction_AreEqual()
        {
            var item0 = Fraction.Parse("ComplexRegexPatternA");
            var item1 = Fraction.Parse("ComplexRegexPatternB");
            var item2 = Fraction.Parse("ComplexRegexPatternC");
            var item3 = Fraction.Parse("ComplexRegexPatternD");
            var inp = new List<Fraction> { Fraction.Zero, item3, item2, item0, item1, Fraction.Zero };
            var exp = new List<Fraction> { item3, item2, item1, item0, Fraction.Zero, Fraction.Zero };
            var act = inp.OrderByDescending(item => item).ToList();
            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Compare with a to object casted instance should be fine.</summary>
        [Test]
        public void CompareTo_ObjectTestStruct_0()
        {
            Assert.AreEqual(0, TestStruct.CompareTo((object)TestStruct));
        }

        /// <summary>Compare with null should return 1.</summary>
        [Test]
        public void CompareTo_null_1()
        {
            object @null = null;
            Assert.AreEqual(1, TestStruct.CompareTo(@null));
        }

        /// <summary>Compare with a random object should throw an exception.</summary>
        [Test]
        public void CompareTo_newObject_Throw()
        {
            var x = Assert.Catch<ArgumentException>(() => TestStruct.CompareTo(new object()));
            Assert.AreEqual("Argument must be Fraction. (Parameter 'obj')", x.Message);
        }

        //[Test]
        //public void LessThan_17LT19_IsTrue()
        //{
        //    Fraction l = 17;
        //    Fraction r = 19;
        //    Assert.IsTrue(l < r);
        //}

        //[Test]
        //public void GreaterThan_21LT19_IsTrue()
        //{
        //    Fraction l = 21;
        //    Fraction r = 19;
        //    Assert.IsTrue(l > r);
        //}

        //[Test]
        //public void LessThanOrEqual_17LT19_IsTrue()
        //{
        //    Fraction l = 17;
        //    Fraction r = 19;
        //    Assert.IsTrue(l <= r);
        //}

        //[Test]
        //public void GreaterThanOrEqual_21LT19_IsTrue()
        //{
        //    Fraction l = 21;
        //    Fraction r = 19;
        //    Assert.IsTrue(l >= r);
        //}

        //[Test]
        //public void LessThanOrEqual_17LT17_IsTrue()
        //{
        //    Fraction l = 17;
        //    Fraction r = 17;
        //    Assert.IsTrue(l <= r);
        //}

        //[Test]
        //public void GreaterThanOrEqual_21LT21_IsTrue()
        //{
        //    Fraction l = 21;
        //    Fraction r = 21;
        //    Assert.IsTrue(l >= r);
        //}

        //[Test]
        //public void Explicit_Int32ToFraction_AreEqual()
        //{
        //    var exp = TestStruct;
        //    var act = (Fraction)123456789;
        //    Assert.AreEqual(exp, act);
        //}

        //[Test]
        //public void Explicit_FractionToInt32_AreEqual()
        //{
        //    var exp = 123456789;
        //    var act = (int)TestStruct;
        //    Assert.AreEqual(exp, act);
        //}

        [Test]
        public void ConverterExists_Fraction_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(Fraction));
        }

        [Test]
        public void CanNotConvertFromInt32_Fraction_IsTrue()
        {
            TypeConverterAssert.CanNotConvertFrom(typeof(Fraction), typeof(int));
        }

        [Test]
        public void CanNotConvertToInt32_Fraction_IsTrue()
        {
            TypeConverterAssert.CanNotConvertTo(typeof(Fraction), typeof(int));
        }

        [Test]
        public void CanConvertFromString_Fraction_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(Fraction));
        }

        [Test]
        public void CanConvertToString_Fraction_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(Fraction));
        }

        [Test]
        public void ConvertFrom_StringNull_FractionEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(Fraction.Zero, (string)null);
            }
        }

        [Test]
        public void ConvertFrom_StringEmpty_FractionEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(Fraction.Zero, string.Empty);
            }
        }

        [Test]
        public void ConvertFromString_StringValue_TestStruct()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(TestStruct, TestStruct.ToString());
            }
        }

        [Test]
        public void ConvertToString_TestStruct_StringValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertToStringEquals(TestStruct.ToString(), TestStruct);
            }
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("0xFF")]
        public void IsInvalid_String(string str)
        {
            Assert.IsFalse(Fraction.IsValid(str));
        }

        [TestCase("13/666")]
        public void IsValid_String(string str)
        {
            Assert.IsTrue(Fraction.IsValid(str));
        }
    }

    [Serializable]
    public class FractionSerializeObject
    {
        public int Id { get; set; }
        public Fraction Obj { get; set; }
        public DateTime Date { get; set; }
    }
}

