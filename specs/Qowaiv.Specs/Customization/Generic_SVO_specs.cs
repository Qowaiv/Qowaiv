namespace Customization.SVO_specs;

public class Default_behavior
{
	[Test]
	public void MinLength_is_0()
		=> new WithDefaultBehavior().MinLength.Should().Be(0);

	[Test]
	public void MaxLength_is_int_MaxValue()
		=> new WithDefaultBehavior().MaxLength.Should().Be(int.MaxValue);

	[Test]
	public void Regex_Pattern_is_null()
		=> new WithDefaultBehavior().Pattern.Should().BeNull();

	[Test]
	public void ToJson_returns_underlying_value()
		=> GenericSvo.Empty.ToJson().Should().BeNull();
}

public class With_domain_logic
{
	[TestCase(true, "QOWAIV")]
	[TestCase(true, "?")]
	[TestCase(false, "")]
	public void HasValue_is(bool result, CustomSvo svo) => svo.HasValue.Should().Be(result);

	[TestCase(true, "QOWAIV")]
	[TestCase(false, "?")]
	[TestCase(false, "")]
	public void IsKnown_is(bool result, CustomSvo svo) => svo.IsKnown.Should().Be(result);

	[TestCase("")]
	[TestCase("?")]
	public void has_length_zero_for_empty_and_unknown(CustomSvo svo)
		=> svo.Length.Should().Be(0);

	[TestCase(6, "QOWAIV")]
	public void has_length(int length, CustomSvo svo)
		=> svo.Length.Should().Be(length);

	[TestCase(false, "QOWAIV")]
	[TestCase(false, "?")]
	[TestCase(true, "")]
	public void IsEmpty_returns(bool result, CustomSvo svo)
		=> svo.IsEmpty().Should().Be(result);

	[TestCase(false, "QOWAIV")]
	[TestCase(true, "?")]
	[TestCase(true, "")]
	public void IsEmptyOrUnknown_returns(bool result, CustomSvo svo)
		=> svo.IsEmptyOrUnknown().Should().Be(result);

	[TestCase(false, "QOWAIV")]
	[TestCase(true, "?")]
	[TestCase(false, "")]
	public void IsUnknown_returns(bool result, CustomSvo svo)
		=> svo.IsUnknown().Should().Be(result);
}

public class Has_constant
{
	[Test]
	public void Empty_represent_default_value()
		=> CustomSvo.Empty.Should().Be(default);
}

public class Is_equal_by_value
{
	[Test]
	public void not_equal_to_null()
		=> Svo.CustomSvo.Equals(null).Should().BeFalse();

	[Test]
	public void not_equal_to_other_type()
		=> Svo.CustomSvo.Equals(new object()).Should().BeFalse();

	[Test]
	public void not_equal_to_different_value()
		=> Svo.CustomSvo.Equals(CustomSvo.Parse("different")).Should().BeFalse();

	[Test]
	public void equal_to_same_value()
		=> Svo.CustomSvo.Equals(CustomSvo.Parse("QOWAIV")).Should().BeTrue();

	[Test]
	public void equal_operator_returns_true_for_same_values()
		=> (Svo.CustomSvo == CustomSvo.Parse("QOWAIV")).Should().BeTrue();

	[Test]
	public void equal_operator_returns_false_for_different_values()
		=> (Svo.CustomSvo == CustomSvo.Parse("different")).Should().BeFalse();

	[Test]
	public void not_equal_operator_returns_false_for_same_values()
		=> (Svo.CustomSvo != CustomSvo.Parse("QOWAIV")).Should().BeFalse();

	[Test]
	public void not_equal_operator_returns_true_for_different_values()
		=> (Svo.CustomSvo != CustomSvo.Parse("different")).Should().BeTrue();

	[TestCase("", 0)]
	[TestCase("QOWAIV", 467820648)]
	public void hash_code_is_value_based(CustomSvo svo, int hash)
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
		=> CustomSvo.Parse(null).Should().Be(CustomSvo.Empty);

	[Test]
	public void from_empty_string_represents_Empty()
		=> CustomSvo.Parse(string.Empty).Should().Be(CustomSvo.Empty);

	[Test]
	public void from_question_mark_represents_Unknown()
		=> CustomSvo.Parse("?").Should().Be(CustomSvo.Unknown);

	[TestCase("en", "QOWAIV")]
	public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
	{
		using (culture.Scoped())
		{
			CustomSvo.Parse(input).Should().Be(Svo.CustomSvo);
		}
	}

	[Test]
	public void from_valid_input_only_otherwise_throws_on_Parse()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Func<CustomSvo> parse = () => CustomSvo.Parse("invalid input!");
			parse.Should().Throw<FormatException>()
				.WithMessage("Not a valid CustomSvo");
		}
	}

	[Test]
	public void from_valid_input_only_otherwise_return_false_on_TryParse()
		=> CustomSvo.TryParse("invalid input", out _).Should().BeFalse();

	[Test]
	public void from_invalid_as_null_with_TryParse()
		=> CustomSvo.TryParse("invalid input!").Should().BeNull();

	[Test]
	public void with_TryParse_returns_SVO()
		=> CustomSvo.TryParse("QOWAIV").Should().Be(Svo.CustomSvo);
}

public class Has_custom_formatting
{
	[Test]
	public void _default()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Svo.CustomSvo.ToString().Should().Be("QOWAIV");
		}
	}

	[Test]
	public void with_null_pattern_equal_to_default()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Svo.CustomSvo.ToString().Should().Be(Svo.CustomSvo.ToString(default(string)));
		}
	}

	[Test]
	public void with_string_empty_pattern_equal_to_default()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Svo.CustomSvo.ToString().Should().Be(Svo.CustomSvo.ToString(string.Empty));
		}
	}

	[Test]
	public void default_value_is_represented_as_string_empty()
		=> default(CustomSvo).ToString().Should().BeEmpty();

	[Test]
	public void unknown_value_is_represented_as_unknown()
		=> CustomSvo.Unknown.ToString().Should().Be("?");

	[Test]
	public void with_empty_format_provider()
	{
		using (TestCultures.es_EC.Scoped())
		{
			Svo.CustomSvo.ToString(FormatProvider.Empty).Should().Be("QOWAIV");
		}
	}

	[Test]
	public void custom_format_provider_is_applied()
	{
		var formatted = Svo.CustomSvo.ToString("SomeFormat", FormatProvider.CustomFormatter);
		formatted.Should().Be("Unit Test Formatter, value: 'QOWAIV', format: 'SomeFormat'");
	}

	[TestCase("en-GB", null, "QOWAIV", "QOWAIV")]
	[TestCase("nl-BE", "f", "QOWAIV", "QOWAIV")]
	public void culture_dependent(CultureInfo culture, string format, CustomSvo svo, string expected)
	{
		using (culture.Scoped())
		{
			svo.ToString(format).Should().Be(expected);
		}
	}

	[Test]
	public void with_current_thread_culture_as_default()
	{
		using (new CultureInfoScope(
			culture: TestCultures.nl_NL,
			cultureUI: TestCultures.en_GB))
		{
			Svo.CustomSvo.ToString(provider: null).Should().Be("QOWAIV");
		}
	}
}

public class Is_comparable
{
	[Test]
	public void to_null_is_1() => Svo.CustomSvo.CompareTo(Nil.Object).Should().Be(1);

	[Test]
	public void to_Svo_as_object()
	{
		object obj = Svo.CustomSvo;
		Svo.CustomSvo.CompareTo(obj).Should().Be(0);
	}

	[Test]
	public void to_Svo_only()
		=> new object().Invoking(Svo.CustomSvo.CompareTo).Should().Throw<ArgumentException>();

	[Test]
	public void can_be_sorted_using_compare()
	{
		var sorted = new[]
		{
			default,
			default,
			CustomSvo.Parse("ABC"),
			CustomSvo.Parse("ABCD"),
			CustomSvo.Parse("ABCDE"),
			CustomSvo.Unknown,
		};

		var list = new List<CustomSvo> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
		list.Sort();

		list.Should().BeEquivalentTo(sorted);
	}
}

public class Casts
{
	[Test]
	public void explicitly_from_string()
	{
		var casted = (CustomSvo)"QOWAIV";
		casted.Should().Be(Svo.CustomSvo);
	}

	[Test]
	public void explicitly_to_string()
	{
		var casted = (string)Svo.CustomSvo;
		casted.Should().Be("QOWAIV");
	}
}

public class Supports_type_conversion
{
	[Test]
	public void via_TypeConverter_registered_with_attribute()
		=> typeof(CustomSvo).Should().HaveTypeConverterDefined();

	[Test]
	public void from_null_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.FromNull<string>().To<CustomSvo>().Should().Be(CustomSvo.Empty);
		}
	}

	[Test]
	public void from_empty_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.From(string.Empty).To<CustomSvo>().Should().Be(CustomSvo.Empty);
		}
	}

	[Test]
	public void from_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.From("QOWAIV").To<CustomSvo>().Should().Be(Svo.CustomSvo);
		}
	}

	[Test]
	public void to_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.ToString().From(Svo.CustomSvo).Should().Be("QOWAIV");
		}
	}
}

public class Supports_JSON_serialization
{
#if NET6_0_OR_GREATER
	[TestCase(null, null)]
	[TestCase("?", "?")]
	[TestCase("QOWAIV", "QOWAIV")]
	public void System_Text_JSON_deserialization(object json, CustomSvo svo)
		=> JsonTester.Read_System_Text_JSON<CustomSvo>(json).Should().Be(svo);

	[TestCase(null, null)]
	[TestCase("QOWAIV", "QOWAIV")]
	public void System_Text_JSON_serialization(CustomSvo svo, object json)
		=> JsonTester.Write_System_Text_JSON(svo).Should().Be(json);

	[TestCase("{}")]
	public void System_Text_JSON_throws_on(string json)
	{
		json.Invoking(json => System.Text.Json.JsonSerializer.Deserialize<CustomSvo>(json))
			.Should().Throw<System.Text.Json.JsonException>();
	}
#endif
	[TestCase("?", "?")]
	[TestCase("QOWAIV", "QOWAIV")]
	public void convention_based_deserialization(object json, CustomSvo svo)
	   => JsonTester.Read<CustomSvo>(json).Should().Be(svo);


	[TestCase(null, null)]
	[TestCase("QOWAIV", "QOWAIV")]
	public void convention_based_serialization(CustomSvo svo, object json)
		=> JsonTester.Write(svo).Should().Be(json);

	[TestCase("Invalid input!", typeof(FormatException))]
	[TestCase(5L, typeof(InvalidOperationException))]
	public void throws_for_invalid_json(object json, Type exceptionType)
	{
		Func<CustomSvo> read = () => JsonTester.Read<CustomSvo>(json);
		read.Should().Throw<Exception>().Subject.Single().Should().BeOfType(exceptionType);
	}
}

public class Supports_XML_serialization
{
	[Test]
	public void using_XmlSerializer_to_serialize()
	{
		var xml = Serialize.Xml(Svo.CustomSvo);
		xml.Should().Be("QOWAIV");
	}

	[Test]
	public void using_XmlSerializer_to_deserialize()
	{
		var svo = Deserialize.Xml<CustomSvo>("QOWAIV");
		svo.Should().Be(Svo.CustomSvo);
	}

	[Test]
	public void using_DataContractSerializer()
	{
		var round_tripped = SerializeDeserialize.DataContract(Svo.CustomSvo);
		Svo.CustomSvo.Should().Be(round_tripped);
	}

	[Test]
	public void as_part_of_a_structure()
	{
		var structure = XmlStructure.New(Svo.CustomSvo);
		var round_tripped = SerializeDeserialize.Xml(structure);
		structure.Should().Be(round_tripped);
	}

	[Test]
	public void has_no_custom_XML_schema()
	{
		IXmlSerializable obj = Svo.CustomSvo;
		obj.GetSchema().Should().BeNull();
	}
}

public class Is_Open_API_data_type
{
	[Test]
	public void with_info()
	   => Qowaiv.OpenApi.OpenApiDataType.FromType(typeof(ForCustomSvo))
	   .Should().Be(new Qowaiv.OpenApi.OpenApiDataType(
		   dataType: typeof(CustomSvo),
		   description: "Custom SVO Example",
		   example: "QOWAIV",
		   type: "string",
		   format: "custom",
		   pattern: null));
}

#if NET8_0_OR_GREATER
#else
public class Supports_binary_serialization
{
	[Test]
	[Obsolete("Usage of the binary formatter is considered harmful.")]
	public void using_BinaryFormatter()
	{
		var round_tripped = SerializeDeserialize.Binary(Svo.CustomSvo);
		round_tripped.Should().Be(Svo.CustomSvo);
	}
	[Test]
	public void storing_string_in_SerializationInfo()
	{
		var info = Serialize.GetInfo(Svo.CustomSvo);
		info.GetString("Value").Should().Be("QOWAIV");
	}
}
#endif
public class Debugger
{
	[TestCase("{empty}", "")]
	[TestCase("{unknown}", "?")]
	[TestCase("QOWAIV", "QOWAIV")]
	public void has_custom_display(object display, CustomSvo svo)
		=> svo.Should().HaveDebuggerDisplay(display);
}
