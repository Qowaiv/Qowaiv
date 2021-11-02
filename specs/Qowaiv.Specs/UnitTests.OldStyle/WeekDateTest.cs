using FluentAssertions;
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
    /// <summary>Tests the week date SVO.</summary>
    public class WeekDateTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly WeekDate TestStruct = new WeekDate(1997, 14, 6);

        #region week date const tests

        /// <summary>WeekDate.MinValue should be equal to the default of week date.</summary>
        [Test]
        public void MinValue_None_EqualsDefault()
        {
            Assert.AreEqual(default(WeekDate), WeekDate.MinValue);
        }

        #endregion

        #region week date constructor tests

        [Test]
        public void Ctor_Y0_ThrowsArgumentOutofRangeException()
        {
            Action create = () => new WeekDate(0000, 10, 4);
            create.Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithMessage("Year should be in range [1,9999].");
        }
        [Test]
        public void Ctor_Y10000_ThrowsArgumentOutofRangeException()
        {
            Action create = () => new WeekDate(10000, 10, 4);
            create.Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithMessage("Year should be in range [1,9999].");
        }

        [Test]
        public void Ctor_W0_ThrowsArgumentOutofRangeException()
        {
            Action create = () => new WeekDate(1980, 0, 4);
            create.Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithMessage("Week should be in range [1,53].");
        }
        [Test]
        public void Ctor_W54_ThrowsArgumentOutofRangeException()
        {
            Action create = () => new WeekDate(1980, 54, 4);
            create.Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithMessage("Week should be in range [1,53].");
        }

        [Test]
        public void Ctor_D0_ThrowsArgumentOutofRangeException()
        {
            Action create = () => new WeekDate(1980, 10, 0);
            create.Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithMessage("Day should be in range [1,7]."); 
        }
        [Test]
        public void Ctor_D8_ThrowsArgumentOutofRangeException()
        {
            Action create = () => new WeekDate(1980, 10, 8);
            create.Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithMessage("Day should be in range [1,7].");

        }

        [Test]
        public void Ctor_Y9999W52D6_ThrowsArgumentOutofRangeException()
        {
            Action create = () => new WeekDate(9999, 52, 6);
            create.Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithMessage("Year, Week, and Day parameters describe an un-representable Date.");
        }

        [Test]
        public void Ctor_Y9999W53D1_ThrowsArgumentOutofRangeException()
        {
            Action create = () => new WeekDate(9999, 32, 6);
            create.Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithMessage("Year, Week, and Day parameters describe an un-representable Date.");
        }

        #endregion

        #region TryParse tests

        /// <summary>TryParse null should be valid.</summary>
        [Test]
        public void TryParse_Null_IsInvalid()
        {
            string str = null;
            Assert.IsFalse(WeekDate.TryParse(str, out _), "Not valid");
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TryParse_StringEmpty_IsInvalid()
        {
            Assert.IsFalse(WeekDate.TryParse(string.Empty, out _), "Not valid");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TryParse_StringValue_IsValid()
        {
            string str = "1234-W50-6";
            Assert.IsTrue(WeekDate.TryParse(str, out WeekDate val), "Valid");
            Assert.AreEqual(str, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [Test]
        public void TryParse_StringValue_IsNotValid()
        {
            Assert.IsFalse(WeekDate.TryParse("string", out _), "Valid");
        }

        [Test]
        public void Parse_InvalidInput_ThrowsFormatException()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Assert.Catch<FormatException>
                (() =>
                {
                    WeekDate.Parse("InvalidInput");
                },
                "Not a valid week date");
            }
        }

        [Test]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var exp = TestStruct;
                var act = WeekDate.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void TryParse_InvalidInput_DefaultValue()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var exp = default(WeekDate);
                var act = WeekDate.TryParse("InvalidInput");

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void TryParse_Y0000W21D7_DefaultValue()
        {
            WeekDate exp = default;
            Assert.IsFalse(WeekDate.TryParse("0000-W21-7", out WeekDate act));
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void TryParse_Y2000W53D7_DefaultValue()
        {
            WeekDate exp = default;
            Assert.IsFalse(WeekDate.TryParse("2000-W53-7", out WeekDate act));
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region (XML) (De)serialization tests

        [Test]
        public void GetObjectData_SerializationInfo_AreEqual()
        {
            ISerializable obj = TestStruct;
            var info = new SerializationInfo(typeof(WeekDate), new System.Runtime.Serialization.FormatterConverter());
            obj.GetObjectData(info, default);

            Assert.AreEqual((DateTime)TestStruct, info.GetDateTime("Value"));
        }

        [Test]
        public void SerializeDeserialize_TestStruct_AreEqual()
        {
            var input = TestStruct;
            var exp = TestStruct;
            var act = SerializeDeserialize.Binary(input);
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void DataContractSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = TestStruct;
            var exp = TestStruct;
            var act = SerializeDeserialize.DataContract(input);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void XmlSerialize_TestStruct_AreEqual()
        {
            var act = Serialize.Xml(TestStruct);
            var exp = "1997-W14-6";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void XmlDeserialize_XmlString_AreEqual()
        {
            var act =Deserialize.Xml<WeekDate>("1997-W14-6");
            Assert.AreEqual(TestStruct, act);
        }

        [Test]
        public void SerializeDeserialize_WeekDateSerializeObject_AreEqual()
        {
            var input = new WeekDateSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new WeekDateSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializeDeserialize.Binary(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void XmlSerializeDeserialize_WeekDateSerializeObject_AreEqual()
        {
            var input = new WeekDateSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new WeekDateSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializeDeserialize.Xml(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void DataContractSerializeDeserialize_WeekDateSerializeObject_AreEqual()
        {
            var input = new WeekDateSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new WeekDateSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializeDeserialize.DataContract(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void SerializeDeserialize_MinValue_AreEqual()
        {
            var input = new WeekDateSerializeObject
            {
                Id = 17,
                Obj = WeekDate.MinValue,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new WeekDateSerializeObject
            {
                Id = 17,
                Obj = WeekDate.MinValue,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializeDeserialize.Binary(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void XmlSerializeDeserialize_Empty_AreEqual()
        {
            var input = new WeekDateSerializeObject
            {
                Id = 17,
                Obj = WeekDate.MinValue,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new WeekDateSerializeObject
            {
                Id = 17,
                Obj = WeekDate.MinValue,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializeDeserialize.Xml(input);
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

        [TestCase("Invalid input")]
        public void FromJson_Invalid_Throws(object json)
        {
            Assert.Catch<FormatException>(() => JsonTester.Read<WeekDate>(json));
        }

        [Test]
        public void FromJson_1997W14D6_EqualsTestStruct()
        {
            var actual = JsonTester.Read<WeekDate>("1997-W14-6");
            Assert.AreEqual(TestStruct, actual);
        }

        [Test]
        public void ToJson_DefaultValue_AreEqual()
        {
            object act = JsonTester.Write(default(WeekDate));
            object exp = "0001-W01-1";
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToJson_TestStruct_AreEqual()
        {
            var act = JsonTester.Write(TestStruct);
            var exp = "1997-W14-6";
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IFormattable / ToString tests

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("y#W", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: '1997#14', format: 'y#W'";

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_NullFormatProvider_FormattedString()
        {
            using (TestCultures.En_US.Scoped())
            {
                var act = TestStruct.ToString(@"y-\WW-d", null);
                var exp = "1997-W14-6";

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_TestStruct_ComplexPattern()
        {
            var act = TestStruct.ToString("");
            var exp = "1997-W14-6";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_Y1979W3D5FormatWUpper_ComplexPattern()
        {
            var act = new WeekDate(1979, 3, 5).ToString(@"y-\WW-d");
            var exp = "1979-W3-5";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_Y1979W3D5FormatWLower_ComplexPattern()
        {
            var act = new WeekDate(1979, 3, 5).ToString(@"y-\Ww-d");
            var exp = "1979-W03-5";
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for WeekDate.Empty.</summary>
        [Test]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, WeekDate.MinValue.GetHashCode());
        }

        /// <summary>GetHash should not fail for the test struct.</summary>
        [Test]
        public void GetHash_TestStruct_Hash()
        {
            Assert.AreEqual(2027589483, TestStruct.GetHashCode());
        }

        [Test]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = WeekDate.Parse("1997-14-6", CultureInfo.InvariantCulture);
            var r = WeekDate.Parse("1997-W14-6", CultureInfo.InvariantCulture);

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
            Assert.IsFalse(TestStruct.Equals(WeekDate.MinValue));
        }

        [Test]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(WeekDate.MinValue.Equals(TestStruct));
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

        /// <summary>Orders a list of week dates ascending.</summary>
        [Test]
        public void OrderBy_WeekDate_AreEqual()
        {
            var item0 = WeekDate.Parse("2000-W01-3");
            var item1 = WeekDate.Parse("2000-W11-2");
            var item2 = WeekDate.Parse("2000-W21-1");
            var item3 = WeekDate.Parse("2000-W31-7");

            var inp = new List<WeekDate> { WeekDate.MinValue, item3, item2, item0, item1, WeekDate.MinValue };
            var exp = new List<WeekDate> { WeekDate.MinValue, WeekDate.MinValue, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of week dates descending.</summary>
        [Test]
        public void OrderByDescending_WeekDate_AreEqual()
        {
            var item0 = WeekDate.Parse("2000-W01-3");
            var item1 = WeekDate.Parse("2000-W11-2");
            var item2 = WeekDate.Parse("2000-W21-1");
            var item3 = WeekDate.Parse("2000-W31-7");

            var inp = new List<WeekDate> { WeekDate.MinValue, item3, item2, item0, item1, WeekDate.MinValue };
            var exp = new List<WeekDate> { item3, item2, item1, item0, WeekDate.MinValue, WeekDate.MinValue };
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
            Action compare = () => TestStruct.CompareTo(new object());
            compare.Should().Throw<ArgumentException>();
        }

        [Test]
        public void LessThan_17LT19_IsTrue()
        {
            WeekDate l = new WeekDate(1980, 17, 5);
            WeekDate r = new WeekDate(1980, 19, 5);

            Assert.IsTrue(l < r);
        }
        [Test]
        public void GreaterThan_21LT19_IsTrue()
        {
            WeekDate l = new WeekDate(1980, 21, 5);
            WeekDate r = new WeekDate(1980, 19, 5);

            Assert.IsTrue(l > r);
        }

        [Test]
        public void LessThanOrEqual_17LT19_IsTrue()
        {
            WeekDate l = new WeekDate(1980, 17, 5);
            WeekDate r = new WeekDate(1980, 19, 5);

            Assert.IsTrue(l <= r);
        }
        [Test]
        public void GreaterThanOrEqual_21LT19_IsTrue()
        {
            WeekDate l = new WeekDate(1980, 21, 5);
            WeekDate r = new WeekDate(1980, 19, 5);

            Assert.IsTrue(l >= r);
        }

        [Test]
        public void LessThanOrEqual_17LT17_IsTrue()
        {
            WeekDate l = new WeekDate(1980, 17, 5);
            WeekDate r = new WeekDate(1980, 17, 5);

            Assert.IsTrue(l <= r);
        }
        [Test]
        public void GreaterThanOrEqual_21LT21_IsTrue()
        {
            WeekDate l = new WeekDate(1980, 21, 5);
            WeekDate r = new WeekDate(1980, 21, 5);

            Assert.IsTrue(l >= r);
        }
        #endregion

        #region Casting tests

        [Test]
        public void Explicit_StringToWeekDate_AreEqual()
        {
            var exp = TestStruct;
            var act = (WeekDate)TestStruct.ToString();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_WeekDateToString_AreEqual()
        {
            var exp = TestStruct.ToString();
            var act = (string)TestStruct;

            Assert.AreEqual(exp, act);
        }


        [Test]
        public void Explicit_Int32ToWeekDate_AreEqual()
        {
            var exp = TestStruct;
            var act = (WeekDate)new DateTime(1997, 04, 05);

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_WeekDateToInt32_AreEqual()
        {
            DateTime exp = new DateTime(1997, 04, 05);
            DateTime act = TestStruct;

            Assert.AreEqual(exp, act);
        }
        #endregion

        #region Properties

        [Test]
        public void Date_TestStruct_AreEqual()
        {
            var exp = new Date(1997, 04, 05);
            var act = TestStruct.Date;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Year_MinValue_AreEqual()
        {
            var exp = WeekDate.MinValue.Year;
            var act = 1;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Year_MaxValue_AreEqual()
        {
            var exp = WeekDate.MaxValue.Year;
            var act = 9999;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Year_Y2010W52D7_AreEqual()
        {
            var date = new WeekDate(2010, 52, 7);
            var exp = 2010;
            var act = date.Year;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Year_Y2020W01D1_AreEqual()
        {
            var date = new WeekDate(2020, 01, 1);
            var exp = 2020;
            var act = date.Year;

            Assert.AreEqual(exp, act);
        }




        [Test]
        public void Day_TestStruct_AreEqual()
        {
            var exp = 6;
            var act = TestStruct.Day;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Day_Sunday_AreEqual()
        {
            var date = new WeekDate(1990, 40, 7);
            var exp = 7;
            var act = date.Day;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void DayOfYear_TestStruct_AreEqual()
        {
            var exp = 96;
            var act = TestStruct.DayOfYear;

            Assert.AreEqual(exp, act);
        }



        #endregion

        #region Type converter tests

        [Test]
        public void ConverterExists_WeekDate_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(WeekDate));
        }

        [Test]
        public void CanNotConvertFromInt32_WeekDate_IsTrue()
        {
            TypeConverterAssert.CanNotConvertFrom(typeof(WeekDate), typeof(Int32));
        }
        [Test]
        public void CanNotConvertToInt32_WeekDate_IsTrue()
        {
            TypeConverterAssert.CanNotConvertTo(typeof(WeekDate), typeof(Int32));
        }

        [Test]
        public void CanConvertFromString_WeekDate_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(WeekDate));
        }

        [Test]
        public void CanConvertToString_WeekDate_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(WeekDate));
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
        public void ConvertFrom_DateTime_Successful()
        {
            TypeConverterAssert.ConvertFromEquals(TestStruct, new DateTime(1997, 04, 05));
        }

        [Test]
        public void ConvertFrom_DateTimeOffset_Successful()
        {
            TypeConverterAssert.ConvertFromEquals(TestStruct, new DateTimeOffset(1997, 04, 05, 00, 00, 00, TimeSpan.Zero));
        }

        [Test]
        public void ConvertFrom_LocalDateTime_Successful()
        {
            TypeConverterAssert.ConvertFromEquals(TestStruct, new LocalDateTime(1997, 04, 05));
        }

        [Test]
        public void ConvertFrom_Date_Successful()
        {
            TypeConverterAssert.ConvertFromEquals(TestStruct, (Date)TestStruct);
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
        public void ConverTo_DateTime_Successful()
        {
            TypeConverterAssert.ConvertToEquals(new DateTime(1997, 04, 05), TestStruct);
        }

        [Test]
        public void ConverTo_LocalDateTime_Successful()
        {
            TypeConverterAssert.ConvertToEquals(new LocalDateTime(1997, 04, 05), TestStruct);
        }

        [Test]
        public void ConverTo_DateTimeOffset_Successful()
        {
            using (Clock.SetTimeZoneForCurrentThread(TimeZoneInfo.Utc))
            {
                TypeConverterAssert.ConvertToEquals(new DateTimeOffset(1997, 04, 05, 00, 00, 00, TimeSpan.Zero), TestStruct);
            }
        }

        [Test]
        public void ConverTo_Date_Successful()
        {
            TypeConverterAssert.ConvertToEquals((Date)TestStruct, TestStruct);
        }

        #endregion

        #region IsValid tests

        [Test]
        public void IsValid_Data_IsFalse()
        {
            Assert.IsFalse(WeekDate.IsValid("Complex"), "Complex");
            Assert.IsFalse(WeekDate.IsValid((String)null), "(String)null");
            Assert.IsFalse(WeekDate.IsValid(string.Empty), "string.Empty");
            Assert.IsFalse(WeekDate.IsValid("0000-W12-6"), "0000-W12-6");
            Assert.IsFalse(WeekDate.IsValid("0001-W12-8"), "0001-W12-8");
            Assert.IsFalse(WeekDate.IsValid("9999-W53-1"), "9999-W53-1");
        }
        [Test]
        public void IsValid_Data_IsTrue()
        {
            Assert.IsTrue(WeekDate.IsValid("1234-50-6"));
        }
        #endregion
    }

    [Serializable]
    public class WeekDateSerializeObject
    {
        public int Id { get; set; }
        public WeekDate Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
