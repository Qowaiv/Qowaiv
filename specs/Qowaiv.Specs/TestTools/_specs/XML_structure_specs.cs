namespace TestTools.XML_structure_specs;

public class Is_equatable
{
    [Test]
    public void with_same_type_only()
        => XmlStructure.New(18).Equals(new object()).Should().BeFalse();

    [TestCase(42, "2017-06-11", false)]
    [TestCase(17, "2042-06-11", false)]
    [TestCase(17, "2017-06-11", true)]
    public void by_value(int id, DateTime date, bool svo)
        => new XmlStructure<bool>()
        .Should().NotBe(new XmlStructure<bool>
        {
            Id = id,
            Date = date,
            Svo = svo,
        });
}

public class With_debugger_experience
{
    [Test]
    public void via_ToString()
        => XmlStructure.New(true).ToString().Should().Be("ID: 17, SVO: True, Date: 2017-06-11T00:00:00.0000000Z");
}
