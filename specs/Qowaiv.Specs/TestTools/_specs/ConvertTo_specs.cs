namespace TestTools.ConvertTo_specs;

public class From_type
{
    [Test]
    public void tests_CanConvertTo()
        => Converting.To(typeof(string)).From<int>().Should().BeTrue();
}

public class From_typed_value
{
    [Test]
    public void tests_ConvertToString()
       => Converting.To<string>().From(17).Should().Be("17");

    [Test]
    public void tests_ConvertTo()
       => Converting.To<long>().From(17).Should().Be(17);
}
