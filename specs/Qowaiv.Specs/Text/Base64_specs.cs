namespace Text.Base64_specs;

public class ToString
{
    [Test]
    public void Null_StringEmpty()
        => Base64.ToString(null).Should().Be(string.Empty);

    [Test]
    public void EmptyArray_StringEmpty()
        => Base64.ToString([]).Should().Be(string.Empty);

    [TestCase("Aao=", 1, 170)]
    [TestCase("Cxct", 11, 23, 45)]
    [TestCase("Qowaig==", 66, 140, 26, 138)]
    public void bytes(string base64, params int[] data)
    {
        var bytes = data.Select(v => (byte)v).ToArray();
        Base64.ToString(bytes).Should().Be(base64);
    }
}

public class TryGetBytes_from
{
    [Test]
    public void Null__returns_EmptyArray()
    {
        Base64.TryGetBytes(null, out var bytes).Should().BeTrue();
        bytes.Should().BeEmpty();
    }

    [Test]
    public void StringEmpty_returns_EmptyArray()
    {
        Base64.TryGetBytes(string.Empty, out var bytes).Should().BeTrue();
        bytes.Should().BeEmpty();
    }

    [TestCase("Aap=", 1, 170)]
    [TestCase("Cxct", 11, 23, 45)]
    [TestCase("Qowaiv==", 66, 140, 26, 138)]
    public void String_returns(string str, params int[] expected)
    {
        var bytes = expected.Select(v => (byte)v).ToArray();
        Base64.TryGetBytes(str, out byte[] actualBytes).Should().BeTrue();
        actualBytes.Should().BeEquivalentTo(bytes);
    }

    [Test]
    public void Not_support_chars_returns_false_with_EmptyArray()
    {
        Base64.TryGetBytes("ABC}", out byte[] bytes).Should().BeFalse();
        bytes.Should().BeEmpty();
    }
}
