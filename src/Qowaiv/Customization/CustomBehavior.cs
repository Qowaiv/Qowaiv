namespace Qowaiv.Customization;

/// <summary>Inheritable (custom) behavior for ad-hoc SVO's.</summary>
/// <typeparam name="TValue">
/// The type of the underlying value.
/// </typeparam>
public abstract class CustomBehavior<TValue> : TypeConverter, IComparer<TValue>
    where TValue : IEquatable<TValue>
{
    /// <inheritdoc />
    [Pure]
    public virtual int Compare(TValue? x, TValue? y) => Comparer<TValue>.Default.Compare(x!, y!);

    /// <inheritdoc />
    [Pure]
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    [Pure]
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        => TryTransform(value?.ToString(), culture, out var transformed)
        ? transformed
        : throw InvalidFormat(value?.ToString(), culture);

    /// <summary>Returns a formatted <see cref="string" /> that represents the underlying value of the identifier.</summary>
    [Pure]
    public abstract string ToString(TValue value, string? format, IFormatProvider? formatProvider);

    /// <summary>Serializes the underlying value to a JSON node.</summary>
    [Pure]
    public virtual object? ToJson(TValue value) => value switch
    {
        _ when Equals(value, default(TValue)) => null,
        IFormattable formattable /*........*/ => formattable.ToString(null, CultureInfo.InvariantCulture),
        _ /*...............................*/ => value.ToString(),
    };

    /// <summary>Serializes theidentifier to an XML string.</summary>
    /// <param name="value">
    /// The string representing the identifier.
    /// </param>
    [Pure]
    public virtual string? ToXml(TValue value) => value switch
    {
        _ when Equals(value, default(TValue)) => null,
        IFormattable formattable /*........*/ => formattable.ToString(null, CultureInfo.InvariantCulture),
        _ /*...............................*/ => value.ToString(),
    };

    /// <summary>Creates a <see cref="FormatException" /> using the <see cref="InvalidFormatMessage(string?, IFormatProvider?)" />.</summary>
    [Pure]
    public virtual FormatException InvalidFormat(string? str, IFormatProvider? formatProvider)
        => new(InvalidFormatMessage(str, formatProvider));

    /// <summary>Composes an invalid format message.</summary>
    /// <remarks>
    /// A 'For'-prefix will be stripped from the name in the message.
    /// </remarks>
    [Pure]
    public virtual string InvalidFormatMessage(string? str, IFormatProvider? formatProvider) => GetType() switch
    {
        var t when t.DeclaringType is { } declaring => $"Not a valid {declaring.Name}",
        var t when t.Name.StartsWith("For") /*...*/ => $"Not a valid {t.Name[3..]}",
        var t /*.................................*/ => $"Not a valid {t.Name}",
    };

    /// <summary>Converts the <see cref="string" /> to a SVO.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="str">
    /// A string containing the Single Value Object to convert.
    /// </param>
    /// <param name="formatProvider">
    /// The specified format provider.
    /// </param>
    /// <param name="transformed">
    /// The transformed value.
    /// </param>
    public abstract bool TryTransform(string? str, IFormatProvider? formatProvider, [NotNullWhen(true)] out TValue? transformed);

    /// <summary>Converts the to a SVO.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="value">
    /// A value containing the Single Value Object to convert.
    /// </param>
    /// <param name="transformed">
    /// The transformed value.
    /// </param>
    [Pure]
    public virtual bool TryTransform(TValue value, [NotNullWhen(true)] out TValue? transformed)
    {
        transformed = value;
        return true;
    }
}
