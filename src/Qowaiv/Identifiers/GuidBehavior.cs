﻿namespace Qowaiv.Identifiers;

/// <summary>Implements <see cref="IIdentifierBehavior"/> for an identifier based on <see cref="Guid"/>.</summary>
public class GuidBehavior : IdentifierBehavior
{
    internal static readonly GuidBehavior Instance = new();

    /// <summary>Creates a new instance of the <see cref="GuidBehavior"/> class.</summary>
    protected GuidBehavior() { }

    /// <summary>Returns the type of the underlying value (<see cref="Guid"/>).</summary>
    public sealed override Type BaseType => typeof(Guid);

    /// <summary>Gets the default format used to represent the <see cref="Guid"/> as <see cref="string"/>.</summary>
    protected virtual string DefaultFormat => "d";

    /// <inheritdoc/>
    [Pure]
    public override int Compare(object? x, object? y) => Id(x).CompareTo(Id(y));

    /// <inheritdoc/>
    [Pure]
    public override bool Equals(object? x, object? y) => Id(x).Equals(Id(y));

    /// <inheritdoc/>
    [Pure]
    public override int GetHashCode(object? obj) => Id(obj).GetHashCode();

    /// <inheritdoc/>
    [Pure]
    public override byte[] ToByteArray(object? obj)
        => obj is Guid guid ? guid.ToByteArray() : Array.Empty<byte>();

    /// <inheritdoc/>
    [Pure]
    public override object? FromBytes(byte[] bytes) => new Guid(bytes);

    /// <summary>Returns a formatted <see cref="string"/> that represents the <see cref="Guid"/>.</summary>
    /// <param name="obj">
    /// The object that is expected to be a <see cref="Guid"/>.
    /// </param>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    /// <remarks>
    /// S => 22 base64 chars:  0123465798aAbBcCdDeE_-
    /// 
    /// H => 26 base32 chars: ABCDEFGHIJKLMNOPQRSTUVWXYZ234567
    /// 
    /// N => 32 digits: 00000000000000000000000000000000
    /// 
    /// D => 32 digits separated by hyphens: 00000000-0000-0000-0000-000000000000
    /// 
    /// B => 32 digits separated by hyphens, enclosed in braces: {00000000-0000-0000-0000-000000000000}
    /// 
    /// P => 32 digits separated by hyphens, enclosed in parentheses: (00000000-0000-0000-0000-000000000000)
    /// 
    /// X => Four hexadecimal values enclosed in braces, where the fourth value is a subset of eight hexadecimal values 
    ///     that is also enclosed in braces: {0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}}
    /// 
    /// the lowercase formats are lowercase (except the 's').
    /// </remarks>
    [Pure]
    public override string ToString(object? obj, string? format, IFormatProvider? formatProvider)
    {
        var id = Id(obj);
        return id == Guid.Empty 
            ? string.Empty
            : Tostring(id, format.WithDefault(DefaultFormat), formatProvider);
    }

#pragma warning disable S1541 // Not that complex
    [Pure]
    private static string Tostring(Guid id, string format, IFormatProvider? formatProvider)
        => format switch
        {
            "s" or "S" => ToBase64String(id),
            "h" => Base32.ToString(id.ToByteArray(), true),
            "H" => Base32.ToString(id.ToByteArray(), false),
            "N" or "D" or "B" or "P" => id.ToString(format, formatProvider).ToUpperInvariant(),
            "X" => id.ToString(format, formatProvider).ToUpperInvariant().Replace('X', 'x'),
            _ => id.ToString(format, formatProvider),
        };
#pragma warning restore S1541 // Methods and properties should not be too complex

    /// <remarks>Avoids invalid URL characters.</remarks>
    [Pure]
    private static string ToBase64String(Guid id)
        => Convert.ToBase64String(id.ToByteArray()).Replace('+', '-').Replace('/', '_').Substring(0, 22);

    /// <inheritdoc/>
    [Pure]
    public override object? ToJson(object? obj) => ToString(obj, DefaultFormat, CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    [Pure]
    public override bool TryParse(string? str, out object? id)
    {
        if (str is not { Length: > 0})
        {
            id = default;
            return true;
        }
        else if (Guid.TryParse(str, out Guid guid))
        {
            id = guid == Guid.Empty ? null : guid;
            return true;
        }
        else if (Uuid.Pattern.IsMatch(str))
        {
            var bytes = Convert.FromBase64String(str.Replace('-', '+').Replace('_', '/').Substring(0, 22) + "==");
            id = new Guid(bytes);

            if (Guid.Empty.Equals(id))
            {
                id = null;
            }
            return true;
        }
        else if (str.Length == 26 && Base32.TryGetBytes(str, out var b32))
        {
            id = new Guid(b32);

            if (Guid.Empty.Equals(id))
            {
                id = null;
            }
            return true;
        }
        else
        {
            id = default;
            return false;
        }
    }

    /// <inheritdoc/>
    [Pure]
    public override bool TryCreate(object? obj, out object? id)
    {
        if (obj is Guid guid)
        {
            id = guid == Guid.Empty ? null : guid;
            return true;
        }
        else if (obj is Uuid uuid)
        {
            id = uuid == Uuid.Empty ? null : (Guid)uuid;
            return true;
        }
        else if (obj is string str && TryParse(str, out id))
        {
            return true;
        }
        else
        {
            id = default;
            return false;
        }
    }

    /// <inheritdoc/>
    [Pure]
    public override object Next() => Guid.NewGuid();

    /// <inheritdoc />
    [Pure]
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(Guid)
        || sourceType == typeof(Uuid)
        || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    [Pure]
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => destinationType == typeof(Guid)
        || destinationType == typeof(Uuid)
        || base.CanConvertTo(context, destinationType);

    /// <inheritdoc />
    [Pure]
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        => destinationType == typeof(Uuid) && value is Guid guid
        ? (Uuid)guid
        : base.ConvertTo(context, culture, value, destinationType);

    [Pure]
    private static Guid Id(object? obj) => obj is Guid guid ? guid : Guid.Empty;
}
