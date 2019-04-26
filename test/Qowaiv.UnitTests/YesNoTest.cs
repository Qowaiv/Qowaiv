using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.TestTools;
using System;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.Fiancial.UnitTests
{
    /// <summary>Tests the Yes-no SVO.</summary>
    public class YesNoTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly YesNo TestStruct = YesNo.Yes;

        #region Yes-no const tests

        /// <summary>YesNo.Empty should be equal to the default of Yes-no.</summary>
        [Test]
        public void Empty_None_EqualsDefault()
        {
            Assert.AreEqual(default(YesNo), YesNo.Empty);
        }

        #endregion

        #region Yes-no IsEmpty tests

        /// <summary>YesNo.IsEmpty() should be true for the default of Yes-no.</summary>
        [Test]
        public void IsEmpty_Default_IsTrue()
        {
            Assert.IsTrue(default(YesNo).IsEmpty());
        }
        /// <summary>YesNo.IsEmpty() should be false for YesNo.Unknown.</summary>
        [Test]
        public void IsEmpty_Unknown_IsFalse()
        {
            Assert.IsFalse(YesNo.Unknown.IsEmpty());
        }
        /// <summary>YesNo.IsEmpty() should be false for the TestStruct.</summary>
        [Test]
        public void IsEmpty_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmpty());
        }

        /// <summary>YesNo.IsUnknown() should be false for the default of Yes-no.</summary>
        [Test]
        public void IsUnknown_Default_IsFalse()
        {
            Assert.IsFalse(default(YesNo).IsUnknown());
        }
        /// <summary>YesNo.IsUnknown() should be true for YesNo.Unknown.</summary>
        [Test]
        public void IsUnknown_Unknown_IsTrue()
        {
            Assert.IsTrue(YesNo.Unknown.IsUnknown());
        }
        /// <summary>YesNo.IsUnknown() should be false for the TestStruct.</summary>
        [Test]
        public void IsUnknown_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsUnknown());
        }

        /// <summary>YesNo.IsEmptyOrUnknown() should be true for the default of Yes-no.</summary>
        [Test]
        public void IsEmptyOrUnknown_Default_IsFalse()
        {
            Assert.IsTrue(default(YesNo).IsEmptyOrUnknown());
        }
        /// <summary>YesNo.IsEmptyOrUnknown() should be true for YesNo.Unknown.</summary>
        [Test]
        public void IsEmptyOrUnknown_Unknown_IsTrue()
        {
            Assert.IsTrue(YesNo.Unknown.IsEmptyOrUnknown());
        }
        /// <summary>YesNo.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
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

            Assert.IsTrue(YesNo.TryParse(str, out YesNo val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TyrParse_StringEmpty_IsValid()
        {
            string str = string.Empty;

            Assert.IsTrue(YesNo.TryParse(str, out YesNo val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse "?" should be valid and the result should be YesNo.Unknown.</summary>
        [Test]
        public void TyrParse_Questionmark_IsValid()
        {
            string str = "?";

            Assert.IsTrue(YesNo.TryParse(str, out YesNo val), "Valid");
            Assert.IsTrue(val.IsUnknown(), "Value");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TyrParse_StringValue_IsValid()
        {
            using (new CultureInfoScope("en"))
            {
                string str = "yes";
                Assert.IsTrue(YesNo.TryParse(str, out YesNo val), "Valid");
                Assert.AreEqual(str, val.ToString(), "Value");
            }
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [Test]
        public void TyrParse_StringValue_IsNotValid()
        {
            string str = "string";

            Assert.IsFalse(YesNo.TryParse(str, out YesNo val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        [Test]
        public void Parse_Unknown_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var act = YesNo.Parse("?");
                var exp = YesNo.Unknown;
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
                    YesNo.Parse("InvalidInput");
                },
                "Not a valid Yes-no");
            }
        }

        [Test]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = TestStruct;
                var act = YesNo.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void TryParse_InvalidInput_DefaultValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = default(YesNo);
                var act = YesNo.TryParse("InvalidInput");

                Assert.AreEqual(exp, act);
            }
        }

        #endregion


        #region TryCreate tests

        [Test]
        public void TryCreate_Null_IsEmpty()
        {
            YesNo exp = YesNo.Empty;
            Assert.IsTrue(YesNo.TryCreate(null, out YesNo act));
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void TryCreate_69_IsEmpty()
        {
            YesNo exp = YesNo.Empty;
            Assert.IsFalse(YesNo.TryCreate(69, out YesNo act));
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void TryCreate_Null_AreEqual()
        {
            var exp = YesNo.Empty;
            var act = YesNo.TryCreate(null);
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void TryCreate_Value_AreEqual()
        {
            var exp = TestStruct;
            var act = YesNo.TryCreate(1);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Create_SomeValue_ThrowsArgumentOutOfRangeException()
        {
            ExceptionAssert.CatchArgumentOutOfRangeException(() =>
            {
                YesNo.Create(69);
            },
            "val",
            "Not a valid yes-no value");
        }

        #endregion

        #region (XML) (De)serialization tests

        [Test]
        public void Constructor_SerializationInfoIsNull_ThrowsArgumentNullException()
        {
            ExceptionAssert.CatchArgumentNullException
            (() =>
            {
                SerializationTest.DeserializeUsingConstructor<YesNo>
        (null, default(StreamingContext));
            },
            "info");
        }

        [Test]
        public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
        {
            Assert.Catch<SerializationException>
            (() =>
            {
                var info = new SerializationInfo(typeof(YesNo), new System.Runtime.Serialization.FormatterConverter());
                SerializationTest.DeserializeUsingConstructor<YesNo>
        (info, default(StreamingContext));
            });
        }

        [Test]
        public void GetObjectData_Null_ThrowsArgumentNullException()
        {
            ExceptionAssert.CatchArgumentNullException
            (() =>
            {
                ISerializable obj = TestStruct;
                obj.GetObjectData(null, default(StreamingContext));
            },
            "info");
        }

        [Test]
        public void GetObjectData_SerializationInfo_AreEqual()
        {
            ISerializable obj = TestStruct;
            var info = new SerializationInfo(typeof(YesNo), new System.Runtime.Serialization.FormatterConverter());
            obj.GetObjectData(info, default(StreamingContext));

            Assert.AreEqual((Byte)2, info.GetByte("Value"));
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
        public void XmlSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = TestStruct;
            var exp = TestStruct;
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void SerializeDeserialize_YesNoSerializeObject_AreEqual()
        {
            var input = new YesNoSerializeObject()
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new YesNoSerializeObject()
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date"); ;
        }
        [Test]
        public void XmlSerializeDeserialize_YesNoSerializeObject_AreEqual()
        {
            var input = new YesNoSerializeObject()
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new YesNoSerializeObject()
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date"); ;
        }
        [Test]
        public void DataContractSerializeDeserialize_YesNoSerializeObject_AreEqual()
        {
            var input = new YesNoSerializeObject()
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new YesNoSerializeObject()
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date"); ;
        }

        [Test]
        public void SerializeDeserialize_Default_AreEqual()
        {
            var input = new YesNoSerializeObject()
            {
                Id = 17,
                Obj = default(YesNo),
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new YesNoSerializeObject()
            {
                Id = 17,
                Obj = default(YesNo),
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void XmlSerializeDeserialize_Default_AreEqual()
        {
            var input = new YesNoSerializeObject()
            {
                Id = 17,
                Obj = default(YesNo),
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new YesNoSerializeObject()
            {
                Id = 17,
                Obj = default(YesNo),
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
        public void FromJson_None_EmptyValue()
        {
            var act = JsonTester.Read<YesNo>();
            var exp = YesNo.Empty;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_InvalidStringValue_AssertFormatException()
        {
            Assert.Catch<FormatException>
            (() =>
            {
                JsonTester.Read<YesNo>
        ("InvalidStringValue");
            },
            "Not a valid Yes-no");
        }
        [Test]
        public void FromJson_StringValue_AreEqual()
        {
            var act = JsonTester.Read<YesNo>
            (TestStruct.ToString(CultureInfo.InvariantCulture));
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_Int64Value_AreEqual()
        {
            var act = JsonTester.Read<YesNo>(0);
            var exp = YesNo.No;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_DoubleValue_AreEqual()
        {
            var act = JsonTester.Read<YesNo>(1.0);
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }
   
        [Test]
        public void FromJson_DateTimeValue_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>
            (() =>
            {
                JsonTester.Read<YesNo>(new DateTime(1972, 02, 14));
            },
            "JSON deserialization from a date is not supported.");
        }

        [Test]
        public void ToJson_DefaultValue_AreEqual()
        {
            object act = JsonTester.Write(default(YesNo));
            object exp = null;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToJson_TestStruct_AreEqual()
        {
            var act = JsonTester.Write(TestStruct);
            var exp = TestStruct.ToString(CultureInfo.InvariantCulture);

            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IFormattable / Tostring tests

        [Test]
        public void ToString_Empty_StringEmpty()
        {
            var act = YesNo.Empty.ToString();
            var exp = "";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_Unknown_QuestionMark()
        {
            var act = YesNo.Unknown.ToString("c");
            var exp = "?";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: 'yes', format: 'Unit Test Format'";

            Assert.AreEqual(exp, act);
        }
        [TestCase("en-GB", null, "Yes", "yes")]
        [TestCase("nl-BE", null, "Yes", "ja")]
        [TestCase("es-EQ", null, "Yes", "si")]
        [TestCase("en-GB", null, "No", "no")]
        [TestCase("nl-BE", null, "no", "nee")]
        [TestCase("es-EQ", null, "no", "no")]
        [TestCase("en-GB", "c", "Yes", "Y")]
        [TestCase("nl-BE", "c", "Yes", "J")]
        [TestCase("es-EQ", "c", "Yes", "S")]
        [TestCase("en-GB", "c", "No", "N")]
        [TestCase("nl-BE", "c", "no", "N")]
        [TestCase("es-EQ", "c", "no", "N")]
        public void ToString_UsingCultureWithPattern(string culture, string format, string str, string expected)
        {
            using (new CultureInfoScope(culture))
            {
                var actual = YesNo.Parse(str).ToString(format);
                Assert.AreEqual(expected, actual);
            }
        }

        [Test]
        public void DebuggerDisplay_DebugToString_HasAttribute()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(YesNo));
        }

        [Test]
        public void DebuggerDisplay_DefaultValue_String()
        {
            DebuggerDisplayAssert.HasResult("{empty} (YesNo)", default(YesNo));
        }
        [Test]
        public void DebuggerDisplay_Unknown_String()
        {
            DebuggerDisplayAssert.HasResult("unknown (YesNo)", YesNo.Unknown);
        }

        [Test]
        public void DebuggerDisplay_TestStruct_String()
        {
            DebuggerDisplayAssert.HasResult("yes", TestStruct);
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for YesNo.Empty.</summary>
        [Test]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, YesNo.Empty.GetHashCode());
        }

        /// <summary>GetHash should not fail for the test struct.</summary>
        [Test]
        public void GetHash_TestStruct_Hash()
        {
            Assert.AreEqual(2, TestStruct.GetHashCode());
        }

        [Test]
        public void Equals_EmptyEmpty_IsTrue()
        {
            Assert.IsTrue(YesNo.Empty.Equals(YesNo.Empty));
        }

        [Test]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = YesNo.Parse("Yes", CultureInfo.InvariantCulture);
            var r = YesNo.Parse("yes", CultureInfo.InvariantCulture);

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
            Assert.IsFalse(TestStruct.Equals(YesNo.Empty));
        }

        [Test]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(YesNo.Empty.Equals(TestStruct));
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

        /// <summary>Orders a list of Yes-nos ascending.</summary>
        [Test]
        public void OrderBy_YesNo_AreEqual()
        {
            var item0 = YesNo.No;
            var item1 = YesNo.Yes;
            var item2 = YesNo.Unknown;

            var inp = new []{ YesNo.Empty, item2, item0, item1, YesNo.Empty };
            var exp = new []{ YesNo.Empty, YesNo.Empty, item0, item1, item2 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of Yes-nos descending.</summary>
        [Test]
        public void OrderByDescending_YesNo_AreEqual()
        {
            var item0 = YesNo.No;
            var item1 = YesNo.Yes;
            var item2 = YesNo.Unknown;

            var inp = new []{ YesNo.Empty, item2, item0, item1, YesNo.Empty };
            var exp = new []{ item2, item1, item0, YesNo.Empty, YesNo.Empty };
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
                TestStruct.CompareTo(null);
            },
            "obj",
            "Argument must be a Yes-no"
            );
        }
        /// <summary>Compare with a random object should throw an exception.</summary>
        [Test]
        public void CompareTo_newObject_ThrowsArgumentException()
        {
            ExceptionAssert.CatchArgumentException
            (() =>
            {
                TestStruct.CompareTo(new object());
            },
            "obj",
            "Argument must be a Yes-no"
            );
        }

        #endregion

        #region Casting tests

        [Test]
        public void Explicit_StringToYesNo_AreEqual()
        {
            var exp = TestStruct;
            var act = (YesNo)TestStruct.ToString();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_YesNoToString_AreEqual()
        {
            var exp = TestStruct.ToString();
            var act = (string)TestStruct;

            Assert.AreEqual(exp, act);
        }

        #endregion

        #region Properties
        #endregion

        #region Type converter tests

        [Test]
        public void ConverterExists_YesNo_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(YesNo));
        }

        [Test]
        public void CanNotConvertFromInt32_YesNo_IsTrue()
        {
            TypeConverterAssert.CanNotConvertFrom(typeof(YesNo), typeof(Int32));
        }
        [Test]
        public void CanNotConvertToInt32_YesNo_IsTrue()
        {
            TypeConverterAssert.CanNotConvertTo(typeof(YesNo), typeof(Int32));
        }

        [Test]
        public void CanConvertFromString_YesNo_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(YesNo));
        }

        [Test]
        public void CanConvertToString_YesNo_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(YesNo));
        }

        [Test]
        public void ConvertFrom_StringNull_YesNoEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(YesNo.Empty, (string)null);
            }
        }

        [Test]
        public void ConvertFrom_StringEmpty_YesNoEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(YesNo.Empty, string.Empty);
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
        public void ConvertFromInstanceDescriptor_YesNo_Successful()
        {
            TypeConverterAssert.ConvertFromInstanceDescriptor(typeof(YesNo));
        }

        [Test]
        public void ConvertToString_TestStruct_StringValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertToStringEquals(TestStruct.ToString(), TestStruct);
            }
        }

        #endregion
        #region IsValid tests

        [TestCase(null)]
        [TestCase("")]
        [TestCase("Complex")]
        public void IsInvalid_String(string pattern)
        {
            Assert.IsFalse(YesNo.IsValid(pattern));
        }

        [Test]
        public void IsInvalid_SystemNullableByte()
            {
            System.Byte? value = null;
            Assert.IsFalse(YesNo.IsValid(value));
        }

        [Test]
        public void IsValid_Data_IsTrue()
        {
            Assert.IsTrue(YesNo.IsValid("Unknown"));
        }
        #endregion
    }

    [Serializable]
    public class YesNoSerializeObject
    {
        public int Id { get; set; }
        public YesNo Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
