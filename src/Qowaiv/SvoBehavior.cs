namespace Qowaiv;

public abstract class SvoBehavior : TypeConverter, IComparer<string>
{
    /// <summary>If specified, it defines the minimum length the string representation of the Single Value Object may be.</summary>
    public virtual int? MinLength => default;

    /// <summary>If specified, it defines the maximum length the string representation of the Single Value Object may be.</summary>
    public virtual int? MaxLength => default;

    /// <summary>If specified, it defines the maximum length the string representation of the Single Value Object may be.</summary>
    public virtual Regex? Pattern => null;

    /// <inheritdoc />
    [Pure]
    public virtual int Compare(string? x, string? y) => Comparer<string>.Default.Compare(x, y);

    /// <inheritdoc />
    [Pure]
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string)
        || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    [Pure]
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => destinationType == typeof(string)
        || base.CanConvertTo(context, destinationType);

    /// <inheritdoc />
    [Pure]
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        => TryParse(value?.ToString(), culture, out var parsed)
        ? parsed
        : throw new FormatException(InvalidFormat(value?.ToString()));

    /// <inheritdoc />
    [Pure]
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        => ToString(value?.ToString(), default, culture);

    [Pure]
    public virtual int Length(string str) => str is null ? 0 : str.Length;

    [Pure]
    public virtual string Unknown(string? format, IFormatProvider? formatProvider) => "?";

    [Pure]
    public virtual string ToString(string str, string? format, IFormatProvider? formatProvider)
    {
        return str;
    }

    [Pure]
    public virtual string ToJson(string str) => str;

    [Pure]
    public virtual bool IsUnknown(string str, IFormatProvider? formatProvider) => Qowaiv.Unknown.IsUnknown(str, formatProvider as CultureInfo);

    /// <summary>Converts the <see cref="string"/> to <see cref="Svo"/>.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="str">
    /// A string containing the Single Value Object to convert.
    /// </param>
    /// <param name="formatProvider">
    /// The specified format provider.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    [Pure]
    public virtual bool TryParse(string? str, IFormatProvider? formatProvider, out string result)
    {
        result = str;
        return true;

        //result = default;
        //if (string.IsNullOrEmpty(s))
        //{
        //    return true;
        //}
        //if (Qowaiv.Unknown.IsUnknown(s, formatProvider as CultureInfo))
        //{
        //    result = Unknown;
        //    return true;
        //}
        //throw new NotImplementedException();
    }

    [Pure]
    public virtual string Normalize(string? str) => str?.Trim() ?? string.Empty;

    [Pure]
    public virtual string InvalidFormat(string? str)
        => GetType().Name.StartsWith("For")
        ? $"Not a valid {GetType().Name.Substring(3)}"
        : $"Not a valid {GetType().Name}";

    
}
