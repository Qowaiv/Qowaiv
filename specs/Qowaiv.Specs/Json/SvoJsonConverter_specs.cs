#if NET6_0_OR_GREATER

using Specs_Generated;

namespace Json.SvoJsonConverter_specs;

public class Read_from
{
    [TestCase(true, "True")]
    [TestCase(false, "False")]
    public void Boolean(bool value, string text)
        => JsonTester.Read_System_Text_JSON<StringBasedId>(value)
        .ToString().Should().Be(text);
}
#endif
