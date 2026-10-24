namespace Qowaiv.Humanizer;

public static partial class Humanize
{
    extension(int years)
    {
        /// <summary>Create a <see cref="YearSpan" /> from a <see cref="int" />.</summary>
        [Pure]
        public YearSpan Years() => YearSpan.Create(years);
    }
}
