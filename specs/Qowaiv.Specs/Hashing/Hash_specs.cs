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
    public void Supports_fluent_syntax()
    {
        using (Hash.WithoutRandomizer())
        {
            int hash = Hash.Code(12)
                .And(Svo.EmailAddress)
                .And(Svo.DateTime);
            hash.Should().Be(-1238227536);
        }
    }

    [Test]
    public void Supports_fluent_syntax_empty_collection()
    {
        using (Hash.WithoutRandomizer())
        {
            int hash = Hash.Code(12)
                .And(Array.Empty<byte>());
            hash.Should().Be(490959278);
        }
    }

    [Test]
    public void Supports_fluent_syntax_null_collection()
    {
        var hash = Hash.Code(12);
        var adjusted = hash.And<object>(null);
        adjusted.Should().Be(hash);
    }

    [Test]
    public void Supports_fluent_syntax_for_collection()
    {
        using (Hash.WithoutRandomizer())
        {
            int hash = Hash.Code(12)
                .And(new[] { 1234, 123, 2345, 213 });
            hash.Should().Be(535185610);
        }
    }

    [Test]
    public void Different_order_different_hash()
    {
        using (Hash.WithoutRandomizer())
        {
            int hash = Hash.Code(12).And(new[] { 1234, 123, 2345, 213 });
            int othr = Hash.Code(12).And(new[] { 123, 1234, 2345, 213 });
            hash.Should().NotBe(othr);
        }
    }

    [Test]
    public void GetHashCode_is_not_supported()
    {
        Func<int> call = () => Hash.Code(12).GetHashCode();
        call.Should().Throw<HashingNotSupported>();
    }
}

public class Get_hash_code_with_fixed_randomizer
{
    [Test]
    public void String_is_fixed()
    {
        using (Hash.WithoutRandomizer())
        {
            int hash = Hash.Code("QOWAIV string");
            hash.Should().Be(1856816985);
        }
    }

    [Test]
    public void Int32_is_fixed()
    {
        using (Hash.WithoutRandomizer())
        {
            int hash = Hash.Code(17);
            hash.Should().Be(665630146);
        }
    }

    [Test]
    public void Int64_is_fixed()
    {
        using (Hash.WithoutRandomizer())
        {
            int hash = Hash.Code(12345678909876L);
            hash.Should().Be(1415769949);
        }
    }
}

public class Get_hash_code_without_fixed_randomizer
{
    [Test]
    public void String_is_different_from_fixed()
    {
        int fix;
        using (Hash.WithoutRandomizer())
        {
            fix = Hash.Code("QOWAIV string");
        }
        fix.Should().NotBe(Hash.Code("QOWAIV string"));
    }

    [Test]
    public void Int32_is_different_from_fixed()
    {
        int fix;
        using (Hash.WithoutRandomizer())
        {
            fix = Hash.Code(17);
        }
        fix.Should().NotBe(Hash.Code(17));
    }

    [Test]
    public void Int64_is_different_from_fixed()
    {
        int fix;
        using (Hash.WithoutRandomizer())
        {
            fix = Hash.Code(12345678909876L);
        }
        fix.Should().NotBe(Hash.Code(1712345678909876L));
    }

    [Test]
    public void String_gives_same_result()
    {
        var first = Hash.Code("QOWAIV string");
        first.Should().Be(Hash.Code("QOWAIV string"));
    }

    [Test]
    public void Int32_gives_same_result()
    {
        var first = Hash.Code(17);
        first.Should().Be(Hash.Code(17));
    }

    [Test]
    public void Int64_gives_same_result()
    {
        var first = Hash.Code(1712345678909876L);
        first.Should().Be(Hash.Code(1712345678909876L));
    }
}

public class Not_supported
{
    [Test]
    public void for_type_throws_exception()
    {
        Func<int> hash = () => Hash.NotSupportedBy<string>();
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
        using (Hash.WithoutRandomizer())
        {
            var hash = Hash.Code(12);
            hash.ToString().Should().Be("665630175");
        }
    }
}
