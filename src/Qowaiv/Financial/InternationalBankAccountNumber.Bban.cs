#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Globalization;
using Qowaiv.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Qowaiv.Financial
{
    public partial struct InternationalBankAccountNumber
    {
        /// <summary>Constructs a <see cref="Regex"/> based on its BBAN pattern.</summary>
        private static KeyValuePair<Country, Regex> Bban(Country country, string bban, int? checksum = default)
        {
            var blocks = bban.Split(',').Select(b => b.Buffer());
            var pattern = CharBuffer.Empty(64)
                .Add('^')
                .Add(country.IsoAlpha2Code)
                .Add(checksum.HasValue ? checksum.Value.ToString("00") : "[0-9][0-9]");

            foreach(var block in blocks)
            {
                var type = block.Last();
                int.TryParse(block.RemoveFromEnd(1), out int length);

                switch (type)
                {
                    case 'n': pattern.Add("[0-9]").Add('{').Add(length.ToString()).Add('}'); break;
                    case 'a': pattern.Add("[A-Z]").Add('{').Add(length.ToString()).Add('}'); break;
                    case 'c': pattern.Add("[0-9A-Z]").Add('{').Add(length.ToString()).Add('}'); break;
                    case ']': pattern.Add(block.RemoveFromStart(1)); break;
                    default: throw new FormatException();
                }
            }

            return new KeyValuePair<Country, Regex>(country, new Regex(pattern.Add('$'), RegexOptions.Compiled));
        }
    }
}
