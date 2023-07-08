namespace Security.Secret_specs;

public class Cryptographic_seed_can_be_created
{
    [Test]
    public void from_empty_secret()
    {
        var algorithm = MD5.Create();
        Secret.Empty.ComputeHash(algorithm).ToByteArray()
            .Should().BeEquivalentTo(new byte[] { 0xD4, 0x1D, 0x8C, 0xD9, 0x8F, 0x00, 0xB2, 0x04, 0xE9, 0x80, 0x09, 0x98, 0xEC, 0xF8, 0x42, 0x7E });
    }

    [Test]
    public void from_none_empty_secret()
    {
        var algorithm = MD5.Create();
        Svo.Secret.ComputeHash(algorithm).ToByteArray()
            .Should().BeEquivalentTo(new byte[] { 0xB5, 0x95, 0xC7, 0x15, 0x1A, 0x69, 0x18, 0x94, 0x8D, 0xAE, 0xAE, 0xE4, 0xA8, 0xA2, 0xEB, 0x70 });
    }
}

public class With_domain_logic
{
    [TestCase(false, "Ken sent me!")]
    [TestCase(true, "")]
    public void IsEmpty_returns(bool result, Secret svo)
        => svo.IsEmpty().Should().Be(result);

    [Test]
    public void value_accessable_via_Value_method()
        => Svo.Secret.Value().Should().Be("Ken sent me!");
}

public class Has_constant
{
    [Test]
    public void Empty_represent_default_value()
        => Secret.Empty.Should().Be(default(Secret));
}

public class equality_is_limited_to_empty
{
    [Test]
    public void not_equal_to_null()
        => Svo.Secret.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.Secret.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.Secret.Equals(Secret.Parse("different")).Should().BeFalse();

    [Test]
    public void not_equal_to_same_value()
        => Svo.Secret.Equals(Secret.Parse("Ken sent me!")).Should().BeFalse();

    [Test]
    public void equal_to_for_two_empties()
        => Secret.Empty.Equals(Secret.Empty).Should().BeTrue();
}

public class Hashing
{
    [Test]
    public void is_not_supported()
        => Svo.Secret.Invoking(x => x.GetHashCode())
        .Should().Throw<HashingNotSupported>();
}

public class Can_be_parsed
{
    [Test]
    public void from_null_string_represents_Empty()
    {
        Assert.AreEqual(Secret.Empty, Secret.Parse(null));
    }

    [Test]
    public void from_empty_string_represents_Empty()
    {
        Assert.AreEqual(Secret.Empty, Secret.Parse(string.Empty));
    }
}

public class Supports_type_conversion_from
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(Secret).Should().HaveTypeConverterDefined();

    [Test]
    public void null_string()
        => Converting.FromNull<string>().To<Secret>().Should().Be(Secret.Empty);

    [Test]
    public void empty_string()
        => Converting.From(string.Empty).To<Secret>().Should().Be(Secret.Empty);

    [Test]
    public void @string()
        => Converting.From("Ken sent me!").To<Secret>().Value()
        .Should().Be("Ken sent me!");
}

public class Does_not_support_type_converstion_to
{
    [Test]
    public void convertered_is_null()
        => Converting.ToString().From(Svo.Secret).Should().BeNull();

    [TestCase(typeof(string))]
    [TestCase(typeof(byte[]))]
    public void can_convert_to_always_return_false(Type type)
        => new SecretTypeConverter().CanConvertTo(type).Should().BeFalse();
}

public class Supports_JSON_deserialization
{
#if NET6_0_OR_GREATER
    [Test]
    public void System_Text_JSON_deserialization()
         => JsonTester.Read_System_Text_JSON<Secret>("Ken sent me!").Value().Should().Be(Svo.Secret.Value());
#endif
    [Test]
    public void convention_based_deserialization()
        => JsonTester.Read<Secret>("Ken sent me!").Value().Should().Be(Svo.Secret.Value());
}

public class Does_not_supports_JSON_serialization
{
#if NET6_0_OR_GREATER
    [Test]
    public void serializes_to_null_System_Text_JSON()
        => JsonTester.Write_System_Text_JSON(Svo.Secret).Should().BeNull();
#endif
    [Test]
    public void serializes_to_null()
        => JsonTester.Write(Svo.Secret).Should().BeNull();
}

public class ToString
{
    [Test]
    public void does_not_reveal_content()
        => Svo.Secret.ToString().Should().Be("*****");

    [Test]
    public void does_not_reveal_content_for_empty()
       => Secret.Empty.ToString().Should().Be(string.Empty);
}

public class Debugger
{
    [TestCase("{empty}", "")]
    [TestCase("Ken sent me!", "Ken sent me!")]
    public void has_custom_display(object display, Secret svo) => svo.Should().HaveDebuggerDisplay(display);
}
