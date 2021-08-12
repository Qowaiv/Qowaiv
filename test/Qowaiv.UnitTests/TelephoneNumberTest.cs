using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.TestTools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests
{
    /// <summary>Tests the telephone number SVO.</summary>
    public class TelephoneNumberTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly TelephoneNumber TestStruct = TelephoneNumber.Parse("+31 (0)123.456.789");

        /// <summary><see cref = "TelephoneNumber.Empty"/> should be equal to the default of telephone number.</summary>
        [Test]
        public void Empty_EqualsDefault()
        {
            Assert.AreEqual(default(TelephoneNumber), TelephoneNumber.Empty);
        }

        /// <summary>TelephoneNumber.IsEmpty() should be true for the default of telephone number.</summary>
        [Test]
        public void IsEmpty_Default_IsTrue()
        {
            Assert.IsTrue(default(TelephoneNumber).IsEmpty());
        }

        /// <summary>TelephoneNumber.IsEmpty() should be false for TelephoneNumber.Unknown.</summary>
        [Test]
        public void IsEmpty_Unknown_IsFalse()
        {
            Assert.IsFalse(TelephoneNumber.Unknown.IsEmpty());
        }

        /// <summary>TelephoneNumber.IsEmpty() should be false for the TestStruct.</summary>
        [Test]
        public void IsEmpty_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmpty());
        }

        /// <summary>TelephoneNumber.IsUnknown() should be false for the default of telephone number.</summary>
        [Test]
        public void IsUnknown_Default_IsFalse()
        {
            Assert.IsFalse(default(TelephoneNumber).IsUnknown());
        }

        /// <summary>TelephoneNumber.IsUnknown() should be true for TelephoneNumber.Unknown.</summary>
        [Test]
        public void IsUnknown_Unknown_IsTrue()
        {
            Assert.IsTrue(TelephoneNumber.Unknown.IsUnknown());
        }

        /// <summary>TelephoneNumber.IsUnknown() should be false for the TestStruct.</summary>
        [Test]
        public void IsUnknown_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsUnknown());
        }

        /// <summary>TelephoneNumber.IsEmptyOrUnknown() should be true for the default of telephone number.</summary>
        [Test]
        public void IsEmptyOrUnknown_Default_IsFalse()
        {
            Assert.IsTrue(default(TelephoneNumber).IsEmptyOrUnknown());
        }

        /// <summary>TelephoneNumber.IsEmptyOrUnknown() should be true for TelephoneNumber.Unknown.</summary>
        [Test]
        public void IsEmptyOrUnknown_Unknown_IsTrue()
        {
            Assert.IsTrue(TelephoneNumber.Unknown.IsEmptyOrUnknown());
        }

        /// <summary>TelephoneNumber.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
        [Test]
        public void IsEmptyOrUnknown_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmptyOrUnknown());
        }

        /// <summary>TryParse null should be valid.</summary>
        [Test]
        public void TryParse_Null_IsValid()
        {
            Assert.IsTrue(TelephoneNumber.TryParse(null, out var val));
            Assert.AreEqual(default(TelephoneNumber), val);
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TryParse_StringEmpty_IsValid()
        {
            Assert.IsTrue(TelephoneNumber.TryParse(string.Empty, out var val));
            Assert.AreEqual(default(TelephoneNumber), val);
        }

        /// <summary>TryParse "?" should be valid and the result should be TelephoneNumber.Unknown.</summary>
        [Test]
        public void TryParse_Questionmark_IsUnkown()
        {
            string str = "?";
            Assert.IsTrue(TelephoneNumber.TryParse(str, out var val));
            Assert.IsTrue(val.IsUnknown(), "Should be unknown");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TryParse_StringValue_IsValid()
        {
            string str = "+31123456789";
            Assert.IsTrue(TelephoneNumber.TryParse(str, out var val));
            Assert.AreEqual(str, val.ToString());
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [Test]
        public void TryParse_StringValue_IsNotValid()
        {
            string str = "invalid";
            Assert.IsFalse(TelephoneNumber.TryParse(str, out var val));
            Assert.AreEqual(default(TelephoneNumber), val);
        }

        [Test]
        public void Parse_Unknown_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var act = TelephoneNumber.Parse("?");
                var exp = TelephoneNumber.Unknown;
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void Parse_InvalidInput_ThrowsFormatException()
        {
            using (new CultureInfoScope("en-GB"))
            {
                Assert.Catch<FormatException>(() =>
                {
                    TelephoneNumber.Parse("InvalidInput");
                }

                , "Not a valid telephone number");
            }
        }

        [Test]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = TestStruct;
                var act = TelephoneNumber.TryParse(exp.ToString());
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void TryParse_InvalidInput_DefaultValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = default(TelephoneNumber);
                var act = TelephoneNumber.TryParse("InvalidInput");
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void Constructor_SerializationInfoIsNull_Throws()
        {
            Assert.Catch<ArgumentNullException>(() => SerializationTest.DeserializeUsingConstructor<TelephoneNumber>(null, default));
        }

        [Test]
        public void Constructor_InvalidSerializationInfo_Throws()
        {
            var info = new SerializationInfo(typeof(TelephoneNumber), new FormatterConverter());
            Assert.Catch<SerializationException>(() => SerializationTest.DeserializeUsingConstructor<TelephoneNumber>(info, default));
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
            var info = new SerializationInfo(typeof(TelephoneNumber), new FormatterConverter());
            obj.GetObjectData(info, default);
            Assert.AreEqual("+31123456789", info.GetValue("Value", typeof(string)));
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
            var exp = "+31123456789";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void XmlDeserialize_XmlString_AreEqual()
        {
            var act = SerializationTest.XmlDeserialize<TelephoneNumber>("+31 123456789");
            Assert.AreEqual(TestStruct, act);
        }

        [Test]
        public void SerializeDeserialize_TelephoneNumberSerializeObject_AreEqual()
        {
            var input = new TelephoneNumberSerializeObject { Id = 17, Obj = TestStruct, Date = new DateTime(1970, 02, 14), };
            var exp = new TelephoneNumberSerializeObject { Id = 17, Obj = TestStruct, Date = new DateTime(1970, 02, 14), };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void XmlSerializeDeserialize_TelephoneNumberSerializeObject_AreEqual()
        {
            var input = new TelephoneNumberSerializeObject { Id = 17, Obj = TestStruct, Date = new DateTime(1970, 02, 14), };
            var exp = new TelephoneNumberSerializeObject { Id = 17, Obj = TestStruct, Date = new DateTime(1970, 02, 14), };
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void DataContractSerializeDeserialize_TelephoneNumberSerializeObject_AreEqual()
        {
            var input = new TelephoneNumberSerializeObject { Id = 17, Obj = TestStruct, Date = new DateTime(1970, 02, 14), };
            var exp = new TelephoneNumberSerializeObject { Id = 17, Obj = TestStruct, Date = new DateTime(1970, 02, 14), };
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void SerializeDeserialize_Default_AreEqual()
        {
            var input = new TelephoneNumberSerializeObject { Id = 17, Obj = default, Date = new DateTime(1970, 02, 14), };
            var exp = new TelephoneNumberSerializeObject { Id = 17, Obj = default, Date = new DateTime(1970, 02, 14), };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void XmlSerializeDeserialize_Default_AreEqual()
        {
            var input = new TelephoneNumberSerializeObject { Id = 17, Obj = default, Date = new DateTime(1970, 02, 14), };
            var exp = new TelephoneNumberSerializeObject { Id = 17, Obj = default, Date = new DateTime(1970, 02, 14), };
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
        public void FromJson_Invalid_Throws(object json)
        {
            Assert.Catch<FormatException>(() => JsonTester.Read<TelephoneNumber>(json));
        }

        [TestCase("+42123456798", "+42123456798")]
        public void FromJson(TelephoneNumber expected, object json)
        {
            var actual = JsonTester.Read<TelephoneNumber>(json);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToJson_TestStruct_JsonString()
        {
            var act = JsonTester.Write(TestStruct);
            var exp = "+31123456789";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_Empty_StringEmpty()
        {
            var act = TelephoneNumber.Empty.ToString();
            var exp = "";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_Unknown_QuestionMark()
        {
            var act = TelephoneNumber.Unknown.ToString();
            var exp = "?";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: '+31123456789', format: 'Unit Test Format'";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void DebuggerDisplay_DebugToString_HasAttribute()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(TelephoneNumber));
        }

        [Test]
        public void DebuggerDisplay_DefaultValue_String()
        {
            DebuggerDisplayAssert.HasResult("", default(TelephoneNumber));
        }

        [Test]
        public void DebuggerDisplay_Unknown_String()
        {
            DebuggerDisplayAssert.HasResult("?", TelephoneNumber.Unknown);
        }

        [Test]
        public void DebuggerDisplay_TestStruct_String()
        {
            DebuggerDisplayAssert.HasResult("+31123456789", TestStruct);
        }

        /// <summary>GetHash should not fail for TelephoneNumber.Empty.</summary>
        [Test]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, TelephoneNumber.Empty.GetHashCode());
        }

        /// <summary>GetHash should not fail for the test struct.</summary>
        [Test]
        public void GetHash_TestStruct_Hash()
        {
            Assert.AreNotEqual(0, TestStruct.GetHashCode());
        }

        [Test]
        public void Equals_EmptyEmpty_IsTrue()
        {
            Assert.IsTrue(TelephoneNumber.Empty.Equals(TelephoneNumber.Empty));
        }

        [Test]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = TelephoneNumber.Parse("0031 (0)123.456.789", CultureInfo.InvariantCulture);
            var r = TelephoneNumber.Parse("+31123456789", CultureInfo.InvariantCulture);
            Assert.IsTrue(l.Equals(r));
        }

        [Test]
        public void Equals_TestStructTestStruct_IsTrue()
        {
            Assert.IsTrue(TestStruct.Equals(TestStruct));
        }

        [Test]
        public void Equals_TestStructEmpty_IsFalse()
        {
            Assert.IsFalse(TestStruct.Equals(TelephoneNumber.Empty));
        }

        [Test]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(TelephoneNumber.Empty.Equals(TestStruct));
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

        /// <summary>Orders a list of telephone numbers ascending.</summary>
        [Test]
        public void OrderBy_TelephoneNumber_AreEqual()
        {
            var item0 = TelephoneNumber.Parse("+31 465789");
            var item1 = TelephoneNumber.Parse("+42 465789");
            var item2 = TelephoneNumber.Parse("+42 965789");
            var item3 = TelephoneNumber.Parse("012-465789");
            var inp = new List<TelephoneNumber> { TelephoneNumber.Empty, item3, item2, item0, item1, TelephoneNumber.Empty };
            var exp = new List<TelephoneNumber> { TelephoneNumber.Empty, TelephoneNumber.Empty, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();
            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of telephone numbers descending.</summary>
        [Test]
        public void OrderByDescending_TelephoneNumber_AreEqual()
        {
            var item0 = TelephoneNumber.Parse("+31 465789");
            var item1 = TelephoneNumber.Parse("+42 465789");
            var item2 = TelephoneNumber.Parse("+42 965789");
            var item3 = TelephoneNumber.Parse("012-465789");
            var inp = new List<TelephoneNumber> { TelephoneNumber.Empty, item3, item2, item0, item1, TelephoneNumber.Empty };
            var exp = new List<TelephoneNumber> { item3, item2, item1, item0, TelephoneNumber.Empty, TelephoneNumber.Empty };
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
            Assert.AreEqual("Argument must be TelephoneNumber. (Parameter 'obj')", x.Message);
        }

        //[Test]
        //public void Explicit_StringToTelephoneNumber_AreEqual()
        //{
        //    var exp = TestStruct;
        //    var act = (TelephoneNumber)TestStruct.ToString();
        //    Assert.AreEqual(exp, act);
        //}

        //[Test]
        //public void Explicit_TelephoneNumberToString_AreEqual()
        //{
        //    var exp = TestStruct.ToString();
        //    var act = (string)TestStruct;
        //    Assert.AreEqual(exp, act);
        //}

        [Test]
        public void Length_DefaultValue_0()
        {
            var exp = 0;
            var act = TelephoneNumber.Empty.Length;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Length_TestStruct_IntValue()
        {
            var exp = 12;
            var act = TestStruct.Length;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ConverterExists_TelephoneNumber_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(TelephoneNumber));
        }

        [Test]
        public void CanNotConvertFromInt32_TelephoneNumber_IsTrue()
        {
            TypeConverterAssert.CanNotConvertFrom(typeof(TelephoneNumber), typeof(int));
        }

        [Test]
        public void CanNotConvertToInt32_TelephoneNumber_IsTrue()
        {
            TypeConverterAssert.CanNotConvertTo(typeof(TelephoneNumber), typeof(int));
        }

        [Test]
        public void CanConvertFromString_TelephoneNumber_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(TelephoneNumber));
        }

        [Test]
        public void CanConvertToString_TelephoneNumber_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(TelephoneNumber));
        }

        [Test]
        public void ConvertFrom_StringNull_TelephoneNumberEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(TelephoneNumber.Empty, (string)null);
            }
        }

        [Test]
        public void ConvertFrom_StringEmpty_TelephoneNumberEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(TelephoneNumber.Empty, string.Empty);
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
        [TestCase("(0)1324657")]
        [TestCase("23")]
        public void IsInvalid_String(string str)
        {
            Assert.IsFalse(TelephoneNumber.IsValid(str));
        }

        [TestCase("112")]
        [TestCase("015-2624424")]
        public void IsValid_String(string str)
        {
            Assert.IsTrue(TelephoneNumber.IsValid(str));
        }

        [Test]
        public void IsValid_DefaultValue_IsFalse()
        {
            string? value = default;
            Assert.IsFalse(TelephoneNumber.IsValid(value));
        }
    }

    [Serializable]
    public class TelephoneNumberSerializeObject
    {
        public int Id{get;set;}
        public TelephoneNumber Obj{get;set;}
        public DateTime Date{get;set;}
    }
}
