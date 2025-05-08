using Specs_Generated;

namespace Specs.Customization.UuidBasedId_specs;

public class With_domain_logic
{
    [Test]
    public void Next_generates_unique_ids()
        => Enumerable
            .Range(0, 100)
            .Select(_ => UuidBasedId.Next())
            .ToHashSet()
            .Should().HaveCount(100);
}

public class Has_constant
{
    [Test]
    public void Empty_represent_default_value()
        => UuidBasedId.Empty.Should().Be(default);

    [TestCase(true, "Qowaiv_SVOLibrary_GUIA")]
    [TestCase(false, "")]
    public void HasValue_is(bool result, UuidBasedId svo) => svo.HasValue.Should().Be(result);
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.Generated.CustomUuid.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.Generated.CustomUuid.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.Generated.CustomUuid.Equals(UuidBasedId.Next()).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.Generated.CustomUuid.Equals(UuidBasedId.Parse("Qowaiv_SVOLibrary_GUIA")).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (Svo.Generated.CustomUuid == UuidBasedId.Parse("Qowaiv_SVOLibrary_GUIA")).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (Svo.Generated.CustomUuid == UuidBasedId.Next()).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (Svo.Generated.CustomUuid != UuidBasedId.Parse("Qowaiv_SVOLibrary_GUIA")).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (Svo.Generated.CustomUuid != UuidBasedId.Next()).Should().BeTrue();

    [TestCase("", 0)]
    [TestCase("Qowaiv_SVOLibrary_GUIA", -479411820)]
    public void hash_code_is_value_based(UuidBasedId svo, int hashCode)
    {
        using (Hash.WithoutRandomizer())
        {
            svo.GetHashCode().Should().Be(hashCode);
        }
    }
}

public class Bytes
{
    [TestCase("")]
    [TestCase("Qowaiv_SVOLibrary_GUIA", (byte)0x42, (byte)0x8C, (byte)0x1A, (byte)0x8A, (byte)0xFF, (byte)0xD2, (byte)0x54, (byte)0xE2, (byte)0xE2, (byte)0x6E, (byte)0xB6, (byte)0xAB, (byte)0xCB, (byte)0xF1, (byte)0x94, (byte)0x20)]
    public void describe_the_id(UuidBasedId svo, params byte[] bytes)
         => svo.ToByteArray().Should().BeEquivalentTo(bytes);

    [TestCase("")]
    [TestCase("Qowaiv_SVOLibrary_GUIA", (byte)0x42, (byte)0x8C, (byte)0x1A, (byte)0x8A, (byte)0xFF, (byte)0xD2, (byte)0x54, (byte)0xE2, (byte)0xE2, (byte)0x6E, (byte)0xB6, (byte)0xAB, (byte)0xCB, (byte)0xF1, (byte)0x94, (byte)0x20)]
    public void init_the_id(UuidBasedId svo, params byte[] bytes)
        => UuidBasedId.FromBytes(bytes)
        .Should().Be(svo);
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.Generated.CustomUuid.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_UuidBasedId_as_object()
    {
        object obj = Svo.Generated.CustomUuid;
        Svo.Generated.CustomUuid.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_UuidBasedId_only()
        => new object().Invoking(Svo.Generated.CustomUuid.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new[]
        {
            UuidBasedId.Empty,
            UuidBasedId.Parse("33ef5805-c472-4b1f-88bb-2f0723c43889"),
            UuidBasedId.Parse("58617a65-2a14-4a9a-82a8-c1a82c956c25"),
            UuidBasedId.Parse("853634b4-e474-4b0f-b9ba-01fc732b56d8"),
            UuidBasedId.Parse("93ca7b43-8fb3-44e5-a21f-feeebb8e0f6f"),
            UuidBasedId.Parse("f5e6c39a-adcf-4eca-bcf2-6b8317ac502c"),
        };

        var list = new List<UuidBasedId> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(UuidBasedId).Should().BeDecoratedWith<TypeConverterAttribute>();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<UuidBasedId>().Should().Be(UuidBasedId.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<UuidBasedId>().Should().Be(UuidBasedId.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("Qowaiv_SVOLibrary_GUIA").To<UuidBasedId>().Should().Be(Svo.Generated.CustomUuid);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.Generated.CustomUuid).Should().Be("Qowaiv_SVOLibrary_GUIA");
        }
    }

    [Test]
    public void from_Guid()
        => Converting.From(Svo.Guid).To<UuidBasedId>().Should().Be(Svo.Generated.CustomUuid);


    [Test]
    public void from_Uuid()
        => Converting.From(Svo.Uuid).To<UuidBasedId>().Should().Be(Svo.Generated.CustomUuid);

    [Test]
    public void to_Guid()
        => Converting.To<Guid>().From(Svo.Generated.CustomUuid).Should().Be(Svo.Guid);

    [Test]
    public void to_Uuid()
        => Converting.To<Uuid>().From(Svo.Generated.CustomUuid).Should().Be(Svo.Uuid);
}

public class Supports_JSON_serialization
{
    [Test]
    public void writes_null_for_default_value()
        => JsonTester.Write(default(UuidBasedId)).Should().BeNull();

    [Test]
    public void writes_Base64_string_for_non_default_value()
        => JsonTester.Write(Svo.Generated.CustomUuid).Should().Be("Qowaiv_SVOLibrary_GUIA");

#if NET6_0_OR_GREATER

    [Test]
    public void System_Text_JSON_deserialization_of_dictionary_keys()
    {
        System.Text.Json.JsonSerializer.Deserialize<Dictionary<UuidBasedId, int>>(@"{""Qowaiv_SVOLibrary_GUIA"":42}")
            .Should().BeEquivalentTo(new Dictionary<UuidBasedId, int>()
            {
                [Svo.Generated.CustomUuid] = 42,
            });
    }

    [Test]
    public void System_Text_JSON_serialization_of_dictionary_keys()
    {
        var dictionary = new Dictionary<UuidBasedId, int>()
        {
            [default] = 17,
            [Svo.Generated.CustomUuid] = 42,
        };
        System.Text.Json.JsonSerializer.Serialize(dictionary)
            .Should().Be(@"{"""":17,""Qowaiv_SVOLibrary_GUIA"":42}");
    }
#endif
}
