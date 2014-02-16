using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qowaiv.UnitTests.Json;
using Qowaiv.UnitTests.TestTools;
using Qowaiv.UnitTests.TestTools.Formatting;
using Qowaiv.UnitTests.TestTools.Globalization;
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
    [TestClass]
    public partial class PostalCodeTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly PostalCode TestStruct = PostalCode.Parse("H0H0H0");

        #region postal code const tests

        /// <summary>PostalCode.Empty should be equal to the default of postal code.</summary>
        [TestMethod]
        public void Empty_None_EqualsDefault()
        {
            Assert.AreEqual(default(PostalCode), PostalCode.Empty);
        }

        #endregion

        #region postal code IsEmpty tests

        /// <summary>PostalCode.IsEmpty() should be true for the default of postal code.</summary>
        [TestMethod]
        public void IsEmpty_Default_IsTrue()
        {
            Assert.IsTrue(default(PostalCode).IsEmpty());
        }
        /// <summary>PostalCode.IsEmpty() should be false for PostalCode.Unknown.</summary>
        [TestMethod]
        public void IsEmpty_Unknown_IsFalse()
        {
            Assert.IsFalse(PostalCode.Unknown.IsEmpty());
        }
        /// <summary>PostalCode.IsEmpty() should be false for the TestStruct.</summary>
        [TestMethod]
        public void IsEmpty_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmpty());
        }

        /// <summary>PostalCode.IsUnknown() should be false for the default of postal code.</summary>
        [TestMethod]
        public void IsUnknown_Default_IsFalse()
        {
            Assert.IsFalse(default(PostalCode).IsUnknown());
        }
        /// <summary>PostalCode.IsUnknown() should be true for PostalCode.Unknown.</summary>
        [TestMethod]
        public void IsUnknown_Unknown_IsTrue()
        {
            Assert.IsTrue(PostalCode.Unknown.IsUnknown());
        }
        /// <summary>PostalCode.IsUnknown() should be false for the TestStruct.</summary>
        [TestMethod]
        public void IsUnknown_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsUnknown());
        }

        /// <summary>PostalCode.IsEmptyOrUnknown() should be true for the default of postal code.</summary>
        [TestMethod]
        public void IsEmptyOrUnknown_Default_IsFalse()
        {
            Assert.IsTrue(default(PostalCode).IsEmptyOrUnknown());
        }
        /// <summary>PostalCode.IsEmptyOrUnknown() should be true for PostalCode.Unknown.</summary>
        [TestMethod]
        public void IsEmptyOrUnknown_Unknown_IsTrue()
        {
            Assert.IsTrue(PostalCode.Unknown.IsEmptyOrUnknown());
        }
        /// <summary>PostalCode.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
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
            PostalCode val;

            string str = null;

            Assert.IsTrue(PostalCode.TryParse(str, out val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [TestMethod]
        public void TyrParse_StringEmpty_IsValid()
        {
            PostalCode val;

            string str = string.Empty;

            Assert.IsTrue(PostalCode.TryParse(str, out val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [TestMethod]
        public void TyrParse_StringValue_IsValid()
        {
            PostalCode val;

            string str = "H0H0H0";

            Assert.IsTrue(PostalCode.TryParse(str, out val), "Valid");
            Assert.AreEqual(str, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [TestMethod]
        public void TyrParse_StringValue_IsNotValid()
        {
            PostalCode val;

            string str = "1";

            Assert.IsFalse(PostalCode.TryParse(str, out val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        [TestMethod]
        public void Parse_Unknown_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var act = PostalCode.Parse("?");
                var exp = PostalCode.Unknown;
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
                    PostalCode.Parse("InvalidInput");
                },
                "Not a valid postal code");
            }
        }

        [TestMethod]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = TestStruct;
                var act = PostalCode.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [TestMethod]
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

        [TestMethod]
        public void Constructor_SerializationInfoIsNull_ThrowsArgumentNullException()
        {
            ExceptionAssert.ExpectArgumentNullException
            (() =>
            {
                SerializationTest.DeserializeUsingConstructor<PostalCode>(null, default(StreamingContext));
            },
            "info");
        }

        [TestMethod]
        public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
        {
            ExceptionAssert.ExpectException<SerializationException>
            (() =>
            {
                var info = new SerializationInfo(typeof(PostalCode), new System.Runtime.Serialization.FormatterConverter());
                SerializationTest.DeserializeUsingConstructor<PostalCode>(info, default(StreamingContext));
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
            var info = new SerializationInfo(typeof(PostalCode), new System.Runtime.Serialization.FormatterConverter());
            obj.GetObjectData(info, default(StreamingContext));

            Assert.AreEqual(TestStruct.ToString(), info.GetString("Value"));
        }

        [TestMethod]
        public void SerializeDeserialize_TestStruct_AreEqual()
        {
            var input = PostalCodeTest.TestStruct;
            var exp = PostalCodeTest.TestStruct;
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void DataContractSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = PostalCodeTest.TestStruct;
            var exp = PostalCodeTest.TestStruct;
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void XmlSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = PostalCodeTest.TestStruct;
            var exp = PostalCodeTest.TestStruct;
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void SerializeDeserialize_PostalCodeSerializeObject_AreEqual()
        {
            var input = new PostalCodeSerializeObject()
            {
                Id = 17,
                Obj = PostalCodeTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new PostalCodeSerializeObject()
            {
                Id = 17,
                Obj = PostalCodeTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [TestMethod]
        public void XmlSerializeDeserialize_PostalCodeSerializeObject_AreEqual()
        {
            var input = new PostalCodeSerializeObject()
            {
                Id = 17,
                Obj = PostalCodeTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new PostalCodeSerializeObject()
            {
                Id = 17,
                Obj = PostalCodeTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [TestMethod]
        public void DataContractSerializeDeserialize_PostalCodeSerializeObject_AreEqual()
        {
            var input = new PostalCodeSerializeObject()
            {
                Id = 17,
                Obj = PostalCodeTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new PostalCodeSerializeObject()
            {
                Id = 17,
                Obj = PostalCodeTest.TestStruct,
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
            var input = new PostalCodeSerializeObject()
            {
                Id = 17,
                Obj = PostalCodeTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new PostalCodeSerializeObject()
            {
                Id = 17,
                Obj = PostalCodeTest.TestStruct,
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
            var act = JsonTester.Read<PostalCode>();
            var exp = PostalCode.Empty;

            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void FromJson_InvalidStringValue_AssertFormatException()
        {
            ExceptionAssert.ExpectException<FormatException>(() =>
            {
                JsonTester.Read<PostalCode>("InvalidStringValue");
            },
            "Not a valid postal code");
        }
        [TestMethod]
        public void FromJson_StringValue_AreEqual()
        {
            var act = JsonTester.Read<PostalCode>(TestStruct.ToString(CultureInfo.InvariantCulture));
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void FromJson_Int64Value_AssertNotSupportedException()
        {
            ExceptionAssert.ExpectException<NotSupportedException>(() =>
            {
                JsonTester.Read<PostalCode>(123456L);
            },
            "JSON deserialization from an integer is not supported.");
        }

        [TestMethod]
        public void FromJson_DoubleValue_AssertNotSupportedException()
        {
            ExceptionAssert.ExpectException<NotSupportedException>(() =>
            {
                JsonTester.Read<PostalCode>(1234.56);
            },
            "JSON deserialization from a number is not supported.");
        }

        [TestMethod]
        public void FromJson_DateTimeValue_AssertNotSupportedException()
        {
            ExceptionAssert.ExpectException<NotSupportedException>(() =>
            {
                JsonTester.Read<PostalCode>(new DateTime(1972, 02, 14));
            },
            "JSON deserialization from a date is not supported.");
        }

        [TestMethod]
        public void ToJson_DefaultValue_AreEqual()
        {
            object act = JsonTester.Write(default(PostalCode));
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
            var act = PostalCode.Empty.ToString();
            var exp = "";
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: 'H0H0H0', format: 'Unit Test Format'";

            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void ToString_TestStructCA_ComplexPattern()
        {
            var act = TestStruct.ToString("CA");
            var exp = "H0H 0H0";
            Assert.AreEqual(exp, act);
        }

        /// <remarks>No valid for the Netherlands.</summary>
        [TestMethod]
        public void ToString_TestStructNL_ComplexPattern()
        {
            var act = TestStruct.ToString(Country.NL);
            var exp = "H0H0H0";
            Assert.AreEqual(exp, act);
        }

        /// <remarks>No postal code in Somalia.</summary>
        [TestMethod]
        public void ToString_TestStructSO_ComplexPattern()
        {
            var act = TestStruct.ToString(Country.SO);
            var exp = "H0H0H0";
            Assert.AreEqual(exp, act);
        }

        /// <remarks>No formatting in Albania.</summary>
        [TestMethod]
        public void ToString_TestStructAL_ComplexPattern()
        {
            var act = TestStruct.ToString(Country.AL);
            var exp = "H0H0H0";
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void ToString_AD765AD_ComplexPattern()
        {
            var postalcode = PostalCode.Parse("AD765");
            var act = postalcode.ToString("AD");
            var exp = "AD-765";
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void ToString_765AD_ComplexPattern()
        {
            var postalcode = PostalCode.Parse("765");
            var act = postalcode.ToString("AD");
            var exp = "AD-765";
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void DebuggerDisplay_DebugToString_HasAttribute()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(PostalCode));
        }

        [TestMethod]
        public void DebugToString_DefaultValue_String()
        {
            DebuggerDisplayAssert.HasResult("PostalCode: (empty)", default(PostalCode));
        }

        [TestMethod]
        public void DebugToString_TestStruct_String()
        {
            DebuggerDisplayAssert.HasResult("PostalCode: H0H0H0", TestStruct);
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for PostalCode.Empty.</summary>
        [TestMethod]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, PostalCode.Empty.GetHashCode());
        }

        /// <summary>GetHash should not fail for the test struct.</summary>
        [TestMethod]
        public void GetHash_TestStruct_Hash()
        {
            Assert.AreEqual(-896925736, PostalCodeTest.TestStruct.GetHashCode());
        }

        [TestMethod]
        public void Equals_EmptyEmpty_IsTrue()
        {
            Assert.IsTrue(PostalCode.Empty.Equals(PostalCode.Empty));
        }

        [TestMethod]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = PostalCode.Parse("H0 H0 H0-", CultureInfo.InvariantCulture);
            var r = PostalCode.Parse("h0h0h0", CultureInfo.InvariantCulture);

            Assert.IsTrue(l.Equals(r));
        }

        [TestMethod]
        public void Equals_TestStructTestStruct_IsTrue()
        {
            Assert.IsTrue(PostalCodeTest.TestStruct.Equals(PostalCodeTest.TestStruct));
        }

        [TestMethod]
        public void Equals_TestStructEmpty_IsFalse()
        {
            Assert.IsFalse(PostalCodeTest.TestStruct.Equals(PostalCode.Empty));
        }

        [TestMethod]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(PostalCode.Empty.Equals(PostalCodeTest.TestStruct));
        }

        [TestMethod]
        public void Equals_TestStructObjectTestStruct_IsTrue()
        {
            Assert.IsTrue(PostalCodeTest.TestStruct.Equals((object)PostalCodeTest.TestStruct));
        }

        [TestMethod]
        public void Equals_TestStructNull_IsFalse()
        {
            Assert.IsFalse(PostalCodeTest.TestStruct.Equals(null));
        }

        [TestMethod]
        public void Equals_TestStructObject_IsFalse()
        {
            Assert.IsFalse(PostalCodeTest.TestStruct.Equals(new object()));
        }

        [TestMethod]
        public void OperatorIs_TestStructTestStruct_IsTrue()
        {
            var l = PostalCodeTest.TestStruct;
            var r = PostalCodeTest.TestStruct;
            Assert.IsTrue(l == r);
        }

        [TestMethod]
        public void OperatorIsNot_TestStructTestStruct_IsFalse()
        {
            var l = PostalCodeTest.TestStruct;
            var r = PostalCodeTest.TestStruct;
            Assert.IsFalse(l != r);
        }

        #endregion

        #region IComparable tests

        /// <summary>Orders a list of postal codes ascending.</summary>
        [TestMethod]
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
        [TestMethod]
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
                "Argument must be a postal code"
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
                "Argument must be a postal code"
            );
        }
        #endregion

        #region Casting tests

        [TestMethod]
        public void Explicit_StringToPostalCode_AreEqual()
        {
            var exp = TestStruct;
            var act = (PostalCode)TestStruct.ToString();

            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void Explicit_PostalCodeToString_AreEqual()
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
            var act = PostalCode.Empty.Length;
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void Length_TestStruct_IntValue()
        {
            var exp = 6;
            var act = TestStruct.Length;
            Assert.AreEqual(exp, act);
        }
        #endregion

        #region Type converter tests

        [TestMethod]
        public void ConverterExists_PostalCode_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(PostalCode));
        }

        [TestMethod]
        public void CanNotConvertFromInt32_PostalCode_IsTrue()
        {
            TypeConverterAssert.CanNotConvertFrom(typeof(PostalCode), typeof(Int32));
        }
        [TestMethod]
        public void CanNotConvertToInt32_PostalCode_IsTrue()
        {
            TypeConverterAssert.CanNotConvertTo(typeof(PostalCode), typeof(Int32));
        }

        [TestMethod]
        public void CanConvertFromString_PostalCode_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(PostalCode));
        }

        [TestMethod]
        public void CanConvertToString_PostalCode_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(PostalCode));
        }

        [TestMethod]
        public void ConvertFrom_StringNull_PostalCodeEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(PostalCode.Empty, (string)null);
            }
        }

        [TestMethod]
        public void ConvertFrom_StringEmpty_PostalCodeEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(PostalCode.Empty, string.Empty);
            }
        }

        [TestMethod]
        public void ConvertFromString_StringValue_TestStruct()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(PostalCodeTest.TestStruct, PostalCodeTest.TestStruct.ToString());
            }
        }

        [TestMethod]
        public void ConvertToString_TestStruct_StringValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertToStringEquals(PostalCodeTest.TestStruct.ToString(), PostalCodeTest.TestStruct);
            }
        }

        #endregion

        #region IsValid tests

        [TestMethod]
        public void IsValid_Data_IsFalse()
        {
            Assert.IsFalse(PostalCode.IsValid("1"), "1");
            Assert.IsFalse(PostalCode.IsValid("12345678901"), "12345678901");
            Assert.IsFalse(PostalCode.IsValid((String)null), "(String)null");
            Assert.IsFalse(PostalCode.IsValid(String.Empty), "String.Empty");
        }
        
        [TestMethod]
        public void IsValid_Data_IsTrue()
        {
            Assert.IsTrue(PostalCode.IsValid("1234AB"));
        }

        [TestMethod]
        public void IsValid_EmptyCA_IsFalse()
        {
            Assert.IsFalse(PostalCode.Empty.IsValid(Country.CA));
        }
        [TestMethod]
        public void IsValid_UnknownCA_IsFalse()
        {
            Assert.IsFalse(PostalCode.Unknown.IsValid(Country.CA));
        }

        [TestMethod]
        public void IsValid_TestStructCA_IsTrue()
        {
            Assert.IsTrue(TestStruct.IsValid(Country.CA));
        }
        [TestMethod]
        public void IsValid_TestStructBE_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsValid(Country.BE));
        }
        [TestMethod]
        public void IsValidFor_TestStruct_1Country()
        {
            var act = TestStruct.IsValidFor().ToArray();
            var exp = new Country[] { Country.CA };
            CollectionAssert.AllItemsAreUnique(act);

            CollectionAssert.AreEqual(exp, act);
        }

        [TestMethod]
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

        /// <summary>Tests patterns that should be valid for Andorra (AD).</summary>
        [TestMethod]
        public void IsValid_AD_All()
        {
            var country = Country.AD;

            IsValid("AD123", country);
            IsValid("AD234", country);
            IsValid("AD345", country);
            IsValid("AD678", country);
            IsValid("AD789", country);
        }

        /// <summary>Tests patterns that should be valid for Afghanistan (AF).</summary>
        [TestMethod]
        public void IsValid_AF_All()
        {
            var country = Country.AF;

            IsValid("4301", country);
            IsValid("1001", country);
            IsValid("2023", country);
            IsValid("1102", country);
            IsValid("4020", country);
            IsValid("3077", country);
            IsValid("2650", country);
            IsValid("4241", country);
        }

        /// <summary>Tests patterns that should be valid for Anguilla (AI).</summary>
        [TestMethod]
        public void IsValid_AI_All()
        {
            var country = Country.AI;

            IsValid("2640", country);
            IsValid("AI-2640", country);
            IsValid("AI2640", country);
            IsValid("ai-2640", country);
            IsValid("ai2640", country);
            IsValid("ai 2640", country);
            IsValid("ai.2640", country);
        }

        /// <summary>Tests patterns that should be valid for Albania (AL).</summary>
        [TestMethod]
        public void IsValid_AL_All()
        {
            var country = Country.AL;

            IsValid("1872", country);
            IsValid("2540", country);
            IsValid("7900", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for Armenia (AM).</summary>
        [TestMethod]
        public void IsValid_AM_All()
        {
            var country = Country.AM;

            IsValid("0123", country);
            IsValid("1234", country);
            IsValid("2000", country);
            IsValid("3248", country);
            IsValid("4945", country);
        }

        /// <summary>Tests patterns that should be valid for Argentina (AR).</summary>
        [TestMethod]
        public void IsValid_AR_All()
        {
            var country = Country.AR;

            IsValid("A4400XXX", country);
            IsValid("C 1420 ABC", country);
            IsValid("S 2300DDD", country);
            IsValid("Z9400 QOW", country);
        }

        /// <summary>Tests patterns that should be valid for American Samoa (AS).</summary>
        [TestMethod]
        public void IsValid_AS_All()
        {
            var country = Country.AS;

            IsValid("91000-0060", country);
            IsValid("91000-9996", country);
            IsValid("90126", country);
            IsValid("92345", country);
        }

        /// <summary>Tests patterns that should be valid for Austria (AT).</summary>
        [TestMethod]
        public void IsValid_AT_All()
        {
            var country = Country.AT;

            IsValid("2471", country);
            IsValid("1000", country);
            IsValid("5120", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for Australia (AU).</summary>
        [TestMethod]
        public void IsValid_AU_All()
        {
            var country = Country.AU;

            IsValid("0872", country);
            IsValid("2540", country);
            IsValid("0900", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for Åland Islands (AX).</summary>
        [TestMethod]
        public void IsValid_AX_All()
        {
            var country = Country.AX;

            IsValid("22-000", country);
            IsValid("22-123", country);
            IsValid("22000", country);
            IsValid("22345", country);
        }

        /// <summary>Tests patterns that should be valid for Azerbaijan (AZ).</summary>
        [TestMethod]
        public void IsValid_AZ_All()
        {
            var country = Country.AZ;

            IsValid("1499", country);
            IsValid("az 1499", country);
            IsValid("AZ-1499", country);
            IsValid("az1499", country);
            IsValid("AZ0499", country);
            IsValid("AZ0099", country);
            IsValid("aZ6990", country);
        }

        /// <summary>Tests patterns that should be valid for Bosnia And Herzegovina (BA).</summary>
        [TestMethod]
        public void IsValid_BA_All()
        {
            var country = Country.BA;

            IsValid("00000", country);
            IsValid("01235", country);
            IsValid("12346", country);
            IsValid("20004", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
            IsValid("76345", country);
            IsValid("67552", country);
            IsValid("87182", country);
            IsValid("99999", country);
        }

        /// <summary>Tests patterns that should be valid for Barbados (BB).</summary>
        [TestMethod]
        public void IsValid_BB_All()
        {
            var country = Country.BB;

            IsValid("21499", country);
            IsValid("01499", country);
            IsValid("bB-31499", country);
            IsValid("BB 01499", country);
            IsValid("bb81499", country);
            IsValid("BB71499", country);
            IsValid("BB56990", country);

        }

        /// <summary>Tests patterns that should be valid for Bangladesh (BD).</summary>
        [TestMethod]
        public void IsValid_BD_All()
        {
            var country = Country.BD;

            IsValid("0483", country);
            IsValid("1480", country);
            IsValid("5492", country);
            IsValid("7695", country);
            IsValid("9796", country);
        }

        /// <summary>Tests patterns that should be valid for Belgium (BE).</summary>
        [TestMethod]
        public void IsValid_BE_All()
        {
            var country = Country.BE;

            IsValid("2471", country);
            IsValid("1000", country);
            IsValid("5120", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for Bulgaria (BG).</summary>
        [TestMethod]
        public void IsValid_BG_All()
        {
            var country = Country.BG;

            IsValid("1000", country);
            IsValid("2077", country);
            IsValid("2650", country);
            IsValid("4241", country);
        }

        /// <summary>Tests patterns that should be valid for Bahrain (BH).</summary>
        [TestMethod]
        public void IsValid_BH_All()
        {
            var country = Country.BH;

            IsValid("199", country);
            IsValid("1299", country);
            IsValid("666", country);
            IsValid("890", country);
            IsValid("768", country);
            IsValid("1000", country);
            IsValid("1176", country);
        }

        /// <summary>Tests patterns that should be valid for Saint Barthélemy (BL).</summary>
        [TestMethod]
        public void IsValid_BL_All()
        {
            var country = Country.BL;

            IsValid("97700", country);
            IsValid("97701", country);
            IsValid("97712", country);
            IsValid("97720", country);
            IsValid("97732", country);
            IsValid("97740", country);
            IsValid("97756", country);
            IsValid("97762", country);
            IsValid("97776", country);
            IsValid("97767", country);
            IsValid("97787", country);
            IsValid("97799", country);
        }

        /// <summary>Tests patterns that should be valid for Bermuda (BM).</summary>
        [TestMethod]
        public void IsValid_BM_All()
        {
            var country = Country.BM;

            IsValid("AA", country);
            IsValid("AS", country);
            IsValid("BJ", country);
            IsValid("CD", country);
            IsValid("DE", country);
            IsValid("EO", country);
            IsValid("FN", country);
            IsValid("GF", country);
            IsValid("HL", country);
            IsValid("ID", country);
            IsValid("JS", country);
            IsValid("KN", country);
            IsValid("LO", country);
            IsValid("ME", country);
            IsValid("NN", country);
            IsValid("OL", country);
            IsValid("PS", country);
            IsValid("QD", country);
            IsValid("RN", country);
            IsValid("SE", country);
            IsValid("TM", country);
            IsValid("UF", country);
            IsValid("VE", country);
            IsValid("WL", country);
            IsValid("XM", country);
            IsValid("YE", country);
            IsValid("ZL", country);
            IsValid("ZZ", country);

            IsValid("AA0F", country);
            IsValid("AS0S", country);
            IsValid("BJ1F", country);
            IsValid("CD2K", country);
            IsValid("DE3D", country);
            IsValid("EO4J", country);
            IsValid("FN5F", country);
            IsValid("GF6S", country);
            IsValid("HL7D", country);
            IsValid("ID69", country);
            IsValid("JS66", country);
            IsValid("KN48", country);
            IsValid("LO12", country);
            IsValid("MEDS", country);
            IsValid("NNRF", country);
            IsValid("OLWK", country);
            IsValid("PSSD", country);
            IsValid("QDPJ", country);
            IsValid("RNKF", country);
            IsValid("SELS", country);
            IsValid("TMD1", country);
            IsValid("UFO7", country);
            IsValid("VEF2", country);
            IsValid("WLS9", country);
            IsValid("XMF0", country);
            IsValid("YES4", country);
            IsValid("ZLF2", country);
            IsValid("ZZK7", country);
        }

        /// <summary>Tests patterns that should be valid for Brunei Darussalam (BN).</summary>
        [TestMethod]
        public void IsValid_BN_All()
        {
            var country = Country.BN;

            IsValid("YZ0000", country);
            IsValid("BU2529", country);
            IsValid("bU2529", country);
            IsValid("bu2529", country);
            IsValid("Bu2529", country);
        }

        /// <summary>Tests patterns that should be valid for Bolivia (BO).</summary>
        [TestMethod]
        public void IsValid_BO_All()
        {
            var country = Country.BO;

            IsValid("0123", country);
            IsValid("1234", country);
            IsValid("2000", country);
            IsValid("3248", country);
            IsValid("4945", country);
            IsValid("5640", country);
            IsValid("6208", country);
            IsValid("7645", country);
            IsValid("6752", country);
            IsValid("8782", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for Brazil (BR).</summary>
        [TestMethod]
        public void IsValid_BR_All()
        {
            var country = Country.BR;

            IsValid("01000-000", country);
            IsValid("01000999", country);
            IsValid("88000-123", country);
        }

        /// <summary>Tests patterns that should be valid for Bhutan (BT).</summary>
        [TestMethod]
        public void IsValid_BT_All()
        {
            var country = Country.BT;

            IsValid("000", country);
            IsValid("012", country);
            IsValid("123", country);
            IsValid("200", country);
            IsValid("326", country);
            IsValid("409", country);
            IsValid("566", country);
            IsValid("629", country);
            IsValid("763", country);
            IsValid("675", country);
            IsValid("871", country);
            IsValid("999", country);

        }

        /// <summary>Tests patterns that should be valid for Belarus (BY).</summary>
        [TestMethod]
        public void IsValid_BY_All()
        {
            var country = Country.BY;

            IsValid("010185", country);
            IsValid("110000", country);
            IsValid("342600", country);
            IsValid("610185", country);
            IsValid("910185", country);
        }

        /// <summary>Tests patterns that should be valid for Canada (CA).</summary>
        [TestMethod]
        public void IsValid_CA_All()
        {
            var country = Country.CA;

            IsValid("H0H-0H0", country);
            IsValid("K8 N5W 6", country);
            IsValid("A1A 1A1", country);
            IsValid("K0H 9Z0", country);
            IsValid("T1R 9Z0", country);
            IsValid("P2V9z0", country);
        }

        /// <summary>Tests patterns that should be valid for Cocos (CC).</summary>
        [TestMethod]
        public void IsValid_CC_All()
        {
            var country = Country.CC;

            IsValid("0123", country);
            IsValid("1234", country);
            IsValid("2000", country);
            IsValid("3248", country);
            IsValid("4945", country);
            IsValid("5640", country);
            IsValid("6208", country);
            IsValid("7645", country);
            IsValid("6752", country);
            IsValid("8782", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for Switzerland (CH).</summary>
        [TestMethod]
        public void IsValid_CH_All()
        {
            var country = Country.CH;

            IsValid("1001", country);
            IsValid("8023", country);
            IsValid("9100", country);
            IsValid("1000", country);
            IsValid("2077", country);
            IsValid("2650", country);
            IsValid("4241", country);
        }

        /// <summary>Tests patterns that should be valid for Chile (CL).</summary>
        [TestMethod]
        public void IsValid_CL_All()
        {
            var country = Country.CL;

            IsValid("0000000", country);
            IsValid("0231145", country);
            IsValid("1342456", country);
            IsValid("2000974", country);
            IsValid("3642438", country);
            IsValid("4940375", country);
            IsValid("5646230", country);
            IsValid("6902168", country);
            IsValid("7346345", country);
            IsValid("6557682", country);
            IsValid("8187992", country);
            IsValid("9999999", country);
        }

        /// <summary>Tests patterns that should be valid for China (CN).</summary>
        [TestMethod]
        public void IsValid_CN_All()
        {
            var country = Country.CN;

            IsValid("010000", country);
            IsValid("342600", country);
            IsValid("810185", country);
        }

        /// <summary>Tests patterns that should be valid for Colombia (CO).</summary>
        [TestMethod]
        public void IsValid_CO_All()
        {
            var country = Country.CO;

            IsValid("000000", country);
            IsValid("023145", country);
            IsValid("134256", country);
            IsValid("200074", country);
            IsValid("364238", country);
            IsValid("494075", country);
            IsValid("564630", country);
            IsValid("690268", country);
            IsValid("734645", country);
            IsValid("655782", country);
            IsValid("818792", country);
            IsValid("999999", country);

        }

        /// <summary>Tests patterns that should be valid for Costa Rica (CR).</summary>
        [TestMethod]
        public void IsValid_CR_All()
        {
            var country = Country.CR;

            IsValid("00000", country);
            IsValid("01235", country);
            IsValid("12346", country);
            IsValid("20004", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
            IsValid("76345", country);
            IsValid("67552", country);
            IsValid("87182", country);
            IsValid("99999", country);
        }

        /// <summary>Tests patterns that should be valid for Cuba (CU).</summary>
        [TestMethod]
        public void IsValid_CU_All()
        {
            var country = Country.CU;

            IsValid("00000", country);
            IsValid("01235", country);
            IsValid("12346", country);
            IsValid("20004", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
            IsValid("76345", country);
            IsValid("67552", country);
            IsValid("87182", country);
            IsValid("99999", country);
            IsValid("CP00000", country);
            IsValid("CP01235", country);
            IsValid("CP12346", country);
            IsValid("CP20004", country);
            IsValid("CP32648", country);
        }

        /// <summary>Tests patterns that should be valid for Cape Verde (CV).</summary>
        [TestMethod]
        public void IsValid_CV_All()
        {
            var country = Country.CV;

            IsValid("0000", country);
            IsValid("1000", country);
            IsValid("2077", country);
            IsValid("2650", country);
            IsValid("4241", country);
        }

        /// <summary>Tests patterns that should be valid for Christmas Island (CX).</summary>
        [TestMethod]
        public void IsValid_CX_All()
        {
            var country = Country.CX;

            IsValid("0000", country);
            IsValid("0144", country);
            IsValid("1282", country);
            IsValid("2037", country);
            IsValid("3243", country);
            IsValid("4008", country);
            IsValid("5697", country);
            IsValid("6282", country);
            IsValid("7611", country);
            IsValid("6767", country);
            IsValid("8752", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for Cyprus (CY).</summary>
        [TestMethod]
        public void IsValid_CY_All()
        {
            var country = Country.CY;

            IsValid("1000", country);
            IsValid("2077", country);
            IsValid("2650", country);
            IsValid("4241", country);
        }

        /// <summary>Tests patterns that should be valid for Czech Republic (CZ).</summary>
        [TestMethod]
        public void IsValid_CZ_All()
        {
            var country = Country.CZ;

            IsValid("21234", country);
            IsValid("12345", country);
            IsValid("11111", country);
            IsValid("123 45", country);
        }

        /// <summary>Tests patterns that should be valid for Germany (DE).</summary>
        [TestMethod]
        public void IsValid_DE_All()
        {
            var country = Country.DE;

            IsValid("10000", country);
            IsValid("01123", country);
            IsValid("89000", country);
            IsValid("12345", country);
        }

        /// <summary>Tests patterns that should be valid for Denmark (DK).</summary>
        [TestMethod]
        public void IsValid_DK_All()
        {
            var country = Country.DK;

            IsValid("1499", country);
            IsValid("dk-1499", country);
            IsValid("DK-1499", country);
            IsValid("dk1499", country);
            IsValid("DK1499", country);
            IsValid("DK6990", country);
        }

        /// <summary>Tests patterns that should be valid for Algeria (DZ).</summary>
        [TestMethod]
        public void IsValid_DZ_All()
        {
            var country = Country.DZ;

            IsValid("01234", country);
            IsValid("12345", country);
            IsValid("11111", country);
        }

        /// <summary>Tests patterns that should be valid for Ecuador (EC).</summary>
        [TestMethod]
        public void IsValid_EC_All()
        {
            var country = Country.EC;

            IsValid("000000", country);
            IsValid("023145", country);
            IsValid("134256", country);
            IsValid("200074", country);
            IsValid("364238", country);
            IsValid("494075", country);
            IsValid("564630", country);
            IsValid("690268", country);
            IsValid("734645", country);
            IsValid("655782", country);
            IsValid("818792", country);
            IsValid("999999", country);

        }

        /// <summary>Tests patterns that should be valid for Estonia (EE).</summary>
        [TestMethod]
        public void IsValid_EE_All()
        {
            var country = Country.EE;

            IsValid("00000", country);
            IsValid("01235", country);
            IsValid("12346", country);
            IsValid("20004", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
            IsValid("76345", country);
            IsValid("67552", country);
            IsValid("87182", country);
            IsValid("99999", country);
        }

        /// <summary>Tests patterns that should be valid for Egypt (EG).</summary>
        [TestMethod]
        public void IsValid_EG_All()
        {
            var country = Country.EG;

            IsValid("12346", country);
            IsValid("20004", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
            IsValid("76345", country);
            IsValid("67552", country);
            IsValid("87182", country);
            IsValid("99999", country);
        }

        /// <summary>Tests patterns that should be valid for Spain (ES).</summary>
        [TestMethod]
        public void IsValid_ES_All()
        {
            var country = Country.ES;

            IsValid("01070", country);
            IsValid("10070", country);
            IsValid("20767", country);
            IsValid("26560", country);
            IsValid("32451", country);
            IsValid("09112", country);
            IsValid("48221", country);
            IsValid("50636", country);
            IsValid("52636", country);
            IsValid("51050", country);
        }

        /// <summary>Tests patterns that should be valid for Ethiopia (ET).</summary>
        [TestMethod]
        public void IsValid_ET_All()
        {
            var country = Country.ET;

            IsValid("0123", country);
            IsValid("1234", country);
            IsValid("2000", country);
            IsValid("3248", country);
            IsValid("4945", country);
            IsValid("5640", country);
            IsValid("6208", country);
            IsValid("7645", country);
            IsValid("6752", country);
            IsValid("8782", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for Finland (FI).</summary>
        [TestMethod]
        public void IsValid_FI_All()
        {
            var country = Country.FI;

            IsValid("00-000", country);
            IsValid("01-123", country);
            IsValid("00000", country);
            IsValid("12345", country);
        }

        /// <summary>Tests patterns that should be valid for Falkland Islands (FK).</summary>
        [TestMethod]
        public void IsValid_FK_All()
        {
            var country = Country.FK;

            IsValid("FIQQ1ZZ", country);
        }

        /// <summary>Tests patterns that should be valid for Micronesia (FM).</summary>
        [TestMethod]
        public void IsValid_FM_All()
        {
            var country = Country.FM;

            IsValid("96941", country);
            IsValid("96942", country);
            IsValid("96943", country);
            IsValid("96944", country);

            IsValid("969410000", country);
            IsValid("969420123", country);
            IsValid("969430144", country);
            IsValid("969441282", country);
        }

        /// <summary>Tests patterns that should be valid for Faroe Islands (FO).</summary>
        [TestMethod]
        public void IsValid_FO_All()
        {
            var country = Country.FO;

            IsValid("399", country);
            IsValid("fo-399", country);
            IsValid("FO-199", country);
            IsValid("fO399", country);
            IsValid("FO678", country);
            IsValid("FO123", country);
        }

        /// <summary>Tests patterns that should be valid for France (FR).</summary>
        [TestMethod]
        public void IsValid_FR_All()
        {
            var country = Country.FR;

            IsValid("10000", country);
            IsValid("01123", country);
            IsValid("89000", country);
            IsValid("12345", country);
        }

        /// <summary>Tests patterns that should be valid for Gabon (GA).</summary>
        [TestMethod]
        public void IsValid_GA_All()
        {
            var country = Country.GA;

            IsValid("0123", country);
            IsValid("1234", country);
            IsValid("2000", country);
            IsValid("3248", country);
            IsValid("4945", country);
            IsValid("5640", country);
            IsValid("6208", country);
            IsValid("7645", country);
            IsValid("6752", country);
            IsValid("8782", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for United Kingdom (GB).</summary>
        [TestMethod]
        public void IsValid_GB_All()
        {
            var country = Country.GB;

            IsValid("M11AA", country);
            IsValid("M11aA", country);
            IsValid("M11AA", country);
            IsValid("m11AA", country);
            IsValid("m11aa", country);

            IsValid("B338TH", country);
            IsValid("B338TH", country);

            IsValid("CR26XH", country);
            IsValid("CR26XH", country);

            IsValid("DN551PT", country);
            IsValid("DN551PT", country);

            IsValid("W1A1HQ", country);
            IsValid("W1A1HQ", country);

            IsValid("EC1A1BB", country);
            IsValid("EC1A1BB", country);
        }

        /// <summary>Tests patterns that should be valid for Georgia (GE).</summary>
        [TestMethod]
        public void IsValid_GE_All()
        {
            var country = Country.GE;

            IsValid("0123", country);
            IsValid("1234", country);
            IsValid("2000", country);
            IsValid("3248", country);
            IsValid("4945", country);
            IsValid("5640", country);
            IsValid("6208", country);
            IsValid("7645", country);
            IsValid("6752", country);
            IsValid("8782", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for French Guiana (GF).</summary>
        [TestMethod]
        public void IsValid_GF_All()
        {
            var country = Country.GF;

            IsValid("97300", country);
            IsValid("97301", country);
            IsValid("97312", country);
            IsValid("97320", country);
            IsValid("97332", country);
            IsValid("97340", country);
            IsValid("97356", country);
            IsValid("97362", country);
            IsValid("97376", country);
            IsValid("97367", country);
            IsValid("97387", country);
            IsValid("97399", country);
        }

        /// <summary>Tests patterns that should be valid for Guernsey (GG).</summary>
        [TestMethod]
        public void IsValid_GG_All()
        {
            var country = Country.GG;

            IsValid("00DF", country);
            IsValid("03DS", country);
            IsValid("14RF", country);
            IsValid("20WK", country);
            IsValid("34SD", country);
            IsValid("44PJ", country);
            IsValid("54KF", country);
            IsValid("60LS", country);
            IsValid("74JD", country);
            IsValid("65MO", country);
            IsValid("88DF", country);
            IsValid("99JS", country);

            IsValid("000DF", country);
            IsValid("015DS", country);
            IsValid("126RF", country);
            IsValid("204WK", country);
            IsValid("328SD", country);
            IsValid("405PJ", country);
            IsValid("560KF", country);
            IsValid("628LS", country);
            IsValid("765JD", country);
            IsValid("672MO", country);
            IsValid("872DF", country);
            IsValid("999JS", country);

        }

        /// <summary>Tests patterns that should be valid for Gibraltar (GI).</summary>
        [TestMethod]
        public void IsValid_GI_All()
        {
            var country = Country.GI;

            IsValid("GX111AA", country);
        }

        /// <summary>Tests patterns that should be valid for Greenland (GL).</summary>
        [TestMethod]
        public void IsValid_GL_All()
        {
            var country = Country.GL;

            IsValid("3999", country);
            IsValid("gl-3999", country);
            IsValid("GL-3999", country);
            IsValid("gL 3999", country);
            IsValid("GL3999", country);
            IsValid("GL3990", country);
        }

        /// <summary>Tests patterns that should be valid for Guadeloupe (GP).</summary>
        [TestMethod]
        public void IsValid_GP_All()
        {
            var country = Country.GP;

            IsValid("97100", country);
            IsValid("97101", country);
            IsValid("97112", country);
            IsValid("97120", country);
            IsValid("97132", country);
            IsValid("97140", country);
            IsValid("97156", country);
            IsValid("97162", country);
            IsValid("97176", country);
            IsValid("97167", country);
            IsValid("97187", country);
            IsValid("97199", country);
        }

        /// <summary>Tests patterns that should be valid for Greece (GR).</summary>
        [TestMethod]
        public void IsValid_GR_All()
        {
            var country = Country.GR;

            IsValid("10000", country);
            IsValid("31123", country);
            IsValid("89000", country);
            IsValid("12345", country);
        }

        /// <summary>Tests patterns that should be valid for South Georgia And The South Sandwich Islands (GS).</summary>
        [TestMethod]
        public void IsValid_GS_All()
        {
            var country = Country.GS;

            IsValid("SIQQ1ZZ", country);
        }

        /// <summary>Tests patterns that should be valid for Guatemala (GT).</summary>
        [TestMethod]
        public void IsValid_GT_All()
        {
            var country = Country.GT;

            IsValid("00000", country);
            IsValid("01235", country);
            IsValid("12346", country);
            IsValid("20004", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
            IsValid("76345", country);
            IsValid("67552", country);
            IsValid("87182", country);
            IsValid("99999", country);
        }

        /// <summary>Tests patterns that should be valid for Guam (GU).</summary>
        [TestMethod]
        public void IsValid_GU_All()
        {
            var country = Country.GU;

            IsValid("96910", country);
            IsValid("96910", country);
            IsValid("96911", country);
            IsValid("96912", country);
            IsValid("96923", country);
            IsValid("96924", country);
            IsValid("96925", country);
            IsValid("96926", country);
            IsValid("96927", country);
            IsValid("96926", country);
            IsValid("96931", country);
            IsValid("96932", country);
            IsValid("969100000", country);
            IsValid("969103015", country);
            IsValid("969114126", country);
            IsValid("969120204", country);
            IsValid("969234328", country);
            IsValid("969244405", country);
            IsValid("969254560", country);
            IsValid("969260628", country);
            IsValid("969274765", country);
            IsValid("969265672", country);
            IsValid("969318872", country);
            IsValid("969329999", country);
        }

        /// <summary>Tests patterns that should be valid for Guinea-Bissau (GW).</summary>
        [TestMethod]
        public void IsValid_GW_All()
        {
            var country = Country.GW;

            IsValid("0123", country);
            IsValid("1234", country);
            IsValid("2000", country);
            IsValid("3248", country);
            IsValid("4945", country);
            IsValid("5640", country);
            IsValid("6208", country);
            IsValid("7645", country);
            IsValid("6752", country);
            IsValid("8782", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for Heard Island And Mcdonald Islands (HM).</summary>
        [TestMethod]
        public void IsValid_HM_All()
        {
            var country = Country.HM;

            IsValid("0123", country);
            IsValid("1234", country);
            IsValid("2000", country);
            IsValid("3248", country);
            IsValid("4945", country);
            IsValid("5640", country);
            IsValid("6208", country);
            IsValid("7645", country);
            IsValid("6752", country);
            IsValid("8782", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for Honduras (HN).</summary>
        [TestMethod]
        public void IsValid_HN_All()
        {
            var country = Country.HN;

            IsValid("00000", country);
            IsValid("01235", country);
            IsValid("12346", country);
            IsValid("20004", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
            IsValid("76345", country);
            IsValid("67552", country);
            IsValid("87182", country);
            IsValid("99999", country);
        }

        /// <summary>Tests patterns that should be valid for Croatia (HR).</summary>
        [TestMethod]
        public void IsValid_HR_All()
        {
            var country = Country.HR;

            IsValid("00000", country);
            IsValid("01235", country);
            IsValid("12346", country);
            IsValid("20004", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
            IsValid("76345", country);
            IsValid("67552", country);
            IsValid("87182", country);
            IsValid("99999", country);
        }

        /// <summary>Tests patterns that should be valid for Haiti (HT).</summary>
        [TestMethod]
        public void IsValid_HT_All()
        {
            var country = Country.HT;

            IsValid("0123", country);
            IsValid("1234", country);
            IsValid("2000", country);
            IsValid("3248", country);
            IsValid("4945", country);
            IsValid("5640", country);
            IsValid("6208", country);
            IsValid("7645", country);
            IsValid("6752", country);
            IsValid("8782", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for Hungary (HU).</summary>
        [TestMethod]
        public void IsValid_HU_All()
        {
            var country = Country.HU;

            IsValid("1000", country);
            IsValid("2077", country);
            IsValid("2650", country);
            IsValid("4241", country);
        }

        /// <summary>Tests patterns that should be valid for Indonesia (ID).</summary>
        [TestMethod]
        public void IsValid_ID_All()
        {
            var country = Country.ID;

            IsValid("10000", country);
            IsValid("31123", country);
            IsValid("89000", country);
            IsValid("89007", country);
            IsValid("12340", country);
        }

        /// <summary>Tests patterns that should be valid for Israel (IL).</summary>
        [TestMethod]
        public void IsValid_IL_All()
        {
            var country = Country.IL;

            IsValid("0110023", country);
            IsValid("1084023", country);
            IsValid("3108701", country);
            IsValid("4201907", country);
            IsValid("5403506", country);
            IsValid("6177008", country);
        }

        /// <summary>Tests patterns that should be valid for Isle Of Man (IM).</summary>
        [TestMethod]
        public void IsValid_IM_All()
        {
            var country = Country.IM;

            IsValid("00DF", country);
            IsValid("04DS", country);
            IsValid("18RF", country);
            IsValid("23WK", country);
            IsValid("34SD", country);
            IsValid("40PJ", country);
            IsValid("59KF", country);
            IsValid("68LS", country);
            IsValid("71JD", country);
            IsValid("66MO", country);
            IsValid("85DF", country);
            IsValid("99JS", country);
            IsValid("00DF", country);

            IsValid("000DF", country);
            IsValid("014DS", country);
            IsValid("128RF", country);
            IsValid("203WK", country);
            IsValid("324SD", country);
            IsValid("400PJ", country);
            IsValid("569KF", country);
            IsValid("628LS", country);
            IsValid("761JD", country);
            IsValid("676MO", country);
            IsValid("875DF", country);
            IsValid("999JS", country);
            IsValid("000DF", country);

            IsValid("IM00DF", country);
            IsValid("IM04DS", country);
            IsValid("IM18RF", country);
            IsValid("IM23WK", country);
            IsValid("IM34SD", country);
            IsValid("IM40PJ", country);
            IsValid("IM59KF", country);
            IsValid("IM68LS", country);
            IsValid("IM71JD", country);
            IsValid("IM66MO", country);
            IsValid("IM85DF", country);
            IsValid("IM99JS", country);
            IsValid("IM00DF", country);

            IsValid("IM000DF", country);
            IsValid("IM014DS", country);
            IsValid("IM128RF", country);
            IsValid("IM203WK", country);
            IsValid("IM324SD", country);
            IsValid("IM400PJ", country);
            IsValid("IM569KF", country);
            IsValid("IM628LS", country);
            IsValid("IM761JD", country);
            IsValid("IM676MO", country);
            IsValid("IM875DF", country);
            IsValid("IM999JS", country);
            IsValid("IM000DF", country);

            IsValid("IM00DF", country);
            IsValid("IM04DS", country);
            IsValid("IM18RF", country);
            IsValid("IM23WK", country);
            IsValid("IM34SD", country);
            IsValid("IM40PJ", country);
            IsValid("IM59KF", country);
            IsValid("IM68LS", country);
            IsValid("IM71JD", country);
            IsValid("IM66MO", country);
            IsValid("IM85DF", country);
            IsValid("IM99JS", country);
            IsValid("IM00DF", country);
        }

        /// <summary>Tests patterns that should be valid for India (IN).</summary>
        [TestMethod]
        public void IsValid_IN_All()
        {
            var country = Country.IN;

            IsValid("110000", country);
            IsValid("342600", country);
            IsValid("810185", country);
            IsValid("810 185", country);
        }

        /// <summary>Tests patterns that should be valid for British Indian Ocean Territory (IO).</summary>
        [TestMethod]
        public void IsValid_IO_All()
        {
            var country = Country.IO;

            IsValid("BBND1ZZ", country);
        }

        /// <summary>Tests patterns that should be valid for Iraq (IQ).</summary>
        [TestMethod]
        public void IsValid_IQ_All()
        {
            var country = Country.IQ;

            IsValid("12346", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
        }

        /// <summary>Tests patterns that should be valid for Iran (IR).</summary>
        [TestMethod]
        public void IsValid_IR_All()
        {
            var country = Country.IR;

            IsValid("0000000000", country);
            IsValid("0144942325", country);
            IsValid("1282353436", country);
            IsValid("2037570044", country);
            IsValid("3243436478", country);
            IsValid("4008279475", country);
            IsValid("5697836450", country);
            IsValid("6282469088", country);
            IsValid("7611343495", country);
            IsValid("6767185502", country);
            IsValid("8752391832", country);
            IsValid("9999999999", country);

        }

        /// <summary>Tests patterns that should be valid for Iceland (IS).</summary>
        [TestMethod]
        public void IsValid_IS_All()
        {
            var country = Country.IS;

            IsValid("000", country);
            IsValid("035", country);
            IsValid("146", country);
            IsValid("204", country);
            IsValid("348", country);
            IsValid("445", country);
            IsValid("540", country);
            IsValid("608", country);
            IsValid("745", country);
            IsValid("652", country);
            IsValid("882", country);
            IsValid("999", country);
        }

        /// <summary>Tests patterns that should be valid for Italy (IT).</summary>
        [TestMethod]
        public void IsValid_IT_All()
        {
            var country = Country.IT;

            IsValid("00123", country);
            IsValid("02123", country);
            IsValid("31001", country);
            IsValid("42007", country);
            IsValid("54006", country);
            IsValid("91008", country);
        }

        /// <summary>Tests patterns that should be valid for Jersey (JE).</summary>
        [TestMethod]
        public void IsValid_JE_All()
        {
            var country = Country.JE;

            IsValid("00AS", country);
            IsValid("25GS", country);
            IsValid("36DF", country);
            IsValid("44DS", country);
            IsValid("78RF", country);
            IsValid("75WK", country);
            IsValid("50SD", country);
            IsValid("88PJ", country);
            IsValid("95KF", country);
            IsValid("02LS", country);
            IsValid("32JD", country);
            IsValid("99MO", country);
            IsValid("00AS", country);
            IsValid("042GS", country);
            IsValid("153DF", country);
            IsValid("274DS", country);
            IsValid("337RF", country);
            IsValid("477WK", country);
            IsValid("535SD", country);
            IsValid("668PJ", country);
            IsValid("749KF", country);
            IsValid("680LS", country);
            IsValid("893JD", country);
            IsValid("999MO", country);
        }

        /// <summary>Tests patterns that should be valid for Jordan (JO).</summary>
        [TestMethod]
        public void IsValid_JO_All()
        {
            var country = Country.JO;

            IsValid("00000", country);
            IsValid("01235", country);
            IsValid("12346", country);
            IsValid("20004", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
            IsValid("76345", country);
            IsValid("67552", country);
            IsValid("87182", country);
            IsValid("99999", country);
        }

        /// <summary>Tests patterns that should be valid for Japan (JP).</summary>
        [TestMethod]
        public void IsValid_JP_All()
        {
            var country = Country.JP;

            IsValid("000-0000", country);
            IsValid("000-0999", country);
            IsValid("010-0000", country);
            IsValid("0100999", country);
            IsValid("880-0123", country);
            IsValid("900-0123", country);
        }

        /// <summary>Tests patterns that should be valid for Kyrgyzstan (KG).</summary>
        [TestMethod]
        public void IsValid_KG_All()
        {
            var country = Country.KG;

            IsValid("000000", country);
            IsValid("023145", country);
            IsValid("134256", country);
            IsValid("200074", country);
            IsValid("364238", country);
            IsValid("494075", country);
            IsValid("564630", country);
            IsValid("690268", country);
            IsValid("734645", country);
            IsValid("655782", country);
            IsValid("818792", country);
            IsValid("999999", country);

        }

        /// <summary>Tests patterns that should be valid for Cambodia (KH).</summary>
        [TestMethod]
        public void IsValid_KH_All()
        {
            var country = Country.KH;

            IsValid("00000", country);
            IsValid("01235", country);
            IsValid("12346", country);
            IsValid("20004", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
            IsValid("76345", country);
            IsValid("67552", country);
            IsValid("87182", country);
            IsValid("99999", country);
        }

        /// <summary>Tests patterns that should be valid for Korea (KR).</summary>
        [TestMethod]
        public void IsValid_KR_All()
        {
            var country = Country.KR;

            IsValid("110000", country);
            IsValid("342600", country);
            IsValid("610185", country);
            IsValid("410-185", country);
            IsValid("710-185", country);
        }

        /// <summary>Tests patterns that should be valid for Cayman Islands (KY).</summary>
        [TestMethod]
        public void IsValid_KY_All()
        {
            var country = Country.KY;

            IsValid("00000", country);
            IsValid("01235", country);
            IsValid("12346", country);
            IsValid("20004", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
            IsValid("76345", country);
            IsValid("67552", country);
            IsValid("87182", country);
            IsValid("99999", country);
        }

        /// <summary>Tests patterns that should be valid for Kazakhstan (KZ).</summary>
        [TestMethod]
        public void IsValid_KZ_All()
        {
            var country = Country.KZ;

            IsValid("000000", country);
            IsValid("023145", country);
            IsValid("134256", country);
            IsValid("200074", country);
            IsValid("364238", country);
            IsValid("494075", country);
            IsValid("564630", country);
            IsValid("690268", country);
            IsValid("734645", country);
            IsValid("655782", country);
            IsValid("818792", country);
            IsValid("999999", country);

        }

        /// <summary>Tests patterns that should be valid for Lao People'S Democratic Republic (LA).</summary>
        [TestMethod]
        public void IsValid_LA_All()
        {
            var country = Country.LA;

            IsValid("00000", country);
            IsValid("01235", country);
            IsValid("12346", country);
            IsValid("20004", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
            IsValid("76345", country);
            IsValid("67552", country);
            IsValid("87182", country);
            IsValid("99999", country);
        }

        /// <summary>Tests patterns that should be valid for Lebanon (LB).</summary>
        [TestMethod]
        public void IsValid_LB_All()
        {
            var country = Country.LB;

            IsValid("00000000", country);
            IsValid("01442325", country);
            IsValid("12853436", country);
            IsValid("20370044", country);
            IsValid("32436478", country);
            IsValid("40079475", country);
            IsValid("56936450", country);
            IsValid("62869088", country);
            IsValid("76143495", country);
            IsValid("67685502", country);
            IsValid("87591832", country);
            IsValid("99999999", country);
        }

        /// <summary>Tests patterns that should be valid for Liechtenstein (LI).</summary>
        [TestMethod]
        public void IsValid_LI_All()
        {
            var country = Country.LI;

            for (int code = 9485; code <= 9498; code++)
            {
                IsValid(code.ToString(), country);
            }
        }

        /// <summary>Tests patterns that should be valid for Sri Lanka (LK).</summary>
        [TestMethod]
        public void IsValid_LK_All()
        {
            var country = Country.LK;

            IsValid("00000", country);
            IsValid("10070", country);
            IsValid("20767", country);
            IsValid("26560", country);
            IsValid("32451", country);
            IsValid("09112", country);
            IsValid("48221", country);
            IsValid("54636", country);
            IsValid("65050", country);
            IsValid("70162", country);
            IsValid("81271", country);
            IsValid("92686", country);
        }

        /// <summary>Tests patterns that should be valid for Liberia (LR).</summary>
        [TestMethod]
        public void IsValid_LR_All()
        {
            var country = Country.LR;

            IsValid("0123", country);
            IsValid("1234", country);
            IsValid("2000", country);
            IsValid("3248", country);
            IsValid("4945", country);
            IsValid("5640", country);
            IsValid("6208", country);
            IsValid("7645", country);
            IsValid("6752", country);
            IsValid("8782", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for Lesotho (LS).</summary>
        [TestMethod]
        public void IsValid_LS_All()
        {
            var country = Country.LS;

            IsValid("000", country);
            IsValid("015", country);
            IsValid("126", country);
            IsValid("204", country);
            IsValid("328", country);
            IsValid("405", country);
            IsValid("560", country);
            IsValid("628", country);
            IsValid("765", country);
            IsValid("672", country);
            IsValid("872", country);
            IsValid("999", country);
        }

        /// <summary>Tests patterns that should be valid for Lithuania (LT).</summary>
        [TestMethod]
        public void IsValid_LT_All()
        {
            var country = Country.LT;

            IsValid("21499", country);
            IsValid("01499", country);
            IsValid("lT-31499", country);
            IsValid("LT-01499", country);
            IsValid("lt81499", country);
            IsValid("LT71499", country);
            IsValid("LT56990", country);
        }

        /// <summary>Tests patterns that should be valid for Luxembourg (LU).</summary>
        [TestMethod]
        public void IsValid_LU_All()
        {
            var country = Country.LU;

            IsValid("0123", country);
            IsValid("1234", country);
            IsValid("2000", country);
            IsValid("3248", country);
            IsValid("4945", country);
            IsValid("5640", country);
            IsValid("6208", country);
            IsValid("7645", country);
            IsValid("6752", country);
            IsValid("8782", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for Latvia (LV).</summary>
        [TestMethod]
        public void IsValid_LV_All()
        {
            var country = Country.LV;

            IsValid("0123", country);
            IsValid("1234", country);
            IsValid("2000", country);
            IsValid("3248", country);
            IsValid("4945", country);
            IsValid("5640", country);
            IsValid("6208", country);
            IsValid("7645", country);
            IsValid("6752", country);
            IsValid("8782", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for Libya (LY).</summary>
        [TestMethod]
        public void IsValid_LY_All()
        {
            var country = Country.LY;

            IsValid("00000", country);
            IsValid("01235", country);
            IsValid("12346", country);
            IsValid("20004", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
            IsValid("76345", country);
            IsValid("67552", country);
            IsValid("87182", country);
            IsValid("99999", country);
        }

        /// <summary>Tests patterns that should be valid for Morocco (MA).</summary>
        [TestMethod]
        public void IsValid_MA_All()
        {
            var country = Country.MA;

            IsValid("11 302", country);
            IsValid("24 023", country);
            IsValid("45001", country);
            IsValid("89607", country);
            IsValid("86096", country);
            IsValid("85808", country);
        }

        /// <summary>Tests patterns that should be valid for Monaco (MC).</summary>
        [TestMethod]
        public void IsValid_MC_All()
        {
            var country = Country.MC;

            IsValid("MC-98000", country);
            IsValid("MC-98012", country);
            IsValid("MC 98023", country);
            IsValid("mc98089", country);
            IsValid("MC98099", country);
            IsValid("Mc98077", country);
            IsValid("mC98066", country);
            IsValid("98089", country);
            IsValid("98099", country);
            IsValid("98077", country);
            IsValid("98066", country);
        }

        /// <summary>Tests patterns that should be valid for Moldova (MD).</summary>
        [TestMethod]
        public void IsValid_MD_All()
        {
            var country = Country.MD;

            IsValid("1499", country);
            IsValid("md-1499", country);
            IsValid("MD-1499", country);
            IsValid("md1499", country);
            IsValid("MD0499", country);
            IsValid("MD0099", country);
            IsValid("mD6990", country);
            IsValid("0123", country);
            IsValid("1234", country);
            IsValid("2000", country);
            IsValid("3248", country);
            IsValid("4945", country);
            IsValid("5640", country);
            IsValid("6208", country);
            IsValid("7645", country);
            IsValid("6752", country);
            IsValid("8782", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for Montenegro (ME).</summary>
        [TestMethod]
        public void IsValid_ME_All()
        {
            var country = Country.ME;

            IsValid("81302", country);
            IsValid("84023", country);
            IsValid("85001", country);
            IsValid("81607", country);
            IsValid("84096", country);
            IsValid("85808", country);
        }

        /// <summary>Tests patterns that should be valid for Saint Martin (MF).</summary>
        [TestMethod]
        public void IsValid_MF_All()
        {
            var country = Country.MF;

            IsValid("97800", country);
            IsValid("97805", country);
            IsValid("97816", country);
            IsValid("97824", country);
            IsValid("97838", country);
            IsValid("97845", country);
            IsValid("97850", country);
            IsValid("97868", country);
            IsValid("97875", country);
            IsValid("97862", country);
            IsValid("97882", country);
            IsValid("97899", country);
        }

        /// <summary>Tests patterns that should be valid for Madagascar (MG).</summary>
        [TestMethod]
        public void IsValid_MG_All()
        {
            var country = Country.MG;

            IsValid("000", country);
            IsValid("015", country);
            IsValid("126", country);
            IsValid("204", country);
            IsValid("328", country);
            IsValid("405", country);
            IsValid("560", country);
            IsValid("628", country);
            IsValid("765", country);
            IsValid("672", country);
            IsValid("872", country);
            IsValid("999", country);
        }

        /// <summary>Tests patterns that should be valid for Marshall Islands (MH).</summary>
        [TestMethod]
        public void IsValid_MH_All()
        {
            var country = Country.MH;

            IsValid("96960", country);
            IsValid("96960", country);
            IsValid("96961", country);
            IsValid("96962", country);
            IsValid("96963", country);
            IsValid("96964", country);
            IsValid("96965", country);
            IsValid("96976", country);
            IsValid("96977", country);
            IsValid("96976", country);
            IsValid("96978", country);
            IsValid("96979", country);
            IsValid("96970", country);
            IsValid("969600000", country);
            IsValid("969604423", country);
            IsValid("969612534", country);
            IsValid("969627700", country);
            IsValid("969633364", country);
            IsValid("969648794", country);
            IsValid("969657364", country);
            IsValid("969762690", country);
            IsValid("969771434", country);
            IsValid("969767855", country);
            IsValid("969782918", country);
            IsValid("969799999", country);
            IsValid("969700000", country);
        }

        /// <summary>Tests patterns that should be valid for Macedonia (MK).</summary>
        [TestMethod]
        public void IsValid_MK_All()
        {
            var country = Country.MK;

            IsValid("0123", country);
            IsValid("1234", country);
            IsValid("2000", country);
            IsValid("3248", country);
            IsValid("4945", country);
            IsValid("5640", country);
            IsValid("6208", country);
            IsValid("7645", country);
            IsValid("6752", country);
            IsValid("8782", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for Myanmar (MM).</summary>
        [TestMethod]
        public void IsValid_MM_All()
        {
            var country = Country.MM;

            IsValid("00000", country);
            IsValid("01235", country);
            IsValid("12346", country);
            IsValid("20004", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
            IsValid("76345", country);
            IsValid("67552", country);
            IsValid("87182", country);
            IsValid("99999", country);
        }

        /// <summary>Tests patterns that should be valid for Mongolia (MN).</summary>
        [TestMethod]
        public void IsValid_MN_All()
        {
            var country = Country.MN;

            IsValid("00000", country);
            IsValid("01235", country);
            IsValid("12346", country);
            IsValid("20004", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
            IsValid("76345", country);
            IsValid("67552", country);
            IsValid("87182", country);
            IsValid("99999", country);
        }

        /// <summary>Tests patterns that should be valid for Northern Mariana Islands (MP).</summary>
        [TestMethod]
        public void IsValid_MP_All()
        {
            var country = Country.MP;

            IsValid("96950", country);
            IsValid("96951", country);
            IsValid("96952", country);
            IsValid("969500000", country);
            IsValid("969500143", country);
            IsValid("969501254", country);
            IsValid("969502070", country);
            IsValid("969513234", country);
            IsValid("969514074", country);
            IsValid("969515634", country);
            IsValid("969516260", country);
            IsValid("969527644", country);
            IsValid("969526785", country);
            IsValid("969528798", country);
            IsValid("969529999", country);
        }

        /// <summary>Tests patterns that should be valid for Martinique (MQ).</summary>
        [TestMethod]
        public void IsValid_MQ_All()
        {
            var country = Country.MQ;

            IsValid("97200", country);
            IsValid("97201", country);
            IsValid("97212", country);
            IsValid("97220", country);
            IsValid("97232", country);
            IsValid("97240", country);
            IsValid("97256", country);
            IsValid("97262", country);
            IsValid("97276", country);
            IsValid("97267", country);
            IsValid("97287", country);
            IsValid("97299", country);
        }

        /// <summary>Tests patterns that should be valid for Malta (MT).</summary>
        [TestMethod]
        public void IsValid_MT_All()
        {
            var country = Country.MT;

            IsValid("AAA0000", country);
            IsValid("ASD0132", country);
            IsValid("BJR1243", country);
            IsValid("CDW2004", country);
            IsValid("DES3247", country);
            IsValid("EOP4047", country);
            IsValid("FNK5645", country);
            IsValid("GFL6208", country);
            IsValid("HLJ7649", country);
            IsValid("IDM6750", country);
            IsValid("JSD8783", country);
            IsValid("KNJ9999", country);
            IsValid("LOD0000", country);
            IsValid("MED0132", country);
            IsValid("NNR1243", country);
            IsValid("OLW2004", country);
            IsValid("PSS3247", country);
            IsValid("QDP4047", country);
            IsValid("RNK5645", country);
            IsValid("SEL6208", country);
            IsValid("TMJ7649", country);
            IsValid("UFM6750", country);
            IsValid("VED8783", country);
            IsValid("WLJ9999", country);
            IsValid("XMD0000", country);
            IsValid("YED0132", country);
            IsValid("ZLR1243", country);
            IsValid("ZZZ9999", country);
        }

        /// <summary>Tests patterns that should be valid for Mexico (MX).</summary>
        [TestMethod]
        public void IsValid_MX_All()
        {
            var country = Country.MX;

            IsValid("09302", country);
            IsValid("10023", country);
            IsValid("31001", country);
            IsValid("42007", country);
            IsValid("54006", country);
            IsValid("61008", country);
        }

        /// <summary>Tests patterns that should be valid for Malaysia (MY).</summary>
        [TestMethod]
        public void IsValid_MY_All()
        {
            var country = Country.MY;

            IsValid("10023", country);
            IsValid("31001", country);
            IsValid("42007", country);
            IsValid("54006", country);
            IsValid("61008", country);
        }

        /// <summary>Tests patterns that should be valid for Mozambique (MZ).</summary>
        [TestMethod]
        public void IsValid_MZ_All()
        {
            var country = Country.MZ;

            IsValid("0123", country);
            IsValid("1234", country);
            IsValid("2000", country);
            IsValid("3248", country);
            IsValid("4945", country);
            IsValid("5640", country);
            IsValid("6208", country);
            IsValid("7645", country);
            IsValid("6752", country);
            IsValid("8782", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for Namibia (NA).</summary>
        [TestMethod]
        public void IsValid_NA_All()
        {
            var country = Country.NA;

            IsValid("90000", country);
            IsValid("90015", country);
            IsValid("90126", country);
            IsValid("90204", country);
            IsValid("91328", country);
            IsValid("91405", country);
            IsValid("91560", country);
            IsValid("91628", country);
            IsValid("92765", country);
            IsValid("92672", country);
            IsValid("92872", country);
            IsValid("92999", country);
        }

        /// <summary>Tests patterns that should be valid for New Caledonia (NC).</summary>
        [TestMethod]
        public void IsValid_NC_All()
        {
            var country = Country.NC;

            IsValid("98800", country);
            IsValid("98802", country);
            IsValid("98813", country);
            IsValid("98820", country);
            IsValid("98836", country);
            IsValid("98884", country);
            IsValid("98895", country);
            IsValid("98896", country);
            IsValid("98897", country);
            IsValid("98896", country);
            IsValid("98898", country);
            IsValid("98899", country);
        }

        /// <summary>Tests patterns that should be valid for Niger (NE).</summary>
        [TestMethod]
        public void IsValid_NE_All()
        {
            var country = Country.NE;

            IsValid("0123", country);
            IsValid("1234", country);
            IsValid("2000", country);
            IsValid("3248", country);
            IsValid("4945", country);
            IsValid("5640", country);
            IsValid("6208", country);
            IsValid("7645", country);
            IsValid("6752", country);
            IsValid("8782", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for Norfolk Island (NF).</summary>
        [TestMethod]
        public void IsValid_NF_All()
        {
            var country = Country.NF;

            IsValid("0123", country);
            IsValid("1234", country);
            IsValid("2000", country);
            IsValid("3248", country);
            IsValid("4945", country);
            IsValid("5640", country);
            IsValid("6208", country);
            IsValid("7645", country);
            IsValid("6752", country);
            IsValid("8782", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for Nigeria (NG).</summary>
        [TestMethod]
        public void IsValid_NG_All()
        {
            var country = Country.NG;

            IsValid("009999", country);
            IsValid("018010", country);
            IsValid("110000", country);
            IsValid("342600", country);
            IsValid("810185", country);
            IsValid("810185", country);
        }

        /// <summary>Tests patterns that should be valid for Nicaragua (NI).</summary>
        [TestMethod]
        public void IsValid_NI_All()
        {
            var country = Country.NI;

            IsValid("00000", country);
            IsValid("01235", country);
            IsValid("12346", country);
            IsValid("20004", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
            IsValid("76345", country);
            IsValid("67552", country);
            IsValid("87182", country);
            IsValid("99999", country);
        }

        /// <summary>Tests patterns that should be valid for Netherlands (NL).</summary>
        [TestMethod]
        public void IsValid_NL_All()
        {
            var country = Country.NL;

            IsValid("1236RF", country);
            IsValid("2044WK", country);
            IsValid("4075PJ", country);
            IsValid("5650KF", country);
            IsValid("6288LS", country);
            IsValid("7695JD", country);
            IsValid("6702MO", country);
            IsValid("8732DF", country);
            IsValid("9999JS", country);
        }

        /// <summary>Tests patterns that should be valid for Norway (NO).</summary>
        [TestMethod]
        public void IsValid_NO_All()
        {
            var country = Country.NO;

            IsValid("0912", country);
            IsValid("0821", country);
            IsValid("0666", country);
            IsValid("0000", country);
            IsValid("1000", country);
            IsValid("2077", country);
            IsValid("2650", country);
            IsValid("4241", country);
        }

        /// <summary>Tests patterns that should be valid for Nepal (NP).</summary>
        [TestMethod]
        public void IsValid_NP_All()
        {
            var country = Country.NP;

            IsValid("00000", country);
            IsValid("01235", country);
            IsValid("12346", country);
            IsValid("20004", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
            IsValid("76345", country);
            IsValid("67552", country);
            IsValid("87182", country);
            IsValid("99999", country);
        }

        /// <summary>Tests patterns that should be valid for New Zealand (NZ).</summary>
        [TestMethod]
        public void IsValid_NZ_All()
        {
            var country = Country.NZ;

            IsValid("0912", country);
            IsValid("0821", country);
            IsValid("0666", country);
            IsValid("0000", country);
            IsValid("1000", country);
            IsValid("2077", country);
            IsValid("2650", country);
            IsValid("4241", country);
        }

        /// <summary>Tests patterns that should be valid for Oman (OM).</summary>
        [TestMethod]
        public void IsValid_OM_All()
        {
            var country = Country.OM;

            IsValid("000", country);
            IsValid("015", country);
            IsValid("126", country);
            IsValid("204", country);
            IsValid("328", country);
            IsValid("405", country);
            IsValid("560", country);
            IsValid("628", country);
            IsValid("765", country);
            IsValid("672", country);
            IsValid("872", country);
            IsValid("999", country);
        }

        /// <summary>Tests patterns that should be valid for Panama (PA).</summary>
        [TestMethod]
        public void IsValid_PA_All()
        {
            var country = Country.PA;

            IsValid("000000", country);
            IsValid("023145", country);
            IsValid("134256", country);
            IsValid("200074", country);
            IsValid("364238", country);
            IsValid("494075", country);
            IsValid("564630", country);
            IsValid("690268", country);
            IsValid("734645", country);
            IsValid("655782", country);
            IsValid("818792", country);
            IsValid("999999", country);

        }

        /// <summary>Tests patterns that should be valid for Peru (PE).</summary>
        [TestMethod]
        public void IsValid_PE_All()
        {
            var country = Country.PE;

            IsValid("00000", country);
            IsValid("01235", country);
            IsValid("12346", country);
            IsValid("20004", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
            IsValid("76345", country);
            IsValid("67552", country);
            IsValid("87182", country);
            IsValid("99999", country);
        }

        /// <summary>Tests patterns that should be valid for French Polynesia (PF).</summary>
        [TestMethod]
        public void IsValid_PF_All()
        {
            var country = Country.PF;

            IsValid("98700", country);
            IsValid("98725", country);
            IsValid("98736", country);
            IsValid("98704", country);
            IsValid("98768", country);
            IsValid("98795", country);
            IsValid("98760", country);
            IsValid("98798", country);
            IsValid("98735", country);
            IsValid("98752", country);
            IsValid("98712", country);
            IsValid("98799", country);
        }

        /// <summary>Tests patterns that should be valid for Papua New Guinea (PG).</summary>
        [TestMethod]
        public void IsValid_PG_All()
        {
            var country = Country.PG;

            IsValid("000", country);
            IsValid("015", country);
            IsValid("126", country);
            IsValid("204", country);
            IsValid("328", country);
            IsValid("405", country);
            IsValid("560", country);
            IsValid("628", country);
            IsValid("765", country);
            IsValid("672", country);
            IsValid("872", country);
            IsValid("999", country);
        }

        /// <summary>Tests patterns that should be valid for Philippines (PH).</summary>
        [TestMethod]
        public void IsValid_PH_All()
        {
            var country = Country.PH;

            IsValid("0123", country);
            IsValid("1234", country);
            IsValid("2000", country);
            IsValid("3248", country);
            IsValid("4945", country);
            IsValid("5640", country);
            IsValid("6208", country);
            IsValid("7645", country);
            IsValid("6752", country);
            IsValid("8782", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for Pakistan (PK).</summary>
        [TestMethod]
        public void IsValid_PK_All()
        {
            var country = Country.PK;

            IsValid("11302", country);
            IsValid("24023", country);
            IsValid("45001", country);
            IsValid("89607", country);
            IsValid("86096", country);
            IsValid("85808", country);
        }

        /// <summary>Tests patterns that should be valid for Poland (PL).</summary>
        [TestMethod]
        public void IsValid_PL_All()
        {
            var country = Country.PL;

            IsValid("01302", country);
            IsValid("11302", country);
            IsValid("24023", country);
            IsValid("45001", country);
            IsValid("89607", country);
            IsValid("86096", country);
            IsValid("85808", country);
            IsValid("06-096", country);
            IsValid("85-808", country);
        }

        /// <summary>Tests patterns that should be valid for Saint Pierre And Miquelon (PM).</summary>
        [TestMethod]
        public void IsValid_PM_All()
        {
            var country = Country.PM;

            IsValid("97500", country);
        }

        /// <summary>Tests patterns that should be valid for Pitcairn (PN).</summary>
        [TestMethod]
        public void IsValid_PN_All()
        {
            var country = Country.PN;

            IsValid("PCRN1ZZ", country);
        }

        /// <summary>Tests patterns that should be valid for Puerto Rico (PR).</summary>
        [TestMethod]
        public void IsValid_PR_All()
        {
            var country = Country.PR;

            IsValid("01302", country);
            IsValid("00802", country);
            IsValid("11302", country);
            IsValid("24023", country);
            IsValid("45001", country);
            IsValid("89607", country);
            IsValid("86096", country);
            IsValid("85808", country);
            IsValid("06096", country);
            IsValid("85808", country);
        }

        /// <summary>Tests patterns that should be valid for Palestinian Territory (PS).</summary>
        [TestMethod]
        public void IsValid_PS_All()
        {
            var country = Country.PS;

            IsValid("00000", country);
            IsValid("01235", country);
            IsValid("12346", country);
            IsValid("20004", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
            IsValid("76345", country);
            IsValid("67552", country);
            IsValid("87182", country);
            IsValid("99999", country);
        }

        /// <summary>Tests patterns that should be valid for Portugal (PT).</summary>
        [TestMethod]
        public void IsValid_PT_All()
        {
            var country = Country.PT;

            IsValid("1282353", country);
            IsValid("2037570", country);
            IsValid("3243436", country);
            IsValid("4008279", country);
            IsValid("5697836", country);
            IsValid("6282469", country);
            IsValid("7611343", country);
            IsValid("6767185", country);
            IsValid("8752391", country);
            IsValid("9999999", country);
        }

        /// <summary>Tests patterns that should be valid for Palau (PW).</summary>
        [TestMethod]
        public void IsValid_PW_All()
        {
            var country = Country.PW;

            IsValid("96940", country);
        }

        /// <summary>Tests patterns that should be valid for Paraguay (PY).</summary>
        [TestMethod]
        public void IsValid_PY_All()
        {
            var country = Country.PY;

            IsValid("0123", country);
            IsValid("1234", country);
            IsValid("2000", country);
            IsValid("3248", country);
            IsValid("4945", country);
            IsValid("5640", country);
            IsValid("6208", country);
            IsValid("7645", country);
            IsValid("6752", country);
            IsValid("8782", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for Réunion (RE).</summary>
        [TestMethod]
        public void IsValid_RE_All()
        {
            var country = Country.RE;

            IsValid("97400", country);
            IsValid("97402", country);
            IsValid("97413", country);
            IsValid("97420", country);
            IsValid("97436", country);
            IsValid("97449", country);
            IsValid("97456", country);
            IsValid("97469", country);
            IsValid("97473", country);
            IsValid("97465", country);
            IsValid("97481", country);
            IsValid("97499", country);
        }

        /// <summary>Tests patterns that should be valid for Romania (RO).</summary>
        [TestMethod]
        public void IsValid_RO_All()
        {
            var country = Country.RO;

            IsValid("018010", country);
            IsValid("110000", country);
            IsValid("342600", country);
            IsValid("810185", country);
            IsValid("810185", country);
        }

        /// <summary>Tests patterns that should be valid for Serbia (RS).</summary>
        [TestMethod]
        public void IsValid_RS_All()
        {
            var country = Country.RS;

            IsValid("10070", country);
            IsValid("20767", country);
            IsValid("26560", country);
            IsValid("32451", country);
        }

        /// <summary>Tests patterns that should be valid for Russian Federation (RU).</summary>
        [TestMethod]
        public void IsValid_RU_All()
        {
            var country = Country.RU;

            IsValid("110000", country);
            IsValid("342600", country);
            IsValid("610185", country);
            IsValid("410185", country);
        }

        /// <summary>Tests patterns that should be valid for Saudi Arabia (SA).</summary>
        [TestMethod]
        public void IsValid_SA_All()
        {
            var country = Country.SA;

            IsValid("00000", country);
            IsValid("03145", country);
            IsValid("14256", country);
            IsValid("20074", country);
            IsValid("34238", country);
            IsValid("44075", country);
            IsValid("54630", country);
            IsValid("60268", country);
            IsValid("74645", country);
            IsValid("65782", country);
            IsValid("88792", country);
            IsValid("99999", country);
            IsValid("000000000", country);
            IsValid("031452003", country);
            IsValid("142563114", country);
            IsValid("200740220", country);
            IsValid("342386334", country);
            IsValid("440759444", country);
            IsValid("546306554", country);
            IsValid("602689660", country);
            IsValid("746453774", country);
            IsValid("657825665", country);
            IsValid("887921888", country);
            IsValid("999999999", country);
        }

        /// <summary>Tests patterns that should be valid for Sudan (SD).</summary>
        [TestMethod]
        public void IsValid_SD_All()
        {
            var country = Country.SD;

            IsValid("00000", country);
            IsValid("03145", country);
            IsValid("14256", country);
            IsValid("20074", country);
            IsValid("34238", country);
            IsValid("44075", country);
            IsValid("54630", country);
            IsValid("60268", country);
            IsValid("74645", country);
            IsValid("65782", country);
            IsValid("88792", country);
            IsValid("99999", country);
        }

        /// <summary>Tests patterns that should be valid for Sweden (SE).</summary>
        [TestMethod]
        public void IsValid_SE_All()
        {
            var country = Country.SE;

            IsValid("10000", country);
            IsValid("10070", country);
            IsValid("20767", country);
            IsValid("86560", country);
            IsValid("32451", country);
            IsValid("99112", country);
            IsValid("482 21", country);
            IsValid("546 36", country);
            IsValid("650 50", country);
            IsValid("701 62", country);
            IsValid("812 71", country);
            IsValid("926 86", country);
        }

        /// <summary>Tests patterns that should be valid for Singapore (SG).</summary>
        [TestMethod]
        public void IsValid_SG_All()
        {
            var country = Country.SG;

            IsValid("11000", country);
            IsValid("34600", country);
            IsValid("61185", country);
            IsValid("41185", country);
            IsValid("00999", country);
            IsValid("01010", country);
            IsValid("71185", country);
            IsValid("81185", country);
            IsValid("91185", country);
        }

        /// <summary>Tests patterns that should be valid for Saint Helena (SH).</summary>
        [TestMethod]
        public void IsValid_SH_All()
        {
            var country = Country.SH;

            IsValid("STHL1ZZ", country);
        }

        /// <summary>Tests patterns that should be valid for Slovenia (SI).</summary>
        [TestMethod]
        public void IsValid_SI_All()
        {
            var country = Country.SI;

            IsValid("0123", country);
            IsValid("1234", country);
            IsValid("2000", country);
            IsValid("3248", country);
            IsValid("4945", country);
            IsValid("5640", country);
            IsValid("6208", country);
            IsValid("7645", country);
            IsValid("6752", country);
            IsValid("8782", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for Slovakia (SK).</summary>
        [TestMethod]
        public void IsValid_SK_All()
        {
            var country = Country.SK;

            IsValid("10070", country);
            IsValid("20767", country);
            IsValid("26560", country);
            IsValid("32451", country);
            IsValid("09112", country);
            IsValid("48221", country);
            IsValid("546 36", country);
            IsValid("650 50", country);
            IsValid("701 62", country);
            IsValid("812 71", country);
            IsValid("926 86", country);
        }

        /// <summary>Tests patterns that should be valid for San Marino (SM).</summary>
        [TestMethod]
        public void IsValid_SM_All()
        {
            var country = Country.SM;

            IsValid("47890", country);
            IsValid("47891", country);
            IsValid("47892", country);
            IsValid("47895", country);
            IsValid("47899", country);
        }

        /// <summary>Tests patterns that should be valid for Senegal (SN).</summary>
        [TestMethod]
        public void IsValid_SN_All()
        {
            var country = Country.SN;

            IsValid("00000", country);
            IsValid("01235", country);
            IsValid("12346", country);
            IsValid("20004", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
            IsValid("76345", country);
            IsValid("67552", country);
            IsValid("87182", country);
            IsValid("99999", country);
        }

        /// <summary>Tests patterns that should be valid for El Salvador (SV).</summary>
        [TestMethod]
        public void IsValid_SV_All()
        {
            var country = Country.SV;

            IsValid("01101", country);
        }

        /// <summary>Tests patterns that should be valid for Swaziland (SZ).</summary>
        [TestMethod]
        public void IsValid_SZ_All()
        {
            var country = Country.SZ;

            IsValid("H761", country);
            IsValid("L000", country);
            IsValid("M014", country);
            IsValid("S628", country);
            IsValid("H611", country);
            IsValid("L760", country);
            IsValid("M754", country);
            IsValid("S998", country);
            IsValid("H000", country);
            IsValid("L023", country);
            IsValid("M182", country);
            IsValid("S282", country);
        }

        /// <summary>Tests patterns that should be valid for Turks And Caicos Islands (TC).</summary>
        [TestMethod]
        public void IsValid_TC_All()
        {
            var country = Country.TC;

            IsValid("TKCA1ZZ", country);
        }

        /// <summary>Tests patterns that should be valid for Chad (TD).</summary>
        [TestMethod]
        public void IsValid_TD_All()
        {
            var country = Country.TD;

            IsValid("00000", country);
            IsValid("01235", country);
            IsValid("12346", country);
            IsValid("20004", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
            IsValid("76345", country);
            IsValid("67552", country);
            IsValid("87182", country);
            IsValid("99999", country);
        }

        /// <summary>Tests patterns that should be valid for Thailand (TH).</summary>
        [TestMethod]
        public void IsValid_TH_All()
        {
            var country = Country.TH;

            IsValid("10023", country);
            IsValid("31001", country);
            IsValid("42007", country);
            IsValid("54006", country);
            IsValid("61008", country);
        }

        /// <summary>Tests patterns that should be valid for Tajikistan (TJ).</summary>
        [TestMethod]
        public void IsValid_TJ_All()
        {
            var country = Country.TJ;

            IsValid("000000", country);
            IsValid("023145", country);
            IsValid("134256", country);
            IsValid("200074", country);
            IsValid("364238", country);
            IsValid("494075", country);
            IsValid("564630", country);
            IsValid("690268", country);
            IsValid("734645", country);
            IsValid("655782", country);
            IsValid("818792", country);
            IsValid("999999", country);

        }

        /// <summary>Tests patterns that should be valid for Turkmenistan (TM).</summary>
        [TestMethod]
        public void IsValid_TM_All()
        {
            var country = Country.TM;

            IsValid("000000", country);
            IsValid("023145", country);
            IsValid("134256", country);
            IsValid("200074", country);
            IsValid("364238", country);
            IsValid("494075", country);
            IsValid("564630", country);
            IsValid("690268", country);
            IsValid("734645", country);
            IsValid("655782", country);
            IsValid("818792", country);
            IsValid("999999", country);

        }

        /// <summary>Tests patterns that should be valid for Tunisia (TN).</summary>
        [TestMethod]
        public void IsValid_TN_All()
        {
            var country = Country.TN;

            IsValid("0123", country);
            IsValid("1234", country);
            IsValid("2000", country);
            IsValid("3248", country);
            IsValid("4945", country);
            IsValid("5640", country);
            IsValid("6208", country);
            IsValid("7645", country);
            IsValid("6752", country);
            IsValid("8782", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for Turkey (TR).</summary>
        [TestMethod]
        public void IsValid_TR_All()
        {
            var country = Country.TR;

            IsValid("01302", country);
            IsValid("08302", country);
            IsValid("10023", country);
            IsValid("31001", country);
            IsValid("42007", country);
            IsValid("74006", country);
            IsValid("91008", country);
        }

        /// <summary>Tests patterns that should be valid for Trinidad And Tobago (TT).</summary>
        [TestMethod]
        public void IsValid_TT_All()
        {
            var country = Country.TT;

            IsValid("000000", country);
            IsValid("023145", country);
            IsValid("134256", country);
            IsValid("200074", country);
            IsValid("364238", country);
            IsValid("494075", country);
            IsValid("564630", country);
            IsValid("690268", country);
            IsValid("734645", country);
            IsValid("655782", country);
            IsValid("818792", country);
            IsValid("999999", country);

        }

        /// <summary>Tests patterns that should be valid for Taiwan (TW).</summary>
        [TestMethod]
        public void IsValid_TW_All()
        {
            var country = Country.TW;

            IsValid("10023", country);
            IsValid("31001", country);
            IsValid("42007", country);
            IsValid("54006", country);
            IsValid("61008", country);
            IsValid("91008", country);
        }

        /// <summary>Tests patterns that should be valid for Ukraine (UA).</summary>
        [TestMethod]
        public void IsValid_UA_All()
        {
            var country = Country.UA;

            IsValid("01235", country);
            IsValid("12346", country);
            IsValid("20004", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
            IsValid("76345", country);
            IsValid("67552", country);
            IsValid("87182", country);
            IsValid("99999", country);
        }

        /// <summary>Tests patterns that should be valid for United States (US).</summary>
        [TestMethod]
        public void IsValid_US_All()
        {
            var country = Country.US;

            IsValid("01000-0060", country);
            IsValid("11000-9996", country);
            IsValid("00126", country);
            IsValid("12345", country);
        }

        /// <summary>Tests patterns that should be valid for Uruguay (UY).</summary>
        [TestMethod]
        public void IsValid_UY_All()
        {
            var country = Country.UY;

            IsValid("00000", country);
            IsValid("01235", country);
            IsValid("12346", country);
            IsValid("20004", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
            IsValid("76345", country);
            IsValid("67552", country);
            IsValid("87182", country);
            IsValid("99999", country);
        }

        /// <summary>Tests patterns that should be valid for Holy See (VA).</summary>
        [TestMethod]
        public void IsValid_VA_All()
        {
            var country = Country.VA;

            IsValid("00120", country);
        }

        /// <summary>Tests patterns that should be valid for Saint Vincent And The Grenadines (VC).</summary>
        [TestMethod]
        public void IsValid_VC_All()
        {
            var country = Country.VC;

            IsValid("0123", country);
            IsValid("1234", country);
            IsValid("2000", country);
            IsValid("3248", country);
            IsValid("4945", country);
            IsValid("5640", country);
            IsValid("6208", country);
            IsValid("7645", country);
            IsValid("6752", country);
            IsValid("8782", country);
            IsValid("9999", country);
        }

        /// <summary>Tests patterns that should be valid for Venezuela (VE).</summary>
        [TestMethod]
        public void IsValid_VE_All()
        {
            var country = Country.VE;

            IsValid("0000", country);
            IsValid("0123", country);
            IsValid("1234", country);
            IsValid("2000", country);
            IsValid("3264", country);
            IsValid("4094", country);
            IsValid("5664", country);
            IsValid("6290", country);
            IsValid("7634", country);
            IsValid("6755", country);
            IsValid("8718", country);
            IsValid("9999", country);

            IsValid("0000A", country);
            IsValid("0325A", country);
            IsValid("1436B", country);
            IsValid("2044C", country);
            IsValid("3478D", country);
            IsValid("4475E", country);
            IsValid("5450F", country);
            IsValid("6088G", country);
            IsValid("7495H", country);
            IsValid("6502I", country);
            IsValid("8832J", country);
            IsValid("9999K", country);
            IsValid("0000L", country);
            IsValid("0325M", country);
            IsValid("1436N", country);
            IsValid("2044O", country);
            IsValid("3478P", country);
            IsValid("4475Q", country);
            IsValid("5450R", country);
            IsValid("6088S", country);
            IsValid("7495T", country);
            IsValid("6502U", country);
            IsValid("8832V", country);
            IsValid("9999W", country);
            IsValid("0000X", country);
            IsValid("0325Y", country);
            IsValid("1436Z", country);
            IsValid("2044Z", country);

        }

        /// <summary>Tests patterns that should be valid for Virgin Islands (VG).</summary>
        [TestMethod]
        public void IsValid_VG_All()
        {
            var country = Country.VG;

            IsValid("1103", country);
            IsValid("1114", country);
            IsValid("1120", country);
            IsValid("1138", country);
            IsValid("1145", country);
            IsValid("1150", country);
            IsValid("1168", country);
            IsValid("1135", country);
            IsValid("1162", country);

            IsValid("VG1101", country);
            IsValid("VG1112", country);
            IsValid("VG1120", country);
            IsValid("VG1132", country);
            IsValid("VG1149", country);
            IsValid("VG1156", country);
            IsValid("VG1162", country);
            IsValid("VG1106", country);
            IsValid("VG1167", country);
        }

        /// <summary>Tests patterns that should be valid for Virgin Islands (VI).</summary>
        [TestMethod]
        public void IsValid_VI_All()
        {
            var country = Country.VI;

            IsValid("00815", country);
            IsValid("00826", country);
            IsValid("00837", country);
            IsValid("00846", country);
            IsValid("00858", country);
            IsValid("008152346", country);
            IsValid("008260004", country);
            IsValid("008372648", country);
            IsValid("008460945", country);
            IsValid("008586640", country);
        }

        /// <summary>Tests patterns that should be valid for Viet Nam (VN).</summary>
        [TestMethod]
        public void IsValid_VN_All()
        {
            var country = Country.VN;

            IsValid("000000", country);
            IsValid("023145", country);
            IsValid("134256", country);
            IsValid("200074", country);
            IsValid("364238", country);
            IsValid("494075", country);
            IsValid("564630", country);
            IsValid("690268", country);
            IsValid("734645", country);
            IsValid("655782", country);
            IsValid("818792", country);
            IsValid("999999", country);

        }

        /// <summary>Tests patterns that should be valid for Wallis And Futuna (WF).</summary>
        [TestMethod]
        public void IsValid_WF_All()
        {
            var country = Country.WF;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should be valid for Mayotte (YT).</summary>
        [TestMethod]
        public void IsValid_YT_All()
        {
            var country = Country.YT;

            IsValid("97600", country);
            IsValid("97605", country);
            IsValid("97616", country);
            IsValid("97624", country);
            IsValid("97638", country);
            IsValid("97645", country);
            IsValid("97650", country);
            IsValid("97668", country);
            IsValid("97675", country);
            IsValid("97662", country);
            IsValid("97682", country);
            IsValid("97699", country);
        }

        /// <summary>Tests patterns that should be valid for South Africa (ZA).</summary>
        [TestMethod]
        public void IsValid_ZA_All()
        {
            var country = Country.ZA;

            IsValid("0001", country);
            IsValid("0023", country);
            IsValid("0100", country);
            IsValid("1000", country);
            IsValid("2077", country);
            IsValid("2650", country);
            IsValid("4241", country);
        }

        /// <summary>Tests patterns that should be valid for Zambia (ZM).</summary>
        [TestMethod]
        public void IsValid_ZM_All()
        {
            var country = Country.ZM;

            IsValid("00000", country);
            IsValid("01235", country);
            IsValid("12346", country);
            IsValid("20004", country);
            IsValid("32648", country);
            IsValid("40945", country);
            IsValid("56640", country);
            IsValid("62908", country);
            IsValid("76345", country);
            IsValid("67552", country);
            IsValid("87182", country);
            IsValid("99999", country);
        }

        #endregion

        #region IsNotValid Country tests

        /// <summary>Tests patterns that should not be valid for Andorra (AD).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_AD_All()
        {
            var country = Country.AD;

            IsNotValid("AD890", country);
            IsNotValid("AD901", country);
            IsNotValid("AD012", country);
        }

        /// <summary>Tests patterns that should not be valid for Afghanistan (AF).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_AF_All()
        {
            var country = Country.AF;

            IsNotValid("0077", country);
            IsNotValid("5301", country);
            IsNotValid("6001", country);
            IsNotValid("7023", country);
            IsNotValid("8100", country);
            IsNotValid("9020", country);
            IsNotValid("4441", country);
            IsNotValid("4300", country);
        }

        /// <summary>Tests patterns that should not be valid for Anguilla (AI).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_AI_All()
        {
            var country = Country.AI;

            IsNotValid("9218", country);
            IsNotValid("AI1890", country);
            IsNotValid("AI2901", country);
            IsNotValid("AI2012", country);
        }

        /// <summary>Tests patterns that should not be valid for Albania (AL).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_AL_All()
        {
            var country = Country.AL;

            IsNotValid("0000", country);
            IsNotValid("0125", country);
            IsNotValid("0279", country);
            IsNotValid("0399", country);
            IsNotValid("0418", country);
            IsNotValid("0540", country);
            IsNotValid("0654", country);
            IsNotValid("0790", country);
            IsNotValid("0852", country);
            IsNotValid("0999", country);
        }

        /// <summary>Tests patterns that should not be valid for Armenia (AM).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_AM_All()
        {
            var country = Country.AM;

            IsNotValid("5697", country);
            IsNotValid("6282", country);
            IsNotValid("7611", country);
            IsNotValid("6767", country);
            IsNotValid("8752", country);
            IsNotValid("9999", country);
        }

        /// <summary>Tests patterns that should not be valid for Argentina (AR).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_AR_All()
        {
            var country = Country.AR;

            IsNotValid("A0400XXX", country);
            IsNotValid("S03004DD", country);
        }

        /// <summary>Tests patterns that should not be valid for American Samoa (AS).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_AS_All()
        {
            var country = Country.AS;

            IsNotValid("00000", country);
            IsNotValid("01449", country);
            IsNotValid("12823", country);
            IsNotValid("20375", country);
            IsNotValid("32434", country);
            IsNotValid("40082", country);
            IsNotValid("56978", country);
            IsNotValid("62824", country);
            IsNotValid("76113", country);
            IsNotValid("67671", country);
            IsNotValid("87523", country);
            IsNotValid("00000000", country);
            IsNotValid("01494232", country);
            IsNotValid("12835343", country);
            IsNotValid("20357004", country);
            IsNotValid("32443647", country);
            IsNotValid("40027947", country);
            IsNotValid("56983645", country);
            IsNotValid("62846908", country);
            IsNotValid("76134349", country);
            IsNotValid("67618550", country);
            IsNotValid("87539183", country);
        }

        /// <summary>Tests patterns that should not be valid for Austria (AT).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_AT_All()
        {
            var country = Country.AT;

            IsNotValid("0000", country);
            IsNotValid("0014", country);
            IsNotValid("0128", country);
            IsNotValid("0203", country);
            IsNotValid("0324", country);
            IsNotValid("0400", country);
            IsNotValid("0569", country);
            IsNotValid("0628", country);
            IsNotValid("0761", country);
            IsNotValid("0676", country);
            IsNotValid("0875", country);
            IsNotValid("0999", country);
        }

        /// <summary>Tests patterns that should not be valid for Australia (AU).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_AU_All()
        {
            var country = Country.AU;

            IsNotValid("0000", country);
            IsNotValid("0014", country);
            IsNotValid("0128", country);
            IsNotValid("0203", country);
            IsNotValid("0324", country);
            IsNotValid("0400", country);
            IsNotValid("0569", country);
            IsNotValid("0628", country);
            IsNotValid("0761", country);
        }

        /// <summary>Tests patterns that should not be valid for Åland Islands (AX).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_AX_All()
        {
            var country = Country.AX;

            IsNotValid("0000", country);
            IsNotValid("0144", country);
            IsNotValid("1282", country);
            IsNotValid("2037", country);
            IsNotValid("2000", country);
            IsNotValid("2014", country);
            IsNotValid("2122", country);
            IsNotValid("2323", country);
            IsNotValid("2408", country);
            IsNotValid("2567", country);
            IsNotValid("2622", country);
            IsNotValid("2761", country);
            IsNotValid("2677", country);
            IsNotValid("2872", country);
            IsNotValid("2999", country);
            IsNotValid("0144", country);
            IsNotValid("1282", country);
            IsNotValid("2037", country);
            IsNotValid("3243", country);
            IsNotValid("4008", country);
            IsNotValid("5697", country);
            IsNotValid("6282", country);
            IsNotValid("7611", country);
            IsNotValid("6767", country);
            IsNotValid("8752", country);
            IsNotValid("9999", country);
        }

        /// <summary>Tests patterns that should not be valid for Azerbaijan (AZ).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_AZ_All()
        {
            var country = Country.AZ;

            IsNotValid("DK 6990", country);
            IsNotValid("GL3990", country);
            IsNotValid("FO1990", country);
            IsNotValid("FO990", country);
        }

        /// <summary>Tests patterns that should not be valid for Bosnia And Herzegovina (BA).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_BA_All()
        {
            var country = Country.BA;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Barbados (BB).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_BB_All()
        {
            var country = Country.BB;

            IsNotValid("AA00000", country);
            IsNotValid("AS01449", country);
            IsNotValid("BJ12823", country);
            IsNotValid("CD20375", country);
            IsNotValid("DE32434", country);
            IsNotValid("EO40082", country);
            IsNotValid("FN56978", country);
            IsNotValid("GF62824", country);
            IsNotValid("HL76113", country);
            IsNotValid("ID67671", country);
            IsNotValid("JS87523", country);
            IsNotValid("KN99999", country);
            IsNotValid("LO00000", country);
            IsNotValid("ME01449", country);
            IsNotValid("NN12823", country);
            IsNotValid("OL20375", country);
            IsNotValid("PS32434", country);
            IsNotValid("QD40082", country);
            IsNotValid("RN56978", country);
            IsNotValid("SE62824", country);
            IsNotValid("TM76113", country);
            IsNotValid("UF67671", country);
            IsNotValid("VE87523", country);
            IsNotValid("WL99999", country);
            IsNotValid("XM00000", country);
            IsNotValid("YE01449", country);
            IsNotValid("ZL12823", country);
            IsNotValid("ZZ20375", country);
        }

        /// <summary>Tests patterns that should not be valid for Bangladesh (BD).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_BD_All()
        {
            var country = Country.BD;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Belgium (BE).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_BE_All()
        {
            var country = Country.BE;

            IsNotValid("0000", country);
            IsNotValid("0014", country);
            IsNotValid("0128", country);
            IsNotValid("0203", country);
            IsNotValid("0324", country);
            IsNotValid("0400", country);
            IsNotValid("0569", country);
            IsNotValid("0628", country);
            IsNotValid("0761", country);
            IsNotValid("0676", country);
            IsNotValid("0875", country);
            IsNotValid("0999", country);
        }

        /// <summary>Tests patterns that should not be valid for Bulgaria (BG).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_BG_All()
        {
            var country = Country.BG;

            IsNotValid("0000", country);
            IsNotValid("0014", country);
            IsNotValid("0128", country);
            IsNotValid("0203", country);
            IsNotValid("0324", country);
            IsNotValid("0400", country);
            IsNotValid("0569", country);
            IsNotValid("0628", country);
            IsNotValid("0761", country);
            IsNotValid("0676", country);
            IsNotValid("0875", country);
            IsNotValid("0999", country);
        }

        /// <summary>Tests patterns that should not be valid for Bahrain (BH).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_BH_All()
        {
            var country = Country.BH;

            IsNotValid("000", country);
            IsNotValid("014", country);
            IsNotValid("1328", country);
            IsNotValid("2037", country);
            IsNotValid("3243", country);
            IsNotValid("4008", country);
            IsNotValid("5697", country);
            IsNotValid("6282", country);
            IsNotValid("7611", country);
            IsNotValid("6767", country);
            IsNotValid("8752", country);
            IsNotValid("9999", country);
        }

        /// <summary>Tests patterns that should not be valid for Saint Barthélemy (BL).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_BL_All()
        {
            var country = Country.BL;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Bermuda (BM).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_BM_All()
        {
            var country = Country.BM;

            IsNotValid("A0", country);
            IsNotValid("A1", country);
            IsNotValid("B2", country);
            IsNotValid("C0", country);
            IsNotValid("D2", country);
            IsNotValid("E0", country);
            IsNotValid("6F", country);
            IsNotValid("2G", country);
            IsNotValid("6H", country);
            IsNotValid("7I", country);
            IsNotValid("7J", country);
            IsNotValid("9K", country);
            IsNotValid("0L", country);
            IsNotValid("013S", country);
            IsNotValid("12RF", country);
            IsNotValid("2034", country);
            IsNotValid("2A34", country);
        }

        /// <summary>Tests patterns that should not be valid for Brunei Darussalam (BN).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_BN_All()
        {
            var country = Country.BN;

            IsNotValid("0000DF", country);
            IsNotValid("2325DS", country);
            IsNotValid("3436RF", country);
            IsNotValid("0044WK", country);
            IsNotValid("6478SD", country);
            IsNotValid("9475PJ", country);
            IsNotValid("6450KF", country);
            IsNotValid("9088LS", country);
            IsNotValid("3495JD", country);
            IsNotValid("5502MO", country);
            IsNotValid("1832DF", country);
            IsNotValid("K999JS", country);
            IsNotValid("L000DF", country);
            IsNotValid("M325DS", country);
            IsNotValid("N436RF", country);
            IsNotValid("O044WK", country);
            IsNotValid("P478SD", country);
            IsNotValid("Q475PJ", country);
            IsNotValid("RN578F", country);
            IsNotValid("SE624S", country);
            IsNotValid("TM713D", country);
            IsNotValid("UF671O", country);
            IsNotValid("VE823F", country);
            IsNotValid("WL999S", country);
            IsNotValid("XMD000", country);
            IsNotValid("YED014", country);
            IsNotValid("ZLR128", country);
            IsNotValid("ZZW203", country);
        }

        /// <summary>Tests patterns that should not be valid for Bolivia (BO).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_BO_All()
        {
            var country = Country.BO;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Brazil (BR).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_BR_All()
        {
            var country = Country.BR;

            IsNotValid("00000000", country);
            IsNotValid("00014494", country);
            IsNotValid("00128235", country);
            IsNotValid("00203757", country);
            IsNotValid("00324343", country);
            IsNotValid("00400827", country);
            IsNotValid("00569783", country);
            IsNotValid("00628246", country);
            IsNotValid("00761134", country);
            IsNotValid("00676718", country);
            IsNotValid("00875239", country);
            IsNotValid("00999999", country);
        }

        /// <summary>Tests patterns that should not be valid for Bhutan (BT).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_BT_All()
        {
            var country = Country.BT;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Belarus (BY).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_BY_All()
        {
            var country = Country.BY;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Canada (CA).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_CA_All()
        {
            var country = Country.CA;

            // No D, F, I, O, Q, and U (and W and Z to start with).

            IsNotValid("H0D0H0", country);
            IsNotValid("A0F0D0", country);
            IsNotValid("A0I1D4", country);
            IsNotValid("B1O2R8", country);
            IsNotValid("C2Q0W3", country);
            IsNotValid("D3U2S4", country);
            IsNotValid("E4O0D0", country);
            IsNotValid("F5N6F9", country);
            IsNotValid("G6F2I8", country);
            IsNotValid("D7L6O1", country);
            IsNotValid("F6D7Q6", country);
            IsNotValid("I8S7U5", country);
            IsNotValid("O9N9J9", country);
            IsNotValid("Q0O0D0", country);
            IsNotValid("U0E1D4", country);
            IsNotValid("W1N2R8", country);
            IsNotValid("Z2L0W3", country);
        }

        /// <summary>Tests patterns that should not be valid for Cocos (CC).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_CC_All()
        {
            var country = Country.CC;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Switzerland (CH).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_CH_All()
        {
            var country = Country.CH;

            IsNotValid("0000", country);
            IsNotValid("0014", country);
            IsNotValid("0128", country);
            IsNotValid("0203", country);
            IsNotValid("0324", country);
            IsNotValid("0400", country);
            IsNotValid("0569", country);
            IsNotValid("0628", country);
            IsNotValid("0761", country);
            IsNotValid("0676", country);
            IsNotValid("0875", country);
            IsNotValid("0999", country);
        }

        /// <summary>Tests patterns that should not be valid for Chile (CL).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_CL_All()
        {
            var country = Country.CL;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for China (CN).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_CN_All()
        {
            var country = Country.CN;

            IsNotValid("0000", country);
            IsNotValid("0004", country);
            IsNotValid("0018", country);
            IsNotValid("0023", country);
            IsNotValid("0034", country);
            IsNotValid("0040", country);
            IsNotValid("0059", country);
            IsNotValid("0068", country);
            IsNotValid("0071", country);
            IsNotValid("0066", country);
            IsNotValid("0085", country);
            IsNotValid("0099", country);
        }

        /// <summary>Tests patterns that should not be valid for Colombia (CO).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_CO_All()
        {
            var country = Country.CO;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Costa Rica (CR).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_CR_All()
        {
            var country = Country.CR;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Cuba (CU).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_CU_All()
        {
            var country = Country.CU;

            IsNotValid("CP000", country);
            IsNotValid("CP035", country);
            IsNotValid("CP146", country);
            IsNotValid("CP204", country);
            IsNotValid("CP348", country);
        }

        /// <summary>Tests patterns that should not be valid for Cape Verde (CV).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_CV_All()
        {
            var country = Country.CV;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Christmas Island (CX).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_CX_All()
        {
            var country = Country.CX;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Cyprus (CY).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_CY_All()
        {
            var country = Country.CY;

            IsNotValid("0000", country);
            IsNotValid("0014", country);
            IsNotValid("0128", country);
            IsNotValid("0203", country);
            IsNotValid("0324", country);
            IsNotValid("0400", country);
            IsNotValid("0569", country);
            IsNotValid("0628", country);
            IsNotValid("0761", country);
            IsNotValid("0676", country);
            IsNotValid("0875", country);
            IsNotValid("0999", country);
        }

        /// <summary>Tests patterns that should not be valid for Czech Republic (CZ).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_CZ_All()
        {
            var country = Country.CZ;

            IsNotValid("0000", country);
            IsNotValid("0014", country);
            IsNotValid("0128", country);
            IsNotValid("0203", country);
            IsNotValid("0324", country);
            IsNotValid("0400", country);
            IsNotValid("0569", country);
            IsNotValid("0628", country);
            IsNotValid("0761", country);
            IsNotValid("0676", country);
            IsNotValid("0875", country);
            IsNotValid("0999", country);

            IsNotValid("8000", country);
            IsNotValid("8004", country);
            IsNotValid("8018", country);
            IsNotValid("8023", country);
            IsNotValid("8034", country);
            IsNotValid("8040", country);
            IsNotValid("9059", country);
            IsNotValid("9068", country);
            IsNotValid("9071", country);
            IsNotValid("9066", country);
            IsNotValid("9085", country);
            IsNotValid("9099", country);
        }

        /// <summary>Tests patterns that should not be valid for Germany (DE).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_DE_All()
        {
            var country = Country.DE;

            IsNotValid("00007", country);
            IsNotValid("00043", country);
            IsNotValid("00188", country);
            IsNotValid("00237", country);
            IsNotValid("00342", country);
            IsNotValid("00401", country);
            IsNotValid("00597", country);
            IsNotValid("00682", country);
            IsNotValid("00719", country);
            IsNotValid("00665", country);
            IsNotValid("00853", country);
            IsNotValid("00999", country);
        }

        /// <summary>Tests patterns that should not be valid for Denmark (DK).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_DK_All()
        {
            var country = Country.DK;

            IsNotValid("0000", country);
            IsNotValid("0014", country);
            IsNotValid("0128", country);
            IsNotValid("0203", country);
            IsNotValid("0324", country);
            IsNotValid("0400", country);
            IsNotValid("0569", country);
            IsNotValid("0628", country);
            IsNotValid("0761", country);
            IsNotValid("0676", country);
            IsNotValid("0875", country);
            IsNotValid("0999", country);

            IsNotValid("DK0000", country);
            IsNotValid("DK0014", country);
            IsNotValid("DK0128", country);
            IsNotValid("DK0203", country);
            IsNotValid("DK0324", country);
            IsNotValid("DK0400", country);
            IsNotValid("DK0569", country);
            IsNotValid("DK0628", country);
            IsNotValid("DK0761", country);
            IsNotValid("DK0676", country);
            IsNotValid("DK0875", country);
            IsNotValid("DK0999", country);

            IsNotValid("AA0000", country);
            IsNotValid("AS0144", country);
            IsNotValid("BJ1282", country);
            IsNotValid("CD2037", country);
            IsNotValid("DE3243", country);
            IsNotValid("EO4008", country);
            IsNotValid("FN5697", country);
            IsNotValid("GF6282", country);
            IsNotValid("HL7611", country);
            IsNotValid("ID6767", country);
            IsNotValid("JS8752", country);
            IsNotValid("KN9999", country);
            IsNotValid("LO0000", country);
            IsNotValid("ME0144", country);
            IsNotValid("NN1282", country);
        }

        /// <summary>Tests patterns that should not be valid for Algeria (DZ).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_DZ_All()
        {
            var country = Country.DZ;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Ecuador (EC).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_EC_All()
        {
            var country = Country.EC;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Estonia (EE).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_EE_All()
        {
            var country = Country.EE;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Egypt (EG).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_EG_All()
        {
            var country = Country.EG;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            //IsValid("12346", country);
            //IsValid("20004", country);
            //IsValid("32648", country);
            //IsValid("40945", country);
            //IsValid("56640", country);
            //IsValid("62908", country);
            //IsValid("76345", country);
            //IsValid("67552", country);
            //IsValid("87182", country);
            //IsValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Spain (ES).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_ES_All()
        {
            var country = Country.ES;

            IsNotValid("00000", country);
            IsNotValid("53434", country);
            IsNotValid("54082", country);
            IsNotValid("55978", country);
            IsNotValid("56824", country);
            IsNotValid("57113", country);
            IsNotValid("56671", country);
            IsNotValid("58523", country);
            IsNotValid("59999", country);
            IsNotValid("62824", country);
            IsNotValid("76113", country);
            IsNotValid("67671", country);
            IsNotValid("87523", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Ethiopia (ET).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_ET_All()
        {
            var country = Country.ET;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Finland (FI).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_FI_All()
        {
            var country = Country.FI;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Falkland Islands (FK).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_FK_All()
        {
            var country = Country.FK;

            IsNotValid("FIAA1ZZ", country);
            IsNotValid("FIAA9ZZ", country);
            IsNotValid("DN551PT", country);
            IsNotValid("DN551PT", country);
            IsNotValid("EC1A1BB", country);
            IsNotValid("EC1A1BB", country);

        }

        /// <summary>Tests patterns that should not be valid for Micronesia (FM).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_FM_All()
        {
            var country = Country.FM;

            IsNotValid("00000", country);
            IsNotValid("01234", country);
            IsNotValid("01449", country);
            IsNotValid("12823", country);
            IsNotValid("20375", country);
            IsNotValid("32434", country);
            IsNotValid("40082", country);
            IsNotValid("56978", country);
            IsNotValid("62824", country);
            IsNotValid("76113", country);
            IsNotValid("67671", country);
            IsNotValid("87523", country);
            IsNotValid("99999", country);

            IsNotValid("000000000", country);
            IsNotValid("012345678", country);
            IsNotValid("014494232", country);
            IsNotValid("128235343", country);
            IsNotValid("203757004", country);
            IsNotValid("324343647", country);
            IsNotValid("400827947", country);
            IsNotValid("569783645", country);
            IsNotValid("628246908", country);
            IsNotValid("761134349", country);
            IsNotValid("676718550", country);
            IsNotValid("875239183", country);
            IsNotValid("999999999", country);
        }

        /// <summary>Tests patterns that should not be valid for Faroe Islands (FO).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_FO_All()
        {
            var country = Country.FO;

            IsNotValid("000", country);
            IsNotValid("004", country);
            IsNotValid("018", country);
            IsNotValid("023", country);
            IsNotValid("034", country);
            IsNotValid("040", country);
            IsNotValid("059", country);
            IsNotValid("068", country);
            IsNotValid("071", country);
            IsNotValid("066", country);
            IsNotValid("085", country);
            IsNotValid("099", country);

            IsNotValid("FO000", country);
            IsNotValid("FO004", country);
            IsNotValid("FO018", country);
            IsNotValid("FO023", country);
            IsNotValid("FO034", country);
            IsNotValid("FO040", country);
            IsNotValid("FO059", country);
            IsNotValid("FO068", country);
            IsNotValid("FO071", country);
            IsNotValid("FO066", country);
            IsNotValid("FO085", country);
            IsNotValid("FO099", country);

            IsNotValid("AA000", country);
            IsNotValid("AS044", country);
            IsNotValid("BJ182", country);
            IsNotValid("CD237", country);
            IsNotValid("DE343", country);
            IsNotValid("EO408", country);
            IsNotValid("FN597", country);
            IsNotValid("GF682", country);
            IsNotValid("HL711", country);
            IsNotValid("ID667", country);
            IsNotValid("JS852", country);
            IsNotValid("KN999", country);
            IsNotValid("LO000", country);
            IsNotValid("ME044", country);
            IsNotValid("NN182", country);
        }

        /// <summary>Tests patterns that should not be valid for France (FR).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_FR_All()
        {
            var country = Country.FR;

            IsNotValid("00007", country);
            IsNotValid("00043", country);
            IsNotValid("00188", country);
            IsNotValid("00237", country);
            IsNotValid("00342", country);
            IsNotValid("00401", country);
            IsNotValid("00597", country);
            IsNotValid("00682", country);
            IsNotValid("00719", country);
            IsNotValid("00665", country);
            IsNotValid("00853", country);
            IsNotValid("00999", country);
        }

        /// <summary>Tests patterns that should not be valid for Gabon (GA).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_GA_All()
        {
            var country = Country.GA;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for United Kingdom (GB).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_GB_All()
        {
            var country = Country.GB;

            IsNotValid("311AA", country);
            IsNotValid("M113A", country);
            IsNotValid("M11A8", country);
            IsNotValid("M1BAA", country);

            IsNotValid("1338TH", country);
            IsNotValid("B338B9", country);

            IsNotValid("CRABXH", country);
            IsNotValid("CR26X9", country);

            IsNotValid("DN55PPT", country);
            IsNotValid("D1551PT", country);

            IsNotValid("WWA1HQ", country);
            IsNotValid("W1A123", country);

            IsNotValid("EC1A112", country);
            IsNotValid("EC1816B", country);
        }

        /// <summary>Tests patterns that should not be valid for Georgia (GE).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_GE_All()
        {
            var country = Country.GE;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for French Guiana (GF).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_GF_All()
        {
            var country = Country.GF;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("99999", country);

        }

        /// <summary>Tests patterns that should not be valid for Guernsey (GG).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_GG_All()
        {
            var country = Country.GG;

            IsNotValid("336R", country);
            IsNotValid("044W", country);
            IsNotValid("678S", country);
            IsNotValid("975P", country);
            IsNotValid("650K", country);
            IsNotValid("988L", country);
            IsNotValid("395J", country);

            IsNotValid("5502M", country);
            IsNotValid("1832D", country);
            IsNotValid("9999J", country);
            IsNotValid("0000D", country);
            IsNotValid("2325D", country);
            IsNotValid("3436R", country);
            IsNotValid("0044W", country);

            IsNotValid("GG336R", country);
            IsNotValid("GG044W", country);
            IsNotValid("GG678S", country);
            IsNotValid("GG975P", country);
            IsNotValid("GG650K", country);
            IsNotValid("GG988L", country);
            IsNotValid("GG395J", country);

            IsNotValid("GG5502M", country);
            IsNotValid("GG1832D", country);
            IsNotValid("GG9999J", country);
            IsNotValid("GG0000D", country);
            IsNotValid("GG2325D", country);
            IsNotValid("GG3436R", country);
            IsNotValid("GG0044W", country);

            IsNotValid("GF88LS", country);
            IsNotValid("HL95JD", country);
            IsNotValid("ID02MO", country);
            IsNotValid("JS32DF", country);
            IsNotValid("KN99JS", country);
            IsNotValid("LO00DF", country);
            IsNotValid("ME25DS", country);

            IsNotValid("AA000DF", country);
            IsNotValid("AS325DS", country);
            IsNotValid("BJ436RF", country);
            IsNotValid("CD044WK", country);
            IsNotValid("DE478SD", country);
            IsNotValid("EO475PJ", country);
            IsNotValid("FN450KF", country);
        }

        /// <summary>Tests patterns that should not be valid for Gibraltar (GI).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_GI_All()
        {
            var country = Country.GI;

            IsNotValid("DN123AA", country);
            IsNotValid("GX123BB", country);
        }

        /// <summary>Tests patterns that should not be valid for Greenland (GL).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_GL_All()
        {
            var country = Country.GL;

            IsNotValid("5502", country);
            IsNotValid("1832", country);
            IsNotValid("9999", country);
            IsNotValid("0000", country);
            IsNotValid("2325", country);
            IsNotValid("3136", country);
            IsNotValid("3236", country);
            IsNotValid("3436", country);
            IsNotValid("3436", country);
            IsNotValid("3567", country);
            IsNotValid("0044", country);

            IsNotValid("GL3365", country);
            IsNotValid("GL0448", country);
            IsNotValid("GL6789", country);
            IsNotValid("GL9750", country);
            IsNotValid("GL6503", country);
            IsNotValid("GL9881", country);
            IsNotValid("GL3852", country);
      
            IsNotValid("AA3900", country);
            IsNotValid("AS3935", country);
            IsNotValid("BJ3946", country);
            IsNotValid("CD3904", country);
            IsNotValid("DE3948", country);
            IsNotValid("EO3945", country);
            IsNotValid("FN3940", country);
        }

        /// <summary>Tests patterns that should not be valid for Guadeloupe (GP).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_GP_All()
        {
            var country = Country.GP;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Greece (GR).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_GR_All()
        {
            var country = Country.GR;

            IsNotValid("00072", country);
            IsNotValid("00433", country);
            IsNotValid("01885", country);
            IsNotValid("02374", country);
            IsNotValid("03421", country);
            IsNotValid("04014", country);
            IsNotValid("05957", country);
            IsNotValid("06862", country);
            IsNotValid("07159", country);
            IsNotValid("06685", country);
            IsNotValid("08593", country);
            IsNotValid("09999", country);
        }

        /// <summary>Tests patterns that should not be valid for South Georgia And The South Sandwich Islands (GS).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_GS_All()
        {
            var country = Country.GS;

            IsNotValid("SIQQ1AZ", country);
            IsNotValid("SIQQ9ZZ", country);
            IsNotValid("SIBB1ZZ", country);
            IsNotValid("DN551PT", country);
            IsNotValid("DN551PT", country);
            IsNotValid("EC1A1BB", country);
            IsNotValid("EC1A1BB", country);
        }

        /// <summary>Tests patterns that should not be valid for Guatemala (GT).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_GT_All()
        {
            var country = Country.GT;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Guam (GU).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_GU_All()
        {
            var country = Country.GU;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("96908", country);
            IsNotValid("96938", country);
            IsNotValid("99999", country);

            IsNotValid("000000000", country);
            IsNotValid("012301235", country);
            IsNotValid("123412346", country);
            IsNotValid("200020004", country);
            IsNotValid("326432648", country);
            IsNotValid("409440945", country);
            IsNotValid("566456640", country);
            IsNotValid("629062908", country);
            IsNotValid("763476345", country);
            IsNotValid("675567552", country);
            IsNotValid("871887182", country);
            IsNotValid("969087182", country);
            IsNotValid("969387182", country);
            IsNotValid("999999999", country);
        }

        /// <summary>Tests patterns that should not be valid for Guinea-Bissau (GW).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_GW_All()
        {
            var country = Country.GW;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Heard Island And Mcdonald Islands (HM).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_HM_All()
        {
            var country = Country.HM;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Honduras (HN).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_HN_All()
        {
            var country = Country.HN;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Croatia (HR).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_HR_All()
        {
            var country = Country.HR;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Haiti (HT).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_HT_All()
        {
            var country = Country.HT;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Hungary (HU).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_HU_All()
        {
            var country = Country.HU;

            IsNotValid("0007", country);
            IsNotValid("0043", country);
            IsNotValid("0188", country);
            IsNotValid("0237", country);
            IsNotValid("0342", country);
            IsNotValid("0401", country);
            IsNotValid("0595", country);
            IsNotValid("0686", country);
            IsNotValid("0715", country);
            IsNotValid("0668", country);
            IsNotValid("0859", country);
            IsNotValid("0999", country);
        }

        /// <summary>Tests patterns that should not be valid for Indonesia (ID).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_ID_All()
        {
            var country = Country.ID;

            IsNotValid("00072", country);
            IsNotValid("00433", country);
            IsNotValid("01885", country);
            IsNotValid("02374", country);
            IsNotValid("03421", country);
            IsNotValid("04014", country);
            IsNotValid("05957", country);
            IsNotValid("06862", country);
            IsNotValid("07159", country);
            IsNotValid("06685", country);
            IsNotValid("08593", country);
            IsNotValid("09999", country);
        }

        /// <summary>Tests patterns that should not be valid for Israel (IL).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_IL_All()
        {
            var country = Country.IL;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Isle Of Man (IM).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_IM_All()
        {
            var country = Country.IM;

            IsNotValid("IMAA00DF", country);
            IsNotValid("IMAS04DS", country);
            IsNotValid("IMBJ18RF", country);
            IsNotValid("IMCD23WK", country);
            IsNotValid("IMDE34SD", country);
            IsNotValid("IMEO40PJ", country);
            IsNotValid("IMFN59KF", country);
            IsNotValid("IMGF68LS", country);
            IsNotValid("IMHL71JD", country);
            IsNotValid("IMID66MO", country);
            IsNotValid("IMJS85DF", country);
            IsNotValid("IMKN99JS", country);
            IsNotValid("IMLO00DF", country);

            IsNotValid("AA000DF", country);
            IsNotValid("AS014DS", country);
            IsNotValid("BJ128RF", country);
            IsNotValid("CD203WK", country);
            IsNotValid("DE324SD", country);
            IsNotValid("EO400PJ", country);
            IsNotValid("FN569KF", country);
            IsNotValid("GF628LS", country);
            IsNotValid("HL761JD", country);
            IsNotValid("ID676MO", country);
            IsNotValid("JS875DF", country);
            IsNotValid("KN999JS", country);
            IsNotValid("LO000DF", country);
        }

        /// <summary>Tests patterns that should not be valid for India (IN).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_IN_All()
        {
            var country = Country.IN;

            IsNotValid("00000", country);
            IsNotValid("00149", country);
            IsNotValid("01283", country);
            IsNotValid("02035", country);
            IsNotValid("03244", country);
            IsNotValid("04002", country);
            IsNotValid("05698", country);
            IsNotValid("06284", country);
            IsNotValid("07613", country);
            IsNotValid("06761", country);
            IsNotValid("08753", country);
            IsNotValid("09999", country);
        }

        /// <summary>Tests patterns that should not be valid for British Indian Ocean Territory (IO).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_IO_All()
        {
            var country = Country.IO;

            IsNotValid("AADF0DF", country);
            IsNotValid("ASDS0DS", country);
            IsNotValid("BJRF1RF", country);
            IsNotValid("CDWK2WK", country);
            IsNotValid("DESD3SD", country);
            IsNotValid("EOPJ4PJ", country);
            IsNotValid("FNKF5KF", country);
            IsNotValid("GFLS6LS", country);
            IsNotValid("HLJD7JD", country);
            IsNotValid("IDMO6MO", country);
            IsNotValid("JSDF8DF", country);
            IsNotValid("KNJS9JS", country);
        }

        /// <summary>Tests patterns that should not be valid for Iraq (IQ).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_IQ_All()
        {
            var country = Country.IQ;

            IsNotValid("00000", country);
            IsNotValid("20004", country);
            IsNotValid("76345", country);
            IsNotValid("87182", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Iran (IR).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_IR_All()
        {
            var country = Country.IR;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Iceland (IS).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_IS_All()
        {
            var country = Country.IS;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Italy (IT).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_IT_All()
        {
            var country = Country.IT;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Jersey (JE).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_JE_All()
        {
            var country = Country.JE;

            IsNotValid("0", country);

            IsNotValid("A00S", country);
            IsNotValid("G25S", country);
            IsNotValid("D36F", country);
            IsNotValid("D44S", country);
            IsNotValid("R78F", country);
            IsNotValid("W75K", country);
            IsNotValid("5D0S", country);
            IsNotValid("8J8P", country);
            IsNotValid("9F5K", country);
            IsNotValid("0LS2", country);
            IsNotValid("3JD2", country);
            IsNotValid("9MO9", country);
            IsNotValid("0AS0", country);
            IsNotValid("G042S", country);
            IsNotValid("D153F", country);
            IsNotValid("D274S", country);
            IsNotValid("3R37F", country);
            IsNotValid("4W77K", country);
            IsNotValid("5S35D", country);
            IsNotValid("66PJ8", country);
            IsNotValid("74KF9", country);
            IsNotValid("6S80L", country);
            IsNotValid("8D93J", country);
            IsNotValid("9O99M", country);
        }

        /// <summary>Tests patterns that should not be valid for Jordan (JO).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_JO_All()
        {
            var country = Country.JO;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Japan (JP).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_JP_All()
        {
            var country = Country.JP;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Kyrgyzstan (KG).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_KG_All()
        {
            var country = Country.KG;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Cambodia (KH).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_KH_All()
        {
            var country = Country.KH;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Korea (KR).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_KR_All()
        {
            var country = Country.KR;

            IsNotValid("000000", country);
            IsNotValid("023456", country);
            IsNotValid("012823", country);
            IsNotValid("020375", country);
            IsNotValid("032434", country);
            IsNotValid("040082", country);
            IsNotValid("056978", country);
            IsNotValid("862824", country);
            IsNotValid("861134", country);
            IsNotValid("876718", country);
            IsNotValid("975239", country);
            IsNotValid("999999", country);
        }

        /// <summary>Tests patterns that should not be valid for Cayman Islands (KY).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_KY_All()
        {
            var country = Country.KY;

            IsNotValid("AA00000", country);
            IsNotValid("AS01449", country);
            IsNotValid("BJ12823", country);
            IsNotValid("CD20375", country);
            IsNotValid("DE32434", country);
            IsNotValid("EO40082", country);
            IsNotValid("FN56978", country);
            IsNotValid("GF62824", country);
            IsNotValid("HL76113", country);
            IsNotValid("ID67671", country);
            IsNotValid("JS87523", country);
            IsNotValid("KN99999", country);
            IsNotValid("LO00000", country);
            IsNotValid("ME01449", country);
            IsNotValid("NN12823", country);
            IsNotValid("OL20375", country);
            IsNotValid("PS32434", country);
            IsNotValid("QD40082", country);
            IsNotValid("RN56978", country);
            IsNotValid("SE62824", country);
            IsNotValid("TM76113", country);
            IsNotValid("UF67671", country);
            IsNotValid("VE87523", country);
            IsNotValid("WL99999", country);
            IsNotValid("XM00000", country);
            IsNotValid("YE01449", country);
            IsNotValid("ZL12823", country);
            IsNotValid("ZZ20375", country);
        }

        /// <summary>Tests patterns that should not be valid for Kazakhstan (KZ).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_KZ_All()
        {
            var country = Country.KZ;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Lao People'S Democratic Republic (LA).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_LA_All()
        {
            var country = Country.LA;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Lebanon (LB).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_LB_All()
        {
            var country = Country.LB;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Liechtenstein (LI).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_LI_All()
        {
            var country = Country.LI;

            IsNotValid("0000", country); 
            IsNotValid("0144", country); 
            IsNotValid("1282", country); 
            IsNotValid("2037", country); 
            IsNotValid("3243", country); 
            IsNotValid("4008", country); 
            IsNotValid("5697", country); 
            IsNotValid("6282", country); 
            IsNotValid("7611", country); 
            IsNotValid("6767", country); 
            IsNotValid("8752", country); 
            IsNotValid("9999", country); 

            IsNotValid("9300", country);
            IsNotValid("9499", country);
            IsNotValid("9593", country);
            IsNotValid("9679", country);
            IsNotValid("9480", country);
            IsNotValid("9481", country);
            IsNotValid("9482", country);
            IsNotValid("9483", country);
            IsNotValid("9484", country);
            IsNotValid("9499", country);
        }

        /// <summary>Tests patterns that should not be valid for Sri Lanka (LK).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_LK_All()
        {
            var country = Country.LK;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Liberia (LR).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_LR_All()
        {
            var country = Country.LR;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Lesotho (LS).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_LS_All()
        {
            var country = Country.LS;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Lithuania (LT).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_LT_All()
        {
            var country = Country.LT;

            IsNotValid("AA00000", country);
            IsNotValid("AS01449", country);
            IsNotValid("BJ12823", country);
            IsNotValid("CD20375", country);
            IsNotValid("DE32434", country);
            IsNotValid("EO40082", country);
            IsNotValid("FN56978", country);
            IsNotValid("GF62824", country);
            IsNotValid("HL76113", country);
            IsNotValid("ID67671", country);
            IsNotValid("JS87523", country);
            IsNotValid("KN99999", country);
            IsNotValid("LO00000", country);
            IsNotValid("ME01449", country);
            IsNotValid("NN12823", country);
            IsNotValid("OL20375", country);
            IsNotValid("PS32434", country);
            IsNotValid("QD40082", country);
            IsNotValid("RN56978", country);
            IsNotValid("SE62824", country);
            IsNotValid("TM76113", country);
            IsNotValid("UF67671", country);
            IsNotValid("VE87523", country);
            IsNotValid("WL99999", country);
            IsNotValid("XM00000", country);
            IsNotValid("YE01449", country);
            IsNotValid("ZL12823", country);
            IsNotValid("ZZ20375", country);
        }

        /// <summary>Tests patterns that should not be valid for Luxembourg (LU).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_LU_All()
        {
            var country = Country.LU;

            IsNotValid("AA0000", country);
            IsNotValid("AS0144", country);
            IsNotValid("BJ1282", country);
            IsNotValid("CD2037", country);
            IsNotValid("DE3243", country);
            IsNotValid("EO4008", country);
            IsNotValid("FN5697", country);
            IsNotValid("GF6282", country);
            IsNotValid("HL7611", country);
            IsNotValid("ID6767", country);
            IsNotValid("JS8752", country);
            IsNotValid("KN9999", country);
            IsNotValid("LO0000", country);
            IsNotValid("ME0144", country);
            IsNotValid("NN1282", country);
            IsNotValid("OL2037", country);
            IsNotValid("PS3243", country);
        }

        /// <summary>Tests patterns that should not be valid for Latvia (LV).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_LV_All()
        {
            var country = Country.KY;

            IsNotValid("AA0000", country);
            IsNotValid("AS0449", country);
            IsNotValid("BJ1823", country);
            IsNotValid("CD2375", country);
            IsNotValid("DE3434", country);
            IsNotValid("EO4082", country);
            IsNotValid("FN5978", country);
            IsNotValid("GF6824", country);
            IsNotValid("HL7113", country);
            IsNotValid("ID6671", country);
            IsNotValid("JS8523", country);
            IsNotValid("KN9999", country);
            IsNotValid("LO0000", country);
            IsNotValid("ME0449", country);
            IsNotValid("NN1823", country);
            IsNotValid("OL2375", country);
            IsNotValid("PS3434", country);
            IsNotValid("QD4082", country);
            IsNotValid("RN5978", country);
            IsNotValid("SE6824", country);
            IsNotValid("TM7113", country);
            IsNotValid("UF6671", country);
            IsNotValid("VE8523", country);
            IsNotValid("WL9999", country);
            IsNotValid("XM0000", country);
            IsNotValid("YE0449", country);
            IsNotValid("ZL1823", country);
            IsNotValid("ZZ2375", country);
        }

        /// <summary>Tests patterns that should not be valid for Libya (LY).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_LY_All()
        {
            var country = Country.LY;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Morocco (MA).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_MA_All()
        {
            var country = Country.MA;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Monaco (MC).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_MC_All()
        {
            var country = Country.MC;

            IsNotValid("00000", country);
            IsNotValid("04249", country);
            IsNotValid("18323", country);
            IsNotValid("23475", country);
            IsNotValid("34734", country);
            IsNotValid("40782", country);
            IsNotValid("59578", country);
            IsNotValid("68824", country);
            IsNotValid("71913", country);
            IsNotValid("66071", country);
            IsNotValid("98123", country);
            IsNotValid("98299", country);
            IsNotValid("98344", country);
            IsNotValid("98402", country);
            IsNotValid("98598", country);
            IsNotValid("98684", country);
            IsNotValid("98713", country);
            IsNotValid("98661", country);
            IsNotValid("98983", country);
            IsNotValid("98989", country);
            IsNotValid("00000", country);

            IsNotValid("MC00000", country);
            IsNotValid("MC04249", country);
            IsNotValid("MC18323", country);
            IsNotValid("MC23475", country);
            IsNotValid("MC34734", country);
            IsNotValid("MC40782", country);
            IsNotValid("MC59578", country);
            IsNotValid("MC68824", country);
            IsNotValid("MC71913", country);
            IsNotValid("MC66071", country);
            IsNotValid("MC85323", country);
            IsNotValid("MC99999", country);
            IsNotValid("MC00000", country);

            IsNotValid("AA98000", country);
            IsNotValid("AS98049", country);
            IsNotValid("BJ98023", country);
            IsNotValid("CD98075", country);
            IsNotValid("DE98034", country);
            IsNotValid("EO98082", country);
            IsNotValid("FN98078", country);
            IsNotValid("GF98024", country);
            IsNotValid("HL98013", country);
            IsNotValid("ID98071", country);
            IsNotValid("JS98023", country);
            IsNotValid("KN98099", country);
            IsNotValid("LO98000", country);
            IsNotValid("ME98049", country);
            IsNotValid("NN98023", country);
            IsNotValid("OL98075", country);
            IsNotValid("PS98034", country);
            IsNotValid("QD98082", country);
            IsNotValid("RN98078", country);
            IsNotValid("SE98024", country);
            IsNotValid("TM98013", country);
            IsNotValid("UF98071", country);
            IsNotValid("VE98023", country);
            IsNotValid("WL98099", country);
            IsNotValid("XM98000", country);
            IsNotValid("YE98049", country);
            IsNotValid("ZL98023", country);
            IsNotValid("ZZ98075", country);
        }

        /// <summary>Tests patterns that should not be valid for Moldova (MD).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_MD_All()
        {
            var country = Country.MD;

            IsNotValid("AA0000", country);
            IsNotValid("AS0144", country);
            IsNotValid("BJ1282", country);
            IsNotValid("CD2037", country);
            IsNotValid("DE3243", country);
            IsNotValid("EO4008", country);
            IsNotValid("FN5697", country);
            IsNotValid("GF6282", country);
            IsNotValid("HL7611", country);
            IsNotValid("ID6767", country);
            IsNotValid("JS8752", country);
            IsNotValid("KN9999", country);
            IsNotValid("LO0000", country);
            IsNotValid("ME0144", country);
            IsNotValid("NN1282", country);
            IsNotValid("OL2037", country);
            IsNotValid("PS3243", country);
        }

        /// <summary>Tests patterns that should not be valid for Montenegro (ME).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_ME_All()
        {
            var country = Country.MD;

            IsNotValid("00000", country);
            IsNotValid("01449", country);
            IsNotValid("12823", country);
            IsNotValid("20375", country);
            IsNotValid("32434", country);
            IsNotValid("40082", country);
            IsNotValid("56978", country);
            IsNotValid("62824", country);
            IsNotValid("76113", country);
            IsNotValid("67671", country);
            IsNotValid("87523", country);
            IsNotValid("99999", country);

            IsNotValid("80000", country);
            IsNotValid("80149", country);
            IsNotValid("82035", country);
            IsNotValid("83244", country);
            IsNotValid("86284", country);
            IsNotValid("87613", country);
            IsNotValid("86761", country);
            IsNotValid("88753", country);
            IsNotValid("89999", country);
        }

        /// <summary>Tests patterns that should not be valid for Saint Martin (MF).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_MF_All()
        {
            var country = Country.MF;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Madagascar (MG).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_MG_All()
        {
            var country = Country.MG;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Marshall Islands (MH).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_MH_All()
        {
            var country = Country.MH;

            IsNotValid("06000", country);
            IsNotValid("00425", country);
            IsNotValid("11836", country);
            IsNotValid("22304", country);
            IsNotValid("33468", country);
            IsNotValid("44095", country);
            IsNotValid("55960", country);
            IsNotValid("66898", country);
            IsNotValid("77135", country);
            IsNotValid("66652", country);
            IsNotValid("88512", country);
            IsNotValid("99999", country);
            IsNotValid("96932", country);
            IsNotValid("96951", country);
            IsNotValid("96989", country);
            IsNotValid("00000", country);

            IsNotValid("000000000", country);
            IsNotValid("012345678", country);
            IsNotValid("128253436", country);
            IsNotValid("203770044", country);
            IsNotValid("324336478", country);
            IsNotValid("400879475", country);
            IsNotValid("569736450", country);
            IsNotValid("628269088", country);
            IsNotValid("761143495", country);
            IsNotValid("676785502", country);
            IsNotValid("875291832", country);
            IsNotValid("999999999", country);
            IsNotValid("000000000", country);
        }

        /// <summary>Tests patterns that should not be valid for Macedonia (MK).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_MK_All()
        {
            var country = Country.MK;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Myanmar (MM).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_MM_All()
        {
            var country = Country.MM;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Mongolia (MN).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_MN_All()
        {
            var country = Country.MN;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Northern Mariana Islands (MP).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_MP_All()
        {
            var country = Country.MP;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("99999", country);
            IsNotValid("96949", country);
            IsNotValid("96953", country);
            IsNotValid("96954", country);
            IsNotValid("000000000", country);
            IsNotValid("012354423", country);
            IsNotValid("123462534", country);
            IsNotValid("200047700", country);
            IsNotValid("326483364", country);
            IsNotValid("409458794", country);
            IsNotValid("566407364", country);
            IsNotValid("629082690", country);
            IsNotValid("763451434", country);
            IsNotValid("675527855", country);
            IsNotValid("871822918", country);
            IsNotValid("969496831", country);
            IsNotValid("969535348", country);
            IsNotValid("969545607", country);
            IsNotValid("999999999", country);
        }

        /// <summary>Tests patterns that should not be valid for Martinique (MQ).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_MQ_All()
        {
            var country = Country.MQ;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Malta (MT).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_MT_All()
        {
            var country = Country.MT;

            IsNotValid("AA00000", country);
            IsNotValid("ASD01D2", country);
        }

        /// <summary>Tests patterns that should not be valid for Mexico (MX).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_MX_All()
        {
            var country = Country.MX;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Malaysia (MY).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_MY_All()
        {
            var country = Country.MY;

            IsNotValid("00000", country);
            IsNotValid("00035", country);
            IsNotValid("00146", country);
            IsNotValid("00204", country);
            IsNotValid("00348", country);
            IsNotValid("00445", country);
            IsNotValid("00540", country);
            IsNotValid("00608", country);
            IsNotValid("00745", country);
            IsNotValid("00652", country);
            IsNotValid("00882", country);
            IsNotValid("00999", country);
        }

        /// <summary>Tests patterns that should not be valid for Mozambique (MZ).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_MZ_All()
        {
            var country = Country.MZ;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Namibia (NA).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_NA_All()
        {
            var country = Country.NA;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("89999", country);
            IsNotValid("93000", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for New Caledonia (NC).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_NC_All()
        {
            var country = Country.NC;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("99999", country);

            IsNotValid("98600", country);
            IsNotValid("98535", country);
            IsNotValid("98346", country);
            IsNotValid("98204", country);
            IsNotValid("98648", country);
            IsNotValid("98545", country);
            IsNotValid("98140", country);
            IsNotValid("98108", country);
            IsNotValid("99045", country);
            IsNotValid("99052", country);
            IsNotValid("97982", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Niger (NE).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_NE_All()
        {
            var country = Country.NE;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Norfolk Island (NF).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_NF_All()
        {
            var country = Country.NF;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Nigeria (NG).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_NG_All()
        {
            var country = Country.NG;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Nicaragua (NI).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_NI_All()
        {
            var country = Country.NI;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Netherlands (NL).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_NL_All()
        {
            var country = Country.NL;

            IsNotValid("0000DF", country);
            IsNotValid("0125DS", country);
            IsNotValid("3278SA", country);
            IsNotValid("8732SD", country);
            IsNotValid("9999SS", country);
        }

        /// <summary>Tests patterns that should not be valid for Norway (NO).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_NO_All()
        {
            var country = Country.NO;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Nepal (NP).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_NP_All()
        {
            var country = Country.NP;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for New Zealand (NZ).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_NZ_All()
        {
            var country = Country.NZ;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Oman (OM).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_OM_All()
        {
            var country = Country.OM;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Panama (PA).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_PA_All()
        {
            var country = Country.PA;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Peru (PE).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_PE_All()
        {
            var country = Country.PE;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for French Polynesia (PF).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_PF_All()
        {
            var country = Country.PF;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("98524", country);
            IsNotValid("98600", country);
            IsNotValid("98805", country);
            IsNotValid("98916", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Papua New Guinea (PG).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_PG_All()
        {
            var country = Country.PG;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Philippines (PH).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_PH_All()
        {
            var country = Country.PH;

            IsNotValid("0000", country);
            IsNotValid("0003", country);
            IsNotValid("0014", country);
            IsNotValid("0020", country);
            IsNotValid("0034", country);
            IsNotValid("0044", country);
            IsNotValid("0054", country);
            IsNotValid("0060", country);
            IsNotValid("0074", country);
            IsNotValid("0065", country);
            IsNotValid("0088", country);
            IsNotValid("0099", country);
        }

        /// <summary>Tests patterns that should not be valid for Pakistan (PK).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_PK_All()
        {
            var country = Country.PK;

            IsNotValid("00000", country);
            IsNotValid("00125", country);
            IsNotValid("01236", country);
            IsNotValid("02004", country);
            IsNotValid("03268", country);
            IsNotValid("04095", country);
            IsNotValid("05660", country);
            IsNotValid("06298", country);
            IsNotValid("07635", country);
            IsNotValid("06752", country);
            IsNotValid("08712", country);
            IsNotValid("09854", country);
            IsNotValid("09860", country);
            IsNotValid("09885", country);
            IsNotValid("09896", country);
            IsNotValid("09999", country);
        }

        /// <summary>Tests patterns that should not be valid for Poland (PL).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_PL_All()
        {
            var country = Country.PL;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Saint Pierre And Miquelon (PM).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_PM_All()
        {
            var country = Country.PM;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("97000", country);
            IsNotValid("97101", country);
            IsNotValid("97212", country);
            IsNotValid("97320", country);
            IsNotValid("97432", country);
            IsNotValid("97640", country);
            IsNotValid("97756", country);
            IsNotValid("97862", country);
            IsNotValid("97976", country);
            IsNotValid("99999", country);
            IsNotValid("97502", country);
            IsNotValid("97513", country);
            IsNotValid("97520", country);
            IsNotValid("97536", country);
            IsNotValid("97549", country);
            IsNotValid("97556", country);
            IsNotValid("97569", country);
            IsNotValid("97573", country);
            IsNotValid("97565", country);
            IsNotValid("97581", country);
            IsNotValid("97599", country);
        }

        /// <summary>Tests patterns that should not be valid for Pitcairn (PN).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_PN_All()
        {
            var country = Country.PN;

            IsNotValid("PCRN2ZZ", country);
            IsNotValid("AADF0FD", country);
            IsNotValid("ASDS0KD", country);
            IsNotValid("BJRF1DR", country);
            IsNotValid("CDWK2JW", country);
            IsNotValid("DESD3FS", country);
            IsNotValid("EOPJ4SP", country);
            IsNotValid("FNKF5DK", country);
            IsNotValid("GFLS6OL", country);
            IsNotValid("HLJD7FJ", country);
            IsNotValid("IDMO6SM", country);
            IsNotValid("JSDF8FD", country);
            IsNotValid("KNJS9SJ", country);
            IsNotValid("LODF0FD", country);
            IsNotValid("MEDS0KD", country);
            IsNotValid("NNRF1RF", country);
            IsNotValid("OLWK2WS", country);
            IsNotValid("PSSD3SF", country);
            IsNotValid("QDPJ4PK", country);
            IsNotValid("RNKF5KD", country);
            IsNotValid("SELS6LJ", country);
            IsNotValid("TMJD7JF", country);
            IsNotValid("UFMO6MS", country);
            IsNotValid("VEDF8DD", country);
            IsNotValid("WLJS9JO", country);
            IsNotValid("XMDF0DF", country);
            IsNotValid("YEDS0DS", country);
            IsNotValid("ZLRF1RF", country);
            IsNotValid("ZZWK2WS", country);
        }

        /// <summary>Tests patterns that should not be valid for Puerto Rico (PR).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_PR_All()
        {
            var country = Country.PR;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Palestinian Territory (PS).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_PS_All()
        {
            var country = Country.PS;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Portugal (PT).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_PT_All()
        {
                 var country = Country.PT;
                 
                 IsNotValid("0000000", country); 
                 IsNotValid("0014494", country); 
                 IsNotValid("0123456", country); 
                 IsNotValid("0203757", country); 
                 IsNotValid("0324343", country); 
                 IsNotValid("0400827", country); 
                 IsNotValid("0569783", country); 
                 IsNotValid("0628246", country); 
                 IsNotValid("0761134", country); 
                 IsNotValid("0676718", country); 
                 IsNotValid("0875239", country); 
                 IsNotValid("0999999", country); 
        }

        /// <summary>Tests patterns that should not be valid for Palau (PW).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_PW_All()
        {
            var country = Country.PW;

            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("96939", country);
            IsNotValid("96941", country);
            IsNotValid("96948", country);
            IsNotValid("96952", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Paraguay (PY).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_PY_All()
        {
            var country = Country.PY;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Réunion (RE).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_RE_All()
        {
            var country = Country.RE;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("97000", country);
            IsNotValid("97102", country);
            IsNotValid("97213", country);
            IsNotValid("97320", country);
            IsNotValid("97536", country);
            IsNotValid("97649", country);
            IsNotValid("97756", country);
            IsNotValid("97869", country);
            IsNotValid("97973", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Romania (RO).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_RO_All()
        {
            var country = Country.RO;

            IsNotValid("00000", country);
            IsNotValid("00012", country);
            IsNotValid("00123", country);
            IsNotValid("00200", country);
            IsNotValid("00326", country);
            IsNotValid("00409", country);
            IsNotValid("00566", country);
            IsNotValid("00629", country);
            IsNotValid("00763", country);
            IsNotValid("00675", country);
            IsNotValid("00871", country);
            IsNotValid("00970", country);
            IsNotValid("00999", country);
        }

        /// <summary>Tests patterns that should not be valid for Serbia (RS).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_RS_All()
        {
            var country = Country.RS;

            IsNotValid("000000", country); 
            IsNotValid("012345", country); 
            IsNotValid("400827", country); 
            IsNotValid("569783", country); 
            IsNotValid("628246", country); 
            IsNotValid("761134", country); 
            IsNotValid("676718", country); 
            IsNotValid("875239", country); 
            IsNotValid("999999", country); 
        }

        /// <summary>Tests patterns that should not be valid for Russian Federation (RU).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_RU_All()
        {
            var country = Country.RU;

            IsNotValid("0000000", country);
            IsNotValid("0012345", country);
            IsNotValid("0128235", country);
            IsNotValid("0203757", country);
            IsNotValid("0324343", country);
            IsNotValid("0400827", country);
            IsNotValid("0569783", country);
            IsNotValid("0628246", country);
            IsNotValid("0761134", country);
            IsNotValid("0676718", country);
            IsNotValid("0875239", country);
            IsNotValid("0999999", country);
        }

        /// <summary>Tests patterns that should not be valid for Saudi Arabia (SA).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_SA_All()
        {
            var country = Country.SA;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Sudan (SD).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_SD_All()
        {
            var country = Country.SD;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Sweden (SE).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_SE_All()
        {
            var country = Country.SE;

            IsNotValid("000000", country);
            IsNotValid("001235", country);
            IsNotValid("012825", country);
            IsNotValid("020377", country);
            IsNotValid("032433", country);
            IsNotValid("040087", country);
            IsNotValid("056973", country);
            IsNotValid("062826", country);
            IsNotValid("076114", country);
            IsNotValid("067678", country);
            IsNotValid("087529", country);
            IsNotValid("099999", country);
        }

        /// <summary>Tests patterns that should not be valid for Singapore (SG).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_SG_All()
        {
            var country = Country.SG;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Saint Helena (SH).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_SH_All()
        {
            var country = Country.SH;

            IsNotValid("SAHL1ZZ", country);
            IsNotValid("STBL1AZ", country);
            IsNotValid("STHL2ZZ", country);
            IsNotValid("AADF0DF", country);
            IsNotValid("ASDS2DS", country);
            IsNotValid("BJRF3RF", country);
            IsNotValid("CDWK4WK", country);
            IsNotValid("DESD7SD", country);
            IsNotValid("EOPJ7PJ", country);
            IsNotValid("FNKF5KF", country);
            IsNotValid("GFLS8LS", country);
            IsNotValid("HLJD9JD", country);
            IsNotValid("IDMO0MO", country);
            IsNotValid("JSDF3DF", country);
            IsNotValid("KNJS9JS", country);
            IsNotValid("LODF0DF", country);
            IsNotValid("MEDS2DS", country);
            IsNotValid("NNRF3RF", country);
            IsNotValid("OLWK4WK", country);
            IsNotValid("PSSD7SD", country);
            IsNotValid("QDPJ7PJ", country);
            IsNotValid("RNKF5KF", country);
            IsNotValid("SELS8LS", country);
            IsNotValid("TMJD9JD", country);
            IsNotValid("UFMO0MO", country);
            IsNotValid("VEDF3DF", country);
            IsNotValid("WLJS9JS", country);
            IsNotValid("XMDF0DF", country);
            IsNotValid("YEDS2DS", country);
            IsNotValid("ZLRF3RF", country);
            IsNotValid("ZZWK4WK", country);
        }

        /// <summary>Tests patterns that should not be valid for Slovenia (SI).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_SI_All()
        {
            var country = Country.SI;

            IsNotValid("AA0000", country);
            IsNotValid("AS0144", country);
            IsNotValid("BJ1282", country);
            IsNotValid("CD2037", country);
            IsNotValid("DE3243", country);
            IsNotValid("EO4008", country);
            IsNotValid("FN5697", country);
            IsNotValid("GF6282", country);
            IsNotValid("HL7611", country);
            IsNotValid("ID6767", country);
            IsNotValid("JS8752", country);
            IsNotValid("KN9999", country);
            IsNotValid("LO0000", country);
            IsNotValid("ME0144", country);
            IsNotValid("NN1282", country);
            IsNotValid("OL2037", country);
            IsNotValid("PS3243", country);
            IsNotValid("QD4008", country);
            IsNotValid("RN5697", country);
            IsNotValid("SE6282", country);
            IsNotValid("TM7611", country);
            IsNotValid("UF6767", country);
            IsNotValid("VE8752", country);
            IsNotValid("WL9999", country);
            IsNotValid("XM0000", country);
            IsNotValid("YE0144", country);
            IsNotValid("ZL1282", country);
            IsNotValid("ZZ2037", country);
        }

        /// <summary>Tests patterns that should not be valid for Slovakia (SK).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_SK_All()
        {
            var country = Country.SK;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for San Marino (SM).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_SM_All()
        {
            var country = Country.SM;

            IsNotValid("00000", country);
            IsNotValid("05125", country);
            IsNotValid("16285", country);
            IsNotValid("27037", country);
            IsNotValid("36243", country);
            IsNotValid("46890", country);
            IsNotValid("47797", country);
            IsNotValid("59693", country);
            IsNotValid("66286", country);
            IsNotValid("76614", country);
            IsNotValid("66768", country);
            IsNotValid("83759", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Senegal (SN).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_SN_All()
        {
            var country = Country.SN;

            IsNotValid("AA00000", country);
            IsNotValid("AB01234", country);
            IsNotValid("BJ12823", country);
            IsNotValid("CO20375", country);
            IsNotValid("DE32434", country);
            IsNotValid("EO40082", country);
            IsNotValid("FN56978", country);
            IsNotValid("GF62824", country);
            IsNotValid("HL76113", country);
            IsNotValid("ID67671", country);
            IsNotValid("JS87523", country);
            IsNotValid("KN99999", country);
            IsNotValid("LO00000", country);
            IsNotValid("ME01449", country);
            IsNotValid("NN12823", country);
            IsNotValid("OL20375", country);
            IsNotValid("PS32434", country);
            IsNotValid("QD40082", country);
            IsNotValid("RN56978", country);
            IsNotValid("SE62824", country);
            IsNotValid("TM76113", country);
            IsNotValid("UF67671", country);
            IsNotValid("VE87523", country);
            IsNotValid("WL99999", country);
            IsNotValid("XM00000", country);
            IsNotValid("YE01449", country);
            IsNotValid("ZL12823", country);
            IsNotValid("ZZ20375", country);
        }

        /// <summary>Tests patterns that should not be valid for El Salvador (SV).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_SV_All()
        {
            var country = Country.SV;

            IsNotValid("00000", country);
            IsNotValid("01001", country);
            IsNotValid("01131", country);
            IsNotValid("02131", country);
            IsNotValid("02331", country);
            IsNotValid("12234", country);
            IsNotValid("27000", country);
            IsNotValid("33248", country);
            IsNotValid("48945", country);
            IsNotValid("57640", country);
            IsNotValid("62208", country);
            IsNotValid("71645", country);
            IsNotValid("67752", country);
            IsNotValid("82782", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Swaziland (SZ).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_SZ_All()
        {
            var country = Country.SZ;

            IsNotValid("A000", country);
            IsNotValid("A014", country);
            IsNotValid("B128", country);
            IsNotValid("C203", country);
            IsNotValid("D324", country);
            IsNotValid("E400", country);
            IsNotValid("F569", country);
            IsNotValid("G628", country);
            IsNotValid("I676", country);
            IsNotValid("J875", country);
            IsNotValid("K999", country);
            IsNotValid("N128", country);
            IsNotValid("O203", country);
            IsNotValid("P324", country);
            IsNotValid("Q400", country);
            IsNotValid("R569", country);
            IsNotValid("T761", country);
            IsNotValid("U676", country);
            IsNotValid("V875", country);
            IsNotValid("W999", country);
            IsNotValid("X000", country);
            IsNotValid("Y014", country);
            IsNotValid("Z128", country);
            IsNotValid("Z999", country);

            IsNotValid("A00Z", country);
            IsNotValid("0A14", country);
            IsNotValid("1B28", country);
            IsNotValid("2C03", country);
            IsNotValid("3D24", country);
            IsNotValid("E40A", country);
            IsNotValid("F5B9", country);
            IsNotValid("G6BB", country);
            IsNotValid("H7A1", country);
            IsNotValid("I6A6", country);
            IsNotValid("875J", country);
            IsNotValid("999K", country);
            IsNotValid("000L", country);
            IsNotValid("014M", country);
            IsNotValid("128N", country);
            IsNotValid("203O", country);
            IsNotValid("324P", country);
            IsNotValid("Q4J0", country);
            IsNotValid("R5K6", country);
            IsNotValid("S6L2", country);
            IsNotValid("T7M6", country);
            IsNotValid("U6N7", country);
            IsNotValid("V8O7", country);
        }

        /// <summary>Tests patterns that should not be valid for Turks And Caicos Islands (TC).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_TC_All()
        {
            var country = Country.TC;

            IsNotValid("AKCA1ZZ", country);
            IsNotValid("TBCA1ZZ", country);
            IsNotValid("TKDA1ZZ", country);
            IsNotValid("TKCE1ZZ", country);
            IsNotValid("TKCA9ZZ", country);
            IsNotValid("TKCA1FZ", country);
            IsNotValid("TKCA1ZG", country);

            IsNotValid("AADF0DF", country);
            IsNotValid("ASDS0DS", country);
            IsNotValid("BJRF1RF", country);
            IsNotValid("CDWK2WK", country);
            IsNotValid("DESD3SD", country);
            IsNotValid("EOPJ4PJ", country);
            IsNotValid("FNKF5KF", country);
            IsNotValid("GFLS6LS", country);
            IsNotValid("HLJD7JD", country);
            IsNotValid("IDMO6MO", country);
            IsNotValid("JSDF8DF", country);
            IsNotValid("KNJS9JS", country);
            IsNotValid("LODF0DF", country);
            IsNotValid("MEDS0DS", country);
            IsNotValid("NNRF1RF", country);
            IsNotValid("OLWK2WK", country);
            IsNotValid("PSSD3SD", country);
            IsNotValid("QDPJ4PJ", country);
            IsNotValid("RNKF5KF", country);
            IsNotValid("SELS6LS", country);
            IsNotValid("TMJD7JD", country);
            IsNotValid("UFMO6MO", country);
            IsNotValid("VEDF8DF", country);
            IsNotValid("WLJS9JS", country);
            IsNotValid("XMDF0DF", country);
            IsNotValid("YEDS0DS", country);
            IsNotValid("ZLRF1RF", country);
            IsNotValid("ZZWK2WK", country);
        }

        /// <summary>Tests patterns that should not be valid for Chad (TD).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_TD_All()
        {
            var country = Country.TD;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Thailand (TH).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_TH_All()
        {
            var country = Country.TH;

            IsNotValid("00000", country);
            IsNotValid("00124", country);
            IsNotValid("01283", country);
            IsNotValid("02035", country);
            IsNotValid("03244", country);
            IsNotValid("04002", country);
            IsNotValid("05698", country);
            IsNotValid("06284", country);
            IsNotValid("07613", country);
            IsNotValid("06761", country);
            IsNotValid("08753", country);
            IsNotValid("09999", country);
        }

        /// <summary>Tests patterns that should not be valid for Tajikistan (TJ).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_TJ_All()
        {
            var country = Country.TJ;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Turkmenistan (TM).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_TM_All()
        {
            var country = Country.TM;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Tunisia (TN).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_TN_All()
        {
            var country = Country.TN;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Turkey (TR).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_TR_All()
        {
            var country = Country.TR;

            IsNotValid("00000", country);
            IsNotValid("00012", country);
            IsNotValid("00128", country);
            IsNotValid("00203", country);
            IsNotValid("00324", country);
            IsNotValid("00400", country);
            IsNotValid("00569", country);
            IsNotValid("00628", country);
            IsNotValid("00761", country);
            IsNotValid("00676", country);
            IsNotValid("00875", country);
            IsNotValid("00999", country);
        }

        /// <summary>Tests patterns that should not be valid for Trinidad And Tobago (TT).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_TT_All()
        {
            var country = Country.TT;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Taiwan (TW).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_TW_All()
        {
            var country = Country.TW;

            IsNotValid("000000", country);
            IsNotValid("001234", country);
            IsNotValid("012823", country);
            IsNotValid("020375", country);
            IsNotValid("032434", country);
            IsNotValid("040082", country);
            IsNotValid("056978", country);
            IsNotValid("062824", country);
            IsNotValid("076113", country);
            IsNotValid("067671", country);
            IsNotValid("087523", country);
            IsNotValid("099999", country);
        }

        /// <summary>Tests patterns that should not be valid for Ukraine (UA).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_UA_All()
        {
            var country = Country.UA;

            IsNotValid("00000", country);
            IsNotValid("00012", country);
            IsNotValid("00123", country);
            IsNotValid("00200", country);
            IsNotValid("00326", country);
            IsNotValid("00409", country);
            IsNotValid("00566", country);
            IsNotValid("00629", country);
            IsNotValid("00763", country);
            IsNotValid("00675", country);
            IsNotValid("00871", country);
            IsNotValid("00999", country);
        }

        /// <summary>Tests patterns that should not be valid for United States (US).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_US_All()
        {
                 var country = Country.US;
                 
                 IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Uruguay (UY).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_UY_All()
        {
            var country = Country.UY;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Holy See (VA).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_VA_All()
        {
            var country = Country.VA;

            IsNotValid("00000", country); 
            IsNotValid("01234", country); 
            IsNotValid("12823", country); 
            IsNotValid("20375", country); 
            IsNotValid("32434", country); 
            IsNotValid("40082", country); 
            IsNotValid("56978", country); 
            IsNotValid("62824", country); 
            IsNotValid("76113", country); 
            IsNotValid("67671", country); 
            IsNotValid("87523", country); 
            IsNotValid("99999", country); 
        }

        /// <summary>Tests patterns that should not be valid for Saint Vincent And The Grenadines (VC).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_VC_All()
        {
            var country = Country.VC;

            IsNotValid("AA0000", country);
            IsNotValid("AS0144", country);
            IsNotValid("BJ1282", country);
            IsNotValid("CD2037", country);
            IsNotValid("DE3243", country);
            IsNotValid("EO4008", country);
            IsNotValid("FN5697", country);
            IsNotValid("GF6282", country);
            IsNotValid("HL7611", country);
            IsNotValid("ID6767", country);
            IsNotValid("JS8752", country);
            IsNotValid("KN9999", country);
            IsNotValid("LO0000", country);
            IsNotValid("ME0144", country);
            IsNotValid("NN1282", country);
            IsNotValid("OL2037", country);
            IsNotValid("PS3243", country);
            IsNotValid("QD4008", country);
            IsNotValid("RN5697", country);
            IsNotValid("SE6282", country);
            IsNotValid("TM7611", country);
            IsNotValid("UF6767", country);
            IsNotValid("VE8752", country);
            IsNotValid("WL9999", country);
            IsNotValid("XM0000", country);
            IsNotValid("YE0144", country);
            IsNotValid("ZL1282", country);
            IsNotValid("ZZ2037", country);
        }

        /// <summary>Tests patterns that should not be valid for Venezuela (VE).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_VE_All()
        {
            var country = Country.VE;

            IsNotValid("00000", country);
            IsNotValid("01223", country);
            IsNotValid("12334", country);
            IsNotValid("20400", country);
            IsNotValid("32764", country);
            IsNotValid("40794", country);
            IsNotValid("56564", country);
            IsNotValid("62890", country);
            IsNotValid("76934", country);
            IsNotValid("67055", country);
            IsNotValid("87318", country);
            IsNotValid("99999", country);

            IsNotValid("000A", country);
            IsNotValid("032A", country);
            IsNotValid("143B", country);
            IsNotValid("204C", country);
            IsNotValid("347D", country);
            IsNotValid("447E", country);
            IsNotValid("545F", country);
            IsNotValid("608G", country);
            IsNotValid("749H", country);
            IsNotValid("J650I", country);
            IsNotValid("K8832", country);
            IsNotValid("L9999", country);
            IsNotValid("M0000", country);
            IsNotValid("N0325", country);
            IsNotValid("O1436", country);
            IsNotValid("20412", country);
            IsNotValid("34787", country);
            IsNotValid("44757", country);
            IsNotValid("54505", country);
            IsNotValid("60888", country);
        }

        /// <summary>Tests patterns that should not be valid for Virgin Islands (VG).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_VG_All()
        {
            var country = Country.VG;

            IsNotValid("0123", country);
            IsNotValid("1187", country);
            IsNotValid("1199", country);
            IsNotValid("1234", country);
            IsNotValid("2000", country);
            IsNotValid("3248", country);
            IsNotValid("4945", country);
            IsNotValid("5640", country);
            IsNotValid("6208", country);
            IsNotValid("7645", country);
            IsNotValid("6752", country);
            IsNotValid("8782", country);
            IsNotValid("9999", country);

            IsNotValid("VG0123", country);
            IsNotValid("VG1187", country);
            IsNotValid("VG1199", country);
            IsNotValid("VG1234", country);
            IsNotValid("VG2000", country);
            IsNotValid("VG3248", country);
            IsNotValid("VG4945", country);
            IsNotValid("VG5640", country);
            IsNotValid("VG6208", country);
            IsNotValid("VG7645", country);
            IsNotValid("VG6752", country);
            IsNotValid("VG8782", country);
            IsNotValid("VG9999", country);
        }

        /// <summary>Tests patterns that should not be valid for Virgin Islands (VI).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_VI_All()
        {
            var country = Country.VI;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("99999", country);
            IsNotValid("000000000", country);
            IsNotValid("013582794", country);
            IsNotValid("124678364", country);
            IsNotValid("200424690", country);
            IsNotValid("324813434", country);
            IsNotValid("404571855", country);
            IsNotValid("564023918", country);
            IsNotValid("620899999", country);
            IsNotValid("764500000", country);
            IsNotValid("675249423", country);
            IsNotValid("878227947", country);
            IsNotValid("999999999", country);

            IsNotValid("00800", country);
            IsNotValid("00804", country);
            IsNotValid("00869", country);
            IsNotValid("00870", country);
            IsNotValid("00860", country);
            IsNotValid("00881", country);
            IsNotValid("00892", country);
            IsNotValid("008000000", country);
            IsNotValid("008041235", country);
            IsNotValid("008692908", country);
            IsNotValid("008706345", country);
            IsNotValid("008607552", country);
            IsNotValid("008817182", country);
            IsNotValid("008929999", country);
        }

        /// <summary>Tests patterns that should not be valid for Viet Nam (VN).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_VN_All()
        {
            var country = Country.VN;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Wallis And Futuna (WF).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_WF_All()
        {
            var country = Country.WF;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Mayotte (YT).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_YT_All()
        {
            var country = Country.YT;

            IsNotValid("M11AA", country);
            IsNotValid("M11aA", country);
            IsNotValid("M11AA", country);
            IsNotValid("m11AA", country);
            IsNotValid("m11aa", country);

            IsNotValid("B338TH", country);
            IsNotValid("B338TH", country);

            IsNotValid("CR26XH", country);
            IsNotValid("CR26XH", country);

            IsNotValid("DN551PT", country);
            IsNotValid("DN551PT", country);

            IsNotValid("W1A1HQ", country);
            IsNotValid("W1A1HQ", country);

            IsNotValid("EC1A1BB", country);
            IsNotValid("EC1A1BB", country);
        }

        /// <summary>Tests patterns that should not be valid for South Africa (ZA).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_ZA_All()
        {
            var country = Country.ZA;

            IsNotValid("0000", country); 
        }

        /// <summary>Tests patterns that should not be valid for Zambia (ZM).</summary>
        [TestMethod]
        public void IsNotValidCustomCases_ZM_All()
        {
            var country = Country.ZM;

            IsNotValid("0", country);
        }

        #endregion

        private static void IsValid(string code, Country country)
        {
            Assert.IsTrue(PostalCode.IsValid(code, country), "Postal code '{0}' should be valid for {1:f}.", code, country);
        }
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