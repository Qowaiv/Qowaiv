using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.Identifiers;
using Qowaiv.TestTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests.Identifiers
{
    public class ForGuid : GuidLogic { }

    /// <summary>Tests the identifier SVO.</summary>
    public class IdForGuidTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly Id<ForGuid> TestStruct = Id<ForGuid>.Parse("0F5AB5AB-12CB-4629-878D-B18B88B9A504");

        /// <summary>Id<ForGuid>.Empty should be equal to the default of identifier.</summary>
        [Test]
        public void Empty_None_EqualsDefault()
        {
            Assert.AreEqual(default(Id<ForGuid>), Id<ForGuid>.Empty);
        }

        /// <summary>Id<ForGuid>.IsEmpty() should be true for the default of identifier.</summary>
        [Test]
        public void IsEmpty_Default_IsTrue()
        {
            Assert.IsTrue(default(Id<ForGuid>).IsEmpty());
        }

        /// <summary>Id<ForGuid>.IsEmpty() should be false for the TestStruct.</summary>
        [Test]
        public void IsEmpty_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmpty());
        }

        [Test]
        public void FromBytes_Null_IsEmpty()
        {
            var fromBytes = Id<ForGuid>.FromBytes(null);
            Assert.AreEqual(Id<ForGuid>.Empty, fromBytes);
        }

        [Test]
        public void FromBytes_Bytes_IsTestStruct()
        {
            var fromBytes = Id<ForGuid>.FromBytes(new byte[] { 171, 181, 90, 15, 203, 18, 41, 70, 135, 141, 177, 139, 136, 185, 165, 4 });
            Assert.AreEqual(TestStruct, fromBytes);
        }

        [Test]
        public void ToByteArray_Empty_EmptyArray()
        {
            var bytes = Id<ForGuid>.Empty.ToByteArray();
            Assert.AreEqual(Array.Empty<byte>(), bytes);
        }

        [Test]
        public void ToByteArray_TestStruct_FilledArray()
        {
            var bytes = TestStruct.ToByteArray();
            var exepected = new byte[] { 171, 181, 90, 15, 203, 18, 41, 70, 135, 141, 177, 139, 136, 185, 165, 4 };
            Assert.AreEqual(exepected, bytes);
        }

        /// <summary>TryParse null should be valid.</summary>
        [Test]
        public void TyrParse_Null_IsValid()
        {
            Assert.IsTrue(Id<ForGuid>.TryParse(null, out var val));
            Assert.AreEqual(default(Id<ForGuid>), val);
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TyrParse_StringEmpty_IsValid()
        {
            Assert.IsTrue(Id<ForGuid>.TryParse(string.Empty, out var val));
            Assert.AreEqual(default(Id<ForGuid>), val);
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TyrParse_StringValue_IsValid()
        {
            string str = "0f5ab5ab-12cb-4629-878d-b18b88b9a504";
            Assert.IsTrue(Id<ForGuid>.TryParse(str, out var val));
            Assert.AreEqual(str, val.ToString());
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [Test]
        public void TyrParse_StringValue_IsNotValid()
        {
            string str = "0F5AB5AB-12CB-4629-878D";
            Assert.IsFalse(Id<ForGuid>.TryParse(str, out var val));
            Assert.AreEqual(default(Id<ForGuid>), val);
        }

        [Test]
        public void Parse_InvalidInput_ThrowsFormatException()
        {
            using (new CultureInfoScope("en-GB"))
            {
                Assert.Catch<FormatException>(() =>
                {
                    Id<ForGuid>.Parse("InvalidInput");
                }

                , "Not a valid identifier");
            }
        }

        [Test]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = TestStruct;
                var act = Id<ForGuid>.TryParse(exp.ToString());
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void TryParse_InvalidInput_DefaultValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = default(Id<ForGuid>);
                var act = Id<ForGuid>.TryParse("InvalidInput");
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void Constructor_SerializationInfoIsNull_Throws()
        {
            Assert.Catch<ArgumentNullException>(() => SerializationTest.DeserializeUsingConstructor<Id<ForGuid>>(null, default));
        }

        [Test]
        public void Constructor_InvalidSerializationInfo_Throws()
        {
            var info = new SerializationInfo(typeof(Id<ForGuid>), new FormatterConverter());
            Assert.Catch<SerializationException>(() => SerializationTest.DeserializeUsingConstructor<Id<ForGuid>>(info, default));
        }

        [Test]
        public void GetObjectData_NulSerializationInfo_Throws()
        {
            ISerializable obj = TestStruct;
            Assert.Catch<ArgumentNullException>(() => obj.GetObjectData(null, default));
        }

        [Test]
        public void GetObjectData_SerializationInfo_AreEqual()
        {
            ISerializable obj = TestStruct;
            var info = new SerializationInfo(typeof(Id<ForGuid>), new FormatterConverter());
            obj.GetObjectData(info, default);
            Assert.AreEqual(Guid.Parse("0F5AB5AB-12CB-4629-878D-B18B88B9A504"), info.GetValue("Value", typeof(Guid)));
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
            var exp = "0f5ab5ab-12cb-4629-878d-b18b88b9a504";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void XmlDeserialize_XmlString_AreEqual()
        {
            var act = SerializationTest.XmlDeserialize<Id<ForGuid>>("0F5AB5AB-12CB-4629-878D-B18B88B9A504");
            Assert.AreEqual(TestStruct, act);
        }

        [Test]
        public void SerializeDeserialize_IdForGuidSerializeObject_AreEqual()
        {
            var input = new IdForGuidSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            }

            ;
            var exp = new IdForGuidSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            }

            ;
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void XmlSerializeDeserialize_IdForGuidSerializeObject_AreEqual()
        {
            var input = new IdForGuidSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            }

            ;
            var exp = new IdForGuidSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            }

            ;
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void DataContractSerializeDeserialize_IdForGuidSerializeObject_AreEqual()
        {
            var input = new IdForGuidSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            }

            ;
            var exp = new IdForGuidSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            }

            ;
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void SerializeDeserialize_Default_AreEqual()
        {
            var input = new IdForGuidSerializeObject
            {
                Id = 17,
                Obj = default,
                Date = new DateTime(1970, 02, 14),
            }

            ;
            var exp = new IdForGuidSerializeObject
            {
                Id = 17,
                Obj = default,
                Date = new DateTime(1970, 02, 14),
            }

            ;
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void XmlSerializeDeserialize_Default_AreEqual()
        {
            var input = new IdForGuidSerializeObject
            {
                Id = 17,
                Obj = default,
                Date = new DateTime(1970, 02, 14),
            }

            ;
            var exp = new IdForGuidSerializeObject
            {
                Id = 17,
                Obj = default,
                Date = new DateTime(1970, 02, 14),
            }

            ;
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

        [TestCase("2017-06-11")]
        public void FromJson_Invalid_Throws(object json)
        {
            Assert.Catch<FormatException>(() => JsonTester.Read<Id<ForGuid>>(json));
        }

        [TestCase("0F5AB5AB-12CB-4629-878D-B18B88B9A504", "0F5AB5AB-12CB-4629-878D-B18B88B9A504")]
        [TestCase("", "")]
        public void FromJson(Id<ForGuid> expected, object json)
        {
            var actual = JsonTester.Read<Id<ForGuid>>(json);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_Empty_StringEmpty()
        {
            var act = Id<ForGuid>.Empty.ToString();
            var exp = "";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("S", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: 'q7VaD8sSKUaHjbGLiLmlBA', format: 'S'";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void DebuggerDisplay_DebugToString_HasAttribute()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(Id<ForGuid>));
        }

        [Test]
        public void DebuggerDisplay_DefaultValue_String()
        {
            DebuggerDisplayAssert.HasResult("{empty} (ForGuid)", default(Id<ForGuid>));
        }

        [Test]
        public void DebuggerDisplay_TestStruct_String()
        {
            DebuggerDisplayAssert.HasResult("0f5ab5ab-12cb-4629-878d-b18b88b9a504 (ForGuid)", TestStruct);
        }

        /// <summary>GetHash should not fail for Id<ForGuid>.Empty.</summary>
        [Test]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, Id<ForGuid>.Empty.GetHashCode());
        }

        /// <summary>GetHash should not fail for the test struct.</summary>
        [Test]
        public void GetHash_TestStruct_Hash()
        {
            Assert.AreNotEqual(0, TestStruct.GetHashCode());
        }

        [Test]
        public void Equals_EmptyEmpty_IsTrue()
        {
            Assert.IsTrue(Id<ForGuid>.Empty.Equals(Id<ForGuid>.Empty));
        }

        [Test]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = Id<ForGuid>.Parse("Qowaiv_SVOLibrary_GUIA");
            var r = Id<ForGuid>.Parse("8a1a8c42-d2ff-e254-e26e-b6abcbf19420");
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
            Assert.IsFalse(TestStruct.Equals(Id<ForGuid>.Empty));
        }

        [Test]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(Id<ForGuid>.Empty.Equals(TestStruct));
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

        /// <summary>Orders a list of identifiers ascending.</summary>
        [Test]
        public void OrderBy_IdForGuid_AreEqual()
        {
            var item0 = Id<ForGuid>.Parse("45140308-9961-40D7-8907-31592772F556");
            var item1 = Id<ForGuid>.Parse("56AE40C9-A285-4B9B-A725-DE4B48F24BB0");
            var item2 = Id<ForGuid>.Parse("63FBD011-595C-46EB-AF59-5E4C6AD23C41");
            var item3 = Id<ForGuid>.Parse("BE236238-7A6D-4CE3-A955-B3D672593B8F");
            var inp = new List<Id<ForGuid>> { Id<ForGuid>.Empty, item3, item2, item0, item1, Id<ForGuid>.Empty };
            var exp = new List<Id<ForGuid>> { Id<ForGuid>.Empty, Id<ForGuid>.Empty, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();
            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of identifiers descending.</summary>
        [Test]
        public void OrderByDescending_IdForGuid_AreEqual()
        {
            var item0 = Id<ForGuid>.Parse("45140308-9961-40D7-8907-31592772F556");
            var item1 = Id<ForGuid>.Parse("56AE40C9-A285-4B9B-A725-DE4B48F24BB0");
            var item2 = Id<ForGuid>.Parse("63FBD011-595C-46EB-AF59-5E4C6AD23C41");
            var item3 = Id<ForGuid>.Parse("BE236238-7A6D-4CE3-A955-B3D672593B8F");
            var inp = new List<Id<ForGuid>> { Id<ForGuid>.Empty, item3, item2, item0, item1, Id<ForGuid>.Empty };
            var exp = new List<Id<ForGuid>> { item3, item2, item1, item0, Id<ForGuid>.Empty, Id<ForGuid>.Empty };
            var act = inp.OrderByDescending(item => item).ToList();
            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Compare with a to object casted instance should be fine.</summary>
        [Test]
        public void CompareTo_ObjectTestStruct_0()
        {
            Assert.AreEqual(0, TestStruct.CompareTo((object)TestStruct));
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
        public void CompareTo_newObject_Throw()
        {
            var x = Assert.Catch<ArgumentException>(() => TestStruct.CompareTo(new object()));
            Assert.AreEqual("Argument must be Id<ForGuid>. (Parameter 'obj')", x.Message);
        }

        [Test]
        public void ConverterExists_IdForGuid_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(Id<ForGuid>));
        }

        [Test]
        public void CanNotConvertFromInt32_IdForGuid_IsTrue()
        {
            TypeConverterAssert.CanNotConvertFrom(typeof(Id<ForGuid>), typeof(int));
        }

        [Test]
        public void CanNotConvertToInt32_IdForGuid_IsTrue()
        {
            TypeConverterAssert.CanNotConvertTo(typeof(Id<ForGuid>), typeof(int));
        }

        [Test]
        public void CanConvertFromString_IdForGuid_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(Id<ForGuid>));
        }

        [Test]
        public void CanConvertToString_IdForGuid_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(Id<ForGuid>));
        }

        [Test]
        public void ConvertFrom_StringNull_IdForGuidEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(Id<ForGuid>.Empty, (string)null);
            }
        }

        [Test]
        public void ConvertFrom_StringEmpty_IdForGuid_Empty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(Id<ForGuid>.Empty, string.Empty);
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

        [TestCase(null)]
        [TestCase("")]
        [TestCase("Complex")]
        public void IsInvalid_String(string str)
        {
            Assert.IsFalse(Id<ForGuid>.IsValid(str));
        }

        [TestCase("0F5AB5AB-12CB-4629-878D-B18B88B9A504")]
        [TestCase("Qowaiv_SVOLibrary_GUIA")]
        public void IsValid_String(string str)
        {
            Assert.IsTrue(Id<ForGuid>.IsValid(str));
        }
    }

    [Serializable]
    public class IdForGuidSerializeObject
    {
        public int Id { get; set; }
        public Id<ForGuid> Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
