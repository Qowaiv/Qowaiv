using System;

namespace Qowaiv
{
    /// <summary>Extensions on <see cref="decimal"/> rounding.</summary>
    public static class DecimalRound
    {
        public static decimal Round(this decimal value) => value.Round(0);
        public static decimal Round(this decimal value, int decimals) => value.Round(decimals, DecimalRounding.Default);
        public static decimal Round(this decimal value, int decimals, int multiplyOf) => value.Round(decimals, DecimalRounding.Default, multiplyOf);
        public static decimal Round(this decimal value, int decimals, DecimalRounding rounding) => value.Round(decimals, rounding, 1);
        public static decimal Round(this decimal value, int decimals, DecimalRounding rounding, int multiplyOf)
        {
            Guard.DefinedEnum(rounding, nameof(rounding));
            Guard.Positive(multiplyOf, nameof(multiplyOf));

            if ((decimals < -28) || (decimals > 28))
            {
                throw new ArgumentOutOfRangeException(nameof(decimals), QowaivMessages.ArgumentOutOfRange_DecimalRound);
            }

            

            var rounded = multiplyOf == 1 ? value : value / multiplyOf;

            switch (rounding)
            {
                
                case DecimalRounding.AwayFromZero:
                    rounded = decimal.Round(rounded, decimals, MidpointRounding.AwayFromZero);
                    break;
                case DecimalRounding.Floor:
                    break;
                case DecimalRounding.Ceiling:
                    break;

                case DecimalRounding.Truncate:
                    break;

                // case DecimalRounding.ToEven
                default:
                    rounded = decimal.Round(rounded, decimals, MidpointRounding.ToEven);
                    break;
            }

            return rounded * multiplyOf;
        }
    }
}
