namespace Qowaiv.Financial;

internal static partial class IbanParser
{
    private const int IB = (('I' - 'A') * 26) + 'B' - 'A';

    /// <summary>Parses a string representing an <see cref="InternationalBankAccountNumber"/>.</summary>
    /// <returns>
    /// A normalized (uppercased without markup) string, or null for invalid input.
    /// </returns>
    [Pure]
    public static string? Parse(string str)
    {
        var index = 0;
        var pos = 0;
        var id = 0;
        var prefixed = false;

        while (index < str.Length)
        {
            var ch = str[index++];

            if (ASCII.IsAscii(ch) && ASCII.IsLetter(ch))
            {
                id *= 26;
                id += ASCII.Upper(ch) - 'A';

                if (++pos == 1) continue;

                if (Parsers[id] is { } bban)
                {
                    return bban.Parse(str, index, id);
                }
                else if (!prefixed && HasIbanPrefix(id, str, index))
                {
                    pos = 0;
                    id = 0;
                    index += 3;
                    prefixed = true;
                }
                else return null;
            }
            else if (pos != 0)
            {
                return null;
            }
            else if (!prefixed && HasIbanPrefix(str, index))
            {
                index += 5;
                prefixed = true;
            }
            else if (!IsMarkup(ch))
            {
                return null;
            }
        }
        return null;
    }

    [Pure]
    internal static bool IsMarkup(char ch)
        => ASCII.IsAscii(ch)
            ? ASCII.IsMarkup(ch)
            : char.IsWhiteSpace(ch);

    [Pure]
    private static bool HasIbanPrefix(int id, string str, int index)
        => id == IB
        && str[(index - 2)..].StartsWith("IBAN", StringComparison.OrdinalIgnoreCase)
        && (IsMarkup(str[index + 2]) || str[index + 2] == ':');

    [Pure]
    private static bool HasIbanPrefix(string str, int index)
        => str[(index - 1)..].StartsWith("(IBAN)", StringComparison.OrdinalIgnoreCase);
}
