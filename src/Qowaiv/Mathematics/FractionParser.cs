using Qowaiv.Text;
using System;
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
            if (long.TryParse(s, IntegerStyle, info, out var lng) && IsValid(lng))
            {
                return Fraction.Create(lng);
            }
            // Decimals are fine.
            if (decimal.TryParse(s, DecimalStyle, info, out var dec) && IsValid(dec))
            {
                return Fraction.Create(dec);
            }
            // Percentages are fine
            if (PotentialPercentage(s) && Percentage.TryParse(s, info, out var percentage) && IsValid((decimal)percentage))
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

        /// <summary>All long values are valid, except <see cref="long.MinValue"/>.</summary>
        private static bool IsValid(long number) => number != long.MinValue;

        /// <summary>Number should be in the range of <see cref="Fraction.MinValue"/> and <see cref="Fraction.MaxValue"/>.</summary>
        private static bool IsValid(decimal number) => number >= Fraction.MinValue.Numerator && number <= Fraction.MaxValue.Numerator;

        /// <summary>Only strings containing percentage markers (%, ‰, ‱) should be parsed by <see cref="Percentage.TryParse(string)"/>.</summary>
        private static bool PotentialPercentage(string str)
        {
            return "%‰‱".IndexOf(str, StringComparison.InvariantCulture) != Parsing.NotFound;
        }

        /// <summary>Returns true if the <see cref="char"/> is /, : or ÷.</summary>
        private static bool IsDivisionOperator(char ch) => "/:÷".IndexOf(ch) != Parsing.NotFound;
    }
}
