using Qowaiv.Text;
using System;
using System.Globalization;
using System.Linq;

namespace Qowaiv.Mathematics
{
    internal static class FractionParser
    {
        private const NumberStyles IntegerStyle = NumberStyles.Integer | NumberStyles.AllowThousands ^ NumberStyles.AllowTrailingSign;
        private const NumberStyles DecimalStyle = IntegerStyle | NumberStyles.AllowDecimalPoint | NumberStyles.AllowTrailingSign;

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
            var integerSeperator = info.NumberGroupSeparator == " " ? '+' : ' ';

            var sign = 1;

            var str = new CharBuffer(s).Trim();
            var pos = 0;

            // check for +/-.
            if (str.StartsWith(plus))
            {
                pos += plus.Length;
            }
            else if (str.StartsWith(min))
            {
                pos += min.Length;
                sign = -1;
            }

            long integer = Parsing.NotFound;
            long nominator = Parsing.NotFound;
            var buffer = new CharBuffer(str.Length);

            // Detect the nominator/integer.
            for (; pos < str.Length; pos++)
            {
                var ch = str[pos];

                if (IsDivisionOperator(ch))
                {
                    if (str.EndOfBuffer(pos) || !buffer.ParseInteger(info, out nominator))
                    {
                        return null;
                    }
                    pos++;
                    break;
                }
                else if(ch == integerSeperator)
                {
                    if (str.EndOfBuffer(pos) || !buffer.ParseInteger(info, out integer))
                    {
                        return null;
                    }
                    pos++;
                    break;
                }
                else
                {
                    buffer.Add(ch);
                }
            }

            // Detect the nominator.
            if (integer != Parsing.NotFound)
            {
                for (; pos < str.Length; pos++)
                {
                    var ch = str[pos];

                    if (IsDivisionOperator(ch))
                    {
                        if (str.EndOfBuffer(pos) || !buffer.ParseInteger(info, out nominator))
                        {
                            return null;
                        }
                        pos++;
                        break;
                    }
                    buffer.Add(ch);
                }
            }

            // Detect the denominator.
            if (!long.TryParse(str.Substring(pos), IntegerStyle, info, out long denominator) || denominator == 0)
            {
                return null;
            }

            // Add the integer part if relevant.
            if (integer != Parsing.NotFound) 
            {
                try
                {
                    checked
                    {
                        nominator += integer * denominator;
                    }
                }
                catch (OverflowException)
                {
                    return null;
                }
            }

            return new Fraction(sign * nominator, denominator);
        }

        private static bool ParseInteger(this CharBuffer buffer, NumberFormatInfo formatInfo, out long integer)
        {
            var result = long.TryParse(buffer.ToString(), IntegerStyle, formatInfo, out integer);
            buffer.Clear();
            return result;
        }

        /// <summary>All long values are valid, except <see cref="long.MinValue"/>.</summary>
        private static bool IsValid(long number) => number != long.MinValue;

        /// <summary>Number should be in the range of <see cref="Fraction.MinValue"/> and <see cref="Fraction.MaxValue"/>.</summary>
        private static bool IsValid(decimal number) => number >= Fraction.MinValue.Numerator && number <= Fraction.MaxValue.Numerator;

        /// <summary>Only strings containing percentage markers (%, ‰, ‱) should be parsed by <see cref="Percentage.TryParse(string)"/>.</summary>
        private static bool PotentialPercentage(string str)
        {
            return str.Any(ch => "%‰‱".IndexOf(ch) != Parsing.NotFound);
        }

        /// <summary>Returns true if the <see cref="char"/> is /, : or ÷.</summary>
        private static bool IsDivisionOperator(char ch) => "/:÷".IndexOf(ch) != Parsing.NotFound;
    }
}
