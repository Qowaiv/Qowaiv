﻿namespace Qowaiv.Financial;

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
    /// <inheritdoc />
    /// <remarks>
    /// Applies only to the bank code + branch code fields.
    /// </remarks>
    [Pure]
    protected override string? Validate(string iban)
    {
        var weighted = IbanValidator.Weighted(iban, start: 4, mod: 10, 9, 7, 3, 1, 9, 7, 3);
        var checksum = ASCII.Digit(iban[11]);
        return (10 - weighted) == checksum
            ? iban
            : null;
    }
}

internal sealed class BbanBelgiumParser(string pattern) : BbanParser(pattern)
{
    /// <inheritdoc />
    [Pure]
    protected override string? Validate(string iban)
    {
        var mod97 = IbanValidator.Mod97(iban, 4, iban.Length - 2);
        var check = IbanValidator.Checksum(iban, iban.Length - 2);
        return mod97 == check ? iban : null;
    }
}

internal sealed class BbanCzechParser(string pattern) : BbanParser(pattern)
{
    /// <inheritdoc />
    /// <remarks>
    /// Weighted is calculated separately for the account number (ten digits)
    /// and branch number (six digits, using the last six weights).
    /// Both should be 0.
    /// </remarks>
    [Pure]
    protected override string? Validate(string iban)
        => IbanValidator.Weighted(iban, start: 14, mod: 11, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1) == 0
        && IbanValidator.Weighted(iban, start: 08, mod: 11, /*.......*/ 10, 5, 8, 4, 2, 1) == 0
            ? iban
            : null;
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