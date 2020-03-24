using System;
using System.Globalization;

namespace Qowaiv
{
    /// <summary>TryCreate factory method.</summary>
    internal delegate bool TryCreate<TPrimitive, TSvo>(TPrimitive? value, out TSvo result) where TPrimitive : struct;

    /// <summary>Culture dependent TryParse factory method.</summary>
    internal delegate bool TryParse<TSvo>(string str, IFormatProvider formatProvider, out TSvo result);

    /// <summary>Culture independent TryParse factory method.</summary>
    internal delegate bool TryParseInvariant<TSvo>(string str, out TSvo result);

    /// <summary>Helper class to facilitate <see cref="InvalidCastException"/> on SVO casting.</summary>
    internal static class Cast
    {
        /// <summary>Casts from a primitive (not <see cref="string"/>) to a SVO.</summary>
        public static TSvo Primitive<TPrimitive, TSvo>(TryCreate<TPrimitive, TSvo> tryCreate, TPrimitive? value)
            where TPrimitive : struct
        {
            if (tryCreate(value, out TSvo result))
            {
                return result;
            }
            throw Exceptions.InvalidCast<TPrimitive, TSvo>();
        }

        /// <summary>Casts from a <see cref="string"/> to a SVO.</summary>
        public static TSvo String<TSvo>(TryParse<TSvo> tryParse, string str)
        {
            if (tryParse(str, CultureInfo.CurrentCulture, out TSvo result))
            {
                return result;
            }
            throw Exceptions.InvalidCast<string, TSvo>();
        }

        /// <summary>Casts from a <see cref="string"/> that is not culture dependent to a SVO.</summary>
        public static TSvo InvariantString<TSvo>(TryParseInvariant<TSvo> tryParse, string str)
        {
            if (tryParse(str, out TSvo result))
            {
                return result;
            }
            throw Exceptions.InvalidCast<string, TSvo>();
        }

        /// <summary>Casts a <see cref="double"/> to <see cref="decimal"/> for the SVO.</summary>
        public static decimal ToDecimal<TSvo>(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value) || value < (double)decimal.MinValue || value > (double)decimal.MaxValue)
            {
                throw Exceptions.InvalidCast<double, TSvo>();
            }

            return (decimal)value;
        }

        /// <summary>Casts a <see cref="long"/> to <see cref="int"/> for the SVO.</summary>
        public static int ToInt<TSvo>(long value)
        {
            if (value < int.MinValue || value > int.MaxValue)
            {
                throw Exceptions.InvalidCast<long, TSvo>();
            }

            return (int)value;
        }


    }
}
