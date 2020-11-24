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

namespace Postal_code_specs
{
    public class With_domain_logic
    {
        [TestCase("")]
        [TestCase("?")]
        public void has_length_zero_for_empty_and_unknown(PostalCode svo)
        {
            Assert.AreEqual(0, svo.Length);
        }

        [TestCase(6, "H0H0H0")]
        public void has_length(int length, PostalCode svo)
        {
            Assert.AreEqual(length, svo.Length);
        }

        [TestCase(false, "H0H0H0")]
        [TestCase(false, "?")]
        [TestCase(true, "")]
        public void IsEmpty_returns(bool result, PostalCode svo)
        {
            Assert.AreEqual(result, svo.IsEmpty());
        }

        [TestCase(false, "H0H0H0")]
        [TestCase(true, "?")]
        [TestCase(true, "")]
        public void IsEmptyOrUnknown_returns(bool result, PostalCode svo)
        {
            Assert.AreEqual(result, svo.IsEmptyOrUnknown());
        }

        [TestCase(false, "H0H0H0")]
        [TestCase(true, "?")]
        [TestCase(false, "")]
        public void IsUnknown_returns(bool result, PostalCode svo)
        {
            Assert.AreEqual(result, svo.IsUnknown());
        }
    }

    public class Is_valid_for
    {
        [TestCase("?")]
        [TestCase("unknown")]
        public void strings_representing_unknown(string input)
        {
            Assert.IsTrue(PostalCode.IsValid(input));
        }

        [TestCase("H0H0H0", "nl")]
        [TestCase("H0H0H0", "nl")]
        public void strings_representing_SVO(string input, CultureInfo culture)
        {
            Assert.IsTrue(PostalCode.IsValid(input, culture));
        }
    }

    public class Is_not_valid_for
    {
        [Test]
        public void string_empty()
        {
            Assert.IsFalse(PostalCode.IsValid(string.Empty));
        }

        [Test]
        public void string_null()
        {
            Assert.IsFalse(PostalCode.IsValid(null));
        }

        [Test]
        public void whitespace()
        {
            Assert.IsFalse(PostalCode.IsValid(" "));
        }

        [Test]
        public void garbage()
        {
            Assert.IsFalse(PostalCode.IsValid("01234567890"));
        }
    }

    public class Has_constant
    {
        [Test]
        public void Empty_represent_default_value()
        {
            Assert.AreEqual(default(PostalCode), PostalCode.Empty);
        }
    }

    public class Is_equal_by_value
    {
        [Test]
        public void not_equal_to_null()
        {
            Assert.IsFalse(Svo.PostalCode.Equals(null));
        }

        [Test]
        public void not_equal_to_other_type()
        {
            Assert.IsFalse(Svo.PostalCode.Equals(new object()));
        }

        [Test]
        public void not_equal_to_different_value()
        {
            Assert.IsFalse(Svo.PostalCode.Equals(PostalCode.Parse("different")));
        }

        [Test]
        public void equal_to_same_value()
        {
            Assert.IsTrue(Svo.PostalCode.Equals(PostalCode.Parse("H0H0H0")));
        }

        [Test]
        public void equal_operator_returns_true_for_same_values()
        {
            Assert.IsTrue(Svo.PostalCode == PostalCode.Parse("H0H0H0"));
        }

        [Test]
        public void equal_operator_returns_false_for_different_values()
        {
            Assert.IsFalse(Svo.PostalCode == PostalCode.Parse("different"));
        }

        [Test]
        public void not_equal_operator_returns_false_for_same_values()
        {
            Assert.IsFalse(Svo.PostalCode != PostalCode.Parse("H0H0H0"));
        }

        [Test]
        public void not_equal_operator_returns_true_for_different_values()
        {
            Assert.IsTrue(Svo.PostalCode != PostalCode.Parse("different"));
        }

        [TestCase("")]
        [TestCase("H0H0H0")]
        public void hash_code_is_value_based(string svo)
        {
            var first = PostalCode.Parse(svo);
            var second = PostalCode.Parse(svo);
            Assert.AreEqual(second.GetHashCode(), first.GetHashCode());
        }
    }

    public class Can_be_parsed
    {
        [Test]
        public void from_null_string_represents_Empty()
        {
            Assert.AreEqual(PostalCode.Empty, PostalCode.Parse(null));
        }

        [Test]
        public void from_empty_string_represents_Empty()
        {
            Assert.AreEqual(PostalCode.Empty, PostalCode.Parse(string.Empty));
        }

        [Test]
        public void from_question_mark_represents_Unknown()
        {
            Assert.AreEqual(PostalCode.Unknown, PostalCode.Parse("?"));
        }

        [TestCase("en", "H0H0H0")]
        public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
        {
            using (culture.Scoped())
            {
                var parsed = PostalCode.Parse(input);
                Assert.AreEqual(Svo.PostalCode, parsed);
            }
        }

        [Test]
        public void from_valid_input_only_otherwise_throws_on_Parse()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var exception = Assert.Throws<FormatException>(() => PostalCode.Parse("invalid input"));
                Assert.AreEqual("Not a valid postal code", exception.Message);
            }
        }

        [Test]
        public void from_valid_input_only_otherwise_return_false_on_TryParse()
        {
            Assert.IsFalse(PostalCode.TryParse("invalid input", out _));
        }

        [Test]
        public void from_invalid_as_empty_with_TryParse()
        {
            Assert.AreEqual(default(PostalCode), PostalCode.TryParse("invalid input"));
        }

        [Test]
        public void with_TryParse_returns_SVO()
        {
            Assert.AreEqual(Svo.PostalCode, PostalCode.TryParse("H0H0H0"));
        }
    }

    public class Has_custom_formatting
    {
        [Test]
        public void default_value_is_represented_as_string_empty()
        {
            Assert.AreEqual(string.Empty, default(PostalCode).ToString());
        }

        [Test]
        public void unknown_value_is_represented_as_unknown()
        {
            Assert.AreEqual("?", PostalCode.Unknown.ToString());
        }

        [Test]
        public void custom_format_provider_is_applied()
        {
            var formatted = Svo.PostalCode.ToString("SomeFormat", new UnitTestFormatProvider());
            Assert.AreEqual("Unit Test Formatter, value: 'H0H0H0', format: 'SomeFormat'", formatted);
        }

        [TestCase("en-GB", null, "H0H0H0", "H0H0H0")]
        [TestCase("nl-BE", "NL", "2624DP", "2624 DP")]
        [TestCase("es-ES", "AD", "765", "AD-765")]
        public void culture_dependent(CultureInfo culture, string format, PostalCode svo, string expected)
        {
            using (culture.Scoped())
            {
                Assert.AreEqual(expected, svo.ToString(format));
            }
        }

        [Test]
        public void with_current_thread_culture_as_default()
        {
            using (new CultureInfoScope(culture: TestCultures.Nl_NL, cultureUI: TestCultures.En_GB))
            {
                Assert.AreEqual("H0H0H0", Svo.PostalCode.ToString(provider: null));
            }
        }
    }

    public class Is_comparable
    {
        [Test]
        public void to_null()
        {
            Assert.AreEqual(1, Svo.PostalCode.CompareTo(null));
        }

        [Test]
        public void to_PostalCode_as_object()
        {
            object obj = Svo.PostalCode;
            Assert.AreEqual(0, Svo.PostalCode.CompareTo(obj));
        }

        [Test]
        public void to_PostalCode_only()
        {
            Assert.Throws<ArgumentException>(() => Svo.PostalCode.CompareTo(new object()));
        }

        [Test]
        public void can_be_sorted_using_compare()
        {
            var sorted = new [] 
            {
                default, 
                default, 
                PostalCode.Parse("12345"),
                PostalCode.Parse("78900"),
                PostalCode.Parse("8904"),
                PostalCode.Unknown, 
            };
            var list = new List<PostalCode> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
            list.Sort();
            Assert.AreEqual(sorted, list);
        }
    }

    public class Casts
    {
        [Test]
        public void explicitly_from_string()
        {
            var casted = (PostalCode)"H0H0H0";
            Assert.AreEqual(Svo.PostalCode, casted);
        }

        [Test]
        public void explicitly_to_string()
        {
            var casted = (string)Svo.PostalCode;
            Assert.AreEqual("H0H0H0", casted);
        }
    }

    public class Supports_type_conversion
    {
        [Test]
        public void via_TypeConverter_registered_with_attribute()
        {
            TypeConverterAssert.ConverterExists(typeof(PostalCode));
        }

        [Test]
        public void from_null_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(default(PostalCode), null);
            }
        }

        [Test]
        public void from_empty_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(default(PostalCode), string.Empty);
            }
        }

        [Test]
        public void from_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(Svo.PostalCode, Svo.PostalCode.ToString());
            }
        }

        [Test]
        public void to_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertToStringEquals(Svo.PostalCode.ToString(), Svo.PostalCode);
            }
        }
    }

    public class Supports_JSON_serialization
    {
        [TestCase("?", "unknown")]
        [TestCase("1234", "1234")]
        public void convention_based_deserialization(PostalCode expected, object json)
        {
            var actual = JsonTester.Read<PostalCode>(json);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(null, "")]
        [TestCase("1234", "1234")]
        public void convention_based_serialization(object expected, PostalCode svo)
        {
            var serialized = JsonTester.Write(svo);
            Assert.AreEqual(expected, serialized);
        }

        [TestCase("01234567890", typeof(FormatException))]
        public void throws_for_invalid_json(object json, Type exceptionType)
        {
            var exception = Assert.Catch(() => JsonTester.Read<PostalCode>(json));
            Assert.IsInstanceOf(exceptionType, exception);
        }
    }

    public class Supports_XML_serialization
    {
        [Test]
        public void using_XmlSerializer_to_serialize()
        {
            var xml = SerializationTest.XmlSerialize(Svo.PostalCode);
            Assert.AreEqual("H0H0H0", xml);
        }

        [Test]
        public void using_XmlSerializer_to_deserialize()
        {
            var svo = SerializationTest.XmlDeserialize<PostalCode>("H0H0H0");
            Assert.AreEqual(Svo.PostalCode, svo);
        }

        [Test]
        public void using_DataContractSerializer()
        {
            var round_tripped = SerializationTest.DataContractSerializeDeserialize(Svo.PostalCode);
            Assert.AreEqual(Svo.PostalCode, round_tripped);
        }

        [Test]
        public void as_part_of_a_structure()
        {
            var structure = XmlStructure.New(Svo.PostalCode);
            var round_tripped = SerializationTest.XmlSerializeDeserialize(structure);
            Assert.AreEqual(structure, round_tripped);
        }

        [Test]
        public void has_no_custom_XML_schema()
        {
            IXmlSerializable obj = Svo.PostalCode;
            Assert.IsNull(obj.GetSchema());
        }
    }

    public class Supports_binary_serialization
    {
        [Test]
        public void using_BinaryFormatter()
        {
            var round_tripped = SerializationTest.BinaryFormatterSerializeDeserialize(Svo.PostalCode);
            Assert.AreEqual(Svo.PostalCode, round_tripped);
        }

        [Test]
        public void storing_string_in_SerializationInfo()
        {
            var info = SerializationTest.GetSerializationInfo(Svo.PostalCode);
            Assert.AreEqual("H0H0H0", info.GetString("Value"));
        }
    }

    public class Debugger
    {
        [TestCase("{empty}", "")]
        [TestCase("{unknown}", "?")]
        [TestCase("H0H0H0", "H0H0H0")]
        public void has_custom_display(object display, PostalCode svo)
        {
            DebuggerDisplayAssert.HasResult(display, svo);
        }
    }
}

