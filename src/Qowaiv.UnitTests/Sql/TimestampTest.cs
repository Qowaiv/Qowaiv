using NUnit.Framework;
using Qowaiv.Sql;
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

namespace Qowaiv.UnitTests.Sql
{
	/// <summary>Tests the timestamp SVO.</summary>
	[TestFixture]
	public class TimestampTest
	{
		/// <summary>The test instance for most tests.</summary>
		public static readonly Timestamp TestStruct = 123456789L;

		#region TryParse tests

		/// <summary>TryParse null should be valid.</summary>
		[Test]
		public void TyrParse_Null_IsValid()
		{
			Timestamp val;

			string str = null;

			Assert.IsFalse(Timestamp.TryParse(str, out val), "Valid");
			Assert.AreEqual(Timestamp.MinValue, val, "Value");
		}

		/// <summary>TryParse string.Empty should be valid.</summary>
		[Test]
		public void TyrParse_StringEmpty_IsValid()
		{
			Timestamp val;

			string str = string.Empty;

			Assert.IsFalse(Timestamp.TryParse(str, out val), "Valid");
			Assert.AreEqual(Timestamp.MinValue, val, "Value");
		}

		[Test]
		public void TyrParse_0x00000000075BCD15_IsValid()
		{
			Timestamp val;

			string str = "0x00000000075BCD15";

			Assert.IsTrue(Timestamp.TryParse(str, out val), "Valid");
			Assert.AreEqual(TestStruct, val, "Value");
		}
		[Test]
		public void TyrParse_123456789_IsValid()
		{
			Timestamp val;

			string str = "123456789";

			Assert.IsTrue(Timestamp.TryParse(str, out val), "Valid");
			Assert.AreEqual(TestStruct, val, "Value");
		}

		/// <summary>TryParse with specified string value should be invalid.</summary>
		[Test]
		public void TyrParse_invalidTimeStamp_IsNotValid()
		{
			Timestamp val;

			string str = "invalidTimeStamp";

			Assert.IsFalse(Timestamp.TryParse(str, out val), "Valid");
			Assert.AreEqual(Timestamp.MinValue, val, "Value");
		}
		/// <summary>TryParse with specified string value should be invalid.</summary>
		[Test]
		public void TyrParse_0xInvalidTimeStamp_IsNotValid()
		{
			Timestamp val;

			string str = "0xInvalidTimeStamp";

			Assert.IsFalse(Timestamp.TryParse(str, out val), "Valid");
			Assert.AreEqual(Timestamp.MinValue, val, "Value");
		}

		[Test]
		public void Parse_InvalidInput_ThrowsFormatException()
		{
			using (new CultureInfoScope("en-GB"))
			{
				Assert.Catch<FormatException>
				(() =>
				{
					Timestamp.Parse("InvalidInput");
				},
				"Not a valid SQL timestamp");
			}
		}

		[Test]
		public void TryParse_TestStructInput_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = TestStruct;
				var act = Timestamp.TryParse(exp.ToString());

				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void TryParse_InvalidInput_DefaultValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = Timestamp.MinValue;
				var act = Timestamp.TryParse("InvalidInput");

				Assert.AreEqual(exp, act);
			}
		}

		#endregion

		#region Create tests

		[Test]
		public void Create_Length8ByteArray_Is578437695752307201()
		{
			Timestamp act = Timestamp.Create(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
			Timestamp exp = 578437695752307201L;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Create_Length6ByteArray_throwsArgumentException()
		{
			ExceptionAssert.CatchArgumentException(() =>
			{
				Timestamp.Create(new byte[] { 1, 2, 3, 4, 5, 6 });
			},
			"bytes",
			"The byte array should have size of 8.");
		}
		[Test]
		public void Create_NegativeInteger_18446744073709551593()
		{
			Timestamp act = Timestamp.Create(-23);
			Timestamp exp = 18446744073709551593L;

			Assert.AreEqual(exp, act);
		}

		#endregion

		#region (XML) (De)serialization tests

		[Test]
		public void Constructor_SerializationInfoIsNull_ThrowsArgumentNullException()
		{
			ExceptionAssert.CatchArgumentNullException
			(() =>
			{
				SerializationTest.DeserializeUsingConstructor<Timestamp>(null, default(StreamingContext));
			},
			"info");
		}

		[Test]
		public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
		{
			Assert.Catch<SerializationException>
			(() =>
			{
				var info = new SerializationInfo(typeof(Timestamp), new System.Runtime.Serialization.FormatterConverter());
				SerializationTest.DeserializeUsingConstructor<Timestamp>(info, default(StreamingContext));
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
			var info = new SerializationInfo(typeof(Timestamp), new System.Runtime.Serialization.FormatterConverter());
			obj.GetObjectData(info, default(StreamingContext));

			Assert.AreEqual((Int64)123456789, info.GetInt64("Value"));
		}

		[Test]
		public void SerializeDeserialize_TestStruct_AreEqual()
		{
			var input = TimestampTest.TestStruct;
			var exp = TimestampTest.TestStruct;
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void DataContractSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = TimestampTest.TestStruct;
			var exp = TimestampTest.TestStruct;
			var act = SerializationTest.DataContractSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void XmlSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = TimestampTest.TestStruct;
			var exp = TimestampTest.TestStruct;
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void SerializeDeserialize_TimestampSerializeObject_AreEqual()
		{
			var input = new TimestampSerializeObject()
			{
				Id = 17,
				Obj = TimestampTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new TimestampSerializeObject()
			{
				Id = 17,
				Obj = TimestampTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			DateTimeAssert.AreEqual(exp.Date, act.Date, "Date");;
		}
		[Test]
		public void XmlSerializeDeserialize_TimestampSerializeObject_AreEqual()
		{
			var input = new TimestampSerializeObject()
			{
				Id = 17,
				Obj = TimestampTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new TimestampSerializeObject()
			{
				Id = 17,
				Obj = TimestampTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			DateTimeAssert.AreEqual(exp.Date, act.Date, "Date");;
		}
		[Test]
		public void DataContractSerializeDeserialize_TimestampSerializeObject_AreEqual()
		{
			var input = new TimestampSerializeObject()
			{
				Id = 17,
				Obj = TimestampTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new TimestampSerializeObject()
			{
				Id = 17,
				Obj = TimestampTest.TestStruct,
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
			var input = new TimestampSerializeObject()
			{
				Id = 17,
				Obj = TimestampTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new TimestampSerializeObject()
			{
				Id = 17,
				Obj = TimestampTest.TestStruct,
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
			var input = new TimestampSerializeObject()
			{
				Id = 17,
				Obj = Timestamp.MinValue,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new TimestampSerializeObject()
			{
				Id = 17,
				Obj = Timestamp.MinValue,
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
				JsonTester.Read<Timestamp>();
			},
			"JSON deserialization from null is not supported.");
		}

		[Test]
		public void FromJson_InvalidStringValue_AssertFormatException()
		{
			Assert.Catch<FormatException>(() =>
			{
				JsonTester.Read<Timestamp>("InvalidStringValue");
			},
			"Not a valid SQL timestamp");
		}
		[Test]
		public void FromJson_StringValue_AreEqual()
		{
			var act = JsonTester.Read<Timestamp>(TestStruct.ToString(CultureInfo.InvariantCulture));
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_Int64Value_AreEqual()
		{
			var act = JsonTester.Read<Timestamp>((Int64)((UInt64)TestStruct));
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}
		
		[Test]
		public void FromJson_DoubleValue_AreEqual()
		{
			var act = JsonTester.Read<Timestamp>((Double)(UInt64)TestStruct);
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}
		
		[Test]
		public void FromJson_DateTimeValue_AssertNotSupportedException()
		{
			Assert.Catch<NotSupportedException>(() =>
			{
				JsonTester.Read<Timestamp>(new DateTime(1972, 02, 14));
			},
			"JSON deserialization from a date is not supported.");
		}

		[Test]
		public void ToJson_DefaultValue_AreEqual()
		{
			object act = JsonTester.Write(Timestamp.MinValue);
			object exp = "0x0000000000000000";

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
		public void ToString_MinValue_StringEmpty()
		{
			var act = Timestamp.MinValue.ToString();
			var exp = "0x0000000000000000";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_MaxValue_QuestionMark()
		{
			var act = Timestamp.MaxValue.ToString();
			var exp = "0xFFFFFFFFFFFFFFFF";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_CustomFormatter_SupportsCustomFormatting()
		{
			var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
			var exp = "Unit Test Formatter, value: '0x00000000075BCD15', format: 'Unit Test Format'";

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void ToString_TestStruct_ComplexPattern()
		{
			var act = TestStruct.ToString("");
			var exp = "0x00000000075BCD15";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_ValueDutchBelgium_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = Timestamp.Parse("1600").ToString();
				var exp = "0x0000000000000640";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_FormatValueDutchBelgium_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = Timestamp.Parse("800").ToString("0000");
				var exp = "0800";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_FormatValueEnglishGreatBritain_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var act = Timestamp.Parse("800").ToString("0000");
				var exp = "0800";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_FormatValueSpanishEcuador_AreEqual()
		{
			var act = Timestamp.Parse("1700").ToString("00000.0", new CultureInfo("es-EC"));
			var exp = "01700,0";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void DebuggerDisplay_DebugToString_HasAttribute()
		{
			DebuggerDisplayAssert.HasAttribute(typeof(Timestamp));
		}

		[Test]
		public void DebuggerDisplay_TestStruct_String()
		{
			DebuggerDisplayAssert.HasResult("0x00000000075BCD15", TestStruct);
		}

		#endregion

		#region IEquatable tests

		/// <summary>GetHash should not fail for Timestamp.MinValue.</summary>
		[Test]
		public void GetHash_Empty_Hash()
		{
			Assert.AreEqual(0, Timestamp.MinValue.GetHashCode());
		}

		/// <summary>GetHash should not fail for the test struct.</summary>
		[Test]
		public void GetHash_TestStruct_Hash()
		{
			Assert.AreEqual(123456789, TimestampTest.TestStruct.GetHashCode());
		}

		[Test]
		public void Equals_EmptyEmpty_IsTrue()
		{
			Assert.IsTrue(Timestamp.MinValue.Equals(Timestamp.MinValue));
		}

		[Test]
		public void Equals_FormattedAndUnformatted_IsTrue()
		{
			var l = Timestamp.Parse("0x75bcd15", CultureInfo.InvariantCulture);
			var r = Timestamp.Parse("0x00000000075BCD15", CultureInfo.InvariantCulture);

			Assert.IsTrue(l.Equals(r));
		}

		[Test]
		public void Equals_TestStructTestStruct_IsTrue()
		{
			Assert.IsTrue(TimestampTest.TestStruct.Equals(TimestampTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructEmpty_IsFalse()
		{
			Assert.IsFalse(TimestampTest.TestStruct.Equals(Timestamp.MinValue));
		}

		[Test]
		public void Equals_EmptyTestStruct_IsFalse()
		{
			Assert.IsFalse(Timestamp.MinValue.Equals(TimestampTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructObjectTestStruct_IsTrue()
		{
			Assert.IsTrue(TimestampTest.TestStruct.Equals((object)TimestampTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructNull_IsFalse()
		{
			Assert.IsFalse(TimestampTest.TestStruct.Equals(null));
		}

		[Test]
		public void Equals_TestStructObject_IsFalse()
		{
			Assert.IsFalse(TimestampTest.TestStruct.Equals(new object()));
		}

		[Test]
		public void OperatorIs_TestStructTestStruct_IsTrue()
		{
			var l = TimestampTest.TestStruct;
			var r = TimestampTest.TestStruct;
			Assert.IsTrue(l == r);
		}

		[Test]
		public void OperatorIsNot_TestStructTestStruct_IsFalse()
		{
			var l = TimestampTest.TestStruct;
			var r = TimestampTest.TestStruct;
			Assert.IsFalse(l != r);
		}

		#endregion

		#region IComparable tests

		/// <summary>Orders a list of timestamps ascending.</summary>
		[Test]
		public void OrderBy_Timestamp_AreEqual()
		{
			Timestamp item0 = 3245;
			Timestamp item1 = 13245;
			Timestamp item2 = 132456;
			Timestamp item3 = 1324589;

			var inp = new List<Timestamp>() { Timestamp.MinValue, item3, item2, item0, item1, Timestamp.MinValue };
			var exp = new List<Timestamp>() { Timestamp.MinValue, Timestamp.MinValue, item0, item1, item2, item3 };
			var act = inp.OrderBy(item => item).ToList();

			CollectionAssert.AreEqual(exp, act);
		}

		/// <summary>Orders a list of timestamps descending.</summary>
		[Test]
		public void OrderByDescending_Timestamp_AreEqual()
		{
			Timestamp item0 = 3245;
			Timestamp item1 = 13245;
			Timestamp item2 =132456;
			Timestamp item3 = 1324589;

			var inp = new List<Timestamp>() { Timestamp.MinValue, item3, item2, item0, item1, Timestamp.MinValue };
			var exp = new List<Timestamp>() { item3, item2, item1, item0, Timestamp.MinValue, Timestamp.MinValue };
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
				"Argument must be a timestamp"
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
				"Argument must be a timestamp"
			);
		}

		[Test]
		public void LessThan_17LT19_IsTrue()
		{
			Timestamp l = 17;
			Timestamp r = 19;

			Assert.IsTrue(l < r);
		}
		[Test]
		public void GreaterThan_21LT19_IsTrue()
		{
			Timestamp l = 21;
			Timestamp r = 19;

			Assert.IsTrue(l > r);
		}

		[Test]
		public void LessThanOrEqual_17LT19_IsTrue()
		{
			Timestamp l = 17;
			Timestamp r = 19;

			Assert.IsTrue(l <= r);
		}
		[Test]
		public void GreaterThanOrEqual_21LT19_IsTrue()
		{
			Timestamp l = 21;
			Timestamp r = 19;

			Assert.IsTrue(l >= r);
		}

		[Test]
		public void LessThanOrEqual_17LT17_IsTrue()
		{
			Timestamp l = 17;
			Timestamp r = 17;

			Assert.IsTrue(l <= r);
		}
		[Test]
		public void GreaterThanOrEqual_21LT21_IsTrue()
		{
			Timestamp l = 21;
			Timestamp r = 21;

			Assert.IsTrue(l >= r);
		}
		#endregion
		
		#region Methods

		[Test]
		public void ToByteArray_TestStruct_()
		{
			var act = TestStruct.ToByteArray();
			var exp = new byte[] { 21, 205, 91, 7, 0, 0, 0, 0 };

			CollectionAssert.AreEqual(exp, act);
		}

		#endregion

		#region Casting tests

		[Test]
		public void Explicit_StringToTimestamp_AreEqual()
		{
			var exp = TestStruct;
			var act = (Timestamp)TestStruct.ToString();

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_TimestampToString_AreEqual()
		{
			var exp = TestStruct.ToString();
			var act = (string)TestStruct;

			Assert.AreEqual(exp, act);
		}

		

	[Test]
		public void Explicit_ByteArrayToTimestamp_AreEqual()
		{
			var exp = TestStruct;
			var act = (Timestamp)new byte[] { 21, 205, 91, 7, 0, 0, 0, 0 };

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_TimestampToByteArray_AreEqual()
		{
			var exp = new byte[] { 21, 205, 91, 7, 0, 0, 0, 0 };
			var act = (byte[])TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Explicit_Int64ToTimestamp_AreEqual()
		{
			var exp = TestStruct;
			var act = (Timestamp)123456789L;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_TimestampToInt64_AreEqual()
		{
			var exp = 123456789L;
			var act = (Int64)TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Explicit_UInt64ToTimestamp_AreEqual()
		{
			var exp = TestStruct;
			var act = (Timestamp)123456789UL;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_TimestampToUInt64_AreEqual()
		{
			var exp = 123456789UL;
			var act = (UInt64)TestStruct;

			Assert.AreEqual(exp, act);
		}
		
		#endregion

		#region Type converter tests

		[Test]
		public void ConverterExists_Timestamp_IsTrue()
		{
			TypeConverterAssert.ConverterExists(typeof(Timestamp));
		}

		[Test]
		public void CanNotConvertFromInt32_Timestamp_IsTrue()
		{
			TypeConverterAssert.CanNotConvertFrom(typeof(Timestamp), typeof(Int32));
		}
		[Test]
		public void CanNotConvertToInt32_Timestamp_IsTrue()
		{
			TypeConverterAssert.CanNotConvertTo(typeof(Timestamp), typeof(Int32));
		}

		[Test]
		public void CanConvertFromString_Timestamp_IsTrue()
		{
			TypeConverterAssert.CanConvertFromString(typeof(Timestamp));
		}

		[Test]
		public void CanConvertToString_Timestamp_IsTrue()
		{
			TypeConverterAssert.CanConvertToString(typeof(Timestamp));
		}

		[Test]
		public void ConvertFromString_StringValue_TestStruct()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(TimestampTest.TestStruct, TimestampTest.TestStruct.ToString());
			}
		}

		[Test]
		public void ConvertFromInstanceDescriptor_Timestamp_Successful()
		{
			TypeConverterAssert.ConvertFromInstanceDescriptor(typeof(Timestamp));
		}

		[Test]
		public void ConvertToString_TestStruct_StringValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertToStringEquals(TimestampTest.TestStruct.ToString(), TimestampTest.TestStruct);
			}
		}

		#endregion
		
		#region IsValid tests

		[Test]
		public void IsValid_Data_IsFalse()
		{
			Assert.IsFalse(Timestamp.IsValid((String)null), "(String)null");
			Assert.IsFalse(Timestamp.IsValid(string.Empty), "string.Empty");

			Assert.IsFalse(Timestamp.IsValid("75bcd15"), "75bcd15");
		}
		[Test]
		public void IsValid_Data_IsTrue()
		{
			Assert.IsTrue(Timestamp.IsValid("0x75BCD15"));
		}
		#endregion
	}

	[Serializable]
	public class TimestampSerializeObject
	{
		public int Id { get; set; }
		public Timestamp Obj { get; set; }
		public DateTime Date { get; set; }
	}
}
