using NUnit.Framework;
using Qowaiv.Financial;
using Qowaiv.Globalization;
using Qowaiv.TestTools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests.Financial
{
    /// <summary>Tests the BIC SVO.</summary>
    public class BusinessIdentifierCodeTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly BusinessIdentifierCode TestStruct = BusinessIdentifierCode.Parse("AEGONL2UXXX");

        #region BIC const tests

        /// <summary>BusinessIdentifierCode.Empty should be equal to the default of BIC.</summary>
        [Test]
        public void Empty_None_EqualsDefault()
        {
            Assert.AreEqual(default(BusinessIdentifierCode), BusinessIdentifierCode.Empty);
        }

        #endregion

        #region BIC IsEmpty tests

        /// <summary>BusinessIdentifierCode.IsEmpty() should be true for the default of BIC.</summary>
        [Test]
        public void IsEmpty_Default_IsTrue()
        {
            Assert.IsTrue(default(BusinessIdentifierCode).IsEmpty());
        }
        /// <summary>BusinessIdentifierCode.IsEmpty() should be false for BusinessIdentifierCode.Unknown.</summary>
        [Test]
        public void IsEmpty_Unknown_IsFalse()
        {
            Assert.IsFalse(BusinessIdentifierCode.Unknown.IsEmpty());
        }
        /// <summary>BusinessIdentifierCode.IsEmpty() should be false for the TestStruct.</summary>
        [Test]
        public void IsEmpty_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmpty());
        }

        /// <summary>BusinessIdentifierCode.IsUnknown() should be false for the default of BIC.</summary>
        [Test]
        public void IsUnknown_Default_IsFalse()
        {
            Assert.IsFalse(default(BusinessIdentifierCode).IsUnknown());
        }
        /// <summary>BusinessIdentifierCode.IsUnknown() should be true for BusinessIdentifierCode.Unknown.</summary>
        [Test]
        public void IsUnknown_Unknown_IsTrue()
        {
            Assert.IsTrue(BusinessIdentifierCode.Unknown.IsUnknown());
        }
        /// <summary>BusinessIdentifierCode.IsUnknown() should be false for the TestStruct.</summary>
        [Test]
        public void IsUnknown_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsUnknown());
        }

        /// <summary>BusinessIdentifierCode.IsEmptyOrUnknown() should be true for the default of BIC.</summary>
        [Test]
        public void IsEmptyOrUnknown_Default_IsFalse()
        {
            Assert.IsTrue(default(BusinessIdentifierCode).IsEmptyOrUnknown());
        }
        /// <summary>BusinessIdentifierCode.IsEmptyOrUnknown() should be true for BusinessIdentifierCode.Unknown.</summary>
        [Test]
        public void IsEmptyOrUnknown_Unknown_IsTrue()
        {
            Assert.IsTrue(BusinessIdentifierCode.Unknown.IsEmptyOrUnknown());
        }
        /// <summary>BusinessIdentifierCode.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
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
            Assert.IsTrue(BusinessIdentifierCode.TryParse(str, out BusinessIdentifierCode val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TyrParse_StringEmpty_IsValid()
        {
            string str = string.Empty;
            Assert.IsTrue(BusinessIdentifierCode.TryParse(str, out BusinessIdentifierCode val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse "?" should be valid and the result should be BusinessIdentifierCode.Unknown.</summary>
        [Test]
        public void TyrParse_Questionmark_IsValid()
        {
            string str = "?";
            Assert.IsTrue(BusinessIdentifierCode.TryParse(str, out BusinessIdentifierCode val), "Valid");
            Assert.IsTrue(val.IsUnknown(), "Value");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TyrParse_StringValue_IsValid()
        {
            string str = "AEGONL2UXXX";
            Assert.IsTrue(BusinessIdentifierCode.TryParse(str, out BusinessIdentifierCode val), "Valid");
            Assert.AreEqual(str, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [Test]
        public void TyrParse_StringValue_IsNotValid()
        {
            string str = "string";
            Assert.IsFalse(BusinessIdentifierCode.TryParse(str, out BusinessIdentifierCode val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        [Test]
        public void Parse_Unknown_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var act = BusinessIdentifierCode.Parse("?");
                var exp = BusinessIdentifierCode.Unknown;
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
                    BusinessIdentifierCode.Parse("InvalidInput");
                },
                "Not a valid BIC");
            }
        }

        [Test]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = TestStruct;
                var act = BusinessIdentifierCode.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void TryParse_InvalidInput_DefaultValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = default(BusinessIdentifierCode);
                var act = BusinessIdentifierCode.TryParse("InvalidInput");

                Assert.AreEqual(exp, act);
            }
        }

        #endregion

        #region (XML) (De)serialization tests

        [Test]
        public void Constructor_SerializationInfoIsNull_ThrowsArgumentNullException()
        {
            ExceptionAssert.CatchArgumentNullException
            (() =>
            {
                SerializationTest.DeserializeUsingConstructor<BusinessIdentifierCode>(null, default);
            },
            "info");
        }

        [Test]
        public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
        {
            Assert.Catch<SerializationException>
            (() =>
            {
                var info = new SerializationInfo(typeof(BusinessIdentifierCode), new System.Runtime.Serialization.FormatterConverter());
                SerializationTest.DeserializeUsingConstructor<BusinessIdentifierCode>(info, default);
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
            var info = new SerializationInfo(typeof(BusinessIdentifierCode), new System.Runtime.Serialization.FormatterConverter());
            obj.GetObjectData(info, default);

            Assert.AreEqual(TestStruct.ToString(), info.GetString("Value"));
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
            var exp = "AEGONL2UXXX";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void XmlDeserialize_XmlString_AreEqual()
        {
            var act = SerializationTest.XmlDeserialize<BusinessIdentifierCode>("AEGONL2UXXX");
            Assert.AreEqual(TestStruct, act);
        }


        [Test]
        public void SerializeDeserialize_BusinessIdentifierCodeSerializeObject_AreEqual()
        {
            var input = new BusinessIdentifierCodeSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new BusinessIdentifierCodeSerializeObject
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
        public void XmlSerializeDeserialize_BusinessIdentifierCodeSerializeObject_AreEqual()
        {
            var input = new BusinessIdentifierCodeSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new BusinessIdentifierCodeSerializeObject
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
        public void DataContractSerializeDeserialize_BusinessIdentifierCodeSerializeObject_AreEqual()
        {
            var input = new BusinessIdentifierCodeSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new BusinessIdentifierCodeSerializeObject
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
            var input = new BusinessIdentifierCodeSerializeObject
            {
                Id = 17,
                Obj = BusinessIdentifierCode.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new BusinessIdentifierCodeSerializeObject
            {
                Id = 17,
                Obj = BusinessIdentifierCode.Empty,
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
            var input = new BusinessIdentifierCodeSerializeObject
            {
                Id = 17,
                Obj = BusinessIdentifierCode.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new BusinessIdentifierCodeSerializeObject
            {
                Id = 17,
                Obj = BusinessIdentifierCode.Empty,
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
            var act = JsonTester.Read<BusinessIdentifierCode>();
            var exp = BusinessIdentifierCode.Empty;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_InvalidStringValue_AssertFormatException()
        {
            Assert.Catch<FormatException>(() =>
            {
                JsonTester.Read<BusinessIdentifierCode>("InvalidStringValue");
            },
            "Not a valid BIC");
        }
        [Test]
        public void FromJson_StringValue_AreEqual()
        {
            var act = JsonTester.Read<BusinessIdentifierCode>("AEGONL2UXXX");
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_Int64Value_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>(() =>
            {
                JsonTester.Read<BusinessIdentifierCode>(123456L);
            },
            "JSON deserialization from an integer is not supported.");
        }

        [Test]
        public void FromJson_DoubleValue_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>(() =>
            {
                JsonTester.Read<BusinessIdentifierCode>(1234.56);
            },
            "JSON deserialization from a number is not supported.");
        }

        [Test]
        public void FromJson_DateTimeValue_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>(() =>
            {
                JsonTester.Read<BusinessIdentifierCode>(new DateTime(1972, 02, 14));
            },
            "JSON deserialization from a date is not supported.");
        }

        [Test]
        public void ToJson_DefaultValue_IsNull()
        {
            object act = JsonTester.Write(default(BusinessIdentifierCode));
            Assert.IsNull(act);
        }
        [Test]
        public void ToJson_TestStruct_AreEqual()
        {
            var act = JsonTester.Write(TestStruct);
            var exp = "AEGONL2UXXX";
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IFormattable / ToString tests

        [Test]
        public void ToString_Empty_StringEmpty()
        {
            var act = BusinessIdentifierCode.Empty.ToString();
            var exp = "";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_Unknown_QuestionMark()
        {
            var act = BusinessIdentifierCode.Unknown.ToString();
            var exp = "?";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: 'AEGONL2UXXX', format: 'Unit Test Format'";

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToString_TestStruct_ComplexPattern()
        {
            var act = TestStruct.ToString("");
            var exp = "AEGONL2UXXX";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void DebuggerDisplay_DebugToString_HasAttribute()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(BusinessIdentifierCode));
        }

        [Test]
        public void DebuggerDisplay_DefaultValue_String()
        {
            DebuggerDisplayAssert.HasResult("{empty}", default(BusinessIdentifierCode));
        }
        [Test]
        public void DebuggerDisplay_Unknown_String()
        {
            DebuggerDisplayAssert.HasResult("?", BusinessIdentifierCode.Unknown);
        }

        [Test]
        public void DebuggerDisplay_TestStruct_String()
        {
            DebuggerDisplayAssert.HasResult("AEGONL2UXXX", TestStruct);
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for BusinessIdentifierCode.Empty.</summary>
        [Test]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, BusinessIdentifierCode.Empty.GetHashCode());
        }

        /// <summary>GetHash should not fail for the test struct.</summary>
        [Test]
        public void GetHash_TestStruct_NotZero()
        {
            Assert.NotZero(TestStruct.GetHashCode());
        }

        [Test]
        public void Equals_EmptyEmpty_IsTrue()
        {
            Assert.IsTrue(BusinessIdentifierCode.Empty.Equals(BusinessIdentifierCode.Empty));
        }

        [Test]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = BusinessIdentifierCode.Parse("AEGONL2UXXX", CultureInfo.InvariantCulture);
            var r = BusinessIdentifierCode.Parse("AEgonL2Uxxx", CultureInfo.InvariantCulture);

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
            Assert.IsFalse(TestStruct.Equals(BusinessIdentifierCode.Empty));
        }

        [Test]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(BusinessIdentifierCode.Empty.Equals(TestStruct));
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

        /// <summary>Orders a list of BICs ascending.</summary>
        [Test]
        public void OrderBy_BusinessIdentifierCode_AreEqual()
        {
            var item0 = BusinessIdentifierCode.Parse("AEGONL2UXXX");
            var item1 = BusinessIdentifierCode.Parse("CEBUNL2U");
            var item2 = BusinessIdentifierCode.Parse("DSSBNL22");
            var item3 = BusinessIdentifierCode.Parse("FTSBNL2R");

            var inp = new List<BusinessIdentifierCode> { BusinessIdentifierCode.Empty, item3, item2, item0, item1, BusinessIdentifierCode.Empty };
            var exp = new List<BusinessIdentifierCode> { BusinessIdentifierCode.Empty, BusinessIdentifierCode.Empty, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of BICs descending.</summary>
        [Test]
        public void OrderByDescending_BusinessIdentifierCode_AreEqual()
        {
            var item0 = BusinessIdentifierCode.Parse("AEGONL2UXXX");
            var item1 = BusinessIdentifierCode.Parse("CEBUNL2U");
            var item2 = BusinessIdentifierCode.Parse("DSSBNL22");
            var item3 = BusinessIdentifierCode.Parse("FTSBNL2R");

            var inp = new List<BusinessIdentifierCode> { BusinessIdentifierCode.Empty, item3, item2, item0, item1, BusinessIdentifierCode.Empty };
            var exp = new List<BusinessIdentifierCode> { item3, item2, item1, item0, BusinessIdentifierCode.Empty, BusinessIdentifierCode.Empty };
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
                "Argument must be BusinessIdentifierCode."
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
                "Argument must be BusinessIdentifierCode."
            );
        }

        #endregion

        #region Casting tests

        [Test]
        public void Explicit_StringToBusinessIdentifierCode_AreEqual()
        {
            var exp = TestStruct;
            var act = (BusinessIdentifierCode)TestStruct.ToString();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_BusinessIdentifierCodeToString_AreEqual()
        {
            var exp = TestStruct.ToString();
            var act = (string)TestStruct;

            Assert.AreEqual(exp, act);
        }

        #endregion

        #region Properties

        [Test]
        public void Length_DefaultValue_0()
        {
            var exp = 0;
            var act = BusinessIdentifierCode.Empty.Length;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Length_Unknown_0()
        {
            var exp = 0;
            var act = BusinessIdentifierCode.Unknown.Length;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Length_TestStruct_IntValue()
        {
            var exp = 11;
            var act = TestStruct.Length;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void BusinessCode_DefaultValue_StringEmpty()
        {
            var exp = "";
            var act = BusinessIdentifierCode.Empty.Business;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void BusinessCode_Unknown_StringEmpty()
        {
            var exp = "";
            var act = BusinessIdentifierCode.Unknown.Business;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void BusinessCode_TestStruct_AEGO()
        {
            var exp = "AEGO";
            var act = TestStruct.Business;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Country_DefaultValue_CountryEmpty()
        {
            var exp = Country.Empty;
            var act = BusinessIdentifierCode.Empty.Country;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Country_Unknown_CountryUnknown()
        {
            var exp = Country.Unknown;
            var act = BusinessIdentifierCode.Unknown.Country;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Country_TestStruct_NL()
        {
            var exp = Country.NL;
            var act = TestStruct.Country;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void LocationCode_DefaultValue_StringEmpty()
        {
            var exp = "";
            var act = BusinessIdentifierCode.Empty.Location;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void LocationCode_Unknown_StringEmpty()
        {
            var exp = "";
            var act = BusinessIdentifierCode.Unknown.Location;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void LocationCode_TestStruct_NL()
        {
            var exp = "2U";
            var act = TestStruct.Location;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void BranchCode_DefaultValue_StringEmpty()
        {
            var exp = "";
            var act = BusinessIdentifierCode.Empty.Branch;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void BranchCode_Unknown_StringEmpty()
        {
            var exp = "";
            var act = BusinessIdentifierCode.Unknown.Branch;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void BranchCode_TestStruct_NL()
        {
            var exp = "XXX";
            var act = TestStruct.Branch;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void BranchCode_AEGONL2U_StringEmpty()
        {
            var exp = "";
            var act = BusinessIdentifierCode.Parse("AEGONL2U").Branch;
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region Type converter tests

        [Test]
        public void ConverterExists_BusinessIdentifierCode_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(BusinessIdentifierCode));
        }

        [Test]
        public void CanNotConvertFromInt32_BusinessIdentifierCode_IsTrue()
        {
            TypeConverterAssert.CanNotConvertFrom(typeof(BusinessIdentifierCode), typeof(Int32));
        }

        [Test]
        public void CanNotConvertToInt32_BusinessIdentifierCode_IsTrue()
        {
            TypeConverterAssert.CanNotConvertTo(typeof(BusinessIdentifierCode), typeof(Int32));
        }

        [Test]
        public void CanConvertFromString_BusinessIdentifierCode_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(BusinessIdentifierCode));
        }

        [Test]
        public void CanConvertToString_BusinessIdentifierCode_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(BusinessIdentifierCode));
        }

        [Test]
        public void ConvertFrom_StringNull_BusinessIdentifierCodeEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(BusinessIdentifierCode.Empty, (string)null);
            }
        }

        [Test]
        public void ConvertFrom_StringEmpty_BusinessIdentifierCodeEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(BusinessIdentifierCode.Empty, string.Empty);
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

        #endregion

        #region IsValid


        [TestCase(null, "null")]
        [TestCase("", "string.Empty")]
        [TestCase("1AAANL01", "digit in first four characters")]
        [TestCase("AAAANLBB1", "Branch length of 1")]
        [TestCase("AAAANLBB12", "Branch length of 2")]
        [TestCase("ABCDXX01", "Not existing country")]
        [TestCase("AAAANLBË", "Diacritic")]
        public void IsValid_False(string str, string message)
        {
            Assert.IsFalse(BusinessIdentifierCode.IsValid(str), "{0}, {1}", str, message);
        }

        [TestCase("PSTBNL21")]
        [TestCase("ABNANL2A")]
        [TestCase("BACBBEBB")]
        [TestCase("GEBABEBB36A")]
        [TestCase("DEUTDEFF")]
        [TestCase("NEDSZAJJ")]
        [TestCase("DABADKKK")]
        [TestCase("UNCRIT2B912")]
        [TestCase("DSBACNBXSHA")]
        public void IsValid_True(string str)
        {
            Assert.IsTrue(BusinessIdentifierCode.IsValid(str), str);
        }

        #endregion
    }

    [Serializable]
    public class BusinessIdentifierCodeSerializeObject
    {
        public int Id { get; set; }
        public BusinessIdentifierCode Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
