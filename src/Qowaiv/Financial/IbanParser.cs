namespace Qowaiv.Financial;

internal static class IbanParser
{
    private const int IB = (('I' - 'A') * 26) + 'B' - 'A';

    /// <summary>Parses a string representing an <see cref="InternationalBankAccountNumber" />.</summary>
    /// <returns>
    /// A normalized (uppercased without markup) string, or null for invalid input.
    /// </returns>
    /// <remarks>
    /// This method is optimized for speed, hence some nesting, and inlining.
    /// </remarks>
    [Pure]
    public static string? Parse(ReadOnlySpan<char> reader)
        => reader.Length >= 12
        && (MachineReadable(reader) ?? HumanReadable(reader)) is { } iban
        && Mod97(iban)
        ? iban
        : null;

    /// <summary>No spaces and uppercased.</summary>
    [Pure]
    private static string? MachineReadable(ReadOnlySpan<char> reader)
    {
        var (i, length) = (2, 2);
        Span<char> buffer = stackalloc char[InternationalBankAccountNumber.MaxLength];

        var (f, s) = (reader[0], reader[1]);

        if (!IsLetter(f) || !IsLetter(s)) return null;
        buffer[0] = f;
        buffer[1] = s;
        var id = Id(f, s);

        // Not a known country.
        if (Bban.All[id] is not { Pattern: not null } bban) return null;

        var pattern = bban.Pattern;

        while (i < reader.Length && length < pattern.Length)
        {
            var (ch, type) = (reader[i++], pattern[length]);

            if (IsMatch(ch, type)) buffer[length++] = ch;
            else return null;
        }

        // Not everything consumed, or wrong length.
        if (reader.Length != i
            || !(bban.IsGeneric ? length >= 12 : length == pattern.Length)) return null;

        var iban = buffer[..length].ToString();

        return !bban.Currency || Currency.TryParse(iban[^3..]) is { IsKnown: true }
            ? iban
            : null;
    }

    /// <summary>Supports markup, lowercase and prefixes.</summary>
    [Pure]
    private static string? HumanReadable(ReadOnlySpan<char> reader, bool prefixed = false)
    {
        reader = reader.Trim();

        // The minimum length of an IBAN.
        if (reader.Length < 12) return null;

        var (i, length) = (2, 2);
        Span<char> buffer = stackalloc char[InternationalBankAccountNumber.MaxLength];

        var f = Id(reader[0]);

        if (IsLetter(f)) buffer[0] = f;
        else return !prefixed && Has_IBAN_Prefix(reader)
            ? HumanReadable(reader[6..], true)
            : null;

        var s = Id(reader[1]);

        if (IsLetter(s)) buffer[1] = s;
        else return null;

        var id = Id(f, s);

        if (id == IB) return prefixed || !HasIBANPrefix(reader)
            ? null
            : HumanReadable(reader[5..], true);

        // Not a known country.
        if (Bban.All[id] is not { Pattern: not null } bban) return null;

        var pattern = bban.Pattern;

        while (i < reader.Length && length < pattern.Length)
        {
            var (ch, type) = (reader[i++], pattern[length]);
            var up = char.ToUpperInvariant(ch);

            if (IsMatch(up, type)) buffer[length++] = up;

            // Markup within the ckecksum is not allowed.
            else if (length is 3 || !IsMarkup(ch)) return null;
        }

        // Not everything consumed, or wrong length.
        if (reader.Length != i
            || !(bban.IsGeneric ? length >= 12 : length == pattern.Length)) return null;

        var iban = buffer[..length].ToString();

        return !bban.Currency || Currency.TryParse(iban[^3..]) is { IsKnown: true }
            ? iban
            : null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsMarkup(char ch)
            => ASCII.IsAscii(ch)
            ? ASCII.IsMarkup(ch)
            : char.IsWhiteSpace(ch);

        // Starts with "(IBAN)".
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool Has_IBAN_Prefix(ReadOnlySpan<char> reader)
            => reader.StartsWith("(IBAN)", StringComparison.OrdinalIgnoreCase);

        // Starts with "IBAN " or "IBAN:".
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool HasIBANPrefix(ReadOnlySpan<char> reader)
            => reader.StartsWith("IBAN", StringComparison.OrdinalIgnoreCase)
            && (IsMarkup(reader[4]) || reader[4] == ':');
    }

    /// <summary>Checks the Mod97 constraint.</summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool Mod97(string iban)
    {
        ulong num = 0;

        // Calculate the first 4 characters (country and checksum) last
        for (var i = 4; i < iban.Length; i++)
        {
            num = Next(num, iban[i]);

            // If we wait longer, we could overflow.
            if (num >> 57 is not 0) num %= 97;
        }

        // If we wait longer, we could overflow.
        if (num >> 44 is not 0) num %= 97;

        for (var i = 0; i < 4; i++)
        {
            num = Next(num, iban[i]);
        }

        return num % 97 is 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ulong Next(ulong num, char ch)
            => ch <= '9'
            ? (num * 10) + ch - '0'
            : (num * 100) + ch - 'A' + 10;
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int Id(char f, char s) => ((f - 'A') * 26) + (s - 'A');

    /// <summary>Uppercases the char assuming it an ASCII character.</summary>
    /// <remarks>
    /// If an character is non-ASCII it stays non-ASCII but will be corrupted, which is fine.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static char Id(char c) => (char)(c & 0b_1111_1111_1101_1111);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsMatch(char c, char type) => type switch
    {
        'n' => IsDigit(c),
        'a' => IsLetter(c),
        'c' => IsDigit(c) || IsLetter(c),
        _ => type == c,
    };

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsDigit(char c) => IsBetween(c, '0', '9' - '0');

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsLetter(char c) => IsBetween(c, 'A', 'Z' - 'A');

    /// <summary>Indicates whether a character is within the specified inclusive range.</summary>
    /// <remarks>
    /// This is a tweaked copy of .NET's char.IsBetween(). Is is not avialable for .NET standard 2.0.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsBetween(char c, char min, uint delta) =>
        (uint)(c - min) <= delta;
}
