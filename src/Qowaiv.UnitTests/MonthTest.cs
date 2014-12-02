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
	/// <summary>Tests the month SVO.</summary>
	[TestFixture]
	public class MonthTest
	{
		/// <summary>The test instance for most tests.</summary>
		public static readonly Month TestStruct = Month.February;

		#region month const tests

		/// <summary>Month.Empty should be equal to the default of month.</summary>
		[Test]
		public void Empty_None_EqualsDefault()
		{
			Assert.AreEqual(default(Month), Month.Empty);
		}

		[Test]
		public void All_None_EqualsDefault()
		{
			var exp = Enumerable.Range(1, 12)
			.Select(n => Month.Create(n))
			.ToArray();

			CollectionAssert.AreEqual(exp, Month.All);
		}

		#endregion

		#region month IsEmpty tests

		/// <summary>Month.IsEmpty() should be true for the default of month.</summary>
		[Test]
		public void IsEmpty_Default_IsTrue()
		{
			Assert.IsTrue(default(Month).IsEmpty());
		}
		/// <summary>Month.IsEmpty() should be false for Month.Unknown.</summary>
		[Test]
		public void IsEmpty_Unknown_IsFalse()
		{
			Assert.IsFalse(Month.Unknown.IsEmpty());
		}
		/// <summary>Month.IsEmpty() should be false for the TestStruct.</summary>
		[Test]
		public void IsEmpty_TestStruct_IsFalse()
		{
			Assert.IsFalse(TestStruct.IsEmpty());
		}

		/// <summary>Month.IsUnknown() should be false for the default of month.</summary>
		[Test]
		public void IsUnknown_Default_IsFalse()
		{
			Assert.IsFalse(default(Month).IsUnknown());
		}
		/// <summary>Month.IsUnknown() should be true for Month.Unknown.</summary>
		[Test]
		public void IsUnknown_Unknown_IsTrue()
		{
			Assert.IsTrue(Month.Unknown.IsUnknown());
		}
		/// <summary>Month.IsUnknown() should be false for the TestStruct.</summary>
		[Test]
		public void IsUnknown_TestStruct_IsFalse()
		{
			Assert.IsFalse(TestStruct.IsUnknown());
		}

		/// <summary>Month.IsEmptyOrUnknown() should be true for the default of month.</summary>
		[Test]
		public void IsEmptyOrUnknown_Default_IsFalse()
		{
			Assert.IsTrue(default(Month).IsEmptyOrUnknown());
		}
		/// <summary>Month.IsEmptyOrUnknown() should be true for Month.Unknown.</summary>
		[Test]
		public void IsEmptyOrUnknown_Unknown_IsTrue()
		{
			Assert.IsTrue(Month.Unknown.IsEmptyOrUnknown());
		}
		/// <summary>Month.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
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
			Month val;

			string str = null;

			Assert.IsTrue(Month.TryParse(str, out val), "Valid");
			Assert.AreEqual(string.Empty, val.ToString(), "Value");
		}

		/// <summary>TryParse string.Empty should be valid.</summary>
		[Test]
		public void TyrParse_StringEmpty_IsValid()
		{
			Month val;

			string str = string.Empty;

			Assert.IsTrue(Month.TryParse(str, out val), "Valid");
			Assert.AreEqual(string.Empty, val.ToString(), "Value");
		}

		/// <summary>TryParse "?" should be valid and the result should be Month.Unknown.</summary>
		[Test]
		public void TyrParse_Questionmark_IsValid()
		{
			Month val;

			string str = "?";

			Assert.IsTrue(Month.TryParse(str, out val), "Valid");
			Assert.IsTrue(val.IsUnknown(), "Value");
		}

		/// <summary>TryParse with specified string value should be valid.</summary>
		[Test]
		public void TyrParse_StringValue_IsValid()
		{
			Month val;

			string str = "December";

			Assert.IsTrue(Month.TryParse(str, out val), "Valid");
			Assert.AreEqual(Month.December, val, "Value");
		}

		/// <summary>TryParse with specified string value should be invalid.</summary>
		[Test]
		public void TyrParse_StringValue_IsNotValid()
		{
			Month val;

			string str = "string";

			Assert.IsFalse(Month.TryParse(str, out val), "Valid");
			Assert.AreEqual(string.Empty, val.ToString(), "Value");
		}

		[Test]
		public void Parse_Unknown_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var act = Month.Parse("?");
				var exp = Month.Unknown;
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
					Month.Parse("InvalidInput");
				},
				"Not a valid month");
			}
		}

		[Test]
		public void TryParse_TestStructInput_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = TestStruct;
				var act = Month.TryParse(exp.ToString());

				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void TryParse_NullCulture_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = TestStruct;

				Month act;
				Assert.IsTrue(Month.TryParse(exp.ShortName, null, out act));
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void TryParse_InvalidInput_DefaultValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = default(Month);
				var act = Month.TryParse("InvalidInput");

				Assert.AreEqual(exp, act);
			}
		}

		#endregion

		#region TryCreate tests

		[Test]
		public void TryCreate_Null_IsEmpty()
		{
			Month exp = Month.Empty;
			Month act;

			Assert.IsTrue(Month.TryCreate(null, out act));
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void TryCreate_ByteMinValue_IsEmpty()
		{
			Month exp = Month.Empty;
			Month act;

			Assert.IsFalse(Month.TryCreate(Byte.MinValue, out act));
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void TryCreate_ByteMinValue_AreEqual()
		{
			var exp = Month.Empty;
			var act = Month.TryCreate(Byte.MinValue);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void TryCreate_Value_AreEqual()
		{
			var exp = TestStruct;
			var act = Month.TryCreate(2);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Create_ByteMinValue_ThrowsArgumentOutOfRangeException()
		{
			ExceptionAssert.ExpectArgumentOutOfRangeException
			(() =>
			{
				Month.Create(Byte.MinValue);
			},
			"val",
			"Not a valid month");
		}

		#endregion

		#region (XML) (De)serialization tests

		[Test]
		public void Constructor_SerializationInfoIsNull_ThrowsArgumentNullException()
		{
			ExceptionAssert.ExpectArgumentNullException
			(() =>
			{
				SerializationTest.DeserializeUsingConstructor<Month>(null, default(StreamingContext));
			},
			"info");
		}

		[Test]
		public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
		{
			ExceptionAssert.ExpectException<SerializationException>
			(() =>
			{
				var info = new SerializationInfo(typeof(Month), new System.Runtime.Serialization.FormatterConverter());
				SerializationTest.DeserializeUsingConstructor<Month>(info, default(StreamingContext));
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
			var info = new SerializationInfo(typeof(Month), new System.Runtime.Serialization.FormatterConverter());
			obj.GetObjectData(info, default(StreamingContext));

			Assert.AreEqual((Byte)2, info.GetByte("Value"));
		}

		[Test]
		public void SerializeDeserialize_TestStruct_AreEqual()
		{
			var input = MonthTest.TestStruct;
			var exp = MonthTest.TestStruct;
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void DataContractSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = MonthTest.TestStruct;
			var exp = MonthTest.TestStruct;
			var act = SerializationTest.DataContractSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void XmlSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = MonthTest.TestStruct;
			var exp = MonthTest.TestStruct;
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void SerializeDeserialize_MonthSerializeObject_AreEqual()
		{
			var input = new MonthSerializeObject()
			{
				Id = 17,
				Obj = MonthTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new MonthSerializeObject()
			{
				Id = 17,
				Obj = MonthTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			Assert.AreEqual(exp.Date, act.Date, "Date");
		}
		[Test]
		public void XmlSerializeDeserialize_MonthSerializeObject_AreEqual()
		{
			var input = new MonthSerializeObject()
			{
				Id = 17,
				Obj = MonthTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new MonthSerializeObject()
			{
				Id = 17,
				Obj = MonthTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			Assert.AreEqual(exp.Date, act.Date, "Date");
		}
		[Test]
		public void DataContractSerializeDeserialize_MonthSerializeObject_AreEqual()
		{
			var input = new MonthSerializeObject()
			{
				Id = 17,
				Obj = MonthTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new MonthSerializeObject()
			{
				Id = 17,
				Obj = MonthTest.TestStruct,
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
			var input = new MonthSerializeObject()
			{
				Id = 17,
				Obj = MonthTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new MonthSerializeObject()
			{
				Id = 17,
				Obj = MonthTest.TestStruct,
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
			var input = new MonthSerializeObject()
			{
				Id = 17,
				Obj = Month.Empty,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new MonthSerializeObject()
			{
				Id = 17,
				Obj = Month.Empty,
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
			var act = JsonTester.Read<Month>();
			var exp = Month.Empty;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_InvalidStringValue_AssertFormatException()
		{
			ExceptionAssert.ExpectException<FormatException>(() =>
			{
				JsonTester.Read<Month>("InvalidStringValue");
			},
			"Not a valid month");
		}
		[Test]
		public void FromJson_StringValue_AreEqual()
		{
			var act = JsonTester.Read<Month>(TestStruct.ToString(CultureInfo.InvariantCulture));
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_Int64Value_AreEqual()
		{
			var act = JsonTester.Read<Month>((Int64)TestStruct);
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_DoubleValue_AreEqual()
		{
			var act = JsonTester.Read<Month>((Double)TestStruct);
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_DateTimeValue_AssertNotSupportedException()
		{
			ExceptionAssert.ExpectException<NotSupportedException>(() =>
			{
				JsonTester.Read<Month>(new DateTime(1972, 02, 14));
			},
			"JSON deserialization from a date is not supported.");
		}

		[Test]
		public void ToJson_DefaultValue_AreEqual()
		{
			object act = JsonTester.Write(default(Month));
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
			var act = Month.Empty.ToString();
			var exp = "";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_Unknown_QuestionMark()
		{
			var act = Month.Unknown.ToString();
			var exp = "?";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_CustomFormatter_SupportsCustomFormatting()
		{
			var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
			var exp = "Unit Test Formatter, value: '2', format: 'Unit Test Format'";

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_TestStructf_FormattedString()
		{
			using (new CultureInfoScope(CultureInfo.InvariantCulture))
			{
				var act = TestStruct.ToString("f");
				var exp = "February";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_TestStructs_FormattedString()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = TestStruct.ToString("s");
				var exp = "feb";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_TestStructMUpper_FormattedString()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = TestStruct.ToString("M");
				var exp = "2";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_TestStructMLower_FormattedString()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = TestStruct.ToString("m");
				var exp = "02";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_EmptyMLower_FormattedString()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = Month.Empty.ToString("m");
				var exp = "";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void DebuggerDisplay_DebugToString_HasAttribute()
		{
			DebuggerDisplayAssert.HasAttribute(typeof(Month));
		}

		[Test]
		public void DebuggerDisplay_DefaultValue_String()
		{
			DebuggerDisplayAssert.HasResult("Month: (empty)", default(Month));
		}

		[Test]
		public void DebuggerDisplay_Unknown_String()
		{
			DebuggerDisplayAssert.HasResult("Month: (unknown)", Month.Unknown);
		}

		[Test]
		public void DebuggerDisplay_TestStruct_String()
		{
			DebuggerDisplayAssert.HasResult("Month: February (02)", TestStruct);
		}

		#endregion

		#region IEquatable tests

		/// <summary>GetHash should not fail for Month.Empty.</summary>
		[Test]
		public void GetHash_Empty_Hash()
		{
			Assert.AreEqual(0, Month.Empty.GetHashCode());
		}

		/// <summary>GetHash should not fail for the test struct.</summary>
		[Test]
		public void GetHash_TestStruct_Hash()
		{
			Assert.AreEqual(2, MonthTest.TestStruct.GetHashCode());
		}

		[Test]
		public void Equals_EmptyEmpty_IsTrue()
		{
			Assert.IsTrue(Month.Empty.Equals(Month.Empty));
		}

		[Test]
		public void Equals_FormattedAndUnformatted_IsTrue()
		{
			var l = Month.Parse("February", CultureInfo.InvariantCulture);
			var r = Month.Parse("02", CultureInfo.InvariantCulture);

			Assert.IsTrue(l.Equals(r));
		}

		[Test]
		public void Equals_TestStructTestStruct_IsTrue()
		{
			Assert.IsTrue(MonthTest.TestStruct.Equals(MonthTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructEmpty_IsFalse()
		{
			Assert.IsFalse(MonthTest.TestStruct.Equals(Month.Empty));
		}

		[Test]
		public void Equals_EmptyTestStruct_IsFalse()
		{
			Assert.IsFalse(Month.Empty.Equals(MonthTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructObjectTestStruct_IsTrue()
		{
			Assert.IsTrue(MonthTest.TestStruct.Equals((object)MonthTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructNull_IsFalse()
		{
			Assert.IsFalse(MonthTest.TestStruct.Equals(null));
		}

		[Test]
		public void Equals_TestStructObject_IsFalse()
		{
			Assert.IsFalse(MonthTest.TestStruct.Equals(new object()));
		}

		[Test]
		public void OperatorIs_TestStructTestStruct_IsTrue()
		{
			var l = MonthTest.TestStruct;
			var r = MonthTest.TestStruct;
			Assert.IsTrue(l == r);
		}

		[Test]
		public void OperatorIsNot_TestStructTestStruct_IsFalse()
		{
			var l = MonthTest.TestStruct;
			var r = MonthTest.TestStruct;
			Assert.IsFalse(l != r);
		}

		#endregion

		#region IComparable tests

		/// <summary>Orders a list of months ascending.</summary>
		[Test]
		public void OrderBy_Month_AreEqual()
		{
			var item0 = Month.January;
			var item1 = Month.March;
			var item2 = Month.April;
			var item3 = Month.December;

			var inp = new List<Month>() { Month.Empty, item3, item2, item0, item1, Month.Empty };
			var exp = new List<Month>() { Month.Empty, Month.Empty, item0, item1, item2, item3 };
			var act = inp.OrderBy(item => item).ToList();

			CollectionAssert.AreEqual(exp, act);
		}

		/// <summary>Orders a list of months descending.</summary>
		[Test]
		public void OrderByDescending_Month_AreEqual()
		{
			var item0 = Month.January;
			var item1 = Month.March;
			var item2 = Month.April;
			var item3 = Month.December;

			var inp = new List<Month>() { Month.Empty, item3, item2, item0, item1, Month.Empty };
			var exp = new List<Month>() { item3, item2, item1, item0, Month.Empty, Month.Empty };
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
				"Argument must be a month"
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
				"Argument must be a month"
			);
		}

		[Test]
		public void LessThan_17LT19_IsTrue()
		{
			Month l = 7;
			Month r = 9;

			Assert.IsTrue(l < r);
		}
		[Test]
		public void GreaterThan_21LT19_IsTrue()
		{
			Month l = 2;
			Month r = 1;

			Assert.IsTrue(l > r);
		}

		[Test]
		public void LessThanOrEqual_17LT19_IsTrue()
		{
			Month l = 7;
			Month r = 9;

			Assert.IsTrue(l <= r);
		}
		[Test]
		public void GreaterThanOrEqual_21LT19_IsTrue()
		{
			Month l = 2;
			Month r = 1;

			Assert.IsTrue(l >= r);
		}

		[Test]
		public void LessThanOrEqual_17LT17_IsTrue()
		{
			Month l = 7;
			Month r = 7;

			Assert.IsTrue(l <= r);
		}
		[Test]
		public void GreaterThanOrEqual_21LT21_IsTrue()
		{
			Month l = 12;
			Month r = 12;

			Assert.IsTrue(l >= r);
		}
		#endregion

		#region Casting tests

		[Test]
		public void Explicit_StringToMonth_AreEqual()
		{
			var exp = TestStruct;
			var act = (Month)TestStruct.ToString();

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_MonthToString_AreEqual()
		{
			var exp = TestStruct.ToString();
			var act = (string)TestStruct;

			Assert.AreEqual(exp, act);
		}

		#endregion

		#region Properties

		[Test]
		public void FullName_Empty_AreEqual()
		{
			using (new CultureInfoScope("es-EC"))
			{
				var act = Month.Empty.FullName;
				var exp = "";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void FullName_Unknown_AreEqual()
		{
			using (new CultureInfoScope("es-EC"))
			{
				var act = Month.Unknown.FullName;
				var exp = "?";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void FullName_TestStruct_AreEqual()
		{
			using (new CultureInfoScope("es-EC"))
			{
				var act = TestStruct.FullName;
				var exp = "febrero";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ShortName_Empty_AreEqual()
		{
			using (new CultureInfoScope("es-EC"))
			{
				var act = Month.Empty.ShortName;
				var exp = "";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ShortName_Unknown_AreEqual()
		{
			using (new CultureInfoScope("es-EC"))
			{
				var act = Month.Unknown.ShortName;
				var exp = "?";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ShortName_TestStruct_AreEqual()
		{
			using (new CultureInfoScope("es-EC"))
			{
				var act = TestStruct.ShortName;
				var exp = "feb";
				Assert.AreEqual(exp, act);
			}
		}


		#endregion

		#region Methods

		[Test]
		public void GetFullName_Empty_AreEqual()
		{
			using (new CultureInfoScope("es-EC"))
			{
				var act = Month.Empty.GetFullName(null);
				var exp = "";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void GetShortName_Empty_AreEqual()
		{
			using (new CultureInfoScope("es-EC"))
			{
				var act = Month.Empty.GetShortName(null);
				var exp = "";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void GetFullName_TestStructNullProvider_AreEqual()
		{
			var act = TestStruct.GetFullName(null);
			var exp = "February";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void GetShortName_TestStructNullProvider_AreEqual()
		{
			var act = TestStruct.GetShortName(null);
			var exp = "Feb";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void GetFullName_TestStruct_AreEqual()
		{
			var act = TestStruct.GetFullName(new CultureInfo("es-EC"));
			var exp = "febrero";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void GetShortName_TestStruct_AreEqual()
		{
			var act = TestStruct.GetShortName(new CultureInfo("es-EC"));
			var exp = "feb";
			Assert.AreEqual(exp, act);
		}

		#endregion

		#region Type converter tests

		[Test]
		public void ConverterExists_Month_IsTrue()
		{
			TypeConverterAssert.ConverterExists(typeof(Month));
		}

		[Test]
		public void CanNotConvertFromInt32_Month_IsTrue()
		{
			TypeConverterAssert.CanNotConvertFrom(typeof(Month), typeof(Int32));
		}
		[Test]
		public void CanNotConvertToInt32_Month_IsTrue()
		{
			TypeConverterAssert.CanNotConvertTo(typeof(Month), typeof(Int32));
		}

		[Test]
		public void CanConvertFromString_Month_IsTrue()
		{
			TypeConverterAssert.CanConvertFromString(typeof(Month));
		}

		[Test]
		public void CanConvertToString_Month_IsTrue()
		{
			TypeConverterAssert.CanConvertToString(typeof(Month));
		}

		[Test]
		public void ConvertFrom_StringNull_MonthEmpty()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(Month.Empty, (string)null);
			}
		}

		[Test]
		public void ConvertFrom_StringEmpty_MonthEmpty()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(Month.Empty, string.Empty);
			}
		}

		[Test]
		public void ConvertFromString_StringValue_TestStruct()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(MonthTest.TestStruct, MonthTest.TestStruct.ToString());
			}
		}

		[Test]
		public void ConvertFromInstanceDescriptor_Month_Successful()
		{
			TypeConverterAssert.ConvertFromInstanceDescriptor(typeof(Month));
		}

		[Test]
		public void ConvertToString_TestStruct_StringValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertToStringEquals(MonthTest.TestStruct.ToString(), MonthTest.TestStruct);
			}
		}

		#endregion

		#region IsValid tests

		[Test]
		public void IsValid_Data_IsFalse()
		{
			Assert.IsFalse(Month.IsValid("0"), "0");
			Assert.IsFalse(Month.IsValid((String)null), "(String)null");
			Assert.IsFalse(Month.IsValid(String.Empty), "String.Empty");

			Assert.IsFalse(Month.IsValid((System.Byte?)null), "(System.Byte?)null");
		}
		[Test]
		public void IsValid_Data_IsTrue()
		{
			Assert.IsTrue(Month.IsValid("11"), "11 on current culture.");
			Assert.IsTrue(Month.IsValid("April"), "April on current culture.");
			Assert.IsTrue(Month.IsValid("December", null), "December on no culture.");
			Assert.IsTrue(Month.IsValid("mei", new CultureInfo("nl-NL")), "mei on Dutch culture.");
			Assert.IsTrue(Month.IsValid("March", new CultureInfo("nl-NL")), "March on Dutch culture.");
		}
		#endregion
	}

	[Serializable]
	public class MonthSerializeObject
	{
		public int Id { get; set; }
		public Month Obj { get; set; }
		public DateTime Date { get; set; }
	}
}
