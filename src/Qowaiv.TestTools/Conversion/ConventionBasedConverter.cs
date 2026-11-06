using System.Reflection;
using static System.Convert;

namespace Qowaiv.TestTools.Conversion;

/// <summary>
/// Comverter that can convert an <see cref="object"/> value of an assumed
/// source type to a (predefined) target type.
/// </summary>
/// <remarks>
/// Can be useful when registering AOT Converters for TUnit.
/// </remarks>
public sealed class ConventionBasedConverter(Type sourceType, Type targetType, TypeConverter? typeConverter = null)
{
    /// <summary>Describes the conversion.</summary>
    public TypeConversion Conversion { get; } = new(sourceType, targetType);

    /// <summary>The source type.</summary>
    public Type SourceType => Conversion.SourceType;

    /// <summary>The target type.</summary>
    public Type TargetType => Conversion.TargetType;

    /// <summary>The <see cref="System.ComponentModel.TypeConverter"/> that can do the conversion.</summary>
    /// <remarks>
    /// If not specified by the constructor, and the <see cref="TargetType" />s
    /// converter supports the conversion, that one is picked.
    /// </remarks>
    public TypeConverter? TypeConverter => field ??= typeConverter ?? Conversion.TypeConverter;

    /// <summary>The (implict or explict) cast operator (if existing).</summary>
    public MethodInfo? Cast => Conversion.Cast;

    /// <summary>Converts the value.</summary>
    /// <remarks>
    /// Order of preference:
    /// 1. <see cref="TypeConverter"/>
    /// 2. <see cref="Cast"/>
    /// 3. <see cref="ChangeType(object, Type, IFormatProvider)" />.
    /// </remarks>
    [Pure]
    public object? Convert(object? value) => value switch
    {
        null => null,
        _ when TypeConverter is { } converter => converter.ConvertFrom(context: null, CultureInfo.InvariantCulture, value),
        _ when Cast is { } cast => cast.Invoke(null, [value]),
        _ => ChangeType(value, TargetType, CultureInfo.InvariantCulture),
    };

    /// <inheritdoc />
    [Pure]
    public override string ToString() => Conversion.ToString();
}
