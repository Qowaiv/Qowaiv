using NUnit.Framework;
using Qowaiv;
using Qowaiv.Globalization;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;
using Qowaiv.UnitTests;
using Qowaiv.UnitTests.TestTools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace YesNo_specs
{
    public class With_domain_logic
    {
        [TestCase(false, "yes")]
        [TestCase(false, "no")]
        [TestCase(false, "?")]
        [TestCase(true, "")]
        public void IsEmpty_returns(bool result, YesNo svo)
        {
            Assert.AreEqual(result, svo.IsEmpty());
        }

        [TestCase(false, "yes")]
        [TestCase(false, "no")]
        [TestCase(true, "?")]
        [TestCase(true, "")]
        public void IsEmptyOrUnknown_returns(bool result, YesNo svo)
        {
            Assert.AreEqual(result, svo.IsEmptyOrUnknown());
        }

        [TestCase(false, "yes")]
        [TestCase(false, "no")]
        [TestCase(true, "?")]
        [TestCase(false, "")]
        public void IsUnknown_returns(bool result, YesNo svo)
        {
            Assert.AreEqual(result, svo.IsUnknown());
        }

        [TestCase(true, "yes")]
        [TestCase(true, "no")]
        [TestCase(false, "?")]
        [TestCase(false, "")]
        public void IsYesOrNo_returns(bool result, YesNo svo)
        {
            Assert.AreEqual(result, svo.IsYesOrNo());
        }

        [TestCase(false, "")]
        [TestCase(false, "N")]
        [TestCase(true, "Y")]
        [TestCase(false, "?")]

        public void IsYes_returns(bool result, YesNo svo)
        {
            Assert.AreEqual(result, svo.IsYes());
        }

        [TestCase(false, "")]
        [TestCase(true, "N")]
        [TestCase(false, "Y")]
        [TestCase(false, "?")]

        public void IsNo_returns(bool result, YesNo svo)
        {
            Assert.AreEqual(result, svo.IsNo());
        }
    }

    public class Is_valid_for
    {
        [TestCase("?")]
        [TestCase("unknown")]
        public void strings_representing_unknown(string input)
        {
            Assert.IsTrue(YesNo.IsValid(input));
        }

        [TestCase("ja", "nl")]
        [TestCase("yes", "en-GB")]
        [TestCase("y", null)]
        [TestCase("true", null)]
        public void strings_representing_yes(string input, CultureInfo culture)
        {
            Assert.IsTrue(YesNo.IsValid(input, culture));
        }

        [TestCase("nee", "nl")]
        [TestCase("no", "en-GB")]
        [TestCase("n", null)]
        [TestCase("false", null)]
        public void strings_representing_no(string input, CultureInfo culture)
        {
            Assert.IsTrue(YesNo.IsValid(input, culture));
        }
    }

    public class Is_not_valid_for
    {
        [Test]
        public void string_empty()
        {
            Assert.IsFalse(YesNo.IsValid(string.Empty));
        }

        [Test]
        public void string_null()
        {
            Assert.IsFalse(YesNo.IsValid(null));
        }

        [Test]
        public void whitespace()
        {
            Assert.IsFalse(YesNo.IsValid(" "));
        }

        [Test]
        public void garbage()
        {
            Assert.IsFalse(YesNo.IsValid("garbage"));
        }
    }

    public class Has_constant
    {
        [Test]
        public void Empty_represent_default_value()
        {
            Assert.AreEqual(default(YesNo), YesNo.Empty);
        }
    }

    public class Is_equal_by_value
    {
        [Test]
        public void not_equal_to_null()
        {
            Assert.IsFalse(Svo.YesNo.Equals(null));
        }

        [Test]
        public void not_equal_to_other_type()
        {
            Assert.IsFalse(Svo.YesNo.Equals(new object()));
        }

        [Test]
        public void not_equal_to_different_value()
        {
            Assert.IsFalse(Svo.YesNo.Equals(YesNo.Unknown));
        }

        [Test]
        public void equal_to_same_value()
        {
            Assert.IsTrue(Svo.YesNo.Equals(YesNo.Yes));
        }

        [Test]
        public void equal_operator_returns_true_for_same_values()
        {
            Assert.IsTrue(YesNo.Yes == Svo.YesNo);
        }

        [Test]
        public void equal_operator_returns_false_for_different_values()
        {
            Assert.IsFalse(YesNo.Yes == YesNo.No);
        }

        [Test]
        public void not_equal_operator_returns_false_for_same_values()
        {
            Assert.IsFalse(YesNo.Yes != Svo.YesNo);
        }

        [Test]
        public void not_equal_operator_returns_true_for_different_values()
        {
            Assert.IsTrue(YesNo.Yes != YesNo.No);
        }

        [TestCase("", 0)]
        [TestCase("yes", 2)]
        public void hash_code_is_value_based(YesNo svo, int hashcode)
        {
            Assert.AreEqual(hashcode, svo.GetHashCode());
        }
    }

    public class Can_be_parsed
    {
        [Test]
        public void from_null_string_represents_Empty()
        {
            Assert.AreEqual(YesNo.Empty, YesNo.Parse(null));
        }

        [Test]
        public void from_empty_string_represents_Empty()
        {
            Assert.AreEqual(YesNo.Empty, YesNo.Parse(string.Empty));
        }

        [Test]
        public void from_question_mark_represents_Unknown()
        {
            Assert.AreEqual(YesNo.Unknown, YesNo.Parse("?"));
        }

        [TestCase("en", "y")]
        [TestCase("nl", "j")]
        [TestCase("nl", "ja")]
        [TestCase("fr", "oui")]
        public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
        {
            using (culture.Scoped())
            {
                var parsed = YesNo.Parse(input);
                Assert.AreEqual(Svo.YesNo, parsed);
            }
        }

        [Test]
        public void from_valid_input_only_otherwise_throws_on_Parse()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var exception = Assert.Throws<FormatException>(() => YesNo.Parse("invalid input"));
                Assert.AreEqual("Not a valid yes-no value", exception.Message);
            }
        }

        [Test]
        public void from_valid_input_only_otherwise_return_false_on_TryParse()
        {
            Assert.IsFalse(YesNo.TryParse("invalid input", out _));
        }
    
        [Test]
        public void from_invalid_as_empty_with_TryParse()
        {
            Assert.AreEqual(default(YesNo), YesNo.TryParse("invalid input"));
        }

        [Test]
        public void with_TryParse_returns_SVO()
        {
            Assert.AreEqual(Svo.YesNo, YesNo.TryParse("yes"));
        }
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
        public void culture_dependent(CultureInfo culture, string format, YesNo svo, string expected)
        {
            using (culture.Scoped())
            {
                Assert.AreEqual(expected, svo.ToString(format));
            }
        }

        [Test]
        public void with_current_thread_culture_as_default()
        {
            using (new CultureInfoScope(
                culture: TestCultures.Nl_NL,
                cultureUI: TestCultures.En_GB))
            {
                Assert.AreEqual("ja", Svo.YesNo.ToString(provider: null));
            }
        }
    }

    public class Is_comparable
    {
        [Test]
        public void to_null()
        {
            Assert.AreEqual(1, Svo.YesNo.CompareTo(null));
        }

        [Test]
        public void to_YesNo_as_object()
        {
            object obj = Svo.YesNo;
            Assert.AreEqual(0, Svo.YesNo.CompareTo(obj));
        }

        [Test]
        public void to_YesNo_only()
        {
            Assert.Throws<ArgumentException>(() => Svo.YesNo.CompareTo(new object()));
        }

        [Test]
        public void can_be_sorted_using_compare()
        {
            var sorted = new[]
            {
                YesNo.Empty,
                YesNo.Empty,
                YesNo.No,
                YesNo.Yes,
                YesNo.Unknown,
            };
            var list = new List<YesNo> { sorted[3], sorted[4], sorted[2], sorted[0], sorted[1] };
            list.Sort();

            Assert.AreEqual(sorted, list);
        }
    }

    public class Casts
    {
        [Test]
        public void explicitly_from_string()
        {
            var casted = (YesNo)"yes";
            Assert.AreEqual(YesNo.Yes, casted);
        }

        [Test]
        public void explicitly_to_string()
        {
            var casted = (string)Svo.YesNo;
            Assert.AreEqual("yes", casted);
        }

        [TestCase("yes", true)]
        [TestCase("no", false)]
        public void explicitly_from_boolean(YesNo casted, bool boolean)
        {
            Assert.AreEqual(casted, (YesNo)boolean);
        }

        [Test]
        public void yes_implicitly_to_true()
        {
            var result = YesNo.Yes ? "passed" : "failed";
            Assert.AreEqual("passed", result);
        }

        [Test]
        public void no_implicitly_to_false()
        {
            var result = YesNo.No ? "passed" : "failed";
            Assert.AreNotEqual("passed", result);
        }

        [TestCase(null, "")]
        [TestCase(true, "y")]
        [TestCase(false, "n")]
        public void explicitly_from_nullable_boolean(bool? value, YesNo expected)
        {
            var casted = (YesNo)value;
            Assert.AreEqual(expected, casted);
        }

        [TestCase("", null)]
        [TestCase("y", true)]
        [TestCase("n", false)]
        [TestCase("?", null)]
        public void explicitly_to_nullable_boolean(YesNo svo, bool? expected)
        {
            var casted = (bool?)svo;
            Assert.AreEqual(expected, casted);
        }

        [TestCase("", null)]
        [TestCase("y", true)]
        [TestCase("n", false)]
        [TestCase("?", false)]
        public void explicitly_to_boolean(YesNo svo, bool expected)
        {
            var casted = (bool)svo;
            Assert.AreEqual(expected, casted);
        }
    }

    public class Supports_type_conversion
    {
        [Test]
        public void via_TypeConverter_registered_with_attribute()
        {
            TypeConverterAssert.ConverterExists(typeof(YesNo));
        }

        [Test]
        public void from_null_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(default(YesNo), null);
            }
        }

        [Test]
        public void from_empty_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(default(YesNo), string.Empty);
            }
        }

        [Test]
        public void from_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(Svo.YesNo, Svo.YesNo.ToString());
            }
        }

        [Test]
        public void to_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertToStringEquals(Svo.YesNo.ToString(), Svo.YesNo);
            }
        }
    }

    public class Supports_JSON_serialization
    {
        [TestCase("yes", "yes")]
        [TestCase("yes", true)]
        [TestCase("no", false)]
        [TestCase("yes", 1L)]
        [TestCase("yes", 1.1)]
        [TestCase("no", 0.0)]
        [TestCase("?", (long)byte.MaxValue)]
        [TestCase("?", (long)short.MaxValue)]
        [TestCase("?", (long)int.MaxValue)]
        [TestCase("?", long.MaxValue)]
        [TestCase("?", "unknown")]
        public void convention_based_deserialization(YesNo expected, object json)
        {
            var actual = JsonTester.Read<YesNo>(json);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(null, "")]
        [TestCase("yes", "yes")]
        public void convention_based_serialization(object expected, YesNo svo)
        {
            var serialized = JsonTester.Write(svo);
            Assert.AreEqual(expected, serialized);
        }

        [TestCase("Invalid input", typeof(FormatException))]
        [TestCase("2017-06-11", typeof(FormatException))]
        [TestCase(5L, typeof(InvalidCastException))]
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

    public class Debugger
    {
        [TestCase("{empty}", "")]
        [TestCase("{unknown}", "?")]
        [TestCase("yes", "Y")]
        public void has_custom_display(object display, YesNo svo)
        {
            DebuggerDisplayAssert.HasResult(display, svo);
        }
    }
}
