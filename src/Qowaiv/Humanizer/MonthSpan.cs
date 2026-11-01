namespace Qowaiv.Humanizer;

public static partial class Humanize
{
    extension(int months)
    {
        /// <summary>Create a <see cref="MonthSpan" /> from a <see cref="int" />.</summary>
        [Pure]
        public MonthSpan Months() => MonthSpan.FromMonths(months);
    }
}
