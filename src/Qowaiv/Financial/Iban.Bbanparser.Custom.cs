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
    protected override string? Validate(string iban)
        => Currency.TryParse(iban[^3..]) is { IsKnown: true }
            ? iban
            : null;
}

internal sealed class BbanAlbaniaParser(string pattern) : BbanParser(pattern)
{
    [Pure]
    protected override string? Validate(string iban)
    {
        var weighted = IbanValidator.Weighted(iban[4..], 9, 7, 3, 1, 9, 7, 3) % 10;
        var checksum = ASCII.Digit(iban[11]);
        return (10 - weighted) == checksum
            ? iban
            : null;
    }
}

/// <summary>
/// Extends <see cref="BbanParser"/>'s validation applying the Luhn algorithm.
/// </summary>
/// <remarks>
/// Used by <see cref="Globalization.Country.FI"/>.
///
/// See https://en.wikipedia.org/wiki/Luhn_algorithm.
/// </remarks>
internal sealed class BbanFinlandParser(string pattern) : BbanParser(pattern)
{
    [Pure]
    protected override string? Validate(string iban)
    {
        var checksum = 0;

        for (var i = 4; i < iban.Length - 1; i++)
        {
            var digit = ASCII.Digit(iban[i]);
            checksum += Odd(i) ? digit : Sum(digit * 2);
        }

        return 10 - (checksum % 10) == ASCII.Digit(iban[^1])
            ? iban
            : null;

        static int Sum(int n) => (n / 10) + (n % 10);
        static bool Odd(int i) => (i & 1) == 1;
    }
}
