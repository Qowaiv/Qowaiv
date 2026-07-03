namespace Qowaiv.Financial;

internal static class BBAN
{
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasPrefix(ReadOnlySpan<char> reader)
        => reader.StartsWith("IBAN", StringComparison.OrdinalIgnoreCase)
        && (IsMarkup(reader[4]) || reader[4] == ':');

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsMatch(char ch, char pattern) => pattern switch
    {
        'n' => IsDigit(ch),
        'a' => IsLetter(ch),
        'c' => IsDigit(ch) || IsLetter(ch),
        _ => pattern == ch,
    };

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsMarkup(char ch) => ASCII.IsAscii(ch)
        ? ASCII.IsMarkup(ch)
        : char.IsWhiteSpace(ch);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDigit(char c) => c is >= '0' and <= '9';

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLetter(char c) => c is >= 'A' and <= 'Z';

    /// <summary>Uppercases the char assuming it an ASCII character.</summary>
    /// <remarks>
    /// If an character is non-ASCII it stays non-ASCII but will be corrupted.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char Upper(char c) => (char)(c & 0b_1111_1111_1101_1111);
}
