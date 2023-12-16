#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable
using Qowaiv.Identifiers;
using System.Security.Cryptography;

namespace Qowaiv;

/// <summary>Represents a UUID (Universally unique identifier) aka GUID (Globally unique identifier).</summary>
/// <remarks>
/// The main difference between this UUID and the default System.GUID is
/// the default string representation. For this, that is a 22 char long
/// Base64 representation.
///
/// The reason not to call this a GUID but an UUID it just to prevent users of
/// Qowaiv to be forced to specify the namespace of there GUID of choice
/// everywhere.
/// </remarks>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.AllExcludingCulture ^ SingleValueStaticOptions.HasUnknownValue, typeof(Guid))]
[OpenApiDataType(description: "Universally unique identifier, Base64 encoded.", example: "lmZO_haEOTCwGsCcbIZFFg", type: "string", format: "uuid-base64", nullable: true)]
[OpenApi.OpenApiDataType(description: "Universally unique identifier, Base64 encoded.", example: "lmZO_haEOTCwGsCcbIZFFg", type: "string", format: "uuid-base64", nullable: true)]
[TypeConverter(typeof(UuidTypeConverter))]
#if NET5_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.UuidJsonConverter))]
#endif
public readonly partial struct Uuid : IXmlSerializable, IFormattable, IEquatable<Uuid>, IComparable, IComparable<Uuid>
{
    private static readonly UuidBehavior behavior = UuidBehavior.Instance;

    /// <summary>Gets the size of the <see cref="byte"/> array representation.</summary>
    public static readonly int ArraySize = 16;

    /// <summary>The index of the byte containing the version of a UUID is 7.</summary>
    internal const int IndexOfVersion = 7;

    /// <summary>Represents the pattern of a (potential) valid GUID.</summary>
    internal static readonly Regex Pattern = GetPattern();

#if NET7_0_OR_GREATER
    [GeneratedRegex(@"^[a-zA-Z0-9_-]{22}(=){0,2}$", RegOptions.Default, RegOptions.TimeoutMilliseconds)]
    private static partial Regex GetPattern();
#else
    [Pure]
    private static Regex GetPattern() => new(@"^[a-zA-Z0-9_-]{22}(=){0,2}$", RegOptions.Default, RegOptions.Timeout);
#endif

    /// <summary>Represents an empty/not set UUID.</summary>
    public static readonly Uuid Empty;

    /// <summary>Get the version of the UUID.</summary>
    public UuidVersion Version => m_Value.GetVersion();

    /// <summary>Returns a 16-element byte array that contains the value of this instance.</summary>
    [Pure]
    public byte[] ToByteArray() => m_Value.ToByteArray();

    /// <summary>Serializes the UUID to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string? ToJson() => m_Value == Guid.Empty ? null : ToString(CultureInfo.InvariantCulture);

    /// <summary>Returns a <see cref="string"/> that represents the current UUID for debug purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0:S}");

    /// <summary>Returns a formatted <see cref="string"/> that represents the current UUID.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    /// <remarks>
    /// S
    /// 22 base64 chars:
    /// 0123465798aAbBcCdDeE_-
    ///
    /// H
    /// 26 base32 chars:
    /// ABCDEFGHIJKLMNOPQRSTUVWXYZ234567
    ///
    /// N
    /// 32 digits:
    /// 00000000000000000000000000000000
    ///
    /// D
    /// 32 digits separated by hyphens:
    /// 00000000-0000-0000-0000-000000000000
    ///
    /// B
    /// 32 digits separated by hyphens, enclosed in braces:
    /// {00000000-0000-0000-0000-000000000000}
    ///
    /// P
    /// 32 digits separated by hyphens, enclosed in parentheses:
    /// (00000000-0000-0000-0000-000000000000)
    ///
    /// X
    /// Four hexadecimal values enclosed in braces, where the fourth value is a subset of eight hexadecimal values that is also enclosed in braces:
    /// {0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}}
    ///
    /// the lowercase formats are lowercase (except the the 's').
    /// </remarks>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
        => StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted)
        ? formatted
        : behavior.ToString(m_Value, format, formatProvider);

    /// <summary>Gets an XML string representation of the UUID.</summary>
    [Pure]
    private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Casts a Qowaiv.UUID to a System.GUID.</summary>
    public static implicit operator Guid(Uuid val) => val.m_Value;

    /// <summary>Casts a System.GUID to a Qowaiv.UUID.</summary>
    public static implicit operator Uuid(Guid val) => new(val);

    /// <summary>Initializes a new instance of a UUID.</summary>
    [Pure]
    public static Uuid NewUuid() => new(Guid.NewGuid());

    /// <summary>Initializes a new  instance of a UUID that is sequential.</summary>
    /// <remarks>
    /// * The first 7 bytes are based on ticks from <see cref="Clock.UtcNow()"/>.
    /// * The UUID's generated are sequential between 1970-01-01 and  9276-12-03.
    /// * Withing a timespan of 32 ticks (0.32 nanoseconds) the sequential part
    /// of UUID's are identical.
    /// </remarks>
    [Pure]
    public static Uuid NewSequential() => NewSequential(null);

    /// <summary>Initializes a new  instance of a UUID that is sequential.</summary>
    /// <param name="comparer">
    /// The comparer that determines the order to put the generated bytes in.
    /// </param>
    /// <remarks>
    /// * The first 7 bytes are based on ticks from <see cref="Clock.UtcNow()"/>.
    /// * The UUID's generated are sequential between 1970-01-01 and  9276-12-03.
    /// * Withing a timespan of 32 ticks (0.32 nanoseconds) the sequential part
    /// of UUID's are identical.
    /// </remarks>
    [Pure]
    public static Uuid NewSequential(UuidComparer? comparer)
    {
        var sequential = Clock.UtcNow().Ticks - TicksYear1970;
        if (sequential < 0) { throw new InvalidOperationException(QowaivMessages.InvalidOperation_SequentialUUID); }
        if (sequential > MaxTicks) { throw new InvalidOperationException(QowaivMessages.InvalidOperation_SequentialUUID); }
        sequential >>= 5;

        var prioritizer = (comparer ?? UuidComparer.Default).Priority;

        // replace the version byte with a fully random one.
        var random = Guid.NewGuid().ToByteArray();
        random[IndexOfVersion] = random[0];

        // set the sequential part.
        for (var i = 6; i >= 0; i--)
        {
            random[i] = (byte)sequential;
            sequential >>= 8;
        }

        var bytes = new byte[ArraySize];

        // setting the priority.
        for (var index = 0; index < ArraySize; index++)
        {
            var prio = prioritizer[index];
            bytes[prio] = random[index];
        }

        // setting the version.
        SetVersion(bytes, UuidVersion.Sequential);

        return new Guid(bytes);
    }

    private const long TicksYear1970 = 0x89F_7FF5_F7B5_8000;

    private const long MaxTicks = 0x1FFF_FFFF_FFFF_FFFF;

    /// <summary>Converts the string to a UUID.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing a UUID to convert.
    /// </param>
    /// <param name="provider">
    /// The specified format provider.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    [Pure]
    public static bool TryParse(string? s, IFormatProvider? provider, out Uuid result)
    {
        result = default;
        if (behavior.TryParse(s, out var id))
        {
            result = id is Guid guid ? new Uuid(guid) : Empty;
            return true;
        }
        return false;
    }

    /// <summary>Generates an <see cref="Uuid"/> applying a <see cref="MD5"/> hash on the data.</summary>
    [Pure]
    public static Uuid GenerateWithMD5(byte[] data)
    {
        Guard.NotNull(data);
#if NETSTANDARD
        var hash = MD5.Create().ComputeHash(data);
#else
        var hash = MD5.HashData(data);
#endif
        SetVersion(hash, UuidVersion.MD5);
        return new Guid(hash);
    }

    /// <summary>Generates an <see cref="Uuid"/> applying a <see cref="SHA1"/> hash on the data.</summary>
    [Pure]
    public static Uuid GenerateWithSHA1(byte[] data)
    {
        Guard.NotNull(data);
#if NETSTANDARD
        var bytes = SHA1.Create().ComputeHash(data);
#else
        var bytes = SHA1.HashData(data);
#endif
        var hash = new byte[ArraySize];
        Array.Copy(bytes, hash, ArraySize);
        SetVersion(hash, UuidVersion.SHA1);
        return new Guid(hash);
    }

    private static void SetVersion(byte[] uuid, UuidVersion version)
    {
        uuid[IndexOfVersion] &= 0x0F;
        uuid[IndexOfVersion] |= unchecked((byte)((int)version << 4));
    }
}
