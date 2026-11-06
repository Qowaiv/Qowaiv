namespace Financial.IBAN_specs;

public class Supported
{
    private static readonly Country[] All =
    [
        Country.AD, Country.AE, Country.AL, Country.AO, Country.AT, Country.AZ,
        Country.BA, Country.BE, Country.BF, Country.BG, Country.BH, Country.BI, Country.BJ, Country.BR, Country.BY,
        Country.CF, Country.CG, Country.CH,Country.CI, Country.CM, Country.CR, Country.CV, Country.CY, Country.CZ,
        Country.DE, Country.DJ, Country.DK, Country.DO, Country.DZ,
        Country.EE, Country.EG, Country.ES,
        Country.FI, Country.FK, Country.FO, Country.FR,
        Country.GA, Country.GB, Country.GE, Country.GI, Country.GL, Country.GQ, Country.GR, Country.GT, Country.GW,
        Country.HN, Country.HR, Country.HU,
        Country.IE, Country.IL, Country.IQ, Country.IR, Country.IS, Country.IT,
        Country.JO,
        Country.KM, Country.KW, Country.KZ,
        Country.LB, Country.LC, Country.LI, Country.LT, Country.LU, Country.LV, Country.LY,
        Country.MA, Country.MC, Country.MD, Country.ME, Country.MG, Country.MK, Country.ML,  Country.MN, Country.MR, Country.MT, Country.MU,
        Country.MZ, Country.NE, Country.NI, Country.NL, Country.NO,
        Country.OM,
        Country.PK, Country.PL, Country.PS, Country.PT,
        Country.QA,
        Country.RO, Country.RS, Country.RU,
        Country.SA, Country.SC, Country.SD, Country.SE, Country.SI, Country.SK, Country.SM, Country.SN,Country.SO, Country.ST, Country.SV,
        Country.TD, Country.TG, Country.TL, Country.TN, Country.TR,
        Country.UA, Country.VA, Country.VG,
        Country.XK,
        Country.YE,
    ];

    [Test]
    public void by_111_Countries()
        => InternationalBankAccountNumber.Supported.OrderBy(c => c.IsoAlpha2Code)
        .Should().BeEquivalentTo(All)
        .And.HaveCount(111);
}

public class With_domain_logic
{
    [Test]
    public void has_max_length_of_34() => InternationalBankAccountNumber.MaxLength.Should().Be(34);

    [Test]
    [Arguments("")]
    [Arguments("?")]
    public void has_length_zero_for_empty_and_unknown(InternationalBankAccountNumber svo)
      => svo.Length.Should().Be(0);

    [Test]
    [Arguments(18, "NL20INGB0001234567")]
    public void has_length(int length, InternationalBankAccountNumber svo)
        => svo.Length.Should().Be(length);

    [Test]
    [Arguments(false, "NL20INGB0001234567")]
    [Arguments(false, "?")]
    [Arguments(true, "")]
    public void IsEmpty_returns(bool result, InternationalBankAccountNumber svo)
       => svo.IsEmpty().Should().Be(result);

    [Test]
    [Arguments(false, "NL20INGB0001234567")]
    [Arguments(true, "?")]
    [Arguments(true, "")]
    public void IsEmptyOrUnknown_returns(bool result, InternationalBankAccountNumber svo)
        => svo.IsEmptyOrUnknown().Should().Be(result);

    [Test]
    [Arguments(false, "NL20INGB0001234567")]
    [Arguments(true, "?")]
    [Arguments(false, "")]
    public void IsUnknown_returns(bool result, InternationalBankAccountNumber svo)
        => svo.IsUnknown().Should().Be(result);

    [Test]
    [Arguments(true, "NL20INGB0001234567")]
    [Arguments(true, "?")]
    [Arguments(false, "")]
    public void HasValue_is(bool result, InternationalBankAccountNumber svo) => svo.HasValue.Should().Be(result);

    [Test]
    [Arguments(true, "NL20INGB0001234567")]
    [Arguments(false, "?")]
    [Arguments(false, "")]
    public void IsKnown_is(bool result, InternationalBankAccountNumber svo) => svo.IsKnown.Should().Be(result);

    [Test]
    [Arguments("", "")]
    [Arguments("?", "?")]
    [Arguments("NL", "NL20INGB0001234567")]
    public void with_county(Country country, InternationalBankAccountNumber svo)
        => svo.Country.Should().Be(country);
}

public class Has_constant
{
    [Test]
    public void Empty() => InternationalBankAccountNumber.Empty.Should().Be(default);

    [Test]
    public void Unknown() => InternationalBankAccountNumber.Unknown.Should().Be(InternationalBankAccountNumber.Parse("?"));
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.Iban.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.Iban.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_other_value()
        => Svo.Iban.Equals(InternationalBankAccountNumber.Parse("DE68210501700012345678")).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.Iban.Equals(InternationalBankAccountNumber.Parse("NL20INGB0001234567")).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (Svo.Iban == InternationalBankAccountNumber.Parse("NL20INGB0001234567")).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_other_values()
        => (Svo.Iban == InternationalBankAccountNumber.Parse("DE68210501700012345678")).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (Svo.Iban != InternationalBankAccountNumber.Parse("NL20INGB0001234567")).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_other_values()
        => (Svo.Iban != InternationalBankAccountNumber.Parse("DE68210501700012345678")).Should().BeTrue();

    [Test]
    [Arguments("", 0)]
    [Arguments("NL20INGB0001234567", 684896179)]
    public void hash_code_is_value_based(InternationalBankAccountNumber svo, int hash)
    {
        using (Hash.WithoutRandomizer())
        {
            svo.GetHashCode().Should().Be(hash);
        }
    }
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.Iban.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_InternationalBankAccountNumber_as_object()
    {
        object obj = Svo.Iban;
        Svo.Iban.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_InternationalBankAccountNumber_only()
        => new object().Invoking(Svo.Iban.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        InternationalBankAccountNumber[] sorted =
        [
            default,
            default,
            InternationalBankAccountNumber.Parse("CG39 3001 1000 1010 1345 1300 019"),
            InternationalBankAccountNumber.Parse("CH36 0838 7000 0010 8017 3"),
            InternationalBankAccountNumber.Parse("CI15 QO48 7501 9424 6931 1090 1733"),
            InternationalBankAccountNumber.Unknown,
        ];

        var list = new List<InternationalBankAccountNumber> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }
}

public class Has_custom_formatting
{
    [Test]
    public void _default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Iban.ToString().Should().Be("NL20INGB0001234567");
        }
    }

    [Test]
    public void with_null_format_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Iban.ToString().Should().Be(Svo.Iban.ToString(default(string)));
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Iban.ToString().Should().Be(Svo.Iban.ToString(string.Empty));
        }
    }

    [Test]
    public void default_value_is_represented_as_string_empty()
        => default(InternationalBankAccountNumber).ToString().Should().BeEmpty();

    [Test]
    public void unknown_value_is_represented_as_unknown()
        => InternationalBankAccountNumber.Unknown.ToString().Should().Be("?");

    [Test]
    public void with_empty_format_provider()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Svo.Iban.ToString(FormatProvider.Empty).Should().Be("NL20INGB0001234567");
        }
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.Iban.ToString("F", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: 'NL20 INGB 0001 2345 67', format: 'F'");
    }

    [Test]
    [Arguments(null, "NL20INGB0001234567", "NL20INGB0001234567")]
    [Arguments("f", "NL20INGB0001234567", "nl20 ingb 0001 2345 67")]
    [Arguments("h", "NL20INGB0001234567", "nl20 ingb 0001 2345 67")]
    [Arguments("H", "NL20INGB0001234567", "NL20 INGB 0001 2345 67")]
    [Arguments("u", "NL20INGB0001234567", "nl20ingb0001234567")]
    [Arguments("U", "NL20INGB0001234567", "NL20INGB0001234567")]
    [Arguments("m", "NL20INGB0001234567", "nl20ingb0001234567")]
    [Arguments("M", "NL20INGB0001234567", "NL20INGB0001234567")]
    [Arguments("F", "HR1723600001101234565", /*.......*/ "HR17 2360 0001 1012 3456 5")]
    [Arguments("F", "NL20INGB0001234567", /*..........*/ "NL20 INGB 0001 2345 67")]
    [Arguments("F", "FR1420041010050500013M02606", /*.*/ "FR14 2004 1010 0505 0001 3M02 606")]
    [Arguments("F", "EE382200221020145685", /*........*/ "EE38 2200 2210 2014 5685")]
    [Arguments("F", "", "")]
    [Arguments("F", "?", "?")]
    [Arguments("o", "NL20INGB0001234567", "nl20xxxxxxxxxxx567")]
    [Arguments("O", "NL20INGB0001234567", "NL20XXXXXXXXXXX567")]
    [Arguments("O", "", "")]
    [Arguments("O", "?", "?")]
    public void with_format(string format, InternationalBankAccountNumber svo, string formatted)
        => svo.ToString(format).Should().Be(formatted);

    [Test]
    public void HumanReadable()
        => Svo.Iban.HumanReadable().Should().Be("NL20 INGB 0001 2345 67");
}

public class Can_be_parsed
{
    [Test]
    public void from_null_string_represents_Empty()
        => InternationalBankAccountNumber.Parse(null).Should().Be(InternationalBankAccountNumber.Empty);

    [Test]
    public void from_empty_string_represents_Empty()
        => InternationalBankAccountNumber.Parse(string.Empty).Should().Be(InternationalBankAccountNumber.Empty);

    [Test]
    public void from_question_mark_represents_Unknown()
        => InternationalBankAccountNumber.Parse("?").Should().Be(InternationalBankAccountNumber.Unknown);

    [Test]
    public void from_valid_input_only_otherwise_return_false_on_TryParse()
        => (InternationalBankAccountNumber.TryParse("invalid input", out _)).Should().BeFalse();

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => InternationalBankAccountNumber.TryParse("invalid input").Should().BeNull();

    [Test]
    public void with_TryParse_returns_SVO()
        => InternationalBankAccountNumber.TryParse("NL20INGB0001234567").Should().Be(Svo.Iban);

    [Test]
    [Arguments(' ')]
    [Arguments((char)160)]
    [Arguments('\t')]
    [Arguments('\r')]
    [Arguments('\n')]
    [Arguments('-')]
    [Arguments('_')]
    [Arguments('.')]
    public void ignoring_formatting(char ch)
    {
        var iban = InternationalBankAccountNumber.Parse($"NL{ch}20{ch}INGB00{ch}{ch}0123456{ch}7");
        iban.Should().Be(Svo.Iban);
    }

    [Test]
    [Arguments(' ')]
    [Arguments((char)160)]
    [Arguments('\t')]
    [Arguments('\r')]
    [Arguments('\n')]

    public void trims_input(char ch)
    {
        var iban = InternationalBankAccountNumber.Parse($"{ch}NL20INGB0001234567{ch}");
        iban.Should().Be(Svo.Iban);
    }
}

public class Can_not_be_parsed
{
    [Test]
    [Arguments("N L20INGB0001234567", "with_markup_within_the_country_code")]
    [Arguments("NL2 0INGB0001234567", "with_markup_within_the_checksum_code")]
    public void for_invalid_IBANs(string str, string because)
    {
        InternationalBankAccountNumber.TryParse(str).Should().BeNull(because);
        InternationalBankAccountNumber.TryParse(str, TestCultures.ar).Should().BeNull();
    }

    [Test]
    public void Throws()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Func<InternationalBankAccountNumber> parse = () => InternationalBankAccountNumber.Parse("invalid input");
            parse.Should().Throw<FormatException>()
                .WithMessage("Not a valid IBAN");
        }
    }
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(InternationalBankAccountNumber).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<InternationalBankAccountNumber>().Should().Be(InternationalBankAccountNumber.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<InternationalBankAccountNumber>().Should().Be(InternationalBankAccountNumber.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("NL20 INGB 0001 2345 67").To<InternationalBankAccountNumber>().Should().Be(Svo.Iban);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.Iban).Should().Be("NL20INGB0001234567");
        }
    }
}

public class Supports_JSON_serialization
{
#if NET10_0_OR_GREATER
    [Test]
    [Arguments(null, null)]
    [Arguments("NL20INGB0001234567", "NL20INGB0001234567")]
    public void System_Text_JSON_deserialization(object json, InternationalBankAccountNumber svo)
        => JsonTester.Read_System_Text_JSON<InternationalBankAccountNumber>(json).Should().Be(svo);

    [Test]
    [Arguments(null, null)]
    [Arguments("NL20INGB0001234567", "NL20INGB0001234567")]
    public void System_Text_JSON_serialization(InternationalBankAccountNumber svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [Test]
    [Arguments("NL20INGB0001234567", "NL20INGB0001234567")]
    public void convention_based_deserialization(object json, InternationalBankAccountNumber svo)
        => JsonTester.Read<InternationalBankAccountNumber>(json).Should().Be(svo);

    [Test]
    [Arguments(null, null)]
    [Arguments("NL20INGB0001234567", "NL20INGB0001234567")]
    public void convention_based_serialization(InternationalBankAccountNumber svo, object json)
        => JsonTester.Write(svo).Should().Be(json);

    [Test]
    [Arguments("Invalid input", typeof(FormatException))]
    [Arguments("2017-06-11", typeof(FormatException))]
    [Arguments(true, typeof(InvalidOperationException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
        => json
            .Invoking(JsonTester.Read<InternationalBankAccountNumber>)
            .Should().Throw<Exception>()
            .And.Should().BeOfType(exceptionType);
}

public class Supports_XML_serialization
{
    [Test]
    public void using_XmlSerializer_to_serialize()
    {
        var xml = Serialize.Xml(Svo.Iban);
        xml.Should().Be("NL20INGB0001234567");
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<InternationalBankAccountNumber>("NL20INGB0001234567");
        svo.Should().Be(Svo.Iban);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.Iban);
        Svo.Iban.Should().Be(round_tripped);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.Iban);
        var round_tripped = SerializeDeserialize.Xml(structure);
        structure.Should().Be(round_tripped);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.Iban;
        obj.GetSchema().Should().BeNull();
    }
}

public class Input_is_invalid_when
{
    [Test]
    public void country_does_not_exist()
        => InternationalBankAccountNumber.TryParse("XX950210000000693123456").Should().BeNull();

    [Test]
    [Arguments("           X")]
    [Arguments("US34 5678 9AB")]
    public void shorter_than_12(string iban)
        => InternationalBankAccountNumber.TryParse(iban).Should().BeNull();

    [Test]
    public void longer_than_36()
        => InternationalBankAccountNumber.TryParse("US00 2222 3333 4444 5555 6666 7777 8888 9999 A").Should().BeNull();

    [Test]
    [Arguments("NL76 CZHQ 9695 4545 9")]
    [Arguments("NL45 HEQN 0564 4242 147")]
    public void length_does_not_match_country_BBAN(string iban)
        => InternationalBankAccountNumber.TryParse(iban).Should().BeNull();

    [Test]
    public void other_than_alpha_numeric()
        => InternationalBankAccountNumber.TryParse("AE20 #$12 0070 3456 7890 1234 5678").Should().BeNull();

    [Test]
    [Arguments("ibanNL20INGB0001234567")]
    [Arguments("iban :NL20INGB0001234567")]
    [Arguments("iban : NL20INGB0001234567")]
    [Arguments("(IB AN) NL20INGB0001234567")]
    [Arguments("IBA-N NL20INGB0001234567")]
    [Arguments("IBAN IBAN NL20INGB0001234567")]
    [Arguments("(IBAN) IBAN NL20INGB0001234567")]
    [Arguments("IBAN (IBAN) NL20INGB0001234567")]
    public void string_with_malformed_IBAN_prefix(string str)
        => InternationalBankAccountNumber.TryParse(str).Should().BeNull();

    [Test]
    [Arguments("MU60 BOMM 0835 4151 5881 3959 000A BC")]
    [Arguments("MU53 BOMM 0835 4151 5881 3959 000Z ZZ")]
    public void Mauritius_does_not_end_with_currency_code(string iban)
        => InternationalBankAccountNumber.TryParse(iban).Should().BeNull();

    [Test]
    [Arguments("SC96 BANK 0835 4151 5881 3959 8706 ABC")]
    [Arguments("SC89 BANK 0835 4151 5881 3959 8706 ZZZ")]
    public void Seychelles_does_not_end_with_currency_code(string iban)
        => InternationalBankAccountNumber.TryParse(iban).Should().BeNull();

    [Test]
    [Arguments("BA23 1973 0058 1527 443")]
    [Arguments("ME74 0986 4623 4212 3918 5")]
    [Arguments("MK14 3935 0032 6437 91")]
    [Arguments("MR34 8380 0832 4152 5888 3958 87")]
    [Arguments("PT43 1185 0586 3461 2219 4930")]
    [Arguments("RS07 8688 9281 0642 3946 9")]
    [Arguments("SI72 3749 8042 5870 17")]
    [Arguments("TL96 4551 5574 6824 5444 05")]
    [Arguments("TN33 6926 5530 1193 5329 855")]
    public void IBANs_with_fixed_checksum_have_a_different_one(string iban)
        => InternationalBankAccountNumber.TryParse(iban).Should().BeNull();
}

public class Input_is_valid
{
    [Test]
    [Arguments("iban NL20INGB0001234567")]
    [Arguments("iban:NL20INGB0001234567")]
    [Arguments("iban: NL20INGB0001234567")]
    [Arguments("(iban)NL20INGB0001234567")]
    [Arguments("(iban) NL20INGB0001234567")]
    [Arguments("(Iban) NL20INGB0001234567")]
    [Arguments("(IBAN) NL20INGB0001234567")]
    [Arguments("Iban-NL20INGB0001234567")]
    [Arguments("Iban NL20INGB0001234567")]
    [Arguments("IBAN NL20INGB0001234567")]
    [Arguments(" IBAN NL20INGB0001234567")]
    [Arguments(" IBAN\tNL20INGB0001234567")]
    [Arguments(" IBAN  NL20INGB0001234567")]
    public void string_with_IBAN_prefix(string str)
        => InternationalBankAccountNumber.Parse(str).Should().Be(Svo.Iban);

    [Test]
    [Arguments("US70 ABCD 1234")]
    [Arguments("US41 1234 5678 90AB CDEF GHIJ KLMN OPQR")]
    [Arguments("US78 590A 55XL 4071 0933 7937 5649 3947 97")]
    public void for_countries_without_IBAN(string str)
    {
        var iban = InternationalBankAccountNumber.Parse(str);
        InternationalBankAccountNumber.Supported.Should().NotContain(iban.Country);
    }

    [Test]
    public void despite_irregular_white_spacing()
        => InternationalBankAccountNumber.Parse("AE950 2100000006  93123456").IsEmptyOrUnknown().Should().BeFalse();

    [Test]
    [Arguments("CZ55 0800 0000 0012 3456 7899")]
    [Arguments("CZ65 0800 0000 1920 0014 5399")]
    [Arguments("EE86 2200 2210 6411 5891")]
    [Arguments("HR17 2360 0001 1012 3456 5")]
    [Arguments("HU13 1176 3842 0065 8885 0000 0000")]
    [Arguments("HU39 1176 3842 0073 9012 0000 0000")]
    [Arguments("HU32 1040 5004 0002 6548 0000 0009")]
    [Arguments("HU32 1170 5008 2046 4565 0000 0000")]
    [Arguments("PL02 2490 0005 0000 4600 8316 8772")]
    [Arguments("PL16 1160 2202 0000 0002 7718 3060")]
    [Arguments("PL53 1240 4650 1787 0010 7345 2383")]
    [Arguments("PL83 2030 0045 1110 0000 0390 3540")]
    public void for_countries_with_extended_validation(string iban)
        => InternationalBankAccountNumber.TryParse(iban).Should().NotBeNull();
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
       => OpenApiDataType.FromType(typeof(InternationalBankAccountNumber))
       .Should().Be(new OpenApiDataType(
           dataType: typeof(InternationalBankAccountNumber),
            description: "International Bank Account Number notation as defined by ISO 13616:2007.",
            example: "BE71096123456769",
            type: "string",
            format: "iban",
            pattern: "[A-Z]{2}[0-9]{2}[A-Z0-9]{8,32}"));

    [Test]
    [Arguments("NL20INGB0001234567")]
    public void pattern_matches(string input)
        => OpenApiDataType.FromType(typeof(InternationalBankAccountNumber))!.Matches(input).Should().BeTrue();
}

public class Debugger
{
    [Test]
    [Arguments("{empty}", "")]
    [Arguments("{unknown}", "?")]
    [Arguments("NL20 INGB 0001 2345 67", "NL20INGB0001234567")]
    public void has_custom_display(object display, InternationalBankAccountNumber svo)
        => svo.Should().HaveDebuggerDisplay(display);
}

public class Mod97
{
    [Test]
    [Arguments(55)]
    [Arguments(56)]
    public void No_overflow_until(int bits)
    {
        var max = (1UL << (bits + 1)) - 1d;
        // Add 1 Z.
        max *= 100;
        max += 'Z' - 'A' + 10;
        max.Should().BeLessThanOrEqualTo(ulong.MaxValue);
    }

    [Test]
    [Arguments(42)]
    [Arguments(43)]
    public void No_checsum_overflow_until(int bits)
    {
        var max = (1UL << (bits + 1)) - 1d;
        // Add 2 Z's.
        max *= 100;
        max += 'Z' - 'A' + 10;
        max *= 100;
        max += 'Z' - 'A' + 10;
        // Add checksum 99
        max *= 100;
        max += 99;
        max.Should().BeLessThanOrEqualTo(ulong.MaxValue);
    }
}
