﻿#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

namespace Qowaiv;

/// <summary>Represents a Sex.</summary>
/// <remarks>
/// Defines a representation of human sexes through a language-neutral 
/// single-digit code. International standard ISO 5218, titled Information
/// technology - Codes for the representation of human sexes.
/// 
/// It can be used in information systems such as database applications.
/// 
/// The four codes specified in ISO/IEC 5218 are:
/// 0 = not known,
/// 1 = male,
/// 2 = female,
/// 9 = not applicable.
/// The standard specifies that its use may be referred to by the
/// designator "SEX".
/// </remarks>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable, SingleValueObject(SingleValueStaticOptions.All, typeof(byte))]
[OpenApiDataType(description: "Sex as specified by ISO/IEC 5218.", example: "female", type: "string", format: "sex", nullable: true, @enum: "NotKnown,Male,Female,NotApplicable")]
[TypeConverter(typeof(SexTypeConverter))]
public partial struct Sex : ISerializable, IXmlSerializable, IFormattable, IEquatable<Sex>, IComparable, IComparable<Sex>
{
    /// <summary>Represents an empty/not set Sex.</summary>
    public static readonly Sex Empty;

    /// <summary>Represents a not known/unknown sex.</summary>
    public static readonly Sex Unknown = new(1);
    /// <summary>Represents a male.</summary>
    public static readonly Sex Male = new(2);
    /// <summary>Represents a female.</summary>
    public static readonly Sex Female = new(4);
    /// <summary>Represents a not applicable sex.</summary>
    public static readonly Sex NotApplicable = new(18);

    /// <summary>Contains not known, male, female, not applicable.</summary>
    public static readonly IReadOnlyCollection<Sex> All = new[] { Male, Female, NotApplicable, Unknown, };

    /// <summary>Contains male and female.</summary>
    public static readonly IReadOnlyCollection<Sex> MaleAndFemale = new[] { Male, Female, };

    /// <summary>Contains male, female, not applicable.</summary>
    public static readonly IReadOnlyCollection<Sex> MaleFemaleAndNotApplicable = new[] { Male, Female, NotApplicable, };

    /// <summary>Gets the display name.</summary>
    public string DisplayName => GetDisplayName(null);

    /// <summary>Returns true if the Sex is male or female, otherwise false.</summary>
    [Pure]
    public bool IsMaleOrFemale() => Equals(Male) || Equals(Female);

    /// <summary>Gets the display name for a specified culture.</summary>
    /// <param name="culture">
    /// The culture of the display name.
    /// </param>
    /// <returns></returns>
    [Pure]
    public string GetDisplayName(CultureInfo culture) => GetResourceString(string.Empty, culture);

    /// <summary>Converts the Sex to an int.</summary>
    [Pure]
    private int ToInt32() => m_Value >> 1;

    /// <summary>Converts the Sex to an int.</summary>
    [Pure]
    private int? ToNullableInt32() => ToNullableInt32s[m_Value];

    /// <summary>Deserializes the sex from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized sex.
    /// </returns>
    [Pure]
    public static Sex FromJson(double json) => Create((int)json);

    /// <summary>Deserializes the sex from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized sex.
    /// </returns>
    [Pure]
    public static Sex FromJson(long json) => Create((int)json);

    /// <summary>Serializes the sex to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string ToJson() => SexLabels[m_Value];

    /// <summary>Returns a <see cref="string"/> that represents the current Sex for debug purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => IsEmpty()
        ? DebugDisplay.Empty
        : GetDisplayName(CultureInfo.InvariantCulture);

    /// <summary>Returns a formatted <see cref="string"/> that represents the current Sex.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    /// <remarks>
    /// The formats:
    /// 
    /// i: as integer.
    /// c: as single character.
    /// h: as Honorific.
    /// s: as Symbol.
    /// f: as formatted/display name.
    /// </remarks>
    [Pure]
    public string ToString(string format, IFormatProvider formatProvider)
        => StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted)
        ? formatted
        : StringFormatter.Apply(this, format.WithDefault("f"), formatProvider, FormatTokens);

    /// <summary>The format token instructions.</summary>
    private static readonly Dictionary<char, Func<Sex, IFormatProvider, string>> FormatTokens = new()
    {
        { 'i', (svo, provider) => svo.GetResourceString("int_", provider) },
        { 'c', (svo, provider) => svo.GetResourceString("char_", provider) },
        { 'h', (svo, provider) => svo.GetResourceString("honorific_", provider) },
        { 's', (svo, provider) => svo.GetResourceString("symbol_", provider) },
        { 'f', (svo, provider) => svo.GetResourceString("", provider) },
    };

    /// <summary>Gets an XML string representation of the sex.</summary>
    [Pure]
    private string ToXmlString() => SexLabels[m_Value] ?? string.Empty;

    /// <summary>Casts a Sex to a <see cref="byte"/>.</summary>
    public static explicit operator byte(Sex val) => (byte)val.ToInt32();

    /// <summary>Casts a Sex to a <see cref="int"/>.</summary>
    public static explicit operator int(Sex val) => val.ToInt32();

    /// <summary>Casts an <see cref="int"/> to a Sex.</summary>
    public static implicit operator Sex(int val) => Cast.Primitive<int, Sex>(TryCreate, val);

    /// <summary>Casts a Sex to a <see cref="int"/>.</summary>
    public static explicit operator int?(Sex val) => val.ToNullableInt32();

    /// <summary>Casts an <see cref="int"/> to a Sex.</summary>
    public static implicit operator Sex(int? val) => Cast.Primitive<int, Sex>(TryCreate, val);

    /// <summary>Converts the string to a Sex.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing a Sex to convert.
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
    public static bool TryParse(string s, IFormatProvider formatProvider, out Sex result)
    {
        result = Empty;
        var buffer = s.Buffer().Unify();

        if (buffer.IsEmpty()) return true;
        else if (Parsings[AddCulture(formatProvider)].TryGetValue(buffer, out byte val)
            || Parsings[CultureInfo.InvariantCulture].TryGetValue(buffer, out val))
        {
            result = new Sex(val);
            return true;
        }
        else return false;
    }

    /// <summary>Creates a Sex from a int.</summary>
    /// <param name="val">
    /// A decimal describing a Sex.
    /// </param>
    /// <exception cref="FormatException">
    /// val is not a valid Sex.
    /// </exception>
    [Pure]
    public static Sex Create(int? val)
        => TryCreate(val, out Sex result)
        ? result
        : throw new ArgumentOutOfRangeException(nameof(val), QowaivMessages.FormatExceptionSex);

    /// <summary>Creates a Sex from a int.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="val">
    /// A decimal describing a Sex.
    /// </param>
    /// <returns>
    /// A Sex if the creation was successfully, otherwise Sex.Empty.
    /// </returns>
    [Pure]
    public static Sex TryCreate(int? val)
        => TryCreate(val, out Sex result) ? result : default;

    /// <summary>Creates a Sex from a int.
    /// A return value indicates whether the creation succeeded.
    /// </summary>
    /// <param name="val">
    /// A int describing a Sex.
    /// </param>
    /// <param name="result">
    /// The result of the creation.
    /// </param>
    /// <returns>
    /// True if a Sex was created successfully, otherwise false.
    /// </returns>
    public static bool TryCreate(int? val, out Sex result)
    {
        result = default;
        if (!val.HasValue) { return true; }
        else if (FromInt32s.TryGetValue(val.Value, out byte b))
        {
            result = new Sex(b);
            return true;
        }
        else { return false; }
    }

    /// <summary>Returns true if the val represents a valid Sex, otherwise false.</summary>
    [Pure]
    public static bool IsValid(int? val)
        => val.HasValue
        && val != 0
        && FromInt32s.ContainsKey(val.Value);

    private static readonly ResourceManager ResourceManager = new("Qowaiv.SexLabels", typeof(Sex).Assembly);

    /// <summary>Get resource string.</summary>
    /// <param name="prefix">
    /// The prefix of the resource key.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    [Pure]
    private string GetResourceString(string prefix, IFormatProvider formatProvider)
        => ResourceManager.Localized(formatProvider, prefix, SexLabels[m_Value]);

    /// <summary>Gets the valid values.</summary>
    private static readonly Dictionary<int, byte> FromInt32s = new()
    {
        { 0, 1 },
        { 1, 2 },
        { 2, 4 },
        { 9, 18 },
    };

    private static readonly Dictionary<byte, int?> ToNullableInt32s = new()
    {
        { 0, null },
        { 1, 0 },
        { 2, 1 },
        { 4, 2 },
        { 18, 9 },
    };

    /// <summary>Gets the sex labels.</summary>
    /// <remarks>
    /// Used for both serialization and resource lookups.
    /// </remarks>
    private static readonly Dictionary<byte, string> SexLabels = new()
    {
        { 0, null },
        { 1, "NotKnown" },
        { 2, "Male" },
        { 4, "Female" },
        { 18, "NotApplicable" }
    };

    /// <summary>Adds a culture to the parsings.</summary>
    [Impure]
    private static CultureInfo AddCulture(IFormatProvider formatProvider)
    {
        var culture = formatProvider as CultureInfo ?? CultureInfo.CurrentCulture;
        lock (Locker)
        {
            if (Parsings.ContainsKey(culture)) return culture;
            else
            {
                Parsings[culture] = new Dictionary<string, byte>();

                foreach (var sex in All)
                {
                    var longname = sex.ToString("", culture).ToUpper(culture);
                    var shortname = sex.ToString("c", culture).ToUpper(culture);

                    Parsings[culture][longname] = sex.m_Value;
                    Parsings[culture][shortname] = sex.m_Value;
                }
            }
            return culture;
        }
    }

    /// <summary>Represents the parsing keys.</summary>
    private static readonly Dictionary<CultureInfo, Dictionary<string, byte>> Parsings = new()
    {
        {
            CultureInfo.InvariantCulture,
            new()
            {
                { "", 0 },
                { "0", 1 },
                { "1", 2 },
                { "2", 4 },
                { "9", 18 },
                { "?", 1 },
                { "M", 2 },
                { "F", 4 },
                { "X", 18 },
                { "♂", 2 },
                { "♀", 4 },
                { "NOTKNOWN", 1 },
                { "NOT KNOWN", 1 },
                { "UNKNOWN", 1 },
                { "MALE", 2 },
                { "FEMALE", 4 },
                { "NOTAPPLICABLE", 18 },
                { "NOT APPLICABLE", 18 }
            }
        }
    };

    /// <summary>The locker for adding a culture.</summary>
    private static readonly object Locker = new();
}
