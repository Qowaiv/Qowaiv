namespace Qowaiv.Conversion;

/// <summary>Provides a conversion for a month span.</summary>
[Inheritable]
public class YearSpanTypeConverter : NumericTypeConverter<YearSpan, int>
{
    /// <inheritdoc/>
    [Pure]
    protected override YearSpan FromRaw(int raw) => YearSpan.Create(raw);

    /// <inheritdoc/>
    [Pure]
    protected override YearSpan FromString(string? str, CultureInfo? culture) => YearSpan.Parse(str, culture);

    /// <inheritdoc/>
    [Pure]
    protected override int ToRaw(YearSpan svo) => (int)svo;
}
