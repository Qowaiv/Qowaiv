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
    /// <summary>Tests the BIC SVO.</summary>
    [TestClass]
    public class BankIdentifierCodeTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly BankIdentifierCode TestStruct = BankIdentifierCode.Parse("AEGONL2UXXX");

        #region BIC const tests

        /// <summary>BankIdentifierCode.Empty should be equal to the default of BIC.</summary>
        [TestMethod]
        public void Empty_None_EqualsDefault()
        {
            Assert.AreEqual(default(BankIdentifierCode), BankIdentifierCode.Empty);
        }

        #endregion

        #region BIC IsEmpty tests

        /// <summary>BankIdentifierCode.IsEmpty() should be true for the default of BIC.</summary>
        [TestMethod]
        public void IsEmpty_Default_IsTrue()
        {
            Assert.IsTrue(default(BankIdentifierCode).IsEmpty());
        }
        /// <summary>BankIdentifierCode.IsEmpty() should be false for BankIdentifierCode.Unknown.</summary>
        [TestMethod]
        public void IsEmpty_Unknown_IsFalse()
        {
            Assert.IsFalse(BankIdentifierCode.Unknown.IsEmpty());
        }
        /// <summary>BankIdentifierCode.IsEmpty() should be false for the TestStruct.</summary>
        [TestMethod]
        public void IsEmpty_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmpty());
        }

        /// <summary>BankIdentifierCode.IsUnknown() should be false for the default of BIC.</summary>
        [TestMethod]
        public void IsUnknown_Default_IsFalse()
        {
            Assert.IsFalse(default(BankIdentifierCode).IsUnknown());
        }
        /// <summary>BankIdentifierCode.IsUnknown() should be true for BankIdentifierCode.Unknown.</summary>
        [TestMethod]
        public void IsUnknown_Unknown_IsTrue()
        {
            Assert.IsTrue(BankIdentifierCode.Unknown.IsUnknown());
        }
        /// <summary>BankIdentifierCode.IsUnknown() should be false for the TestStruct.</summary>
        [TestMethod]
        public void IsUnknown_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsUnknown());
        }

        /// <summary>BankIdentifierCode.IsEmptyOrUnknown() should be true for the default of BIC.</summary>
        [TestMethod]
        public void IsEmptyOrUnknown_Default_IsFalse()
        {
            Assert.IsTrue(default(BankIdentifierCode).IsEmptyOrUnknown());
        }
        /// <summary>BankIdentifierCode.IsEmptyOrUnknown() should be true for BankIdentifierCode.Unknown.</summary>
        [TestMethod]
        public void IsEmptyOrUnknown_Unknown_IsTrue()
        {
            Assert.IsTrue(BankIdentifierCode.Unknown.IsEmptyOrUnknown());
        }
        /// <summary>BankIdentifierCode.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
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
            BankIdentifierCode val;

            string str = null;

            Assert.IsTrue(BankIdentifierCode.TryParse(str, out val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [TestMethod]
        public void TyrParse_StringEmpty_IsValid()
        {
            BankIdentifierCode val;

            string str = string.Empty;

            Assert.IsTrue(BankIdentifierCode.TryParse(str, out val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse "?" should be valid and the result should be BankIdentifierCode.Unknown.</summary>
        [TestMethod]
        public void TyrParse_Questionmark_IsValid()
        {
            BankIdentifierCode val;

            string str = "?";

            Assert.IsTrue(BankIdentifierCode.TryParse(str, out val), "Valid");
            Assert.IsTrue(val.IsUnknown(), "Value");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [TestMethod]
        public void TyrParse_StringValue_IsValid()
        {
            BankIdentifierCode val;

            string str = "AEGONL2UXXX";

            Assert.IsTrue(BankIdentifierCode.TryParse(str, out val), "Valid");
            Assert.AreEqual(str, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [TestMethod]
        public void TyrParse_StringValue_IsNotValid()
        {
            BankIdentifierCode val;

            string str = "string";

            Assert.IsFalse(BankIdentifierCode.TryParse(str, out val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }
        
        [TestMethod]
        public void Parse_Unknown_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var act = BankIdentifierCode.Parse("?");
                var exp = BankIdentifierCode.Unknown;
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
                    BankIdentifierCode.Parse("InvalidInput");
                },
                "Not a valid BIC");
            }
        }

        [TestMethod]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = TestStruct;
                var act = BankIdentifierCode.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [TestMethod]
        public void TryParse_InvalidInput_DefaultValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = default(BankIdentifierCode);
                var act = BankIdentifierCode.TryParse("InvalidInput");

                Assert.AreEqual(exp, act);
            }
        }

        #endregion

        #region (XML) (De)serialization tests

		[TestMethod]
		public void Constructor_SerializationInfoIsNull_ThrowsArgumentNullException()
		{
			ExceptionAssert.ExpectArgumentNullException
			(() =>
			{
				SerializationTest.DeserializeUsingConstructor<BankIdentifierCode>(null, default(StreamingContext));
			},
			"info");
		}
		
		[TestMethod]
        public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
        {
            ExceptionAssert.ExpectException<SerializationException>
            (() =>
            {
                var info = new SerializationInfo(typeof(BankIdentifierCode), new System.Runtime.Serialization.FormatterConverter());
				SerializationTest.DeserializeUsingConstructor<BankIdentifierCode>(info, default(StreamingContext));
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
            var info = new SerializationInfo(typeof(BankIdentifierCode), new System.Runtime.Serialization.FormatterConverter());
            obj.GetObjectData(info, default(StreamingContext));

            Assert.AreEqual(TestStruct.ToString(), info.GetString("Value"));
        }
		
        [TestMethod]
        public void SerializeDeserialize_TestStruct_AreEqual()
        {
            var input = BankIdentifierCodeTest.TestStruct;
            var exp = BankIdentifierCodeTest.TestStruct;
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void DataContractSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = BankIdentifierCodeTest.TestStruct;
            var exp = BankIdentifierCodeTest.TestStruct;
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void XmlSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = BankIdentifierCodeTest.TestStruct;
            var exp = BankIdentifierCodeTest.TestStruct;
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void SerializeDeserialize_BankIdentifierCodeSerializeObject_AreEqual()
        {
            var input = new BankIdentifierCodeSerializeObject()
            {
                Id = 17,
                Obj = BankIdentifierCodeTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new BankIdentifierCodeSerializeObject()
            {
                Id = 17,
                Obj = BankIdentifierCodeTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [TestMethod]
        public void XmlSerializeDeserialize_BankIdentifierCodeSerializeObject_AreEqual()
        {
            var input = new BankIdentifierCodeSerializeObject()
            {
                Id = 17,
                Obj = BankIdentifierCodeTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new BankIdentifierCodeSerializeObject()
            {
                Id = 17,
                Obj = BankIdentifierCodeTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [TestMethod]
        public void DataContractSerializeDeserialize_BankIdentifierCodeSerializeObject_AreEqual()
        {
            var input = new BankIdentifierCodeSerializeObject()
            {
                Id = 17,
                Obj = BankIdentifierCodeTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new BankIdentifierCodeSerializeObject()
            {
                Id = 17,
                Obj = BankIdentifierCodeTest.TestStruct,
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
            var input = new BankIdentifierCodeSerializeObject()
            {
                Id = 17,
                Obj = BankIdentifierCodeTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new BankIdentifierCodeSerializeObject()
            {
                Id = 17,
                Obj = BankIdentifierCodeTest.TestStruct,
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
            var input = new  BankIdentifierCodeSerializeObject()
            {
                Id = 17,
                Obj = BankIdentifierCode.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new BankIdentifierCodeSerializeObject()
            {
                Id = 17,
                Obj = BankIdentifierCode.Empty,
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
            var act = JsonTester.Read<BankIdentifierCode>();
            var exp = BankIdentifierCode.Empty;
            
            Assert.AreEqual(exp, act);
        }
        
        [TestMethod]
        public void FromJson_InvalidStringValue_AssertFormatException()
        {
            ExceptionAssert.ExpectException<FormatException>(() =>
            {
                JsonTester.Read<BankIdentifierCode>("InvalidStringValue");
            },
            "Not a valid BIC");
        }
        [TestMethod]
        public void FromJson_StringValue_AreEqual()
        {
            var act = JsonTester.Read<BankIdentifierCode>(TestStruct.ToString(CultureInfo.InvariantCulture));
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }
                
        [TestMethod]
        public void FromJson_Int64Value_AssertNotSupportedException()
        {
            ExceptionAssert.ExpectException<NotSupportedException>(() =>
            {
                JsonTester.Read<BankIdentifierCode>(123456L);
            },
            "JSON deserialization from an integer is not supported.");
        }
        
        [TestMethod]
        public void FromJson_DoubleValue_AssertNotSupportedException()
        {
            ExceptionAssert.ExpectException<NotSupportedException>(() =>
            {
                JsonTester.Read<BankIdentifierCode>(1234.56);
            },
            "JSON deserialization from a number is not supported.");
        }
                
        [TestMethod]
        public void FromJson_DateTimeValue_AssertNotSupportedException()
        {
            ExceptionAssert.ExpectException<NotSupportedException>(() =>
            {
                JsonTester.Read<BankIdentifierCode>(new DateTime(1972, 02, 14));
            },
            "JSON deserialization from a date is not supported.");
        }

        [TestMethod]
        public void ToJson_DefaultValue_AreEqual()
        {
            object act = JsonTester.Write(default(BankIdentifierCode));
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
        public void ToString_Empty_StringEmpty()
        {
            var act = BankIdentifierCode.Empty.ToString();
            var exp = "";
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void ToString_Unknown_QuestionMark()
        {
            var act = BankIdentifierCode.Unknown.ToString();
            var exp = "?";
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: 'AEGONL2UXXX', format: 'Unit Test Format'";

        Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void ToString_TestStruct_ComplexPattern()
        {
            var act = TestStruct.ToString("");
            var exp = "AEGONL2UXXX";
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void DebuggerDisplay_DebugToString_HasAttribute()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(BankIdentifierCode));
        }

        [TestMethod]
        public void DebugToString_DefaultValue_String()
        {
            DebuggerDisplayAssert.HasResult("BankIdentifierCode: (empty)", default(BankIdentifierCode));
        }
        [TestMethod]
        public void DebugToString_Unknown_String()
        {
            DebuggerDisplayAssert.HasResult("BankIdentifierCode: (unknown)", BankIdentifierCode.Unknown);
        }

        [TestMethod]
        public void DebugToString_TestStruct_String()
        {
            DebuggerDisplayAssert.HasResult("BankIdentifierCode: AEGONL2UXXX", TestStruct);
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for BankIdentifierCode.Empty.</summary>
        [TestMethod]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, BankIdentifierCode.Empty.GetHashCode());
        }

        /// <summary>GetHash should not fail for the test struct.</summary>
        [TestMethod]
        public void GetHash_TestStruct_Hash()
        {
            Assert.AreEqual(-845590006, BankIdentifierCodeTest.TestStruct.GetHashCode());
        }

        [TestMethod]
        public void Equals_EmptyEmpty_IsTrue()
        {
            Assert.IsTrue(BankIdentifierCode.Empty.Equals(BankIdentifierCode.Empty));
        }

        [TestMethod]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = BankIdentifierCode.Parse("AEGONL2UXXX", CultureInfo.InvariantCulture);
            var r = BankIdentifierCode.Parse("AEgonL2Uxxx", CultureInfo.InvariantCulture);

            Assert.IsTrue(l.Equals(r));
        }

        [TestMethod]
        public void Equals_TestStructTestStruct_IsTrue()
        {
            Assert.IsTrue(BankIdentifierCodeTest.TestStruct.Equals(BankIdentifierCodeTest.TestStruct));
        }

        [TestMethod]
        public void Equals_TestStructEmpty_IsFalse()
        {
            Assert.IsFalse(BankIdentifierCodeTest.TestStruct.Equals(BankIdentifierCode.Empty));
        }

        [TestMethod]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(BankIdentifierCode.Empty.Equals(BankIdentifierCodeTest.TestStruct));
        }

        [TestMethod]
        public void Equals_TestStructObjectTestStruct_IsTrue()
        {
            Assert.IsTrue(BankIdentifierCodeTest.TestStruct.Equals((object)BankIdentifierCodeTest.TestStruct));
        }

        [TestMethod]
        public void Equals_TestStructNull_IsFalse()
        {
            Assert.IsFalse(BankIdentifierCodeTest.TestStruct.Equals(null));
        }

        [TestMethod]
        public void Equals_TestStructObject_IsFalse()
        {
            Assert.IsFalse(BankIdentifierCodeTest.TestStruct.Equals(new object()));
        }

        [TestMethod]
        public void OperatorIs_TestStructTestStruct_IsTrue()
        {
            var l = BankIdentifierCodeTest.TestStruct;
            var r = BankIdentifierCodeTest.TestStruct;
            Assert.IsTrue(l == r);
        }

        [TestMethod]
        public void OperatorIsNot_TestStructTestStruct_IsFalse()
        {
            var l = BankIdentifierCodeTest.TestStruct;
            var r = BankIdentifierCodeTest.TestStruct;
            Assert.IsFalse(l != r);
        }

        #endregion
        
        #region IComparable tests

        /// <summary>Orders a list of BICs ascending.</summary>
        [TestMethod]
        public void OrderBy_BankIdentifierCode_AreEqual()
        {
            var item0 = BankIdentifierCode.Parse("AEGONL2UXXX");
            var item1 = BankIdentifierCode.Parse("CEBUNL2U");
            var item2 = BankIdentifierCode.Parse("DSSBNL22");
            var item3 = BankIdentifierCode.Parse("FTSBNL2R");

            var inp = new List<BankIdentifierCode>() { BankIdentifierCode.Empty, item3, item2, item0, item1, BankIdentifierCode.Empty };
            var exp = new List<BankIdentifierCode>() { BankIdentifierCode.Empty, BankIdentifierCode.Empty, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of BICs descending.</summary>
        [TestMethod]
        public void OrderByDescending_BankIdentifierCode_AreEqual()
        {
            var item0 = BankIdentifierCode.Parse("AEGONL2UXXX");
            var item1 = BankIdentifierCode.Parse("CEBUNL2U");
            var item2 = BankIdentifierCode.Parse("DSSBNL22");
            var item3 = BankIdentifierCode.Parse("FTSBNL2R");

            var inp = new List<BankIdentifierCode>() { BankIdentifierCode.Empty, item3, item2, item0, item1, BankIdentifierCode.Empty };
            var exp = new List<BankIdentifierCode>() { item3, item2, item1, item0, BankIdentifierCode.Empty, BankIdentifierCode.Empty };
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
                "Argument must be a BIC"
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
                "Argument must be a BIC"
            );
        }
        #endregion
        
        #region Casting tests

        [TestMethod]
        public void Explicit_StringToBankIdentifierCode_AreEqual()
        {
            var exp = TestStruct;
            var act = (BankIdentifierCode)TestStruct.ToString();

            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void Explicit_BankIdentifierCodeToString_AreEqual()
        {
            var exp = TestStruct.ToString();
            var act = (string)TestStruct;

            Assert.AreEqual(exp, act);
        }

        #endregion

        #region Properties

        [TestMethod]
        public void Length_DefaultValue_0()
        {
            var exp = 0;
            var act = BankIdentifierCode.Empty.Length;
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void Length_Unknown_0()
        {
            var exp = 0;
            var act = BankIdentifierCode.Unknown.Length;
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void Length_TestStruct_IntValue()
        {
            var exp = 11;
            var act = TestStruct.Length;
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void BankCode_DefaultValue_StringEmpty()
        {
            var exp = "";
            var act = BankIdentifierCode.Empty.BankCode;
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void BankCode_Unknown_StringEmpty()
        {
            var exp = "";
            var act = BankIdentifierCode.Unknown.BankCode;
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void BankCode_TestStruct_AEGO()
        {
            var exp = "AEGO";
            var act = TestStruct.BankCode;
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void CountryCode_DefaultValue_StringEmpty()
        {
            var exp = "";
            var act = BankIdentifierCode.Empty.CountryCode;
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void CountryCode_Unknown_StringEmpty()
        {
            var exp = "";
            var act = BankIdentifierCode.Unknown.CountryCode;
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void CountryCode_TestStruct_NL()
        {
            var exp = "NL";
            var act = TestStruct.CountryCode;
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void Country_DefaultValue_CountryEmpty()
        {
            var exp = Country.Empty;
            var act = BankIdentifierCode.Empty.Country;
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void Country_Unknown_CountryUnknown()
        {
            var exp = Country.Unknown;
            var act = BankIdentifierCode.Unknown.Country;
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void Country_TestStruct_NL()
        {
            var exp = Country.NL;
            var act = TestStruct.Country;
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void LocationCode_DefaultValue_StringEmpty()
        {
            var exp = "";
            var act = BankIdentifierCode.Empty.LocationCode;
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void LocationCode_Unknown_StringEmpty()
        {
            var exp = "";
            var act = BankIdentifierCode.Unknown.LocationCode;
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void LocationCode_TestStruct_NL()
        {
            var exp = "2U";
            var act = TestStruct.LocationCode;
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void BranchCode_DefaultValue_StringEmpty()
        {
            var exp = "";
            var act = BankIdentifierCode.Empty.BranchCode;
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void BranchCode_Unknown_StringEmpty()
        {
            var exp = "";
            var act = BankIdentifierCode.Unknown.BranchCode;
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void BranchCode_TestStruct_NL()
        {
            var exp = "XXX";
            var act = TestStruct.BranchCode;
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void BranchCode_AEGONL2U_StringEmpty()
        {
            var exp = "";
            var act = BankIdentifierCode.Parse("AEGONL2U").BranchCode;
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region Type converter tests

        [TestMethod]
        public void ConverterExists_BankIdentifierCode_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(BankIdentifierCode));
        }

        [TestMethod]
        public void CanNotConvertFromInt32_BankIdentifierCode_IsTrue()
        {
        TypeConverterAssert.CanNotConvertFrom(typeof(BankIdentifierCode), typeof(Int32));
        }
        [TestMethod]
        public void CanNotConvertToInt32_BankIdentifierCode_IsTrue()
        {
        TypeConverterAssert.CanNotConvertTo(typeof(BankIdentifierCode), typeof(Int32));
        }

        [TestMethod]
        public void CanConvertFromString_BankIdentifierCode_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(BankIdentifierCode));
        }

        [TestMethod]
        public void CanConvertToString_BankIdentifierCode_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(BankIdentifierCode));
        }

        [TestMethod]
        public void ConvertFrom_StringNull_BankIdentifierCodeEmpty()
        {
			using (new CultureInfoScope("en-GB"))
            {
				TypeConverterAssert.ConvertFromEquals(BankIdentifierCode.Empty, (string)null);
			}
        }

        [TestMethod]
        public void ConvertFrom_StringEmpty_BankIdentifierCodeEmpty()
        {
			using (new CultureInfoScope("en-GB"))
            {
				TypeConverterAssert.ConvertFromEquals(BankIdentifierCode.Empty, string.Empty);
			}
        }

        [TestMethod]
        public void ConvertFromString_StringValue_TestStruct()
        {
			using (new CultureInfoScope("en-GB"))
            {
				TypeConverterAssert.ConvertFromEquals(BankIdentifierCodeTest.TestStruct, BankIdentifierCodeTest.TestStruct.ToString());
			}
        }

        [TestMethod]
        public void ConvertToString_TestStruct_StringValue()
        {
			using (new CultureInfoScope("en-GB"))
            {
				TypeConverterAssert.ConvertToStringEquals(BankIdentifierCodeTest.TestStruct.ToString(), BankIdentifierCodeTest.TestStruct);
			}
        }

        #endregion

        #region IsValid

        [TestMethod]
        public void IsValid_Data_IsFalse()
        {
            Assert.IsFalse(BankIdentifierCode.IsValid("1AAANL01"), "1AAANL01, cijfer in eerste vier");
            Assert.IsFalse(BankIdentifierCode.IsValid("AAAANLBB1"), "AAAANLBB1, lengte van 1 voor branch");
            Assert.IsFalse(BankIdentifierCode.IsValid("AAAANLBB12"), "AAAANLBB12, lengte van 2 voor branch");
            Assert.IsFalse(BankIdentifierCode.IsValid("ABCDXX01"), "ABCD1E01, cijfer in landcode");
            Assert.IsFalse(BankIdentifierCode.IsValid("ABCDXX01"), "ABCDXX01, niet bestaand land");
            Assert.IsFalse(BankIdentifierCode.IsValid("AAAANLBË"), "AAAANLBË, diacriet");

            Assert.IsFalse(BankIdentifierCode.IsValid((String)null), "(String)null");
        }
        [TestMethod]
        public void IsValid_Data_IsTrue()
        {
            Assert.IsTrue(BankIdentifierCode.IsValid("PSTBNL21"), "PSTBNL21");
            Assert.IsTrue(BankIdentifierCode.IsValid("ABNANL2A"), "ABNANL2A");
            Assert.IsTrue(BankIdentifierCode.IsValid("BACBBEBB"), "BACBBEBB");
            Assert.IsTrue(BankIdentifierCode.IsValid("GEBABEBB36A"), "GEBABEBB36A");
            Assert.IsTrue(BankIdentifierCode.IsValid("DEUTDEFF"), "DEUTDEFF");
            Assert.IsTrue(BankIdentifierCode.IsValid("NEDSZAJJ"), "NEDSZAJJ");
            Assert.IsTrue(BankIdentifierCode.IsValid("DABADKKK"), "DABADKKK");
            Assert.IsTrue(BankIdentifierCode.IsValid("UNCRIT2B912"), "UNCRIT2B912");
            Assert.IsTrue(BankIdentifierCode.IsValid("DSBACNBXSHA"), "DSBACNBXSHA");
        }

        #endregion
    }

    [Serializable]
    public class BankIdentifierCodeSerializeObject
    {
        public int Id { get; set; }
        public BankIdentifierCode Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
