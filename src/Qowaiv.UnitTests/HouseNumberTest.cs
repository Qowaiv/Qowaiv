using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qowaiv.UnitTests.TestTools;
using Qowaiv.UnitTests.TestTools.Formatting;
using Qowaiv.UnitTests.TestTools.Globalization;
using Qowaiv.UnitTests.Json;
using Qowaiv;

namespace Qowaiv.UnitTests
{
    /// <summary>Tests the house number SVO.</summary>
    [TestClass]
    public class HouseNumberTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly HouseNumber TestStruct = 123456789L;

        #region house number const tests

        /// <summary>HouseNumber.Empty should be equal to the default of house number.</summary>
        [TestMethod]
        public void Empty_None_EqualsDefault()
        {
            Assert.AreEqual(default(HouseNumber), HouseNumber.Empty);
        }

        [TestMethod]
        public void MinValue_None_1()
        {
            Assert.AreEqual(HouseNumber.Create(1), HouseNumber.MinValue);
        }

        [TestMethod]
        public void MaxValue_None_999999999()
        {
            Assert.AreEqual(HouseNumber.Create(999999999), HouseNumber.MaxValue);
        }

        #endregion

        #region house number IsEmpty tests

        /// <summary>HouseNumber.IsEmpty() should be true for the default of house number.</summary>
        [TestMethod]
        public void IsEmpty_Default_IsTrue()
        {
            Assert.IsTrue(default(HouseNumber).IsEmpty());
        }
        /// <summary>HouseNumber.IsEmpty() should be false for HouseNumber.Unknown.</summary>
        [TestMethod]
        public void IsEmpty_Unknown_IsFalse()
        {
            Assert.IsFalse(HouseNumber.Unknown.IsEmpty());
        }
        /// <summary>HouseNumber.IsEmpty() should be false for the TestStruct.</summary>
        [TestMethod]
        public void IsEmpty_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmpty());
        }

        /// <summary>HouseNumber.IsUnknown() should be false for the default of house number.</summary>
        [TestMethod]
        public void IsUnknown_Default_IsFalse()
        {
            Assert.IsFalse(default(HouseNumber).IsUnknown());
        }
        /// <summary>HouseNumber.IsUnknown() should be true for HouseNumber.Unknown.</summary>
        [TestMethod]
        public void IsUnknown_Unknown_IsTrue()
        {
            Assert.IsTrue(HouseNumber.Unknown.IsUnknown());
        }
        /// <summary>HouseNumber.IsUnknown() should be false for the TestStruct.</summary>
        [TestMethod]
        public void IsUnknown_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsUnknown());
        }

        /// <summary>HouseNumber.IsEmptyOrUnknown() should be true for the default of house number.</summary>
        [TestMethod]
        public void IsEmptyOrUnknown_Default_IsFalse()
        {
            Assert.IsTrue(default(HouseNumber).IsEmptyOrUnknown());
        }
        /// <summary>HouseNumber.IsEmptyOrUnknown() should be true for HouseNumber.Unknown.</summary>
        [TestMethod]
        public void IsEmptyOrUnknown_Unknown_IsTrue()
        {
            Assert.IsTrue(HouseNumber.Unknown.IsEmptyOrUnknown());
        }
        /// <summary>HouseNumber.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
        [TestMethod]
        public void IsEmptyOrUnknown_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmptyOrUnknown());
        }

        #endregion

        #region TryParse tests

        /// <summary>TryParse null should be valid.</summary>
        [TestMethod]
        public void TyrParse_Null_IsValid()
        {
            HouseNumber val;

            string str = null;

            Assert.IsTrue(HouseNumber.TryParse(str, out val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [TestMethod]
        public void TyrParse_StringEmpty_IsValid()
        {
            HouseNumber val;

            string str = string.Empty;

            Assert.IsTrue(HouseNumber.TryParse(str, out val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse "?" should be valid and the result should be HouseNumber.Unknown.</summary>
        [TestMethod]
        public void TyrParse_Questionmark_IsValid()
        {
            HouseNumber val;

            string str = "?";

            Assert.IsTrue(HouseNumber.TryParse(str, out val), "Valid");
            Assert.IsTrue(val.IsUnknown(), "Value");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [TestMethod]
        public void TyrParse_StringValue_IsValid()
        {
            HouseNumber val;

            string str = "123";

            Assert.IsTrue(HouseNumber.TryParse(str, out val), "Valid");
            Assert.AreEqual(str, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [TestMethod]
        public void TyrParse_StringValue_IsNotValid()
        {
            HouseNumber val;

            string str = "string";

            Assert.IsFalse(HouseNumber.TryParse(str, out val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        [TestMethod]
        public void Parse_Unknown_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var act = HouseNumber.Parse("?");
                var exp = HouseNumber.Unknown;
                Assert.AreEqual(exp, act);
            }
        }

        [TestMethod]
        public void Parse_InvalidInput_ThrowsFormatException()
        {
            using (new CultureInfoScope("en-GB"))
            {
                ExceptionAssert.ExpectException<FormatException>
                (() =>
                {
                    HouseNumber.Parse("InvalidInput");
                },
                "Not a valid house number");
            }
        }

        [TestMethod]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = TestStruct;
                var act = HouseNumber.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [TestMethod]
        public void TryParse_InvalidInput_DefaultValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = default(HouseNumber);
                var act = HouseNumber.TryParse("InvalidInput");

                Assert.AreEqual(exp, act);
            }
        }

        #endregion

        #region TryCreate tests

        [TestMethod]
        public void TryCreate_Null_IsEmpty()
        {
            HouseNumber exp = HouseNumber.Empty;
            HouseNumber act;

            Assert.IsTrue(HouseNumber.TryCreate(null, out act));
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void TryCreate_Int32MinValue_IsEmpty()
        {
            HouseNumber exp = HouseNumber.Empty;
            HouseNumber act;

            Assert.IsFalse(HouseNumber.TryCreate(Int32.MinValue, out act));
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void TryCreate_Int32MinValue_AreEqual()
        {
            var exp = HouseNumber.Empty;
            var act = HouseNumber.TryCreate(Int32.MinValue);
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void TryCreate_Value_AreEqual()
        {
            var exp = TestStruct;
            var act = HouseNumber.TryCreate(123456789);
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void Create_Int32MinValue_ThrowsArgumentOutOfRangeException()
        {
            ExceptionAssert.ExpectArgumentOutOfRangeException
            (() =>
            {
                HouseNumber.Create(Int32.MinValue);
            },
            "val",
            "Not a valid house number");
        }

        #endregion

        #region (XML) (De)serialization tests

        [TestMethod]
        public void Constructor_SerializationInfoIsNull_ThrowsArgumentNullException()
        {
            ExceptionAssert.ExpectArgumentNullException
            (() =>
            {
                SerializationTest.DeserializeUsingConstructor<HouseNumber>(null, default(StreamingContext));
            },
            "info");
        }

        [TestMethod]
        public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
        {
            ExceptionAssert.ExpectException<SerializationException>
            (() =>
            {
                var info = new SerializationInfo(typeof(HouseNumber), new System.Runtime.Serialization.FormatterConverter());
                SerializationTest.DeserializeUsingConstructor<HouseNumber>(info, default(StreamingContext));
            });
        }

        [TestMethod]
        public void GetObjectData_Null_ThrowsArgumentNullException()
        {
            ExceptionAssert.ExpectArgumentNullException
            (() =>
            {
                ISerializable obj = TestStruct;
                obj.GetObjectData(null, default(StreamingContext));
            },
            "info");
        }

        [TestMethod]
        public void GetObjectData_SerializationInfo_AreEqual()
        {
            ISerializable obj = TestStruct;
            var info = new SerializationInfo(typeof(HouseNumber), new System.Runtime.Serialization.FormatterConverter());
            obj.GetObjectData(info, default(StreamingContext));

            Assert.AreEqual(123456789, info.GetInt32("Value"));
        }

        [TestMethod]
        public void SerializeDeserialize_TestStruct_AreEqual()
        {
            var input = HouseNumberTest.TestStruct;
            var exp = HouseNumberTest.TestStruct;
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void DataContractSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = HouseNumberTest.TestStruct;
            var exp = HouseNumberTest.TestStruct;
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void XmlSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = HouseNumberTest.TestStruct;
            var exp = HouseNumberTest.TestStruct;
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void SerializeDeserialize_HouseNumberSerializeObject_AreEqual()
        {
            var input = new HouseNumberSerializeObject()
            {
                Id = 17,
                Obj = HouseNumberTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new HouseNumberSerializeObject()
            {
                Id = 17,
                Obj = HouseNumberTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [TestMethod]
        public void XmlSerializeDeserialize_HouseNumberSerializeObject_AreEqual()
        {
            var input = new HouseNumberSerializeObject()
            {
                Id = 17,
                Obj = HouseNumberTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new HouseNumberSerializeObject()
            {
                Id = 17,
                Obj = HouseNumberTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [TestMethod]
        public void DataContractSerializeDeserialize_HouseNumberSerializeObject_AreEqual()
        {
            var input = new HouseNumberSerializeObject()
            {
                Id = 17,
                Obj = HouseNumberTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new HouseNumberSerializeObject()
            {
                Id = 17,
                Obj = HouseNumberTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [TestMethod]
        public void SerializeDeserialize_Empty_AreEqual()
        {
            var input = new HouseNumberSerializeObject()
            {
                Id = 17,
                Obj = HouseNumberTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new HouseNumberSerializeObject()
            {
                Id = 17,
                Obj = HouseNumberTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [TestMethod]
        public void XmlSerializeDeserialize_Empty_AreEqual()
        {
            var input = new HouseNumberSerializeObject()
            {
                Id = 17,
                Obj = HouseNumber.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new HouseNumberSerializeObject()
            {
                Id = 17,
                Obj = HouseNumber.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [TestMethod]
        public void GetSchema_None_IsNull()
        {
            IXmlSerializable obj = TestStruct;
            Assert.IsNull(obj.GetSchema());
        }

        #endregion

        #region JSON (De)serialization tests

        [TestMethod]
        public void FromJson_Null_AreEqual()
        {
            var act = JsonTester.Read<HouseNumber>();
            var exp = HouseNumber.Empty;

            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void FromJson_InvalidStringValue_AssertFormatException()
        {
            ExceptionAssert.ExpectException<FormatException>(() =>
            {
                JsonTester.Read<HouseNumber>("InvalidStringValue");
            },
            "Not a valid house number");
        }
        [TestMethod]
        public void FromJson_StringValue_AreEqual()
        {
            var act = JsonTester.Read<HouseNumber>(TestStruct.ToString(CultureInfo.InvariantCulture));
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void FromJson_Int64Value_AreEqual()
        {
            var act = JsonTester.Read<HouseNumber>((Int64)TestStruct);
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void FromJson_DoubleValue_AreEqual()
        {
            var act = JsonTester.Read<HouseNumber>((Double)TestStruct);
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void FromJson_DateTimeValue_AssertNotSupportedException()
        {
            ExceptionAssert.ExpectException<NotSupportedException>(() =>
            {
                JsonTester.Read<HouseNumber>(new DateTime(1972, 02, 14));
            },
            "JSON deserialization from a date is not supported.");
        }

        [TestMethod]
        public void ToJson_DefaultValue_AreEqual()
        {
            object act = JsonTester.Write(default(HouseNumber));
            object exp = null;

            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void ToJson_TestStruct_AreEqual()
        {
            var act = JsonTester.Write(TestStruct);
            var exp = TestStruct.ToString(CultureInfo.InvariantCulture);

            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IFormattable / ToString tests

        [TestMethod]
        public void ToString_Empty_IsStringEmpty()
        {
            var act = HouseNumber.Empty.ToString();
            var exp = "";
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void ToString_Unknown_QuestionMark()
        {
            var act = HouseNumber.Unknown.ToString();
            var exp = "?";
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: '123456789', format: 'Unit Test Format'";

            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void ToString_TestStruct_ComplexPattern()
        {
            var act = TestStruct.ToString("");
            var exp = "123456789";
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void ToString_FormatValueDutchBelgium_AreEqual()
        {
            using (new CultureInfoScope("nl-BE"))
            {
                var act = HouseNumber.Parse("800").ToString("0000");
                var exp = "0800";
                Assert.AreEqual(exp, act);
            }
        }

        [TestMethod]
        public void ToString_FormatValueEnglishGreatBritain_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var act = HouseNumber.Parse("800").ToString("0000");
                var exp = "0800";
                Assert.AreEqual(exp, act);
            }
        }

        [TestMethod]
        public void ToString_FormatValueSpanishEcuador_AreEqual()
        {
            var act = HouseNumber.Parse("1700").ToString("00000.0", new CultureInfo("es-EC"));
            var exp = "01700,0";
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void DebuggerDisplay_DebugToString_HasAttribute()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(HouseNumber));
        }

        [TestMethod]
        public void DebugToString_DefaultValue_String()
        {
            DebuggerDisplayAssert.HasResult("HouseNumber: (empty)", default(HouseNumber));
        }
        [TestMethod]
        public void DebugToString_Unknown_String()
        {
            DebuggerDisplayAssert.HasResult("HouseNumber: (unknown)", HouseNumber.Unknown);
        }

        [TestMethod]
        public void DebugToString_TestStruct_String()
        {
            DebuggerDisplayAssert.HasResult("HouseNumber: 123456789", TestStruct);
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for HouseNumber.Empty.</summary>
        [TestMethod]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, HouseNumber.Empty.GetHashCode());
        }

        /// <summary>GetHash should not fail for the test struct.</summary>
        [TestMethod]
        public void GetHash_TestStruct_Hash()
        {
            Assert.AreEqual(123456789, HouseNumberTest.TestStruct.GetHashCode());
        }

        [TestMethod]
        public void Equals_EmptyEmpty_IsTrue()
        {
            Assert.IsTrue(HouseNumber.Empty.Equals(HouseNumber.Empty));
        }

        [TestMethod]
        public void Equals_TestStructTestStruct_IsTrue()
        {
            Assert.IsTrue(HouseNumberTest.TestStruct.Equals(HouseNumberTest.TestStruct));
        }

        [TestMethod]
        public void Equals_TestStructEmpty_IsFalse()
        {
            Assert.IsFalse(HouseNumberTest.TestStruct.Equals(HouseNumber.Empty));
        }

        [TestMethod]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(HouseNumber.Empty.Equals(HouseNumberTest.TestStruct));
        }

        [TestMethod]
        public void Equals_TestStructObjectTestStruct_IsTrue()
        {
            Assert.IsTrue(HouseNumberTest.TestStruct.Equals((object)HouseNumberTest.TestStruct));
        }

        [TestMethod]
        public void Equals_TestStructNull_IsFalse()
        {
            Assert.IsFalse(HouseNumberTest.TestStruct.Equals(null));
        }

        [TestMethod]
        public void Equals_TestStructObject_IsFalse()
        {
            Assert.IsFalse(HouseNumberTest.TestStruct.Equals(new object()));
        }

        [TestMethod]
        public void OperatorIs_TestStructTestStruct_IsTrue()
        {
            var l = HouseNumberTest.TestStruct;
            var r = HouseNumberTest.TestStruct;
            Assert.IsTrue(l == r);
        }

        [TestMethod]
        public void OperatorIsNot_TestStructTestStruct_IsFalse()
        {
            var l = HouseNumberTest.TestStruct;
            var r = HouseNumberTest.TestStruct;
            Assert.IsFalse(l != r);
        }

        #endregion

        #region IComparable tests

        /// <summary>Orders a list of house numbers ascending.</summary>
        [TestMethod]
        public void OrderBy_HouseNumber_AreEqual()
        {
            HouseNumber item0 = 1;
            HouseNumber item1 = 12;
            HouseNumber item2 = 123;
            HouseNumber item3 = 1234;

            var inp = new List<HouseNumber>() { HouseNumber.Empty, item3, item2, item0, item1, HouseNumber.Empty };
            var exp = new List<HouseNumber>() { HouseNumber.Empty, HouseNumber.Empty, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of house numbers descending.</summary>
        [TestMethod]
        public void OrderByDescending_HouseNumber_AreEqual()
        {
            HouseNumber item0 = 1;
            HouseNumber item1 = 12;
            HouseNumber item2 = 123;
            HouseNumber item3 = 1234;

            var inp = new List<HouseNumber>() { HouseNumber.Empty, item3, item2, item0, item1, HouseNumber.Empty };
            var exp = new List<HouseNumber>() { item3, item2, item1, item0, HouseNumber.Empty, HouseNumber.Empty };
            var act = inp.OrderByDescending(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Compare with a to object casted instance should be fine.</summary>
        [TestMethod]
        public void CompareTo_ObjectTestStruct_0()
        {
            object other = (object)TestStruct;

            var exp = 0;
            var act = TestStruct.CompareTo(other);

            Assert.AreEqual(exp, act);
        }

        /// <summary>Compare with null should throw an expception.</summary>
        [TestMethod]
        public void CompareTo_null_ThrowsArgumentException()
        {
            ExceptionAssert.ExpectArgumentException
            (() =>
                {
                    object other = null;
                    var act = TestStruct.CompareTo(other);
                },
                "obj",
                "Argument must be a house number"
            );
        }
        /// <summary>Compare with a random object should throw an expception.</summary>
        [TestMethod]
        public void CompareTo_newObject_ThrowsArgumentException()
        {
            ExceptionAssert.ExpectArgumentException
            (() =>
                {
                    object other = new object();
                    var act = TestStruct.CompareTo(other);
                },
                "obj",
                "Argument must be a house number"
            );
        }

        [TestMethod]
        public void LessThan_17LT19_IsTrue()
        {
            HouseNumber l = 17;
            HouseNumber r = 19;

            Assert.IsTrue(l < r);
        }
        [TestMethod]
        public void GreaterThan_21LT19_IsTrue()
        {
            HouseNumber l = 21;
            HouseNumber r = 19;

            Assert.IsTrue(l > r);
        }

        [TestMethod]
        public void LessThanOrEqual_17LT19_IsTrue()
        {
            HouseNumber l = 17;
            HouseNumber r = 19;

            Assert.IsTrue(l <= r);
        }
        [TestMethod]
        public void GreaterThanOrEqual_21LT19_IsTrue()
        {
            HouseNumber l = 21;
            HouseNumber r = 19;

            Assert.IsTrue(l >= r);
        }

        [TestMethod]
        public void LessThanOrEqual_17LT17_IsTrue()
        {
            HouseNumber l = 17;
            HouseNumber r = 17;

            Assert.IsTrue(l <= r);
        }
        [TestMethod]
        public void GreaterThanOrEqual_21LT21_IsTrue()
        {
            HouseNumber l = 21;
            HouseNumber r = 21;

            Assert.IsTrue(l >= r);
        }
        #endregion

        #region Casting tests

        [TestMethod]
        public void Explicit_StringToHouseNumber_AreEqual()
        {
            var exp = TestStruct;
            var act = (HouseNumber)TestStruct.ToString();

            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void Explicit_HouseNumberToString_AreEqual()
        {
            var exp = TestStruct.ToString();
            var act = (string)TestStruct;

            Assert.AreEqual(exp, act);
        }


        [TestMethod]
        public void Explicit_Int32ToHouseNumber_AreEqual()
        {
            var exp = TestStruct;
            var act = (HouseNumber)123456789;

            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void Explicit_HouseNumberToInt32_AreEqual()
        {
            var exp = 123456789;
            var act = (Int32)TestStruct;

            Assert.AreEqual(exp, act);
        }
        #endregion

        #region Properties

        [TestMethod]
        public void IsOdd_Empty_IsFalse()
        {
            Assert.IsFalse(HouseNumber.Empty.IsOdd);
        }

        [TestMethod]
        public void IsOdd_Unknown_IsFalse()
        {
            Assert.IsFalse(HouseNumber.Unknown.IsOdd);
        }

        [TestMethod]
        public void IsOdd_TestStruct_IsTrue()
        {
            Assert.IsTrue(TestStruct.IsOdd);
        }

        [TestMethod]
        public void IsEven_Empty_IsFalse()
        {
            Assert.IsFalse(HouseNumber.Empty.IsEven);
        }

        [TestMethod]
        public void IsEven_Unknown_IsFalse()
        {
            Assert.IsFalse(HouseNumber.Unknown.IsEven);
        }

        [TestMethod]
        public void IsEven_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEven);
        }

        [TestMethod]
        public void IsEven_1234_IsTrue()
        {
            Assert.IsTrue(HouseNumber.Create(1234).IsEven);
        }


        [TestMethod]
        public void Length_Empty_0()
        {
            var act = HouseNumber.Empty.Length;
            var exp = 0;
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void Length_Unknown_0()
        {
            var act = HouseNumber.Unknown.Length;
            var exp = 0;
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void Length_TestStruct_9()
        {
            var act = TestStruct.Length;
            var exp = 9;
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void Length_1234_4()
        {
            var act = HouseNumber.Create(1234).Length;
            var exp = 4;
            Assert.AreEqual(exp, act);
        }
        #endregion

        #region Type converter tests

        [TestMethod]
        public void ConverterExists_HouseNumber_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(HouseNumber));
        }

        [TestMethod]
        public void CanNotConvertFromInt32_HouseNumber_IsTrue()
        {
            TypeConverterAssert.CanNotConvertFrom(typeof(HouseNumber), typeof(Int32));
        }
        [TestMethod]
        public void CanNotConvertToInt32_HouseNumber_IsTrue()
        {
            TypeConverterAssert.CanNotConvertTo(typeof(HouseNumber), typeof(Int32));
        }

        [TestMethod]
        public void CanConvertFromString_HouseNumber_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(HouseNumber));
        }

        [TestMethod]
        public void CanConvertToString_HouseNumber_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(HouseNumber));
        }

        [TestMethod]
        public void ConvertFrom_StringNull_HouseNumberEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(HouseNumber.Empty, (string)null);
            }
        }

        [TestMethod]
        public void ConvertFrom_StringEmpty_HouseNumberEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(HouseNumber.Empty, string.Empty);
            }
        }

        [TestMethod]
        public void ConvertFromString_StringValue_TestStruct()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(HouseNumberTest.TestStruct, HouseNumberTest.TestStruct.ToString());
            }
        }

        [TestMethod]
        public void ConvertToString_TestStruct_StringValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertToStringEquals(HouseNumberTest.TestStruct.ToString(), HouseNumberTest.TestStruct);
            }
        }

        #endregion

        #region IsValid tests

        [TestMethod]
        public void IsValid_Data_IsFalse()
        {
            Assert.IsFalse(HouseNumber.IsValid("1234567890"), "1234567890");
            Assert.IsFalse(HouseNumber.IsValid((String)null), "(String)null");
            Assert.IsFalse(HouseNumber.IsValid(String.Empty), "String.Empty");

            Assert.IsFalse(HouseNumber.IsValid((System.Int32?)null), "(System.Int32?)null");
        }
        [TestMethod]
        public void IsValid_Data_IsTrue()
        {
            Assert.IsTrue(HouseNumber.IsValid("123456"));
        }
        #endregion
    }

    [Serializable]
    public class HouseNumberSerializeObject
    {
        public int Id { get; set; }
        public HouseNumber Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
