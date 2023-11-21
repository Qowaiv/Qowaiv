namespace String_formatter_specs;

public class TryApplyCustomFormatter
{
    [Test]
    public void false_if_no_customformatter_could_be_found()
    {
        StringFormatter.TryApplyCustomFormatter("", 42, CultureInfo.InvariantCulture, out var formatted)
            .Should().BeFalse();

        formatted.Should().Be(string.Empty);
    }

    [Test]
    public void false_if_customformatter_could_be_found_but_returns_null()
    {
        StringFormatter.TryApplyCustomFormatter("", 42, new CustomFormatter(), out var formatted)
            .Should().BeFalse();

        formatted.Should().Be(string.Empty);
    }

    [Test]
    public void true_if_customformatter_could_be_found()
    {
        StringFormatter.TryApplyCustomFormatter("", -42, new CustomFormatter(), out var formatted)
            .Should().BeTrue();

        formatted.Should().Be("-42");
    }

    private sealed class CustomFormatter : IFormatProvider, ICustomFormatter
    {
        public string? Format(string? format, object? arg, IFormatProvider? formatProvider)
            => arg is int n && n < 0 ? n.ToString() : null;

        public object? GetFormat(Type? formatType)
            => formatType == typeof(ICustomFormatter) ? this : null;
    }
}
