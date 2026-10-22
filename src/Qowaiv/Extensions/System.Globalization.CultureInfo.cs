using Qowaiv.Globalization;

namespace Qowaiv.Extensions;

/// <summary>Extensions on <see cref="CultureInfo" />.</summary>
public static class CultureInfoExtensions
{
    extension(CultureInfo culture)
    {
        /// <summary>Gets a <see cref="CultureInfoScope" /> based on the <see cref="CultureInfo" />.</summary>
        [Impure]
        public CultureInfoScope Scoped() => new(culture);
    }
}
