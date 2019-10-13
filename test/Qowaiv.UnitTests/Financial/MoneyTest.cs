using NUnit.Framework;
using Qowaiv.Financial;
using Qowaiv.Globalization;
using Qowaiv.TestTools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests.Financial
{
    /// <summary>Tests the Money SVO.</summary>
    [TestFixture]
    public class MoneyTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly Money TestStruct = 42.17 + Currency.EUR;

        #region Money const tests

        /// <summary>Money.Empty should be equal to the default of Money.</summary>
        [Test]
        public void Zero_None_EqualsDefault()
        {
            Assert.AreEqual(default(Money), Money.Zero);
        }

        #endregion

        #region TryParse tests

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TyrParse_StringValue_IsValid()
        {
            using (new CultureInfoScope("nl-NL"))
            {
                Money exp = 42.17 + Currency.EUR;
                Assert.IsTrue(Money.TryParse("€42,17", out Money act), "Valid");
                Assert.AreEqual(exp, act, "Value");
            }
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [Test]
        public void TyrParse_StringValue_IsNotValid()
        {
            string str = "string";
            Assert.IsFalse(Money.TryParse(str, out Money val), "Valid");
            Assert.AreEqual(Money.Zero, val, "Value");
        }

        [Test]
        public void Parse_InvalidInput_ThrowsFormatException()
        {
            using (new CultureInfoScope("en-GB"))
            {
                Assert.Catch<FormatException>
                (() =>
                {
                    Money.Parse("InvalidInput");
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
                var act = Money.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void TryParse_InvalidInput_DefaultValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = default(Money);
                var act = Money.TryParse("InvalidInput");

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void Parse_EuroSpace12_Parsed()
        {
            using (new CultureInfoScope("fr-FR"))
            {
                var exp = 12 + Currency.EUR;
                var act = Money.Parse("€ 12");
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void Parse_Min12Comma765SpaceEuro_Parsed()
        {
            using (new CultureInfoScope("fr-FR"))
            {
                var exp = -12.765 + Currency.EUR;
                var act = Money.Parse("-12,765 €");
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
                SerializationTest.DeserializeUsingConstructor<Money>(null, default);
            },
            "info");
        }

        [Test]
        public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
        {
            Assert.Catch<SerializationException>
            (() =>
            {
                var info = new SerializationInfo(typeof(Money), new System.Runtime.Serialization.FormatterConverter());
                SerializationTest.DeserializeUsingConstructor<Money>(info, default);
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
            var info = new SerializationInfo(typeof(Money), new FormatterConverter());
            obj.GetObjectData(info, default);

            Assert.AreEqual(42.17m, info.GetDecimal("Value"));
            Assert.AreEqual("EUR", info.GetString("Currency"));
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
        public void XmlSerialize_TestStruct_AreEqual()
        {
            var act = SerializationTest.XmlSerialize(TestStruct);
            var exp = "EUR42.17";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void XmlDeserialize_XmlString_AreEqual()
        {
            var act = SerializationTest.XmlDeserialize<Money>("EUR42.17");
            Assert.AreEqual(TestStruct, act);
        }


        [Test]
        public void SerializeDeserialize_MoneySerializeObject_AreEqual()
        {
            var input = new MoneySerializeObject()
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new MoneySerializeObject()
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
        public void XmlSerializeDeserialize_MoneySerializeObject_AreEqual()
        {
            var input = new MoneySerializeObject()
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new MoneySerializeObject()
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
        public void DataContractSerializeDeserialize_MoneySerializeObject_AreEqual()
        {
            var input = new MoneySerializeObject()
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new MoneySerializeObject()
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
            var input = new MoneySerializeObject()
            {
                Id = 17,
                Obj = default,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new MoneySerializeObject()
            {
                Id = 17,
                Obj = default,
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
            var input = new MoneySerializeObject()
            {
                Id = 17,
                Obj = Money.Zero,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new MoneySerializeObject()
            {
                Id = 17,
                Obj = Money.Zero,
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
                JsonTester.Read<Money>();
            },
            "JSON deserialization from null is not supported.");
        }

        [Test]
        public void FromJson_InvalidStringValue_AssertFormatException()
        {
            Assert.Catch<FormatException>(() =>
            {
                JsonTester.Read<Money>("InvalidStringValue");
            },
            "Not a valid Money");
        }
        [Test]
        public void FromJson_StringValue_AreEqual()
        {
            var act = JsonTester.Read<Money>("EUR 42.17");
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_Int64Value_AreEqual()
        {
            Money act = JsonTester.Read<Money>(42);
            Money exp = 42 + Currency.Empty;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_DoubleValue_AreEqual()
        {
            var act = JsonTester.Read<Money>(-42.17);
            var exp = -42.17 + Currency.Empty;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_DateTimeValue_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>(() =>
            {
                JsonTester.Read<Money>(new DateTime(1972, 02, 14));
            },
            "JSON deserialization from a date is not supported.");
        }

        [Test]
        public void ToJson_TestStruct_AreEqual()
        {
            var act = JsonTester.Write(TestStruct);
            var exp = "€42.17";
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IFormattable / ToString tests

        [Test]
        public void ToString_Empty_StringEmpty()
        {
            using (CultureInfoScope.NewInvariant())
            {
                var act = Money.Zero.ToString();
                var exp = "0";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            using (new CultureInfoScope("fr-FR"))
            {
                var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
                var exp = "Unit Test Formatter, value: '42,17 €', format: 'Unit Test Format'";

                Assert.AreEqual(exp, act);
            }
        }
        [Test]
        public void ToString_TestStruct_ComplexPattern()
        {
            using (new CultureInfoScope("nl-BE"))
            {
                var act = TestStruct.ToString("");
                var exp = "42,17";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_ValueDutchBelgium_AreEqual()
        {
            using (new CultureInfoScope("nl-BE"))
            {
                // 3: n $
                CultureInfo.CurrentCulture.NumberFormat.CurrencyPositivePattern = 3;
                var act = Money.Parse("ALL 1600,1").ToString();
                var exp = "1.600,10 Lekë";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_ValueEnglishGreatBritain_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var act = Money.Parse("EUR 1600.1").ToString();
                var exp = "€1,600.10";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_AED_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var act = Money.Parse("AED 1600.1").ToString();
                var exp = "AED1,600.10";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_FormatValueDutchBelgium_AreEqual()
        {
            using (new CultureInfoScope("nl-BE"))
            {
                var act = Money.Parse("800").ToString("0000");
                var exp = "0800";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_FormatValueEnglishGreatBritain_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var act = Money.Parse("800").ToString("0000");
                var exp = "0800";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_FormatValueSpanishEcuador_AreEqual()
        {
            var act = Money.Parse("1700").ToString("00000.0", new CultureInfo("es-EC"));
            var exp = "01700,0";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void DebuggerDisplay_DebugToString_HasAttribute()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(Money));
        }

        [Test]
        public void DebuggerDisplay_TestStruct_String()
        {
            DebuggerDisplayAssert.HasResult("EUR 42.17", TestStruct);
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for Money.Empty.</summary>
        [Test]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, Money.Zero.GetHashCode());
        }

        /// <summary>GetHash should not fail for the test struct.</summary>
        [Test]
        public void GetHash_TestStruct_NotZero()
        {
            Assert.NotZero(TestStruct.GetHashCode());
        }

        [Test]
        public void Equals_EmptyEmpty_IsTrue()
        {
            Assert.IsTrue(Money.Zero.Equals(Money.Zero));
        }

        [Test]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = Money.Parse("€1,234,456.789", CultureInfo.InvariantCulture);
            var r = Money.Parse("EUR1234456.789", CultureInfo.InvariantCulture);

            Assert.IsTrue(l.Equals(r));
        }

        [Test]
        public void Equals_TestStructTestStruct_IsTrue()
        {
            Assert.IsTrue(TestStruct.Equals(TestStruct));
        }

        [Test]
        public void Equals_TestStructEmpty_IsFalse()
        {
            Assert.IsFalse(TestStruct.Equals(Money.Zero));
        }

        [Test]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(Money.Zero.Equals(TestStruct));
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

        /// <summary>Orders a list of Moneys ascending.</summary>
        [Test]
        public void OrderBy_Money_AreEqual()
        {
            var item0 = 124.40 + Currency.EUR;
            var item1 = 124.41 + Currency.EUR;
            var item2 = 124.42 + Currency.EUR;
            var item3 = 124.39 + Currency.GBP;

            var inp = new List<Money>() { Money.Zero, item3, item2, item0, item1, Money.Zero };
            var exp = new List<Money>() { Money.Zero, Money.Zero, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of Moneys descending.</summary>
        [Test]
        public void OrderByDescending_Money_AreEqual()
        {
            var item0 = 124.40 + Currency.EUR;
            var item1 = 124.41 + Currency.EUR;
            var item2 = 124.42 + Currency.EUR;
            var item3 = 124.39 + Currency.GBP;

            var inp = new List<Money>() { Money.Zero, item3, item2, item0, item1, Money.Zero };
            var exp = new List<Money>() { item3, item2, item1, item0, Money.Zero, Money.Zero };
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
                "Argument must be Money"
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
                "Argument must be Money"
            );
        }

        [Test]
        public void LessThan_17LT19_IsTrue()
        {
            Money l = 17;
            Money r = 19;

            Assert.IsTrue(l < r);
        }
        [Test]
        public void GreaterThan_21LT19_IsTrue()
        {
            Money l = 21;
            Money r = 19;

            Assert.IsTrue(l > r);
        }

        [Test]
        public void LessThanOrEqual_17LT19_IsTrue()
        {
            Money l = 17;
            Money r = 19;

            Assert.IsTrue(l <= r);
        }
        [Test]
        public void GreaterThanOrEqual_21LT19_IsTrue()
        {
            Money l = 21;
            Money r = 19;

            Assert.IsTrue(l >= r);
        }

        [Test]
        public void LessThanOrEqual_17LT17_IsTrue()
        {
            Money l = 17;
            Money r = 17;

            Assert.IsTrue(l <= r);
        }
        [Test]
        public void GreaterThanOrEqual_21LT21_IsTrue()
        {
            Money l = 21;
            Money r = 21;

            Assert.IsTrue(l >= r);
        }
        #endregion

        #region Casting tests

        [Test]
        public void Explicit_StringToMoney_AreEqual()
        {
            var exp = TestStruct;
            var act = (Money)TestStruct.ToString();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_MoneyToString_AreEqual()
        {
            var exp = TestStruct.ToString();
            var act = (string)TestStruct;

            Assert.AreEqual(exp, act);
        }

        #endregion

        #region Properties

        [Test]
        public void Currency_Set()
        {
            var money = 42 + Currency.EUR;
            Assert.AreEqual(Currency.EUR, money.Currency);
        }

        #endregion

        #region Operations

        [Test]
        public void Plus_TestStruct_SameValue()
        {
            var act = +TestStruct;
            Assert.AreEqual(TestStruct, act);
        }

        [Test]
        public void Negates_TestStruct_NegativeValue()
        {
            var act = -TestStruct;
            var exp = -42.17 + Currency.EUR;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Increase_TestStruct_Plus1()
        {
            var act = TestStruct;
            var exp = 43.17 + Currency.EUR;
            Assert.AreEqual(exp, ++act);
        }

        [Test]
        public void Decrease_TestStruct_Minus1()
        {
            var act = TestStruct;
            var exp = 41.17 + Currency.EUR;
            Assert.AreEqual(exp, --act);
        }

        [Test]
        public void Add_SameCurrency_Added()
        {
            var l = 16 + Currency.EUR;
            var r = 26 + Currency.EUR;
            var a = 42 + Currency.EUR;

            Assert.AreEqual(a, l + r);
        }

        [Test]
        public void Add_DifferentCurrency_Throws()
        {
            var l = 16 + Currency.EUR;
            var r = 666 + Currency.USD;

            var x = Assert.Catch<CurrencyMismatchException>(()=>  Money.Add(l, r));

            Assert.AreEqual("The addition operation could not be applied. There is a mismatch between EUR and USD.", x.Message);
        }

        [Test]
        public void Subtract_SameCurrency_Subtracted()
        {
            var l = 69 + Currency.EUR;
            var r = 27 + Currency.EUR;
            var a = 42 + Currency.EUR;

            Assert.AreEqual(a, l - r);
        }

        [Test]
        public void Subtract_DifferentCurrency_Throws()
        {
            var l = 16 + Currency.EUR;
            var r = 666 + Currency.USD;

            var x = Assert.Catch<CurrencyMismatchException>(() => Money.Subtract(l, r));

            Assert.AreEqual("The subtraction operation could not be applied. There is a mismatch between EUR and USD.", x.Message);
        }

        #endregion

        #region Type converter tests

        [Test]
        public void ConverterExists_Money_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(Money));
        }

        [Test]
        public void CanNotConvertFromInt32_Money_IsTrue()
        {
            TypeConverterAssert.CanNotConvertFrom(typeof(Money), typeof(Int32));
        }
        [Test]
        public void CanNotConvertToInt32_Money_IsTrue()
        {
            TypeConverterAssert.CanNotConvertTo(typeof(Money), typeof(Int32));
        }

        [Test]
        public void CanConvertFromString_Money_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(Money));
        }

        [Test]
        public void CanConvertToString_Money_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(Money));
        }

        [Test]
        public void ConvertFrom_StringEmpty_MoneyEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(Money.Zero, "0");
            }
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
        public void ConvertFromInstanceDescriptor_Money_Successful()
        {
            TypeConverterAssert.ConvertFromInstanceDescriptor(typeof(Money));
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
            Assert.IsFalse(Money.IsValid("Complex"), "Complex");
            Assert.IsFalse(Money.IsValid((String)null), "(String)null");
            Assert.IsFalse(Money.IsValid(string.Empty), "string.Empty");
        }
        [Test]
        public void IsValid_Data_IsTrue()
        {
            Assert.IsTrue(Money.IsValid("USD -12,345.789", CultureInfo.InvariantCulture));
            Assert.IsTrue(Money.IsValid("USD +12,345.789", CultureInfo.InvariantCulture));
            Assert.IsTrue(Money.IsValid("€ 12,345.789", CultureInfo.InvariantCulture));
        }
        #endregion
    }

    [Serializable]
    public class MoneySerializeObject
    {
        public int Id { get; set; }
        public Money Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
