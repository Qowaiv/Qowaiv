using NUnit.Framework;
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
	/// <summary>Tests the Date SVO.</summary>
	[TestFixture]
	public class DateTest
	{
		/// <summary>The test instance for most tests.</summary>
		public static readonly Date TestStruct = new Date(1970, 02, 14);

		#region Date const tests

		/// <summary>Date.Empty should be equal to the default of Date.</summary>
		[Test]
		public void MinValue_None_EqualsDefault()
		{
			Assert.AreEqual(default(Date), Date.MinValue);
		}

		[Test]
		public void Today_None_EqualsDateTimeToday()
		{
			var act = Date.Today;
			var exp = (Date)DateTime.Today;

			Assert.AreEqual(act, exp);
		}

		#endregion

		#region Constructor Tests

		[Test]
		public void Ctor_621393984000000017_AreEqual()
		{
			var act = new Date(621393984000000017L);
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		#endregion

		#region TryParse tests

		/// <summary>TryParse with specified string value should be valid.</summary>
		[Test]
		public void TyrParse_StringValue_IsValid()
		{
			Date val;

			string str = "1983-05-02";

			Assert.IsTrue(Date.TryParse(str, out val), "Valid");
			Assert.AreEqual(new Date(1983, 05, 02), val, "Value");
		}

		/// <summary>TryParse with specified string value should be invalid.</summary>
		[Test]
		public void TyrParse_StringValue_IsNotValid()
		{
			using (new CultureInfoScope("nl-NL"))
			{
				Date val;

				string str = "string";

				Assert.IsFalse(Date.TryParse(str, out val), "Valid");
				Assert.AreEqual("1-1-0001", val.ToString(), "Value");
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
					Date.Parse("InvalidInput");
				},
				"Not a valid date");
			}
		}

		[Test]
		public void TryParse_TestStructInput_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = TestStruct;
				var act = Date.TryParse(exp.ToString());

				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void TryParse_InvalidInput_DefaultValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = default(Date);
				var act = Date.TryParse("InvalidInput");

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
				SerializationTest.DeserializeUsingConstructor<Date>(null, default(StreamingContext));
			},
			"info");
		}

		[Test]
		public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
		{
			Assert.Catch<SerializationException>
			(() =>
			{
				var info = new SerializationInfo(typeof(Date), new System.Runtime.Serialization.FormatterConverter());
				SerializationTest.DeserializeUsingConstructor<Date>(info, default(StreamingContext));
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
			var info = new SerializationInfo(typeof(Date), new System.Runtime.Serialization.FormatterConverter());
			obj.GetObjectData(info, default(StreamingContext));

			Assert.AreEqual(new DateTime(1970, 02, 14), info.GetDateTime("Value"));
		}

		[Test]
		public void SerializeDeserialize_TestStruct_AreEqual()
		{
			var input = DateTest.TestStruct;
			var exp = DateTest.TestStruct;
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void DataContractSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = DateTest.TestStruct;
			var exp = DateTest.TestStruct;
			var act = SerializationTest.DataContractSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void XmlSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = DateTest.TestStruct;
			var exp = DateTest.TestStruct;
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void SerializeDeserialize_DateSerializeObject_AreEqual()
		{
			var input = new DateSerializeObject()
			{
				Id = 17,
				Obj = DateTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new DateSerializeObject()
			{
				Id = 17,
				Obj = DateTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			DateTimeAssert.AreEqual(exp.Date, act.Date, "Date");;
		}
		[Test]
		public void XmlSerializeDeserialize_DateSerializeObject_AreEqual()
		{
			var input = new DateSerializeObject()
			{
				Id = 17,
				Obj = DateTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new DateSerializeObject()
			{
				Id = 17,
				Obj = DateTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			DateTimeAssert.AreEqual(exp.Date, act.Date, "Date");;
		}
		[Test]
		public void DataContractSerializeDeserialize_DateSerializeObject_AreEqual()
		{
			var input = new DateSerializeObject()
			{
				Id = 17,
				Obj = DateTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new DateSerializeObject()
			{
				Id = 17,
				Obj = DateTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.DataContractSerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			DateTimeAssert.AreEqual(exp.Date, act.Date, "Date");;
		}

		[Test]
		public void SerializeDeserialize_Empty_AreEqual()
		{
			var input = new DateSerializeObject()
			{
				Id = 17,
				Obj = DateTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new DateSerializeObject()
			{
				Id = 17,
				Obj = DateTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			DateTimeAssert.AreEqual(exp.Date, act.Date, "Date");;
		}
		[Test]
		public void XmlSerializeDeserialize_Empty_AreEqual()
		{
			var input = new DateSerializeObject()
			{
				Id = 17,
				Obj = Date.MinValue,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new DateSerializeObject()
			{
				Id = 17,
				Obj = Date.MinValue,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			DateTimeAssert.AreEqual(exp.Date, act.Date, "Date");;
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
			Assert.Catch<NotSupportedException>(() =>
			{
				JsonTester.Read<Date>();
			},
			"JSON deserialization from null is not supported.");
		}

		[Test]
		public void FromJson_InvalidStringValue_AssertFormatException()
		{
			Assert.Catch<FormatException>(() =>
			{
				JsonTester.Read<Date>("InvalidStringValue");
			},
			"Not a valid date");
		}
		[Test]
		public void FromJson_StringValue_AreEqual()
		{
			var act = JsonTester.Read<Date>(TestStruct.ToString(CultureInfo.InvariantCulture));
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_Int64Value_AreEqual()
		{
			var act = JsonTester.Read<Date>(TestStruct.Ticks);
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_DoubleValue_AssertNotSupportedException()
		{
			Assert.Catch<NotSupportedException>(() =>
			{
				JsonTester.Read<Date>(1234.56);
			},
			"JSON deserialization from a number is not supported.");
		}

		[Test]
		public void FromJson_DateTimeValue_AreEqual()
		{
			var act = JsonTester.Read<Date>((DateTime)TestStruct);
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToJson_DefaultValue_AreEqual()
		{
			object act = JsonTester.Write(default(Date));
			object exp = "0001-01-01";

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void ToJson_TestStruct_AreEqual()
		{
			var act = JsonTester.Write(TestStruct);
			var exp = "1970-02-14";

			Assert.AreEqual(exp, act);
		}

		#endregion

		#region IFormattable / ToString tests

		[Test]
		public void ToString_CustomFormatter_SupportsCustomFormatting()
		{
			using (new CultureInfoScope("nl-NL"))
			{
				var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
				var exp = "Unit Test Formatter, value: '14-2-1970', format: 'Unit Test Format'";

				Assert.AreEqual(exp, act);
			}
		}
		[Test]
		public void ToString_TestStruct_ComplexPattern()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = TestStruct.ToString("");
				var exp = "14/02/1970";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_FormatValueEnglishGreatBritain_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var act = new Date(1988, 08, 08).ToString("yy-M-d");
				var exp = "88-8-8";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_FormatValueSpanishSpain_AreEqual()
		{
			var act = new Date(1988, 08, 08).ToString("d", new CultureInfo("es-EC"));
			var exp = "8/8/1988";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void DebuggerDisplay_DebugToString_HasAttribute()
		{
			DebuggerDisplayAssert.HasAttribute(typeof(Date));
		}

		[Test]
		public void DebuggerDisplay_DefaultValue_String()
		{
			DebuggerDisplayAssert.HasResult("0001-01-01", default(Date));
		}

		[Test]
		public void DebuggerDisplay_TestStruct_String()
		{
			DebuggerDisplayAssert.HasResult("1970-02-14", TestStruct);
		}

		#endregion

		#region IEquatable tests

		/// <summary>GetHash should not fail for Date.Empty.</summary>
		[Test]
		public void GetHash_MinValue_Hash()
		{
			Assert.AreEqual(0, Date.MinValue.GetHashCode());
		}

		/// <summary>GetHash should not fail for the test struct.</summary>
		[Test]
		public void GetHash_TestStruct_Hash()
		{
			Assert.AreEqual(1232937609, DateTest.TestStruct.GetHashCode());
		}

		[Test]
		public void Equals_EmptyEmpty_IsTrue()
		{
			Assert.IsTrue(Date.MinValue.Equals(Date.MinValue));
		}

		[Test]
		public void Equals_FormattedAndUnformatted_IsTrue()
		{
			var l = Date.Parse("1970-02-14", CultureInfo.InvariantCulture);
			var r = Date.Parse("14 february 1970", CultureInfo.InvariantCulture);

			Assert.IsTrue(l.Equals(r));
		}

		[Test]
		public void Equals_TestStructTestStruct_IsTrue()
		{
			Assert.IsTrue(DateTest.TestStruct.Equals(DateTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructEmpty_IsFalse()
		{
			Assert.IsFalse(DateTest.TestStruct.Equals(Date.MaxValue));
		}

		[Test]
		public void Equals_EmptyTestStruct_IsFalse()
		{
			Assert.IsFalse(Date.MinValue.Equals(DateTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructObjectTestStruct_IsTrue()
		{
			Assert.IsTrue(DateTest.TestStruct.Equals((object)DateTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructNull_IsFalse()
		{
			Assert.IsFalse(DateTest.TestStruct.Equals(null));
		}

		[Test]
		public void Equals_TestStructObject_IsFalse()
		{
			Assert.IsFalse(DateTest.TestStruct.Equals(new object()));
		}

		[Test]
		public void OperatorIs_TestStructTestStruct_IsTrue()
		{
			var l = DateTest.TestStruct;
			var r = DateTest.TestStruct;
			Assert.IsTrue(l == r);
		}

		[Test]
		public void OperatorIsNot_TestStructTestStruct_IsFalse()
		{
			var l = DateTest.TestStruct;
			var r = DateTest.TestStruct;
			Assert.IsFalse(l != r);
		}

		#endregion

		#region IComparable tests

		/// <summary>Orders a list of Dates ascending.</summary>
		[Test]
		public void OrderBy_Date_AreEqual()
		{
			var item0 = Date.Parse("1970-01-03");
			var item1 = Date.Parse("1970-02-01");
			var item2 = Date.Parse("1970-03-28");
			var item3 = Date.Parse("1970-04-12");

			var inp = new List<Date>() { Date.MinValue, item3, item2, item0, item1, Date.MinValue };
			var exp = new List<Date>() { Date.MinValue, Date.MinValue, item0, item1, item2, item3 };
			var act = inp.OrderBy(item => item).ToList();

			CollectionAssert.AreEqual(exp, act);
		}

		/// <summary>Orders a list of Dates descending.</summary>
		[Test]
		public void OrderByDescending_Date_AreEqual()
		{
			var item0 = Date.Parse("1970-01-03");
			var item1 = Date.Parse("1970-02-01");
			var item2 = Date.Parse("1970-03-28");
			var item3 = Date.Parse("1970-04-12");

			var inp = new List<Date>() { Date.MinValue, item3, item2, item0, item1, Date.MinValue };
			var exp = new List<Date>() { item3, item2, item1, item0, Date.MinValue, Date.MinValue };
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
					var act = TestStruct.CompareTo(other);
				},
				"obj",
				"Argument must be a Date"
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
					var act = TestStruct.CompareTo(other);
				},
				"obj",
				"Argument must be a Date"
			);
		}

		[Test]
		public void LessThan_17LT19_IsTrue()
		{
			Date l = new Date(1990, 10, 17);
			Date r = new Date(1990, 10, 19);

			Assert.IsTrue(l < r);
		}
		[Test]
		public void GreaterThan_21LT19_IsTrue()
		{
			Date l = new Date(1990, 10, 21);
			Date r = new Date(1990, 10, 19);

			Assert.IsTrue(l > r);
		}

		[Test]
		public void LessThanOrEqual_17LT19_IsTrue()
		{
			Date l = new Date(1990, 10, 17);
			Date r = new Date(1990, 10, 19);

			Assert.IsTrue(l <= r);
		}
		[Test]
		public void GreaterThanOrEqual_21LT19_IsTrue()
		{
			Date l = new Date(1990, 10, 21);
			Date r = new Date(1990, 10, 19);

			Assert.IsTrue(l >= r);
		}

		[Test]
		public void LessThanOrEqual_17LT17_IsTrue()
		{
			Date l = new Date(1990, 10, 17);
			Date r = new Date(1990, 10, 17);

			Assert.IsTrue(l <= r);
		}
		[Test]
		public void GreaterThanOrEqual_21LT21_IsTrue()
		{
			Date l = new Date(1990, 10, 21);
			Date r = new Date(1990, 10, 21);

			Assert.IsTrue(l >= r);
		}
		#endregion

		#region Casting tests

		[Test]
		public void Explicit_StringToDate_AreEqual()
		{
			var exp = TestStruct;
			var act = (Date)TestStruct.ToString();

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_DateToString_AreEqual()
		{
			var exp = TestStruct.ToString();
			var act = (string)TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Implicit_WeekDateToDate_AreEqual()
		{
			Date exp = new WeekDate(1970, 07, 6);
			Date act = TestStruct;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Implicit_DateToWeekDate_AreEqual()
		{
			WeekDate exp = TestStruct;
			WeekDate act = new WeekDate(1970, 07, 6);

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Explicit_DateTimeToDate_AreEqual()
		{
			Date exp = (Date)new DateTime(1970, 02, 14);
			Date act = TestStruct;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Implicit_DateToDateTime_AreEqual()
		{
			DateTime exp = TestStruct;
			DateTime act = new DateTime(1970, 02, 14);

			Assert.AreEqual(exp, act);
		}

		#endregion

		#region Properties

		[Test]
		public void Year_TestStruct_AreEqual()
		{
			var act = TestStruct.Year;
			var exp = 1970;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Month_TestStruct_AreEqual()
		{
			var act = TestStruct.Month;
			var exp = 2;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Day_TestStruct_AreEqual()
		{
			var act = TestStruct.Day;
			var exp = 14;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void DayOfWeek_TestStruct_AreEqual()
		{
			var act = TestStruct.DayOfWeek;
			var exp = DayOfWeek.Saturday;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void DayOfYear_TestStruct_AreEqual()
		{
			var act = TestStruct.DayOfYear;
			var exp = 45;
			Assert.AreEqual(exp, act);
		}

		#endregion

		#region Methods

		[Test]
		public void Increment_None_AreEqual()
		{
			var act = TestStruct;
			act++;
			var exp = new Date(1970, 02, 15);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Decrement_None_AreEqual()
		{
			var act = TestStruct;
			act--;
			var exp = new Date(1970, 02, 13);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Plus_TimeSpan_AreEqual()
		{
			var act = TestStruct + new TimeSpan(25, 30, 15);
			var exp = new Date(1970, 02, 15);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Min_TimeSpan_AreEqual()
		{
			var act = TestStruct - new TimeSpan(25, 30, 15);
			var exp = new Date(1970, 02, 12);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Min_DateTime_AreEqual()
		{
			var act = TestStruct - new Date(1970, 02, 12);
			var exp = TimeSpan.FromDays(2);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void AddTicks_4000000000017_AreEqual()
		{
			var act = TestStruct.AddTicks(4000000000017L);
			var exp = new Date(1970, 02, 18);

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void AddMilliseconds_Arround3Days_AreEqual()
		{
			var act = TestStruct.AddMilliseconds(3 * 24 * 60 * 60 * 1003);
			var exp = new Date(1970, 02, 17);

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void AddSeconds_Arround3Days_AreEqual()
		{
			var act = TestStruct.AddSeconds(3 * 24 * 60 * 64);
			var exp = new Date(1970, 02, 17);

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void AddMinutes_2280_AreEqual()
		{
			var act = TestStruct.AddMinutes(2 * 24 * 60);
			var exp = new Date(1970, 02, 16);

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void AddHours_41_AreEqual()
		{
			var act = TestStruct.AddHours(41);
			var exp = new Date(1970, 02, 15);

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void AddMonths_12_AreEqual()
		{
			var act = TestStruct.AddMonths(12);
			var exp = new Date(1971, 02, 14);

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void AddYears_Min12_AreEqual()
		{
			var act = TestStruct.AddYears(-12);
			var exp = new Date(1958, 02, 14);

			Assert.AreEqual(exp, act);
		}

		#endregion

		#region Type converter tests

		[Test]
		public void ConverterExists_Date_IsTrue()
		{
			TypeConverterAssert.ConverterExists(typeof(Date));
		}

		[Test]
		public void CanNotConvertFromInt32_Date_IsTrue()
		{
			TypeConverterAssert.CanNotConvertFrom(typeof(Date), typeof(Int32));
		}
		[Test]
		public void CanNotConvertToInt32_Date_IsTrue()
		{
			TypeConverterAssert.CanNotConvertTo(typeof(Date), typeof(Int32));
		}

		[Test]
		public void CanConvertFromString_Date_IsTrue()
		{
			TypeConverterAssert.CanConvertFromString(typeof(Date));
		}

		[Test]
		public void CanConvertToString_Date_IsTrue()
		{
			TypeConverterAssert.CanConvertToString(typeof(Date));
		}

		[Test]
		public void ConvertFromString_StringValue_TestStruct()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(DateTest.TestStruct, DateTest.TestStruct.ToString());
			}
		}

		[Test]
		public void ConvertFromInstanceDescriptor_Date_Successful()
		{
			TypeConverterAssert.ConvertFromInstanceDescriptor(typeof(Date));
		}

		[Test]
		public void ConvertToString_TestStruct_StringValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertToStringEquals(DateTest.TestStruct.ToString(), DateTest.TestStruct);
			}
		}

		#endregion

		#region IsValid tests

		[Test]
		public void IsValid_Data_IsFalse()
		{
			Assert.IsFalse(Date.IsValid("Complex"), "Complex");
			Assert.IsFalse(Date.IsValid((String)null), "(String)null");
			Assert.IsFalse(Date.IsValid(String.Empty), "String.Empty");
		}
		[Test]
		public void IsValid_Data_IsTrue()
		{
			Assert.IsTrue(Date.IsValid("1980-04-12 4:00"));
		}
		#endregion
	}

	[Serializable]
	public class DateSerializeObject
	{
		public int Id { get; set; }
		public Date Obj { get; set; }
		public DateTime Date { get; set; }
	}
}
