using Qowaiv.Mathematics;

namespace Qowaiv;

/// <summary>Extensions to create <see cref="Percentage" />s, inspired by Humanizer.NET.</summary>
public static class NumberToPercentageExtensions
{
    /// <summary>Interprets the <see cref="int" /> if it was written with a '%' sign.</summary>
    [Pure]
    public static Percentage Percent(this int number) => ((decimal)number).Percent();

    /// <summary>Interprets the <see cref="double" /> if it was written with a '%' sign.</summary>
    [Pure]
    public static Percentage Percent(this double number) => Cast.ToDecimal<Percentage>(number).Percent();

    /// <summary>Interprets the <see cref="decimal" /> if it was written with a '%' sign.</summary>
    [Pure]
    public static Percentage Percent(this decimal number) => Percentage.Create(DecimalMath.ChangeScale(number, -2));
}
