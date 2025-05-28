namespace TestTools.Should.Have_debugger_display_specs;

public class Discovers
{
    [Test]
    public void DebuggerDisplay_property_on_class()
        => new SimpleClass().Should().HaveDebuggerDisplay("SimpleClass display");

    [Test]
    public void DebuggerDisplay_property_on_base()
        => new ChildClass().Should().HaveDebuggerDisplay("ChildClass display");
}
public class Fails_on
{
    [Test]
    public void not_defined_debugger_display()
        => new object()
        .Invoking(x => x.Should().HaveDebuggerDisplay("wrong"))
        .Should().Throw<AssertionException>()
        .WithMessage("'System.Object' has no DebuggerDisplay defined.");

    [Test]
    public void unexpected_debugger_display()
        => new SimpleClass()
        .Invoking(x => x.Should().HaveDebuggerDisplay("wrong"))
        .Should().Throw<AssertionException>()
        .WithMessage(@"Expected * to be ""wrong"", but found ""SimpleClass display"".");
}

public class can_be_changed
{
    [Test]
    public void with_And_constraint()
        => new SimpleClass().Should().HaveDebuggerDisplay("SimpleClass display")
        .And.NotBeNull();
}


[DebuggerDisplay("{DebuggerDisplay}")]
internal class SimpleClass
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => $"{GetType().Name} display";
}

[EmptyTestClass]
internal sealed class ChildClass : SimpleClass { }
