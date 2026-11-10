namespace Specs.Customization.UuidBasedId_specs;

internal class Implements
{
    [Test]
    public void INext()
        => typeof(GlobalId).GetMethod("Next", BindingFlags.Public | BindingFlags.Static)
        .Should().NotBeNull();
}
