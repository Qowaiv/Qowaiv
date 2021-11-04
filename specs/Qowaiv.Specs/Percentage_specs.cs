using FluentAssertions;
using NUnit.Framework;
using Qowaiv;
using Qowaiv.Financial;
using Qowaiv.Globalization;
using Qowaiv.Hashing;
using Qowaiv.Json;
using Qowaiv.Specs;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
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
        public void two_symbols(string str)
        {
            Assert.IsFalse(Percentage.IsValid(str));
        }

        [TestCase("1‱1")]
        [TestCase("1‰1")]
        [TestCase("1%1")]
        public void symbol_in_the_middle(string str)
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
            Assert.AreEqual("1%", Percentage.One.ToString("0%", CultureInfo.InvariantCulture));
        }

        [Test]
        public void Hundred_represent_100_percent()
        {
            Assert.AreEqual("100%", Percentage.Hundred.ToString("0%", CultureInfo.InvariantCulture));
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
        [TestCase("17.51%", 20431268)]
        public void hash_code_is_value_based(Percentage svo, int hash)
        {
            using (Hash.WithoutRandomizer())
            {
                svo.GetHashCode().Should().Be(hash);
            }
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

    public class Can_be_created_with_percentage_extension
    {
        [Test]
        public void from_int()
        {
            var p = 3.Percent();
            Assert.AreEqual("3%", p.ToString(CultureInfo.InvariantCulture));
        }

        [Test]
        public void from_double()
        {
            var p = 3.14.Percent();
            Assert.AreEqual("3.14%", p.ToString(CultureInfo.InvariantCulture));
        }

        [Test]
        public void from_decimal()
        {
            var p = 3.14m.Percent();
            Assert.AreEqual("3.14%", p.ToString(CultureInfo.InvariantCulture));
        }
    }

    public class Has_custom_formatting
    {
        [Test]
        public void _default()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Assert.AreEqual("17.51%", Svo.Percentage.ToString());
            }
        }

        [Test]
        public void with_null_pattern_equal_to_default()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Assert.AreEqual(Svo.Percentage.ToString(), Svo.Percentage.ToString(default(string)));
            }
        }

        [Test]
        public void with_string_empty_pattern_equal_to_default()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Assert.AreEqual(Svo.Percentage.ToString(), Svo.Percentage.ToString(string.Empty));
            }
        }

        [Test]
        public void custom_format_provider_is_applied()
        {
            var formatted = Svo.Percentage.ToString("0.000%", new UnitTestFormatProvider());
            Assert.AreEqual("Unit Test Formatter, value: '17.510%', format: '0.000%'", formatted);
        }

        [TestCase("en-GB", null, "17.51%", "17.51%")]
        [TestCase("nl-BE", "0.000%", "17.51%", "17,510%")]
        [TestCase("en", "%0.###", "17.51%", "%17.51")]
        [TestCase("en", "‰0.###", "17.51%", "‰175.1")]
        [TestCase("en", "‱0.###", "17.51%", "‱1751")]
        [TestCase("en", "0.###%", "17.51%", "17.51%")]
        [TestCase("en", "0.###‰", "17.51%", "175.1‰")]
        [TestCase("en", "0.###‱", "17.51%", "1751‱")]
        [TestCase("en", "0.###", "17.51%", "17.51")]
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
    
        [TestCase("fr-FR", "%")]
        [TestCase("fa-IR", "٪")]
        public void with_percent_sign_before_for(CultureInfo culture, string sign)
        {
            StringAssert.StartsWith(sign, Svo.Percentage.ToString(culture));
        }

        [Test]
        public void using_per_mille_sign()
        {
            using(TestCultures.En_GB.Scoped())
            {
                Assert.AreEqual("175.1‰", Svo.Percentage.ToString("PM"));
            }
        }

        [Test]
        public void using_per_then_thousand_sign()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Assert.AreEqual("1751‱", Svo.Percentage.ToString("PT"));
            }
        }
    }

    public class Formatting_is_invalid
    {
        [Test]
        public void when_multiple_symbols_are_specified()
        {
            Assert.Catch<FormatException>(() => Svo.Percentage.ToString("0%%"));
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
        public void _operators_for_different_values()
        {
            Percentage smaller = 17.51.Percent();
            Percentage bigger = 84.17.Percent();
            Assert.IsTrue(smaller < bigger);
            Assert.IsTrue(smaller <= bigger);
            Assert.IsFalse(smaller > bigger);
            Assert.IsFalse(smaller >= bigger);
        }

        [Test]
        public void _operators_for_equal_values()
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

    public class Can_be_multiplied_by
    {
        [Test]
        public void _percentage()
        {
            var multiplied = 17.Percent() * 42.Percent();
            Assert.AreEqual(7.14.Percent(), multiplied);
        }

        [Test]
        public void _decimal()
        {
            var multiplied = 17.Percent() * 0.42m;
            Assert.AreEqual(7.14.Percent(), multiplied);
        }

        [Test]
        public void _double()
        {
            var multiplied = 17.Percent() * 0.42;
            Assert.AreEqual(7.14.Percent(), multiplied);
        }

        [Test]
        public void _float()
        {
            var multiplied = 17.Percent() * 0.42F;
            Assert.AreEqual(7.14.Percent(), multiplied);
        }

        [Test]
        public void _int()
        {
            var multiplied = 17.Percent() * 2;
            Assert.AreEqual(34.Percent(), multiplied);
        }

        [Test]
        public void _uint()
        {
            var multiplied = 17.Percent() * 2U;
            Assert.AreEqual(34.Percent(), multiplied);
        }

        [Test]
        public void _long()
        {
            var multiplied = 17.Percent() * 2L;
            Assert.AreEqual(34.Percent(), multiplied);
        }

        [Test]
        public void _ulong()
        {
            var multiplied = 17.Percent() * 2UL;
            Assert.AreEqual(34.Percent(), multiplied);
        }

        [Test]
        public void _short()
        {
            var multiplied = 17.Percent() * ((short)2);
            Assert.AreEqual(34.Percent(), multiplied);
        }

        [Test]
        public void _ushort()
        {
            var multiplied = 17.Percent() * ((ushort)2);
            Assert.AreEqual(34.Percent(), multiplied);
        }
    }

    public class Can_be_devided_by
    {
        [Test]
        public void _percentage()
        {
            var multiplied = 17.Percent() / 50.Percent();
            Assert.AreEqual(34.Percent(), multiplied);
        }

        [Test]
        public void _decimal()
        {
            var multiplied = 17.Percent() / 0.5m;
            Assert.AreEqual(34.Percent(), multiplied);
        }

        [Test]
        public void _double()
        {
            var multiplied = 17.Percent() / 0.5;
            Assert.AreEqual(34.Percent(), multiplied);
        }

        [Test]
        public void _float()
        {
            var multiplied = 17.Percent() / 0.5F;
            Assert.AreEqual(34.Percent(), multiplied);
        }

        [Test]
        public void _int()
        {
            var multiplied = 17.Percent() / 2;
            Assert.AreEqual(8.5.Percent(), multiplied);
        }

        [Test]
        public void _uint()
        {
            var multiplied = 17.Percent() / 2U;
            Assert.AreEqual(8.5.Percent(), multiplied);
        }

        [Test]
        public void _long()
        {
            var multiplied = 17.Percent() / 2L;
            Assert.AreEqual(8.5.Percent(), multiplied);
        }

        [Test]
        public void _ulong()
        {
            var multiplied = 17.Percent() / 2UL;
            Assert.AreEqual(8.5.Percent(), multiplied);
        }

        [Test]
        public void _short()
        {
            var multiplied = 17.Percent() / ((short)2);
            Assert.AreEqual(8.5.Percent(), multiplied);
        }

        [Test]
        public void _ushort()
        {
            var multiplied = 17.Percent() / ((ushort)2);
            Assert.AreEqual(8.5.Percent(), multiplied);
        }
    }

    public class Can_be_added_to
    {
        [Test]
        public void _percentage()
        {
            var addition = 13.Percent() + 34.Percent();
            Assert.AreEqual(47.Percent(), addition);
        }

        [Test]
        public void _amount()
        {
            var addition = (Amount)44 + 50.Percent();
            Assert.AreEqual((Amount)66, addition);
        }

        [Test]
        public void _money()
        {
            var addition = (44.6 + Currency.EUR)+ 50.Percent();
            Assert.AreEqual(66.9 + Currency.EUR, addition);
        }

        [Test]
        public void _decimal()
        {
            var addition = 34.586m + 75.Percent();
            Assert.AreEqual(60.5255m, addition);
        }

        [Test]
        public void _double()
        {
            var addition = 34.586 + 75.Percent();
            Assert.That(addition, Is.EqualTo(60.5255).Within(0.00001));
        }

        [Test]
        public void _float()
        {
            var addition = 34.586f + 75.Percent();
            Assert.That(addition, Is.EqualTo(60.5255f).Within(0.00001));
        }

        [Test]
        public void _int()
        {
            var addition = 400 + 17.Percent();
            Assert.AreEqual(468, addition);
        }

        [Test]
        public void _uint()
        {
            var addition = 400U + 17.Percent();
            Assert.AreEqual(468U, addition);
        }

        [Test]
        public void _long()
        {
            var addition = 400L + 17.Percent();
            Assert.AreEqual(468L, addition);
        }

        [Test]
        public void _ulong()
        {
            var addition = 400UL + 17.Percent();
            Assert.AreEqual(468UL, addition);
        }

        [Test]
        public void _short()
        {
            var addition = ((short)400) + 17.Percent();
            Assert.AreEqual((short)468, addition);
        }

        [Test]
        public void _ushort()
        {
            var addition = ((ushort)400) + 17.Percent();
            Assert.AreEqual((ushort)468, addition);
        }
    }

    public class Can_be_subtracted_from
    {
        [Test]
        public void _percentage()
        {
            var addition = 13.Percent() - 34.Percent();
            Assert.AreEqual(-21.Percent(), addition);
        }

        [Test]
        public void _amount()
        {
            var addition = (Amount)44.6 - 50.Percent();
            Assert.AreEqual((Amount)22.3, addition);
        }

        [Test]
        public void _money()
        {
            var addition = (44.6 + Currency.EUR) - 50.Percent();
            Assert.AreEqual(22.3 + Currency.EUR, addition);
        }

        [Test]
        public void _decimal()
        {
            var addition = 34.586m - 75.Percent();
            Assert.AreEqual(8.6465m, addition);
        }

        [Test]
        public void _double()
        {
            var addition = 34.586 - 75.Percent();
            Assert.That(addition, Is.EqualTo(8.6465).Within(0.00001));
        }

        [Test]
        public void _float()
        {
            var addition = 34.586f - 75.Percent();
            Assert.That(addition, Is.EqualTo(8.6465f).Within(0.00001));
        }

        [Test]
        public void _int()
        {
            var addition = 400 - 17.Percent();
            Assert.AreEqual(332, addition);
        }

        [Test]
        public void _uint()
        {
            var addition = 400U - 17.Percent();
            Assert.AreEqual(332U, addition);
        }

        [Test]
        public void _long()
        {
            var addition = 400L - 17.Percent();
            Assert.AreEqual(332L, addition);
        }

        [Test]
        public void _ulong()
        {
            var addition = 400UL - 17.Percent();
            Assert.AreEqual(332UL, addition);
        }

        [Test]
        public void _short()
        {
            var addition = ((short)400) - 17.Percent();
            Assert.AreEqual((short)332, addition);
        }

        [Test]
        public void _ushort()
        {
            var addition = ((ushort)400) - 17.Percent();
            Assert.AreEqual((ushort)332, addition);
        }
    }

    public class Can_get_a_percentage_of
    {
        [Test]
        public void _percentage()
        {
            var addition = 13.Percent() * 34.Percent();
            Assert.AreEqual(4.42.Percent(), addition);
        }

        [Test]
        public void _amount()
        {
            var addition = (Amount)44.6 * 80.Percent();
            Assert.AreEqual((Amount)35.68, addition);
        }

        [Test]
        public void _money()
        {
            var addition = (44.6 + Currency.EUR) * 80.Percent();
            Assert.AreEqual(35.68 + Currency.EUR, addition);
        }

        [Test]
        public void _decimal()
        {
            var addition = 34.586m * 75.Percent();
            Assert.AreEqual(25.9395m, addition);
        }

        [Test]
        public void _double()
        {
            var addition = 34.586 * 75.Percent();
            Assert.That(addition, Is.EqualTo(25.9395).Within(0.00001));
        }

        [Test]
        public void _float()
        {
            var addition = 34.586f * 75.Percent();
            Assert.That(addition, Is.EqualTo(25.9395f).Within(0.00001));
        }

        [Test]
        public void _int()
        {
            var addition = 400 * 17.Percent();
            Assert.AreEqual(68, addition);
        }

        [Test]
        public void _uint()
        {
            var addition = 400U * 17.Percent();
            Assert.AreEqual(68U, addition);
        }

        [Test]
        public void _long()
        {
            var addition = 400L * 17.Percent();
            Assert.AreEqual(68L, addition);
        }

        [Test]
        public void _ulong()
        {
            var addition = 400UL * 17.Percent();
            Assert.AreEqual(68UL, addition);
        }

        [Test]
        public void _short()
        {
            var addition = ((short)400) * 17.Percent();
            Assert.AreEqual((short)68, addition);
        }

        [Test]
        public void _ushort()
        {
            var addition = ((ushort)400) * 17.Percent();
            Assert.AreEqual((ushort)68, addition);
        }
    }

    public class Can_get_100_percent_based_on_percentage
    {
        [Test]
        public void _percentage()
        {
            var addition = 13.Percent() / 25.Percent();
            Assert.AreEqual(52.Percent(), addition);
        }

        [Test]
        public void _amount()
        {
            var addition = (Amount)44.6 / 80.Percent();
            Assert.AreEqual((Amount)55.75, addition);
        }

        [Test]
        public void _money()
        {
            var addition = (44.6 + Currency.EUR) / 80.Percent();
            Assert.AreEqual(55.75 + Currency.EUR, addition);
        }

        [Test]
        public void _decimal()
        {
            var addition = 34.586m / 75.Percent();
            Assert.That(addition, Is.EqualTo(46.11467m).Within(0.00001));
        }

        [Test]
        public void _double()
        {
            var addition = 34.586 / 75.Percent();
            Assert.That(addition, Is.EqualTo(46.11467).Within(0.00001));
        }

        [Test]
        public void _float()
        {
            var addition = 34.586f / 75.Percent();
            Assert.That(addition, Is.EqualTo(46.11467f).Within(0.00001));
        }

        [Test]
        public void _int()
        {
            var addition = 400 / 17.Percent();
            Assert.AreEqual(2352, addition);
        }

        [Test]
        public void _uint()
        {
            var addition = 400U / 17.Percent();
            Assert.AreEqual(2352U, addition);
        }

        [Test]
        public void _long()
        {
            var addition = 400L / 17.Percent();
            Assert.AreEqual(2352L, addition);
        }

        [Test]
        public void _ulong()
        {
            var addition = 400UL / 17.Percent();
            Assert.AreEqual(2352UL, addition);
        }

        [Test]
        public void _short()
        {
            var addition = ((short)400) / 17.Percent();
            Assert.AreEqual((short)2352, addition);
        }

        [Test]
        public void _ushort()
        {
            var addition = ((ushort)400) / 17.Percent();
            Assert.AreEqual((ushort)2352, addition);
        }
    }

    public class Can_be_rounded
    {
        [Test]
        public void zero_decimals()
        {
            var actual = Svo.Percentage.Round();
            Assert.AreEqual(18.Percent(), actual);
        }

        [Test]
        public void one_decimal()
        {
            var actual = Svo.Percentage.Round(1);
            Assert.AreEqual(17.5.Percent(), actual);
        }

        [Test]
        public void away_from_zero()
        {
            var actual = 16.5.Percent().Round(0, DecimalRounding.AwayFromZero);
            Assert.AreEqual(17.Percent(), actual);
        }

        [Test]
        public void to_even()
        {
            var actual = 16.5.Percent().Round(0, DecimalRounding.ToEven);
            Assert.AreEqual(16.Percent(), actual);
        }

        [Test]
        public void to_multiple()
        {
            var actual = 16.4.Percent().RoundToMultiple(3.Percent());
            Assert.AreEqual(15.Percent(), actual);
        }

        [Test]
        public void up_to_26_digits()
        {
            var exception = Assert.Catch<ArgumentOutOfRangeException>(() => Svo.Percentage.Round(27));
            StringAssert.StartsWith("Percentages can only round to between -26 and 26 digits of precision.", exception.Message);
        }

        [Test]
        public void up_to_minus_26_digits()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Svo.Percentage.Round(-27));
        }
    
        [Test, Obsolete("Only exists for guidance towards decimal rounding methods.")]
        public void using_system_midpoint_rounding()
        {
            var rounded = Svo.Percentage.Round(0, MidpointRounding.AwayFromZero);
            Assert.AreEqual(18.Percent(), rounded);
        }
    }

    public class Can_be_increased
    {
        [Test]
        public void with_1_percent()
        {
            var increased = Svo.Percentage;
            increased++;
            Assert.AreEqual(18.51.Percent(), increased);
        }
    }

    public class Can_be_decreased
    {
        [Test]
        public void with_1_percent()
        {
            var decreased = Svo.Percentage;
            decreased--;
            Assert.AreEqual(16.51.Percent(), decreased);
        }
    }

    public class Can_be_negated
    {
        [TestCase("17.51%", "-17.51%")]
        [TestCase("-17.51%", "17.51%")]
        public void negate(Percentage negated, Percentage input)
        {
            Assert.AreEqual(negated, -input);
        }
    }

    public class Can_be_plussed
    {
        [TestCase("-17.51%", "-17.51%")]
        [TestCase("17.51%", "17.51%")]
        public void plus(Percentage negated, Percentage input)
        {
            Assert.AreEqual(negated, +input);
        }
    }


    public class Can_get
    {
        [TestCase(-1, "-3%")]
        [TestCase(0, "0%")]
        [TestCase(+1, "10%")]
        public void Sign(int expected, Percentage percentage)
        {
            var actual = percentage.Sign();
            Assert.AreEqual(expected, actual);
        }
   
        [TestCase("3%", "-3%")]
        [TestCase("0%", "0%")]
        [TestCase("10%", "10%")]
        public void absolute_value(Percentage expected, Percentage percentage)
        {
            var actual = percentage.Abs();
            Assert.AreEqual(expected, actual);
        }
    }

    public class Can_get_maximum_of
    {
        [TestCase("12%", "12%", "5%")]
        [TestCase("15%", "5%", "15%")]
        [TestCase("12%", "12%", "12%")]
        public void two_values(Percentage max, Percentage p0, Percentage p1)
        {
            Assert.AreEqual(max, Percentage.Max(p0, p1));
        }

        [Test]
        public void multiple_values()
        {
            var max = Percentage.Max(15.Percent(), 66.Percent(), -117.Percent());
            Assert.AreEqual(66.Percent(), max);
        }
    }

    public class Can_get_minimum_of
    {
        [TestCase("5%", "12%", "5%")]
        [TestCase("5%", "5%", "15%")]
        [TestCase("5%", "5%", "5%")]
        public void two_values(Percentage min, Percentage p0, Percentage p1)
        {
            Assert.AreEqual(min, Percentage.Min(p0, p1));
        }

        [Test]
        public void multiple_values()
        {
            var min = Percentage.Min(15.Percent(), 66.Percent(), -117.Percent());
            Assert.AreEqual(-117.Percent(), min);
        }
    }

    public class Supports_type_conversion
    {
        [Test]
        public void via_TypeConverter_registered_with_attribute()
            => typeof(Percentage).Should().HaveTypeConverterDefined();

        [Test]
        public void from_null_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.To<Percentage>().From(null).Should().Be(Percentage.Zero);
            }
        }

        [Test]
        public void from_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.To<Percentage>().From("17.51%").Should().Be(Svo.Percentage);
            }
        }

        [Test]
        public void to_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.Value(Svo.Percentage).ToString().Should().Be("17.51%");
            }
        }

        [Test]
        public void from_int()
            => Converting.To<Percentage>().From(-17).Should().Be(-1700.Percent());

        [Test]
        public void to_int()
            => Converting.Value(1700.Percent()).To<int>().Should().Be(17);
        
        [Test]
        public void from_decimal()
            => Converting.To<Percentage>().From(0.1751m).Should().Be(Svo.Percentage);

        [Test]
        public void to_decimal()
            => Converting.Value(Svo.Percentage).To<decimal>().Should().Be(0.1751m);
        
        [Test]
        public void from_double()
            => Converting.To<Percentage>().From(0.1751).Should().Be(Svo.Percentage);

        [Test]
        public void to_double()
            => Converting.Value(Svo.Percentage).To<double>().Should().Be(0.1751);
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
            var xml = Serialize.Xml(Svo.Percentage);
            Assert.AreEqual("17.51%", xml);
        }

        [Test]
        public void using_XmlSerializer_to_deserialize()
        {
            var svo =Deserialize.Xml<Percentage>("17.51%");
            Assert.AreEqual(Svo.Percentage, svo);
        }

        [Test]
        public void using_DataContractSerializer()
        {
            var round_tripped = SerializeDeserialize.DataContract(Svo.Percentage);
            Assert.AreEqual(Svo.Percentage, round_tripped);
        }

        [Test]
        public void as_part_of_a_structure()
        {
            var structure = XmlStructure.New(Svo.Percentage);
            var round_tripped = SerializeDeserialize.Xml(structure);
            Assert.AreEqual(structure, round_tripped);
        }

        [Test]
        public void has_no_custom_XML_schema()
        {
            IXmlSerializable obj = Svo.Percentage;
            Assert.IsNull(obj.GetSchema());
        }
    }

    public class Is_Open_API_data_type
    {
        internal static readonly OpenApiDataTypeAttribute Attribute = OpenApiDataTypeAttribute.From(typeof(Percentage)).FirstOrDefault();

        [Test]
        public void with_description() => Attribute.Description.Should().Be("Ratio expressed as a fraction of 100 denoted using the percent sign '%'.");

        [Test]
        public void with_example() => Attribute.Example.Should().Be("13.76%");

        [Test]
        public void has_type() => Attribute.Type.Should().Be("string");

        [Test]
        public void has_format() => Attribute.Format.Should().Be("percentage");

        [TestCase("17.51%")]
        [TestCase("-4.1%")]
        [TestCase("-0.1%")]
        [TestCase("31%")]
        public void pattern_matches(string input) => Regex.IsMatch(input, Attribute.Pattern).Should().BeTrue();
    }

    public class Supports_binary_serialization
    {
        [Test]
        public void using_BinaryFormatter()
        {
            var round_tripped = SerializeDeserialize.Binary(Svo.Percentage);
            Assert.AreEqual(Svo.Percentage, round_tripped);
        }

        [Test]
        public void storing_decimal_in_SerializationInfo()
        {
            var info = Serialize.GetInfo(Svo.Percentage);
            Assert.AreEqual(0.1751m, info.GetDecimal("Value"));
        }
    }

    public class Debugger
    {
        [TestCase("17.51%", "17.51%")]
        public void has_custom_display(object display, Percentage svo)
            => svo.Should().HaveDebuggerDisplay(display);
    }
}
