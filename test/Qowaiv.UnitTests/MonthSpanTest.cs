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
    namespace Qowaiv.UnitTests
    {
        /// <summary>Tests the month span SVO.</summary>
        public class MonthSpanTest
        {
            /// <summary>The test instance for most tests.</summary>
            public static readonly MonthSpan TestStruct = MonthSpan.FromMonths(69);

            /// <summary>MonthSpan.Zero should be equal to the default of month span.</summary>
            [Test]
            public void Zero_EqualsDefault()
            {
                Assert.AreEqual(default(MonthSpan), MonthSpan.Zero);
            }

            /// <summary>TryParse null should be valid.</summary>
            [Test]
            public void TyrParse_Null_IsValid()
            {
                Assert.IsTrue(MonthSpan.TryParse(null, out var val));
                Assert.AreEqual(default(MonthSpan), val);
            }

            /// <summary>TryParse string.Empty should be valid.</summary>
            [Test]
            public void TyrParse_StringEmpty_IsValid()
            {
                Assert.IsTrue(MonthSpan.TryParse(string.Empty, out var val));
                Assert.AreEqual(default(MonthSpan), val);
            }

            /// <summary>TryParse with specified string value should be valid.</summary>
            [Test]
            public void TyrParse_StringValue_IsValid()
            {
                string str = "0Y+0M";
                Assert.IsTrue(MonthSpan.TryParse(str, out var val));
                Assert.AreEqual(str, val.ToString());
            }

            /// <summary>TryParse with specified string value should be invalid.</summary>
            [Test]
            public void TyrParse_StringValue_IsNotValid()
            {
                string str = "5Y#9M";
                Assert.IsFalse(MonthSpan.TryParse(str, out var val));
                Assert.AreEqual(default(MonthSpan), val);
            }

            [Test]
            public void Parse_InvalidInput_ThrowsFormatException()
            {
                using (new CultureInfoScope("en-GB"))
                {
                    Assert.Catch<FormatException>(() =>
                    {
                        MonthSpan.Parse("InvalidInput");
                    }
                    , "Not a valid month span");
                }
            }

            [Test]
            public void TryParse_TestStructInput_AreEqual()
            {
                using (new CultureInfoScope("en-GB"))
                {
                    var exp = TestStruct;
                    var act = MonthSpan.TryParse(exp.ToString());
                    Assert.AreEqual(exp, act);
                }
            }

            [Test]
            public void TryParse_InvalidInput_DefaultValue()
            {
                using (new CultureInfoScope("en-GB"))
                {
                    var exp = default(MonthSpan);
                    var act = MonthSpan.TryParse("InvalidInput");
                    Assert.AreEqual(exp, act);
                }
            }

            [Test]
            public void Constructor_SerializationInfoIsNull_Throws()
            {
                Assert.Catch<ArgumentNullException>(() => SerializationTest.DeserializeUsingConstructor<MonthSpan>(null, default));
            }

            [Test]
            public void Constructor_InvalidSerializationInfo_Throws()
            {
                var info = new SerializationInfo(typeof(MonthSpan), new FormatterConverter());
                Assert.Catch<SerializationException>(() => SerializationTest.DeserializeUsingConstructor<MonthSpan>(info, default));
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
                var info = new SerializationInfo(typeof(MonthSpan), new FormatterConverter());
                obj.GetObjectData(info, default);
                Assert.AreEqual(69, info.GetValue("Value", typeof(int)));
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
                var exp = "5Y+9M";
                Assert.AreEqual(exp, act);
            }

            [Test]
            public void XmlDeserialize_XmlString_AreEqual()
            {
                var act = SerializationTest.XmlDeserialize<MonthSpan>("69");
                Assert.AreEqual(TestStruct, act);
            }

            [Test]
            public void SerializeDeserialize_MonthSpanSerializeObject_AreEqual()
            {
                var input = new MonthSpanSerializeObject { Id = 17, Obj = TestStruct, Date = new DateTime(1970, 02, 14), };
                var exp = new MonthSpanSerializeObject { Id = 17, Obj = TestStruct, Date = new DateTime(1970, 02, 14), };
                var act = SerializationTest.SerializeDeserialize(input);
                Assert.AreEqual(exp.Id, act.Id, "Id");
                Assert.AreEqual(exp.Obj, act.Obj, "Obj");
                Assert.AreEqual(exp.Date, act.Date, "Date");
            }

            [Test]
            public void XmlSerializeDeserialize_MonthSpanSerializeObject_AreEqual()
            {
                var input = new MonthSpanSerializeObject { Id = 17, Obj = TestStruct, Date = new DateTime(1970, 02, 14), };
                var exp = new MonthSpanSerializeObject { Id = 17, Obj = TestStruct, Date = new DateTime(1970, 02, 14), };
                var act = SerializationTest.XmlSerializeDeserialize(input);
                Assert.AreEqual(exp.Id, act.Id, "Id");
                Assert.AreEqual(exp.Obj, act.Obj, "Obj");
                Assert.AreEqual(exp.Date, act.Date, "Date");
            }

            [Test]
            public void DataContractSerializeDeserialize_MonthSpanSerializeObject_AreEqual()
            {
                var input = new MonthSpanSerializeObject { Id = 17, Obj = TestStruct, Date = new DateTime(1970, 02, 14), };
                var exp = new MonthSpanSerializeObject { Id = 17, Obj = TestStruct, Date = new DateTime(1970, 02, 14), };
                var act = SerializationTest.DataContractSerializeDeserialize(input);
                Assert.AreEqual(exp.Id, act.Id, "Id");
                Assert.AreEqual(exp.Obj, act.Obj, "Obj");
                Assert.AreEqual(exp.Date, act.Date, "Date");
            }

            [Test]
            public void SerializeDeserialize_Default_AreEqual()
            {
                var input = new MonthSpanSerializeObject { Id = 17, Obj = default, Date = new DateTime(1970, 02, 14), };
                var exp = new MonthSpanSerializeObject { Id = 17, Obj = default, Date = new DateTime(1970, 02, 14), };
                var act = SerializationTest.SerializeDeserialize(input);
                Assert.AreEqual(exp.Id, act.Id, "Id");
                Assert.AreEqual(exp.Obj, act.Obj, "Obj");
                Assert.AreEqual(exp.Date, act.Date, "Date");
            }

            [Test]
            public void XmlSerializeDeserialize_Default_AreEqual()
            {
                var input = new MonthSpanSerializeObject { Id = 17, Obj = default, Date = new DateTime(1970, 02, 14), };
                var exp = new MonthSpanSerializeObject { Id = 17, Obj = default, Date = new DateTime(1970, 02, 14), };
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
            [TestCase((long)int.MinValue)]
            public void FromJson_Invalid_Throws(object json)
            {
                Assert.Catch(() => JsonTester.Read<MonthSpan>(json));
            }

            [TestCase("5Y+9M", 69L)]
            [TestCase("5Y+9M", "5Y+9M")]
                public void FromJson(MonthSpan expected, object json)
            {
                var actual = JsonTester.Read<MonthSpan>(json);
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void FromsYear_3_36M()
            {
                var span = MonthSpan.FromYears(3);
                Assert.AreEqual(MonthSpan.FromMonths(36), span);
            }

            [Test]
            public void ToString_Zero_StringEmpty()
            {
                var act = MonthSpan.Zero.ToString();
                var exp = "0Y+0M";
                Assert.AreEqual(exp, act);
            }

            [Test]
            public void ToString_CustomFormatter_SupportsCustomFormatting()
            {
                var act = TestStruct.ToString("0.00", new UnitTestFormatProvider());
                var exp = "Unit Test Formatter, value: '69.00', format: '0.00'";
                Assert.AreEqual(exp, act);
            }

            [Test]
            public void ToString_FormatValueSpanishEcuador_AreEqual()
            {
                var act = MonthSpan.Parse("1700").ToString("00000.0", new CultureInfo("es-EC"));
                var exp = "01700,0";
                Assert.AreEqual(exp, act);
            }

            [Test]
            public void DebuggerDisplay_DebugToString_HasAttribute()
            {
                DebuggerDisplayAssert.HasAttribute(typeof(MonthSpan));
            }

            [Test]
            public void DebuggerDisplay_DefaultValue_String()
            {
                DebuggerDisplayAssert.HasResult("0Y+0M", default(MonthSpan));
            }

            [Test]
            public void DebuggerDisplay_TestStruct_String()
            {
                DebuggerDisplayAssert.HasResult("5Y+9M", TestStruct);
            }

            /// <summary>GetHash should not fail for MonthSpan.Zero.</summary>
            [Test]
            public void GetHash_Zero_Hash()
            {
                Assert.AreEqual(0, MonthSpan.Zero.GetHashCode());
            }

            /// <summary>GetHash should not fail for the test struct.</summary>
            [Test]
            public void GetHash_TestStruct_Hash()
            {
                Assert.AreEqual(69, TestStruct.GetHashCode());
            }

            [Test]
            public void Equals_EmptyEmpty_IsTrue()
            {
                Assert.IsTrue(MonthSpan.Zero.Equals(MonthSpan.Zero));
            }

            [Test]
            public void Equals_FormattedAndUnformatted_IsTrue()
            {
                var l = MonthSpan.Parse("69", CultureInfo.InvariantCulture);
                var r = MonthSpan.Parse("5Y+9M", CultureInfo.InvariantCulture);
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
                Assert.IsFalse(TestStruct.Equals(MonthSpan.Zero));
            }

            [Test]
            public void Equals_EmptyTestStruct_IsFalse()
            {
                Assert.IsFalse(MonthSpan.Zero.Equals(TestStruct));
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

            /// <summary>Orders a list of month spans ascending.</summary>
            [Test]
            public void OrderBy_MonthSpan_AreEqual()
            {
                var item0 = MonthSpan.FromMonths(1);
                var item1 = MonthSpan.FromMonths(12);
                var item2 = MonthSpan.FromMonths(13);
                var item3 = MonthSpan.FromMonths(145);
                var inp = new List<MonthSpan> { MonthSpan.Zero, item3, item2, item0, item1, MonthSpan.Zero };
                var exp = new List<MonthSpan> { MonthSpan.Zero, MonthSpan.Zero, item0, item1, item2, item3 };
                var act = inp.OrderBy(item => item).ToList();
                CollectionAssert.AreEqual(exp, act);
            }

            /// <summary>Orders a list of month spans descending.</summary>
            [Test]
            public void OrderByDescending_MonthSpan_AreEqual()
            {
                var item0 = MonthSpan.FromMonths(1);
                var item1 = MonthSpan.FromMonths(12);
                var item2 = MonthSpan.FromMonths(13);
                var item3 = MonthSpan.FromMonths(145);
                var inp = new List<MonthSpan> { MonthSpan.Zero, item3, item2, item0, item1, MonthSpan.Zero };
                var exp = new List<MonthSpan> { item3, item2, item1, item0, MonthSpan.Zero, MonthSpan.Zero };
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
                Assert.AreEqual("Argument must be MonthSpan. (Parameter 'obj')", x.Message);
            }

            [Test]
            public void LessThan_17LT19_IsTrue()
            {
                MonthSpan l = MonthSpan.FromMonths(17);
                MonthSpan r = MonthSpan.FromMonths(19);
                Assert.IsTrue(l < r);
            }

            [Test]
            public void GreaterThan_21LT19_IsTrue()
            {
                MonthSpan l = MonthSpan.FromMonths(21);
                MonthSpan r = MonthSpan.FromMonths(19);
                Assert.IsTrue(l > r);
            }

            [Test]
            public void LessThanOrEqual_17LT19_IsTrue()
            {
                MonthSpan l = MonthSpan.FromMonths(17);
                MonthSpan r = MonthSpan.FromMonths(19);
                Assert.IsTrue(l <= r);
            }

            [Test]
            public void GreaterThanOrEqual_21LT19_IsTrue()
            {
                MonthSpan l =  MonthSpan.FromMonths(21);
                MonthSpan r = MonthSpan.FromMonths(19);
                Assert.IsTrue(l >= r);
            }

            [Test]
            public void LessThanOrEqual_17LT17_IsTrue()
            {
                MonthSpan l =  MonthSpan.FromMonths(17);
                MonthSpan r = MonthSpan.FromMonths(17);
                Assert.IsTrue(l <= r);
            }

            [Test]
            public void GreaterThanOrEqual_21LT21_IsTrue()
            {
                MonthSpan l = MonthSpan.FromMonths(21);
                MonthSpan r = MonthSpan.FromMonths(21);
                Assert.IsTrue(l >= r);
            }

            [Test]
            public void Explicit_StringToMonthSpan_AreEqual()
            {
                var exp = TestStruct;
                var act = (MonthSpan)TestStruct.ToString();
                Assert.AreEqual(exp, act);
            }

            [Test]
            public void Explicit_MonthSpanToString_AreEqual()
            {
                var exp = TestStruct.ToString();
                var act = (string)TestStruct;
                Assert.AreEqual(exp, act);
            }

            [Test]
            public void Explicit_Int32ToMonthSpan_AreEqual()
            {
                var exp = TestStruct;
                var act = (MonthSpan)69;
                Assert.AreEqual(exp, act);
            }

            [Test]
            public void Explicit_MonthSpanToInt32_AreEqual()
            {
                var exp = 69;
                var act = (int)TestStruct;
                Assert.AreEqual(exp, act);
            }

            [Test]
            public void ConverterExists_MonthSpan_IsTrue()
            {
                TypeConverterAssert.ConverterExists(typeof(MonthSpan));
            }

            [Test]
            public void CanConvertFromString_MonthSpan_IsTrue()
            {
                TypeConverterAssert.CanConvertFromString(typeof(MonthSpan));
            }

            [Test]
            public void CanConvertToString_MonthSpan_IsTrue()
            {
                TypeConverterAssert.CanConvertToString(typeof(MonthSpan));
            }

            [Test]
            public void ConvertFrom_StringNull_MonthSpanEmpty()
            {
                using (new CultureInfoScope("en-GB"))
                {
                    TypeConverterAssert.ConvertFromEquals(MonthSpan.Zero, (string)null);
                }
            }

            [Test]
            public void ConvertFrom_StringEmpty_MonthSpanEmpty()
            {
                using (new CultureInfoScope("en-GB"))
                {
                    TypeConverterAssert.ConvertFromEquals(MonthSpan.Zero, string.Empty);
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
                Assert.IsFalse(MonthSpan.IsValid(str));
            }

            [TestCase("-30Y+50M+2D")]
            [TestCase("+2Y+0M")]
            [TestCase("69")]
            public void IsValid_String(string str)
            {
                Assert.IsTrue(MonthSpan.IsValid(str));
            }
        }

        [Serializable]
        public class MonthSpanSerializeObject
        {
            public int Id { get; set; }
            public MonthSpan Obj { get; set; }
            public DateTime Date { get; set; }
        }
    }
}
