namespace HouseNumber_specs;

public class With_domain_logic
{
    [TestCase(true, 123456789)]
    [TestCase(true, "?")]
    [TestCase(false, "")]
    public void HasValue_is(bool result, HouseNumber svo) => svo.HasValue.Should().Be(result);

    [TestCase(true, 123456789)]
    [TestCase(false, "?")]
    [TestCase(false, "")]
    public void IsKnown_is(bool result, HouseNumber svo) => svo.IsKnown.Should().Be(result);
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.HouseNumber.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.HouseNumber.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.HouseNumber.Equals(HouseNumber.MinValue).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.HouseNumber.Equals(HouseNumber.Create(123456789)).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (HouseNumber.Create(123456789) == Svo.HouseNumber).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (HouseNumber.Create(123456789) == HouseNumber.MinValue).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (HouseNumber.Create(123456789) != Svo.HouseNumber).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (HouseNumber.Create(123456789) != HouseNumber.MinValue).Should().BeTrue();

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

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.HouseNumber.CompareTo(Nil.Object).Should().Be(1);
}
public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(HouseNumber).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<HouseNumber>().Should().Be(HouseNumber.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<HouseNumber>().Should().Be(HouseNumber.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("123456789").To<HouseNumber>().Should().Be(Svo.HouseNumber);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.HouseNumber).Should().Be("123456789");
        }
    }

    [Test]
    public void from_long()
        => Converting.From(123456789L).To<HouseNumber>().Should().Be(Svo.HouseNumber);

    [Test]
    public void to_long()
        => Converting.To<long>().From(Svo.HouseNumber).Should().Be(123456789L);
}

public class Supports_JSON_serialization
{
#if NET6_0_OR_GREATER
    [TestCase(null, null)]
    [TestCase("?", "?")]
    [TestCase("17", "17")]
    [TestCase(17d, "17")]
    [TestCase(17L, "17")]
    public void System_Text_JSON_deserialization(object json, HouseNumber svo)
        => JsonTester.Read_System_Text_JSON<HouseNumber>(json).Should().Be(svo);

    [TestCase(null, null)]
    [TestCase(17, "17")]
    public void System_Text_JSON_serialization(HouseNumber svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [TestCase("?", "?")]
    [TestCase("17", "17")]
    [TestCase(17d, "17")]
    [TestCase(17L, "17")]
    public void convention_based_deserialization(HouseNumber svo, object json)
        => JsonTester.Read<HouseNumber>(json).Should().Be(svo);

    [TestCase(null, null)]
    [TestCase(17, "17")]
    public void convention_based_serialization(HouseNumber svo, object json)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("2017-06-11", typeof(FormatException))]
    [TestCase(true, typeof(InvalidOperationException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
        => json
            .Invoking(JsonTester.Read<HouseNumber>)
            .Should().Throw<Exception>()
            .And.Should().BeOfType(exceptionType);
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
       => OpenApiDataType.FromType(typeof(HouseNumber))
       .Should().Be(new OpenApiDataType(
           dataType: typeof(HouseNumber),
           description: "House number notation.",
           example: "13",
           type: "string",
           format: "house-number",
           nullable: true));
}
