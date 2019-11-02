using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.TestTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests
{
    /// <summary>Tests the GUID SVO.</summary>
    [TestFixture]
    public class UuidTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly Uuid TestStruct = Uuid.Parse("Qowaiv_SVOLibrary_GUIA");
        public static readonly Guid TestGuid = Guid.Parse("8a1a8c42-d2ff-e254-e26e-b6abcbf19420");

        #region GUID const tests

        /// <summary>QGuid.Empty should be equal to the default of GUID.</summary>
        [Test]
        public void Empty_None_EqualsDefault()
        {
            Assert.AreEqual(default(Uuid), Uuid.Empty);
        }

        #endregion

        #region GUID IsEmpty tests

        /// <summary>QGuid.IsEmpty() should be true for the default of GUID.</summary>
        [Test]
        public void IsEmpty_Default_IsTrue()
        {
            Assert.IsTrue(default(Uuid).IsEmpty());
        }
        /// <summary>QGuid.IsEmpty() should be false for the TestStruct.</summary>
        [Test]
        public void IsEmpty_TestStruct_IsFalse()
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

            Assert.IsTrue(Uuid.TryParse(str, out Uuid val), "Valid");
            Assert.AreEqual("", val.ToString(), "Value");
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TyrParse_StringEmpty_IsValid()
        {
            string str = string.Empty;

            Assert.IsTrue(Uuid.TryParse(str, out Uuid val), "Valid");
            Assert.AreEqual("", val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TyrParse_StringValue_IsValid()
        {
            string str = "8a1a8c42-d2ff-e254-e26e-b6abcbf19420";

            Assert.IsTrue(Uuid.TryParse(str, out Uuid val), "Valid");
            Assert.AreEqual(str, val.ToString("d"), "Value");
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [Test]
        public void TyrParse_StringValue_IsNotValid()
        {
            string str = "string";

            Assert.IsFalse(Uuid.TryParse(str, out Uuid val), "Valid");
            Assert.AreEqual("", val.ToString(), "Value");
        }

        [Test]
        public void Parse_InvalidInput_ThrowsFormatException()
        {
            using (new CultureInfoScope("en-GB"))
            {
                Assert.Catch<FormatException>
                (() =>
                {
                    Uuid.Parse("InvalidInput");
                },
                "Not a valid GUID");
            }
        }

        [Test]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = TestStruct;
                var act = Uuid.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void TryParse_InvalidInput_DefaultValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = default(Uuid);
                var act = Uuid.TryParse("InvalidInput");

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
                SerializationTest.DeserializeUsingConstructor<Uuid>(null, default);
            },
            "info");
        }

        [Test]
        public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
        {
            Assert.Catch<SerializationException>
            (() =>
            {
                var info = new SerializationInfo(typeof(Uuid), new System.Runtime.Serialization.FormatterConverter());
                SerializationTest.DeserializeUsingConstructor<Uuid>(info, default);
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
            var info = new SerializationInfo(typeof(Uuid), new System.Runtime.Serialization.FormatterConverter());
            obj.GetObjectData(info, default);

            Assert.AreEqual(TestGuid, info.GetValue("Value", typeof(Guid)));
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
            var exp = "Qowaiv_SVOLibrary_GUIA";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void XmlDeserialize_XmlString_AreEqual()
        {
            var act = SerializationTest.XmlDeserialize<Uuid>("Qowaiv_SVOLibrary_GUIA");
            Assert.AreEqual(TestStruct, act);
        }

        [Test]
        public void SerializeDeserialize_GuidSerializeObject_AreEqual()
        {
            var input = new QGuidSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new QGuidSerializeObject
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
        public void XmlSerializeDeserialize_GuidSerializeObject_AreEqual()
        {
            var input = new QGuidSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new QGuidSerializeObject
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
        public void DataContractSerializeDeserialize_GuidSerializeObject_AreEqual()
        {
            var input = new QGuidSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new QGuidSerializeObject
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
            var input = new QGuidSerializeObject
            {
                Id = 17,
                Obj = Uuid.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new QGuidSerializeObject
            {
                Id = 17,
                Obj = Uuid.Empty,
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
            var input = new QGuidSerializeObject
            {
                Id = 17,
                Obj = Uuid.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new QGuidSerializeObject
            {
                Id = 17,
                Obj = Uuid.Empty,
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
            var act = JsonTester.Read<Uuid>();
            var exp = Uuid.Empty;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_InvalidStringValue_AssertFormatException()
        {
            Assert.Catch<FormatException>(() =>
            {
                JsonTester.Read<Uuid>("InvalidStringValue");
            },
            "Not a valid GUID");
        }
        [Test]
        public void FromJson_StringValue_AreEqual()
        {
            var act = JsonTester.Read<Uuid>("Qowaiv_SVOLibrary_GUIA");
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_Int64Value_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>(() =>
            {
                JsonTester.Read<Uuid>(123456L);
            },
            "JSON deserialization from an integer is not supported.");
        }

        [Test]
        public void FromJson_DoubleValue_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>(() =>
            {
                JsonTester.Read<Uuid>(1234.56);
            },
            "JSON deserialization from a number is not supported.");
        }

        [Test]
        public void FromJson_DateTimeValue_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>(() =>
            {
                JsonTester.Read<Uuid>(new DateTime(1972, 02, 14));
            },
            "JSON deserialization from a date is not supported.");
        }

        [Test]
        public void ToJson_DefaultValue_IsNull()
        {
            object act = JsonTester.Write(default(Uuid));
            Assert.IsNull(act);
        }
        [Test]
        public void ToJson_TestStruct_AreEqual()
        {
            var act = JsonTester.Write(TestStruct);
            var exp = "Qowaiv_SVOLibrary_GUIA";
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IFormattable / ToString tests

        [Test]
        public void ToString_Empty_StringEmpty()
        {
            var act = Uuid.Empty.ToString();
            var exp = "";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: 'Qowaiv_SVOLibrary_GUIA', format: 'Unit Test Format'";

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToString_Formats_FormattedGuid()
        {
            var formats = new string[] { null, "", "s", "S", "b", "B", "d", "D", "p", "P", "x", "X" };

            var expected = new []
            {
                "Qowaiv_SVOLibrary_GUIA",
                "Qowaiv_SVOLibrary_GUIA",
                "Qowaiv_SVOLibrary_GUIA",
                "Qowaiv_SVOLibrary_GUIA",
                "{8a1a8c42-d2ff-e254-e26e-b6abcbf19420}",
                "{8A1A8C42-D2FF-E254-E26E-B6ABCBF19420}",
                "8a1a8c42-d2ff-e254-e26e-b6abcbf19420",
                "8A1A8C42-D2FF-E254-E26E-B6ABCBF19420",
                "(8a1a8c42-d2ff-e254-e26e-b6abcbf19420)",
                "(8A1A8C42-D2FF-E254-E26E-B6ABCBF19420)",
                "{0x8a1a8c42,0xd2ff,0xe254,{0xe2,0x6e,0xb6,0xab,0xcb,0xf1,0x94,0x20}}",
                "{0x8A1A8C42,0xD2FF,0xE254,{0xE2,0x6E,0xB6,0xAB,0xCB,0xF1,0x94,0x20}}"
            };

            for (var i = 0; i < expected.Length; i++)
            {
                var actual = TestStruct.ToString(formats[i]);
                Assert.AreEqual(expected[i], actual);
            }
        }

        [Test]
        public void ToString_Invalid_FormattedGuid()
        {
            Assert.Catch<FormatException>(() =>
            {
                TestStruct.ToString("invalid");
            }, "Input string was not in a correct format.");
        }

        [Test]
        public void DebuggerDisplay_DebugToString_HasAttribute()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(Uuid));
        }

        [Test]
        public void DebuggerDisplay_DefaultValue_String()
        {
            DebuggerDisplayAssert.HasResult("", default(Uuid));
        }

        [Test]
        public void DebuggerDisplay_TestStruct_String()
        {
            DebuggerDisplayAssert.HasResult("Qowaiv_SVOLibrary_GUIA", TestStruct);
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for QGuid.Empty.</summary>
        [Test]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, Uuid.Empty.GetHashCode());
        }

        /// <summary>GetHash should not fail for the test struct.</summary>
        [Test]
        public void GetHash_TestStruct_Hash()
        {
            Assert.AreEqual(TestGuid.GetHashCode(), TestStruct.GetHashCode());
        }

        [Test]
        public void Equals_EmptyEmpty_IsTrue()
        {
            Assert.IsTrue(Uuid.Empty.Equals(Uuid.Empty));
        }

        [Test]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = Uuid.Parse("{95A20FBA-347D-44C9-BEBF-65F06B73F82C}");
            var r = Uuid.Parse("95a20fba347d44c9bebf65f06b73f82c");

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
            Assert.IsFalse(TestStruct.Equals(Uuid.Empty));
        }

        [Test]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(Uuid.Empty.Equals(TestStruct));
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

        /// <summary>Orders a list of GUIDs ascending.</summary>
        [Test]
        public void OrderBy_Uuid_AreEqual()
        {
            var item0 = Uuid.Parse("3BE968F7-AAEA-422C-BA74-72A4D045FD74");
            var item1 = Uuid.Parse("59ED7F38-8E6A-45A9-B3A2-6D32FDF4DD10");
            var item2 = Uuid.Parse("5BD0EF29-C625-4B8D-A063-E474B28E8653");
            var item3 = Uuid.Parse("77185219-193C-4D39-B4B1-9ED05B0FC4C8");


            var inp = new List<Uuid> { Uuid.Empty, item3, item2, item0, item1, Uuid.Empty };
            var exp = new List<Uuid> { Uuid.Empty, Uuid.Empty, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of GUIDs descending.</summary>
        [Test]
        public void OrderByDescending_Uuid_AreEqual()
        {
            var item0 = Uuid.Parse("3BE968F7-AAEA-422C-BA74-72A4D045FD74");
            var item1 = Uuid.Parse("59ED7F38-8E6A-45A9-B3A2-6D32FDF4DD10");
            var item2 = Uuid.Parse("5BD0EF29-C625-4B8D-A063-E474B28E8653");
            var item3 = Uuid.Parse("77185219-193C-4D39-B4B1-9ED05B0FC4C8");

            var inp = new List<Uuid> { Uuid.Empty, item3, item2, item0, item1, Uuid.Empty };
            var exp = new List<Uuid> { item3, item2, item1, item0, Uuid.Empty, Uuid.Empty };
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
                "Argument must be a GUID"
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
                "Argument must be a GUID"
            );
        }

        #endregion

        #region Casting tests

        [Test]
        public void Explicit_StringToQGuid_AreEqual()
        {
            var exp = TestStruct;
            var act = (Uuid)TestStruct.ToString();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_GuidToString_AreEqual()
        {
            var exp = TestStruct.ToString();
            var act = (string)TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Implicit_GuidToQGuid_AreEqual()
        {
            Uuid exp = TestStruct;
            Uuid act = TestGuid;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_UuidToGuid_AreEqual()
        {
            Guid exp = TestGuid;
            Guid act = TestStruct;

            Assert.AreEqual(exp, act);
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        [Test]
        public void ToByteArray_TestStruct_EqualsTestGuidToByteArray()
        {
            var act = TestStruct.ToByteArray();
            var exp = TestGuid.ToByteArray();

            CollectionAssert.AreEqual(exp, act);
        }

        [Test]
        public void NewGuid_None_NotEmpty()
        {
            Uuid actual = Uuid.NewUuid();
            Uuid expected = Guid.Empty;
            Assert.AreNotEqual(expected, actual);
        }

        #endregion

        #region Type converter tests

        [Test]
        public void ConverterExists_Uuid_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(Uuid));
        }

        [Test]
        public void CanNotConvertFromInt32_Uuid_IsTrue()
        {
            TypeConverterAssert.CanNotConvertFrom(typeof(Uuid), typeof(Int32));
        }
        [Test]
        public void CanNotConvertToInt32_Uuid_IsTrue()
        {
            TypeConverterAssert.CanNotConvertTo(typeof(Uuid), typeof(Int32));
        }

        [Test]
        public void CanConvertFromString_Uuid_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(Uuid));
        }

        [Test]
        public void CanConvertToString_Uuid_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(Uuid));
        }

        [Test]
        public void ConvertFrom_StringNull_UuidEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(Uuid.Empty, (string)null);
            }
        }

        [Test]
        public void ConvertFrom_StringEmpty_UuidEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(Uuid.Empty, string.Empty);
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
        public void ConvertFromUnderlyingType_Guid_Successful()
        {
            TypeConverterAssert.ConvertFromEquals(TestStruct, TestGuid);
        }

        [Test]
        public void ConverToUnderlyingType_Guid_Successful()
        {
            TypeConverterAssert.ConvertToEquals(TestGuid, TestStruct);
        }

        #endregion

        #region IsValid tests

        [Test]
        public void IsValid_Data_IsFalse()
        {
            Assert.IsFalse(Uuid.IsValid("Complex"), "Complex");
            Assert.IsFalse(Uuid.IsValid((String)null), "(String)null");
            Assert.IsFalse(Uuid.IsValid(string.Empty), "string.Empty");
        }
        [Test]
        public void IsValid_Data_IsTrue()
        {
            Assert.IsTrue(Uuid.IsValid("Qowaiv_SVOLibrary_GUIA"), "Qowaiv_SVOLibrary_GUIA");
            Assert.IsTrue(Uuid.IsValid("8a1a8c42-d2ff-e254-e26e-b6abcbf19420"), "8a1a8c42-d2ff-e254-e26e-b6abcbf19420");
        }
        #endregion

        #region Version

        [Test]
        public void GenerateWithMd5_HasVersion3()
        {
            var uuid = Uuid.GenerateWithMD5(Encoding.ASCII.GetBytes("Qowaiv"));
            var actual = Uuid.Parse("lmZO_haEOTCwGsCcbIZFFg");

            Assert.AreEqual(UuidVersion.MD5, uuid.Version);
            Assert.AreEqual(uuid, actual);
        }

        [Test]
        public void NewUuid_HasVersion4()
        {
            var uuid = Uuid.NewUuid();
            Assert.AreEqual(UuidVersion.Random, uuid.Version);
        }

        [Test]
        public void GenerateWithSHA1_HasVersion5()
        {
            var uuid = Uuid.GenerateWithSHA1(Encoding.ASCII.GetBytes("Qowaiv"));
            var actual = Uuid.Parse("39h-Y1rR51ym_t78x9h0bA");

            Assert.AreEqual(UuidVersion.SHA1, uuid.Version);
            Assert.AreEqual(uuid, actual);
        }

        #endregion
    }

    [Serializable]
    public class QGuidSerializeObject
    {
        public int Id { get; set; }
        public Uuid Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
