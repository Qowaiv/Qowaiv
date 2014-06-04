using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qowaiv.UnitTests.TestTools;
using Qowaiv.UnitTests.TestTools.Formatting;
using Qowaiv.UnitTests.TestTools.Globalization;
using Qowaiv.UnitTests.Json;
using Qowaiv;

namespace Qowaiv.UnitTests
{
	/// <summary>Tests the Date SVO.</summary>
	[TestClass]
	public class DateTest
	{
		/// <summary>The test instance for most tests.</summary>
		public static readonly Date TestStruct = new Date(1970, 02, 14);

		#region Date const tests

		/// <summary>Date.Empty should be equal to the default of Date.</summary>
		[TestMethod]
		public void MinValue_None_EqualsDefault()
		{
			Assert.AreEqual(default(Date), Date.MinValue);
		}

		#endregion

		#region Constructor Tests

		[TestMethod]
		public void Ctor_621393984000000017_AreEqual()
		{
			var act = new Date(621393984000000017L);
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		#endregion

		#region TryParse tests

		/// <summary>TryParse with specified string value should be valid.</summary>
		[TestMethod]
		public void TyrParse_StringValue_IsValid()
		{
			Date val;

			string str = "1983-05-02";

			Assert.IsTrue(Date.TryParse(str, out val), "Valid");
			Assert.AreEqual(new Date(1983, 05, 02), val, "Value");
		}

		/// <summary>TryParse with specified string value should be invalid.</summary>
		[TestMethod]
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

		[TestMethod]
		public void Parse_InvalidInput_ThrowsFormatException()
		{
			using (new CultureInfoScope("en-GB"))
			{
				ExceptionAssert.ExpectException<FormatException>
				(() =>
				{
					Date.Parse("InvalidInput");
				},
				"Not a valid date");
			}
		}

		[TestMethod]
		public void TryParse_TestStructInput_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = TestStruct;
				var act = Date.TryParse(exp.ToString());

				Assert.AreEqual(exp, act);
			}
		}

		[TestMethod]
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

		[TestMethod]
		public void Constructor_SerializationInfoIsNull_ThrowsArgumentNullException()
		{
			ExceptionAssert.ExpectArgumentNullException
			(() =>
			{
				SerializationTest.DeserializeUsingConstructor<Date>(null, default(StreamingContext));
			},
			"info");
		}

		[TestMethod]
		public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
		{
			ExceptionAssert.ExpectException<SerializationException>
			(() =>
			{
				var info = new SerializationInfo(typeof(Date), new System.Runtime.Serialization.FormatterConverter());
				SerializationTest.DeserializeUsingConstructor<Date>(info, default(StreamingContext));
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
			var info = new SerializationInfo(typeof(Date), new System.Runtime.Serialization.FormatterConverter());
			obj.GetObjectData(info, default(StreamingContext));

			Assert.AreEqual(new DateTime(1970, 02, 14), info.GetDateTime("Value"));
		}

		[TestMethod]
		public void SerializeDeserialize_TestStruct_AreEqual()
		{
			var input = DateTest.TestStruct;
			var exp = DateTest.TestStruct;
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[TestMethod]
		public void DataContractSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = DateTest.TestStruct;
			var exp = DateTest.TestStruct;
			var act = SerializationTest.DataContractSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[TestMethod]
		public void XmlSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = DateTest.TestStruct;
			var exp = DateTest.TestStruct;
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}

		[TestMethod]
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
			Assert.AreEqual(exp.Date, act.Date, "Date");
		}
		[TestMethod]
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
			Assert.AreEqual(exp.Date, act.Date, "Date");
		}
		[TestMethod]
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
			Assert.AreEqual(exp.Date, act.Date, "Date");
		}

		[TestMethod]
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
			Assert.AreEqual(exp.Date, act.Date, "Date");
		}
		[TestMethod]
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
		public void FromJson_Null_AssertNotSupportedException()
		{
			ExceptionAssert.ExpectException<NotSupportedException>(() =>
			{
				JsonTester.Read<Date>();
			},
			"JSON deserialization from null is not supported.");
		}

		[TestMethod]
		public void FromJson_InvalidStringValue_AssertFormatException()
		{
			ExceptionAssert.ExpectException<FormatException>(() =>
			{
				JsonTester.Read<Date>("InvalidStringValue");
			},
			"Not a valid date");
		}
		[TestMethod]
		public void FromJson_StringValue_AreEqual()
		{
			var act = JsonTester.Read<Date>(TestStruct.ToString(CultureInfo.InvariantCulture));
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		[TestMethod]
		public void FromJson_Int64Value_AreEqual()
		{
			var act = JsonTester.Read<Date>(TestStruct.Ticks);
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}
		
		[TestMethod]
		public void FromJson_DoubleValue_AssertNotSupportedException()
		{
			ExceptionAssert.ExpectException<NotSupportedException>(() =>
			{
				JsonTester.Read<Date>(1234.56);
			},
			"JSON deserialization from a number is not supported.");
		}

		[TestMethod]
		public void FromJson_DateTimeValue_AreEqual()
		{
			var act = JsonTester.Read<Date>((DateTime)TestStruct);
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}
		
		[TestMethod]
		public void ToJson_DefaultValue_AreEqual()
		{
			object act = JsonTester.Write(default(Date));
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
		public void ToString_CustomFormatter_SupportsCustomFormatting()
		{
			using (new CultureInfoScope("nl-NL"))
			{
				var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
				var exp = "Unit Test Formatter, value: '02/14/1970', format: 'Unit Test Format'";

				Assert.AreEqual(exp, act);
			}
		}
		[TestMethod]
		public void ToString_TestStruct_ComplexPattern()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = TestStruct.ToString("");
				var exp = "14/02/1970";
				Assert.AreEqual(exp, act);
			}
		}

		[TestMethod]
		public void ToString_FormatValueEnglishGreatBritain_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var act = new Date(1988, 08, 08).ToString("yy-M-d");
				var exp = "88-8-8";
				Assert.AreEqual(exp, act);
			}
		}

		[TestMethod]
		public void ToString_FormatValueSpanishEcuador_AreEqual()
		{
			var act = new Date(1988, 08, 08).ToString("d", new CultureInfo("es-EC"));
			var exp = "08/08/1988";
			Assert.AreEqual(exp, act);
		}

		[TestMethod]
		public void DebuggerDisplay_DebugToString_HasAttribute()
		{
			DebuggerDisplayAssert.HasAttribute(typeof(Date));
		}

		[TestMethod]
		public void DebugToString_DefaultValue_String()
		{
			DebuggerDisplayAssert.HasResult("0001-01-01", default(Date));
		}

		[TestMethod]
		public void DebugToString_TestStruct_String()
		{
			DebuggerDisplayAssert.HasResult("1970-02-14", TestStruct);
		}

		#endregion

		#region IEquatable tests

		/// <summary>GetHash should not fail for Date.Empty.</summary>
		[TestMethod]
		public void GetHash_MinValue_Hash()
		{
			Assert.AreEqual(0, Date.MinValue.GetHashCode());
		}

		/// <summary>GetHash should not fail for the test struct.</summary>
		[TestMethod]
		public void GetHash_TestStruct_Hash()
		{
			Assert.AreEqual(1232937609, DateTest.TestStruct.GetHashCode());
		}

		[TestMethod]
		public void Equals_EmptyEmpty_IsTrue()
		{
			Assert.IsTrue(Date.MinValue.Equals(Date.MinValue));
		}

		[TestMethod]
		public void Equals_FormattedAndUnformatted_IsTrue()
		{
			var l = Date.Parse("1970-02-14", CultureInfo.InvariantCulture);
			var r = Date.Parse("14 february 1970", CultureInfo.InvariantCulture);

			Assert.IsTrue(l.Equals(r));
		}

		[TestMethod]
		public void Equals_TestStructTestStruct_IsTrue()
		{
			Assert.IsTrue(DateTest.TestStruct.Equals(DateTest.TestStruct));
		}

		[TestMethod]
		public void Equals_TestStructEmpty_IsFalse()
		{
			Assert.IsFalse(DateTest.TestStruct.Equals(Date.MaxValue));
		}

		[TestMethod]
		public void Equals_EmptyTestStruct_IsFalse()
		{
			Assert.IsFalse(Date.MinValue.Equals(DateTest.TestStruct));
		}

		[TestMethod]
		public void Equals_TestStructObjectTestStruct_IsTrue()
		{
			Assert.IsTrue(DateTest.TestStruct.Equals((object)DateTest.TestStruct));
		}

		[TestMethod]
		public void Equals_TestStructNull_IsFalse()
		{
			Assert.IsFalse(DateTest.TestStruct.Equals(null));
		}

		[TestMethod]
		public void Equals_TestStructObject_IsFalse()
		{
			Assert.IsFalse(DateTest.TestStruct.Equals(new object()));
		}

		[TestMethod]
		public void OperatorIs_TestStructTestStruct_IsTrue()
		{
			var l = DateTest.TestStruct;
			var r = DateTest.TestStruct;
			Assert.IsTrue(l == r);
		}

		[TestMethod]
		public void OperatorIsNot_TestStructTestStruct_IsFalse()
		{
			var l = DateTest.TestStruct;
			var r = DateTest.TestStruct;
			Assert.IsFalse(l != r);
		}

		#endregion

		#region IComparable tests

		/// <summary>Orders a list of Dates ascending.</summary>
		[TestMethod]
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
		[TestMethod]
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
				"Argument must be a Date"
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
				"Argument must be a Date"
			);
		}

		[TestMethod]
		public void LessThan_17LT19_IsTrue()
		{
			Date l = new Date(1990, 10, 17);
			Date r = new Date(1990, 10, 19);

			Assert.IsTrue(l < r);
		}
		[TestMethod]
		public void GreaterThan_21LT19_IsTrue()
		{
			Date l =new Date(1990, 10,  21);
			Date r =new Date(1990, 10,  19);

			Assert.IsTrue(l > r);
		}

		[TestMethod]
		public void LessThanOrEqual_17LT19_IsTrue()
		{
			Date l = new Date(1990, 10, 17);
			Date r = new Date(1990, 10, 19);

			Assert.IsTrue(l <= r);
		}
		[TestMethod]
		public void GreaterThanOrEqual_21LT19_IsTrue()
		{
			Date l = new Date(1990, 10, 21);
			Date r = new Date(1990, 10, 19);

			Assert.IsTrue(l >= r);
		}

		[TestMethod]
		public void LessThanOrEqual_17LT17_IsTrue()
		{
			Date l = new Date(1990, 10, 17);
			Date r = new Date(1990, 10, 17);

			Assert.IsTrue(l <= r);
		}
		[TestMethod]
		public void GreaterThanOrEqual_21LT21_IsTrue()
		{
			Date l = new Date(1990, 10, 21);
			Date r = new Date(1990, 10, 21);

			Assert.IsTrue(l >= r);
		}
		#endregion

		#region Casting tests

		[TestMethod]
		public void Explicit_StringToDate_AreEqual()
		{
			var exp = TestStruct;
			var act = (Date)TestStruct.ToString();

			Assert.AreEqual(exp, act);
		}
		[TestMethod]
		public void Explicit_DateToString_AreEqual()
		{
			var exp = TestStruct.ToString();
			var act = (string)TestStruct;

			Assert.AreEqual(exp, act);
		}

		[TestMethod]
		public void Implicit_WeekDateToDate_AreEqual()
		{
			Date exp = new WeekDate(1970, 05, 6);
			Date act = TestStruct;

			Assert.AreEqual(exp, act);
		}
		[TestMethod]
		public void Implicit_DateToWeekDate_AreEqual()
		{
			WeekDate exp = TestStruct;
			WeekDate act = new WeekDate(1970, 05, 6);

			Assert.AreEqual(exp, act);
		}

		[TestMethod]
		public void Explicit_DateTimeToDate_AreEqual()
		{
			Date exp = (Date)new DateTime(1970, 02, 14);
			Date act = TestStruct;

			Assert.AreEqual(exp, act);
		}
		[TestMethod]
		public void Implicit_DateToDateTime_AreEqual()
		{
			DateTime exp = TestStruct;
			DateTime act = new DateTime(1970, 02, 14);

			Assert.AreEqual(exp, act);
		}

		#endregion

		#region Properties

		[TestMethod]
		public void Year_TestStruct_AreEqual()
		{
			var act = TestStruct.Year;
			var exp = 1970;
			Assert.AreEqual(exp, act);
		}
		[TestMethod]
		public void Month_TestStruct_AreEqual()
		{
			var act = TestStruct.Month;
			var exp = 2;
			Assert.AreEqual(exp, act);
		}
		[TestMethod]
		public void Day_TestStruct_AreEqual()
		{
			var act = TestStruct.Day;
			var exp = 14;
			Assert.AreEqual(exp, act);
		}
		[TestMethod]
		public void DayOfWeek_TestStruct_AreEqual()
		{
			var act = TestStruct.DayOfWeek;
			var exp = DayOfWeek.Saturday;
			Assert.AreEqual(exp, act);
		}
		[TestMethod]
		public void DayOfYear_TestStruct_AreEqual()
		{
			var act = TestStruct.DayOfYear;
			var exp = 45;
			Assert.AreEqual(exp, act);
		}

		#endregion

		#region Methods

		[TestMethod]
		public void Increment_None_AreEqual()
		{
			var act = TestStruct;
			act++;
			var exp = new Date(1970, 02, 15);
			Assert.AreEqual(exp, act);
		}
		[TestMethod]
		public void Decrement_None_AreEqual()
		{
			var act = TestStruct;
			act--;
			var exp = new Date(1970, 02, 13);
			Assert.AreEqual(exp, act);
		}

		[TestMethod]
		public void Plus_TimeSpan_AreEqual()
		{
			var act = TestStruct + new TimeSpan(25, 30, 15);
			var exp = new Date(1970, 02, 15);
			Assert.AreEqual(exp, act);
		}
		[TestMethod]
		public void Min_TimeSpan_AreEqual()
		{
			var act = TestStruct - new TimeSpan(25, 30, 15);
			var exp = new Date(1970, 02, 12);
			Assert.AreEqual(exp, act);
		}

		[TestMethod]
		public void Min_DateTime_AreEqual()
		{
			var act = TestStruct - new Date(1970, 02, 12);
			var exp = TimeSpan.FromDays(2);
			Assert.AreEqual(exp, act);
		}

		[TestMethod]
		public void AddTicks_4000000000017_AreEqual()
		{
			var act = TestStruct.AddTicks(4000000000017L);
			var exp = new Date(1970, 02, 18);

			Assert.AreEqual(exp, act);
		}

		[TestMethod]
		public void AddMilliseconds_Arround3Days_AreEqual()
		{
			var act = TestStruct.AddMilliseconds(3 * 24 * 60 * 60 * 1003);
			var exp = new Date(1970, 02, 17);

			Assert.AreEqual(exp, act);
		}
		[TestMethod]
		public void AddSeconds_Arround3Days_AreEqual()
		{
			var act = TestStruct.AddSeconds(3 * 24 * 60 * 64);
			var exp = new Date(1970, 02, 17);

			Assert.AreEqual(exp, act);
		}
		[TestMethod]
		public void AddMinutes_2280_AreEqual()
		{
			var act = TestStruct.AddMinutes(2 * 24 * 60);
			var exp = new Date(1970, 02, 16);

			Assert.AreEqual(exp, act);
		}

		[TestMethod]
		public void AddHours_41_AreEqual()
		{
			var act = TestStruct.AddHours(41);
			var exp = new Date(1970, 02, 15);

			Assert.AreEqual(exp, act);
		}

		[TestMethod]
		public void AddMonths_12_AreEqual()
		{
			var act = TestStruct.AddMonths(12);
			var exp = new Date(1971, 02, 14);

			Assert.AreEqual(exp, act);
		}
		[TestMethod]
		public void AddYears_Min12_AreEqual()
		{
			var act = TestStruct.AddYears(-12);
			var exp = new Date(1958, 02, 14);

			Assert.AreEqual(exp, act);
		}

		#endregion

		#region Type converter tests

		[TestMethod]
		public void ConverterExists_Date_IsTrue()
		{
			TypeConverterAssert.ConverterExists(typeof(Date));
		}

		[TestMethod]
		public void CanNotConvertFromInt32_Date_IsTrue()
		{
			TypeConverterAssert.CanNotConvertFrom(typeof(Date), typeof(Int32));
		}
		[TestMethod]
		public void CanNotConvertToInt32_Date_IsTrue()
		{
			TypeConverterAssert.CanNotConvertTo(typeof(Date), typeof(Int32));
		}

		[TestMethod]
		public void CanConvertFromString_Date_IsTrue()
		{
			TypeConverterAssert.CanConvertFromString(typeof(Date));
		}

		[TestMethod]
		public void CanConvertToString_Date_IsTrue()
		{
			TypeConverterAssert.CanConvertToString(typeof(Date));
		}

		[TestMethod]
		public void ConvertFromString_StringValue_TestStruct()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(DateTest.TestStruct, DateTest.TestStruct.ToString());
			}
		}

		[TestMethod]
		public void ConvertToString_TestStruct_StringValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertToStringEquals(DateTest.TestStruct.ToString(), DateTest.TestStruct);
			}
		}

		#endregion

		#region IsValid tests

		[TestMethod]
		public void IsValid_Data_IsFalse()
		{
			Assert.IsFalse(Date.IsValid("Complex"), "Complex");
			Assert.IsFalse(Date.IsValid((String)null), "(String)null");
			Assert.IsFalse(Date.IsValid(String.Empty), "String.Empty");
		}
		[TestMethod]
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
