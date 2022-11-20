namespace Sql.Timestamp_specs;

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.Timestamp.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.Timestamp.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.Timestamp.Equals(Timestamp.MinValue).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.Timestamp.Equals(Timestamp.Create(1234567890L)).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (Timestamp.Create(1234567890L) == Svo.Timestamp).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (Timestamp.Create(1234567890L) == Timestamp.MinValue).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (Timestamp.Create(1234567890L) != Svo.Timestamp).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (Timestamp.Create(1234567890L) != Timestamp.MinValue).Should().BeTrue();

    [TestCase("0", 0)]
    [TestCase("1234567890", 1849341697)]
    public void hash_code_is_value_based(Timestamp svo, int hash)
    {
        using (Hash.WithoutRandomizer())
        {
            svo.GetHashCode().Should().Be(hash);
        }
    }
}

public class Can_be_created
{
    [Test]
    public void from_byte_arrays_with_length_8()
        => Timestamp.Create(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }).Should().Be((Timestamp)578437695752307201L);

    [Test]
    public void from_negative_numbers()
        => Timestamp.Create(-23).Should().Be((Timestamp)18446744073709551593L);
}

public class Can_not_be_created
{
    [TestCase(7)]
    [TestCase(9)]
    public void form_byte_arrays_with_a_length_other_than_8(int length)
    {
        Func<Timestamp> create = () => Timestamp.Create(new byte[length]);
        create.Should().Throw<ArgumentException>()
           .WithMessage("The byte array should have size of 8.*");
    }
}

public class Supports_JSON_serialization
{
#if NET6_0_OR_GREATER
    [TestCase(null, null)]
    [TestCase(1849341697d, "0x000000006E3AB701")]
    [TestCase(1849341697L, "0x000000006E3AB701")]
    [TestCase("1849341697", "0x000000006E3AB701")]
    [TestCase("0x000000006E3AB701", "0x000000006E3AB701")]
    public void System_Text_JSON_deserialization(object json, Timestamp svo)
        => JsonTester.Read_System_Text_JSON<Timestamp>(json).Should().Be(svo);

    [TestCase(null, "0x0000000000000000")]
    [TestCase("0x000000006E3AB701", "0x000000006E3AB701")]
    public void System_Text_JSON_serialization(Timestamp svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [TestCase(1849341697d, "0x000000006E3AB701")]
    [TestCase(1849341697L, "0x000000006E3AB701")]
    [TestCase("1849341697", "0x000000006E3AB701")]
    [TestCase("0x000000006E3AB701", "0x000000006E3AB701")]
    public void convention_based_deserialization(object json, Timestamp svo)
        => JsonTester.Read<Timestamp>(json).Should().Be(svo);

    [TestCase(null, "0x0000000000000000")]
    [TestCase("0x000000006E3AB701", "0x000000006E3AB701")]
    public void convention_based_serialization(Timestamp svo, object json)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("2017-06-11", typeof(FormatException))]
    [TestCase(true, typeof(InvalidOperationException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
    {
        var exception = Assert.Catch(() => JsonTester.Read<Timestamp>(json));
        Assert.IsInstanceOf(exceptionType, exception);
    }
}


public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
        => Qowaiv.OpenApi.OpenApiDataType.FromType(typeof(Timestamp))
        .Should().Be(new Qowaiv.OpenApi.OpenApiDataType(
            dataType: typeof(Timestamp),
            description: "SQL Server timestamp notation.",
            example: "0x00000000000007D9",
            type: "string",
            format: "timestamp"));
}
