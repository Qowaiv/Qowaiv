using NUnit.Framework;
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

namespace Qowaiv.UnitTests
{
	/// <summary>Tests the file size SVO.</summary>
	[TestFixture]
	public class FileSizeTest
	{
		/// <summary>The test instance for most tests.</summary>
		public static readonly FileSize TestStruct = 123456789;

		#region file size const tests

		/// <summary>FileSize.Empty should be equal to the default of file size.</summary>
		[Test]
		public void Empty_None_EqualsDefault()
		{
			Assert.AreEqual(default(FileSize), FileSize.Zero);
		}

		#endregion

		#region TryParse tests

		/// <summary>TryParse null should be valid.</summary>
		[Test]
		public void TyrParse_Null_IsInvalid()
		{
			FileSize val;
			string str = null;

			Assert.IsFalse(FileSize.TryParse(str, out val));
		}

		/// <summary>TryParse string.Empty should be valid.</summary>
		[Test]
		public void TyrParse_StringEmpty_IsInvalid()
		{
			FileSize val;
			string str = string.Empty;

			Assert.IsFalse(FileSize.TryParse(str, out val));
		}

		/// <summary>TryParse with specified string value should be valid.</summary>
		[Test]
		public void TyrParse_StringValue_IsValid()
		{
			FileSize val;

			string str = "17kb";

			Assert.IsTrue(FileSize.TryParse(str, out val), "Valid");
			Assert.AreEqual("17408 byte", val.ToString(), "Value");
		}

		[Test]
		public void Parse_InvalidInput_ThrowsFormatException()
		{
			using (new CultureInfoScope("en-GB"))
			{
				ExceptionAssert.ExpectException<FormatException>
				(() =>
				{
					FileSize.Parse("InvalidInput");
				},
				"Not a valid file size");
			}
		}

		[Test]
		public void TryParse_TestStructInput_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = TestStruct;
				var act = FileSize.TryParse(exp.ToString());

				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void TryParse_InvalidInput_DefaultValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = default(FileSize);
				var act = FileSize.TryParse("InvalidInput");

				Assert.AreEqual(exp, act);
			}
		}
		#endregion

		#region (XML) (De)serialization tests

		[Test]
		public void Constructor_SerializationInfoIsNull_ThrowsArgumentNullException()
		{
			ExceptionAssert.ExpectArgumentNullException
			(() =>
			{
				SerializationTest.DeserializeUsingConstructor<FileSize>(null, default(StreamingContext));
			},
			"info");
		}

		[Test]
		public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
		{
			ExceptionAssert.ExpectException<SerializationException>
			(() =>
			{
				var info = new SerializationInfo(typeof(FileSize), new System.Runtime.Serialization.FormatterConverter());
				SerializationTest.DeserializeUsingConstructor<FileSize>(info, default(StreamingContext));
			});
		}

		[Test]
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

		[Test]
		public void GetObjectData_SerializationInfo_AreEqual()
		{
			ISerializable obj = TestStruct;
			var info = new SerializationInfo(typeof(FileSize), new System.Runtime.Serialization.FormatterConverter());
			obj.GetObjectData(info, default(StreamingContext));

			Assert.AreEqual((Int64)123456789, info.GetInt64("Value"));
		}

		[Test]
		public void SerializeDeserialize_TestStruct_AreEqual()
		{
			var input = FileSizeTest.TestStruct;
			var exp = FileSizeTest.TestStruct;
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void DataContractSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = FileSizeTest.TestStruct;
			var exp = FileSizeTest.TestStruct;
			var act = SerializationTest.DataContractSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void XmlSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = FileSizeTest.TestStruct;
			var exp = FileSizeTest.TestStruct;
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void SerializeDeserialize_FileSizeSerializeObject_AreEqual()
		{
			var input = new FileSizeSerializeObject()
			{
				Id = 17,
				Obj = FileSizeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new FileSizeSerializeObject()
			{
				Id = 17,
				Obj = FileSizeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			Assert.AreEqual(exp.Date, act.Date, "Date");
		}
		[Test]
		public void XmlSerializeDeserialize_FileSizeSerializeObject_AreEqual()
		{
			var input = new FileSizeSerializeObject()
			{
				Id = 17,
				Obj = FileSizeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new FileSizeSerializeObject()
			{
				Id = 17,
				Obj = FileSizeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			Assert.AreEqual(exp.Date, act.Date, "Date");
		}
		[Test]
		public void DataContractSerializeDeserialize_FileSizeSerializeObject_AreEqual()
		{
			var input = new FileSizeSerializeObject()
			{
				Id = 17,
				Obj = FileSizeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new FileSizeSerializeObject()
			{
				Id = 17,
				Obj = FileSizeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.DataContractSerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			Assert.AreEqual(exp.Date, act.Date, "Date");
		}

		[Test]
		public void SerializeDeserialize_Empty_AreEqual()
		{
			var input = new FileSizeSerializeObject()
			{
				Id = 17,
				Obj = FileSizeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new FileSizeSerializeObject()
			{
				Id = 17,
				Obj = FileSizeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			Assert.AreEqual(exp.Date, act.Date, "Date");
		}
		[Test]
		public void XmlSerializeDeserialize_Empty_AreEqual()
		{
			var input = new FileSizeSerializeObject()
			{
				Id = 17,
				Obj = FileSize.Zero,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new FileSizeSerializeObject()
			{
				Id = 17,
				Obj = FileSize.Zero,
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
		public void FromJson_Null_AreEqual()
		{
			var act = JsonTester.Read<FileSize>();
			var exp = FileSize.Zero;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_InvalidStringValue_AssertFormatException()
		{
			ExceptionAssert.ExpectException<FormatException>(() =>
			{
				JsonTester.Read<FileSize>("InvalidStringValue");
			},
			"Not a valid file size");
		}
		[Test]
		public void FromJson_StringValue_AreEqual()
		{
			var act = JsonTester.Read<FileSize>(TestStruct.ToString(CultureInfo.InvariantCulture));
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_Int64Value_AreEqual()
		{
			var act = JsonTester.Read<FileSize>((Int64)TestStruct);
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_DoubleValue_AreEqual()
		{
			var act = JsonTester.Read<FileSize>((Double)TestStruct);
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_DateTimeValue_AssertNotSupportedException()
		{
			ExceptionAssert.ExpectException<NotSupportedException>(() =>
			{
				JsonTester.Read<FileSize>(new DateTime(1972, 02, 14));
			},
			"JSON deserialization from a date is not supported.");
		}

		[Test]
		public void ToJson_DefaultValue_AreEqual()
		{
			object act = JsonTester.Write(default(FileSize));
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
			var act = FileSize.Zero.ToString();
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
			using (new CultureInfoScope("nl-BE"))
			{
				var act = TestStruct.ToString("#,##0b");
				var exp = "123.456.789b";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_kB_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = TestStruct.ToString("#,##0.00 kB");
				var exp = "120.563,27 kB";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_MegaByte_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = TestStruct.ToString("0.0 MegaByte");
				var exp = "117,7 MegaByte";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_GB_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = TestStruct.ToString("0.00GB");
				var exp = "0,11GB";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_tb_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = FileSize.PB.ToString("tb");
				var exp = "1024tb";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_pb_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = FileSize.TB.ToString(" petabyte");
				var exp = "0,0009765625 petabyte";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_Exabyte_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = FileSize.MaxValue.ToString("#,##0.## Exabyte");
				var exp = "8 Exabyte";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_spaceF_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = TestStruct.ToString("#,##0.## F");
				var exp = "117,74 Megabyte";
				Assert.AreEqual(exp, act);
			}
		}
		[Test]
		public void ToString_spaceFLower_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = TestStruct.ToString("0 f");
				var exp = "118 megabyte";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_spaceS_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = TestStruct.ToString("0000 S");
				var exp = "0118 MB";
				Assert.AreEqual(exp, act);
			}
		}
		[Test]
		public void ToString_spaceSLower_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = TestStruct.ToString("0 s");
				var exp = "118 mb";
				Assert.AreEqual(exp, act);
			}
		}
		[Test]
		public void ToString_SLower_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = TestStruct.ToString("0s");
				var exp = "118mb";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_ValueDutchBelgium_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = FileSize.Parse("1600,1").ToString();
				var exp = "1600 byte";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_ValueEnglishGreatBritain_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var act = FileSize.Parse("1600.1").ToString();
				var exp = "1600 byte";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_FormatValueDutchBelgium_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = FileSize.Parse("800").ToString("0000 byte");
				var exp = "0800 byte";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_FormatValueEnglishGreatBritain_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var act = FileSize.Parse("800").ToString("0000");
				var exp = "0800";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_FormatValueSpanishEcuador_AreEqual()
		{
			var act = FileSize.Parse("1700").ToString("00000.0", new CultureInfo("es-EC"));
			var exp = "01700,0";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void DebuggerDisplay_DebugToString_HasAttribute()
		{
			DebuggerDisplayAssert.HasAttribute(typeof(FileSize));
		}

		[Test]
		public void DebugToString_DefaultValue_String()
		{
			DebuggerDisplayAssert.HasResult("0 byte", default(FileSize));
		}

		[Test]
		public void DebugToString_TestStruct_String()
		{
			DebuggerDisplayAssert.HasResult("117.7 Megabyte", TestStruct);
		}

		#endregion

		#region IEquatable tests

		/// <summary>GetHash should not fail for FileSize.Zero.</summary>
		[Test]
		public void GetHash_Empty_0()
		{
			Assert.AreEqual(0, FileSize.Zero.GetHashCode());
		}

		/// <summary>GetHash should not fail for the test struct.</summary>
		[Test]
		public void GetHash_TestStruct_123456789()
		{
			Assert.AreEqual(123456789, FileSizeTest.TestStruct.GetHashCode());
		}

		[Test]
		public void Equals_EmptyEmpty_IsTrue()
		{
			Assert.IsTrue(FileSize.Zero.Equals(FileSize.Zero));
		}

		[Test]
		public void Equals_FormattedAndUnformatted_IsTrue()
		{
			var l = FileSize.Parse("12,345 byte", CultureInfo.InvariantCulture);
			var r = FileSize.Parse("12345", CultureInfo.InvariantCulture);

			Assert.IsTrue(l.Equals(r));
		}

		[Test]
		public void Equals_TestStructTestStruct_IsTrue()
		{
			Assert.IsTrue(FileSizeTest.TestStruct.Equals(FileSizeTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructEmpty_IsFalse()
		{
			Assert.IsFalse(FileSizeTest.TestStruct.Equals(FileSize.Zero));
		}

		[Test]
		public void Equals_EmptyTestStruct_IsFalse()
		{
			Assert.IsFalse(FileSize.Zero.Equals(FileSizeTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructObjectTestStruct_IsTrue()
		{
			Assert.IsTrue(FileSizeTest.TestStruct.Equals((object)FileSizeTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructNull_IsFalse()
		{
			Assert.IsFalse(FileSizeTest.TestStruct.Equals(null));
		}

		[Test]
		public void Equals_TestStructObject_IsFalse()
		{
			Assert.IsFalse(FileSizeTest.TestStruct.Equals(new object()));
		}

		[Test]
		public void OperatorIs_TestStructTestStruct_IsTrue()
		{
			var l = FileSizeTest.TestStruct;
			var r = FileSizeTest.TestStruct;
			Assert.IsTrue(l == r);
		}

		[Test]
		public void OperatorIsNot_TestStructTestStruct_IsFalse()
		{
			var l = FileSizeTest.TestStruct;
			var r = FileSizeTest.TestStruct;
			Assert.IsFalse(l != r);
		}

		#endregion

		#region IComparable tests

		/// <summary>Orders a list of file sizes ascending.</summary>
		[Test]
		public void OrderBy_FileSize_AreEqual()
		{
			FileSize item0 = 13465;
			FileSize item1 = 83465;
			FileSize item2 = 113465;
			FileSize item3 = 773465;

			var inp = new List<FileSize>() { FileSize.Zero, item3, item2, item0, item1, FileSize.Zero };
			var exp = new List<FileSize>() { FileSize.Zero, FileSize.Zero, item0, item1, item2, item3 };
			var act = inp.OrderBy(item => item).ToList();

			CollectionAssert.AreEqual(exp, act);
		}

		/// <summary>Orders a list of file sizes descending.</summary>
		[Test]
		public void OrderByDescending_FileSize_AreEqual()
		{
			FileSize item0 = 13465;
			FileSize item1 = 83465;
			FileSize item2 = 113465;
			FileSize item3 = 773465;

			var inp = new List<FileSize>() { FileSize.Zero, item3, item2, item0, item1, FileSize.Zero };
			var exp = new List<FileSize>() { item3, item2, item1, item0, FileSize.Zero, FileSize.Zero };
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
			ExceptionAssert.ExpectArgumentException
			(() =>
				{
					object other = null;
					var act = TestStruct.CompareTo(other);
				},
				"obj",
				"Argument must be a file size"
			);
		}
		/// <summary>Compare with a random object should throw an expception.</summary>
		[Test]
		public void CompareTo_newObject_ThrowsArgumentException()
		{
			ExceptionAssert.ExpectArgumentException
			(() =>
				{
					object other = new object();
					var act = TestStruct.CompareTo(other);
				},
				"obj",
				"Argument must be a file size"
			);
		}

		[Test]
		public void LessThan_17LT19_IsTrue()
		{
			FileSize l = 17;
			FileSize r = 19;

			Assert.IsTrue(l < r);
		}
		[Test]
		public void GreaterThan_21LT19_IsTrue()
		{
			FileSize l = 21;
			FileSize r = 19;

			Assert.IsTrue(l > r);
		}

		[Test]
		public void LessThanOrEqual_17LT19_IsTrue()
		{
			FileSize l = 17;
			FileSize r = 19;

			Assert.IsTrue(l <= r);
		}
		[Test]
		public void GreaterThanOrEqual_21LT19_IsTrue()
		{
			FileSize l = 21;
			FileSize r = 19;

			Assert.IsTrue(l >= r);
		}

		[Test]
		public void LessThanOrEqual_17LT17_IsTrue()
		{
			FileSize l = 17;
			FileSize r = 17;

			Assert.IsTrue(l <= r);
		}
		[Test]
		public void GreaterThanOrEqual_21LT21_IsTrue()
		{
			FileSize l = 21;
			FileSize r = 21;

			Assert.IsTrue(l >= r);
		}
		#endregion

		#region Casting tests

		[Test]
		public void Explicit_StringToFileSize_AreEqual()
		{
			var exp = TestStruct;
			var act = (FileSize)TestStruct.ToString();

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_FileSizeToString_AreEqual()
		{
			var exp = TestStruct.ToString();
			var act = (string)TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Implicit_Int32ToFileSize_AreEqual()
		{
			FileSize exp = TestStruct;
			FileSize act = 123456789;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_FileSizeToInt32_AreEqual()
		{
			var exp = 123456789;
			var act = (Int32)TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Implicit_Int64ToFileSize_AreEqual()
		{
			var exp = TestStruct;
			FileSize act = 123456789L;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_FileSizeToInt64_AreEqual()
		{
			var exp = 123456789L;
			var act = (Int64)TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Explicit_DoubleToFileSize_AreEqual()
		{
			var exp = TestStruct;
			var act = (FileSize)123456789d;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_FileSizeToDouble_AreEqual()
		{
			var exp = 123456789d;
			var act = (Double)TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Explicit_DecimalToFileSize_AreEqual()
		{
			var exp = TestStruct;
			var act = (FileSize)123456789m;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_FileSizeToDecimal_AreEqual()
		{
			var exp = 123456789m;
			var act = (Decimal)TestStruct;

			Assert.AreEqual(exp, act);
		}


		#endregion

		#region Properties
		#endregion

		#region File size manipulation tests

		[Test]
		public void Increment_21_22()
		{
			FileSize act = 21;
			FileSize exp = 22;
			act++;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Decrement_21_20()
		{
			FileSize act = 21;
			FileSize exp = 20;
			act--;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Plus_21_21()
		{
			FileSize act = +((FileSize)21);
			FileSize exp = 21;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Negate_21_Minus21()
		{
			FileSize act = -((FileSize)21);
			FileSize exp = -21;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Addition_17Percentage10_18()
		{
			FileSize act = 17;
			FileSize exp = 18;
			act = act + Percentage.Create(0.1);

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Addition_17And5_24()
		{
			FileSize act = 17;
			FileSize exp = 24;
			act = act + (FileSize)7;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Subtraction_17Percentage10_16()
		{
			FileSize act = 17;
			FileSize exp = 16;
			act = act - Percentage.Create(0.1);

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Subtraction_17And5_12()
		{
			FileSize act = 17;
			FileSize exp = 12;
			act = act - (FileSize)5;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Division_81And2Int16_40()
		{
			FileSize act = 81;
			FileSize exp = 40;
			act = act / (Int16)2;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Division_81And2Int32_40()
		{
			FileSize act = 81;
			FileSize exp = 40;
			act = act / (Int32)2;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Division_81And2Int64_40()
		{
			FileSize act = 81;
			FileSize exp = 40;
			act = act / (Int64)2;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Division_81And2UInt16_40()
		{
			FileSize act = 81;
			FileSize exp = 40;
			act = act / (UInt16)2;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Division_81And2UInt32_40()
		{
			FileSize act = 81;
			FileSize exp = 40;
			act = act / (UInt32)2;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Division_81And2UInt64_40()
		{
			FileSize act = 81;
			FileSize exp = 40;
			act = act / (UInt64)2;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Division_81And150Percentage_54()
		{
			FileSize act = 81;
			FileSize exp = 54;
			act = act / (Percentage)1.50;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Division_81And1Point5Single_54()
		{
			FileSize act = 81;
			FileSize exp = 54;
			act = act / (Single)1.5;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Division_81And1Point5Double_54()
		{
			FileSize act = 81;
			FileSize exp = 54;
			act = act / (Double)1.5;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Division_81And1Point5Decimal_54()
		{
			FileSize act = 81;
			FileSize exp = 54;
			act = act / (Decimal)1.5;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Multiply_42And3Int16_126()
		{
			FileSize act = 42;
			FileSize exp = 126;
			act = act * (Int16)3;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Multiply_42And3Int32_126()
		{
			FileSize act = 42;
			FileSize exp = 126;
			act = act * (Int32)3;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Multiply_42And3Int64_126()
		{
			FileSize act = 42;
			FileSize exp = 126;
			act = act * (Int64)3;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Multiply_42And3UInt16_126()
		{
			FileSize act = 42;
			FileSize exp = 126;
			act = act * (UInt16)3;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Multiply_42And3UInt32_126()
		{
			FileSize act = 42;
			FileSize exp = 126;
			act = act * (UInt32)3;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Multiply_42And3UInt64_126()
		{
			FileSize act = 42;
			FileSize exp = 126;
			act = act * (UInt64)3;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Multiply_42And50Percentage_21()
		{
			FileSize act = 42;
			FileSize exp = 21;
			act = act * (Percentage)0.5;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Multiply_42AndHalfSingle_21()
		{
			FileSize act = 42;
			FileSize exp = 21;
			act = act * (Single)0.5;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Multiply_42AndHalfDouble_21()
		{
			FileSize act = 42;
			FileSize exp = 21;
			act = act * (Double)0.5;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Multiply_42AndHalfDecimal_21()
		{
			FileSize act = 42;
			FileSize exp = 21;
			act = act * (Decimal)0.5;

			Assert.AreEqual(exp, act);
		}

		#endregion

		#region Type converter tests

		[Test]
		public void ConverterExists_FileSize_IsTrue()
		{
			TypeConverterAssert.ConverterExists(typeof(FileSize));
		}

		[Test]
		public void CanNotConvertFromInt32_FileSize_IsTrue()
		{
			TypeConverterAssert.CanNotConvertFrom(typeof(FileSize), typeof(Int32));
		}
		[Test]
		public void CanNotConvertToInt32_FileSize_IsTrue()
		{
			TypeConverterAssert.CanNotConvertTo(typeof(FileSize), typeof(Int32));
		}

		[Test]
		public void CanConvertFromString_FileSize_IsTrue()
		{
			TypeConverterAssert.CanConvertFromString(typeof(FileSize));
		}

		[Test]
		public void CanConvertToString_FileSize_IsTrue()
		{
			TypeConverterAssert.CanConvertToString(typeof(FileSize));
		}

		[Test]
		public void ConvertFromString_StringValue_TestStruct()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(FileSizeTest.TestStruct, FileSizeTest.TestStruct.ToString());
			}
		}

		[Test]
		public void ConvertToString_TestStruct_StringValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertToStringEquals(FileSizeTest.TestStruct.ToString(), FileSizeTest.TestStruct);
			}
		}

		#endregion

		#region IsValid tests

		[Test]
		public void IsValid_Data_IsFalse()
		{
			using (CultureInfoScope.NewInvariant())
			{
				Assert.IsFalse(FileSize.IsValid("Complex"), "Complex");
				Assert.IsFalse(FileSize.IsValid((String)null), "(String)null");
				Assert.IsFalse(FileSize.IsValid(String.Empty), "String.Empty");

				Assert.IsFalse(FileSize.IsValid("1234 EB"), "1234 EB, to big");
				Assert.IsFalse(FileSize.IsValid("-1234EB"), "-1234EB, to small");

				Assert.IsFalse(FileSize.IsValid("12.9 EB"), "12.9 EB, to big");
				Assert.IsFalse(FileSize.IsValid("-12.9EB"), "-12.9EB, to small");

				Assert.IsFalse(FileSize.IsValid("79,228,162,514,264,337,593,543,950,335 kB"), "12.9 EB, to big for decimal");
				Assert.IsFalse(FileSize.IsValid("-9,228,162,514,264,337,593,543,950,335 kB"), "-12.9EB, to small for decimal");
			}
		}
		[Test]
		public void IsValid_Data_IsTrue()
		{
			using (CultureInfoScope.NewInvariant())
			{
				Assert.IsTrue(FileSize.IsValid("19 MB"));
				Assert.IsTrue(FileSize.IsValid("1,456.134 MB"));
			}
		}
		#endregion

		#region Extension tests

		[Test]
		public void GetFileSize_NullStream_ThrowsArgumentNullException()
		{
			ExceptionAssert.ExpectArgumentNullException(() =>
			{
				Stream stream = null;
				stream.GetFileSize();
			}
			, "stream");
		}
		[Test]
		public void GetFileSize_Stream_17Byte()
		{
			using (var stream = new MemoryStream(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 }))
			{
				FileSize act = stream.GetFileSize();
				FileSize exp = 17;

				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void GetFileSize_FileInfo_9Byte()
		{
			var file = new FileInfo("GetFileSize_FileInfo_9.test");
			using (var writer = new StreamWriter(file.FullName, false))
			{
				writer.Write("Unit Test");
			}

			FileSize act = file.GetFileSize();
			FileSize exp = 9;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void GetFileSize_NullFileInfo_ThrowsArgumentNullException()
		{
			ExceptionAssert.ExpectArgumentNullException(() =>
			{
				FileInfo fileInfo = null;
				fileInfo.GetFileSize();
			}
			, "fileInfo");
		}

		[Test]
		public void Average_ArrayOfFileSizes_5Byte()
		{
			var arr = new FileSize[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

			FileSize act = arr.Average();
			FileSize exp = 5;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Sum_ArrayOfFileSizes_45Byte()
		{
			var arr = new FileSize[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

			FileSize act = arr.Sum();
			FileSize exp = 45;

			Assert.AreEqual(exp, act);
		}

		#endregion
	}

	[Serializable]
	public class FileSizeSerializeObject
	{
		public int Id { get; set; }
		public FileSize Obj { get; set; }
		public DateTime Date { get; set; }
	}
}
