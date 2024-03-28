namespace YesNo_specs;

public class With_domain_logic
{
	[TestCase(true, "yes")]
	[TestCase(true, "no")]
	[TestCase(true, "?")]
	[TestCase(false, "")]
	public void HasValue_is(bool result, YesNo svo) => svo.HasValue.Should().Be(result);

	[TestCase(true, "yes")]
	[TestCase(true, "no")]
	[TestCase(false, "?")]
	[TestCase(false, "")]
	public void IsKnown_is(bool result, YesNo svo) => svo.IsKnown.Should().Be(result);

	[TestCase(false, "yes")]
	[TestCase(false, "no")]
	[TestCase(false, "?")]
	[TestCase(true, "")]
	public void IsEmpty_returns(bool result, YesNo svo) => svo.IsEmpty().Should().Be(result);

	[TestCase(false, "yes")]
	[TestCase(false, "no")]
	[TestCase(true, "?")]
	[TestCase(true, "")]
	public void IsEmptyOrUnknown_returns(bool result, YesNo svo) => svo.IsEmptyOrUnknown().Should().Be(result);

	[TestCase(false, "yes")]
	[TestCase(false, "no")]
	[TestCase(true, "?")]
	[TestCase(false, "")]
	public void IsUnknown_returns(bool result, YesNo svo) => svo.IsUnknown().Should().Be(result);

	[TestCase(true, "yes")]
	[TestCase(true, "no")]
	[TestCase(false, "?")]
	[TestCase(false, "")]
	public void IsYesOrNo_returns(bool result, YesNo svo) => svo.IsYesOrNo().Should().Be(result);


	[TestCase(false, "")]
	[TestCase(false, "N")]
	[TestCase(true, "Y")]
	[TestCase(false, "?")]

	public void IsYes_returns(bool result, YesNo svo) => svo.IsYes().Should().Be(result);

	[TestCase(false, "")]
	[TestCase(true, "N")]
	[TestCase(false, "Y")]
	[TestCase(false, "?")]

	public void IsNo_returns(bool result, YesNo svo) => svo.IsNo().Should().Be(result);
}

public class Is_valid_for
{
	[TestCase("ja", "nl")]
	[TestCase("yes", "en-GB")]
	[TestCase("y", null)]
	[TestCase("true", null)]
	public void strings_representing_yes(string input, CultureInfo culture)
		=> YesNo.Parse(input, culture).Should().Be(YesNo.Yes);

	[TestCase("nee", "nl")]
	[TestCase("no", "en-GB")]
	[TestCase("n", null)]
	[TestCase("false", null)]
	public void strings_representing_no(string input, CultureInfo culture)
		=> YesNo.Parse(input, culture).Should().Be(YesNo.No);
}

public class Has_constant
{
	[Test]
	public void Empty_represent_default_value()
	{
		YesNo.Empty.Should().Be(default);
	}
}

public class Is_equal_by_value
{
	[Test]
	public void not_equal_to_null()
	{
		Svo.YesNo.Equals(null).Should().BeFalse();
	}

	[Test]
	public void not_equal_to_other_type()
	{
		Svo.YesNo.Equals(new object()).Should().BeFalse();
	}

	[Test]
	public void not_equal_to_different_value()
	{
		Svo.YesNo.Equals(YesNo.Unknown).Should().BeFalse();
	}

	[Test]
	public void equal_to_same_value()
	{
		Svo.YesNo.Equals(YesNo.Yes).Should().BeTrue();
	}

	[Test]
	public void equal_operator_returns_true_for_same_values()
	{
		(YesNo.Yes == Svo.YesNo).Should().BeTrue();
	}

	[Test]
	public void equal_operator_returns_false_for_different_values()
	{
		(YesNo.Yes == YesNo.No).Should().BeFalse();
	}

	[Test]
	public void not_equal_operator_returns_false_for_same_values()
	{
		(YesNo.Yes != Svo.YesNo).Should().BeFalse();
	}

	[Test]
	public void not_equal_operator_returns_true_for_different_values()
	{
		(YesNo.Yes != YesNo.No).Should().BeTrue();
	}

	[TestCase("", 0)]
	[TestCase("yes", 665630161)]
	public void hash_code_is_value_based(YesNo svo, int hash)
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
	{
		YesNo.Parse(null).Should().Be(YesNo.Empty);
	}

	[Test]
	public void from_empty_string_represents_Empty()
	{
		YesNo.Parse(string.Empty).Should().Be(YesNo.Empty);
	}

	[Test]
	public void from_question_mark_represents_Unknown()
	{
		YesNo.Parse("?").Should().Be(YesNo.Unknown);
	}

	[TestCase("en", "y")]
	[TestCase("nl", "j")]
	[TestCase("nl", "ja")]
	[TestCase("fr", "oui")]
	public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
	{
		using (culture.Scoped())
		{
			var parsed = YesNo.Parse(input);
			parsed.Should().Be(Svo.YesNo);
		}
	}

	[Test]
	public void from_valid_input_only_otherwise_throws_on_Parse()
	{
		using (TestCultures.en_GB.Scoped())
		{
			"invalid input".Invoking(YesNo.Parse)
				.Should().Throw<FormatException>()
				.WithMessage("Not a valid yes-no value");
		}
	}

	[Test]
	public void from_valid_input_only_otherwise_return_false_on_TryParse()
	{
		YesNo.TryParse("invalid input", out _).Should().BeFalse();
	}

	[Test]
	public void from_invalid_as_null_with_TryParse()
		=> YesNo.TryParse("invalid input").Should().BeNull();

	[Test]
	public void with_TryParse_returns_SVO()
	{
		YesNo.TryParse("yes").Should().Be(Svo.YesNo);
	}
}

public class Has_custom_formatting
{
	[Test]
	public void default_value_is_represented_as_string_empty()
	{
		default(YesNo).ToString().Should().Be(string.Empty);
	}

	[Test]
	public void unknown_value_is_represented_as_unknown()
	{
		YesNo.Unknown.ToString(CultureInfo.InvariantCulture).Should().Be("unknown");
	}

	[Test]
	public void custom_format_provider_is_applied()
	{
		var formatted = Svo.YesNo.ToString("B", FormatProvider.CustomFormatter);
		formatted.Should().Be("Unit Test Formatter, value: 'True', format: 'B'");
	}

	[Test]
	public void with_empty_format_provider()
	{
		using (TestCultures.es_EC.Scoped())
		{
			Svo.YesNo.ToString(FormatProvider.Empty).Should().Be("si");
		}
	}

	[TestCase("en-GB", null, "Yes", "yes")]
	[TestCase("nl-BE", "f", "Yes", "ja")]
	[TestCase("es-EQ", "F", "Yes", "Si")]
	[TestCase("en-GB", null, "No", "no")]
	[TestCase("nl-BE", "f", "No", "nee")]
	[TestCase("es-EQ", "F", "No", "No")]
	[TestCase("en-GB", "C", "Yes", "Y")]
	[TestCase("nl-BE", "C", "Yes", "J")]
	[TestCase("es-EQ", "C", "Yes", "S")]
	[TestCase("en-GB", "C", "No", "N")]
	[TestCase("nl-BE", "c", "No", "n")]
	[TestCase("es-EQ", "c", "No", "n")]
	[TestCase("en-US", "B", "Yes", "True")]
	[TestCase("en-US", "b", "No", "false")]
	[TestCase("en-US", "i", "Yes", "1")]
	[TestCase("en-US", "i", "No", "0")]
	[TestCase("en-US", "i", "?", "?")]
	public void culture_dependent(CultureInfo culture, string format, YesNo svo, string expected)
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
			Svo.YesNo.ToString(provider: null).Should().Be("ja");
		}
	}
}

public class Is_comparable
{
	[Test]
	public void to_null_is_1() => Svo.YesNo.CompareTo(Nil.Object).Should().Be(1);

	[Test]
	public void to_YesNo_as_object()
	{
		object obj = Svo.YesNo;
		Svo.YesNo.CompareTo(obj).Should().Be(0);
	}

	[Test]
	public void to_YesNo_only()
		=> new object().Invoking(Svo.YesNo.CompareTo).Should().Throw<ArgumentException>();

	[Test]
	public void can_be_sorted_using_compare()
	{
		var sorted = new[]
		{
				YesNo.Empty,
				YesNo.Empty,
				YesNo.No,
				YesNo.Yes,
				YesNo.Unknown,
			};
		var list = new List<YesNo> { sorted[3], sorted[4], sorted[2], sorted[0], sorted[1] };
		list.Sort();

		list.Should().BeEquivalentTo(sorted);
	}
}

public class Casts
{
	[TestCase("yes", true)]
	[TestCase("no", false)]
	public void explicitly_from_boolean(YesNo casted, bool boolean)
	{
		((YesNo)boolean).Should().Be(casted);
	}

	[Test]
	public void yes_implicitly_to_true()
	{
		var result = YesNo.Yes ? "passed" : "failed";
		result.Should().Be("passed");
	}

	[Test]
	public void no_implicitly_to_false()
	{
		var result = YesNo.No ? "passed" : "failed";
		result.Should().Be("failed");
	}

	[TestCase(null, "")]
	[TestCase(true, "y")]
	[TestCase(false, "n")]
	public void explicitly_from_nullable_boolean(bool? value, YesNo expected)
	{
		var casted = (YesNo)value;
		casted.Should().Be(expected);
	}

	[TestCase("", null)]
	[TestCase("y", true)]
	[TestCase("n", false)]
	[TestCase("?", null)]
	public void explicitly_to_nullable_boolean(YesNo svo, bool? expected)
	{
		var casted = (bool?)svo;
		casted.Should().Be(expected);
	}

	[TestCase("", null)]
	[TestCase("y", true)]
	[TestCase("n", false)]
	[TestCase("?", false)]
	public void explicitly_to_boolean(YesNo svo, bool expected)
	{
		var casted = (bool)svo;
		casted.Should().Be(expected);
	}
}

public class Supports_type_conversion
{
	[Test]
	public void via_TypeConverter_registered_with_attribute()
		=> typeof(YesNo).Should().HaveTypeConverterDefined();

	[Test]
	public void from_null_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.FromNull<string>().To<YesNo>().Should().Be(YesNo.Empty);
		}
	}

	[Test]
	public void from_empty_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.From(string.Empty).To<YesNo>().Should().Be(YesNo.Empty);
		}
	}

	[Test]
	public void from_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.From("Yes").To<YesNo>().Should().Be(Svo.YesNo);
		}
	}

	[Test]
	public void to_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.ToString().From(Svo.YesNo).Should().Be("yes");
		}
	}

	[TestCase(true, "yes")]
	[TestCase(false, "no")]
	public void from_boolean(bool from, YesNo yesNo)
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.From(from).To<YesNo>().Should().Be(yesNo);
		}
	}

	[TestCase("yes", true)]
	[TestCase("no", false)]
	public void to_boolean(YesNo yesNo, bool boolean)
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.To<bool>().From(yesNo).Should().Be(boolean);
		}
	}
}

public class Supports_JSON_serialization
{
#if NET6_0_OR_GREATER
	[TestCase(null, null)]
	[TestCase("yes", "yes")]
	[TestCase("no", "no")]
	[TestCase(true, "yes")]
	[TestCase(false, "no")]
	[TestCase(1L, "yes")]
	[TestCase(1.0, "yes")]
	[TestCase(0.0, "no")]
	[TestCase((long)byte.MaxValue, "?")]
	[TestCase((long)short.MaxValue, "?")]
	[TestCase((long)int.MaxValue, "?")]
	[TestCase(long.MaxValue, "?")]
	[TestCase("?", "?")]
	public void System_Text_JSON_deserialization(object json, YesNo svo)
		=> JsonTester.Read_System_Text_JSON<YesNo>(json).Should().Be(svo);

	[TestCase(null, null)]
	[TestCase("yes", "yes")]
	public void System_Text_JSON_serialization(YesNo svo, object json)
		=> JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
	[TestCase("yes", "yes")]
	[TestCase("no", "no")]
	[TestCase(true, "yes")]
	[TestCase(false, "no")]
	[TestCase(1L, "yes")]
	[TestCase(1.0, "yes")]
	[TestCase(0.0, "no")]
	[TestCase((long)byte.MaxValue, "?")]
	[TestCase((long)short.MaxValue, "?")]
	[TestCase((long)int.MaxValue, "?")]
	[TestCase(long.MaxValue, "?")]
	[TestCase("?", "?")]
	public void convention_based_deserialization(object json, YesNo svo)
		=> JsonTester.Read<YesNo>(json).Should().Be(svo);

	[TestCase(null, null)]
	[TestCase("yes", "yes")]
	public void convention_based_serialization(YesNo svo, object json)
		=> JsonTester.Write(svo).Should().Be(json);

	[TestCase("Invalid input", typeof(FormatException))]
	[TestCase("2017-06-11", typeof(FormatException))]
	[TestCase(5L, typeof(InvalidCastException))]
	public void throws_for_invalid_json(object json, Type exceptionType)
		=> json
			.Invoking(JsonTester.Read<YesNo>)
			.Should().Throw<Exception>()
			.And.Should().BeOfType(exceptionType);
}

public class Supports_XML_serialization
{
	[Test]
	public void using_XmlSerializer_to_serialize()
	{
		var xml = Serialize.Xml(Svo.YesNo);
		xml.Should().Be("yes");
	}

	[Test]
	public void using_XmlSerializer_to_deserialize()
	{
		var svo = Deserialize.Xml<YesNo>("yes");
		svo.Should().Be(Svo.YesNo);
	}

	[Test]
	public void using_data_contract_serializer()
	{
		var round_tripped = SerializeDeserialize.DataContract(Svo.YesNo);
		round_tripped.Should().Be(Svo.YesNo);
	}

	[Test]
	public void as_part_of_a_structure()
	{
		var structure = XmlStructure.New(Svo.YesNo);
		var round_tripped = SerializeDeserialize.Xml(structure);

		round_tripped.Should().Be(structure);
	}

	[Test]
	public void has_no_custom_XML_schema()
	{
		IXmlSerializable obj = Svo.YesNo;
		obj.GetSchema().Should().BeNull();
	}
}

public class Is_Open_API_data_type
{
	[Test]
	public void with_info()
		=> Qowaiv.OpenApi.OpenApiDataType.FromType(typeof(YesNo))
		.Should().BeEquivalentTo(new Qowaiv.OpenApi.OpenApiDataType(
			dataType: typeof(YesNo),
			description: "Yes-No notation.",
			example: "yes",
			type: "string",
			format: "yes-no",
			@enum: new[] { "yes", "no", "?" },
			nullable: true));
}

#if NET8_0_OR_GREATER
#else
public class Supports_binary_serialization
{
	[Test]
	[Obsolete("Usage of the binary formatter is considered harmful.")]
	public void using_BinaryFormatter()
	{
		var round_tripped = SerializeDeserialize.Binary(Svo.YesNo);
		round_tripped.Should().Be(Svo.YesNo);
	}

	[Test]
	public void storing_byte_in_SerializationInfo()
	{
		var info = Serialize.GetInfo(Svo.YesNo);
		info.GetByte("Value").Should().Be((byte)2);
	}
}
#endif

public class Debugger
{
	[TestCase("{empty}", "")]
	[TestCase("{unknown}", "?")]
	[TestCase("yes", "Y")]
	public void has_custom_display(object display, YesNo svo)
		=> svo.Should().HaveDebuggerDisplay(display);
}
