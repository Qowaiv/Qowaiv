using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml.Serialization;
using NUnit.Framework;
using Qowaiv.UnitTests.TestTools;
using Qowaiv.UnitTests.TestTools.Formatting;
using Qowaiv.UnitTests.TestTools.Globalization;
using Qowaiv.UnitTests.Json;
using Qowaiv;

namespace Qowaiv.UnitTests
{
	/// <summary>Tests the local date time SVO.</summary>
	[TestFixture]
	public class LocalDateTimeTest
	{
		/// <summary>The test instance for most tests.</summary>
		public static readonly LocalDateTime TestStruct = new LocalDateTime(1988, 06, 13, 22, 10, 05, 001);

		/// <summary>The test instance for most tests.</summary>
		public static readonly LocalDateTime TestStructNoMilliseconds = new LocalDateTime(2001, 07, 30, 21, 55, 08);

		#region local date time const tests

		/// <summary>LocalDateTime.MinValue should be equal to the default of local date time.</summary>
		[Test]
		public void MinValue_None_EqualsDefault()
		{
			Assert.AreEqual(default(LocalDateTime), LocalDateTime.MinValue);
		}

		#endregion
		
		#region TryParse tests

		/// <summary>TryParse null should be valid.</summary>
		[Test]
		public void TyrParse_Null_IsNotValid()
		{
			LocalDateTime val;

			string str = null;

			Assert.IsFalse(LocalDateTime.TryParse(str, out val), "Valid");
		}

		/// <summary>TryParse string.MinValue should be valid.</summary>
		[Test]
		public void TyrParse_StringMinValue_IsNotValid()
		{
			LocalDateTime val;

			string str = string.Empty;

			Assert.IsFalse(LocalDateTime.TryParse(str, out val), "Valid");
		}

		/// <summary>TryParse with specified string value should be valid.</summary>
		[Test]
		public void TyrParse_StringValue_IsValid()
		{
			using (new CultureInfoScope("nl-NL"))
			{
				LocalDateTime val;

				string str = "26-4-2015 17:07:13";

				Assert.IsTrue(LocalDateTime.TryParse(str, out val), "Valid");
				Assert.AreEqual(str, val.ToString(), "Value");
			}
		}

		/// <summary>TryParse with specified string value should be invalid.</summary>
		[Test]
		public void TyrParse_StringValue_IsNotValid()
		{
			LocalDateTime val;

			string str = "invalid format";

			Assert.IsFalse(LocalDateTime.TryParse(str, out val), "Valid");
		}

		[Test]
		public void Parse_InvalidInput_ThrowsFormatException()
		{
			using (new CultureInfoScope("en-GB"))
			{
				Assert.Catch<FormatException>
				(() =>
				{
					LocalDateTime.Parse("InvalidInput");
				},
				"Not a valid date");
			}
		}

		[Test]
		public void TryParse_TestStructInput_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = TestStructNoMilliseconds;
				var act = LocalDateTime.TryParse(exp.ToString());

				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void TryParse_InvalidInput_DefaultValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = default(LocalDateTime);
				var act = LocalDateTime.TryParse("InvalidInput");

				Assert.AreEqual(exp, act);
			}
		}

		#endregion
	  
		#region TryCreate tests

		//[Test]
		//public void TryCreate_Null_IsMinValue()
		//{
		//	LocalDateTime exp = LocalDateTime.MinValue;
		//	LocalDateTime act;

		//	Assert.IsTrue(LocalDateTime.TryCreate(null, out act));
		//	Assert.AreEqual(exp, act);
		//}
		//[Test]
		//public void TryCreate_DateTimeMinValue_IsMinValue()
		//{
		//	LocalDateTime exp = LocalDateTime.MinValue;
		//	LocalDateTime act;

		//	Assert.IsFalse(LocalDateTime.TryCreate(DateTime.MinValue, out act));
		//	Assert.AreEqual(exp, act);
		//}

		//[Test]
		//public void TryCreate_DateTimeMinValue_AreEqual()
		//{
		//	var exp = LocalDateTime.MinValue;
		//	var act = LocalDateTime.TryCreate(DateTime.MinValue);
		//	Assert.AreEqual(exp, act);
		//}
		//[Test]
		//public void TryCreate_Value_AreEqual()
		//{
		//	var exp = TestStruct;
		//	var act = LocalDateTime.TryCreate(1234567);
		//	Assert.AreEqual(exp, act);
		//}

		//[Test]
		//public void Create_DateTimeMinValue_ThrowsArgumentOutOfRangeException()
		//{
		//	ExceptionAssert.CatchArgumentOutOfRangeException
		//	(() =>
		//	{
		//		LocalDateTime.Create(DateTime.MinValue);
		//	},
		//	"val",
		//	"Not a valid local date time");
		//}

		#endregion

		#region (XML) (De)serialization tests

		[Test]
		public void Constructor_SerializationInfoIsNull_ThrowsArgumentNullException()
		{
			ExceptionAssert.CatchArgumentNullException
			(() =>
			{
				SerializationTest.DeserializeUsingConstructor<LocalDateTime>(null, default(StreamingContext));
			},
			"info");
		}
		
		[Test]
		public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
		{
			Assert.Catch<SerializationException>
			(() =>
			{
				var info = new SerializationInfo(typeof(LocalDateTime), new System.Runtime.Serialization.FormatterConverter());
				SerializationTest.DeserializeUsingConstructor<LocalDateTime>(info, default(StreamingContext));
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
			var info = new SerializationInfo(typeof(LocalDateTime), new System.Runtime.Serialization.FormatterConverter());
			obj.GetObjectData(info, default(StreamingContext));

			Assert.AreEqual(new DateTime(1988, 06, 13, 22, 10, 05, 001), info.GetDateTime("Value"));
		}
		
		[Test]
		public void SerializeDeserialize_TestStruct_AreEqual()
		{
			var input = LocalDateTimeTest.TestStructNoMilliseconds;
			var exp = LocalDateTimeTest.TestStructNoMilliseconds;
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void DataContractSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = LocalDateTimeTest.TestStructNoMilliseconds;
			var exp = LocalDateTimeTest.TestStructNoMilliseconds;
			var act = SerializationTest.DataContractSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void XmlSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = LocalDateTimeTest.TestStructNoMilliseconds;
			var exp = LocalDateTimeTest.TestStructNoMilliseconds;
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void SerializeDeserialize_LocalDateTimeSerializeObject_AreEqual()
		{
			var input = new LocalDateTimeSerializeObject()
			{
				Id = 17,
				Obj = LocalDateTimeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new LocalDateTimeSerializeObject()
			{
				Id = 17,
				Obj = LocalDateTimeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			DateTimeAssert.AreEqual(exp.Date, act.Date, "Date");;;
		}
		[Test]
		public void XmlSerializeDeserialize_LocalDateTimeSerializeObject_AreEqual()
		{
			var input = new LocalDateTimeSerializeObject()
			{
				Id = 17,
				Obj = LocalDateTimeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new LocalDateTimeSerializeObject()
			{
				Id = 17,
				Obj = LocalDateTimeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			DateTimeAssert.AreEqual(exp.Date, act.Date, "Date");;
		}
		[Test]
		public void DataContractSerializeDeserialize_LocalDateTimeSerializeObject_AreEqual()
		{
			var input = new LocalDateTimeSerializeObject()
			{
				Id = 17,
				Obj = LocalDateTimeTest.TestStructNoMilliseconds,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new LocalDateTimeSerializeObject()
			{
				Id = 17,
				Obj = LocalDateTimeTest.TestStructNoMilliseconds,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.DataContractSerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			DateTimeAssert.AreEqual(exp.Date, act.Date, "Date");;
		}

		[Test]
		public void SerializeDeserialize_MinValue_AreEqual()
		{
			var input = new LocalDateTimeSerializeObject()
			{
				Id = 17,
				Obj = LocalDateTimeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new LocalDateTimeSerializeObject()
			{
				Id = 17,
				Obj = LocalDateTimeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			DateTimeAssert.AreEqual(exp.Date, act.Date, "Date");;
		}
		[Test]
		public void XmlSerializeDeserialize_MinValue_AreEqual()
		{
			var input = new  LocalDateTimeSerializeObject()
			{
				Id = 17,
				Obj = LocalDateTime.MinValue,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new LocalDateTimeSerializeObject()
			{
				Id = 17,
				Obj = LocalDateTime.MinValue,
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
				JsonTester.Read<LocalDateTime>();
			},
			"JSON deserialization from null is not supported.");
		}

		[Test]
		public void FromJson_InvalidStringValue_AssertFormatException()
		{
			Assert.Catch<FormatException>(() =>
			{
				JsonTester.Read<LocalDateTime>("InvalidStringValue");
			},
			"Not a valid date");
		}
		[Test]
		public void FromJson_StringValue_AreEqual()
		{
			var act = JsonTester.Read<LocalDateTime>(TestStructNoMilliseconds.ToString(CultureInfo.InvariantCulture));
			var exp = TestStructNoMilliseconds;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_Int64Value_AreEqual()
		{
			var act = JsonTester.Read<LocalDateTime>(627178398050010000L);
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}
		
		[Test]
		public void FromJson_DoubleValue_AssertNotSupportedException()
		{
			Assert.Catch<NotSupportedException>(() =>
			{
				JsonTester.Read<LocalDateTime>(1234.56);
			},
			"JSON deserialization from a number is not supported.");
		}

		[Test]
		public void FromJson_DateTimeValue_AreEqual()
		{
			var act = JsonTester.Read<LocalDateTime>((DateTime)TestStruct);
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}
		
		[Test]
		public void ToJson_DefaultValue_AreEqual()
		{
			object act = JsonTester.Write(default(LocalDateTime));
			object exp = "0001-01-01 00:00:00";

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void ToJson_TestStruct_AreEqual()
		{
			var act = JsonTester.Write(TestStruct);
			var exp = "1988-06-13 22:10:05.001";

			Assert.AreEqual(exp, act);
		}

		#endregion
		
		#region IFormattable / ToString tests
		
		[Test]
		public void ToString_CustomFormatter_SupportsCustomFormatting()
		{
			using (new CultureInfoScope("es-ES"))
			{
				var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
				var exp = "Unit Test Formatter, value: '13/06/1988 22:10:05', format: 'Unit Test Format'";

				Assert.AreEqual(exp, act);
			}
		}
		[Test]
		public void ToString_TestStruct_ComplexPattern()
		{
			var act = TestStruct.ToString(@"yyyy-MM-dd\THH:mm:ss.FFFFFFF");
			var exp = "1988-06-13T22:10:05.001";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void DebuggerDisplay_DebugToString_HasAttribute()
		{
			DebuggerDisplayAssert.HasAttribute(typeof(LocalDateTime));
		}

		[Test]
		public void DebuggerDisplay_DefaultValue_String()
		{
			DebuggerDisplayAssert.HasResult("0001-01-01 12:00:00", default(LocalDateTime));
		}

		[Test]
		public void DebuggerDisplay_TestStruct_String()
		{
			DebuggerDisplayAssert.HasResult("1988-06-13 10:10:05.001", TestStruct);
		}

		#endregion

		#region IEquatable tests

		/// <summary>GetHash should not fail for LocalDateTime.MinValue.</summary>
		[Test]
		public void GetHash_MinValue_Hash()
		{
			Assert.AreEqual(0, LocalDateTime.MinValue.GetHashCode());
		}

		/// <summary>GetHash should not fail for the test struct.</summary>
		[Test]
		public void GetHash_TestStruct_Hash()
		{
			Assert.AreEqual(304475390, LocalDateTimeTest.TestStruct.GetHashCode());
		}

		[Test]
		public void Equals_MinValueMinValue_IsTrue()
		{
			Assert.IsTrue(LocalDateTime.MinValue.Equals(LocalDateTime.MinValue));
		}

		[Test]
		public void Equals_FormattedAndUnformatted_IsTrue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var l = LocalDateTime.Parse("14 february 2010", CultureInfo.InvariantCulture);
				var r = LocalDateTime.Parse("2010-02-14", CultureInfo.InvariantCulture);

				Assert.IsTrue(l.Equals(r));
			}
		}

		[Test]
		public void Equals_TestStructTestStruct_IsTrue()
		{
			Assert.IsTrue(LocalDateTimeTest.TestStruct.Equals(LocalDateTimeTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructMinValue_IsFalse()
		{
			Assert.IsFalse(LocalDateTimeTest.TestStruct.Equals(LocalDateTime.MinValue));
		}

		[Test]
		public void Equals_MinValueTestStruct_IsFalse()
		{
			Assert.IsFalse(LocalDateTime.MinValue.Equals(LocalDateTimeTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructObjectTestStruct_IsTrue()
		{
			Assert.IsTrue(LocalDateTimeTest.TestStruct.Equals((object)LocalDateTimeTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructNull_IsFalse()
		{
			Assert.IsFalse(LocalDateTimeTest.TestStruct.Equals(null));
		}

		[Test]
		public void Equals_TestStructObject_IsFalse()
		{
			Assert.IsFalse(LocalDateTimeTest.TestStruct.Equals(new object()));
		}

		[Test]
		public void OperatorIs_TestStructTestStruct_IsTrue()
		{
			var l = LocalDateTimeTest.TestStruct;
			var r = LocalDateTimeTest.TestStruct;
			Assert.IsTrue(l == r);
		}

		[Test]
		public void OperatorIsNot_TestStructTestStruct_IsFalse()
		{
			var l = LocalDateTimeTest.TestStruct;
			var r = LocalDateTimeTest.TestStruct;
			Assert.IsFalse(l != r);
		}

		#endregion
		
		#region IComparable tests

		/// <summary>Orders a list of local date times ascending.</summary>
		[Test]
		public void OrderBy_LocalDateTime_AreEqual()
		{
			var item0 = new LocalDateTime(1900, 10, 01, 22, 10, 16);
			var item1 = new LocalDateTime(1963, 08, 23, 23, 59, 15);
			var item2 = new LocalDateTime(1999, 12, 05, 04, 13, 14);
			var item3 = new LocalDateTime(2010, 07, 13, 00, 44, 13);

			var inp = new List<LocalDateTime>() { LocalDateTime.MinValue, item3, item2, item0, item1, LocalDateTime.MinValue };
			var exp = new List<LocalDateTime>() { LocalDateTime.MinValue, LocalDateTime.MinValue, item0, item1, item2, item3 };
			var act = inp.OrderBy(item => item).ToList();

			CollectionAssert.AreEqual(exp, act);
		}

		/// <summary>Orders a list of local date times descending.</summary>
		[Test]
		public void OrderByDescending_LocalDateTime_AreEqual()
		{
			var item0 = new LocalDateTime(1900, 10, 01, 22, 10, 16);
			var item1 = new LocalDateTime(1963, 08, 23, 23, 59, 15);
			var item2 = new LocalDateTime(1999, 12, 05, 04, 13, 14);
			var item3 = new LocalDateTime(2010, 07, 13, 00, 44, 13);

			var inp = new List<LocalDateTime>() { LocalDateTime.MinValue, item3, item2, item0, item1, LocalDateTime.MinValue };
			var exp = new List<LocalDateTime>() { item3, item2, item1, item0, LocalDateTime.MinValue, LocalDateTime.MinValue };
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
				"Argument must be a local date time"
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
				"Argument must be a local date time"
			);
		}

		//[Test]
		//public void LessThan_17LT19_IsTrue()
		//{
		//	LocalDateTime l = 17;
		//	LocalDateTime r = 19;

		//	Assert.IsTrue(l < r);
		//}
		//[Test]
		//public void GreaterThan_21LT19_IsTrue()
		//{
		//	LocalDateTime l = 21;
		//	LocalDateTime r = 19;

		//	Assert.IsTrue(l > r);
		//}

		//[Test]
		//public void LessThanOrEqual_17LT19_IsTrue()
		//{
		//	LocalDateTime l = 17;
		//	LocalDateTime r = 19;

		//	Assert.IsTrue(l <= r);
		//}
		//[Test]
		//public void GreaterThanOrEqual_21LT19_IsTrue()
		//{
		//	LocalDateTime l = 21;
		//	LocalDateTime r = 19;

		//	Assert.IsTrue(l >= r);
		//}

		//[Test]
		//public void LessThanOrEqual_17LT17_IsTrue()
		//{
		//	LocalDateTime l = 17;
		//	LocalDateTime r = 17;

		//	Assert.IsTrue(l <= r);
		//}
		//[Test]
		//public void GreaterThanOrEqual_21LT21_IsTrue()
		//{
		//	LocalDateTime l = 21;
		//	LocalDateTime r = 21;

		//	Assert.IsTrue(l >= r);
		//}
		#endregion
		
		#region Casting tests

		[Test]
		public void Explicit_StringToLocalDateTime_AreEqual()
		{
			var exp = TestStructNoMilliseconds;
			var act = (LocalDateTime)TestStructNoMilliseconds.ToString();

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_LocalDateTimeToString_AreEqual()
		{
			var exp = TestStructNoMilliseconds.ToString();
			var act = (string)TestStructNoMilliseconds;

			Assert.AreEqual(exp, act);
		}

		#endregion

		#region Properties
		#endregion

		#region Methods

		[Test]
		public void Increment_None_AreEqual()
		{
			var act = TestStruct;
			act++;
			var exp = new LocalDateTime(1988, 06, 14, 22, 10, 05, 001);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Decrement_None_AreEqual()
		{
			var act = TestStruct;
			act--;
			var exp = new LocalDateTime(1988, 06, 12, 22, 10, 05, 001);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Plus_TimeSpan_AreEqual()
		{
			var act = TestStruct + new TimeSpan(25, 30, 15);
			var exp = new LocalDateTime(1988, 06, 14, 23, 40, 20, 001);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Min_TimeSpan_AreEqual()
		{
			var act = TestStruct - new TimeSpan(25, 30, 15);
			var exp = new LocalDateTime(1988, 06, 12, 20, 39, 50, 001);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Min_LocalDateTimeTime_AreEqual()
		{
			var act = TestStruct - new LocalDateTime(1988, 06, 11, 20, 10, 05);
			var exp = new TimeSpan(2, 02, 00, 00, 001);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void AddTicks_4000000000017_AreEqual()
		{
			var act = TestStruct.AddTicks(4000000000017L);
			var exp = new LocalDateTime(1988, 06, 18, 13, 16, 45, 001);

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void AddMilliseconds_Arround3Days_AreEqual()
		{
			var act = TestStruct.AddMilliseconds(3 * 24 * 60 * 60 * 1003);
			var exp = new LocalDateTime(1988, 06, 16, 22, 23, 02, 601);

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void AddSeconds_Arround3Days_AreEqual()
		{
			var act = TestStruct.AddSeconds(3 * 24 * 60 * 64);
			var exp = new LocalDateTime(1988, 06, 17, 02, 58, 05, 001);

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void AddMinutes_2280_AreEqual()
		{
			var act = TestStruct.AddMinutes(2 * 24 * 60);
			var exp = new LocalDateTime(1988, 06, 15, 22, 10, 05, 001);

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void AddHours_41_AreEqual()
		{
			var act = TestStruct.AddHours(41);
			var exp = new LocalDateTime(1988, 06, 15, 15, 10, 05, 001);

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void AddMonths_12_AreEqual()
		{
			var act = TestStruct.AddMonths(12);
			var exp = new LocalDateTime(1989, 06, 13, 22, 10, 05, 001);

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void AddYears_Min12_AreEqual()
		{
			var act = TestStruct.AddYears(-12);
			var exp = new LocalDateTime(1976, 06, 13, 22, 10, 05, 001);

			Assert.AreEqual(exp, act);
		}

		#endregion

		#region Type converter tests

		[Test]
		public void ConverterExists_LocalDateTime_IsTrue()
		{
			TypeConverterAssert.ConverterExists(typeof(LocalDateTime));
		}

		[Test]
		public void CanNotConvertFromInt32_LocalDateTime_IsTrue()
		{
		TypeConverterAssert.CanNotConvertFrom(typeof(LocalDateTime), typeof(Int32));
		}
		[Test]
		public void CanNotConvertToInt32_LocalDateTime_IsTrue()
		{
		TypeConverterAssert.CanNotConvertTo(typeof(LocalDateTime), typeof(Int32));
		}

		[Test]
		public void CanConvertFromString_LocalDateTime_IsTrue()
		{
			TypeConverterAssert.CanConvertFromString(typeof(LocalDateTime));
		}

		[Test]
		public void CanConvertToString_LocalDateTime_IsTrue()
		{
			TypeConverterAssert.CanConvertToString(typeof(LocalDateTime));
		}
		
		[Test]
		public void ConvertFromString_StringValue_TestStruct()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(LocalDateTimeTest.TestStructNoMilliseconds, LocalDateTimeTest.TestStructNoMilliseconds.ToString());
			}
		}

		[Test]
		public void ConvertFromInstanceDescriptor_LocalDateTime_Successful()
		{
			TypeConverterAssert.ConvertFromInstanceDescriptor(typeof(LocalDateTime));
		}

		[Test]
		public void ConvertToString_TestStruct_StringValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertToStringEquals(LocalDateTimeTest.TestStruct.ToString(), LocalDateTimeTest.TestStruct);
			}
		}

		#endregion
		
		#region IsValid tests

		[Test]
		public void IsValid_Data_IsFalse()
		{
			Assert.IsFalse(LocalDateTime.IsValid("Complex"), "Complex");
			Assert.IsFalse(LocalDateTime.IsValid((String)null), "(String)null");
			Assert.IsFalse(LocalDateTime.IsValid(String.Empty), "String.MinValue");
		}
		[Test]
		public void IsValid_Data_IsTrue()
		{
			Assert.IsTrue(LocalDateTime.IsValid("1931-10-10 14:12:03.041", CultureInfo.InvariantCulture));
		}
		#endregion
	}

	[Serializable]
	public class LocalDateTimeSerializeObject
	{
		public int Id { get; set; }
		public LocalDateTime Obj { get; set; }
		public DateTime Date { get; set; }
	}
}
