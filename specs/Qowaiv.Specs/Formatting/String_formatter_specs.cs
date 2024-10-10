namespace String_formatter_specs;

public class Try_apply_custom_formatter
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
        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
            => arg is int n && n < 0 ? n.ToString() : null!;

        public object? GetFormat(Type? formatType)
            => formatType == typeof(ICustomFormatter) ? this : null;
    }
}

public class Apply
{

    [Test]
    public void Apply_InvalidFormat_ThrowsFormatException()
        => int.MinValue.Invoking(v => StringFormatter.Apply(v, "\\", CultureInfo.InvariantCulture, []))
            .Should()
            .Throw<FormatException>()
            .WithMessage("Input string was not in a correct format.");
}

public class To_non_diacritic
{
    [Test]
    public void null_stays_null()
        => StringFormatter.ToNonDiacritic(Nil.String).Should().Be(Nil.String);

    [Test]
    public void string_empty_stays_string_empty()
        => StringFormatter.ToNonDiacritic(string.Empty).Should().Be(string.Empty);

    [Test]
    public void updates_diacritic()
        => StringFormatter.ToNonDiacritic("Café & Straße").Should().Be("Cafe & Strasze");

    [Test]
    public void updates_diacritic_ignoring_specified()
        => StringFormatter.ToNonDiacritic("Café & Straße", "é").Should().Be("Café & Strasze");
}