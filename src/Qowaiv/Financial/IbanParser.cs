#pragma warning disable S1541 // Complexity as result of performance
#pragma warning disable S3776 // Complexity as result of performance

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
        => MachineReadable(reader)
        ?? Normalize(reader);

    /// <summary>Validates an IBAN assuming its normalized, and the BBAN is already resolved.</summary>
    [Pure]

    private static string? MachineReadable(string iban)
    {
        if (iban.Length is < MinLength or > InternationalBankAccountNumber.MaxLength) return null;

        var (f, s) = (iban[0], iban[1]);

        // No valide country code.
        if (!Is.Letter(f) || !Is.Letter(s)
            || Bban.All[((f - 'A') * 26) + (s - 'A')] is not { Pattern: not null } bban) return null;

        var pattern = bban.Pattern;

        // Invalid length.
        if (!bban.IsGeneric && iban.Length != pattern.Length) return null;

        // The first to characters are already checked by selecting the BBAN.
        for (var i = 2; i < iban.Length; i++)
        {
            // Non-ASCII chars will be invalidated by the cast to byte.
            var c = (byte)iban[i];

            // If there is a catagory missmatch, stop.
            if ((Catagory[c] & pattern[i]) is 0) return null;
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

        // Pre-calculated Country code.
        mod = (mod * 10000) + bban.Mod;

        // Checksum.
        mod = (mod * 10) + iban[2] - '0';
        mod = (mod * 10) + iban[3] - '0';

        return mod % 97 is 1 ? iban : null;
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
            && (Is.Markup(reader[4]) || reader[4] == ':'))
            reader = reader[5..].TrimStart();

        // The minimum length of an IBAN.
        if (reader.Length < 12) return null;

        Span<char> writer = stackalloc char[InternationalBankAccountNumber.MaxLength];

        var (r, w) = (0, 0);
        while (w < writer.Length && r < reader.Length)
        {
            var ch = reader[r++];
            if (Is.DigitOrLetter(ch)) writer[w++] = ch;
            else if (Is.Lower(ch)) writer[w++] = (char)(ch & 0x5F);
            else if (Is.Markup(ch) && w is not 1 and not 3) { /* Markup is allowed except for within the country or the checksum */ }
            else return null;
        }

        return r == reader.Length && r >= MinLength
            ? MachineReadable(writer[..w].ToString())
            : null;
    }

    private static class Is
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Letter(char ch) => (Catagory[(byte)ch] & (1 << 10)) is not 0;

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool DigitOrLetter(char ch) => (Catagory[(byte)ch] & (3 << 10)) is not 0;

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Lower(char ch) => (Catagory[(byte)ch] & (1 << 12)) is not 0;

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Markup(char ch) => ASCII.IsAscii(ch)
            ? ASCII.IsMarkup(ch)
            : char.IsWhiteSpace(ch);
    }

    /// <summary>Catagories for different ASCII chars.</summary>
    private static readonly ushort[] Catagory =
    [
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x0801, 0x0802, 0x0804, 0x0808, 0x0810, 0x0820, 0x0840, 0x0880, 0x0900, 0x0a00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x0400, 0x0400, 0x0400, 0x0400, 0x0400, 0x0400, 0x0400, 0x0400, 0x0400, 0x0400, 0x0400, 0x0400, 0x0400, 0x0400, 0x0400,
        0x0400, 0x0400, 0x0400, 0x0400, 0x0400, 0x0400, 0x0400, 0x0400, 0x0400, 0x0400, 0x0400, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x01000, 0x01000, 0x01000, 0x01000, 0x01000, 0x01000, 0x01000, 0x01000, 0x01000, 0x01000, 0x01000, 0x01000, 0x01000, 0x01000, 0x01000,
        0x01000, 0x01000, 0x01000, 0x01000, 0x01000, 0x01000, 0x01000, 0x01000, 0x01000, 0x01000, 0x01000, 0x00, 0x00, 0x00, 0x00, 0x00,

        // Non-ASCII
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    ];
}
