using NUnit.Framework;
using Qowaiv.Financial;
using Qowaiv.Globalization;
using Qowaiv.TestTools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests.Financial
{
    [TestFixture]
    public class InternationalBankAccountNumberTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly InternationalBankAccountNumber TestStruct = InternationalBankAccountNumber.Parse("NL20 INGB 0001 2345 67");

        #region IBAN const tests

        /// <summary>InternationalBankAccountNumber.Empty should be equal to the default of IBAN.</summary>
        [Test]
        public void Empty_None_EqualsDefault()
        {
            Assert.AreEqual(default(InternationalBankAccountNumber), InternationalBankAccountNumber.Empty);
        }

        #endregion

        #region IBAN IsEmpty tests

        /// <summary>InternationalBankAccountNumber.IsEmpty() should true for the default of IBAN.</summary>
        [Test]
        public void IsEmpty_Default_IsTrue()
        {
            Assert.IsTrue(default(InternationalBankAccountNumber).IsEmpty());
        }

        /// <summary>InternationalBankAccountNumber.IsEmpty() should false for the TestStruct.</summary>
        [Test]
        public void IsEmpty_Default_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmpty());
        }

        #endregion

        #region TryParse tests

        /// <summary>TryParse null should be valid.</summary>
        [Test]
        public void TyrParse_Null_IsValid()
        {
            string str = null;
            Assert.IsTrue(InternationalBankAccountNumber.TryParse(str, out var val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TyrParse_StringEmpty_IsValid()
        {
            string str = string.Empty;
            Assert.IsTrue(InternationalBankAccountNumber.TryParse(str, out var val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TyrParse_StringValue_IsValid()
        {
            string str = "NL20INGB0001234567";
            Assert.IsTrue(InternationalBankAccountNumber.TryParse(str, out var val), "Valid");
            Assert.AreEqual(str, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TyrParse_QuestionMark_IsValid()
        {
            string str = "?";
            Assert.IsTrue(InternationalBankAccountNumber.TryParse(str, out var val), "Valid");
            Assert.AreEqual(str, val.ToString(), "Value.ToString()");
            Assert.AreEqual(InternationalBankAccountNumber.Unknown, val, "Value");
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [Test]
        public void TyrParse_StringValue_IsNotValid()
        {
            string str = "string";
            Assert.IsFalse(InternationalBankAccountNumber.TryParse(str, out var val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        [Test]
        public void Parse_InvalidInput_ThrowsFormatException()
        {
            using (new CultureInfoScope("en-GB"))
            {
                Assert.Catch<FormatException>
                (() =>
                {
                    InternationalBankAccountNumber.Parse("InvalidInput");
                },
                "Not a valid IBAN");
            }
        }

        [Test]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = TestStruct;
                var act = InternationalBankAccountNumber.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void TryParse_InvalidInput_DefaultValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = default(InternationalBankAccountNumber);
                var act = InternationalBankAccountNumber.TryParse("InvalidInput");

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
                SerializationTest.DeserializeUsingConstructor<InternationalBankAccountNumber>(null, default);
            },
            "info");
        }

        [Test]
        public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
        {
            Assert.Catch<SerializationException>
            (() =>
            {
                var info = new SerializationInfo(typeof(InternationalBankAccountNumber), new System.Runtime.Serialization.FormatterConverter());
                SerializationTest.DeserializeUsingConstructor<InternationalBankAccountNumber>(info, default);
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
        public void SerializeDeserialize_TestStruct_AreEqual()
        {
            var input = InternationalBankAccountNumberTest.TestStruct;
            var exp = InternationalBankAccountNumberTest.TestStruct;
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void DataContractSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = InternationalBankAccountNumberTest.TestStruct;
            var exp = InternationalBankAccountNumberTest.TestStruct;
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void XmlSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = InternationalBankAccountNumberTest.TestStruct;
            var exp = InternationalBankAccountNumberTest.TestStruct;
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void SerializeDeserialize_InternationalBankAccountNumberSerializeObject_AreEqual()
        {
            var input = new InternationalBankAccountNumberSerializeObject()
            {
                Id = 17,
                Obj = InternationalBankAccountNumberTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new InternationalBankAccountNumberSerializeObject()
            {
                Id = 17,
                Obj = InternationalBankAccountNumberTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void XmlSerializeDeserialize_InternationalBankAccountNumberSerializeObject_AreEqual()
        {
            var input = new InternationalBankAccountNumberSerializeObject()
            {
                Id = 17,
                Obj = InternationalBankAccountNumberTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new InternationalBankAccountNumberSerializeObject()
            {
                Id = 17,
                Obj = InternationalBankAccountNumberTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void DataContractSerializeDeserialize_InternationalBankAccountNumberSerializeObject_AreEqual()
        {
            var input = new InternationalBankAccountNumberSerializeObject()
            {
                Id = 17,
                Obj = InternationalBankAccountNumberTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new InternationalBankAccountNumberSerializeObject()
            {
                Id = 17,
                Obj = InternationalBankAccountNumberTest.TestStruct,
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
            var input = new InternationalBankAccountNumberSerializeObject()
            {
                Id = 17,
                Obj = InternationalBankAccountNumber.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new InternationalBankAccountNumberSerializeObject()
            {
                Id = 17,
                Obj = InternationalBankAccountNumber.Empty,
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
            var input = new InternationalBankAccountNumberSerializeObject()
            {
                Id = 17,
                Obj = InternationalBankAccountNumber.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new InternationalBankAccountNumberSerializeObject()
            {
                Id = 17,
                Obj = InternationalBankAccountNumber.Empty,
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
            var act = JsonTester.Read<InternationalBankAccountNumber>();
            var exp = InternationalBankAccountNumber.Empty;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_InvalidStringValue_AssertFormatException()
        {
            Assert.Catch<FormatException>(() =>
            {
                JsonTester.Read<InternationalBankAccountNumber>("InvalidStringValue");
            },
            "Not a valid IBAN");
        }
        [Test]
        public void FromJson_StringValue_AreEqual()
        {
            var act = JsonTester.Read<InternationalBankAccountNumber>("NL20INGB0001234567");
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_Int64Value_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>(() =>
            {
                JsonTester.Read<InternationalBankAccountNumber>(123456L);
            },
            "JSON deserialization from an integer is not supported.");
        }

        [Test]
        public void FromJson_DoubleValue_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>(() =>
            {
                JsonTester.Read<InternationalBankAccountNumber>(1234.56);
            },
            "JSON deserialization from a number is not supported.");
        }

        [Test]
        public void FromJson_DateTimeValue_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>(() =>
            {
                JsonTester.Read<InternationalBankAccountNumber>(new DateTime(1972, 02, 14));
            },
            "JSON deserialization from a date is not supported.");
        }

        [Test]
        public void ToJson_DefaultValue_IsNull()
        {
            object act = JsonTester.Write(default(InternationalBankAccountNumber));
            Assert.IsNull(act);
        }
        [Test]
        public void ToJson_TestStruct_AreEqual()
        {
            var act = JsonTester.Write(TestStruct);
            var exp = "NL20INGB0001234567";
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IFormattable / ToString tests

        [Test]
        public void ToString_Empty_IsStringEmpty()
        {
            var act = InternationalBankAccountNumber.Empty.ToString();
            var exp = "";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: 'NL20INGB0001234567', format: 'Unit Test Format'";

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_Unknown_IsStringEmpty()
        {
            var act = InternationalBankAccountNumber.Unknown.ToString("", null);
            var exp = "?";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_TestStruct_AreEqual()
        {
            var act = TestStruct.ToString();
            var exp = "NL20INGB0001234567";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_TestStructFormatULower_AreEqual()
        {
            var act = TestStruct.ToString("u");
            var exp = "nl20ingb0001234567";
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToString_TestStructFormatUUpper_AreEqual()
        {
            var act = TestStruct.ToString("U");
            var exp = "NL20INGB0001234567";
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToString_TestStructFormatFLower_AreEqual()
        {
            var act = TestStruct.ToString("f");
            var exp = "nl20 ingb 0001 2345 67";
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToString_TestStructFormatFUpper_AreEqual()
        {
            var act = TestStruct.ToString("F");
            var exp = "NL20 INGB 0001 2345 67";
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToString_EmptyFormatF_AreEqual()
        {
            var act = InternationalBankAccountNumber.Empty.ToString("F");
            var exp = "";
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToString_UnknownFormatF_AreEqual()
        {
            var act = InternationalBankAccountNumber.Unknown.ToString("F");
            var exp = "?";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void DebuggerDisplay_DebugToString_HasAttribute()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(InternationalBankAccountNumber));
        }

        [Test]
        public void DebuggerDisplay_DefaultValue_String()
        {
            DebuggerDisplayAssert.HasResult("IBAN: (empty)", default(InternationalBankAccountNumber));
        }
        [Test]
        public void DebuggerDisplay_Unknown_String()
        {
            DebuggerDisplayAssert.HasResult("IBAN: (unknown)", InternationalBankAccountNumber.Unknown);
        }

        [Test]
        public void DebuggerDisplay_TestStruct_String()
        {
            DebuggerDisplayAssert.HasResult("IBAN: NL20 INGB 0001 2345 67", TestStruct);
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for InternationalBankAccountNumber.Empty.</summary>
        [Test]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, InternationalBankAccountNumber.Empty.GetHashCode());
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
            Assert.IsTrue(InternationalBankAccountNumber.Empty.Equals(InternationalBankAccountNumber.Empty));
        }

        [Test]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = InternationalBankAccountNumber.Parse("NL20 INGB 0001 2345 67");
            var r = InternationalBankAccountNumber.Parse("nl20ingb0001234567");

            Assert.IsTrue(l.Equals(r));
        }

        [Test]
        public void Equals_TestStructTestStruct_IsTrue()
        {
            Assert.IsTrue(InternationalBankAccountNumberTest.TestStruct.Equals(InternationalBankAccountNumberTest.TestStruct));
        }

        [Test]
        public void Equals_TestStructEmpty_IsFalse()
        {
            Assert.IsFalse(InternationalBankAccountNumberTest.TestStruct.Equals(InternationalBankAccountNumber.Empty));
        }

        [Test]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(InternationalBankAccountNumber.Empty.Equals(InternationalBankAccountNumberTest.TestStruct));
        }

        [Test]
        public void Equals_TestStructObjectTestStruct_IsTrue()
        {
            Assert.IsTrue(InternationalBankAccountNumberTest.TestStruct.Equals((object)InternationalBankAccountNumberTest.TestStruct));
        }

        [Test]
        public void Equals_TestStructNull_IsFalse()
        {
            Assert.IsFalse(InternationalBankAccountNumberTest.TestStruct.Equals(null));
        }

        [Test]
        public void Equals_TestStructObject_IsFalse()
        {
            Assert.IsFalse(InternationalBankAccountNumberTest.TestStruct.Equals(new object()));
        }

        [Test]
        public void OperatorIs_TestStructTestStruct_IsTrue()
        {
            var l = InternationalBankAccountNumberTest.TestStruct;
            var r = InternationalBankAccountNumberTest.TestStruct;
            Assert.IsTrue(l == r);
        }

        [Test]
        public void OperatorIsNot_TestStructTestStruct_IsFalse()
        {
            var l = InternationalBankAccountNumberTest.TestStruct;
            var r = InternationalBankAccountNumberTest.TestStruct;
            Assert.IsFalse(l != r);
        }

        #endregion

        #region IComparable tests

        /// <summary>Orders a list of IBANs ascending.</summary>
        [Test]
        public void OrderBy_InternationalBankAccountNumber_AreEqual()
        {
            var item0 = InternationalBankAccountNumber.Parse("AE950210000000693123456");
            var item1 = InternationalBankAccountNumber.Parse("BH29BMAG1299123456BH00");
            var item2 = InternationalBankAccountNumber.Parse("CY17002001280000001200527600");
            var item3 = InternationalBankAccountNumber.Parse("DK5000400440116243");

            var inp = new List<InternationalBankAccountNumber>() { InternationalBankAccountNumber.Empty, item3, item2, item0, item1, InternationalBankAccountNumber.Empty };
            var exp = new List<InternationalBankAccountNumber>() { InternationalBankAccountNumber.Empty, InternationalBankAccountNumber.Empty, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of IBANs descending.</summary>
        [Test]
        public void OrderByDescending_InternationalBankAccountNumber_AreEqual()
        {
            var item0 = InternationalBankAccountNumber.Parse("AE950210000000693123456");
            var item1 = InternationalBankAccountNumber.Parse("BH29BMAG1299123456BH00");
            var item2 = InternationalBankAccountNumber.Parse("CY17002001280000001200527600");
            var item3 = InternationalBankAccountNumber.Parse("DK5000400440116243");

            var inp = new List<InternationalBankAccountNumber>() { InternationalBankAccountNumber.Empty, item3, item2, item0, item1, InternationalBankAccountNumber.Empty };
            var exp = new List<InternationalBankAccountNumber>() { item3, item2, item1, item0, InternationalBankAccountNumber.Empty, InternationalBankAccountNumber.Empty };
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
                "Argument must be an IBAN"
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
                "Argument must be an IBAN"
            );
        }
        #endregion

        #region Casting tests

        [Test]
        public void Explicit_StringToInternationalBankAccountNumber_AreEqual()
        {
            var exp = TestStruct;
            var act = (InternationalBankAccountNumber)TestStruct.ToString();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_InternationalBankAccountNumberToString_AreEqual()
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
            var act = InternationalBankAccountNumber.Empty.Length;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Length_Unknown_0()
        {
            var exp = 0;
            var act = InternationalBankAccountNumber.Unknown.Length;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Length_TestStruct_IntValue()
        {
            var exp = 18;
            var act = TestStruct.Length;
            Assert.AreEqual(exp, act);
        }


        [Test]
        public void Country_Empty_Null()
        {
            var exp = Country.Empty;
            var act = InternationalBankAccountNumber.Empty.Country;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Country_Unknown_Null()
        {
            var exp = Country.Unknown;
            var act = InternationalBankAccountNumber.Unknown.Country;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Country_TestStruct_Null()
        {
            var exp = Country.NL;
            var act = TestStruct.Country;
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region Type converter tests

        [Test]
        public void ConverterExists_InternationalBankAccountNumber_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(InternationalBankAccountNumber));
        }

        [Test]
        public void CanNotConvertFromInt32_InternationalBankAccountNumber_IsTrue()
        {
            TypeConverterAssert.CanNotConvertFrom(typeof(InternationalBankAccountNumber), typeof(Int32));
        }
        [Test]
        public void CanNotConvertToInt32_InternationalBankAccountNumber_IsTrue()
        {
            TypeConverterAssert.CanNotConvertTo(typeof(InternationalBankAccountNumber), typeof(Int32));
        }

        [Test]
        public void CanConvertFromString_InternationalBankAccountNumber_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(InternationalBankAccountNumber));
        }

        [Test]
        public void CanConvertToString_InternationalBankAccountNumber_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(InternationalBankAccountNumber));
        }

        [Test]
        public void ConvertFrom_StringNull_InternationalBankAccountNumberEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(InternationalBankAccountNumber.Empty, (string)null);
            }
        }

        [Test]
        public void ConvertFrom_StringEmpty_InternationalBankAccountNumberEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(InternationalBankAccountNumber.Empty, string.Empty);
            }
        }

        [Test]
        public void ConvertFromString_StringValue_TestStruct()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(InternationalBankAccountNumberTest.TestStruct, InternationalBankAccountNumberTest.TestStruct.ToString());
            }
        }

        [Test]
        public void ConvertFromInstanceDescriptor_InternationalBankAccountNumber_Successful()
        {
            TypeConverterAssert.ConvertFromInstanceDescriptor(typeof(InternationalBankAccountNumber));
        }

        [Test]
        public void ConvertToString_TestStruct_StringValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertToStringEquals(InternationalBankAccountNumberTest.TestStruct.ToString(), InternationalBankAccountNumberTest.TestStruct);
            }
        }

        #endregion

        #region IsValid tests

        [Test]
        public void IsValid_NullValues_IsFalse()
        {
            Assert.IsFalse(InternationalBankAccountNumber.IsValid(string.Empty), "string.Empty");
            Assert.IsFalse(InternationalBankAccountNumber.IsValid((String)null), "(String)null");
        }

        [Test]
        public void IsValid_QuestionMark_IsFalse()
        {
            Assert.IsFalse(InternationalBankAccountNumber.IsValid("?"), "string.Empty");
        }

        [Test]
        public void IsValid_XX_IsFalse()
        {
            Assert.IsFalse(InternationalBankAccountNumber.IsValid("XX950210000000693123456"), "Not existing country.");
        }

        [Test]
        public void IsValid_Null_IsFalse()
        {
            Assert.IsFalse(InternationalBankAccountNumber.IsValid(null));
        }
        [Test]
        public void IsValid_StringEmpty_IsFalse()
        {
            Assert.IsFalse(InternationalBankAccountNumber.IsValid(""));
        }

        // To Short.
        [Test]
        public void IsValid_NL20INGB007_IsFalse()
        {
            Assert.IsFalse(InternationalBankAccountNumber.IsValid("NL20INGB007"));
        }

        // Wrong pattern
        [Test]
        public void IsValid_WilhelmusVanNassau_IsFalse()
        {
            Assert.IsFalse(InternationalBankAccountNumber.IsValid("Wilhelmus van Nassau"));
        }

        [Test]
        public void IsValid_NLWrongSubpattern_IsFalse()
        {
            Assert.IsFalse(InternationalBankAccountNumber.IsValid("NL20INGB000123456Z"));
        }

        [Test]
        public void IsValid_XXNonExistentCountry_IsFalse()
        {
            Assert.IsFalse(InternationalBankAccountNumber.IsValid("XX20INGB0001234567"));
        }

        [Test]
        public void IsValid_USCountryWithoutPattern_IsFalse()
        {
            Assert.IsFalse(InternationalBankAccountNumber.IsValid("US20INGB0001234567"));
        }

        [TestCase("AE950 2100000006  93123456", "United Arab Emirates")]
        [TestCase("AE95 0210 0000 0069 3123 456", "United Arab Emirates")]
        [TestCase("AL47 2121 1009 0000 0002 3569 8741", "Albania")]
        [TestCase("AD12 0001 2030 2003 5910 0100", "Andorra")]
        [TestCase("AT61 1904 3002 3457 3201", "Austria")]
        [TestCase("BA39 1290 0794 0102 8494", "Bosnia and Herzegovina")]
        [TestCase("BE43 0689 9999 9501", "Belgium")]
        [TestCase("BG80 BNBG 9661 1020 3456 78", "Bulgaria")]
        [TestCase("BH29 BMAG 1299 1234 56BH 00", "Bahrain")]
        [TestCase("BY13 NBRB 3600 9000 0000 2Z00 AB00", "Belarus ")]
        [TestCase("CH36 0838 7000 0010 8017 3", "Switzerland")]
        [TestCase("CY17 0020 0128 0000 0012 0052 7600", "Cyprus")]
        [TestCase("CZ65 0800 0000 1920 0014 5399", "Czech Republic")]
        [TestCase("DE68 2105 0170 0012 3456 78", "Germany")]
        [TestCase("DK50 0040 0440 1162 43", "Denmark")]
        [TestCase("DO28 BAGR 0000 0001 2124 5361 1324", "Dominican Republic")]
        [TestCase("EE38 2200 2210 2014 5685", "Estonia")]
        [TestCase("ES91 2100 0418 4502 0005 1332", "Spain")]
        [TestCase("FI21 1234 5600 0007 85", "Finland")]
        [TestCase("FO20 0040 0440 1162 43", "Faroe Islands")]
        [TestCase("FR14 2004 1010 0505 0001 3M02 606", "Frankrijk")]
        [TestCase("GB82 WEST 1234 5698 7654 32", "United Kingdom")]
        [TestCase("GE29 NB00 0000 0101 9049 17", "Georgia")]
        [TestCase("GI75 NWBK 0000 0000 7099 453", "Gibraltar")]
        [TestCase("GL20 0040 0440 1162 43", "Greenland")]
        [TestCase("GR16 0110 1250 0000 0001 2300 695", "Greece")]
        [TestCase("HR12 1001 0051 8630 0016 0", "United Kingdom")]
        [TestCase("HU42 1177 3016 1111 1018 0000 0000", "Hungary")]
        [TestCase("IE29 AIBK 9311 5212 3456 78", "Ireland")]
        [TestCase("IL62 0108 0000 0009 9999 999", "Israel")]
        [TestCase("IS14 0159 2600 7654 5510 7303 39", "Iceland")]
        [TestCase("IT60 X054 2811 1010 0000 0123 456", "Italy")]
        [TestCase("KW81 CBKU 0000 0000 0000 1234 5601 01", "Kuwait")]
        [TestCase("KZ75 125K ZT20 6910 0100", "Kazakhstan")]
        [TestCase("LB30 0999 0000 0001 0019 2557 9115", "Lebanon")]
        [TestCase("LI21 0881 0000 2324 013A A", "Liechtenstein")]
        [TestCase("LT12 1000 0111 0100 1000", "Lithuania")]
        [TestCase("LU28 0019 4006 4475 0000", "Luxembourg")]
        [TestCase("LV80 BANK 0000 4351 9500 1", "Latvia")]
        [TestCase("MC11 1273 9000 7000 1111 1000 H79", "Monaco")]
        [TestCase("ME25 5050 0001 2345 6789 51", "Montenegro")]
        [TestCase("MK07 2501 2000 0058 984", "Macedonia")]
        [TestCase("MR13 0002 0001 0100 0012 3456 753", "Mauritania")]
        [TestCase("MT84 MALT 0110 0001 2345 MTLC AST0 01S", "Malta")]
        [TestCase("MU17 BOMM 0101 1010 3030 0200 000M UR", "Mauritius")]
        [TestCase("NL20 INGB 0001 2345 67", "Netherlands")]
        [TestCase("NL44 RABO 0123 4567 89", "Netherlands")]
        [TestCase("NO93 8601 1117 947", "Norway")]
        [TestCase("PL61 1090 1014 0000 0712 1981 2874", "Poland")]
        [TestCase("PT50 0002 0123 1234 5678 9015 4", "Portugal")]
        [TestCase("RO49 AAAA 1B31 0075 9384 0000", "Romania")]
        [TestCase("RS35 2600 0560 1001 6113 79", "Romania")]
        [TestCase("SA84 4000 0108 0540 1173 0013", "Saudi Arabia")]
        [TestCase("SE35 5000 0000 0549 1000 0003", "Sweden")]
        [TestCase("SI56 1910 0000 0123 438", "Slovenia")]
        [TestCase("SK31 1200 0000 1987 4263 7541", "Slovakia")]
        [TestCase("SM86 U032 2509 8000 0000 0270 100", "San Marino")]
        [TestCase("TL38 0010 0123 4567 8910 106", "Timor Leste")]
        [TestCase("TN59 1000 6035 1835 9847 8831", "Tunisia")]
        [TestCase("TR33 0006 1005 1978 6457 8413 26", "Turkey")]
        [TestCase("UA21 3996 2200 0002 6007 2335 6600 1", "Ukraine")]
        [TestCase("VA59 0011 2300 0012 3456 78", "Vatican City")]
        public void IsValid(string iban, string description)
        {
            Assert.IsTrue(InternationalBankAccountNumber.IsValid(iban), description);
        }

        #endregion

        private static IEnumerable<object[]> LocalizedPatterns()
        {
            var info = typeof(InternationalBankAccountNumber).GetField(nameof(LocalizedPatterns), BindingFlags.Static | BindingFlags.NonPublic);
            return ((Dictionary<Country, Regex>)info.GetValue(null))
                .Select(kvp => new object[] { kvp.Key, kvp.Value });
        }

        [TestCaseSource(nameof(LocalizedPatterns))]
        public void Pattern_StartsWith(Country country, Regex pattern)
        {
            StringAssert.StartsWith("^" + country.IsoAlpha2Code, pattern.ToString());
        }
    }

    [Serializable]
    public class InternationalBankAccountNumberSerializeObject
    {
        public int Id { get; set; }
        public InternationalBankAccountNumber Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
