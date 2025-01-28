using System.Runtime.CompilerServices;

namespace Qowaiv.Financial;

internal static partial class IbanParser
{
    private const int IB = (('I' - 'A') * 26) + 'B' - 'A';

#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high

    /// <summary>Parses a string representing an <see cref="InternationalBankAccountNumber" />.</summary>
    /// <returns>
    /// A normalized (uppercased without markup) string, or null for invalid input.
    /// </returns>
    /// <remarks>
    /// This method is optimized for speed, hence some nesting, and inlining.
    /// </remarks>
    [Pure]
    public static string? Parse(CharSpan span, bool prefixed = false)
    {
        // The minimum length of an IBAN.
        while (span.Length >= 12)
        {
            if (IsLetter(span.First))
            {
                var id = Id(span++);

                if (!IsLetter(span.First))
                {
                    return null;
                }

                id = (id * 26) + Id(span++);

                if (Parsers[id] is { } bban)
                {
                    return bban.Parse(span, id);
                }
                else if (!prefixed && HasIbanPrefix(id, span))
                {
                    return Parse(span.Next(3), prefixed: true);
                }
                else
                {
                    return null;
                }
            }
            else if (IsMarkup(span.First))
            {
                span++;
            }
            else if (!prefixed && HasIbanPrefix(span))
            {
                return Parse(span.Next(6), prefixed: true);
            }
            else
            {
                return null;
            }
        }
        return null;
    }
#pragma warning restore S3776

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int Id(CharSpan span) => ASCII.Upper(span.First) - 'A';

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsLetter(char ch) => ASCII.IsAscii(ch) && ASCII.IsLetter(ch);

    [Pure]
    internal static bool IsMarkup(char ch)
        => ASCII.IsAscii(ch)
            ? ASCII.IsMarkup(ch)
            : char.IsWhiteSpace(ch);

    [Pure]
    private static bool HasIbanPrefix(int id, CharSpan span)
        => id == IB
        && span.Prev(2).StartsWith("IBAN", ignoreCase: true)
        && (IsMarkup(span[2]) || span[2] == ':');

    [Pure]
    private static bool HasIbanPrefix(CharSpan span)
        => span.StartsWith("(IBAN)", ignoreCase: true);
}
