namespace Month_specs;

public class With_domain_logic
{
    [TestCase(false, "February")]
    [TestCase(false, "?")]
    [TestCase(true, "")]
    public void IsEmpty_returns(bool result, Month svo)
    {
        Assert.AreEqual(result, svo.IsEmpty());
    }

    [TestCase(false, "February")]
    [TestCase(true, "?")]
    [TestCase(true, "")]
    public void IsEmptyOrUnknown_returns(bool result, Month svo)
    {
        Assert.AreEqual(result, svo.IsEmptyOrUnknown());
    }

    [TestCase(false, "February")]
    [TestCase(true, "?")]
    [TestCase(false, "")]
    public void IsUnknown_returns(bool result, Month svo)
    {
        Assert.AreEqual(result, svo.IsUnknown());
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
        Assert.AreEqual(days, month.Days(year));
    }
}

public class Short_name
{
    [Test]
    public void is_string_empty_for_empty()
    {
        Assert.AreEqual(string.Empty, Month.Empty.ShortName);
    }
    [Test]
    public void is_question_mark_for_unknown()
    {
        Assert.AreEqual("?", Month.Unknown.ShortName);
    }
    [Test]
    public void picks_current_culture()
    {
        using (TestCultures.Nl_BE.Scoped())
        {
            Assert.AreEqual("feb.", Svo.Month.ShortName);
        }
    }
    [Test]
    public void supports_custom_culture()
    {
        Assert.AreEqual("feb.", Svo.Month.GetShortName(TestCultures.Nl_BE));
    }
}

public class Full_name
{
    [Test]
    public void is_string_empty_for_empty()
    {
        Assert.AreEqual(string.Empty, Month.Empty.FullName);
    }
    [Test]
    public void is_question_mark_for_unknown()
    {
        Assert.AreEqual("?", Month.Unknown.FullName);
    }
    [Test]
    public void picks_current_culture()
    {
        using (TestCultures.Nl_BE.Scoped())
        {
            Assert.AreEqual("februari", Svo.Month.FullName);
        }
    }
    [Test]
    public void supports_custom_culture()
    {
        Assert.AreEqual("februari", Svo.Month.GetFullName(TestCultures.Nl_BE));
    }
}

public class Is_valid_for
{
    [TestCase("?")]
    [TestCase("unknown")]
    public void strings_representing_unknown(string input)
    {
        Assert.IsTrue(Month.IsValid(input));
    }

    [TestCase("February", "nl")]
    [TestCase("February", "nl")]
    public void strings_representing_SVO(string input, CultureInfo culture)
    {
        Assert.IsTrue(Month.IsValid(input, culture));
    }
}

public class Is_not_valid_for
{
    [Test]
    public void string_empty()
    {
        Assert.IsFalse(Month.IsValid(string.Empty));
    }

    [Test]
    public void string_null()
    {
        Assert.IsFalse(Month.IsValid((string)null));
    }

    [Test]
    public void whitespace()
    {
        Assert.IsFalse(Month.IsValid(" "));
    }

    [Test]
    public void garbage()
    {
        Assert.IsFalse(Month.IsValid("garbage"));
    }
}

public class Has_constant
{
    [Test]
    public void Empty_represent_default_value()
    {
        Assert.AreEqual(default(Month), Month.Empty);
    }
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
    {
        Assert.IsFalse(Svo.Month.Equals(null));
    }

    [Test]
    public void not_equal_to_other_type()
    {
        Assert.IsFalse(Svo.Month.Equals(new object()));
    }

    [Test]
    public void not_equal_to_different_value()
    {
        Assert.IsFalse(Svo.Month.Equals(Month.December));
    }

    [Test]
    public void equal_to_same_value()
    {
        Assert.IsTrue(Svo.Month.Equals(Month.February));
    }

    [Test]
    public void equal_operator_returns_true_for_same_values()
    {
        Assert.IsTrue(Svo.Month == Month.February);
    }

    [Test]
    public void equal_operator_returns_false_for_different_values()
    {
        Assert.IsFalse(Svo.Month == Month.December);
    }

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
    {
        Assert.IsFalse(Svo.Month != Month.February);
    }

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
    {
        Assert.IsTrue(Svo.Month != Month.December);
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
        Assert.AreEqual(Month.Empty, Month.Parse(null));
    }

    [Test]
    public void from_empty_string_represents_Empty()
    {
        Assert.AreEqual(Month.Empty, Month.Parse(string.Empty));
    }

    [Test]
    public void from_question_mark_represents_Unknown()
    {
        Assert.AreEqual(Month.Unknown, Month.Parse("?"));
    }

    [TestCase("en", "February")]
    [TestCase("en", "02")]
    [TestCase("en", "2")]
    public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
    {
        using (culture.Scoped())
        {
            var parsed = Month.Parse(input);
            Assert.AreEqual(Svo.Month, parsed);
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_throws_on_Parse()
    {
        using (TestCultures.En_GB.Scoped())
        {
            var exception = Assert.Throws<FormatException>(() => Month.Parse("invalid input"));
            Assert.AreEqual("Not a valid month", exception.Message);
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_return_false_on_TryParse()
    {
        Assert.IsFalse(Month.TryParse("invalid input", out _));
    }

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => Month.TryParse("invalid input").Should().BeNull();

    [Test]
    public void with_TryParse_returns_SVO()
    {
        Assert.AreEqual(Svo.Month, Month.TryParse("February"));
    }
}

public class Can_be_created
{
    [Test]
    public void with_TryCreate_returns_SVO()
    {
        Assert.AreEqual(Svo.Month, Month.TryCreate(2));
    }
    [Test]
    public void with_TryCreate_returns_Empty()
    {
        Assert.AreEqual(Month.Empty, Month.TryCreate(null));
    }
}
public class Has_custom_formatting
{
    [Test]
    public void _default()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Assert.AreEqual("February", Svo.Month.ToString());
        }
    }

    [Test]
    public void with_null_pattern_equal_to_default()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Assert.AreEqual(Svo.Month.ToString(), Svo.Month.ToString(default(string)));
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Assert.AreEqual(Svo.Month.ToString(), Svo.Month.ToString(string.Empty));
        }
    }

    [Test]
    public void default_value_is_represented_as_string_empty()
    {
        Assert.AreEqual(string.Empty, default(Month).ToString());
    }

    [Test]
    public void unknown_value_is_represented_as_unknown()
    {
        Assert.AreEqual("?", Month.Unknown.ToString());
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.Month.ToString("f", FormatProvider.CustomFormatter);
        Assert.AreEqual("Unit Test Formatter, value: 'February', format: 'f'", formatted);
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
            Assert.AreEqual(expected, svo.ToString(format));
        }
    }

    [Test]
    public void with_current_thread_culture_as_default()
    {
        using (new CultureInfoScope(culture: TestCultures.Nl_NL, cultureUI: TestCultures.En_GB))
        {
            Assert.AreEqual("februari", Svo.Month.ToString(provider: null));
        }
    }
}

public class Is_comparable
{
    [Test]
    public void to_null()
    {
        Assert.AreEqual(1, Svo.Month.CompareTo(null));
    }

    [Test]
    public void to_Month_as_object()
    {
        object obj = Svo.Month;
        Assert.AreEqual(0, Svo.Month.CompareTo(obj));
    }

    [Test]
    public void to_Month_only()
    {
        Assert.Throws<ArgumentException>(() => Svo.Month.CompareTo(new object()));
    }

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
        Assert.AreEqual(sorted, list);
    }

    [Test]
    public void by_operators_for_different_values()
    {
        Month smaller = Month.February;
        Month bigger = Month.March;
        Assert.IsTrue(smaller < bigger);
        Assert.IsTrue(smaller <= bigger);
        Assert.IsFalse(smaller > bigger);
        Assert.IsFalse(smaller >= bigger);
    }

    [Test]
    public void by_operators_for_equal_values()
    {
        Month left = Month.February;
        Month right = Svo.Month;
        Assert.IsFalse(left < right);
        Assert.IsTrue(left <= right);
        Assert.IsFalse(left > right);
        Assert.IsTrue(left >= right);
    }

    [TestCase("", "February")]
    [TestCase("?", "February")]
    [TestCase("February", "")]
    [TestCase("February", "?")]
    public void by_operators_for_empty_or_unknown_always_false(Month l, Month r)
    {
        Assert.IsFalse(l <= r);
        Assert.IsFalse(l < r);
        Assert.IsFalse(l > r);
        Assert.IsFalse(l >= r);
    }
}

public class Casts
{
    [Test]
    public void explicitly_from_byte()
    {
        var casted = (Month)2;
        Assert.AreEqual(Svo.Month, casted);
    }

    [Test]
    public void explicitly_to_byte()
    {
        var casted = (byte)Svo.Month;
        Assert.AreEqual(2, casted);
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
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From<string>(null).To<Month>().Should().Be(default);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From(string.Empty).To<Month>().Should().Be(default);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From("February").To<Month>().Should().Be(Svo.Month);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.En_GB.Scoped())
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
    [TestCase("?", "unknown")]
    [TestCase("February", 2.0)]
    [TestCase("February", 2L)]
    public void convention_based_deserialization(Month expected, object json)
    {
        var actual = JsonTester.Read<Month>(json);
        Assert.AreEqual(expected, actual);
    }

    [TestCase(null, "")]
    [TestCase("Feb", "February")]
    public void convention_based_serialization(object expected, Month svo)
    {
        var serialized = JsonTester.Write(svo);
        Assert.AreEqual(expected, serialized);
    }

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("2017-06-11", typeof(FormatException))]
    [TestCase(-1L, typeof(ArgumentOutOfRangeException))]
    [TestCase(long.MaxValue, typeof(ArgumentOutOfRangeException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
    {
        var exception = Assert.Catch(() => JsonTester.Read<Month>(json));
        Assert.IsInstanceOf(exceptionType, exception);
    }
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
        Assert.AreEqual(Svo.Month, round_tripped);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.Month);
        var round_tripped = SerializeDeserialize.Xml(structure);
        Assert.AreEqual(structure, round_tripped);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.Month;
        Assert.IsNull(obj.GetSchema());
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

    internal static readonly OpenApiDataTypeAttribute Attribute = OpenApiDataTypeAttribute.From(typeof(Month)).FirstOrDefault();
    
    [Test]
    public void with_description()
    {
        Assert.AreEqual("Month(-only) notation.", Attribute.Description);
    }

    [Test]
    public void has_type()
    {
        Assert.AreEqual("string", Attribute.Type);
    }

    [Test]
    public void has_format()
    {
        Assert.AreEqual("month", Attribute.Format);
    }

    [Test]
    public void pattern_is_null()
    {
        Assert.IsNull(Attribute.Pattern);
    }
}

public class Supports_binary_serialization
{
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void using_BinaryFormatter()
    {
        var round_tripped = SerializeDeserialize.Binary(Svo.Month);
        Assert.AreEqual(Svo.Month, round_tripped);
    }

    [Test]
    public void storing_byte_in_SerializationInfo()
    {
        var info = Serialize.GetInfo(Svo.Month);
        Assert.AreEqual((byte)2, info.GetByte("Value"));
    }
}

public class Debugger
{
    [TestCase("{empty}", "")]
    [TestCase("{unknown}", "?")]
    [TestCase("February (02)", "February")]
    public void has_custom_display(object display, Month svo)
        => svo.Should().HaveDebuggerDisplay(display);
}
