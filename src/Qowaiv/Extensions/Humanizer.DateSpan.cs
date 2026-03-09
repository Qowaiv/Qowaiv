namespace Qowaiv;

/// <summary>Extensions to create <see cref="DateSpan" />s, inspired by Humanizer.NET.</summary>
public static class NumberToDateSpanExtensions
{
    /// <summary>Create a <see cref="DateSpan" /> from a <see cref="int" />.</summary>
    [Pure]
    public static DateSpan Days(this int days) => new(months: 0, days);
}
