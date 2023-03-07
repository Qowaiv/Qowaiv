namespace Qowaiv.Formatting;

/// <summary>Represents formatting arguments.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable]
public readonly struct FormattingArguments : ISerializable, IEquatable<FormattingArguments>
{
    /// <summary>Represents empty/not set formatting arguments.</summary>
    public static readonly FormattingArguments None;

    /// <summary>Initializes a new instance of the <see cref="FormattingArguments"/> struct.</summary>
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

    /// <summary>Initializes a new instance of the <see cref="FormattingArguments"/> struct.</summary>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    public FormattingArguments(IFormatProvider? formatProvider) : this(format: null, formatProvider) { }

    /// <summary>Initializes a new instance of the <see cref="FormattingArguments"/> struct.</summary>
    /// <param name="format">
    /// The format.
    /// </param>
    public FormattingArguments(string? format) : this(format, formatProvider: null) { }

    /// <summary>Gets the format.</summary>
    public string? Format { get; }

    /// <summary>Gets the format provider.</summary>
    public IFormatProvider? FormatProvider { get; }

    /// <summary>Formats the object using the formatting arguments.</summary>
    /// <param name="obj">
    /// The IFormattable object to get the formatted string representation from.
    /// </param>
    /// <returns>
    /// A formatted string representing the object.
    /// </returns>
    [Pure]
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
    public string? ToString(object? obj)
        => obj is IFormattable formattable
        ? ToString(formattable)
        : obj?.ToString();

    /// <summary>Initializes a new instance of the <see cref="FormattingArguments"/> struct.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    private FormattingArguments(SerializationInfo info, StreamingContext context)
    {
        Guard.NotNull(info, nameof(info));
        Format = info.GetString(nameof(Format));
        FormatProvider = (IFormatProvider?)info.GetValue(nameof(FormatProvider), typeof(IFormatProvider));
    }

    /// <summary>Adds the underlying property of formatting arguments to the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
        Guard.NotNull(info, nameof(info));
        info.AddValue(nameof(Format), Format);
        info.AddValue(nameof(FormatProvider), FormatProvider);
    }

    /// <summary>Returns a <see cref="string"/> that represents the current formatting arguments for debug purposes.</summary>
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
    public override int GetHashCode()
    {
        int hash = (Format == null) ? 0 : Format.GetHashCode();

        if (FormatProvider != null)
        {
            hash ^= FormatProvider.GetHashCode();
        }
        return hash;
    }

    /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    [Pure]
    public static bool operator ==(FormattingArguments left, FormattingArguments right)
        => left.Equals(right);

    /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator !=(FormattingArguments left, FormattingArguments right)
    {
        return !(left == right);
    }
}
