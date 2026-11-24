namespace Qowaiv.Locale.NL.Conversion;

/// <summary>Provides a conversion for an RSIN.</summary>
[Inheritable]
public class RsinypeConverter : NumericTypeConverter<Rsin, long>
{
    /// <inheritdoc/>
    [Pure]
    protected override Rsin FromRaw(long raw) => Rsin.Create(raw);

    /// <inheritdoc/>
    [Pure]
    protected override Rsin FromString(string? str, CultureInfo? culture) => Rsin.Parse(str, culture);

    /// <inheritdoc/>
    [Pure]
    protected override long ToRaw(Rsin svo) => (long)svo;
}
