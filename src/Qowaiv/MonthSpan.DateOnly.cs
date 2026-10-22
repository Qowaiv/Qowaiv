#if !NETSTANDARD2_0

namespace Qowaiv;

public readonly partial struct MonthSpan
{
    /// <inheritdoc cref="Subtract(Date, Date)" />
    [Pure]
    public static MonthSpan Subtract(DateOnly d1, DateOnly d2) => Subtract((Date)d1, (Date)d2);
}

#endif
