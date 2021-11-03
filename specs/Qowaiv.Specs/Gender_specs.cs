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

namespace Gender_specs
{
    public class With_domain_logic
    {
        [TestCase(false, "Male")]
        [TestCase(false, "Female")]
        [TestCase(false, "?")]
        [TestCase(true, "")]
        public void IsEmpty_returns(bool result, Gender svo)
        {
            Assert.AreEqual(result, svo.IsEmpty());
        }

        [TestCase(false, "Male")]
        [TestCase(false, "Female")]
        [TestCase(true, "?")]
        [TestCase(true, "")]
        public void IsEmptyOrUnknown_returns(bool result, Gender svo)
        {
            Assert.AreEqual(result, svo.IsEmptyOrUnknown());
        }

        [TestCase(false, "Male")]
        [TestCase(false, "Female")]
        [TestCase(true, "?")]
        [TestCase(false, "")]
        public void IsUnknown_returns(bool result, Gender svo)
        {
            Assert.AreEqual(result, svo.IsUnknown());
        }

        [TestCase(true, "Male")]
        [TestCase(true, "Female")]
        [TestCase(false, "?")]
        [TestCase(false, "")]
        public void IsMaleOrFemale_returns(bool result, Gender svo)
        {
            Assert.AreEqual(result, svo.IsMaleOrFemale());
        }
    }

    public class Display_name
    {
        [Test]
        public void for_current_culture_by_default()
        {
            using (TestCultures.Nl_BE.Scoped())
            {
                Assert.AreEqual("Vrouwelijk", Svo.Gender.DisplayName);
            }
        }

        [Test]
        public void for_custom_culture_if_specified()
        {
            Assert.AreEqual("Mujer", Svo.Gender.GetDisplayName(TestCultures.Es_EC));
        }
    }

    public class Is_valid_for
    {
        [TestCase("?")]
        [TestCase("unknown")]
        public void strings_representing_unknown(string input)
        {
            Assert.IsTrue(Gender.IsValid(input));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(9)]
        public void numbers(int? number)
        {
            Assert.IsTrue(Gender.IsValid(number));
        }

        [TestCase("Female", "nl")]
        [TestCase("Female", "nl")]
        public void strings_representing_SVO(string input, CultureInfo culture)
        {
            Assert.IsTrue(Gender.IsValid(input, culture));
        }
    }

    public class Is_not_valid_for
    {
        [Test]
        public void string_empty()
        {
            Assert.IsFalse(Gender.IsValid(string.Empty));
        }

        [Test]
        public void string_null()
        {
            Assert.IsFalse(Gender.IsValid((string)null));
        }

        [Test]
        public void whitespace()
        {
            Assert.IsFalse(Gender.IsValid(" "));
        }

        [Test]
        public void garbage()
        {
            Assert.IsFalse(Gender.IsValid("garbage"));
        }

        [TestCase(null)]
        [TestCase(0)]
        [TestCase(3)]
        [TestCase(10)]
        public void numbers(int? number)
        {
            Assert.IsFalse(Gender.IsValid(number));
        }
    }

    public class Has_constant
    {
        [Test]
        public void Empty_represent_default_value()
        {
            Assert.AreEqual(default(Gender), Gender.Empty);
        }
    }

    public class Is_equal_by_value
    {
        [Test]
        public void not_equal_to_null()
        {
            Assert.IsFalse(Svo.Gender.Equals(null));
        }

        [Test]
        public void not_equal_to_other_type()
        {
            Assert.IsFalse(Svo.Gender.Equals(new object()));
        }

        [Test]
        public void not_equal_to_different_value()
        {
            Assert.IsFalse(Svo.Gender.Equals(Gender.Male));
        }

        [Test]
        public void equal_to_same_value()
        {
            Assert.IsTrue(Svo.Gender.Equals(Gender.Female));
        }

        [Test]
        public void equal_operator_returns_true_for_same_values()
        {
            Assert.IsTrue(Svo.Gender == Gender.Female);
        }

        [Test]
        public void equal_operator_returns_false_for_different_values()
        {
            Assert.IsFalse(Svo.Gender == Gender.Male);
        }

        [Test]
        public void not_equal_operator_returns_false_for_same_values()
        {
            Assert.IsFalse(Svo.Gender != Gender.Female);
        }

        [Test]
        public void not_equal_operator_returns_true_for_different_values()
        {
            Assert.IsTrue(Svo.Gender != Gender.Male);
        }

        [TestCase("", 0)]
        [TestCase("Male", 2)]
        [TestCase("Female", 4)]
        public void hash_code_is_value_based(Gender svo, int hashcode)
        {
            Assert.AreEqual(hashcode, svo.GetHashCode());
        }
    }

    public class Can_be_parsed
    {
        [Test]
        public void from_null_string_represents_Empty()
        {
            Assert.AreEqual(Gender.Empty, Gender.Parse(null));
        }

        [Test]
        public void from_empty_string_represents_Empty()
        {
            Assert.AreEqual(Gender.Empty, Gender.Parse(string.Empty));
        }

        [Test]
        public void from_question_mark_represents_Unknown()
        {
            Assert.AreEqual(Gender.Unknown, Gender.Parse("?"));
        }

        [TestCase("en", "Female")]
        public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
        {
            using (culture.Scoped())
            {
                var parsed = Gender.Parse(input);
                Assert.AreEqual(Svo.Gender, parsed);
            }
        }

        [Test]
        public void from_valid_input_only_otherwise_throws_on_Parse()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var exception = Assert.Throws<FormatException>(() => Gender.Parse("invalid input"));
                Assert.AreEqual("Not a valid gender", exception.Message);
            }
        }

        [Test]
        public void from_valid_input_only_otherwise_return_false_on_TryParse()
        {
            Assert.IsFalse(Gender.TryParse("invalid input", out _));
        }

        [Test]
        public void from_invalid_as_empty_with_TryParse()
        {
            Assert.AreEqual(default(Gender), Gender.TryParse("invalid input"));
        }

        [Test]
        public void with_TryParse_returns_SVO()
        {
            Assert.AreEqual(Svo.Gender, Gender.TryParse("Female"));
        }
    }

    public class Has_custom_formatting
    {
        [Test]
        public void _default()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Assert.AreEqual("Female", Svo.Gender.ToString());
            }
        }

        [Test]
        public void with_null_pattern_equal_to_default()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Assert.AreEqual(Svo.Gender.ToString(), Svo.Gender.ToString(default(string)));
            }
        }

        [Test]
        public void with_string_empty_pattern_equal_to_default()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Assert.AreEqual(Svo.Gender.ToString(), Svo.Gender.ToString(string.Empty));
            }
        }

        [Test]
        public void default_value_is_represented_as_string_empty()
        {
            Assert.AreEqual(string.Empty, default(Gender).ToString());
        }

        [Test]
        public void custom_format_provider_is_applied()
        {
            var formatted = Svo.Gender.ToString("s", new UnitTestFormatProvider());
            Assert.AreEqual("Unit Test Formatter, value: '♀', format: 's'", formatted);
        }

        [TestCase("en-GB", null, "Female", "Female")]
        [TestCase("en-GB", "c", "Female", "F")]
        [TestCase("ru-RU", "s", "Female", "♀")]
        [TestCase("nl-BE", "i", "Female", "2")]
        [TestCase("nl-BE", "h", "Female", "Mevr.")]
        [TestCase("nl-BE", "f", "Female", "Vrouwelijk")]
        public void culture_dependent(CultureInfo culture, string format, Gender svo, string expected)
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
                Assert.AreEqual("Vrouwelijk", Svo.Gender.ToString(provider: null));
            }
        }
    }

    public class Is_comparable
    {
        [Test]
        public void to_null_is_1()
        {
            object obj = null;
            Assert.AreEqual(1, Svo.Gender.CompareTo(obj));
        }

        [Test]
        public void to_Gender_as_object()
        {
            object obj = Svo.Gender;
            Assert.AreEqual(0, Svo.Gender.CompareTo(obj));
        }

        [Test]
        public void to_Gender_only()
        {
            Assert.Throws<ArgumentException>(() => Svo.Gender.CompareTo(new object()));
        }

        [Test]
        public void can_be_sorted_using_compare()
        {
            var sorted = new[]
            {
                default,
                default,
                Gender.Unknown,
                Gender.Male,
                Gender.Female,
            };
            var list = new List<Gender> { sorted[3], sorted[4], sorted[2], sorted[0], sorted[1] };
            list.Sort();
            Assert.AreEqual(sorted, list);
        }
    }

    public class Casts
    {
        [Test]
        public void explicitly_from_string()
        {
            var casted = (Gender)"Female";
            Assert.AreEqual(Svo.Gender, casted);
        }

        [Test]
        public void explicitly_to_string()
        {
            var casted = (string)Svo.Gender;
            Assert.AreEqual("Female", casted);
        }

        [Test]
        public void explicitly_to_byte()
        {
            var casted = (byte)Svo.Gender;
            Assert.AreEqual((byte)2, casted);
        }

        [Test]
        public void explicitly_to_int()
        {
            var casted = (int)Svo.Gender;
            Assert.AreEqual(2, casted);
        }

        [TestCase(2, "Female")]
        [TestCase(null, "?")]
        public void explicitly_to_nullable_int(int casted, Gender gender)
        {
            Assert.AreEqual(casted, (int?)gender);
        }

        [TestCase("Female", 2)]
        [TestCase("", null)]
        public void implictly_from_nullable_int(Gender casted, int? value)
        {
            Gender gender = value;
            Assert.AreEqual(casted, gender);
        }

        [TestCase("Female", 2)]
        [TestCase("?", 0)]
        public void implictly_from_int(Gender casted, int value)
        {
            Gender gender = value;
            Assert.AreEqual(casted, gender);
        }
    }

    public class Supports_type_conversion
    {
        [Test]
        public void via_TypeConverter_registered_with_attribute()
            => typeof(Gender).Should().HaveTypeConverterDefined();

        [Test]
        public void from_null_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(default(Gender), null);
            }
        }

        [Test]
        public void from_empty_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(default(Gender), string.Empty);
            }
        }

        [Test]
        public void from_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(Svo.Gender, Svo.Gender.ToString());
            }
        }

        [Test]
        public void to_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertToStringEquals(Svo.Gender.ToString(), Svo.Gender);
            }
        }
    }

    public class Supports_JSON_serialization
    {
        [TestCase("?", "unknown")]
        [TestCase("Female", 2L)]
        [TestCase("Female", 2d)]
        public void convention_based_deserialization(Gender expected, object json)
        {
            var actual = JsonTester.Read<Gender>(json);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(null, "")]

        public void convention_based_serialization(object expected, Gender svo)
        {
            var serialized = JsonTester.Write(svo);
            Assert.AreEqual(expected, serialized);
        }

        [TestCase("Invalid input", typeof(FormatException))]
        [TestCase("2017-06-11", typeof(FormatException))]
        [TestCase(5L, typeof(ArgumentOutOfRangeException))]
        [TestCase(long.MaxValue, typeof(ArgumentOutOfRangeException))]
        public void throws_for_invalid_json(object json, Type exceptionType)
        {
            var exception = Assert.Catch(() => JsonTester.Read<Gender>(json));
            Assert.IsInstanceOf(exceptionType, exception);
        }
    }

    public class Supports_XML_serialization
    {

        [TestCase("", "")]
        [TestCase("Female", "F")]
        public void using_XmlSerializer_to_serialize(string xml, Gender gender)
        {
            Assert.AreEqual(xml, Serialize.Xml(gender));
        }

        [Test]
        public void using_XmlSerializer_to_deserialize()
        {
            var svo =Deserialize.Xml<Gender>("Female");
            Assert.AreEqual(Svo.Gender, svo);
        }

        [Test]
        public void using_DataContractSerializer()
        {
            var round_tripped = SerializeDeserialize.DataContract(Svo.Gender);
            Assert.AreEqual(Svo.Gender, round_tripped);
        }

        [Test]
        public void as_part_of_a_structure()
        {
            var structure = XmlStructure.New(Svo.Gender);
            var round_tripped = SerializeDeserialize.Xml(structure);
            Assert.AreEqual(structure, round_tripped);
        }

        [Test]
        public void has_no_custom_XML_schema()
        {
            IXmlSerializable obj = Svo.Gender;
            Assert.IsNull(obj.GetSchema());
        }
    }

    public class Is_Open_API_data_type
    {
        internal static readonly OpenApiDataTypeAttribute Attribute = OpenApiDataTypeAttribute.From(typeof(Gender)).FirstOrDefault();
        [Test]
        public void with_description()
        {
            Assert.AreEqual("Gender as specified by ISO/IEC 5218.", Attribute.Description);
        }

        [Test]
        public void has_type()
        {
            Assert.AreEqual("string", Attribute.Type);
        }

        [Test]
        public void has_format()
        {
            Assert.AreEqual("gender", Attribute.Format);
        }

        [Test]
        public void has_enum()
        {
            Assert.AreEqual(new[] { "NotKnown", "Male", "Female", "NotApplicable" }, Attribute.Enum);
        }

        [Test]
        public void pattern_is_null()
        {
            Assert.IsNull(Attribute.Pattern);
        }
    }

    public class Supports_binary_serialization
    {
        [Test]
        public void using_BinaryFormatter()
        {
            var round_tripped = SerializeDeserialize.Binary(Svo.Gender);
            Assert.AreEqual(Svo.Gender, round_tripped);
        }

        [Test]
        public void storing_byte_in_SerializationInfo()
        {
            var info = Serialize.GetInfo(Svo.Gender);
            Assert.AreEqual((byte)4, info.GetByte("Value"));
        }
    }

    public class Debugger
    {
        [TestCase("{empty}", "")]
        [TestCase("Not known", "?")]
        [TestCase("Female", "Female")]
        public void has_custom_display(object display, Gender svo)
            => svo.Should().HaveDebuggerDisplay(display);
    }
}

