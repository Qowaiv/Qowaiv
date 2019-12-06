﻿#pragma warning disable CS0618 // Type or member is obsolete
// This class only exists to test the obsolete BankIdentifierCode class.

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
    public class BankIdentifierCodeTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly BankIdentifierCode TestStruct = BankIdentifierCode.Parse("AEGONL2UXXX");

        #region BIC const tests

        /// <summary>BankIdentifierCode.Empty should be equal to the default of BIC.</summary>
        [Test]
        public void Empty_None_EqualsDefault()
        {
            Assert.AreEqual(default(BankIdentifierCode), BankIdentifierCode.Empty);
        }

        #endregion

        #region BIC IsEmpty tests

        /// <summary>BankIdentifierCode.IsEmpty() should be true for the default of BIC.</summary>
        [Test]
        public void IsEmpty_Default_IsTrue()
        {
            Assert.IsTrue(default(BankIdentifierCode).IsEmpty());
        }
        /// <summary>BankIdentifierCode.IsEmpty() should be false for BankIdentifierCode.Unknown.</summary>
        [Test]
        public void IsEmpty_Unknown_IsFalse()
        {
            Assert.IsFalse(BankIdentifierCode.Unknown.IsEmpty());
        }
        /// <summary>BankIdentifierCode.IsEmpty() should be false for the TestStruct.</summary>
        [Test]
        public void IsEmpty_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmpty());
        }

        /// <summary>BankIdentifierCode.IsUnknown() should be false for the default of BIC.</summary>
        [Test]
        public void IsUnknown_Default_IsFalse()
        {
            Assert.IsFalse(default(BankIdentifierCode).IsUnknown());
        }
        /// <summary>BankIdentifierCode.IsUnknown() should be true for BankIdentifierCode.Unknown.</summary>
        [Test]
        public void IsUnknown_Unknown_IsTrue()
        {
            Assert.IsTrue(BankIdentifierCode.Unknown.IsUnknown());
        }
        /// <summary>BankIdentifierCode.IsUnknown() should be false for the TestStruct.</summary>
        [Test]
        public void IsUnknown_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsUnknown());
        }

        /// <summary>BankIdentifierCode.IsEmptyOrUnknown() should be true for the default of BIC.</summary>
        [Test]
        public void IsEmptyOrUnknown_Default_IsFalse()
        {
            Assert.IsTrue(default(BankIdentifierCode).IsEmptyOrUnknown());
        }
        /// <summary>BankIdentifierCode.IsEmptyOrUnknown() should be true for BankIdentifierCode.Unknown.</summary>
        [Test]
        public void IsEmptyOrUnknown_Unknown_IsTrue()
        {
            Assert.IsTrue(BankIdentifierCode.Unknown.IsEmptyOrUnknown());
        }
        /// <summary>BankIdentifierCode.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
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
            Assert.IsTrue(BankIdentifierCode.TryParse(str, out BankIdentifierCode val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TyrParse_StringEmpty_IsValid()
        {
            string str = string.Empty;

            Assert.IsTrue(BankIdentifierCode.TryParse(str, out BankIdentifierCode val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse "?" should be valid and the result should be BankIdentifierCode.Unknown.</summary>
        [Test]
        public void TyrParse_Questionmark_IsValid()
        {
            string str = "?";
            Assert.IsTrue(BankIdentifierCode.TryParse(str, out BankIdentifierCode val), "Valid");
            Assert.IsTrue(val.IsUnknown(), "Value");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TyrParse_StringValue_IsValid()
        {
            string str = "AEGONL2UXXX";
            Assert.IsTrue(BankIdentifierCode.TryParse(str, out BankIdentifierCode val), "Valid");
            Assert.AreEqual(str, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [Test]
        public void TyrParse_StringValue_IsNotValid()
        {
            string str = "string";
            Assert.IsFalse(BankIdentifierCode.TryParse(str, out BankIdentifierCode val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        [Test]
        public void Parse_Unknown_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var act = BankIdentifierCode.Parse("?");
                var exp = BankIdentifierCode.Unknown;
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
                    BankIdentifierCode.Parse("InvalidInput");
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
                var act = BankIdentifierCode.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
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

        [Test]
        public void Constructor_SerializationInfoIsNull_ThrowsArgumentNullException()
        {
            ExceptionAssert.CatchArgumentNullException
            (() =>
            {
                SerializationTest.DeserializeUsingConstructor<BankIdentifierCode>(null, default);
            },
            "info");
        }

        [Test]
        public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
        {
            Assert.Catch<SerializationException>
            (() =>
            {
                var info = new SerializationInfo(typeof(BankIdentifierCode), new System.Runtime.Serialization.FormatterConverter());
                SerializationTest.DeserializeUsingConstructor<BankIdentifierCode>(info, default);
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
            var info = new SerializationInfo(typeof(BankIdentifierCode), new System.Runtime.Serialization.FormatterConverter());
            obj.GetObjectData(info, default);

            Assert.AreEqual(TestStruct.ToString(), info.GetString("Value"));
        }

        [Test]
        public void SerializeDeserialize_TestStruct_AreEqual()
        {
            var input = BankIdentifierCodeTest.TestStruct;
            var exp = BankIdentifierCodeTest.TestStruct;
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void DataContractSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = BankIdentifierCodeTest.TestStruct;
            var exp = BankIdentifierCodeTest.TestStruct;
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void XmlSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = BankIdentifierCodeTest.TestStruct;
            var exp = BankIdentifierCodeTest.TestStruct;
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void SerializeDeserialize_BankIdentifierCodeSerializeObject_AreEqual()
        {
            var input = new BankIdentifierCodeSerializeObject
            {
                Id = 17,
                Obj = BankIdentifierCodeTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new BankIdentifierCodeSerializeObject
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
        [Test]
        public void XmlSerializeDeserialize_BankIdentifierCodeSerializeObject_AreEqual()
        {
            var input = new BankIdentifierCodeSerializeObject
            {
                Id = 17,
                Obj = BankIdentifierCodeTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new BankIdentifierCodeSerializeObject
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
        [Test]
        public void DataContractSerializeDeserialize_BankIdentifierCodeSerializeObject_AreEqual()
        {
            var input = new BankIdentifierCodeSerializeObject
            {
                Id = 17,
                Obj = BankIdentifierCodeTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new BankIdentifierCodeSerializeObject
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

        [Test]
        public void SerializeDeserialize_Empty_AreEqual()
        {
            var input = new BankIdentifierCodeSerializeObject
            {
                Id = 17,
                Obj = BankIdentifierCode.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new BankIdentifierCodeSerializeObject
            {
                Id = 17,
                Obj = BankIdentifierCode.Empty,
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
            var input = new BankIdentifierCodeSerializeObject
            {
                Id = 17,
                Obj = BankIdentifierCode.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new BankIdentifierCodeSerializeObject
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
            Assert.Catch<FormatException>(() => JsonTester.Read<BankIdentifierCode>(json));
        }

        [Test]
        public void FromJson_AEGONL2UXXX_EqualsTestStruct()
        {
            var actual = JsonTester.Read<BankIdentifierCode>("AEGONL2UXXX");
            Assert.AreEqual(TestStruct, actual);
        }

        [Test]
        public void ToJson_DefaultValue_IsNull()
        {
            object act = JsonTester.Write(default(BankIdentifierCode));
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
            var act = BankIdentifierCode.Empty.ToString();
            var exp = "";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_Unknown_QuestionMark()
        {
            var act = BankIdentifierCode.Unknown.ToString();
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
            DebuggerDisplayAssert.HasAttribute(typeof(BankIdentifierCode));
        }

        [Test]
        public void DebuggerDisplay_DefaultValue_String()
        {
            DebuggerDisplayAssert.HasResult("{empty}", default(BankIdentifierCode));
        }
        [Test]
        public void DebuggerDisplay_Unknown_String()
        {
            DebuggerDisplayAssert.HasResult("?", BankIdentifierCode.Unknown);
        }

        [Test]
        public void DebuggerDisplay_TestStruct_String()
        {
            DebuggerDisplayAssert.HasResult("AEGONL2UXXX", TestStruct);
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for BankIdentifierCode.Empty.</summary>
        [Test]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, BankIdentifierCode.Empty.GetHashCode());
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
            Assert.IsTrue(BankIdentifierCode.Empty.Equals(BankIdentifierCode.Empty));
        }

        [Test]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = BankIdentifierCode.Parse("AEGONL2UXXX", CultureInfo.InvariantCulture);
            var r = BankIdentifierCode.Parse("AEgonL2Uxxx", CultureInfo.InvariantCulture);

            Assert.IsTrue(l.Equals(r));
        }

        [Test]
        public void Equals_TestStructTestStruct_IsTrue()
        {
            Assert.IsTrue(BankIdentifierCodeTest.TestStruct.Equals(BankIdentifierCodeTest.TestStruct));
        }

        [Test]
        public void Equals_TestStructEmpty_IsFalse()
        {
            Assert.IsFalse(BankIdentifierCodeTest.TestStruct.Equals(BankIdentifierCode.Empty));
        }

        [Test]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(BankIdentifierCode.Empty.Equals(BankIdentifierCodeTest.TestStruct));
        }

        [Test]
        public void Equals_TestStructObjectTestStruct_IsTrue()
        {
            Assert.IsTrue(BankIdentifierCodeTest.TestStruct.Equals((object)BankIdentifierCodeTest.TestStruct));
        }

        [Test]
        public void Equals_TestStructNull_IsFalse()
        {
            Assert.IsFalse(BankIdentifierCodeTest.TestStruct.Equals(null));
        }

        [Test]
        public void Equals_TestStructObject_IsFalse()
        {
            Assert.IsFalse(BankIdentifierCodeTest.TestStruct.Equals(new object()));
        }

        [Test]
        public void OperatorIs_TestStructTestStruct_IsTrue()
        {
            var l = BankIdentifierCodeTest.TestStruct;
            var r = BankIdentifierCodeTest.TestStruct;
            Assert.IsTrue(l == r);
        }

        [Test]
        public void OperatorIsNot_TestStructTestStruct_IsFalse()
        {
            var l = BankIdentifierCodeTest.TestStruct;
            var r = BankIdentifierCodeTest.TestStruct;
            Assert.IsFalse(l != r);
        }

        #endregion

        #region IComparable tests

        /// <summary>Orders a list of BICs ascending.</summary>
        [Test]
        public void OrderBy_BankIdentifierCode_AreEqual()
        {
            var item0 = BankIdentifierCode.Parse("AEGONL2UXXX");
            var item1 = BankIdentifierCode.Parse("CEBUNL2U");
            var item2 = BankIdentifierCode.Parse("DSSBNL22");
            var item3 = BankIdentifierCode.Parse("FTSBNL2R");

            var inp = new List<BankIdentifierCode> { BankIdentifierCode.Empty, item3, item2, item0, item1, BankIdentifierCode.Empty };
            var exp = new List<BankIdentifierCode> { BankIdentifierCode.Empty, BankIdentifierCode.Empty, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of BICs descending.</summary>
        [Test]
        public void OrderByDescending_BankIdentifierCode_AreEqual()
        {
            var item0 = BankIdentifierCode.Parse("AEGONL2UXXX");
            var item1 = BankIdentifierCode.Parse("CEBUNL2U");
            var item2 = BankIdentifierCode.Parse("DSSBNL22");
            var item3 = BankIdentifierCode.Parse("FTSBNL2R");

            var inp = new List<BankIdentifierCode> { BankIdentifierCode.Empty, item3, item2, item0, item1, BankIdentifierCode.Empty };
            var exp = new List<BankIdentifierCode> { item3, item2, item1, item0, BankIdentifierCode.Empty, BankIdentifierCode.Empty };
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
                "Argument must be BankIdentifierCode."
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
                "Argument must be BankIdentifierCode."
            );
        }

        #endregion

        #region Casting tests

        [Test]
        public void Explicit_StringToBankIdentifierCode_AreEqual()
        {
            var exp = TestStruct;
            var act = (BankIdentifierCode)TestStruct.ToString();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_BankIdentifierCodeToString_AreEqual()
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
            var act = BankIdentifierCode.Empty.Length;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Length_Unknown_0()
        {
            var exp = 0;
            var act = BankIdentifierCode.Unknown.Length;
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
        public void BankCode_DefaultValue_StringEmpty()
        {
            var exp = "";
            var act = BankIdentifierCode.Empty.BankCode;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void BankCode_Unknown_StringEmpty()
        {
            var exp = "";
            var act = BankIdentifierCode.Unknown.BankCode;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void BankCode_TestStruct_AEGO()
        {
            var exp = "AEGO";
            var act = TestStruct.BankCode;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void CountryCode_DefaultValue_StringEmpty()
        {
            var exp = "";
            var act = BankIdentifierCode.Empty.CountryCode;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void CountryCode_Unknown_StringEmpty()
        {
            var exp = "";
            var act = BankIdentifierCode.Unknown.CountryCode;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void CountryCode_TestStruct_NL()
        {
            var exp = "NL";
            var act = TestStruct.CountryCode;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Country_DefaultValue_CountryEmpty()
        {
            var exp = Country.Empty;
            var act = BankIdentifierCode.Empty.Country;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Country_Unknown_CountryUnknown()
        {
            var exp = Country.Unknown;
            var act = BankIdentifierCode.Unknown.Country;
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
            var act = BankIdentifierCode.Empty.LocationCode;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void LocationCode_Unknown_StringEmpty()
        {
            var exp = "";
            var act = BankIdentifierCode.Unknown.LocationCode;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void LocationCode_TestStruct_NL()
        {
            var exp = "2U";
            var act = TestStruct.LocationCode;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void BranchCode_DefaultValue_StringEmpty()
        {
            var exp = "";
            var act = BankIdentifierCode.Empty.BranchCode;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void BranchCode_Unknown_StringEmpty()
        {
            var exp = "";
            var act = BankIdentifierCode.Unknown.BranchCode;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void BranchCode_TestStruct_NL()
        {
            var exp = "XXX";
            var act = TestStruct.BranchCode;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void BranchCode_AEGONL2U_StringEmpty()
        {
            var exp = "";
            var act = BankIdentifierCode.Parse("AEGONL2U").BranchCode;
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region Type converter tests

        [Test]
        public void ConverterExists_BankIdentifierCode_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(BankIdentifierCode));
        }

        [Test]
        public void CanNotConvertFromInt32_BankIdentifierCode_IsTrue()
        {
            TypeConverterAssert.CanNotConvertFrom(typeof(BankIdentifierCode), typeof(Int32));
        }

        [Test]
        public void CanNotConvertToInt32_BankIdentifierCode_IsTrue()
        {
            TypeConverterAssert.CanNotConvertTo(typeof(BankIdentifierCode), typeof(Int32));
        }

        [Test]
        public void CanConvertFromString_BankIdentifierCode_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(BankIdentifierCode));
        }

        [Test]
        public void CanConvertToString_BankIdentifierCode_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(BankIdentifierCode));
        }

        [Test]
        public void ConvertFrom_StringNull_BankIdentifierCodeEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(BankIdentifierCode.Empty, (string)null);
            }
        }

        [Test]
        public void ConvertFrom_StringEmpty_BankIdentifierCodeEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(BankIdentifierCode.Empty, string.Empty);
            }
        }

        [Test]
        public void ConvertFromString_StringValue_TestStruct()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(BankIdentifierCodeTest.TestStruct, BankIdentifierCodeTest.TestStruct.ToString());
            }
        }

        [Test]
        public void ConvertToString_TestStruct_StringValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertToStringEquals(BankIdentifierCodeTest.TestStruct.ToString(), BankIdentifierCodeTest.TestStruct);
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
            Assert.IsFalse(BankIdentifierCode.IsValid(str), "{0}, {1}", str, message);
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
            Assert.IsTrue(BankIdentifierCode.IsValid(str), str);
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
