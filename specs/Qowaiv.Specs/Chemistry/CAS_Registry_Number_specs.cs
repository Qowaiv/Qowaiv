using Qowaiv.Chemistry;

namespace Chemistry.CAS_Registry_Number_specs;

public class With_domain_logic
{
	[TestCase(true, "73–24–5")]
	[TestCase(true, "?")]
	[TestCase(false, "")]
	public void HasValue_is(bool result, CasRegistryNumber svo) => svo.HasValue.Should().Be(result);

	[TestCase(true, "73–24–5")]
	[TestCase(false, "?")]
	[TestCase(false, "")]
	public void IsKnown_is(bool result, CasRegistryNumber svo) => svo.IsKnown.Should().Be(result);

	[TestCase("")]
	[TestCase("?")]
	public void has_length_zero_for_empty_and_unknown(CasRegistryNumber svo)
		=> svo.Length.Should().Be(0);

	[TestCase(5, "73–24–5")]
	[TestCase(7, "7732-18-5")]
	[TestCase(8, "10028-14-5")]
	public void has_length(int length, CasRegistryNumber svo)
		=> svo.Length.Should().Be(length);

	[TestCase(false, "10028-14-5")]
	[TestCase(false, "?")]
	[TestCase(true, "")]
	public void IsEmpty_returns(bool result, CasRegistryNumber svo)
		=> svo.IsEmpty().Should().Be(result);

	[TestCase(false, "10028-14-5")]
	[TestCase(true, "?")]
	[TestCase(true, "")]
	public void IsEmptyOrUnknown_returns(bool result, CasRegistryNumber svo)
		=> svo.IsEmptyOrUnknown().Should().Be(result);

	[TestCase(false, "10028-14-5")]
	[TestCase(true, "?")]
	[TestCase(false, "")]
	public void IsUnknown_returns(bool result, CasRegistryNumber svo)
		=> svo.IsUnknown().Should().Be(result);
}

public class Is_valid_for
{
	[TestCase("10028-14-5")]
	[TestCase("10028-14-5")]
	public void strings_representing_SVO(string input)
		=> CasRegistryNumber.Parse(input).IsEmptyOrUnknown().Should().BeFalse();
}

public class Is_not_valid_for
{
	[Test]
	public void Numbers_with_less_then_5_digits()
		=> CasRegistryNumber.TryParse("9-99-4").Should().BeNull();

	[Test]
	public void Numbers_with_more_then_10_digits()
		=> CasRegistryNumber.TryParse("10000000-00-0").Should().BeNull();

	[TestCase("10028-14-3")]
	[TestCase("10028-15-5")]
	[TestCase("10028-84-5")]
	[TestCase("10020-14-5")]
	[TestCase("10068-14-5")]
	[TestCase("10128-14-5")]
	[TestCase("32028-14-5")]
	public void checksum_mismatches(string number)
		=> CasRegistryNumber.TryParse(number).Should().BeNull();
}

public class Has_constant
{
	[Test]
	public void Empty_represent_default_value()
		=> CasRegistryNumber.Empty.Should().Be(default);
}

public class Is_equal_by_value
{
	[Test]
	public void not_equal_to_null()
		=> Svo.CasRegistryNumber.Equals(null).Should().BeFalse();

	[Test]
	public void not_equal_to_other_type()
		=> Svo.CasRegistryNumber.Equals(new object()).Should().BeFalse();

	[Test]
	public void not_equal_to_different_value()
		=> Svo.CasRegistryNumber.Equals(7732_18_5.CasNr()).Should().BeFalse();

	[Test]
	public void equal_to_same_value()
		=> Svo.CasRegistryNumber.Equals(10028_14_5.CasNr()).Should().BeTrue();

	[Test]
	public void equal_operator_returns_true_for_same_values()
		=> (Svo.CasRegistryNumber == 10028_14_5.CasNr()).Should().BeTrue();

	[Test]
	public void equal_operator_returns_false_for_different_values()
		=> (Svo.CasRegistryNumber == 7732_18_5.CasNr()).Should().BeFalse();

	[Test]
	public void not_equal_operator_returns_false_for_same_values()
		=> (Svo.CasRegistryNumber != 10028_14_5.CasNr()).Should().BeFalse();

	[Test]
	public void not_equal_operator_returns_true_for_different_values()
		=> (Svo.CasRegistryNumber != 7732_18_5.CasNr()).Should().BeTrue();

	[TestCase("", 0)]
	[TestCase("10028-14-5", 657830306)]
	public void hash_code_is_value_based(CasRegistryNumber svo, int hash)
	{
		using (Hash.WithoutRandomizer())
		{
			svo.GetHashCode().Should().Be(hash);
		}
	}
}

public class Can_be_parsed
{
	[Test]
	public void from_null_string_represents_Empty()
		=> CasRegistryNumber.Parse(null).Should().Be(CasRegistryNumber.Empty);

	[Test]
	public void from_empty_string_represents_Empty()
		=> CasRegistryNumber.Parse(string.Empty).Should().Be(CasRegistryNumber.Empty);

	[Test]
	public void from_question_mark_represents_Unknown()
		=> CasRegistryNumber.Parse("?").Should().Be(CasRegistryNumber.Unknown);

	[TestCase("en-US", "10028145")]
	[TestCase("en-GB", "10028-14-5")]
	[TestCase("en-US", "10028.14.5")]
	[TestCase("en-GB", "10028 14 5")]
	[TestCase("en-CA", "100.281.45")]
	[TestCase("nl-BE", "10028-14-5")]
	public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
	{
		using (culture.Scoped())
		{
			CasRegistryNumber.Parse(input).Should().Be(Svo.CasRegistryNumber);
		}
	}

	[Test]
	public void from_valid_input_only_otherwise_throws_on_Parse()
	{
		using (TestCultures.en_GB.Scoped())
		{
			"not a CAS registry".Invoking(CasRegistryNumber.Parse)
				.Should().Throw<FormatException>()
				.WithMessage("Not a valid CAS Registry Number")
				.And.InnerException.Should().BeEquivalentTo(new
				{
					Type = "Qowaiv.Chemistry.CasRegistryNumber",
					Value = "not a CAS registry"
				});
		}
	}

	[Test]
	public void from_valid_input_only_otherwise_return_false_on_TryParse()
		=> CasRegistryNumber.TryParse("invalid input", out _).Should().BeFalse();

	[Test]
	public void from_invalid_as_null_with_TryParse()
		=> CasRegistryNumber.TryParse("invalid input").Should().BeNull();

	[Test]
	public void with_TryParse_returns_SVO()
		=> CasRegistryNumber.TryParse("10028-14-5").Should().Be(Svo.CasRegistryNumber);
}

public class Has_custom_formatting
{
	[Test]
	public void _default()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Svo.CasRegistryNumber.ToString().Should().Be("10028-14-5");
		}
	}

	[Test]
	public void with_null_pattern_equal_to_default()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Svo.CasRegistryNumber.ToString().Should().Be(Svo.CasRegistryNumber.ToString(default(string)));
		}
	}

	[Test]
	public void with_string_empty_pattern_equal_to_default()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Svo.CasRegistryNumber.ToString().Should().Be(Svo.CasRegistryNumber.ToString(string.Empty));
		}
	}

	[Test]
	public void default_value_is_represented_as_string_empty()
		=> default(CasRegistryNumber).ToString().Should().BeEmpty();

	[Test]
	public void unknown_value_is_represented_as_unknown()
		=> CasRegistryNumber.Unknown.ToString().Should().Be("?");

	[Test]
	public void with_empty_format_provider()
	{
		using (TestCultures.es_EC.Scoped())
		{
			Svo.CasRegistryNumber.ToString(FormatProvider.Empty).Should().Be("10028-14-5");
		}
	}

	[Test]
	public void custom_format_provider_is_applied()
	{
		var formatted = Svo.CasRegistryNumber.ToString("#_00_00_0", FormatProvider.CustomFormatter);
		formatted.Should().Be("Unit Test Formatter, value: '100_28_14_5', format: '#_00_00_0'");
	}

	[Test]
	public void with_current_thread_culture_as_default()
	{
		using (new CultureInfoScope(culture: TestCultures.nl_NL, cultureUI: TestCultures.en_GB))
		{
			Svo.CasRegistryNumber.ToString(provider: null).Should().Be("10028-14-5");
		}
	}
}

public class Is_comparable
{
	[Test]
	public void to_null_is_1() => Svo.CasRegistryNumber.CompareTo(Nil.Object).Should().Be(1);

	[Test]
	public void to_CasRegistryNumber_as_object()
	{
		object obj = Svo.CasRegistryNumber;
		Svo.CasRegistryNumber.CompareTo(obj).Should().Be(0);
	}

	[Test]
	public void to_CasRegistryNumber_only()
		=> new object().Invoking(Svo.CasRegistryNumber.CompareTo).Should().Throw<ArgumentException>();

	[Test]
	public void can_be_sorted_using_compare()
	{
		var sorted = new[]
		{
			default,
			default,
			64_19_7.CasNr(),
			67_64_1.CasNr(),
			74_86_2.CasNr(),
			CasRegistryNumber.Unknown,
		};

		var list = new List<CasRegistryNumber> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
		list.Sort();

		list.Should().BeEquivalentTo(sorted);
	}
}

public class Casts
{
	[Test]
	public void explicitly_from_int()
	{
		var casted = (CasRegistryNumber)10028_14_5;
		casted.Should().Be(Svo.CasRegistryNumber);
	}

	[Test]
	public void explicitly_from_long()
	{
		var casted = (CasRegistryNumber)10028_14_5L;
		casted.Should().Be(Svo.CasRegistryNumber);
	}

	[Test]
	public void explicitly_to_long()
	{
		var casted = (long)Svo.CasRegistryNumber;
		casted.Should().Be(10028_14_5L);
	}
}

public class Has_humanizer_creators
{
	[Test]
	public void CasNr_from_int() => 10028_14_5.CasNr().Should().Be(Svo.CasRegistryNumber);

	[Test]
	public void CasNr_from_long() => 10028_14_5L.CasNr().Should().Be(Svo.CasRegistryNumber);
}

public class Supports_type_conversion
{
	[Test]
	public void via_TypeConverter_registered_with_attribute()
		=> typeof(CasRegistryNumber).Should().HaveTypeConverterDefined();

	[Test]
	public void from_null_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.FromNull<string>().To<CasRegistryNumber>().Should().Be(CasRegistryNumber.Empty);
		}
	}

	[Test]
	public void from_empty_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.From(string.Empty).To<CasRegistryNumber>().Should().Be(CasRegistryNumber.Empty);
		}
	}

	[Test]
	public void from_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.From("10028-14-5").To<CasRegistryNumber>().Should().Be(Svo.CasRegistryNumber);
		}
	}

	[Test]
	public void to_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.ToString().From(Svo.CasRegistryNumber).Should().Be("10028-14-5");
		}
	}

	[Test]
	public void from_long()
		=> Converting.From(10028_14_5L).To<CasRegistryNumber>().Should().Be(Svo.CasRegistryNumber);

	[Test]
	public void to_int()
		=> Converting.To<long>().From(Svo.CasRegistryNumber).Should().Be(10028_14_5L);
}

public class Supports_JSON_serialization
{
#if NET6_0_OR_GREATER
	[TestCase("?", "?")]
	[TestCase(10028_14_5L, "10028-14-5")]
	public void System_Text_JSON_deserialization(object json, CasRegistryNumber svo)
		=> JsonTester.Read_System_Text_JSON<CasRegistryNumber>(json).Should().Be(svo);

	[TestCase(null, null)]
	[TestCase("10028-14-5", "10028-14-5")]
	public void System_Text_JSON_serialization(CasRegistryNumber svo, object json)
		=> JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
	[TestCase("?", "?")]
	[TestCase(10028_14_5L, "10028-14-5")]
	public void convention_based_deserialization(object json, CasRegistryNumber svo)
	   => JsonTester.Read<CasRegistryNumber>(json).Should().Be(svo);

	[TestCase(null, null)]
	[TestCase("10028-14-5", "10028-14-5")]
	public void convention_based_serialization(CasRegistryNumber svo, object json)
		=> JsonTester.Write(svo).Should().Be(json);

	[TestCase("Invalid input", typeof(FormatException))]
	[TestCase("2017-06-11", typeof(FormatException))]
	[TestCase(true, typeof(InvalidOperationException))]
	public void throws_for_invalid_json(object json, Type exceptionType)
		=> json
			.Invoking(JsonTester.Read<CasRegistryNumber>)
			.Should().Throw<Exception>()
			.And.Should().BeOfType(exceptionType);
}

public class Supports_XML_serialization
{
	[Test]
	public void using_XmlSerializer_to_serialize()
	{
		var xml = Serialize.Xml(Svo.CasRegistryNumber);
		xml.Should().Be("10028-14-5");
	}

	[Test]
	public void using_XmlSerializer_to_deserialize()
	{
		var svo = Deserialize.Xml<CasRegistryNumber>("10028-14-5");
		svo.Should().Be(Svo.CasRegistryNumber);
	}

	[Test]
	public void using_DataContractSerializer()
	{
		var round_tripped = SerializeDeserialize.DataContract(Svo.CasRegistryNumber);
		Svo.CasRegistryNumber.Should().Be(round_tripped);
	}

	[Test]
	public void as_part_of_a_structure()
	{
		var structure = XmlStructure.New(Svo.CasRegistryNumber);
		var round_tripped = SerializeDeserialize.Xml(structure);
		structure.Should().Be(round_tripped);
	}

	[Test]
	public void has_no_custom_XML_schema()
	{
		IXmlSerializable obj = Svo.CasRegistryNumber;
		obj.GetSchema().Should().BeNull();
	}
}

public class Is_Open_API_data_type
{
	[Test]
	public void with_info()
	   => OpenApiDataType.FromType(typeof(CasRegistryNumber))
	   .Should().Be(new OpenApiDataType(
		   dataType: typeof(CasRegistryNumber),
		   description: "CAS Registry Number",
		   example: "7732-18-5",
		   type: "string",
		   format: "cas-nr",
		   pattern: "[1-9][0-9]+\\-[0-9]{2}\\-[0-9]",
		   nullable: true));

	[TestCase("7732-18-5")]
	[TestCase("10028-14-5")]
	public void pattern_matches(string input)
		=> OpenApiDataType.FromType(typeof(CasRegistryNumber))!.Matches(input).Should().BeTrue();
}

#if NET8_0_OR_GREATER
#else
public class Supports_binary_serialization
{
	[Test]
	[Obsolete("Usage of the binary formatter is considered harmful.")]
	public void using_BinaryFormatter()
	{
		var round_tripped = SerializeDeserialize.Binary(Svo.CasRegistryNumber);
		round_tripped.Should().Be(Svo.CasRegistryNumber);
	}

	[Test]
	public void storing_long_in_SerializationInfo()
	{
		var info = Serialize.GetInfo(Svo.CasRegistryNumber);
		info.GetInt64("Value").Should().Be(10028_14_5L);
	}
}
#endif

public class Debugger
{
	[TestCase("{empty}", "")]
	[TestCase("{unknown}", "?")]
	[TestCase("10028-14-5", "10028-14-5")]
	public void has_custom_display(object display, CasRegistryNumber svo)
		=> svo.Should().HaveDebuggerDisplay(display);
}

