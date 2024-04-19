namespace Qowaiv;

/// <summary>TryCreate factory method.</summary>
internal delegate bool TryCreate<TPrimitive, TSvo>(TPrimitive? value, out TSvo result) where TPrimitive : struct;

/// <summary>Culture dependent TryParse factory method.</summary>
internal delegate bool TryParse<TSvo>(string str, IFormatProvider formatProvider, out TSvo result);

/// <summary>Culture independent TryParse factory method.</summary>
internal delegate bool TryParseInvariant<TSvo>(string str, out TSvo result);

/// <summary>Helper class to facilitate <see cref="InvalidCastException"/> on SVO casting.</summary>
internal static class Cast
{
    private static class Dbl
    {
        public const double DecimalMin = (double)decimal.MinValue;
        public const double DecimalMax = (double)decimal.MaxValue;
    }

    /// <summary>Casts from a primitive (not <see cref="string"/>) to a SVO.</summary>
    [Pure]
    public static TSvo Primitive<TPrimitive, TSvo>(TryCreate<TPrimitive, TSvo> tryCreate, TPrimitive? value)
        where TPrimitive : struct
        => tryCreate(value, out TSvo result)
        ? result
        : throw Exceptions.InvalidCast<TPrimitive, TSvo>();

    /// <summary>Casts from a <see cref="string"/> to a SVO.</summary>
    [Pure]
    public static TSvo String<TSvo>(TryParse<TSvo> tryParse, string str)
        => tryParse(str, CultureInfo.CurrentCulture, out TSvo result)
        ? result
        : throw Exceptions.InvalidCast<string, TSvo>();

    /// <summary>Casts from a <see cref="string"/> that is not culture dependent to a SVO.</summary>
    [Pure]
    public static TSvo InvariantString<TSvo>(TryParseInvariant<TSvo> tryParse, string str)
        => tryParse(str, out TSvo result)
        ? result
        : throw Exceptions.InvalidCast<string, TSvo>();

    /// <summary>Casts a <see cref="double"/> to <see cref="decimal"/> for the SVO.</summary>
    [Pure]
    public static decimal ToDecimal<TSvo>(double value)
        => double.IsNaN(value)
        || double.IsInfinity(value)
        || !value.IsInRange(Dbl.DecimalMin, Dbl.DecimalMax)
        ? throw Exceptions.InvalidCast<double, TSvo>()
        : ToDecimal(value);

    [Pure]
    private static decimal ToDecimal(double value)
    {
        if (value >= Dbl.DecimalMax) return decimal.MaxValue;
        if (value <= Dbl.DecimalMin) return decimal.MinValue;
        return (decimal)value;
    }

    /// <summary>Casts a <see cref="double"/> to <see cref="int"/> for the SVO.</summary>
    [Pure]
    public static int ToInt<TSvo>(double value)
        => value < int.MinValue
        || value > int.MaxValue
        ? throw Exceptions.InvalidCast<long, TSvo>()
        : (int)value;

    /// <summary>Casts a <see cref="long"/> to <see cref="int"/> for the SVO.</summary>
    [Pure]
    public static int ToInt<TSvo>(long value)
        => value < int.MinValue
        || value > int.MaxValue
        ? throw Exceptions.InvalidCast<long, TSvo>()
        : (int)value;
}
