using System.Diagnostics.Contracts;

namespace Qowaiv
{
    /// <summary>Extensions to create <see cref="Qowaiv.Year"/>s, inspired by Humanizer.NET.</summary>
    public static class NumberToYearExtensions
    {
        /// <summary>Create a <see cref="Qowaiv.Year"/> from a <see cref="int"/>.</summary>
        [Pure]
        public static Year Year(this int year) => Qowaiv.Year.Create(year);
    }
}
