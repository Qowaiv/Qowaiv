using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;
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
        public static readonly DateSpan Smaller = new DateSpan(10, 3, -5);
        public static readonly DateSpan Bigger = new DateSpan(10, 3, +02);

        #region date span const tests

        /// <summary>DateSpan.Zero should be equal to the default of date span.</summary>
        [Test]
        public void Zero_None_EqualsDefault()
        {
            Assert.AreEqual(default(DateSpan), DateSpan.Zero);
        }

        [Test]
        public void MaxValue_EqualsDateMaxDateMin()
        {
            var max = DateSpan.Subtract(Date.MaxValue, Date.MinValue);
            Assert.AreEqual(DateSpan.MaxValue, max);
        }

        [Test]
        public void MinValue_EqualsDateMinDateMax()
        {
            var min = DateSpan.Subtract(Date.MinValue, Date.MaxValue);
            Assert.AreEqual(DateSpan.MinValue, min);
        }

        #endregion

        #region TryParse tests

        /// <summary>TryParse null should be valid.</summary>
        [Test]
        public void TryParse_Null_IsInvalid()
        {
            string str = null;
            Assert.IsFalse(DateSpan.TryParse(str, out _));
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TryParse_StringEmpty_IsInvalid()
        {
            string str = string.Empty;
            Assert.IsFalse(DateSpan.TryParse(str, out _));
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TryParse_StringValue_IsValid()
        {
            string str = "5Y+3M+2D";
            Assert.IsTrue(DateSpan.TryParse(str, out DateSpan val), "Valid");
            Assert.AreEqual(str, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [Test]
        public void TryParse_StringValue_IsInvalid()
        {
            string str = "InvalidString";
            Assert.IsFalse(DateSpan.TryParse(str, out _), "Valid");
        }

        [Test]
        public void Parse_InvalidInput_ThrowsFormatException()
        {
            using (TestCultures.En_GB.Scoped())
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
            using (TestCultures.En_GB.Scoped())
            {
                var exp = TestStruct;
                var act = DateSpan.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void TryParse_InvalidInput_DefaultValue()
        {
            using (TestCultures.En_GB.Scoped())
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
                SerializationTest.DeserializeUsingConstructor<DateSpan>(null, default);
            },
            "info");
        }

        [Test]
        public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
        {
            Assert.Catch<SerializationException>
            (() =>
            {
                var info = new SerializationInfo(typeof(DateSpan), new FormatterConverter());
                SerializationTest.DeserializeUsingConstructor<DateSpan>(info, default);
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
            var info = new SerializationInfo(typeof(DateSpan), new FormatterConverter());
            obj.GetObjectData(info, default);

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
            var input = new DateSpanSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new DateSpanSerializeObject
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
            var input = new DateSpanSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new DateSpanSerializeObject
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
        public void DataContractSerializeDeserialize_DateSpanSerializeObject_AreEqual()
        {
            var input = new DateSpanSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new DateSpanSerializeObject
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
        public void SerializeDeserialize_Default_AreEqual()
        {
            var input = new DateSpanSerializeObject
            {
                Id = 17,
                Obj = default,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new DateSpanSerializeObject
            {
                Id = 17,
                Obj = default,
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
            var input = new DateSpanSerializeObject
            {
                Id = 17,
                Obj = default,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new DateSpanSerializeObject
            {
                Id = 17,
                Obj = default,
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
        public void FromJson_StringValue_AreEqual()
        {
            var act = JsonTester.Read<DateSpan>(TestStruct.ToString(CultureInfo.InvariantCulture));
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToJson_DefaultValue_AreEqual()
        {
            object act = JsonTester.Write(default(DateSpan));
            object exp = "0Y+0M+0D";

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
        public void ToString_Zero_0Y0M0D()
        {
            var act = DateSpan.Zero.ToString();
            var exp = "0Y+0M+0D";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: '10Y+3M-5D', format: 'Unit Test Format'";

            Assert.AreEqual(exp, act);
        }

        [TestCase("0Y+0M+0D", 0, 0)]
        [TestCase("0Y+0M+1D", 0, 1)]
        [TestCase("0Y+1M+0D", 1, 0)]
        [TestCase("1Y+0M+0D", 12, 0)]
        [TestCase("1Y+0M+1D", 12, 1)]
        [TestCase("1Y+0M+1D", 12, 1)]
        [TestCase("1Y+1M+1D", 13, 1)]
        [TestCase("0Y+0M-11D", 0, -11)]
        [TestCase("0Y+1M-12D", +1, -12)]
        [TestCase("0Y-1M-12D", -1, -12)]
        [TestCase("-1Y-1M+1D", -13, 1)]
        public void ToString_Invariant(string expected, int months, int days)
        {
            using (CultureInfoScope.NewInvariant())
            {
                var span = new DateSpan(months, days);
                Assert.AreEqual(expected, span.ToString());
            }
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
            Assert.AreNotEqual(0, TestStruct.GetHashCode());
        }

        [Test]
        public void Equals_ZeroZero_IsTrue()
        {
            Assert.IsTrue(DateSpan.Zero.Equals(DateSpan.Zero));
        }

        [Test]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = DateSpan.Parse("3Y-0M+3D", CultureInfo.InvariantCulture);
            var r = DateSpan.Parse("-0y+36m+3d", CultureInfo.InvariantCulture);

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
            Assert.AreEqual(daysPerMonth, DateSpan.DaysPerMonth, 0.000000001);
        }

        /// <summary>Orders a list of date spans ascending.</summary>
        [Test]
        public void OrderBy_DateSpan_AreEqual()
        {
            var item0 = new DateSpan(0, 00, -1);
            var item1 = new DateSpan(1, +2, 0);
            var item2 = new DateSpan(0, 00, +500);
            var item3 = new DateSpan(4, 00, -40);

            var inp = new List<DateSpan> { DateSpan.Zero, item3, item2, item0, item1, DateSpan.Zero };
            var exp = new List<DateSpan> { item0, DateSpan.Zero, DateSpan.Zero,  item1, item2, item3 };
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

            var inp = new List<DateSpan> { DateSpan.Zero, item3, item2, item0, item1, DateSpan.Zero };
            var exp = new List<DateSpan> { item3, item2, item1, DateSpan.Zero, DateSpan.Zero, item0 };
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

        /// <summary>Compare with null should return 1.</summary>
        [Test]
        public void CompareTo_null_1()
        {
            object @null = null;
            Assert.AreEqual(1, TestStruct.CompareTo(@null));
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
            "Argument must be DateSpan."
            );
        }

        [Test]
        public void Smaller_LessThan_Bigger_IsTrue()
        {
            Assert.IsTrue(Smaller < Bigger);
        }
        [Test]
        public void Bigger_GreaterThan_Smaller_IsTrue()
        {
            Assert.IsTrue(Bigger > Smaller);
        }

        [Test]
        public void Smaller_LessThanOrEqual_Bigger_IsTrue()
        {
            Assert.IsTrue(Smaller <= Bigger);
        }
        [Test]
        public void Bigger_GreaterThanOrEqual_Smaller_IsTrue()
        {
            Assert.IsTrue(Bigger >= Smaller);
        }

        [Test]
        public void Smaller_LessThanOrEqual_Smaller_IsTrue()
        {
            var left = Smaller;
            var right = Smaller;
            Assert.IsTrue(left <= right);
        }

        [Test]
        public void Smaller_GreaterThanOrEqual_Smaller_IsTrue()
        {
            var left = Smaller;
            var right = Smaller;
            Assert.IsTrue(left >= right);
        }

        #endregion

        #region Properties

        [TestCase(1, 2, +3)]
        [TestCase(0, 0, +3)]
        [TestCase(9, 6, +0)]
        [TestCase(9, 6, -1)]
        [TestCase(-9, -6, -1)]
        public void Days(int years, int months, int days)
        {
            var span = new DateSpan(years, months, days);
            Assert.AreEqual(days, span.Days);
        }

        [TestCase(1, 2, +3)]
        [TestCase(0, 0, +3)]
        [TestCase(9, 6, +0)]
        [TestCase(9, 6, -1)]
        [TestCase(-9, -6, -1)]
        public void Months(int years, int months, int days)
        {
            var span = new DateSpan(years, months, days);
            Assert.AreEqual(months, span.Months);
        }

        [TestCase(1, 2, +3)]
        [TestCase(0, 0, +3)]
        [TestCase(9, 6, +0)]
        [TestCase(9, 6, -1)]
        [TestCase(-9, -6, -1)]
        public void Years(int years, int months, int days)
        {
            var span = new DateSpan(years, months, days);
            Assert.AreEqual(years, span.Years);
        }

        [TestCase(014, 1, 2, +3)]
        [TestCase(012, 1, 0, +3)]
        [TestCase(117, 9, 9, +0)]
        [TestCase(006, 0, 6, -1)]
        [TestCase(-19, -1, -7, -1)]
        public void TotalMonths(int total, int years, int months, int days)
        {
            var span = new DateSpan(years, months, days);
            Assert.AreEqual(total, span.TotalMonths);
        }

        #endregion

        #region Operations

        [Test]
        public void Negate_TestStruct_Negated()
        {
            var negated = -TestStruct;
            Assert.AreEqual(new DateSpan(-10, -3, +5), negated);
        }
        [Test]
        public void Pluse_TestStruct_Unchanged()
        {
            var negated = +TestStruct;
            Assert.AreEqual(TestStruct, negated);
        }

        [Test]
        public void Mutate_Overflows()
        {
            var x = Assert.Catch<OverflowException>(()=> DateSpan.MaxValue.AddDays(1));
            Assert.AreEqual("DateSpan overflowed because the resulting duration is too long.", x.Message);
        }

        [Test]
        public void Add_DateSpan_AddsUp()
        {
            var l = new DateSpan(12, 3, 4);
            var r = new DateSpan(-2, 2, 7);

            var exp = new DateSpan(10, 5, 11);
            var act = l + r;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Subtract_DateSpan_AddsUp()
        {
            var l = new DateSpan(12, 3, 4);
            var r = new DateSpan(-2, 2, 7);

            var exp = new DateSpan(14, 1, -3);
            var act = l - r;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Add_Days_AddsUp()
        {
            var span = new DateSpan(12, 3, 4);
            var exp = new DateSpan(12, 3, 21);
            var act = span.AddDays(17);

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Add_Months_AddsUp()
        {
            var span = new DateSpan(12, 3, 4);
            var exp = new DateSpan(12, 20, 4);
            var act = span.AddMonths(17);

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Add_Years_AddsUp()
        {
            var span = new DateSpan(12, 3, 4);
            var exp = new DateSpan(29, 3, 4);
            var act = span.AddYears(17);

            Assert.AreEqual(exp, act);
        }


        [Test]
        public void Age_HasNoMonths()
        {
            using (Clock.SetTimeForCurrentThread(() => new Date(2019, 10, 10)))
            {
                var age = DateSpan.Age(new Date(2017, 06, 11));
                var exp = new DateSpan(2, 0, 121);
                Assert.AreEqual(exp, age);
            }
        }

        [TestCase(+0, +364, "2018-06-10", "2017-06-11", DateSpanSettings.DaysOnly)]
        [TestCase(+0, -364, "2017-06-11", "2018-06-10", DateSpanSettings.DaysOnly)]
        [TestCase(+11, +30, "2018-06-10", "2017-06-11", DateSpanSettings.Default)]
        [TestCase(+12, -01, "2018-06-10", "2017-06-11", DateSpanSettings.MixedSigns)]
        [TestCase(+15, +14, "2018-06-10", "2017-02-27", DateSpanSettings.Default)]
        [TestCase(+15, +11, "2018-06-10", "2017-02-27", DateSpanSettings.DaysFirst)]
        [TestCase(+24, +119, "2019-10-08", "2017-06-11", DateSpanSettings.WithoutMonths)]
        [TestCase(+36, +120, "2020-10-08", "2017-06-11", DateSpanSettings.WithoutMonths)]
        [TestCase(+12, +331, "2019-05-08", "2017-06-11", DateSpanSettings.WithoutMonths)]
        [TestCase(+24, +332, "2020-05-08", "2017-06-11", DateSpanSettings.WithoutMonths)]
        [TestCase(-11, -30, "2017-06-11", "2018-06-10", DateSpanSettings.Default)]
        [TestCase(-12, +01, "2017-06-11","2018-06-10", DateSpanSettings.MixedSigns)]
        public void Subtract(int months, int days, Date d1, Date d2, DateSpanSettings settings)
        {
            var span = DateSpan.Subtract(d1, d2, settings);
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

        #endregion

        [Test]
        public void Ctor_OutOfRange_Throws()
        {
            var x = Assert.Catch<ArgumentOutOfRangeException>(() => new DateSpan(int.MaxValue, int.MaxValue));
            Assert.AreEqual("The specified years, months and days results in an un-representable DateSpan.", x.Message);
        }

        [Test]
        public void FromDays_4_4Days()
        {
            var span = DateSpan.FromDays(4);
            var exp = new DateSpan(0, 4);

            Assert.AreEqual(exp, span);
        }

        [Test]
        public void FromMonths_17_17Months()
        {
            var span = DateSpan.FromMonths(17);
            var exp = new DateSpan(17, 0);

            Assert.AreEqual(exp, span);
        }

        [Test]
        public void FromYears_17_17Years()
        {
            var span = DateSpan.FromYears(17);
            var exp = new DateSpan(17, 0, 0);

            Assert.AreEqual(exp, span);
        }


        [TestCase("23Y+0M+0D", "Without starting sign")]
        [TestCase("+9998Y+0M+0D", "A lot of years")]
        [TestCase("-9998Y+0M+0D", "A lot of years")]
        [TestCase("0Y+100000M+1D", "A lot of months")]
        [TestCase("0Y-100000M+1D", "A lot of months")]
        [TestCase("0Y+0M+3650000D", "A lot of days")]
        [TestCase("0Y+0M-3650000D", "A lot of days")]
        public void IsValid(string val, string scenario)
        {
            Assert.IsTrue(DateSpan.IsValid(val), scenario);
        }

        [TestCase(null, "Null")]
        [TestCase("", "String.Empty")]
        [TestCase("234adf", "Noise")]
        [TestCase("+9999Y+0M+0D", "Years out of reach")]
        [TestCase("-9999Y+0M+0D", "Years out of reach")]
        [TestCase("0Y+0M+4650000D", "Days out of reach")]
        [TestCase("0Y+0M-4650000D", "Days out of reach")]
        public void IsInvalid(string val, string scenario)
        {
            Assert.IsFalse(DateSpan.IsValid(val), scenario);
        }
    }

    [Serializable]
    public class DateSpanSerializeObject
    {
        public int Id { get; set; }
        public DateSpan Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
