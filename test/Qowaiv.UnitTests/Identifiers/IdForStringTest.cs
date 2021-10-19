using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.Identifiers;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests.Identifiers
{
    public class ForString : StringIdBehavior { }

    /// <summary>Tests the identifier SVO.</summary>
    public class IdForStringTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly Id<ForString> TestStruct = Id<ForString>.Parse("Qowaiv-ID");

        /// <summary>Id<ForString>.Empty should be equal to the default of identifier.</summary>
        [Test]
        public void Empty_None_EqualsDefault()
        {
            Assert.AreEqual(default(Id<ForString>), Id<ForString>.Empty);
        }

        /// <summary>Id<ForString>.IsEmpty() should be true for the default of identifier.</summary>
        [Test]
        public void IsEmpty_Default_IsTrue()
        {
            Assert.IsTrue(default(Id<ForString>).IsEmpty());
        }

        /// <summary>Id<ForString>.IsEmpty() should be false for the TestStruct.</summary>
        [Test]
        public void IsEmpty_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmpty());
        }

        [Test]
        public void FromBytes_Null_IsEmpty()
        {
            var fromBytes = Id<ForString>.FromBytes(null);
            Assert.AreEqual(Id<ForString>.Empty, fromBytes);
        }

        [Test]
        public void FromBytes_Bytes_IsTestStruct()
        {
            var fromBytes = Id<ForString>.FromBytes(new byte[] { 81, 111, 119, 97, 105, 118, 45, 73, 68 });
            Assert.AreEqual(TestStruct, fromBytes);
        }

        [Test]
        public void ToByteArray_Empty_EmptyArray()
        {
            var bytes = Id<ForString>.Empty.ToByteArray();
            Assert.AreEqual(Array.Empty<byte>(), bytes);
        }

        [Test]
        public void ToByteArray_TestStruct_FilledArray()
        {
            var bytes = TestStruct.ToByteArray();
            Console.WriteLine(string.Join(", ", bytes));
            var exepected = new byte[] { 81, 111, 119, 97, 105, 118, 45, 73, 68 };
            Assert.AreEqual(exepected, bytes);
        }

        /// <summary>TryParse null should be valid.</summary>
        [Test]
        public void TryParse_Null_IsValid()
        {
            Assert.IsTrue(Id<ForString>.TryParse(null, out var val));
            Assert.AreEqual(default(Id<ForString>), val);
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TryParse_StringEmpty_IsValid()
        {
            Assert.IsTrue(Id<ForString>.TryParse(string.Empty, out var val));
            Assert.AreEqual(default(Id<ForString>), val);
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TryParse_StringValue_IsValid()
        {
            string str = "0f5ab5ab-12cb-4629-878d-b18b88b9a504";
            Assert.IsTrue(Id<ForString>.TryParse(str, out var val));
            Assert.AreEqual(str, val.ToString());
        }

        [Test]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var exp = TestStruct;
                var act = Id<ForString>.TryParse(exp.ToString());
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void Constructor_SerializationInfoIsNull_Throws()
        {
            Assert.Catch<ArgumentNullException>(() => SerializationTest.DeserializeUsingConstructor<Id<ForString>>(null, default));
        }

        [Test]
        public void Constructor_InvalidSerializationInfo_Throws()
        {
            var info = new SerializationInfo(typeof(Id<ForString>), new FormatterConverter());
            Assert.Catch<SerializationException>(() => SerializationTest.DeserializeUsingConstructor<Id<ForString>>(info, default));
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
            var info = new SerializationInfo(typeof(Id<ForString>), new FormatterConverter());
            obj.GetObjectData(info, default);
            Assert.AreEqual("Qowaiv-ID", info.GetValue("Value", typeof(string)));
        }

        [Test]
        public void SerializeDeserialize_TestStruct_AreEqual()
        {
            var input = TestStruct;
            var exp = TestStruct;
            var act = SerializationTest.BinaryFormatterSerializeDeserialize(input);
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
            var exp = "Qowaiv-ID";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void XmlDeserialize_XmlString_AreEqual()
        {
            var act = SerializationTest.XmlDeserialize<Id<ForString>>("Qowaiv-ID");
            Assert.AreEqual(TestStruct, act);
        }

        [Test]
        public void SerializeDeserialize_IdForStringSerializeObject_AreEqual()
        {
            var input = new IdForStringSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            }

            ;
            var exp = new IdForStringSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            }

            ;
            var act = SerializationTest.BinaryFormatterSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void XmlSerializeDeserialize_IdForStringSerializeObject_AreEqual()
        {
            var input = new IdForStringSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            }

            ;
            var exp = new IdForStringSerializeObject
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
        public void DataContractSerializeDeserialize_IdForStringSerializeObject_AreEqual()
        {
            var input = new IdForStringSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            }

            ;
            var exp = new IdForStringSerializeObject
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
            var input = new IdForStringSerializeObject
            {
                Id = 17,
                Obj = default,
                Date = new DateTime(1970, 02, 14),
            }

            ;
            var exp = new IdForStringSerializeObject
            {
                Id = 17,
                Obj = default,
                Date = new DateTime(1970, 02, 14),
            }

            ;
            var act = SerializationTest.BinaryFormatterSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void XmlSerializeDeserialize_Default_AreEqual()
        {
            var input = new IdForStringSerializeObject
            {
                Id = 17,
                Obj = default,
                Date = new DateTime(1970, 02, 14),
            }

            ;
            var exp = new IdForStringSerializeObject
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

        [TestCase("0F5AB5AB-12CB-4629-878D-B18B88B9A504", "0F5AB5AB-12CB-4629-878D-B18B88B9A504")]
        [TestCase("", "")]
        [TestCase("123456789", 123456789L)]
        public void FromJson(Id<ForString> expected, object json)
        {
            var actual = JsonTester.Read<Id<ForString>>(json);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_Empty_StringEmpty()
        {
            var act = Id<ForString>.Empty.ToString();
            var exp = "";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("S", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: 'Qowaiv-ID', format: 'S'";
            Assert.AreEqual(exp, act);
        }

        /// <summary>GetHash should not fail for Id<ForString>.Empty.</summary>
        [Test]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, Id<ForString>.Empty.GetHashCode());
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
            Assert.IsTrue(Id<ForString>.Empty.Equals(Id<ForString>.Empty));
        }

        [Test]
        public void Equals_TestStructTestStruct_IsTrue()
        {
            Assert.IsTrue(TestStruct.Equals(TestStruct));
        }

        [Test]
        public void Equals_TestStructEmpty_IsFalse()
        {
            Assert.IsFalse(TestStruct.Equals(Id<ForString>.Empty));
        }

        [Test]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(Id<ForString>.Empty.Equals(TestStruct));
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
        public void OrderBy_IdForString_AreEqual()
        {
            var item0 = Id<ForString>.Parse("45140308-9961-40D7-8907-31592772F556");
            var item1 = Id<ForString>.Parse("56AE40C9-A285-4B9B-A725-DE4B48F24BB0");
            var item2 = Id<ForString>.Parse("63FBD011-595C-46EB-AF59-5E4C6AD23C41");
            var item3 = Id<ForString>.Parse("BE236238-7A6D-4CE3-A955-B3D672593B8F");
            var inp = new List<Id<ForString>> { Id<ForString>.Empty, item3, item2, item0, item1, Id<ForString>.Empty };
            var exp = new List<Id<ForString>> { Id<ForString>.Empty, Id<ForString>.Empty, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();
            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of identifiers descending.</summary>
        [Test]
        public void OrderByDescending_IdForString_AreEqual()
        {
            var item0 = Id<ForString>.Parse("45140308-9961-40D7-8907-31592772F556");
            var item1 = Id<ForString>.Parse("56AE40C9-A285-4B9B-A725-DE4B48F24BB0");
            var item2 = Id<ForString>.Parse("63FBD011-595C-46EB-AF59-5E4C6AD23C41");
            var item3 = Id<ForString>.Parse("BE236238-7A6D-4CE3-A955-B3D672593B8F");
            var inp = new List<Id<ForString>> { Id<ForString>.Empty, item3, item2, item0, item1, Id<ForString>.Empty };
            var exp = new List<Id<ForString>> { item3, item2, item1, item0, Id<ForString>.Empty, Id<ForString>.Empty };
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
            Assert.AreEqual("Argument must be Id<ForString>. (Parameter 'obj')", x.Message);
        }

        [Test]
        public void ConverterExists_IdForString_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(Id<ForString>));
        }

        [Test]
        public void CanNotConvertToInt32_IdForString_IsTrue()
        {
            TypeConverterAssert.CanNotConvertTo(typeof(Id<ForString>), typeof(int));
        }

        [Test]
        public void CanConvertFromString_IdForString_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(Id<ForString>));
        }

        [Test]
        public void CanConvertToString_IdForString_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(Id<ForString>));
        }

        [Test]
        public void ConvertFrom_StringNull_IdForStringEmpty()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(Id<ForString>.Empty, (string)null);
            }
        }

        [Test]
        public void ConvertFrom_StringEmpty_IdForString_Empty()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(Id<ForString>.Empty, string.Empty);
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

        [Test]
        public void Next_NotSupported()
        {
            Assert.Throws<NotSupportedException>(()=> Id<ForString>.Next());
        }

        [TestCase(null)]
        [TestCase("")]
        public void IsInvalid_String(string str)
        {
            Assert.IsFalse(Id<ForString>.IsValid(str));
        }

        [TestCase("0F5AB5AB-12CB-4629-878D-B18B88B9A504")]
        [TestCase("Qowaiv_SVOLibrary_GUIA")]
        public void IsValid_String(string str)
        {
            Assert.IsTrue(Id<ForString>.IsValid(str));
        }
    }

    [Serializable]
    public class IdForStringSerializeObject
    {
        public int Id { get; set; }
        public Id<ForString> Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
