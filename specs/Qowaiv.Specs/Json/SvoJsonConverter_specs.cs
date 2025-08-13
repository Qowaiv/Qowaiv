#if NET6_0_OR_GREATER

using Specs_Generated;

namespace Json.SvoJsonConverter_specs;

public class Read_from
{
    [TestCase(true)]
    [TestCase(false)]
    public void Boolean(bool value)
        => JsonTester.Read_System_Text_JSON<StringBasedId>(value)
        .ToString().Should().Be(value.ToString());
}
#endif
