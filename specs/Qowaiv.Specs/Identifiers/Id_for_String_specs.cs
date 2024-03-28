namespace Identifiers.Id_for_String_specs;

public class Is_comparable
{
	[Test]
	public void to_null_is_1() => Svo.StringId.CompareTo(Nil.Object).Should().Be(1);

	[Test]
	public void to_StringId_as_object()
	{
		object obj = Svo.StringId;
		Svo.StringId.CompareTo(obj).Should().Be(0);
	}

	[Test]
	public void to_StringId_only()
		=> new object().Invoking(Svo.StringId.CompareTo).Should().Throw<ArgumentException>();

	[Test]
	public void can_be_sorted_using_compare()
	{
		var sorted = new[]
		{
			StringId.Empty,
			StringId.Parse("33ef5805c472"),
			StringId.Parse("58617a652a14"),
			StringId.Parse("853634b4e474"),
			StringId.Parse("93ca7b438fb3"),
			StringId.Parse("f5e6c39aadcf"),
		};

		var list = new List<StringId> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
		list.Sort();
		list.Should().BeEquivalentTo(sorted);
	}
}

public class Supports_type_conversion
{
	[Test]
	public void via_TypeConverter_registered_with_attribute()
		=> typeof(StringId).Should().HaveTypeConverterDefined();

	[Test]
	public void from_null_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.FromNull<string>().To<StringId>().Should().Be(StringId.Empty);
		}
	}

	[Test]
	public void from_empty_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.From(string.Empty).To<StringId>().Should().Be(StringId.Empty);
		}
	}

	[Test]
	public void from_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.From("Qowaiv-ID").To<StringId>().Should().Be(Svo.StringId);
		}
	}

	[Test]
	public void to_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.ToString().From(Svo.StringId).Should().Be("Qowaiv-ID");
		}
	}
}

#if NET8_0_OR_GREATER
#else
public class Supports_binary_serialization
{
	[Test]
	[Obsolete("Usage of the binary formatter is considered harmful.")]
	public void using_BinaryFormatter()
	{
		var round_tripped = SerializeDeserialize.Binary(Svo.StringId);
		round_tripped.Should().Be(Svo.StringId);
	}

	[Test]
	public void storing_value_in_SerializationInfo()
	{
		var info = Serialize.GetInfo(Svo.StringId);
		info.GetString("Value").Should().Be("Qowaiv-ID");
	}
}
#endif

public class Is_Open_API_data_type
{
	[Test]
	public void with_info()
		=> Qowaiv.OpenApi.OpenApiDataType.FromType(typeof(ForString))
		.Should().Be(new Qowaiv.OpenApi.OpenApiDataType(
			dataType: typeof(StringId),
			description: "String based identifier",
			example: "Order-UK-2022-215",
			type: "string",
			format: "identifier",
			nullable: true));
}
