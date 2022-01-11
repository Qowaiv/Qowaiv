#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

using Qowaiv.Globalization;

namespace Qowaiv.Financial;

public partial struct InternationalBankAccountNumber
{
    /// <summary>Constructs a <see cref="Regex"/> based on its BBAN pattern.</summary>
    [Pure]
    private static KeyValuePair<Country, Regex> Bban(Country country, string bban, int? checksum = default)
    {
        var pattern = new StringBuilder(64)
            .Append('^')
            .Append(country.IsoAlpha2Code)
            .Append(checksum.HasValue ? checksum.Value.ToString("00") : "[0-9][0-9]");

        foreach (var block in bban.Split(','))
        {
            var type = block.Last();

            if (!int.TryParse(block.Substring(0, block.Length - 1), out int length) && type == ']')
            {
                pattern.Append(block.Substring(1, block.Length - 2));
            }
            else
            {
                pattern.Append(CharType(type)).Append('{').Append(length).Append('}');
            }
        }
        return new KeyValuePair<Country, Regex>(country, new Regex(pattern.Append('$').ToString(), RegexOptions.Compiled));

        static string CharType(char type) => type switch { 'n' => "[0-9]", 'a' => "[A-Z]", 'c' => "[0-9A-Z]", _ => throw new FormatException() };
    }
}
