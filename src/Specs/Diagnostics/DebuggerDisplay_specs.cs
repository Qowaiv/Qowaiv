namespace Diagnostics.DebuggerDisplay_specs;

public class Displays
{
    [Test]
    public void default_of_continious_as_value()
        => Percentage.Zero.Should().HaveDebuggerDisplay("0.00%");

    [Test]
    public void default_of_non_continious_as_empty()
        => EmailAddress.Empty.Should().HaveDebuggerDisplay("{empty}");
}
