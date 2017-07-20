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
	/// <summary>Tests the postal code SVO.</summary>
	/// <remarks>
	/// The type of tests that are done per country:
	/// <example>
	///     var country = Country.*;
	///     
	///     IsValid("AA0000000000DF", country); //Not
	///     IsValid("AB0123456789CD", country); //Not
	///     IsValid("BJ1282353436RF", country); //Not
	///     IsValid("CD2037570044WK", country); //Not
	///     IsValid("DE3243436478SD", country); //Not
	///     IsValid("EO4008279475PJ", country); //Not
	///     IsValid("FN5697836450KF", country); //Not
	///     IsValid("GF6282469088LS", country); //Not
	///     IsValid("HL7611343495JD", country); //Not
	///     IsValid("ID6767185502MO", country); //Not
	///     IsValid("JS8752391832DF", country); //Not
	///     IsValid("KN9999999999JS", country); //Not
	///     IsValid("LO0000000000DF", country); //Not
	///     IsValid("ME0144942325DS", country); //Not
	///     IsValid("NN1282353436RF", country); //Not
	///     IsValid("OL2037570044WK", country); //Not
	///     IsValid("PS3243436478SD", country); //Not
	///     IsValid("QD4008279475PJ", country); //Not
	///     IsValid("RN5697836450KF", country); //Not
	///     IsValid("SE6282469088LS", country); //Not
	///     IsValid("TM7611343495JD", country); //Not
	///     IsValid("UF6767185502MO", country); //Not
	///     IsValid("VE8752391832DF", country); //Not
	///     IsValid("WL9999999999JS", country); //Not
	///     IsValid("XM0000000000DF", country); //Not
	///     IsValid("YE0144942325DS", country); //Not
	///     IsValid("ZL1282353436RF", country); //Not
	///     IsValid("ZZ2037570044WK", country); //Not
	/// </example>
	/// </remarks>
	[TestFixture]
	public partial class PostalCodeTest
	{
		/// <summary>The test instance for most tests.</summary>
		public static readonly PostalCode TestStruct = PostalCode.Parse("H0H0H0");

		#region postal code const tests

		/// <summary>PostalCode.Empty should be equal to the default of postal code.</summary>
		[Test]
		public void Empty_None_EqualsDefault()
		{
			Assert.AreEqual(default(PostalCode), PostalCode.Empty);
		}

		#endregion

		#region postal code IsEmpty tests

		/// <summary>PostalCode.IsEmpty() should be true for the default of postal code.</summary>
		[Test]
		public void IsEmpty_Default_IsTrue()
		{
			Assert.IsTrue(default(PostalCode).IsEmpty());
		}
		/// <summary>PostalCode.IsEmpty() should be false for PostalCode.Unknown.</summary>
		[Test]
		public void IsEmpty_Unknown_IsFalse()
		{
			Assert.IsFalse(PostalCode.Unknown.IsEmpty());
		}
		/// <summary>PostalCode.IsEmpty() should be false for the TestStruct.</summary>
		[Test]
		public void IsEmpty_TestStruct_IsFalse()
		{
			Assert.IsFalse(TestStruct.IsEmpty());
		}

		/// <summary>PostalCode.IsUnknown() should be false for the default of postal code.</summary>
		[Test]
		public void IsUnknown_Default_IsFalse()
		{
			Assert.IsFalse(default(PostalCode).IsUnknown());
		}
		/// <summary>PostalCode.IsUnknown() should be true for PostalCode.Unknown.</summary>
		[Test]
		public void IsUnknown_Unknown_IsTrue()
		{
			Assert.IsTrue(PostalCode.Unknown.IsUnknown());
		}
		/// <summary>PostalCode.IsUnknown() should be false for the TestStruct.</summary>
		[Test]
		public void IsUnknown_TestStruct_IsFalse()
		{
			Assert.IsFalse(TestStruct.IsUnknown());
		}

		/// <summary>PostalCode.IsEmptyOrUnknown() should be true for the default of postal code.</summary>
		[Test]
		public void IsEmptyOrUnknown_Default_IsFalse()
		{
			Assert.IsTrue(default(PostalCode).IsEmptyOrUnknown());
		}
		/// <summary>PostalCode.IsEmptyOrUnknown() should be true for PostalCode.Unknown.</summary>
		[Test]
		public void IsEmptyOrUnknown_Unknown_IsTrue()
		{
			Assert.IsTrue(PostalCode.Unknown.IsEmptyOrUnknown());
		}
		/// <summary>PostalCode.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
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
			PostalCode val;

			string str = null;

			Assert.IsTrue(PostalCode.TryParse(str, out val), "Valid");
			Assert.AreEqual(string.Empty, val.ToString(), "Value");
		}

		/// <summary>TryParse string.Empty should be valid.</summary>
		[Test]
		public void TyrParse_StringEmpty_IsValid()
		{
			PostalCode val;

			string str = string.Empty;

			Assert.IsTrue(PostalCode.TryParse(str, out val), "Valid");
			Assert.AreEqual(string.Empty, val.ToString(), "Value");
		}

		/// <summary>TryParse with specified string value should be valid.</summary>
		[Test]
		public void TyrParse_StringValue_IsValid()
		{
			PostalCode val;

			string str = "H0H0H0";

			Assert.IsTrue(PostalCode.TryParse(str, out val), "Valid");
			Assert.AreEqual(str, val.ToString(), "Value");
		}

		/// <summary>TryParse with specified string value should be invalid.</summary>
		[Test]
		public void TyrParse_StringValue_IsNotValid()
		{
			PostalCode val;

			string str = "1";

			Assert.IsFalse(PostalCode.TryParse(str, out val), "Valid");
			Assert.AreEqual(string.Empty, val.ToString(), "Value");
		}

		[Test]
		public void Parse_Unknown_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var act = PostalCode.Parse("?");
				var exp = PostalCode.Unknown;
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
					PostalCode.Parse("InvalidInput");
				},
				"Not a valid postal code");
			}
		}

		[Test]
		public void TryParse_TestStructInput_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = TestStruct;
				var act = PostalCode.TryParse(exp.ToString());

				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void TryParse_InvalidInput_DefaultValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = default(PostalCode);
				var act = PostalCode.TryParse("InvalidInput");

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
				SerializationTest.DeserializeUsingConstructor<PostalCode>(null, default(StreamingContext));
			},
			"info");
		}

		[Test]
		public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
		{
			Assert.Catch<SerializationException>
			(() =>
			{
				var info = new SerializationInfo(typeof(PostalCode), new System.Runtime.Serialization.FormatterConverter());
				SerializationTest.DeserializeUsingConstructor<PostalCode>(info, default(StreamingContext));
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
			var info = new SerializationInfo(typeof(PostalCode), new System.Runtime.Serialization.FormatterConverter());
			obj.GetObjectData(info, default(StreamingContext));

			Assert.AreEqual(TestStruct.ToString(), info.GetString("Value"));
		}

		[Test]
		public void SerializeDeserialize_TestStruct_AreEqual()
		{
			var input = PostalCodeTest.TestStruct;
			var exp = PostalCodeTest.TestStruct;
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void DataContractSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = PostalCodeTest.TestStruct;
			var exp = PostalCodeTest.TestStruct;
			var act = SerializationTest.DataContractSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void XmlSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = PostalCodeTest.TestStruct;
			var exp = PostalCodeTest.TestStruct;
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void SerializeDeserialize_PostalCodeSerializeObject_AreEqual()
		{
			var input = new PostalCodeSerializeObject()
			{
				Id = 17,
				Obj = PostalCodeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new PostalCodeSerializeObject()
			{
				Id = 17,
				Obj = PostalCodeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			DateTimeAssert.AreEqual(exp.Date, act.Date, "Date");;
		}
		[Test]
		public void XmlSerializeDeserialize_PostalCodeSerializeObject_AreEqual()
		{
			var input = new PostalCodeSerializeObject()
			{
				Id = 17,
				Obj = PostalCodeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new PostalCodeSerializeObject()
			{
				Id = 17,
				Obj = PostalCodeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			DateTimeAssert.AreEqual(exp.Date, act.Date, "Date");;
		}
		[Test]
		public void DataContractSerializeDeserialize_PostalCodeSerializeObject_AreEqual()
		{
			var input = new PostalCodeSerializeObject()
			{
				Id = 17,
				Obj = PostalCodeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new PostalCodeSerializeObject()
			{
				Id = 17,
				Obj = PostalCodeTest.TestStruct,
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
			var input = new PostalCodeSerializeObject()
			{
				Id = 17,
				Obj = PostalCodeTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new PostalCodeSerializeObject()
			{
				Id = 17,
				Obj = PostalCodeTest.TestStruct,
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
			var input = new PostalCodeSerializeObject()
			{
				Id = 17,
				Obj = PostalCode.Empty,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new PostalCodeSerializeObject()
			{
				Id = 17,
				Obj = PostalCode.Empty,
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
			var act = JsonTester.Read<PostalCode>();
			var exp = PostalCode.Empty;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_InvalidStringValue_AssertFormatException()
		{
			Assert.Catch<FormatException>(() =>
			{
				JsonTester.Read<PostalCode>("InvalidStringValue");
			},
			"Not a valid postal code");
		}
		[Test]
		public void FromJson_StringValue_AreEqual()
		{
			var act = JsonTester.Read<PostalCode>(TestStruct.ToString(CultureInfo.InvariantCulture));
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_Int64Value_AssertNotSupportedException()
		{
			Assert.Catch<NotSupportedException>(() =>
			{
				JsonTester.Read<PostalCode>(123456L);
			},
			"JSON deserialization from an integer is not supported.");
		}

		[Test]
		public void FromJson_DoubleValue_AssertNotSupportedException()
		{
			Assert.Catch<NotSupportedException>(() =>
			{
				JsonTester.Read<PostalCode>(1234.56);
			},
			"JSON deserialization from a number is not supported.");
		}

		[Test]
		public void FromJson_DateTimeValue_AssertNotSupportedException()
		{
			Assert.Catch<NotSupportedException>(() =>
			{
				JsonTester.Read<PostalCode>(new DateTime(1972, 02, 14));
			},
			"JSON deserialization from a date is not supported.");
		}

		[Test]
		public void ToJson_DefaultValue_AreEqual()
		{
			object act = JsonTester.Write(default(PostalCode));
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

		#region IFormattable / Tostring tests

		[Test]
		public void ToString_Empty_IsStringEmpty()
		{
			var act = PostalCode.Empty.ToString();
			var exp = "";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_EmptyCA_IsStringEmpty()
		{
			var act = PostalCode.Empty.ToString("CA");
			var exp = "";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_Unknown_IsQuestionMark()
		{
			var act = PostalCode.Unknown.ToString();
			var exp = "?";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_UnknownCA_IsQuestionMark()
		{
			var act = PostalCode.Unknown.ToString("CA");
			var exp = "?";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_CustomFormatter_SupportsCustomFormatting()
		{
			var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
			var exp = "Unit Test Formatter, value: 'H0H0H0', format: 'Unit Test Format'";

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_TestStructCA_ComplexPattern()
		{
			var act = TestStruct.ToString("CA");
			var exp = "H0H 0H0";
			Assert.AreEqual(exp, act);
		}

		/// <remarks>No valid for the Netherlands.</summary>
		[Test]
		public void ToString_TestStructNL_ComplexPattern()
		{
			var act = TestStruct.ToString(Country.NL);
			var exp = "H0H0H0";
			Assert.AreEqual(exp, act);
		}

		/// <remarks>No postal code in Somalia.</summary>
		[Test]
		public void ToString_TestStructSO_ComplexPattern()
		{
			var act = TestStruct.ToString(Country.SO);
			var exp = "H0H0H0";
			Assert.AreEqual(exp, act);
		}

		/// <remarks>No formatting in Albania.</summary>
		[Test]
		public void ToString_TestStructAL_ComplexPattern()
		{
			var act = TestStruct.ToString(Country.AL);
			var exp = "H0H0H0";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_AD765AD_ComplexPattern()
		{
			var postalcode = PostalCode.Parse("AD765");
			var act = postalcode.ToString("AD");
			var exp = "AD-765";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_765AD_ComplexPattern()
		{
			var postalcode = PostalCode.Parse("765");
			var act = postalcode.ToString("AD");
			var exp = "AD-765";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void DebuggerDisplay_DebugToString_HasAttribute()
		{
			DebuggerDisplayAssert.HasAttribute(typeof(PostalCode));
		}

		[Test]
		public void DebuggerDisplay_DefaultValue_String()
		{
			DebuggerDisplayAssert.HasResult("PostalCode: (empty)", default(PostalCode));
		}

		[Test]
		public void DebuggerDisplay_TestStruct_String()
		{
			DebuggerDisplayAssert.HasResult("PostalCode: H0H0H0", TestStruct);
		}

		#endregion

		#region IEquatable tests

		/// <summary>GetHash should not fail for PostalCode.Empty.</summary>
		[Test]
		public void GetHash_Empty_Hash()
		{
			Assert.AreEqual(0, PostalCode.Empty.GetHashCode());
		}

		/// <summary>GetHash should not fail for the test struct.</summary>
		[Test]
		public void GetHash_TestStruct_Hash()
		{
			Assert.AreEqual(-1500506634, PostalCodeTest.TestStruct.GetHashCode());
		}

		[Test]
		public void Equals_EmptyEmpty_IsTrue()
		{
			Assert.IsTrue(PostalCode.Empty.Equals(PostalCode.Empty));
		}

		[Test]
		public void Equals_FormattedAndUnformatted_IsTrue()
		{
			var l = PostalCode.Parse("H0 H0 H0-", CultureInfo.InvariantCulture);
			var r = PostalCode.Parse("h0h0h0", CultureInfo.InvariantCulture);

			Assert.IsTrue(l.Equals(r));
		}

		[Test]
		public void Equals_TestStructTestStruct_IsTrue()
		{
			Assert.IsTrue(PostalCodeTest.TestStruct.Equals(PostalCodeTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructEmpty_IsFalse()
		{
			Assert.IsFalse(PostalCodeTest.TestStruct.Equals(PostalCode.Empty));
		}

		[Test]
		public void Equals_EmptyTestStruct_IsFalse()
		{
			Assert.IsFalse(PostalCode.Empty.Equals(PostalCodeTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructObjectTestStruct_IsTrue()
		{
			Assert.IsTrue(PostalCodeTest.TestStruct.Equals((object)PostalCodeTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructNull_IsFalse()
		{
			Assert.IsFalse(PostalCodeTest.TestStruct.Equals(null));
		}

		[Test]
		public void Equals_TestStructObject_IsFalse()
		{
			Assert.IsFalse(PostalCodeTest.TestStruct.Equals(new object()));
		}

		[Test]
		public void OperatorIs_TestStructTestStruct_IsTrue()
		{
			var l = PostalCodeTest.TestStruct;
			var r = PostalCodeTest.TestStruct;
			Assert.IsTrue(l == r);
		}

		[Test]
		public void OperatorIsNot_TestStructTestStruct_IsFalse()
		{
			var l = PostalCodeTest.TestStruct;
			var r = PostalCodeTest.TestStruct;
			Assert.IsFalse(l != r);
		}

		#endregion

		#region IComparable tests

		/// <summary>Orders a list of postal codes ascending.</summary>
		[Test]
		public void OrderBy_PostalCode_AreEqual()
		{
			var item0 = PostalCode.Parse("012");
			var item1 = PostalCode.Parse("1234");
			var item2 = PostalCode.Parse("23456");
			var item3 = PostalCode.Parse("345678");

			var inp = new List<PostalCode>() { PostalCode.Empty, item3, item2, item0, item1, PostalCode.Empty };
			var exp = new List<PostalCode>() { PostalCode.Empty, PostalCode.Empty, item0, item1, item2, item3 };
			var act = inp.OrderBy(item => item).ToList();

			CollectionAssert.AreEqual(exp, act);
		}

		/// <summary>Orders a list of postal codes descending.</summary>
		[Test]
		public void OrderByDescending_PostalCode_AreEqual()
		{
			var item0 = PostalCode.Parse("012");
			var item1 = PostalCode.Parse("1234");
			var item2 = PostalCode.Parse("23456");
			var item3 = PostalCode.Parse("345678");

			var inp = new List<PostalCode>() { PostalCode.Empty, item3, item2, item0, item1, PostalCode.Empty };
			var exp = new List<PostalCode>() { item3, item2, item1, item0, PostalCode.Empty, PostalCode.Empty };
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
				"Argument must be a postal code"
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
				"Argument must be a postal code"
			);
		}
		#endregion

		#region Casting tests

		[Test]
		public void Explicit_StringToPostalCode_AreEqual()
		{
			var exp = TestStruct;
			var act = (PostalCode)TestStruct.ToString();

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_PostalCodeToString_AreEqual()
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
			var act = PostalCode.Empty.Length;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Length_TestStruct_IntValue()
		{
			var exp = 6;
			var act = TestStruct.Length;
			Assert.AreEqual(exp, act);
		}
		#endregion

		#region Type converter tests

		[Test]
		public void ConverterExists_PostalCode_IsTrue()
		{
			TypeConverterAssert.ConverterExists(typeof(PostalCode));
		}

		[Test]
		public void CanNotConvertFromInt32_PostalCode_IsTrue()
		{
			TypeConverterAssert.CanNotConvertFrom(typeof(PostalCode), typeof(Int32));
		}
		[Test]
		public void CanNotConvertToInt32_PostalCode_IsTrue()
		{
			TypeConverterAssert.CanNotConvertTo(typeof(PostalCode), typeof(Int32));
		}

		[Test]
		public void CanConvertFromString_PostalCode_IsTrue()
		{
			TypeConverterAssert.CanConvertFromString(typeof(PostalCode));
		}

		[Test]
		public void CanConvertToString_PostalCode_IsTrue()
		{
			TypeConverterAssert.CanConvertToString(typeof(PostalCode));
		}

		[Test]
		public void ConvertFrom_StringNull_PostalCodeEmpty()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(PostalCode.Empty, (string)null);
			}
		}

		[Test]
		public void ConvertFrom_StringEmpty_PostalCodeEmpty()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(PostalCode.Empty, string.Empty);
			}
		}

		[Test]
		public void ConvertFromString_StringValue_TestStruct()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(PostalCodeTest.TestStruct, PostalCodeTest.TestStruct.ToString());
			}
		}

		[Test]
		public void ConvertFromInstanceDescriptor_PostalCode_Successful()
		{
			TypeConverterAssert.ConvertFromInstanceDescriptor(typeof(PostalCode));
		}

		[Test]
		public void ConvertToString_TestStruct_StringValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertToStringEquals(PostalCodeTest.TestStruct.ToString(), PostalCodeTest.TestStruct);
			}
		}

		#endregion

		#region IsValid tests

		[Test]
		public void IsValid_Data_IsFalse()
		{
			Assert.IsFalse(PostalCode.IsValid("1"), "1");
			Assert.IsFalse(PostalCode.IsValid("12345678901"), "12345678901");
			Assert.IsFalse(PostalCode.IsValid((String)null), "(String)null");
			Assert.IsFalse(PostalCode.IsValid(string.Empty), "string.Empty");
		}

		[Test]
		public void IsValid_Data_IsTrue()
		{
			Assert.IsTrue(PostalCode.IsValid("1234AB"));
		}

		[Test]
		public void IsValid_EmptyCA_IsFalse()
		{
			Assert.IsFalse(PostalCode.Empty.IsValid(Country.CA));
		}
		[Test]
		public void IsValid_UnknownCA_IsFalse()
		{
			Assert.IsFalse(PostalCode.Unknown.IsValid(Country.CA));
		}

		[Test]
		public void IsValid_TestStructCA_IsTrue()
		{
			Assert.IsTrue(TestStruct.IsValid(Country.CA));
		}
		[Test]
		public void IsValid_TestStructBE_IsFalse()
		{
			Assert.IsFalse(TestStruct.IsValid(Country.BE));
		}
		[Test]
		public void IsValidFor_TestStruct_1Country()
		{
			var act = TestStruct.IsValidFor().ToArray();
			var exp = new Country[] { Country.CA };
			CollectionAssert.AllItemsAreUnique(act);

			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void IsValidFor_0123456_3Countries()
		{
			var postalcode = PostalCode.Parse("0123456");
			var act = postalcode.IsValidFor().ToArray();
			var exp = new Country[] { Country.CL, Country.IL, Country.JP };
			CollectionAssert.AllItemsAreUnique(act);

			CollectionAssert.AreEqual(exp, act);
		}

		#endregion

		#region IsValid Country tests

		// Tests patterns that should be valid for Andorra (AD).
		[TestCase("AD", "AD123")]
		[TestCase("AD", "AD345")]
		[TestCase("AD", "AD678")]
		[TestCase("AD", "AD789")]
		// Tests patterns that should be valid for Afghanistan (AF)
		[TestCase("AF", "4301")]
		[TestCase("AF", "1001")]
		[TestCase("AF", "2023")]
		[TestCase("AF", "1102")]
		[TestCase("AF", "4020")]
		[TestCase("AF", "3077")]
		[TestCase("AF", "2650")]
		[TestCase("AF", "4241")]
		// Tests patterns that should be valid for Anguilla(AI).
		[TestCase("AI", "2640")]
		[TestCase("AI", "AI-2640")]
		[TestCase("AI", "AI2640")]
		[TestCase("AI", "ai-2640")]
		[TestCase("AI", "ai2640")]
		[TestCase("AI", "ai 2640")]
		[TestCase("AI", "ai.2640")]
		// Tests patterns that should be valid for Albania (AL).
		[TestCase("AL", "1872")]
		[TestCase("AL", "2540")]
		[TestCase("AL", "7900")]
		[TestCase("AL", "9999")]
		// Tests patterns that should be valid for Armenia (AM).
		[TestCase("AM", "0123")]
		[TestCase("AM", "1234")]
		[TestCase("AM", "2000")]
		[TestCase("AM", "3248")]
		[TestCase("AM", "4945")]
		// Tests patterns that should be valid for Argentina (AR).
		[TestCase("AR", "A4400XXX")]
		[TestCase("AR", "C 1420 ABC")]
		[TestCase("AR", "S 2300DDD")]
		[TestCase("AR", "Z9400 QOW")]
		// Tests patterns that should be valid for American Samoa (AS).
		[TestCase("AS", "91000-0060")]
		[TestCase("AS", "91000-9996")]
		[TestCase("AS", "90126")]
		[TestCase("AS", "92345")]
		// Tests patterns that should be valid for Austria (AT).
		[TestCase("AT", "2471")]
		[TestCase("AT", "1000")]
		[TestCase("AT", "5120")]
		[TestCase("AT", "9999")]
		// Tests patterns that should be valid for Australia (AU).
		[TestCase("AU", "0872")]
		[TestCase("AU", "2540")]
		[TestCase("AU", "0900")]
		[TestCase("AU", "9999")]
		// Tests patterns that should be valid for Åland Islands (AX).
		[TestCase("AX", "22-000")]
		[TestCase("AX", "22-123")]
		[TestCase("AX", "22000")]
		[TestCase("AX", "22345")]
		// Tests patterns that should be valid for Azerbaijan (AZ).
		[TestCase("AZ", "1499")]
		[TestCase("AZ", "az 1499")]
		[TestCase("AZ", "AZ-1499")]
		[TestCase("AZ", "az1499")]
		[TestCase("AZ", "AZ0499")]
		[TestCase("AZ", "AZ0099")]
		[TestCase("AZ", "aZ6990")]
		// Tests patterns that should be valid for Bosnia And Herzegovina (BA).
		[TestCase("BA", "00000")]
		[TestCase("BA", "01235")]
		[TestCase("BA", "12346")]
		[TestCase("BA", "20004")]
		[TestCase("BA", "32648")]
		[TestCase("BA", "40945")]
		[TestCase("BA", "56640")]
		[TestCase("BA", "62908")]
		[TestCase("BA", "76345")]
		[TestCase("BA", "67552")]
		[TestCase("BA", "87182")]
		[TestCase("BA", "99999")]
		// Tests patterns that should be valid for Barbados (BB).
		[TestCase("BB", "21499")]
		[TestCase("BB", "01499")]
		[TestCase("BB", "bB-31499")]
		[TestCase("BB", "BB 01499")]
		[TestCase("BB", "bb81499")]
		[TestCase("BB", "BB71499")]
		[TestCase("BB", "BB56990")]
		// Tests patterns that should be valid for Bangladesh (BD).
		[TestCase("BD", "0483")]
		[TestCase("BD", "1480")]
		[TestCase("BD", "5492")]
		[TestCase("BD", "7695")]
		[TestCase("BD", "9796")]
		// Tests patterns that should be valid for Belgium (BE).
		[TestCase("BE", "2471")]
		[TestCase("BE", "1000")]
		[TestCase("BE", "5120")]
		[TestCase("BE", "9999")]
		// Tests patterns that should be valid for Bulgaria (BG).
		[TestCase("BG", "1000")]
		[TestCase("BG", "2077")]
		[TestCase("BG", "2650")]
		[TestCase("BG", "4241")]
		// Tests patterns that should be valid for Bahrain (BH).
		[TestCase("BH", "199")]
		[TestCase("BH", "1299")]
		[TestCase("BH", "666")]
		[TestCase("BH", "890")]
		[TestCase("BH", "768")]
		[TestCase("BH", "1000")]
		[TestCase("BH", "1176")]
		// Tests patterns that should be valid for Saint Barthélemy (BL).
		[TestCase("BL", "97700")]
		[TestCase("BL", "97701")]
		[TestCase("BL", "97712")]
		[TestCase("BL", "97720")]
		[TestCase("BL", "97732")]
		[TestCase("BL", "97740")]
		[TestCase("BL", "97756")]
		[TestCase("BL", "97762")]
		[TestCase("BL", "97776")]
		[TestCase("BL", "97767")]
		[TestCase("BL", "97787")]
		[TestCase("BL", "97799")]
		// Tests patterns that should be valid for Bermuda (BM).
		[TestCase("BM", "AA")]
		[TestCase("BM", "AS")]
		[TestCase("BM", "BJ")]
		[TestCase("BM", "CD")]
		[TestCase("BM", "DE")]
		[TestCase("BM", "EO")]
		[TestCase("BM", "FN")]
		[TestCase("BM", "GF")]
		[TestCase("BM", "HL")]
		[TestCase("BM", "ID")]
		[TestCase("BM", "JS")]
		[TestCase("BM", "KN")]
		[TestCase("BM", "LO")]
		[TestCase("BM", "ME")]
		[TestCase("BM", "NN")]
		[TestCase("BM", "OL")]
		[TestCase("BM", "PS")]
		[TestCase("BM", "QD")]
		[TestCase("BM", "RN")]
		[TestCase("BM", "SE")]
		[TestCase("BM", "TM")]
		[TestCase("BM", "UF")]
		[TestCase("BM", "VE")]
		[TestCase("BM", "WL")]
		[TestCase("BM", "XM")]
		[TestCase("BM", "YE")]
		[TestCase("BM", "ZL")]
		[TestCase("BM", "ZZ")]
		[TestCase("BM", "AA0F")]
		[TestCase("BM", "AS0S")]
		[TestCase("BM", "BJ1F")]
		[TestCase("BM", "CD2K")]
		[TestCase("BM", "DE3D")]
		[TestCase("BM", "EO4J")]
		[TestCase("BM", "FN5F")]
		[TestCase("BM", "GF6S")]
		[TestCase("BM", "HL7D")]
		[TestCase("BM", "ID69")]
		[TestCase("BM", "JS66")]
		[TestCase("BM", "KN48")]
		[TestCase("BM", "LO12")]
		[TestCase("BM", "MEDS")]
		[TestCase("BM", "NNRF")]
		[TestCase("BM", "OLWK")]
		[TestCase("BM", "PSSD")]
		[TestCase("BM", "QDPJ")]
		[TestCase("BM", "RNKF")]
		[TestCase("BM", "SELS")]
		[TestCase("BM", "TMD1")]
		[TestCase("BM", "UFO7")]
		[TestCase("BM", "VEF2")]
		[TestCase("BM", "WLS9")]
		[TestCase("BM", "XMF0")]
		[TestCase("BM", "YES4")]
		[TestCase("BM", "ZLF2")]
		[TestCase("BM", "ZZK7")]
		// Tests patterns that should be valid for Brunei Darussalam (BN).
		[TestCase("BN", "YZ0000")]
		[TestCase("BN", "BU2529")]
		[TestCase("BN", "bU2529")]
		[TestCase("BN", "bu2529")]
		[TestCase("BN", "Bu2529")]
		// Tests patterns that should be valid for Bolivia (BO).
		[TestCase("BO", "0123")]
		[TestCase("BO", "1234")]
		[TestCase("BO", "2000")]
		[TestCase("BO", "3248")]
		[TestCase("BO", "4945")]
		[TestCase("BO", "5640")]
		[TestCase("BO", "6208")]
		[TestCase("BO", "7645")]
		[TestCase("BO", "6752")]
		[TestCase("BO", "8782")]
		[TestCase("BO", "9999")]
		// Tests patterns that should be valid for Brazil (BR).
		[TestCase("BR", "01000-000")]
		[TestCase("BR", "01000999")]
		[TestCase("BR", "88000-123")]
		// Tests patterns that should be valid for Bhutan (BT).
		[TestCase("BT", "000")]
		[TestCase("BT", "012")]
		[TestCase("BT", "123")]
		[TestCase("BT", "200")]
		[TestCase("BT", "326")]
		[TestCase("BT", "409")]
		[TestCase("BT", "566")]
		[TestCase("BT", "629")]
		[TestCase("BT", "763")]
		[TestCase("BT", "675")]
		[TestCase("BT", "871")]
		[TestCase("BT", "999")]
		// Tests patterns that should be valid for Belarus (BY).
		[TestCase("BY", "010185")]
		[TestCase("BY", "110000")]
		[TestCase("BY", "342600")]
		[TestCase("BY", "610185")]
		[TestCase("BY", "910185")]
		// Tests patterns that should be valid for Canada (CA).
		[TestCase("CA", "H0H-0H0")]
		[TestCase("CA", "K8 N5W 6")]
		[TestCase("CA", "A1A 1A1")]
		[TestCase("CA", "K0H 9Z0")]
		[TestCase("CA", "T1R 9Z0")]
		[TestCase("CA", "P2V9z0")]
		// Tests patterns that should be valid for Cocos (CC).
		[TestCase("CC", "0123")]
		[TestCase("CC", "1234")]
		[TestCase("CC", "2000")]
		[TestCase("CC", "3248")]
		[TestCase("CC", "4945")]
		[TestCase("CC", "5640")]
		[TestCase("CC", "6208")]
		[TestCase("CC", "7645")]
		[TestCase("CC", "6752")]
		[TestCase("CC", "8782")]
		[TestCase("CC", "9999")]
		// Tests patterns that should be valid for Switzerland (CH).
		[TestCase("CH", "1001")]
		[TestCase("CH", "8023")]
		[TestCase("CH", "9100")]
		[TestCase("CH", "1000")]
		[TestCase("CH", "2077")]
		[TestCase("CH", "2650")]
		[TestCase("CH", "4241")]
		// Tests patterns that should be valid for Chile (CL).
		[TestCase("CL", "0000000")]
		[TestCase("CL", "0231145")]
		[TestCase("CL", "1342456")]
		[TestCase("CL", "2000974")]
		[TestCase("CL", "3642438")]
		[TestCase("CL", "4940375")]
		[TestCase("CL", "5646230")]
		[TestCase("CL", "6902168")]
		[TestCase("CL", "7346345")]
		[TestCase("CL", "6557682")]
		[TestCase("CL", "8187992")]
		[TestCase("CL", "9999999")]
		// Tests patterns that should be valid for China (CN).
		[TestCase("CN", "010000")]
		[TestCase("CN", "342600")]
		[TestCase("CN", "810185")]
		// Tests patterns that should be valid for Colombia (CO).
		[TestCase("CO", "000000")]
		[TestCase("CO", "023145")]
		[TestCase("CO", "134256")]
		[TestCase("CO", "200074")]
		[TestCase("CO", "364238")]
		[TestCase("CO", "494075")]
		[TestCase("CO", "564630")]
		[TestCase("CO", "690268")]
		[TestCase("CO", "734645")]
		[TestCase("CO", "655782")]
		[TestCase("CO", "818792")]
		[TestCase("CO", "999999")]
		// Tests patterns that should be valid for Costa Rica (CR).
		[TestCase("CR", "00000")]
		[TestCase("CR", "01235")]
		[TestCase("CR", "12346")]
		[TestCase("CR", "20004")]
		[TestCase("CR", "32648")]
		[TestCase("CR", "40945")]
		[TestCase("CR", "56640")]
		[TestCase("CR", "62908")]
		[TestCase("CR", "76345")]
		[TestCase("CR", "67552")]
		[TestCase("CR", "87182")]
		[TestCase("CR", "99999")]
		// Tests patterns that should be valid for Cuba (CU).
		[TestCase("CU", "00000")]
		[TestCase("CU", "01235")]
		[TestCase("CU", "12346")]
		[TestCase("CU", "20004")]
		[TestCase("CU", "32648")]
		[TestCase("CU", "40945")]
		[TestCase("CU", "56640")]
		[TestCase("CU", "62908")]
		[TestCase("CU", "76345")]
		[TestCase("CU", "67552")]
		[TestCase("CU", "87182")]
		[TestCase("CU", "99999")]
		[TestCase("CU", "CP00000")]
		[TestCase("CU", "CP01235")]
		[TestCase("CU", "CP12346")]
		[TestCase("CU", "CP20004")]
		[TestCase("CU", "CP32648")]
		// Tests patterns that should be valid for Cape Verde (CV).
		[TestCase("CV", "0000")]
		[TestCase("CV", "1000")]
		[TestCase("CV", "2077")]
		[TestCase("CV", "2650")]
		[TestCase("CV", "4241")]
		// Tests patterns that should be valid for Christmas Island (CX).
		[TestCase("CX", "0000")]
		[TestCase("CX", "0144")]
		[TestCase("CX", "1282")]
		[TestCase("CX", "2037")]
		[TestCase("CX", "3243")]
		[TestCase("CX", "4008")]
		[TestCase("CX", "5697")]
		[TestCase("CX", "6282")]
		[TestCase("CX", "7611")]
		[TestCase("CX", "6767")]
		[TestCase("CX", "8752")]
		[TestCase("CX", "9999")]
		// Tests patterns that should be valid for Cyprus (CY).
		[TestCase("CY", "1000")]
		[TestCase("CY", "2077")]
		[TestCase("CY", "2650")]
		[TestCase("CY", "4241")]
		// Tests patterns that should be valid for Czech Republic (CZ).
		[TestCase("CZ", "21234")]
		[TestCase("CZ", "12345")]
		[TestCase("CZ", "11111")]
		[TestCase("CZ", "123 45")]
		// Tests patterns that should be valid for Germany (DE).
		[TestCase("DE", "10000")]
		[TestCase("DE", "01123")]
		[TestCase("DE", "89000")]
		[TestCase("DE", "12345")]
		// Tests patterns that should be valid for Denmark (DK).
		[TestCase("DK", "1499")]
		[TestCase("DK", "dk-1499")]
		[TestCase("DK", "DK-1499")]
		[TestCase("DK", "dk1499")]
		[TestCase("DK", "DK1499")]
		[TestCase("DK", "DK6990")]
		// Tests patterns that should be valid for Algeria (DZ).
		[TestCase("DZ", "01234")]
		[TestCase("DZ", "12345")]
		[TestCase("DZ", "11111")]
		// Tests patterns that should be valid for Ecuador (EC).
		[TestCase("EC", "000000")]
		[TestCase("EC", "023145")]
		[TestCase("EC", "134256")]
		[TestCase("EC", "200074")]
		[TestCase("EC", "364238")]
		[TestCase("EC", "494075")]
		[TestCase("EC", "564630")]
		[TestCase("EC", "690268")]
		[TestCase("EC", "734645")]
		[TestCase("EC", "655782")]
		[TestCase("EC", "818792")]
		[TestCase("EC", "999999")]
		// Tests patterns that should be valid for Estonia (EE).
		[TestCase("EE", "00000")]
		[TestCase("EE", "01235")]
		[TestCase("EE", "12346")]
		[TestCase("EE", "20004")]
		[TestCase("EE", "32648")]
		[TestCase("EE", "40945")]
		[TestCase("EE", "56640")]
		[TestCase("EE", "62908")]
		[TestCase("EE", "76345")]
		[TestCase("EE", "67552")]
		[TestCase("EE", "87182")]
		[TestCase("EE", "99999")]
		// Tests patterns that should be valid for Egypt (EG).
		[TestCase("EG", "12346")]
		[TestCase("EG", "20004")]
		[TestCase("EG", "32648")]
		[TestCase("EG", "40945")]
		[TestCase("EG", "56640")]
		[TestCase("EG", "62908")]
		[TestCase("EG", "76345")]
		[TestCase("EG", "67552")]
		[TestCase("EG", "87182")]
		[TestCase("EG", "99999")]
		// Tests patterns that should be valid for Spain (ES).
		[TestCase("ES", "01070")]
		[TestCase("ES", "10070")]
		[TestCase("ES", "20767")]
		[TestCase("ES", "26560")]
		[TestCase("ES", "32451")]
		[TestCase("ES", "09112")]
		[TestCase("ES", "48221")]
		[TestCase("ES", "50636")]
		[TestCase("ES", "52636")]
		[TestCase("ES", "51050")]
		// Tests patterns that should be valid for Ethiopia (ET).
		[TestCase("ET", "0123")]
		[TestCase("ET", "1234")]
		[TestCase("ET", "2000")]
		[TestCase("ET", "3248")]
		[TestCase("ET", "4945")]
		[TestCase("ET", "5640")]
		[TestCase("ET", "6208")]
		[TestCase("ET", "7645")]
		[TestCase("ET", "6752")]
		[TestCase("ET", "8782")]
		[TestCase("ET", "9999")]
		// Tests patterns that should be valid for Finland (FI).
		[TestCase("FI", "00-000")]
		[TestCase("FI", "01-123")]
		[TestCase("FI", "00000")]
		[TestCase("FI", "12345")]
		// Tests patterns that should be valid for Falkland Islands (FK).
		[TestCase("FK", "FIQQ1ZZ")]
		// Tests patterns that should be valid for Micronesia (FM).
		[TestCase("FM", "96941")]
		[TestCase("FM", "96942")]
		[TestCase("FM", "96943")]
		[TestCase("FM", "96944")]
		[TestCase("FM", "969410000")]
		[TestCase("FM", "969420123")]
		[TestCase("FM", "969430144")]
		[TestCase("FM", "969441282")]
		// Tests patterns that should be valid for Faroe Islands (FO).
		[TestCase("FO", "399")]
		[TestCase("FO", "fo-399")]
		[TestCase("FO", "FO-199")]
		[TestCase("FO", "fO399")]
		[TestCase("FO", "FO678")]
		[TestCase("FO", "FO123")]
		// Tests patterns that should be valid for France (FR).
		[TestCase("FR", "10000")]
		[TestCase("FR", "01123")]
		[TestCase("FR", "89000")]
		[TestCase("FR", "12345")]
		// Tests patterns that should be valid for Gabon (GA).
		[TestCase("GA", "0123")]
		[TestCase("GA", "1234")]
		[TestCase("GA", "2000")]
		[TestCase("GA", "3248")]
		[TestCase("GA", "4945")]
		[TestCase("GA", "5640")]
		[TestCase("GA", "6208")]
		[TestCase("GA", "7645")]
		[TestCase("GA", "6752")]
		[TestCase("GA", "8782")]
		[TestCase("GA", "9999")]
		// Tests patterns that should be valid for United Kingdom (GB).
		[TestCase("GB", "M11AA")]
		[TestCase("GB", "M11aA")]
		[TestCase("GB", "M11AA")]
		[TestCase("GB", "m11AA")]
		[TestCase("GB", "m11aa")]
		[TestCase("GB", "B338TH")]
		[TestCase("GB", "B338TH")]
		[TestCase("GB", "CR26XH")]
		[TestCase("GB", "CR26XH")]
		[TestCase("GB", "DN551PT")]
		[TestCase("GB", "DN551PT")]
		[TestCase("GB", "W1A1HQ")]
		[TestCase("GB", "W1A1HQ")]
		[TestCase("GB", "EC1A1BB")]
		[TestCase("GB", "EC1A1BB")]
		// Tests patterns that should be valid for Georgia (GE).
		[TestCase("GE", "0123")]
		[TestCase("GE", "1234")]
		[TestCase("GE", "2000")]
		[TestCase("GE", "3248")]
		[TestCase("GE", "4945")]
		[TestCase("GE", "5640")]
		[TestCase("GE", "6208")]
		[TestCase("GE", "7645")]
		[TestCase("GE", "6752")]
		[TestCase("GE", "8782")]
		[TestCase("GE", "9999")]
		// Tests patterns that should be valid for French Guiana (GF).
		[TestCase("GF", "97300")]
		[TestCase("GF", "97301")]
		[TestCase("GF", "97312")]
		[TestCase("GF", "97320")]
		[TestCase("GF", "97332")]
		[TestCase("GF", "97340")]
		[TestCase("GF", "97356")]
		[TestCase("GF", "97362")]
		[TestCase("GF", "97376")]
		[TestCase("GF", "97367")]
		[TestCase("GF", "97387")]
		[TestCase("GF", "97399")]
		// Tests patterns that should be valid for Guernsey (GG).
		[TestCase("GG", "00DF")]
		[TestCase("GG", "03DS")]
		[TestCase("GG", "14RF")]
		[TestCase("GG", "20WK")]
		[TestCase("GG", "34SD")]
		[TestCase("GG", "44PJ")]
		[TestCase("GG", "54KF")]
		[TestCase("GG", "60LS")]
		[TestCase("GG", "74JD")]
		[TestCase("GG", "65MO")]
		[TestCase("GG", "88DF")]
		[TestCase("GG", "99JS")]
		[TestCase("GG", "000DF")]
		[TestCase("GG", "015DS")]
		[TestCase("GG", "126RF")]
		[TestCase("GG", "204WK")]
		[TestCase("GG", "328SD")]
		[TestCase("GG", "405PJ")]
		[TestCase("GG", "560KF")]
		[TestCase("GG", "628LS")]
		[TestCase("GG", "765JD")]
		[TestCase("GG", "672MO")]
		[TestCase("GG", "872DF")]
		[TestCase("GG", "999JS")]
		// Tests patterns that should be valid for Gibraltar (GI).
		[TestCase("GI", "GX111AA")]
		// Tests patterns that should be valid for Greenland (GL).
		[TestCase("GL", "3999")]
		[TestCase("GL", "gl-3999")]
		[TestCase("GL", "GL-3999")]
		[TestCase("GL", "gL 3999")]
		[TestCase("GL", "GL3999")]
		[TestCase("GL", "GL3990")]
		// Tests patterns that should be valid for Guadeloupe (GP).
		[TestCase("GP", "97100")]
		[TestCase("GP", "97101")]
		[TestCase("GP", "97112")]
		[TestCase("GP", "97120")]
		[TestCase("GP", "97132")]
		[TestCase("GP", "97140")]
		[TestCase("GP", "97156")]
		[TestCase("GP", "97162")]
		[TestCase("GP", "97176")]
		[TestCase("GP", "97167")]
		[TestCase("GP", "97187")]
		[TestCase("GP", "97199")]
		// Tests patterns that should be valid for Greece (GR).
		[TestCase("GR", "10000")]
		[TestCase("GR", "31123")]
		[TestCase("GR", "89000")]
		[TestCase("GR", "12345")]
		// Tests patterns that should be valid for South Georgia And The South Sandwich Islands (GS).
		[TestCase("GS", "SIQQ1ZZ")]
		// Tests patterns that should be valid for Guatemala (GT).
		[TestCase("GT", "00000")]
		[TestCase("GT", "01235")]
		[TestCase("GT", "12346")]
		[TestCase("GT", "20004")]
		[TestCase("GT", "32648")]
		[TestCase("GT", "40945")]
		[TestCase("GT", "56640")]
		[TestCase("GT", "62908")]
		[TestCase("GT", "76345")]
		[TestCase("GT", "67552")]
		[TestCase("GT", "87182")]
		[TestCase("GT", "99999")]
		// Tests patterns that should be valid for Guam (GU).
		[TestCase("GU", "96910")]
		[TestCase("GU", "96910")]
		[TestCase("GU", "96911")]
		[TestCase("GU", "96912")]
		[TestCase("GU", "96923")]
		[TestCase("GU", "96924")]
		[TestCase("GU", "96925")]
		[TestCase("GU", "96926")]
		[TestCase("GU", "96927")]
		[TestCase("GU", "96926")]
		[TestCase("GU", "96931")]
		[TestCase("GU", "96932")]
		[TestCase("GU", "969100000")]
		[TestCase("GU", "969103015")]
		[TestCase("GU", "969114126")]
		[TestCase("GU", "969120204")]
		[TestCase("GU", "969234328")]
		[TestCase("GU", "969244405")]
		[TestCase("GU", "969254560")]
		[TestCase("GU", "969260628")]
		[TestCase("GU", "969274765")]
		[TestCase("GU", "969265672")]
		[TestCase("GU", "969318872")]
		[TestCase("GU", "969329999")]
		// Tests patterns that should be valid for Guinea-Bissau (GW).
		[TestCase("GW", "0123")]
		[TestCase("GW", "1234")]
		[TestCase("GW", "2000")]
		[TestCase("GW", "3248")]
		[TestCase("GW", "4945")]
		[TestCase("GW", "5640")]
		[TestCase("GW", "6208")]
		[TestCase("GW", "7645")]
		[TestCase("GW", "6752")]
		[TestCase("GW", "8782")]
		[TestCase("GW", "9999")]
		// Tests patterns that should be valid for Heard Island And Mcdonald Islands (HM).
		[TestCase("HM", "0123")]
		[TestCase("HM", "1234")]
		[TestCase("HM", "2000")]
		[TestCase("HM", "3248")]
		[TestCase("HM", "4945")]
		[TestCase("HM", "5640")]
		[TestCase("HM", "6208")]
		[TestCase("HM", "7645")]
		[TestCase("HM", "6752")]
		[TestCase("HM", "8782")]
		[TestCase("HM", "9999")]
		// Tests patterns that should be valid for Honduras (HN).
		[TestCase("HN", "00000")]
		[TestCase("HN", "01235")]
		[TestCase("HN", "12346")]
		[TestCase("HN", "20004")]
		[TestCase("HN", "32648")]
		[TestCase("HN", "40945")]
		[TestCase("HN", "56640")]
		[TestCase("HN", "62908")]
		[TestCase("HN", "76345")]
		[TestCase("HN", "67552")]
		[TestCase("HN", "87182")]
		[TestCase("HN", "99999")]
		// Tests patterns that should be valid for Croatia (HR).
		[TestCase("HR", "00000")]
		[TestCase("HR", "01235")]
		[TestCase("HR", "12346")]
		[TestCase("HR", "20004")]
		[TestCase("HR", "32648")]
		[TestCase("HR", "40945")]
		[TestCase("HR", "56640")]
		[TestCase("HR", "62908")]
		[TestCase("HR", "76345")]
		[TestCase("HR", "67552")]
		[TestCase("HR", "87182")]
		[TestCase("HR", "99999")]
		// Tests patterns that should be valid for Haiti (HT).
		[TestCase("HT", "0123")]
		[TestCase("HT", "1234")]
		[TestCase("HT", "2000")]
		[TestCase("HT", "3248")]
		[TestCase("HT", "4945")]
		[TestCase("HT", "5640")]
		[TestCase("HT", "6208")]
		[TestCase("HT", "7645")]
		[TestCase("HT", "6752")]
		[TestCase("HT", "8782")]
		[TestCase("HT", "9999")]
		// Tests patterns that should be valid for Hungary (HU).
		[TestCase("HU", "1000")]
		[TestCase("HU", "2077")]
		[TestCase("HU", "2650")]
		[TestCase("HU", "4241")]
		// Tests patterns that should be valid for Indonesia (ID).
		[TestCase("ID", "10000")]
		[TestCase("ID", "31123")]
		[TestCase("ID", "89000")]
		[TestCase("ID", "89007")]
		[TestCase("ID", "12340")]
		// Tests patterns that should be valid for Israel (IL).
		[TestCase("IL", "0110023")]
		[TestCase("IL", "1084023")]
		[TestCase("IL", "3108701")]
		[TestCase("IL", "4201907")]
		[TestCase("IL", "5403506")]
		[TestCase("IL", "6177008")]
		// Tests patterns that should be valid for Isle Of Man (IM).
		[TestCase("IM", "00DF")]
		[TestCase("IM", "04DS")]
		[TestCase("IM", "18RF")]
		[TestCase("IM", "23WK")]
		[TestCase("IM", "34SD")]
		[TestCase("IM", "40PJ")]
		[TestCase("IM", "59KF")]
		[TestCase("IM", "68LS")]
		[TestCase("IM", "71JD")]
		[TestCase("IM", "66MO")]
		[TestCase("IM", "85DF")]
		[TestCase("IM", "99JS")]
		[TestCase("IM", "00DF")]
		[TestCase("IM", "000DF")]
		[TestCase("IM", "014DS")]
		[TestCase("IM", "128RF")]
		[TestCase("IM", "203WK")]
		[TestCase("IM", "324SD")]
		[TestCase("IM", "400PJ")]
		[TestCase("IM", "569KF")]
		[TestCase("IM", "628LS")]
		[TestCase("IM", "761JD")]
		[TestCase("IM", "676MO")]
		[TestCase("IM", "875DF")]
		[TestCase("IM", "999JS")]
		[TestCase("IM", "000DF")]
		[TestCase("IM", "IM00DF")]
		[TestCase("IM", "IM04DS")]
		[TestCase("IM", "IM18RF")]
		[TestCase("IM", "IM23WK")]
		[TestCase("IM", "IM34SD")]
		[TestCase("IM", "IM40PJ")]
		[TestCase("IM", "IM59KF")]
		[TestCase("IM", "IM68LS")]
		[TestCase("IM", "IM71JD")]
		[TestCase("IM", "IM66MO")]
		[TestCase("IM", "IM85DF")]
		[TestCase("IM", "IM99JS")]
		[TestCase("IM", "IM00DF")]
		[TestCase("IM", "IM000DF")]
		[TestCase("IM", "IM014DS")]
		[TestCase("IM", "IM128RF")]
		[TestCase("IM", "IM203WK")]
		[TestCase("IM", "IM324SD")]
		[TestCase("IM", "IM400PJ")]
		[TestCase("IM", "IM569KF")]
		[TestCase("IM", "IM628LS")]
		[TestCase("IM", "IM761JD")]
		[TestCase("IM", "IM676MO")]
		[TestCase("IM", "IM875DF")]
		[TestCase("IM", "IM999JS")]
		[TestCase("IM", "IM000DF")]
		[TestCase("IM", "IM00DF")]
		[TestCase("IM", "IM04DS")]
		[TestCase("IM", "IM18RF")]
		[TestCase("IM", "IM23WK")]
		[TestCase("IM", "IM34SD")]
		[TestCase("IM", "IM40PJ")]
		[TestCase("IM", "IM59KF")]
		[TestCase("IM", "IM68LS")]
		[TestCase("IM", "IM71JD")]
		[TestCase("IM", "IM66MO")]
		[TestCase("IM", "IM85DF")]
		[TestCase("IM", "IM99JS")]
		[TestCase("IM", "IM00DF")]
		// Tests patterns that should be valid for India (IN).
		[TestCase("IN", "110000")]
		[TestCase("IN", "342600")]
		[TestCase("IN", "810185")]
		[TestCase("IN", "810 185")]
		// Tests patterns that should be valid for British Indian Ocean Territory (IO).
		[TestCase("IO", "BBND1ZZ")]
		// Tests patterns that should be valid for Iraq (IQ).
		[TestCase("IQ", "12346")]
		[TestCase("IQ", "32648")]
		[TestCase("IQ", "40945")]
		[TestCase("IQ", "56640")]
		[TestCase("IQ", "62908")]
		// Tests patterns that should be valid for Iran (IR).
		[TestCase("IR", "0000000000")]
		[TestCase("IR", "0144942325")]
		[TestCase("IR", "1282353436")]
		[TestCase("IR", "2037570044")]
		[TestCase("IR", "3243436478")]
		[TestCase("IR", "4008279475")]
		[TestCase("IR", "5697836450")]
		[TestCase("IR", "6282469088")]
		[TestCase("IR", "7611343495")]
		[TestCase("IR", "6767185502")]
		[TestCase("IR", "8752391832")]
		[TestCase("IR", "9999999999")]
		// Tests patterns that should be valid for Iceland (IS).
		[TestCase("IS", "000")]
		[TestCase("IS", "035")]
		[TestCase("IS", "146")]
		[TestCase("IS", "204")]
		[TestCase("IS", "348")]
		[TestCase("IS", "445")]
		[TestCase("IS", "540")]
		[TestCase("IS", "608")]
		[TestCase("IS", "745")]
		[TestCase("IS", "652")]
		[TestCase("IS", "882")]
		[TestCase("IS", "999")]
		// Tests patterns that should be valid for Italy (IT).
		[TestCase("IT", "00123")]
		[TestCase("IT", "02123")]
		[TestCase("IT", "31001")]
		[TestCase("IT", "42007")]
		[TestCase("IT", "54006")]
		[TestCase("IT", "91008")]
		// Tests patterns that should be valid for Jersey (JE).
		[TestCase("JE", "00AS")]
		[TestCase("JE", "25GS")]
		[TestCase("JE", "36DF")]
		[TestCase("JE", "44DS")]
		[TestCase("JE", "78RF")]
		[TestCase("JE", "75WK")]
		[TestCase("JE", "50SD")]
		[TestCase("JE", "88PJ")]
		[TestCase("JE", "95KF")]
		[TestCase("JE", "02LS")]
		[TestCase("JE", "32JD")]
		[TestCase("JE", "99MO")]
		[TestCase("JE", "00AS")]
		[TestCase("JE", "042GS")]
		[TestCase("JE", "153DF")]
		[TestCase("JE", "274DS")]
		[TestCase("JE", "337RF")]
		[TestCase("JE", "477WK")]
		[TestCase("JE", "535SD")]
		[TestCase("JE", "668PJ")]
		[TestCase("JE", "749KF")]
		[TestCase("JE", "680LS")]
		[TestCase("JE", "893JD")]
		[TestCase("JE", "999MO")]
		// Tests patterns that should be valid for Jordan (JO).
		[TestCase("JO", "00000")]
		[TestCase("JO", "01235")]
		[TestCase("JO", "12346")]
		[TestCase("JO", "20004")]
		[TestCase("JO", "32648")]
		[TestCase("JO", "40945")]
		[TestCase("JO", "56640")]
		[TestCase("JO", "62908")]
		[TestCase("JO", "76345")]
		[TestCase("JO", "67552")]
		[TestCase("JO", "87182")]
		[TestCase("JO", "99999")]
		// Tests patterns that should be valid for Japan (JP).
		[TestCase("JP", "000-0000")]
		[TestCase("JP", "000-0999")]
		[TestCase("JP", "010-0000")]
		[TestCase("JP", "0100999")]
		[TestCase("JP", "880-0123")]
		[TestCase("JP", "900-0123")]
		// Tests patterns that should be valid for Kyrgyzstan (KG).
		[TestCase("KG", "000000")]
		[TestCase("KG", "023145")]
		[TestCase("KG", "134256")]
		[TestCase("KG", "200074")]
		[TestCase("KG", "364238")]
		[TestCase("KG", "494075")]
		[TestCase("KG", "564630")]
		[TestCase("KG", "690268")]
		[TestCase("KG", "734645")]
		[TestCase("KG", "655782")]
		[TestCase("KG", "818792")]
		[TestCase("KG", "999999")]
		// Tests patterns that should be valid for Cambodia (KH).
		[TestCase("KH", "00000")]
		[TestCase("KH", "01235")]
		[TestCase("KH", "12346")]
		[TestCase("KH", "20004")]
		[TestCase("KH", "32648")]
		[TestCase("KH", "40945")]
		[TestCase("KH", "56640")]
		[TestCase("KH", "62908")]
		[TestCase("KH", "76345")]
		[TestCase("KH", "67552")]
		[TestCase("KH", "87182")]
		[TestCase("KH", "99999")]
		// Tests patterns that should be valid for Korea (KR).
		[TestCase("KR", "110000")]
		[TestCase("KR", "342600")]
		[TestCase("KR", "610185")]
		[TestCase("KR", "410-185")]
		[TestCase("KR", "710-185")]
		// Tests patterns that should be valid for Cayman Islands (KY).
		[TestCase("KY", "00000")]
		[TestCase("KY", "01235")]
		[TestCase("KY", "12346")]
		[TestCase("KY", "20004")]
		[TestCase("KY", "32648")]
		[TestCase("KY", "40945")]
		[TestCase("KY", "56640")]
		[TestCase("KY", "62908")]
		[TestCase("KY", "76345")]
		[TestCase("KY", "67552")]
		[TestCase("KY", "87182")]
		[TestCase("KY", "99999")]
		// Tests patterns that should be valid for Kazakhstan (KZ).
		[TestCase("KZ", "000000")]
		[TestCase("KZ", "023145")]
		[TestCase("KZ", "134256")]
		[TestCase("KZ", "200074")]
		[TestCase("KZ", "364238")]
		[TestCase("KZ", "494075")]
		[TestCase("KZ", "564630")]
		[TestCase("KZ", "690268")]
		[TestCase("KZ", "734645")]
		[TestCase("KZ", "655782")]
		[TestCase("KZ", "818792")]
		[TestCase("KZ", "999999")]
		// Tests patterns that should be valid for Lao People'S Democratic Re
		[TestCase("LA", "00000")]
		[TestCase("LA", "01235")]
		[TestCase("LA", "12346")]
		[TestCase("LA", "20004")]
		[TestCase("LA", "32648")]
		[TestCase("LA", "40945")]
		[TestCase("LA", "56640")]
		[TestCase("LA", "62908")]
		[TestCase("LA", "76345")]
		[TestCase("LA", "67552")]
		[TestCase("LA", "87182")]
		[TestCase("LA", "99999")]
		// Tests patterns that should be valid for Lebanon (LB).
		[TestCase("LB", "00000000")]
		[TestCase("LB", "01442325")]
		[TestCase("LB", "12853436")]
		[TestCase("LB", "20370044")]
		[TestCase("LB", "32436478")]
		[TestCase("LB", "40079475")]
		[TestCase("LB", "56936450")]
		[TestCase("LB", "62869088")]
		[TestCase("LB", "76143495")]
		[TestCase("LB", "67685502")]
		[TestCase("LB", "87591832")]
		[TestCase("LB", "99999999")]
		// Tests patterns that should be valid for Liechtenstein (LI).
		[TestCase("LI", "9485")]
		[TestCase("LI", "9489")]
		[TestCase("LI", "9490")]
		[TestCase("LI", "9498")]
		// Tests patterns that should be valid for Sri Lanka (LK).
		[TestCase("LK", "00000")]
		[TestCase("LK", "10070")]
		[TestCase("LK", "20767")]
		[TestCase("LK", "26560")]
		[TestCase("LK", "32451")]
		[TestCase("LK", "09112")]
		[TestCase("LK", "48221")]
		[TestCase("LK", "54636")]
		[TestCase("LK", "65050")]
		[TestCase("LK", "70162")]
		[TestCase("LK", "81271")]
		[TestCase("LK", "92686")]
		// Tests patterns that should be valid for Liberia (LR).
		[TestCase("LR", "0123")]
		[TestCase("LR", "1234")]
		[TestCase("LR", "2000")]
		[TestCase("LR", "3248")]
		[TestCase("LR", "4945")]
		[TestCase("LR", "5640")]
		[TestCase("LR", "6208")]
		[TestCase("LR", "7645")]
		[TestCase("LR", "6752")]
		[TestCase("LR", "8782")]
		[TestCase("LR", "9999")]
		// Tests patterns that should be valid for Lesotho (LS).
		[TestCase("LS", "000")]
		[TestCase("LS", "015")]
		[TestCase("LS", "126")]
		[TestCase("LS", "204")]
		[TestCase("LS", "328")]
		[TestCase("LS", "405")]
		[TestCase("LS", "560")]
		[TestCase("LS", "628")]
		[TestCase("LS", "765")]
		[TestCase("LS", "672")]
		[TestCase("LS", "872")]
		[TestCase("LS", "999")]
		// Tests patterns that should be valid for Lithuania (LT).
		[TestCase("LT", "21499")]
		[TestCase("LT", "01499")]
		[TestCase("LT", "lT-31499")]
		[TestCase("LT", "LT-01499")]
		[TestCase("LT", "lt81499")]
		[TestCase("LT", "LT71499")]
		[TestCase("LT", "LT56990")]
		// Tests patterns that should be valid for Luxembourg (LU).
		[TestCase("LU", "0123")]
		[TestCase("LU", "1234")]
		[TestCase("LU", "2000")]
		[TestCase("LU", "3248")]
		[TestCase("LU", "4945")]
		[TestCase("LU", "5640")]
		[TestCase("LU", "6208")]
		[TestCase("LU", "7645")]
		[TestCase("LU", "6752")]
		[TestCase("LU", "8782")]
		[TestCase("LU", "9999")]
		// Tests patterns that should be valid for Latvia (LV).
		[TestCase("LV", "0123")]
		[TestCase("LV", "1234")]
		[TestCase("LV", "2000")]
		[TestCase("LV", "3248")]
		[TestCase("LV", "4945")]
		[TestCase("LV", "5640")]
		[TestCase("LV", "6208")]
		[TestCase("LV", "7645")]
		[TestCase("LV", "6752")]
		[TestCase("LV", "8782")]
		[TestCase("LV", "9999")]
		// Tests patterns that should be valid for Libya (LY).
		[TestCase("LY", "00000")]
		[TestCase("LY", "01235")]
		[TestCase("LY", "12346")]
		[TestCase("LY", "20004")]
		[TestCase("LY", "32648")]
		[TestCase("LY", "40945")]
		[TestCase("LY", "56640")]
		[TestCase("LY", "62908")]
		[TestCase("LY", "76345")]
		[TestCase("LY", "67552")]
		[TestCase("LY", "87182")]
		[TestCase("LY", "99999")]
		// Tests patterns that should be valid for Morocco (MA).
		[TestCase("MA", "11 302")]
		[TestCase("MA", "24 023")]
		[TestCase("MA", "45001")]
		[TestCase("MA", "89607")]
		[TestCase("MA", "86096")]
		[TestCase("MA", "85808")]
		// Tests patterns that should be valid for Monaco (MC).
		[TestCase("MC", "MC-98000")]
		[TestCase("MC", "MC-98012")]
		[TestCase("MC", "MC 98023")]
		[TestCase("MC", "mc98089")]
		[TestCase("MC", "MC98099")]
		[TestCase("MC", "Mc98077")]
		[TestCase("MC", "mC98066")]
		[TestCase("MC", "98089")]
		[TestCase("MC", "98099")]
		[TestCase("MC", "98077")]
		[TestCase("MC", "98066")]
		// Tests patterns that should be valid for Moldova (MD).
		[TestCase("MD", "1499")]
		[TestCase("MD", "md-1499")]
		[TestCase("MD", "MD-1499")]
		[TestCase("MD", "md1499")]
		[TestCase("MD", "MD0499")]
		[TestCase("MD", "MD0099")]
		[TestCase("MD", "mD6990")]
		[TestCase("MD", "0123")]
		[TestCase("MD", "1234")]
		[TestCase("MD", "2000")]
		[TestCase("MD", "3248")]
		[TestCase("MD", "4945")]
		[TestCase("MD", "5640")]
		[TestCase("MD", "6208")]
		[TestCase("MD", "7645")]
		[TestCase("MD", "6752")]
		[TestCase("MD", "8782")]
		[TestCase("MD", "9999")]
		// Tests patterns that should be valid for Montenegro (ME).
		[TestCase("ME", "81302")]
		[TestCase("ME", "84023")]
		[TestCase("ME", "85001")]
		[TestCase("ME", "81607")]
		[TestCase("ME", "84096")]
		[TestCase("ME", "85808")]
		// Tests patterns that should be valid for Saint Martin (MF).
		[TestCase("MF", "97800")]
		[TestCase("MF", "97805")]
		[TestCase("MF", "97816")]
		[TestCase("MF", "97824")]
		[TestCase("MF", "97838")]
		[TestCase("MF", "97845")]
		[TestCase("MF", "97850")]
		[TestCase("MF", "97868")]
		[TestCase("MF", "97875")]
		[TestCase("MF", "97862")]
		[TestCase("MF", "97882")]
		[TestCase("MF", "97899")]
		// Tests patterns that should be valid for Madagascar (MG).
		[TestCase("MG", "000")]
		[TestCase("MG", "015")]
		[TestCase("MG", "126")]
		[TestCase("MG", "204")]
		[TestCase("MG", "328")]
		[TestCase("MG", "405")]
		[TestCase("MG", "560")]
		[TestCase("MG", "628")]
		[TestCase("MG", "765")]
		[TestCase("MG", "672")]
		[TestCase("MG", "872")]
		[TestCase("MG", "999")]
		// Tests patterns that should be valid for Marshall Islands (MH).
		[TestCase("MH", "96960")]
		[TestCase("MH", "96960")]
		[TestCase("MH", "96961")]
		[TestCase("MH", "96962")]
		[TestCase("MH", "96963")]
		[TestCase("MH", "96964")]
		[TestCase("MH", "96965")]
		[TestCase("MH", "96976")]
		[TestCase("MH", "96977")]
		[TestCase("MH", "96976")]
		[TestCase("MH", "96978")]
		[TestCase("MH", "96979")]
		[TestCase("MH", "96970")]
		[TestCase("MH", "969600000")]
		[TestCase("MH", "969604423")]
		[TestCase("MH", "969612534")]
		[TestCase("MH", "969627700")]
		[TestCase("MH", "969633364")]
		[TestCase("MH", "969648794")]
		[TestCase("MH", "969657364")]
		[TestCase("MH", "969762690")]
		[TestCase("MH", "969771434")]
		[TestCase("MH", "969767855")]
		[TestCase("MH", "969782918")]
		[TestCase("MH", "969799999")]
		[TestCase("MH", "969700000")]
		// Tests patterns that should be valid for Macedonia (MK).
		[TestCase("MK", "0123")]
		[TestCase("MK", "1234")]
		[TestCase("MK", "2000")]
		[TestCase("MK", "3248")]
		[TestCase("MK", "4945")]
		[TestCase("MK", "5640")]
		[TestCase("MK", "6208")]
		[TestCase("MK", "7645")]
		[TestCase("MK", "6752")]
		[TestCase("MK", "8782")]
		[TestCase("MK", "9999")]
		// Tests patterns that should be valid for Myanmar (MM).
		[TestCase("MM", "00000")]
		[TestCase("MM", "01235")]
		[TestCase("MM", "12346")]
		[TestCase("MM", "20004")]
		[TestCase("MM", "32648")]
		[TestCase("MM", "40945")]
		[TestCase("MM", "56640")]
		[TestCase("MM", "62908")]
		[TestCase("MM", "76345")]
		[TestCase("MM", "67552")]
		[TestCase("MM", "87182")]
		[TestCase("MM", "99999")]
		// Tests patterns that should be valid for Mongolia (MN).
		[TestCase("MN", "00000")]
		[TestCase("MN", "01235")]
		[TestCase("MN", "12346")]
		[TestCase("MN", "20004")]
		[TestCase("MN", "32648")]
		[TestCase("MN", "40945")]
		[TestCase("MN", "56640")]
		[TestCase("MN", "62908")]
		[TestCase("MN", "76345")]
		[TestCase("MN", "67552")]
		[TestCase("MN", "87182")]
		[TestCase("MN", "99999")]
		// Tests patterns that should be valid for Northern Mariana Islands (MP).
		[TestCase("MP", "96950")]
		[TestCase("MP", "96951")]
		[TestCase("MP", "96952")]
		[TestCase("MP", "969500000")]
		[TestCase("MP", "969500143")]
		[TestCase("MP", "969501254")]
		[TestCase("MP", "969502070")]
		[TestCase("MP", "969513234")]
		[TestCase("MP", "969514074")]
		[TestCase("MP", "969515634")]
		[TestCase("MP", "969516260")]
		[TestCase("MP", "969527644")]
		[TestCase("MP", "969526785")]
		[TestCase("MP", "969528798")]
		[TestCase("MP", "969529999")]
		// Tests patterns that should be valid for Martinique (MQ).
		[TestCase("MQ", "97200")]
		[TestCase("MQ", "97201")]
		[TestCase("MQ", "97212")]
		[TestCase("MQ", "97220")]
		[TestCase("MQ", "97232")]
		[TestCase("MQ", "97240")]
		[TestCase("MQ", "97256")]
		[TestCase("MQ", "97262")]
		[TestCase("MQ", "97276")]
		[TestCase("MQ", "97267")]
		[TestCase("MQ", "97287")]
		[TestCase("MQ", "97299")]
		// Tests patterns that should be valid for Malta (MT).
		[TestCase("MT", "AAA0000")]
		[TestCase("MT", "ASD0132")]
		[TestCase("MT", "BJR1243")]
		[TestCase("MT", "CDW2004")]
		[TestCase("MT", "DES3247")]
		[TestCase("MT", "EOP4047")]
		[TestCase("MT", "FNK5645")]
		[TestCase("MT", "GFL6208")]
		[TestCase("MT", "HLJ7649")]
		[TestCase("MT", "IDM6750")]
		[TestCase("MT", "JSD8783")]
		[TestCase("MT", "KNJ9999")]
		[TestCase("MT", "LOD0000")]
		[TestCase("MT", "MED0132")]
		[TestCase("MT", "NNR1243")]
		[TestCase("MT", "OLW2004")]
		[TestCase("MT", "PSS3247")]
		[TestCase("MT", "QDP4047")]
		[TestCase("MT", "RNK5645")]
		[TestCase("MT", "SEL6208")]
		[TestCase("MT", "TMJ7649")]
		[TestCase("MT", "UFM6750")]
		[TestCase("MT", "VED8783")]
		[TestCase("MT", "WLJ9999")]
		[TestCase("MT", "XMD0000")]
		[TestCase("MT", "YED0132")]
		[TestCase("MT", "ZLR1243")]
		[TestCase("MT", "ZZZ9999")]
		// Tests patterns that should be valid for Mexico (MX).
		[TestCase("MX", "09302")]
		[TestCase("MX", "10023")]
		[TestCase("MX", "31001")]
		[TestCase("MX", "42007")]
		[TestCase("MX", "54006")]
		[TestCase("MX", "61008")]
		// Tests patterns that should be valid for Malaysia (MY).
		[TestCase("MY", "10023")]
		[TestCase("MY", "31001")]
		[TestCase("MY", "42007")]
		[TestCase("MY", "54006")]
		[TestCase("MY", "61008")]
		// Tests patterns that should be valid for Mozambique (MZ).
		[TestCase("MZ", "0123")]
		[TestCase("MZ", "1234")]
		[TestCase("MZ", "2000")]
		[TestCase("MZ", "3248")]
		[TestCase("MZ", "4945")]
		[TestCase("MZ", "5640")]
		[TestCase("MZ", "6208")]
		[TestCase("MZ", "7645")]
		[TestCase("MZ", "6752")]
		[TestCase("MZ", "8782")]
		[TestCase("MZ", "9999")]
		// Tests patterns that should be valid for Namibia (NA).
		[TestCase("NA", "90000")]
		[TestCase("NA", "90015")]
		[TestCase("NA", "90126")]
		[TestCase("NA", "90204")]
		[TestCase("NA", "91328")]
		[TestCase("NA", "91405")]
		[TestCase("NA", "91560")]
		[TestCase("NA", "91628")]
		[TestCase("NA", "92765")]
		[TestCase("NA", "92672")]
		[TestCase("NA", "92872")]
		[TestCase("NA", "92999")]
		// Tests patterns that should be valid for New Caledonia (NC).
		[TestCase("NC", "98800")]
		[TestCase("NC", "98802")]
		[TestCase("NC", "98813")]
		[TestCase("NC", "98820")]
		[TestCase("NC", "98836")]
		[TestCase("NC", "98884")]
		[TestCase("NC", "98895")]
		[TestCase("NC", "98896")]
		[TestCase("NC", "98897")]
		[TestCase("NC", "98896")]
		[TestCase("NC", "98898")]
		[TestCase("NC", "98899")]
		// Tests patterns that should be valid for Niger (NE).
		[TestCase("NE", "0123")]
		[TestCase("NE", "1234")]
		[TestCase("NE", "2000")]
		[TestCase("NE", "3248")]
		[TestCase("NE", "4945")]
		[TestCase("NE", "5640")]
		[TestCase("NE", "6208")]
		[TestCase("NE", "7645")]
		[TestCase("NE", "6752")]
		[TestCase("NE", "8782")]
		[TestCase("NE", "9999")]
		// Tests patterns that should be valid for Norfolk Island (NF).
		[TestCase("NF", "0123")]
		[TestCase("NF", "1234")]
		[TestCase("NF", "2000")]
		[TestCase("NF", "3248")]
		[TestCase("NF", "4945")]
		[TestCase("NF", "5640")]
		[TestCase("NF", "6208")]
		[TestCase("NF", "7645")]
		[TestCase("NF", "6752")]
		[TestCase("NF", "8782")]
		[TestCase("NF", "9999")]
		// Tests patterns that should be valid for Nigeria (NG).
		[TestCase("NG", "009999")]
		[TestCase("NG", "018010")]
		[TestCase("NG", "110000")]
		[TestCase("NG", "342600")]
		[TestCase("NG", "810185")]
		[TestCase("NG", "810185")]
		// Tests patterns that should be valid for Nicaragua (NI).
		[TestCase("NI", "00000")]
		[TestCase("NI", "01235")]
		[TestCase("NI", "12346")]
		[TestCase("NI", "20004")]
		[TestCase("NI", "32648")]
		[TestCase("NI", "40945")]
		[TestCase("NI", "56640")]
		[TestCase("NI", "62908")]
		[TestCase("NI", "76345")]
		[TestCase("NI", "67552")]
		[TestCase("NI", "87182")]
		[TestCase("NI", "99999")]
		// Tests patterns that should be valid for Netherlands (NL).
		[TestCase("NL", "1236RF")]
		[TestCase("NL", "2044WK")]
		[TestCase("NL", "4075PJ")]
		[TestCase("NL", "5650KF")]
		[TestCase("NL", "6288LS")]
		[TestCase("NL", "7695JD")]
		[TestCase("NL", "6702MO")]
		[TestCase("NL", "8732DF")]
		[TestCase("NL", "9999JS")]
		[TestCase("NL", "2331 PS")]
		// Tests patterns that should be valid for Norway (NO).
		[TestCase("NO", "0912")]
		[TestCase("NO", "0821")]
		[TestCase("NO", "0666")]
		[TestCase("NO", "0000")]
		[TestCase("NO", "1000")]
		[TestCase("NO", "2077")]
		[TestCase("NO", "2650")]
		[TestCase("NO", "4241")]
		// Tests patterns that should be valid for Nepal (NP).
		[TestCase("NP", "00000")]
		[TestCase("NP", "01235")]
		[TestCase("NP", "12346")]
		[TestCase("NP", "20004")]
		[TestCase("NP", "32648")]
		[TestCase("NP", "40945")]
		[TestCase("NP", "56640")]
		[TestCase("NP", "62908")]
		[TestCase("NP", "76345")]
		[TestCase("NP", "67552")]
		[TestCase("NP", "87182")]
		[TestCase("NP", "99999")]
		// Tests patterns that should be valid for New Zealand (NZ).
		[TestCase("NZ", "0912")]
		[TestCase("NZ", "0821")]
		[TestCase("NZ", "0666")]
		[TestCase("NZ", "0000")]
		[TestCase("NZ", "1000")]
		[TestCase("NZ", "2077")]
		[TestCase("NZ", "2650")]
		[TestCase("NZ", "4241")]
		// Tests patterns that should be valid for Oman (OM).
		[TestCase("OM", "000")]
		[TestCase("OM", "015")]
		[TestCase("OM", "126")]
		[TestCase("OM", "204")]
		[TestCase("OM", "328")]
		[TestCase("OM", "405")]
		[TestCase("OM", "560")]
		[TestCase("OM", "628")]
		[TestCase("OM", "765")]
		[TestCase("OM", "672")]
		[TestCase("OM", "872")]
		[TestCase("OM", "999")]
		// Tests patterns that should be valid for Panama (PA).
		[TestCase("PA", "000000")]
		[TestCase("PA", "023145")]
		[TestCase("PA", "134256")]
		[TestCase("PA", "200074")]
		[TestCase("PA", "364238")]
		[TestCase("PA", "494075")]
		[TestCase("PA", "564630")]
		[TestCase("PA", "690268")]
		[TestCase("PA", "734645")]
		[TestCase("PA", "655782")]
		[TestCase("PA", "818792")]
		[TestCase("PA", "999999")]
		// Tests patterns that should be valid for Peru (PE).
		[TestCase("PE", "00000")]
		[TestCase("PE", "01235")]
		[TestCase("PE", "12346")]
		[TestCase("PE", "20004")]
		[TestCase("PE", "32648")]
		[TestCase("PE", "40945")]
		[TestCase("PE", "56640")]
		[TestCase("PE", "62908")]
		[TestCase("PE", "76345")]
		[TestCase("PE", "67552")]
		[TestCase("PE", "87182")]
		[TestCase("PE", "99999")]
		// Tests patterns that should be valid for French Polynesia (PF).
		[TestCase("PF", "98700")]
		[TestCase("PF", "98725")]
		[TestCase("PF", "98736")]
		[TestCase("PF", "98704")]
		[TestCase("PF", "98768")]
		[TestCase("PF", "98795")]
		[TestCase("PF", "98760")]
		[TestCase("PF", "98798")]
		[TestCase("PF", "98735")]
		[TestCase("PF", "98752")]
		[TestCase("PF", "98712")]
		[TestCase("PF", "98799")]
		// Tests patterns that should be valid for Papua New Guinea (PG).
		[TestCase("PG", "000")]
		[TestCase("PG", "015")]
		[TestCase("PG", "126")]
		[TestCase("PG", "204")]
		[TestCase("PG", "328")]
		[TestCase("PG", "405")]
		[TestCase("PG", "560")]
		[TestCase("PG", "628")]
		[TestCase("PG", "765")]
		[TestCase("PG", "672")]
		[TestCase("PG", "872")]
		[TestCase("PG", "999")]
		// Tests patterns that should be valid for Philippines (PH).
		[TestCase("PH", "0123")]
		[TestCase("PH", "1234")]
		[TestCase("PH", "2000")]
		[TestCase("PH", "3248")]
		[TestCase("PH", "4945")]
		[TestCase("PH", "5640")]
		[TestCase("PH", "6208")]
		[TestCase("PH", "7645")]
		[TestCase("PH", "6752")]
		[TestCase("PH", "8782")]
		[TestCase("PH", "9999")]
		// Tests patterns that should be valid for Pakistan (PK).
		[TestCase("PK", "11302")]
		[TestCase("PK", "24023")]
		[TestCase("PK", "45001")]
		[TestCase("PK", "89607")]
		[TestCase("PK", "86096")]
		[TestCase("PK", "85808")]
		// Tests patterns that should be valid for Poland (PL).
		[TestCase("PL", "01302")]
		[TestCase("PL", "11302")]
		[TestCase("PL", "24023")]
		[TestCase("PL", "45001")]
		[TestCase("PL", "89607")]
		[TestCase("PL", "86096")]
		[TestCase("PL", "85808")]
		[TestCase("PL", "06-096")]
		[TestCase("PL", "85-808")]
		// Tests patterns that should be valid for Saint Pierre And Miquelon (PM).
		[TestCase("PM", "97500")]
		// Tests patterns that should be valid for Pitcairn (PN).
		[TestCase("PN", "PCRN1ZZ")]
		// Tests patterns that should be valid for Puerto Rico (PR).
		[TestCase("PR", "01302")]
		[TestCase("PR", "00802")]
		[TestCase("PR", "11302")]
		[TestCase("PR", "24023")]
		[TestCase("PR", "45001")]
		[TestCase("PR", "89607")]
		[TestCase("PR", "86096")]
		[TestCase("PR", "85808")]
		[TestCase("PR", "06096")]
		[TestCase("PR", "85808")]
		// Tests patterns that should be valid for Palestinian Territory (PS).
		[TestCase("PS", "00000")]
		[TestCase("PS", "01235")]
		[TestCase("PS", "12346")]
		[TestCase("PS", "20004")]
		[TestCase("PS", "32648")]
		[TestCase("PS", "40945")]
		[TestCase("PS", "56640")]
		[TestCase("PS", "62908")]
		[TestCase("PS", "76345")]
		[TestCase("PS", "67552")]
		[TestCase("PS", "87182")]
		[TestCase("PS", "99999")]
		// Tests patterns that should be valid for Portugal (PT).
		[TestCase("PT", "1282353")]
		[TestCase("PT", "2037570")]
		[TestCase("PT", "3243436")]
		[TestCase("PT", "4008279")]
		[TestCase("PT", "5697836")]
		[TestCase("PT", "6282469")]
		[TestCase("PT", "7611343")]
		[TestCase("PT", "6767185")]
		[TestCase("PT", "8752391")]
		[TestCase("PT", "9999999")]
		// Tests patterns that should be valid for Palau (PW).
		[TestCase("PW", "96940")]
		// Tests patterns that should be valid for Paraguay (PY).
		[TestCase("PY", "0123")]
		[TestCase("PY", "1234")]
		[TestCase("PY", "2000")]
		[TestCase("PY", "3248")]
		[TestCase("PY", "4945")]
		[TestCase("PY", "5640")]
		[TestCase("PY", "6208")]
		[TestCase("PY", "7645")]
		[TestCase("PY", "6752")]
		[TestCase("PY", "8782")]
		[TestCase("PY", "9999")]
		// Tests patterns that should be valid for Réunion (RE).
		[TestCase("RE", "97400")]
		[TestCase("RE", "97402")]
		[TestCase("RE", "97413")]
		[TestCase("RE", "97420")]
		[TestCase("RE", "97436")]
		[TestCase("RE", "97449")]
		[TestCase("RE", "97456")]
		[TestCase("RE", "97469")]
		[TestCase("RE", "97473")]
		[TestCase("RE", "97465")]
		[TestCase("RE", "97481")]
		[TestCase("RE", "97499")]
		// Tests patterns that should be valid for Romania (RO).
		[TestCase("RO", "018010")]
		[TestCase("RO", "110000")]
		[TestCase("RO", "342600")]
		[TestCase("RO", "810185")]
		[TestCase("RO", "810185")]
		// Tests patterns that should be valid for Serbia (RS).
		[TestCase("RS", "10070")]
		[TestCase("RS", "20767")]
		[TestCase("RS", "26560")]
		[TestCase("RS", "32451")]
		// Tests patterns that should be valid for Russian Federation (RU).
		[TestCase("RU", "110000")]
		[TestCase("RU", "342600")]
		[TestCase("RU", "610185")]
		[TestCase("RU", "410185")]
		// Tests patterns that should be valid for Saudi Arabia (SA).
		[TestCase("SA", "00000")]
		[TestCase("SA", "03145")]
		[TestCase("SA", "14256")]
		[TestCase("SA", "20074")]
		[TestCase("SA", "34238")]
		[TestCase("SA", "44075")]
		[TestCase("SA", "54630")]
		[TestCase("SA", "60268")]
		[TestCase("SA", "74645")]
		[TestCase("SA", "65782")]
		[TestCase("SA", "88792")]
		[TestCase("SA", "99999")]
		[TestCase("SA", "000000000")]
		[TestCase("SA", "031452003")]
		[TestCase("SA", "142563114")]
		[TestCase("SA", "200740220")]
		[TestCase("SA", "342386334")]
		[TestCase("SA", "440759444")]
		[TestCase("SA", "546306554")]
		[TestCase("SA", "602689660")]
		[TestCase("SA", "746453774")]
		[TestCase("SA", "657825665")]
		[TestCase("SA", "887921888")]
		[TestCase("SA", "999999999")]
		// Tests patterns that should be valid for Sudan (SD).
		[TestCase("SD", "00000")]
		[TestCase("SD", "03145")]
		[TestCase("SD", "14256")]
		[TestCase("SD", "20074")]
		[TestCase("SD", "34238")]
		[TestCase("SD", "44075")]
		[TestCase("SD", "54630")]
		[TestCase("SD", "60268")]
		[TestCase("SD", "74645")]
		[TestCase("SD", "65782")]
		[TestCase("SD", "88792")]
		[TestCase("SD", "99999")]
		// Tests patterns that should be valid for Sweden (SE).
		[TestCase("SE", "10000")]
		[TestCase("SE", "10070")]
		[TestCase("SE", "20767")]
		[TestCase("SE", "86560")]
		[TestCase("SE", "32451")]
		[TestCase("SE", "99112")]
		[TestCase("SE", "482 21")]
		[TestCase("SE", "546 36")]
		[TestCase("SE", "650 50")]
		[TestCase("SE", "701 62")]
		[TestCase("SE", "812 71")]
		[TestCase("SE", "926 86")]
		// Tests patterns that should be valid for Singapore (SG).
		[TestCase("SG", "11000")]
		[TestCase("SG", "34600")]
		[TestCase("SG", "61185")]
		[TestCase("SG", "41185")]
		[TestCase("SG", "00999")]
		[TestCase("SG", "01010")]
		[TestCase("SG", "71185")]
		[TestCase("SG", "81185")]
		[TestCase("SG", "91185")]
		// Tests patterns that should be valid for Saint Helena (SH).
		[TestCase("SH", "STHL1ZZ")]
		// Tests patterns that should be valid for Slovenia (SI).
		[TestCase("SI", "0123")]
		[TestCase("SI", "1234")]
		[TestCase("SI", "2000")]
		[TestCase("SI", "3248")]
		[TestCase("SI", "4945")]
		[TestCase("SI", "5640")]
		[TestCase("SI", "6208")]
		[TestCase("SI", "7645")]
		[TestCase("SI", "6752")]
		[TestCase("SI", "8782")]
		[TestCase("SI", "9999")]
		// Tests patterns that should be valid for Slovakia (SK).
		[TestCase("SK", "10070")]
		[TestCase("SK", "20767")]
		[TestCase("SK", "26560")]
		[TestCase("SK", "32451")]
		[TestCase("SK", "09112")]
		[TestCase("SK", "48221")]
		[TestCase("SK", "546 36")]
		[TestCase("SK", "650 50")]
		[TestCase("SK", "701 62")]
		[TestCase("SK", "812 71")]
		[TestCase("SK", "926 86")]
		// Tests patterns that should be valid for San Marino (SM).
		[TestCase("SM", "47890")]
		[TestCase("SM", "47891")]
		[TestCase("SM", "47892")]
		[TestCase("SM", "47895")]
		[TestCase("SM", "47899")]
		// Tests patterns that should be valid for Senegal (SN).
		[TestCase("SN", "00000")]
		[TestCase("SN", "01235")]
		[TestCase("SN", "12346")]
		[TestCase("SN", "20004")]
		[TestCase("SN", "32648")]
		[TestCase("SN", "40945")]
		[TestCase("SN", "56640")]
		[TestCase("SN", "62908")]
		[TestCase("SN", "76345")]
		[TestCase("SN", "67552")]
		[TestCase("SN", "87182")]
		[TestCase("SN", "99999")]
		// Tests patterns that should be valid for El Salvador (SV).
		[TestCase("SV", "01101")]
		// Tests patterns that should be valid for Swaziland (SZ).
		[TestCase("SZ", "H761")]
		[TestCase("SZ", "L000")]
		[TestCase("SZ", "M014")]
		[TestCase("SZ", "S628")]
		[TestCase("SZ", "H611")]
		[TestCase("SZ", "L760")]
		[TestCase("SZ", "M754")]
		[TestCase("SZ", "S998")]
		[TestCase("SZ", "H000")]
		[TestCase("SZ", "L023")]
		[TestCase("SZ", "M182")]
		[TestCase("SZ", "S282")]
		// Tests patterns that should be valid for Turks And Caicos Islands (TC).
		[TestCase("TC", "TKCA1ZZ")]
		// Tests patterns that should be valid for Chad (TD).
		[TestCase("TD", "00000")]
		[TestCase("TD", "01235")]
		[TestCase("TD", "12346")]
		[TestCase("TD", "20004")]
		[TestCase("TD", "32648")]
		[TestCase("TD", "40945")]
		[TestCase("TD", "56640")]
		[TestCase("TD", "62908")]
		[TestCase("TD", "76345")]
		[TestCase("TD", "67552")]
		[TestCase("TD", "87182")]
		[TestCase("TD", "99999")]
		// Tests patterns that should be valid for Thailand (TH).
		[TestCase("TH", "10023")]
		[TestCase("TH", "31001")]
		[TestCase("TH", "42007")]
		[TestCase("TH", "54006")]
		[TestCase("TH", "61008")]
		// Tests patterns that should be valid for Tajikistan (TJ).
		[TestCase("TJ", "000000")]
		[TestCase("TJ", "023145")]
		[TestCase("TJ", "134256")]
		[TestCase("TJ", "200074")]
		[TestCase("TJ", "364238")]
		[TestCase("TJ", "494075")]
		[TestCase("TJ", "564630")]
		[TestCase("TJ", "690268")]
		[TestCase("TJ", "734645")]
		[TestCase("TJ", "655782")]
		[TestCase("TJ", "818792")]
		[TestCase("TJ", "999999")]
		// Tests patterns that should be valid for Turkmenistan (TM).
		[TestCase("TM", "000000")]
		[TestCase("TM", "023145")]
		[TestCase("TM", "134256")]
		[TestCase("TM", "200074")]
		[TestCase("TM", "364238")]
		[TestCase("TM", "494075")]
		[TestCase("TM", "564630")]
		[TestCase("TM", "690268")]
		[TestCase("TM", "734645")]
		[TestCase("TM", "655782")]
		[TestCase("TM", "818792")]
		[TestCase("TM", "999999")]
		// Tests patterns that should be valid for Tunisia (TN).
		[TestCase("TN", "0123")]
		[TestCase("TN", "1234")]
		[TestCase("TN", "2000")]
		[TestCase("TN", "3248")]
		[TestCase("TN", "4945")]
		[TestCase("TN", "5640")]
		[TestCase("TN", "6208")]
		[TestCase("TN", "7645")]
		[TestCase("TN", "6752")]
		[TestCase("TN", "8782")]
		[TestCase("TN", "9999")]
		// Tests patterns that should be valid for Turkey (TR).
		[TestCase("TR", "01302")]
		[TestCase("TR", "08302")]
		[TestCase("TR", "10023")]
		[TestCase("TR", "31001")]
		[TestCase("TR", "42007")]
		[TestCase("TR", "74006")]
		[TestCase("TR", "91008")]
		// Tests patterns that should be valid for Trinidad And Tobago (TT).
		[TestCase("TT", "000000")]
		[TestCase("TT", "023145")]
		[TestCase("TT", "134256")]
		[TestCase("TT", "200074")]
		[TestCase("TT", "364238")]
		[TestCase("TT", "494075")]
		[TestCase("TT", "564630")]
		[TestCase("TT", "690268")]
		[TestCase("TT", "734645")]
		[TestCase("TT", "655782")]
		[TestCase("TT", "818792")]
		[TestCase("TT", "999999")]
		// Tests patterns that should be valid for Taiwan (TW).
		[TestCase("TW", "10023")]
		[TestCase("TW", "31001")]
		[TestCase("TW", "42007")]
		[TestCase("TW", "54006")]
		[TestCase("TW", "61008")]
		[TestCase("TW", "91008")]
		// Tests patterns that should be valid for Ukraine (UA).
		[TestCase("UA", "01235")]
		[TestCase("UA", "12346")]
		[TestCase("UA", "20004")]
		[TestCase("UA", "32648")]
		[TestCase("UA", "40945")]
		[TestCase("UA", "56640")]
		[TestCase("UA", "62908")]
		[TestCase("UA", "76345")]
		[TestCase("UA", "67552")]
		[TestCase("UA", "87182")]
		[TestCase("UA", "99999")]
		// Tests patterns that should be valid for United States (US).
		[TestCase("US", "01000-0060")]
		[TestCase("US", "11000-9996")]
		[TestCase("US", "00126")]
		[TestCase("US", "12345")]
		// Tests patterns that should be valid for Uruguay (UY).
		[TestCase("UY", "00000")]
		[TestCase("UY", "01235")]
		[TestCase("UY", "12346")]
		[TestCase("UY", "20004")]
		[TestCase("UY", "32648")]
		[TestCase("UY", "40945")]
		[TestCase("UY", "56640")]
		[TestCase("UY", "62908")]
		[TestCase("UY", "76345")]
		[TestCase("UY", "67552")]
		[TestCase("UY", "87182")]
		[TestCase("UY", "99999")]
		// Tests patterns that should be valid for Holy See (VA).
		[TestCase("VA", "00120")]
		// Tests patterns that should be valid for Saint Vincent And The Grenadines (VC).
		[TestCase("VC", "0123")]
		[TestCase("VC", "1234")]
		[TestCase("VC", "2000")]
		[TestCase("VC", "3248")]
		[TestCase("VC", "4945")]
		[TestCase("VC", "5640")]
		[TestCase("VC", "6208")]
		[TestCase("VC", "7645")]
		[TestCase("VC", "6752")]
		[TestCase("VC", "8782")]
		[TestCase("VC", "9999")]
		// Tests patterns that should be valid for Venezuela (VE).
		[TestCase("VE", "0000")]
		[TestCase("VE", "0123")]
		[TestCase("VE", "1234")]
		[TestCase("VE", "2000")]
		[TestCase("VE", "3264")]
		[TestCase("VE", "4094")]
		[TestCase("VE", "5664")]
		[TestCase("VE", "6290")]
		[TestCase("VE", "7634")]
		[TestCase("VE", "6755")]
		[TestCase("VE", "8718")]
		[TestCase("VE", "9999")]
		[TestCase("VE", "0000A")]
		[TestCase("VE", "0325A")]
		[TestCase("VE", "1436B")]
		[TestCase("VE", "2044C")]
		[TestCase("VE", "3478D")]
		[TestCase("VE", "4475E")]
		[TestCase("VE", "5450F")]
		[TestCase("VE", "6088G")]
		[TestCase("VE", "7495H")]
		[TestCase("VE", "6502I")]
		[TestCase("VE", "8832J")]
		[TestCase("VE", "9999K")]
		[TestCase("VE", "0000L")]
		[TestCase("VE", "0325M")]
		[TestCase("VE", "1436N")]
		[TestCase("VE", "2044O")]
		[TestCase("VE", "3478P")]
		[TestCase("VE", "4475Q")]
		[TestCase("VE", "5450R")]
		[TestCase("VE", "6088S")]
		[TestCase("VE", "7495T")]
		[TestCase("VE", "6502U")]
		[TestCase("VE", "8832V")]
		[TestCase("VE", "9999W")]
		[TestCase("VE", "0000X")]
		[TestCase("VE", "0325Y")]
		[TestCase("VE", "1436Z")]
		[TestCase("VE", "2044Z")]
		// Tests patterns that should be valid for Virgin Islands (VG).
		[TestCase("VG", "1103")]
		[TestCase("VG", "1114")]
		[TestCase("VG", "1120")]
		[TestCase("VG", "1138")]
		[TestCase("VG", "1145")]
		[TestCase("VG", "1150")]
		[TestCase("VG", "1168")]
		[TestCase("VG", "1135")]
		[TestCase("VG", "1162")]
		[TestCase("VG", "VG1101")]
		[TestCase("VG", "VG1112")]
		[TestCase("VG", "VG1120")]
		[TestCase("VG", "VG1132")]
		[TestCase("VG", "VG1149")]
		[TestCase("VG", "VG1156")]
		[TestCase("VG", "VG1162")]
		[TestCase("VG", "VG1106")]
		[TestCase("VG", "VG1167")]
		// Tests patterns that should be valid for Virgin Islands (VI).
		[TestCase("VI", "00815")]
		[TestCase("VI", "00826")]
		[TestCase("VI", "00837")]
		[TestCase("VI", "00846")]
		[TestCase("VI", "00858")]
		[TestCase("VI", "008152346")]
		[TestCase("VI", "008260004")]
		[TestCase("VI", "008372648")]
		[TestCase("VI", "008460945")]
		[TestCase("VI", "008586640")]
		// Tests patterns that should be valid for Viet Nam (VN).
		[TestCase("VN", "000000")]
		[TestCase("VN", "023145")]
		[TestCase("VN", "134256")]
		[TestCase("VN", "200074")]
		[TestCase("VN", "364238")]
		[TestCase("VN", "494075")]
		[TestCase("VN", "564630")]
		[TestCase("VN", "690268")]
		[TestCase("VN", "734645")]
		[TestCase("VN", "655782")]
		[TestCase("VN", "818792")]
		[TestCase("VN", "999999")]
		// Tests patterns that should be valid for Wallis And Futuna (WF).
		[TestCase("WF", "98600")]
		[TestCase("WF", "98617")]
		[TestCase("WF", "98699")]
		// Tests patterns that should be valid for Mayotte (YT).
		[TestCase("YT", "97600")]
		[TestCase("YT", "97605")]
		[TestCase("YT", "97616")]
		[TestCase("YT", "97624")]
		[TestCase("YT", "97638")]
		[TestCase("YT", "97645")]
		[TestCase("YT", "97650")]
		[TestCase("YT", "97668")]
		[TestCase("YT", "97675")]
		[TestCase("YT", "97662")]
		[TestCase("YT", "97682")]
		[TestCase("YT", "97699")]
		// Tests patterns that should be valid for South Africa (ZA).
		[TestCase("ZA", "0001")]
		[TestCase("ZA", "0023")]
		[TestCase("ZA", "0100")]
		[TestCase("ZA", "1000")]
		[TestCase("ZA", "2077")]
		[TestCase("ZA", "2650")]
		[TestCase("ZA", "4241")]
		// Tests patterns that should be valid for Zambia (ZM).
		[TestCase("ZM", "00000")]
		[TestCase("ZM", "01235")]
		[TestCase("ZM", "12346")]
		[TestCase("ZM", "20004")]
		[TestCase("ZM", "32648")]
		[TestCase("ZM", "40945")]
		[TestCase("ZM", "56640")]
		[TestCase("ZM", "62908")]
		[TestCase("ZM", "76345")]
		[TestCase("ZM", "67552")]
		[TestCase("ZM", "87182")]
		[TestCase("ZM", "99999")]
		public void IsValid(string countrycode, string postalcode)
		{
			var country = Country.Parse(countrycode);
			IsValid(postalcode, country);
		}

		private static void IsValid(string postalcode, Country country)
		{
			Assert.IsTrue(PostalCode.IsValid(postalcode, country), "Postal code '{0}' should be valid for {1:f}.", postalcode, country);
		}

		#endregion

		#region IsNotValid Country tests

		/// <summary>Tests patterns that should not be valid for Andorra (AD).</summary>
		[Test]
		public void IsNotValidCustomCases_AD_All()
		{
			var country = Country.AD;

			IsNotValid("AD890", country);
			IsNotValid("AD901", country);
			IsNotValid("AD012", country);
		}

		/// <summary>Tests patterns that should not be valid for Afghanistan (AF).</summary>
		[Test]
		public void IsNotValidCustomCases_AF_All()
		{
			var country = Country.AF;

			IsNotValid("0077", country);
			IsNotValid("5301", country);
			IsNotValid("6001", country);
			IsNotValid("7023", country);
			IsNotValid("8100", country);
			IsNotValid("9020", country);
			IsNotValid("4441", country);
			IsNotValid("4300", country);
		}

		/// <summary>Tests patterns that should not be valid for Anguilla (AI).</summary>
		[Test]
		public void IsNotValidCustomCases_AI_All()
		{
			var country = Country.AI;

			IsNotValid("9218", country);
			IsNotValid("AI1890", country);
			IsNotValid("AI2901", country);
			IsNotValid("AI2012", country);
		}

		/// <summary>Tests patterns that should not be valid for Albania (AL).</summary>
		[Test]
		public void IsNotValidCustomCases_AL_All()
		{
			var country = Country.AL;

			IsNotValid("0000", country);
			IsNotValid("0125", country);
			IsNotValid("0279", country);
			IsNotValid("0399", country);
			IsNotValid("0418", country);
			IsNotValid("0540", country);
			IsNotValid("0654", country);
			IsNotValid("0790", country);
			IsNotValid("0852", country);
			IsNotValid("0999", country);
		}

		/// <summary>Tests patterns that should not be valid for Armenia (AM).</summary>
		[Test]
		public void IsNotValidCustomCases_AM_All()
		{
			var country = Country.AM;

			IsNotValid("5697", country);
			IsNotValid("6282", country);
			IsNotValid("7611", country);
			IsNotValid("6767", country);
			IsNotValid("8752", country);
			IsNotValid("9999", country);
		}

		/// <summary>Tests patterns that should not be valid for Argentina (AR).</summary>
		[Test]
		public void IsNotValidCustomCases_AR_All()
		{
			var country = Country.AR;

			IsNotValid("A0400XXX", country);
			IsNotValid("S03004DD", country);
		}

		/// <summary>Tests patterns that should not be valid for American Samoa (AS).</summary>
		[Test]
		public void IsNotValidCustomCases_AS_All()
		{
			var country = Country.AS;

			IsNotValid("00000", country);
			IsNotValid("01449", country);
			IsNotValid("12823", country);
			IsNotValid("20375", country);
			IsNotValid("32434", country);
			IsNotValid("40082", country);
			IsNotValid("56978", country);
			IsNotValid("62824", country);
			IsNotValid("76113", country);
			IsNotValid("67671", country);
			IsNotValid("87523", country);
			IsNotValid("00000000", country);
			IsNotValid("01494232", country);
			IsNotValid("12835343", country);
			IsNotValid("20357004", country);
			IsNotValid("32443647", country);
			IsNotValid("40027947", country);
			IsNotValid("56983645", country);
			IsNotValid("62846908", country);
			IsNotValid("76134349", country);
			IsNotValid("67618550", country);
			IsNotValid("87539183", country);
		}

		/// <summary>Tests patterns that should not be valid for Austria (AT).</summary>
		[Test]
		public void IsNotValidCustomCases_AT_All()
		{
			var country = Country.AT;

			IsNotValid("0000", country);
			IsNotValid("0014", country);
			IsNotValid("0128", country);
			IsNotValid("0203", country);
			IsNotValid("0324", country);
			IsNotValid("0400", country);
			IsNotValid("0569", country);
			IsNotValid("0628", country);
			IsNotValid("0761", country);
			IsNotValid("0676", country);
			IsNotValid("0875", country);
			IsNotValid("0999", country);
		}

		/// <summary>Tests patterns that should not be valid for Australia (AU).</summary>
		[Test]
		public void IsNotValidCustomCases_AU_All()
		{
			var country = Country.AU;

			IsNotValid("0000", country);
			IsNotValid("0014", country);
			IsNotValid("0128", country);
			IsNotValid("0203", country);
			IsNotValid("0324", country);
			IsNotValid("0400", country);
			IsNotValid("0569", country);
			IsNotValid("0628", country);
			IsNotValid("0761", country);
		}

		/// <summary>Tests patterns that should not be valid for Åland Islands (AX).</summary>
		[Test]
		public void IsNotValidCustomCases_AX_All()
		{
			var country = Country.AX;

			IsNotValid("0000", country);
			IsNotValid("0144", country);
			IsNotValid("1282", country);
			IsNotValid("2037", country);
			IsNotValid("2000", country);
			IsNotValid("2014", country);
			IsNotValid("2122", country);
			IsNotValid("2323", country);
			IsNotValid("2408", country);
			IsNotValid("2567", country);
			IsNotValid("2622", country);
			IsNotValid("2761", country);
			IsNotValid("2677", country);
			IsNotValid("2872", country);
			IsNotValid("2999", country);
			IsNotValid("0144", country);
			IsNotValid("1282", country);
			IsNotValid("2037", country);
			IsNotValid("3243", country);
			IsNotValid("4008", country);
			IsNotValid("5697", country);
			IsNotValid("6282", country);
			IsNotValid("7611", country);
			IsNotValid("6767", country);
			IsNotValid("8752", country);
			IsNotValid("9999", country);
		}

		/// <summary>Tests patterns that should not be valid for Azerbaijan (AZ).</summary>
		[Test]
		public void IsNotValidCustomCases_AZ_All()
		{
			var country = Country.AZ;

			IsNotValid("DK 6990", country);
			IsNotValid("GL3990", country);
			IsNotValid("FO1990", country);
			IsNotValid("FO990", country);
		}

		/// <summary>Tests patterns that should not be valid for Bosnia And Herzegovina (BA).</summary>
		[Test]
		public void IsNotValidCustomCases_BA_All()
		{
			var country = Country.BA;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Barbados (BB).</summary>
		[Test]
		public void IsNotValidCustomCases_BB_All()
		{
			var country = Country.BB;

			IsNotValid("AA00000", country);
			IsNotValid("AS01449", country);
			IsNotValid("BJ12823", country);
			IsNotValid("CD20375", country);
			IsNotValid("DE32434", country);
			IsNotValid("EO40082", country);
			IsNotValid("FN56978", country);
			IsNotValid("GF62824", country);
			IsNotValid("HL76113", country);
			IsNotValid("ID67671", country);
			IsNotValid("JS87523", country);
			IsNotValid("KN99999", country);
			IsNotValid("LO00000", country);
			IsNotValid("ME01449", country);
			IsNotValid("NN12823", country);
			IsNotValid("OL20375", country);
			IsNotValid("PS32434", country);
			IsNotValid("QD40082", country);
			IsNotValid("RN56978", country);
			IsNotValid("SE62824", country);
			IsNotValid("TM76113", country);
			IsNotValid("UF67671", country);
			IsNotValid("VE87523", country);
			IsNotValid("WL99999", country);
			IsNotValid("XM00000", country);
			IsNotValid("YE01449", country);
			IsNotValid("ZL12823", country);
			IsNotValid("ZZ20375", country);
		}

		/// <summary>Tests patterns that should not be valid for Bangladesh (BD).</summary>
		[Test]
		public void IsNotValidCustomCases_BD_All()
		{
			var country = Country.BD;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Belgium (BE).</summary>
		[Test]
		public void IsNotValidCustomCases_BE_All()
		{
			var country = Country.BE;

			IsNotValid("0000", country);
			IsNotValid("0014", country);
			IsNotValid("0128", country);
			IsNotValid("0203", country);
			IsNotValid("0324", country);
			IsNotValid("0400", country);
			IsNotValid("0569", country);
			IsNotValid("0628", country);
			IsNotValid("0761", country);
			IsNotValid("0676", country);
			IsNotValid("0875", country);
			IsNotValid("0999", country);
		}

		/// <summary>Tests patterns that should not be valid for Bulgaria (BG).</summary>
		[Test]
		public void IsNotValidCustomCases_BG_All()
		{
			var country = Country.BG;

			IsNotValid("0000", country);
			IsNotValid("0014", country);
			IsNotValid("0128", country);
			IsNotValid("0203", country);
			IsNotValid("0324", country);
			IsNotValid("0400", country);
			IsNotValid("0569", country);
			IsNotValid("0628", country);
			IsNotValid("0761", country);
			IsNotValid("0676", country);
			IsNotValid("0875", country);
			IsNotValid("0999", country);
		}

		/// <summary>Tests patterns that should not be valid for Bahrain (BH).</summary>
		[Test]
		public void IsNotValidCustomCases_BH_All()
		{
			var country = Country.BH;

			IsNotValid("000", country);
			IsNotValid("014", country);
			IsNotValid("1328", country);
			IsNotValid("2037", country);
			IsNotValid("3243", country);
			IsNotValid("4008", country);
			IsNotValid("5697", country);
			IsNotValid("6282", country);
			IsNotValid("7611", country);
			IsNotValid("6767", country);
			IsNotValid("8752", country);
			IsNotValid("9999", country);
		}

		/// <summary>Tests patterns that should not be valid for Saint Barthélemy (BL).</summary>
		[Test]
		public void IsNotValidCustomCases_BL_All()
		{
			var country = Country.BL;

			IsNotValid("00000", country);
			IsNotValid("01235", country);
			IsNotValid("12346", country);
			IsNotValid("20004", country);
			IsNotValid("32648", country);
			IsNotValid("40945", country);
			IsNotValid("56640", country);
			IsNotValid("62908", country);
			IsNotValid("76345", country);
			IsNotValid("67552", country);
			IsNotValid("87182", country);
			IsNotValid("99999", country);
		}

		/// <summary>Tests patterns that should not be valid for Bermuda (BM).</summary>
		[Test]
		public void IsNotValidCustomCases_BM_All()
		{
			var country = Country.BM;

			IsNotValid("A0", country);
			IsNotValid("A1", country);
			IsNotValid("B2", country);
			IsNotValid("C0", country);
			IsNotValid("D2", country);
			IsNotValid("E0", country);
			IsNotValid("6F", country);
			IsNotValid("2G", country);
			IsNotValid("6H", country);
			IsNotValid("7I", country);
			IsNotValid("7J", country);
			IsNotValid("9K", country);
			IsNotValid("0L", country);
			IsNotValid("013S", country);
			IsNotValid("12RF", country);
			IsNotValid("2034", country);
			IsNotValid("2A34", country);
		}

		/// <summary>Tests patterns that should not be valid for Brunei Darussalam (BN).</summary>
		[Test]
		public void IsNotValidCustomCases_BN_All()
		{
			var country = Country.BN;

			IsNotValid("0000DF", country);
			IsNotValid("2325DS", country);
			IsNotValid("3436RF", country);
			IsNotValid("0044WK", country);
			IsNotValid("6478SD", country);
			IsNotValid("9475PJ", country);
			IsNotValid("6450KF", country);
			IsNotValid("9088LS", country);
			IsNotValid("3495JD", country);
			IsNotValid("5502MO", country);
			IsNotValid("1832DF", country);
			IsNotValid("K999JS", country);
			IsNotValid("L000DF", country);
			IsNotValid("M325DS", country);
			IsNotValid("N436RF", country);
			IsNotValid("O044WK", country);
			IsNotValid("P478SD", country);
			IsNotValid("Q475PJ", country);
			IsNotValid("RN578F", country);
			IsNotValid("SE624S", country);
			IsNotValid("TM713D", country);
			IsNotValid("UF671O", country);
			IsNotValid("VE823F", country);
			IsNotValid("WL999S", country);
			IsNotValid("XMD000", country);
			IsNotValid("YED014", country);
			IsNotValid("ZLR128", country);
			IsNotValid("ZZW203", country);
		}

		/// <summary>Tests patterns that should not be valid for Bolivia (BO).</summary>
		[Test]
		public void IsNotValidCustomCases_BO_All()
		{
			var country = Country.BO;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Brazil (BR).</summary>
		[Test]
		public void IsNotValidCustomCases_BR_All()
		{
			var country = Country.BR;

			IsNotValid("00000000", country);
			IsNotValid("00014494", country);
			IsNotValid("00128235", country);
			IsNotValid("00203757", country);
			IsNotValid("00324343", country);
			IsNotValid("00400827", country);
			IsNotValid("00569783", country);
			IsNotValid("00628246", country);
			IsNotValid("00761134", country);
			IsNotValid("00676718", country);
			IsNotValid("00875239", country);
			IsNotValid("00999999", country);
		}

		/// <summary>Tests patterns that should not be valid for Bhutan (BT).</summary>
		[Test]
		public void IsNotValidCustomCases_BT_All()
		{
			var country = Country.BT;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Belarus (BY).</summary>
		[Test]
		public void IsNotValidCustomCases_BY_All()
		{
			var country = Country.BY;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Canada (CA).</summary>
		[Test]
		public void IsNotValidCustomCases_CA_All()
		{
			var country = Country.CA;

			// No D, F, I, O, Q, and U (and W and Z to start with).

			IsNotValid("H0D0H0", country);
			IsNotValid("A0F0D0", country);
			IsNotValid("A0I1D4", country);
			IsNotValid("B1O2R8", country);
			IsNotValid("C2Q0W3", country);
			IsNotValid("D3U2S4", country);
			IsNotValid("E4O0D0", country);
			IsNotValid("F5N6F9", country);
			IsNotValid("G6F2I8", country);
			IsNotValid("D7L6O1", country);
			IsNotValid("F6D7Q6", country);
			IsNotValid("I8S7U5", country);
			IsNotValid("O9N9J9", country);
			IsNotValid("Q0O0D0", country);
			IsNotValid("U0E1D4", country);
			IsNotValid("W1N2R8", country);
			IsNotValid("Z2L0W3", country);
		}

		/// <summary>Tests patterns that should not be valid for Cocos (CC).</summary>
		[Test]
		public void IsNotValidCustomCases_CC_All()
		{
			var country = Country.CC;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Switzerland (CH).</summary>
		[Test]
		public void IsNotValidCustomCases_CH_All()
		{
			var country = Country.CH;

			IsNotValid("0000", country);
			IsNotValid("0014", country);
			IsNotValid("0128", country);
			IsNotValid("0203", country);
			IsNotValid("0324", country);
			IsNotValid("0400", country);
			IsNotValid("0569", country);
			IsNotValid("0628", country);
			IsNotValid("0761", country);
			IsNotValid("0676", country);
			IsNotValid("0875", country);
			IsNotValid("0999", country);
		}

		/// <summary>Tests patterns that should not be valid for Chile (CL).</summary>
		[Test]
		public void IsNotValidCustomCases_CL_All()
		{
			var country = Country.CL;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for China (CN).</summary>
		[Test]
		public void IsNotValidCustomCases_CN_All()
		{
			var country = Country.CN;

			IsNotValid("0000", country);
			IsNotValid("0004", country);
			IsNotValid("0018", country);
			IsNotValid("0023", country);
			IsNotValid("0034", country);
			IsNotValid("0040", country);
			IsNotValid("0059", country);
			IsNotValid("0068", country);
			IsNotValid("0071", country);
			IsNotValid("0066", country);
			IsNotValid("0085", country);
			IsNotValid("0099", country);
		}

		/// <summary>Tests patterns that should not be valid for Colombia (CO).</summary>
		[Test]
		public void IsNotValidCustomCases_CO_All()
		{
			var country = Country.CO;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Costa Rica (CR).</summary>
		[Test]
		public void IsNotValidCustomCases_CR_All()
		{
			var country = Country.CR;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Cuba (CU).</summary>
		[Test]
		public void IsNotValidCustomCases_CU_All()
		{
			var country = Country.CU;

			IsNotValid("CP000", country);
			IsNotValid("CP035", country);
			IsNotValid("CP146", country);
			IsNotValid("CP204", country);
			IsNotValid("CP348", country);
		}

		/// <summary>Tests patterns that should not be valid for Cape Verde (CV).</summary>
		[Test]
		public void IsNotValidCustomCases_CV_All()
		{
			var country = Country.CV;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Christmas Island (CX).</summary>
		[Test]
		public void IsNotValidCustomCases_CX_All()
		{
			var country = Country.CX;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Cyprus (CY).</summary>
		[Test]
		public void IsNotValidCustomCases_CY_All()
		{
			var country = Country.CY;

			IsNotValid("0000", country);
			IsNotValid("0014", country);
			IsNotValid("0128", country);
			IsNotValid("0203", country);
			IsNotValid("0324", country);
			IsNotValid("0400", country);
			IsNotValid("0569", country);
			IsNotValid("0628", country);
			IsNotValid("0761", country);
			IsNotValid("0676", country);
			IsNotValid("0875", country);
			IsNotValid("0999", country);
		}

		/// <summary>Tests patterns that should not be valid for Czech Republic (CZ).</summary>
		[Test]
		public void IsNotValidCustomCases_CZ_All()
		{
			var country = Country.CZ;

			IsNotValid("0000", country);
			IsNotValid("0014", country);
			IsNotValid("0128", country);
			IsNotValid("0203", country);
			IsNotValid("0324", country);
			IsNotValid("0400", country);
			IsNotValid("0569", country);
			IsNotValid("0628", country);
			IsNotValid("0761", country);
			IsNotValid("0676", country);
			IsNotValid("0875", country);
			IsNotValid("0999", country);

			IsNotValid("8000", country);
			IsNotValid("8004", country);
			IsNotValid("8018", country);
			IsNotValid("8023", country);
			IsNotValid("8034", country);
			IsNotValid("8040", country);
			IsNotValid("9059", country);
			IsNotValid("9068", country);
			IsNotValid("9071", country);
			IsNotValid("9066", country);
			IsNotValid("9085", country);
			IsNotValid("9099", country);
		}

		/// <summary>Tests patterns that should not be valid for Germany (DE).</summary>
		[Test]
		public void IsNotValidCustomCases_DE_All()
		{
			var country = Country.DE;

			IsNotValid("00007", country);
			IsNotValid("00043", country);
			IsNotValid("00188", country);
			IsNotValid("00237", country);
			IsNotValid("00342", country);
			IsNotValid("00401", country);
			IsNotValid("00597", country);
			IsNotValid("00682", country);
			IsNotValid("00719", country);
			IsNotValid("00665", country);
			IsNotValid("00853", country);
			IsNotValid("00999", country);
		}

		/// <summary>Tests patterns that should not be valid for Denmark (DK).</summary>
		[Test]
		public void IsNotValidCustomCases_DK_All()
		{
			var country = Country.DK;

			IsNotValid("0000", country);
			IsNotValid("0014", country);
			IsNotValid("0128", country);
			IsNotValid("0203", country);
			IsNotValid("0324", country);
			IsNotValid("0400", country);
			IsNotValid("0569", country);
			IsNotValid("0628", country);
			IsNotValid("0761", country);
			IsNotValid("0676", country);
			IsNotValid("0875", country);
			IsNotValid("0999", country);

			IsNotValid("DK0000", country);
			IsNotValid("DK0014", country);
			IsNotValid("DK0128", country);
			IsNotValid("DK0203", country);
			IsNotValid("DK0324", country);
			IsNotValid("DK0400", country);
			IsNotValid("DK0569", country);
			IsNotValid("DK0628", country);
			IsNotValid("DK0761", country);
			IsNotValid("DK0676", country);
			IsNotValid("DK0875", country);
			IsNotValid("DK0999", country);

			IsNotValid("AA0000", country);
			IsNotValid("AS0144", country);
			IsNotValid("BJ1282", country);
			IsNotValid("CD2037", country);
			IsNotValid("DE3243", country);
			IsNotValid("EO4008", country);
			IsNotValid("FN5697", country);
			IsNotValid("GF6282", country);
			IsNotValid("HL7611", country);
			IsNotValid("ID6767", country);
			IsNotValid("JS8752", country);
			IsNotValid("KN9999", country);
			IsNotValid("LO0000", country);
			IsNotValid("ME0144", country);
			IsNotValid("NN1282", country);
		}

		/// <summary>Tests patterns that should not be valid for Algeria (DZ).</summary>
		[Test]
		public void IsNotValidCustomCases_DZ_All()
		{
			var country = Country.DZ;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Ecuador (EC).</summary>
		[Test]
		public void IsNotValidCustomCases_EC_All()
		{
			var country = Country.EC;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Estonia (EE).</summary>
		[Test]
		public void IsNotValidCustomCases_EE_All()
		{
			var country = Country.EE;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Egypt (EG).</summary>
		[Test]
		public void IsNotValidCustomCases_EG_All()
		{
			var country = Country.EG;

			IsNotValid("00000", country);
			IsNotValid("01235", country);
			//IsValid("12346", country);
			//IsValid("20004", country);
			//IsValid("32648", country);
			//IsValid("40945", country);
			//IsValid("56640", country);
			//IsValid("62908", country);
			//IsValid("76345", country);
			//IsValid("67552", country);
			//IsValid("87182", country);
			//IsValid("99999", country);
		}

		/// <summary>Tests patterns that should not be valid for Spain (ES).</summary>
		[Test]
		public void IsNotValidCustomCases_ES_All()
		{
			var country = Country.ES;

			IsNotValid("00000", country);
			IsNotValid("53434", country);
			IsNotValid("54082", country);
			IsNotValid("55978", country);
			IsNotValid("56824", country);
			IsNotValid("57113", country);
			IsNotValid("56671", country);
			IsNotValid("58523", country);
			IsNotValid("59999", country);
			IsNotValid("62824", country);
			IsNotValid("76113", country);
			IsNotValid("67671", country);
			IsNotValid("87523", country);
			IsNotValid("99999", country);
		}

		/// <summary>Tests patterns that should not be valid for Ethiopia (ET).</summary>
		[Test]
		public void IsNotValidCustomCases_ET_All()
		{
			var country = Country.ET;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Finland (FI).</summary>
		[Test]
		public void IsNotValidCustomCases_FI_All()
		{
			var country = Country.FI;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Falkland Islands (FK).</summary>
		[Test]
		public void IsNotValidCustomCases_FK_All()
		{
			var country = Country.FK;

			IsNotValid("FIAA1ZZ", country);
			IsNotValid("FIAA9ZZ", country);
			IsNotValid("DN551PT", country);
			IsNotValid("DN551PT", country);
			IsNotValid("EC1A1BB", country);
			IsNotValid("EC1A1BB", country);

		}

		/// <summary>Tests patterns that should not be valid for Micronesia (FM).</summary>
		[Test]
		public void IsNotValidCustomCases_FM_All()
		{
			var country = Country.FM;

			IsNotValid("00000", country);
			IsNotValid("01234", country);
			IsNotValid("01449", country);
			IsNotValid("12823", country);
			IsNotValid("20375", country);
			IsNotValid("32434", country);
			IsNotValid("40082", country);
			IsNotValid("56978", country);
			IsNotValid("62824", country);
			IsNotValid("76113", country);
			IsNotValid("67671", country);
			IsNotValid("87523", country);
			IsNotValid("99999", country);

			IsNotValid("000000000", country);
			IsNotValid("012345678", country);
			IsNotValid("014494232", country);
			IsNotValid("128235343", country);
			IsNotValid("203757004", country);
			IsNotValid("324343647", country);
			IsNotValid("400827947", country);
			IsNotValid("569783645", country);
			IsNotValid("628246908", country);
			IsNotValid("761134349", country);
			IsNotValid("676718550", country);
			IsNotValid("875239183", country);
			IsNotValid("999999999", country);
		}

		/// <summary>Tests patterns that should not be valid for Faroe Islands (FO).</summary>
		[Test]
		public void IsNotValidCustomCases_FO_All()
		{
			var country = Country.FO;

			IsNotValid("000", country);
			IsNotValid("004", country);
			IsNotValid("018", country);
			IsNotValid("023", country);
			IsNotValid("034", country);
			IsNotValid("040", country);
			IsNotValid("059", country);
			IsNotValid("068", country);
			IsNotValid("071", country);
			IsNotValid("066", country);
			IsNotValid("085", country);
			IsNotValid("099", country);

			IsNotValid("FO000", country);
			IsNotValid("FO004", country);
			IsNotValid("FO018", country);
			IsNotValid("FO023", country);
			IsNotValid("FO034", country);
			IsNotValid("FO040", country);
			IsNotValid("FO059", country);
			IsNotValid("FO068", country);
			IsNotValid("FO071", country);
			IsNotValid("FO066", country);
			IsNotValid("FO085", country);
			IsNotValid("FO099", country);

			IsNotValid("AA000", country);
			IsNotValid("AS044", country);
			IsNotValid("BJ182", country);
			IsNotValid("CD237", country);
			IsNotValid("DE343", country);
			IsNotValid("EO408", country);
			IsNotValid("FN597", country);
			IsNotValid("GF682", country);
			IsNotValid("HL711", country);
			IsNotValid("ID667", country);
			IsNotValid("JS852", country);
			IsNotValid("KN999", country);
			IsNotValid("LO000", country);
			IsNotValid("ME044", country);
			IsNotValid("NN182", country);
		}

		/// <summary>Tests patterns that should not be valid for France (FR).</summary>
		[Test]
		public void IsNotValidCustomCases_FR_All()
		{
			var country = Country.FR;

			IsNotValid("00007", country);
			IsNotValid("00043", country);
			IsNotValid("00188", country);
			IsNotValid("00237", country);
			IsNotValid("00342", country);
			IsNotValid("00401", country);
			IsNotValid("00597", country);
			IsNotValid("00682", country);
			IsNotValid("00719", country);
			IsNotValid("00665", country);
			IsNotValid("00853", country);
			IsNotValid("00999", country);
		}

		/// <summary>Tests patterns that should not be valid for Gabon (GA).</summary>
		[Test]
		public void IsNotValidCustomCases_GA_All()
		{
			var country = Country.GA;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for United Kingdom (GB).</summary>
		[Test]
		public void IsNotValidCustomCases_GB_All()
		{
			var country = Country.GB;

			IsNotValid("311AA", country);
			IsNotValid("M113A", country);
			IsNotValid("M11A8", country);
			IsNotValid("M1BAA", country);

			IsNotValid("1338TH", country);
			IsNotValid("B338B9", country);

			IsNotValid("CRABXH", country);
			IsNotValid("CR26X9", country);

			IsNotValid("DN55PPT", country);
			IsNotValid("D1551PT", country);

			IsNotValid("WWA1HQ", country);
			IsNotValid("W1A123", country);

			IsNotValid("EC1A112", country);
			IsNotValid("EC1816B", country);
		}

		/// <summary>Tests patterns that should not be valid for Georgia (GE).</summary>
		[Test]
		public void IsNotValidCustomCases_GE_All()
		{
			var country = Country.GE;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for French Guiana (GF).</summary>
		[Test]
		public void IsNotValidCustomCases_GF_All()
		{
			var country = Country.GF;

			IsNotValid("00000", country);
			IsNotValid("01235", country);
			IsNotValid("12346", country);
			IsNotValid("20004", country);
			IsNotValid("32648", country);
			IsNotValid("40945", country);
			IsNotValid("56640", country);
			IsNotValid("62908", country);
			IsNotValid("76345", country);
			IsNotValid("67552", country);
			IsNotValid("87182", country);
			IsNotValid("99999", country);

		}

		/// <summary>Tests patterns that should not be valid for Guernsey (GG).</summary>
		[Test]
		public void IsNotValidCustomCases_GG_All()
		{
			var country = Country.GG;

			IsNotValid("336R", country);
			IsNotValid("044W", country);
			IsNotValid("678S", country);
			IsNotValid("975P", country);
			IsNotValid("650K", country);
			IsNotValid("988L", country);
			IsNotValid("395J", country);

			IsNotValid("5502M", country);
			IsNotValid("1832D", country);
			IsNotValid("9999J", country);
			IsNotValid("0000D", country);
			IsNotValid("2325D", country);
			IsNotValid("3436R", country);
			IsNotValid("0044W", country);

			IsNotValid("GG336R", country);
			IsNotValid("GG044W", country);
			IsNotValid("GG678S", country);
			IsNotValid("GG975P", country);
			IsNotValid("GG650K", country);
			IsNotValid("GG988L", country);
			IsNotValid("GG395J", country);

			IsNotValid("GG5502M", country);
			IsNotValid("GG1832D", country);
			IsNotValid("GG9999J", country);
			IsNotValid("GG0000D", country);
			IsNotValid("GG2325D", country);
			IsNotValid("GG3436R", country);
			IsNotValid("GG0044W", country);

			IsNotValid("GF88LS", country);
			IsNotValid("HL95JD", country);
			IsNotValid("ID02MO", country);
			IsNotValid("JS32DF", country);
			IsNotValid("KN99JS", country);
			IsNotValid("LO00DF", country);
			IsNotValid("ME25DS", country);

			IsNotValid("AA000DF", country);
			IsNotValid("AS325DS", country);
			IsNotValid("BJ436RF", country);
			IsNotValid("CD044WK", country);
			IsNotValid("DE478SD", country);
			IsNotValid("EO475PJ", country);
			IsNotValid("FN450KF", country);
		}

		/// <summary>Tests patterns that should not be valid for Gibraltar (GI).</summary>
		[Test]
		public void IsNotValidCustomCases_GI_All()
		{
			var country = Country.GI;

			IsNotValid("DN123AA", country);
			IsNotValid("GX123BB", country);
		}

		/// <summary>Tests patterns that should not be valid for Greenland (GL).</summary>
		[Test]
		public void IsNotValidCustomCases_GL_All()
		{
			var country = Country.GL;

			IsNotValid("5502", country);
			IsNotValid("1832", country);
			IsNotValid("9999", country);
			IsNotValid("0000", country);
			IsNotValid("2325", country);
			IsNotValid("3136", country);
			IsNotValid("3236", country);
			IsNotValid("3436", country);
			IsNotValid("3436", country);
			IsNotValid("3567", country);
			IsNotValid("0044", country);

			IsNotValid("GL3365", country);
			IsNotValid("GL0448", country);
			IsNotValid("GL6789", country);
			IsNotValid("GL9750", country);
			IsNotValid("GL6503", country);
			IsNotValid("GL9881", country);
			IsNotValid("GL3852", country);

			IsNotValid("AA3900", country);
			IsNotValid("AS3935", country);
			IsNotValid("BJ3946", country);
			IsNotValid("CD3904", country);
			IsNotValid("DE3948", country);
			IsNotValid("EO3945", country);
			IsNotValid("FN3940", country);
		}

		/// <summary>Tests patterns that should not be valid for Guadeloupe (GP).</summary>
		[Test]
		public void IsNotValidCustomCases_GP_All()
		{
			var country = Country.GP;

			IsNotValid("00000", country);
			IsNotValid("01235", country);
			IsNotValid("12346", country);
			IsNotValid("20004", country);
			IsNotValid("32648", country);
			IsNotValid("40945", country);
			IsNotValid("56640", country);
			IsNotValid("62908", country);
			IsNotValid("76345", country);
			IsNotValid("67552", country);
			IsNotValid("87182", country);
			IsNotValid("99999", country);
		}

		/// <summary>Tests patterns that should not be valid for Greece (GR).</summary>
		[Test]
		public void IsNotValidCustomCases_GR_All()
		{
			var country = Country.GR;

			IsNotValid("00072", country);
			IsNotValid("00433", country);
			IsNotValid("01885", country);
			IsNotValid("02374", country);
			IsNotValid("03421", country);
			IsNotValid("04014", country);
			IsNotValid("05957", country);
			IsNotValid("06862", country);
			IsNotValid("07159", country);
			IsNotValid("06685", country);
			IsNotValid("08593", country);
			IsNotValid("09999", country);
		}

		/// <summary>Tests patterns that should not be valid for South Georgia And The South Sandwich Islands (GS).</summary>
		[Test]
		public void IsNotValidCustomCases_GS_All()
		{
			var country = Country.GS;

			IsNotValid("SIQQ1AZ", country);
			IsNotValid("SIQQ9ZZ", country);
			IsNotValid("SIBB1ZZ", country);
			IsNotValid("DN551PT", country);
			IsNotValid("DN551PT", country);
			IsNotValid("EC1A1BB", country);
			IsNotValid("EC1A1BB", country);
		}

		/// <summary>Tests patterns that should not be valid for Guatemala (GT).</summary>
		[Test]
		public void IsNotValidCustomCases_GT_All()
		{
			var country = Country.GT;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Guam (GU).</summary>
		[Test]
		public void IsNotValidCustomCases_GU_All()
		{
			var country = Country.GU;

			IsNotValid("00000", country);
			IsNotValid("01235", country);
			IsNotValid("12346", country);
			IsNotValid("20004", country);
			IsNotValid("32648", country);
			IsNotValid("40945", country);
			IsNotValid("56640", country);
			IsNotValid("62908", country);
			IsNotValid("76345", country);
			IsNotValid("67552", country);
			IsNotValid("87182", country);
			IsNotValid("96908", country);
			IsNotValid("96938", country);
			IsNotValid("99999", country);

			IsNotValid("000000000", country);
			IsNotValid("012301235", country);
			IsNotValid("123412346", country);
			IsNotValid("200020004", country);
			IsNotValid("326432648", country);
			IsNotValid("409440945", country);
			IsNotValid("566456640", country);
			IsNotValid("629062908", country);
			IsNotValid("763476345", country);
			IsNotValid("675567552", country);
			IsNotValid("871887182", country);
			IsNotValid("969087182", country);
			IsNotValid("969387182", country);
			IsNotValid("999999999", country);
		}

		/// <summary>Tests patterns that should not be valid for Guinea-Bissau (GW).</summary>
		[Test]
		public void IsNotValidCustomCases_GW_All()
		{
			var country = Country.GW;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Heard Island And Mcdonald Islands (HM).</summary>
		[Test]
		public void IsNotValidCustomCases_HM_All()
		{
			var country = Country.HM;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Honduras (HN).</summary>
		[Test]
		public void IsNotValidCustomCases_HN_All()
		{
			var country = Country.HN;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Croatia (HR).</summary>
		[Test]
		public void IsNotValidCustomCases_HR_All()
		{
			var country = Country.HR;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Haiti (HT).</summary>
		[Test]
		public void IsNotValidCustomCases_HT_All()
		{
			var country = Country.HT;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Hungary (HU).</summary>
		[Test]
		public void IsNotValidCustomCases_HU_All()
		{
			var country = Country.HU;

			IsNotValid("0007", country);
			IsNotValid("0043", country);
			IsNotValid("0188", country);
			IsNotValid("0237", country);
			IsNotValid("0342", country);
			IsNotValid("0401", country);
			IsNotValid("0595", country);
			IsNotValid("0686", country);
			IsNotValid("0715", country);
			IsNotValid("0668", country);
			IsNotValid("0859", country);
			IsNotValid("0999", country);
		}

		/// <summary>Tests patterns that should not be valid for Indonesia (ID).</summary>
		[Test]
		public void IsNotValidCustomCases_ID_All()
		{
			var country = Country.ID;

			IsNotValid("00072", country);
			IsNotValid("00433", country);
			IsNotValid("01885", country);
			IsNotValid("02374", country);
			IsNotValid("03421", country);
			IsNotValid("04014", country);
			IsNotValid("05957", country);
			IsNotValid("06862", country);
			IsNotValid("07159", country);
			IsNotValid("06685", country);
			IsNotValid("08593", country);
			IsNotValid("09999", country);
		}

		/// <summary>Tests patterns that should not be valid for Israel (IL).</summary>
		[Test]
		public void IsNotValidCustomCases_IL_All()
		{
			var country = Country.IL;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Isle Of Man (IM).</summary>
		[Test]
		public void IsNotValidCustomCases_IM_All()
		{
			var country = Country.IM;

			IsNotValid("IMAA00DF", country);
			IsNotValid("IMAS04DS", country);
			IsNotValid("IMBJ18RF", country);
			IsNotValid("IMCD23WK", country);
			IsNotValid("IMDE34SD", country);
			IsNotValid("IMEO40PJ", country);
			IsNotValid("IMFN59KF", country);
			IsNotValid("IMGF68LS", country);
			IsNotValid("IMHL71JD", country);
			IsNotValid("IMID66MO", country);
			IsNotValid("IMJS85DF", country);
			IsNotValid("IMKN99JS", country);
			IsNotValid("IMLO00DF", country);

			IsNotValid("AA000DF", country);
			IsNotValid("AS014DS", country);
			IsNotValid("BJ128RF", country);
			IsNotValid("CD203WK", country);
			IsNotValid("DE324SD", country);
			IsNotValid("EO400PJ", country);
			IsNotValid("FN569KF", country);
			IsNotValid("GF628LS", country);
			IsNotValid("HL761JD", country);
			IsNotValid("ID676MO", country);
			IsNotValid("JS875DF", country);
			IsNotValid("KN999JS", country);
			IsNotValid("LO000DF", country);
		}

		/// <summary>Tests patterns that should not be valid for India (IN).</summary>
		[Test]
		public void IsNotValidCustomCases_IN_All()
		{
			var country = Country.IN;

			IsNotValid("00000", country);
			IsNotValid("00149", country);
			IsNotValid("01283", country);
			IsNotValid("02035", country);
			IsNotValid("03244", country);
			IsNotValid("04002", country);
			IsNotValid("05698", country);
			IsNotValid("06284", country);
			IsNotValid("07613", country);
			IsNotValid("06761", country);
			IsNotValid("08753", country);
			IsNotValid("09999", country);
		}

		/// <summary>Tests patterns that should not be valid for British Indian Ocean Territory (IO).</summary>
		[Test]
		public void IsNotValidCustomCases_IO_All()
		{
			var country = Country.IO;

			IsNotValid("AADF0DF", country);
			IsNotValid("ASDS0DS", country);
			IsNotValid("BJRF1RF", country);
			IsNotValid("CDWK2WK", country);
			IsNotValid("DESD3SD", country);
			IsNotValid("EOPJ4PJ", country);
			IsNotValid("FNKF5KF", country);
			IsNotValid("GFLS6LS", country);
			IsNotValid("HLJD7JD", country);
			IsNotValid("IDMO6MO", country);
			IsNotValid("JSDF8DF", country);
			IsNotValid("KNJS9JS", country);
		}

		/// <summary>Tests patterns that should not be valid for Iraq (IQ).</summary>
		[Test]
		public void IsNotValidCustomCases_IQ_All()
		{
			var country = Country.IQ;

			IsNotValid("00000", country);
			IsNotValid("20004", country);
			IsNotValid("76345", country);
			IsNotValid("87182", country);
			IsNotValid("99999", country);
		}

		/// <summary>Tests patterns that should not be valid for Iran (IR).</summary>
		[Test]
		public void IsNotValidCustomCases_IR_All()
		{
			var country = Country.IR;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Iceland (IS).</summary>
		[Test]
		public void IsNotValidCustomCases_IS_All()
		{
			var country = Country.IS;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Italy (IT).</summary>
		[Test]
		public void IsNotValidCustomCases_IT_All()
		{
			var country = Country.IT;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Jersey (JE).</summary>
		[Test]
		public void IsNotValidCustomCases_JE_All()
		{
			var country = Country.JE;

			IsNotValid("0", country);

			IsNotValid("A00S", country);
			IsNotValid("G25S", country);
			IsNotValid("D36F", country);
			IsNotValid("D44S", country);
			IsNotValid("R78F", country);
			IsNotValid("W75K", country);
			IsNotValid("5D0S", country);
			IsNotValid("8J8P", country);
			IsNotValid("9F5K", country);
			IsNotValid("0LS2", country);
			IsNotValid("3JD2", country);
			IsNotValid("9MO9", country);
			IsNotValid("0AS0", country);
			IsNotValid("G042S", country);
			IsNotValid("D153F", country);
			IsNotValid("D274S", country);
			IsNotValid("3R37F", country);
			IsNotValid("4W77K", country);
			IsNotValid("5S35D", country);
			IsNotValid("66PJ8", country);
			IsNotValid("74KF9", country);
			IsNotValid("6S80L", country);
			IsNotValid("8D93J", country);
			IsNotValid("9O99M", country);
		}

		/// <summary>Tests patterns that should not be valid for Jordan (JO).</summary>
		[Test]
		public void IsNotValidCustomCases_JO_All()
		{
			var country = Country.JO;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Japan (JP).</summary>
		[Test]
		public void IsNotValidCustomCases_JP_All()
		{
			var country = Country.JP;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Kyrgyzstan (KG).</summary>
		[Test]
		public void IsNotValidCustomCases_KG_All()
		{
			var country = Country.KG;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Cambodia (KH).</summary>
		[Test]
		public void IsNotValidCustomCases_KH_All()
		{
			var country = Country.KH;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Korea (KR).</summary>
		[Test]
		public void IsNotValidCustomCases_KR_All()
		{
			var country = Country.KR;

			IsNotValid("000000", country);
			IsNotValid("023456", country);
			IsNotValid("012823", country);
			IsNotValid("020375", country);
			IsNotValid("032434", country);
			IsNotValid("040082", country);
			IsNotValid("056978", country);
			IsNotValid("862824", country);
			IsNotValid("861134", country);
			IsNotValid("876718", country);
			IsNotValid("975239", country);
			IsNotValid("999999", country);
		}

		/// <summary>Tests patterns that should not be valid for Cayman Islands (KY).</summary>
		[Test]
		public void IsNotValidCustomCases_KY_All()
		{
			var country = Country.KY;

			IsNotValid("AA00000", country);
			IsNotValid("AS01449", country);
			IsNotValid("BJ12823", country);
			IsNotValid("CD20375", country);
			IsNotValid("DE32434", country);
			IsNotValid("EO40082", country);
			IsNotValid("FN56978", country);
			IsNotValid("GF62824", country);
			IsNotValid("HL76113", country);
			IsNotValid("ID67671", country);
			IsNotValid("JS87523", country);
			IsNotValid("KN99999", country);
			IsNotValid("LO00000", country);
			IsNotValid("ME01449", country);
			IsNotValid("NN12823", country);
			IsNotValid("OL20375", country);
			IsNotValid("PS32434", country);
			IsNotValid("QD40082", country);
			IsNotValid("RN56978", country);
			IsNotValid("SE62824", country);
			IsNotValid("TM76113", country);
			IsNotValid("UF67671", country);
			IsNotValid("VE87523", country);
			IsNotValid("WL99999", country);
			IsNotValid("XM00000", country);
			IsNotValid("YE01449", country);
			IsNotValid("ZL12823", country);
			IsNotValid("ZZ20375", country);
		}

		/// <summary>Tests patterns that should not be valid for Kazakhstan (KZ).</summary>
		[Test]
		public void IsNotValidCustomCases_KZ_All()
		{
			var country = Country.KZ;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Lao People'S Democratic Republic (LA).</summary>
		[Test]
		public void IsNotValidCustomCases_LA_All()
		{
			var country = Country.LA;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Lebanon (LB).</summary>
		[Test]
		public void IsNotValidCustomCases_LB_All()
		{
			var country = Country.LB;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Liechtenstein (LI).</summary>
		[Test]
		public void IsNotValidCustomCases_LI_All()
		{
			var country = Country.LI;

			IsNotValid("0000", country);
			IsNotValid("0144", country);
			IsNotValid("1282", country);
			IsNotValid("2037", country);
			IsNotValid("3243", country);
			IsNotValid("4008", country);
			IsNotValid("5697", country);
			IsNotValid("6282", country);
			IsNotValid("7611", country);
			IsNotValid("6767", country);
			IsNotValid("8752", country);
			IsNotValid("9999", country);

			IsNotValid("9300", country);
			IsNotValid("9499", country);
			IsNotValid("9593", country);
			IsNotValid("9679", country);
			IsNotValid("9480", country);
			IsNotValid("9481", country);
			IsNotValid("9482", country);
			IsNotValid("9483", country);
			IsNotValid("9484", country);
			IsNotValid("9499", country);
		}

		/// <summary>Tests patterns that should not be valid for Sri Lanka (LK).</summary>
		[Test]
		public void IsNotValidCustomCases_LK_All()
		{
			var country = Country.LK;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Liberia (LR).</summary>
		[Test]
		public void IsNotValidCustomCases_LR_All()
		{
			var country = Country.LR;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Lesotho (LS).</summary>
		[Test]
		public void IsNotValidCustomCases_LS_All()
		{
			var country = Country.LS;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Lithuania (LT).</summary>
		[Test]
		public void IsNotValidCustomCases_LT_All()
		{
			var country = Country.LT;

			IsNotValid("AA00000", country);
			IsNotValid("AS01449", country);
			IsNotValid("BJ12823", country);
			IsNotValid("CD20375", country);
			IsNotValid("DE32434", country);
			IsNotValid("EO40082", country);
			IsNotValid("FN56978", country);
			IsNotValid("GF62824", country);
			IsNotValid("HL76113", country);
			IsNotValid("ID67671", country);
			IsNotValid("JS87523", country);
			IsNotValid("KN99999", country);
			IsNotValid("LO00000", country);
			IsNotValid("ME01449", country);
			IsNotValid("NN12823", country);
			IsNotValid("OL20375", country);
			IsNotValid("PS32434", country);
			IsNotValid("QD40082", country);
			IsNotValid("RN56978", country);
			IsNotValid("SE62824", country);
			IsNotValid("TM76113", country);
			IsNotValid("UF67671", country);
			IsNotValid("VE87523", country);
			IsNotValid("WL99999", country);
			IsNotValid("XM00000", country);
			IsNotValid("YE01449", country);
			IsNotValid("ZL12823", country);
			IsNotValid("ZZ20375", country);
		}

		/// <summary>Tests patterns that should not be valid for Luxembourg (LU).</summary>
		[Test]
		public void IsNotValidCustomCases_LU_All()
		{
			var country = Country.LU;

			IsNotValid("AA0000", country);
			IsNotValid("AS0144", country);
			IsNotValid("BJ1282", country);
			IsNotValid("CD2037", country);
			IsNotValid("DE3243", country);
			IsNotValid("EO4008", country);
			IsNotValid("FN5697", country);
			IsNotValid("GF6282", country);
			IsNotValid("HL7611", country);
			IsNotValid("ID6767", country);
			IsNotValid("JS8752", country);
			IsNotValid("KN9999", country);
			IsNotValid("LO0000", country);
			IsNotValid("ME0144", country);
			IsNotValid("NN1282", country);
			IsNotValid("OL2037", country);
			IsNotValid("PS3243", country);
		}

		/// <summary>Tests patterns that should not be valid for Latvia (LV).</summary>
		[Test]
		public void IsNotValidCustomCases_LV_All()
		{
			var country = Country.KY;

			IsNotValid("AA0000", country);
			IsNotValid("AS0449", country);
			IsNotValid("BJ1823", country);
			IsNotValid("CD2375", country);
			IsNotValid("DE3434", country);
			IsNotValid("EO4082", country);
			IsNotValid("FN5978", country);
			IsNotValid("GF6824", country);
			IsNotValid("HL7113", country);
			IsNotValid("ID6671", country);
			IsNotValid("JS8523", country);
			IsNotValid("KN9999", country);
			IsNotValid("LO0000", country);
			IsNotValid("ME0449", country);
			IsNotValid("NN1823", country);
			IsNotValid("OL2375", country);
			IsNotValid("PS3434", country);
			IsNotValid("QD4082", country);
			IsNotValid("RN5978", country);
			IsNotValid("SE6824", country);
			IsNotValid("TM7113", country);
			IsNotValid("UF6671", country);
			IsNotValid("VE8523", country);
			IsNotValid("WL9999", country);
			IsNotValid("XM0000", country);
			IsNotValid("YE0449", country);
			IsNotValid("ZL1823", country);
			IsNotValid("ZZ2375", country);
		}

		/// <summary>Tests patterns that should not be valid for Libya (LY).</summary>
		[Test]
		public void IsNotValidCustomCases_LY_All()
		{
			var country = Country.LY;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Morocco (MA).</summary>
		[Test]
		public void IsNotValidCustomCases_MA_All()
		{
			var country = Country.MA;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Monaco (MC).</summary>
		[Test]
		public void IsNotValidCustomCases_MC_All()
		{
			var country = Country.MC;

			IsNotValid("00000", country);
			IsNotValid("04249", country);
			IsNotValid("18323", country);
			IsNotValid("23475", country);
			IsNotValid("34734", country);
			IsNotValid("40782", country);
			IsNotValid("59578", country);
			IsNotValid("68824", country);
			IsNotValid("71913", country);
			IsNotValid("66071", country);
			IsNotValid("98123", country);
			IsNotValid("98299", country);
			IsNotValid("98344", country);
			IsNotValid("98402", country);
			IsNotValid("98598", country);
			IsNotValid("98684", country);
			IsNotValid("98713", country);
			IsNotValid("98661", country);
			IsNotValid("98983", country);
			IsNotValid("98989", country);
			IsNotValid("00000", country);

			IsNotValid("MC00000", country);
			IsNotValid("MC04249", country);
			IsNotValid("MC18323", country);
			IsNotValid("MC23475", country);
			IsNotValid("MC34734", country);
			IsNotValid("MC40782", country);
			IsNotValid("MC59578", country);
			IsNotValid("MC68824", country);
			IsNotValid("MC71913", country);
			IsNotValid("MC66071", country);
			IsNotValid("MC85323", country);
			IsNotValid("MC99999", country);
			IsNotValid("MC00000", country);

			IsNotValid("AA98000", country);
			IsNotValid("AS98049", country);
			IsNotValid("BJ98023", country);
			IsNotValid("CD98075", country);
			IsNotValid("DE98034", country);
			IsNotValid("EO98082", country);
			IsNotValid("FN98078", country);
			IsNotValid("GF98024", country);
			IsNotValid("HL98013", country);
			IsNotValid("ID98071", country);
			IsNotValid("JS98023", country);
			IsNotValid("KN98099", country);
			IsNotValid("LO98000", country);
			IsNotValid("ME98049", country);
			IsNotValid("NN98023", country);
			IsNotValid("OL98075", country);
			IsNotValid("PS98034", country);
			IsNotValid("QD98082", country);
			IsNotValid("RN98078", country);
			IsNotValid("SE98024", country);
			IsNotValid("TM98013", country);
			IsNotValid("UF98071", country);
			IsNotValid("VE98023", country);
			IsNotValid("WL98099", country);
			IsNotValid("XM98000", country);
			IsNotValid("YE98049", country);
			IsNotValid("ZL98023", country);
			IsNotValid("ZZ98075", country);
		}

		/// <summary>Tests patterns that should not be valid for Moldova (MD).</summary>
		[Test]
		public void IsNotValidCustomCases_MD_All()
		{
			var country = Country.MD;

			IsNotValid("AA0000", country);
			IsNotValid("AS0144", country);
			IsNotValid("BJ1282", country);
			IsNotValid("CD2037", country);
			IsNotValid("DE3243", country);
			IsNotValid("EO4008", country);
			IsNotValid("FN5697", country);
			IsNotValid("GF6282", country);
			IsNotValid("HL7611", country);
			IsNotValid("ID6767", country);
			IsNotValid("JS8752", country);
			IsNotValid("KN9999", country);
			IsNotValid("LO0000", country);
			IsNotValid("ME0144", country);
			IsNotValid("NN1282", country);
			IsNotValid("OL2037", country);
			IsNotValid("PS3243", country);
		}

		/// <summary>Tests patterns that should not be valid for Montenegro (ME).</summary>
		[Test]
		public void IsNotValidCustomCases_ME_All()
		{
			var country = Country.MD;

			IsNotValid("00000", country);
			IsNotValid("01449", country);
			IsNotValid("12823", country);
			IsNotValid("20375", country);
			IsNotValid("32434", country);
			IsNotValid("40082", country);
			IsNotValid("56978", country);
			IsNotValid("62824", country);
			IsNotValid("76113", country);
			IsNotValid("67671", country);
			IsNotValid("87523", country);
			IsNotValid("99999", country);

			IsNotValid("80000", country);
			IsNotValid("80149", country);
			IsNotValid("82035", country);
			IsNotValid("83244", country);
			IsNotValid("86284", country);
			IsNotValid("87613", country);
			IsNotValid("86761", country);
			IsNotValid("88753", country);
			IsNotValid("89999", country);
		}

		/// <summary>Tests patterns that should not be valid for Saint Martin (MF).</summary>
		[Test]
		public void IsNotValidCustomCases_MF_All()
		{
			var country = Country.MF;

			IsNotValid("00000", country);
			IsNotValid("01235", country);
			IsNotValid("12346", country);
			IsNotValid("20004", country);
			IsNotValid("32648", country);
			IsNotValid("40945", country);
			IsNotValid("56640", country);
			IsNotValid("62908", country);
			IsNotValid("76345", country);
			IsNotValid("67552", country);
			IsNotValid("87182", country);
			IsNotValid("99999", country);
		}

		/// <summary>Tests patterns that should not be valid for Madagascar (MG).</summary>
		[Test]
		public void IsNotValidCustomCases_MG_All()
		{
			var country = Country.MG;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Marshall Islands (MH).</summary>
		[Test]
		public void IsNotValidCustomCases_MH_All()
		{
			var country = Country.MH;

			IsNotValid("06000", country);
			IsNotValid("00425", country);
			IsNotValid("11836", country);
			IsNotValid("22304", country);
			IsNotValid("33468", country);
			IsNotValid("44095", country);
			IsNotValid("55960", country);
			IsNotValid("66898", country);
			IsNotValid("77135", country);
			IsNotValid("66652", country);
			IsNotValid("88512", country);
			IsNotValid("99999", country);
			IsNotValid("96932", country);
			IsNotValid("96951", country);
			IsNotValid("96989", country);
			IsNotValid("00000", country);

			IsNotValid("000000000", country);
			IsNotValid("012345678", country);
			IsNotValid("128253436", country);
			IsNotValid("203770044", country);
			IsNotValid("324336478", country);
			IsNotValid("400879475", country);
			IsNotValid("569736450", country);
			IsNotValid("628269088", country);
			IsNotValid("761143495", country);
			IsNotValid("676785502", country);
			IsNotValid("875291832", country);
			IsNotValid("999999999", country);
			IsNotValid("000000000", country);
		}

		/// <summary>Tests patterns that should not be valid for Macedonia (MK).</summary>
		[Test]
		public void IsNotValidCustomCases_MK_All()
		{
			var country = Country.MK;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Myanmar (MM).</summary>
		[Test]
		public void IsNotValidCustomCases_MM_All()
		{
			var country = Country.MM;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Mongolia (MN).</summary>
		[Test]
		public void IsNotValidCustomCases_MN_All()
		{
			var country = Country.MN;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Northern Mariana Islands (MP).</summary>
		[Test]
		public void IsNotValidCustomCases_MP_All()
		{
			var country = Country.MP;

			IsNotValid("00000", country);
			IsNotValid("01235", country);
			IsNotValid("12346", country);
			IsNotValid("20004", country);
			IsNotValid("32648", country);
			IsNotValid("40945", country);
			IsNotValid("56640", country);
			IsNotValid("62908", country);
			IsNotValid("76345", country);
			IsNotValid("67552", country);
			IsNotValid("87182", country);
			IsNotValid("99999", country);
			IsNotValid("96949", country);
			IsNotValid("96953", country);
			IsNotValid("96954", country);
			IsNotValid("000000000", country);
			IsNotValid("012354423", country);
			IsNotValid("123462534", country);
			IsNotValid("200047700", country);
			IsNotValid("326483364", country);
			IsNotValid("409458794", country);
			IsNotValid("566407364", country);
			IsNotValid("629082690", country);
			IsNotValid("763451434", country);
			IsNotValid("675527855", country);
			IsNotValid("871822918", country);
			IsNotValid("969496831", country);
			IsNotValid("969535348", country);
			IsNotValid("969545607", country);
			IsNotValid("999999999", country);
		}

		/// <summary>Tests patterns that should not be valid for Martinique (MQ).</summary>
		[Test]
		public void IsNotValidCustomCases_MQ_All()
		{
			var country = Country.MQ;

			IsNotValid("00000", country);
			IsNotValid("01235", country);
			IsNotValid("12346", country);
			IsNotValid("20004", country);
			IsNotValid("32648", country);
			IsNotValid("40945", country);
			IsNotValid("56640", country);
			IsNotValid("62908", country);
			IsNotValid("76345", country);
			IsNotValid("67552", country);
			IsNotValid("87182", country);
			IsNotValid("99999", country);
		}

		/// <summary>Tests patterns that should not be valid for Malta (MT).</summary>
		[Test]
		public void IsNotValidCustomCases_MT_All()
		{
			var country = Country.MT;

			IsNotValid("AA00000", country);
			IsNotValid("ASD01D2", country);
		}

		/// <summary>Tests patterns that should not be valid for Mexico (MX).</summary>
		[Test]
		public void IsNotValidCustomCases_MX_All()
		{
			var country = Country.MX;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Malaysia (MY).</summary>
		[Test]
		public void IsNotValidCustomCases_MY_All()
		{
			var country = Country.MY;

			IsNotValid("00000", country);
			IsNotValid("00035", country);
			IsNotValid("00146", country);
			IsNotValid("00204", country);
			IsNotValid("00348", country);
			IsNotValid("00445", country);
			IsNotValid("00540", country);
			IsNotValid("00608", country);
			IsNotValid("00745", country);
			IsNotValid("00652", country);
			IsNotValid("00882", country);
			IsNotValid("00999", country);
		}

		/// <summary>Tests patterns that should not be valid for Mozambique (MZ).</summary>
		[Test]
		public void IsNotValidCustomCases_MZ_All()
		{
			var country = Country.MZ;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Namibia (NA).</summary>
		[Test]
		public void IsNotValidCustomCases_NA_All()
		{
			var country = Country.NA;

			IsNotValid("00000", country);
			IsNotValid("01235", country);
			IsNotValid("12346", country);
			IsNotValid("20004", country);
			IsNotValid("32648", country);
			IsNotValid("40945", country);
			IsNotValid("56640", country);
			IsNotValid("62908", country);
			IsNotValid("76345", country);
			IsNotValid("67552", country);
			IsNotValid("87182", country);
			IsNotValid("89999", country);
			IsNotValid("93000", country);
			IsNotValid("99999", country);
		}

		/// <summary>Tests patterns that should not be valid for New Caledonia (NC).</summary>
		[Test]
		public void IsNotValidCustomCases_NC_All()
		{
			var country = Country.NC;

			IsNotValid("00000", country);
			IsNotValid("01235", country);
			IsNotValid("12346", country);
			IsNotValid("20004", country);
			IsNotValid("32648", country);
			IsNotValid("40945", country);
			IsNotValid("56640", country);
			IsNotValid("62908", country);
			IsNotValid("76345", country);
			IsNotValid("67552", country);
			IsNotValid("87182", country);
			IsNotValid("99999", country);

			IsNotValid("98600", country);
			IsNotValid("98535", country);
			IsNotValid("98346", country);
			IsNotValid("98204", country);
			IsNotValid("98648", country);
			IsNotValid("98545", country);
			IsNotValid("98140", country);
			IsNotValid("98108", country);
			IsNotValid("99045", country);
			IsNotValid("99052", country);
			IsNotValid("97982", country);
			IsNotValid("99999", country);
		}

		/// <summary>Tests patterns that should not be valid for Niger (NE).</summary>
		[Test]
		public void IsNotValidCustomCases_NE_All()
		{
			var country = Country.NE;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Norfolk Island (NF).</summary>
		[Test]
		public void IsNotValidCustomCases_NF_All()
		{
			var country = Country.NF;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Nigeria (NG).</summary>
		[Test]
		public void IsNotValidCustomCases_NG_All()
		{
			var country = Country.NG;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Nicaragua (NI).</summary>
		[Test]
		public void IsNotValidCustomCases_NI_All()
		{
			var country = Country.NI;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Netherlands (NL).</summary>
		[Test]
		public void IsNotValidCustomCases_NL_All()
		{
			var country = Country.NL;

			IsNotValid("0000DF", country);
			IsNotValid("0125DS", country);
			IsNotValid("3278SA", country);
			IsNotValid("8732SD", country);
			IsNotValid("9999SS", country);
		}

		/// <summary>Tests patterns that should not be valid for Norway (NO).</summary>
		[Test]
		public void IsNotValidCustomCases_NO_All()
		{
			var country = Country.NO;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Nepal (NP).</summary>
		[Test]
		public void IsNotValidCustomCases_NP_All()
		{
			var country = Country.NP;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for New Zealand (NZ).</summary>
		[Test]
		public void IsNotValidCustomCases_NZ_All()
		{
			var country = Country.NZ;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Oman (OM).</summary>
		[Test]
		public void IsNotValidCustomCases_OM_All()
		{
			var country = Country.OM;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Panama (PA).</summary>
		[Test]
		public void IsNotValidCustomCases_PA_All()
		{
			var country = Country.PA;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Peru (PE).</summary>
		[Test]
		public void IsNotValidCustomCases_PE_All()
		{
			var country = Country.PE;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for French Polynesia (PF).</summary>
		[Test]
		public void IsNotValidCustomCases_PF_All()
		{
			var country = Country.PF;

			IsNotValid("00000", country);
			IsNotValid("01235", country);
			IsNotValid("12346", country);
			IsNotValid("20004", country);
			IsNotValid("32648", country);
			IsNotValid("40945", country);
			IsNotValid("56640", country);
			IsNotValid("62908", country);
			IsNotValid("76345", country);
			IsNotValid("67552", country);
			IsNotValid("87182", country);
			IsNotValid("98524", country);
			IsNotValid("98600", country);
			IsNotValid("98805", country);
			IsNotValid("98916", country);
			IsNotValid("99999", country);
		}

		/// <summary>Tests patterns that should not be valid for Papua New Guinea (PG).</summary>
		[Test]
		public void IsNotValidCustomCases_PG_All()
		{
			var country = Country.PG;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Philippines (PH).</summary>
		[Test]
		public void IsNotValidCustomCases_PH_All()
		{
			var country = Country.PH;

			IsNotValid("0000", country);
			IsNotValid("0003", country);
			IsNotValid("0014", country);
			IsNotValid("0020", country);
			IsNotValid("0034", country);
			IsNotValid("0044", country);
			IsNotValid("0054", country);
			IsNotValid("0060", country);
			IsNotValid("0074", country);
			IsNotValid("0065", country);
			IsNotValid("0088", country);
			IsNotValid("0099", country);
		}

		/// <summary>Tests patterns that should not be valid for Pakistan (PK).</summary>
		[Test]
		public void IsNotValidCustomCases_PK_All()
		{
			var country = Country.PK;

			IsNotValid("00000", country);
			IsNotValid("00125", country);
			IsNotValid("01236", country);
			IsNotValid("02004", country);
			IsNotValid("03268", country);
			IsNotValid("04095", country);
			IsNotValid("05660", country);
			IsNotValid("06298", country);
			IsNotValid("07635", country);
			IsNotValid("06752", country);
			IsNotValid("08712", country);
			IsNotValid("09854", country);
			IsNotValid("09860", country);
			IsNotValid("09885", country);
			IsNotValid("09896", country);
			IsNotValid("09999", country);
		}

		/// <summary>Tests patterns that should not be valid for Poland (PL).</summary>
		[Test]
		public void IsNotValidCustomCases_PL_All()
		{
			var country = Country.PL;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Saint Pierre And Miquelon (PM).</summary>
		[Test]
		public void IsNotValidCustomCases_PM_All()
		{
			var country = Country.PM;

			IsNotValid("00000", country);
			IsNotValid("01235", country);
			IsNotValid("12346", country);
			IsNotValid("20004", country);
			IsNotValid("32648", country);
			IsNotValid("40945", country);
			IsNotValid("56640", country);
			IsNotValid("62908", country);
			IsNotValid("76345", country);
			IsNotValid("67552", country);
			IsNotValid("87182", country);
			IsNotValid("97000", country);
			IsNotValid("97101", country);
			IsNotValid("97212", country);
			IsNotValid("97320", country);
			IsNotValid("97432", country);
			IsNotValid("97640", country);
			IsNotValid("97756", country);
			IsNotValid("97862", country);
			IsNotValid("97976", country);
			IsNotValid("99999", country);
			IsNotValid("97502", country);
			IsNotValid("97513", country);
			IsNotValid("97520", country);
			IsNotValid("97536", country);
			IsNotValid("97549", country);
			IsNotValid("97556", country);
			IsNotValid("97569", country);
			IsNotValid("97573", country);
			IsNotValid("97565", country);
			IsNotValid("97581", country);
			IsNotValid("97599", country);
		}

		/// <summary>Tests patterns that should not be valid for Pitcairn (PN).</summary>
		[Test]
		public void IsNotValidCustomCases_PN_All()
		{
			var country = Country.PN;

			IsNotValid("PCRN2ZZ", country);
			IsNotValid("AADF0FD", country);
			IsNotValid("ASDS0KD", country);
			IsNotValid("BJRF1DR", country);
			IsNotValid("CDWK2JW", country);
			IsNotValid("DESD3FS", country);
			IsNotValid("EOPJ4SP", country);
			IsNotValid("FNKF5DK", country);
			IsNotValid("GFLS6OL", country);
			IsNotValid("HLJD7FJ", country);
			IsNotValid("IDMO6SM", country);
			IsNotValid("JSDF8FD", country);
			IsNotValid("KNJS9SJ", country);
			IsNotValid("LODF0FD", country);
			IsNotValid("MEDS0KD", country);
			IsNotValid("NNRF1RF", country);
			IsNotValid("OLWK2WS", country);
			IsNotValid("PSSD3SF", country);
			IsNotValid("QDPJ4PK", country);
			IsNotValid("RNKF5KD", country);
			IsNotValid("SELS6LJ", country);
			IsNotValid("TMJD7JF", country);
			IsNotValid("UFMO6MS", country);
			IsNotValid("VEDF8DD", country);
			IsNotValid("WLJS9JO", country);
			IsNotValid("XMDF0DF", country);
			IsNotValid("YEDS0DS", country);
			IsNotValid("ZLRF1RF", country);
			IsNotValid("ZZWK2WS", country);
		}

		/// <summary>Tests patterns that should not be valid for Puerto Rico (PR).</summary>
		[Test]
		public void IsNotValidCustomCases_PR_All()
		{
			var country = Country.PR;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Palestinian Territory (PS).</summary>
		[Test]
		public void IsNotValidCustomCases_PS_All()
		{
			var country = Country.PS;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Portugal (PT).</summary>
		[Test]
		public void IsNotValidCustomCases_PT_All()
		{
			var country = Country.PT;

			IsNotValid("0000000", country);
			IsNotValid("0014494", country);
			IsNotValid("0123456", country);
			IsNotValid("0203757", country);
			IsNotValid("0324343", country);
			IsNotValid("0400827", country);
			IsNotValid("0569783", country);
			IsNotValid("0628246", country);
			IsNotValid("0761134", country);
			IsNotValid("0676718", country);
			IsNotValid("0875239", country);
			IsNotValid("0999999", country);
		}

		/// <summary>Tests patterns that should not be valid for Palau (PW).</summary>
		[Test]
		public void IsNotValidCustomCases_PW_All()
		{
			var country = Country.PW;

			IsNotValid("01235", country);
			IsNotValid("12346", country);
			IsNotValid("20004", country);
			IsNotValid("32648", country);
			IsNotValid("40945", country);
			IsNotValid("56640", country);
			IsNotValid("62908", country);
			IsNotValid("76345", country);
			IsNotValid("67552", country);
			IsNotValid("87182", country);
			IsNotValid("96939", country);
			IsNotValid("96941", country);
			IsNotValid("96948", country);
			IsNotValid("96952", country);
			IsNotValid("99999", country);
		}

		/// <summary>Tests patterns that should not be valid for Paraguay (PY).</summary>
		[Test]
		public void IsNotValidCustomCases_PY_All()
		{
			var country = Country.PY;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Réunion (RE).</summary>
		[Test]
		public void IsNotValidCustomCases_RE_All()
		{
			var country = Country.RE;

			IsNotValid("00000", country);
			IsNotValid("01235", country);
			IsNotValid("12346", country);
			IsNotValid("20004", country);
			IsNotValid("32648", country);
			IsNotValid("40945", country);
			IsNotValid("56640", country);
			IsNotValid("62908", country);
			IsNotValid("76345", country);
			IsNotValid("67552", country);
			IsNotValid("87182", country);
			IsNotValid("97000", country);
			IsNotValid("97102", country);
			IsNotValid("97213", country);
			IsNotValid("97320", country);
			IsNotValid("97536", country);
			IsNotValid("97649", country);
			IsNotValid("97756", country);
			IsNotValid("97869", country);
			IsNotValid("97973", country);
			IsNotValid("99999", country);
		}

		/// <summary>Tests patterns that should not be valid for Romania (RO).</summary>
		[Test]
		public void IsNotValidCustomCases_RO_All()
		{
			var country = Country.RO;

			IsNotValid("00000", country);
			IsNotValid("00012", country);
			IsNotValid("00123", country);
			IsNotValid("00200", country);
			IsNotValid("00326", country);
			IsNotValid("00409", country);
			IsNotValid("00566", country);
			IsNotValid("00629", country);
			IsNotValid("00763", country);
			IsNotValid("00675", country);
			IsNotValid("00871", country);
			IsNotValid("00970", country);
			IsNotValid("00999", country);
		}

		/// <summary>Tests patterns that should not be valid for Serbia (RS).</summary>
		[Test]
		public void IsNotValidCustomCases_RS_All()
		{
			var country = Country.RS;

			IsNotValid("000000", country);
			IsNotValid("012345", country);
			IsNotValid("400827", country);
			IsNotValid("569783", country);
			IsNotValid("628246", country);
			IsNotValid("761134", country);
			IsNotValid("676718", country);
			IsNotValid("875239", country);
			IsNotValid("999999", country);
		}

		/// <summary>Tests patterns that should not be valid for Russian Federation (RU).</summary>
		[Test]
		public void IsNotValidCustomCases_RU_All()
		{
			var country = Country.RU;

			IsNotValid("0000000", country);
			IsNotValid("0012345", country);
			IsNotValid("0128235", country);
			IsNotValid("0203757", country);
			IsNotValid("0324343", country);
			IsNotValid("0400827", country);
			IsNotValid("0569783", country);
			IsNotValid("0628246", country);
			IsNotValid("0761134", country);
			IsNotValid("0676718", country);
			IsNotValid("0875239", country);
			IsNotValid("0999999", country);
		}

		/// <summary>Tests patterns that should not be valid for Saudi Arabia (SA).</summary>
		[Test]
		public void IsNotValidCustomCases_SA_All()
		{
			var country = Country.SA;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Sudan (SD).</summary>
		[Test]
		public void IsNotValidCustomCases_SD_All()
		{
			var country = Country.SD;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Sweden (SE).</summary>
		[Test]
		public void IsNotValidCustomCases_SE_All()
		{
			var country = Country.SE;

			IsNotValid("000000", country);
			IsNotValid("001235", country);
			IsNotValid("012825", country);
			IsNotValid("020377", country);
			IsNotValid("032433", country);
			IsNotValid("040087", country);
			IsNotValid("056973", country);
			IsNotValid("062826", country);
			IsNotValid("076114", country);
			IsNotValid("067678", country);
			IsNotValid("087529", country);
			IsNotValid("099999", country);
		}

		/// <summary>Tests patterns that should not be valid for Singapore (SG).</summary>
		[Test]
		public void IsNotValidCustomCases_SG_All()
		{
			var country = Country.SG;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Saint Helena (SH).</summary>
		[Test]
		public void IsNotValidCustomCases_SH_All()
		{
			var country = Country.SH;

			IsNotValid("SAHL1ZZ", country);
			IsNotValid("STBL1AZ", country);
			IsNotValid("STHL2ZZ", country);
			IsNotValid("AADF0DF", country);
			IsNotValid("ASDS2DS", country);
			IsNotValid("BJRF3RF", country);
			IsNotValid("CDWK4WK", country);
			IsNotValid("DESD7SD", country);
			IsNotValid("EOPJ7PJ", country);
			IsNotValid("FNKF5KF", country);
			IsNotValid("GFLS8LS", country);
			IsNotValid("HLJD9JD", country);
			IsNotValid("IDMO0MO", country);
			IsNotValid("JSDF3DF", country);
			IsNotValid("KNJS9JS", country);
			IsNotValid("LODF0DF", country);
			IsNotValid("MEDS2DS", country);
			IsNotValid("NNRF3RF", country);
			IsNotValid("OLWK4WK", country);
			IsNotValid("PSSD7SD", country);
			IsNotValid("QDPJ7PJ", country);
			IsNotValid("RNKF5KF", country);
			IsNotValid("SELS8LS", country);
			IsNotValid("TMJD9JD", country);
			IsNotValid("UFMO0MO", country);
			IsNotValid("VEDF3DF", country);
			IsNotValid("WLJS9JS", country);
			IsNotValid("XMDF0DF", country);
			IsNotValid("YEDS2DS", country);
			IsNotValid("ZLRF3RF", country);
			IsNotValid("ZZWK4WK", country);
		}

		/// <summary>Tests patterns that should not be valid for Slovenia (SI).</summary>
		[Test]
		public void IsNotValidCustomCases_SI_All()
		{
			var country = Country.SI;

			IsNotValid("AA0000", country);
			IsNotValid("AS0144", country);
			IsNotValid("BJ1282", country);
			IsNotValid("CD2037", country);
			IsNotValid("DE3243", country);
			IsNotValid("EO4008", country);
			IsNotValid("FN5697", country);
			IsNotValid("GF6282", country);
			IsNotValid("HL7611", country);
			IsNotValid("ID6767", country);
			IsNotValid("JS8752", country);
			IsNotValid("KN9999", country);
			IsNotValid("LO0000", country);
			IsNotValid("ME0144", country);
			IsNotValid("NN1282", country);
			IsNotValid("OL2037", country);
			IsNotValid("PS3243", country);
			IsNotValid("QD4008", country);
			IsNotValid("RN5697", country);
			IsNotValid("SE6282", country);
			IsNotValid("TM7611", country);
			IsNotValid("UF6767", country);
			IsNotValid("VE8752", country);
			IsNotValid("WL9999", country);
			IsNotValid("XM0000", country);
			IsNotValid("YE0144", country);
			IsNotValid("ZL1282", country);
			IsNotValid("ZZ2037", country);
		}

		/// <summary>Tests patterns that should not be valid for Slovakia (SK).</summary>
		[Test]
		public void IsNotValidCustomCases_SK_All()
		{
			var country = Country.SK;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for San Marino (SM).</summary>
		[Test]
		public void IsNotValidCustomCases_SM_All()
		{
			var country = Country.SM;

			IsNotValid("00000", country);
			IsNotValid("05125", country);
			IsNotValid("16285", country);
			IsNotValid("27037", country);
			IsNotValid("36243", country);
			IsNotValid("46890", country);
			IsNotValid("47797", country);
			IsNotValid("59693", country);
			IsNotValid("66286", country);
			IsNotValid("76614", country);
			IsNotValid("66768", country);
			IsNotValid("83759", country);
			IsNotValid("99999", country);
		}

		/// <summary>Tests patterns that should not be valid for Senegal (SN).</summary>
		[Test]
		public void IsNotValidCustomCases_SN_All()
		{
			var country = Country.SN;

			IsNotValid("AA00000", country);
			IsNotValid("AB01234", country);
			IsNotValid("BJ12823", country);
			IsNotValid("CO20375", country);
			IsNotValid("DE32434", country);
			IsNotValid("EO40082", country);
			IsNotValid("FN56978", country);
			IsNotValid("GF62824", country);
			IsNotValid("HL76113", country);
			IsNotValid("ID67671", country);
			IsNotValid("JS87523", country);
			IsNotValid("KN99999", country);
			IsNotValid("LO00000", country);
			IsNotValid("ME01449", country);
			IsNotValid("NN12823", country);
			IsNotValid("OL20375", country);
			IsNotValid("PS32434", country);
			IsNotValid("QD40082", country);
			IsNotValid("RN56978", country);
			IsNotValid("SE62824", country);
			IsNotValid("TM76113", country);
			IsNotValid("UF67671", country);
			IsNotValid("VE87523", country);
			IsNotValid("WL99999", country);
			IsNotValid("XM00000", country);
			IsNotValid("YE01449", country);
			IsNotValid("ZL12823", country);
			IsNotValid("ZZ20375", country);
		}

		/// <summary>Tests patterns that should not be valid for El Salvador (SV).</summary>
		[Test]
		public void IsNotValidCustomCases_SV_All()
		{
			var country = Country.SV;

			IsNotValid("00000", country);
			IsNotValid("01001", country);
			IsNotValid("01131", country);
			IsNotValid("02131", country);
			IsNotValid("02331", country);
			IsNotValid("12234", country);
			IsNotValid("27000", country);
			IsNotValid("33248", country);
			IsNotValid("48945", country);
			IsNotValid("57640", country);
			IsNotValid("62208", country);
			IsNotValid("71645", country);
			IsNotValid("67752", country);
			IsNotValid("82782", country);
			IsNotValid("99999", country);
		}

		/// <summary>Tests patterns that should not be valid for Swaziland (SZ).</summary>
		[Test]
		public void IsNotValidCustomCases_SZ_All()
		{
			var country = Country.SZ;

			IsNotValid("A000", country);
			IsNotValid("A014", country);
			IsNotValid("B128", country);
			IsNotValid("C203", country);
			IsNotValid("D324", country);
			IsNotValid("E400", country);
			IsNotValid("F569", country);
			IsNotValid("G628", country);
			IsNotValid("I676", country);
			IsNotValid("J875", country);
			IsNotValid("K999", country);
			IsNotValid("N128", country);
			IsNotValid("O203", country);
			IsNotValid("P324", country);
			IsNotValid("Q400", country);
			IsNotValid("R569", country);
			IsNotValid("T761", country);
			IsNotValid("U676", country);
			IsNotValid("V875", country);
			IsNotValid("W999", country);
			IsNotValid("X000", country);
			IsNotValid("Y014", country);
			IsNotValid("Z128", country);
			IsNotValid("Z999", country);

			IsNotValid("A00Z", country);
			IsNotValid("0A14", country);
			IsNotValid("1B28", country);
			IsNotValid("2C03", country);
			IsNotValid("3D24", country);
			IsNotValid("E40A", country);
			IsNotValid("F5B9", country);
			IsNotValid("G6BB", country);
			IsNotValid("H7A1", country);
			IsNotValid("I6A6", country);
			IsNotValid("875J", country);
			IsNotValid("999K", country);
			IsNotValid("000L", country);
			IsNotValid("014M", country);
			IsNotValid("128N", country);
			IsNotValid("203O", country);
			IsNotValid("324P", country);
			IsNotValid("Q4J0", country);
			IsNotValid("R5K6", country);
			IsNotValid("S6L2", country);
			IsNotValid("T7M6", country);
			IsNotValid("U6N7", country);
			IsNotValid("V8O7", country);
		}

		/// <summary>Tests patterns that should not be valid for Turks And Caicos Islands (TC).</summary>
		[Test]
		public void IsNotValidCustomCases_TC_All()
		{
			var country = Country.TC;

			IsNotValid("AKCA1ZZ", country);
			IsNotValid("TBCA1ZZ", country);
			IsNotValid("TKDA1ZZ", country);
			IsNotValid("TKCE1ZZ", country);
			IsNotValid("TKCA9ZZ", country);
			IsNotValid("TKCA1FZ", country);
			IsNotValid("TKCA1ZG", country);

			IsNotValid("AADF0DF", country);
			IsNotValid("ASDS0DS", country);
			IsNotValid("BJRF1RF", country);
			IsNotValid("CDWK2WK", country);
			IsNotValid("DESD3SD", country);
			IsNotValid("EOPJ4PJ", country);
			IsNotValid("FNKF5KF", country);
			IsNotValid("GFLS6LS", country);
			IsNotValid("HLJD7JD", country);
			IsNotValid("IDMO6MO", country);
			IsNotValid("JSDF8DF", country);
			IsNotValid("KNJS9JS", country);
			IsNotValid("LODF0DF", country);
			IsNotValid("MEDS0DS", country);
			IsNotValid("NNRF1RF", country);
			IsNotValid("OLWK2WK", country);
			IsNotValid("PSSD3SD", country);
			IsNotValid("QDPJ4PJ", country);
			IsNotValid("RNKF5KF", country);
			IsNotValid("SELS6LS", country);
			IsNotValid("TMJD7JD", country);
			IsNotValid("UFMO6MO", country);
			IsNotValid("VEDF8DF", country);
			IsNotValid("WLJS9JS", country);
			IsNotValid("XMDF0DF", country);
			IsNotValid("YEDS0DS", country);
			IsNotValid("ZLRF1RF", country);
			IsNotValid("ZZWK2WK", country);
		}

		/// <summary>Tests patterns that should not be valid for Chad (TD).</summary>
		[Test]
		public void IsNotValidCustomCases_TD_All()
		{
			var country = Country.TD;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Thailand (TH).</summary>
		[Test]
		public void IsNotValidCustomCases_TH_All()
		{
			var country = Country.TH;

			IsNotValid("00000", country);
			IsNotValid("00124", country);
			IsNotValid("01283", country);
			IsNotValid("02035", country);
			IsNotValid("03244", country);
			IsNotValid("04002", country);
			IsNotValid("05698", country);
			IsNotValid("06284", country);
			IsNotValid("07613", country);
			IsNotValid("06761", country);
			IsNotValid("08753", country);
			IsNotValid("09999", country);
		}

		/// <summary>Tests patterns that should not be valid for Tajikistan (TJ).</summary>
		[Test]
		public void IsNotValidCustomCases_TJ_All()
		{
			var country = Country.TJ;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Turkmenistan (TM).</summary>
		[Test]
		public void IsNotValidCustomCases_TM_All()
		{
			var country = Country.TM;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Tunisia (TN).</summary>
		[Test]
		public void IsNotValidCustomCases_TN_All()
		{
			var country = Country.TN;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Turkey (TR).</summary>
		[Test]
		public void IsNotValidCustomCases_TR_All()
		{
			var country = Country.TR;

			IsNotValid("00000", country);
			IsNotValid("00012", country);
			IsNotValid("00128", country);
			IsNotValid("00203", country);
			IsNotValid("00324", country);
			IsNotValid("00400", country);
			IsNotValid("00569", country);
			IsNotValid("00628", country);
			IsNotValid("00761", country);
			IsNotValid("00676", country);
			IsNotValid("00875", country);
			IsNotValid("00999", country);
		}

		/// <summary>Tests patterns that should not be valid for Trinidad And Tobago (TT).</summary>
		[Test]
		public void IsNotValidCustomCases_TT_All()
		{
			var country = Country.TT;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Taiwan (TW).</summary>
		[Test]
		public void IsNotValidCustomCases_TW_All()
		{
			var country = Country.TW;

			IsNotValid("000000", country);
			IsNotValid("001234", country);
			IsNotValid("012823", country);
			IsNotValid("020375", country);
			IsNotValid("032434", country);
			IsNotValid("040082", country);
			IsNotValid("056978", country);
			IsNotValid("062824", country);
			IsNotValid("076113", country);
			IsNotValid("067671", country);
			IsNotValid("087523", country);
			IsNotValid("099999", country);
		}

		/// <summary>Tests patterns that should not be valid for Ukraine (UA).</summary>
		[Test]
		public void IsNotValidCustomCases_UA_All()
		{
			var country = Country.UA;

			IsNotValid("00000", country);
			IsNotValid("00012", country);
			IsNotValid("00123", country);
			IsNotValid("00200", country);
			IsNotValid("00326", country);
			IsNotValid("00409", country);
			IsNotValid("00566", country);
			IsNotValid("00629", country);
			IsNotValid("00763", country);
			IsNotValid("00675", country);
			IsNotValid("00871", country);
			IsNotValid("00999", country);
		}

		/// <summary>Tests patterns that should not be valid for United States (US).</summary>
		[Test]
		public void IsNotValidCustomCases_US_All()
		{
			var country = Country.US;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Uruguay (UY).</summary>
		[Test]
		public void IsNotValidCustomCases_UY_All()
		{
			var country = Country.UY;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Holy See (VA).</summary>
		[Test]
		public void IsNotValidCustomCases_VA_All()
		{
			var country = Country.VA;

			IsNotValid("00000", country);
			IsNotValid("01234", country);
			IsNotValid("12823", country);
			IsNotValid("20375", country);
			IsNotValid("32434", country);
			IsNotValid("40082", country);
			IsNotValid("56978", country);
			IsNotValid("62824", country);
			IsNotValid("76113", country);
			IsNotValid("67671", country);
			IsNotValid("87523", country);
			IsNotValid("99999", country);
		}

		/// <summary>Tests patterns that should not be valid for Saint Vincent And The Grenadines (VC).</summary>
		[Test]
		public void IsNotValidCustomCases_VC_All()
		{
			var country = Country.VC;

			IsNotValid("AA0000", country);
			IsNotValid("AS0144", country);
			IsNotValid("BJ1282", country);
			IsNotValid("CD2037", country);
			IsNotValid("DE3243", country);
			IsNotValid("EO4008", country);
			IsNotValid("FN5697", country);
			IsNotValid("GF6282", country);
			IsNotValid("HL7611", country);
			IsNotValid("ID6767", country);
			IsNotValid("JS8752", country);
			IsNotValid("KN9999", country);
			IsNotValid("LO0000", country);
			IsNotValid("ME0144", country);
			IsNotValid("NN1282", country);
			IsNotValid("OL2037", country);
			IsNotValid("PS3243", country);
			IsNotValid("QD4008", country);
			IsNotValid("RN5697", country);
			IsNotValid("SE6282", country);
			IsNotValid("TM7611", country);
			IsNotValid("UF6767", country);
			IsNotValid("VE8752", country);
			IsNotValid("WL9999", country);
			IsNotValid("XM0000", country);
			IsNotValid("YE0144", country);
			IsNotValid("ZL1282", country);
			IsNotValid("ZZ2037", country);
		}

		/// <summary>Tests patterns that should not be valid for Venezuela (VE).</summary>
		[Test]
		public void IsNotValidCustomCases_VE_All()
		{
			var country = Country.VE;

			IsNotValid("00000", country);
			IsNotValid("01223", country);
			IsNotValid("12334", country);
			IsNotValid("20400", country);
			IsNotValid("32764", country);
			IsNotValid("40794", country);
			IsNotValid("56564", country);
			IsNotValid("62890", country);
			IsNotValid("76934", country);
			IsNotValid("67055", country);
			IsNotValid("87318", country);
			IsNotValid("99999", country);

			IsNotValid("000A", country);
			IsNotValid("032A", country);
			IsNotValid("143B", country);
			IsNotValid("204C", country);
			IsNotValid("347D", country);
			IsNotValid("447E", country);
			IsNotValid("545F", country);
			IsNotValid("608G", country);
			IsNotValid("749H", country);
			IsNotValid("J650I", country);
			IsNotValid("K8832", country);
			IsNotValid("L9999", country);
			IsNotValid("M0000", country);
			IsNotValid("N0325", country);
			IsNotValid("O1436", country);
			IsNotValid("20412", country);
			IsNotValid("34787", country);
			IsNotValid("44757", country);
			IsNotValid("54505", country);
			IsNotValid("60888", country);
		}

		/// <summary>Tests patterns that should not be valid for Virgin Islands (VG).</summary>
		[Test]
		public void IsNotValidCustomCases_VG_All()
		{
			var country = Country.VG;

			IsNotValid("0123", country);
			IsNotValid("1187", country);
			IsNotValid("1199", country);
			IsNotValid("1234", country);
			IsNotValid("2000", country);
			IsNotValid("3248", country);
			IsNotValid("4945", country);
			IsNotValid("5640", country);
			IsNotValid("6208", country);
			IsNotValid("7645", country);
			IsNotValid("6752", country);
			IsNotValid("8782", country);
			IsNotValid("9999", country);

			IsNotValid("VG0123", country);
			IsNotValid("VG1187", country);
			IsNotValid("VG1199", country);
			IsNotValid("VG1234", country);
			IsNotValid("VG2000", country);
			IsNotValid("VG3248", country);
			IsNotValid("VG4945", country);
			IsNotValid("VG5640", country);
			IsNotValid("VG6208", country);
			IsNotValid("VG7645", country);
			IsNotValid("VG6752", country);
			IsNotValid("VG8782", country);
			IsNotValid("VG9999", country);
		}

		/// <summary>Tests patterns that should not be valid for Virgin Islands (VI).</summary>
		[Test]
		public void IsNotValidCustomCases_VI_All()
		{
			var country = Country.VI;

			IsNotValid("00000", country);
			IsNotValid("01235", country);
			IsNotValid("12346", country);
			IsNotValid("20004", country);
			IsNotValid("32648", country);
			IsNotValid("40945", country);
			IsNotValid("56640", country);
			IsNotValid("62908", country);
			IsNotValid("76345", country);
			IsNotValid("67552", country);
			IsNotValid("87182", country);
			IsNotValid("99999", country);
			IsNotValid("000000000", country);
			IsNotValid("013582794", country);
			IsNotValid("124678364", country);
			IsNotValid("200424690", country);
			IsNotValid("324813434", country);
			IsNotValid("404571855", country);
			IsNotValid("564023918", country);
			IsNotValid("620899999", country);
			IsNotValid("764500000", country);
			IsNotValid("675249423", country);
			IsNotValid("878227947", country);
			IsNotValid("999999999", country);

			IsNotValid("00800", country);
			IsNotValid("00804", country);
			IsNotValid("00869", country);
			IsNotValid("00870", country);
			IsNotValid("00860", country);
			IsNotValid("00881", country);
			IsNotValid("00892", country);
			IsNotValid("008000000", country);
			IsNotValid("008041235", country);
			IsNotValid("008692908", country);
			IsNotValid("008706345", country);
			IsNotValid("008607552", country);
			IsNotValid("008817182", country);
			IsNotValid("008929999", country);
		}

		/// <summary>Tests patterns that should not be valid for Viet Nam (VN).</summary>
		[Test]
		public void IsNotValidCustomCases_VN_All()
		{
			var country = Country.VN;

			IsNotValid("0", country);
		}

		/// <summary>Tests patterns that should not be valid for Wallis And Futuna (WF).</summary>
		[Test]
		public void IsNotValidCustomCases_WF_All()
		{
			var country = Country.WF;

			IsNotValid("00000", country);
			IsNotValid("01235", country);
			IsNotValid("12346", country);
			IsNotValid("20004", country);
			IsNotValid("32648", country);
			IsNotValid("40945", country);
			IsNotValid("56640", country);
			IsNotValid("62908", country);
			IsNotValid("76345", country);
			IsNotValid("67552", country);
			IsNotValid("87182", country);
			IsNotValid("99999", country);
		}

		/// <summary>Tests patterns that should not be valid for Mayotte (YT).</summary>
		[Test]
		public void IsNotValidCustomCases_YT_All()
		{
			var country = Country.YT;

			IsNotValid("M11AA", country);
			IsNotValid("M11aA", country);
			IsNotValid("M11AA", country);
			IsNotValid("m11AA", country);
			IsNotValid("m11aa", country);

			IsNotValid("B338TH", country);
			IsNotValid("B338TH", country);

			IsNotValid("CR26XH", country);
			IsNotValid("CR26XH", country);

			IsNotValid("DN551PT", country);
			IsNotValid("DN551PT", country);

			IsNotValid("W1A1HQ", country);
			IsNotValid("W1A1HQ", country);

			IsNotValid("EC1A1BB", country);
			IsNotValid("EC1A1BB", country);
		}

		/// <summary>Tests patterns that should not be valid for South Africa (ZA).</summary>
		[Test]
		public void IsNotValidCustomCases_ZA_All()
		{
			var country = Country.ZA;

			IsNotValid("0000", country);
		}

		/// <summary>Tests patterns that should not be valid for Zambia (ZM).</summary>
		[Test]
		public void IsNotValidCustomCases_ZM_All()
		{
			var country = Country.ZM;

			IsNotValid("0", country);
		}

		#endregion

		private static void IsNotValid(string code, Country country)
		{
			Assert.IsFalse(PostalCode.IsValid(code, country), "Postal code '{0}' should be not valid for {1:f}.", code, country);
		}

		private static void IsNotValid(Country country, bool alfa, bool prefix, params int[] lengths)
		{
			// Length 1
			IsNotValid(country, "1", alfa, alfa, prefix, lengths);
			IsNotValid(country, "A", !alfa, alfa, prefix, lengths);

			// Length 2
			IsNotValid(country, "12", alfa, alfa, prefix, lengths);
			IsNotValid(country, "AB", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "A1", !alfa, alfa, prefix, lengths);

			// Length 3
			IsNotValid(country, "123", alfa, alfa, prefix, lengths);
			IsNotValid(country, "ABC", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "AB1", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "A12", !alfa, alfa, prefix, lengths);

			// Length 4
			IsNotValid(country, "1234", alfa, alfa, prefix, lengths);
			IsNotValid(country, "AB1C", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "AB12", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "A123", !alfa, alfa, prefix, lengths);

			// Length 5
			IsNotValid(country, "12345", alfa, alfa, prefix, lengths);
			IsNotValid(country, "AB12C", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "AB123", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "A1234", !alfa, alfa, prefix, lengths);

			// Length 6
			IsNotValid(country, "123456", alfa, alfa, prefix, lengths);
			IsNotValid(country, "AB123C", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "AB1234", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "A12345", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "A1B3C3", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "1234AB", !alfa, alfa, prefix, lengths);

			// Length 7
			IsNotValid(country, "1234567", alfa, alfa, prefix, lengths);
			IsNotValid(country, "AB1234C", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "AB12345", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "A123456", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "A1B3C3D", !alfa, alfa, prefix, lengths);

			// Length 8
			IsNotValid(country, "12345678", alfa, alfa, prefix, lengths);
			IsNotValid(country, "AB1234CD", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "AB123456", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "A1234567", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "A1B3C3D4", !alfa, alfa, prefix, lengths);

			// Length 9
			IsNotValid(country, "123456789", alfa, alfa, prefix, lengths);
			IsNotValid(country, "AB12345CD", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "AB1234567", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "A12345678", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "A1B3C3D4E", !alfa, alfa, prefix, lengths);

			// Length 10
			IsNotValid(country, "1234567890", alfa, alfa, prefix, lengths);
			IsNotValid(country, "AB123456CD", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "AB12345678", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "A123456789", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "A1B3C3D4EF", !alfa, alfa, prefix, lengths);

			// Length 11
			IsNotValid(country, "12345678901", alfa, alfa, prefix, lengths);
			IsNotValid(country, "AB1234567CD", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "AB123456789", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "A1234567890", !alfa, alfa, prefix, lengths);
			IsNotValid(country, "A1B3C3D4E5F", !alfa, alfa, prefix, lengths);
		}
		private static void IsNotValid(Country country, string code, bool test, bool alpha, bool prefix, int[] lengths)
		{
			var pcode = country.IsoAlpha2Code + code;

			var len = code.Length;
			var plen = pcode.Length;

			// General tests. These are test more than once (so be it).
			Assert.AreEqual(len > 1 && len <= 10, PostalCode.IsValid(code), "The postal code '{0}' should be {1}valid.", code, len > 1 && len <= 10 ? "" : "not ");
			Assert.AreEqual(plen <= 10, PostalCode.IsValid(country.IsoAlpha2Code + code), "The postal code '{1:2}{0}' should be {2}valid.", code, country, plen <= 10 ? "" : "not ");

			// Tests if the types differ (alphanumeric versus numeric) or if the lenghts differ.
			if (test || !lengths.Contains(len))
			{
				Assert.IsFalse(PostalCode.IsValid(code, country), "The postal code '{0}' should not be valid for {1:f (2)}.", code, country);
			}
			// Tests if the types differ (alphanumeric versus numeric), if the lenghts differ, or if prefix is not supported.
			// And if not testing for an alfa pattern, or an alfa pattern that does not match the concatenated code.
			if ((test || !prefix || !lengths.Contains(len)) && (!alpha || !lengths.Contains(plen)))
			{
				Assert.IsFalse(PostalCode.IsValid(country.IsoAlpha2Code + code, country), "The postal code '{1:2}{0}' should not be valid for {1:f (2)}.", code, country);
			}
		}
	}

	[Serializable]
	public class PostalCodeSerializeObject
	{
		public int Id { get; set; }
		public PostalCode Obj { get; set; }
		public DateTime Date { get; set; }
	}
}