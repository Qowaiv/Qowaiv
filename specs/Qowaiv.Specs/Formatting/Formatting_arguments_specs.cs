namespace Formatting_arguments_specs;

public class ToString
{
    [Test]
    public void null_for_null_object() => Svo.FormattingArguments.ToString(Nil.Object).Should().BeNull();

    [Test]
    public void null_for_null_IFormattable() => Svo.FormattingArguments.ToString(Nil.IFormattable).Should().BeNull();

    [Test]
    public void applies_format_of_arguments_when_applicable() => Svo.FormattingArguments.ToString(7).Should().Be("7,000");

    [Test]
    public void ignores_format_of_arguments_when_not_applicable() => Svo.FormattingArguments.ToString(typeof(int)).Should().Be("System.Int32");

    [Test]
    public void uses_current_culture_when_not_specified()
    {
        using var _ = TestCultures.En_GB.Scoped();

        var arguments = new FormattingArguments("0.000");
        arguments.ToString(7).Should().Be("7.000");
    }
}
