namespace Qowaiv;

/// <summary>Extensions to create <see cref="MonthSpan" />s, inspired by Humanizer.NET.</summary>
public static class NumberToMonthSpanExtensions
{
    /// <summary>Create a <see cref="MonthSpan" /> from a <see cref="int" />.</summary>
    [Pure]
    public static MonthSpan Months(this int months) => MonthSpan.FromMonths(months);
}
