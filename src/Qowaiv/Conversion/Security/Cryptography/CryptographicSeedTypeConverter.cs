using Qowaiv.Security.Cryptography;

namespace Qowaiv.Conversion.Security.Cryptography;

/// <summary>Provides a conversion for a cryptographic seed.</summary>
public class CryptographicSeedTypeConverter : TypeConverter
{
    /// <summary>A secret can only be converted from a string.</summary>
    [Pure]
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        => sourceType == typeof(string)
        || sourceType == typeof(byte[]);

    /// <summary>A secret can not be converted to anything.</summary>
    [Pure]
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) => false;

    /// <summary>A secret can not be converted to anything.</summary>
    [Pure]
    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) => null;

    /// <inheritdoc />
    [Pure]
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        => value switch
        {
            string str => CryptographicSeed.Parse(str),
            byte[] bytes => CryptographicSeed.Create(bytes),
            _ => CryptographicSeed.Empty,
        };
}
