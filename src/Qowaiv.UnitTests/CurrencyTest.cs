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
	/// <summary>Tests the currency SVO.</summary>
	[TestFixture]
	public class CurrencyTest
	{
		/// <summary>The test instance for most tests.</summary>
		public static readonly Currency TestStruct = Currency.EUR;

		#region currency const tests

		/// <summary>Currency.Empty should be equal to the default of currency.</summary>
		[Test]
		public void Empty_None_EqualsDefault()
		{
			Assert.AreEqual(default(Currency), Currency.Empty);
		}

		#endregion


		#region Current

		[Test]
		public void Current_CurrentCultureNlNL_Germany()
		{
			using (new CultureInfoScope("nl-NL"))
			{
				var act = Currency.Current;
				var exp = Currency.EUR;

				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void Current_CurrentCultureEsEC_Ecuador()
		{
			using (new CultureInfoScope("es-EC"))
			{
				var act = Currency.Current;
				var exp = Currency.USD;

				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void Current_CurrentCultureEn_Empty()
		{
			using (new CultureInfoScope("en"))
			{
				var act = Currency.Current;
				var exp = Currency.Empty;

				Assert.AreEqual(exp, act);
			}
		}

		#endregion


		#region currency IsEmpty tests

		/// <summary>Currency.IsEmpty() should be true for the default of currency.</summary>
		[Test]
		public void IsEmpty_Default_IsTrue()
		{
			Assert.IsTrue(default(Currency).IsEmpty());
		}
		/// <summary>Currency.IsEmpty() should be false for Currency.Unknown.</summary>
		[Test]
		public void IsEmpty_Unknown_IsFalse()
		{
			Assert.IsFalse(Currency.Unknown.IsEmpty());
		}
		/// <summary>Currency.IsEmpty() should be false for the TestStruct.</summary>
		[Test]
		public void IsEmpty_TestStruct_IsFalse()
		{
			Assert.IsFalse(TestStruct.IsEmpty());
		}

		/// <summary>Currency.IsUnknown() should be false for the default of currency.</summary>
		[Test]
		public void IsUnknown_Default_IsFalse()
		{
			Assert.IsFalse(default(Currency).IsUnknown());
		}
		/// <summary>Currency.IsUnknown() should be true for Currency.Unknown.</summary>
		[Test]
		public void IsUnknown_Unknown_IsTrue()
		{
			Assert.IsTrue(Currency.Unknown.IsUnknown());
		}
		/// <summary>Currency.IsUnknown() should be false for the TestStruct.</summary>
		[Test]
		public void IsUnknown_TestStruct_IsFalse()
		{
			Assert.IsFalse(TestStruct.IsUnknown());
		}

		/// <summary>Currency.IsEmptyOrUnknown() should be true for the default of currency.</summary>
		[Test]
		public void IsEmptyOrUnknown_Default_IsFalse()
		{
			Assert.IsTrue(default(Currency).IsEmptyOrUnknown());
		}
		/// <summary>Currency.IsEmptyOrUnknown() should be true for Currency.Unknown.</summary>
		[Test]
		public void IsEmptyOrUnknown_Unknown_IsTrue()
		{
			Assert.IsTrue(Currency.Unknown.IsEmptyOrUnknown());
		}
		/// <summary>Currency.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
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
			Currency val;

			string str = null;

			Assert.IsTrue(Currency.TryParse(str, out val), "Valid");
			Assert.AreEqual(string.Empty, val.ToString(), "Value");
		}

		/// <summary>TryParse string.Empty should be valid.</summary>
		[Test]
		public void TyrParse_StringEmpty_IsValid()
		{
			Currency val;

			string str = string.Empty;

			Assert.IsTrue(Currency.TryParse(str, out val), "Valid");
			Assert.AreEqual(string.Empty, val.ToString(), "Value");
		}

		/// <summary>TryParse "?" should be valid and the result should be Currency.Unknown.</summary>
		[Test]
		public void TyrParse_Questionmark_IsValid()
		{
			Currency val;

			string str = "?";

			Assert.IsTrue(Currency.TryParse(str, out val), "Valid");
			Assert.IsTrue(val.IsUnknown(), "Value");
		}

		/// <summary>TryParse with specified string value should be valid.</summary>
		[Test]
		public void TyrParse_StringValue_IsValid()
		{
			Currency val;

			string str = "USD";

			Assert.IsTrue(Currency.TryParse(str, out val), "Valid");
			Assert.AreEqual(str, val.ToString(), "Value");
		}

		/// <summary>TryParse with specified string value should be invalid.</summary>
		[Test]
		public void TyrParse_StringValue_IsNotValid()
		{
			Currency val;

			string str = "string";

			Assert.IsFalse(Currency.TryParse(str, out val), "Valid");
			Assert.AreEqual(string.Empty, val.ToString(), "Value");
		}

		[Test]
		public void Parse_Unknown_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var act = Currency.Parse("?");
				var exp = Currency.Unknown;
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void Parse_InvalidInput_ThrowsFormatException()
		{
			using (new CultureInfoScope("en-GB"))
			{
				ExceptionAssert.ExpectException<FormatException>
				(() =>
				{
					Currency.Parse("InvalidInput");
				},
				"Not a valid currency");
			}
		}

		[Test]
		public void TryParse_TestStructInput_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = TestStruct;
				var act = Currency.TryParse(exp.ToString());

				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void TryParse_InvalidInput_DefaultValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = default(Currency);
				var act = Currency.TryParse("InvalidInput");

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
				SerializationTest.DeserializeUsingConstructor<Currency>(null, default(StreamingContext));
			},
			"info");
		}

		[Test]
		public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
		{
			ExceptionAssert.ExpectException<SerializationException>
			(() =>
			{
				var info = new SerializationInfo(typeof(Currency), new System.Runtime.Serialization.FormatterConverter());
				SerializationTest.DeserializeUsingConstructor<Currency>(info, default(StreamingContext));
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
			var info = new SerializationInfo(typeof(Currency), new System.Runtime.Serialization.FormatterConverter());
			obj.GetObjectData(info, default(StreamingContext));

			Assert.AreEqual("EUR", info.GetString("Value"));
		}

		[Test]
		public void SerializeDeserialize_TestStruct_AreEqual()
		{
			var input = CurrencyTest.TestStruct;
			var exp = CurrencyTest.TestStruct;
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void DataContractSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = CurrencyTest.TestStruct;
			var exp = CurrencyTest.TestStruct;
			var act = SerializationTest.DataContractSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void XmlSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = CurrencyTest.TestStruct;
			var exp = CurrencyTest.TestStruct;
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void SerializeDeserialize_CurrencySerializeObject_AreEqual()
		{
			var input = new CurrencySerializeObject()
			{
				Id = 17,
				Obj = CurrencyTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new CurrencySerializeObject()
			{
				Id = 17,
				Obj = CurrencyTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			Assert.AreEqual(exp.Date, act.Date, "Date");
		}
		[Test]
		public void XmlSerializeDeserialize_CurrencySerializeObject_AreEqual()
		{
			var input = new CurrencySerializeObject()
			{
				Id = 17,
				Obj = CurrencyTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new CurrencySerializeObject()
			{
				Id = 17,
				Obj = CurrencyTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			Assert.AreEqual(exp.Date, act.Date, "Date");
		}
		[Test]
		public void DataContractSerializeDeserialize_CurrencySerializeObject_AreEqual()
		{
			var input = new CurrencySerializeObject()
			{
				Id = 17,
				Obj = CurrencyTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new CurrencySerializeObject()
			{
				Id = 17,
				Obj = CurrencyTest.TestStruct,
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
			var input = new CurrencySerializeObject()
			{
				Id = 17,
				Obj = CurrencyTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new CurrencySerializeObject()
			{
				Id = 17,
				Obj = CurrencyTest.TestStruct,
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
			var input = new CurrencySerializeObject()
			{
				Id = 17,
				Obj = Currency.Empty,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new CurrencySerializeObject()
			{
				Id = 17,
				Obj = Currency.Empty,
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
			var act = JsonTester.Read<Currency>();
			var exp = Currency.Empty;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_InvalidStringValue_AssertFormatException()
		{
			ExceptionAssert.ExpectException<FormatException>(() =>
			{
				JsonTester.Read<Currency>("InvalidStringValue");
			},
			"Not a valid currency");
		}
		[Test]
		public void FromJson_StringValue_AreEqual()
		{
			var act = JsonTester.Read<Currency>(TestStruct.ToString(CultureInfo.InvariantCulture));
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_Int64Value_AreEqual()
		{
			var act = JsonTester.Read<Currency>((Int64)TestStruct);
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_DoubleValue_AssertNotSupportedException()
		{
			ExceptionAssert.ExpectException<NotSupportedException>(() =>
			{
				JsonTester.Read<Currency>(1234.56);
			},
			"JSON deserialization from a number is not supported.");
		}

		[Test]
		public void FromJson_DateTimeValue_AssertNotSupportedException()
		{
			ExceptionAssert.ExpectException<NotSupportedException>(() =>
			{
				JsonTester.Read<Currency>(new DateTime(1972, 02, 14));
			},
			"JSON deserialization from a date is not supported.");
		}

		[Test]
		public void ToJson_DefaultValue_AreEqual()
		{
			object act = JsonTester.Write(default(Currency));
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
			var act = Currency.Empty.ToString();
			var exp = "";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_Unknown_QuestionMark()
		{
			var act = Currency.Unknown.ToString();
			var exp = "?";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_CustomFormatter_SupportsCustomFormatting()
		{
			var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
			var exp = "Unit Test Formatter, value: 'EUR', format: 'Unit Test Format'";

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void ToString_TestStruct_ComplexPattern()
		{
			var act = TestStruct.ToString("");
			var exp = "EUR";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_ValueDutchBelgium_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = Currency.Parse("Amerikaanse dollar").ToString();
				var exp = "USD";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_ValueEnglishGreatBritain_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var act = Currency.Parse("pound sterling").ToString("f");
				var exp = "Pound sterling";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void DebuggerDisplay_DebugToString_HasAttribute()
		{
			DebuggerDisplayAssert.HasAttribute(typeof(Currency));
		}

		[Test]
		public void DebuggerDisplay_DefaultValue_String()
		{
			DebuggerDisplayAssert.HasResult("Currency: (empty)", default(Currency));
		}
		[Test]
		public void DebuggerDisplay_Unknown_String()
		{
			DebuggerDisplayAssert.HasResult("Currency: (unknown)", Currency.Unknown);
		}

		[Test]
		public void DebuggerDisplay_TestStruct_String()
		{
			DebuggerDisplayAssert.HasResult("Currency: Euro (EUR/978)", TestStruct);
		}

		#endregion

		#region IEquatable tests

		/// <summary>GetHash should not fail for Currency.Empty.</summary>
		[Test]
		public void GetHash_Empty_0()
		{
			Assert.AreEqual(0, Currency.Empty.GetHashCode());
		}

		/// <summary>GetHash should not fail for the test struct.</summary>
		[Test]
		public void GetHash_TestStruct_Min31357301()
		{
			Assert.AreEqual(-31357301, CurrencyTest.TestStruct.GetHashCode());
		}

		[Test]
		public void Equals_EmptyEmpty_IsTrue()
		{
			Assert.IsTrue(Currency.Empty.Equals(Currency.Empty));
		}

		[Test]
		public void Equals_FormattedAndUnformatted_IsTrue()
		{
			var l = Currency.Parse("eur", CultureInfo.InvariantCulture);
			var r = Currency.Parse("EUR", CultureInfo.InvariantCulture);

			Assert.IsTrue(l.Equals(r));
		}

		[Test]
		public void Equals_TestStructTestStruct_IsTrue()
		{
			Assert.IsTrue(CurrencyTest.TestStruct.Equals(CurrencyTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructEmpty_IsFalse()
		{
			Assert.IsFalse(CurrencyTest.TestStruct.Equals(Currency.Empty));
		}

		[Test]
		public void Equals_EmptyTestStruct_IsFalse()
		{
			Assert.IsFalse(Currency.Empty.Equals(CurrencyTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructObjectTestStruct_IsTrue()
		{
			Assert.IsTrue(CurrencyTest.TestStruct.Equals((object)CurrencyTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructNull_IsFalse()
		{
			Assert.IsFalse(CurrencyTest.TestStruct.Equals(null));
		}

		[Test]
		public void Equals_TestStructObject_IsFalse()
		{
			Assert.IsFalse(CurrencyTest.TestStruct.Equals(new object()));
		}

		[Test]
		public void OperatorIs_TestStructTestStruct_IsTrue()
		{
			var l = CurrencyTest.TestStruct;
			var r = CurrencyTest.TestStruct;
			Assert.IsTrue(l == r);
		}

		[Test]
		public void OperatorIsNot_TestStructTestStruct_IsFalse()
		{
			var l = CurrencyTest.TestStruct;
			var r = CurrencyTest.TestStruct;
			Assert.IsFalse(l != r);
		}

		#endregion

		#region IComparable tests

		/// <summary>Orders a list of currencys ascending.</summary>
		[Test]
		public void OrderBy_Currency_AreEqual()
		{
			var item0 = Currency.AED;
			var item1 = Currency.BAM;
			var item2 = Currency.CAD;
			var item3 = Currency.EUR;

			var inp = new List<Currency>() { Currency.Empty, item3, item2, item0, item1, Currency.Empty };
			var exp = new List<Currency>() { Currency.Empty, Currency.Empty, item0, item1, item2, item3 };
			var act = inp.OrderBy(item => item).ToList();

			CollectionAssert.AreEqual(exp, act);
		}

		/// <summary>Orders a list of currencys descending.</summary>
		[Test]
		public void OrderByDescending_Currency_AreEqual()
		{
			var item0 = Currency.AED;
			var item1 = Currency.BAM;
			var item2 = Currency.CAD;
			var item3 = Currency.EUR;

			var inp = new List<Currency>() { Currency.Empty, item3, item2, item0, item1, Currency.Empty };
			var exp = new List<Currency>() { item3, item2, item1, item0, Currency.Empty, Currency.Empty };
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
				"Argument must be a currency"
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
				"Argument must be a currency"
			);
		}
		#endregion

		#region Casting tests

		[Test]
		public void Explicit_StringToCurrency_AreEqual()
		{
			var exp = TestStruct;
			var act = (Currency)TestStruct.ToString();

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_CurrencyToString_AreEqual()
		{
			var exp = TestStruct.ToString();
			var act = (string)TestStruct;

			Assert.AreEqual(exp, act);
		}

		#endregion

		#region Properties

		#endregion

		#region Type converter tests

		[Test]
		public void ConverterExists_Currency_IsTrue()
		{
			TypeConverterAssert.ConverterExists(typeof(Currency));
		}

		[Test]
		public void CanNotConvertFromInt32_Currency_IsTrue()
		{
			TypeConverterAssert.CanNotConvertFrom(typeof(Currency), typeof(Int32));
		}
		[Test]
		public void CanNotConvertToInt32_Currency_IsTrue()
		{
			TypeConverterAssert.CanNotConvertTo(typeof(Currency), typeof(Int32));
		}

		[Test]
		public void CanConvertFromString_Currency_IsTrue()
		{
			TypeConverterAssert.CanConvertFromString(typeof(Currency));
		}

		[Test]
		public void CanConvertToString_Currency_IsTrue()
		{
			TypeConverterAssert.CanConvertToString(typeof(Currency));
		}

		[Test]
		public void ConvertFrom_StringNull_CurrencyEmpty()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(Currency.Empty, (string)null);
			}
		}

		[Test]
		public void ConvertFrom_StringEmpty_CurrencyEmpty()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(Currency.Empty, string.Empty);
			}
		}

		[Test]
		public void ConvertFromString_StringValue_TestStruct()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(CurrencyTest.TestStruct, CurrencyTest.TestStruct.ToString());
			}
		}

		[Test]
		public void ConvertFromInstanceDescriptor_Currency_Successful()
		{
			TypeConverterAssert.ConvertFromInstanceDescriptor(typeof(Currency));
		}

		[Test]
		public void ConvertToString_TestStruct_StringValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertToStringEquals(CurrencyTest.TestStruct.ToString(), CurrencyTest.TestStruct);
			}
		}

		#endregion

		#region IsValid tests

		[Test]
		public void IsValid_Data_IsFalse()
		{
			Assert.IsFalse(Currency.IsValid("Complex"), "Complex");
			Assert.IsFalse(Currency.IsValid((String)null), "(String)null");
			Assert.IsFalse(Currency.IsValid(String.Empty), "String.Empty");
		}
		[Test]
		public void IsValid_Data_IsTrue()
		{
			Assert.IsTrue(Currency.IsValid("Euro"));
		}
		#endregion
	}

	[Serializable]
	public class CurrencySerializeObject
	{
		public int Id { get; set; }
		public Currency Obj { get; set; }
		public DateTime Date { get; set; }
	}
}
