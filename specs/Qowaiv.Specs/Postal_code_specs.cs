namespace Postal_code_specs;

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
    [TestCase("2624DP", "en")]
    [TestCase("12345", null)]
    public void strings_representing_SVO(string input, CultureInfo culture)
    {
        Assert.IsTrue(PostalCode.IsValid(input, culture));
    }

    [TestCaseSource(nameof(ValidForCountry))]
    public void country(Country country, PostalCode postalCode)
    {
        Assert.IsTrue(postalCode.IsValid(country));
    }
    private static readonly IEnumerable<object[]> ValidForCountry = PostalCodes.Valid
        .SelectMany(codes => codes.ToArrays());

    [Test]
    public void multiple_countries()
    {
        var postalCode = PostalCode.Parse("666");
        CollectionAssert.AreEqual(new[]
        {
                Country.AD,
                Country.BH,
                Country.BT,
                Country.FO,
                Country.IS,
                Country.LS,
                Country.MG,
                Country.OM,
                Country.PG,
            },
        postalCode.IsValidFor());
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
    public void from_invalid_as_null_with_TryParse()
        => PostalCode.TryParse("invalid input").Should().BeNull();

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
        var formatted = Svo.PostalCode.ToString("SomeFormat", FormatProvider.CustomFormatter);
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

    [TestCaseSource(nameof(FormattedPostalCodes))]
    public void per_country(Country country, PostalCode svo, string expected)
    {
        Assert.AreEqual(expected, svo.ToString(country));
    }

    [Test]
    public void for_58_countries()
    {
        Assert.AreEqual(58, PostalCodeCountryInfo.GetCountriesWithFormatting().Count());
        CollectionAssert.AreEqual(
            PostalCodeCountryInfo.GetCountriesWithFormatting(),
            FormattedPostalCodes.Select(record => record[0]));
    }

    [Test]
    public void with_current_thread_culture_as_default()
    {
        using (new CultureInfoScope(culture: TestCultures.Nl_NL, cultureUI: TestCultures.En_GB))
        {
            Assert.AreEqual("H0H0H0", Svo.PostalCode.ToString(provider: null));
        }
    }

    private readonly static IEnumerable<object[]> FormattedPostalCodes = new[]
    {
            new object[] { Country.AD, PostalCode.Parse("765"), "AD-765" },
            new object[] { Country.AI, PostalCode.Parse("2640"), "AI-2640" },
            new object[] { Country.AR, PostalCode.Parse("Z1230ABC"), "Z 1230 ABC" },
            new object[] { Country.AS, PostalCode.Parse("987654321"), "98765 4321" },
            new object[] { Country.AX, PostalCode.Parse("22123"), "22-123" },
            new object[] { Country.AZ, PostalCode.Parse("2123"), "AZ-2123" },
            new object[] { Country.BB, PostalCode.Parse("01234"), "BB-01234" },
            new object[] { Country.BM, PostalCode.Parse("AB34"), "AB 34" },
            new object[] { Country.BR, PostalCode.Parse("01012345"), "01012-345" },
            new object[] { Country.CA, PostalCode.Parse("H0H0H0"), "H0H 0H0" },
            new object[] { Country.CL, PostalCode.Parse("1234567"), "123-4567" },
            new object[] { Country.CU, PostalCode.Parse("12345"), "CP12345" },
            new object[] { Country.CZ, PostalCode.Parse("12345"), "123 45" },
            new object[] { Country.DK, PostalCode.Parse("1234"), "DK-1234" },
            new object[] { Country.FI, PostalCode.Parse("12345"), "12-345" },
            new object[] { Country.FK, PostalCode.Parse("FIQQ1ZZ"), "FIQQ 1ZZ" },
            new object[] { Country.FM, PostalCode.Parse("969414321"), "96941-4321" },
            new object[] { Country.FO, PostalCode.Parse("123"), "FO-123" },
            new object[] { Country.GA, PostalCode.Parse("1234"), "12 34" },
            new object[] { Country.GB, PostalCode.Parse("EC1A1BB"), "EC1A 1BB" },
            new object[] { Country.GG, PostalCode.Parse("999JS"), "GY99 9JS" },
            new object[] { Country.GI, PostalCode.Parse("GX111AA"), "GX11 1AA" },
            new object[] { Country.GL, PostalCode.Parse("3977"), "GL-3977" },
            new object[] { Country.GR, PostalCode.Parse("12345"), "123 45" },
            new object[] { Country.GS, PostalCode.Parse("SIQQ1ZZ"), "SIQQ 1ZZ" },
            new object[] { Country.GU, PostalCode.Parse("969329999"), "96932-9999" },
            new object[] { Country.IM, PostalCode.Parse("00DF"), "IM0 0DF" },
            new object[] { Country.IO, PostalCode.Parse("BBND1ZZ"), "BBND 1ZZ" },
            new object[] { Country.IR, PostalCode.Parse("0123456789"), "01234-56789" },
            new object[] { Country.JE, PostalCode.Parse("999MO"), "JE99 9MO" },
            new object[] { Country.JP, PostalCode.Parse("1234567"), "1234-567" },
            new object[] { Country.KR, PostalCode.Parse("123456"), "123-456" },
            new object[] { Country.KY, PostalCode.Parse("12345"), "KY1-2345" },
            new object[] { Country.LB, PostalCode.Parse("12345678"), "1234 5678" },
            new object[] { Country.LT, PostalCode.Parse("12345"), "LT-12345" },
            new object[] { Country.LV, PostalCode.Parse("1234"), "LV-1234" },
            new object[] { Country.MA, PostalCode.Parse("12345"), "12 345" },
            new object[] { Country.MD, PostalCode.Parse("1234"), "MD-1234" },
            new object[] { Country.MH, PostalCode.Parse("969701234"), "96970-1234" },
            new object[] { Country.MP, PostalCode.Parse("969501234"), "96950-1234" },
            new object[] { Country.MT, PostalCode.Parse("ABC1234"), "ABC 1234" },
            new object[] { Country.NL, PostalCode.Parse("2624DP"), "2624 DP" },
            new object[] { Country.PL, PostalCode.Parse("12345"), "12-345" },
            new object[] { Country.PN, PostalCode.Parse("PCRN1ZZ"), "PCRN 1ZZ" },
            new object[] { Country.PT, PostalCode.Parse("1234567"), "1234 567" },
            new object[] { Country.PW, PostalCode.Parse("969401234"), "96940-1234" },
            new object[] { Country.SA, PostalCode.Parse("123456789"), "12345-6789" },
            new object[] { Country.SE, PostalCode.Parse("12345"), "123 45" },
            new object[] { Country.SH, PostalCode.Parse("STHL1ZZ"), "STHL 1ZZ" },
            new object[] { Country.SI, PostalCode.Parse("1234"), "SI-1234" },
            new object[] { Country.SK, PostalCode.Parse("12345"), "123 45" },
            new object[] { Country.SN, PostalCode.Parse("12345"), "CP12345" },
            new object[] { Country.TC, PostalCode.Parse("TKCA1ZZ"), "TKCA 1ZZ" },
            new object[] { Country.US, PostalCode.Parse("123456789"), "12345-6789" },
            new object[] { Country.VC, PostalCode.Parse("1234"), "VC1234" },
            new object[] { Country.VE, PostalCode.Parse("1234X"), "1234-X" },
            new object[] { Country.VG, PostalCode.Parse("1169"), "VG1169" },
            new object[] { Country.VI, PostalCode.Parse("008501234"), "00850-1234" },
        };
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
        var sorted = new[]
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

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(PostalCode).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From<string>(null).To<PostalCode>().Should().Be(default);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From(string.Empty).To<PostalCode>().Should().Be(default);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From("H0H0H0").To<PostalCode>().Should().Be(Svo.PostalCode);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.ToString().From(Svo.PostalCode).Should().Be("H0H0H0");
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
        var xml = Serialize.Xml(Svo.PostalCode);
        Assert.AreEqual("H0H0H0", xml);
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<PostalCode>("H0H0H0");
        Assert.AreEqual(Svo.PostalCode, svo);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.PostalCode);
        Assert.AreEqual(Svo.PostalCode, round_tripped);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.PostalCode);
        var round_tripped = SerializeDeserialize.Xml(structure);
        Assert.AreEqual(structure, round_tripped);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.PostalCode;
        Assert.IsNull(obj.GetSchema());
    }
}

public class Is_Open_API_data_type
{
    internal static readonly OpenApiDataTypeAttribute Attribute = OpenApiDataTypeAttribute.From(typeof(PostalCode)).FirstOrDefault();

    [Test]
    public void with_description()
    {
        Assert.AreEqual(
            "Postal code notation.",
            Attribute.Description);
    }

    [Test]
    public void has_type()
    {
        Assert.AreEqual("string", Attribute.Type);
    }

    [Test]
    public void has_format()
    {
        Assert.AreEqual("postal-code", Attribute.Format);
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
        var round_tripped = SerializeDeserialize.Binary(Svo.PostalCode);
        Assert.AreEqual(Svo.PostalCode, round_tripped);
    }

    [Test]
    public void storing_string_in_SerializationInfo()
    {
        var info = Serialize.GetInfo(Svo.PostalCode);
        Assert.AreEqual("H0H0H0", info.GetString("Value"));
    }
}

public class Not_supported_by
{
    [Test]
    public void _77_countries()
    {
        var exp = new[] { Country.AE, Country.AG, Country.AO, Country.AQ, Country.AW, Country.BF, Country.BI, Country.BJ, Country.BQ, Country.BS, Country.BV, Country.BW, Country.BZ, Country.CD, Country.CF, Country.CG, Country.CI, Country.CK, Country.CM, Country.CW, Country.DJ, Country.DM, Country.DO, Country.EH, Country.ER, Country.FJ, Country.GD, Country.GH, Country.GM, Country.GN, Country.GQ, Country.GY, Country.HK, Country.IE, Country.JM, Country.KE, Country.KI, Country.KM, Country.KN, Country.KP, Country.KW, Country.LC, Country.ML, Country.MO, Country.MR, Country.MS, Country.MU, Country.MV, Country.MW, Country.NR, Country.NU, Country.QA, Country.RW, Country.SB, Country.SC, Country.SJ, Country.SL, Country.SO, Country.SR, Country.SS, Country.ST, Country.SX, Country.SY, Country.TF, Country.TG, Country.TK, Country.TL, Country.TO, Country.TV, Country.TZ, Country.UG, Country.UM, Country.UZ, Country.VU, Country.WS, Country.YE, Country.ZW };
        var act = PostalCodeCountryInfo.GetCountriesWithoutPostalCode().ToArray();

        foreach (var item in act)
        {
            Console.WriteLine(item);
        }
        CollectionAssert.AreEqual(exp, act);
    }
}
public class For_10_countries
{
    [Test]
    public void only_1_postal_code_exists()
    {
        var exp = new Dictionary<Country, string>
            {
                { Country.AI, "AI-2640" },
                { Country.FK, "FIQQ 1ZZ" },
                { Country.GI, "GX11 1AA" },
                { Country.GS, "SIQQ 1ZZ" },
                { Country.IO, "BBND 1ZZ" },
                { Country.PN, "PCRN 1ZZ" },
                { Country.SH, "STHL 1ZZ" },
                { Country.SV, "01101" },
                { Country.TC, "TKCA 1ZZ" },
                { Country.VA, "00120" },

            };
        var act = PostalCodeCountryInfo.GetCountriesWithSingleValue().ToArray();

        foreach (var item in act)
        {
            Console.WriteLine(item);
        }

        Assert.AreEqual(exp.Keys.Count, act.Length, "act.Length");

        CollectionAssert.AreEqual(exp.Keys, act);

        foreach (var kvp in exp)
        {
            var info = PostalCodeCountryInfo.GetInstance(kvp.Key);

            Assert.AreEqual(kvp.Value, info.GetSingleValue(), "GetSingleValue(), {0}.", kvp.Key);
        }
    }
}

public class Debugger
{
    [TestCase("{empty}", "")]
    [TestCase("{unknown}", "?")]
    [TestCase("H0H0H0", "H0H0H0")]
    public void has_custom_display(object display, PostalCode svo)
        => svo.Should().HaveDebuggerDisplay(display);
}

internal class PostalCodes
{
    public PostalCodes(Country country, params string[] values)
    {
        Country = country;
        Values = values.Select(v => PostalCode.Parse(v)).ToArray();
    }

    public Country Country { get; }
    public PostalCode[] Values { get; }

    public IEnumerable<object[]> ToArrays() => Values.Select(value => new object[] { Country, value });

    public static readonly PostalCodes[] Valid = new[]
{
            new PostalCodes(Country.AD, "AD123", "AD345", "AD678", "AD789"),
            new PostalCodes(Country.AF, "4301", "1001", "2023", "1102", "4020", "3077", "2650", "4241"),
            new PostalCodes(Country.AI, "2640", "AI-2640", "AI2640", "ai-2640", "ai2640", "ai 2640", "ai.2640"),
            new PostalCodes(Country.AL, "1872", "2540", "7900", "9999"),
            new PostalCodes(Country.AM, "0123", "1234", "2000", "3248", "4945"),
            new PostalCodes(Country.AR, "A4400XXX", "C 1420 ABC", "S 2300DDD", "Z9400 QOW"),
            new PostalCodes(Country.AS, "91000-0060", "91000-9996", "90126", "92345"),
            new PostalCodes(Country.AT, "2471", "1000", "5120", "9999"),
            new PostalCodes(Country.AU, "0872", "2540", "0900", "9999"),
            new PostalCodes(Country.AX, "22-000", "22-123", "22000", "22345"),
            new PostalCodes(Country.AZ, "1499", "az 1499", "AZ-1499", "az1499", "AZ0499", "AZ0099", "aZ6990"),
            new PostalCodes(Country.BA, "00000", "01235", "12346", "20004", "32648", "40945", "56640", "62908", "76345", "67552", "87182", "99999"),
            new PostalCodes(Country.BB, "21499", "01499", "bB-31499", "BB 01499", "bb81499", "BB71499", "BB56990"),
            new PostalCodes(Country.BD, "0483", "1480", "5492", "7695", "9796"),
            new PostalCodes(Country.BE, "2471", "1000", "5120", "9999"),
            new PostalCodes(Country.BG, "1000", "2077", "2650", "4241"),
            new PostalCodes(Country.BH, "199", "1299", "666", "890", "768", "1000", "1176"),
            new PostalCodes(Country.BL, "97700", "97701", "97712", "97720", "97732", "97740", "97756", "97762", "97776", "97767", "97787", "97799"),
            new PostalCodes(Country.BM, "AA", "AS", "BJ", "CD", "DE", "EO", "FN", "GF", "HL", "ID", "JS", "KN", "LO", "ME", "NN", "OL", "PS", "QD", "RN", "SE", "TM", "UF", "VE", "WL", "XM", "YE", "ZL", "ZZ", "AA0F", "AS0S", "BJ1F", "CD2K", "DE3D", "EO4J", "FN5F", "GF6S", "HL7D", "ID69", "JS66", "KN48", "LO12", "MEDS", "NNRF", "OLWK", "PSSD", "QDPJ", "RNKF", "SELS", "TMD1", "UFO7", "VEF2", "WLS9", "XMF0", "YES4", "ZLF2", "ZZK7"),
            new PostalCodes(Country.BN, "YZ0000", "BU2529", "bU2529", "bu2529", "Bu2529"),
            new PostalCodes(Country.BO, "0123", "1234", "2000", "3248", "4945", "5640", "6208", "7645", "6752", "8782", "9999"),
            new PostalCodes(Country.BR, "01000-000", "01000999", "88000-123"),
            new PostalCodes(Country.BT, "000", "012", "123", "200", "326", "409", "566", "629", "763", "675", "871", "999"),
            new PostalCodes(Country.BY, "010185", "110000", "342600", "610185", "910185"),
            new PostalCodes(Country.CA, "H0H-0H0", "K8 N5W 6", "A1A 1A1", "K0H 9Z0", "T1R 9Z0", "P2V9z0"),
            new PostalCodes(Country.CC, "0123", "1234", "2000", "3248", "4945", "5640", "6208", "7645", "6752", "8782", "9999"),
            new PostalCodes(Country.CH, "1001", "8023", "9100", "1000", "2077", "2650", "4241"),
            new PostalCodes(Country.CL, "0000000", "0231145", "1342456", "2000974", "3642438", "4940375", "5646230", "6902168", "7346345", "6557682", "8187992", "9999999"),
            new PostalCodes(Country.CN, "010000", "342600", "810185"),
            new PostalCodes(Country.CO, "000000", "023145", "134256", "200074", "364238", "494075", "564630", "690268", "734645", "655782", "818792", "999999"),
            new PostalCodes(Country.CR, "00000", "01235", "12346", "20004", "32648", "40945", "56640", "62908", "76345", "67552", "87182", "99999"),
            new PostalCodes(Country.CU, "00000", "01235", "12346", "20004", "32648", "40945", "56640", "62908", "76345", "67552", "87182", "99999", "CP00000", "CP01235", "CP12346", "CP20004", "CP32648"),
            new PostalCodes(Country.CV, "0000", "1000", "2077", "2650", "4241"),
            new PostalCodes(Country.CX, "0000", "0144", "1282", "2037", "3243", "4008", "5697", "6282", "7611", "6767", "8752", "9999"),
            new PostalCodes(Country.CY, "1000", "2077", "2650", "4241"),
            new PostalCodes(Country.CZ, "21234", "12345", "11111", "123 45"),
            new PostalCodes(Country.DE, "10000", "01123", "89000", "12345"),
            new PostalCodes(Country.DK, "1499", "dk-1499", "DK-1499", "dk1499", "DK1499", "DK6990"),
            new PostalCodes(Country.DZ, "01234", "12345", "11111"),
            new PostalCodes(Country.EC, "000000", "023145", "134256", "200074", "364238", "494075", "564630", "690268", "734645", "655782", "818792", "999999"),
            new PostalCodes(Country.EE, "00000", "01235", "12346", "20004", "32648", "40945", "56640", "62908", "76345", "67552", "87182", "99999"),
            new PostalCodes(Country.EG, "12346", "20004", "32648", "40945", "56640", "62908", "76345", "67552", "87182", "99999"),
            new PostalCodes(Country.ES, "01070", "10070", "20767", "26560", "32451", "09112", "48221", "50636", "52636", "51050"),
            new PostalCodes(Country.ET, "0123", "1234", "2000", "3248", "4945", "5640", "6208", "7645", "6752", "8782", "9999"),
            new PostalCodes(Country.FI, "00-000", "01-123", "00000", "12345"),
            new PostalCodes(Country.FK, "FIQQ1ZZ"),
            new PostalCodes(Country.FM, "96941", "96942", "96943", "96944", "969410000", "969420123", "969430144", "969441282"),
            new PostalCodes(Country.FO, "399", "fo-399", "FO-199", "fO399", "FO678", "FO123"),
            new PostalCodes(Country.FR, "10000", "01123", "89000", "12345"),
            new PostalCodes(Country.GA, "0123", "1234", "2000", "3248", "4945", "5640", "6208", "7645", "6752", "8782", "9999"),
            new PostalCodes(Country.GB, "M11AA", "M11aA", "M11AA", "m11AA", "m11aa", "B338TH", "B338TH", "CR26XH", "CR26XH", "DN551PT", "DN551PT", "W1A1HQ", "W1A1HQ", "EC1A1BB", "EC1A1BB"),
            new PostalCodes(Country.GE, "0123", "1234", "2000", "3248", "4945", "5640", "6208", "7645", "6752", "8782", "9999"),
            new PostalCodes(Country.GF, "97300", "97301", "97312", "97320", "97332", "97340", "97356", "97362", "97376", "97367", "97387", "97399"),
            new PostalCodes(Country.GG, "00DF", "03DS", "14RF", "20WK", "34SD", "44PJ", "54KF", "60LS", "74JD", "65MO", "88DF", "99JS", "000DF", "015DS", "126RF", "204WK", "328SD", "405PJ", "560KF", "628LS", "765JD", "672MO", "872DF", "999JS", "GY999JS"),
            new PostalCodes(Country.GI, "GX111AA"),
            new PostalCodes(Country.GL, "3999", "gl-3999", "GL-3999", "gL 3999", "GL3999", "GL3990"),
            new PostalCodes(Country.GP, "97100", "97101", "97112", "97120", "97132", "97140", "97156", "97162", "97176", "97167", "97187", "97199"),
            new PostalCodes(Country.GR, "10000", "31123", "89000", "12345"),
            new PostalCodes(Country.GS, "SIQQ1ZZ"),
            new PostalCodes(Country.GT, "00000", "01235", "12346", "20004", "32648", "40945", "56640", "62908", "76345", "67552", "87182", "99999"),
            new PostalCodes(Country.GU, "96910", "96910", "96911", "96912", "96923", "96924", "96925", "96926", "96927", "96926", "96931", "96932", "969100000", "969103015", "969114126", "969120204", "969234328", "969244405", "969254560", "969260628", "969274765", "969265672", "969318872", "969329999"),
            new PostalCodes(Country.GW, "0123", "1234", "2000", "3248", "4945", "5640", "6208", "7645", "6752", "8782", "9999"),
            new PostalCodes(Country.HM, "0123", "1234", "2000", "3248", "4945", "5640", "6208", "7645", "6752", "8782", "9999"),
            new PostalCodes(Country.HN, "00000", "01235", "12346", "20004", "32648", "40945", "56640", "62908", "76345", "67552", "87182", "99999"),
            new PostalCodes(Country.HR, "00000", "01235", "12346", "20004", "32648", "40945", "56640", "62908", "76345", "67552", "87182", "99999"),
            new PostalCodes(Country.HT, "0123", "1234", "2000", "3248", "4945", "5640", "6208", "7645", "6752", "8782", "9999"),
            new PostalCodes(Country.HU, "1000", "2077", "2650", "4241"),
            new PostalCodes(Country.ID, "10000", "31123", "89000", "89007", "12340"),
            new PostalCodes(Country.IL, "0110023", "1084023", "3108701", "4201907", "5403506", "6177008"),
            new PostalCodes(Country.IM, "00DF", "04DS", "18RF", "23WK", "34SD", "40PJ", "59KF", "68LS", "71JD", "66MO", "85DF", "99JS", "00DF", "000DF", "014DS", "128RF", "203WK", "324SD", "400PJ", "569KF", "628LS", "761JD", "676MO", "875DF", "999JS", "000DF", "IM00DF", "IM04DS", "IM18RF", "IM23WK", "IM34SD", "IM40PJ", "IM59KF", "IM68LS", "IM71JD", "IM66MO", "IM85DF", "IM99JS", "IM00DF", "IM000DF", "IM014DS", "IM128RF", "IM203WK", "IM324SD", "IM400PJ", "IM569KF", "IM628LS", "IM761JD", "IM676MO", "IM875DF", "IM999JS", "IM000DF", "IM00DF", "IM04DS", "IM18RF", "IM23WK", "IM34SD", "IM40PJ", "IM59KF", "IM68LS", "IM71JD", "IM66MO", "IM85DF", "IM99JS", "IM00DF"),
            new PostalCodes(Country.IN, "110000", "342600", "810185", "810 185"),
            new PostalCodes(Country.IO, "BBND1ZZ"),
            new PostalCodes(Country.IQ, "12346", "32648", "40945", "56640", "62908"),
            new PostalCodes(Country.IR, "0000000000", "0144942325", "1282353436", "2037570044", "3243436478", "4008279475", "5697836450", "6282469088", "7611343495", "6767185502", "8752391832", "9999999999"),
            new PostalCodes(Country.IS, "000", "035", "146", "204", "348", "445", "540", "608", "745", "652", "882", "999"),
            new PostalCodes(Country.IT, "00123", "02123", "31001", "42007", "54006", "91008"),
            new PostalCodes(Country.JE, "00AS", "25GS", "36DF", "44DS", "78RF", "75WK", "50SD", "88PJ", "95KF", "02LS", "32JD", "99MO", "00AS", "042GS", "153DF", "274DS", "337RF", "477WK", "535SD", "668PJ", "749KF", "680LS", "893JD", "999MO", "JE999MO"),
            new PostalCodes(Country.JO, "00000", "01235", "12346", "20004", "32648", "40945", "56640", "62908", "76345", "67552", "87182", "99999"),
            new PostalCodes(Country.JP, "000-0000", "000-0999", "010-0000", "0100999", "880-0123", "900-0123"),
            new PostalCodes(Country.KG, "000000", "023145", "134256", "200074", "364238", "494075", "564630", "690268", "734645", "655782", "818792", "999999"),
            new PostalCodes(Country.KH, "00000", "01235", "12346", "20004", "32648", "40945", "56640", "62908", "76345", "67552", "87182", "99999"),
            new PostalCodes(Country.KR, "110000", "342600", "610185", "410-185", "710-185"),
            new PostalCodes(Country.KY, "00000", "01235", "12346", "20004", "32648", "40945", "56640", "62908", "76345", "67552", "87182", "99999", "KY99999"),
            new PostalCodes(Country.KZ, "000000", "023145", "134256", "200074", "364238", "494075", "564630", "690268", "734645", "655782", "818792"),
            new PostalCodes(Country.LA, "00000", "01235", "12346", "20004", "32648", "40945", "56640", "62908", "76345", "67552", "87182", "99999"),
            new PostalCodes(Country.LB, "00000000", "01442325", "12853436", "20370044", "32436478", "40079475", "56936450", "62869088", "76143495", "67685502", "87591832", "99999999"),
            new PostalCodes(Country.LI, "9485", "9489", "9490", "9498"),
            new PostalCodes(Country.LK, "00000", "10070", "20767", "26560", "32451", "09112", "48221", "54636", "65050", "70162", "81271", "92686"),
            new PostalCodes(Country.LR, "0123", "1234", "2000", "3248", "4945", "5640", "6208", "7645", "6752", "8782", "9999"),
            new PostalCodes(Country.LS, "000", "015", "126", "204", "328", "405", "560", "628", "765", "672", "872", "999"),
            new PostalCodes(Country.LT, "21499", "01499", "lT-31499", "LT-01499", "lt81499", "LT71499", "LT56990"),
            new PostalCodes(Country.LU, "0123", "1234", "2000", "3248", "4945", "5640", "6208", "7645", "6752", "8782", "9999"),
            new PostalCodes(Country.LV, "0123", "1234", "2000", "3248", "4945", "5640", "6208", "7645", "6752", "8782", "9999"),
            new PostalCodes(Country.LY, "00000", "01235", "12346", "20004", "32648", "40945", "56640", "62908", "76345", "67552", "87182", "99999"),
            new PostalCodes(Country.MA, "11 302", "24 023", "45001", "89607", "86096", "85808"),
            new PostalCodes(Country.MC, "MC-98000", "MC-98012", "MC 98023", "mc98089", "MC98099", "Mc98077", "mC98066", "98089", "98099", "98077", "98066"),
            new PostalCodes(Country.MD, "1499", "md-1499", "MD-1499", "md1499", "MD0499", "MD0099", "mD6990", "0123", "1234", "2000", "3248", "4945", "5640", "6208", "7645", "6752", "8782", "9999"),
            new PostalCodes(Country.ME, "81302", "84023", "85001", "81607", "84096", "85808"),
            new PostalCodes(Country.MF, "97800", "97805", "97816", "97824", "97838", "97845", "97850", "97868", "97875", "97862", "97882", "97899"),
            new PostalCodes(Country.MG, "000", "015", "126", "204", "328", "405", "560", "628", "765", "672", "872", "999"),
            new PostalCodes(Country.MH, "96960", "96960", "96961", "96962", "96963", "96964", "96965", "96976", "96977", "96976", "96978", "96979", "96970", "969600000", "969604423", "969612534", "969627700", "969633364", "969648794", "969657364", "969762690", "969771434", "969767855", "969782918", "969799999", "969700000"),
            new PostalCodes(Country.MK, "0123", "1234", "2000", "3248", "4945", "5640", "6208", "7645", "6752", "8782", "9999"),
            new PostalCodes(Country.MM, "00000", "01235", "12346", "20004", "32648", "40945", "56640", "62908", "76345", "67552", "87182", "99999"),
            new PostalCodes(Country.MN, "00000", "01235", "12346", "20004", "32648", "40945", "56640", "62908", "76345", "67552", "87182", "99999"),
            new PostalCodes(Country.MP, "96950", "96951", "96952", "969500000", "969500143", "969501254", "969502070", "969513234", "969514074", "969515634", "969516260", "969527644", "969526785", "969528798", "969529999"),
            new PostalCodes(Country.MQ, "97200", "97201", "97212", "97220", "97232", "97240", "97256", "97262", "97276", "97267", "97287", "97299"),
            new PostalCodes(Country.MT, "AAA0000", "ASD0132", "BJR1243", "CDW2004", "DES3247", "EOP4047", "FNK5645", "GFL6208", "HLJ7649", "IDM6750", "JSD8783", "KNJ9999", "LOD0000", "MED0132", "NNR1243", "OLW2004", "PSS3247", "QDP4047", "RNK5645", "SEL6208", "TMJ7649", "UFM6750", "VED8783", "WLJ9999", "XMD0000", "YED0132", "ZLR1243", "ZZZ9999"),
            new PostalCodes(Country.MX, "09302", "10023", "31001", "42007", "54006", "61008"),
            new PostalCodes(Country.MY, "10023", "31001", "42007", "54006", "61008"),
            new PostalCodes(Country.MZ, "0123", "1234", "2000", "3248", "4945", "5640", "6208", "7645", "6752", "8782", "9999"),
            new PostalCodes(Country.NA, "90000", "90015", "90126", "90204", "91328", "91405", "91560", "91628", "92765", "92672", "92872", "92999"),
            new PostalCodes(Country.NC, "98800", "98802", "98813", "98820", "98836", "98884", "98895", "98896", "98897", "98896", "98898", "98899"),
            new PostalCodes(Country.NE, "0123", "1234", "2000", "3248", "4945", "5640", "6208", "7645", "6752", "8782", "9999"),
            new PostalCodes(Country.NF, "0123", "1234", "2000", "3248", "4945", "5640", "6208", "7645", "6752", "8782", "9999"),
            new PostalCodes(Country.NG, "009999", "018010", "110000", "342600", "810185", "810185"),
            new PostalCodes(Country.NI, "00000", "01235", "12346", "20004", "32648", "40945", "56640", "62908", "76345", "67552", "87182", "99999"),
            new PostalCodes(Country.NL, "1236RF", "2044WK", "4075PJ", "5650KF", "6288LS", "7695JD", "6702MO", "8732DF", "9999JS", "2331 PS"),
            new PostalCodes(Country.NO, "0912", "0821", "0666", "0000", "1000", "2077", "2650", "4241"),
            new PostalCodes(Country.NP, "00000", "01235", "12346", "20004", "32648", "40945", "56640", "62908", "76345", "67552", "87182", "99999"),
            new PostalCodes(Country.NZ, "0912", "0821", "0666", "0000", "1000", "2077", "2650", "4241"),
            new PostalCodes(Country.OM, "000", "015", "126", "204", "328", "405", "560", "628", "765", "672", "872", "999"),
            new PostalCodes(Country.PA, "000000", "023145", "134256", "200074", "364238", "494075", "564630", "690268", "734645", "655782", "818792", "999999"),
            new PostalCodes(Country.PE, "00000", "01235", "12346", "20004", "32648", "40945", "56640", "62908", "76345", "67552", "87182", "99999"),
            new PostalCodes(Country.PF, "98700", "98725", "98736", "98704", "98768", "98795", "98760", "98798", "98735", "98752", "98712", "98799"),
            new PostalCodes(Country.PG, "000", "015", "126", "204", "328", "405", "560", "628", "765", "672", "872", "999"),
            new PostalCodes(Country.PH, "0123", "1234", "2000", "3248", "4945", "5640", "6208", "7645", "6752", "8782", "9999"),
            new PostalCodes(Country.PK, "11302", "24023", "45001", "89607", "86096", "85808"),
            new PostalCodes(Country.PL, "01302", "11302", "24023", "45001", "89607", "86096", "85808", "06-096", "85-808"),
            new PostalCodes(Country.PM, "97500"),
            new PostalCodes(Country.PN, "PCRN1ZZ"),
            new PostalCodes(Country.PR, "01302", "00802", "11302", "24023", "45001", "89607", "86096", "85808", "06096", "85808"),
            new PostalCodes(Country.PS, "00000", "01235", "12346", "20004", "32648", "40945", "56640", "62908", "76345", "67552", "87182", "99999"),
            new PostalCodes(Country.PT, "1282353", "2037570", "3243436", "4008279", "5697836", "6282469", "7611343", "6767185", "8752391", "9999999"),
            new PostalCodes(Country.PW, "96940"),
            new PostalCodes(Country.PY, "0123", "1234", "2000", "3248", "4945", "5640", "6208", "7645", "6752", "8782", "9999"),
            new PostalCodes(Country.RE, "97400", "97402", "97413", "97420", "97436", "97449", "97456", "97469", "97473", "97465", "97481", "97499"),
            new PostalCodes(Country.RO, "018010", "110000", "342600", "810185", "810185"),
            new PostalCodes(Country.RS, "10070", "20767", "26560", "32451"),
            new PostalCodes(Country.RU, "110000", "342600", "610185", "410185"),
            new PostalCodes(Country.SA, "00000", "03145", "14256", "20074", "34238", "44075", "54630", "60268", "74645", "65782", "88792", "99999", "000000000", "031452003", "142563114", "200740220", "342386334", "440759444", "546306554", "602689660", "746453774", "657825665", "887921888", "999999999"),
            new PostalCodes(Country.SD, "00000", "03145", "14256", "20074", "34238", "44075", "54630", "60268", "74645", "65782", "88792", "99999"),
            new PostalCodes(Country.SE, "10000", "10070", "20767", "86560", "32451", "99112", "482 21", "546 36", "650 50", "701 62", "812 71", "926 86"),
            new PostalCodes(Country.SG, "11000", "34600", "61185", "41185", "00999", "01010", "71185", "81185", "91185"),
            new PostalCodes(Country.SH, "STHL1ZZ"),
            new PostalCodes(Country.SI, "0123", "1234", "2000", "3248", "4945", "5640", "6208", "7645", "6752", "8782", "9999"),
            new PostalCodes(Country.SK, "10070", "20767", "26560", "32451", "09112", "48221", "546 36", "650 50", "701 62", "812 71", "926 86"),
            new PostalCodes(Country.SM, "47890", "47891", "47892", "47895", "47899"),
            new PostalCodes(Country.SN, "00000", "01235", "12346", "20004", "32648", "40945", "56640", "62908", "76345", "67552", "87182", "99999"),
            new PostalCodes(Country.SV, "01101"),
            new PostalCodes(Country.SZ, "H761", "L000", "M014", "S628", "H611", "L760", "M754", "S998", "H000", "L023", "M182", "S282"),
            new PostalCodes(Country.TC, "TKCA1ZZ"),
            new PostalCodes(Country.TD, "00000", "01235", "12346", "20004", "32648", "40945", "56640", "62908", "76345", "67552", "87182", "99999"),
            new PostalCodes(Country.TH, "10023", "31001", "42007", "54006", "61008"),
            new PostalCodes(Country.TJ, "000000", "023145", "134256", "200074", "364238", "494075", "564630", "690268", "734645", "655782", "818792", "999999"),
            new PostalCodes(Country.TM, "000000", "023145", "134256", "200074", "364238", "494075", "564630", "690268", "734645", "655782", "818792", "999999"),
            new PostalCodes(Country.TN, "0123", "1234", "2000", "3248", "4945", "5640", "6208", "7645", "6752", "8782", "9999"),
            new PostalCodes(Country.TR, "01302", "08302", "10023", "31001", "42007", "74006", "91008"),
            new PostalCodes(Country.TT, "000000", "023145", "134256", "200074", "364238", "494075", "564630", "690268", "734645", "655782", "818792", "999999"),
            new PostalCodes(Country.TW, "10023", "31001", "42007", "54006", "61008", "91008"),
            new PostalCodes(Country.UA, "01235", "12346", "20004", "32648", "40945", "56640", "62908", "76345", "67552", "87182", "99999"),
            new PostalCodes(Country.US, "01000-0060", "11000-9996", "00126", "12345"),
            new PostalCodes(Country.UY, "00000", "01235", "12346", "20004", "32648", "40945", "56640", "62908", "76345", "67552", "87182", "99999"),
            new PostalCodes(Country.VA, "00120"),
            new PostalCodes(Country.VC, "0123", "1234", "2000", "3248", "4945", "5640", "6208", "7645", "6752", "8782", "9999"),
            new PostalCodes(Country.VE, "0000", "0123", "1234", "2000", "3264", "4094", "5664", "6290", "7634", "6755", "8718", "9999", "0000A", "0325A", "1436B", "2044C", "3478D", "4475E", "5450F", "6088G", "7495H", "6502I", "8832J", "9999K", "0000L", "0325M", "1436N", "2044O", "3478P", "4475Q", "5450R", "6088S", "7495T", "6502U", "8832V", "9999W", "0000X", "0325Y", "1436Z", "2044Z"),
            new PostalCodes(Country.VG, "1103", "1114", "1120", "1138", "1145", "1150", "1168", "1135", "1162", "VG1101", "VG1112", "VG1120", "VG1132", "VG1149", "VG1156", "VG1162", "VG1106", "VG1167"),
            new PostalCodes(Country.VI, "00815", "00826", "00837", "00846", "00858", "008152346", "008260004", "008372648", "008460945", "008586640"),
            new PostalCodes(Country.VN, "000000", "023145", "134256", "200074", "364238", "494075", "564630", "690268", "734645", "655782", "818792", "999999"),
            new PostalCodes(Country.WF, "98600", "98617", "98699"),
            new PostalCodes(Country.YT, "97600", "97605", "97616", "97624", "97638", "97645", "97650", "97668", "97675", "97662", "97682", "97699"),
            new PostalCodes(Country.ZA, "0001", "0023", "0100", "1000", "2077", "2650", "4241"),
            new PostalCodes(Country.ZM, "00000", "01235", "12346", "20004", "32648", "40945", "56640", "62908", "76345", "67552", "87182", "99999"),
        };
}
