using Qowaiv.Financial;

namespace Qowaiv.Conversion.Financial;

/// <summary>Provides a conversion for an Amount.</summary>
[Inheritable]
public class AmountTypeConverter : NumericTypeConverter<Amount, decimal>
{
    /// <inheritdoc/>
    [Pure]
    protected override Amount FromRaw(decimal raw) => (Amount)raw;

    /// <inheritdoc/>
    [Pure]
    protected override Amount FromString(string? str, CultureInfo? culture) => Amount.Parse(str, culture);

    /// <inheritdoc/>
    [Pure]
    protected override decimal ToRaw(Amount svo) => (decimal)svo;
}
