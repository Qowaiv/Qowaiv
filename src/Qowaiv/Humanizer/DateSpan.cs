namespace Qowaiv.Humanizer;

public static partial class Humanize
{
    extension(int days)
    {
        /// <summary>Create a <see cref="DateSpan" /> from a <see cref="int" />.</summary>
        [Pure]
        public DateSpan Days() => new(months: 0, days);
    }
}
