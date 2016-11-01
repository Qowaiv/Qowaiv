using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.UnitTests.Json;
using Qowaiv.UnitTests.TestTools;
using Qowaiv.UnitTests.TestTools.Formatting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests
{
	/// <summary>Tests the year SVO.</summary>
	[TestFixture]
	public class YearTest
	{
		/// <summary>The test instance for most tests.</summary>
		public static readonly Year TestStruct = 1979;

		#region year const tests

		/// <summary>Year.Empty should be equal to the default of year.</summary>
		[Test]
		public void Empty_None_EqualsDefault()
		{
			Assert.AreEqual(default(Year), Year.Empty);
		}

		#endregion

		#region year IsEmpty tests

		/// <summary>Year.IsEmpty() should be true for the default of year.</summary>
		[Test]
		public void IsEmpty_Default_IsTrue()
		{
			Assert.IsTrue(default(Year).IsEmpty());
		}
		/// <summary>Year.IsEmpty() should be false for Year.Unknown.</summary>
		[Test]
		public void IsEmpty_Unknown_IsFalse()
		{
			Assert.IsFalse(Year.Unknown.IsEmpty());
		}
		/// <summary>Year.IsEmpty() should be false for the TestStruct.</summary>
		[Test]
		public void IsEmpty_TestStruct_IsFalse()
		{
			Assert.IsFalse(TestStruct.IsEmpty());
		}

		/// <summary>Year.IsUnknown() should be false for the default of year.</summary>
		[Test]
		public void IsUnknown_Default_IsFalse()
		{
			Assert.IsFalse(default(Year).IsUnknown());
		}
		/// <summary>Year.IsUnknown() should be true for Year.Unknown.</summary>
		[Test]
		public void IsUnknown_Unknown_IsTrue()
		{
			Assert.IsTrue(Year.Unknown.IsUnknown());
		}
		/// <summary>Year.IsUnknown() should be false for the TestStruct.</summary>
		[Test]
		public void IsUnknown_TestStruct_IsFalse()
		{
			Assert.IsFalse(TestStruct.IsUnknown());
		}

		/// <summary>Year.IsEmptyOrUnknown() should be true for the default of year.</summary>
		[Test]
		public void IsEmptyOrUnknown_Default_IsFalse()
		{
			Assert.IsTrue(default(Year).IsEmptyOrUnknown());
		}
		/// <summary>Year.IsEmptyOrUnknown() should be true for Year.Unknown.</summary>
		[Test]
		public void IsEmptyOrUnknown_Unknown_IsTrue()
		{
			Assert.IsTrue(Year.Unknown.IsEmptyOrUnknown());
		}
		/// <summary>Year.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
		[Test]
		public void IsEmptyOrUnknown_TestStruct_IsFalse()
		{
			Assert.IsFalse(TestStruct.IsEmptyOrUnknown());
		}

		#endregion

		#region TryParse tests

		/// <summary>TryParse null should be valid.</summary>
		[Test]
		public void TyrParse_Null_IsValid()
		{
			Year val;

			string str = null;

			Assert.IsTrue(Year.TryParse(str, out val), "Valid");
			Assert.AreEqual(string.Empty, val.ToString(), "Value");
		}

		/// <summary>TryParse string.Empty should be valid.</summary>
		[Test]
		public void TyrParse_StringEmpty_IsValid()
		{
			Year val;

			string str = string.Empty;

			Assert.IsTrue(Year.TryParse(str, out val), "Valid");
			Assert.AreEqual(string.Empty, val.ToString(), "Value");
		}

		/// <summary>TryParse "?" should be valid and the result should be Year.Unknown.</summary>
		[Test]
		public void TyrParse_Questionmark_IsValid()
		{
			Year val;

			string str = "?";

			Assert.IsTrue(Year.TryParse(str, out val), "Valid");
			Assert.IsTrue(val.IsUnknown(), "Value");
		}

		/// <summary>TryParse with specified string value should be valid.</summary>
		[Test]
		public void TyrParse_StringValue_IsValid()
		{
			Year val;

			string str = "1979";

			Assert.IsTrue(Year.TryParse(str, out val), "Valid");
			Assert.AreEqual(str, val.ToString(), "Value");
		}

		/// <summary>TryParse with specified string value should be invalid.</summary>
		[Test]
		public void TyrParse_StringValue_IsNotValid()
		{
			Year val;

			string str = "string";

			Assert.IsFalse(Year.TryParse(str, out val), "Valid");
			Assert.AreEqual(string.Empty, val.ToString(), "Value");
		}

		[Test]
		public void Parse_Unknown_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var act = Year.Parse("?");
				var exp = Year.Unknown;
				Assert.AreEqual(exp, act);
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
					Year.Parse("InvalidInput");
				},
				"Not a valid year");
			}
		}

		[Test]
		public void TryParse_TestStructInput_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = TestStruct;
				var act = Year.TryParse(exp.ToString());

				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void TryParse_InvalidInput_DefaultValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = default(Year);
				var act = Year.TryParse("InvalidInput");

				Assert.AreEqual(exp, act);
			}
		}

		#endregion

		#region TryCreate tests

		[Test]
		public void TryCreate_Null_IsEmpty()
		{
			Year exp = Year.Empty;
			Year act;

			Assert.IsTrue(Year.TryCreate(null, out act));
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void TryCreate_Int32MinValue_IsEmpty()
		{
			Year exp = Year.Empty;
			Year act;

			Assert.IsFalse(Year.TryCreate(Int32.MinValue, out act));
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void TryCreate_Int32MinValue_AreEqual()
		{
			var exp = Year.Empty;
			var act = Year.TryCreate(Int32.MinValue);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void TryCreate_Value_AreEqual()
		{
			var exp = TestStruct;
			var act = Year.TryCreate(1979);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Create_Int32MinValue_ThrowsArgumentOutOfRangeException()
		{
			ExceptionAssert.CatchArgumentOutOfRangeException
			(() =>
			{
				Year.Create(Int32.MinValue);
			},
			"val",
			"Not a valid year");
		}

		#endregion

		#region (XML) (De)serialization tests

		[Test]
		public void Constructor_SerializationInfoIsNull_ThrowsArgumentNullException()
		{
			ExceptionAssert.CatchArgumentNullException
			(() =>
			{
				SerializationTest.DeserializeUsingConstructor<Year>(null, default(StreamingContext));
			},
			"info");
		}

		[Test]
		public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
		{
			Assert.Catch<SerializationException>
			(() =>
			{
				var info = new SerializationInfo(typeof(Year), new System.Runtime.Serialization.FormatterConverter());
				SerializationTest.DeserializeUsingConstructor<Year>(info, default(StreamingContext));
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
			var info = new SerializationInfo(typeof(Year), new System.Runtime.Serialization.FormatterConverter());
			obj.GetObjectData(info, default(StreamingContext));

			Assert.AreEqual(1979, info.GetInt16("Value"));
		}

		[Test]
		public void SerializeDeserialize_TestStruct_AreEqual()
		{
			var input = YearTest.TestStruct;
			var exp = YearTest.TestStruct;
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void DataContractSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = YearTest.TestStruct;
			var exp = YearTest.TestStruct;
			var act = SerializationTest.DataContractSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void XmlSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = YearTest.TestStruct;
			var exp = YearTest.TestStruct;
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void SerializeDeserialize_YearSerializeObject_AreEqual()
		{
			var input = new YearSerializeObject()
			{
				Id = 17,
				Obj = YearTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new YearSerializeObject()
			{
				Id = 17,
				Obj = YearTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			DateTimeAssert.AreEqual(exp.Date, act.Date, "Date");;
		}
		[Test]
		public void XmlSerializeDeserialize_YearSerializeObject_AreEqual()
		{
			var input = new YearSerializeObject()
			{
				Id = 17,
				Obj = YearTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new YearSerializeObject()
			{
				Id = 17,
				Obj = YearTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			DateTimeAssert.AreEqual(exp.Date, act.Date, "Date");;
		}
		[Test]
		public void DataContractSerializeDeserialize_YearSerializeObject_AreEqual()
		{
			var input = new YearSerializeObject()
			{
				Id = 17,
				Obj = YearTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new YearSerializeObject()
			{
				Id = 17,
				Obj = YearTest.TestStruct,
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
			var input = new YearSerializeObject()
			{
				Id = 17,
				Obj = YearTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new YearSerializeObject()
			{
				Id = 17,
				Obj = YearTest.TestStruct,
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
			var input = new YearSerializeObject()
			{
				Id = 17,
				Obj = Year.Empty,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new YearSerializeObject()
			{
				Id = 17,
				Obj = Year.Empty,
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
		public void FromJson_None_EmptyValue()
		{
			var act = JsonTester.Read<Year>();
			var exp = Year.Empty;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_InvalidStringValue_AssertFormatException()
		{
			Assert.Catch<FormatException>(() =>
			{
				JsonTester.Read<Year>("InvalidStringValue");
			},
			"Not a valid year");
		}
		[Test]
		public void FromJson_StringValue_AreEqual()
		{
			var act = JsonTester.Read<Year>(TestStruct.ToString(CultureInfo.InvariantCulture));
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_Int64Value_AreEqual()
		{
			var act = JsonTester.Read<Year>((Int64)TestStruct);
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_DoubleValue_AreEqual()
		{
			var act = JsonTester.Read<Year>((Double)TestStruct);
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_DateTimeValue_AssertNotSupportedException()
		{
			Assert.Catch<NotSupportedException>(() =>
			{
				JsonTester.Read<Year>(new DateTime(1972, 02, 14));
			},
			"JSON deserialization from a date is not supported.");
		}

		[Test]
		public void ToJson_DefaultValue_AreEqual()
		{
			object act = JsonTester.Write(default(Year));
			object exp = null;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void ToJson_Unknown_AreEqual()
		{
			var act = JsonTester.Write(Year.Unknown);
			var exp = "?";

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void ToJson_TestStruct_AreEqual()
		{
			var act = JsonTester.Write(TestStruct);
			var exp = (Int16)TestStruct;

			Assert.AreEqual(exp, act);
		}

		#endregion

		#region IFormattable / Tostring tests

		[Test]
		public void ToString_CustomFormatter_SupportsCustomFormatting()
		{
			var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
			var exp = "Unit Test Formatter, value: '1979', format: 'Unit Test Format'";

			Assert.AreEqual(exp, act);
		}

		[TestCase("en-US", "", "1979", "1979")]
		[TestCase("en-GB", "", "?", "?")]
		[TestCase("nl-BE", "0000", "800", "0800")]
		[TestCase("en-GB", "0000", "800", "0800")]
		public void ToString_UsingCultureWithPattern(string culture, string format, string str, string expected)
		{
			using (new CultureInfoScope(culture))
			{
				var actual = Year.Parse(str).ToString(format);
				Assert.AreEqual(expected, actual);
			}
		}

		[Test]
		public void ToString_FormatValueSpanishEcuador_AreEqual()
		{
			var act = Year.Parse("1700").ToString("00000.0", new CultureInfo("es-EC"));
			var exp = "01700,0";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void DebuggerDisplay_DebugToString_HasAttribute()
		{
			DebuggerDisplayAssert.HasAttribute(typeof(Year));
		}

		[Test]
		public void DebuggerDisplay_DefaultValue_String()
		{
			DebuggerDisplayAssert.HasResult("Year: (empty)", default(Year));
		}
		[Test]
		public void DebuggerDisplay_Unknown_String()
		{
			DebuggerDisplayAssert.HasResult("Year: (unknown)", Year.Unknown);
		}

		[Test]
		public void DebuggerDisplay_TestStruct_String()
		{
			DebuggerDisplayAssert.HasResult("Year: 1979", TestStruct);
		}

		#endregion

		#region IEquatable tests

		/// <summary>GetHash should not fail for Year.Empty.</summary>
		[Test]
		public void GetHash_Empty_Hash()
		{
			Assert.AreEqual(0, Year.Empty.GetHashCode());
		}

		/// <summary>GetHash should not fail for the test struct.</summary>
		[Test]
		public void GetHash_TestStruct_Hash()
		{
			Assert.AreEqual(1979, YearTest.TestStruct.GetHashCode());
		}

		[Test]
		public void Equals_EmptyEmpty_IsTrue()
		{
			Assert.IsTrue(Year.Empty.Equals(Year.Empty));
		}

		[Test]
		public void Equals_TestStructTestStruct_IsTrue()
		{
			Assert.IsTrue(YearTest.TestStruct.Equals(YearTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructEmpty_IsFalse()
		{
			Assert.IsFalse(YearTest.TestStruct.Equals(Year.Empty));
		}

		[Test]
		public void Equals_EmptyTestStruct_IsFalse()
		{
			Assert.IsFalse(Year.Empty.Equals(YearTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructObjectTestStruct_IsTrue()
		{
			Assert.IsTrue(YearTest.TestStruct.Equals((object)YearTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructNull_IsFalse()
		{
			Assert.IsFalse(YearTest.TestStruct.Equals(null));
		}

		[Test]
		public void Equals_TestStructObject_IsFalse()
		{
			Assert.IsFalse(YearTest.TestStruct.Equals(new object()));
		}

		[Test]
		public void OperatorIs_TestStructTestStruct_IsTrue()
		{
			var l = YearTest.TestStruct;
			var r = YearTest.TestStruct;
			Assert.IsTrue(l == r);
		}

		[Test]
		public void OperatorIsNot_TestStructTestStruct_IsFalse()
		{
			var l = YearTest.TestStruct;
			var r = YearTest.TestStruct;
			Assert.IsFalse(l != r);
		}

		#endregion

		#region IComparable tests

		/// <summary>Orders a list of years ascending.</summary>
		[Test]
		public void OrderBy_Year_AreEqual()
		{
			Year item0 = 1980;
			Year item1 = 1981;
			Year item2 = 1982;
			Year item3 = 1983;

			var inp = new List<Year>() { Year.Empty, item3, item2, item0, item1, Year.Empty };
			var exp = new List<Year>() { Year.Empty, Year.Empty, item0, item1, item2, item3 };
			var act = inp.OrderBy(item => item).ToList();

			CollectionAssert.AreEqual(exp, act);
		}

		/// <summary>Orders a list of years descending.</summary>
		[Test]
		public void OrderByDescending_Year_AreEqual()
		{
			Year item0 = 1980;
			Year item1 = 1981;
			Year item2 = 1982;
			Year item3 = 1983;

			var inp = new List<Year>() { Year.Empty, item3, item2, item0, item1, Year.Empty };
			var exp = new List<Year>() { item3, item2, item1, item0, Year.Empty, Year.Empty };
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
				"Argument must be a year"
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
				"Argument must be a year"
			);
		}

		[Test]
		public void LessThan_17LT19_IsTrue()
		{
			Year l = 17;
			Year r = 19;

			Assert.IsTrue(l < r);
		}
		[Test]
		public void GreaterThan_21LT19_IsTrue()
		{
			Year l = 21;
			Year r = 19;

			Assert.IsTrue(l > r);
		}

		[Test]
		public void LessThanOrEqual_17LT19_IsTrue()
		{
			Year l = 17;
			Year r = 19;

			Assert.IsTrue(l <= r);
		}
		[Test]
		public void GreaterThanOrEqual_21LT19_IsTrue()
		{
			Year l = 21;
			Year r = 19;

			Assert.IsTrue(l >= r);
		}

		[Test]
		public void LessThanOrEqual_17LT17_IsTrue()
		{
			Year l = 17;
			Year r = 17;

			Assert.IsTrue(l <= r);
		}
		[Test]
		public void GreaterThanOrEqual_21LT21_IsTrue()
		{
			Year l = 21;
			Year r = 21;

			Assert.IsTrue(l >= r);
		}
		#endregion

		#region Casting tests

		[Test]
		public void Explicit_StringToYear_AreEqual()
		{
			var exp = TestStruct;
			var act = (Year)TestStruct.ToString();

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_YearToString_AreEqual()
		{
			var exp = TestStruct.ToString();
			var act = (string)TestStruct;

			Assert.AreEqual(exp, act);
		}


		[Test]
		public void Explicit_Int32ToYear_AreEqual()
		{
			var exp = TestStruct;
			var act = (Year)1979;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_YearToInt32_AreEqual()
		{
			var exp = 1979;
			var act = (Int32)TestStruct;

			Assert.AreEqual(exp, act);
		}
		#endregion

		#region Properties

		[Test]
		public void IsLeapYear_Empty_IsFalse()
		{
			Assert.IsFalse(Year.Empty.IsLeapYear);
		}

		[Test]
		public void IsLeapYear_Unknown_IsFalse()
		{
			Assert.IsFalse(Year.Unknown.IsLeapYear);
		}

		[Test]
		public void IsLeapYear_TestStruct_IsFalse()
		{
			Assert.IsFalse(TestStruct.IsLeapYear);
		}

		[Test]
		public void IsLeapYear_1980_IsTrue()
		{
			Year year = 1980;
			Assert.IsTrue(year.IsLeapYear);
		}

		#endregion

		#region Type converter tests

		[Test]
		public void ConverterExists_Year_IsTrue()
		{
			TypeConverterAssert.ConverterExists(typeof(Year));
		}

		[Test]
		public void CanNotConvertFromInt32_Year_IsTrue()
		{
			TypeConverterAssert.CanNotConvertFrom(typeof(Year), typeof(Int32));
		}
		[Test]
		public void CanNotConvertToInt32_Year_IsTrue()
		{
			TypeConverterAssert.CanNotConvertTo(typeof(Year), typeof(Int32));
		}

		[Test]
		public void CanConvertFromString_Year_IsTrue()
		{
			TypeConverterAssert.CanConvertFromString(typeof(Year));
		}

		[Test]
		public void CanConvertToString_Year_IsTrue()
		{
			TypeConverterAssert.CanConvertToString(typeof(Year));
		}

		[Test]
		public void ConvertFrom_StringNull_YearEmpty()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(Year.Empty, (string)null);
			}
		}

		[Test]
		public void ConvertFrom_StringEmpty_YearEmpty()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(Year.Empty, string.Empty);
			}
		}

		[Test]
		public void ConvertFromString_StringValue_TestStruct()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(YearTest.TestStruct, YearTest.TestStruct.ToString());
			}
		}

		[Test]
		public void ConvertFromInstanceDescriptor_Year_Successful()
		{
			TypeConverterAssert.ConvertFromInstanceDescriptor(typeof(Year));
		}

		[Test]
		public void ConvertToString_TestStruct_StringValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertToStringEquals(YearTest.TestStruct.ToString(), YearTest.TestStruct);
			}
		}

		#endregion

		#region IsValid tests

		[TestCase("0000")]
		[TestCase("000")]
		[TestCase("00")]
		[TestCase("0")]
		[TestCase((string)null)]
		[TestCase("")]
		public void IsInvalid_String(string value)
		{
			Assert.IsFalse(Year.IsValid(value));
		}

		[TestCase]
		public void IsInvalid_NullabelInt()
		{
			int? number = null;
			Assert.IsFalse(Year.IsValid(number));
		}
		[Test]
		public void IsValid_Data_IsTrue()
		{
			Assert.IsTrue(Year.IsValid("0001"), "0001");
			Assert.IsTrue(Year.IsValid("1800"), "1800");
		}
		#endregion
	}

	[Serializable]
	public class YearSerializeObject
	{
		public int Id { get; set; }
		public Year Obj { get; set; }
		public DateTime Date { get; set; }
	}
}
