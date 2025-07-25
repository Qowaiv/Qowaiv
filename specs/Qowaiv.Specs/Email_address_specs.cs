namespace Email_address_specs;

public class With_domain_logic
{
    [TestCase(true, "info@qowaiv.org")]
    [TestCase(true, "?")]
    [TestCase(false, "")]
    public void HasValue_is(bool result, EmailAddress svo) => svo.HasValue.Should().Be(result);

    [TestCase(true, "info@qowaiv.org")]
    [TestCase(false, "?")]
    [TestCase(false, "")]
    public void IsKnown_is(bool result, EmailAddress svo) => svo.IsKnown.Should().Be(result);

    [TestCase("")]
    [TestCase("?")]
    public void has_length_zero_for_empty_and_unknown(EmailAddress svo)
        => svo.Length.Should().Be(0);

    [TestCase(15, "info@qowaiv.org")]
    public void has_length(int length, EmailAddress svo)
        => svo.Length.Should().Be(length);

    [TestCase(false, "info@qowaiv.org")]
    [TestCase(false, "?")]
    [TestCase(true, "")]
    public void IsEmpty_returns(bool result, EmailAddress svo)
        => svo.IsEmpty().Should().Be(result);

    [TestCase(false, "info@qowaiv.org")]
    [TestCase(true, "?")]
    [TestCase(true, "")]
    public void IsEmptyOrUnknown_returns(bool result, EmailAddress svo)
            => svo.IsEmptyOrUnknown().Should().Be(result);

    [TestCase(false, "info@qowaiv.org")]
    [TestCase(true, "?")]
    [TestCase(false, "")]
    public void IsUnknown_returns(bool result, EmailAddress svo)
        => svo.IsUnknown().Should().Be(result);

    [TestCase(false, "info@qowaiv.org")]
    [TestCase(true, "info@[192.0.2.1]")]
    [TestCase(false, "?")]
    [TestCase(false, "")]
    public void IsIPBased_returns(bool result, EmailAddress svo)
        => svo.IsIPBased.Should().Be(result);

    [TestCase("info@qowaiv.org", "255.255.255.255")]
    [TestCase("info@[192.0.2.1]", "192.0.2.1")]
    [TestCase("info@[IPv6:2001:0db8:0000:0000:0000:ff00:0042:8329]", "2001:db8::ff00:42:8329")]
    public void IP_domain(EmailAddress email, string address)
        => email.IPDomain.ToString().Should().Be(address);

    [TestCase("info", "info@qowaiv.org")]
    [TestCase("", "?")]
    [TestCase("", "")]
    public void Local_part_returns(string local, EmailAddress email)
        => email.Local.Should().Be(local);

    [TestCase("qowaiv.org", "info@qowaiv.org")]
    [TestCase("[192.0.2.1]", "info@192.0.2.1")]
    [TestCase("", "?")]
    [TestCase("", "")]
    public void Domain_part_returns(string local, EmailAddress email)
        => email.Domain.Should().Be(local);
}

public class Has_constant
{
    [Test]
    public void Empty_represent_default_value()
        => EmailAddress.Empty.Should().Be(default);
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
    {
        Svo.EmailAddress.Equals(null).Should().BeFalse();
    }

    [Test]
    public void not_equal_to_other_type()
    {
        Svo.EmailAddress.Equals(new object()).Should().BeFalse();
    }

    [Test]
    public void not_equal_to_different_value()
    {
        Svo.EmailAddress.Equals(EmailAddress.Parse("no_spam@qowaiv.org")).Should().BeFalse();
    }

    [Test]
    public void equal_to_same_value()
    {
        Svo.EmailAddress.Equals(EmailAddress.Parse("info@qowaiv.org")).Should().BeTrue();
    }

    [Test]
    public void equal_operator_returns_true_for_same_values()
    {
        (Svo.EmailAddress == EmailAddress.Parse("info@qowaiv.org")).Should().BeTrue();
    }

    [Test]
    public void equal_operator_returns_false_for_different_values()
    {
        (Svo.EmailAddress == EmailAddress.Parse("no_spam@qowaiv.org")).Should().BeFalse();
    }

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
    {
        (Svo.EmailAddress != EmailAddress.Parse("info@qowaiv.org")).Should().BeFalse();
    }

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
    {
        (Svo.EmailAddress != EmailAddress.Parse("no_spam@qowaiv.org")).Should().BeTrue();
    }

    [TestCase("", 0)]
    [TestCase("info@qowaiv.org", 798543550)]
    public void hash_code_is_value_based(EmailAddress svo, int hashCode)
    {
        using (Hash.WithoutRandomizer())
        {
            svo.GetHashCode().Should().Be(hashCode);
        }
    }
}

public class Can_be_parsed
{
    [Test]
    public void from_null_string_represents_Empty()
        => EmailAddress.Parse(null).Should().Be(EmailAddress.Empty);

    [Test]
    public void from_empty_string_represents_Empty()
        => EmailAddress.Parse(string.Empty).Should().Be(EmailAddress.Empty);

    [TestCase("  ")]
    [TestCase("\r")]
    [TestCase("\t  ")]
    [TestCase("\t  \n")]
    public void from_whitespace_represents_Empty(string ws)
        => EmailAddress.Parse(ws).Should().Be(EmailAddress.Empty);

    [Test]
    public void from_question_mark_represents_Unknown()
        => EmailAddress.Parse("?").Should().Be(EmailAddress.Unknown);

    [TestCase("en", "info@qowaiv.org")]
    public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
    {
        using (culture.Scoped())
        {
            var parsed = EmailAddress.Parse(input);
            parsed.Should().Be(Svo.EmailAddress);
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_throws_on_Parse()
    {
        using (TestCultures.en_GB.Scoped())
        {
            "invalid input".Invoking(EmailAddress.Parse)
                .Should().Throw<FormatException>()
                .WithMessage("Not a valid email address");
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_return_false_on_TryParse()
    {
        EmailAddress.TryParse("invalid input", out _).Should().BeFalse();
    }

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => EmailAddress.TryParse("invalid input").Should().BeNull();

    [Test]
    public void with_TryParse_returns_SVO()
    {
        EmailAddress.TryParse("info@qowaiv.org").Should().Be(Svo.EmailAddress);
    }
}

public class Has_custom_formatting
{
    [Test]
    public void _default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.EmailAddress.ToString().Should().Be("info@qowaiv.org");
        }
    }

    [Test]
    public void with_null_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.EmailAddress.ToString(default(string)).Should().Be(Svo.EmailAddress.ToString());
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.EmailAddress.ToString(string.Empty).Should().Be(Svo.EmailAddress.ToString());
        }
    }

    [Test]
    public void default_value_is_represented_as_string_empty()
    {
        default(EmailAddress).ToString().Should().Be(string.Empty);
    }

    [Test]
    public void unknown_value_is_represented_as_unknown()
    {
        EmailAddress.Unknown.ToString().Should().Be("?");
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.EmailAddress.ToString("f", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: 'info@qowaiv.org', format: 'f'");
    }

    [TestCase("en-GB", null, "info@qowaiv.org", "info@qowaiv.org")]
    [TestCase("nl-BE", "f", "info@qowaiv.org", "info@qowaiv.org")]
    public void culture_dependent(CultureInfo culture, string format, EmailAddress svo, string expected)
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
            Svo.EmailAddress.ToString(provider: null).Should().Be("info@qowaiv.org");
        }
    }

    [TestCase("info@qowaiv.org", null)]
    [TestCase("info@qowaiv.org", "")]
    [TestCase("info@qowaiv.org", "f")]
    [TestCase("INFO@QOWAIV.ORG", "U")]
    [TestCase("INFO", "L")]
    [TestCase("info", "l")]
    [TestCase("qowaiv.org", "d")]
    [TestCase("QOWAIV.ORG", "D")]
    [TestCase("info@qowaiv.org", "l@d")]
    public void with_format(string formatted, string format)
        => Svo.EmailAddress.ToString(format).Should().Be(formatted);
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.EmailAddress.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_EmailAddress_as_object()
    {
        object obj = Svo.EmailAddress;
        Svo.EmailAddress.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_EmailAddress_only()
        => new object().Invoking(Svo.EmailAddress.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new[]
        {
                default,
                default,
                EmailAddress.Unknown,
                EmailAddress.Parse("info@qowaiv.com"),
                EmailAddress.Parse("info@qowaiv.org"),
                EmailAddress.Parse("spam@qowaiv.org"),
            };
        var list = new List<EmailAddress> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(EmailAddress).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<EmailAddress>().Should().Be(EmailAddress.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<EmailAddress>().Should().Be(EmailAddress.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("info@qowaiv.org").To<EmailAddress>().Should().Be(Svo.EmailAddress);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.EmailAddress).Should().Be("info@qowaiv.org");
        }
    }
}

public class Supports_JSON_serialization
{
#if NET6_0_OR_GREATER
    [TestCase("?", "?")]
    [TestCase("info@qowaiv.org", "info@qowaiv.org")]
    public void System_Text_JSON_deserialization(object json, EmailAddress svo)
        => JsonTester.Read_System_Text_JSON<EmailAddress>(json).Should().Be(svo);

    [TestCase(null, null)]
    [TestCase("info@qowaiv.org", "info@qowaiv.org")]
    public void System_Text_JSON_serialization(object json, EmailAddress svo)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);

    [Test]
    public void System_Text_JSON_deserialization_of_dictionary_keys()
    {
        System.Text.Json.JsonSerializer.Deserialize<Dictionary<EmailAddress, int>>(@"{""info@qowaiv.org"":42}")
            .Should().BeEquivalentTo(new Dictionary<EmailAddress, int>()
            {
                [Svo.EmailAddress] = 42,
            });
    }

    [Test]
    public void System_Text_JSON_serialization_of_dictionary_keys()
    {
        var dictionary = new Dictionary<EmailAddress, int>()
        {
            [default] = 17,
            [Svo.EmailAddress] = 42,
        };
        System.Text.Json.JsonSerializer.Serialize(dictionary)
            .Should().Be(@"{"""":17,""info@qowaiv.org"":42}");
    }
#endif

    [TestCase("?", "?")]
    [TestCase("info@qowaiv.org", "info@qowaiv.org")]
    public void convention_based_deserialization(object json, EmailAddress svo)
        => JsonTester.Read<EmailAddress>(json).Should().Be(svo);

    [TestCase(null, null)]
    [TestCase("info@qowaiv.org", "info@qowaiv.org")]
    public void convention_based_serialization(object json, EmailAddress svo)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("2017-06-11", typeof(FormatException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
        => json
            .Invoking(JsonTester.Read<EmailAddress>)
            .Should().Throw<Exception>()
            .And.Should().BeOfType(exceptionType);
}

public class Supports_XML_serialization
{
    [Test]
    public void using_XmlSerializer_to_serialize()
    {
        var xml = Serialize.Xml(Svo.EmailAddress);
        xml.Should().Be("info@qowaiv.org");
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<EmailAddress>("info@qowaiv.org");
        svo.Should().Be(Svo.EmailAddress);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.EmailAddress);
        round_tripped.Should().Be(Svo.EmailAddress);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.EmailAddress);
        var round_tripped = SerializeDeserialize.Xml(structure);
        round_tripped.Should().Be(structure);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.EmailAddress;
        obj.GetSchema().Should().BeNull();
    }
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
       => OpenApiDataType.FromType(typeof(EmailAddress))
       .Should().Be(new OpenApiDataType(
           dataType: typeof(EmailAddress),
           description: "Email notation as defined by RFC 5322.",
           type: "string",
           example: "svo@qowaiv.org",
           format: "email",
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
        var round_tripped = SerializeDeserialize.Binary(Svo.EmailAddress);
        round_tripped.Should().Be(Svo.EmailAddress);
    }

    [Test]
    public void storing_string_in_SerializationInfo()
    {
        var info = Serialize.GetInfo(Svo.EmailAddress);
        info.GetString("Value").Should().Be("info@qowaiv.org");
    }
}
#endif

public class Debugger
{
    [TestCase("{empty}", "")]
    [TestCase("{unknown}", "?")]
    [TestCase("info@qowaiv.org", "info@qowaiv.org")]
    public void has_custom_display(object display, EmailAddress svo)
        => svo.Should().HaveDebuggerDisplay(display);
}

