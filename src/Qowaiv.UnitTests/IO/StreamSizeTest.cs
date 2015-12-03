using NUnit.Framework;
using Qowaiv.IO;
using Qowaiv.UnitTests.Json;
using Qowaiv.UnitTests.TestTools;
using Qowaiv.UnitTests.TestTools.Formatting;
using Qowaiv.UnitTests.TestTools.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests.IO
{
	/// <summary>Tests the stream size SVO.</summary>
	[TestFixture]
	public class StreamSizeTest
	{
		/// <summary>The test instance for most tests.</summary>
		public static readonly StreamSize TestStruct = 123456789;

		#region stream size const tests

		/// <summary>StreamSize.Empty should be equal to the default of stream size.</summary>
		[Test]
		public void Empty_None_EqualsDefault()
		{
			Assert.AreEqual(default(StreamSize), StreamSize.Zero);
		}

		#endregion

		#region TryParse tests

		/// <summary>TryParse null should be valid.</summary>
		[Test]
		public void TyrParse_Null_IsInvalid()
		{
			StreamSize val;
			string str = null;

			Assert.IsFalse(StreamSize.TryParse(str, out val));
		}

		/// <summary>TryParse string.Empty should be valid.</summary>
		[Test]
		public void TyrParse_StringEmpty_IsInvalid()
		{
			StreamSize val;
			string str = string.Empty;

			Assert.IsFalse(StreamSize.TryParse(str, out val));
		}

		/// <summary>TryParse with specified string value should be valid.</summary>
		[Test]
		public void TyrParse_StringValue_IsValid()
		{
			StreamSize val;

			string str = "17kb";

			Assert.IsTrue(StreamSize.TryParse(str, out val), "Valid");
			Assert.AreEqual("17000 byte", val.ToString(), "Value");
		}

		[Test]
		public void Parse_InvalidInput_ThrowsFormatException()
		{
			using (new CultureInfoScope("en-GB"))
			{
				Assert.Catch<FormatException>
				(() =>
				{
					StreamSize.Parse("InvalidInput");
				},
				"Not a valid stream size");
			}
		}

		[Test]
		public void TryParse_TestStructInput_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = TestStruct;
				var act = StreamSize.TryParse(exp.ToString());

				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void TryParse_InvalidInput_DefaultValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = default(StreamSize);
				var act = StreamSize.TryParse("InvalidInput");

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
				SerializationTest.DeserializeUsingConstructor<StreamSize>(null, default(StreamingContext));
			},
			"info");
		}

		[Test]
		public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
		{
			Assert.Catch<SerializationException>
			(() =>
			{
				var info = new SerializationInfo(typeof(StreamSize), new System.Runtime.Serialization.FormatterConverter());
				SerializationTest.DeserializeUsingConstructor<StreamSize>(info, default(StreamingContext));
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
			var info = new SerializationInfo(typeof(StreamSize), new System.Runtime.Serialization.FormatterConverter());
			obj.GetObjectData(info, default(StreamingContext));

			Assert.AreEqual((Int64)123456789, info.GetInt64("Value"));
		}

		[Test]
		public void SerializeDeserialize_TestStruct_AreEqual()
		{
			var input = StreamSizeTest.TestStruct;
			var exp = StreamSizeTest.TestStruct;
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void DataContractSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = StreamSizeTest.TestStruct;
			var exp = StreamSizeTest.TestStruct;
			var act = SerializationTest.DataContractSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void XmlSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = StreamSizeTest.TestStruct;
			var exp = StreamSizeTest.TestStruct;
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void SerializeDeserialize_StreamSizeSerializeObject_AreEqual()
		{
			var input = new StreamSizeSerializeObject()
			{
				Id = 17,
				Obj = StreamSizeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new StreamSizeSerializeObject()
			{
				Id = 17,
				Obj = StreamSizeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			DateTimeAssert.AreEqual(exp.Date, act.Date, "Date");;
		}
		[Test]
		public void XmlSerializeDeserialize_StreamSizeSerializeObject_AreEqual()
		{
			var input = new StreamSizeSerializeObject()
			{
				Id = 17,
				Obj = StreamSizeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new StreamSizeSerializeObject()
			{
				Id = 17,
				Obj = StreamSizeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			DateTimeAssert.AreEqual(exp.Date, act.Date, "Date");;
		}
		[Test]
		public void DataContractSerializeDeserialize_StreamSizeSerializeObject_AreEqual()
		{
			var input = new StreamSizeSerializeObject()
			{
				Id = 17,
				Obj = StreamSizeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new StreamSizeSerializeObject()
			{
				Id = 17,
				Obj = StreamSizeTest.TestStruct,
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
			var input = new StreamSizeSerializeObject()
			{
				Id = 17,
				Obj = StreamSizeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new StreamSizeSerializeObject()
			{
				Id = 17,
				Obj = StreamSizeTest.TestStruct,
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
			var input = new StreamSizeSerializeObject()
			{
				Id = 17,
				Obj = StreamSize.Zero,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new StreamSizeSerializeObject()
			{
				Id = 17,
				Obj = StreamSize.Zero,
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
			var act = JsonTester.Read<StreamSize>();
			var exp = StreamSize.Zero;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_InvalidStringValue_AssertFormatException()
		{
			Assert.Catch<FormatException>(() =>
			{
				JsonTester.Read<StreamSize>("InvalidStringValue");
			},
			"Not a valid stream size");
		}
		[Test]
		public void FromJson_StringValue_AreEqual()
		{
			var act = JsonTester.Read<StreamSize>(TestStruct.ToString(CultureInfo.InvariantCulture));
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_Int64Value_AreEqual()
		{
			var act = JsonTester.Read<StreamSize>((Int64)TestStruct);
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_DoubleValue_AreEqual()
		{
			var act = JsonTester.Read<StreamSize>((Double)TestStruct);
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_DateTimeValue_AssertNotSupportedException()
		{
			Assert.Catch<NotSupportedException>(() =>
			{
				JsonTester.Read<StreamSize>(new DateTime(1972, 02, 14));
			},
			"JSON deserialization from a date is not supported.");
		}

		[Test]
		public void ToJson_DefaultValue_AreEqual()
		{
			object act = JsonTester.Write(default(StreamSize));
			object exp = 0;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void ToJson_TestStruct_AreEqual()
		{
			var act = JsonTester.Write(TestStruct);
			var exp = 123456789L;

			Assert.AreEqual(exp, act);
		}

		#endregion

		#region IFormattable / ToString tests

		[Test]
		public void ToString_Zero_StringEmpty()
		{
			var act = StreamSize.Zero.ToString();
			var exp = "0 byte";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_CustomFormatter_SupportsCustomFormatting()
		{
			var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
			var exp = "Unit Test Formatter, value: '123456789 byte', format: 'Unit Test Format'";

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_Null_ComplexPattern()
		{
			var act = TestStruct.ToString((String)null);
			var exp = "123456789";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_TestStruct_ComplexPattern()
		{
			var act = TestStruct.ToString("");
			var exp = "123456789";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_b_AreEqual()
		{
			using (new CultureInfoScope("nl-NL"))
			{
				var act = TestStruct.ToString("#,##0b");
				var exp = "123.456.789b";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_kB_AreEqual()
		{
			using (new CultureInfoScope("nl-NL"))
			{
				var act = TestStruct.ToString("#,##0.00 kB");
				var exp = "123.456,79 kB";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_MegaByte_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = TestStruct.ToString("0.0 MegaByte");
				var exp = "123,5 MegaByte";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_Negative_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = (-TestStruct).ToString("0.0 F");
				var exp = "-123,5 Megabyte";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_GB_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = TestStruct.ToString("0.00GB");
				var exp = "0,12GB";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_tb_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = StreamSize.PB.ToString("tb");
				var exp = "1000tb";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_pb_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = StreamSize.TB.ToString(" petabyte");
				var exp = "0,001 petabyte";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_Exabyte_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = StreamSize.MaxValue.ToString("#,##0.## Exabyte");
				var exp = "9,22 Exabyte";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_spaceF_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = TestStruct.ToString("#,##0.## F");
				var exp = "123,46 Megabyte";
				Assert.AreEqual(exp, act);
			}
		}
		[Test]
		public void ToString_spaceFLower_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = TestStruct.ToString("0 f");
				var exp = "123 megabyte";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_spaceS_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = TestStruct.ToString("0000 S");
				var exp = "0123 MB";
				Assert.AreEqual(exp, act);
			}
		}
		[Test]
		public void ToString_spaceSLower_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = TestStruct.ToString("0 s");
				var exp = "123 mb";
				Assert.AreEqual(exp, act);
			}
		}
		[Test]
		public void ToString_SLower_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = TestStruct.ToString("0s");
				var exp = "123mb";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_ValueDutchBelgium_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = StreamSize.Parse("1600,1").ToString();
				var exp = "1600 byte";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_ValueEnglishGreatBritain_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var act = StreamSize.Parse("1600.1").ToString();
				var exp = "1600 byte";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_FormatValueDutchBelgium_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = StreamSize.Parse("800").ToString("0000 byte");
				var exp = "0800 byte";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_FormatValueEnglishGreatBritain_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var act = StreamSize.Parse("800").ToString("0000");
				var exp = "0800";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_FormatValueSpanishEcuador_AreEqual()
		{
			var act = StreamSize.Parse("1700").ToString("00000.0", new CultureInfo("es-EC"));
			var exp = "01700,0";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void DebuggerDisplay_DebugToString_HasAttribute()
		{
			DebuggerDisplayAssert.HasAttribute(typeof(StreamSize));
		}

		[Test]
		public void DebuggerDisplay_DefaultValue_String()
		{
			DebuggerDisplayAssert.HasResult("0 byte", default(StreamSize));
		}

		[Test]
		public void DebuggerDisplay_TestStruct_String()
		{
			DebuggerDisplayAssert.HasResult("123.5 Megabyte", TestStruct);
		}

		#endregion

		#region IEquatable tests

		/// <summary>GetHash should not fail for StreamSize.Zero.</summary>
		[Test]
		public void GetHash_Empty_0()
		{
			Assert.AreEqual(0, StreamSize.Zero.GetHashCode());
		}

		/// <summary>GetHash should not fail for the test struct.</summary>
		[Test]
		public void GetHash_TestStruct_123456789()
		{
			Assert.AreEqual(123456789, StreamSizeTest.TestStruct.GetHashCode());
		}

		[Test]
		public void Equals_EmptyEmpty_IsTrue()
		{
			Assert.IsTrue(StreamSize.Zero.Equals(StreamSize.Zero));
		}

		[Test]
		public void Equals_FormattedAndUnformatted_IsTrue()
		{
			var l = StreamSize.Parse("12,345 byte", CultureInfo.InvariantCulture);
			var r = StreamSize.Parse("12345", CultureInfo.InvariantCulture);

			Assert.IsTrue(l.Equals(r));
		}

		[Test]
		public void Equals_TestStructTestStruct_IsTrue()
		{
			Assert.IsTrue(StreamSizeTest.TestStruct.Equals(StreamSizeTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructEmpty_IsFalse()
		{
			Assert.IsFalse(StreamSizeTest.TestStruct.Equals(StreamSize.Zero));
		}

		[Test]
		public void Equals_EmptyTestStruct_IsFalse()
		{
			Assert.IsFalse(StreamSize.Zero.Equals(StreamSizeTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructObjectTestStruct_IsTrue()
		{
			Assert.IsTrue(StreamSizeTest.TestStruct.Equals((object)StreamSizeTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructNull_IsFalse()
		{
			Assert.IsFalse(StreamSizeTest.TestStruct.Equals(null));
		}

		[Test]
		public void Equals_TestStructObject_IsFalse()
		{
			Assert.IsFalse(StreamSizeTest.TestStruct.Equals(new object()));
		}

		[Test]
		public void OperatorIs_TestStructTestStruct_IsTrue()
		{
			var l = StreamSizeTest.TestStruct;
			var r = StreamSizeTest.TestStruct;
			Assert.IsTrue(l == r);
		}

		[Test]
		public void OperatorIsNot_TestStructTestStruct_IsFalse()
		{
			var l = StreamSizeTest.TestStruct;
			var r = StreamSizeTest.TestStruct;
			Assert.IsFalse(l != r);
		}

		#endregion

		#region IComparable tests

		/// <summary>Orders a list of stream sizes ascending.</summary>
		[Test]
		public void OrderBy_StreamSize_AreEqual()
		{
			StreamSize item0 = 13465;
			StreamSize item1 = 83465;
			StreamSize item2 = 113465;
			StreamSize item3 = 773465;

			var inp = new List<StreamSize>() { StreamSize.Zero, item3, item2, item0, item1, StreamSize.Zero };
			var exp = new List<StreamSize>() { StreamSize.Zero, StreamSize.Zero, item0, item1, item2, item3 };
			var act = inp.OrderBy(item => item).ToList();

			CollectionAssert.AreEqual(exp, act);
		}

		/// <summary>Orders a list of stream sizes descending.</summary>
		[Test]
		public void OrderByDescending_StreamSize_AreEqual()
		{
			StreamSize item0 = 13465;
			StreamSize item1 = 83465;
			StreamSize item2 = 113465;
			StreamSize item3 = 773465;

			var inp = new List<StreamSize>() { StreamSize.Zero, item3, item2, item0, item1, StreamSize.Zero };
			var exp = new List<StreamSize>() { item3, item2, item1, item0, StreamSize.Zero, StreamSize.Zero };
			var act = inp.OrderByDescending(item => item).ToList();

			CollectionAssert.AreEqual(exp, act);
		}

		/// <summary>Compare with a to object casted instance should be fine.</summary>
		[Test]
		public void CompareTo_ObjectTestStruct_0()
		{
			object other = (object)TestStruct;

			var exp = 0;
			var act = TestStruct.CompareTo(other);

			Assert.AreEqual(exp, act);
		}

		/// <summary>Compare with null should throw an expception.</summary>
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
				"Argument must be a stream size"
			);
		}
		/// <summary>Compare with a random object should throw an expception.</summary>
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
				"Argument must be a stream size"
			);
		}

		[Test]
		public void LessThan_17LT19_IsTrue()
		{
			StreamSize l = 17;
			StreamSize r = 19;

			Assert.IsTrue(l < r);
		}
		[Test]
		public void GreaterThan_21LT19_IsTrue()
		{
			StreamSize l = 21;
			StreamSize r = 19;

			Assert.IsTrue(l > r);
		}

		[Test]
		public void LessThanOrEqual_17LT19_IsTrue()
		{
			StreamSize l = 17;
			StreamSize r = 19;

			Assert.IsTrue(l <= r);
		}
		[Test]
		public void GreaterThanOrEqual_21LT19_IsTrue()
		{
			StreamSize l = 21;
			StreamSize r = 19;

			Assert.IsTrue(l >= r);
		}

		[Test]
		public void LessThanOrEqual_17LT17_IsTrue()
		{
			StreamSize l = 17;
			StreamSize r = 17;

			Assert.IsTrue(l <= r);
		}
		[Test]
		public void GreaterThanOrEqual_21LT21_IsTrue()
		{
			StreamSize l = 21;
			StreamSize r = 21;

			Assert.IsTrue(l >= r);
		}
		#endregion

		#region Casting tests

		[Test]
		public void Explicit_StringToStreamSize_AreEqual()
		{
			var exp = TestStruct;
			var act = (StreamSize)TestStruct.ToString();

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_StreamSizeToString_AreEqual()
		{
			var exp = TestStruct.ToString();
			var act = (string)TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Implicit_Int32ToStreamSize_AreEqual()
		{
			StreamSize exp = TestStruct;
			StreamSize act = 123456789;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_StreamSizeToInt32_AreEqual()
		{
			var exp = 123456789;
			var act = (Int32)TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Implicit_Int64ToStreamSize_AreEqual()
		{
			var exp = TestStruct;
			StreamSize act = 123456789L;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_StreamSizeToInt64_AreEqual()
		{
			var exp = 123456789L;
			var act = (Int64)TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Explicit_DoubleToStreamSize_AreEqual()
		{
			var exp = TestStruct;
			var act = (StreamSize)123456789d;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_StreamSizeToDouble_AreEqual()
		{
			var exp = 123456789d;
			var act = (Double)TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Explicit_DecimalToStreamSize_AreEqual()
		{
			var exp = TestStruct;
			var act = (StreamSize)123456789m;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_StreamSizeToDecimal_AreEqual()
		{
			var exp = 123456789m;
			var act = (Decimal)TestStruct;

			Assert.AreEqual(exp, act);
		}


		#endregion

		#region Properties
		#endregion

		#region Stream size manipulation tests

		[Test]
		public void Increment_21_22()
		{
			StreamSize act = 21;
			StreamSize exp = 22;
			act++;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Decrement_21_20()
		{
			StreamSize act = 21;
			StreamSize exp = 20;
			act--;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Plus_21_21()
		{
			StreamSize act = +((StreamSize)21);
			StreamSize exp = 21;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Negate_21_Minus21()
		{
			StreamSize act = -((StreamSize)21);
			StreamSize exp = -21;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Addition_17Percentage10_18()
		{
			StreamSize act = 17;
			StreamSize exp = 18;
			act = act + Percentage.Create(0.1);

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Addition_17And5_24()
		{
			StreamSize act = 17;
			StreamSize exp = 24;
			act = act + (StreamSize)7;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Subtraction_17Percentage10_16()
		{
			StreamSize act = 17;
			StreamSize exp = 16;
			act = act - Percentage.Create(0.1);

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Subtraction_17And5_12()
		{
			StreamSize act = 17;
			StreamSize exp = 12;
			act = act - (StreamSize)5;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Division_81And2Int16_40()
		{
			StreamSize act = 81;
			StreamSize exp = 40;
			act = act / (Int16)2;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Division_81And2Int32_40()
		{
			StreamSize act = 81;
			StreamSize exp = 40;
			act = act / (Int32)2;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Division_81And2Int64_40()
		{
			StreamSize act = 81;
			StreamSize exp = 40;
			act = act / (Int64)2;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Division_81And2UInt16_40()
		{
			StreamSize act = 81;
			StreamSize exp = 40;
			act = act / (UInt16)2;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Division_81And2UInt32_40()
		{
			StreamSize act = 81;
			StreamSize exp = 40;
			act = act / (UInt32)2;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Division_81And2UInt64_40()
		{
			StreamSize act = 81;
			StreamSize exp = 40;
			act = act / (UInt64)2;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Division_81And150Percentage_54()
		{
			StreamSize act = 81;
			StreamSize exp = 54;
			act = act / (Percentage)1.50;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Division_81And1Point5Single_54()
		{
			StreamSize act = 81;
			StreamSize exp = 54;
			act = act / (Single)1.5;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Division_81And1Point5Double_54()
		{
			StreamSize act = 81;
			StreamSize exp = 54;
			act = act / (Double)1.5;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Division_81And1Point5Decimal_54()
		{
			StreamSize act = 81;
			StreamSize exp = 54;
			act = act / (Decimal)1.5;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Multiply_42And3Int16_126()
		{
			StreamSize act = 42;
			StreamSize exp = 126;
			act = act * (Int16)3;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Multiply_42And3Int32_126()
		{
			StreamSize act = 42;
			StreamSize exp = 126;
			act = act * (Int32)3;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Multiply_42And3Int64_126()
		{
			StreamSize act = 42;
			StreamSize exp = 126;
			act = act * (Int64)3;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Multiply_42And3UInt16_126()
		{
			StreamSize act = 42;
			StreamSize exp = 126;
			act = act * (UInt16)3;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Multiply_42And3UInt32_126()
		{
			StreamSize act = 42;
			StreamSize exp = 126;
			act = act * (UInt32)3;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Multiply_42And3UInt64_126()
		{
			StreamSize act = 42;
			StreamSize exp = 126;
			act = act * (UInt64)3;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Multiply_42And50Percentage_21()
		{
			StreamSize act = 42;
			StreamSize exp = 21;
			act = act * (Percentage)0.5;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Multiply_42AndHalfSingle_21()
		{
			StreamSize act = 42;
			StreamSize exp = 21;
			act = act * (Single)0.5;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Multiply_42AndHalfDouble_21()
		{
			StreamSize act = 42;
			StreamSize exp = 21;
			act = act * (Double)0.5;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Multiply_42AndHalfDecimal_21()
		{
			StreamSize act = 42;
			StreamSize exp = 21;
			act = act * (Decimal)0.5;

			Assert.AreEqual(exp, act);
		}

		#endregion

		#region Type converter tests

		[Test]
		public void ConverterExists_StreamSize_IsTrue()
		{
			TypeConverterAssert.ConverterExists(typeof(StreamSize));
		}

		[Test]
		public void CanNotConvertFromInt32_StreamSize_IsTrue()
		{
			TypeConverterAssert.CanNotConvertFrom(typeof(StreamSize), typeof(Int32));
		}
		[Test]
		public void CanNotConvertToInt32_StreamSize_IsTrue()
		{
			TypeConverterAssert.CanNotConvertTo(typeof(StreamSize), typeof(Int32));
		}

		[Test]
		public void CanConvertFromString_StreamSize_IsTrue()
		{
			TypeConverterAssert.CanConvertFromString(typeof(StreamSize));
		}

		[Test]
		public void CanConvertToString_StreamSize_IsTrue()
		{
			TypeConverterAssert.CanConvertToString(typeof(StreamSize));
		}

		[Test]
		public void ConvertFromString_StringValue_TestStruct()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(StreamSizeTest.TestStruct, StreamSizeTest.TestStruct.ToString());
			}
		}

		[Test]
		public void ConvertToString_TestStruct_StringValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertToStringEquals(StreamSizeTest.TestStruct.ToString(), StreamSizeTest.TestStruct);
			}
		}

		[Test]
		public void ConvertFromInstanceDescriptor_StreamSize_Successful()
		{
			TypeConverterAssert.ConvertFromInstanceDescriptor(typeof(StreamSize));
		}

		#endregion

		#region IsValid tests

		[Test]
		public void IsValid_Data_IsFalse()
		{
			using (CultureInfoScope.NewInvariant())
			{
				Assert.IsFalse(StreamSize.IsValid("Complex"), "Complex");
				Assert.IsFalse(StreamSize.IsValid((String)null), "(String)null");
				Assert.IsFalse(StreamSize.IsValid(String.Empty), "String.Empty");

				Assert.IsFalse(StreamSize.IsValid("1234 EB"), "1234 EB, to big");
				Assert.IsFalse(StreamSize.IsValid("-1234EB"), "-1234EB, to small");

				Assert.IsFalse(StreamSize.IsValid("12.9 EB"), "12.9 EB, to big");
				Assert.IsFalse(StreamSize.IsValid("-12.9EB"), "-12.9EB, to small");

				Assert.IsFalse(StreamSize.IsValid("79,228,162,514,264,337,593,543,950,335 kB"), "12.9 EB, to big for decimal");
				Assert.IsFalse(StreamSize.IsValid("-9,228,162,514,264,337,593,543,950,335 kB"), "-12.9EB, to small for decimal");
			}
		}
		[Test]
		public void IsValid_Data_IsTrue()
		{
			using (CultureInfoScope.NewInvariant())
			{
				Assert.IsTrue(StreamSize.IsValid("19 MB"));
				Assert.IsTrue(StreamSize.IsValid("1,456.134 MB"));
			}
		}
		#endregion

		#region Extension tests

		[Test]
		public void GetStreamSize_NullStream_ThrowsArgumentNullException()
		{
			ExceptionAssert.CatchArgumentNullException(() =>
			{
				Stream stream = null;
				stream.GetStreamSize();
			}
			, "stream");
		}
		[Test]
		public void GetStreamSize_Stream_17Byte()
		{
			using (var stream = new MemoryStream(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 }))
			{
				StreamSize act = stream.GetStreamSize();
				StreamSize exp = 17;

				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void GetStreamSize_FileInfo_9Byte()
		{
			var file = new FileInfo("GetStreamSize_FileInfo_9.test");
			using (var writer = new StreamWriter(file.FullName, false))
			{
				writer.Write("Unit Test");
			}

			StreamSize act = file.GetStreamSize();
			StreamSize exp = 9;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void GetStreamSize_NullFileInfo_ThrowsArgumentNullException()
		{
			ExceptionAssert.CatchArgumentNullException(() =>
			{
				FileInfo fileInfo = null;
				fileInfo.GetStreamSize();
			}
			, "fileInfo");
		}

		[Test]
		public void Average_ArrayOfStreamSizes_5Byte()
		{
			var arr = new StreamSize[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

			StreamSize act = arr.Average();
			StreamSize exp = 5;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Sum_ArrayOfStreamSizes_45Byte()
		{
			var arr = new StreamSize[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

			StreamSize act = arr.Sum();
			StreamSize exp = 45;

			Assert.AreEqual(exp, act);
		}

		#endregion
	}

	[Serializable]
	public class StreamSizeSerializeObject
	{
		public int Id { get; set; }
		public StreamSize Obj { get; set; }
		public DateTime Date { get; set; }
	}
}
