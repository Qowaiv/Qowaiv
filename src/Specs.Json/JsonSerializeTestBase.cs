using AwesomeAssertions.Extensions;
using Qowaiv.Json.UnitTests.Models;

namespace Qowaiv.Json.UnitTests;

/// <remarks>
/// This abstract base class should help to guarantee that different
/// implementations have the same behaviour.
/// </remarks>
public abstract class JsonSerializeTestBase<TException>  where TException: Exception
{
    [Test]
    public void Deserialize_Null_Successful()
        => Deserialize<SvoWithFromJson>(@"null").Should().Be(new SvoWithFromJson(null));

    [Test]
    public void Deserialize_NullNullable_Successful()
        => Deserialize<SvoWithFromJson?>(@"null").Should().BeNull();

    [Test]
    public void Deserialize_NullClass_Successful()
        => Deserialize<SvoWithFromJsonClass>(@"null").Should().BeNull();

    [Test]
    public void Deserialize_String_Successful()
        => Deserialize<SvoWithFromJson>(@"""test""").Should().Be(new SvoWithFromJson("test"));

    [Test]
    public void Deserialize_Long_Successful()
        => Deserialize<SvoWithFromJson>("666").Should().Be(new SvoWithFromJson(666L));

    [Test]
    public void Deserialize_Double_Successful()
        => Deserialize<SvoWithFromJson>("2.5").Should().Be(new SvoWithFromJson(2.5));

    [Test]
    public void Deserialize_True_Successful()
        => Deserialize<SvoWithFromJson>("true").Should().Be(new SvoWithFromJson(true));
        
    [Test]
    public void Deserialize_False_Successful()
        => Deserialize<SvoWithFromJson>("false").Should().Be(new SvoWithFromJson(false));

    [Test]
    public void Deseralize_SvoWithFromJsonClass_Successful()
        => Deserialize<SvoWithFromJsonClass>("2.5").Value.Should().Be(2.5);

    [Test]
    public void Deserialize_Object_Successful()
    {
        var json = @"{ ""Identifier"": 3, ""Svo"": 2017, ""Message"": ""Hello World!"" }";
        var dto = Deserialize<DtoClass>(json);

        dto.Should().BeEquivalentTo(new
        {
            Identifier = 3,
            Svo = new SvoWithFromJson(2017L),
            Message = "Hello World!",
        });
    }

    [Test]
    public void Deserialize_DtoWithNullSvo_Successful()
    {
        var json = @"{ ""Identifier"": 3, ""Svo"": null, ""Message"": ""Hello World!"" }";
        var dto = Deserialize<DtoClass>(json);

        dto.Should().BeEquivalentTo(new
        {
            Identifier = 3,
            Svo = default(SvoWithFromJson),
            Message = "Hello World!",
        });
    }

    [Test]
    public void Deserialize_NotSupported_Throws()
        => @"""test""".Invoking(Deserialize<SvoThatThrows>)
            .Should()
            .Throw<TException>()
            .WithMessage("Value in the wrong format.");

    [Test]
    public void Serialize_Null_Successful()
        => Serialize(new SvoWithFromJson()).Should().Be("null");

    [Test]
    public void Serialize_String_Successful()
        => Serialize(new SvoWithFromJson("test")).Should().Be(@"""test""");
    
    [Test]
    public void Serialize_Double_Successful()
        => Serialize(new SvoWithFromJson(2.5)).Should().Be("2.5");

    [Test]
    public void Serialize_Long_Successful()
        => Serialize(new SvoWithFromJson(666L)).Should().Be("666");

    [Test]
    public void Serialize_Boolean_Successful()
        => Serialize(new SvoWithFromJson(true)).Should().Be("true");

    [Test]
    public void Serialize_Decimal_Successful()
        => Serialize(new SvoWithFromJson(2.5m)).Should().Be("2.5");

    [Test]
    public virtual void Serialize_DateTime_Successful()
        => Serialize(new SvoWithFromJson(11.June(2017).AsUtc())).Should().Be(@"""2017-06-11T00:00:00Z""");

    protected abstract T Deserialize<T>(string? jsonString);

    protected abstract string Serialize(object obj);
}
