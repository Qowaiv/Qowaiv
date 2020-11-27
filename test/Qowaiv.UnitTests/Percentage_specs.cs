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

namespace Percentage_specs
{
    public class Is_valid_for
    {
        [TestCase("1751‱", "en")]
        [TestCase("175.1‰", "en")]
        [TestCase("17.51", "en")]
        [TestCase("17.51%", "en")]
        [TestCase("17,51%", "nl")]
        public void strings_representing_SVO(string input, CultureInfo culture)
        {
            Assert.IsTrue(Percentage.IsValid(input, culture));
        }

        [TestCase("175.1<>", "en")]
        [TestCase("17,51#", "nl")]
        public void custom_culture_with_different_symbols(string input, CultureInfo culture)
        {
            using(culture.WithPercentageSymbols("#", "<>").Scoped())
            {
                Assert.IsTrue(Percentage.IsValid(input));
            }
        }
    }

    public class Is_not_valid_for
    {
        [Test]
        public void string_empty()
        {
            Assert.IsFalse(Percentage.IsValid(string.Empty));
        }

        [Test]
        public void string_null()
        {
            Assert.IsFalse(Percentage.IsValid(null));
        }

        [Test]
        public void whitespace()
        {
            Assert.IsFalse(Percentage.IsValid(" "));
        }

        [TestCase("‱1‱")]
        [TestCase("‱1‰")]
        [TestCase("‱1%")]
        public void two_marks(string str)
        {
            Assert.IsFalse(Percentage.IsValid(str));
        }

        [TestCase("1‱1")]
        [TestCase("1‰1")]
        [TestCase("1%1")]
        public void mark_in_the_middle(string str)
        {
            Assert.IsFalse(Percentage.IsValid(str));
        }

        [Test]
        public void garbage()
        {
            Assert.IsFalse(Percentage.IsValid("garbage"));
        }
    }

    public class Has_constant
    {
        [Test]
        public void Zero_represent_default_value()
        {
            Assert.AreEqual(default(Percentage), Percentage.Zero);
        }

        [Test]
        public void One_represent_1_percent()
        {
            Assert.AreEqual(1.Percent(), Percentage.One);
        }

        [Test]
        public void Hundred_represent_100_percent()
        {
            Assert.AreEqual(100.Percent(), Percentage.Hundred);
        }
    }

    public class Is_equal_by_value
    {
        [Test]
        public void not_equal_to_null()
        {
            Assert.IsFalse(Svo.Percentage.Equals(null));
        }

        [Test]
        public void not_equal_to_other_type()
        {
            Assert.IsFalse(Svo.Percentage.Equals(new object()));
        }

        [Test]
        public void not_equal_to_different_value()
        {
            Assert.IsFalse(Svo.Percentage.Equals(84.17.Percent()));
        }

        [Test]
        public void equal_to_same_value()
        {
            Assert.IsTrue(Svo.Percentage.Equals(17.51.Percent()));
        }

        [Test]
        public void equal_operator_returns_true_for_same_values()
        {
            Assert.IsTrue(Svo.Percentage == 17.51.Percent());
        }

        [Test]
        public void equal_operator_returns_false_for_different_values()
        {
            Assert.IsFalse(Svo.Percentage == 6.66.Percent());
        }

        [Test]
        public void not_equal_operator_returns_false_for_same_values()
        {
            Assert.IsFalse(Svo.Percentage != 17.51.Percent());
        }

        [Test]
        public void not_equal_operator_returns_true_for_different_values()
        {
            Assert.IsTrue(Svo.Percentage != 6.66.Percent());
        }

        [TestCase("0%", 0)]
        [TestCase("17.51%", 263895)]
        public void hash_code_is_value_based(Percentage svo, int hashcode)
        {
            Assert.AreEqual(hashcode, svo.GetHashCode());
        }
    }

    public class Can_be_parsed
    {
        [TestCase("en", "175.1‰")]
        [TestCase("en", "175.1‰")]
        [TestCase("en", "1751‱")]
        [TestCase("nl", "17,51%")]
        [TestCase("fr-FR", "%17,51")]
        public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
        {
            using (culture.Scoped())
            {
                var parsed = Percentage.Parse(input);
                Assert.AreEqual(Svo.Percentage, parsed);
            }
        }

        [TestCase("175.1<>", "en")]
        [TestCase("17,51#", "nl")]
        public void with_custom_culture_with_different_symbols(string input, CultureInfo culture)
        {
            var parsed = Percentage.Parse(input, culture.WithPercentageSymbols("#", "<>"));
            Assert.AreEqual(Svo.Percentage, parsed);
        }

        [Test]
        public void from_valid_input_only_otherwise_throws_on_Parse()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var exception = Assert.Throws<FormatException>(() => Percentage.Parse("invalid input"));
                Assert.AreEqual("Not a valid percentage", exception.Message);
            }
        }

        [Test]
        public void from_valid_input_only_otherwise_return_false_on_TryParse()
        {
            Assert.IsFalse(Percentage.TryParse("invalid input", out _));
        }

        [Test]
        public void from_invalid_as_empty_with_TryParse()
        {
            Assert.AreEqual(default(Percentage), Percentage.TryParse("invalid input"));
        }

        [Test]
        public void with_TryParse_returns_SVO()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Assert.AreEqual(Svo.Percentage, Percentage.TryParse("17.51%"));
            }
        }
    }

    public class Has_custom_formatting
    {
        [Test]
        public void custom_format_provider_is_applied()
        {
            var formatted = Svo.Percentage.ToString("0.000%", new UnitTestFormatProvider());
            Assert.AreEqual("Unit Test Formatter, value: '17.510%', format: '0.000%'", formatted);
        }

        [TestCase("en-GB", null, "17.51%", "17.51")]
        [TestCase("nl-BE", "0.000%", "17.51%", "17,510%")]
        public void culture_dependent(CultureInfo culture, string format, Percentage svo, string expected)
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
                Assert.AreEqual("17,51%", Svo.Percentage.ToString(provider: null));
            }
        }
    }

    public class Is_comparable
    {
        [Test]
        public void to_null()
        {
            Assert.AreEqual(1, Svo.Percentage.CompareTo(null));
        }

        [Test]
        public void to_Percentage_as_object()
        {
            object obj = Svo.Percentage;
            Assert.AreEqual(0, Svo.Percentage.CompareTo(obj));
        }

        [Test]
        public void to_Percentage_only()
        {
            Assert.Throws<ArgumentException>(() => Svo.Percentage.CompareTo(new object()));
        }

        [Test]
        public void can_be_sorted_using_compare()
        {
            var sorted = new [] 
            {
                Percentage.Zero,
                Percentage.One,
                17.51.Percent(),
                33.33.Percent(),
                84.17.Percent(),
                Percentage.Hundred, 
            };
            var list = new List<Percentage> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
            list.Sort();
            Assert.AreEqual(sorted, list);
        }

        [Test]
        public void by_operators_for_different_values()
        {
            Percentage smaller = 17.51.Percent();
            Percentage bigger = 84.17.Percent();
            Assert.IsTrue(smaller < bigger);
            Assert.IsTrue(smaller <= bigger);
            Assert.IsFalse(smaller > bigger);
            Assert.IsFalse(smaller >= bigger);
        }

        [Test]
        public void by_operators_for_equal_values()
        {
            Percentage left = 17.51.Percent();
            Percentage right = 17.51.Percent();
            Assert.IsFalse(left < right);
            Assert.IsTrue(left <= right);
            Assert.IsFalse(left > right);
            Assert.IsTrue(left >= right);
        }
    }

    public class Casts
    {
        [Test]
        public void explicitly_from_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var casted = (Percentage)"17.51%";
                Assert.AreEqual(Svo.Percentage, casted);
            }
        }

        [Test]
        public void explicitly_to_string()
        {
            using (TestCultures.Nl_NL.Scoped())
            {
                var casted = (string)Svo.Percentage;
                Assert.AreEqual("17,51%", casted);
            }
        }

        [Test]
        public void explicitly_from_decimal()
        {
            var casted = (Percentage)0.1751m;
            Assert.AreEqual(Svo.Percentage, casted);
        }

        [Test]
        public void explicitly_to_decimal()
        {
            var casted = (decimal)Svo.Percentage;
            Assert.AreEqual(0.1751m, casted);
        }

        [Test]
        public void explicitly_from_double()
        {
            var casted = (Percentage)0.1751;
            Assert.AreEqual(Svo.Percentage, casted);
        }

        [Test]
        public void explicitly_to_double()
        {
            var casted = (double)Svo.Percentage;
            Assert.AreEqual(0.1751, casted);
        }
    }

    public class Supports_type_conversion
    {
        [Test]
        public void via_TypeConverter_registered_with_attribute()
        {
            TypeConverterAssert.ConverterExists(typeof(Percentage));
        }

        [Test]
        public void from_null_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(default(Percentage), null);
            }
        }

        [Test]
        public void from_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(Svo.Percentage, Svo.Percentage.ToString());
            }
        }

        [Test]
        public void to_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertToStringEquals(Svo.Percentage.ToString(), Svo.Percentage);
            }
        }

        [Test]
        public void from_int()
        {
            TypeConverterAssert.ConvertFromEquals(-1700.Percent(), -17);
        }

        [Test]
        public void to_int()
        {
            TypeConverterAssert.ConvertToEquals(17, 1700.Percent());
        }

        [Test]
        public void from_decimal()
        {
            TypeConverterAssert.ConvertFromEquals(Svo.Percentage, 0.1751m);
        }

        [Test]
        public void to_decimal()
        {
            TypeConverterAssert.ConvertToEquals(0.1751m, Svo.Percentage);
        }

        [Test]
        public void from_double()
        {
            TypeConverterAssert.ConvertFromEquals(Svo.Percentage, 0.1751);
        }

        [Test]
        public void to_double()
        {
            TypeConverterAssert.ConvertToEquals(0.1751, Svo.Percentage);
        }
    }

    public class Supports_JSON_serialization
    {
        [TestCase("17.51%", "17.51")]
        [TestCase("17.51%", "175.1‰")]
        [TestCase("17.51%", 0.1751)]
        public void convention_based_deserialization(Percentage expected, object json)
        {
            var actual = JsonTester.Read<Percentage>(json);
            Assert.AreEqual(expected, actual);
        }

        [TestCase("17.51%", "17.51%")]
        [TestCase("0%", "0%")]
        public void convention_based_serialization(object expected, Percentage svo)
        {
            var serialized = JsonTester.Write(svo);
            Assert.AreEqual(expected, serialized);
        }

        [TestCase("Invalid input", typeof(FormatException))]
        public void throws_for_invalid_json(object json, Type exceptionType)
        {
            var exception = Assert.Catch(() => JsonTester.Read<Percentage>(json));
            Assert.IsInstanceOf(exceptionType, exception);
        }
    }

    public class Supports_XML_serialization
    {
        [Test]
        public void using_XmlSerializer_to_serialize()
        {
            var xml = SerializationTest.XmlSerialize(Svo.Percentage);
            Assert.AreEqual("17.51%", xml);
        }

        [Test]
        public void using_XmlSerializer_to_deserialize()
        {
            var svo = SerializationTest.XmlDeserialize<Percentage>("17.51%");
            Assert.AreEqual(Svo.Percentage, svo);
        }

        [Test]
        public void using_DataContractSerializer()
        {
            var round_tripped = SerializationTest.DataContractSerializeDeserialize(Svo.Percentage);
            Assert.AreEqual(Svo.Percentage, round_tripped);
        }

        [Test]
        public void as_part_of_a_structure()
        {
            var structure = XmlStructure.New(Svo.Percentage);
            var round_tripped = SerializationTest.XmlSerializeDeserialize(structure);
            Assert.AreEqual(structure, round_tripped);
        }

        [Test]
        public void has_no_custom_XML_schema()
        {
            IXmlSerializable obj = Svo.Percentage;
            Assert.IsNull(obj.GetSchema());
        }
    }

    public class Supports_binary_serialization
    {
        [Test]
        public void using_BinaryFormatter()
        {
            var round_tripped = SerializationTest.BinaryFormatterSerializeDeserialize(Svo.Percentage);
            Assert.AreEqual(Svo.Percentage, round_tripped);
        }

        [Test]
        public void storing_decimal_in_SerializationInfo()
        {
            var info = SerializationTest.GetSerializationInfo(Svo.Percentage);
            Assert.AreEqual(0.1751m, info.GetDecimal("Value"));
        }
    }

    public class Debugger
    {
        [TestCase("17.51%", "17.51%")]
        public void has_custom_display(object display, Percentage svo)
        {
            DebuggerDisplayAssert.HasResult(display, svo);
        }
    }
}

