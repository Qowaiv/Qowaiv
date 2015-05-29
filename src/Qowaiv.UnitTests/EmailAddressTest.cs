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
	/// <summary>Tests the email address SVO.</summary>
	[TestFixture]
	public class EmailAddressTest
	{
		/// <summary>The test instance for most tests.</summary>
		public static readonly EmailAddress TestStruct = EmailAddress.Parse("svo@qowaiv.org");

		#region email address const tests

		/// <summary>EmailAddress.Empty should be equal to the default of email address.</summary>
		[Test]
		public void Empty_None_EqualsDefault()
		{
			Assert.AreEqual(default(EmailAddress), EmailAddress.Empty);
		}

		#endregion

		#region email address IsEmpty tests

		/// <summary>EmailAddress.IsEmpty() should be true for the default of email address.</summary>
		[Test]
		public void IsEmpty_Default_IsTrue()
		{
			Assert.IsTrue(default(EmailAddress).IsEmpty());
		}
		/// <summary>EmailAddress.IsEmpty() should be false for EmailAddress.Unknown.</summary>
		[Test]
		public void IsEmpty_Unknown_IsFalse()
		{
			Assert.IsFalse(EmailAddress.Unknown.IsEmpty());
		}
		/// <summary>EmailAddress.IsEmpty() should be false for the TestStruct.</summary>
		[Test]
		public void IsEmpty_TestStruct_IsFalse()
		{
			Assert.IsFalse(TestStruct.IsEmpty());
		}

		/// <summary>EmailAddress.IsUnknown() should be false for the default of email address.</summary>
		[Test]
		public void IsUnknown_Default_IsFalse()
		{
			Assert.IsFalse(default(EmailAddress).IsUnknown());
		}
		/// <summary>EmailAddress.IsUnknown() should be true for EmailAddress.Unknown.</summary>
		[Test]
		public void IsUnknown_Unknown_IsTrue()
		{
			Assert.IsTrue(EmailAddress.Unknown.IsUnknown());
		}
		/// <summary>EmailAddress.IsUnknown() should be false for the TestStruct.</summary>
		[Test]
		public void IsUnknown_TestStruct_IsFalse()
		{
			Assert.IsFalse(TestStruct.IsUnknown());
		}

		/// <summary>EmailAddress.IsEmptyOrUnknown() should be true for the default of email address.</summary>
		[Test]
		public void IsEmptyOrUnknown_Default_IsFalse()
		{
			Assert.IsTrue(default(EmailAddress).IsEmptyOrUnknown());
		}
		/// <summary>EmailAddress.IsEmptyOrUnknown() should be true for EmailAddress.Unknown.</summary>
		[Test]
		public void IsEmptyOrUnknown_Unknown_IsTrue()
		{
			Assert.IsTrue(EmailAddress.Unknown.IsEmptyOrUnknown());
		}
		/// <summary>EmailAddress.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
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
			EmailAddress val;

			string str = null;

			Assert.IsTrue(EmailAddress.TryParse(str, out val), "Valid");
			Assert.AreEqual(string.Empty, val.ToString(), "Value");
		}

		/// <summary>TryParse string.Empty should be valid.</summary>
		[Test]
		public void TyrParse_StringEmpty_IsValid()
		{
			EmailAddress val;

			string str = string.Empty;

			Assert.IsTrue(EmailAddress.TryParse(str, out val), "Valid");
			Assert.AreEqual(string.Empty, val.ToString(), "Value");
		}

		/// <summary>TryParse with specified string value should be valid.</summary>
		[Test]
		public void TyrParse_StringValue_IsValid()
		{
			EmailAddress val;

			string str = "svo@qowaiv.org";

			Assert.IsTrue(EmailAddress.TryParse(str, out val), "Valid");
			Assert.AreEqual(str, val.ToString(), "Value");
		}

		/// <summary>TryParse with specified string value should be invalid.</summary>
		[Test]
		public void TyrParse_StringValue_IsNotValid()
		{
			EmailAddress val;

			string str = "string";

			Assert.IsFalse(EmailAddress.TryParse(str, out val), "Valid");
			Assert.AreEqual(string.Empty, val.ToString(), "Value");
		}

		[Test]
		public void Parse_Unknown_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var act = EmailAddress.Parse("?");
				var exp = EmailAddress.Unknown;
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
					EmailAddress.Parse("InvalidInput");
				},
				"Not a valid email address");
			}
		}

		[Test]
		public void TryParse_TestStructInput_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = TestStruct;
				var act = EmailAddress.TryParse(exp.ToString());

				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void TryParse_InvalidInput_DefaultValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = default(EmailAddress);
				var act = EmailAddress.TryParse("InvalidInput");

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
				SerializationTest.DeserializeUsingConstructor<EmailAddress>(null, default(StreamingContext));
			},
			"info");
		}

		[Test]
		public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
		{
			ExceptionAssert.ExpectException<SerializationException>
			(() =>
			{
				var info = new SerializationInfo(typeof(EmailAddress), new System.Runtime.Serialization.FormatterConverter());
				SerializationTest.DeserializeUsingConstructor<EmailAddress>(info, default(StreamingContext));
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
			var info = new SerializationInfo(typeof(EmailAddress), new System.Runtime.Serialization.FormatterConverter());
			obj.GetObjectData(info, default(StreamingContext));

			Assert.AreEqual("svo@qowaiv.org", info.GetString("Value"));
		}

		[Test]
		public void SerializeDeserialize_TestStruct_AreEqual()
		{
			var input = EmailAddressTest.TestStruct;
			var exp = EmailAddressTest.TestStruct;
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void DataContractSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = EmailAddressTest.TestStruct;
			var exp = EmailAddressTest.TestStruct;
			var act = SerializationTest.DataContractSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void XmlSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = EmailAddressTest.TestStruct;
			var exp = EmailAddressTest.TestStruct;
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void SerializeDeserialize_EmailAddressSerializeObject_AreEqual()
		{
			var input = new EmailAddressSerializeObject()
			{
				Id = 17,
				Obj = EmailAddressTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new EmailAddressSerializeObject()
			{
				Id = 17,
				Obj = EmailAddressTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			Assert.AreEqual(exp.Date, act.Date, "Date");
		}
		[Test]
		public void XmlSerializeDeserialize_EmailAddressSerializeObject_AreEqual()
		{
			var input = new EmailAddressSerializeObject()
			{
				Id = 17,
				Obj = EmailAddressTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new EmailAddressSerializeObject()
			{
				Id = 17,
				Obj = EmailAddressTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			Assert.AreEqual(exp.Date, act.Date, "Date");
		}
		[Test]
		public void DataContractSerializeDeserialize_EmailAddressSerializeObject_AreEqual()
		{
			var input = new EmailAddressSerializeObject()
			{
				Id = 17,
				Obj = EmailAddressTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new EmailAddressSerializeObject()
			{
				Id = 17,
				Obj = EmailAddressTest.TestStruct,
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
			var input = new EmailAddressSerializeObject()
			{
				Id = 17,
				Obj = EmailAddressTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new EmailAddressSerializeObject()
			{
				Id = 17,
				Obj = EmailAddressTest.TestStruct,
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
			var input = new EmailAddressSerializeObject()
			{
				Id = 17,
				Obj = EmailAddress.Empty,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new EmailAddressSerializeObject()
			{
				Id = 17,
				Obj = EmailAddress.Empty,
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
			var act = JsonTester.Read<EmailAddress>();
			var exp = EmailAddress.Empty;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_InvalidStringValue_AssertFormatException()
		{
			ExceptionAssert.ExpectException<FormatException>(() =>
			{
				JsonTester.Read<EmailAddress>("InvalidStringValue");
			},
			"Not a valid email address");
		}
		[Test]
		public void FromJson_StringValue_AreEqual()
		{
			var act = JsonTester.Read<EmailAddress>(TestStruct.ToString(CultureInfo.InvariantCulture));
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_Int64Value_AssertNotSupportedException()
		{
			ExceptionAssert.ExpectException<NotSupportedException>(() =>
			{
				JsonTester.Read<EmailAddress>(123456L);
			},
			"JSON deserialization from an integer is not supported.");
		}

		[Test]
		public void FromJson_DoubleValue_AssertNotSupportedException()
		{
			ExceptionAssert.ExpectException<NotSupportedException>(() =>
			{
				JsonTester.Read<EmailAddress>(1234.56);
			},
			"JSON deserialization from a number is not supported.");
		}

		[Test]
		public void FromJson_DateTimeValue_AssertNotSupportedException()
		{
			ExceptionAssert.ExpectException<NotSupportedException>(() =>
			{
				JsonTester.Read<EmailAddress>(new DateTime(1972, 02, 14));
			},
			"JSON deserialization from a date is not supported.");
		}

		[Test]
		public void ToJson_DefaultValue_AreEqual()
		{
			object act = JsonTester.Write(default(EmailAddress));
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
			var act = EmailAddress.Empty.ToString();
			var exp = "";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_CustomFormatter_SupportsCustomFormatting()
		{
			var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
			var exp = "Unit Test Formatter, value: 'svo@qowaiv.org', format: 'Unit Test Format'";

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_EmptyFormatF_AreEqual()
		{
			var act = EmailAddress.Empty.ToString(@"f");
			var exp = "";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_TestStructFormatMailtoF_AreEqual()
		{
			var act = TestStruct.ToString(@"mai\lto:f");
			var exp = "mailto:svo@qowaiv.org";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_TestStructFormatU_AreEqual()
		{
			var act = TestStruct.ToString("U");
			var exp = "SVO@QOWAIV.ORG";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_TestStructFormatLLowerAtd_ComplexPatternAreEqual()
		{
			var act = TestStruct.ToString("l[at]d");
			var exp = "svo[at]qowaiv.org";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_TestStructFormatUpperLAtD_ComplexPatternAreEqual()
		{
			var act = TestStruct.ToString("L[at]D");
			var exp = "SVO[at]QOWAIV.ORG";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void DebuggerDisplay_DebugToString_HasAttribute()
		{
			DebuggerDisplayAssert.HasAttribute(typeof(EmailAddress));
		}

		[Test]
		public void DebuggerDisplay_DefaultValue_String()
		{
			DebuggerDisplayAssert.HasResult("EmailAddress: (empty)", default(EmailAddress));
		}

		[Test]
		public void DebuggerDisplay_Unknown_String()
		{
			DebuggerDisplayAssert.HasResult("EmailAddress: (unknown)", EmailAddress.Unknown);
		}

		[Test]
		public void DebuggerDisplay_TestStruct_String()
		{
			DebuggerDisplayAssert.HasResult("EmailAddress: svo@qowaiv.org", TestStruct);
		}

		#endregion

		#region IEquatable tests

		/// <summary>GetHash should not fail for EmailAddress.Empty.</summary>
		[Test]
		public void GetHash_Empty_Hash()
		{
			Assert.AreEqual(0, EmailAddress.Empty.GetHashCode());
		}

		/// <summary>GetHash should not fail for the test struct.</summary>
		[Test]
		public void GetHash_TestStruct_Hash()
		{
			Assert.AreEqual(624335706, EmailAddressTest.TestStruct.GetHashCode());
		}

		[Test]
		public void Equals_EmptyEmpty_IsTrue()
		{
			Assert.IsTrue(EmailAddress.Empty.Equals(EmailAddress.Empty));
		}

		[Test]
		public void Equals_FormattedAndUnformatted_IsTrue()
		{
			var l = EmailAddress.Parse("svo@qowaiv.org", CultureInfo.InvariantCulture);
			var r = EmailAddress.Parse("SVO@Qowaiv.org", CultureInfo.InvariantCulture);

			Assert.IsTrue(l.Equals(r));
		}

		[Test]
		public void Equals_TestStructTestStruct_IsTrue()
		{
			Assert.IsTrue(EmailAddressTest.TestStruct.Equals(EmailAddressTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructEmpty_IsFalse()
		{
			Assert.IsFalse(EmailAddressTest.TestStruct.Equals(EmailAddress.Empty));
		}

		[Test]
		public void Equals_EmptyTestStruct_IsFalse()
		{
			Assert.IsFalse(EmailAddress.Empty.Equals(EmailAddressTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructObjectTestStruct_IsTrue()
		{
			Assert.IsTrue(EmailAddressTest.TestStruct.Equals((object)EmailAddressTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructNull_IsFalse()
		{
			Assert.IsFalse(EmailAddressTest.TestStruct.Equals(null));
		}

		[Test]
		public void Equals_TestStructObject_IsFalse()
		{
			Assert.IsFalse(EmailAddressTest.TestStruct.Equals(new object()));
		}

		[Test]
		public void OperatorIs_TestStructTestStruct_IsTrue()
		{
			var l = EmailAddressTest.TestStruct;
			var r = EmailAddressTest.TestStruct;
			Assert.IsTrue(l == r);
		}

		[Test]
		public void OperatorIsNot_TestStructTestStruct_IsFalse()
		{
			var l = EmailAddressTest.TestStruct;
			var r = EmailAddressTest.TestStruct;
			Assert.IsFalse(l != r);
		}

		#endregion

		#region IComparable tests

		/// <summary>Orders a list of email addresss ascending.</summary>
		[Test]
		public void OrderBy_EmailAddress_AreEqual()
		{
			var item0 = EmailAddress.Parse("a@qowaiv.org");
			var item1 = EmailAddress.Parse("b@qowaiv.org");
			var item2 = EmailAddress.Parse("c@qowaiv.org");
			var item3 = EmailAddress.Parse("d@qowaiv.org");

			var inp = new List<EmailAddress>() { EmailAddress.Empty, item3, item2, item0, item1, EmailAddress.Empty };
			var exp = new List<EmailAddress>() { EmailAddress.Empty, EmailAddress.Empty, item0, item1, item2, item3 };
			var act = inp.OrderBy(item => item).ToList();

			CollectionAssert.AreEqual(exp, act);
		}

		/// <summary>Orders a list of email addresss descending.</summary>
		[Test]
		public void OrderByDescending_EmailAddress_AreEqual()
		{
			var item0 = EmailAddress.Parse("a@qowaiv.org");
			var item1 = EmailAddress.Parse("b@qowaiv.org");
			var item2 = EmailAddress.Parse("c@qowaiv.org");
			var item3 = EmailAddress.Parse("d@qowaiv.org");

			var inp = new List<EmailAddress>() { EmailAddress.Empty, item3, item2, item0, item1, EmailAddress.Empty };
			var exp = new List<EmailAddress>() { item3, item2, item1, item0, EmailAddress.Empty, EmailAddress.Empty };
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
				"Argument must be an email address"
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
				"Argument must be an email address"
			);
		}
		#endregion

		#region Casting tests

		[Test]
		public void Explicit_StringToEmailAddress_AreEqual()
		{
			var exp = TestStruct;
			var act = (EmailAddress)TestStruct.ToString();

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_EmailAddressToString_AreEqual()
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
			var act = EmailAddress.Empty.Length;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Length_Unknown_0()
		{
			var exp = 0;
			var act = EmailAddress.Unknown.Length;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Length_TestStruct_IntValue()
		{
			var exp = 14;
			var act = TestStruct.Length;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Local_Empty_StringEmpty()
		{
			var email = EmailAddress.Empty;
			Assert.AreEqual(string.Empty, email.Local);
		}

		[Test]
		public void Local_Unknown_StringEmpty()
		{
			var email = EmailAddress.Unknown;
			Assert.AreEqual(string.Empty, email.Local);
		}

		[Test]
		public void Local_TestStruct_Info()
		{
			var exp = "svo";
			var act = TestStruct.Local;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Domain_Empty_StringEmpty()
		{
			var email = EmailAddress.Empty;
			Assert.AreEqual(string.Empty, email.Domain);
		}

		[Test]
		public void Domain_Unknown_StringEmpty()
		{
			var email = EmailAddress.Unknown;
			Assert.AreEqual(string.Empty, email.Domain);
		}

		[Test]
		public void Domain_TestStruct_QowaivOrg()
		{
			var exp = "qowaiv.org";
			var act = TestStruct.Domain;
			Assert.AreEqual(exp, act);
		}

		#endregion

		#region Type converter tests

		[Test]
		public void ConverterExists_EmailAddress_IsTrue()
		{
			TypeConverterAssert.ConverterExists(typeof(EmailAddress));
		}

		[Test]
		public void CanNotConvertFromInt32_EmailAddress_IsTrue()
		{
			TypeConverterAssert.CanNotConvertFrom(typeof(EmailAddress), typeof(Int32));
		}
		[Test]
		public void CanNotConvertToInt32_EmailAddress_IsTrue()
		{
			TypeConverterAssert.CanNotConvertTo(typeof(EmailAddress), typeof(Int32));
		}

		[Test]
		public void CanConvertFromString_EmailAddress_IsTrue()
		{
			TypeConverterAssert.CanConvertFromString(typeof(EmailAddress));
		}

		[Test]
		public void CanConvertToString_EmailAddress_IsTrue()
		{
			TypeConverterAssert.CanConvertToString(typeof(EmailAddress));
		}

		[Test]
		public void ConvertFrom_StringNull_EmailAddressEmpty()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(EmailAddress.Empty, (string)null);
			}
		}

		[Test]
		public void ConvertFrom_StringEmpty_EmailAddressEmpty()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(EmailAddress.Empty, string.Empty);
			}
		}

		[Test]
		public void ConvertFromString_StringValue_TestStruct()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(EmailAddressTest.TestStruct, EmailAddressTest.TestStruct.ToString());
			}
		}

		[Test]
		public void ConvertFromInstanceDescriptor_EmailAddress_Successful()
		{
			TypeConverterAssert.ConvertFromInstanceDescriptor(typeof(EmailAddress));
		}

		[Test]
		public void ConvertToString_TestStruct_StringValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertToStringEquals(EmailAddressTest.TestStruct.ToString(), EmailAddressTest.TestStruct);
			}
		}

		#endregion

		#region IsValid tests

		[Test]
		public void IsValid_Data_IsFalse()
		{
			Assert.IsFalse(EmailAddress.IsValid(String.Empty), "String.Empty");
			Assert.IsFalse(EmailAddress.IsValid((String)null), "(String)null");

			foreach (var email in new string[] { 
				"..@test.com", 
				".a@test.com", 
				"ab@sd@dd", 
				".@s.dd", 
				"ab@01.120.150.1", 
				"ab@88.120.150.021", 
				"ab@88.120.150.01", 
				"ab@988.120.150.10", 
				"ab@120.256.256.120", 
				"ab@120.25.1111.120", 
				"ab@[188.120.150.10",
				"ab@188.120.150.10]",
				"ab@[188.120.150.10].com", 
				"a@b.-de.cc", 
				"a@bde-.cc", 
				"a@bde.c-c", 
				"a@bde.cc.", 
				"ab@b+de.cc",
				"a..b@bde.cc", 
				"_@bde.cc,"
			})
			{
				Assert.IsFalse(EmailAddress.IsValid(email), email);
			}
		}

		[Test]
		public void IsValid_Data_IsTrue()
		{
			foreach (var email in new string[] { 
				"w@com",
				"w.b.f@test.com",
				"w.b.f@test.museum",
				"a.a@test.com",
				"ab@288.120.150.10.com",
				"ab@188.120.150.10",
				"ab@1.0.0.10",
				"ab@120.25.254.120",
				"ab@[120.254.254.120]",
				"2@bde.cc", 
				"-@bde.cc", 
				"a2@bde.cc",
				"a-b@bde.cc",
				"ab@b-de.cc",
				"a+b@bde.cc",
				"f.f.f@bde.cc",
				"ab_c@bde.cc",
				"_-_@bde.cc",
				"k.haak@12move.nl",
				"K.HAAK@12MOVE.NL"
			})
			{
				Assert.IsTrue(EmailAddress.IsValid(email), email);
			}
		}
		#endregion
	}

	[Serializable]
	public class EmailAddressSerializeObject
	{
		public int Id { get; set; }
		public EmailAddress Obj { get; set; }
		public DateTime Date { get; set; }
	}
}
