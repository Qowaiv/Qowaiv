using NUnit.Framework;
using Qowaiv;
using Qowaiv.Globalization;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;
using Qowaiv.UnitTests;
using Qowaiv.UnitTests.TestTools;
using System;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace YesNo_specs
{
    /// <summary>Tests the Yes-no SVO.</summary>
    public class YesNoTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly YesNo TestStruct = YesNo.Yes;

        #region Yes-no const tests

        /// <summary>YesNo.Empty should be equal to the default of Yes-no.</summary>
        [Test]
        public void Empty_None_EqualsDefault()
        {
            Assert.AreEqual(default(YesNo), YesNo.Empty);
        }

        #endregion

        #region Yes-no IsEmpty tests

        /// <summary>YesNo.IsEmpty() should be true for the default of Yes-no.</summary>
        [Test]
        public void IsEmpty_Default_IsTrue()
        {
            Assert.IsTrue(default(YesNo).IsEmpty());
        }
        /// <summary>YesNo.IsEmpty() should be false for YesNo.Unknown.</summary>
        [Test]
        public void IsEmpty_Unknown_IsFalse()
        {
            Assert.IsFalse(YesNo.Unknown.IsEmpty());
        }
        /// <summary>YesNo.IsEmpty() should be false for the TestStruct.</summary>
        [Test]
        public void IsEmpty_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmpty());
        }

        /// <summary>YesNo.IsUnknown() should be false for the default of Yes-no.</summary>
        [Test]
        public void IsUnknown_Default_IsFalse()
        {
            Assert.IsFalse(default(YesNo).IsUnknown());
        }
        /// <summary>YesNo.IsUnknown() should be true for YesNo.Unknown.</summary>
        [Test]
        public void IsUnknown_Unknown_IsTrue()
        {
            Assert.IsTrue(YesNo.Unknown.IsUnknown());
        }
        /// <summary>YesNo.IsUnknown() should be false for the TestStruct.</summary>
        [Test]
        public void IsUnknown_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsUnknown());
        }

        /// <summary>YesNo.IsEmptyOrUnknown() should be true for the default of Yes-no.</summary>
        [Test]
        public void IsEmptyOrUnknown_Default_IsFalse()
        {
            Assert.IsTrue(default(YesNo).IsEmptyOrUnknown());
        }
        /// <summary>YesNo.IsEmptyOrUnknown() should be true for YesNo.Unknown.</summary>
        [Test]
        public void IsEmptyOrUnknown_Unknown_IsTrue()
        {
            Assert.IsTrue(YesNo.Unknown.IsEmptyOrUnknown());
        }
        /// <summary>YesNo.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
        [Test]
        public void IsEmptyOrUnknown_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmptyOrUnknown());
        }

        [TestCase(true, "yes")]
        [TestCase(true, "no")]
        [TestCase(false, "?")]
        [TestCase(false, "")]
        public void IsYesOrNo(bool expected, YesNo yesNo)
        {
            Assert.AreEqual(expected, yesNo.IsYesOrNo());
        }

        #endregion

        #region Boolean mappings

        [TestCase(false, "")]
        [TestCase(false, "N")]
        [TestCase(true, "Y")]
        [TestCase(false, "?")]
        public void IsYes(bool expected, string str)
        {
            var yesNo = YesNo.Parse(str);
            Assert.AreEqual(expected, yesNo.IsYes());
        }

        [TestCase(false, "")]
        [TestCase(true, "N")]
        [TestCase(false, "Y")]
        [TestCase(false, "?")]
        public void IsNo(bool expected, string str)
        {
            var yesNo = YesNo.Parse(str);
            Assert.AreEqual(expected, yesNo.IsNo());
        }

        #endregion

        #region TryParse tests

        /// <summary>TryParse null should be valid.</summary>
        [Test]
        public void TryParse_Null_IsValid()
        {
            string str = null;

            Assert.IsTrue(YesNo.TryParse(str, out YesNo val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TryParse_StringEmpty_IsValid()
        {
            string str = string.Empty;

            Assert.IsTrue(YesNo.TryParse(str, out YesNo val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse "?" should be valid and the result should be YesNo.Unknown.</summary>
        [Test]
        public void TryParse_Questionmark_IsValid()
        {
            string str = "?";

            Assert.IsTrue(YesNo.TryParse(str, out YesNo val), "Valid");
            Assert.IsTrue(val.IsUnknown(), "Value");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TryParse_StringValue_IsValid()
        {
            using (TestCultures.En.Scoped())
            {
                string str = "yes";
                Assert.IsTrue(YesNo.TryParse(str, out YesNo val), "Valid");
                Assert.AreEqual(str, val.ToString(), "Value");
            }
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [Test]
        public void TryParse_StringValue_IsNotValid()
        {
            string str = "string";

            Assert.IsFalse(YesNo.TryParse(str, out YesNo val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        [Test]
        public void Parse_Unknown_AreEqual()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var act = YesNo.Parse("?");
                var exp = YesNo.Unknown;
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
                    YesNo.Parse("InvalidInput");
                },
                "Not a valid Yes-no");
            }
        }

        [Test]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var exp = TestStruct;
                var act = YesNo.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void TryParse_InvalidInput_DefaultValue()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var exp = default(YesNo);
                var act = YesNo.TryParse("InvalidInput");

                Assert.AreEqual(exp, act);
            }
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for YesNo.Empty.</summary>
        [Test]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, YesNo.Empty.GetHashCode());
        }

        /// <summary>GetHash should not fail for the test struct.</summary>
        [Test]
        public void GetHash_TestStruct_Hash()
        {
            Assert.AreEqual(2, TestStruct.GetHashCode());
        }

        [Test]
        public void Equals_EmptyEmpty_IsTrue()
        {
            Assert.IsTrue(YesNo.Empty.Equals(YesNo.Empty));
        }

        [Test]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = YesNo.Parse("Yes", CultureInfo.InvariantCulture);
            var r = YesNo.Parse("yes", CultureInfo.InvariantCulture);

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
            Assert.IsFalse(TestStruct.Equals(YesNo.Empty));
        }

        [Test]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(YesNo.Empty.Equals(TestStruct));
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

        /// <summary>Orders a list of Yes-nos ascending.</summary>
        [Test]
        public void OrderBy_YesNo_AreEqual()
        {
            var item0 = YesNo.No;
            var item1 = YesNo.Yes;
            var item2 = YesNo.Unknown;

            var inp = new []{ YesNo.Empty, item2, item0, item1, YesNo.Empty };
            var exp = new []{ YesNo.Empty, YesNo.Empty, item0, item1, item2 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of Yes-nos descending.</summary>
        [Test]
        public void OrderByDescending_YesNo_AreEqual()
        {
            var item0 = YesNo.No;
            var item1 = YesNo.Yes;
            var item2 = YesNo.Unknown;

            var inp = new []{ YesNo.Empty, item2, item0, item1, YesNo.Empty };
            var exp = new []{ item2, item1, item0, YesNo.Empty, YesNo.Empty };
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
                TestStruct.CompareTo(new object());
            },
            "obj",
            "Argument must be YesNo."
            );
        }

        #endregion

        #region Casting tests

        [Test]
        public void Explicit_StringToYesNo_AreEqual()
        {
            var exp = TestStruct;
            var act = (YesNo)TestStruct.ToString();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_YesNoToString_AreEqual()
        {
            var exp = TestStruct.ToString();
            var act = (string)TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Explicit_BoolToYesNo_Empty()
        {
            var yesNo = (YesNo)default(bool?);
            Assert.AreEqual(YesNo.Empty, yesNo);
        }
        [Test]
        public void Explicit_BoolToYesNo_No()
        {
            var yesNo = (YesNo)false;
            Assert.AreEqual(YesNo.No, yesNo);
        }
        [Test]
        public void Explicit_BoolToYesNo_Yes()
        {
            var yesNo = (YesNo)true;
            Assert.AreEqual(YesNo.Yes, yesNo);
        }

        [TestCase(null, "")]
        [TestCase(true, "Y")]
        [TestCase(false, "N")]
        [TestCase(null, "?")]
        public void Explicit_YesNoToNullableBool(bool? expected, YesNo yesNo)
        {
            var casted = (bool?)yesNo;
            Assert.AreEqual(expected, casted);
        }

        [TestCase(false, "")]
        [TestCase(true, "Y")]
        [TestCase(false, "N")]
        [TestCase(false, "?")]
        public void Explicit_YesNoToBool(bool expected, YesNo yesNo)
        {
            var casted = (bool)yesNo;
            Assert.AreEqual(expected, casted);
        }

        [Test]
        public void Implicit_InIfStatement()
        {
            Assert.Throws<DivideByZeroException>(() =>
            {
                var answer = YesNo.Yes;
                if (answer)
                {
                    throw new DivideByZeroException();
                }
            });
        }

        #endregion

        #region Type converter tests

        [Test]
        public void ConverterExists_YesNo_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(YesNo));
        }

        [Test]
        public void CanNotConvertFromInt32_YesNo_IsTrue()
        {
            TypeConverterAssert.CanNotConvertFrom(typeof(YesNo), typeof(Int32));
        }
        [Test]
        public void CanNotConvertToInt32_YesNo_IsTrue()
        {
            TypeConverterAssert.CanNotConvertTo(typeof(YesNo), typeof(Int32));
        }

        [Test]
        public void CanConvertFromString_YesNo_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(YesNo));
        }

        [Test]
        public void CanConvertToString_YesNo_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(YesNo));
        }

        [Test]
        public void ConvertFrom_StringNull_YesNoEmpty()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(YesNo.Empty, (string)null);
            }
        }

        [Test]
        public void ConvertFrom_StringEmpty_YesNoEmpty()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(YesNo.Empty, string.Empty);
            }
        }

        [Test]
        public void ConvertFromString_StringValue_TestStruct()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(TestStruct, TestStruct.ToString());
            }
        }

        [Test]
        public void ConvertToString_TestStruct_StringValue()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertToStringEquals(TestStruct.ToString(), TestStruct);
            }
        }

        #endregion

        #region IsValid tests

        [TestCase(null)]
        [TestCase("")]
        [TestCase("Complex")]
        public void IsInvalid_String(string pattern)
        {
            Assert.IsFalse(YesNo.IsValid(pattern));
        }

        [Test]
        public void IsValid_Data_IsTrue()
        {
            Assert.IsTrue(YesNo.IsValid("Unknown"));
        }
        #endregion
    }

    public class Has_custom_formatting
    {
        [Test]
        public void default_value_is_represented_as_string_empty()
        {
            Assert.AreEqual(string.Empty, default(YesNo).ToString());
        }

        [Test]
        public void unknown_value_is_represented_as_unknown()
        {
            Assert.AreEqual("unknown", YesNo.Unknown.ToString());
        }

        [Test]
        public void custom_format_provider_is_applied()
        {
            var formatted = Svo.YesNo.ToString("B", new UnitTestFormatProvider());
            Assert.AreEqual("Unit Test Formatter, value: 'True', format: 'B'", formatted);
        }

        [TestCase("en-GB", null, "Yes", "yes")]
        [TestCase("nl-BE", "f", "Yes", "ja")]
        [TestCase("es-EQ", "F", "Yes", "Si")]
        [TestCase("en-GB", null, "No", "no")]
        [TestCase("nl-BE", "f", "No", "nee")]
        [TestCase("es-EQ", "F", "No", "No")]
        [TestCase("en-GB", "C", "Yes", "Y")]
        [TestCase("nl-BE", "C", "Yes", "J")]
        [TestCase("es-EQ", "C", "Yes", "S")]
        [TestCase("en-GB", "C", "No", "N")]
        [TestCase("nl-BE", "c", "No", "n")]
        [TestCase("es-EQ", "c", "No", "n")]
        [TestCase("en-US", "B", "Yes", "True")]
        [TestCase("en-US", "b", "No", "false")]
        [TestCase("en-US", "i", "Yes", "1")]
        [TestCase("en-US", "i", "No", "0")]
        [TestCase("en-US", "i", "?", "?")]
        public void culture_dependend(CultureInfo culture, string format, YesNo svo, string expected)
        {
            using (culture.Scoped())
            {
                Assert.AreEqual(expected, svo.ToString(format));
            }
        }
    }

    public class Supports_JSON_serialization
    {
        [TestCase("yes", "yes")]
        [TestCase("yes", true)]
        [TestCase("yes", 1L)]
        [TestCase("yes", 1.1)]
        [TestCase("no", 0.0)]
        [TestCase("?", "unknown")]
        public void convension_based_deserialization(YesNo expected, object json)
        {
            var actual = JsonTester.Read<YesNo>(json);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(null, "")]
        [TestCase("yes", "yes")]
        public void convension_based_serialization(object expected, YesNo svo)
        {
            var serialized = JsonTester.Write(svo);
            Assert.AreEqual(expected, serialized);
        }

        [TestCase("Invalid input", typeof(FormatException))]
        [TestCase("2017-06-11", typeof(FormatException))]
        [TestCase(5L, typeof(ArgumentOutOfRangeException))]
        public void throws_for_invalid_json(object json, Type exceptionType)
        {
            var exception = Assert.Catch(() => JsonTester.Read<YesNo>(json));
            Assert.IsInstanceOf(exceptionType, exception);
        }
    }

    public class Supports_XML_serialization
    {
        [Test]
        public void using_XmlSerializer_to_serialize()
        {
            var xml = SerializationTest.XmlSerialize(Svo.YesNo);
            Assert.AreEqual("yes", xml);
        }

        [Test]
        public void using_XmlSerializer_to_deserialize()
        {
            var svo = SerializationTest.XmlDeserialize<YesNo>("yes");
            Assert.AreEqual(Svo.YesNo, svo);
        }

        [Test]
        public void using_data_contract_serializer()
        {
            var round_tripped = SerializationTest.DataContractSerializeDeserialize(Svo.YesNo);
            Assert.AreEqual(Svo.YesNo, round_tripped);
        }

        [Test]
        public void as_part_of_a_structure()
        {
            var structure = XmlStructure.New(Svo.YesNo);
            var round_tripped = SerializationTest.XmlSerializeDeserialize(structure);

            Assert.AreEqual(structure, round_tripped);
        }

        [Test]
        public void has_no_custom_XML_schema()
        {
            IXmlSerializable obj = Svo.YesNo;
            Assert.IsNull(obj.GetSchema());
        }
    }

    public class Supports_binary_serialization
    {
        [Test]
        public void using_BinaryFormatter()
        {
            var round_tripped = SerializationTest.BinaryFormatterSerializeDeserialize(Svo.YesNo);
            Assert.AreEqual(Svo.YesNo, round_tripped);
        }

        [Test]
        public void storing_byte_in_SerializationInfo()
        {
            var info = SerializationTest.GetSerializationInfo(Svo.YesNo);
            Assert.AreEqual((byte)2, info.GetByte("Value"));
        }
    }
}
