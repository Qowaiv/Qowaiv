#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable
using Qowaiv.Globalization;

namespace Qowaiv;

/// <summary>Represents a postal code.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.All, typeof(string))]
[OpenApiDataType(description: "Postal code notation.", example: "2624DP", type: "string", format: "postal-code", nullable: true)]
[TypeConverter(typeof(PostalCodeTypeConverter))]
#if NET6_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.PostalCodeJsonConverter))]
#endif
public readonly partial struct PostalCode : IXmlSerializable, IFormattable, IEquatable<PostalCode>, IComparable, IComparable<PostalCode>
{
    /// <summary>Represents an unknown (but set) postal code.</summary>
    public static PostalCode Unknown => new("ZZZZZZZZZ");

    /// <summary>Gets the number of characters of postal code.</summary>
    public int Length => m_Value is null || IsUnknown() ? 0 : m_Value.Length;

    /// <summary>Returns true if the postal code is valid for the specified country, otherwise false.</summary>
    /// <param name="country">
    /// The country to validate for.
    /// </param>
    /// <remarks>
    /// Returns false if the country does not have postal codes, unless the postal code is empty.
    /// </remarks>
    [Pure]
    public bool IsValid(Country country) => IsValid(m_Value, country);

    /// <summary>Returns a collection countries where the postal code is valid for.</summary>
    [Pure]
    public IEnumerable<Country> IsValidFor()
    {
        var postalcode = m_Value;
        return Country.All.Where(country => IsValid(postalcode, country));
    }

    /// <summary>Serializes the postal code to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string? ToJson() => m_Value;

    /// <summary>Returns a <see cref="string" /> that represents the current postal code for debug purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0}");

    /// <summary>Returns a formatted <see cref="string" /> that represents the current postal code.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
        => StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted)
        ? formatted
        : ToString(Country.TryParse(format) ?? default);

    /// <summary>Returns a formatted <see cref="string" /> that represents the current postal code.</summary>
    /// <param name="country">
    /// The country to format for.
    /// </param>
    /// <remarks>
    /// If the postal code is not valid for the country,
    /// or the country does not have special formattings,
    /// the unformatted value is returned.
    /// </remarks>
    [Pure]
    public string ToString(Country country)
    {
        // send a question mark in case of Unknown.
        var normalized = Unknown.m_Value == m_Value ? "?" : m_Value ?? string.Empty;
        return PostalCodeCountryInfo.GetInstance(country).Format(normalized);
    }

    /// <summary>Gets an XML string representation of the postal code.</summary>
    [Pure]
    private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Converts the string to a postal code.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing a postal code to convert.
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
    public static bool TryParse(string? s, IFormatProvider? provider, out PostalCode result)
    {
        result = default;
        var str = s.Unify();
        if (str.IsEmpty())
        {
            return true;
        }
        else if (str.IsUnknown(provider))
        {
            result = Unknown;
            return true;
        }
        else if (str.Length >= 2 && str.Length <= 10)
        {
            result = new PostalCode(str);
            return true;
        }
        else return false;
    }

    /// <summary>Returns true if the postal code is valid for the specified country, otherwise false.</summary>
    /// <param name="postalcode">
    /// The postal code to test.
    /// </param>
    /// <param name="country">
    /// The country to valid for.
    /// </param>
    /// <remarks>
    /// Returns false if the country does not have postal codes, unless the postal code is empty.
    /// </remarks>
    [Pure]
    public static bool IsValid(string? postalcode, Country country)
        => PostalCodeCountryInfo.GetInstance(country).IsValid(postalcode);
}
