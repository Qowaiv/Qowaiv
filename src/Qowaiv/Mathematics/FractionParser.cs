using Qowaiv.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Qowaiv.Mathematics
{
    internal static class FractionParser
    {
        private const NumberStyles IntegerStyle = NumberStyles.Integer | NumberStyles.AllowThousands ^ NumberStyles.AllowTrailingSign;
        private const NumberStyles DecimalStyle = IntegerStyle | NumberStyles.AllowDecimalPoint | NumberStyles.AllowTrailingSign;

        [Flags]
        private enum Tokens
        {
            None /*       */ = 0x00,
            Operator /*   */ = 0x01,
            Space /*      */ = 0x02,
            Number /*     */ = 0x04,
            SuperScript /**/ = 0x08,
            SubScript   /**/ = 0x10,

            Reset = Operator| Space,
        }

        /// <summary>Parses a <see cref="string"/> that potentially contains a <see cref="Fraction"/>.</summary>
        /// <remarks>
        /// Category                   |  Pattern                      | Example
        /// ---------------------------------------------------------------------
        /// Simple                     |        [0-9]+ [/:÷] [0/9]+    |   1/3
        /// Simple with whole number   | [0-9]+ [0-9]+ [/:÷] [0/9]+    | 3 1/3
        /// Single vulgar              |         [¼½¾]                 |     ¾
        /// Whole number with vulgar   | [0-9]+ ?[¼½¾]                 |     ¾
        /// </remarks>
        public static Fraction? Parse(string s, NumberFormatInfo formatInfo)
        {
            if (ParseExternal(s, formatInfo, out var external))
            {
                return external;
            }

            var plus = formatInfo.PositiveSign;
            var min = formatInfo.NegativeSign;
            var integerSeperator = formatInfo.NumberGroupSeparator == " " ? '+' : ' ';

            var sign = 1;
            var str = new CharBuffer(s).Trim();
            var index = 0;

            // check for +/-.
            if (str.StartsWith(plus))
            {
                index += plus.Length;
            }
            else if (str.StartsWith(min))
            {
                index += min.Length;
                sign = -1;
            }

            long integer = 0;
            long nominator = 0;

            var buffer = new CharBuffer(str.Length - index);
            var tokens = Tokens.None;

            for (; index < str.Length; index++)
            {
                var ch = str[index];

                if (Fraction.Formatting.IsFractionBar(ch))
                {
                    // A second divide operator, or as last character is not allowed.
                    if (tokens.HasAll(Tokens.Operator) || str.EndOfBuffer(index))
                    {
                        return null;
                    }
                    // On a '/' assume the buffer is filled with the nominator.
                    if (!ParseInteger(buffer, formatInfo, out nominator))
                    {
                        return null;
                    }
                    tokens |= Tokens.Operator;
                    tokens &= Tokens.Reset;
                }
                else if (ch == integerSeperator)
                {
                    // A second spacer, or as last character is not allowed.
                    if (tokens.HasAll(Tokens.Space) || str.EndOfBuffer(index))
                    {
                        return null;
                    }
                    // On a ' ' assume the buffer is filled with the integer part.
                    if (!ParseInteger(buffer, formatInfo, out integer))
                    {
                        return null;
                    }
                    tokens |= Tokens.Space;
                    tokens &= Tokens.Reset;
                }
                else if (ch.IsVulgar(out var vulgar))
                {
                    // A vulgar is only allowed directly followed by an integer, or as single character.
                    if (tokens.HasAny(Tokens.Operator | Tokens.SuperScript| Tokens.SubScript) || !str.EndOfBuffer(index))
                    {
                        return null;
                    }

                    // If the buffer is not empty (integer without a space).
                    if (buffer.NotEmpty() && !buffer.ParseInteger(formatInfo, out integer))
                    {
                        return null;
                    }

                    // No overflow. :)
                    if (integer == 0)
                    {
                        return sign == +1 ? vulgar : -vulgar;
                    }
                    // Only if not overflow.
                    if (CalculateNominator(sign, integer, vulgar.Numerator, vulgar.Denominator, out nominator))
                    {
                        return nominator.DividedBy(vulgar.Denominator);
                    }
                    return null;
                }
                else if(ch.IsSuperScript(out char digit))
                {
                    if (tokens.HasAny(Tokens.SubScript | Tokens.Operator))
                    {
                        return null;
                    }
                    // If the buffer is not empty (integer without a space).
                    if (tokens.HasNone(Tokens.Space | Tokens.SuperScript) && buffer.NotEmpty())
                    {
                        if (!buffer.ParseInteger(formatInfo, out integer))
                        {
                            return null;
                        }
                        tokens |= Tokens.Space;
                        tokens &= Tokens.Reset;
                    }
                    buffer.Add(digit);
                    tokens |= Tokens.SuperScript;
                }
                else if(ch.IsSubScript(out digit))
                {
                    if (tokens.HasNone(Tokens.Operator) || tokens.HasAny(Tokens.SuperScript | Tokens.Number))
                    {
                        return null;
                    }
                    buffer.Add(digit);
                    tokens |= Tokens.SubScript;
                }
                else
                {
                    if (tokens.HasAny(Tokens.SuperScript | Tokens.SubScript))
                    {
                        return null;
                    }
                    buffer.Add(ch);
                    tokens |= Tokens.Number;
                }
            }

            // The remaining part should be the denominator.
            if (!buffer.ParseInteger(formatInfo, out var denominator) || denominator == 0)
            {
                return null;
            }

            // No overflow.
            if(CalculateNominator(sign, integer, nominator, denominator, out var combined))
            {
                return combined.DividedBy(denominator);
            }

            return null;
        }

        /// <summary>Parse the fraction, using <see cref="long.Parse(string)"/>, <see cref="decimal.Parse(string)"/>, and <see cref="Percentage.Parse(string)"/>.</summary>
        private static bool ParseExternal(string s, IFormatProvider formatInfo, out Fraction fraction)
        {
            fraction = default;

            if (long.TryParse(s, IntegerStyle, formatInfo, out var lng) && IsValid(lng))
            {
                fraction = Fraction.Create(lng);
                return true;
            }

            if (decimal.TryParse(s, DecimalStyle, formatInfo, out var dec) && IsValid(dec))
            {
                fraction = Fraction.Create(dec);
                return true;
            }

            if (PotentialPercentage(s) && Percentage.TryParse(s, formatInfo, out var percentage) && IsValid((decimal)percentage))
            {
                fraction = Fraction.Create((decimal)percentage);
                return true;
            }

            return false;
        }

        private static bool ParseInteger(this CharBuffer buffer, IFormatProvider formatInfo, out long integer)
        {
            var result = long.TryParse(buffer.ToString(), IntegerStyle, formatInfo, out integer);
            buffer.Clear();
            return result;
        }

        private static bool CalculateNominator(int sign, long integer, long nominator, long denominator, out long result)
        {
            try
            {
                checked
                {
                    result = (integer * denominator + nominator) * sign;
                    return true;
                }
            }
            catch (OverflowException)
            {
                result = default;
                return false;
            }
        }

        private static bool IsVulgar(this char ch, out Fraction fraction) => Vulgars.TryGetValue(ch, out fraction);

        private static bool IsSuperScript(this char ch, out char digit)
        {
            digit = default;
            var index = Fraction.Formatting.SuperScript.IndexOf(ch);

            if (index == Parsing.NotFound) { return false; }

            digit = (char)(index + '0');
            return true;
        }

        private static bool IsSubScript(this char ch, out char digit)
        {
            digit = default;
            var index = Fraction.Formatting.SubScript.IndexOf(ch);

            if (index == Parsing.NotFound) { return false; }

            digit = (char)(index + '0');
            return true;
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

        private static bool HasAll(this Tokens tokens, Tokens flag) => (tokens & flag) == flag;
        private static bool HasAny(this Tokens tokens, Tokens flag) => (tokens & flag) != 0;
        private static bool HasNone(this Tokens tokens, Tokens flag) => (tokens & flag) == 0;

        private static readonly Dictionary<char, Fraction> Vulgars = new Dictionary<char, Fraction>
        {
            { '½', 1.DividedBy(2) },

            { '⅓', 1.DividedBy(3) },
            { '⅔', 2.DividedBy(3) },

            { '¼', 1.DividedBy(4) },
            { '¾', 3.DividedBy(4) },

            { '⅕', 1.DividedBy(5) },
            { '⅖', 2.DividedBy(5) },
            { '⅗', 3.DividedBy(5) },
            { '⅘', 4.DividedBy(5) },

            { '⅙', 1.DividedBy(6) },
            { '⅚', 5.DividedBy(6) },

            { '⅐', 1.DividedBy(7) },

            { '⅛', 1.DividedBy(8) },
            { '⅜', 3.DividedBy(8) },
            { '⅝', 5.DividedBy(8) },
            { '⅞', 7.DividedBy(8) },

            { '⅑', 1.DividedBy(9) },
            { '⅒', 1.DividedBy(10) },

            { '↉', Fraction.Zero },
        };
    }
}
