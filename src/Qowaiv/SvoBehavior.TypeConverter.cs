#pragma warning disable S1694 // An abstract class should have both abstract and concrete methods
// Justification: SvoBehavior instances can be created via reflection, this one should then be excluded.

namespace Qowaiv;

public partial class SvoBehavior : TypeConverter
{  
    /// <inheritdoc />
    [Pure]
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string);

    /// <inheritdoc />
    [Pure]
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => destinationType == typeof(string);

    /// <inheritdoc />
    [Pure]
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        => TryParse(value?.ToString(), culture, out var parsed)
        ? parsed
        : throw InvalidFormat(value?.ToString());

    /// <inheritdoc />
    [Pure]
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        => ToString(value?.ToString(), default, culture);

}
