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
using System.Linq;
using System.Text;
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

    public class Has_version
    {
        [Test]
        public void Random_for_new()
        {
            var id = Uuid.NewUuid();
            Assert.AreEqual(UuidVersion.Random, id.Version);
        }

        [Test]
        public void Sequential_for_new_sequential()
        {
            var id = Uuid.NewSequential();
            Assert.AreEqual(UuidVersion.Sequential, id.Version);
        }

        [Test]
        public void MD5_for_generated_with_MD5()
        {
            var id = Uuid.GenerateWithMD5(Encoding.UTF8.GetBytes("Qowaiv"));
            Assert.AreEqual(UuidVersion.MD5, id.Version);
        }

        [Test]
        public void SHA1_for_generated_with_SHA1()
        {
            var id = Uuid.GenerateWithSHA1(Encoding.UTF8.GetBytes("Qowaiv"));
            Assert.AreEqual(UuidVersion.SHA1, id.Version);
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

    public class Can_be_created
    {
        [Test]
        public void with_global_unique_value()
        {
            CollectionAssert.AllItemsAreUnique(Enumerable.Range(0, 10_000)
                 .Select(i => Uuid.NewUuid()));
        }

        [Test]
        public void with_MD5()
        {
            var hashed = Uuid.GenerateWithMD5(Encoding.UTF8.GetBytes("Qowaiv"));
            Assert.AreEqual(Uuid.Parse("lmZO_haEOTCwGsCcbIZFFg"), hashed);
        }

        [Test]
        public void with_SHA1()
        {
            var hashed = Uuid.GenerateWithSHA1(Encoding.UTF8.GetBytes("Qowaiv"));
            Assert.AreEqual(Uuid.Parse("39h-Y1rR51ym_t78x9h0bA"), hashed);
        }
    }

    public class Can_be_created_sequential
    {
        [Test]
        public void from_1_Jan_1970_on()
        {
            using (Clock.SetTimeForCurrentThread(() => new DateTime(1970, 01, 01).AddTicks(-1)))
            {
                Assert.Catch<InvalidOperationException>(() => Uuid.NewSequential());
            }
        }

        [Test]
        public void until_3_Dec_9276()
        {
            using (Clock.SetTimeForCurrentThread(() => new DateTime(9276, 12, 04)))
            {
                Assert.Catch<InvalidOperationException>(() => Uuid.NewSequential());
            }
        }

        /// <remarks>Due to the reordening the version ands up in index 7.</remarks>
        [Test]
        public void on_min_date_first_6_bytes_are_0_for_default()
        {
            using (Clock.SetTimeForCurrentThread(() => new DateTime(1970, 01, 01)))
            {
                var actual = Uuid.NewSequential();
                var expected = new byte?[]
                {
                    0, 0, 0, 0,

                    0, 0, null, 0x60,
                };
                AssertBytes(expected, actual);
            }
        }

        [Test]
        public void on_min_date_first_6_bytes_are_0_for_SQL_Server()
        {
            using (Clock.SetTimeForCurrentThread(() => new DateTime(1970, 01, 01)))
            {
                var actual = Uuid.NewSequential(UuidComparer.SqlServer);
                var expected = new byte?[]
                {
                    null, null, null, null,

                    null, null, null, null,

                    0, null, 0, 0,

                    0, 0, 0, 0,
                };
                AssertBytes(expected, actual);
            }
        }

        /// <remarks>Due to the reordening the version ands up in index 7.</remarks>
        [Test]
        public void on_max_date_first_7_bytes_are_255_for_default()
        {
            using (Clock.SetTimeForCurrentThread(() => MaxDate))
            {
                var actual = Uuid.NewSequential();
                var expected = new byte?[]
                {
                    0xFF, 0xFF, 0xFF, 0xFF,

                    0xFF, 0xFF, null, 0x6F,
                };
                AssertBytes(expected, actual);
            }
        }

        [Test]
        public void on_max_date_first_7_bytes_are_255_for_SQL_Server()
        {
            using (Clock.SetTimeForCurrentThread(() => MaxDate))
            {
                var actual = Uuid.NewSequential(UuidComparer.SqlServer);
                var expected = new byte?[]
                {
                    null, null, null, null,

                    null, null, null, null,

                    0xFF, null, 0xFF, 0xFF,

                    0xFF, 0xFF, 0xFF, 0xFF,
                };
                AssertBytes(expected, actual);
            }
        }

        [Test]
        public void is_sorted_for_default() => AssertIsSorted(UuidComparer.Default);

        [Test]
        public void is_sorted_for_MongoDb() => AssertIsSorted(UuidComparer.MongoDb);

        [Test]
        public void is_sorted_for_SQL_Server() => AssertIsSorted(UuidComparer.SqlServer);

        private const int MultipleCount = 10000;
        
        private DateTime MaxDate => new DateTime(9276, 12, 03, 18, 42, 01).AddTicks(3693920);

        private static void AssertIsSorted(UuidComparer comparer)
        {
            var ids = new List<Uuid>(MultipleCount);

            foreach (var date in GetTimes().Take(MultipleCount))
            {
                using (Clock.SetTimeForCurrentThread(() => date))
                {
                    var id = Uuid.NewSequential(comparer);
                    Console.WriteLine(ToString(id, comparer));
                    ids.Add(id);
                }
            }
            CollectionAssert.IsOrdered(ids, comparer);
        }

        private static void AssertBytes(byte?[] expected, Uuid actual)
        {
            var act = new List<string>();
            var exp = new List<string>();
            var fail = false;

            var bytes = actual.ToByteArray();

            for (var i = 0; i < bytes.Length; i++)
            {
                var a = bytes[i].ToString("X2");

                if (i < expected.Length && expected[i].HasValue)
                {
                    var e = expected[i].Value.ToString("X2");

                    if (e == a)
                    {
                        act.Add(a);
                        exp.Add(e);
                    }
                    else
                    {
                        act.Add('[' + a + ']');
                        exp.Add('[' + e + ']');
                        fail = true;
                    }
                }
                else
                {
                    act.Add(a);
                    exp.Add("..");
                }
            }

            if (fail)
            {
                Assert.Fail($@"Expected: [{(string.Join(", ", exp))}]
Actual:   [{(string.Join(", ", act))}]");
            }

            Assert.AreEqual(UuidVersion.Sequential, actual.Version);
        }

        private static IEnumerable<DateTime> GetTimes()
        {
            var i = 17;

            var date = new DateTime(1970, 01, 01);

            while (true)
            {
                date = date.AddSeconds(3).AddTicks(i++);
                yield return date;
            }
        }

        private static string ToString(Uuid uuid, UuidComparer comparer)
        {
            var bytes = uuid.ToByteArray();
            var sb = new StringBuilder(48);

            foreach (var index in comparer.Priority)
            {
                sb.Append(bytes[index].ToString("X2")).Append(" ");
            }
            return sb.ToString();
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
        [TestCase("en-GB", "H", "Qowaiv_SVOLibrary_GUIA", "IKGBVCX72JKOFYTOW2V4X4MUEA")]
        [TestCase("en-GB", "h", "Qowaiv_SVOLibrary_GUIA", "ikgbvcx72jkofytow2v4x4muea")]
        [TestCase("en-GB", "N", "Qowaiv_SVOLibrary_GUIA", "8A1A8C42D2FFE254E26EB6ABCBF19420")]
        [TestCase("en-GB", "n", "Qowaiv_SVOLibrary_GUIA", "8a1a8c42d2ffe254e26eb6abcbf19420")]
        [TestCase("en-GB", "D", "Qowaiv_SVOLibrary_GUIA", "8A1A8C42-D2FF-E254-E26E-B6ABCBF19420")]
        [TestCase("en-GB", "d", "Qowaiv_SVOLibrary_GUIA", "8a1a8c42-d2ff-e254-e26e-b6abcbf19420")]
        [TestCase("nl-BE", "B", "Qowaiv_SVOLibrary_GUIA", "{8A1A8C42-D2FF-E254-E26E-B6ABCBF19420}")]
        [TestCase("nl-BE", "b", "Qowaiv_SVOLibrary_GUIA", "{8a1a8c42-d2ff-e254-e26e-b6abcbf19420}")]
        [TestCase("nl-BE", "B", "Qowaiv_SVOLibrary_GUIA", "{8A1A8C42-D2FF-E254-E26E-B6ABCBF19420}")]
        [TestCase("nl-BE", "b", "Qowaiv_SVOLibrary_GUIA", "{8a1a8c42-d2ff-e254-e26e-b6abcbf19420}")]
        [TestCase("nl-BE", "P", "Qowaiv_SVOLibrary_GUIA", "(8A1A8C42-D2FF-E254-E26E-B6ABCBF19420)")]
        [TestCase("nl-BE", "p", "Qowaiv_SVOLibrary_GUIA", "(8a1a8c42-d2ff-e254-e26e-b6abcbf19420)")]
        [TestCase("nl-BE", "X", "Qowaiv_SVOLibrary_GUIA", "{0x8A1A8C42,0xD2FF,0xE254,{0xE2,0x6E,0xB6,0xAB,0xCB,0xF1,0x94,0x20}}")]
        public void culture_independent(CultureInfo culture, string format, Uuid svo, string expected)
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
        public void from_Guid()
        {
            TypeConverterAssert.ConvertFromEquals(Svo.Uuid, Svo.Guid);
        }

        [Test]
        public void to_Guid()
        {
            TypeConverterAssert.ConvertToEquals(Svo.Guid, Svo.Uuid);
        }
    }

    public class Supports_JSON_serialization
    {
        [TestCase(null, "")]
        [TestCase("Qowaiv_SVOLibrary_GUIA", "Qowaiv_SVOLibrary_GUIA")]
        public void convention_based_deserialization(Uuid expected, object json)
        {
            var actual = JsonTester.Read<Uuid>(json);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(null, "")]
        [TestCase("Qowaiv_SVOLibrary_GUIA", "Qowaiv_SVOLibrary_GUIA")]
        public void convention_based_serialization(object expected, Uuid svo)
        {
            var serialized = JsonTester.Write(svo);
            Assert.AreEqual(expected, serialized);
        }

        [TestCase("Invalid input", typeof(FormatException))]
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

        [Test]
        public void export_to_byte_array_equal_to_GUID_equivalent()
        {
            var bytes = Svo.Uuid.ToByteArray();
            Assert.AreEqual(((Guid)Svo.Uuid).ToByteArray(), bytes);
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

