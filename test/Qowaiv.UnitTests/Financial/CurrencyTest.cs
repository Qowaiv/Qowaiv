using NUnit.Framework;
using Qowaiv.Financial;
using Qowaiv.Globalization;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests.Financial
{
    /// <summary>Tests the currency SVO.</summary>
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
            using (TestCultures.Nl_NL.Scoped())
            {
                var act = Currency.Current;
                var exp = Currency.EUR;

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void Current_CurrentCultureEsEC_Ecuador()
        {
            using (TestCultures.Es_EC.Scoped())
            {
                var act = Currency.Current;
                var exp = Currency.USD;

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void Current_CurrentCultureEn_Empty()
        {
            using (TestCultures.En.Scoped())
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
            string str = null;
            Assert.IsTrue(Currency.TryParse(str, out Currency val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TyrParse_StringEmpty_IsValid()
        {
            string str = string.Empty;
            Assert.IsTrue(Currency.TryParse(str, out Currency val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse "?" should be valid and the result should be Currency.Unknown.</summary>
        [Test]
        public void TyrParse_Questionmark_IsValid()
        {
            string str = "?";
            Assert.IsTrue(Currency.TryParse(str, out Currency val), "Valid");
            Assert.IsTrue(val.IsUnknown(), "Value");
        }

        /// <summary>TryParse "¤" should be valid and the result should be Currency.Unknown.</summary>
        [Test]
        public void TyrParse_UnknownCurrencySymbol_IsValid()
        {
            string str = "¤";
            Assert.IsTrue(Currency.TryParse(str, out Currency val), "Valid");
            Assert.IsTrue(val.IsUnknown(), "Value");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TyrParse_StringValue_IsValid()
        {
            string str = "USD";
            Assert.IsTrue(Currency.TryParse(str, out Currency val), "Valid");
            Assert.AreEqual(str, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [Test]
        public void TyrParse_StringValue_IsNotValid()
        {
            string str = "string";
            Assert.IsFalse(Currency.TryParse(str, out Currency val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        [Test]
        public void Parse_Unknown_AreEqual()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var act = Currency.Parse("?");
                var exp = Currency.Unknown;
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void Parse_InvalidInput_ThrowsFormatException()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Assert.Catch<FormatException>
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
            using (TestCultures.En_GB.Scoped())
            {
                var exp = TestStruct;
                var act = Currency.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void TryParse_InvalidInput_DefaultValue()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var exp = default(Currency);
                var act = Currency.TryParse("InvalidInput");

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void Parse_EuroSign_EUR()
        {
            var act = Currency.Parse("€");
            var exp = Currency.EUR;
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
                SerializationTest.DeserializeUsingConstructor<Currency>(null, default);
            },
            "info");
        }

        [Test]
        public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
        {
            Assert.Catch<SerializationException>
            (() =>
            {
                var info = new SerializationInfo(typeof(Currency), new System.Runtime.Serialization.FormatterConverter());
                SerializationTest.DeserializeUsingConstructor<Currency>(info, default);
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
            var info = new SerializationInfo(typeof(Currency), new System.Runtime.Serialization.FormatterConverter());
            obj.GetObjectData(info, default);

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
        public void XmlSerialize_TestStruct_AreEqual()
        {
            var act = SerializationTest.XmlSerialize(TestStruct);
            var exp = "EUR";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void XmlDeserialize_XmlString_AreEqual()
        {
            var act = SerializationTest.XmlDeserialize<Currency>("EUR");
            Assert.AreEqual(TestStruct, act);
        }

        [Test]
        public void SerializeDeserialize_CurrencySerializeObject_AreEqual()
        {
            var input = new CurrencySerializeObject
            {
                Id = 17,
                Obj = CurrencyTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new CurrencySerializeObject
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
            var input = new CurrencySerializeObject
            {
                Id = 17,
                Obj = CurrencyTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new CurrencySerializeObject
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
            var input = new CurrencySerializeObject
            {
                Id = 17,
                Obj = CurrencyTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new CurrencySerializeObject
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
            var input = new CurrencySerializeObject
            {
                Id = 17,
                Obj = Currency.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new CurrencySerializeObject
            {
                Id = 17,
                Obj = Currency.Empty,
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
            var input = new CurrencySerializeObject
            {
                Id = 17,
                Obj = Currency.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new CurrencySerializeObject
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

        [TestCase("Invalid input")]
        [TestCase("2017-06-11")]
        [TestCase(long.MinValue)]
        public void FromJson_Invalid_Throws(object json)
        {
            Assert.Catch<FormatException>(() => JsonTester.Read<Currency>(json));
        }

        [Test]
        public void FromJson_EUR_EqualsTestStruct()
        {
            var actual = JsonTester.Read<Currency>("EUR");
            Assert.AreEqual(TestStruct, actual);
        }

        [Test]
        public void ToJson_DefaultValue_IsNull()
        {
            object act = JsonTester.Write(default(Currency));
            Assert.IsNull(act);
        }
        [Test]
        public void ToJson_TestStruct_AreEqual()
        {
            var act = JsonTester.Write(TestStruct);
            var exp = "EUR";
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
            var act = TestStruct.ToString("$: e", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: '€: Euro', format: '$: e'";

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
            using (TestCultures.Nl_BE.Scoped())
            {
                var act = Currency.Parse("Amerikaanse dollar").ToString();
                var exp = "USD";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_ValueEnglishGreatBritain_AreEqual()
        {
            using (TestCultures.En_GB.Scoped())
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
            DebuggerDisplayAssert.HasResult("{empty}", default(Currency));
        }
        [Test]
        public void DebuggerDisplay_Unknown_String()
        {
            DebuggerDisplayAssert.HasResult("?", Currency.Unknown);
        }

        [Test]
        public void DebuggerDisplay_TestStruct_String()
        {
            DebuggerDisplayAssert.HasResult("Euro (EUR/978)", TestStruct);
        }

        #endregion

        #region IFormatProvider

        [Test]
        public void FormatAmount_BYR_NAf12Dot34()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Amount number = (Amount)1200.34m;
                var act = number.ToString("C", Currency.BYR);
                var exp = "BYR1,200";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void FormatAmount_ANG_NAf12Dot34()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Amount number = (Amount)12.34m;
                var act = number.ToString("C", Currency.ANG);
                var exp = "NAf.12.34";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void FormatDecimal_ANG_ANG12Dot34()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var number = 12.34m;
                var act = number.ToString("C", Currency.ANG);
                var exp = "NAf.12.34";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void FormatDecimal_TND_TND12Dot340()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var number = 12.34m;
                var act = number.ToString("C", Currency.TND);
                var exp = "TND12.340";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void FormatDecimal_EUR_E12Dot34()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var number = 12.34m;
                var act = number.ToString("C", Currency.EUR);
                var exp = "€12.34";

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void FormatDouble_EUR_E12Dot34()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var number = 12.34;
                var act = number.ToString("C", Currency.EUR);
                var exp = "€12.34";

                Assert.AreEqual(exp, act);
            }
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
        public void GetHash_TestStruct_NotZero()
        {
            Assert.NotZero(TestStruct.GetHashCode());
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

            var inp = new List<Currency> { Currency.Empty, item3, item2, item0, item1, Currency.Empty };
            var exp = new List<Currency> { Currency.Empty, Currency.Empty, item0, item1, item2, item3 };
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

            var inp = new List<Currency> { Currency.Empty, item3, item2, item0, item1, Currency.Empty };
            var exp = new List<Currency> { item3, item2, item1, item0, Currency.Empty, Currency.Empty };
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

        /// <summary>Compare with null should return 1.</summary>
        [Test]
        public void CompareTo_null_1()
        {
            object @null = null;
            Assert.AreEqual(1, TestStruct.CompareTo(@null));
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
                "Argument must be Currency."
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

        [Test]
        public void Explicit_Int32ToCurrency_AreEqual()
        {
            var exp = TestStruct;
            var act = (Currency)TestStruct.IsoCode;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_Minus3ToCurrency_CurrencyEmpty()
        {
            var exp = Currency.Empty;
            var act = (Currency)(-3);

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_CurrencyToInt32_AreEqual()
        {
            var exp = TestStruct.IsoNumericCode;
            var act = (Int32)TestStruct;

            Assert.AreEqual(exp, act);
        }

        #endregion

        #region Properties

        [Test]
        public void Symbol_AZN_Special()
        {
            var act = Currency.AZN.Symbol;
            var exp = "₼";
            Assert.AreEqual(exp, act);
        }

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
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(Currency.Empty, (string)null);
            }
        }

        [Test]
        public void ConvertFrom_StringEmpty_CurrencyEmpty()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(Currency.Empty, string.Empty);
            }
        }

        [Test]
        public void ConvertFromString_StringValue_TestStruct()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(CurrencyTest.TestStruct, CurrencyTest.TestStruct.ToString());
            }
        }

        [Test]
        public void ConvertToString_TestStruct_StringValue()
        {
            using (TestCultures.En_GB.Scoped())
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
            Assert.IsFalse(Currency.IsValid(string.Empty), "string.Empty");
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
