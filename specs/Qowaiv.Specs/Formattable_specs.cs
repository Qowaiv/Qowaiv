namespace Formattable_specs;

public class ToString_with_format_and_provider
{
    private static IEnumerable<IFormattable> Svos => Svo.All().OfType<IFormattable>();

    [TestCaseSource(nameof(Svos))]
    public void null_and_string_empty_are_equal(IFormattable svo)
    {
        var string_empty = svo.ToString(string.Empty, TestCultures.nl_NL);
        var @null = svo.ToString(null, TestCultures.nl_NL);
        string_empty.Should().Be(@null);
    }
}
