using FluentAssertions;
using NUnit.Framework;
using Qowaiv;
using Qowaiv.Globalization;
using Qowaiv.Json;
using Qowaiv.Specs;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace Email_address_specs
{
    public class With_domain_logic
    {
        [TestCase("")]
        [TestCase("?")]
        public void has_length_zero_for_empty_and_unknown(EmailAddress svo)
        {
            Assert.AreEqual(0, svo.Length);
        }

        [TestCase(15, "info@qowaiv.org")]
        public void has_length(int length, EmailAddress svo)
        {
            Assert.AreEqual(length, svo.Length);
        }

        [TestCase(false, "info@qowaiv.org")]
        [TestCase(false, "?")]
        [TestCase(true, "")]
        public void IsEmpty_returns(bool result, EmailAddress svo)
        {
            Assert.AreEqual(result, svo.IsEmpty());
        }

        [TestCase(false, "info@qowaiv.org")]
        [TestCase(true, "?")]
        [TestCase(true, "")]
        public void IsEmptyOrUnknown_returns(bool result, EmailAddress svo)
        {
            Assert.AreEqual(result, svo.IsEmptyOrUnknown());
        }

        [TestCase(false, "info@qowaiv.org")]
        [TestCase(true, "?")]
        [TestCase(false, "")]
        public void IsUnknown_returns(bool result, EmailAddress svo)
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
            Assert.IsTrue(EmailAddress.IsValid(input));
        }

        [TestCase("info@qowaiv.org", "nl")]
        [TestCase("info@qowaiv.org", "nl")]
        public void strings_representing_SVO(string input, CultureInfo culture)
        {
            Assert.IsTrue(EmailAddress.IsValid(input, culture));
        }
    }

    public class Is_not_valid_for
    {
        [Test]
        public void string_empty()
        {
            Assert.IsFalse(EmailAddress.IsValid(string.Empty));
        }

        [Test]
        public void string_null()
        {
            Assert.IsFalse(EmailAddress.IsValid(null));
        }

        [Test]
        public void whitespace()
        {
            Assert.IsFalse(EmailAddress.IsValid(" "));
        }

        [Test]
        public void garbage()
        {
            Assert.IsFalse(EmailAddress.IsValid("garbage"));
        }
    }

    public class Has_constant
    {
        [Test]
        public void Empty_represent_default_value()
        {
            Assert.AreEqual(default(EmailAddress), EmailAddress.Empty);
        }
    }

    public class Is_equal_by_value
    {
        [Test]
        public void not_equal_to_null()
        {
            Assert.IsFalse(Svo.EmailAddress.Equals(null));
        }

        [Test]
        public void not_equal_to_other_type()
        {
            Assert.IsFalse(Svo.EmailAddress.Equals(new object()));
        }

        [Test]
        public void not_equal_to_different_value()
        {
            Assert.IsFalse(Svo.EmailAddress.Equals(EmailAddress.Parse("no_spam@qowaiv.org")));
        }

        [Test]
        public void equal_to_same_value()
        {
            Assert.IsTrue(Svo.EmailAddress.Equals(EmailAddress.Parse("info@qowaiv.org")));
        }

        [Test]
        public void equal_operator_returns_true_for_same_values()
        {
            Assert.IsTrue(Svo.EmailAddress == EmailAddress.Parse("info@qowaiv.org"));
        }

        [Test]
        public void equal_operator_returns_false_for_different_values()
        {
            Assert.IsFalse(Svo.EmailAddress == EmailAddress.Parse("no_spam@qowaiv.org"));
        }

        [Test]
        public void not_equal_operator_returns_false_for_same_values()
        {
            Assert.IsFalse(Svo.EmailAddress != EmailAddress.Parse("info@qowaiv.org"));
        }

        [Test]
        public void not_equal_operator_returns_true_for_different_values()
        {
            Assert.IsTrue(Svo.EmailAddress != EmailAddress.Parse("no_spam@qowaiv.org"));
        }

        [Test]
        public void hash_code_is_zero_for_empty()
        {
            Assert.That(EmailAddress.Empty.GetHashCode(), Is.EqualTo(0));
        }

        [Test]
        public void hash_code_is_not_zero_and_reproducable_for_not_empty()
        {
            var hash = Svo.EmailAddress.GetHashCode();
            Assert.That(hash, Is.Not.EqualTo(0));
            Assert.That(Svo.EmailAddress.GetHashCode(), Is.EqualTo(hash));
        }
    }

    public class Can_be_parsed
    {
        [Test]
        public void from_null_string_represents_Empty()
        {
            Assert.AreEqual(EmailAddress.Empty, EmailAddress.Parse(null));
        }

        [Test]
        public void from_empty_string_represents_Empty()
        {
            Assert.AreEqual(EmailAddress.Empty, EmailAddress.Parse(string.Empty));
        }

        [Test]
        public void from_question_mark_represents_Unknown()
        {
            Assert.AreEqual(EmailAddress.Unknown, EmailAddress.Parse("?"));
        }

        [TestCase("en", "info@qowaiv.org")]
        public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
        {
            using (culture.Scoped())
            {
                var parsed = EmailAddress.Parse(input);
                Assert.AreEqual(Svo.EmailAddress, parsed);
            }
        }

        [Test]
        public void from_valid_input_only_otherwise_throws_on_Parse()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var exception = Assert.Throws<FormatException>(() => EmailAddress.Parse("invalid input"));
                Assert.AreEqual("Not a valid email address", exception.Message);
            }
        }

        [Test]
        public void from_valid_input_only_otherwise_return_false_on_TryParse()
        {
            Assert.IsFalse(EmailAddress.TryParse("invalid input", out _));
        }

        [Test]
        public void from_invalid_as_null_with_TryParse()
            => EmailAddress.TryParse("invalid input").Should().BeNull();

        [Test]
        public void with_TryParse_returns_SVO()
        {
            Assert.AreEqual(Svo.EmailAddress, EmailAddress.TryParse("info@qowaiv.org"));
        }
    }

    public class Has_custom_formatting
    {
        [Test]
        public void _default()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Assert.AreEqual("info@qowaiv.org", Svo.EmailAddress.ToString());
            }
        }

        [Test]
        public void with_null_pattern_equal_to_default()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Assert.AreEqual(Svo.EmailAddress.ToString(), Svo.EmailAddress.ToString(default(string)));
            }
        }

        [Test]
        public void with_string_empty_pattern_equal_to_default()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Assert.AreEqual(Svo.EmailAddress.ToString(), Svo.EmailAddress.ToString(string.Empty));
            }
        }

        [Test]
        public void default_value_is_represented_as_string_empty()
        {
            Assert.AreEqual(string.Empty, default(EmailAddress).ToString());
        }

        [Test]
        public void unknown_value_is_represented_as_unknown()
        {
            Assert.AreEqual("?", EmailAddress.Unknown.ToString());
        }

        [Test]
        public void custom_format_provider_is_applied()
        {
            var formatted = Svo.EmailAddress.ToString("f", new UnitTestFormatProvider());
            Assert.AreEqual("Unit Test Formatter, value: 'info@qowaiv.org', format: 'f'", formatted);
        }

        [TestCase("en-GB", null, "info@qowaiv.org", "info@qowaiv.org")]
        [TestCase("nl-BE", "f", "info@qowaiv.org", "info@qowaiv.org")]
        public void culture_dependent(CultureInfo culture, string format, EmailAddress svo, string expected)
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
                Assert.AreEqual("info@qowaiv.org", Svo.EmailAddress.ToString(provider: null));
            }
        }

        [TestCase("info@qowaiv.org", null)]
        [TestCase("info@qowaiv.org", "")]
        [TestCase("info@qowaiv.org", "f")]
        [TestCase("INFO@QOWAIV.ORG", "U")]
        [TestCase("INFO", "L")]
        [TestCase("info", "l")]
        [TestCase("qowaiv.org", "d")]
        [TestCase("QOWAIV.ORG", "D")]
        [TestCase("info@qowaiv.org", "l@d")]
        public void with_format(string formatted, string format)
        {
            Assert.That(Svo.EmailAddress.ToString(format), Is.EqualTo(formatted));
        }
    }

    public class Is_comparable
    {
        [Test]
        public void to_null()
        {
            Assert.AreEqual(1, Svo.EmailAddress.CompareTo(null));
        }

        [Test]
        public void to_EmailAddress_as_object()
        {
            object obj = Svo.EmailAddress;
            Assert.AreEqual(0, Svo.EmailAddress.CompareTo(obj));
        }

        [Test]
        public void to_EmailAddress_only()
        {
            Assert.Throws<ArgumentException>(() => Svo.EmailAddress.CompareTo(new object()));
        }

        [Test]
        public void can_be_sorted_using_compare()
        {
            var sorted = new[]
            { 
                default,
                default,
                EmailAddress.Unknown,
                EmailAddress.Parse("info@qowaiv.com"), 
                EmailAddress.Parse("info@qowaiv.org"), 
                EmailAddress.Parse("spam@qowaiv.org"),
            };
            var list = new List<EmailAddress> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
            list.Sort();
            Assert.AreEqual(sorted, list);
        }
    }

    public class Supports_type_conversion
    {
        [Test]
        public void via_TypeConverter_registered_with_attribute()
            => typeof(EmailAddress).Should().HaveTypeConverterDefined();

        [Test]
        public void from_null_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(default(EmailAddress), null);
            }
        }

        [Test]
        public void from_empty_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(default(EmailAddress), string.Empty);
            }
        }

        [Test]
        public void from_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(Svo.EmailAddress, Svo.EmailAddress.ToString());
            }
        }

        [Test]
        public void to_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertToStringEquals(Svo.EmailAddress.ToString(), Svo.EmailAddress);
            }
        }
    }

    public class Supports_JSON_serialization
    {
        [TestCase("?", "unknown")]
        public void convention_based_deserialization(EmailAddress expected, object json)
        {
            var actual = JsonTester.Read<EmailAddress>(json);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(null, "")]
        [TestCase("info@qowaiv.org", "info@qowaiv.org")]
        public void convention_based_serialization(object expected, EmailAddress svo)
        {
            var serialized = JsonTester.Write(svo);
            Assert.AreEqual(expected, serialized);
        }

        [TestCase("Invalid input", typeof(FormatException))]
        [TestCase("2017-06-11", typeof(FormatException))]
        public void throws_for_invalid_json(object json, Type exceptionType)
        {
            var exception = Assert.Catch(() => JsonTester.Read<EmailAddress>(json));
            Assert.IsInstanceOf(exceptionType, exception);
        }
    }

    public class Supports_XML_serialization
    {
        [Test]
        public void using_XmlSerializer_to_serialize()
        {
            var xml = Serialize.Xml(Svo.EmailAddress);
            Assert.AreEqual("info@qowaiv.org", xml);
        }

        [Test]
        public void using_XmlSerializer_to_deserialize()
        {
            var svo =Deserialize.Xml<EmailAddress>("info@qowaiv.org");
            Assert.AreEqual(Svo.EmailAddress, svo);
        }

        [Test]
        public void using_DataContractSerializer()
        {
            var round_tripped = SerializeDeserialize.DataContract(Svo.EmailAddress);
            Assert.AreEqual(Svo.EmailAddress, round_tripped);
        }

        [Test]
        public void as_part_of_a_structure()
        {
            var structure = XmlStructure.New(Svo.EmailAddress);
            var round_tripped = SerializeDeserialize.Xml(structure);
            Assert.AreEqual(structure, round_tripped);
        }

        [Test]
        public void has_no_custom_XML_schema()
        {
            IXmlSerializable obj = Svo.EmailAddress;
            Assert.IsNull(obj.GetSchema());
        }
    }

    public class Is_Open_API_data_type
    {
        internal static readonly OpenApiDataTypeAttribute Attribute = OpenApiDataTypeAttribute.From(typeof(EmailAddress)).FirstOrDefault();

        [Test]
        public void with_description() => Attribute.Description.Should().Be("Email notation as defined by RFC 5322.");

        [Test]
        public void with_example() => Attribute.Example.Should().Be("svo@qowaiv.org");

        [Test]
        public void has_type() => Attribute.Type.Should().Be("string");

        [Test]
        public void has_format() => Attribute.Format.Should().Be("email");

        [Test]
        public void pattern_is_null()=> Attribute.Pattern.Should().BeNull();
    }

    public class Supports_binary_serialization
    {
        [Test]
        public void using_BinaryFormatter()
        {
            var round_tripped = SerializeDeserialize.Binary(Svo.EmailAddress);
            Assert.AreEqual(Svo.EmailAddress, round_tripped);
        }

        [Test]
        public void storing_string_in_SerializationInfo()
        {
            var info = Serialize.GetInfo(Svo.EmailAddress);
            Assert.AreEqual("info@qowaiv.org", info.GetString("Value"));
        }
    }

    public class Debugger
    {
        [TestCase("{empty}", "")]
        [TestCase("{unknown}", "?")]
        [TestCase("info@qowaiv.org", "info@qowaiv.org")]
        public void has_custom_display(object display, EmailAddress svo)
            => svo.Should().HaveDebuggerDisplay(display);
    }
}

