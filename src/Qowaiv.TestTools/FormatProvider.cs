namespace Qowaiv.TestTools;

/// <summary>Helper class for getting <see cref="IFormatProvider"/>s.</summary>
public static class FormatProvider
{
    /// <summary>Gets an empty <see cref="IFormatProvider"/>.</summary>
    /// <remarks>
    /// <see cref="IFormatProvider.GetFormat(Type)"/> always returns null.
    /// </remarks>
    public static readonly IFormatProvider CustomFormatter = new CustomFormatProvider();

    /// <summary>Gets an empty <see cref="IFormatProvider"/>.</summary>
    /// <remarks>
    /// <see cref="IFormatProvider.GetFormat(Type)"/> always returns null.
    /// </remarks>
    public static readonly IFormatProvider Empty = new EmptyFormatProvider();

    /// <summary>Represents a format provider, that contains no format types.</summary>
    private sealed class EmptyFormatProvider : IFormatProvider
    {
        /// <summary>Always returns null.</summary>
        [Pure]
        public object? GetFormat(Type? formatType) => null;
    }

    /// <summary>Represents the unit test format provider.</summary>
    private sealed class CustomFormatProvider: IFormatProvider, ICustomFormatter
    {
        /// <summary>Returns an object that provides formatting services for the specified type.</summary>
        /// <param name="formatType">
        /// The type of format object to return.
        /// </param>
        /// <remarks>
        /// Supports ICustomFormatter.
        /// </remarks>
        [Pure]
        public object? GetFormat(Type? formatType)
            => (typeof(ICustomFormatter).IsAssignableFrom(formatType)) ? this : null;

        /// <summary>Formats the object as String.</summary>
        /// <param name="format">
        /// The format to use.
        /// </param>
        /// <param name="arg">
        /// The object to format.
        /// </param>
        /// <param name="provider">
        /// The specified provider.
        /// </param>
        [Pure]
        public string Format(string? format, object? arg, IFormatProvider? provider)
        {
            var str = "Unit Test Formatter, value: '{0:" + format + "}', format: '{1}'";
            return string.Format(CultureInfo.InvariantCulture, str, arg, format);
        }
    }
}
