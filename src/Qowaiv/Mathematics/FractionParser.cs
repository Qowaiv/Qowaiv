using Qowaiv.Text;
using System.Globalization;

namespace Qowaiv.Mathematics
{
    internal static class FractionParser
    {
        private const NumberStyles IntegerStyle = NumberStyles.Integer | NumberStyles.AllowThousands;
        private const NumberStyles DecimalStyle = IntegerStyle | NumberStyles.AllowDecimalPoint;

        public static Fraction? Parse(string s, NumberFormatInfo info)
        {
            // Integers are fine.
            if (long.TryParse(s, IntegerStyle, info, out var lng))
            {
                return Fraction.Create(lng);
            }
            // Decimals are fine.
            if (decimal.TryParse(s, DecimalStyle, info, out var dec) && dec >= long.MinValue && dec <= long.MaxValue)
            {
                return Fraction.Create(dec);
            }
            // Percentages are fine.
            if (Percentage.TryParse(s, out var percentage))
            {
                return Fraction.Create((decimal)percentage);
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
            for (/**/; pos < str.Length; pos++)
            {
                var ch = str[pos];

                if (char.IsNumber(ch))
                {
                    nominator.Add(ch);
                }
                else if (IsDivisionOperator(ch))
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
