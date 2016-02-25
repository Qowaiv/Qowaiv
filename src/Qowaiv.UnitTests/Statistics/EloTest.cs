using NUnit.Framework;
using Qowaiv.Statistics;
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

namespace Qowaiv.UnitTests.Statistics
{
	/// <summary>Tests the Elo SVO.</summary>
	[TestFixture]
	public class EloTest
	{
		/// <summary>The test instance for most tests.</summary>
		public static readonly Elo TestStruct = 1732.4;

		#region Elo const tests

		/// <summary>Elo.Zero should be equal to the default of Elo.</summary>
		[Test]
		public void Zero_None_EqualsDefault()
		{
			Assert.AreEqual(default(Elo), Elo.Zero);
		}
		[Test]
		public void MinValue_None_DoubleMinValue()
		{
			Assert.AreEqual((Elo)Double.MinValue, Elo.MinValue);
		}
		[Test]
		public void MaxValue_None_DoubleMaxValue()
		{
			Assert.AreEqual((Elo)Double.MaxValue, Elo.MaxValue);
		}

		#endregion

		#region Methods

		[Test]
		public void GetZScore_Delta100_0Dot64()
		{
			var act = Elo.GetZScore(1600, 1500);
			var exp = 0.64;

			Assert.AreEqual(exp, act, 0.001);
		}

		#endregion

		#region TryParse tests

		/// <summary>TryParse null should not be valid.</summary>
		[Test]
		public void TyrParse_Null_IsNotValid()
		{
			Elo val;

			string str = null;

			Assert.IsFalse(Elo.TryParse(str, out val), "Valid");
	}

		/// <summary>TryParse string.Empty should not be valid.</summary>
		[Test]
		public void TyrParse_StringEmpty_IsNotValid()
		{
			Elo val;

			string str = string.Empty;
			Assert.IsFalse(Elo.TryParse(str, out val));
		}

		/// <summary>TryParse with specified string value should be valid.</summary>
		[Test]
		public void TyrParse_StringValue_IsValid()
		{
			Elo val;

			string str = "1400";

			Assert.IsTrue(Elo.TryParse(str, out val), "Valid");
			Assert.AreEqual(str, val.ToString(), "Value");
		}
	
		[Test]
		public void Parse_InvalidInput_ThrowsFormatException()
		{
			using (new CultureInfoScope("en-GB"))
			{
				Assert.Catch<FormatException>
				(() =>
				{
					Elo.Parse("InvalidInput");
				},
				"Not a valid Elo");
			}
		}

		[Test]
		public void TryParse_TestStructInput_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = TestStruct;
				var act = Elo.TryParse(exp.ToString());

				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void TryParse_InvalidInput_DefaultValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var exp = default(Elo);
				var act = Elo.TryParse("InvalidInput");

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
				SerializationTest.DeserializeUsingConstructor<Elo>(null, default(StreamingContext));
			},
			"info");
		}
		
		[Test]
		public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
		{
			Assert.Catch<SerializationException>
			(() =>
			{
				var info = new SerializationInfo(typeof(Elo), new System.Runtime.Serialization.FormatterConverter());
				SerializationTest.DeserializeUsingConstructor<Elo>(info, default(StreamingContext));
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
			var info = new SerializationInfo(typeof(Elo), new System.Runtime.Serialization.FormatterConverter());
			obj.GetObjectData(info, default(StreamingContext));

			Assert.AreEqual(1732.4000000000001, info.GetDouble("Value"));
		}
		
		[Test]
		public void SerializeDeserialize_TestStruct_AreEqual()
		{
			var input = EloTest.TestStruct;
			var exp = EloTest.TestStruct;
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void DataContractSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = EloTest.TestStruct;
			var exp = EloTest.TestStruct;
			var act = SerializationTest.DataContractSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void XmlSerializeDeserialize_TestStruct_AreEqual()
		{
			var input = EloTest.TestStruct;
			var exp = EloTest.TestStruct;
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void SerializeDeserialize_EloSerializeObject_AreEqual()
		{
			var input = new EloSerializeObject()
			{
				Id = 17,
				Obj = EloTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new EloSerializeObject()
			{
				Id = 17,
				Obj = EloTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.SerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			DateTimeAssert.AreEqual(exp.Date, act.Date, "Date");;
		}
		[Test]
		public void XmlSerializeDeserialize_EloSerializeObject_AreEqual()
		{
			var input = new EloSerializeObject()
			{
				Id = 17,
				Obj = EloTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new EloSerializeObject()
			{
				Id = 17,
				Obj = EloTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.XmlSerializeDeserialize(input);
			Assert.AreEqual(exp.Id, act.Id, "Id");
			Assert.AreEqual(exp.Obj, act.Obj, "Obj");
			DateTimeAssert.AreEqual(exp.Date, act.Date, "Date");;
		}
		[Test]
		public void DataContractSerializeDeserialize_EloSerializeObject_AreEqual()
		{
			var input = new EloSerializeObject()
			{
				Id = 17,
				Obj = EloTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new EloSerializeObject()
			{
				Id = 17,
				Obj = EloTest.TestStruct,
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
			var input = new EloSerializeObject()
			{
				Id = 17,
				Obj = EloTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var exp = new EloSerializeObject()
			{
				Id = 17,
				Obj = EloTest.TestStruct,
				Date = new DateTime(1970, 02, 14),
			};
			var act = SerializationTest.SerializeDeserialize(input);
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
				JsonTester.Read<Elo>();
			},
			"JSON deserialization from null is not supported.");
		}

		[Test]
		public void FromJson_InvalidStringValue_AssertFormatException()
		{
			Assert.Catch<FormatException>(() =>
			{
				JsonTester.Read<Elo>("InvalidStringValue");
			},
			"Not a valid Elo");
		}
		[Test]
		public void FromJson_StringValue_AreEqual()
		{
			var act = JsonTester.Read<Elo>(TestStruct.ToString(CultureInfo.InvariantCulture));
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_Int64Value_AreEqual()
		{
			Elo act = JsonTester.Read<Elo>((Int64)TestStruct);
			Elo exp = 1732;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void FromJson_DoubleValue_AreEqual()
		{
			var act = JsonTester.Read<Elo>((Double)TestStruct);
			var exp = TestStruct;

			Assert.AreEqual(exp, act);
		}
		
		[Test]
		public void FromJson_DateTimeValue_AssertNotSupportedException()
		{
			Assert.Catch<NotSupportedException>(() =>
			{
				JsonTester.Read<Elo>(new DateTime(1972, 02, 14));
			},
			"JSON deserialization from a date is not supported.");
		}

		[Test]
		public void ToJson_TestStruct_AreEqual()
		{
			var act = JsonTester.Write(TestStruct);
			var exp = 1732.4000000000001;
			
			Assert.AreEqual(exp, act);
		}

		#endregion
		
		#region IFormattable / ToString tests
		
		[Test]
		public void ToString_CustomFormatter_SupportsCustomFormatting()
		{
			using (new CultureInfoScope(CultureInfo.InvariantCulture))
			{
				var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
				var exp = "Unit Test Formatter, value: '1732.4', format: 'Unit Test Format'";

				Assert.AreEqual(exp, act);
			}
		}
		[Test]
		public void ToString_TestStruct_ComplexPattern()
		{
			using (new CultureInfoScope(CultureInfo.InvariantCulture))
			{
				var act = TestStruct.ToString("");
				var exp = "1732.4";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_ValueDutchBelgium_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = Elo.Parse("1600,1").ToString();
				var exp = "1600,1";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_ValueEnglishGreatBritain_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var act = Elo.Parse("1600.1").ToString();
				var exp = "1600.1";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_FormatValueDutchBelgium_AreEqual()
		{
			using (new CultureInfoScope("nl-BE"))
			{
				var act = Elo.Parse("800").ToString("0000");
				var exp = "0800";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_FormatValueEnglishGreatBritain_AreEqual()
		{
			using (new CultureInfoScope("en-GB"))
			{
				var act = Elo.Parse("800").ToString("0000");
				var exp = "0800";
				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_FormatValueSpanishEcuador_AreEqual()
		{
			var act = Elo.Parse("1700").ToString("00000.0", new CultureInfo("es-EC"));
			var exp = "01700,0";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void DebuggerDisplay_DebugToString_HasAttribute()
		{
			DebuggerDisplayAssert.HasAttribute(typeof(Elo));
		}
	
		[Test]
		public void DebuggerDisplay_TestStruct_DoubleValueOfTheTestStruct()
		{
			DebuggerDisplayAssert.HasResult(1732.4000000000001, TestStruct);
		}

		#endregion

		#region IEquatable tests

		/// <summary>GetHash should not fail for Elo.Empty.</summary>
		[Test]
		public void GetHash_Empty_Hash()
		{
			Assert.AreEqual(0, Elo.Zero.GetHashCode());
		}

		/// <summary>GetHash should not fail for the test struct.</summary>
		[Test]
		public void GetHash_TestStruct_Hash()
		{
			Assert.AreEqual(-654145533, EloTest.TestStruct.GetHashCode());
		}

		[Test]
		public void Equals_FormattedAndUnformatted_IsTrue()
		{
			var l = Elo.Parse("1600", CultureInfo.InvariantCulture);
			var r = Elo.Parse("1,600.00*", CultureInfo.InvariantCulture);

			Assert.IsTrue(l.Equals(r));
		}

		[Test]
		public void Equals_TestStructTestStruct_IsTrue()
		{
			Assert.IsTrue(EloTest.TestStruct.Equals(EloTest.TestStruct));
		}
				
		[Test]
		public void Equals_TestStructObjectTestStruct_IsTrue()
		{
			Assert.IsTrue(EloTest.TestStruct.Equals((object)EloTest.TestStruct));
		}

		[Test]
		public void Equals_TestStructNull_IsFalse()
		{
			Assert.IsFalse(EloTest.TestStruct.Equals(null));
		}

		[Test]
		public void Equals_TestStructObject_IsFalse()
		{
			Assert.IsFalse(EloTest.TestStruct.Equals(new object()));
		}

		[Test]
		public void OperatorIs_TestStructTestStruct_IsTrue()
		{
			var l = EloTest.TestStruct;
			var r = EloTest.TestStruct;
			Assert.IsTrue(l == r);
		}

		[Test]
		public void OperatorIsNot_TestStructTestStruct_IsFalse()
		{
			var l = EloTest.TestStruct;
			var r = EloTest.TestStruct;
			Assert.IsFalse(l != r);
		}

		#endregion
		
		#region IComparable tests

		/// <summary>Orders a list of Elos ascending.</summary>
		[Test]
		public void OrderBy_Elo_AreEqual()
		{
			Elo item0 = 1601;
			Elo item1 = 2371;
			Elo item2 = 2416;
			Elo item3 = 2601;

			var inp = new List<Elo>() { Elo.Zero, item3, item2, item0, item1, Elo.Zero };
			var exp = new List<Elo>() { Elo.Zero, Elo.Zero, item0, item1, item2, item3 };
			var act = inp.OrderBy(item => item).ToList();

			CollectionAssert.AreEqual(exp, act);
		}

		/// <summary>Orders a list of Elos descending.</summary>
		[Test]
		public void OrderByDescending_Elo_AreEqual()
		{
			Elo item0 = 1601;
			Elo item1 = 2371;
			Elo item2 = 2416;
			Elo item3 = 2601;

			var inp = new List<Elo>() { Elo.Zero, item3, item2, item0, item1, Elo.Zero };
			var exp = new List<Elo>() { item3, item2, item1, item0, Elo.Zero, Elo.Zero };
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
				"Argument must be an Elo."
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
				"Argument must be an Elo."
			);
		}

		[Test]
		public void LessThan_17LT19_IsTrue()
		{
			Elo l = 17;
			Elo r = 19;

			Assert.IsTrue(l < r);
		}
		[Test]
		public void GreaterThan_21LT19_IsTrue()
		{
			Elo l = 21;
			Elo r = 19;

			Assert.IsTrue(l > r);
		}

		[Test]
		public void LessThanOrEqual_17LT19_IsTrue()
		{
			Elo l = 17;
			Elo r = 19;

			Assert.IsTrue(l <= r);
		}
		[Test]
		public void GreaterThanOrEqual_21LT19_IsTrue()
		{
			Elo l = 21;
			Elo r = 19;

			Assert.IsTrue(l >= r);
		}

		[Test]
		public void LessThanOrEqual_17LT17_IsTrue()
		{
			Elo l = 17;
			Elo r = 17;

			Assert.IsTrue(l <= r);
		}
		[Test]
		public void GreaterThanOrEqual_21LT21_IsTrue()
		{
			Elo l = 21;
			Elo r = 21;

			Assert.IsTrue(l >= r);
		}
		#endregion
		
		#region Casting tests

		[Test]
		public void Explicit_StringToElo_AreEqual()
		{
			var exp = TestStruct;
			var act = (Elo)TestStruct.ToString();

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_EloToString_AreEqual()
		{
			var exp = TestStruct.ToString();
			var act = (string)TestStruct;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Implicit_DoubleToElo_AreEqual()
		{
			Elo exp = Elo.Create(1600.1);
			Elo act = 1600.1;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_EloToDouble_AreEqual()
		{
			var exp = 1600.1;
			var act = (Double)Elo.Create(1600.1);

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Implicit_DecimalToElo_AreEqual()
		{
			Elo exp = Elo.Create(1600.1);
			Elo act = 1600.1m;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_EloToDecimal_AreEqual()
		{
			var exp = 1600.1m;
			var act = (Decimal)Elo.Create(1600.1);

			Assert.AreEqual(exp, act);
		}


		[Test]
		public void Implicit_Int32ToElo_AreEqual()
		{
			Elo exp = Elo.Create(1600);
			Elo act = 1600;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Explicit_EloToInt32_AreEqual()
		{
			var exp = 1600;
			var act = (Int32)Elo.Create(1600);

			Assert.AreEqual(exp, act);
		}

		#endregion

		#region Properties
		#endregion

		#region Operators

		[Test]
		public void Add_1600Add100_1700()
		{
			Elo l = 1600;
			Elo r = 100;

			Elo act = l + r;
			Elo exp = 1700;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Subtract_1600Subtract100_1500()
		{
			Elo l = 1600;
			Elo r = 100;

			Elo act = l - r;
			Elo exp = 1500;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Divde_1600Subtract100_800()
		{
			Elo act = 1600m;

			act /= 2.0;

			Elo exp = 800;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Divde_1600Subtract100_3200()
		{
			Elo act = 1600m;

			act *= 2.0;

			Elo exp = 3200;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Increment_1600_1601()
		{
			Elo act = 1600;
			act++;

			Elo exp = 1601;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Decrement_1600_1599()
		{
			Elo act = 1600;
			act--;

			Elo exp = 1599;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Negate_Min1600_1600()
		{
			Elo act = -1600;

			act = -act;

			Elo exp = 1600;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Plus_1600_1600()
		{
			Elo act = 1600;

			act = +act;

			Elo exp = 1600;

			Assert.AreEqual(exp, act);
		}
		#endregion

		#region Type converter tests

		[Test]
		public void ConverterExists_Elo_IsTrue()
		{
			TypeConverterAssert.ConverterExists(typeof(Elo));
		}

		[Test]
		public void CanNotConvertFromInt32_Elo_IsTrue()
		{
		TypeConverterAssert.CanNotConvertFrom(typeof(Elo), typeof(Int32));
		}
		[Test]
		public void CanNotConvertToInt32_Elo_IsTrue()
		{
		TypeConverterAssert.CanNotConvertTo(typeof(Elo), typeof(Int32));
		}

		[Test]
		public void CanConvertFromString_Elo_IsTrue()
		{
			TypeConverterAssert.CanConvertFromString(typeof(Elo));
		}

		[Test]
		public void CanConvertToString_Elo_IsTrue()
		{
			TypeConverterAssert.CanConvertToString(typeof(Elo));
		}

		[Test]
		public void ConvertFromString_StringValue_TestStruct()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertFromEquals(EloTest.TestStruct, EloTest.TestStruct.ToString());
			}
		}

		[Test]
		public void ConvertFromInstanceDescriptor_Elo_Successful()
		{
			TypeConverterAssert.ConvertFromInstanceDescriptor(typeof(Elo));
		}

		[Test]
		public void ConvertToString_TestStruct_StringValue()
		{
			using (new CultureInfoScope("en-GB"))
			{
				TypeConverterAssert.ConvertToStringEquals(EloTest.TestStruct.ToString(), EloTest.TestStruct);
			}
		}

		#endregion
		
		#region IsValid tests

		[Test]
		public void IsValid_Data_IsFalse()
		{
			Assert.IsFalse(Elo.IsValid("Complex"), "Complex");
			Assert.IsFalse(Elo.IsValid((String)null), "(String)null");
			Assert.IsFalse(Elo.IsValid(String.Empty), "String.Empty");
		}
		[Test]
		public void IsValid_Data_IsTrue()
		{
			Assert.IsTrue(Elo.IsValid("1754.8*"));
		}
		#endregion
	}

	[Serializable]
	public class EloSerializeObject
	{
		public int Id { get; set; }
		public Elo Obj { get; set; }
		public DateTime Date { get; set; }
	}
}
