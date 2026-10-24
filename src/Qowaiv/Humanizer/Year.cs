namespace Qowaiv.Humanizer;

public static partial class Humanize
{
    extension(int year)
    {
        /// <summary>Create a <see cref="Year" /> from a <see cref="int" />  as Common Era. (also known as Anno Domini).</summary>
        [Pure]
        public Year CE() => Year.Create(year);
    }
}
