using NUnit.Framework;
using Qowaiv.Formatting;
using Qowaiv.UnitTests.TestTools;
using Qowaiv.UnitTests.TestTools.Globalization;
using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Qowaiv.UnitTests.Formatting
{
	/// <summary>Tests the formatting arguments SVO.</summary>
	[TestFixture]
	public class FormattingArgumentsTest
	{
		/// <summary>The test instance for most tests.</summary>
		public static readonly FormattingArguments TestStruct = new FormattingArguments("0.000", new CultureInfo("fr-BE"));

		[Test]
		public void ToString_IFormattableNull_IsNull()
		{
			String act = TestStruct.ToString((IFormattable)null);
			String exp = null;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void ToString_ObjectNull_IsNull()
		{
			String act = TestStruct.ToString((Object)null);
			String exp = null;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void ToString_TypeInt32_SystemInt32()
		{
			String act = TestStruct.ToString((Object)typeof(Int32));
			String exp = "System.Int32";

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void ToString_7_7Comma000()
		{
			String act = TestStruct.ToString((Object)7);
			String exp = "7,000";

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void ToString_7_7Dot000()
		{
			using (new CultureInfoScope("en-GB"))
			{
				String act = new FormattingArguments("0.000").ToString((Object)7);
				String exp = "7.000";

				Assert.AreEqual(exp, act);
			}
		}

		#region formatting arguments const tests

		/// <summary>FormattableArguments.None should be equal to the default of formatting arguments.</summary>
		[Test]
		public void None_None_EqualsDefault()
		{
			Assert.AreEqual(default(FormattingArguments), FormattingArguments.None);
		}

		#endregion

		#region (De)serialization tests

		[Test]
		public void Constructor_SerializationInfoIsNull_ThrowsArgumentNullException()
		{
			ExceptionAssert.CatchArgumentNullException
			(() =>
			{
				SerializationTest.DeserializeUsingConstructor<FormattingArguments>(null, default(StreamingContext));
			},
			"info");
		}

		[Test]
		public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
		{
			Assert.Catch<SerializationException>
			(() =>
			{
				var info = new SerializationInfo(typeof(FormattingArguments), new System.Runtime.Serialization.FormatterConverter());
				SerializationTest.DeserializeUsingConstructor<FormattingArguments>(info, default(StreamingContext));
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
			var info = new SerializationInfo(typeof(FormattingArguments), new System.Runtime.Serialization.FormatterConverter());
			obj.GetObjectData(info, default(StreamingContext));

			Assert.AreEqual("0.000", info.GetString("Format"));
			Assert.AreEqual(new CultureInfo("fr-BE"), info.GetValue("FormatProvider", typeof(IFormatProvider)));
		}

		[Test]
		public void SerializeDeserialize_TestStruct_AreEqual()
		{
			var input = FormattingArgumentsTest.TestStruct;
			var exp = FormattingArgumentsTest.TestStruct;
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void SerializeDeserialize_FormattableArgumentsSerializeObject_AreEqual()
		{
			var input = new FormattableArgumentsSerializeObject()
			{
				Id = 17,
				Obj = FormattingArgumentsTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new FormattableArgumentsSerializeObject()
			{
				Id = 17,
				Obj = FormattingArgumentsTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			DateTimeAssert.AreEqual(exp.Date, act.Date, "Date");;
		}

		[Test]
		public void SerializeDeserialize_Empty_AreEqual()
		{
			var input = new FormattableArgumentsSerializeObject()
			{
				Id = 17,
				Obj = FormattingArgumentsTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new FormattableArgumentsSerializeObject()
			{
				Id = 17,
				Obj = FormattingArgumentsTest.TestStruct,
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
			var input = new FormattableArgumentsSerializeObject()
			{
				Id = 17,
				Obj = FormattingArguments.None,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new FormattableArgumentsSerializeObject()
			{
				Id = 17,
				Obj = FormattingArguments.None,
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
			//IXmlSerializable obj = TestStruct;
			//Assert.IsNull(obj.GetSchema());
		}

		#endregion

		#region IEquatable tests

		/// <summary>GetHash should not fail for FormattableArguments.Empty.</summary>
		[Test]
		public void GetHash_Empty_0()
		{
			Assert.AreEqual(0, FormattingArguments.None.GetHashCode());
		}

		/// <summary>GetHash should not fail for the test struct.</summary>
		[Test]
		public void GetHash_TestStruct_1547829240()
		{
			Assert.AreEqual(1547829240, FormattingArgumentsTest.TestStruct.GetHashCode());
		}

		[Test]
		public void Equals_EmptyEmpty_IsTrue()
		{
			Assert.IsTrue(FormattingArguments.None.Equals(FormattingArguments.None));
		}

		[Test]
		public void Equals_TestStructTestStruct_IsTrue()
		{
			Assert.IsTrue(FormattingArgumentsTest.TestStruct.Equals(FormattingArgumentsTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructEmpty_IsFalse()
		{
			Assert.IsFalse(FormattingArgumentsTest.TestStruct.Equals(FormattingArguments.None));
		}

		[Test]
		public void Equals_EmptyTestStruct_IsFalse()
		{
			Assert.IsFalse(FormattingArguments.None.Equals(FormattingArgumentsTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructObjectTestStruct_IsTrue()
		{
			Assert.IsTrue(FormattingArgumentsTest.TestStruct.Equals((object)FormattingArgumentsTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructNull_IsFalse()
		{
			Assert.IsFalse(FormattingArgumentsTest.TestStruct.Equals(null));
		}

		[Test]
		public void Equals_TestStructObject_IsFalse()
		{
			Assert.IsFalse(FormattingArgumentsTest.TestStruct.Equals(new object()));
		}

		[Test]
		public void OperatorIs_TestStructTestStruct_IsTrue()
		{
			var l = FormattingArgumentsTest.TestStruct;
			var r = FormattingArgumentsTest.TestStruct;
			Assert.IsTrue(l == r);
		}

		[Test]
		public void OperatorIsNot_TestStructTestStruct_IsFalse()
		{
			var l = FormattingArgumentsTest.TestStruct;
			var r = FormattingArgumentsTest.TestStruct;
			Assert.IsFalse(l != r);
		}

		#endregion

		#region Properties

		[Test]
		public void Format_DefaultValue_StringNull()
		{
			var exp = (String)null;
			var act = FormattingArguments.None.Format;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Format_TestStruct_FormatString()
		{
			var exp = "0.000";
			var act = TestStruct.Format;
			Assert.AreEqual(exp, act);
		}
		#endregion

		[Test]
		public void DebuggerDisplay_FormattingArgumentsCollection_HasAttribute()
		{
			DebuggerDisplayAssert.HasAttribute(typeof(FormattingArguments));
		}
		[Test]
		public void DebuggerDisplay_FormattingArgumentsCollection_String()
		{
			DebuggerDisplayAssert.HasResult("Format: 'yyyy-MM-dd', Provider: en-GB", new FormattingArguments("yyyy-MM-dd", new CultureInfo("en-GB")));
		}
	}

	[Serializable]
	public class FormattableArgumentsSerializeObject
	{
		public int Id { get; set; }
		public FormattingArguments Obj { get; set; }
		public DateTime Date { get; set; }
	}
}
