namespace Qowaiv;

/// <summary>Extensions to create <see cref="YearSpan" />s, inspired by Humanizer.NET.</summary>
public static class NumberToYearSpanExtensions
{
    extension(int years)
    {
        /// <summary>Create a <see cref="YearSpan" /> from a <see cref="int" />.</summary>
        [Pure]
        public YearSpan Years() => YearSpan.Create(years);
    }
}
