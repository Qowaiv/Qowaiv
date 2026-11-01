#if !NETSTANDARD2_0

namespace Qowaiv;

public readonly partial struct DateSpan
{
    /// <inheritdoc cref="Age(Date, Date)" />
    [Pure]
    public static DateSpan Age(DateOnly date) => Age((Date)date);

    /// <inheritdoc cref="Age(Date, Date)" />
    [Pure]
    public static DateSpan Age(DateOnly date, DateOnly reference) => Age((Date)date, (Date)reference);

    /// <inheritdoc cref="Subtract(Date, Date)" />
    [Pure]
    public static DateSpan Subtract(DateOnly d1, DateOnly d2) => Subtract((Date)d1, (Date)d2);

    /// <inheritdoc cref="Subtract(Date, Date, DateSpanSettings)" />
    [Pure]
    public static DateSpan Subtract(DateOnly d1, DateOnly d2, DateSpanSettings settings) => Subtract((Date)d1, (Date)d2, settings);
}
#endif
