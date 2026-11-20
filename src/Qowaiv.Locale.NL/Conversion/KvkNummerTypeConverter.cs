namespace Qowaiv.Locale.NL.Conversion;

/// <summary>Provides a conversion for a KvK-nummer.</summary>
[Inheritable]
public class KvkNummerTypeConverter : NumericTypeConverter<KvkNummer, int>
{
    /// <inheritdoc/>
    [Pure]
    protected override KvkNummer FromRaw(int raw) => KvkNummer.Create(raw);

    /// <inheritdoc/>
    [Pure]
    protected override KvkNummer FromString(string? str, CultureInfo? culture) => KvkNummer.Parse(str, culture);

    /// <inheritdoc/>
    [Pure]
    protected override int ToRaw(KvkNummer svo) => (int)svo;
}
