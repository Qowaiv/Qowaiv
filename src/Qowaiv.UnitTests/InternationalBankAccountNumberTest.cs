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
	[TestFixture]
	public class InternationalBankAccountNumberTest
	{
		/// <summary>The test instance for most tests.</summary>
		public static readonly InternationalBankAccountNumber TestStruct = InternationalBankAccountNumber.Parse("NL20 INGB 0001 2345 67");

		#region IBAN const tests

		/// <summary>InternationalBankAccountNumber.Empty should be equal to the default of IBAN.</summary>
		[Test]
		public void Empty_None_EqualsDefault()
		{
			Assert.AreEqual(default(InternationalBankAccountNumber), InternationalBankAccountNumber.Empty);
		}

		#endregion

		#region IBAN IsEmpty tests

		/// <summary>InternationalBankAccountNumber.IsEmpty() should true for the default of IBAN.</summary>
		[Test]
		public void IsEmpty_Default_IsTrue()
		{
			Assert.IsTrue(default(InternationalBankAccountNumber).IsEmpty());
		}

		/// <summary>InternationalBankAccountNumber.IsEmpty() should false for the TestStruct.</summary>
		[Test]
		public void IsEmpty_Default_IsFalse()
		{
			Assert.IsFalse(TestStruct.IsEmpty());
		}

		#endregion

		#region TryParse tests

		/// <summary>TryParse null should be valid.</summary>
		[Test]
		public void TyrParse_Null_IsValid()
		{
			InternationalBankAccountNumber val;

			string str = null;

			Assert.IsTrue(InternationalBankAccountNumber.TryParse(str, out val), "Valid");
			Assert.AreEqual(string.Empty, val.ToString(), "Value");
		}

		/// <summary>TryParse string.Empty should be valid.</summary>
		[Test]
		public void TyrParse_StringEmpty_IsValid()
		{
			InternationalBankAccountNumber val;

			string str = string.Empty;

			Assert.IsTrue(InternationalBankAccountNumber.TryParse(str, out val), "Valid");
			Assert.AreEqual(string.Empty, val.ToString(), "Value");
		}

		/// <summary>TryParse with specified string value should be valid.</summary>
		[Test]
		public void TyrParse_StringValue_IsValid()
		{
			InternationalBankAccountNumber val;

			string str = "NL20INGB0001234567";

			Assert.IsTrue(InternationalBankAccountNumber.TryParse(str, out val), "Valid");
			Assert.AreEqual(str, val.ToString(), "Value");
		}

		/// <summary>TryParse with specified string value should be valid.</summary>
		[Test]
		public void TyrParse_QuestionMark_IsValid()
		{
			InternationalBankAccountNumber val;

			string str = "?";

			Assert.IsTrue(InternationalBankAccountNumber.TryParse(str, out val), "Valid");
			Assert.AreEqual(str, val.ToString(), "Value.ToString()");
			Assert.AreEqual(InternationalBankAccountNumber.Unknown, val, "Value");
		}

		/// <summary>TryParse with specified string value should be invalid.</summary>
		[Test]
		public void TyrParse_StringValue_IsNotValid()
		{
			InternationalBankAccountNumber val;

			string str = "string";

			Assert.IsFalse(InternationalBankAccountNumber.TryParse(str, out val), "Valid");
			Assert.AreEqual(string.Empty, val.ToString(), "Value");
		}

		[Test]
		public void Parse_InvalidInput_ThrowsFormatException()
		{
			using (new CultureInfoScope("en-GB"))
			{
				ExceptionAssert.ExpectException<FormatException>
				(() =>
				{
					InternationalBankAccountNumber.Parse("InvalidInput");
				},
				"Not a valid IBAN");
			}
		}

		[Test]
		public void TryParse_TestStructInput_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = TestStruct;
				var act = InternationalBankAccountNumber.TryParse(exp.ToString());

				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void TryParse_InvalidInput_DefaultValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = default(InternationalBankAccountNumber);
				var act = InternationalBankAccountNumber.TryParse("InvalidInput");

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
				SerializationTest.DeserializeUsingConstructor<InternationalBankAccountNumber>(null, default(StreamingContext));
			},
			"info");
		}

		[Test]
		public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
		{
			ExceptionAssert.ExpectException<SerializationException>
			(() =>
			{
				var info = new SerializationInfo(typeof(InternationalBankAccountNumber), new System.Runtime.Serialization.FormatterConverter());
				SerializationTest.DeserializeUsingConstructor<InternationalBankAccountNumber>(info, default(StreamingContext));
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
		public void SerializeDeserialize_TestStruct_AreEqual()
		{
			var input = InternationalBankAccountNumberTest.TestStruct;
			var exp = InternationalBankAccountNumberTest.TestStruct;
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void DataContractSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = InternationalBankAccountNumberTest.TestStruct;
			var exp = InternationalBankAccountNumberTest.TestStruct;
			var act = SerializationTest.DataContractSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void XmlSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = InternationalBankAccountNumberTest.TestStruct;
			var exp = InternationalBankAccountNumberTest.TestStruct;
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void SerializeDeserialize_InternationalBankAccountNumberSerializeObject_AreEqual()
		{
			var input = new InternationalBankAccountNumberSerializeObject()
			{
				Id = 17,
				Obj = InternationalBankAccountNumberTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new InternationalBankAccountNumberSerializeObject()
			{
				Id = 17,
				Obj = InternationalBankAccountNumberTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			DateTimeAssert.AreEqual(exp.Date, act.Date, "Date");;
		}
		[Test]
		public void XmlSerializeDeserialize_InternationalBankAccountNumberSerializeObject_AreEqual()
		{
			var input = new InternationalBankAccountNumberSerializeObject()
			{
				Id = 17,
				Obj = InternationalBankAccountNumberTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new InternationalBankAccountNumberSerializeObject()
			{
				Id = 17,
				Obj = InternationalBankAccountNumberTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			DateTimeAssert.AreEqual(exp.Date, act.Date, "Date");;
		}
		[Test]
		public void DataContractSerializeDeserialize_InternationalBankAccountNumberSerializeObject_AreEqual()
		{
			var input = new InternationalBankAccountNumberSerializeObject()
			{
				Id = 17,
				Obj = InternationalBankAccountNumberTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new InternationalBankAccountNumberSerializeObject()
			{
				Id = 17,
				Obj = InternationalBankAccountNumberTest.TestStruct,
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
			var input = new InternationalBankAccountNumberSerializeObject()
			{
				Id = 17,
				Obj = InternationalBankAccountNumberTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new InternationalBankAccountNumberSerializeObject()
			{
				Id = 17,
				Obj = InternationalBankAccountNumberTest.TestStruct,
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
			var input = new InternationalBankAccountNumberSerializeObject()
			{
				Id = 17,
				Obj = InternationalBankAccountNumber.Empty,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new InternationalBankAccountNumberSerializeObject()
			{
				Id = 17,
				Obj = InternationalBankAccountNumber.Empty,
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
			var act = JsonTester.Read<InternationalBankAccountNumber>();
			var exp = InternationalBankAccountNumber.Empty;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_InvalidStringValue_AssertFormatException()
		{
			ExceptionAssert.ExpectException<FormatException>(() =>
			{
				JsonTester.Read<InternationalBankAccountNumber>("InvalidStringValue");
			},
			"Not a valid IBAN");
		}
		[Test]
		public void FromJson_StringValue_AreEqual()
		{
			var act = JsonTester.Read<InternationalBankAccountNumber>(TestStruct.ToString(CultureInfo.InvariantCulture));
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_Int64Value_AssertNotSupportedException()
		{
			ExceptionAssert.ExpectException<NotSupportedException>(() =>
			{
				JsonTester.Read<InternationalBankAccountNumber>(123456L);
			},
			"JSON deserialization from an integer is not supported.");
		}

		[Test]
		public void FromJson_DoubleValue_AssertNotSupportedException()
		{
			ExceptionAssert.ExpectException<NotSupportedException>(() =>
			{
				JsonTester.Read<InternationalBankAccountNumber>(1234.56);
			},
			"JSON deserialization from a number is not supported.");
		}

		[Test]
		public void FromJson_DateTimeValue_AssertNotSupportedException()
		{
			ExceptionAssert.ExpectException<NotSupportedException>(() =>
			{
				JsonTester.Read<InternationalBankAccountNumber>(new DateTime(1972, 02, 14));
			},
			"JSON deserialization from a date is not supported.");
		}

		[Test]
		public void ToJson_DefaultValue_AreEqual()
		{
			object act = JsonTester.Write(default(InternationalBankAccountNumber));
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
		public void ToString_Empty_IsStringEmpty()
		{
			var act = InternationalBankAccountNumber.Empty.ToString();
			var exp = "";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_CustomFormatter_SupportsCustomFormatting()
		{
			var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
			var exp = "Unit Test Formatter, value: 'NL20INGB0001234567', format: 'Unit Test Format'";

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_Unknown_IsStringEmpty()
		{
			var act = InternationalBankAccountNumber.Unknown.ToString("", null);
			var exp = "?";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_TestStruct_AreEqual()
		{
			var act = TestStruct.ToString();
			var exp = "NL20INGB0001234567";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_TestStructFormatULower_AreEqual()
		{
			var act = TestStruct.ToString("u");
			var exp = "nl20ingb0001234567";
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void ToString_TestStructFormatUUpper_AreEqual()
		{
			var act = TestStruct.ToString("U");
			var exp = "NL20INGB0001234567";
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void ToString_TestStructFormatFLower_AreEqual()
		{
			var act = TestStruct.ToString("f");
			var exp = "nl20 ingb 0001 2345 67";
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void ToString_TestStructFormatFUpper_AreEqual()
		{
			var act = TestStruct.ToString("F");
			var exp = "NL20 INGB 0001 2345 67";
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void ToString_EmptyFormatF_AreEqual()
		{
			var act = InternationalBankAccountNumber.Empty.ToString("F");
			var exp = "";
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void ToString_UnknownFormatF_AreEqual()
		{
			var act = InternationalBankAccountNumber.Unknown.ToString("F");
			var exp = "?";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void DebuggerDisplay_DebugToString_HasAttribute()
		{
			DebuggerDisplayAssert.HasAttribute(typeof(InternationalBankAccountNumber));
		}

		[Test]
		public void DebuggerDisplay_DefaultValue_String()
		{
			DebuggerDisplayAssert.HasResult("IBAN: (empty)", default(InternationalBankAccountNumber));
		}
		[Test]
		public void DebuggerDisplay_Unknown_String()
		{
			DebuggerDisplayAssert.HasResult("IBAN: (unknown)", InternationalBankAccountNumber.Unknown);
		}

		[Test]
		public void DebuggerDisplay_TestStruct_String()
		{
			DebuggerDisplayAssert.HasResult("IBAN: NL20 INGB 0001 2345 67", TestStruct);
		}

		#endregion

		#region IEquatable tests

		/// <summary>GetHash should not fail for InternationalBankAccountNumber.Empty.</summary>
		[Test]
		public void GetHash_Empty_Hash()
		{
			Assert.AreEqual(0, InternationalBankAccountNumber.Empty.GetHashCode());
		}

		/// <summary>GetHash should not fail for the test struct.</summary>
		[Test]
		public void GetHash_TestStruct_Hash()
		{
			Assert.AreEqual(328152589, InternationalBankAccountNumberTest.TestStruct.GetHashCode());
		}

		[Test]
		public void Equals_EmptyEmpty_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.Empty.Equals(InternationalBankAccountNumber.Empty));
		}

		[Test]
		public void Equals_FormattedAndUnformatted_IsTrue()
		{
			var l = InternationalBankAccountNumber.Parse("NL20 INGB 0001 2345 67");
			var r = InternationalBankAccountNumber.Parse("nl20ingb0001234567");

			Assert.IsTrue(l.Equals(r));
		}

		[Test]
		public void Equals_TestStructTestStruct_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumberTest.TestStruct.Equals(InternationalBankAccountNumberTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructEmpty_IsFalse()
		{
			Assert.IsFalse(InternationalBankAccountNumberTest.TestStruct.Equals(InternationalBankAccountNumber.Empty));
		}

		[Test]
		public void Equals_EmptyTestStruct_IsFalse()
		{
			Assert.IsFalse(InternationalBankAccountNumber.Empty.Equals(InternationalBankAccountNumberTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructObjectTestStruct_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumberTest.TestStruct.Equals((object)InternationalBankAccountNumberTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructNull_IsFalse()
		{
			Assert.IsFalse(InternationalBankAccountNumberTest.TestStruct.Equals(null));
		}

		[Test]
		public void Equals_TestStructObject_IsFalse()
		{
			Assert.IsFalse(InternationalBankAccountNumberTest.TestStruct.Equals(new object()));
		}

		[Test]
		public void OperatorIs_TestStructTestStruct_IsTrue()
		{
			var l = InternationalBankAccountNumberTest.TestStruct;
			var r = InternationalBankAccountNumberTest.TestStruct;
			Assert.IsTrue(l == r);
		}

		[Test]
		public void OperatorIsNot_TestStructTestStruct_IsFalse()
		{
			var l = InternationalBankAccountNumberTest.TestStruct;
			var r = InternationalBankAccountNumberTest.TestStruct;
			Assert.IsFalse(l != r);
		}

		#endregion

		#region IComparable tests

		/// <summary>Orders a list of IBANs ascending.</summary>
		[Test]
		public void OrderBy_InternationalBankAccountNumber_AreEqual()
		{
			var item0 = InternationalBankAccountNumber.Parse("AE950210000000693123456");
			var item1 = InternationalBankAccountNumber.Parse("BH29BMAG1299123456BH00");
			var item2 = InternationalBankAccountNumber.Parse("CY17002001280000001200527600");
			var item3 = InternationalBankAccountNumber.Parse("DK5000400440116243");

			var inp = new List<InternationalBankAccountNumber>() { InternationalBankAccountNumber.Empty, item3, item2, item0, item1, InternationalBankAccountNumber.Empty };
			var exp = new List<InternationalBankAccountNumber>() { InternationalBankAccountNumber.Empty, InternationalBankAccountNumber.Empty, item0, item1, item2, item3 };
			var act = inp.OrderBy(item => item).ToList();

			CollectionAssert.AreEqual(exp, act);
		}

		/// <summary>Orders a list of IBANs descending.</summary>
		[Test]
		public void OrderByDescending_InternationalBankAccountNumber_AreEqual()
		{
			var item0 = InternationalBankAccountNumber.Parse("AE950210000000693123456");
			var item1 = InternationalBankAccountNumber.Parse("BH29BMAG1299123456BH00");
			var item2 = InternationalBankAccountNumber.Parse("CY17002001280000001200527600");
			var item3 = InternationalBankAccountNumber.Parse("DK5000400440116243");

			var inp = new List<InternationalBankAccountNumber>() { InternationalBankAccountNumber.Empty, item3, item2, item0, item1, InternationalBankAccountNumber.Empty };
			var exp = new List<InternationalBankAccountNumber>() { item3, item2, item1, item0, InternationalBankAccountNumber.Empty, InternationalBankAccountNumber.Empty };
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
				"Argument must be an IBAN"
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
				"Argument must be an IBAN"
			);
		}
		#endregion

		#region Casting tests

		[Test]
		public void Explicit_StringToInternationalBankAccountNumber_AreEqual()
		{
			var exp = TestStruct;
			var act = (InternationalBankAccountNumber)TestStruct.ToString();

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_InternationalBankAccountNumberToString_AreEqual()
		{
			var exp = TestStruct.ToString();
			var act = (string)TestStruct;

			Assert.AreEqual(exp, act);
		}

		#endregion

		#region Properties

		[Test]
		public void Length_DefaultValue_0()
		{
			var exp = 0;
			var act = InternationalBankAccountNumber.Empty.Length;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Length_Unknown_0()
		{
			var exp = 0;
			var act = InternationalBankAccountNumber.Unknown.Length;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Length_TestStruct_IntValue()
		{
			var exp = 18;
			var act = TestStruct.Length;
			Assert.AreEqual(exp, act);
		}


		[Test]
		public void Country_Empty_Null()
		{
			var exp = Country.Empty;
			var act = InternationalBankAccountNumber.Empty.Country;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Country_Unknown_Null()
		{
			var exp = Country.Unknown;
			var act = InternationalBankAccountNumber.Unknown.Country;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Country_TestStruct_Null()
		{
			var exp = Country.NL;
			var act = TestStruct.Country;
			Assert.AreEqual(exp, act);
		}

		#endregion

		#region Type converter tests

		[Test]
		public void ConverterExists_InternationalBankAccountNumber_IsTrue()
		{
			TypeConverterAssert.ConverterExists(typeof(InternationalBankAccountNumber));
		}

		[Test]
		public void CanNotConvertFromInt32_InternationalBankAccountNumber_IsTrue()
		{
			TypeConverterAssert.CanNotConvertFrom(typeof(InternationalBankAccountNumber), typeof(Int32));
		}
		[Test]
		public void CanNotConvertToInt32_InternationalBankAccountNumber_IsTrue()
		{
			TypeConverterAssert.CanNotConvertTo(typeof(InternationalBankAccountNumber), typeof(Int32));
		}

		[Test]
		public void CanConvertFromString_InternationalBankAccountNumber_IsTrue()
		{
			TypeConverterAssert.CanConvertFromString(typeof(InternationalBankAccountNumber));
		}

		[Test]
		public void CanConvertToString_InternationalBankAccountNumber_IsTrue()
		{
			TypeConverterAssert.CanConvertToString(typeof(InternationalBankAccountNumber));
		}

		[Test]
		public void ConvertFrom_StringNull_InternationalBankAccountNumberEmpty()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(InternationalBankAccountNumber.Empty, (string)null);
			}
		}

		[Test]
		public void ConvertFrom_StringEmpty_InternationalBankAccountNumberEmpty()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(InternationalBankAccountNumber.Empty, string.Empty);
			}
		}

		[Test]
		public void ConvertFromString_StringValue_TestStruct()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(InternationalBankAccountNumberTest.TestStruct, InternationalBankAccountNumberTest.TestStruct.ToString());
			}
		}

		[Test]
		public void ConvertFromInstanceDescriptor_InternationalBankAccountNumber_Successful()
		{
			TypeConverterAssert.ConvertFromInstanceDescriptor(typeof(InternationalBankAccountNumber));
		}

		[Test]
		public void ConvertToString_TestStruct_StringValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertToStringEquals(InternationalBankAccountNumberTest.TestStruct.ToString(), InternationalBankAccountNumberTest.TestStruct);
			}
		}

		#endregion

		#region IsValid tests

		[Test]
		public void IsValid_NullValues_IsFalse()
		{
			Assert.IsFalse(InternationalBankAccountNumber.IsValid(String.Empty), "String.Empty");
			Assert.IsFalse(InternationalBankAccountNumber.IsValid((String)null), "(String)null");
		}

		[Test]
		public void IsValid_QuestionMark_IsFalse()
		{
			Assert.IsFalse(InternationalBankAccountNumber.IsValid("?"), "String.Empty");
		}

		[Test]
		public void IsValid_XX_IsFalse()
		{
			Assert.IsFalse(InternationalBankAccountNumber.IsValid("XX950210000000693123456"), "Not existing country.");
		}

		[Test]
		public void IsValid_Null_IsFalse()
		{
			Assert.IsFalse(InternationalBankAccountNumber.IsValid(null));
		}
		[Test]
		public void IsValid_StringEmpty_IsFalse()
		{
			Assert.IsFalse(InternationalBankAccountNumber.IsValid(""));
		}

		// To Short.
		[Test]
		public void IsValid_NL20INGB007_IsFalse()
		{
			Assert.IsFalse(InternationalBankAccountNumber.IsValid("NL20INGB007"));
		}

		// Wrong pattern
		[Test]
		public void IsValid_WilhelmusVanNassau_IsFalse()
		{
			Assert.IsFalse(InternationalBankAccountNumber.IsValid("Wilhelmus van Nassau"));
		}

		[Test]
		public void IsValid_NLWrongSubpattern_IsFalse()
		{
			Assert.IsFalse(InternationalBankAccountNumber.IsValid("NL20INGB000123456Z"));
		}

		[Test]
		public void IsValid_XXNonExistentCountry_IsFalse()
		{
			Assert.IsFalse(InternationalBankAccountNumber.IsValid("XX20INGB0001234567"));
		}

		[Test]
		public void IsValid_USCountryWithoutPattern_IsFalse()
		{
			Assert.IsFalse(InternationalBankAccountNumber.IsValid("US20INGB0001234567"));
		}

		[Test]
		public void IsValid_StrangeSpacing_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("NL20  INGB000	123 4567"));
		}

		[Test]
		public void IsValid_AE_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("AE950210000000693123456"), "United Arab Emirates");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("AE95 0210 0000 0069 3123 456"), "United Arab Emirates Formatted");
		}

		[Test]
		public void IsValid_AL_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("AL47212110090000000235698741"), "Albania");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("AL47 2121 1009 0000 0002 3569 8741"), "Albania Formatted");
		}

		[Test]
		public void IsValid_AD_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("AD1200012030200359100100"), "Andorra");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("AD12 0001 2030 2003 5910 0100"), "Andorra Formatted");
		}

		[Test]
		public void IsValid_AT_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("AT611904300234573201"), "Austria");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("AT61 1904 3002 3457 3201"), "Austria Formatted");
		}

		[Test]
		public void IsValid_BA_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("BA391290079401028494"), "Bosnia and Herzegovina");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("BA39 1290 0794 0102 8494"), "Bosnia and Herzegovina Formatted");
		}

		[Test]
		public void IsValid_BE_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("BE43068999999501"), "Belgium");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("BE43 0689 9999 9501"), "Belgium Formatted");
		}

		[Test]
		public void IsValid_BG_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("BG80BNBG96611020345678"), "Bulgaria");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("BG80 BNBG 9661 1020 3456 78"), "Bulgaria Formatted");
		}

		[Test]
		public void IsValid_BH_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("BH29BMAG1299123456BH00"), "Bahrain");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("BH29 BMAG 1299 1234 56BH 00"), "Bahrain Formatted");
		}

		[Test]
		public void IsValid_BR_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("BR9700360305000010009795493P1"), "Brazil");
		}


		[Test]
		public void IsValid_CH_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("CH3608387000001080173"), "Switzerland");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("CH36 0838 7000 0010 8017 3"), "Switzerland Formatted");
		}

		[Test]
		public void IsValid_CY_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("CY17002001280000001200527600"), "Cyprus");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("CY17 0020 0128 0000 0012 0052 7600"), "Cyprus Formatted");
		}

		[Test]
		public void IsValid_CZ_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("CZ6508000000192000145399"), "Czech Republic");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("CZ65 0800 0000 1920 0014 5399"), "Czech Republic Formatted");
		}

		[Test]
		public void IsValid_DE_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("DE68210501700012345678"), "Germany");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("DE68 2105 0170 0012 3456 78"), "Germany Formatted");
		}

		[Test]
		public void IsValid_DK_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("DK5000400440116243"), "Denmark");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("DK50 0040 0440 1162 43"), "Denmark Formatted");
		}

		[Test]
		public void IsValid_DO_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("DO28BAGR00000001212453611324"), "Dominican Republic");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("DO28 BAGR 0000 0001 2124 5361 1324"), "Dominican Republic Formatted");
		}

		[Test]
		public void IsValid_EE_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("EE382200221020145685"), "Estonia");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("EE38 2200 2210 2014 5685"), "Estonia Formatted");
		}

		[Test]
		public void IsValid_ES_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("ES9121000418450200051332"), "Spain");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("ES91 2100 0418 4502 0005 1332"), "Spain Formatted");
		}

		[Test]
		public void IsValid_FI_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("FI2112345600000785"), "Finland");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("FI21 1234 5600 0007 85"), "Finland Formatted");
		}

		[Test]
		public void IsValid_FO_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("FO2000400440116243"), "Faroe Islands");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("FO20 0040 0440 1162 43"), "Faroe Islands Formatted");
		}

		[Test]
		public void IsValid_FR_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("FR1420041010050500013M02606"), "Frankrijk");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("FR14 2004 1010 0505 0001 3M02 606"), "Frankrijk Formatted");
		}

		[Test]
		public void IsValid_GB_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("GB82WEST12345698765432"), "United Kingdom");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("GB82 WEST 1234 5698 7654 32"), "United Kingdom Formatted");
		}

		[Test]
		public void IsValid_GE_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("GE29NB0000000101904917"), "Georgia");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("GE29 NB00 0000 0101 9049 17"), "Georgia Formatted");
		}

		[Test]
		public void IsValid_GI_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("GI75NWBK000000007099453"), "Gibraltar");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("GI75 NWBK 0000 0000 7099 453"), "Gibraltar Formatted");
		}

		[Test]
		public void IsValid_GL_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("GL2000400440116243"), "Greenland");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("GL20 0040 0440 1162 43"), "Greenland Formatted");
		}

		[Test]
		public void IsValid_GR_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("GR1601101250000000012300695"), "Greece");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("GR16 0110 1250 0000 0001 2300 695"), "Greece Formatted");
		}

		[Test]
		public void IsValid_HR_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("HR1210010051863000160"), "United Kingdom");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("HR12 1001 0051 8630 0016 0"), "United Kingdom Formatted");
		}

		[Test]
		public void IsValid_HU_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("HU42117730161111101800000000"), "Hungary");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("HU42 1177 3016 1111 1018 0000 0000"), "Hungary Formatted");
		}

		[Test]
		public void IsValid_IE_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("IE29AIBK93115212345678"), "Ireland");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("IE29 AIBK 9311 5212 3456 78"), "Ireland Formatted");
		}

		[Test]
		public void IsValid_IL_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("IL620108000000099999999"), "Israel");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("IL62 0108 0000 0009 9999 999"), "Israel Formatted");
		}

		[Test]
		public void IsValid_IS_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("IS140159260076545510730339"), "Iceland");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("IS14 0159 2600 7654 5510 7303 39"), "Iceland Formatted");
		}

		[Test]
		public void IsValid_IT_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("IT60X0542811101000000123456"), "Italy");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("IT60 X054 2811 1010 0000 0123 456"), "Italy Formatted");
		}

		[Test]
		public void IsValid_KW_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("KW81CBKU0000000000001234560101"), "Kuwait");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("KW81 CBKU 0000 0000 0000 1234 5601 01"), "Kuwait Formatted");
		}

		[Test]
		public void IsValid_KZ_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("KZ75125KZT2069100100"), "Kazakhstan");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("KZ75 125K ZT20 6910 0100"), "Kazakhstan Formatted");
		}

		[Test]
		public void IsValid_LB_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("LB30099900000001001925579115"), "Lebanon");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("LB30 0999 0000 0001 0019 2557 9115"), "Lebanon Formatted");
		}

		[Test]
		public void IsValid_LI_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("LI21088100002324013AA"), "Liechtenstein");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("LI21 0881 0000 2324 013A A"), "Liechtenstein Formatted");
		}

		[Test]
		public void IsValid_LT_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("LT121000011101001000"), "Lithuania");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("LT12 1000 0111 0100 1000"), "Lithuania Formatted");
		}

		[Test]
		public void IsValid_LU_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("LU280019400644750000"), "Luxembourg");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("LU28 0019 4006 4475 0000"), "Luxembourg Formatted");
		}

		[Test]
		public void IsValid_LV_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("LV80BANK0000435195001"), "Latvia");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("LV80 BANK 0000 4351 9500 1"), "Latvia Formatted");
		}

		[Test]
		public void IsValid_MC_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("MC1112739000700011111000H79"), "Monaco");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("MC11 1273 9000 7000 1111 1000 H79"), "Monaco Formatted");
		}

		[Test]
		public void IsValid_ME_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("ME25505000012345678951"), "Montenegro");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("ME25 5050 0001 2345 6789 51"), "Montenegro Formatted");
		}

		[Test]
		public void IsValid_MK_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("MK07250120000058984"), "Macedonia");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("MK07 2501 2000 0058 984"), "Macedonia Formatted");
		}

		[Test]
		public void IsValid_MR_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("MR1300020001010000123456753"), "Mauritania");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("MR13 0002 0001 0100 0012 3456 753"), "Mauritania Formatted");
		}

		[Test]
		public void IsValid_MT_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("MT84MALT011000012345MTLCAST001S"), "Malta");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("MT84 MALT 0110 0001 2345 MTLC AST0 01S"), "Malta Formatted");
		}

		[Test]
		public void IsValid_MU_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("MU17BOMM0101101030300200000MUR"), "Mauritius");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("MU17 BOMM 0101 1010 3030 0200 000M UR"), "Mauritius Formatted");
		}

		[Test]
		public void IsValid_NL20INGB0001234567_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("NL20INGB0001234567"), "Unformatted");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("NL20 INGB 0001 2345 67"), "Formatted");
		}
		[Test]
		public void IsValid_NL44RABO0123456789_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("NL44RABO0123456789"), "Unformatted");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("NL44 RABO 0123 4567 89"), "Formatted");
		}

		[Test]
		public void IsValid_NO_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("NO9386011117947"), "Norway");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("NO93 8601 1117 947"), "Norway Formatted");
		}

		[Test]
		public void IsValid_PL_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("PL61109010140000071219812874"), "Poland");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("PL61 1090 1014 0000 0712 1981 2874"), "Poland Formatted");
		}

		[Test]
		public void IsValid_PT_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("PT50000201231234567890154"), "Portugal");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("PT50 0002 0123 1234 5678 9015 4"), "Portugal Formatted");
		}

		[Test]
		public void IsValid_RO_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("RO49AAAA1B31007593840000"), "Romania");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("RO49 AAAA 1B31 0075 9384 0000"), "Romania Formatted");
		}

		[Test]
		public void IsValid_RS_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("RS35260005601001611379"), "Romania");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("RS35 2600 0560 1001 6113 79"), "Romania Formatted");
		}

		[Test]
		public void IsValid_SA_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("SA8440000108054011730013"), "Saudi Arabia");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("SA84 4000 0108 0540 1173 0013"), "Saudi Arabia Formatted");
		}

		[Test]
		public void IsValid_SE_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("SE3550000000054910000003"), "Sweden");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("SE35 5000 0000 0549 1000 0003"), "Sweden Formatted");
		}

		[Test]
		public void IsValid_SI_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("SI56191000000123438"), "Slovenia");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("SI56 1910 0000 0123 438"), "Slovenia Formatted");
		}

		[Test]
		public void IsValid_SK_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("SK3112000000198742637541"), "Slovakia");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("SK31 1200 0000 1987 4263 7541"), "Slovakia Formatted");
		}

		[Test]
		public void IsValid_SM_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("SM86U0322509800000000270100"), "San Marino");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("SM86 U032 2509 8000 0000 0270 100"), "San Marino Formatted");
		}

		[Test]
		public void IsValid_TN_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("TN5910006035183598478831"), "Tunisia");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("TN59 1000 6035 1835 9847 8831"), "Tunisia Formatted");
		}

		[Test]
		public void IsValid_TR_IsTrue()
		{
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("TR330006100519786457841326"), "Turkey");
			Assert.IsTrue(InternationalBankAccountNumber.IsValid("TR33 0006 1005 1978 6457 8413 26"), "Turkey Formatted");
		}

		#endregion
	}

	[Serializable]
	public class InternationalBankAccountNumberSerializeObject
	{
		public int Id { get; set; }
		public InternationalBankAccountNumber Obj { get; set; }
		public DateTime Date { get; set; }
	}
}
