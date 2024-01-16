#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable
namespace Qowaiv;

/// <summary>Represents a Gender.</summary>
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
[Serializable]
[SingleValueObject(SingleValueStaticOptions.All, typeof(byte))]
[OpenApiDataType(description: "Gender as specified by ISO/IEC 5218.", example: "female", type: "string", format: "gender", nullable: true, @enum: "NotKnown,Male,Female,NotApplicable")]
[TypeConverter(typeof(GenderTypeConverter))]
#if NET6_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.GenderJsonConverter))]
#endif
[Obsolete("Will be dropped in version 7. Use Qowaiv.Sex instead.")]
public readonly partial struct Gender : IXmlSerializable, IFormattable, IEquatable<Gender>, IComparable, IComparable<Gender>
{
    /// <summary>Represents an empty/not set Gender.</summary>
    public static readonly Gender Empty;

    /// <summary>Represents a not known/unknown gender.</summary>
    public static readonly Gender Unknown = new(1);

    /// <summary>Represents a male.</summary>
    public static readonly Gender Male = new(2);

    /// <summary>Represents a female.</summary>
    public static readonly Gender Female = new(4);

    /// <summary>Represents a not applicable gender.</summary>
    public static readonly Gender NotApplicable = new(18);

    /// <summary>Contains not known, male, female, not applicable.</summary>
    public static readonly IReadOnlyCollection<Gender> All = new[] { Male, Female, NotApplicable, Unknown, };

    /// <summary>Contains male and female.</summary>
    public static readonly IReadOnlyCollection<Gender> MaleAndFemale = new[] { Male, Female, };

    /// <summary>Contains male, female, not applicable.</summary>
    public static readonly IReadOnlyCollection<Gender> MaleFemaleAndNotApplicable = new[] { Male, Female, NotApplicable, };

    /// <summary>Gets the display name.</summary>
    public string DisplayName => GetDisplayName(null);

    /// <summary>Returns true if the Gender is male or female, otherwise false.</summary>
    [Pure]
    public bool IsMaleOrFemale() => Equals(Male) || Equals(Female);

    /// <summary>Gets the display name for a specified culture.</summary>
    /// <param name="culture">
    /// The culture of the display name.
    /// </param>
    [Pure]
    public string GetDisplayName(CultureInfo? culture) => GetResourceString(string.Empty, culture);

    /// <summary>Converts the Gender to an int.</summary>
    [Pure]
    private int ToInt32() => m_Value >> 1;

    /// <summary>Converts the Gender to an int.</summary>
    [Pure]
    private int? ToNullableInt32() => ToNullableInt32s[m_Value];

    /// <summary>Deserializes the gender from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized gender.
    /// </returns>
    [Pure]
    public static Gender FromJson(double json) => Create((int)json);

    /// <summary>Deserializes the gender from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized gender.
    /// </returns>
    [Pure]
    public static Gender FromJson(long json) => Create((int)json);

    /// <summary>Serializes the gender to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string? ToJson() => GenderLabels[m_Value];

    /// <summary>Returns a <see cref="string"/> that represents the current Gender for debug purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => IsEmpty()
        ? DebugDisplay.Empty
        : GetDisplayName(CultureInfo.InvariantCulture);

    /// <summary>Returns a formatted <see cref="string"/> that represents the current Gender.</summary>
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
    public string ToString(string? format, IFormatProvider? formatProvider)
        => StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted)
        ? formatted
        : StringFormatter.Apply(this, format.WithDefault("f"), formatProvider, FormatTokens);

    /// <summary>The format token instructions.</summary>
    private static readonly Dictionary<char, Func<Gender, IFormatProvider, string>> FormatTokens = new()
    {
        { 'i', (svo, provider) => svo.GetResourceString("int_", provider) },
        { 'c', (svo, provider) => svo.GetResourceString("char_", provider) },
        { 'h', (svo, provider) => svo.GetResourceString("honorific_", provider) },
        { 's', (svo, provider) => svo.GetResourceString("symbol_", provider) },
        { 'f', (svo, provider) => svo.GetResourceString(string.Empty, provider) },
    };

    /// <summary>Gets an XML string representation of the gender.</summary>
    [Pure]
    private string ToXmlString() => GenderLabels[m_Value] ?? string.Empty;

    /// <summary>Casts a Gender to a <see cref="byte"/>.</summary>
    public static explicit operator byte(Gender val) => (byte)val.ToInt32();

    /// <summary>Casts a Gender to a <see cref="int"/>.</summary>
    public static explicit operator int(Gender val) => val.ToInt32();

    /// <summary>Casts an <see cref="int"/> to a Gender.</summary>
    public static implicit operator Gender(int val) => Cast.Primitive<int, Gender>(TryCreate, val);

    /// <summary>Casts a Gender to a <see cref="int"/>.</summary>
    public static explicit operator int?(Gender val) => val.ToNullableInt32();

    /// <summary>Casts an <see cref="int"/> to a Gender.</summary>
    public static implicit operator Gender(int? val) => Cast.Primitive<int, Gender>(TryCreate, val);

    /// <summary>Converts the string to a Gender.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing a Gender to convert.
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
    public static bool TryParse(string? s, IFormatProvider? formatProvider, out Gender result)
    {
        result = Empty;
        var str = s.Unify();

        if (str.IsEmpty())
        {
            return true;
        }
        else
        {
            var c = formatProvider as CultureInfo ?? CultureInfo.CurrentCulture;
            AddCulture(c);
            if (Parsings[c].TryGetValue(str, out byte val) ||
                Parsings[CultureInfo.InvariantCulture].TryGetValue(str, out val))
            {
                result = new Gender(val);
                return true;
            }
            return false;
        }
    }

    /// <summary>Creates a Gender from a int.</summary>
    /// <param name="val">
    /// A decimal describing a Gender.
    /// </param>
    /// <exception cref="FormatException">
    /// val is not a valid Gender.
    /// </exception>
    [Pure]
    public static Gender Create(int? val)
        => TryCreate(val, out Gender result)
        ? result
        : throw new ArgumentOutOfRangeException(nameof(val), QowaivMessages.FormatExceptionGender);

    /// <summary>Creates a Gender from a int.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="val">
    /// A decimal describing a Gender.
    /// </param>
    /// <returns>
    /// A Gender if the creation was successfully, otherwise Gender.Empty.
    /// </returns>
    [Pure]
    public static Gender TryCreate(int? val)
        => TryCreate(val, out Gender result) ? result : default;

    /// <summary>Creates a Gender from a int.
    /// A return value indicates whether the creation succeeded.
    /// </summary>
    /// <param name="val">
    /// A int describing a Gender.
    /// </param>
    /// <param name="result">
    /// The result of the creation.
    /// </param>
    /// <returns>
    /// True if a Gender was created successfully, otherwise false.
    /// </returns>
    public static bool TryCreate(int? val, out Gender result)
    {
        result = default;
        if (!val.HasValue) { return true; }
        else if (FromInt32s.TryGetValue(val.Value, out byte b))
        {
            result = new Gender(b);
            return true;
        }
        else { return false; }
    }

    /// <summary>Returns true if the val represents a valid Gender, otherwise false.</summary>
    [Pure]
    public static bool IsValid(int? val)
        => val.HasValue
        && val != 0
        && FromInt32s.ContainsKey(val.Value);

    private static readonly ResourceManager ResourceManager = new("Qowaiv.SexLabels", typeof(Gender).Assembly);

    /// <summary>Get resource string.</summary>
    /// <param name="prefix">
    /// The prefix of the resource key.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    [Pure]
    private string GetResourceString(string prefix, IFormatProvider? formatProvider)
        => ResourceManager.Localized(formatProvider, prefix, GenderLabels[m_Value]);

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

    /// <summary>Gets the gender labels.</summary>
    /// <remarks>
    /// Used for both serialization and resource lookups.
    /// </remarks>
    private static readonly Dictionary<byte, string?> GenderLabels = new()
    {
        [00] = null,
        [01] = "NotKnown",
        [02] = "Male",
        [04] = "Female",
        [18] = "NotApplicable",
    };

    /// <summary>Adds a culture to the parsings.</summary>
    /// <param name="culture">
    /// The culture to add.
    /// </param>
    private static void AddCulture(CultureInfo culture)
    {
        lock (Locker)
        {
            if (Parsings.ContainsKey(culture)) return;

            Parsings[culture] = [];

            foreach (var gender in All)
            {
                var longname = gender.ToString(string.Empty, culture).ToUpper(culture);
                var shortname = gender.ToString("c", culture).ToUpper(culture);

                Parsings[culture][longname] = gender.m_Value;
                Parsings[culture][shortname] = gender.m_Value;
            }
        }
    }

    /// <summary>Represents the parsing keys.</summary>
    private static readonly Dictionary<CultureInfo, Dictionary<string, byte>> Parsings = new()
    {
        [CultureInfo.InvariantCulture] = new()
        {
            [string.Empty] = 0,
            ["0"] = 1,
            ["1"] = 2,
            ["2"] = 4,
            ["9"] = 18,
            ["?"] = 1,
            ["M"] = 2,
            ["F"] = 4,
            ["X"] = 18,
            ["♂"] = 2,
            ["♀"] = 4,
            ["NOTKNOWN"] = 1,
            ["NOT KNOWN"] = 1,
            ["UNKNOWN"] = 1,
            ["MALE"] = 2,
            ["FEMALE"] = 4,
            ["NOTAPPLICABLE"] = 18,
            ["NOT APPLICABLE"] = 18,
        },
    };

    /// <summary>The locker for adding a culture.</summary>
    private static readonly object Locker = new();
}
