namespace Qowaiv.Conversion;

/// <summary>Provides a conversion for a month span.</summary>
public class MonthSpanTypeConverter : NumericTypeConverter<MonthSpan, int>
{
    /// <inheritdoc/>
    [Pure]
    protected override MonthSpan FromRaw(int raw) => MonthSpan.FromMonths(raw);

    /// <inheritdoc/>
    [Pure]
    protected override MonthSpan FromString(string? str, CultureInfo? culture) => MonthSpan.Parse(str, culture);

    /// <inheritdoc/>
    [Pure]
    protected override int ToRaw(MonthSpan svo) => (int)svo;
}
