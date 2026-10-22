namespace Qowaiv;

/// <summary>Extensions to create <see cref="DateSpan" />s, inspired by Humanizer.NET.</summary>
public static class NumberToDateSpanExtensions
{
    extension(int days)
    {
        /// <summary>Create a <see cref="DateSpan" /> from a <see cref="int" />.</summary>
        [Pure]
        public DateSpan Days() => new(months: 0, days);
    }
}
