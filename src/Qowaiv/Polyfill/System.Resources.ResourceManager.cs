namespace System.Resources;

internal static class ResourceManagerExtensions
{
    extension(ResourceManager manager)
    {
        /// <summary>Localizing the resource using the
        /// <see cref="IFormatProvider" /> defaulting to
        /// <see cref="CultureInfo.CurrentCulture" /> if not a culture.
        /// </summary>
        [Pure]
        public string Localized(IFormatProvider? provider, string key)
            => manager.GetString(key, provider as CultureInfo ?? CultureInfo.CurrentCulture)
            ?? string.Empty;

        /// <summary>Check if the managers contains resources for the specified culture.</summary>
        [Pure]
        public bool Contains(CultureInfo culture)
            => manager.GetResourceSet(culture, createIfNotExists: true, tryParents: false) is not null;
    }
}
