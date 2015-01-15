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
	/// <summary>Tests the GUID SVO.</summary>
	[TestFixture]
	public class QGuidTest
	{
		/// <summary>The test instance for most tests.</summary>
		public static readonly QGuid TestStruct = QGuid.Parse("Qowaiv_SVOLibrary_GUIA");
		public static readonly Guid TestGuid = Guid.Parse("8a1a8c42-d2ff-e254-e26e-b6abcbf19420");

		#region GUID const tests

		/// <summary>QGuid.Empty should be equal to the default of GUID.</summary>
		[Test]
		public void Empty_None_EqualsDefault()
		{
			Assert.AreEqual(default(QGuid), QGuid.Empty);
		}

		#endregion

		#region GUID IsEmpty tests

		/// <summary>QGuid.IsEmpty() should be true for the default of GUID.</summary>
		[Test]
		public void IsEmpty_Default_IsTrue()
		{
			Assert.IsTrue(default(QGuid).IsEmpty());
		}
		/// <summary>QGuid.IsEmpty() should be false for the TestStruct.</summary>
		[Test]
		public void IsEmpty_TestStruct_IsFalse()
		{
			Assert.IsFalse(TestStruct.IsEmpty());
		}

		#endregion

		#region TryParse tests

		/// <summary>TryParse null should be valid.</summary>
		[Test]
		public void TyrParse_Null_IsValid()
		{
			QGuid val;

			string str = null;

			Assert.IsTrue(QGuid.TryParse(str, out val), "Valid");
			Assert.AreEqual("AAAAAAAAAAAAAAAAAAAAAA", val.ToString(), "Value");
		}

		/// <summary>TryParse string.Empty should be valid.</summary>
		[Test]
		public void TyrParse_StringEmpty_IsValid()
		{
			QGuid val;

			string str = string.Empty;

			Assert.IsTrue(QGuid.TryParse(str, out val), "Valid");
			Assert.AreEqual("AAAAAAAAAAAAAAAAAAAAAA", val.ToString(), "Value");
		}

		/// <summary>TryParse with specified string value should be valid.</summary>
		[Test]
		public void TyrParse_StringValue_IsValid()
		{
			QGuid val;

			string str = "8a1a8c42-d2ff-e254-e26e-b6abcbf19420";

			Assert.IsTrue(QGuid.TryParse(str, out val), "Valid");
			Assert.AreEqual(str, val.ToString("d"), "Value");
		}

		/// <summary>TryParse with specified string value should be invalid.</summary>
		[Test]
		public void TyrParse_StringValue_IsNotValid()
		{
			QGuid val;

			string str = "string";

			Assert.IsFalse(QGuid.TryParse(str, out val), "Valid");
			Assert.AreEqual("AAAAAAAAAAAAAAAAAAAAAA", val.ToString(), "Value");
		}

		[Test]
		public void Parse_InvalidInput_ThrowsFormatException()
		{
			using (new CultureInfoScope("en-GB"))
			{
				ExceptionAssert.ExpectException<FormatException>
				(() =>
				{
					QGuid.Parse("InvalidInput");
				},
				"Not a valid GUID");
			}
		}

		[Test]
		public void TryParse_TestStructInput_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = TestStruct;
				var act = QGuid.TryParse(exp.ToString());

				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void TryParse_InvalidInput_DefaultValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = default(QGuid);
				var act = QGuid.TryParse("InvalidInput");

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
				SerializationTest.DeserializeUsingConstructor<QGuid>(null, default(StreamingContext));
			},
			"info");
		}

		[Test]
		public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
		{
			ExceptionAssert.ExpectException<SerializationException>
			(() =>
			{
				var info = new SerializationInfo(typeof(QGuid), new System.Runtime.Serialization.FormatterConverter());
				SerializationTest.DeserializeUsingConstructor<QGuid>(info, default(StreamingContext));
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
			var info = new SerializationInfo(typeof(QGuid), new System.Runtime.Serialization.FormatterConverter());
			obj.GetObjectData(info, default(StreamingContext));

			Assert.AreEqual(TestGuid, info.GetValue("Value", typeof(Guid)));
		}

		[Test]
		public void SerializeDeserialize_TestStruct_AreEqual()
		{
			var input = QGuidTest.TestStruct;
			var exp = QGuidTest.TestStruct;
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void DataContractSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = QGuidTest.TestStruct;
			var exp = QGuidTest.TestStruct;
			var act = SerializationTest.DataContractSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void XmlSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = QGuidTest.TestStruct;
			var exp = QGuidTest.TestStruct;
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void SerializeDeserialize_QGuidSerializeObject_AreEqual()
		{
			var input = new QGuidSerializeObject()
			{
				Id = 17,
				Obj = QGuidTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new QGuidSerializeObject()
			{
				Id = 17,
				Obj = QGuidTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			Assert.AreEqual(exp.Date, act.Date, "Date");
		}
		[Test]
		public void XmlSerializeDeserialize_QGuidSerializeObject_AreEqual()
		{
			var input = new QGuidSerializeObject()
			{
				Id = 17,
				Obj = QGuidTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new QGuidSerializeObject()
			{
				Id = 17,
				Obj = QGuidTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			Assert.AreEqual(exp.Date, act.Date, "Date");
		}
		[Test]
		public void DataContractSerializeDeserialize_QGuidSerializeObject_AreEqual()
		{
			var input = new QGuidSerializeObject()
			{
				Id = 17,
				Obj = QGuidTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new QGuidSerializeObject()
			{
				Id = 17,
				Obj = QGuidTest.TestStruct,
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
			var input = new QGuidSerializeObject()
			{
				Id = 17,
				Obj = QGuidTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new QGuidSerializeObject()
			{
				Id = 17,
				Obj = QGuidTest.TestStruct,
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
			var input = new QGuidSerializeObject()
			{
				Id = 17,
				Obj = QGuid.Empty,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new QGuidSerializeObject()
			{
				Id = 17,
				Obj = QGuid.Empty,
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
			var act = JsonTester.Read<QGuid>();
			var exp = QGuid.Empty;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_InvalidStringValue_AssertFormatException()
		{
			ExceptionAssert.ExpectException<FormatException>(() =>
			{
				JsonTester.Read<QGuid>("InvalidStringValue");
			},
			"Not a valid GUID");
		}
		[Test]
		public void FromJson_StringValue_AreEqual()
		{
			var act = JsonTester.Read<QGuid>(TestStruct.ToString(CultureInfo.InvariantCulture));
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_Int64Value_AssertNotSupportedException()
		{
			ExceptionAssert.ExpectException<NotSupportedException>(() =>
			{
				JsonTester.Read<QGuid>(123456L);
			},
			"JSON deserialization from an integer is not supported.");
		}

		[Test]
		public void FromJson_DoubleValue_AssertNotSupportedException()
		{
			ExceptionAssert.ExpectException<NotSupportedException>(() =>
			{
				JsonTester.Read<QGuid>(1234.56);
			},
			"JSON deserialization from a number is not supported.");
		}

		[Test]
		public void FromJson_DateTimeValue_AssertNotSupportedException()
		{
			ExceptionAssert.ExpectException<NotSupportedException>(() =>
			{
				JsonTester.Read<QGuid>(new DateTime(1972, 02, 14));
			},
			"JSON deserialization from a date is not supported.");
		}

		[Test]
		public void ToJson_DefaultValue_AreEqual()
		{
			object act = JsonTester.Write(default(QGuid));
			object exp = null;

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

		#region IFormattable / ToString tests

		[Test]
		public void ToString_Empty_StringEmpty()
		{
			var act = QGuid.Empty.ToString();
			var exp = "AAAAAAAAAAAAAAAAAAAAAA";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_CustomFormatter_SupportsCustomFormatting()
		{
			var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
			var exp = "Unit Test Formatter, value: 'Qowaiv_SVOLibrary_GUIA', format: 'Unit Test Format'";

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void ToString_Formats_FormattedGuid()
		{
			var formats = new string[] { null, "", "s", "S", "b", "B", "d", "D", "p", "P", "x", "X" };

			var expected = new string[] 
			{
				"Qowaiv_SVOLibrary_GUIA",				"Qowaiv_SVOLibrary_GUIA",				"Qowaiv_SVOLibrary_GUIA",				"Qowaiv_SVOLibrary_GUIA",				"{8a1a8c42-d2ff-e254-e26e-b6abcbf19420}",				"{8A1A8C42-D2FF-E254-E26E-B6ABCBF19420}",				"8a1a8c42-d2ff-e254-e26e-b6abcbf19420",				"8A1A8C42-D2FF-E254-E26E-B6ABCBF19420",				"(8a1a8c42-d2ff-e254-e26e-b6abcbf19420)",				"(8A1A8C42-D2FF-E254-E26E-B6ABCBF19420)",				"{0x8a1a8c42,0xd2ff,0xe254,{0xe2,0x6e,0xb6,0xab,0xcb,0xf1,0x94,0x20}}",				"{0x8A1A8C42,0xD2FF,0xE254,{0xE2,0x6E,0xB6,0xAB,0xCB,0xF1,0x94,0x20}}"
			};

			for (var i = 0; i < expected.Length; i++)
			{
				var actual = TestStruct.ToString(formats[i]);
				Assert.AreEqual(expected[i], actual);
			}
		}

		[Test]
		public void ToString_Invalid_FormattedGuid()
		{
			ExceptionAssert.ExpectException<FormatException>(() =>
			{
				TestStruct.ToString("invalid");
			}, "Input string was not in a correct format.");
		}

		[Test]
		public void DebuggerDisplay_DebugToString_HasAttribute()
		{
			DebuggerDisplayAssert.HasAttribute(typeof(QGuid));
		}

		[Test]
		public void DebuggerDisplay_DefaultValue_String()
		{
			DebuggerDisplayAssert.HasResult("AAAAAAAAAAAAAAAAAAAAAA", default(QGuid));
		}

		[Test]
		public void DebuggerDisplay_TestStruct_String()
		{
			DebuggerDisplayAssert.HasResult("Qowaiv_SVOLibrary_GUIA", TestStruct);
		}

		#endregion

		#region IEquatable tests

		/// <summary>GetHash should not fail for QGuid.Empty.</summary>
		[Test]
		public void GetHash_Empty_Hash()
		{
			Assert.AreEqual(0, QGuid.Empty.GetHashCode());
		}

		/// <summary>GetHash should not fail for the test struct.</summary>
		[Test]
		public void GetHash_TestStruct_Hash()
		{
			Assert.AreEqual(-286953930, QGuidTest.TestStruct.GetHashCode());
		}

		[Test]
		public void Equals_EmptyEmpty_IsTrue()
		{
			Assert.IsTrue(QGuid.Empty.Equals(QGuid.Empty));
		}

		[Test]
		public void Equals_FormattedAndUnformatted_IsTrue()
		{
			var l = QGuid.Parse("{95A20FBA-347D-44C9-BEBF-65F06B73F82C}");
			var r = QGuid.Parse("95a20fba347d44c9bebf65f06b73f82c");

			Assert.IsTrue(l.Equals(r));
		}

		[Test]
		public void Equals_TestStructTestStruct_IsTrue()
		{
			Assert.IsTrue(QGuidTest.TestStruct.Equals(QGuidTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructEmpty_IsFalse()
		{
			Assert.IsFalse(QGuidTest.TestStruct.Equals(QGuid.Empty));
		}

		[Test]
		public void Equals_EmptyTestStruct_IsFalse()
		{
			Assert.IsFalse(QGuid.Empty.Equals(QGuidTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructObjectTestStruct_IsTrue()
		{
			Assert.IsTrue(QGuidTest.TestStruct.Equals((object)QGuidTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructNull_IsFalse()
		{
			Assert.IsFalse(QGuidTest.TestStruct.Equals(null));
		}

		[Test]
		public void Equals_TestStructObject_IsFalse()
		{
			Assert.IsFalse(QGuidTest.TestStruct.Equals(new object()));
		}

		[Test]
		public void OperatorIs_TestStructTestStruct_IsTrue()
		{
			var l = QGuidTest.TestStruct;
			var r = QGuidTest.TestStruct;
			Assert.IsTrue(l == r);
		}

		[Test]
		public void OperatorIsNot_TestStructTestStruct_IsFalse()
		{
			var l = QGuidTest.TestStruct;
			var r = QGuidTest.TestStruct;
			Assert.IsFalse(l != r);
		}

		#endregion

		#region IComparable tests

		/// <summary>Orders a list of GUIDs ascending.</summary>
		[Test]
		public void OrderBy_QGuid_AreEqual()
		{
			var item0 = QGuid.Parse("3BE968F7-AAEA-422C-BA74-72A4D045FD74");
			var item1 = QGuid.Parse("59ED7F38-8E6A-45A9-B3A2-6D32FDF4DD10");
			var item2 = QGuid.Parse("5BD0EF29-C625-4B8D-A063-E474B28E8653");
			var item3 = QGuid.Parse("77185219-193C-4D39-B4B1-9ED05B0FC4C8");


			var inp = new List<QGuid>() { QGuid.Empty, item3, item2, item0, item1, QGuid.Empty };
			var exp = new List<QGuid>() { QGuid.Empty, QGuid.Empty, item0, item1, item2, item3 };
			var act = inp.OrderBy(item => item).ToList();

			CollectionAssert.AreEqual(exp, act);
		}

		/// <summary>Orders a list of GUIDs descending.</summary>
		[Test]
		public void OrderByDescending_QGuid_AreEqual()
		{
			var item0 = QGuid.Parse("3BE968F7-AAEA-422C-BA74-72A4D045FD74");
			var item1 = QGuid.Parse("59ED7F38-8E6A-45A9-B3A2-6D32FDF4DD10");
			var item2 = QGuid.Parse("5BD0EF29-C625-4B8D-A063-E474B28E8653");
			var item3 = QGuid.Parse("77185219-193C-4D39-B4B1-9ED05B0FC4C8");

			var inp = new List<QGuid>() { QGuid.Empty, item3, item2, item0, item1, QGuid.Empty };
			var exp = new List<QGuid>() { item3, item2, item1, item0, QGuid.Empty, QGuid.Empty };
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
				"Argument must be a GUID"
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
				"Argument must be a GUID"
			);
		}

		#endregion

		#region Casting tests

		[Test]
		public void Explicit_StringToQGuid_AreEqual()
		{
			var exp = TestStruct;
			var act = (QGuid)TestStruct.ToString();

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_QGuidToString_AreEqual()
		{
			var exp = TestStruct.ToString();
			var act = (string)TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Implicit_GuidToQGuid_AreEqual()
		{
			QGuid exp = TestStruct;
			QGuid act = TestGuid;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_QGuidToGuid_AreEqual()
		{
			Guid exp = TestGuid;
			Guid act = TestStruct;

			Assert.AreEqual(exp, act);
		}

		#endregion

		#region Properties
		#endregion

		#region Methods

		[Test]
		public void ToByteArray_TestStruct_EqualsTestGuidToByteArray()
		{
			var act = TestStruct.ToByteArray();
			var exp = TestGuid.ToByteArray();

			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void NewGuid_None_NotEmpty()
		{
			QGuid actual = QGuid.NewGuid();
			QGuid expected = Guid.Empty;
			Assert.AreNotEqual(expected, actual);
		}

		#endregion

		#region Type converter tests

		[Test]
		public void ConverterExists_QGuid_IsTrue()
		{
			TypeConverterAssert.ConverterExists(typeof(QGuid));
		}

		[Test]
		public void CanNotConvertFromInt32_QGuid_IsTrue()
		{
			TypeConverterAssert.CanNotConvertFrom(typeof(QGuid), typeof(Int32));
		}
		[Test]
		public void CanNotConvertToInt32_QGuid_IsTrue()
		{
			TypeConverterAssert.CanNotConvertTo(typeof(QGuid), typeof(Int32));
		}

		[Test]
		public void CanConvertFromString_QGuid_IsTrue()
		{
			TypeConverterAssert.CanConvertFromString(typeof(QGuid));
		}

		[Test]
		public void CanConvertToString_QGuid_IsTrue()
		{
			TypeConverterAssert.CanConvertToString(typeof(QGuid));
		}

		[Test]
		public void ConvertFrom_StringNull_QGuidEmpty()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(QGuid.Empty, (string)null);
			}
		}

		[Test]
		public void ConvertFrom_StringEmpty_QGuidEmpty()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(QGuid.Empty, string.Empty);
			}
		}

		[Test]
		public void ConvertFromString_StringValue_TestStruct()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(QGuidTest.TestStruct, QGuidTest.TestStruct.ToString());
			}
		}

		[Test]
		public void ConvertFromInstanceDescriptor_QGuid_Successful()
		{
			TypeConverterAssert.ConvertFromInstanceDescriptor(typeof(QGuid));
		}

		[Test]
		public void ConvertToString_TestStruct_StringValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertToStringEquals(QGuidTest.TestStruct.ToString(), QGuidTest.TestStruct);
			}
		}

		#endregion

		#region IsValid tests

		[Test]
		public void IsValid_Data_IsFalse()
		{
			Assert.IsFalse(QGuid.IsValid("Complex"), "Complex");
			Assert.IsFalse(QGuid.IsValid((String)null), "(String)null");
			Assert.IsFalse(QGuid.IsValid(String.Empty), "String.Empty");
		}
		[Test]
		public void IsValid_Data_IsTrue()
		{
			Assert.IsTrue(QGuid.IsValid("Qowaiv_SVOLibrary_GUIA"), "Qowaiv_SVOLibrary_GUIA");
			Assert.IsTrue(QGuid.IsValid("8a1a8c42-d2ff-e254-e26e-b6abcbf19420"), "8a1a8c42-d2ff-e254-e26e-b6abcbf19420");
		}
		#endregion

	}

	[Serializable]
	public class QGuidSerializeObject
	{
		public int Id { get; set; }
		public QGuid Obj { get; set; }
		public DateTime Date { get; set; }
	}
}
