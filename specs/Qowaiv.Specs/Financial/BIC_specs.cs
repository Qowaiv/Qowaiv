namespace Financial.BIC_specs;

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(BusinessIdentifierCode).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From<string>(null).To<BusinessIdentifierCode>().Should().Be(default);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From(string.Empty).To<BusinessIdentifierCode>().Should().Be(default);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From("AEGONL2UXXX").To<BusinessIdentifierCode>().Should().Be(Svo.BusinessIdentifierCode);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.ToString().From(Svo.BusinessIdentifierCode).Should().Be("AEGONL2UXXX");
        }
    }
}

public class Is_valid
{
    [TestCase("PSTBNL21")]
    [TestCase("ABNANL2A")]
    [TestCase("BACBBEBB")]
    [TestCase("GEBABEBB36A")]
    [TestCase("DEUTDEFF")]
    [TestCase("NEDSZAJJ")]
    [TestCase("DABADKKK")]
    [TestCase("UNCRIT2B912")]
    [TestCase("DSBACNBXSHA")]
    [TestCase("AAAANLBË")]
    public void For(string str) 
        => BusinessIdentifierCode.Parse(str).IsEmptyOrUnknown().Should().BeFalse();
}

public class Is_invalid
{
    [TestCase("1AAANL01", "digit in first four characters")]
    [TestCase("AAAANLBB1", "Branch length of 1")]
    [TestCase("AAAANLBB12", "Branch length of 2")]
    [TestCase("ABCDXX01", "Not existing country")]
    public void For(string str, string because)
        => BusinessIdentifierCode.TryParse(str).Should().BeNull(because);
}

public class Supports_JSON_serialization
{
#if NET6_0_OR_GREATER
    [TestCase(null, null)]
    [TestCase("AEGONL2UXXX", "AEGONL2UXXX")]
    public void System_Text_JSON_deserialization(object json, BusinessIdentifierCode svo)
        => JsonTester.Read_System_Text_JSON<BusinessIdentifierCode>(json).Should().Be(svo);
    
    [TestCase(null, null)]
    [TestCase("AEGONL2UXXX", "AEGONL2UXXX")]
    public void System_Text_JSON_serialization(BusinessIdentifierCode svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [TestCase("AEGONL2UXXX", "AEGONL2UXXX")]
    public void convention_based_deserialization(object json, BusinessIdentifierCode svo)
        => JsonTester.Read<BusinessIdentifierCode>(json).Should().Be(svo);
        
    [TestCase(null, null)]
    [TestCase("AEGONL2UXXX", "AEGONL2UXXX")]
    public void convention_based_serialization(BusinessIdentifierCode svo, object json)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("2017-06-11", typeof(FormatException))]
    [TestCase(true, typeof(InvalidOperationException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
    {
        var exception = Assert.Catch(() => JsonTester.Read<BusinessIdentifierCode>(json));
        Assert.IsInstanceOf(exceptionType, exception);
    }
}
