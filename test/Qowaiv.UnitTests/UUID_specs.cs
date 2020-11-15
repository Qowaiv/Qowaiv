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

namespace UUID_specs
{
    public class With_domain_logic
    {
        [TestCase(false, "Qowaiv_SVOLibrary_GUIA")]
        [TestCase(true, "")]
        public void IsEmpty_returns(bool result, Uuid svo)
        {
            Assert.AreEqual(result, svo.IsEmpty());
        }
    }

    public class Is_valid_for
    {
        [TestCase("{8A1A8C42-D2FF-E254-E26E-B6ABCBF19420}")]
        [TestCase("8a1a8c42-d2ff-e254-e26e-b6abcbf19420")]
        [TestCase("Qowaiv_SVOLibrary_GUIA")]
        public void strings_representing_SVO(string input)
        {
            Assert.IsTrue(Uuid.IsValid(input));
        }
    }

    public class Is_not_valid_for
    {
        [Test]
        public void string_empty()
        {
            Assert.IsFalse(Uuid.IsValid(string.Empty));
        }

        [Test]
        public void string_null()
        {
            Assert.IsFalse(Uuid.IsValid(null));
        }

        [Test]
        public void whitespace()
        {
            Assert.IsFalse(Uuid.IsValid(" "));
        }

        [Test]
        public void garbage()
        {
            Assert.IsFalse(Uuid.IsValid("garbage"));
        }
    }

    public class Has_constant
    {
        [Test]
        public void Empty_represent_default_value()
        {
            Assert.AreEqual(default(Uuid), Uuid.Empty);
        }
    }

    public class Is_equal_by_value
    {
        [Test]
        public void not_equal_to_null()
        {
            Assert.IsFalse(Svo.Uuid.Equals(null));
        }

        [Test]
        public void not_equal_to_other_type()
        {
            Assert.IsFalse(Svo.Uuid.Equals(new object()));
        }

        [Test]
        public void not_equal_to_different_value()
        {
            Assert.IsFalse(Svo.Uuid.Equals(Uuid.Parse("6D775128-6365-4A96-BDE8-0972CE6CB0BC")));
        }

        [Test]
        public void equal_to_same_value()
        {
            Assert.IsTrue(Svo.Uuid.Equals(Uuid.Parse("Qowaiv_SVOLibrary_GUIA")));
        }

        [Test]
        public void equal_operator_returns_true_for_same_values()
        {
            Assert.IsTrue(Svo.Uuid == Uuid.Parse("Qowaiv_SVOLibrary_GUIA"));
        }

        [Test]
        public void equal_operator_returns_false_for_different_values()
        {
            Assert.IsFalse(Svo.Uuid == Uuid.Parse("6D775128-6365-4A96-BDE8-0972CE6CB0BC"));
        }

        [Test]
        public void not_equal_operator_returns_false_for_same_values()
        {
            Assert.IsFalse(Svo.Uuid != Uuid.Parse("Qowaiv_SVOLibrary_GUIA"));
        }

        [Test]
        public void not_equal_operator_returns_true_for_different_values()
        {
            Assert.IsTrue(Svo.Uuid != Uuid.Parse("6D775128-6365-4A96-BDE8-0972CE6CB0BC"));
        }

        [TestCase("", 0)]
        [TestCase("Qowaiv_SVOLibrary_GUIA", -479411820)]
        public void hash_code_is_value_based(Uuid svo, int hashcode)
        {
            Assert.AreEqual(hashcode, svo.GetHashCode());
        }
    }

    public class Can_be_parsed
    {
        [Test]
        public void from_null_string_represents_Empty()
        {
            Assert.AreEqual(Uuid.Empty, Uuid.Parse(null));
        }

        [Test]
        public void from_empty_string_represents_Empty()
        {
            Assert.AreEqual(Uuid.Empty, Uuid.Parse(string.Empty));
        }

        [TestCase("en", "Qowaiv_SVOLibrary_GUIA")]
        [TestCase("en", "8A1A8C42-D2FF-E254-E26E-B6ABCBF19420")]
        public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
        {
            using (culture.Scoped())
            {
                var parsed = Uuid.Parse(input);
                Assert.AreEqual(Svo.Uuid, parsed);
            }
        }

        [Test]
        public void from_valid_input_only_otherwise_throws_on_Parse()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var exception = Assert.Throws<FormatException>(() => Uuid.Parse("invalid input"));
                Assert.AreEqual("Not a valid GUID", exception.Message);
            }
        }

        [Test]
        public void from_valid_input_only_otherwise_return_false_on_TryParse()
        {
            Assert.IsFalse(Uuid.TryParse("invalid input", out _));
        }

        [Test]
        public void from_invalid_as_empty_with_TryParse()
        {
            Assert.AreEqual(default(Uuid), Uuid.TryParse("invalid input"));
        }

        [Test]
        public void with_TryParse_returns_SVO()
        {
            Assert.AreEqual(Svo.Uuid, Uuid.TryParse("Qowaiv_SVOLibrary_GUIA"));
        }
    }

    public class Has_custom_formatting
    {
        [Test]
        public void default_value_is_represented_as_string_empty()
        {
            Assert.AreEqual(string.Empty, default(Uuid).ToString());
        }

        [Test]
        public void custom_format_provider_is_applied()
        {
            var formatted = Svo.Uuid.ToString("B", new UnitTestFormatProvider());
            Assert.AreEqual("Unit Test Formatter, value: '{8A1A8C42-D2FF-E254-E26E-B6ABCBF19420}', format: 'B'", formatted);
        }

        [TestCase("en-GB", null, "Qowaiv_SVOLibrary_GUIA", "Qowaiv_SVOLibrary_GUIA")]
        [TestCase("en-GB", "S", "Qowaiv_SVOLibrary_GUIA", "Qowaiv_SVOLibrary_GUIA")]
        [TestCase("en-GB", "H", "Qowaiv_SVOLibrary_GUIA", "Qowaiv_SVOLibrary_GUIA")]
        [TestCase("en-GB", "N", "Qowaiv_SVOLibrary_GUIA", "Qowaiv_SVOLibrary_GUIA")]
        [TestCase("en-GB", "n", "Qowaiv_SVOLibrary_GUIA", "Qowaiv_SVOLibrary_GUIA")]
        [TestCase("en-GB", "D", "Qowaiv_SVOLibrary_GUIA", "Qowaiv_SVOLibrary_GUIA")]
        [TestCase("en-GB", "d", "Qowaiv_SVOLibrary_GUIA", "Qowaiv_SVOLibrary_GUIA")]
        [TestCase("nl-BE", "B", "Qowaiv_SVOLibrary_GUIA", "{8A1A8C42-D2FF-E254-E26E-B6ABCBF19420}")]
        [TestCase("nl-BE", "b", "Qowaiv_SVOLibrary_GUIA", "{8a1a8c42-d2ff-e254-e26e-b6abcbf19420}")]
        [TestCase("nl-BE", "B", "Qowaiv_SVOLibrary_GUIA", "{8A1A8C42-D2FF-E254-E26E-B6ABCBF19420}")]
        [TestCase("nl-BE", "b", "Qowaiv_SVOLibrary_GUIA", "{8a1a8c42-d2ff-e254-e26e-b6abcbf19420}")]
        [TestCase("nl-BE", "P", "Qowaiv_SVOLibrary_GUIA", "{8a1a8c42-d2ff-e254-e26e-b6abcbf19420}")]
        [TestCase("nl-BE", "p", "Qowaiv_SVOLibrary_GUIA", "{8a1a8c42-d2ff-e254-e26e-b6abcbf19420}")]
        [TestCase("nl-BE", "X", "Qowaiv_SVOLibrary_GUIA", "{8a1a8c42-d2ff-e254-e26e-b6abcbf19420}")]
        public void culture_indpendend(CultureInfo culture, string format, Uuid svo, string expected)
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
                Assert.AreEqual("Qowaiv_SVOLibrary_GUIA", Svo.Uuid.ToString(provider: null));
            }
        }
    }

    public class Is_comparable
    {
        [Test]
        public void to_null()
        {
            Assert.AreEqual(1, Svo.Uuid.CompareTo(null));
        }

        [Test]
        public void to_Uuid_as_object()
        {
            object obj = Svo.Uuid;
            Assert.AreEqual(0, Svo.Uuid.CompareTo(obj));
        }

        [Test]
        public void to_Uuid_only()
        {
            Assert.Throws<ArgumentException>(() => Svo.Uuid.CompareTo(new object()));
        }

        [Test]
        public void can_be_sorted()
        {
            var sorted = new Uuid[] 
            {
                default, 
                default,
                Uuid.Parse("Qowaiv_SVOLibrary_GUI0"),
                Uuid.Parse("Qowaiv_SVOLibrary_GUI1"), 
                Uuid.Parse("Qowaiv_SVOLibrary_GUI2"),
                Uuid.Parse("Qowaiv_SVOLibrary_GUI3"),
            };
            var list = new List<Uuid> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
            list.Sort();
            Assert.AreEqual(sorted, list);
        }
    }

    public class Casts
    {
        [Test]
        public void explicitly_from_string()
        {
            var casted = (Uuid)"Qowaiv_SVOLibrary_GUIA";
            Assert.AreEqual(Svo.Uuid, casted);
        }

        [Test]
        public void explicitly_to_string()
        {
            var casted = (string)Svo.Uuid;
            Assert.AreEqual("Qowaiv_SVOLibrary_GUIA", casted);
        }

        [Test]
        public void explicitly_from_Guid()
        {
            var casted = (Uuid)Svo.Guid;
            Assert.AreEqual(Svo.Uuid, casted);
        }

        [Test]
        public void explicitly_to_Guid()
        {
            var casted = (Guid)Svo.Uuid;
            Assert.AreEqual(Svo.Guid, casted);
        }
    }

    public class Supports_type_conversion
    {
        [Test]
        public void via_TypeConverter_registered_with_attribute()
        {
            TypeConverterAssert.ConverterExists(typeof(Uuid));
        }

        [Test]
        public void from_null_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(default(Uuid), null);
            }
        }

        [Test]
        public void from_empty_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(default(Uuid), string.Empty);
            }
        }

        [Test]
        public void from_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(Svo.Uuid, Svo.Uuid.ToString());
            }
        }

        [Test]
        public void to_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertToStringEquals(Svo.Uuid.ToString(), Svo.Uuid);
            }
        }

        [Test]
        public void from_int()
        {
            TypeConverterAssert.ConvertFromEquals(Svo.Uuid, -17);
        }

        [Test]
        public void to_int()
        {
            TypeConverterAssert.ConvertToEquals(17, Svo.Uuid);
        }
    }

    public class Supports_JSON_serialization
    {
        [TestCase(null, "")]
        public void convension_based_serialization(object expected, Uuid svo)
        {
            var serialized = JsonTester.Write(svo);
            Assert.AreEqual(expected, serialized);
        }

        [TestCase("Invalid input", typeof(FormatException))]
        [TestCase("2017-06-11", typeof(FormatException))]
        [TestCase(5L, typeof(InvalidCastException))]
        public void throws_for_invalid_json(object json, Type exceptionType)
        {
            var exception = Assert.Catch(() => JsonTester.Read<Uuid>(json));
            Assert.IsInstanceOf(exceptionType, exception);
        }
    }

    public class Supports_XML_serialization
    {
        [Test]
        public void using_XmlSerializer_to_serialize()
        {
            var xml = SerializationTest.XmlSerialize(Svo.Uuid);
            Assert.AreEqual("Qowaiv_SVOLibrary_GUIA", xml);
        }

        [Test]
        public void using_XmlSerializer_to_deserialize()
        {
            var svo = SerializationTest.XmlDeserialize<Uuid>("Qowaiv_SVOLibrary_GUIA");
            Assert.AreEqual(Svo.Uuid, svo);
        }

        [Test]
        public void using_DataContractSerializer()
        {
            var round_tripped = SerializationTest.DataContractSerializeDeserialize(Svo.Uuid);
            Assert.AreEqual(Svo.Uuid, round_tripped);
        }

        [Test]
        public void as_part_of_a_structure()
        {
            var structure = XmlStructure.New(Svo.Uuid);
            var round_tripped = SerializationTest.XmlSerializeDeserialize(structure);
            Assert.AreEqual(structure, round_tripped);
        }

        [Test]
        public void has_no_custom_XML_schema()
        {
            IXmlSerializable obj = Svo.Uuid;
            Assert.IsNull(obj.GetSchema());
        }
    }

    public class Supports_binary_serialization
    {
        [Test]
        public void using_BinaryFormatter()
        {
            var round_tripped = SerializationTest.BinaryFormatterSerializeDeserialize(Svo.Uuid);
            Assert.AreEqual(Svo.Uuid, round_tripped);
        }

        [Test]
        public void storing_Guid_in_SerializationInfo()
        {
            var info = SerializationTest.GetSerializationInfo(Svo.Uuid);
            Assert.AreEqual(Svo.Guid, info.GetValue("Value", typeof(Guid)));
        }
    }

    public class Debug_experience
    {
        [TestCase("{empty}", "")]
        [TestCase("Qowaiv_SVOLibrary_GUIA", "Qowaiv_SVOLibrary_GUIA")]
        public void with_custom_display(object display, Uuid svo)
        {
            DebuggerDisplayAssert.HasResult(display, svo);
        }
    }
}

