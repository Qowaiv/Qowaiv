#if NET8_0_OR_GREATER

namespace Identifiers.Id_specs;

public class Can_parse
{
    [Test]
    public void providing_a_format_provider()
    {
        CustomGuid.Parse("8A1A8C42-D2FF-E254-E26E-B6ABCBF19420", TestCultures.pt).Should().Be(Svo.CustomGuid);
        CustomGuid.TryParse("8A1A8C42-D2FF-E254-E26E-B6ABCBF19420", TestCultures.pt, out _).Should().BeTrue();
    }
}

#endif
