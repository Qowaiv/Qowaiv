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
    /// <summary>Tests the date span SVO.</summary>
    public class DateSpanTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly DateSpan TestStruct = new DateSpan(10, 3, -5);

        #region date span const tests

        /// <summary>DateSpan.Zero should be equal to the default of date span.</summary>
        [Test]
        public void Zero_None_EqualsDefault()
        {
            Assert.AreEqual(default(DateSpan), DateSpan.Zero);
        }

        #endregion

        #region TryParse tests

        /// <summary>TryParse null should be valid.</summary>
        [Test]
        public void TyrParse_Null_IsValid()
        {
            DateSpan val;

            string str = null;

            Assert.IsTrue(DateSpan.TryParse(str, out val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TyrParse_StringZero_IsValid()
        {
            DateSpan val;

            string str = string.Empty;

            Assert.IsTrue(DateSpan.TryParse(str, out val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TyrParse_StringValue_IsValid()
        {
            DateSpan val;

            string str = "string";

            Assert.IsTrue(DateSpan.TryParse(str, out val), "Valid");
            Assert.AreEqual(str, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [Test]
        public void TyrParse_StringValue_IsNotValid()
        {
            DateSpan val;

            string str = "string";

            Assert.IsFalse(DateSpan.TryParse(str, out val), "Valid");
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
                    DateSpan.Parse("InvalidInput");
                },
                "Not a valid date span");
            }
        }

        [Test]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = TestStruct;
                var act = DateSpan.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void TryParse_InvalidInput_DefaultValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = default(DateSpan);
                var act = DateSpan.TryParse("InvalidInput");

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
                SerializationTest.DeserializeUsingConstructor<DateSpan>
        (null, default(StreamingContext));
            },
            "info");
        }

        [Test]
        public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
        {
            Assert.Catch<SerializationException>
            (() =>
            {
                var info = new SerializationInfo(typeof(DateSpan), new System.Runtime.Serialization.FormatterConverter());
                SerializationTest.DeserializeUsingConstructor<DateSpan>
        (info, default(StreamingContext));
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
            var info = new SerializationInfo(typeof(DateSpan), new System.Runtime.Serialization.FormatterConverter());
            obj.GetObjectData(info, default(StreamingContext));

            Assert.AreEqual(532575944699UL, info.GetUInt64("Value"));
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
        public void SerializeDeserialize_DateSpanSerializeObject_AreEqual()
        {
            var input = new DateSpanSerializeObject()
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new DateSpanSerializeObject()
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
        public void XmlSerializeDeserialize_DateSpanSerializeObject_AreEqual()
        {
            var input = new DateSpanSerializeObject()
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new DateSpanSerializeObject()
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date"); ;
        }
        [Test]
        public void DataContractSerializeDeserialize_DateSpanSerializeObject_AreEqual()
        {
            var input = new DateSpanSerializeObject()
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new DateSpanSerializeObject()
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date"); ;
        }

        [Test]
        public void SerializeDeserialize_Default_AreEqual()
        {
            var input = new DateSpanSerializeObject()
            {
                Id = 17,
                Obj = default(DateSpan),
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new DateSpanSerializeObject()
            {
                Id = 17,
                Obj = default(DateSpan),
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void XmlSerializeDeserialize_Default_AreEqual()
        {
            var input = new DateSpanSerializeObject()
            {
                Id = 17,
                Obj = default(DateSpan),
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new DateSpanSerializeObject()
            {
                Id = 17,
                Obj = default(DateSpan),
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
        public void FromJson_Null_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>
            (() =>
            {
                JsonTester.Read<DateSpan>();
            },
            "JSON deserialization from null is not supported.");
        }

        [Test]
        public void FromJson_InvalidStringValue_AssertFormatException()
        {
            Assert.Catch<FormatException>
            (() =>
            {
                JsonTester.Read<DateSpan>("InvalidStringValue");
            },
            "Not a valid date span");
        }
        [Test]
        public void FromJson_StringValue_AreEqual()
        {
            var act = JsonTester.Read<DateSpan>(TestStruct.ToString(CultureInfo.InvariantCulture));
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_DoubleValue_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>
            (() =>
            {
                JsonTester.Read<DateSpan>(1234.56);
            },
            "JSON deserialization from a number is not supported.");
        }

        [Test]
        public void FromJson_DateTimeValue_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>
            (() =>
            {
                JsonTester.Read<DateSpan>(new DateTime(1972, 02, 14));
            },
            "JSON deserialization from a date is not supported.");
        }

        [Test]
        public void ToJson_DefaultValue_AreEqual()
        {
            object act = JsonTester.Write(default(DateSpan));
            object exp = "0D";

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

        #region IFormattable / Tostring tests

        [Test]
        public void ToString_Zero_StringZero()
        {
            var act = DateSpan.Zero.ToString();
            var exp = "";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: 'Some Formatted Value', format: 'Unit Test Format'";

            Assert.AreEqual(exp, act);
        }

        [TestCase("0D", 0, 0)]
        [TestCase("+1D", 0, 1)]
        [TestCase("+1M", 1, 0)]
        [TestCase("+1Y", 12, 0)]
        [TestCase("+1Y+1D", 12, 1)]
        [TestCase("+1Y+1D", 12, 1)]
        [TestCase("+1Y+1M+1D", 13, 1)]
        [TestCase("-11D", 0, -11)]
        [TestCase("+1M-12D", +1, -12)]
        [TestCase("-1M-12D", -1, -12)]
        [TestCase("-1Y-1M+1D", -13, 1)]
        public void ToString_Invariant(string expected, int months, int days)
        {
            using (CultureInfoScope.NewInvariant())
            {
                var span = new DateSpan(months, days);
                Assert.AreEqual(expected, span.ToString());
            }
        }

        [Test]
        public void DebuggerDisplay_DebugToString_HasAttribute()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(DateSpan));
        }

        [Test]
        public void DebuggerDisplay_DefaultValue_String()
        {
            DebuggerDisplayAssert.HasResult("0 Years, 0 Months, 0 Days", default(DateSpan));
        }

        [Test]
        public void DebuggerDisplay_TestStruct_String()
        {
            DebuggerDisplayAssert.HasResult("10 Years, 3 Months, -5 Days", TestStruct);
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for DateSpan.Zero.</summary>
        [Test]
        public void GetHash_Zero_Hash()
        {
            Assert.AreEqual(0, DateSpan.Zero.GetHashCode());
        }

        /// <summary>GetHash should not fail for the test struct.</summary>
        [Test]
        public void GetHash_TestStruct_Hash()
        {
            Assert.AreEqual(-128, TestStruct.GetHashCode());
        }

        [Test]
        public void Equals_ZeroZero_IsTrue()
        {
            Assert.IsTrue(DateSpan.Zero.Equals(DateSpan.Zero));
        }

        [Test]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = DateSpan.Parse("+3Y+3D", CultureInfo.InvariantCulture);
            var r = DateSpan.Parse("+36m+3d", CultureInfo.InvariantCulture);

            Assert.IsTrue(l.Equals(r));
        }

        [Test]
        public void Equals_TestStructTestStruct_IsTrue()
        {
            Assert.IsTrue(TestStruct.Equals(TestStruct));
        }

        [Test]
        public void Equals_TestStructZero_IsFalse()
        {
            Assert.IsFalse(TestStruct.Equals(DateSpan.Zero));
        }

        [Test]
        public void Equals_ZeroTestStruct_IsFalse()
        {
            Assert.IsFalse(DateSpan.Zero.Equals(TestStruct));
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

        [Test]
        public void DaysPerMonth_ShouldMatchTheCalculatedValue()
        {
            var time = new Date(2000, 01, 01) - Date.MinValue;
            double daysPerMonth = time.TotalDays / 2000 / 12;
            Assert.AreEqual(daysPerMonth, DateSpan.DaysPerMonth);
        }

        /// <summary>Orders a list of date spans ascending.</summary>
        [Test]
        public void OrderBy_DateSpan_AreEqual()
        {
            var item0 = new DateSpan(0, 00, -1);
            var item1 = new DateSpan(1, +2, 0);
            var item2 = new DateSpan(0, 00, +500);
            var item3 = new DateSpan(4, 00, -40);

            var inp = new List<DateSpan>() { DateSpan.Zero, item3, item2, item0, item1, DateSpan.Zero };
            var exp = new List<DateSpan>() { item0, DateSpan.Zero, DateSpan.Zero,  item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of date spans descending.</summary>
        [Test]
        public void OrderByDescending_DateSpan_AreEqual()
        {
            var item0 = new DateSpan(0, 00, -1);
            var item1 = new DateSpan(1, +2, 0);
            var item2 = new DateSpan(0, 00, +500);
            var item3 = new DateSpan(4, 00, -40);

            var inp = new List<DateSpan>() { DateSpan.Zero, item3, item2, item0, item1, DateSpan.Zero };
            var exp = new List<DateSpan>() { item3, item2, item1, DateSpan.Zero, DateSpan.Zero, item0 };
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
                TestStruct.CompareTo(null);
            },
            "obj",
            "Argument must be a date span"
            );
        }
        /// <summary>Compare with a random object should throw an exception.</summary>
        [Test]
        public void CompareTo_newObject_ThrowsArgumentException()
        {
            ExceptionAssert.CatchArgumentException
            (() =>
            {
                TestStruct.CompareTo(new object());
            },
            "obj",
            "Argument must be a date span"
            );
        }
        #endregion
    
        #region Properties
        #endregion

        #region Operations

        [TestCase(+0, +364, "2018-06-10", "2017-06-11", DateSpanSettings.DaysOnly)]
        [TestCase(+0, -364, "2017-06-11", "2018-06-10", DateSpanSettings.DaysOnly)]
        [TestCase(+11, +30, "2018-06-10", "2017-06-11", DateSpanSettings.Default)]
        [TestCase(+12, -01, "2018-06-10", "2017-06-11", DateSpanSettings.MixedSigns)]
        [TestCase(+15, +14, "2018-06-10", "2017-02-27", DateSpanSettings.Default)]
        [TestCase(+15, +11, "2018-06-10", "2017-02-27", DateSpanSettings.DaysFirst)]
        [TestCase(+12, +362, "2019-10-08", "2017-06-11", DateSpanSettings.WithoutMonths)]
        [TestCase(+24, +362, "2020-10-08", "2017-06-11", DateSpanSettings.WithoutMonths)]
        [TestCase(-11, -30, "2017-06-11", "2018-06-10", DateSpanSettings.Default)]
        [TestCase(-12, +01, "2017-06-11","2018-06-10", DateSpanSettings.MixedSigns)]
        public void Subtract(int months, int days, Date t1, Date t2, DateSpanSettings settings)
        {
            var span = DateSpan.Subtract(t1, t2, settings);
            var expected = new DateSpan(0, months, days);
            Assert.AreEqual(expected, span);
        }

        #endregion

        #region Type converter tests

        [Test]
        public void ConverterExists_DateSpan_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(DateSpan));
        }

        [Test]
        public void CanNotConvertFromInt32_DateSpan_IsTrue()
        {
            TypeConverterAssert.CanNotConvertFrom(typeof(DateSpan), typeof(Int32));
        }
        [Test]
        public void CanNotConvertToInt32_DateSpan_IsTrue()
        {
            TypeConverterAssert.CanNotConvertTo(typeof(DateSpan), typeof(Int32));
        }

        [Test]
        public void CanConvertFromString_DateSpan_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(DateSpan));
        }

        [Test]
        public void CanConvertToString_DateSpan_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(DateSpan));
        }

        [Test]
        public void ConvertFrom_StringZero_DateSpanZero()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(DateSpan.Zero, string.Empty);
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
        public void ConvertFromInstanceDescriptor_DateSpan_Successful()
        {
            TypeConverterAssert.ConvertFromInstanceDescriptor(typeof(DateSpan));
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
    }

    [Serializable]
    public class DateSpanSerializeObject
    {
        public int Id { get; set; }
        public DateSpan Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
