using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.TestTools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.Financial.UnitTests
{
    /// <summary>Tests the Amount SVO.</summary>
    public class AmountTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly Amount TestStruct = 42.17m;


        public static NumberFormatInfo GetCustomNumberFormatInfo()
        {
            var info = new NumberFormatInfo()
            {
                CurrencyGroupSeparator = "#",
                CurrencyDecimalSeparator = "*",
            };
            return info;
        }

        #region Amount const tests

        /// <summary>Amount.Zero should be equal to the default of Amount.</summary>
        [Test]
        public void Zero_None_EqualsDefault()
        {
            Assert.AreEqual(default(Amount), Amount.Zero);
        }

        #endregion

        #region TryParse tests

        /// <summary>TryParse null should be valid.</summary>
        [Test]
        public void TyrParse_Null_IsInvalid()
        {
            Amount val;

            string str = null;

            Assert.IsFalse(Amount.TryParse(str, out val), "Valid");
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TyrParse_StringEmpty_IsInvalid()
        {
            Amount val;

            string str = string.Empty;

            Assert.IsFalse(Amount.TryParse(str, out val), "Valid");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TyrParse_StringValue_IsValid()
        {
            using (CultureInfoScope.NewInvariant())
            {
                Amount val;

                string str = "14.1804";

                Assert.IsTrue(Amount.TryParse(str, out val), "Valid");
                Assert.AreEqual(str, val.ToString(), "Value");
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
                    Amount.Parse("InvalidInput");
                },
                "Not a valid amount");
            }
        }

        [Test]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = TestStruct;
                var act = Amount.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void TryParse_InvalidInput_DefaultValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = default(Amount);
                var act = Amount.TryParse("InvalidInput");

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void Parse_CustomFormatProvider_ValidParsing()
        {
            Amount act = Amount.Parse("5#123*34", GetCustomNumberFormatInfo());
            Amount exp = 5123.34;

            Assert.AreEqual(exp, act);
        }

        #endregion

        #region (XML) (De)serialization tests

        [Test]
        public void Constructor_SerializationInfoIsNull_ThrowsArgumentNullException()
        {
            ExceptionAssert.CatchArgumentNullException
            (() =>
            {
                SerializationTest.DeserializeUsingConstructor<Amount>(null, default);
            },
            "info");
        }

        [Test]
        public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
        {
            Assert.Catch<SerializationException>
            (() =>
            {
                var info = new SerializationInfo(typeof(Amount), new System.Runtime.Serialization.FormatterConverter());
                SerializationTest.DeserializeUsingConstructor<Amount>(info, default);
            });
        }

        [Test]
        public void GetObjectData_Null_ThrowsArgumentNullException()
        {
            ExceptionAssert.CatchArgumentNullException
            (() =>
            {
                ISerializable obj = TestStruct;
                obj.GetObjectData(null, default);
            },
            "info");
        }

        [Test]
        public void GetObjectData_SerializationInfo_AreEqual()
        {
            ISerializable obj = TestStruct;
            var info = new SerializationInfo(typeof(Amount), new FormatterConverter());
            obj.GetObjectData(info, default);

            Assert.AreEqual(42.17m, info.GetDecimal("Value"));
        }

        [Test]
        public void SerializeDeserialize_TestStruct_AreEqual()
        {
            var input = TestStruct;
            var exp = TestStruct;
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void DataContractSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = TestStruct;
            var exp = TestStruct;
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void XmlSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = TestStruct;
            var exp = TestStruct;
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void SerializeDeserialize_AmountSerializeObject_AreEqual()
        {
            var input = new AmountSerializeObject()
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new AmountSerializeObject()
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void XmlSerializeDeserialize_AmountSerializeObject_AreEqual()
        {
            var input = new AmountSerializeObject()
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new AmountSerializeObject()
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void DataContractSerializeDeserialize_AmountSerializeObject_AreEqual()
        {
            var input = new AmountSerializeObject()
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new AmountSerializeObject()
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void SerializeDeserialize_Default_AreEqual()
        {
            var input = new AmountSerializeObject()
            {
                Id = 17,
                Obj = default(Amount),
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new AmountSerializeObject()
            {
                Id = 17,
                Obj = default(Amount),
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
            var input = new AmountSerializeObject()
            {
                Id = 17,
                Obj = Amount.Zero,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new AmountSerializeObject()
            {
                Id = 17,
                Obj = Amount.Zero,
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
        public void FromJson_Null_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>(() =>
            {
                JsonTester.Read<Amount>();
            },
            "JSON deserialization from null is not supported.");
        }

        [Test]
        public void FromJson_InvalidStringValue_AssertFormatException()
        {
            Assert.Catch<FormatException>(() =>
            {
                JsonTester.Read<Amount>("InvalidStringValue");
            },
            "Not a valid Amount");
        }
        [Test]
        public void FromJson_StringValue_AreEqual()
        {
            var act = JsonTester.Read<Amount>(TestStruct.ToString(CultureInfo.InvariantCulture));
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_Int64Value_AreEqual()
        {
            Amount act = JsonTester.Read<Amount>((long)TestStruct);
            Amount exp = 42;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_DoubleValue_AreEqual()
        {
            var act = JsonTester.Read<Amount>((double)TestStruct);
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_DateTimeValue_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>(() =>
            {
                JsonTester.Read<Amount>(new DateTime(1972, 02, 14));
            },
            "JSON deserialization from a date is not supported.");
        }

        [Test]
        public void ToJson_TestStruct_AreEqual()
        {
            var act = JsonTester.Write(TestStruct);
            var exp = 42.17m;
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IFormattable / ToString tests

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            using (CultureInfoScope.NewInvariant())
            {
                var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
                var exp = "Unit Test Formatter, value: '42.17', format: 'Unit Test Format'";

                Assert.AreEqual(exp, act);
            }
        }
        [Test]
        public void ToString_TestStruct_ComplexPattern()
        {
            var act = TestStruct.ToString("", CultureInfo.InvariantCulture);
            var exp = "42.17";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_ValueDutchBelgium_AreEqual()
        {
            using (new CultureInfoScope("nl-BE"))
            {
                var act = Amount.Parse("1600,1").ToString();
                var exp = "1600,1";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_ValueEnglishGreatBritain_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var act = Amount.Parse("1600.1").ToString();
                var exp = "1600.1";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_FormatValueDutchBelgium_AreEqual()
        {
            using (new CultureInfoScope("nl-BE"))
            {
                var act = Amount.Parse("800").ToString("0000");
                var exp = "0800";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_FormatValueEnglishGreatBritain_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var act = Amount.Parse("800").ToString("0000");
                var exp = "0800";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_FormatValueSpanishEcuador_AreEqual()
        {
            var act = Amount.Parse("1700").ToString("00000.0", new CultureInfo("es-EC"));
            var exp = "01700,0";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_FormatCurrencyFrFr_170Comma42Euro()
        {
            Amount amount = 170.42;
            var act = amount.ToString("C", new CultureInfo("fr-FR"));
            var exp = "170,42 €";
            Assert.AreEqual(exp, act);
        }


        [Test]
        public void ToString_CustomFormatProvider_Formatted()
        {
            Amount amount = 12345678.235m;
            var act = amount.ToString("#,##0.0000", GetCustomNumberFormatInfo());
            var exp = "12#345#678*2350";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void DebuggerDisplay_DebugToString_HasAttribute()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(Amount));
        }

        [Test]
        public void DebuggerDisplay_DefaultValue_String()
        {
            DebuggerDisplayAssert.HasResult("¤0.00", default(Amount));
        }

        [Test]
        public void DebuggerDisplay_TestStruct_String()
        {
            DebuggerDisplayAssert.HasResult("¤42.17", TestStruct);
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for Amount.Zero.</summary>
        [Test]
        public void GetHash_Zero_Hash()
        {
            Assert.AreEqual(0, Amount.Zero.GetHashCode());
        }

        /// <summary>GetHash should not fail for the test struct.</summary>
        [Test]
        public void GetHash_TestStruct_Hash()
        {
            Assert.AreEqual(-820429518, TestStruct.GetHashCode());
        }

        [Test]
        public void Equals_EmptyEmpty_IsTrue()
        {
            Assert.IsTrue(Amount.Zero.Equals(Amount.Zero));
        }

        [Test]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            using (new CultureInfoScope("en-US"))
            {
                var l = Amount.Parse("$ 1,451.070");
                var r = Amount.Parse("1451.07");

                Assert.IsTrue(l.Equals(r));
            }
        }

        [Test]
        public void Equals_TestStructTestStruct_IsTrue()
        {
            Assert.IsTrue(TestStruct.Equals(TestStruct));
        }

        [Test]
        public void Equals_TestStructEmpty_IsFalse()
        {
            Assert.IsFalse(TestStruct.Equals(Amount.Zero));
        }

        [Test]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(Amount.Zero.Equals(TestStruct));
        }

        [Test]
        public void Equals_TestStructObjectTestStruct_IsTrue()
        {
            Assert.IsTrue(TestStruct.Equals((object)TestStruct));
        }

        [Test]
        public void Equals_TestStructNull_IsFalse()
        {
            Assert.IsFalse(TestStruct.Equals(null));
        }

        [Test]
        public void Equals_TestStructObject_IsFalse()
        {
            Assert.IsFalse(TestStruct.Equals(new object()));
        }

        [Test]
        public void OperatorIs_TestStructTestStruct_IsTrue()
        {
            var l = TestStruct;
            var r = TestStruct;
            Assert.IsTrue(l == r);
        }

        [Test]
        public void OperatorIsNot_TestStructTestStruct_IsFalse()
        {
            var l = TestStruct;
            var r = TestStruct;
            Assert.IsFalse(l != r);
        }

        #endregion

        #region IComparable tests

        /// <summary>Orders a list of Amounts ascending.</summary>
        [Test]
        public void OrderBy_Amount_AreEqual()
        {
            Amount item0 = 0.23;
            Amount item1 = 1.24;
            Amount item2 = 2.27;
            Amount item3 = 1300;

            var inp = new List<Amount>() { Amount.Zero, item3, item2, item0, item1, Amount.Zero };
            var exp = new List<Amount>() { Amount.Zero, Amount.Zero, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of Amounts descending.</summary>
        [Test]
        public void OrderByDescending_Amount_AreEqual()
        {
            Amount item0 = 0.23;
            Amount item1 = 1.24;
            Amount item2 = 2.27;
            Amount item3 = 1300;

            var inp = new List<Amount>() { Amount.Zero, item3, item2, item0, item1, Amount.Zero };
            var exp = new List<Amount>() { item3, item2, item1, item0, Amount.Zero, Amount.Zero };
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
                    TestStruct.CompareTo(other);
                },
                "obj",
                "Argument must be an Amount"
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
                    TestStruct.CompareTo(other);
                },
                "obj",
                "Argument must be an Amount"
            );
        }

        [Test]
        public void LessThan_17LT19_IsTrue()
        {
            Amount l = 17;
            Amount r = 19;

            Assert.IsTrue(l < r);
        }
        [Test]
        public void GreaterThan_21LT19_IsTrue()
        {
            Amount l = 21;
            Amount r = 19;

            Assert.IsTrue(l > r);
        }

        [Test]
        public void LessThanOrEqual_17LT19_IsTrue()
        {
            Amount l = 17;
            Amount r = 19;

            Assert.IsTrue(l <= r);
        }
        [Test]
        public void GreaterThanOrEqual_21LT19_IsTrue()
        {
            Amount l = 21;
            Amount r = 19;

            Assert.IsTrue(l >= r);
        }

        [Test]
        public void LessThanOrEqual_17LT17_IsTrue()
        {
            Amount l = 17;
            Amount r = 17;

            Assert.IsTrue(l <= r);
        }
        [Test]
        public void GreaterThanOrEqual_21LT21_IsTrue()
        {
            Amount l = 21;
            Amount r = 21;

            Assert.IsTrue(l >= r);
        }
        #endregion

        #region Casting tests

        [Test]
        public void Explicit_StringToAmount_AreEqual()
        {
            var exp = TestStruct;
            var act = (Amount)TestStruct.ToString();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_AmountToString_AreEqual()
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
        public void ConverterExists_Amount_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(Amount));
        }

        [Test]
        public void CanConvertFromInt32()
        {
            TypeConverterAssert.ConvertFromEquals((Amount)123, 123);
        }
        [Test]
        public void CanConvertToInt32()
        {
            TypeConverterAssert.ConvertToEquals(1234, (Amount)1234);
        }

        [Test]
        public void CanConvertFromString_Amount_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(Amount));
        }

        [Test]
        public void CanConvertToString_Amount_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(Amount));
        }

        [Test]
        public void ConvertFromString_StringValue_TestStruct()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(TestStruct, TestStruct.ToString());
            }
        }

        [Test]
        public void ConvertFromInstanceDescriptor_Amount_Successful()
        {
            TypeConverterAssert.ConvertFromInstanceDescriptor(typeof(Amount));
        }

        [Test]
        public void ConvertToString_TestStruct_StringValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertToStringEquals(TestStruct.ToString(), TestStruct);
            }
        }

        #endregion

        #region IsValid tests

        [Test]
        public void IsValid_Data_IsFalse()
        {
            Assert.IsFalse(Amount.IsValid("Complex"), "Complex");
            Assert.IsFalse(Amount.IsValid((String)null), "(String)null");
            Assert.IsFalse(Amount.IsValid(string.Empty), "string.Empty");
        }
        [Test]
        public void IsValid_Data_IsTrue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                Assert.IsTrue(Amount.IsValid("15.48"));
            }
        }
        #endregion
    }

    [Serializable]
    public class AmountSerializeObject
    {
        public int Id { get; set; }
        public Amount Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
