namespace Qowaiv.Humanizer;

/// <summary>Extensions to create <see cref="MonthSpan" />s, inspired by Humanizer.NET.</summary>
public static class NumberToMonthSpanExtensions
{
    extension(int months)
    {
        /// <summary>Create a <see cref="MonthSpan" /> from a <see cref="int" />.</summary>
        [Pure]
        public MonthSpan Months() => MonthSpan.FromMonths(months);
    }
}
