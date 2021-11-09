using Qowaiv.Globalization;

namespace System.Globalization
{
    /// <summary>Extensions on <see cref="CultureInfo"/>.</summary>
    public static class QowaivCultureInfoExtensions
    {
        /// <summary>Gets a <see cref="CultureInfoScope"/> based on the <see cref="CultureInfo"/>.</summary>
        [Impure]
        public static CultureInfoScope Scoped(this CultureInfo culture) => new(culture);
    }
}
