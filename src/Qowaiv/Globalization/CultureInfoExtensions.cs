using System.Globalization;

namespace Qowaiv.Globalization
{
    /// <summary>Extensions on <see cref="CultureInfo"/>.</summary>
    public static class CultureInfoExtensions
    {
        /// <summary>Gets a <see cref="CultureInfoScope"/> based on the <see cref="CultureInfo"/>.</summary>
        public static CultureInfoScope Scoped(this CultureInfo culture) => new CultureInfoScope(culture);
    }
}
