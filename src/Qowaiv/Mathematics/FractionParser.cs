using Qowaiv.Text;
using System.Globalization;

namespace Qowaiv.Mathematics
{
    internal static class FractionParser
    {
        private const NumberStyles DenominatorStyle = NumberStyles.Integer;
        private const NumberStyles NominatorStyle = DenominatorStyle;

        public static Fraction? Parse(string s, NumberFormatInfo info)
        {
            // A integer is fine.
            if (long.TryParse(s, NominatorStyle, info, out var lng))
            {
                return Fraction.Create(lng);
            }
            // Decimals are fine
            if(decimal.TryParse(s, NominatorStyle, info, out var dec))
            {
                return Fraction.Create(dec);
            }

            var plus = info.PositiveSign;
            var min = info.NegativeSign;

            var str = new CharBuffer(s).Trim();

            // check for +
            if (str.StartsWith(plus))
            {
                str.RemoveRange(0, plus.Length);
            }

            var nominator = new CharBuffer(str.Length);

            var pos = 0;

            if (str.StartsWith(min))
            {
                nominator.Add(min);
                pos += min.Length;
            }

            // Detect the nominator.
            for(/**/; pos < str.Length; pos++)
            {
                var ch = str[pos];

                if (char.IsNumber(ch))
                {
                    nominator.Add(ch);
                }
                else if(IsDivisionOperator(ch))
                {
                    pos++;
                    break;
                }
            }
            if (!long.TryParse(nominator.ToString(), NumberStyles.Number, info, out var n))
            {
                return null;
            }

            string denominator = str.Substring(pos);

            if (!long.TryParse(denominator, NumberStyles.Number, info, out var d) || d == 0)
            {
                return null;
            }

            return new Fraction(n, d);
        }

        /// <summary>Returns true if the <see cref="char"/> is /, : or ÷.</summary>
        private static bool IsDivisionOperator(char ch) => "/:÷".IndexOf(ch) != Parsing.NotFound;
    }
}
