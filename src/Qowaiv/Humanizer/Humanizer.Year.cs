namespace Qowaiv.Humanizer;

/// <summary>Extensions to create <see cref="Year" />s, inspired by Humanizer.NET.</summary>
public static class NumberToYearExtensions
{
    extension(int year)
    {
        /// <summary>Create a <see cref="Year" /> from a <see cref="int" />  as Common Era. (also known as Anno Domini).</summary>
        [Pure]
        public Year CE() => Year.Create(year);
    }
}
