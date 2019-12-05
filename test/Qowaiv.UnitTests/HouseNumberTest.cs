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
    /// <summary>Tests the house number SVO.</summary>
    [TestFixture]
    public class HouseNumberTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly HouseNumber TestStruct = 123456789L;

        #region house number const tests

        /// <summary>HouseNumber.Empty should be equal to the default of house number.</summary>
        [Test]
        public void Empty_None_EqualsDefault()
        {
            Assert.AreEqual(default(HouseNumber), HouseNumber.Empty);
        }

        [Test]
        public void MinValue_None_1()
        {
            Assert.AreEqual(HouseNumber.Create(1), HouseNumber.MinValue);
        }

        [Test]
        public void MaxValue_None_999999999()
        {
            Assert.AreEqual(HouseNumber.Create(999999999), HouseNumber.MaxValue);
        }

        #endregion

        #region house number IsEmpty tests

        /// <summary>HouseNumber.IsEmpty() should be true for the default of house number.</summary>
        [Test]
        public void IsEmpty_Default_IsTrue()
        {
            Assert.IsTrue(default(HouseNumber).IsEmpty());
        }
        /// <summary>HouseNumber.IsEmpty() should be false for HouseNumber.Unknown.</summary>
        [Test]
        public void IsEmpty_Unknown_IsFalse()
        {
            Assert.IsFalse(HouseNumber.Unknown.IsEmpty());
        }
        /// <summary>HouseNumber.IsEmpty() should be false for the TestStruct.</summary>
        [Test]
        public void IsEmpty_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmpty());
        }

        /// <summary>HouseNumber.IsUnknown() should be false for the default of house number.</summary>
        [Test]
        public void IsUnknown_Default_IsFalse()
        {
            Assert.IsFalse(default(HouseNumber).IsUnknown());
        }
        /// <summary>HouseNumber.IsUnknown() should be true for HouseNumber.Unknown.</summary>
        [Test]
        public void IsUnknown_Unknown_IsTrue()
        {
            Assert.IsTrue(HouseNumber.Unknown.IsUnknown());
        }
        /// <summary>HouseNumber.IsUnknown() should be false for the TestStruct.</summary>
        [Test]
        public void IsUnknown_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsUnknown());
        }

        /// <summary>HouseNumber.IsEmptyOrUnknown() should be true for the default of house number.</summary>
        [Test]
        public void IsEmptyOrUnknown_Default_IsFalse()
        {
            Assert.IsTrue(default(HouseNumber).IsEmptyOrUnknown());
        }
        /// <summary>HouseNumber.IsEmptyOrUnknown() should be true for HouseNumber.Unknown.</summary>
        [Test]
        public void IsEmptyOrUnknown_Unknown_IsTrue()
        {
            Assert.IsTrue(HouseNumber.Unknown.IsEmptyOrUnknown());
        }
        /// <summary>HouseNumber.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
        [Test]
        public void IsEmptyOrUnknown_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmptyOrUnknown());
        }

        #endregion

        #region TryParse tests

        /// <summary>TryParse null should be valid.</summary>
        [Test]
        public void TyrParse_Null_IsValid()
        {
            string str = null;

            Assert.IsTrue(HouseNumber.TryParse(str, out HouseNumber val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TyrParse_StringEmpty_IsValid()
        {
            string str = string.Empty;

            Assert.IsTrue(HouseNumber.TryParse(str, out HouseNumber val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse "?" should be valid and the result should be HouseNumber.Unknown.</summary>
        [Test]
        public void TyrParse_Questionmark_IsValid()
        {
            string str = "?";

            Assert.IsTrue(HouseNumber.TryParse(str, out HouseNumber val), "Valid");
            Assert.IsTrue(val.IsUnknown(), "Value");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TyrParse_StringValue_IsValid()
        {
            string str = "123";

            Assert.IsTrue(HouseNumber.TryParse(str, out HouseNumber val), "Valid");
            Assert.AreEqual(str, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [Test]
        public void TyrParse_StringValue_IsNotValid()
        {
            string str = "string";

            Assert.IsFalse(HouseNumber.TryParse(str, out HouseNumber val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        [Test]
        public void Parse_Unknown_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var act = HouseNumber.Parse("?");
                var exp = HouseNumber.Unknown;
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void Parse_InvalidInput_ThrowsFormatException()
        {
            using (new CultureInfoScope("en-GB"))
            {
                Assert.Catch<FormatException>
                (() =>
                {
                    HouseNumber.Parse("InvalidInput");
                },
                "Not a valid house number");
            }
        }

        [Test]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = TestStruct;
                var act = HouseNumber.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
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

        [Test]
        public void TryCreate_Null_IsEmpty()
        {
            HouseNumber exp = HouseNumber.Empty;
            Assert.IsTrue(HouseNumber.TryCreate(null, out HouseNumber act));
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void TryCreate_Int32MinValue_IsEmpty()
        {
            HouseNumber exp = HouseNumber.Empty;
            Assert.IsFalse(HouseNumber.TryCreate(Int32.MinValue, out HouseNumber act));
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void TryCreate_Int32MinValue_AreEqual()
        {
            var exp = HouseNumber.Empty;
            var act = HouseNumber.TryCreate(Int32.MinValue);
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void TryCreate_Value_AreEqual()
        {
            var exp = TestStruct;
            var act = HouseNumber.TryCreate(123456789);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Create_Int32MinValue_ThrowsArgumentOutOfRangeException()
        {
            ExceptionAssert.CatchArgumentOutOfRangeException
            (() =>
            {
                HouseNumber.Create(Int32.MinValue);
            },
            "val",
            "Not a valid house number");
        }

        #endregion

        #region (XML) (De)serialization tests

        [Test]
        public void Constructor_SerializationInfoIsNull_ThrowsArgumentNullException()
        {
            ExceptionAssert.CatchArgumentNullException
            (() =>
            {
                SerializationTest.DeserializeUsingConstructor<HouseNumber>(null, default);
            },
            "info");
        }

        [Test]
        public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
        {
            Assert.Catch<SerializationException>
            (() =>
            {
                var info = new SerializationInfo(typeof(HouseNumber), new System.Runtime.Serialization.FormatterConverter());
                SerializationTest.DeserializeUsingConstructor<HouseNumber>(info, default);
            });
        }

        [Test]
        public void GetObjectData_Null_ThrowsArgumentNullException()
        {
            ExceptionAssert.CatchArgumentNullException
            (() =>
            {
                ISerializable obj = TestStruct;
                obj.GetObjectData(null, default);
            },
            "info");
        }

        [Test]
        public void GetObjectData_SerializationInfo_AreEqual()
        {
            ISerializable obj = TestStruct;
            var info = new SerializationInfo(typeof(HouseNumber), new System.Runtime.Serialization.FormatterConverter());
            obj.GetObjectData(info, default);

            Assert.AreEqual(123456789, info.GetInt32("Value"));
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
            var exp = "123456789";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void XmlDeserialize_XmlString_AreEqual()
        {
            var act = SerializationTest.XmlDeserialize<HouseNumber>("123456789");
            Assert.AreEqual(TestStruct, act);
        }


        [Test]
        public void SerializeDeserialize_HouseNumberSerializeObject_AreEqual()
        {
            var input = new HouseNumberSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new HouseNumberSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void XmlSerializeDeserialize_HouseNumberSerializeObject_AreEqual()
        {
            var input = new HouseNumberSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new HouseNumberSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void DataContractSerializeDeserialize_HouseNumberSerializeObject_AreEqual()
        {
            var input = new HouseNumberSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new HouseNumberSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void SerializeDeserialize_Empty_AreEqual()
        {
            var input = new HouseNumberSerializeObject
            {
                Id = 17,
                Obj = HouseNumber.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new HouseNumberSerializeObject
            {
                Id = 17,
                Obj = HouseNumber.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void XmlSerializeDeserialize_Empty_AreEqual()
        {
            var input = new HouseNumberSerializeObject
            {
                Id = 17,
                Obj = HouseNumber.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new HouseNumberSerializeObject
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

        [Test]
        public void GetSchema_None_IsNull()
        {
            IXmlSerializable obj = TestStruct;
            Assert.IsNull(obj.GetSchema());
        }

        #endregion

        #region JSON (De)serialization tests

        [Test]
        public void FromJson_InvalidStringValue_AssertFormatException()
        {
            Assert.Catch<FormatException>(() =>
            {
                JsonTester.Read<HouseNumber>("InvalidStringValue");
            },
            "Not a valid house number");
        }
        [Test]
        public void FromJson_StringValue_AreEqual()
        {
            var act = JsonTester.Read<HouseNumber>("123456789");
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_Int64Value_AreEqual()
        {
            var act = JsonTester.Read<HouseNumber>(123456789L);
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_DoubleValue_AreEqual()
        {
            var act = JsonTester.Read<HouseNumber>((Double)TestStruct);
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_DateTimeValue_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>(() =>
            {
                JsonTester.Read<HouseNumber>(new DateTime(1972, 02, 14));
            },
            "JSON deserialization from a date is not supported.");
        }

        [Test]
        public void ToJson_DefaultValue_IsNull()
        {
            object act = JsonTester.Write(default(HouseNumber));
            Assert.IsNull(act);
        }
        [Test]
        public void ToJson_TestStruct_AreEqual()
        {
            var act = JsonTester.Write(TestStruct);
            var exp = "123456789";
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IFormattable / Tostring tests

        [Test]
        public void ToString_Empty_IsStringEmpty()
        {
            var act = HouseNumber.Empty.ToString();
            var exp = "";
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToString_Unknown_QuestionMark()
        {
            var act = HouseNumber.Unknown.ToString();
            var exp = "?";
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: '123456789', format: 'Unit Test Format'";

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToString_TestStruct_ComplexPattern()
        {
            var act = TestStruct.ToString("");
            var exp = "123456789";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_FormatValueDutchBelgium_AreEqual()
        {
            using (new CultureInfoScope("nl-BE"))
            {
                var act = HouseNumber.Parse("800").ToString("0000");
                var exp = "0800";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_FormatValueEnglishGreatBritain_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var act = HouseNumber.Parse("800").ToString("0000");
                var exp = "0800";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_FormatValueSpanishEcuador_AreEqual()
        {
            var act = HouseNumber.Parse("1700").ToString("00000.0", new CultureInfo("es-EC"));
            var exp = "01700,0";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void DebuggerDisplay_DebugToString_HasAttribute()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(HouseNumber));
        }

        [Test]
        public void DebuggerDisplay_DefaultValue_String()
        {
            DebuggerDisplayAssert.HasResult("HouseNumber: (empty)", default(HouseNumber));
        }
        [Test]
        public void DebuggerDisplay_Unknown_String()
        {
            DebuggerDisplayAssert.HasResult("HouseNumber: (unknown)", HouseNumber.Unknown);
        }

        [Test]
        public void DebuggerDisplay_TestStruct_String()
        {
            DebuggerDisplayAssert.HasResult("HouseNumber: 123456789", TestStruct);
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for HouseNumber.Empty.</summary>
        [Test]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, HouseNumber.Empty.GetHashCode());
        }

        /// <summary>GetHash should not fail for the test struct.</summary>
        [Test]
        public void GetHash_TestStruct_Hash()
        {
            Assert.AreEqual(123456789, TestStruct.GetHashCode());
        }

        [Test]
        public void Equals_EmptyEmpty_IsTrue()
        {
            Assert.IsTrue(HouseNumber.Empty.Equals(HouseNumber.Empty));
        }

        [Test]
        public void Equals_TestStructTestStruct_IsTrue()
        {
            Assert.IsTrue(TestStruct.Equals(TestStruct));
        }

        [Test]
        public void Equals_TestStructEmpty_IsFalse()
        {
            Assert.IsFalse(TestStruct.Equals(HouseNumber.Empty));
        }

        [Test]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(HouseNumber.Empty.Equals(TestStruct));
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

        #endregion

        #region IComparable tests

        /// <summary>Orders a list of house numbers ascending.</summary>
        [Test]
        public void OrderBy_HouseNumber_AreEqual()
        {
            HouseNumber item0 = 1;
            HouseNumber item1 = 12;
            HouseNumber item2 = 123;
            HouseNumber item3 = 1234;

            var inp = new List<HouseNumber> { HouseNumber.Empty, item3, item2, item0, item1, HouseNumber.Empty };
            var exp = new List<HouseNumber> { HouseNumber.Empty, HouseNumber.Empty, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of house numbers descending.</summary>
        [Test]
        public void OrderByDescending_HouseNumber_AreEqual()
        {
            HouseNumber item0 = 1;
            HouseNumber item1 = 12;
            HouseNumber item2 = 123;
            HouseNumber item3 = 1234;

            var inp = new List<HouseNumber> { HouseNumber.Empty, item3, item2, item0, item1, HouseNumber.Empty };
            var exp = new List<HouseNumber> { item3, item2, item1, item0, HouseNumber.Empty, HouseNumber.Empty };
            var act = inp.OrderByDescending(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Compare with a to object casted instance should be fine.</summary>
        [Test]
        public void CompareTo_ObjectTestStruct_0()
        {
            object other = TestStruct;

            var exp = 0;
            var act = TestStruct.CompareTo(other);

            Assert.AreEqual(exp, act);
        }

        /// <summary>Compare with null should throw an exception.</summary>
        [Test]
        public void CompareTo_null_ThrowsArgumentException()
        {
            ExceptionAssert.CatchArgumentException
            (() =>
                {
                    object other = null;
                    TestStruct.CompareTo(other);
                },
                "obj",
                "Argument must be HouseNumber."
            );
        }
        /// <summary>Compare with a random object should throw an exception.</summary>
        [Test]
        public void CompareTo_newObject_ThrowsArgumentException()
        {
            ExceptionAssert.CatchArgumentException
            (() =>
                {
                    object other = new object();
                    TestStruct.CompareTo(other);
                },
                "obj",
                "Argument must be HouseNumber."
            );
        }

        [Test]
        public void LessThan_17LT19_IsTrue()
        {
            HouseNumber l = 17;
            HouseNumber r = 19;

            Assert.IsTrue(l < r);
        }
        [Test]
        public void GreaterThan_21LT19_IsTrue()
        {
            HouseNumber l = 21;
            HouseNumber r = 19;

            Assert.IsTrue(l > r);
        }

        [Test]
        public void LessThanOrEqual_17LT19_IsTrue()
        {
            HouseNumber l = 17;
            HouseNumber r = 19;

            Assert.IsTrue(l <= r);
        }
        [Test]
        public void GreaterThanOrEqual_21LT19_IsTrue()
        {
            HouseNumber l = 21;
            HouseNumber r = 19;

            Assert.IsTrue(l >= r);
        }

        [Test]
        public void LessThanOrEqual_17LT17_IsTrue()
        {
            HouseNumber l = 17;
            HouseNumber r = 17;

            Assert.IsTrue(l <= r);
        }
        [Test]
        public void GreaterThanOrEqual_21LT21_IsTrue()
        {
            HouseNumber l = 21;
            HouseNumber r = 21;

            Assert.IsTrue(l >= r);
        }
        #endregion

        #region Casting tests

        [Test]
        public void Explicit_StringToHouseNumber_AreEqual()
        {
            var exp = TestStruct;
            var act = (HouseNumber)TestStruct.ToString();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_HouseNumberToString_AreEqual()
        {
            var exp = TestStruct.ToString();
            var act = (string)TestStruct;

            Assert.AreEqual(exp, act);
        }


        [Test]
        public void Explicit_Int32ToHouseNumber_AreEqual()
        {
            var exp = TestStruct;
            var act = (HouseNumber)123456789;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_HouseNumberToInt32_AreEqual()
        {
            var exp = 123456789;
            var act = (Int32)TestStruct;

            Assert.AreEqual(exp, act);
        }
        #endregion

        #region Properties

        [Test]
        public void IsOdd_Empty_IsFalse()
        {
            Assert.IsFalse(HouseNumber.Empty.IsOdd);
        }

        [Test]
        public void IsOdd_Unknown_IsFalse()
        {
            Assert.IsFalse(HouseNumber.Unknown.IsOdd);
        }

        [Test]
        public void IsOdd_TestStruct_IsTrue()
        {
            Assert.IsTrue(TestStruct.IsOdd);
        }

        [Test]
        public void IsEven_Empty_IsFalse()
        {
            Assert.IsFalse(HouseNumber.Empty.IsEven);
        }

        [Test]
        public void IsEven_Unknown_IsFalse()
        {
            Assert.IsFalse(HouseNumber.Unknown.IsEven);
        }

        [Test]
        public void IsEven_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEven);
        }

        [Test]
        public void IsEven_1234_IsTrue()
        {
            Assert.IsTrue(HouseNumber.Create(1234).IsEven);
        }


        [Test]
        public void Length_Empty_0()
        {
            var act = HouseNumber.Empty.Length;
            var exp = 0;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Length_Unknown_0()
        {
            var act = HouseNumber.Unknown.Length;
            var exp = 0;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Length_TestStruct_9()
        {
            var act = TestStruct.Length;
            var exp = 9;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Length_1234_4()
        {
            var act = HouseNumber.Create(1234).Length;
            var exp = 4;
            Assert.AreEqual(exp, act);
        }
        #endregion

        #region Type converter tests

        [Test]
        public void ConverterExists_HouseNumber_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(HouseNumber));
        }

        [Test]
        public void CanNotConvertFromDateTime_HouseNumber_IsTrue()
        {
            TypeConverterAssert.CanNotConvertFrom(typeof(HouseNumber), typeof(DateTime));
        }
       
        [Test]
        public void CanConvertFromString_HouseNumber_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(HouseNumber));
        }

        [Test]
        public void CanConvertToString_HouseNumber_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(HouseNumber));
        }

        [Test]
        public void ConvertFrom_StringNull_HouseNumberEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(HouseNumber.Empty, (string)null);
            }
        }

        [Test]
        public void ConvertFrom_StringEmpty_HouseNumberEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(HouseNumber.Empty, string.Empty);
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

        [Test]
        public void ConvertFromUnderlyingType_NullableInt64_Successful()
        {
            TypeConverterAssert.ConvertFromEquals(HouseNumber.Empty, default(long?));
        }
        [Test]
        public void ConvertFrom_Int_Successful()
        {
            TypeConverterAssert.ConvertFromEquals(TestStruct, 123456789);
        }

        [Test]
        public void ConvertToUnderlyingType_Int_Successful()
        {
            TypeConverterAssert.ConvertToEquals(123456789, TestStruct);
        }
        [Test]
        public void ConvertToUnderlyingType_NullableInt_Successful()
        {
            TypeConverterAssert.ConvertToEquals(default(int?), HouseNumber.Empty);
        }

        #endregion

        #region IsValid tests

        [Test]
        public void IsValid_Data_IsFalse()
        {
            Assert.IsFalse(HouseNumber.IsValid("1234567890"), "1234567890");
            Assert.IsFalse(HouseNumber.IsValid((String)null), "(String)null");
            Assert.IsFalse(HouseNumber.IsValid(string.Empty), "string.Empty");

            Assert.IsFalse(HouseNumber.IsValid((System.Int32?)null), "(System.Int32?)null");
        }
        [Test]
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
