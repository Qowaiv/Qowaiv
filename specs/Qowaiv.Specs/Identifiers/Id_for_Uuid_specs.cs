namespace Identifiers.Id_for_Uuid_specs;

public class With_domain_logic
{
	[TestCase(true, "Qowaiv_SVOLibrary_GUIA")]
	[TestCase(false, "")]
	public void HasValue_is(bool result, CustomUuid svo) => svo.HasValue.Should().Be(result);
}

public class Is_comparable
{
	[Test]
	public void to_null_is_1() => Svo.CustomUuid.CompareTo(Nil.Object).Should().Be(1);

	[Test]
	public void to_CustomUuid_as_object()
	{
		object obj = Svo.CustomUuid;
		Svo.CustomUuid.CompareTo(obj).Should().Be(0);
	}

	[Test]
	public void to_CustomUuid_only()
		=> new object().Invoking(Svo.CustomUuid.CompareTo).Should().Throw<ArgumentException>();

	[Test]
	public void can_be_sorted_using_compare()
	{
		var sorted = new[]
		{
			CustomUuid.Empty,
			CustomUuid.Parse("33ef5805-c472-4b1f-88bb-2f0723c43889"),
			CustomUuid.Parse("58617a65-2a14-4a9a-82a8-c1a82c956c25"),
			CustomUuid.Parse("853634b4-e474-4b0f-b9ba-01fc732b56d8"),
			CustomUuid.Parse("93ca7b43-8fb3-44e5-a21f-feeebb8e0f6f"),
			CustomUuid.Parse("f5e6c39a-adcf-4eca-bcf2-6b8317ac502c"),
		};

		var list = new List<CustomUuid> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
		list.Sort();
		list.Should().BeEquivalentTo(sorted);
	}
}

public class Supports_type_conversion
{
	[Test]
	public void via_TypeConverter_registered_with_attribute()
		=> typeof(CustomUuid).Should().HaveTypeConverterDefined();

	[Test]
	public void from_null_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.FromNull<string>().To<CustomUuid>().Should().Be(CustomUuid.Empty);
		}
	}

	[Test]
	public void from_empty_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.From(string.Empty).To<CustomUuid>().Should().Be(CustomUuid.Empty);
		}
	}

	[Test]
	public void from_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.From("Qowaiv_SVOLibrary_GUIA").To<CustomUuid>().Should().Be(Svo.CustomUuid);
		}
	}

	[Test]
	public void to_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.ToString().From(Svo.CustomUuid).Should().Be("Qowaiv_SVOLibrary_GUIA");
		}
	}

	[Test]
	public void from_Guid()
		=> Converting.From(Svo.Guid).To<CustomUuid>().Should().Be(Svo.CustomUuid);


	[Test]
	public void from_Uuid()
		=> Converting.From(Svo.Uuid).To<CustomUuid>().Should().Be(Svo.CustomUuid);

	[Test]
	public void to_Guid()
		=> Converting.To<Guid>().From(Svo.CustomUuid).Should().Be(Svo.Guid);

	[Test]
	public void to_Uuid()
		=> Converting.To<Uuid>().From(Svo.CustomUuid).Should().Be(Svo.Uuid);
}

public class Supports_JSON_serialization
{
	[Test]
	public void writes_null_for_default_value()
		=> JsonTester.Write(default(CustomUuid)).Should().BeNull();

	[Test]
	public void writes_Base64_string_for_non_default_value()
		=> JsonTester.Write(Svo.CustomUuid).Should().Be("Qowaiv_SVOLibrary_GUIA");

#if NET6_0_OR_GREATER

	[Test]
	public void System_Text_JSON_deserialization_of_dictionary_keys()
	{
		System.Text.Json.JsonSerializer.Deserialize<Dictionary<CustomUuid, int>>(@"{""Qowaiv_SVOLibrary_GUIA"":42}")
			.Should().BeEquivalentTo(new Dictionary<CustomUuid, int>()
			{
				[Svo.CustomUuid] = 42,
			});
	}

	[Test]
	public void System_Text_JSON_serialization_of_dictionary_keys()
	{
		var dictionary = new Dictionary<CustomUuid, int>()
		{
			[default] = 17,
			[Svo.CustomUuid] = 42,
		};
		System.Text.Json.JsonSerializer.Serialize(dictionary)
			.Should().Be(@"{"""":17,""Qowaiv_SVOLibrary_GUIA"":42}");
	}
#endif
}

#if NET8_0_OR_GREATER
#else
public class Supports_binary_serialization
{
	[Test]
	[Obsolete("Usage of the binary formatter is considered harmful.")]
	public void using_BinaryFormatter()
	{
		var round_tripped = SerializeDeserialize.Binary(Svo.CustomUuid);
		round_tripped.Should().Be(Svo.CustomUuid);
	}

	[Test]
	public void storing_value_in_SerializationInfo()
	{
		var info = Serialize.GetInfo(Svo.CustomUuid);
		info.GetValue("Value", typeof(Guid)).Should().Be(Guid.Parse("8A1A8C42-D2FF-E254-E26E-B6ABCBF19420"));
	}
}
#endif

public class Is_Open_API_data_type
{
	[Test]
	public void with_info()
		=> OpenApiDataType.FromType(typeof(ForUuid))
		.Should().Be(new OpenApiDataType(
			dataType: typeof(CustomUuid),
			description: "UUID based identifier",
			example: "lmZO_haEOTCwGsCcbIZFFg",
			type: "string",
			format: "uuid-base64",
			nullable: true));
}
