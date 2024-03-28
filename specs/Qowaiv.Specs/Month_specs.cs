namespace Month_specs;

public class With_domain_logic
{
	[TestCase(true, "February")]
	[TestCase(true, "?")]
	[TestCase(false, "")]
	public void HasValue_is(bool result, Month svo) => svo.HasValue.Should().Be(result);

	[TestCase(true, "February")]
	[TestCase(false, "?")]
	[TestCase(false, "")]
	public void IsKnown_is(bool result, Month svo) => svo.IsKnown.Should().Be(result);

	[TestCase(false, "February")]
	[TestCase(false, "?")]
	[TestCase(true, "")]
	public void IsEmpty_returns(bool result, Month svo)
	{
		svo.IsEmpty().Should().Be(result);
	}

	[TestCase(false, "February")]
	[TestCase(true, "?")]
	[TestCase(true, "")]
	public void IsEmptyOrUnknown_returns(bool result, Month svo)
	{
		svo.IsEmptyOrUnknown().Should().Be(result);
	}

	[TestCase(false, "February")]
	[TestCase(true, "?")]
	[TestCase(false, "")]
	public void IsUnknown_returns(bool result, Month svo)
	{
		svo.IsUnknown().Should().Be(result);
	}
}

public class Days
{
	[TestCase(-1, "", 1999)]
	[TestCase(-1, "February", "?")]
	[TestCase(28, "February", 1999)]
	[TestCase(29, "February", 2020)]
	[TestCase(31, "January", 2020)]
	[TestCase(30, "November", 2020)]
	public void per_year(int days, Month month, Year year)
	{
		month.Days(year).Should().Be(days);
	}
}

public class Short_name
{
	[Test]
	public void is_string_empty_for_empty()
	{
		Month.Empty.ShortName.Should().Be(string.Empty);
	}
	[Test]
	public void is_question_mark_for_unknown()
	{
		Month.Unknown.ShortName.Should().Be("?");
	}
	[Test]
	public void picks_current_culture()
	{
		using (TestCultures.nl_BE.Scoped())
		{
			Svo.Month.ShortName.Should().Be("feb.");
		}
	}
	[Test]
	public void supports_custom_culture()
	{
		Svo.Month.GetShortName(TestCultures.nl_BE).Should().Be("feb.");
	}
}

public class Full_name
{
	[Test]
	public void is_string_empty_for_empty()
	{
		Month.Empty.FullName.Should().Be(string.Empty);
	}
	[Test]
	public void is_question_mark_for_unknown()
	{
		Month.Unknown.FullName.Should().Be("?");
	}
	[Test]
	public void picks_current_culture()
	{
		using (TestCultures.nl_BE.Scoped())
		{
			Svo.Month.FullName.Should().Be("februari");
		}
	}
	[Test]
	public void supports_custom_culture()
	{
		Svo.Month.GetFullName(TestCultures.nl_BE).Should().Be("februari");
	}
}

public class Has_constant
{
	[Test]
	public void Empty_represent_default_value()
		=> Month.Empty.Should().Be(default);
}

public class Is_equal_by_value
{
	[Test]
	public void not_equal_to_null()
	{
		Svo.Month.Equals(null).Should().BeFalse();
	}

	[Test]
	public void not_equal_to_other_type()
	{
		Svo.Month.Equals(new object()).Should().BeFalse();
	}

	[Test]
	public void not_equal_to_different_value()
	{
		Svo.Month.Equals(Month.December).Should().BeFalse();
	}

	[Test]
	public void equal_to_same_value()
	{
		Svo.Month.Equals(Month.February).Should().BeTrue();
	}

	[Test]
	public void equal_operator_returns_true_for_same_values()
	{
		(Svo.Month == Month.February).Should().BeTrue();
	}

	[Test]
	public void equal_operator_returns_false_for_different_values()
	{
		(Svo.Month == Month.December).Should().BeFalse();
	}

	[Test]
	public void not_equal_operator_returns_false_for_same_values()
	{
		(Svo.Month != Month.February).Should().BeFalse();
	}

	[Test]
	public void not_equal_operator_returns_true_for_different_values()
	{
		(Svo.Month != Month.December).Should().BeTrue();
	}

	[TestCase("", 0)]
	[TestCase("February", 665630161)]
	public void hash_code_is_value_based(Month svo, int hash)
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
		Month.Parse(null).Should().Be(Month.Empty);
	}

	[Test]
	public void from_empty_string_represents_Empty()
	{
		Month.Parse(string.Empty).Should().Be(Month.Empty);
	}

	[Test]
	public void from_question_mark_represents_Unknown()
	{
		Month.Parse("?").Should().Be(Month.Unknown);
	}

	[TestCase("en", "February")]
	[TestCase("en", "02")]
	[TestCase("en", "2")]
	public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
	{
		using (culture.Scoped())
		{
			var parsed = Month.Parse(input);
			parsed.Should().Be(Svo.Month);
		}
	}

	[Test]
	public void from_valid_input_only_otherwise_throws_on_Parse()
	{
		using (TestCultures.en_GB.Scoped())
		{
			"invalid input".Invoking(Month.Parse)
				.Should().Throw<FormatException>()
				.WithMessage("Not a valid month");
		}
	}

	[Test]
	public void from_valid_input_only_otherwise_return_false_on_TryParse()
	{
		Month.TryParse("invalid input", out _).Should().BeFalse();
	}

	[Test]
	public void from_invalid_as_null_with_TryParse()
		=> Month.TryParse("invalid input").Should().BeNull();

	[Test]
	public void with_TryParse_returns_SVO()
	{
		Month.TryParse("February").Should().Be(Svo.Month);
	}
}

public class Can_be_created
{
	[Test]
	public void with_TryCreate_returns_SVO()
	{
		Month.TryCreate(2).Should().Be(Svo.Month);
	}
	[Test]
	public void with_TryCreate_returns_Empty()
	{
		Month.TryCreate(null).Should().Be(Month.Empty);
	}
}
public class Has_custom_formatting
{
	[Test]
	public void _default()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Svo.Month.ToString().Should().Be("February");
		}
	}

	[Test]
	public void with_null_pattern_equal_to_default()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Svo.Month.ToString(default(string)).Should().Be(Svo.Month.ToString());
		}
	}

	[Test]
	public void with_string_empty_pattern_equal_to_default()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Svo.Month.ToString(string.Empty).Should().Be(Svo.Month.ToString());
		}
	}

	[Test]
	public void default_value_is_represented_as_string_empty()
	{
		default(Month).ToString().Should().Be(string.Empty);
	}

	[Test]
	public void unknown_value_is_represented_as_unknown()
	{
		Month.Unknown.ToString().Should().Be("?");
	}

	[Test]
	public void custom_format_provider_is_applied()
	{
		var formatted = Svo.Month.ToString("f", FormatProvider.CustomFormatter);
		formatted.Should().Be("Unit Test Formatter, value: 'February', format: 'f'");
	}

	[TestCase("en-GB", null, "February", "February")]
	[TestCase("en-GB", "s", "February", "Feb")]
	[TestCase("en-GB", "M", "February", "2")]
	[TestCase("en-GB", "m", "February", "02")]
	[TestCase("nl-BE", "f", "February", "februari")]
	[TestCase("en-GB", "M", "?", "?")]
	[TestCase("en-GB", "m", "", "")]
	public void culture_dependent(CultureInfo culture, string format, Month svo, string expected)
	{
		using (culture.Scoped())
		{
			svo.ToString(format).Should().Be(expected);
		}
	}

	[Test]
	public void with_current_thread_culture_as_default()
	{
		using (new CultureInfoScope(culture: TestCultures.nl_NL, cultureUI: TestCultures.en_GB))
		{
			Svo.Month.ToString(provider: null).Should().Be("februari");
		}
	}
}

public class Is_comparable
{
	[Test]
	public void to_null_is_1() => Svo.Month.CompareTo(Nil.Object).Should().Be(1);

	[Test]
	public void to_Month_as_object()
	{
		object obj = Svo.Month;
		Svo.Month.CompareTo(obj).Should().Be(0);
	}

	[Test]
	public void to_Month_only()
		=> new object().Invoking(Svo.Month.CompareTo).Should().Throw<ArgumentException>();

	[Test]
	public void can_be_sorted_using_compare()
	{
		var sorted = new[]
		{
				default,
				default,
				Month.January,
				Month.February,
				Month.March,
				Month.Unknown,
			};
		var list = new List<Month> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
		list.Sort();
		list.Should().BeEquivalentTo(sorted);
	}

	[Test]
	public void by_operators_for_different_values()
	{
		Month smaller = Month.February;
		Month bigger = Month.March;
		(smaller < bigger).Should().BeTrue();
		(smaller <= bigger).Should().BeTrue();
		(smaller > bigger).Should().BeFalse();
		(smaller >= bigger).Should().BeFalse();
	}

	[Test]
	public void by_operators_for_equal_values()
	{
		Month left = Month.February;
		Month right = Svo.Month;
		(left < right).Should().BeFalse();
		(left <= right).Should().BeTrue();
		(left > right).Should().BeFalse();
		(left >= right).Should().BeTrue();
	}

	[TestCase("", "February")]
	[TestCase("?", "February")]
	[TestCase("February", "")]
	[TestCase("February", "?")]
	public void by_operators_for_empty_or_unknown_always_false(Month l, Month r)
	{
		(l < r).Should().BeFalse();
		(l <= r).Should().BeFalse();
		(l > r).Should().BeFalse();
		(l >= r).Should().BeFalse();
	}
}

public class Casts
{
	[Test]
	public void explicitly_from_byte()
	{
		var casted = (Month)2;
		casted.Should().Be(Svo.Month);
	}

	[Test]
	public void explicitly_to_byte()
	{
		var casted = (byte)Svo.Month;
		casted.Should().Be(2);
	}
}

public class Supports_type_conversion
{
	[Test]
	public void via_TypeConverter_registered_with_attribute()
		=> typeof(Month).Should().HaveTypeConverterDefined();

	[Test]
	public void from_null_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.FromNull<string>().To<Month>().Should().Be(default);
		}
	}

	[Test]
	public void from_empty_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.From(string.Empty).To<Month>().Should().Be(default);
		}
	}

	[Test]
	public void from_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.From("February").To<Month>().Should().Be(Svo.Month);
		}
	}

	[Test]
	public void to_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.ToString().From(Svo.Month).Should().Be("February");
		}
	}

	[Test]
	public void from_int()
		=> Converting.From(2).To<Month>().Should().Be(Svo.Month);

	[Test]
	public void to_int()
		=> Converting.To<int>().From(Svo.Month).Should().Be(2);
}


public class Supports_JSON_serialization
{
#if NET6_0_OR_GREATER
	[TestCase(null, null)]
	[TestCase("?", "?")]
	[TestCase(2.0, "February")]
	[TestCase(2L, "February")]
	[TestCase("feb", "February")]
	public void System_Text_JSON_deserialization(object json, Month svo)
		=> JsonTester.Read_System_Text_JSON<Month>(json).Should().Be(svo);

	[TestCase(null, null)]
	[TestCase("Feb", "Feb")]
	public void System_Text_JSON_serialization(Month svo, object json)
		=> JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
	[TestCase("?", "?")]
	[TestCase(2.0, "February")]
	[TestCase(2L, "February")]
	[TestCase("feb", "February")]
	public void convention_based_deserialization(object json, Month svo)
		=> JsonTester.Read<Month>(json).Should().Be(svo);

	[TestCase(null, null)]
	[TestCase("Feb", "Feb")]
	public void convention_based_serialization(Month svo, object json)
		=> JsonTester.Write(svo).Should().Be(json);

	[TestCase("Invalid input", typeof(FormatException))]
	[TestCase("2017-06-11", typeof(FormatException))]
	[TestCase(-1L, typeof(ArgumentOutOfRangeException))]
	[TestCase(long.MaxValue, typeof(ArgumentOutOfRangeException))]
	public void throws_for_invalid_json(object json, Type exceptionType)
		=> json
			.Invoking(JsonTester.Read<Month>)
			.Should().Throw<Exception>()
			.And.Should().BeOfType(exceptionType);
}

public class Supports_XML_serialization
{
	[Test]
	public void using_XmlSerializer_to_serialize()
	{
		var xml = Serialize.Xml(Svo.Month);
		xml.Should().Be("Feb");
	}

	[Test]
	public void using_XmlSerializer_to_deserialize()
	{
		var svo = Deserialize.Xml<Month>("Feb");
		Svo.Month.Should().Be(svo);
	}

	[Test]
	public void using_DataContractSerializer()
	{
		var round_tripped = SerializeDeserialize.DataContract(Svo.Month);
		round_tripped.Should().Be(Svo.Month);
	}

	[Test]
	public void as_part_of_a_structure()
	{
		var structure = XmlStructure.New(Svo.Month);
		var round_tripped = SerializeDeserialize.Xml(structure);
		round_tripped.Should().Be(structure);
	}

	[Test]
	public void has_no_custom_XML_schema()
	{
		IXmlSerializable obj = Svo.Month;
		obj.GetSchema().Should().BeNull();
	}
}

public class Is_Open_API_data_type
{
	[Test]
	public void with_info()
		=> Qowaiv.OpenApi.OpenApiDataType.FromType(typeof(Month))
		.Should().BeEquivalentTo(new Qowaiv.OpenApi.OpenApiDataType(
			dataType: typeof(Month),
			description: "Month(-only) notation.",
			type: "string",
			example: "Jun",
			format: "month",
			@enum: new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "?" },
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
		var round_tripped = SerializeDeserialize.Binary(Svo.Month);
		round_tripped.Should().Be(Svo.Month);
	}

	[Test]
	public void storing_byte_in_SerializationInfo()
	{
		var info = Serialize.GetInfo(Svo.Month);
		info.GetByte("Value").Should().Be((byte)2);
	}
}
#endif

public class Debugger
{
	[TestCase("{empty}", "")]
	[TestCase("{unknown}", "?")]
	[TestCase("February (02)", "February")]
	public void has_custom_display(object display, Month svo)
		=> svo.Should().HaveDebuggerDisplay(display);
}
