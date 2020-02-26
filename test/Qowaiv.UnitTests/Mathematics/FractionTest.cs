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
        [TestCase(-12, 1, "-12")]
        [TestCase(12_345, 1, "12,345")]
        [TestCase(-1, 4, "-0.25")]
        [TestCase(487, 1000, "48.70%")]
        [TestCase(487, 1000, "487.0‰")]
        [TestCase(1, 3, "1/3")]
        [TestCase(1, 3, "1:3")]
        [TestCase(1, 3, "1÷3")]
        [TestCase(1, 3, "1⁄3")]
        [TestCase(1, 3, "1⁄3")]
        [TestCase(1, 3, "1⁄3")]
        [TestCase(1, 3, "1∕3")]
        [TestCase(1, 3, "+1/3")]
        [TestCase(-1, 3, "-1/3")]
        [TestCase(11, 43, "11/43")]
        [TestCase(4, 3, "1 1/3")]
        [TestCase(21, 2, "10 1/2")]
        [TestCase(1, 2, "½")]
        [TestCase(-1, 2, "-½")]
        [TestCase(3, 4, "¾")]
        [TestCase(11, 4, "2¾")]
        [TestCase(11, 4, "2 ¾")]
        [TestCase(9, 7, "1²/₇")]
        [TestCase(9, 7, "1 ²/₇")]
        [TestCase(23, 47, "²³/₄₇")]
        [TestCase(3, 7, "3/₇")]
        [TestCase(3, 7, "³/7")]
        public void Parse(long numerator, long denominator, string str)
        {
            var expected = numerator.DividedBy(denominator);
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

        [TestCase(0, 1, "0")]
        [TestCase(00000003, 000000010, "0.3")]
        [TestCase(00000033, 000000100, "0.33")]
        [TestCase(00000333, 000001000, "0.333")]
        [TestCase(00003333, 000010000, "0.3333")]
        [TestCase(00033333, 000100000, "0.33333")]
        [TestCase(00333333, 001000000, "0.333333")]
        [TestCase(03333333, 010000000, "0.3333333")]
        [TestCase(33333333, 100000000, "0.33333333")]
        [TestCase(333333333, 1000000000, "0.333333333")]
        [TestCase(1, 3, "0.33333333333333333333333")]
        public void Create(long numerator, long denominator, decimal number)
        {
            var actual = Fraction.Create(number);
            var expected = numerator.DividedBy(denominator);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Create_Roundtrip()
        {
            var rnd = new Random();

            var failures = new List<Fraction>();

            for (var i = 0; i < 100; i++)
            {
                var expected = rnd.Next(1, int.MaxValue).DividedBy(rnd.Next(3, int.MaxValue));
                var actual = Fraction.Create((decimal)expected);

                if (actual != expected)
                {
                    failures.Add(expected);
                }
            }

            CollectionAssert.AreEqual(Array.Empty<Fraction>(), failures);
        }

        [TestCase("0/1", 0, 8, "Should set zero")]
        [TestCase("1/4", 2, 8, "Should reduce")]
        [TestCase("-1/4", -2, 8, "Should reduce")]
        [TestCase("1/4", 3, 12, "Should reduce")]
        [TestCase("-1/4", -3, 12, "Should reduce")]
        [TestCase("3/7", -3, -7, "Should have no signs")]
        [TestCase("-3/7", 3, -7, "Should have no sign on denominator")]
        [TestCase("-3/7", -3, 7, "Should have no sign on denominator")]
        public void Constructor(Fraction expected, long numerator, long denominator, string description)
        {
            var actual = new Fraction(numerator, denominator);
            Assert.AreEqual(expected, actual, description);
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
            Assert.AreEqual(-69, info.GetInt64("numerator"));
            Assert.AreEqual(17, info.GetInt64("denominator"));
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
            var exp = "-69/17";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void XmlDeserialize_XmlString_AreEqual()
        {
            var act = SerializationTest.XmlDeserialize<Fraction>("-69/17");
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
        public void FromJson_Invalid_Throws(object json)
        {
            Assert.Catch<FormatException>(() => JsonTester.Read<Fraction>(json));
        }

        [TestCase(double.MaxValue)]
        [TestCase(double.MinValue)]
        public void FromJson_Invalid_Overflows(object json)
        {
            Assert.Catch<OverflowException>(() => JsonTester.Read<Fraction>(json));
        }

        [TestCase("4/1", 4L)]
        [TestCase("3/1", 3.0)]
        [TestCase("1/3", "14/42")]
        [TestCase("13/100", "13%")]
        public void FromJson(Fraction expected, object json)
        {
            var actual = JsonTester.Read<Fraction>(json);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_Zero_StringEmpty()
        {
            var act = Fraction.Zero.ToString();
            var exp = "0/1";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("[0] 0/000", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: '-4 1/017', format: '[0] 0/000'";
            Assert.AreEqual(exp, act);
        }

        [TestCase("-2:7", "-2/7", "0:0")]
        [TestCase("4÷3", "4/3", "0÷0")]
        [TestCase("1 1/3", "4/3", "[0]0/0")]
        [TestCase("-1 1/3", "-4/3", "[0]0/0")]
        [TestCase(".33", "1/3", "#.00")]
        [TestCase("5¹¹⁄₁₂", "71/12", "[0]super⁄sub")]
        [TestCase("5¹¹⁄12", "71/12", "[0]super⁄0")]
        [TestCase("5 11⁄₁₂", "71/12", "[0] 0⁄sub")]
        [TestCase("-3¹⁄₂", "-7/2", "[0]super⁄sub")]
        [TestCase("-¹⁄₂", "-1/2", "[#]super⁄sub")]
        [TestCase("-0¹⁄₂", "-1/2", "[0]super⁄sub")]
        [TestCase("⁷¹⁄₁₂", "71/12", "super⁄sub")]
        [TestCase("-⁷⁄₂", "-7/2", "super⁄sub")]
        public void ToString_WithFormat(string expected, Fraction fraction, string format)
        {
            var formatted = fraction.ToString(format, CultureInfo.InvariantCulture);
            Assert.AreEqual(expected, formatted);
        }

        [Test]
        public void DebuggerDisplay_DebugToString_HasAttribute()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(Fraction));
        }

        [Test]
        public void DebuggerDisplay_DefaultValue_String()
        {
            DebuggerDisplayAssert.HasResult("⁰⁄₁ = 0", default(Fraction));
        }

        [Test]
        public void DebuggerDisplay_TestStruct_String()
        {
            DebuggerDisplayAssert.HasResult("-⁶⁹⁄₁₇ = -4.05882353", TestStruct);
        }

        /// <summary>GetHash should not fail for Fraction.Zero.</summary>
        [Test]
        public void GetHash_Zero_Hash()
        {
            Assert.AreEqual(0, Fraction.Zero.GetHashCode());
        }

        /// <summary>GetHash should not fail for the test struct.</summary>
        [Test]
        public void GetHash_TestStruct_Hash()
        {
            Assert.AreEqual(132548, TestStruct.GetHashCode());
        }

        [Test]
        public void Equals_ZeroZero_IsTrue()
        {
            Assert.IsTrue(Fraction.Zero.Equals(Fraction.Zero));
        }

        [Test]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = Fraction.Parse("+71,234/-71,234", CultureInfo.InvariantCulture);
            var r = Fraction.Parse("-1", CultureInfo.InvariantCulture);
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
            var item0 = Fraction.Parse("3/40");
            var item1 = Fraction.Parse("1/10");
            var item2 = Fraction.Parse("7/30");
            var item3 = Fraction.Parse("7234/17");
            var inp = new List<Fraction> { Fraction.Zero, item3, item2, item0, item1, Fraction.Zero };
            var exp = new List<Fraction> { Fraction.Zero, Fraction.Zero, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();
            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of fractions descending.</summary>
        [Test]
        public void OrderByDescending_Fraction_AreEqual()
        {
            var item0 = Fraction.Parse("3/41");
            var item1 = Fraction.Parse("5/41");
            var item2 = Fraction.Parse("7/30");
            var item3 = Fraction.Parse("7234/17");
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

        [Test]
        public void LessThan_17LT19_IsTrue()
        {
            Fraction l = 17.DividedBy(2);
            Fraction r = 19.DividedBy(2);
            Assert.IsTrue(l < r);
        }

        [Test]
        public void GreaterThan_21LT19_IsTrue()
        {
            Fraction l = 21.DividedBy(2);
            Fraction r = 19.DividedBy(2);
            Assert.IsTrue(l > r);
        }

        [Test]
        public void LessThanOrEqual_17LT19_IsTrue()
        {
            Fraction l = 17.DividedBy(2);
            Fraction r = 19.DividedBy(2);
            Assert.IsTrue(l <= r);
        }

        [Test]
        public void GreaterThanOrEqual_21LT19_IsTrue()
        {
            Fraction l = 21.DividedBy(2);
            Fraction r = 19.DividedBy(2);
            Assert.IsTrue(l >= r);
        }

        [Test]
        public void LessThanOrEqual_17LT17_IsTrue()
        {
            Fraction l = 17.DividedBy(2);
            Fraction r = 17.DividedBy(2);
            Assert.IsTrue(l <= r);
        }

        [Test]
        public void GreaterThanOrEqual_21LT21_IsTrue()
        {
            Fraction l = 21.DividedBy(2);
            Fraction r = 21.DividedBy(2);
            Assert.IsTrue(l >= r);
        }

        [Test]
        public void Explicit_Int32ToFraction_AreEqual()
        {
            var exp = 123456789.DividedBy(1);
            var act = (Fraction)123456789;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Explicit_FractionToInt32_AreEqual()
        {
            Assert.AreEqual(-69 / 17, (int)TestStruct);
        }

        [Test]
        public void Explicit_FractionToInt64_AreEqual()
        {
            Assert.AreEqual(-69 / 17L, (long)TestStruct);
        }

        [Test]
        public void Explicit_FractionToDouble_AreEqual()
        {
            Assert.AreEqual(-69 / 17d, (double)TestStruct);
        }

        [Test]
        public void Explicit_FractionToDecimal_AreEqual()
        {
            Assert.AreEqual(-69 / 17m, (decimal)TestStruct);
        }

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

        [TestCase(null, "Null")]
        [TestCase("", "String.Empty")]
        [TestCase("NaN", "NaN")]
        [TestCase("-Infinity", "-Infinity")]
        [TestCase("+Infinity", "+Infinity")]
        [TestCase("0xFF", "Hexa-decimal")]
        [TestCase("15/", "Ends with an operator")]
        [TestCase("1//4", "Two division operators")]
        [TestCase("1/½", "Vulgar with division operator")]
        [TestCase("½1", "Vulgar not at the end")]
        [TestCase("²3/₇", "Normal and superscript mixed")]
        [TestCase("²/₇3", "Normal and subscript mixed")]
        [TestCase("²/3₇", "Normal and subscript mixed")]
        [TestCase("₇/3", "Subscript first")]
        [TestCase("9223372036854775808", "Long.MaxValue + 1")]
        [TestCase("-9223372036854775808", "Long.MinValue")]
        [TestCase("-9223372036854775809", "Long.MinValue - 1")]
        public void IsInvalid_String(string str, string message)
        {
            Assert.IsFalse(Fraction.IsValid(str), message);
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
