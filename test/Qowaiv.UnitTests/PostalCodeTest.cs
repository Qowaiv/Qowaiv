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
    /// <summary>Tests the postal code SVO.</summary>
    /// <remarks>
    /// The type of tests that are done per country:
    /// <example>
    ///     var country = Country.*;
    ///     
    ///     IsValid("AA0000000000DF", country); //Not
    ///     IsValid("AB0123456789CD", country); //Not
    ///     IsValid("BJ1282353436RF", country); //Not
    ///     IsValid("CD2037570044WK", country); //Not
    ///     IsValid("DE3243436478SD", country); //Not
    ///     IsValid("EO4008279475PJ", country); //Not
    ///     IsValid("FN5697836450KF", country); //Not
    ///     IsValid("GF6282469088LS", country); //Not
    ///     IsValid("HL7611343495JD", country); //Not
    ///     IsValid("ID6767185502MO", country); //Not
    ///     IsValid("JS8752391832DF", country); //Not
    ///     IsValid("KN9999999999JS", country); //Not
    ///     IsValid("LO0000000000DF", country); //Not
    ///     IsValid("ME0144942325DS", country); //Not
    ///     IsValid("NN1282353436RF", country); //Not
    ///     IsValid("OL2037570044WK", country); //Not
    ///     IsValid("PS3243436478SD", country); //Not
    ///     IsValid("QD4008279475PJ", country); //Not
    ///     IsValid("RN5697836450KF", country); //Not
    ///     IsValid("SE6282469088LS", country); //Not
    ///     IsValid("TM7611343495JD", country); //Not
    ///     IsValid("UF6767185502MO", country); //Not
    ///     IsValid("VE8752391832DF", country); //Not
    ///     IsValid("WL9999999999JS", country); //Not
    ///     IsValid("XM0000000000DF", country); //Not
    ///     IsValid("YE0144942325DS", country); //Not
    ///     IsValid("ZL1282353436RF", country); //Not
    ///     IsValid("ZZ2037570044WK", country); //Not
    /// </example>
    /// </remarks>
    public partial class PostalCodeTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly PostalCode TestStruct = PostalCode.Parse("H0H0H0");

        #region postal code const tests

        /// <summary>PostalCode.Empty should be equal to the default of postal code.</summary>
        [Test]
        public void Empty_None_EqualsDefault()
        {
            Assert.AreEqual(default(PostalCode), PostalCode.Empty);
        }

        #endregion

        #region postal code IsEmpty tests

        /// <summary>PostalCode.IsEmpty() should be true for the default of postal code.</summary>
        [Test]
        public void IsEmpty_Default_IsTrue()
        {
            Assert.IsTrue(default(PostalCode).IsEmpty());
        }
        /// <summary>PostalCode.IsEmpty() should be false for PostalCode.Unknown.</summary>
        [Test]
        public void IsEmpty_Unknown_IsFalse()
        {
            Assert.IsFalse(PostalCode.Unknown.IsEmpty());
        }
        /// <summary>PostalCode.IsEmpty() should be false for the TestStruct.</summary>
        [Test]
        public void IsEmpty_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmpty());
        }

        /// <summary>PostalCode.IsUnknown() should be false for the default of postal code.</summary>
        [Test]
        public void IsUnknown_Default_IsFalse()
        {
            Assert.IsFalse(default(PostalCode).IsUnknown());
        }
        /// <summary>PostalCode.IsUnknown() should be true for PostalCode.Unknown.</summary>
        [Test]
        public void IsUnknown_Unknown_IsTrue()
        {
            Assert.IsTrue(PostalCode.Unknown.IsUnknown());
        }
        /// <summary>PostalCode.IsUnknown() should be false for the TestStruct.</summary>
        [Test]
        public void IsUnknown_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsUnknown());
        }

        /// <summary>PostalCode.IsEmptyOrUnknown() should be true for the default of postal code.</summary>
        [Test]
        public void IsEmptyOrUnknown_Default_IsFalse()
        {
            Assert.IsTrue(default(PostalCode).IsEmptyOrUnknown());
        }
        /// <summary>PostalCode.IsEmptyOrUnknown() should be true for PostalCode.Unknown.</summary>
        [Test]
        public void IsEmptyOrUnknown_Unknown_IsTrue()
        {
            Assert.IsTrue(PostalCode.Unknown.IsEmptyOrUnknown());
        }
        /// <summary>PostalCode.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
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

            Assert.IsTrue(PostalCode.TryParse(str, out var val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TyrParse_StringEmpty_IsValid()
        {
            string str = string.Empty;

            Assert.IsTrue(PostalCode.TryParse(str, out var val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TyrParse_StringValue_IsValid()
        {
            string str = "H0H0H0";

            Assert.IsTrue(PostalCode.TryParse(str, out var val), "Valid");
            Assert.AreEqual(str, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [Test]
        public void TyrParse_StringValue_IsNotValid()
        {
            string str = "1";

            Assert.IsFalse(PostalCode.TryParse(str, out var val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        [Test]
        public void Parse_Unknown_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var act = PostalCode.Parse("?");
                var exp = PostalCode.Unknown;
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
                    PostalCode.Parse("InvalidInput");
                },
                "Not a valid postal code");
            }
        }

        [Test]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = TestStruct;
                var act = PostalCode.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void TryParse_InvalidInput_DefaultValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = default(PostalCode);
                var act = PostalCode.TryParse("InvalidInput");

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
                SerializationTest.DeserializeUsingConstructor<PostalCode>(null, default);
            },
            "info");
        }

        [Test]
        public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
        {
            Assert.Catch<SerializationException>
            (() =>
            {
                var info = new SerializationInfo(typeof(PostalCode), new System.Runtime.Serialization.FormatterConverter());
                SerializationTest.DeserializeUsingConstructor<PostalCode>(info, default);
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
            var info = new SerializationInfo(typeof(PostalCode), new System.Runtime.Serialization.FormatterConverter());
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
        public void XmlSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = TestStruct;
            var exp = TestStruct;
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void SerializeDeserialize_PostalCodeSerializeObject_AreEqual()
        {
            var input = new PostalCodeSerializeObject()
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new PostalCodeSerializeObject()
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
        public void XmlSerializeDeserialize_PostalCodeSerializeObject_AreEqual()
        {
            var input = new PostalCodeSerializeObject()
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new PostalCodeSerializeObject()
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
        public void DataContractSerializeDeserialize_PostalCodeSerializeObject_AreEqual()
        {
            var input = new PostalCodeSerializeObject()
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new PostalCodeSerializeObject()
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
            var input = new PostalCodeSerializeObject()
            {
                Id = 17,
                Obj = PostalCode.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new PostalCodeSerializeObject()
            {
                Id = 17,
                Obj = PostalCode.Empty,
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
            var input = new PostalCodeSerializeObject()
            {
                Id = 17,
                Obj = PostalCode.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new PostalCodeSerializeObject()
            {
                Id = 17,
                Obj = PostalCode.Empty,
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
            var act = JsonTester.Read<PostalCode>();
            var exp = PostalCode.Empty;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_InvalidStringValue_AssertFormatException()
        {
            Assert.Catch<FormatException>(() =>
            {
                JsonTester.Read<PostalCode>("InvalidStringValue");
            },
            "Not a valid postal code");
        }
        [Test]
        public void FromJson_StringValue_AreEqual()
        {
            var act = JsonTester.Read<PostalCode>(TestStruct.ToString(CultureInfo.InvariantCulture));
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_Int64Value_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>(() =>
            {
                JsonTester.Read<PostalCode>(123456L);
            },
            "JSON deserialization from an integer is not supported.");
        }

        [Test]
        public void FromJson_DoubleValue_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>(() =>
            {
                JsonTester.Read<PostalCode>(1234.56);
            },
            "JSON deserialization from a number is not supported.");
        }

        [Test]
        public void FromJson_DateTimeValue_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>(() =>
            {
                JsonTester.Read<PostalCode>(new DateTime(1972, 02, 14));
            },
            "JSON deserialization from a date is not supported.");
        }

        [Test]
        public void ToJson_DefaultValue_AreEqual()
        {
            object act = JsonTester.Write(default(PostalCode));
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

        #region IFormattable / ToString tests

        [Test]
        public void ToString_Empty_IsStringEmpty()
        {
            var act = PostalCode.Empty.ToString();
            var exp = "";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_EmptyCA_IsStringEmpty()
        {
            var act = PostalCode.Empty.ToString("CA");
            var exp = "";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_Unknown_IsQuestionMark()
        {
            var act = PostalCode.Unknown.ToString();
            var exp = "?";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_UnknownCA_IsQuestionMark()
        {
            var act = PostalCode.Unknown.ToString("CA");
            var exp = "?";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: 'H0H0H0', format: 'Unit Test Format'";

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_TestStructCA_ComplexPattern()
        {
            var act = TestStruct.ToString("CA");
            var exp = "H0H 0H0";
            Assert.AreEqual(exp, act);
        }

        /// <remarks>No valid for the Netherlands.</summary>
        [Test]
        public void ToString_TestStructNL_ComplexPattern()
        {
            var act = TestStruct.ToString(Country.NL);
            var exp = "H0H0H0";
            Assert.AreEqual(exp, act);
        }

        /// <remarks>No postal code in Somalia.</summary>
        [Test]
        public void ToString_TestStructSO_ComplexPattern()
        {
            var act = TestStruct.ToString(Country.SO);
            var exp = "H0H0H0";
            Assert.AreEqual(exp, act);
        }

        /// <remarks>No formatting in Albania.</summary>
        [Test]
        public void ToString_TestStructAL_ComplexPattern()
        {
            var act = TestStruct.ToString(Country.AL);
            var exp = "H0H0H0";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_AD765AD_ComplexPattern()
        {
            var postalcode = PostalCode.Parse("AD765");
            var act = postalcode.ToString("AD");
            var exp = "AD-765";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_765AD_ComplexPattern()
        {
            var postalcode = PostalCode.Parse("765");
            var act = postalcode.ToString("AD");
            var exp = "AD-765";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void DebuggerDisplay_DebugToString_HasAttribute()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(PostalCode));
        }

        [Test]
        public void DebuggerDisplay_DefaultValue_String()
        {
            DebuggerDisplayAssert.HasResult("PostalCode: (empty)", default(PostalCode));
        }

        [Test]
        public void DebuggerDisplay_TestStruct_String()
        {
            DebuggerDisplayAssert.HasResult("PostalCode: H0H0H0", TestStruct);
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for PostalCode.Empty.</summary>
        [Test]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, PostalCode.Empty.GetHashCode());
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
            Assert.IsTrue(PostalCode.Empty.Equals(PostalCode.Empty));
        }

        [Test]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = PostalCode.Parse("H0 H0 H0-", CultureInfo.InvariantCulture);
            var r = PostalCode.Parse("h0h0h0", CultureInfo.InvariantCulture);

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
            Assert.IsFalse(TestStruct.Equals(PostalCode.Empty));
        }

        [Test]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(PostalCode.Empty.Equals(TestStruct));
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

        /// <summary>Orders a list of postal codes ascending.</summary>
        [Test]
        public void OrderBy_PostalCode_AreEqual()
        {
            var item0 = PostalCode.Parse("012");
            var item1 = PostalCode.Parse("1234");
            var item2 = PostalCode.Parse("23456");
            var item3 = PostalCode.Parse("345678");

            var inp = new List<PostalCode>() { PostalCode.Empty, item3, item2, item0, item1, PostalCode.Empty };
            var exp = new List<PostalCode>() { PostalCode.Empty, PostalCode.Empty, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of postal codes descending.</summary>
        [Test]
        public void OrderByDescending_PostalCode_AreEqual()
        {
            var item0 = PostalCode.Parse("012");
            var item1 = PostalCode.Parse("1234");
            var item2 = PostalCode.Parse("23456");
            var item3 = PostalCode.Parse("345678");

            var inp = new List<PostalCode>() { PostalCode.Empty, item3, item2, item0, item1, PostalCode.Empty };
            var exp = new List<PostalCode>() { item3, item2, item1, item0, PostalCode.Empty, PostalCode.Empty };
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
                "Argument must be a postal code"
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
                "Argument must be a postal code"
            );
        }
        #endregion

        #region Casting tests

        [Test]
        public void Explicit_StringToPostalCode_AreEqual()
        {
            var exp = TestStruct;
            var act = (PostalCode)TestStruct.ToString();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_PostalCodeToString_AreEqual()
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
            var act = PostalCode.Empty.Length;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Length_TestStruct_IntValue()
        {
            var exp = 6;
            var act = TestStruct.Length;
            Assert.AreEqual(exp, act);
        }
        #endregion

        #region Type converter tests

        [Test]
        public void ConverterExists_PostalCode_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(PostalCode));
        }

        [Test]
        public void CanNotConvertFromInt32_PostalCode_IsTrue()
        {
            TypeConverterAssert.CanNotConvertFrom(typeof(PostalCode), typeof(Int32));
        }
        [Test]
        public void CanNotConvertToInt32_PostalCode_IsTrue()
        {
            TypeConverterAssert.CanNotConvertTo(typeof(PostalCode), typeof(Int32));
        }

        [Test]
        public void CanConvertFromString_PostalCode_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(PostalCode));
        }

        [Test]
        public void CanConvertToString_PostalCode_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(PostalCode));
        }

        [Test]
        public void ConvertFrom_StringNull_PostalCodeEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(PostalCode.Empty, (string)null);
            }
        }

        [Test]
        public void ConvertFrom_StringEmpty_PostalCodeEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(PostalCode.Empty, string.Empty);
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
        public void ConvertFromInstanceDescriptor_PostalCode_Successful()
        {
            TypeConverterAssert.ConvertFromInstanceDescriptor(typeof(PostalCode));
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

        [Test]
        public void IsValid_Data_IsFalse()
        {
            Assert.IsFalse(PostalCode.IsValid("1"), "1");
            Assert.IsFalse(PostalCode.IsValid("12345678901"), "12345678901");
            Assert.IsFalse(PostalCode.IsValid((String)null), "(String)null");
            Assert.IsFalse(PostalCode.IsValid(string.Empty), "string.Empty");
        }

        [Test]
        public void IsValid_Data_IsTrue()
        {
            Assert.IsTrue(PostalCode.IsValid("1234AB"));
        }

        [Test]
        public void IsValid_EmptyCA_IsFalse()
        {
            Assert.IsFalse(PostalCode.Empty.IsValid(Country.CA));
        }
        [Test]
        public void IsValid_UnknownCA_IsFalse()
        {
            Assert.IsFalse(PostalCode.Unknown.IsValid(Country.CA));
        }

        [Test]
        public void IsValid_TestStructCA_IsTrue()
        {
            Assert.IsTrue(TestStruct.IsValid(Country.CA));
        }
        [Test]
        public void IsValid_TestStructBE_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsValid(Country.BE));
        }
        [Test]
        public void IsValidFor_TestStruct_1Country()
        {
            var act = TestStruct.IsValidFor().ToArray();
            var exp = new Country[] { Country.CA };
            CollectionAssert.AllItemsAreUnique(act);

            CollectionAssert.AreEqual(exp, act);
        }

        [Test]
        public void IsValidFor_0123456_3Countries()
        {
            var postalcode = PostalCode.Parse("0123456");
            var act = postalcode.IsValidFor().ToArray();
            var exp = new Country[] { Country.CL, Country.IL, Country.JP };
            CollectionAssert.AllItemsAreUnique(act);

            CollectionAssert.AreEqual(exp, act);
        }

        #endregion

        #region IsValid Country tests

        // Tests patterns that should be valid for Andorra (AD).
        [TestCase("AD", "AD123")]
        [TestCase("AD", "AD345")]
        [TestCase("AD", "AD678")]
        [TestCase("AD", "AD789")]
        // Tests patterns that should be valid for Afghanistan (AF)
        [TestCase("AF", "4301")]
        [TestCase("AF", "1001")]
        [TestCase("AF", "2023")]
        [TestCase("AF", "1102")]
        [TestCase("AF", "4020")]
        [TestCase("AF", "3077")]
        [TestCase("AF", "2650")]
        [TestCase("AF", "4241")]
        // Tests patterns that should be valid for Anguilla(AI).
        [TestCase("AI", "2640")]
        [TestCase("AI", "AI-2640")]
        [TestCase("AI", "AI2640")]
        [TestCase("AI", "ai-2640")]
        [TestCase("AI", "ai2640")]
        [TestCase("AI", "ai 2640")]
        [TestCase("AI", "ai.2640")]
        // Tests patterns that should be valid for Albania (AL).
        [TestCase("AL", "1872")]
        [TestCase("AL", "2540")]
        [TestCase("AL", "7900")]
        [TestCase("AL", "9999")]
        // Tests patterns that should be valid for Armenia (AM).
        [TestCase("AM", "0123")]
        [TestCase("AM", "1234")]
        [TestCase("AM", "2000")]
        [TestCase("AM", "3248")]
        [TestCase("AM", "4945")]
        // Tests patterns that should be valid for Argentina (AR).
        [TestCase("AR", "A4400XXX")]
        [TestCase("AR", "C 1420 ABC")]
        [TestCase("AR", "S 2300DDD")]
        [TestCase("AR", "Z9400 QOW")]
        // Tests patterns that should be valid for American Samoa (AS).
        [TestCase("AS", "91000-0060")]
        [TestCase("AS", "91000-9996")]
        [TestCase("AS", "90126")]
        [TestCase("AS", "92345")]
        // Tests patterns that should be valid for Austria (AT).
        [TestCase("AT", "2471")]
        [TestCase("AT", "1000")]
        [TestCase("AT", "5120")]
        [TestCase("AT", "9999")]
        // Tests patterns that should be valid for Australia (AU).
        [TestCase("AU", "0872")]
        [TestCase("AU", "2540")]
        [TestCase("AU", "0900")]
        [TestCase("AU", "9999")]
        // Tests patterns that should be valid for Åland Islands (AX).
        [TestCase("AX", "22-000")]
        [TestCase("AX", "22-123")]
        [TestCase("AX", "22000")]
        [TestCase("AX", "22345")]
        // Tests patterns that should be valid for Azerbaijan (AZ).
        [TestCase("AZ", "1499")]
        [TestCase("AZ", "az 1499")]
        [TestCase("AZ", "AZ-1499")]
        [TestCase("AZ", "az1499")]
        [TestCase("AZ", "AZ0499")]
        [TestCase("AZ", "AZ0099")]
        [TestCase("AZ", "aZ6990")]
        // Tests patterns that should be valid for Bosnia And Herzegovina (BA).
        [TestCase("BA", "00000")]
        [TestCase("BA", "01235")]
        [TestCase("BA", "12346")]
        [TestCase("BA", "20004")]
        [TestCase("BA", "32648")]
        [TestCase("BA", "40945")]
        [TestCase("BA", "56640")]
        [TestCase("BA", "62908")]
        [TestCase("BA", "76345")]
        [TestCase("BA", "67552")]
        [TestCase("BA", "87182")]
        [TestCase("BA", "99999")]
        // Tests patterns that should be valid for Barbados (BB).
        [TestCase("BB", "21499")]
        [TestCase("BB", "01499")]
        [TestCase("BB", "bB-31499")]
        [TestCase("BB", "BB 01499")]
        [TestCase("BB", "bb81499")]
        [TestCase("BB", "BB71499")]
        [TestCase("BB", "BB56990")]
        // Tests patterns that should be valid for Bangladesh (BD).
        [TestCase("BD", "0483")]
        [TestCase("BD", "1480")]
        [TestCase("BD", "5492")]
        [TestCase("BD", "7695")]
        [TestCase("BD", "9796")]
        // Tests patterns that should be valid for Belgium (BE).
        [TestCase("BE", "2471")]
        [TestCase("BE", "1000")]
        [TestCase("BE", "5120")]
        [TestCase("BE", "9999")]
        // Tests patterns that should be valid for Bulgaria (BG).
        [TestCase("BG", "1000")]
        [TestCase("BG", "2077")]
        [TestCase("BG", "2650")]
        [TestCase("BG", "4241")]
        // Tests patterns that should be valid for Bahrain (BH).
        [TestCase("BH", "199")]
        [TestCase("BH", "1299")]
        [TestCase("BH", "666")]
        [TestCase("BH", "890")]
        [TestCase("BH", "768")]
        [TestCase("BH", "1000")]
        [TestCase("BH", "1176")]
        // Tests patterns that should be valid for Saint Barthélemy (BL).
        [TestCase("BL", "97700")]
        [TestCase("BL", "97701")]
        [TestCase("BL", "97712")]
        [TestCase("BL", "97720")]
        [TestCase("BL", "97732")]
        [TestCase("BL", "97740")]
        [TestCase("BL", "97756")]
        [TestCase("BL", "97762")]
        [TestCase("BL", "97776")]
        [TestCase("BL", "97767")]
        [TestCase("BL", "97787")]
        [TestCase("BL", "97799")]
        // Tests patterns that should be valid for Bermuda (BM).
        [TestCase("BM", "AA")]
        [TestCase("BM", "AS")]
        [TestCase("BM", "BJ")]
        [TestCase("BM", "CD")]
        [TestCase("BM", "DE")]
        [TestCase("BM", "EO")]
        [TestCase("BM", "FN")]
        [TestCase("BM", "GF")]
        [TestCase("BM", "HL")]
        [TestCase("BM", "ID")]
        [TestCase("BM", "JS")]
        [TestCase("BM", "KN")]
        [TestCase("BM", "LO")]
        [TestCase("BM", "ME")]
        [TestCase("BM", "NN")]
        [TestCase("BM", "OL")]
        [TestCase("BM", "PS")]
        [TestCase("BM", "QD")]
        [TestCase("BM", "RN")]
        [TestCase("BM", "SE")]
        [TestCase("BM", "TM")]
        [TestCase("BM", "UF")]
        [TestCase("BM", "VE")]
        [TestCase("BM", "WL")]
        [TestCase("BM", "XM")]
        [TestCase("BM", "YE")]
        [TestCase("BM", "ZL")]
        [TestCase("BM", "ZZ")]
        [TestCase("BM", "AA0F")]
        [TestCase("BM", "AS0S")]
        [TestCase("BM", "BJ1F")]
        [TestCase("BM", "CD2K")]
        [TestCase("BM", "DE3D")]
        [TestCase("BM", "EO4J")]
        [TestCase("BM", "FN5F")]
        [TestCase("BM", "GF6S")]
        [TestCase("BM", "HL7D")]
        [TestCase("BM", "ID69")]
        [TestCase("BM", "JS66")]
        [TestCase("BM", "KN48")]
        [TestCase("BM", "LO12")]
        [TestCase("BM", "MEDS")]
        [TestCase("BM", "NNRF")]
        [TestCase("BM", "OLWK")]
        [TestCase("BM", "PSSD")]
        [TestCase("BM", "QDPJ")]
        [TestCase("BM", "RNKF")]
        [TestCase("BM", "SELS")]
        [TestCase("BM", "TMD1")]
        [TestCase("BM", "UFO7")]
        [TestCase("BM", "VEF2")]
        [TestCase("BM", "WLS9")]
        [TestCase("BM", "XMF0")]
        [TestCase("BM", "YES4")]
        [TestCase("BM", "ZLF2")]
        [TestCase("BM", "ZZK7")]
        // Tests patterns that should be valid for Brunei Darussalam (BN).
        [TestCase("BN", "YZ0000")]
        [TestCase("BN", "BU2529")]
        [TestCase("BN", "bU2529")]
        [TestCase("BN", "bu2529")]
        [TestCase("BN", "Bu2529")]
        // Tests patterns that should be valid for Bolivia (BO).
        [TestCase("BO", "0123")]
        [TestCase("BO", "1234")]
        [TestCase("BO", "2000")]
        [TestCase("BO", "3248")]
        [TestCase("BO", "4945")]
        [TestCase("BO", "5640")]
        [TestCase("BO", "6208")]
        [TestCase("BO", "7645")]
        [TestCase("BO", "6752")]
        [TestCase("BO", "8782")]
        [TestCase("BO", "9999")]
        // Tests patterns that should be valid for Brazil (BR).
        [TestCase("BR", "01000-000")]
        [TestCase("BR", "01000999")]
        [TestCase("BR", "88000-123")]
        // Tests patterns that should be valid for Bhutan (BT).
        [TestCase("BT", "000")]
        [TestCase("BT", "012")]
        [TestCase("BT", "123")]
        [TestCase("BT", "200")]
        [TestCase("BT", "326")]
        [TestCase("BT", "409")]
        [TestCase("BT", "566")]
        [TestCase("BT", "629")]
        [TestCase("BT", "763")]
        [TestCase("BT", "675")]
        [TestCase("BT", "871")]
        [TestCase("BT", "999")]
        // Tests patterns that should be valid for Belarus (BY).
        [TestCase("BY", "010185")]
        [TestCase("BY", "110000")]
        [TestCase("BY", "342600")]
        [TestCase("BY", "610185")]
        [TestCase("BY", "910185")]
        // Tests patterns that should be valid for Canada (CA).
        [TestCase("CA", "H0H-0H0")]
        [TestCase("CA", "K8 N5W 6")]
        [TestCase("CA", "A1A 1A1")]
        [TestCase("CA", "K0H 9Z0")]
        [TestCase("CA", "T1R 9Z0")]
        [TestCase("CA", "P2V9z0")]
        // Tests patterns that should be valid for Cocos (CC).
        [TestCase("CC", "0123")]
        [TestCase("CC", "1234")]
        [TestCase("CC", "2000")]
        [TestCase("CC", "3248")]
        [TestCase("CC", "4945")]
        [TestCase("CC", "5640")]
        [TestCase("CC", "6208")]
        [TestCase("CC", "7645")]
        [TestCase("CC", "6752")]
        [TestCase("CC", "8782")]
        [TestCase("CC", "9999")]
        // Tests patterns that should be valid for Switzerland (CH).
        [TestCase("CH", "1001")]
        [TestCase("CH", "8023")]
        [TestCase("CH", "9100")]
        [TestCase("CH", "1000")]
        [TestCase("CH", "2077")]
        [TestCase("CH", "2650")]
        [TestCase("CH", "4241")]
        // Tests patterns that should be valid for Chile (CL).
        [TestCase("CL", "0000000")]
        [TestCase("CL", "0231145")]
        [TestCase("CL", "1342456")]
        [TestCase("CL", "2000974")]
        [TestCase("CL", "3642438")]
        [TestCase("CL", "4940375")]
        [TestCase("CL", "5646230")]
        [TestCase("CL", "6902168")]
        [TestCase("CL", "7346345")]
        [TestCase("CL", "6557682")]
        [TestCase("CL", "8187992")]
        [TestCase("CL", "9999999")]
        // Tests patterns that should be valid for China (CN).
        [TestCase("CN", "010000")]
        [TestCase("CN", "342600")]
        [TestCase("CN", "810185")]
        // Tests patterns that should be valid for Colombia (CO).
        [TestCase("CO", "000000")]
        [TestCase("CO", "023145")]
        [TestCase("CO", "134256")]
        [TestCase("CO", "200074")]
        [TestCase("CO", "364238")]
        [TestCase("CO", "494075")]
        [TestCase("CO", "564630")]
        [TestCase("CO", "690268")]
        [TestCase("CO", "734645")]
        [TestCase("CO", "655782")]
        [TestCase("CO", "818792")]
        [TestCase("CO", "999999")]
        // Tests patterns that should be valid for Costa Rica (CR).
        [TestCase("CR", "00000")]
        [TestCase("CR", "01235")]
        [TestCase("CR", "12346")]
        [TestCase("CR", "20004")]
        [TestCase("CR", "32648")]
        [TestCase("CR", "40945")]
        [TestCase("CR", "56640")]
        [TestCase("CR", "62908")]
        [TestCase("CR", "76345")]
        [TestCase("CR", "67552")]
        [TestCase("CR", "87182")]
        [TestCase("CR", "99999")]
        // Tests patterns that should be valid for Cuba (CU).
        [TestCase("CU", "00000")]
        [TestCase("CU", "01235")]
        [TestCase("CU", "12346")]
        [TestCase("CU", "20004")]
        [TestCase("CU", "32648")]
        [TestCase("CU", "40945")]
        [TestCase("CU", "56640")]
        [TestCase("CU", "62908")]
        [TestCase("CU", "76345")]
        [TestCase("CU", "67552")]
        [TestCase("CU", "87182")]
        [TestCase("CU", "99999")]
        [TestCase("CU", "CP00000")]
        [TestCase("CU", "CP01235")]
        [TestCase("CU", "CP12346")]
        [TestCase("CU", "CP20004")]
        [TestCase("CU", "CP32648")]
        // Tests patterns that should be valid for Cape Verde (CV).
        [TestCase("CV", "0000")]
        [TestCase("CV", "1000")]
        [TestCase("CV", "2077")]
        [TestCase("CV", "2650")]
        [TestCase("CV", "4241")]
        // Tests patterns that should be valid for Christmas Island (CX).
        [TestCase("CX", "0000")]
        [TestCase("CX", "0144")]
        [TestCase("CX", "1282")]
        [TestCase("CX", "2037")]
        [TestCase("CX", "3243")]
        [TestCase("CX", "4008")]
        [TestCase("CX", "5697")]
        [TestCase("CX", "6282")]
        [TestCase("CX", "7611")]
        [TestCase("CX", "6767")]
        [TestCase("CX", "8752")]
        [TestCase("CX", "9999")]
        // Tests patterns that should be valid for Cyprus (CY).
        [TestCase("CY", "1000")]
        [TestCase("CY", "2077")]
        [TestCase("CY", "2650")]
        [TestCase("CY", "4241")]
        // Tests patterns that should be valid for Czech Republic (CZ).
        [TestCase("CZ", "21234")]
        [TestCase("CZ", "12345")]
        [TestCase("CZ", "11111")]
        [TestCase("CZ", "123 45")]
        // Tests patterns that should be valid for Germany (DE).
        [TestCase("DE", "10000")]
        [TestCase("DE", "01123")]
        [TestCase("DE", "89000")]
        [TestCase("DE", "12345")]
        // Tests patterns that should be valid for Denmark (DK).
        [TestCase("DK", "1499")]
        [TestCase("DK", "dk-1499")]
        [TestCase("DK", "DK-1499")]
        [TestCase("DK", "dk1499")]
        [TestCase("DK", "DK1499")]
        [TestCase("DK", "DK6990")]
        // Tests patterns that should be valid for Algeria (DZ).
        [TestCase("DZ", "01234")]
        [TestCase("DZ", "12345")]
        [TestCase("DZ", "11111")]
        // Tests patterns that should be valid for Ecuador (EC).
        [TestCase("EC", "000000")]
        [TestCase("EC", "023145")]
        [TestCase("EC", "134256")]
        [TestCase("EC", "200074")]
        [TestCase("EC", "364238")]
        [TestCase("EC", "494075")]
        [TestCase("EC", "564630")]
        [TestCase("EC", "690268")]
        [TestCase("EC", "734645")]
        [TestCase("EC", "655782")]
        [TestCase("EC", "818792")]
        [TestCase("EC", "999999")]
        // Tests patterns that should be valid for Estonia (EE).
        [TestCase("EE", "00000")]
        [TestCase("EE", "01235")]
        [TestCase("EE", "12346")]
        [TestCase("EE", "20004")]
        [TestCase("EE", "32648")]
        [TestCase("EE", "40945")]
        [TestCase("EE", "56640")]
        [TestCase("EE", "62908")]
        [TestCase("EE", "76345")]
        [TestCase("EE", "67552")]
        [TestCase("EE", "87182")]
        [TestCase("EE", "99999")]
        // Tests patterns that should be valid for Egypt (EG).
        [TestCase("EG", "12346")]
        [TestCase("EG", "20004")]
        [TestCase("EG", "32648")]
        [TestCase("EG", "40945")]
        [TestCase("EG", "56640")]
        [TestCase("EG", "62908")]
        [TestCase("EG", "76345")]
        [TestCase("EG", "67552")]
        [TestCase("EG", "87182")]
        [TestCase("EG", "99999")]
        // Tests patterns that should be valid for Spain (ES).
        [TestCase("ES", "01070")]
        [TestCase("ES", "10070")]
        [TestCase("ES", "20767")]
        [TestCase("ES", "26560")]
        [TestCase("ES", "32451")]
        [TestCase("ES", "09112")]
        [TestCase("ES", "48221")]
        [TestCase("ES", "50636")]
        [TestCase("ES", "52636")]
        [TestCase("ES", "51050")]
        // Tests patterns that should be valid for Ethiopia (ET).
        [TestCase("ET", "0123")]
        [TestCase("ET", "1234")]
        [TestCase("ET", "2000")]
        [TestCase("ET", "3248")]
        [TestCase("ET", "4945")]
        [TestCase("ET", "5640")]
        [TestCase("ET", "6208")]
        [TestCase("ET", "7645")]
        [TestCase("ET", "6752")]
        [TestCase("ET", "8782")]
        [TestCase("ET", "9999")]
        // Tests patterns that should be valid for Finland (FI).
        [TestCase("FI", "00-000")]
        [TestCase("FI", "01-123")]
        [TestCase("FI", "00000")]
        [TestCase("FI", "12345")]
        // Tests patterns that should be valid for Falkland Islands (FK).
        [TestCase("FK", "FIQQ1ZZ")]
        // Tests patterns that should be valid for Micronesia (FM).
        [TestCase("FM", "96941")]
        [TestCase("FM", "96942")]
        [TestCase("FM", "96943")]
        [TestCase("FM", "96944")]
        [TestCase("FM", "969410000")]
        [TestCase("FM", "969420123")]
        [TestCase("FM", "969430144")]
        [TestCase("FM", "969441282")]
        // Tests patterns that should be valid for Faroe Islands (FO).
        [TestCase("FO", "399")]
        [TestCase("FO", "fo-399")]
        [TestCase("FO", "FO-199")]
        [TestCase("FO", "fO399")]
        [TestCase("FO", "FO678")]
        [TestCase("FO", "FO123")]
        // Tests patterns that should be valid for France (FR).
        [TestCase("FR", "10000")]
        [TestCase("FR", "01123")]
        [TestCase("FR", "89000")]
        [TestCase("FR", "12345")]
        // Tests patterns that should be valid for Gabon (GA).
        [TestCase("GA", "0123")]
        [TestCase("GA", "1234")]
        [TestCase("GA", "2000")]
        [TestCase("GA", "3248")]
        [TestCase("GA", "4945")]
        [TestCase("GA", "5640")]
        [TestCase("GA", "6208")]
        [TestCase("GA", "7645")]
        [TestCase("GA", "6752")]
        [TestCase("GA", "8782")]
        [TestCase("GA", "9999")]
        // Tests patterns that should be valid for United Kingdom (GB).
        [TestCase("GB", "M11AA")]
        [TestCase("GB", "M11aA")]
        [TestCase("GB", "M11AA")]
        [TestCase("GB", "m11AA")]
        [TestCase("GB", "m11aa")]
        [TestCase("GB", "B338TH")]
        [TestCase("GB", "B338TH")]
        [TestCase("GB", "CR26XH")]
        [TestCase("GB", "CR26XH")]
        [TestCase("GB", "DN551PT")]
        [TestCase("GB", "DN551PT")]
        [TestCase("GB", "W1A1HQ")]
        [TestCase("GB", "W1A1HQ")]
        [TestCase("GB", "EC1A1BB")]
        [TestCase("GB", "EC1A1BB")]
        // Tests patterns that should be valid for Georgia (GE).
        [TestCase("GE", "0123")]
        [TestCase("GE", "1234")]
        [TestCase("GE", "2000")]
        [TestCase("GE", "3248")]
        [TestCase("GE", "4945")]
        [TestCase("GE", "5640")]
        [TestCase("GE", "6208")]
        [TestCase("GE", "7645")]
        [TestCase("GE", "6752")]
        [TestCase("GE", "8782")]
        [TestCase("GE", "9999")]
        // Tests patterns that should be valid for French Guiana (GF).
        [TestCase("GF", "97300")]
        [TestCase("GF", "97301")]
        [TestCase("GF", "97312")]
        [TestCase("GF", "97320")]
        [TestCase("GF", "97332")]
        [TestCase("GF", "97340")]
        [TestCase("GF", "97356")]
        [TestCase("GF", "97362")]
        [TestCase("GF", "97376")]
        [TestCase("GF", "97367")]
        [TestCase("GF", "97387")]
        [TestCase("GF", "97399")]
        // Tests patterns that should be valid for Guernsey (GG).
        [TestCase("GG", "00DF")]
        [TestCase("GG", "03DS")]
        [TestCase("GG", "14RF")]
        [TestCase("GG", "20WK")]
        [TestCase("GG", "34SD")]
        [TestCase("GG", "44PJ")]
        [TestCase("GG", "54KF")]
        [TestCase("GG", "60LS")]
        [TestCase("GG", "74JD")]
        [TestCase("GG", "65MO")]
        [TestCase("GG", "88DF")]
        [TestCase("GG", "99JS")]
        [TestCase("GG", "000DF")]
        [TestCase("GG", "015DS")]
        [TestCase("GG", "126RF")]
        [TestCase("GG", "204WK")]
        [TestCase("GG", "328SD")]
        [TestCase("GG", "405PJ")]
        [TestCase("GG", "560KF")]
        [TestCase("GG", "628LS")]
        [TestCase("GG", "765JD")]
        [TestCase("GG", "672MO")]
        [TestCase("GG", "872DF")]
        [TestCase("GG", "999JS")]
        // Tests patterns that should be valid for Gibraltar (GI).
        [TestCase("GI", "GX111AA")]
        // Tests patterns that should be valid for Greenland (GL).
        [TestCase("GL", "3999")]
        [TestCase("GL", "gl-3999")]
        [TestCase("GL", "GL-3999")]
        [TestCase("GL", "gL 3999")]
        [TestCase("GL", "GL3999")]
        [TestCase("GL", "GL3990")]
        // Tests patterns that should be valid for Guadeloupe (GP).
        [TestCase("GP", "97100")]
        [TestCase("GP", "97101")]
        [TestCase("GP", "97112")]
        [TestCase("GP", "97120")]
        [TestCase("GP", "97132")]
        [TestCase("GP", "97140")]
        [TestCase("GP", "97156")]
        [TestCase("GP", "97162")]
        [TestCase("GP", "97176")]
        [TestCase("GP", "97167")]
        [TestCase("GP", "97187")]
        [TestCase("GP", "97199")]
        // Tests patterns that should be valid for Greece (GR).
        [TestCase("GR", "10000")]
        [TestCase("GR", "31123")]
        [TestCase("GR", "89000")]
        [TestCase("GR", "12345")]
        // Tests patterns that should be valid for South Georgia And The South Sandwich Islands (GS).
        [TestCase("GS", "SIQQ1ZZ")]
        // Tests patterns that should be valid for Guatemala (GT).
        [TestCase("GT", "00000")]
        [TestCase("GT", "01235")]
        [TestCase("GT", "12346")]
        [TestCase("GT", "20004")]
        [TestCase("GT", "32648")]
        [TestCase("GT", "40945")]
        [TestCase("GT", "56640")]
        [TestCase("GT", "62908")]
        [TestCase("GT", "76345")]
        [TestCase("GT", "67552")]
        [TestCase("GT", "87182")]
        [TestCase("GT", "99999")]
        // Tests patterns that should be valid for Guam (GU).
        [TestCase("GU", "96910")]
        [TestCase("GU", "96910")]
        [TestCase("GU", "96911")]
        [TestCase("GU", "96912")]
        [TestCase("GU", "96923")]
        [TestCase("GU", "96924")]
        [TestCase("GU", "96925")]
        [TestCase("GU", "96926")]
        [TestCase("GU", "96927")]
        [TestCase("GU", "96926")]
        [TestCase("GU", "96931")]
        [TestCase("GU", "96932")]
        [TestCase("GU", "969100000")]
        [TestCase("GU", "969103015")]
        [TestCase("GU", "969114126")]
        [TestCase("GU", "969120204")]
        [TestCase("GU", "969234328")]
        [TestCase("GU", "969244405")]
        [TestCase("GU", "969254560")]
        [TestCase("GU", "969260628")]
        [TestCase("GU", "969274765")]
        [TestCase("GU", "969265672")]
        [TestCase("GU", "969318872")]
        [TestCase("GU", "969329999")]
        // Tests patterns that should be valid for Guinea-Bissau (GW).
        [TestCase("GW", "0123")]
        [TestCase("GW", "1234")]
        [TestCase("GW", "2000")]
        [TestCase("GW", "3248")]
        [TestCase("GW", "4945")]
        [TestCase("GW", "5640")]
        [TestCase("GW", "6208")]
        [TestCase("GW", "7645")]
        [TestCase("GW", "6752")]
        [TestCase("GW", "8782")]
        [TestCase("GW", "9999")]
        // Tests patterns that should be valid for Heard Island And Mcdonald Islands (HM).
        [TestCase("HM", "0123")]
        [TestCase("HM", "1234")]
        [TestCase("HM", "2000")]
        [TestCase("HM", "3248")]
        [TestCase("HM", "4945")]
        [TestCase("HM", "5640")]
        [TestCase("HM", "6208")]
        [TestCase("HM", "7645")]
        [TestCase("HM", "6752")]
        [TestCase("HM", "8782")]
        [TestCase("HM", "9999")]
        // Tests patterns that should be valid for Honduras (HN).
        [TestCase("HN", "00000")]
        [TestCase("HN", "01235")]
        [TestCase("HN", "12346")]
        [TestCase("HN", "20004")]
        [TestCase("HN", "32648")]
        [TestCase("HN", "40945")]
        [TestCase("HN", "56640")]
        [TestCase("HN", "62908")]
        [TestCase("HN", "76345")]
        [TestCase("HN", "67552")]
        [TestCase("HN", "87182")]
        [TestCase("HN", "99999")]
        // Tests patterns that should be valid for Croatia (HR).
        [TestCase("HR", "00000")]
        [TestCase("HR", "01235")]
        [TestCase("HR", "12346")]
        [TestCase("HR", "20004")]
        [TestCase("HR", "32648")]
        [TestCase("HR", "40945")]
        [TestCase("HR", "56640")]
        [TestCase("HR", "62908")]
        [TestCase("HR", "76345")]
        [TestCase("HR", "67552")]
        [TestCase("HR", "87182")]
        [TestCase("HR", "99999")]
        // Tests patterns that should be valid for Haiti (HT).
        [TestCase("HT", "0123")]
        [TestCase("HT", "1234")]
        [TestCase("HT", "2000")]
        [TestCase("HT", "3248")]
        [TestCase("HT", "4945")]
        [TestCase("HT", "5640")]
        [TestCase("HT", "6208")]
        [TestCase("HT", "7645")]
        [TestCase("HT", "6752")]
        [TestCase("HT", "8782")]
        [TestCase("HT", "9999")]
        // Tests patterns that should be valid for Hungary (HU).
        [TestCase("HU", "1000")]
        [TestCase("HU", "2077")]
        [TestCase("HU", "2650")]
        [TestCase("HU", "4241")]
        // Tests patterns that should be valid for Indonesia (ID).
        [TestCase("ID", "10000")]
        [TestCase("ID", "31123")]
        [TestCase("ID", "89000")]
        [TestCase("ID", "89007")]
        [TestCase("ID", "12340")]
        // Tests patterns that should be valid for Israel (IL).
        [TestCase("IL", "0110023")]
        [TestCase("IL", "1084023")]
        [TestCase("IL", "3108701")]
        [TestCase("IL", "4201907")]
        [TestCase("IL", "5403506")]
        [TestCase("IL", "6177008")]
        // Tests patterns that should be valid for Isle Of Man (IM).
        [TestCase("IM", "00DF")]
        [TestCase("IM", "04DS")]
        [TestCase("IM", "18RF")]
        [TestCase("IM", "23WK")]
        [TestCase("IM", "34SD")]
        [TestCase("IM", "40PJ")]
        [TestCase("IM", "59KF")]
        [TestCase("IM", "68LS")]
        [TestCase("IM", "71JD")]
        [TestCase("IM", "66MO")]
        [TestCase("IM", "85DF")]
        [TestCase("IM", "99JS")]
        [TestCase("IM", "00DF")]
        [TestCase("IM", "000DF")]
        [TestCase("IM", "014DS")]
        [TestCase("IM", "128RF")]
        [TestCase("IM", "203WK")]
        [TestCase("IM", "324SD")]
        [TestCase("IM", "400PJ")]
        [TestCase("IM", "569KF")]
        [TestCase("IM", "628LS")]
        [TestCase("IM", "761JD")]
        [TestCase("IM", "676MO")]
        [TestCase("IM", "875DF")]
        [TestCase("IM", "999JS")]
        [TestCase("IM", "000DF")]
        [TestCase("IM", "IM00DF")]
        [TestCase("IM", "IM04DS")]
        [TestCase("IM", "IM18RF")]
        [TestCase("IM", "IM23WK")]
        [TestCase("IM", "IM34SD")]
        [TestCase("IM", "IM40PJ")]
        [TestCase("IM", "IM59KF")]
        [TestCase("IM", "IM68LS")]
        [TestCase("IM", "IM71JD")]
        [TestCase("IM", "IM66MO")]
        [TestCase("IM", "IM85DF")]
        [TestCase("IM", "IM99JS")]
        [TestCase("IM", "IM00DF")]
        [TestCase("IM", "IM000DF")]
        [TestCase("IM", "IM014DS")]
        [TestCase("IM", "IM128RF")]
        [TestCase("IM", "IM203WK")]
        [TestCase("IM", "IM324SD")]
        [TestCase("IM", "IM400PJ")]
        [TestCase("IM", "IM569KF")]
        [TestCase("IM", "IM628LS")]
        [TestCase("IM", "IM761JD")]
        [TestCase("IM", "IM676MO")]
        [TestCase("IM", "IM875DF")]
        [TestCase("IM", "IM999JS")]
        [TestCase("IM", "IM000DF")]
        [TestCase("IM", "IM00DF")]
        [TestCase("IM", "IM04DS")]
        [TestCase("IM", "IM18RF")]
        [TestCase("IM", "IM23WK")]
        [TestCase("IM", "IM34SD")]
        [TestCase("IM", "IM40PJ")]
        [TestCase("IM", "IM59KF")]
        [TestCase("IM", "IM68LS")]
        [TestCase("IM", "IM71JD")]
        [TestCase("IM", "IM66MO")]
        [TestCase("IM", "IM85DF")]
        [TestCase("IM", "IM99JS")]
        [TestCase("IM", "IM00DF")]
        // Tests patterns that should be valid for India (IN).
        [TestCase("IN", "110000")]
        [TestCase("IN", "342600")]
        [TestCase("IN", "810185")]
        [TestCase("IN", "810 185")]
        // Tests patterns that should be valid for British Indian Ocean Territory (IO).
        [TestCase("IO", "BBND1ZZ")]
        // Tests patterns that should be valid for Iraq (IQ).
        [TestCase("IQ", "12346")]
        [TestCase("IQ", "32648")]
        [TestCase("IQ", "40945")]
        [TestCase("IQ", "56640")]
        [TestCase("IQ", "62908")]
        // Tests patterns that should be valid for Iran (IR).
        [TestCase("IR", "0000000000")]
        [TestCase("IR", "0144942325")]
        [TestCase("IR", "1282353436")]
        [TestCase("IR", "2037570044")]
        [TestCase("IR", "3243436478")]
        [TestCase("IR", "4008279475")]
        [TestCase("IR", "5697836450")]
        [TestCase("IR", "6282469088")]
        [TestCase("IR", "7611343495")]
        [TestCase("IR", "6767185502")]
        [TestCase("IR", "8752391832")]
        [TestCase("IR", "9999999999")]
        // Tests patterns that should be valid for Iceland (IS).
        [TestCase("IS", "000")]
        [TestCase("IS", "035")]
        [TestCase("IS", "146")]
        [TestCase("IS", "204")]
        [TestCase("IS", "348")]
        [TestCase("IS", "445")]
        [TestCase("IS", "540")]
        [TestCase("IS", "608")]
        [TestCase("IS", "745")]
        [TestCase("IS", "652")]
        [TestCase("IS", "882")]
        [TestCase("IS", "999")]
        // Tests patterns that should be valid for Italy (IT).
        [TestCase("IT", "00123")]
        [TestCase("IT", "02123")]
        [TestCase("IT", "31001")]
        [TestCase("IT", "42007")]
        [TestCase("IT", "54006")]
        [TestCase("IT", "91008")]
        // Tests patterns that should be valid for Jersey (JE).
        [TestCase("JE", "00AS")]
        [TestCase("JE", "25GS")]
        [TestCase("JE", "36DF")]
        [TestCase("JE", "44DS")]
        [TestCase("JE", "78RF")]
        [TestCase("JE", "75WK")]
        [TestCase("JE", "50SD")]
        [TestCase("JE", "88PJ")]
        [TestCase("JE", "95KF")]
        [TestCase("JE", "02LS")]
        [TestCase("JE", "32JD")]
        [TestCase("JE", "99MO")]
        [TestCase("JE", "00AS")]
        [TestCase("JE", "042GS")]
        [TestCase("JE", "153DF")]
        [TestCase("JE", "274DS")]
        [TestCase("JE", "337RF")]
        [TestCase("JE", "477WK")]
        [TestCase("JE", "535SD")]
        [TestCase("JE", "668PJ")]
        [TestCase("JE", "749KF")]
        [TestCase("JE", "680LS")]
        [TestCase("JE", "893JD")]
        [TestCase("JE", "999MO")]
        // Tests patterns that should be valid for Jordan (JO).
        [TestCase("JO", "00000")]
        [TestCase("JO", "01235")]
        [TestCase("JO", "12346")]
        [TestCase("JO", "20004")]
        [TestCase("JO", "32648")]
        [TestCase("JO", "40945")]
        [TestCase("JO", "56640")]
        [TestCase("JO", "62908")]
        [TestCase("JO", "76345")]
        [TestCase("JO", "67552")]
        [TestCase("JO", "87182")]
        [TestCase("JO", "99999")]
        // Tests patterns that should be valid for Japan (JP).
        [TestCase("JP", "000-0000")]
        [TestCase("JP", "000-0999")]
        [TestCase("JP", "010-0000")]
        [TestCase("JP", "0100999")]
        [TestCase("JP", "880-0123")]
        [TestCase("JP", "900-0123")]
        // Tests patterns that should be valid for Kyrgyzstan (KG).
        [TestCase("KG", "000000")]
        [TestCase("KG", "023145")]
        [TestCase("KG", "134256")]
        [TestCase("KG", "200074")]
        [TestCase("KG", "364238")]
        [TestCase("KG", "494075")]
        [TestCase("KG", "564630")]
        [TestCase("KG", "690268")]
        [TestCase("KG", "734645")]
        [TestCase("KG", "655782")]
        [TestCase("KG", "818792")]
        [TestCase("KG", "999999")]
        // Tests patterns that should be valid for Cambodia (KH).
        [TestCase("KH", "00000")]
        [TestCase("KH", "01235")]
        [TestCase("KH", "12346")]
        [TestCase("KH", "20004")]
        [TestCase("KH", "32648")]
        [TestCase("KH", "40945")]
        [TestCase("KH", "56640")]
        [TestCase("KH", "62908")]
        [TestCase("KH", "76345")]
        [TestCase("KH", "67552")]
        [TestCase("KH", "87182")]
        [TestCase("KH", "99999")]
        // Tests patterns that should be valid for Korea (KR).
        [TestCase("KR", "110000")]
        [TestCase("KR", "342600")]
        [TestCase("KR", "610185")]
        [TestCase("KR", "410-185")]
        [TestCase("KR", "710-185")]
        // Tests patterns that should be valid for Cayman Islands (KY).
        [TestCase("KY", "00000")]
        [TestCase("KY", "01235")]
        [TestCase("KY", "12346")]
        [TestCase("KY", "20004")]
        [TestCase("KY", "32648")]
        [TestCase("KY", "40945")]
        [TestCase("KY", "56640")]
        [TestCase("KY", "62908")]
        [TestCase("KY", "76345")]
        [TestCase("KY", "67552")]
        [TestCase("KY", "87182")]
        [TestCase("KY", "99999")]
        // Tests patterns that should be valid for Kazakhstan (KZ).
        [TestCase("KZ", "000000")]
        [TestCase("KZ", "023145")]
        [TestCase("KZ", "134256")]
        [TestCase("KZ", "200074")]
        [TestCase("KZ", "364238")]
        [TestCase("KZ", "494075")]
        [TestCase("KZ", "564630")]
        [TestCase("KZ", "690268")]
        [TestCase("KZ", "734645")]
        [TestCase("KZ", "655782")]
        [TestCase("KZ", "818792")]
        [TestCase("KZ", "999999")]
        // Tests patterns that should be valid for Lao People'S Democratic Re
        [TestCase("LA", "00000")]
        [TestCase("LA", "01235")]
        [TestCase("LA", "12346")]
        [TestCase("LA", "20004")]
        [TestCase("LA", "32648")]
        [TestCase("LA", "40945")]
        [TestCase("LA", "56640")]
        [TestCase("LA", "62908")]
        [TestCase("LA", "76345")]
        [TestCase("LA", "67552")]
        [TestCase("LA", "87182")]
        [TestCase("LA", "99999")]
        // Tests patterns that should be valid for Lebanon (LB).
        [TestCase("LB", "00000000")]
        [TestCase("LB", "01442325")]
        [TestCase("LB", "12853436")]
        [TestCase("LB", "20370044")]
        [TestCase("LB", "32436478")]
        [TestCase("LB", "40079475")]
        [TestCase("LB", "56936450")]
        [TestCase("LB", "62869088")]
        [TestCase("LB", "76143495")]
        [TestCase("LB", "67685502")]
        [TestCase("LB", "87591832")]
        [TestCase("LB", "99999999")]
        // Tests patterns that should be valid for Liechtenstein (LI).
        [TestCase("LI", "9485")]
        [TestCase("LI", "9489")]
        [TestCase("LI", "9490")]
        [TestCase("LI", "9498")]
        // Tests patterns that should be valid for Sri Lanka (LK).
        [TestCase("LK", "00000")]
        [TestCase("LK", "10070")]
        [TestCase("LK", "20767")]
        [TestCase("LK", "26560")]
        [TestCase("LK", "32451")]
        [TestCase("LK", "09112")]
        [TestCase("LK", "48221")]
        [TestCase("LK", "54636")]
        [TestCase("LK", "65050")]
        [TestCase("LK", "70162")]
        [TestCase("LK", "81271")]
        [TestCase("LK", "92686")]
        // Tests patterns that should be valid for Liberia (LR).
        [TestCase("LR", "0123")]
        [TestCase("LR", "1234")]
        [TestCase("LR", "2000")]
        [TestCase("LR", "3248")]
        [TestCase("LR", "4945")]
        [TestCase("LR", "5640")]
        [TestCase("LR", "6208")]
        [TestCase("LR", "7645")]
        [TestCase("LR", "6752")]
        [TestCase("LR", "8782")]
        [TestCase("LR", "9999")]
        // Tests patterns that should be valid for Lesotho (LS).
        [TestCase("LS", "000")]
        [TestCase("LS", "015")]
        [TestCase("LS", "126")]
        [TestCase("LS", "204")]
        [TestCase("LS", "328")]
        [TestCase("LS", "405")]
        [TestCase("LS", "560")]
        [TestCase("LS", "628")]
        [TestCase("LS", "765")]
        [TestCase("LS", "672")]
        [TestCase("LS", "872")]
        [TestCase("LS", "999")]
        // Tests patterns that should be valid for Lithuania (LT).
        [TestCase("LT", "21499")]
        [TestCase("LT", "01499")]
        [TestCase("LT", "lT-31499")]
        [TestCase("LT", "LT-01499")]
        [TestCase("LT", "lt81499")]
        [TestCase("LT", "LT71499")]
        [TestCase("LT", "LT56990")]
        // Tests patterns that should be valid for Luxembourg (LU).
        [TestCase("LU", "0123")]
        [TestCase("LU", "1234")]
        [TestCase("LU", "2000")]
        [TestCase("LU", "3248")]
        [TestCase("LU", "4945")]
        [TestCase("LU", "5640")]
        [TestCase("LU", "6208")]
        [TestCase("LU", "7645")]
        [TestCase("LU", "6752")]
        [TestCase("LU", "8782")]
        [TestCase("LU", "9999")]
        // Tests patterns that should be valid for Latvia (LV).
        [TestCase("LV", "0123")]
        [TestCase("LV", "1234")]
        [TestCase("LV", "2000")]
        [TestCase("LV", "3248")]
        [TestCase("LV", "4945")]
        [TestCase("LV", "5640")]
        [TestCase("LV", "6208")]
        [TestCase("LV", "7645")]
        [TestCase("LV", "6752")]
        [TestCase("LV", "8782")]
        [TestCase("LV", "9999")]
        // Tests patterns that should be valid for Libya (LY).
        [TestCase("LY", "00000")]
        [TestCase("LY", "01235")]
        [TestCase("LY", "12346")]
        [TestCase("LY", "20004")]
        [TestCase("LY", "32648")]
        [TestCase("LY", "40945")]
        [TestCase("LY", "56640")]
        [TestCase("LY", "62908")]
        [TestCase("LY", "76345")]
        [TestCase("LY", "67552")]
        [TestCase("LY", "87182")]
        [TestCase("LY", "99999")]
        // Tests patterns that should be valid for Morocco (MA).
        [TestCase("MA", "11 302")]
        [TestCase("MA", "24 023")]
        [TestCase("MA", "45001")]
        [TestCase("MA", "89607")]
        [TestCase("MA", "86096")]
        [TestCase("MA", "85808")]
        // Tests patterns that should be valid for Monaco (MC).
        [TestCase("MC", "MC-98000")]
        [TestCase("MC", "MC-98012")]
        [TestCase("MC", "MC 98023")]
        [TestCase("MC", "mc98089")]
        [TestCase("MC", "MC98099")]
        [TestCase("MC", "Mc98077")]
        [TestCase("MC", "mC98066")]
        [TestCase("MC", "98089")]
        [TestCase("MC", "98099")]
        [TestCase("MC", "98077")]
        [TestCase("MC", "98066")]
        // Tests patterns that should be valid for Moldova (MD).
        [TestCase("MD", "1499")]
        [TestCase("MD", "md-1499")]
        [TestCase("MD", "MD-1499")]
        [TestCase("MD", "md1499")]
        [TestCase("MD", "MD0499")]
        [TestCase("MD", "MD0099")]
        [TestCase("MD", "mD6990")]
        [TestCase("MD", "0123")]
        [TestCase("MD", "1234")]
        [TestCase("MD", "2000")]
        [TestCase("MD", "3248")]
        [TestCase("MD", "4945")]
        [TestCase("MD", "5640")]
        [TestCase("MD", "6208")]
        [TestCase("MD", "7645")]
        [TestCase("MD", "6752")]
        [TestCase("MD", "8782")]
        [TestCase("MD", "9999")]
        // Tests patterns that should be valid for Montenegro (ME).
        [TestCase("ME", "81302")]
        [TestCase("ME", "84023")]
        [TestCase("ME", "85001")]
        [TestCase("ME", "81607")]
        [TestCase("ME", "84096")]
        [TestCase("ME", "85808")]
        // Tests patterns that should be valid for Saint Martin (MF).
        [TestCase("MF", "97800")]
        [TestCase("MF", "97805")]
        [TestCase("MF", "97816")]
        [TestCase("MF", "97824")]
        [TestCase("MF", "97838")]
        [TestCase("MF", "97845")]
        [TestCase("MF", "97850")]
        [TestCase("MF", "97868")]
        [TestCase("MF", "97875")]
        [TestCase("MF", "97862")]
        [TestCase("MF", "97882")]
        [TestCase("MF", "97899")]
        // Tests patterns that should be valid for Madagascar (MG).
        [TestCase("MG", "000")]
        [TestCase("MG", "015")]
        [TestCase("MG", "126")]
        [TestCase("MG", "204")]
        [TestCase("MG", "328")]
        [TestCase("MG", "405")]
        [TestCase("MG", "560")]
        [TestCase("MG", "628")]
        [TestCase("MG", "765")]
        [TestCase("MG", "672")]
        [TestCase("MG", "872")]
        [TestCase("MG", "999")]
        // Tests patterns that should be valid for Marshall Islands (MH).
        [TestCase("MH", "96960")]
        [TestCase("MH", "96960")]
        [TestCase("MH", "96961")]
        [TestCase("MH", "96962")]
        [TestCase("MH", "96963")]
        [TestCase("MH", "96964")]
        [TestCase("MH", "96965")]
        [TestCase("MH", "96976")]
        [TestCase("MH", "96977")]
        [TestCase("MH", "96976")]
        [TestCase("MH", "96978")]
        [TestCase("MH", "96979")]
        [TestCase("MH", "96970")]
        [TestCase("MH", "969600000")]
        [TestCase("MH", "969604423")]
        [TestCase("MH", "969612534")]
        [TestCase("MH", "969627700")]
        [TestCase("MH", "969633364")]
        [TestCase("MH", "969648794")]
        [TestCase("MH", "969657364")]
        [TestCase("MH", "969762690")]
        [TestCase("MH", "969771434")]
        [TestCase("MH", "969767855")]
        [TestCase("MH", "969782918")]
        [TestCase("MH", "969799999")]
        [TestCase("MH", "969700000")]
        // Tests patterns that should be valid for Macedonia (MK).
        [TestCase("MK", "0123")]
        [TestCase("MK", "1234")]
        [TestCase("MK", "2000")]
        [TestCase("MK", "3248")]
        [TestCase("MK", "4945")]
        [TestCase("MK", "5640")]
        [TestCase("MK", "6208")]
        [TestCase("MK", "7645")]
        [TestCase("MK", "6752")]
        [TestCase("MK", "8782")]
        [TestCase("MK", "9999")]
        // Tests patterns that should be valid for Myanmar (MM).
        [TestCase("MM", "00000")]
        [TestCase("MM", "01235")]
        [TestCase("MM", "12346")]
        [TestCase("MM", "20004")]
        [TestCase("MM", "32648")]
        [TestCase("MM", "40945")]
        [TestCase("MM", "56640")]
        [TestCase("MM", "62908")]
        [TestCase("MM", "76345")]
        [TestCase("MM", "67552")]
        [TestCase("MM", "87182")]
        [TestCase("MM", "99999")]
        // Tests patterns that should be valid for Mongolia (MN).
        [TestCase("MN", "00000")]
        [TestCase("MN", "01235")]
        [TestCase("MN", "12346")]
        [TestCase("MN", "20004")]
        [TestCase("MN", "32648")]
        [TestCase("MN", "40945")]
        [TestCase("MN", "56640")]
        [TestCase("MN", "62908")]
        [TestCase("MN", "76345")]
        [TestCase("MN", "67552")]
        [TestCase("MN", "87182")]
        [TestCase("MN", "99999")]
        // Tests patterns that should be valid for Northern Mariana Islands (MP).
        [TestCase("MP", "96950")]
        [TestCase("MP", "96951")]
        [TestCase("MP", "96952")]
        [TestCase("MP", "969500000")]
        [TestCase("MP", "969500143")]
        [TestCase("MP", "969501254")]
        [TestCase("MP", "969502070")]
        [TestCase("MP", "969513234")]
        [TestCase("MP", "969514074")]
        [TestCase("MP", "969515634")]
        [TestCase("MP", "969516260")]
        [TestCase("MP", "969527644")]
        [TestCase("MP", "969526785")]
        [TestCase("MP", "969528798")]
        [TestCase("MP", "969529999")]
        // Tests patterns that should be valid for Martinique (MQ).
        [TestCase("MQ", "97200")]
        [TestCase("MQ", "97201")]
        [TestCase("MQ", "97212")]
        [TestCase("MQ", "97220")]
        [TestCase("MQ", "97232")]
        [TestCase("MQ", "97240")]
        [TestCase("MQ", "97256")]
        [TestCase("MQ", "97262")]
        [TestCase("MQ", "97276")]
        [TestCase("MQ", "97267")]
        [TestCase("MQ", "97287")]
        [TestCase("MQ", "97299")]
        // Tests patterns that should be valid for Malta (MT).
        [TestCase("MT", "AAA0000")]
        [TestCase("MT", "ASD0132")]
        [TestCase("MT", "BJR1243")]
        [TestCase("MT", "CDW2004")]
        [TestCase("MT", "DES3247")]
        [TestCase("MT", "EOP4047")]
        [TestCase("MT", "FNK5645")]
        [TestCase("MT", "GFL6208")]
        [TestCase("MT", "HLJ7649")]
        [TestCase("MT", "IDM6750")]
        [TestCase("MT", "JSD8783")]
        [TestCase("MT", "KNJ9999")]
        [TestCase("MT", "LOD0000")]
        [TestCase("MT", "MED0132")]
        [TestCase("MT", "NNR1243")]
        [TestCase("MT", "OLW2004")]
        [TestCase("MT", "PSS3247")]
        [TestCase("MT", "QDP4047")]
        [TestCase("MT", "RNK5645")]
        [TestCase("MT", "SEL6208")]
        [TestCase("MT", "TMJ7649")]
        [TestCase("MT", "UFM6750")]
        [TestCase("MT", "VED8783")]
        [TestCase("MT", "WLJ9999")]
        [TestCase("MT", "XMD0000")]
        [TestCase("MT", "YED0132")]
        [TestCase("MT", "ZLR1243")]
        [TestCase("MT", "ZZZ9999")]
        // Tests patterns that should be valid for Mexico (MX).
        [TestCase("MX", "09302")]
        [TestCase("MX", "10023")]
        [TestCase("MX", "31001")]
        [TestCase("MX", "42007")]
        [TestCase("MX", "54006")]
        [TestCase("MX", "61008")]
        // Tests patterns that should be valid for Malaysia (MY).
        [TestCase("MY", "10023")]
        [TestCase("MY", "31001")]
        [TestCase("MY", "42007")]
        [TestCase("MY", "54006")]
        [TestCase("MY", "61008")]
        // Tests patterns that should be valid for Mozambique (MZ).
        [TestCase("MZ", "0123")]
        [TestCase("MZ", "1234")]
        [TestCase("MZ", "2000")]
        [TestCase("MZ", "3248")]
        [TestCase("MZ", "4945")]
        [TestCase("MZ", "5640")]
        [TestCase("MZ", "6208")]
        [TestCase("MZ", "7645")]
        [TestCase("MZ", "6752")]
        [TestCase("MZ", "8782")]
        [TestCase("MZ", "9999")]
        // Tests patterns that should be valid for Namibia (NA).
        [TestCase("NA", "90000")]
        [TestCase("NA", "90015")]
        [TestCase("NA", "90126")]
        [TestCase("NA", "90204")]
        [TestCase("NA", "91328")]
        [TestCase("NA", "91405")]
        [TestCase("NA", "91560")]
        [TestCase("NA", "91628")]
        [TestCase("NA", "92765")]
        [TestCase("NA", "92672")]
        [TestCase("NA", "92872")]
        [TestCase("NA", "92999")]
        // Tests patterns that should be valid for New Caledonia (NC).
        [TestCase("NC", "98800")]
        [TestCase("NC", "98802")]
        [TestCase("NC", "98813")]
        [TestCase("NC", "98820")]
        [TestCase("NC", "98836")]
        [TestCase("NC", "98884")]
        [TestCase("NC", "98895")]
        [TestCase("NC", "98896")]
        [TestCase("NC", "98897")]
        [TestCase("NC", "98896")]
        [TestCase("NC", "98898")]
        [TestCase("NC", "98899")]
        // Tests patterns that should be valid for Niger (NE).
        [TestCase("NE", "0123")]
        [TestCase("NE", "1234")]
        [TestCase("NE", "2000")]
        [TestCase("NE", "3248")]
        [TestCase("NE", "4945")]
        [TestCase("NE", "5640")]
        [TestCase("NE", "6208")]
        [TestCase("NE", "7645")]
        [TestCase("NE", "6752")]
        [TestCase("NE", "8782")]
        [TestCase("NE", "9999")]
        // Tests patterns that should be valid for Norfolk Island (NF).
        [TestCase("NF", "0123")]
        [TestCase("NF", "1234")]
        [TestCase("NF", "2000")]
        [TestCase("NF", "3248")]
        [TestCase("NF", "4945")]
        [TestCase("NF", "5640")]
        [TestCase("NF", "6208")]
        [TestCase("NF", "7645")]
        [TestCase("NF", "6752")]
        [TestCase("NF", "8782")]
        [TestCase("NF", "9999")]
        // Tests patterns that should be valid for Nigeria (NG).
        [TestCase("NG", "009999")]
        [TestCase("NG", "018010")]
        [TestCase("NG", "110000")]
        [TestCase("NG", "342600")]
        [TestCase("NG", "810185")]
        [TestCase("NG", "810185")]
        // Tests patterns that should be valid for Nicaragua (NI).
        [TestCase("NI", "00000")]
        [TestCase("NI", "01235")]
        [TestCase("NI", "12346")]
        [TestCase("NI", "20004")]
        [TestCase("NI", "32648")]
        [TestCase("NI", "40945")]
        [TestCase("NI", "56640")]
        [TestCase("NI", "62908")]
        [TestCase("NI", "76345")]
        [TestCase("NI", "67552")]
        [TestCase("NI", "87182")]
        [TestCase("NI", "99999")]
        // Tests patterns that should be valid for Netherlands (NL).
        [TestCase("NL", "1236RF")]
        [TestCase("NL", "2044WK")]
        [TestCase("NL", "4075PJ")]
        [TestCase("NL", "5650KF")]
        [TestCase("NL", "6288LS")]
        [TestCase("NL", "7695JD")]
        [TestCase("NL", "6702MO")]
        [TestCase("NL", "8732DF")]
        [TestCase("NL", "9999JS")]
        [TestCase("NL", "2331 PS")]
        // Tests patterns that should be valid for Norway (NO).
        [TestCase("NO", "0912")]
        [TestCase("NO", "0821")]
        [TestCase("NO", "0666")]
        [TestCase("NO", "0000")]
        [TestCase("NO", "1000")]
        [TestCase("NO", "2077")]
        [TestCase("NO", "2650")]
        [TestCase("NO", "4241")]
        // Tests patterns that should be valid for Nepal (NP).
        [TestCase("NP", "00000")]
        [TestCase("NP", "01235")]
        [TestCase("NP", "12346")]
        [TestCase("NP", "20004")]
        [TestCase("NP", "32648")]
        [TestCase("NP", "40945")]
        [TestCase("NP", "56640")]
        [TestCase("NP", "62908")]
        [TestCase("NP", "76345")]
        [TestCase("NP", "67552")]
        [TestCase("NP", "87182")]
        [TestCase("NP", "99999")]
        // Tests patterns that should be valid for New Zealand (NZ).
        [TestCase("NZ", "0912")]
        [TestCase("NZ", "0821")]
        [TestCase("NZ", "0666")]
        [TestCase("NZ", "0000")]
        [TestCase("NZ", "1000")]
        [TestCase("NZ", "2077")]
        [TestCase("NZ", "2650")]
        [TestCase("NZ", "4241")]
        // Tests patterns that should be valid for Oman (OM).
        [TestCase("OM", "000")]
        [TestCase("OM", "015")]
        [TestCase("OM", "126")]
        [TestCase("OM", "204")]
        [TestCase("OM", "328")]
        [TestCase("OM", "405")]
        [TestCase("OM", "560")]
        [TestCase("OM", "628")]
        [TestCase("OM", "765")]
        [TestCase("OM", "672")]
        [TestCase("OM", "872")]
        [TestCase("OM", "999")]
        // Tests patterns that should be valid for Panama (PA).
        [TestCase("PA", "000000")]
        [TestCase("PA", "023145")]
        [TestCase("PA", "134256")]
        [TestCase("PA", "200074")]
        [TestCase("PA", "364238")]
        [TestCase("PA", "494075")]
        [TestCase("PA", "564630")]
        [TestCase("PA", "690268")]
        [TestCase("PA", "734645")]
        [TestCase("PA", "655782")]
        [TestCase("PA", "818792")]
        [TestCase("PA", "999999")]
        // Tests patterns that should be valid for Peru (PE).
        [TestCase("PE", "00000")]
        [TestCase("PE", "01235")]
        [TestCase("PE", "12346")]
        [TestCase("PE", "20004")]
        [TestCase("PE", "32648")]
        [TestCase("PE", "40945")]
        [TestCase("PE", "56640")]
        [TestCase("PE", "62908")]
        [TestCase("PE", "76345")]
        [TestCase("PE", "67552")]
        [TestCase("PE", "87182")]
        [TestCase("PE", "99999")]
        // Tests patterns that should be valid for French Polynesia (PF).
        [TestCase("PF", "98700")]
        [TestCase("PF", "98725")]
        [TestCase("PF", "98736")]
        [TestCase("PF", "98704")]
        [TestCase("PF", "98768")]
        [TestCase("PF", "98795")]
        [TestCase("PF", "98760")]
        [TestCase("PF", "98798")]
        [TestCase("PF", "98735")]
        [TestCase("PF", "98752")]
        [TestCase("PF", "98712")]
        [TestCase("PF", "98799")]
        // Tests patterns that should be valid for Papua New Guinea (PG).
        [TestCase("PG", "000")]
        [TestCase("PG", "015")]
        [TestCase("PG", "126")]
        [TestCase("PG", "204")]
        [TestCase("PG", "328")]
        [TestCase("PG", "405")]
        [TestCase("PG", "560")]
        [TestCase("PG", "628")]
        [TestCase("PG", "765")]
        [TestCase("PG", "672")]
        [TestCase("PG", "872")]
        [TestCase("PG", "999")]
        // Tests patterns that should be valid for Philippines (PH).
        [TestCase("PH", "0123")]
        [TestCase("PH", "1234")]
        [TestCase("PH", "2000")]
        [TestCase("PH", "3248")]
        [TestCase("PH", "4945")]
        [TestCase("PH", "5640")]
        [TestCase("PH", "6208")]
        [TestCase("PH", "7645")]
        [TestCase("PH", "6752")]
        [TestCase("PH", "8782")]
        [TestCase("PH", "9999")]
        // Tests patterns that should be valid for Pakistan (PK).
        [TestCase("PK", "11302")]
        [TestCase("PK", "24023")]
        [TestCase("PK", "45001")]
        [TestCase("PK", "89607")]
        [TestCase("PK", "86096")]
        [TestCase("PK", "85808")]
        // Tests patterns that should be valid for Poland (PL).
        [TestCase("PL", "01302")]
        [TestCase("PL", "11302")]
        [TestCase("PL", "24023")]
        [TestCase("PL", "45001")]
        [TestCase("PL", "89607")]
        [TestCase("PL", "86096")]
        [TestCase("PL", "85808")]
        [TestCase("PL", "06-096")]
        [TestCase("PL", "85-808")]
        // Tests patterns that should be valid for Saint Pierre And Miquelon (PM).
        [TestCase("PM", "97500")]
        // Tests patterns that should be valid for Pitcairn (PN).
        [TestCase("PN", "PCRN1ZZ")]
        // Tests patterns that should be valid for Puerto Rico (PR).
        [TestCase("PR", "01302")]
        [TestCase("PR", "00802")]
        [TestCase("PR", "11302")]
        [TestCase("PR", "24023")]
        [TestCase("PR", "45001")]
        [TestCase("PR", "89607")]
        [TestCase("PR", "86096")]
        [TestCase("PR", "85808")]
        [TestCase("PR", "06096")]
        [TestCase("PR", "85808")]
        // Tests patterns that should be valid for Palestinian Territory (PS).
        [TestCase("PS", "00000")]
        [TestCase("PS", "01235")]
        [TestCase("PS", "12346")]
        [TestCase("PS", "20004")]
        [TestCase("PS", "32648")]
        [TestCase("PS", "40945")]
        [TestCase("PS", "56640")]
        [TestCase("PS", "62908")]
        [TestCase("PS", "76345")]
        [TestCase("PS", "67552")]
        [TestCase("PS", "87182")]
        [TestCase("PS", "99999")]
        // Tests patterns that should be valid for Portugal (PT).
        [TestCase("PT", "1282353")]
        [TestCase("PT", "2037570")]
        [TestCase("PT", "3243436")]
        [TestCase("PT", "4008279")]
        [TestCase("PT", "5697836")]
        [TestCase("PT", "6282469")]
        [TestCase("PT", "7611343")]
        [TestCase("PT", "6767185")]
        [TestCase("PT", "8752391")]
        [TestCase("PT", "9999999")]
        // Tests patterns that should be valid for Palau (PW).
        [TestCase("PW", "96940")]
        // Tests patterns that should be valid for Paraguay (PY).
        [TestCase("PY", "0123")]
        [TestCase("PY", "1234")]
        [TestCase("PY", "2000")]
        [TestCase("PY", "3248")]
        [TestCase("PY", "4945")]
        [TestCase("PY", "5640")]
        [TestCase("PY", "6208")]
        [TestCase("PY", "7645")]
        [TestCase("PY", "6752")]
        [TestCase("PY", "8782")]
        [TestCase("PY", "9999")]
        // Tests patterns that should be valid for Réunion (RE).
        [TestCase("RE", "97400")]
        [TestCase("RE", "97402")]
        [TestCase("RE", "97413")]
        [TestCase("RE", "97420")]
        [TestCase("RE", "97436")]
        [TestCase("RE", "97449")]
        [TestCase("RE", "97456")]
        [TestCase("RE", "97469")]
        [TestCase("RE", "97473")]
        [TestCase("RE", "97465")]
        [TestCase("RE", "97481")]
        [TestCase("RE", "97499")]
        // Tests patterns that should be valid for Romania (RO).
        [TestCase("RO", "018010")]
        [TestCase("RO", "110000")]
        [TestCase("RO", "342600")]
        [TestCase("RO", "810185")]
        [TestCase("RO", "810185")]
        // Tests patterns that should be valid for Serbia (RS).
        [TestCase("RS", "10070")]
        [TestCase("RS", "20767")]
        [TestCase("RS", "26560")]
        [TestCase("RS", "32451")]
        // Tests patterns that should be valid for Russian Federation (RU).
        [TestCase("RU", "110000")]
        [TestCase("RU", "342600")]
        [TestCase("RU", "610185")]
        [TestCase("RU", "410185")]
        // Tests patterns that should be valid for Saudi Arabia (SA).
        [TestCase("SA", "00000")]
        [TestCase("SA", "03145")]
        [TestCase("SA", "14256")]
        [TestCase("SA", "20074")]
        [TestCase("SA", "34238")]
        [TestCase("SA", "44075")]
        [TestCase("SA", "54630")]
        [TestCase("SA", "60268")]
        [TestCase("SA", "74645")]
        [TestCase("SA", "65782")]
        [TestCase("SA", "88792")]
        [TestCase("SA", "99999")]
        [TestCase("SA", "000000000")]
        [TestCase("SA", "031452003")]
        [TestCase("SA", "142563114")]
        [TestCase("SA", "200740220")]
        [TestCase("SA", "342386334")]
        [TestCase("SA", "440759444")]
        [TestCase("SA", "546306554")]
        [TestCase("SA", "602689660")]
        [TestCase("SA", "746453774")]
        [TestCase("SA", "657825665")]
        [TestCase("SA", "887921888")]
        [TestCase("SA", "999999999")]
        // Tests patterns that should be valid for Sudan (SD).
        [TestCase("SD", "00000")]
        [TestCase("SD", "03145")]
        [TestCase("SD", "14256")]
        [TestCase("SD", "20074")]
        [TestCase("SD", "34238")]
        [TestCase("SD", "44075")]
        [TestCase("SD", "54630")]
        [TestCase("SD", "60268")]
        [TestCase("SD", "74645")]
        [TestCase("SD", "65782")]
        [TestCase("SD", "88792")]
        [TestCase("SD", "99999")]
        // Tests patterns that should be valid for Sweden (SE).
        [TestCase("SE", "10000")]
        [TestCase("SE", "10070")]
        [TestCase("SE", "20767")]
        [TestCase("SE", "86560")]
        [TestCase("SE", "32451")]
        [TestCase("SE", "99112")]
        [TestCase("SE", "482 21")]
        [TestCase("SE", "546 36")]
        [TestCase("SE", "650 50")]
        [TestCase("SE", "701 62")]
        [TestCase("SE", "812 71")]
        [TestCase("SE", "926 86")]
        // Tests patterns that should be valid for Singapore (SG).
        [TestCase("SG", "11000")]
        [TestCase("SG", "34600")]
        [TestCase("SG", "61185")]
        [TestCase("SG", "41185")]
        [TestCase("SG", "00999")]
        [TestCase("SG", "01010")]
        [TestCase("SG", "71185")]
        [TestCase("SG", "81185")]
        [TestCase("SG", "91185")]
        // Tests patterns that should be valid for Saint Helena (SH).
        [TestCase("SH", "STHL1ZZ")]
        // Tests patterns that should be valid for Slovenia (SI).
        [TestCase("SI", "0123")]
        [TestCase("SI", "1234")]
        [TestCase("SI", "2000")]
        [TestCase("SI", "3248")]
        [TestCase("SI", "4945")]
        [TestCase("SI", "5640")]
        [TestCase("SI", "6208")]
        [TestCase("SI", "7645")]
        [TestCase("SI", "6752")]
        [TestCase("SI", "8782")]
        [TestCase("SI", "9999")]
        // Tests patterns that should be valid for Slovakia (SK).
        [TestCase("SK", "10070")]
        [TestCase("SK", "20767")]
        [TestCase("SK", "26560")]
        [TestCase("SK", "32451")]
        [TestCase("SK", "09112")]
        [TestCase("SK", "48221")]
        [TestCase("SK", "546 36")]
        [TestCase("SK", "650 50")]
        [TestCase("SK", "701 62")]
        [TestCase("SK", "812 71")]
        [TestCase("SK", "926 86")]
        // Tests patterns that should be valid for San Marino (SM).
        [TestCase("SM", "47890")]
        [TestCase("SM", "47891")]
        [TestCase("SM", "47892")]
        [TestCase("SM", "47895")]
        [TestCase("SM", "47899")]
        // Tests patterns that should be valid for Senegal (SN).
        [TestCase("SN", "00000")]
        [TestCase("SN", "01235")]
        [TestCase("SN", "12346")]
        [TestCase("SN", "20004")]
        [TestCase("SN", "32648")]
        [TestCase("SN", "40945")]
        [TestCase("SN", "56640")]
        [TestCase("SN", "62908")]
        [TestCase("SN", "76345")]
        [TestCase("SN", "67552")]
        [TestCase("SN", "87182")]
        [TestCase("SN", "99999")]
        // Tests patterns that should be valid for El Salvador (SV).
        [TestCase("SV", "01101")]
        // Tests patterns that should be valid for Swaziland (SZ).
        [TestCase("SZ", "H761")]
        [TestCase("SZ", "L000")]
        [TestCase("SZ", "M014")]
        [TestCase("SZ", "S628")]
        [TestCase("SZ", "H611")]
        [TestCase("SZ", "L760")]
        [TestCase("SZ", "M754")]
        [TestCase("SZ", "S998")]
        [TestCase("SZ", "H000")]
        [TestCase("SZ", "L023")]
        [TestCase("SZ", "M182")]
        [TestCase("SZ", "S282")]
        // Tests patterns that should be valid for Turks And Caicos Islands (TC).
        [TestCase("TC", "TKCA1ZZ")]
        // Tests patterns that should be valid for Chad (TD).
        [TestCase("TD", "00000")]
        [TestCase("TD", "01235")]
        [TestCase("TD", "12346")]
        [TestCase("TD", "20004")]
        [TestCase("TD", "32648")]
        [TestCase("TD", "40945")]
        [TestCase("TD", "56640")]
        [TestCase("TD", "62908")]
        [TestCase("TD", "76345")]
        [TestCase("TD", "67552")]
        [TestCase("TD", "87182")]
        [TestCase("TD", "99999")]
        // Tests patterns that should be valid for Thailand (TH).
        [TestCase("TH", "10023")]
        [TestCase("TH", "31001")]
        [TestCase("TH", "42007")]
        [TestCase("TH", "54006")]
        [TestCase("TH", "61008")]
        // Tests patterns that should be valid for Tajikistan (TJ).
        [TestCase("TJ", "000000")]
        [TestCase("TJ", "023145")]
        [TestCase("TJ", "134256")]
        [TestCase("TJ", "200074")]
        [TestCase("TJ", "364238")]
        [TestCase("TJ", "494075")]
        [TestCase("TJ", "564630")]
        [TestCase("TJ", "690268")]
        [TestCase("TJ", "734645")]
        [TestCase("TJ", "655782")]
        [TestCase("TJ", "818792")]
        [TestCase("TJ", "999999")]
        // Tests patterns that should be valid for Turkmenistan (TM).
        [TestCase("TM", "000000")]
        [TestCase("TM", "023145")]
        [TestCase("TM", "134256")]
        [TestCase("TM", "200074")]
        [TestCase("TM", "364238")]
        [TestCase("TM", "494075")]
        [TestCase("TM", "564630")]
        [TestCase("TM", "690268")]
        [TestCase("TM", "734645")]
        [TestCase("TM", "655782")]
        [TestCase("TM", "818792")]
        [TestCase("TM", "999999")]
        // Tests patterns that should be valid for Tunisia (TN).
        [TestCase("TN", "0123")]
        [TestCase("TN", "1234")]
        [TestCase("TN", "2000")]
        [TestCase("TN", "3248")]
        [TestCase("TN", "4945")]
        [TestCase("TN", "5640")]
        [TestCase("TN", "6208")]
        [TestCase("TN", "7645")]
        [TestCase("TN", "6752")]
        [TestCase("TN", "8782")]
        [TestCase("TN", "9999")]
        // Tests patterns that should be valid for Turkey (TR).
        [TestCase("TR", "01302")]
        [TestCase("TR", "08302")]
        [TestCase("TR", "10023")]
        [TestCase("TR", "31001")]
        [TestCase("TR", "42007")]
        [TestCase("TR", "74006")]
        [TestCase("TR", "91008")]
        // Tests patterns that should be valid for Trinidad And Tobago (TT).
        [TestCase("TT", "000000")]
        [TestCase("TT", "023145")]
        [TestCase("TT", "134256")]
        [TestCase("TT", "200074")]
        [TestCase("TT", "364238")]
        [TestCase("TT", "494075")]
        [TestCase("TT", "564630")]
        [TestCase("TT", "690268")]
        [TestCase("TT", "734645")]
        [TestCase("TT", "655782")]
        [TestCase("TT", "818792")]
        [TestCase("TT", "999999")]
        // Tests patterns that should be valid for Taiwan (TW).
        [TestCase("TW", "10023")]
        [TestCase("TW", "31001")]
        [TestCase("TW", "42007")]
        [TestCase("TW", "54006")]
        [TestCase("TW", "61008")]
        [TestCase("TW", "91008")]
        // Tests patterns that should be valid for Ukraine (UA).
        [TestCase("UA", "01235")]
        [TestCase("UA", "12346")]
        [TestCase("UA", "20004")]
        [TestCase("UA", "32648")]
        [TestCase("UA", "40945")]
        [TestCase("UA", "56640")]
        [TestCase("UA", "62908")]
        [TestCase("UA", "76345")]
        [TestCase("UA", "67552")]
        [TestCase("UA", "87182")]
        [TestCase("UA", "99999")]
        // Tests patterns that should be valid for United States (US).
        [TestCase("US", "01000-0060")]
        [TestCase("US", "11000-9996")]
        [TestCase("US", "00126")]
        [TestCase("US", "12345")]
        // Tests patterns that should be valid for Uruguay (UY).
        [TestCase("UY", "00000")]
        [TestCase("UY", "01235")]
        [TestCase("UY", "12346")]
        [TestCase("UY", "20004")]
        [TestCase("UY", "32648")]
        [TestCase("UY", "40945")]
        [TestCase("UY", "56640")]
        [TestCase("UY", "62908")]
        [TestCase("UY", "76345")]
        [TestCase("UY", "67552")]
        [TestCase("UY", "87182")]
        [TestCase("UY", "99999")]
        // Tests patterns that should be valid for Holy See (VA).
        [TestCase("VA", "00120")]
        // Tests patterns that should be valid for Saint Vincent And The Grenadines (VC).
        [TestCase("VC", "0123")]
        [TestCase("VC", "1234")]
        [TestCase("VC", "2000")]
        [TestCase("VC", "3248")]
        [TestCase("VC", "4945")]
        [TestCase("VC", "5640")]
        [TestCase("VC", "6208")]
        [TestCase("VC", "7645")]
        [TestCase("VC", "6752")]
        [TestCase("VC", "8782")]
        [TestCase("VC", "9999")]
        // Tests patterns that should be valid for Venezuela (VE).
        [TestCase("VE", "0000")]
        [TestCase("VE", "0123")]
        [TestCase("VE", "1234")]
        [TestCase("VE", "2000")]
        [TestCase("VE", "3264")]
        [TestCase("VE", "4094")]
        [TestCase("VE", "5664")]
        [TestCase("VE", "6290")]
        [TestCase("VE", "7634")]
        [TestCase("VE", "6755")]
        [TestCase("VE", "8718")]
        [TestCase("VE", "9999")]
        [TestCase("VE", "0000A")]
        [TestCase("VE", "0325A")]
        [TestCase("VE", "1436B")]
        [TestCase("VE", "2044C")]
        [TestCase("VE", "3478D")]
        [TestCase("VE", "4475E")]
        [TestCase("VE", "5450F")]
        [TestCase("VE", "6088G")]
        [TestCase("VE", "7495H")]
        [TestCase("VE", "6502I")]
        [TestCase("VE", "8832J")]
        [TestCase("VE", "9999K")]
        [TestCase("VE", "0000L")]
        [TestCase("VE", "0325M")]
        [TestCase("VE", "1436N")]
        [TestCase("VE", "2044O")]
        [TestCase("VE", "3478P")]
        [TestCase("VE", "4475Q")]
        [TestCase("VE", "5450R")]
        [TestCase("VE", "6088S")]
        [TestCase("VE", "7495T")]
        [TestCase("VE", "6502U")]
        [TestCase("VE", "8832V")]
        [TestCase("VE", "9999W")]
        [TestCase("VE", "0000X")]
        [TestCase("VE", "0325Y")]
        [TestCase("VE", "1436Z")]
        [TestCase("VE", "2044Z")]
        // Tests patterns that should be valid for Virgin Islands (VG).
        [TestCase("VG", "1103")]
        [TestCase("VG", "1114")]
        [TestCase("VG", "1120")]
        [TestCase("VG", "1138")]
        [TestCase("VG", "1145")]
        [TestCase("VG", "1150")]
        [TestCase("VG", "1168")]
        [TestCase("VG", "1135")]
        [TestCase("VG", "1162")]
        [TestCase("VG", "VG1101")]
        [TestCase("VG", "VG1112")]
        [TestCase("VG", "VG1120")]
        [TestCase("VG", "VG1132")]
        [TestCase("VG", "VG1149")]
        [TestCase("VG", "VG1156")]
        [TestCase("VG", "VG1162")]
        [TestCase("VG", "VG1106")]
        [TestCase("VG", "VG1167")]
        // Tests patterns that should be valid for Virgin Islands (VI).
        [TestCase("VI", "00815")]
        [TestCase("VI", "00826")]
        [TestCase("VI", "00837")]
        [TestCase("VI", "00846")]
        [TestCase("VI", "00858")]
        [TestCase("VI", "008152346")]
        [TestCase("VI", "008260004")]
        [TestCase("VI", "008372648")]
        [TestCase("VI", "008460945")]
        [TestCase("VI", "008586640")]
        // Tests patterns that should be valid for Viet Nam (VN).
        [TestCase("VN", "000000")]
        [TestCase("VN", "023145")]
        [TestCase("VN", "134256")]
        [TestCase("VN", "200074")]
        [TestCase("VN", "364238")]
        [TestCase("VN", "494075")]
        [TestCase("VN", "564630")]
        [TestCase("VN", "690268")]
        [TestCase("VN", "734645")]
        [TestCase("VN", "655782")]
        [TestCase("VN", "818792")]
        [TestCase("VN", "999999")]
        // Tests patterns that should be valid for Wallis And Futuna (WF).
        [TestCase("WF", "98600")]
        [TestCase("WF", "98617")]
        [TestCase("WF", "98699")]
        // Tests patterns that should be valid for Mayotte (YT).
        [TestCase("YT", "97600")]
        [TestCase("YT", "97605")]
        [TestCase("YT", "97616")]
        [TestCase("YT", "97624")]
        [TestCase("YT", "97638")]
        [TestCase("YT", "97645")]
        [TestCase("YT", "97650")]
        [TestCase("YT", "97668")]
        [TestCase("YT", "97675")]
        [TestCase("YT", "97662")]
        [TestCase("YT", "97682")]
        [TestCase("YT", "97699")]
        // Tests patterns that should be valid for South Africa (ZA).
        [TestCase("ZA", "0001")]
        [TestCase("ZA", "0023")]
        [TestCase("ZA", "0100")]
        [TestCase("ZA", "1000")]
        [TestCase("ZA", "2077")]
        [TestCase("ZA", "2650")]
        [TestCase("ZA", "4241")]
        // Tests patterns that should be valid for Zambia (ZM).
        [TestCase("ZM", "00000")]
        [TestCase("ZM", "01235")]
        [TestCase("ZM", "12346")]
        [TestCase("ZM", "20004")]
        [TestCase("ZM", "32648")]
        [TestCase("ZM", "40945")]
        [TestCase("ZM", "56640")]
        [TestCase("ZM", "62908")]
        [TestCase("ZM", "76345")]
        [TestCase("ZM", "67552")]
        [TestCase("ZM", "87182")]
        [TestCase("ZM", "99999")]
        public void IsValid(Country country, string postalcode)
        {
            var isValid = PostalCode.IsValid(postalcode, country);
            Assert.IsTrue(isValid, "Postal code '{0}' should be valid for {1:f}.", postalcode, country);
        }

        #endregion

        private static void IsNotValid(string code, Country country)
        {
            Assert.IsFalse(PostalCode.IsValid(code, country), "Postal code '{0}' should be not valid for {1:f}.", code, country);
        }

        private static void IsNotValid(Country country, bool alfa, bool prefix, params int[] lengths)
        {
            // Length 1
            IsNotValid(country, "1", alfa, alfa, prefix, lengths);
            IsNotValid(country, "A", !alfa, alfa, prefix, lengths);

            // Length 2
            IsNotValid(country, "12", alfa, alfa, prefix, lengths);
            IsNotValid(country, "AB", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "A1", !alfa, alfa, prefix, lengths);

            // Length 3
            IsNotValid(country, "123", alfa, alfa, prefix, lengths);
            IsNotValid(country, "ABC", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "AB1", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "A12", !alfa, alfa, prefix, lengths);

            // Length 4
            IsNotValid(country, "1234", alfa, alfa, prefix, lengths);
            IsNotValid(country, "AB1C", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "AB12", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "A123", !alfa, alfa, prefix, lengths);

            // Length 5
            IsNotValid(country, "12345", alfa, alfa, prefix, lengths);
            IsNotValid(country, "AB12C", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "AB123", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "A1234", !alfa, alfa, prefix, lengths);

            // Length 6
            IsNotValid(country, "123456", alfa, alfa, prefix, lengths);
            IsNotValid(country, "AB123C", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "AB1234", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "A12345", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "A1B3C3", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "1234AB", !alfa, alfa, prefix, lengths);

            // Length 7
            IsNotValid(country, "1234567", alfa, alfa, prefix, lengths);
            IsNotValid(country, "AB1234C", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "AB12345", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "A123456", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "A1B3C3D", !alfa, alfa, prefix, lengths);

            // Length 8
            IsNotValid(country, "12345678", alfa, alfa, prefix, lengths);
            IsNotValid(country, "AB1234CD", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "AB123456", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "A1234567", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "A1B3C3D4", !alfa, alfa, prefix, lengths);

            // Length 9
            IsNotValid(country, "123456789", alfa, alfa, prefix, lengths);
            IsNotValid(country, "AB12345CD", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "AB1234567", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "A12345678", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "A1B3C3D4E", !alfa, alfa, prefix, lengths);

            // Length 10
            IsNotValid(country, "1234567890", alfa, alfa, prefix, lengths);
            IsNotValid(country, "AB123456CD", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "AB12345678", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "A123456789", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "A1B3C3D4EF", !alfa, alfa, prefix, lengths);

            // Length 11
            IsNotValid(country, "12345678901", alfa, alfa, prefix, lengths);
            IsNotValid(country, "AB1234567CD", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "AB123456789", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "A1234567890", !alfa, alfa, prefix, lengths);
            IsNotValid(country, "A1B3C3D4E5F", !alfa, alfa, prefix, lengths);
        }
        private static void IsNotValid(Country country, string code, bool test, bool alpha, bool prefix, int[] lengths)
        {
            var pcode = country.IsoAlpha2Code + code;

            var len = code.Length;
            var plen = pcode.Length;

            // General tests. These are test more than once (so be it).
            Assert.AreEqual(len > 1 && len <= 10, PostalCode.IsValid(code), "The postal code '{0}' should be {1}valid.", code, len > 1 && len <= 10 ? "" : "not ");
            Assert.AreEqual(plen <= 10, PostalCode.IsValid(country.IsoAlpha2Code + code), "The postal code '{1:2}{0}' should be {2}valid.", code, country, plen <= 10 ? "" : "not ");

            // Tests if the types differ (alphanumeric versus numeric) or if the lenghts differ.
            if (test || !lengths.Contains(len))
            {
                Assert.IsFalse(PostalCode.IsValid(code, country), "The postal code '{0}' should not be valid for {1:f (2)}.", code, country);
            }
            // Tests if the types differ (alphanumeric versus numeric), if the lenghts differ, or if prefix is not supported.
            // And if not testing for an alfa pattern, or an alfa pattern that does not match the concatenated code.
            if ((test || !prefix || !lengths.Contains(len)) && (!alpha || !lengths.Contains(plen)))
            {
                Assert.IsFalse(PostalCode.IsValid(country.IsoAlpha2Code + code, country), "The postal code '{1:2}{0}' should not be valid for {1:f (2)}.", code, country);
            }
        }
    }

    [Serializable]
    public class PostalCodeSerializeObject
    {
        public int Id { get; set; }
        public PostalCode Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
