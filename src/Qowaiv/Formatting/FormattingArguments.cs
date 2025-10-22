namespace Qowaiv.Formatting;

/// <summary>Represents formatting arguments.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
public readonly struct FormattingArguments : IEquatable<FormattingArguments>
{
    /// <summary>Represents empty/not set formatting arguments.</summary>
    public static readonly FormattingArguments None;

    /// <summary>Initializes a new instance of the <see cref="FormattingArguments" /> struct.</summary>
    /// <param name="format">
    /// The format.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    public FormattingArguments(string? format, IFormatProvider? formatProvider)
    {
        Format = format;
        FormatProvider = formatProvider;
    }

    /// <summary>Initializes a new instance of the <see cref="FormattingArguments" /> struct.</summary>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    public FormattingArguments(IFormatProvider? formatProvider) : this(format: null, formatProvider) { }

    /// <summary>Initializes a new instance of the <see cref="FormattingArguments" /> struct.</summary>
    /// <param name="format">
    /// The format.
    /// </param>
    public FormattingArguments(string? format) : this(format, formatProvider: null) { }

    /// <summary>Gets the format.</summary>
    public string? Format { get; }

    /// <summary>Gets the format provider.</summary>
    public IFormatProvider? FormatProvider { get; }

    /// <summary>Deconstructs the formatting arguments in a format and formatProvider.</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Deconstruct(out string? format, out IFormatProvider? formatProvider)
    {
        format = Format;
        formatProvider = FormatProvider;
    }

    /// <summary>Formats the object using the formatting arguments.</summary>
    /// <param name="obj">
    /// The IFormattable object to get the formatted string representation from.
    /// </param>
    /// <returns>
    /// A formatted string representing the object.
    /// </returns>
    [Pure]
    [return: NotNullIfNotNull(nameof(obj))]
    public string? ToString(IFormattable? obj)
        => obj?.ToString(Format, FormatProvider ?? CultureInfo.CurrentCulture);

    /// <summary>Formats the object using the formatting arguments.</summary>
    /// <param name="obj">
    /// The object to get the formatted string representation from.
    /// </param>
    /// <returns>
    /// A formatted string representing the object.
    /// </returns>
    /// <remarks>
    /// If the object does not implement IFormattable, the ToString() will be used.
    /// </remarks>
    [Pure]
    [return: NotNullIfNotNull(nameof(obj))]
    public string? ToString(object? obj)
        => obj is IFormattable formattable
        ? ToString(formattable)
        : obj?.ToString();

    /// <summary>Returns a <see cref="string" /> that represents the current formatting arguments for debug purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay
        => string.Format(CultureInfo.InvariantCulture, "Format: '{0}', Provider: {1}", Format, FormatProvider);

    /// <inheritdoc />
    [Pure]
    public override bool Equals(object? obj) => obj is FormattingArguments args && Equals(args);

    /// <inheritdoc />
    [Pure]
    public bool Equals(FormattingArguments other)
    {
        if (Format != other.Format)
        {
            return false;
        }
        else if (FormatProvider is null)
        {
            return other.FormatProvider is null;
        }
        else return FormatProvider.Equals(other.FormatProvider);
    }

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Hash.Code(Format).And(FormatProvider);

    /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    [Pure]
    public static bool operator ==(FormattingArguments left, FormattingArguments right) => left.Equals(right);

    /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    public static bool operator !=(FormattingArguments left, FormattingArguments right) => !(left == right);
}
