namespace Qowaiv.Conversion;

/// <summary>Provides a conversion for a year.</summary>
[Inheritable]
public class YearTypeConverter : NumericTypeConverter<Year, int>
{
    /// <inheritdoc/>
    [Pure]
    protected override Year FromRaw(int raw) => Year.Create(raw == 0 ? null : (int?)raw);

    /// <inheritdoc/>
    [Pure]
    protected override Year FromString(string? str, CultureInfo? culture) => Year.Parse(str, culture);

    /// <inheritdoc/>
    [Pure]
    protected override int ToRaw(Year svo) => (int)svo;
}
