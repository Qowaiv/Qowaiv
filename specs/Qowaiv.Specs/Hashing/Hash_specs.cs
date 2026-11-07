namespace Hashing.Hash_specs;

public class Get_hash_code
{
    [Test]
    public void Null_String_is_supported()
    {
        int hash = Hash.Code(Nil.String);
        hash.Should().Be(0);
    }

    [Test]
    public void Supports_fluent_syntax() => true
        .Invoking(_ => Hash.Code(12)
            .And(Svo.EmailAddress)
            .And(Svo.DateTime))
        .Should().NotThrow();

    [Test]
    public void Supports_fluent_syntax_empty_collection() => true
        .Invoking(_ => Hash.Code(12).And(Array.Empty<byte>()))
        .Should().NotThrow();

    [Test]
    public void Supports_fluent_syntax_null_collection()
    {
        var hash = Hash.Code(12);
        var adjusted = hash.And<object>(null);
        adjusted.Should().Be(hash);
    }

    [Test]
    public void Supports_fluent_syntax_for_collection() => true
        .Invoking(_ => Hash.Code(12).And(new[] { 1234, 123, 2345, 213 }))
        .Should().NotThrow();

    [Test]
    public void Different_order_different_hash()
    {
        int hash = Hash.Code(12).And(new[] { 1234, 123, 2345, 213 });
        int othr = Hash.Code(12).And(new[] { 123, 1234, 2345, 213 });
        hash.Should().NotBe(othr);
    }


    [Test]
    public void Null_items_are_hashed() => true
        .Invoking(_ => Hash.Code(42).And(new List<int?>() { 17, null, 88 }))
        .Should().NotThrow();

    [Test]
    public void GetHashCode_is_not_supported() => true
        .Invoking(_ => Hash.Code(12).GetHashCode())
        .Should().Throw<HashingNotSupported>();
}

public class Not_supported
{
    [Test]
    public void for_type_throws_exception()
    {
        Func<int> hash = Hash.NotSupportedBy<string>;
        hash.Should().Throw<HashingNotSupported>()
            .WithMessage("Hashing is not supported by System.String.");
    }
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.Hash.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.Hash.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.Hash.Equals(Hash.Code(17)).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.Hash.Equals(Hash.Code("QOWAIV")).Should().BeTrue();
}

public class To_string
{
    [Test]
    public void displays_the_hash()
    {
        var hash = Hash.Code(12);
        hash.ToString().Should().Match(s => Regex.IsMatch(s, "-?[0-9]+"));
    }
}
