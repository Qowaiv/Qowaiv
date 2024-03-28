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
		Country.FI, Country.FO, Country.FR,
		Country.GA, Country.GB, Country.GE, Country.GI, Country.GL, Country.GQ, Country.GR, Country.GT, Country.GW,
		Country.HN, Country.HR, Country.HU,
		Country.IE, Country.IL, Country.IQ, Country.IR, Country.IS, Country.IT,
		Country.JO,
		Country.KM, Country.KW, Country.KZ,
		Country.LB, Country.LC, Country.LI, Country.LT, Country.LU, Country.LV, Country.LY,
		Country.MA, Country.MC, Country.MD, Country.ME, Country.MG, Country.MK, Country.ML, Country.MR, Country.MT, Country.MU,
		Country.MZ, Country.NE, Country.NI, Country.NL, Country.NO,
		Country.PK, Country.PL, Country.PS, Country.PT,
		Country.QA,
		Country.RO, Country.RS, Country.RU,
		Country.SA, Country.SC, Country.SD, Country.SE, Country.SI, Country.SK, Country.SM, Country.SN, Country.ST, Country.SV,
		Country.TD, Country.TG, Country.TL, Country.TN, Country.TR,
		Country.UA, Country.VA, Country.VG,
		Country.XK,
	];

	[Test]
	public void by_106_Countries()
	{
		InternationalBankAccountNumber.Supported.OrderBy(c => c.IsoAlpha2Code).Should().BeEquivalentTo(All);
		InternationalBankAccountNumber.Supported.Count.Should().Be(106);
	}
}

public class With_domain_logic
{
	[TestCase("")]
	[TestCase("?")]
	public void has_length_zero_for_empty_and_unknown(InternationalBankAccountNumber svo)
	  => svo.Length.Should().Be(0);

	[TestCase(18, "NL20INGB0001234567")]
	public void has_length(int length, InternationalBankAccountNumber svo)
		=> svo.Length.Should().Be(length);

	[TestCase(false, "NL20INGB0001234567")]
	[TestCase(false, "?")]
	[TestCase(true, "")]
	public void IsEmpty_returns(bool result, InternationalBankAccountNumber svo)
	   => svo.IsEmpty().Should().Be(result);

	[TestCase(false, "NL20INGB0001234567")]
	[TestCase(true, "?")]
	[TestCase(true, "")]
	public void IsEmptyOrUnknown_returns(bool result, InternationalBankAccountNumber svo)
		=> svo.IsEmptyOrUnknown().Should().Be(result);

	[TestCase(false, "NL20INGB0001234567")]
	[TestCase(true, "?")]
	[TestCase(false, "")]
	public void IsUnknown_returns(bool result, InternationalBankAccountNumber svo)
		=> svo.IsUnknown().Should().Be(result);

	[TestCase(true, "NL20INGB0001234567")]
	[TestCase(true, "?")]
	[TestCase(false, "")]
	public void HasValue_is(bool result, InternationalBankAccountNumber svo) => svo.HasValue.Should().Be(result);

	[TestCase(true, "NL20INGB0001234567")]
	[TestCase(false, "?")]
	[TestCase(false, "")]
	public void IsKnown_is(bool result, InternationalBankAccountNumber svo) => svo.IsKnown.Should().Be(result);

	[TestCase("", "")]
	[TestCase("?", "?")]
	[TestCase("NL", "NL20INGB0001234567")]
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
	public void not_equal_to_DE68210501700012345678_value()
		=> Svo.Iban.Equals(InternationalBankAccountNumber.Parse("DE68210501700012345678")).Should().BeFalse();

	[Test]
	public void equal_to_same_value()
		=> Svo.Iban.Equals(InternationalBankAccountNumber.Parse("NL20INGB0001234567")).Should().BeTrue();

	[Test]
	public void equal_operator_returns_true_for_same_values()
		=> (Svo.Iban == InternationalBankAccountNumber.Parse("NL20INGB0001234567")).Should().BeTrue();

	[Test]
	public void equal_operator_returns_false_for_DE68210501700012345678_values()
		=> (Svo.Iban == InternationalBankAccountNumber.Parse("DE68210501700012345678")).Should().BeFalse();

	[Test]
	public void not_equal_operator_returns_false_for_same_values()
		=> (Svo.Iban != InternationalBankAccountNumber.Parse("NL20INGB0001234567")).Should().BeFalse();

	[Test]
	public void not_equal_operator_returns_true_for_DE68210501700012345678_values()
		=> (Svo.Iban != InternationalBankAccountNumber.Parse("DE68210501700012345678")).Should().BeTrue();

	[TestCase("", 0)]
	[TestCase("NL20INGB0001234567", 684896179)]
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
			InternationalBankAccountNumber.Parse("CI02 N259 9162 9182 8879 7488 1965"),
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
	public void with_null_pattern_equal_to_default()
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

	[TestCase(null, "NL20INGB0001234567", "NL20INGB0001234567")]
	[TestCase("f", "NL20INGB0001234567", "nl20 ingb 0001 2345 67")]
	[TestCase("h", "NL20INGB0001234567", "nl20 ingb 0001 2345 67")]
	[TestCase("H", "NL20INGB0001234567", "NL20 INGB 0001 2345 67")]
	[TestCase("u", "NL20INGB0001234567", "nl20ingb0001234567")]
	[TestCase("U", "NL20INGB0001234567", "NL20INGB0001234567")]
	[TestCase("m", "NL20INGB0001234567", "nl20ingb0001234567")]
	[TestCase("M", "NL20INGB0001234567", "NL20INGB0001234567")]
	[TestCase("F", "HR1723600001101234565", /*.......*/ "HR17 2360 0001 1012 3456 5")]
	[TestCase("F", "NL20INGB0001234567", /*..........*/ "NL20 INGB 0001 2345 67")]
	[TestCase("F", "FR1420041010050500013M02606", /*.*/ "FR14 2004 1010 0505 0001 3M02 606")]
	[TestCase("F", "EE382200221020145685", /*........*/ "EE38 2200 2210 2014 5685")]
	[TestCase("F", "", "")]
	[TestCase("F", "?", "?")]
	public void with_format(string format, InternationalBankAccountNumber svo, string formatted)
		=> svo.ToString(format).Should().Be(formatted);
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
	public void from_valid_input_only_otherwise_throws_on_Parse()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Func<InternationalBankAccountNumber> parse = () => InternationalBankAccountNumber.Parse("invalid input");
			parse.Should().Throw<FormatException>()
				.WithMessage("Not a valid IBAN");
		}
	}

	[Test]
	public void from_valid_input_only_otherwise_return_false_on_TryParse()
		=> (InternationalBankAccountNumber.TryParse("invalid input", out _)).Should().BeFalse();

	[Test]
	public void from_invalid_as_null_with_TryParse()
		=> InternationalBankAccountNumber.TryParse("invalid input").Should().BeNull();

	[Test]
	public void with_TryParse_returns_SVO()
		=> InternationalBankAccountNumber.TryParse("NL20INGB0001234567").Should().Be(Svo.Iban);

	[TestCase(' ')]
	[TestCase((char)160)]
	[TestCase('\t')]
	[TestCase('\r')]
	[TestCase('\n')]
	[TestCase('-')]
	[TestCase('_')]
	[TestCase('.')]
	public void ignoring_formatting(char ch)
	{
		var iban = InternationalBankAccountNumber.Parse($"{ch}NL{ch}20{ch}INGB00{ch}0123456{ch}7{ch}");
		iban.Should().Be(Svo.Iban);
	}
}

public class Can_not_be_parsed
{
	[Test]
	public void with_markup_within_the_country_code()
		=> InternationalBankAccountNumber.TryParse("N L20INGB0001234567").Should().BeNull();

	[Test]
	public void with_markup_within_the_checksum_code()
		=> InternationalBankAccountNumber.TryParse("NL2 0INGB0001234567").Should().BeNull();
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
#if NET6_0_OR_GREATER
	[TestCase(null, null)]
	[TestCase("NL20INGB0001234567", "NL20INGB0001234567")]
	public void System_Text_JSON_deserialization(object json, InternationalBankAccountNumber svo)
		=> JsonTester.Read_System_Text_JSON<InternationalBankAccountNumber>(json).Should().Be(svo);

	[TestCase(null, null)]
	[TestCase("NL20INGB0001234567", "NL20INGB0001234567")]
	public void System_Text_JSON_serialization(InternationalBankAccountNumber svo, object json)
		=> JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
	[TestCase("NL20INGB0001234567", "NL20INGB0001234567")]
	public void convention_based_deserialization(object json, InternationalBankAccountNumber svo)
		=> JsonTester.Read<InternationalBankAccountNumber>(json).Should().Be(svo);

	[TestCase(null, null)]
	[TestCase("NL20INGB0001234567", "NL20INGB0001234567")]
	public void convention_based_serialization(InternationalBankAccountNumber svo, object json)
		=> JsonTester.Write(svo).Should().Be(json);

	[TestCase("Invalid input", typeof(FormatException))]
	[TestCase("2017-06-11", typeof(FormatException))]
	[TestCase(true, typeof(InvalidOperationException))]
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

	[TestCase("           X")]
	[TestCase("US34 5678 9AB")]
	public void shorter_than_12(string iban)
		=> InternationalBankAccountNumber.TryParse(iban).Should().BeNull();

	[Test]
	public void longer_than_36()
		=> InternationalBankAccountNumber.TryParse("US00 2222 3333 4444 5555 6666 7777 8888 9999 A").Should().BeNull();

	[TestCase("NL76 CZHQ 9695 4545 9")]
	[TestCase("NL45 HEQN 0564 4242 147")]
	public void length_does_not_match_country_BBAN(string iban)
		=> InternationalBankAccountNumber.TryParse(iban).Should().BeNull();

	[Test]
	public void other_than_alpha_numeric()
		=> InternationalBankAccountNumber.TryParse("AE20 #$12 0070 3456 7890 1234 5678").Should().BeNull();

	[TestCase("ibanNL20INGB0001234567")]
	[TestCase("iban :NL20INGB0001234567")]
	[TestCase("iban : NL20INGB0001234567")]
	[TestCase("(IB AN) NL20INGB0001234567")]
	[TestCase("IBA-N NL20INGB0001234567")]
	[TestCase("IBAN IBAN NL20INGB0001234567")]
	[TestCase("(IBAN) IBAN NL20INGB0001234567")]
	[TestCase("IBAN (IBAN) NL20INGB0001234567")]
	public void string_with_malformed_IBAN_prefix(string str)
		=> InternationalBankAccountNumber.TryParse(str).Should().BeNull();

	[TestCase("MU60 BOMM 0835 4151 5881 3959 000A BC")]
	[TestCase("MU53 BOMM 0835 4151 5881 3959 000Z ZZ")]
	public void Mauritius_does_not_end_with_currency_code(string iban)
		=> InternationalBankAccountNumber.TryParse(iban).Should().BeNull();

	[TestCase("SC96 BANK 0835 4151 5881 3959 8706 ABC")]
	[TestCase("SC89 BANK 0835 4151 5881 3959 8706 ZZZ")]
	public void Seychelles_does_not_end_with_currency_code(string iban)
		=> InternationalBankAccountNumber.TryParse(iban).Should().BeNull();

	[TestCase("BA23 1973 0058 1527 443")]
	[TestCase("ME74 0986 4623 4212 3918 5")]
	[TestCase("MK14 3935 0032 6437 91")]
	[TestCase("MR34 8380 0832 4152 5888 3958 87")]
	[TestCase("PT43 1185 0586 3461 2219 4930")]
	[TestCase("RS07 8688 9281 0642 3946 9")]
	[TestCase("SI72 3749 8042 5870 17")]
	[TestCase("TL96 4551 5574 6824 5444 05")]
	[TestCase("TN33 6926 5530 1193 5329 855")]
	public void IBANs_with_fixed_checksum_have_a_different_one(string iban)
		=> InternationalBankAccountNumber.TryParse(iban).Should().BeNull();
}

public class Input_is_valid
{
	[TestCase("iban NL20INGB0001234567")]
	[TestCase("iban:NL20INGB0001234567")]
	[TestCase("iban: NL20INGB0001234567")]
	[TestCase("(iban)NL20INGB0001234567")]
	[TestCase("(iban) NL20INGB0001234567")]
	[TestCase("(Iban) NL20INGB0001234567")]
	[TestCase("(IBAN) NL20INGB0001234567")]
	[TestCase("Iban-NL20INGB0001234567")]
	[TestCase("Iban NL20INGB0001234567")]
	[TestCase("IBAN NL20INGB0001234567")]
	[TestCase(" IBAN NL20INGB0001234567")]
	[TestCase(" IBAN\tNL20INGB0001234567")]
	[TestCase(" IBAN  NL20INGB0001234567")]
	public void string_with_IBAN_prefix(string str)
		=> InternationalBankAccountNumber.Parse(str).Should().Be(Svo.Iban);

	[TestCase("US70 ABCD 1234")]
	[TestCase("US41 1234 5678 90AB CDEF GHIJ KLMN OPQR")]
	[TestCase("US19 T3NB 32YP 2588 8395 8870 7523 1343 8517")]
	public void for_countries_without_IBAN(string str)
	{
		var iban = InternationalBankAccountNumber.Parse(str);
		InternationalBankAccountNumber.Supported.Should().NotContain(iban.Country);
	}

	[Test]
	public void despite_irregular_white_spacing()
		=> InternationalBankAccountNumber.Parse("AE950 2100000006  93123456").IsEmptyOrUnknown().Should().BeFalse();

	[TestCase("CZ55 0800 0000 0012 3456 7899")]
	[TestCase("CZ65 0800 0000 1920 0014 5399")]
	[TestCase("EE86 2200 2210 6411 5891")]
	[TestCase("HR17 2360 0001 1012 3456 5")]
	[TestCase("HU13 1176 3842 0065 8885 0000 0000")]
	[TestCase("HU39 1176 3842 0073 9012 0000 0000")]
	[TestCase("HU32 1040 5004 0002 6548 0000 0009")]
	[TestCase("HU32 1170 5008 2046 4565 0000 0000")]
	[TestCase("PL02 2490 0005 0000 4600 8316 8772")]
	[TestCase("PL16 1160 2202 0000 0002 7718 3060")]
	[TestCase("PL53 1240 4650 1787 0010 7345 2383")]
	[TestCase("PL83 2030 0045 1110 0000 0390 3540")]
	public void for_countries_with_extended_validation(string iban)
		=> InternationalBankAccountNumber.TryParse(iban).Should().NotBeNull();
}

public class Is_Open_API_data_type
{
	[Test]
	public void with_info()
	   => Qowaiv.OpenApi.OpenApiDataType.FromType(typeof(InternationalBankAccountNumber))
	   .Should().Be(new Qowaiv.OpenApi.OpenApiDataType(
		   dataType: typeof(InternationalBankAccountNumber),
			description: "International Bank Account Number notation as defined by ISO 13616:2007.",
			example: "BE71096123456769",
			type: "string",
			format: "iban",
			pattern: "[A-Z]{2}[0-9]{2}[A-Z0-9]{8,32}",
			nullable: true));

	[TestCase("NL20INGB0001234567")]
	public void pattern_matches(string input)
		=> Qowaiv.OpenApi.OpenApiDataType.FromType(typeof(InternationalBankAccountNumber))!.Matches(input).Should().BeTrue();
}

#if NET8_0_OR_GREATER
#else
public class Supports_binary_serialization
{
	[Test]
	[Obsolete("Usage of the binary formatter is considered harmful.")]
	public void using_BinaryFormatter()
	{
		var round_tripped = SerializeDeserialize.Binary(Svo.Iban);
		Svo.Iban.Should().Be(round_tripped);
	}

	[Test]
	public void storing_string_in_SerializationInfo()
	{
		var info = Serialize.GetInfo(Svo.Iban);
		info.GetString("Value").Should().Be("NL20INGB0001234567");
	}
}
#endif

public class Debugger
{
	[TestCase("{empty}", "")]
	[TestCase("{unknown}", "?")]
	[TestCase("NL20 INGB 0001 2345 67", "NL20INGB0001234567")]
	public void has_custom_display(object display, InternationalBankAccountNumber svo)
		=> svo.Should().HaveDebuggerDisplay(display);
}
