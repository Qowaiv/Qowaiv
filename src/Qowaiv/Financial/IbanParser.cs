namespace Qowaiv.Financial;

internal static class IbanParser
{
    private const int MinLength = 12;

    /// <summary>Parses a string representing an <see cref="InternationalBankAccountNumber" />.</summary>
    /// <returns>
    /// A normalized (uppercased without markup) string, or null for invalid input.
    /// </returns>
    /// <remarks>
    /// This method is optimized for speed, hence some nesting, and inlining.
    /// </remarks>
    [Pure]
    public static string? Parse(string reader)
        => (MachineReadable(reader) ?? Normalize(reader)) is { } iban
        ? iban
        : null;

    [Pure]
    private static string? MachineReadable(string reader)
    {
        if (reader.Length < MinLength) return null;

        var (f, s) = (reader[0], reader[1]);
        return IsLetter(f) && IsLetter(s)
            ? Validate(reader, Bban.All[((f - 'A') * 26) + (s - 'A')])
            : null;
    }

    /// <summary>Strips markup and uppercases letters.</summary>
    [Pure]
    private static string? Normalize(ReadOnlySpan<char> reader)
    {
        reader = reader.Trim();

        // Starts with "(IBAN)".
        if (reader.StartsWith("(IBAN)", StringComparison.OrdinalIgnoreCase))
            reader = reader[6..].TrimStart();

        // Starts with "IBAN " or "IBAN:".
        else if (reader.StartsWith("IBAN", StringComparison.OrdinalIgnoreCase)
            && (IsMarkup(reader[4]) || reader[4] == ':'))
            reader = reader[5..].TrimStart();

        // The minimum length of an IBAN.
        if (reader.Length < 12) return null;

        Span<char> writer = stackalloc char[InternationalBankAccountNumber.MaxLength];

        var (r, w) = (0, 0);
        while (w < writer.Length && r < reader.Length)
        {
            var c = reader[r++];
            if (IsDigit(c) || IsLetter(c)) writer[w++] = c;
            else if (IsLower(c)) writer[w++] = (char)(c & 0x5F);
            else if (IsMarkup(c) && w is not 1 and not 3) { /* Markup is allowed except for within the country or the checksum */ }
            else return null;
        }

        return r == reader.Length && r >= MinLength
            ? MachineReadable(writer[..w].ToString())
            : null;
    }

    /// <summary>Validates an IBAN assuming its normalized, and the BBAN is already resolved.</summary>
    [Pure]
    private static string? Validate(string iban, Bban bban)
    {
        if (bban.Pattern is not { } pattern || (bban.IsGeneric
            ? iban.Length > InternationalBankAccountNumber.MaxLength
            : iban.Length != pattern.Length)) return null;

        for (var i = 0; i < iban.Length; i++)
        {
            uint c = iban[i];
            var match = pattern[i] switch
            {
                'n' => c - '0' <= ('9' - '0'),
                'a' => c - 'A' <= ('Z' - 'A'),
                'c' => c - '0' <= ('9' - '0') || c - 'A' <= ('Z' - 'A'),
                var t => t == c,
            };

            if (!match) return null;
        }

        // Currency mismatch.
        if (bban.Currency && Currency.TryParse(iban[^3..]) is not { IsKnown: true })
            return null;

        ulong mod = 0;

        // First check the BBAN part.
        for (var i = 4; i < iban.Length; i++)
        {
            var ch = iban[i];
            mod = ch > '9'
                ? (mod * 100) + ch - 'A' + 10
                : (mod * 010) + ch - '0';

            // If we wait longer, we could overflow.
            if (mod >> 57 is not 0) mod %= 97;
        }

        // If we wait longer, we could overflow.
        if (mod >> 44 is not 0) mod %= 97;

        // We end with the country and checksum.
        mod = (mod * 100) + iban[0] - 'A' + 10;
        mod = (mod * 100) + iban[1] - 'A' + 10;
        mod = (mod * 010) + iban[2] - '0';
        mod = (mod * 010) + iban[3] - '0';

        return mod % 97 is 1 ? iban : null;
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsMarkup(char ch)
        => ASCII.IsAscii(ch)
        ? ASCII.IsMarkup(ch)
        : char.IsWhiteSpace(ch);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsDigit(char c) => IsBetween(c, '0', '9' - '0');

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsLetter(char c) => IsBetween(c, 'A', 'Z' - 'A');

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsLower(char c) => IsBetween(c, 'a', 'z' - 'a');

    /// <summary>Indicates whether a character is within the specified inclusive range.</summary>
    /// <remarks>
    /// This is a tweaked copy of .NET's char.IsBetween(). Is is not avialable for .NET standard 2.0.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsBetween(char c, char min, uint delta) =>
        (uint)(c - min) <= delta;
}
