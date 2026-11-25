namespace Qowaiv.Locale.NL.Conversion;

/// <summary>Provides a conversion for a BAG-identificatienummer.</summary>
[Inheritable]
public class BagIdTypeConverter : NumericTypeConverter<BagId, long>
{
    /// <inheritdoc/>
    [Pure]
    protected override BagId FromRaw(long raw) => BagId.Create(raw);

    /// <inheritdoc/>
    [Pure]
    protected override BagId FromString(string? str, CultureInfo? culture) => BagId.Parse(str, culture);

    /// <inheritdoc/>
    [Pure]
    protected override long ToRaw(BagId svo) => (long)svo;
}
