namespace System.Resources;

internal static class QowaivResourceManagerExtensions
{
    /// <summary>Localizing the resource using the 
    /// <see cref="IFormatProvider"/> defaulting to 
    /// <see cref="CultureInfo.CurrentCulture"/> if not a culture.
    /// </summary>
    [Pure]
    public static string Localized(this ResourceManager manager, IFormatProvider? provider, params string?[] keys)
        => manager.GetString(string.Concat(keys), provider as CultureInfo ?? CultureInfo.CurrentCulture)
        ?? string.Empty;
}
