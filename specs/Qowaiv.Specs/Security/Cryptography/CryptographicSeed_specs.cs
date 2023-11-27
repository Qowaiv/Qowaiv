namespace Security.Cryptography.CryptographicSeed_specs;

public class Computed
{
    [Test]
    public void from_algorithm()
    {
        var algorithm = MD5.Create();
        var seed = algorithm.ComputeCryptographicSeed([0xD4, 0x1D, 0x8C, 0xD9]);
        seed.Value().Should().Be("t4VYHkd9cKuZGqBn8j2XKw==");
    }
}

public class Not_computed
{
    [Test]
    public void from_null_input()
    {
        var algorithm = MD5.Create();
        var seed = algorithm.ComputeCryptographicSeed(null);
        seed.IsEmpty().Should().BeTrue();
    }
}

public class With_domain_logic
{
    [TestCase(true, "Qowaiv==")]
    [TestCase(false, "")]
    public void HasValue_is(bool result, CryptographicSeed svo)
      => svo.HasValue.Should().Be(result);

    [TestCase(false, "Qowaiv==")]
    [TestCase(true, "")]
    public void IsEmpty_returns(bool result, CryptographicSeed svo)
        => svo.IsEmpty().Should().Be(result);

    [Test]
    public void value_accessible_via_Value_method()
        => Svo.CryptographicSeed.Value().Should().Be("Qowaig==");

    [Test]
    public void value_accessible_via_ToByteArray_method()
        => Svo.CryptographicSeed.ToByteArray()
        .Should().BeEquivalentTo(new byte[] { 0x42, 0x8C, 0x1A, 0x8A });
}

public class Has_constant
{
    [Test]
    public void Empty_represent_default_value()
        => CryptographicSeed.Empty.Should().Be(default(CryptographicSeed));
}

public class equality_is_limited_to_empty
{
    [Test]
    public void not_equal_to_null()
        => Svo.CryptographicSeed.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.CryptographicSeed.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.CryptographicSeed.Equals(CryptographicSeed.Create(17, 69)).Should().BeFalse();

    [Test]
    public void not_equal_to_same_value()
        => Svo.CryptographicSeed.Equals(CryptographicSeed.Parse("Qowaiv==")).Should().BeFalse();

    [Test]
    public void equal_to_for_two_empties()
        => CryptographicSeed.Empty.Equals(CryptographicSeed.Empty).Should().BeTrue();
}

public class Hashing
{
    [Test]
    public void is_not_supported()
        => Svo.CryptographicSeed.Invoking(x => x.GetHashCode())
        .Should().Throw<HashingNotSupported>();
}

public class ToByteArray
{
    [Test]
    public void from_empty_is_empty_array()
        => CryptographicSeed.Empty
        .ToByteArray().Should().BeEquivalentTo(Array.Empty<byte>());

    [Test]
    public void from_empty_array_stays_empty_array()
        => CryptographicSeed.Create([])
        .ToByteArray().Should().BeEquivalentTo(Array.Empty<byte>());
}

public class Can_be_parsed
{
    [Test]
    public void from_null_string_represents_Empty()
    {
        CryptographicSeed.Parse(null).Should().Be(CryptographicSeed.Empty);
    }

    [Test]
    public void from_empty_string_represents_Empty()
    {
        CryptographicSeed.Parse(string.Empty).Should().Be(CryptographicSeed.Empty);
    }

    [Test]
    public void from_valid_input_only_otherwise_return_false_on_TryParse()
        => CryptographicSeed.TryParse("input = invalid", out _).Should().BeFalse();

    [Test]
    public void with_TryParse()
    {
        CryptographicSeed.TryParse("Qowaiv==", out var seed).Should().BeTrue();
        seed.Value().Should().Be("Qowaig==");
    }
}

public class Can_not_be_parsed
{
    [Test]
    public void from_non_base64_string()
        => "s&&".Invoking(CryptographicSeed.Parse)
        .Should().Throw<FormatException>()
        .WithMessage("Not a valid cryptographic seed");
}

public class Supports_type_conversion_from
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(CryptographicSeed).Should().HaveTypeConverterDefined();

    [Test]
    public void null_string()
        => Converting.FromNull<string>().To<CryptographicSeed>().Should().Be(CryptographicSeed.Empty);

    [Test]
    public void empty_string()
        => Converting.From(string.Empty).To<CryptographicSeed>().Should().Be(CryptographicSeed.Empty);

    [Test]
    public void @string()
        => Converting.From("Qowaiv==").To<CryptographicSeed>().Value()
        .Should().Be("Qowaig==");

    [Test]
    public void byte_array()
        => Converting.From(new byte[] { 0x42, 0x8C, 0x1A, 0x8A }).To<CryptographicSeed>().Value()
        .Should().Be("Qowaig==");
}

public class Does_not_support_type_conversion_to
{
    [Test]
    public void converted_is_null()
        => Converting.ToString().From(Svo.CryptographicSeed).Should().BeNull();

    [TestCase(typeof(string))]
    [TestCase(typeof(byte[]))]
    public void can_convert_to_always_return_false(Type type)
        => new CryptographicSeedTypeConverter().CanConvertTo(type).Should().BeFalse();
}

public class Supports_JSON_deserialization
{
#if NET6_0_OR_GREATER
    [Test]
    public void System_Text_JSON_deserialization()
        => JsonTester.Read_System_Text_JSON<CryptographicSeed>("Qowaiv==").Value().Should().Be(Svo.CryptographicSeed.Value());
#endif
    [Test]
    public void convention_based_deserialization()
        => JsonTester.Read<CryptographicSeed>("Qowaiv==").Value().Should().Be(Svo.CryptographicSeed.Value());
}

public class Does_not_supports_JSON_serialization
{
#if NET6_0_OR_GREATER
    [Test]
    public void serializes_to_null_System_Text_JSON()
       => JsonTester.Write_System_Text_JSON(Svo.CryptographicSeed).Should().BeNull();
#endif
    [Test]
    public void serializes_to_null()
        => JsonTester.Write(Svo.CryptographicSeed).Should().BeNull();
}

public class ToString
{
    [Test]
    public void does_not_reveal_content()
        => Svo.CryptographicSeed.ToString().Should().Be("*****");

    [Test]
    public void does_not_reveal_content_for_empty()
       => CryptographicSeed.Empty.ToString().Should().Be(string.Empty);
}

public class Debugger
{
    [TestCase("{empty}", "")]
    [TestCase("Qowaig==", "Qowaiv==")]
    public void has_custom_display(object display, CryptographicSeed svo) => svo.Should().HaveDebuggerDisplay(display);
}
