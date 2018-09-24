﻿using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.TestTools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.Security.Cryptography.UnitTests
{
    /// <summary>Tests the cryptographic seed SVO.</summary>
    public class CryptographicSeedTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly CryptographicSeed TestStruct = CryptographicSeed.Parse("Qowaiv==");

        #region cryptographic seed const tests

        /// <summary>CryptographicSeed.Empty should be equal to the default of cryptographic seed.</summary>
        [Test]
        public void Empty_None_EqualsDefault()
        {
            Assert.AreEqual(default(CryptographicSeed), CryptographicSeed.Empty);
        }

        #endregion

        #region cryptographic seed IsEmpty tests

        /// <summary>CryptographicSeed.IsEmpty() should be true for the default of cryptographic seed.</summary>
        [Test]
        public void IsEmpty_Default_IsTrue()
        {
            Assert.IsTrue(default(CryptographicSeed).IsEmpty());
        }

        #endregion

        #region TryParse tests

        /// <summary>TryParse null should be valid.</summary>
        [Test]
        public void TyrParse_Null_IsValid()
        {
            string str = null;
            Assert.IsTrue(CryptographicSeed.TryParse(str, out CryptographicSeed val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TyrParse_StringEmpty_IsValid()
        {
            string str = string.Empty;
            Assert.IsTrue(CryptographicSeed.TryParse(str, out CryptographicSeed val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TyrParse_StringValue_IsValid()
        {
            string str = "string==";
            Assert.IsTrue(CryptographicSeed.TryParse(str, out CryptographicSeed val), "Valid");
            Assert.AreEqual(str, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [Test]
        public void TyrParse_StringValue_IsNotValid()
        {
            string str = "string";
            Assert.IsFalse(CryptographicSeed.TryParse(str, out CryptographicSeed val), "Valid");
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
                    CryptographicSeed.Parse("!");
                },
                "Not a valid cryptographic seed");
            }
        }

        [Test]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = TestStruct;
                var act = CryptographicSeed.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void TryParse_InvalidInput_DefaultValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = default(CryptographicSeed);
                var act = CryptographicSeed.TryParse("!");

                Assert.AreEqual(exp, act);
            }
        }

        #endregion

        #region Create tets

        [Test]
        public void Create_GUID_BytesAreEqual()
        {
            var exp = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            var id = new Guid(exp);

            var seed = CryptographicSeed.Create(id);
            var act = seed.ToByteArray();

            CollectionAssert.AreEqual(exp, act);
            Assert.AreNotSame(exp, act);
        }

        [Test]
        public void Create_UUID_BytesAreEqual()
        {
            var exp = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            Uuid id = new Guid(exp);

            var seed = CryptographicSeed.Create(id);
            var act = seed.ToByteArray();

            CollectionAssert.AreEqual(exp, act);
            Assert.AreNotSame(exp, act);
        }

        #endregion

        #region (XML) (De)serialization tests

        [Test]
        public void Constructor_SerializationInfoIsNull_ThrowsArgumentNullException()
        {
            ExceptionAssert.CatchArgumentNullException
            (() =>
            {
                SerializationTest.DeserializeUsingConstructor<CryptographicSeed>(null, default(StreamingContext));
            },
            "info");
        }

        [Test]
        public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
        {
            Assert.Catch<SerializationException>
            (() =>
            {
                var info = new SerializationInfo(typeof(CryptographicSeed), new System.Runtime.Serialization.FormatterConverter());
                SerializationTest.DeserializeUsingConstructor<CryptographicSeed>(info, default(StreamingContext));
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
            var info = new SerializationInfo(typeof(CryptographicSeed), new System.Runtime.Serialization.FormatterConverter());
            obj.GetObjectData(info, default(StreamingContext));

            CollectionAssert.AreEqual(new byte[] { 66, 140, 26, 138 }, (byte[])info.GetValue("Value", typeof(byte[])));
        }

        [Test]
        public void SerializeDeserialize_TestStruct_AreEqual()
        {
            var input = CryptographicSeedTest.TestStruct;
            var exp = CryptographicSeedTest.TestStruct;
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void DataContractSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = CryptographicSeedTest.TestStruct;
            var exp = CryptographicSeedTest.TestStruct;
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void XmlSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = CryptographicSeedTest.TestStruct;
            var exp = CryptographicSeedTest.TestStruct;
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void SerializeDeserialize_CryptographicSeedSerializeObject_AreEqual()
        {
            var input = new CryptographicSeedSerializeObject()
            {
                Id = 17,
                Obj = CryptographicSeedTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new CryptographicSeedSerializeObject()
            {
                Id = 17,
                Obj = CryptographicSeedTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void XmlSerializeDeserialize_CryptographicSeedSerializeObject_AreEqual()
        {
            var input = new CryptographicSeedSerializeObject()
            {
                Id = 17,
                Obj = CryptographicSeedTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new CryptographicSeedSerializeObject()
            {
                Id = 17,
                Obj = CryptographicSeedTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void DataContractSerializeDeserialize_CryptographicSeedSerializeObject_AreEqual()
        {
            var input = new CryptographicSeedSerializeObject()
            {
                Id = 17,
                Obj = CryptographicSeedTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new CryptographicSeedSerializeObject()
            {
                Id = 17,
                Obj = CryptographicSeedTest.TestStruct,
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
            var input = new CryptographicSeedSerializeObject()
            {
                Id = 17,
                Obj = CryptographicSeed.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new CryptographicSeedSerializeObject()
            {
                Id = 17,
                Obj = CryptographicSeed.Empty,
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
            var input = new CryptographicSeedSerializeObject()
            {
                Id = 17,
                Obj = CryptographicSeed.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new CryptographicSeedSerializeObject()
            {
                Id = 17,
                Obj = CryptographicSeed.Empty,
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
            var act = JsonTester.Read<CryptographicSeed>();
            var exp = CryptographicSeed.Empty;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_InvalidStringValue_AssertFormatException()
        {
            Assert.Catch<FormatException>(() =>
            {
                JsonTester.Read<CryptographicSeed>("InvalidStringValue");
            },
            "Not a valid cryptographic seed");
        }
        [Test]
        public void FromJson_StringValue_AreEqual()
        {
            var act = JsonTester.Read<CryptographicSeed>(TestStruct.ToString(CultureInfo.InvariantCulture));
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_Int64Value_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>(() =>
            {
                JsonTester.Read<CryptographicSeed>(123456L);
            },
            "JSON deserialization from an integer is not supported.");
        }

        [Test]
        public void FromJson_DoubleValue_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>(() =>
            {
                JsonTester.Read<CryptographicSeed>(1234.56);
            },
            "JSON deserialization from a number is not supported.");
        }

        [Test]
        public void FromJson_DateTimeValue_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>(() =>
            {
                JsonTester.Read<CryptographicSeed>(new DateTime(1972, 02, 14));
            },
            "JSON deserialization from a date is not supported.");
        }

        [Test]
        public void ToJson_DefaultValue_AreEqual()
        {
            object act = JsonTester.Write(default(CryptographicSeed));
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
        public void ToString_Empty_StringEmpty()
        {
            var act = CryptographicSeed.Empty.ToString();
            var exp = "";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: 'Qowaig==', format: 'Unit Test Format'";

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToString_TestStruct_ComplexPattern()
        {
            var act = TestStruct.ToString("");
            var exp = "Qowaig==";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void DebuggerDisplay_DebugToString_HasAttribute()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(CryptographicSeed));
        }

        [Test]
        public void DebuggerDisplay_DefaultValue_String()
        {
            DebuggerDisplayAssert.HasResult("{empty}", default(CryptographicSeed));
        }

        [Test]
        public void DebuggerDisplay_TestStruct_String()
        {
            DebuggerDisplayAssert.HasResult("Qowaig==", TestStruct);
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for CryptographicSeed.Empty.</summary>
        [Test]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, CryptographicSeed.Empty.GetHashCode());
        }

        /// <summary>GetHash should not fail for the test struct.</summary>
        [Test]
        public void GetHash_TestStruct_Hash()
        {
            Assert.AreEqual(1378, CryptographicSeedTest.TestStruct.GetHashCode());
        }

        [Test]
        public void Equals_EmptyEmpty_IsTrue()
        {
            Assert.IsTrue(CryptographicSeed.Empty.Equals(CryptographicSeed.Empty));
        }

        [Test]
        public void Equals_SameLengthDifferentValues_IsFalse()
        {
            CryptographicSeed left = new byte[] { 1, 2, 3 };
            CryptographicSeed right = new byte[] { 1, 2, 4 };

            Assert.IsFalse(left.Equals(right));
        }

        [Test]
        public void Equals_TestStructTestStruct_IsTrue()
        {
            Assert.IsTrue(CryptographicSeedTest.TestStruct.Equals(CryptographicSeedTest.TestStruct));
        }

        [Test]
        public void Equals_TestStructEmpty_IsFalse()
        {
            Assert.IsFalse(CryptographicSeedTest.TestStruct.Equals(CryptographicSeed.Empty));
        }

        [Test]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(CryptographicSeed.Empty.Equals(CryptographicSeedTest.TestStruct));
        }

        [Test]
        public void Equals_TestStructObjectTestStruct_IsTrue()
        {
            Assert.IsTrue(CryptographicSeedTest.TestStruct.Equals((object)CryptographicSeedTest.TestStruct));
        }

        [Test]
        public void Equals_TestStructNull_IsFalse()
        {
            Assert.IsFalse(CryptographicSeedTest.TestStruct.Equals(null));
        }

        [Test]
        public void Equals_TestStructObject_IsFalse()
        {
            Assert.IsFalse(CryptographicSeedTest.TestStruct.Equals(new object()));
        }

        [Test]
        public void OperatorIs_TestStructTestStruct_IsTrue()
        {
            var l = CryptographicSeedTest.TestStruct;
            var r = CryptographicSeedTest.TestStruct;
            Assert.IsTrue(l == r);
        }

        [Test]
        public void OperatorIsNot_TestStructTestStruct_IsFalse()
        {
            var l = CryptographicSeedTest.TestStruct;
            var r = CryptographicSeedTest.TestStruct;
            Assert.IsFalse(l != r);
        }

        #endregion

        #region IComparable tests

        /// <summary>Orders a list of cryptographic seeds ascending.</summary>
        [Test]
        public void OrderBy_CryptographicSeed_AreEqual()
        {
            var item0 = CryptographicSeed.Create(new byte[] { 1 });
            var item1 = CryptographicSeed.Create(new byte[] { 2 });
            var item2 = CryptographicSeed.Create(new byte[] { 3, 6 });
            var item3 = CryptographicSeed.Create(new byte[] { 4, 2 });

            var inp = new List<CryptographicSeed>() { CryptographicSeed.Empty, item3, item2, item0, item1, CryptographicSeed.Empty };
            var exp = new List<CryptographicSeed>() { CryptographicSeed.Empty, CryptographicSeed.Empty, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of cryptographic seeds descending.</summary>
        [Test]
        public void OrderByDescending_CryptographicSeed_AreEqual()
        {
            var item0 = CryptographicSeed.Create(new byte[] { 1 });
            var item1 = CryptographicSeed.Create(new byte[] { 2 });
            var item2 = CryptographicSeed.Create(new byte[] { 3, 6 });
            var item3 = CryptographicSeed.Create(new byte[] { 4, 2 });

            var inp = new List<CryptographicSeed>() { CryptographicSeed.Empty, item3, item2, item0, item1, CryptographicSeed.Empty };
            var exp = new List<CryptographicSeed>() { item3, item2, item1, item0, CryptographicSeed.Empty, CryptographicSeed.Empty };
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
                "Argument must be a cryptographic seed"
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
                "Argument must be a cryptographic seed"
            );
        }

        #endregion

        #region Casting tests

        [Test]
        public void Explicit_StringToCryptographicSeed_AreEqual()
        {
            var exp = TestStruct;
            var act = (CryptographicSeed)TestStruct.ToString();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_CryptographicSeedToString_AreEqual()
        {
            var exp = TestStruct.ToString();
            var act = (string)TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Explicit_ByteArrayToCryptographicSeed_AreEqual()
        {
            CryptographicSeed exp = TestStruct;
            CryptographicSeed act = new Byte[] { 66, 140, 26, 138 };

            Assert.AreEqual(exp, act);
            Assert.AreNotSame(exp, act);
        }
        [Test]
        public void Explicit_CryptographicSeedToByteArray_AreEqual()
        {
            var exp = TestStruct.ToByteArray();
            var act = (Byte[])TestStruct;

            Assert.AreEqual(exp, act);
            Assert.AreNotSame(exp, act);
        }

        #endregion

        #region Properties
        #endregion

        #region Type converter tests

        [Test]
        public void ConverterExists_CryptographicSeed_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(CryptographicSeed));
        }

        [Test]
        public void CanNotConvertFromInt32_CryptographicSeed_IsTrue()
        {
            TypeConverterAssert.CanNotConvertFrom(typeof(CryptographicSeed), typeof(Int32));
        }
        [Test]
        public void CanNotConvertToInt32_CryptographicSeed_IsTrue()
        {
            TypeConverterAssert.CanNotConvertTo(typeof(CryptographicSeed), typeof(Int32));
        }

        [Test]
        public void CanConvertFromString_CryptographicSeed_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(CryptographicSeed));
        }

        [Test]
        public void CanConvertToString_CryptographicSeed_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(CryptographicSeed));
        }

        [Test]
        public void ConvertFrom_StringNull_CryptographicSeedEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(CryptographicSeed.Empty, (string)null);
            }
        }

        [Test]
        public void ConvertFrom_StringEmpty_CryptographicSeedEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(CryptographicSeed.Empty, string.Empty);
            }
        }

        [Test]
        public void ConvertFromString_StringValue_TestStruct()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(CryptographicSeedTest.TestStruct, CryptographicSeedTest.TestStruct.ToString());
            }
        }

        [Test]
        public void ConvertFromInstanceDescriptor_CryptographicSeed_Successful()
        {
            TypeConverterAssert.ConvertFromInstanceDescriptor(typeof(CryptographicSeed));
        }

        [Test]
        public void ConvertToString_TestStruct_StringValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertToStringEquals(CryptographicSeedTest.TestStruct.ToString(), CryptographicSeedTest.TestStruct);
            }
        }

        #endregion

        #region IsValid tests

        [Test]
        public void IsValid_Data_IsFalse()
        {
            Assert.IsFalse(CryptographicSeed.IsValid("!"), "!");
            Assert.IsFalse(CryptographicSeed.IsValid((String)null), "(String)null");
            Assert.IsFalse(CryptographicSeed.IsValid(string.Empty), "string.Empty");
        }
        [Test]
        public void IsValid_Data_IsTrue()
        {
            Assert.IsTrue(CryptographicSeed.IsValid("ComplexPattern=="));
        }
        #endregion
    }

    [Serializable]
    public class CryptographicSeedSerializeObject
    {
        public int Id { get; set; }
        public CryptographicSeed Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
