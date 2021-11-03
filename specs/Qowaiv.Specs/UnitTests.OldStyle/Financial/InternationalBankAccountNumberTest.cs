using FluentAssertions;
using NUnit.Framework;
using Qowaiv.Financial;
using Qowaiv.Globalization;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests.Financial
{
    [TestFixture]
    public class InternationalBankAccountNumberTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly InternationalBankAccountNumber TestStruct = InternationalBankAccountNumber.Parse("NL20 INGB 0001 2345 67");

        #region IBAN const tests

        /// <summary>InternationalBankAccountNumber.Empty should be equal to the default of IBAN.</summary>
        [Test]
        public void Empty_None_EqualsDefault()
        {
            Assert.AreEqual(default(InternationalBankAccountNumber), InternationalBankAccountNumber.Empty);
        }

        #endregion

        #region IBAN IsEmpty tests

        /// <summary>InternationalBankAccountNumber.IsEmpty() should true for the default of IBAN.</summary>
        [Test]
        public void IsEmpty_Default_IsTrue()
        {
            Assert.IsTrue(default(InternationalBankAccountNumber).IsEmpty());
        }

        /// <summary>InternationalBankAccountNumber.IsEmpty() should false for the TestStruct.</summary>
        [Test]
        public void IsEmpty_Default_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmpty());
        }

        #endregion

        #region TryParse tests

        /// <summary>TryParse null should be valid.</summary>
        [Test]
        public void TryParse_Null_IsValid()
        {
            string str = null;
            Assert.IsTrue(InternationalBankAccountNumber.TryParse(str, out var val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TryParse_StringEmpty_IsValid()
        {
            string str = string.Empty;
            Assert.IsTrue(InternationalBankAccountNumber.TryParse(str, out var val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TryParse_StringValue_IsValid()
        {
            string str = "NL20INGB0001234567";
            Assert.IsTrue(InternationalBankAccountNumber.TryParse(str, out var val), "Valid");
            Assert.AreEqual(str, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TryParse_QuestionMark_IsValid()
        {
            string str = "?";
            Assert.IsTrue(InternationalBankAccountNumber.TryParse(str, out var val), "Valid");
            Assert.AreEqual(str, val.ToString(), "Value.ToString()");
            Assert.AreEqual(InternationalBankAccountNumber.Unknown, val, "Value");
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [Test]
        public void TryParse_StringValue_IsNotValid()
        {
            string str = "string";
            Assert.IsFalse(InternationalBankAccountNumber.TryParse(str, out var val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        [Test]
        public void Parse_InvalidInput_ThrowsFormatException()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Assert.Catch<FormatException>
                (() =>
                {
                    InternationalBankAccountNumber.Parse("InvalidInput");
                },
                "Not a valid IBAN");
            }
        }

        [Test]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var exp = TestStruct;
                var act = InternationalBankAccountNumber.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void TryParse_InvalidInput_DefaultValue()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var exp = default(InternationalBankAccountNumber);
                var act = InternationalBankAccountNumber.TryParse("InvalidInput");

                Assert.AreEqual(exp, act);
            }
        }

        #endregion

        #region (XML) (De)serialization tests

        [Test]
        public void SerializeDeserialize_TestStruct_AreEqual()
        {
            var input = InternationalBankAccountNumberTest.TestStruct;
            var exp = InternationalBankAccountNumberTest.TestStruct;
            var act = SerializeDeserialize.Binary(input);
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void DataContractSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = InternationalBankAccountNumberTest.TestStruct;
            var exp = InternationalBankAccountNumberTest.TestStruct;
            var act = SerializeDeserialize.DataContract(input);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void XmlSerialize_TestStruct_AreEqual()
        {
            var act = Serialize.Xml(TestStruct);
            var exp = "NL20INGB0001234567";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void XmlDeserialize_XmlString_AreEqual()
        {
            var act =Deserialize.Xml<InternationalBankAccountNumber>("NL20INGB0001234567");
            Assert.AreEqual(TestStruct, act);
        }


        [Test]
        public void SerializeDeserialize_InternationalBankAccountNumberSerializeObject_AreEqual()
        {
            var input = new InternationalBankAccountNumberSerializeObject
            {
                Id = 17,
                Obj = InternationalBankAccountNumberTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new InternationalBankAccountNumberSerializeObject
            {
                Id = 17,
                Obj = InternationalBankAccountNumberTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializeDeserialize.Binary(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void XmlSerializeDeserialize_InternationalBankAccountNumberSerializeObject_AreEqual()
        {
            var input = new InternationalBankAccountNumberSerializeObject
            {
                Id = 17,
                Obj = InternationalBankAccountNumberTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new InternationalBankAccountNumberSerializeObject
            {
                Id = 17,
                Obj = InternationalBankAccountNumberTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializeDeserialize.Xml(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void DataContractSerializeDeserialize_InternationalBankAccountNumberSerializeObject_AreEqual()
        {
            var input = new InternationalBankAccountNumberSerializeObject
            {
                Id = 17,
                Obj = InternationalBankAccountNumberTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new InternationalBankAccountNumberSerializeObject
            {
                Id = 17,
                Obj = InternationalBankAccountNumberTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializeDeserialize.DataContract(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void SerializeDeserialize_Empty_AreEqual()
        {
            var input = new InternationalBankAccountNumberSerializeObject
            {
                Id = 17,
                Obj = InternationalBankAccountNumber.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new InternationalBankAccountNumberSerializeObject
            {
                Id = 17,
                Obj = InternationalBankAccountNumber.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializeDeserialize.Binary(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void XmlSerializeDeserialize_Empty_AreEqual()
        {
            var input = new InternationalBankAccountNumberSerializeObject
            {
                Id = 17,
                Obj = InternationalBankAccountNumber.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new InternationalBankAccountNumberSerializeObject
            {
                Id = 17,
                Obj = InternationalBankAccountNumber.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializeDeserialize.Xml(input);
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
        public void FromJson_Invalid_Throws(object json)
        {
            Assert.Catch<FormatException>(() => JsonTester.Read<InternationalBankAccountNumber>(json));
        }
    
        [Test]
        public void FromJson_NL20INGB0001234567_EqualsTestStruct()
        {
            var actual = JsonTester.Read<InternationalBankAccountNumber>("NL20INGB0001234567");
            Assert.AreEqual(TestStruct, actual);
        }

        [Test]
        public void ToJson_DefaultValue_IsNull()
        {
            object act = JsonTester.Write(default(InternationalBankAccountNumber));
            Assert.IsNull(act);
        }
        [Test]
        public void ToJson_TestStruct_AreEqual()
        {
            var act = JsonTester.Write(TestStruct);
            var exp = "NL20INGB0001234567";
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IFormattable / ToString tests

        [Test]
        public void ToString_Empty_IsStringEmpty()
        {
            var act = InternationalBankAccountNumber.Empty.ToString();
            var exp = "";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("[f]", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: '[nl20 ingb 0001 2345 67]', format: '[f]'";

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_Unknown_IsStringEmpty()
        {
            var act = InternationalBankAccountNumber.Unknown.ToString("", null);
            var exp = "?";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_TestStruct_AreEqual()
        {
            var act = TestStruct.ToString();
            var exp = "NL20INGB0001234567";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_TestStructFormatULower_AreEqual()
        {
            var act = TestStruct.ToString("u");
            var exp = "nl20ingb0001234567";
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToString_TestStructFormatUUpper_AreEqual()
        {
            var act = TestStruct.ToString("U");
            var exp = "NL20INGB0001234567";
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToString_TestStructFormatFLower_AreEqual()
        {
            var act = TestStruct.ToString("f");
            var exp = "nl20 ingb 0001 2345 67";
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToString_TestStructFormatFUpper_AreEqual()
        {
            var act = TestStruct.ToString("F");
            var exp = "NL20 INGB 0001 2345 67";
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToString_EmptyFormatF_AreEqual()
        {
            var act = InternationalBankAccountNumber.Empty.ToString("F");
            var exp = "";
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToString_UnknownFormatF_AreEqual()
        {
            var act = InternationalBankAccountNumber.Unknown.ToString("F");
            var exp = "?";
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for InternationalBankAccountNumber.Empty.</summary>
        [Test]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, InternationalBankAccountNumber.Empty.GetHashCode());
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
            Assert.IsTrue(InternationalBankAccountNumber.Empty.Equals(InternationalBankAccountNumber.Empty));
        }

        [Test]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = InternationalBankAccountNumber.Parse("NL20 INGB 0001 2345 67");
            var r = InternationalBankAccountNumber.Parse("nl20ingb0001234567");

            Assert.IsTrue(l.Equals(r));
        }

        [Test]
        public void Equals_TestStructTestStruct_IsTrue()
        {
            Assert.IsTrue(InternationalBankAccountNumberTest.TestStruct.Equals(InternationalBankAccountNumberTest.TestStruct));
        }

        [Test]
        public void Equals_TestStructEmpty_IsFalse()
        {
            Assert.IsFalse(InternationalBankAccountNumberTest.TestStruct.Equals(InternationalBankAccountNumber.Empty));
        }

        [Test]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(InternationalBankAccountNumber.Empty.Equals(InternationalBankAccountNumberTest.TestStruct));
        }

        [Test]
        public void Equals_TestStructObjectTestStruct_IsTrue()
        {
            Assert.IsTrue(InternationalBankAccountNumberTest.TestStruct.Equals((object)InternationalBankAccountNumberTest.TestStruct));
        }

        [Test]
        public void Equals_TestStructNull_IsFalse()
        {
            Assert.IsFalse(InternationalBankAccountNumberTest.TestStruct.Equals(null));
        }

        [Test]
        public void Equals_TestStructObject_IsFalse()
        {
            Assert.IsFalse(InternationalBankAccountNumberTest.TestStruct.Equals(new object()));
        }

        [Test]
        public void OperatorIs_TestStructTestStruct_IsTrue()
        {
            var l = InternationalBankAccountNumberTest.TestStruct;
            var r = InternationalBankAccountNumberTest.TestStruct;
            Assert.IsTrue(l == r);
        }

        [Test]
        public void OperatorIsNot_TestStructTestStruct_IsFalse()
        {
            var l = InternationalBankAccountNumberTest.TestStruct;
            var r = InternationalBankAccountNumberTest.TestStruct;
            Assert.IsFalse(l != r);
        }

        #endregion

        #region IComparable tests

        /// <summary>Orders a list of IBANs ascending.</summary>
        [Test]
        public void OrderBy_InternationalBankAccountNumber_AreEqual()
        {
            var item0 = InternationalBankAccountNumber.Parse("AE950210000000693123456");
            var item1 = InternationalBankAccountNumber.Parse("BH29BMAG1299123456BH00");
            var item2 = InternationalBankAccountNumber.Parse("CY17002001280000001200527600");
            var item3 = InternationalBankAccountNumber.Parse("DK5000400440116243");

            var inp = new List<InternationalBankAccountNumber> { InternationalBankAccountNumber.Empty, item3, item2, item0, item1, InternationalBankAccountNumber.Empty };
            var exp = new List<InternationalBankAccountNumber> { InternationalBankAccountNumber.Empty, InternationalBankAccountNumber.Empty, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of IBANs descending.</summary>
        [Test]
        public void OrderByDescending_InternationalBankAccountNumber_AreEqual()
        {
            var item0 = InternationalBankAccountNumber.Parse("AE950210000000693123456");
            var item1 = InternationalBankAccountNumber.Parse("BH29BMAG1299123456BH00");
            var item2 = InternationalBankAccountNumber.Parse("CY17002001280000001200527600");
            var item3 = InternationalBankAccountNumber.Parse("DK5000400440116243");

            var inp = new List<InternationalBankAccountNumber> { InternationalBankAccountNumber.Empty, item3, item2, item0, item1, InternationalBankAccountNumber.Empty };
            var exp = new List<InternationalBankAccountNumber> { item3, item2, item1, item0, InternationalBankAccountNumber.Empty, InternationalBankAccountNumber.Empty };
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
            Action compare = () => TestStruct.CompareTo(new object());
            compare.Should().Throw<ArgumentException>();
        }
        #endregion

        #region Casting tests

        [Test]
        public void Explicit_StringToInternationalBankAccountNumber_AreEqual()
        {
            var exp = TestStruct;
            var act = (InternationalBankAccountNumber)TestStruct.ToString();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_InternationalBankAccountNumberToString_AreEqual()
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
            var act = InternationalBankAccountNumber.Empty.Length;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Length_Unknown_0()
        {
            var exp = 0;
            var act = InternationalBankAccountNumber.Unknown.Length;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Length_TestStruct_IntValue()
        {
            var exp = 18;
            var act = TestStruct.Length;
            Assert.AreEqual(exp, act);
        }


        [Test]
        public void Country_Empty_Null()
        {
            var exp = Country.Empty;
            var act = InternationalBankAccountNumber.Empty.Country;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Country_Unknown_Null()
        {
            var exp = Country.Unknown;
            var act = InternationalBankAccountNumber.Unknown.Country;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Country_TestStruct_Null()
        {
            var exp = Country.NL;
            var act = TestStruct.Country;
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region Type converter tests

        [Test]
        public void ConverterExists_InternationalBankAccountNumber_IsTrue()
            => typeof(InternationalBankAccountNumber).Should().HaveTypeConverterDefined();

        [Test]
        public void CanNotConvertFromInt32_InternationalBankAccountNumber_IsTrue()
        {
            TypeConverterAssert.CanNotConvertFrom(typeof(InternationalBankAccountNumber), typeof(Int32));
        }
        [Test]
        public void CanNotConvertToInt32_InternationalBankAccountNumber_IsTrue()
        {
            TypeConverterAssert.CanNotConvertTo(typeof(InternationalBankAccountNumber), typeof(Int32));
        }

        [Test]
        public void CanConvertFromString_InternationalBankAccountNumber_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(InternationalBankAccountNumber));
        }

        [Test]
        public void CanConvertToString_InternationalBankAccountNumber_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(InternationalBankAccountNumber));
        }

        [Test]
        public void ConvertFrom_StringNull_InternationalBankAccountNumberEmpty()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(InternationalBankAccountNumber.Empty, (string)null);
            }
        }

        [Test]
        public void ConvertFrom_StringEmpty_InternationalBankAccountNumberEmpty()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(InternationalBankAccountNumber.Empty, string.Empty);
            }
        }

        [Test]
        public void ConvertFromString_StringValue_TestStruct()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(InternationalBankAccountNumberTest.TestStruct, InternationalBankAccountNumberTest.TestStruct.ToString());
            }
        }

        [Test]
        public void ConvertToString_TestStruct_StringValue()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertToStringEquals(InternationalBankAccountNumberTest.TestStruct.ToString(), InternationalBankAccountNumberTest.TestStruct);
            }
        }

        #endregion
    }

    [Serializable]
    public class InternationalBankAccountNumberSerializeObject
    {
        public int Id { get; set; }
        public InternationalBankAccountNumber Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
