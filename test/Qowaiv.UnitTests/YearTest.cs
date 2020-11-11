using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests
{
    /// <summary>Tests the year SVO.</summary>
    public class YearTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly Year TestStruct = 1979;

        #region year const tests

        /// <summary>Year.Empty should be equal to the default of year.</summary>
        [Test]
        public void Empty_None_EqualsDefault()
        {
            Assert.AreEqual(default(Year), Year.Empty);
        }

        #endregion

        #region year IsEmpty tests

        /// <summary>Year.IsEmpty() should be true for the default of year.</summary>
        [Test]
        public void IsEmpty_Default_IsTrue()
        {
            Assert.IsTrue(default(Year).IsEmpty());
        }
        /// <summary>Year.IsEmpty() should be false for Year.Unknown.</summary>
        [Test]
        public void IsEmpty_Unknown_IsFalse()
        {
            Assert.IsFalse(Year.Unknown.IsEmpty());
        }
        /// <summary>Year.IsEmpty() should be false for the TestStruct.</summary>
        [Test]
        public void IsEmpty_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmpty());
        }

        /// <summary>Year.IsUnknown() should be false for the default of year.</summary>
        [Test]
        public void IsUnknown_Default_IsFalse()
        {
            Assert.IsFalse(default(Year).IsUnknown());
        }
        /// <summary>Year.IsUnknown() should be true for Year.Unknown.</summary>
        [Test]
        public void IsUnknown_Unknown_IsTrue()
        {
            Assert.IsTrue(Year.Unknown.IsUnknown());
        }
        /// <summary>Year.IsUnknown() should be false for the TestStruct.</summary>
        [Test]
        public void IsUnknown_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsUnknown());
        }

        /// <summary>Year.IsEmptyOrUnknown() should be true for the default of year.</summary>
        [Test]
        public void IsEmptyOrUnknown_Default_IsFalse()
        {
            Assert.IsTrue(default(Year).IsEmptyOrUnknown());
        }
        /// <summary>Year.IsEmptyOrUnknown() should be true for Year.Unknown.</summary>
        [Test]
        public void IsEmptyOrUnknown_Unknown_IsTrue()
        {
            Assert.IsTrue(Year.Unknown.IsEmptyOrUnknown());
        }
        /// <summary>Year.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
        [Test]
        public void IsEmptyOrUnknown_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmptyOrUnknown());
        }

        #endregion

        #region TryParse tests

        /// <summary>TryParse null should be valid.</summary>
        [Test]
        public void TryParse_Null_IsValid()
        {
            string str = null;
            Assert.IsTrue(Year.TryParse(str, out Year val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TryParse_StringEmpty_IsValid()
        {
            string str = string.Empty;
            Assert.IsTrue(Year.TryParse(str, out Year val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse "?" should be valid and the result should be Year.Unknown.</summary>
        [Test]
        public void TryParse_Questionmark_IsValid()
        {
            string str = "?";
            Assert.IsTrue(Year.TryParse(str, out Year val), "Valid");
            Assert.IsTrue(val.IsUnknown(), "Value");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TryParse_StringValue_IsValid()
        {
            string str = "1979";
            Assert.IsTrue(Year.TryParse(str, out Year val), "Valid");
            Assert.AreEqual(str, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [Test]
        public void TryParse_StringValue_IsNotValid()
        {
            string str = "string";
            Assert.IsFalse(Year.TryParse(str, out Year val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        [Test]
        public void Parse_Unknown_AreEqual()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var act = Year.Parse("?");
                var exp = Year.Unknown;
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void Parse_InvalidInput_ThrowsFormatException()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Assert.Catch<FormatException>
                (() =>
                {
                    Year.Parse("InvalidInput");
                },
                "Not a valid year");
            }
        }

        [Test]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var exp = TestStruct;
                var act = Year.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void TryParse_InvalidInput_DefaultValue()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var exp = default(Year);
                var act = Year.TryParse("InvalidInput");

                Assert.AreEqual(exp, act);
            }
        }

        #endregion

        #region TryCreate tests

        [Test]
        public void TryCreate_Null_IsEmpty()
        {
            Assert.IsTrue(Year.TryCreate(null, out Year act));
            Assert.AreEqual(Year.Empty, act);
        }
        [Test]
        public void TryCreate_Int32MinValue_IsEmpty()
        {
            Assert.IsFalse(Year.TryCreate(int.MinValue, out Year act));
            Assert.AreEqual(Year.Empty, act);
        }

        [Test]
        public void TryCreate_Int32MinValue_AreEqual()
        {
            var exp = Year.Empty;
            var act = Year.TryCreate(int.MinValue);
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void TryCreate_Value_AreEqual()
        {
            var exp = TestStruct;
            var act = Year.TryCreate(1979);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Create_Int32MinValue_ThrowsArgumentOutOfRangeException()
        {
            ExceptionAssert.CatchArgumentOutOfRangeException
            (() =>
            {
                Year.Create(Int32.MinValue);
            },
            "val",
            "Not a valid year");
        }

        #endregion

        #region (XML) (De)serialization tests

        [Test]
        public void Constructor_SerializationInfoIsNull_ThrowsArgumentNullException()
        {
            ExceptionAssert.CatchArgumentNullException
            (() =>
            {
                SerializationTest.DeserializeUsingConstructor<Year>(null, default);
            },
            "info");
        }

        [Test]
        public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
        {
            Assert.Catch<SerializationException>
            (() =>
            {
                var info = new SerializationInfo(typeof(Year), new System.Runtime.Serialization.FormatterConverter());
                SerializationTest.DeserializeUsingConstructor<Year>(info, default);
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
            var info = new SerializationInfo(typeof(Year), new System.Runtime.Serialization.FormatterConverter());
            obj.GetObjectData(info, default);

            Assert.AreEqual(1979, info.GetInt16("Value"));
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
            var exp = "1979";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void XmlDeserialize_XmlString_AreEqual()
        {
            var act = SerializationTest.XmlDeserialize<Year>("1979");
            Assert.AreEqual(TestStruct, act);
        }

        [Test]
        public void SerializeDeserialize_YearSerializeObject_AreEqual()
        {
            var input = new YearSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new YearSerializeObject
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
        public void XmlSerializeDeserialize_YearSerializeObject_AreEqual()
        {
            var input = new YearSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new YearSerializeObject
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
        public void DataContractSerializeDeserialize_YearSerializeObject_AreEqual()
        {
            var input = new YearSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new YearSerializeObject
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
            var input = new YearSerializeObject
            {
                Id = 17,
                Obj = Year.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new YearSerializeObject
            {
                Id = 17,
                Obj = Year.Empty,
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
            var input = new YearSerializeObject
            {
                Id = 17,
                Obj = Year.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new YearSerializeObject
            {
                Id = 17,
                Obj = Year.Empty,
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
        
        [TestCase("Invalid input")]
        [TestCase("2017-06-11")]
        public void FromJson_Invalid_Throws(object json)
        {
            Assert.Catch<FormatException>(() => JsonTester.Read<Year>(json));
        }

        [TestCase(1600, 1600L)]
        [TestCase(2017, 2017.0)]
        [TestCase(1793, "1793")]
        public void FromJson(Year expected, object json)
        {
            var actual = JsonTester.Read<Year>(json);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToJson_DefaultValue_IsNull()
        {
            object act = JsonTester.Write(default(Year));
             Assert.IsNull(act);
        }
        [Test]
        public void ToJson_Unknown_AreEqual()
        {
            var act = JsonTester.Write(Year.Unknown);
            var exp = "?";
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToJson_TestStruct_AreEqual()
        {
            var act = JsonTester.Write(TestStruct);
            var exp = (short)TestStruct;
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IFormattable / ToString tests

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("#000.0", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: '1979.0', format: '#000.0'";

            Assert.AreEqual(exp, act);
        }

        [TestCase("en-US", "", "1979", "1979")]
        [TestCase("en-GB", "", "?", "?")]
        [TestCase("nl-BE", "0000", "800", "0800")]
        [TestCase("en-GB", "0000", "800", "0800")]
        public void ToString_UsingCultureWithPattern(string culture, string format, string str, string expected)
        {
            using (new CultureInfoScope(culture))
            {
                var actual = Year.Parse(str).ToString(format);
                Assert.AreEqual(expected, actual);
            }
        }

        [Test]
        public void ToString_FormatValueSpanishEcuador_AreEqual()
        {
            var act = Year.Parse("1700").ToString("00000.0", new CultureInfo("es-EC"));
            var exp = "01700,0";
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for Year.Empty.</summary>
        [Test]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, Year.Empty.GetHashCode());
        }

        /// <summary>GetHash should not fail for the test struct.</summary>
        [Test]
        public void GetHash_TestStruct_Hash()
        {
            Assert.AreEqual(1979, TestStruct.GetHashCode());
        }

        [Test]
        public void Equals_EmptyEmpty_IsTrue()
        {
            Assert.IsTrue(Year.Empty.Equals(Year.Empty));
        }

        [Test]
        public void Equals_TestStructTestStruct_IsTrue()
        {
            Assert.IsTrue(TestStruct.Equals(TestStruct));
        }

        [Test]
        public void Equals_TestStructEmpty_IsFalse()
        {
            Assert.IsFalse(TestStruct.Equals(Year.Empty));
        }

        [Test]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(Year.Empty.Equals(TestStruct));
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

        /// <summary>Orders a list of years ascending.</summary>
        [Test]
        public void OrderBy_Year_AreEqual()
        {
            Year item0 = 1980;
            Year item1 = 1981;
            Year item2 = 1982;
            Year item3 = 1983;

            var inp = new List<Year> { Year.Empty, item3, item2, item0, item1, Year.Empty };
            var exp = new List<Year> { Year.Empty, Year.Empty, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of years descending.</summary>
        [Test]
        public void OrderByDescending_Year_AreEqual()
        {
            Year item0 = 1980;
            Year item1 = 1981;
            Year item2 = 1982;
            Year item3 = 1983;

            var inp = new List<Year> { Year.Empty, item3, item2, item0, item1, Year.Empty };
            var exp = new List<Year> { item3, item2, item1, item0, Year.Empty, Year.Empty };
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

        /// <summary>Compare with null should return 1.</summary>
        [Test]
        public void CompareTo_null_1()
        {
            object @null = null;
            Assert.AreEqual(1, TestStruct.CompareTo(@null));
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
                "Argument must be Year."
            );
        }

        [Test]
        public void LessThan_17LT19_IsTrue()
        {
            Year l = 17;
            Year r = 19;

            Assert.IsTrue(l < r);
        }
        [Test]
        public void GreaterThan_21LT19_IsTrue()
        {
            Year l = 21;
            Year r = 19;

            Assert.IsTrue(l > r);
        }

        [Test]
        public void LessThanOrEqual_17LT19_IsTrue()
        {
            Year l = 17;
            Year r = 19;

            Assert.IsTrue(l <= r);
        }
        [Test]
        public void GreaterThanOrEqual_21LT19_IsTrue()
        {
            Year l = 21;
            Year r = 19;

            Assert.IsTrue(l >= r);
        }

        [Test]
        public void LessThanOrEqual_17LT17_IsTrue()
        {
            Year l = 17;
            Year r = 17;

            Assert.IsTrue(l <= r);
        }
        [Test]
        public void GreaterThanOrEqual_21LT21_IsTrue()
        {
            Year l = 21;
            Year r = 21;

            Assert.IsTrue(l >= r);
        }
        #endregion

        #region Casting tests

        [Test]
        public void Explicit_StringToYear_AreEqual()
        {
            var exp = TestStruct;
            var act = (Year)TestStruct.ToString();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_YearToString_AreEqual()
        {
            var exp = TestStruct.ToString();
            var act = (string)TestStruct;

            Assert.AreEqual(exp, act);
        }


        [Test]
        public void Explicit_Int32ToYear_AreEqual()
        {
            var exp = TestStruct;
            var act = (Year)1979;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_YearToInt32_AreEqual()
        {
            var exp = 1979;
            var act = (Int32)TestStruct;

            Assert.AreEqual(exp, act);
        }
        #endregion

        #region Properties

        [Test]
        public void IsLeapYear_Empty_IsFalse()
        {
            Assert.IsFalse(Year.Empty.IsLeapYear);
        }

        [Test]
        public void IsLeapYear_Unknown_IsFalse()
        {
            Assert.IsFalse(Year.Unknown.IsLeapYear);
        }

        [Test]
        public void IsLeapYear_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsLeapYear);
        }

        [Test]
        public void IsLeapYear_1980_IsTrue()
        {
            Year year = 1980;
            Assert.IsTrue(year.IsLeapYear);
        }

        #endregion

        #region Type converter tests

        [Test]
        public void ConverterExists_Year_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(Year));
        }

        [Test]
        public void CanConvertFromInt32_Year()
        {
            TypeConverterAssert.ConvertFromEquals((Year)2018, 2018);
        }
        [Test]
        public void CanConvertToInt32_Year()
        {
            TypeConverterAssert.ConvertToEquals(1979, TestStruct);
        }

        [Test]
        public void CanConvertFromString_Year_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(Year));
        }

        [Test]
        public void CanConvertToString_Year_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(Year));
        }

        [Test]
        public void ConvertFrom_StringNull_YearEmpty()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(Year.Empty, (string)null);
            }
        }

        [Test]
        public void ConvertFrom_StringEmpty_YearEmpty()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(Year.Empty, string.Empty);
            }
        }

        [Test]
        public void ConvertFromString_StringValue_TestStruct()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(TestStruct, TestStruct.ToString());
            }
        }

        [Test]
        public void ConvertToString_TestStruct_StringValue()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertToStringEquals(TestStruct.ToString(), TestStruct);
            }
        }

        #endregion

        #region IsValid tests

        [TestCase("0000")]
        [TestCase("000")]
        [TestCase("00")]
        [TestCase("0")]
        [TestCase((string)null)]
        [TestCase("")]
        public void IsInvalid_String(string value)
        {
            Assert.IsFalse(Year.IsValid(value));
        }

        [TestCase]
        public void IsInvalid_NullabelInt()
        {
            int? number = null;
            Assert.IsFalse(Year.IsValid(number));
        }
        [Test]
        public void IsValid_Data_IsTrue()
        {
            Assert.IsTrue(Year.IsValid("0001"), "0001");
            Assert.IsTrue(Year.IsValid("1800"), "1800");
        }
        #endregion
    }

    [Serializable]
    public class YearSerializeObject
    {
        public int Id { get; set; }
        public Year Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
