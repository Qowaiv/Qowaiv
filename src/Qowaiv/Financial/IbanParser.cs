namespace Qowaiv.Financial;

internal static partial class IbanParser
{
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

        foreach (var ch in str)
        {
            index++;

            if (ASCII.IsAscii(ch) && ASCII.IsLetter(ch))
            {
                id *= 26;
                id += ASCII.Upper(ch) - 'A';

                if (++pos == 2)
                {
                    return Parsers[id] is { } bban
                        ? bban.Parse(str, index, id)
                        : null;
                }
            }
            else if (pos != 0 || !IsMarkup(ch))
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
}
