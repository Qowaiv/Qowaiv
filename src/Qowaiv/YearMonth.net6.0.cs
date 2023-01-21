#if NET6_0_OR_GREATER

namespace Qowaiv;

public readonly partial struct YearMonth
{
    /// <summary>Casts a <see cref="YearMonth"/> implictly to a <see cref="YearMonth"/>.</summary>
    public static explicit operator DateOnly(YearMonth date) => date.ToDate(01);

    /// <summary>Casts a <see cref="DateOnly"/> explicitly to a <see cref="YearMonth"/>.</summary>
    public static explicit operator YearMonth(DateOnly date) => new(date.Year, date.Month);
}
#endif
