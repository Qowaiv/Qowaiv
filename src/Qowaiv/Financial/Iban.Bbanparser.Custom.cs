namespace Qowaiv.Financial;

/// <summary>
/// Extends <see cref="BbanParser"/>'s validation by checking if the last 3
/// characters describe a <see cref="Currency"/> code.
/// </summary>
/// <remarks>
/// Used by <see cref="Globalization.Country.MU"/> and <see cref="Globalization.Country.SC"/>.
/// </remarks>
internal sealed class BbanWithCurrencyCodeParser(string pattern) : BbanParser(pattern)
{
    [Pure]
    protected override bool Validate(string iban)
        => Currency.TryParse(iban[^3..]) is { IsKnown: true };
}
