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
        public static readonly Amount TestStruct = (Amount)42.17m;


        public static NumberFormatInfo GetCustomNumberFormatInfo()
        {
            var info = new NumberFormatInfo
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
            string str = null;
            Assert.IsFalse(Amount.TryParse(str, out _));
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TyrParse_StringEmpty_IsInvalid()
        {
            string str = string.Empty;
            Assert.IsFalse(Amount.TryParse(str, out _));
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TyrParse_StringValue_IsValid()
        {
            using (CultureInfoScope.NewInvariant())
            {
                string str = "14.1804";
                Assert.IsTrue(Amount.TryParse(str, out Amount val), "Valid");
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
            Amount exp = (Amount)5123.34;

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
        public void XmlSerialize_TestStruct_AreEqual()
        {
            var act = SerializationTest.XmlSerialize(TestStruct);
            var exp = "42.17";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void XmlDeserialize_XmlString_AreEqual()
        {
            var act = SerializationTest.XmlDeserialize<Amount>("42.17");
            Assert.AreEqual(TestStruct, act);
        }


        [Test]
        public void SerializeDeserialize_AmountSerializeObject_AreEqual()
        {
            var input = new AmountSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new AmountSerializeObject
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
            var input = new AmountSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new AmountSerializeObject
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
            var input = new AmountSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new AmountSerializeObject
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
            var input = new AmountSerializeObject
            {
                Id = 17,
                Obj = default,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new AmountSerializeObject
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
            var input = new AmountSerializeObject
            {
                Id = 17,
                Obj = Amount.Zero,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new AmountSerializeObject
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
            var act = JsonTester.Read<Amount>("42.17");
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_Int64Value_AreEqual()
        {
            Amount act = JsonTester.Read<Amount>((long)TestStruct);
            Amount exp = (Amount)42;

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
            Amount amount = (Amount)170.42;
            var act = amount.ToString("C", new CultureInfo("fr-FR"));
            var exp = "170,42 €";
            Assert.AreEqual(exp, act);
        }


        [Test]
        public void ToString_CustomFormatProvider_Formatted()
        {
            Amount amount = (Amount)12345678.235m;
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
        public void GetHash_SameValue_SameHash()
        {
            var hash0 = ((Amount)451).GetHashCode();
            var hash1 = ((Amount)451).GetHashCode();
            Assert.AreEqual(hash1, hash0);
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
            Amount item0 = (Amount)0.23;
            Amount item1 = (Amount)1.24;
            Amount item2 = (Amount)2.27;
            Amount item3 = (Amount)1300;

            var inp = new List<Amount> { Amount.Zero, item3, item2, item0, item1, Amount.Zero };
            var exp = new List<Amount> { Amount.Zero, Amount.Zero, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of Amounts descending.</summary>
        [Test]
        public void OrderByDescending_Amount_AreEqual()
        {
            Amount item0 = (Amount)0.23;
            Amount item1 = (Amount)1.24;
            Amount item2 = (Amount)2.27;
            Amount item3 = (Amount)1300;

            var inp = new List<Amount> { Amount.Zero, item3, item2, item0, item1, Amount.Zero };
            var exp = new List<Amount> { item3, item2, item1, item0, Amount.Zero, Amount.Zero };
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
                "Argument must be Amount."
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
                "Argument must be Amount."
            );
        }

        [Test]
        public void LessThan_17LT19_IsTrue()
        {
            Amount l = (Amount)17;
            Amount r = (Amount)19;

            Assert.IsTrue(l < r);
        }
        [Test]
        public void GreaterThan_21LT19_IsTrue()
        {
            Amount l = (Amount)21;
            Amount r = (Amount)19;

            Assert.IsTrue(l > r);
        }

        [Test]
        public void LessThanOrEqual_17LT19_IsTrue()
        {
            Amount l = (Amount)17;
            Amount r = (Amount)19;

            Assert.IsTrue(l <= r);
        }
        [Test]
        public void GreaterThanOrEqual_21LT19_IsTrue()
        {
            Amount l = (Amount)21;
            Amount r = (Amount)19;

            Assert.IsTrue(l >= r);
        }

        [Test]
        public void LessThanOrEqual_17LT17_IsTrue()
        {
            Amount l = (Amount)17;
            Amount r = (Amount)17;

            Assert.IsTrue(l <= r);
        }
        [Test]
        public void GreaterThanOrEqual_21LT21_IsTrue()
        {
            Amount l = (Amount)21;
            Amount r = (Amount)21;

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

        #region Methods

        [TestCase(1234.01, -1234.01)]
        [TestCase(1234.01, +1234.01)]
        public void Abs(Amount expected, Amount value)
        {
            var abs = value.Abs();
            Assert.AreEqual(expected, abs);
        }

        [TestCase(-1234.01)]
        [TestCase(+1234.01)]
        public void Plus(Amount expected)
        {
            var plus = +expected;
            Assert.AreEqual(expected, plus);
        }

        [TestCase(+1234.01, -1234.01)]
        [TestCase(-1234.01, +1234.01)]
        public void Negate(Amount expected, Amount value)
        {
            var negated = -value;
            Assert.AreEqual(expected, negated);
        }

        [Test]
        public void Decrement_EqualsTestStruct()
        {
            Amount amount = (Amount)43.17;
            Assert.AreEqual(TestStruct, --amount);
        }

        [Test]
        public void Increment_EqualsTestStruct()
        {
            Amount amount = (Amount)41.17;
            Assert.AreEqual(TestStruct, ++amount);
        }

        [Test]
        public void Add_SomeAmount_Added()
        {
            Amount amount = (Amount)40.10;
            Amount other = (Amount)2.07;
            Assert.AreEqual(TestStruct, amount + other);
        }

        [Test]
        public void Add_SomePercentage_Added()
        {
            Amount amount = (Amount)40.00;
            var p = 10.Percent();
            Assert.AreEqual((Amount)44.00, amount + p);
        }

        [Test]
        public void Subtract_SomeAmount_Subtracted()
        {
            Amount amount = (Amount)43.20;
            Amount other = (Amount)1.03;
            Assert.AreEqual(TestStruct, amount - other);
        }

        [Test]
        public void Subtract_SomePercentage_Subtracted()
        {
            Amount amount = (Amount)40.00;
            var p = 25.Percent();
            Assert.AreEqual((Amount)30.00, amount - p);
        }

        [Test]
        public void Multiply_Percentage()
        {
            Amount amount = (Amount)100.40m;
            var p = 50.Percent();
            Amount expected = (Amount)50.20m;
            Assert.AreEqual(expected, amount * p);
        }

        [Test]
        public void Multiply_Float()
        {
            Amount amount = (Amount)100.40m;
            float p = 0.5F;
            Amount expected = (Amount)50.20m;
            Assert.AreEqual(expected, amount * p);
        }

        [Test]
        public void Multiply_Double()
        {
            Amount amount = (Amount)100.40m;
            double p = 0.5;
            Amount expected = (Amount)50.20m;
            Assert.AreEqual(expected, amount * p);
        }

        [Test]
        public void Multiply_Decimal()
        {
            Amount amount = (Amount)100.40m;
            var p = 0.5m;
            Amount expected = (Amount)50.20m;
            Assert.AreEqual(expected, amount * p);
        }

        [Test]
        public void Multiply_Short()
        {
            Amount amount = (Amount)100.40m;
            short f = 2;
            Amount expected = (Amount)200.80m;
            Assert.AreEqual(expected, amount * f);
        }

        [Test]
        public void Multiply_Int()
        {
            Amount amount = (Amount)100.40m;
            int f = 2;
            Amount expected = (Amount)200.80m;
            Assert.AreEqual(expected, amount * f);
        }

        [Test]
        public void Multiply_Long()
        {
            Amount amount = (Amount)100.40m;
            long f = 2;
            Amount expected = (Amount)200.80m;
            Assert.AreEqual(expected, amount * f);
        }

        [Test]
        public void Multiply_UShort()
        {
            Amount amount = (Amount)100.40m;
            ushort f = 2;
            Amount expected = (Amount)200.80m;
            Assert.AreEqual(expected, amount * f);
        }

        [Test]
        public void Multiply_UInt()
        {
            Amount amount = (Amount)100.40m;
            uint f = 2;
            Amount expected = (Amount)200.80m;
            Assert.AreEqual(expected, amount * f);
        }

        [Test]
        public void Multiply_ULong()
        {
            Amount amount = (Amount)100.40m;
            ulong f = 2;
            Amount expected = (Amount)200.80m;
            Assert.AreEqual(expected, amount * f);
        }

        [Test]
        public void Divide_Percentage()
        {
            Amount amount = (Amount)100.40m;
            var p = 50.Percent();
            Amount expected = (Amount)200.80m;
            Assert.AreEqual(expected, amount / p);
        }

        [Test]
        public void Divide_Float()
        {
            Amount amount = (Amount)100.40m;
            float p = 0.5F;
            Amount expected = (Amount)200.80m;
            Assert.AreEqual(expected, amount / p);
        }

        [Test]
        public void Divide_Double()
        {
            Amount amount = (Amount)100.40m;
            double p = 0.5;
            Amount expected = (Amount)200.80m;
            Assert.AreEqual(expected, amount / p);
        }

        [Test]
        public void Divide_Decimal()
        {
            Amount amount = (Amount)100.40m;
            var p = 0.5m;
            Amount expected = (Amount)200.80m;
            Assert.AreEqual(expected, amount / p);
        }

        [Test]
        public void Divide_Short()
        {
            Amount amount = (Amount)100.40m;
            short f = 2;
            Amount expected = (Amount)50.20m;
            Assert.AreEqual(expected, amount / f);
        }

        [Test]
        public void Divide_Int()
        {
            Amount amount = (Amount)100.40m;
            int f = 2;
            Amount expected = (Amount)50.20m;
            Assert.AreEqual(expected, amount / f);
        }

        [Test]
        public void Divide_Long()
        {
            Amount amount = (Amount)100.40m;
            long f = 2;
            Amount expected = (Amount)50.20m;
            Assert.AreEqual(expected, amount / f);
        }

        [Test]
        public void Divide_UShort()
        {
            Amount amount = (Amount)100.40m;
            ushort f = 2;
            Amount expected = (Amount)50.20m;
            Assert.AreEqual(expected, amount / f);
        }

        [Test]
        public void Divide_UInt()
        {
            Amount amount = (Amount)100.40m;
            uint f = 2;
            Amount expected = (Amount)50.20m;
            Assert.AreEqual(expected, amount / f);
        }

        [Test]
        public void Divide_ULong()
        {
            Amount amount = (Amount)100.40m;
            ulong f = 2;
            Amount expected = (Amount)50.20m;
            Assert.AreEqual(expected, amount / f);
        }

        [Test]
        public void Round_NoDigits()
        {
            var amount = (Amount)123.4567m;
            var rounded = amount.Round();
            Assert.AreEqual((Amount)123m, rounded);
        }

        [Test]
        public void Round_1Digit()
        {
            var amount = (Amount)123.4567m;
            var rounded = amount.Round(1);
            Assert.AreEqual((Amount)123.5m , rounded);
        }

        [Test]
        public void RoundToMultiple_0d25()
        {
            var amount = (Amount)123.6567m;
            var rounded = amount.RoundToMultiple(0.25m);
            Assert.AreEqual((Amount)123.75m, rounded);
        }

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
