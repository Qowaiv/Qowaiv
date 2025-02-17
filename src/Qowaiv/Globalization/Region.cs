#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable
namespace Qowaiv.Globalization;

/// <summary>Represents a region.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.All, typeof(string))]
[OpenApiDataType(description: "region", type: "Region", format: "Region", example: "ABC")]
[TypeConverter(typeof(Conversion.Globalization.RegionTypeConverter))]
#if NET6_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.RegionJsonConverter))]
#endif
public readonly partial struct Region : IXmlSerializable, IFormattable, IEquatable<Region>, IComparable, IComparable<Region>
{
    /// <summary>Represents an unknown (but set) region.</summary>
    public static readonly Region Unknown = new("ZZ-ZZZ");

    /// <summary>Gets the number of characters of region.</summary>
    public int Length => m_Value == null ? 0 : m_Value.Length;

    /// <summary>Returns a <see cref="string" /> that represents the region for DEBUG purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0:F}");

    /// <summary>Returns a formatted <see cref="string" /> that represents the region.</summary>
    /// <param name="format">
    /// The format that this describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
        {
            return formatted;
        }
        throw new NotImplementedException();
    }

    /// <summary>Gets an XML string representation of the region.</summary>
    [Pure]
    private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Serializes the region to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string? ToJson() => m_Value == default ? null : ToString(CultureInfo.InvariantCulture);

    /// <summary>Casts the region to a <see cref="string"/>.</summary>
    public static explicit operator string(Region val) => val.ToString(CultureInfo.CurrentCulture);

    /// <summary>Casts a <see cref="string"/> to a region.</summary>
    public static explicit operator Region(string str) => Parse(str, CultureInfo.CurrentCulture);

    /// <summary>Converts the <see cref="string"/> to <see cref="Region"/>.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the region to convert.
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
    public static bool TryParse(string? s, IFormatProvider? provider, out Region result)
    {
        result = default;
        if (string.IsNullOrEmpty(s))
        {
            return true;
        }
        if (Qowaiv.Unknown.IsUnknown(s, provider as CultureInfo))
        {
            result = Unknown;
            return true;
        }
        throw new NotImplementedException();
    }

    public string Name => m_Value ?? string.Empty;

    public string DisplayName => GetString(nameof(DisplayName))!;

    public RegionKind Kind => GetString(nameof(Kind)) is { Length: > 0 } value
        ? (RegionKind)Enum.Parse(typeof(RegionKind), value)
        : RegionKind.Unknown;

    public Country Country => Country.Parse(m_Value[0..2], CultureInfo.InvariantCulture);


    public static IReadOnlyCollection<Region> All => all ??= InitAll();

    public static void RegisterAdditional(ResourceManager resources)
    {
        Guard.NotNull(resources);
        if (additional is { }) throw new InvalidOperationException("Allready registered");
        additional = resources;
    }

    private static IReadOnlyCollection<Region>? InitAll()
    {
        var list = new List<Region>();
        var entries = ResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, false);

        foreach (DictionaryEntry entry in entries)
        {
            if (entry.Key is string key && key.EndsWith("_DisplayName"))
            {
                list.Add(new(key[..^12]));
            }
        }
        if (additional is { })
        {
            var extras = additional.GetResourceSet(CultureInfo.InvariantCulture, true, true);
            foreach (DictionaryEntry extra in extras)
            {
                if (extra.Key is string key && key.EndsWith("_DisplayName"))
                {
                    list.Add(new(key[..^12]));
                }
            }
        }

        return list.ToArray();
    }

    private static IReadOnlyCollection<Region>? all;

    private static ResourceManager ResourceManager
    {
        get
        {
            rm ??= new("Qowaiv.Globalization.RegionLabels", typeof(Region).Assembly);
            return rm;
        }
    }

    private static ResourceManager? rm;

    private static ResourceManager? additional;

    [Pure]
    private string? GetString(string name)
        => additional?.GetString($"{m_Value}_{name}")
        ?? ResourceManager.GetString($"{m_Value}_{name}");
}
