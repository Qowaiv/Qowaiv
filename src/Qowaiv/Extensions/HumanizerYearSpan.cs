namespace Qowaiv;

/// <summary>Extensions to create <see cref="YearSpan" />s, inspired by Humanizer.NET.</summary>
public static class NumberToYearSpanExtensions
{
    /// <summary>Create a <see cref="YearSpan" /> from a <see cref="int" />.</summary>
    [Pure]
    public static YearSpan Years(this int years) => YearSpan.Create(years);
}
