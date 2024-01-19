namespace Qowaiv.Sustainability;

/// <summary>Represents a EU energy label.</summary>
/// <remarks>
/// EU Directive 2010/30/EU established the energy consumption labeling scheme.
/// The directive was implemented by several other directives thus most white
/// goods, light bulb packaging and cars must have an EU Energy Label clearly
/// displayed when offered for sale or rent.
///
/// The energy efficiency of the appliance is rated in terms of a set of energy
/// efficiency classes from A to G on the label, A being the most energy
/// efficient, G the least efficient. The labels also give other useful
/// information to the customer as they choose between various models.
///
/// In an attempt to keep up with advances in energy efficiency, A+, A++, A+++,
/// and A++++ grades were later introduced for various product.
/// </remarks>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.All, typeof(int))]
[OpenApiDataType(description: "EU energy label", type: "string", format: "energy-label", example: "A++", pattern: @"[A-H]|A\+{1,4}", nullable: true)]
[TypeConverter(typeof(Conversion.Sustainability.EnergyLabelTypeConverter))]
#if NET6_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.Sustainability.EnergyLabelJsonConverter))]
#endif
public readonly partial struct EnergyLabel : IXmlSerializable, IEquatable<EnergyLabel>, IComparable, IComparable<EnergyLabel>
#if NET6_0_OR_GREATER
    , ISpanFormattable
#else
    , IFormattable
#endif
{
    private const int MaxPlusses = 4;

    /// <summary>Represents EU energy label A/A+/A++/...</summary>
    /// <param name="plusses">
    /// The total of '+'s [0, 4].
    /// </param>
    [Pure]
    public static EnergyLabel A(int plusses = 0) => plusses < 0 || plusses > MaxPlusses
        ? throw new ArgumentOutOfRangeException(nameof(plusses), QowaivMessages.FormatExceptionEnergyLabel)
        : new(7 + plusses);

    /// <summary>Represents EU energy label B.</summary>
    public static readonly EnergyLabel B = new(6);

    /// <summary>Represents EU energy label C.</summary>
    public static readonly EnergyLabel C = new(5);

    /// <summary>Represents EU energy label D.</summary>
    public static readonly EnergyLabel D = new(4);

    /// <summary>Represents EU energy label E.</summary>
    public static readonly EnergyLabel E = new(3);

    /// <summary>Represents EU energy label F.</summary>
    public static readonly EnergyLabel F = new(2);

    /// <summary>Represents EU energy label G.</summary>
    public static readonly EnergyLabel G = new(1);

    /// <summary>Represents an unknown (but set) EU energy label.</summary>
    public static readonly EnergyLabel Unknown = new(int.MinValue);

    /// <summary>Returns a <see cref="string" /> that represents the EU energy label for DEBUG purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0:U}");

    /// <summary>Returns a formatted <see cref="string" /> that represents the EU energy label.</summary>
    /// <param name="format">
    /// The format that this describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    /// <remarks>
    /// The formats:
    ///
    /// l: lowercase.
    /// U: uppercase.
    /// </remarks>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
        {
            return formatted;
        }
        else if (IsEmpty()) return string.Empty;
        else if (IsUnknown()) return "?";
        else
        {
            var str = new char[StringLength()];
            str[0] = Char(format == "l");

            for (var i = 1; i < str.Length; i++)
            {
                str[i] = '+';
            }

            return new(str);
        }
    }

#if NET6_0_OR_GREATER
    /// <inheritdoc />
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        if (provider.TryGetCustomFormatter() is null)
        {
            if (IsEmpty()) return destination.TryWrite(string.Empty, out charsWritten);
            else if (IsUnknown()) return destination.TryWrite('?', out charsWritten);
            else if (StringLength() is { } length && destination.Length >= length)
            {
                destination[0] = Char(format.Length == 1 && format[0] == 'l');

                for (var i = 1; i < length; i++)
                {
                    destination[i] = '+';
                }
                charsWritten = length;
                return true;
            }
        }
        charsWritten = 0;
        return false;
    }
#endif

    [Pure]
    private int StringLength() => m_Value > B.m_Value ? m_Value - B.m_Value : 1;

    [Pure]
    private char Char(bool toLower)
    {
        var ch = (char)('H' - m_Value);
        ch = ch <= 'A' ? 'A' : ch;
        return toLower ? char.ToLowerInvariant(ch) : ch;
    }

    /// <summary>Gets an XML string representation of the EU energy label.</summary>
    [Pure]
    private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Serializes the EU energy label to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string? ToJson() => m_Value == default ? null : ToString(CultureInfo.InvariantCulture);

    /// <summary>Casts the EU energy label to a <see cref="string"/>.</summary>
    public static explicit operator string(EnergyLabel val) => val.ToString(CultureInfo.CurrentCulture);

    /// <summary>Casts a <see cref="string"/> to a EU energy label.</summary>
    public static explicit operator EnergyLabel(string str) => Parse(str, CultureInfo.CurrentCulture);

    /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
    public static bool operator <(EnergyLabel l, EnergyLabel r) => AreKnown(l, r) && l.CompareTo(r) < 0;

    /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
    public static bool operator >(EnergyLabel l, EnergyLabel r) => AreKnown(l, r) && l.CompareTo(r) > 0;

    /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
    public static bool operator <=(EnergyLabel l, EnergyLabel r) => AreKnown(l, r) && l.CompareTo(r) <= 0;

    /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
    public static bool operator >=(EnergyLabel l, EnergyLabel r) => AreKnown(l, r) && l.CompareTo(r) >= 0;

    [Pure]
    private static bool AreKnown(EnergyLabel l, EnergyLabel r) => !l.IsUnknown() && !r.IsUnknown();

    /// <summary>Converts the <see cref="string"/> to <see cref="EnergyLabel"/>.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the EU energy label to convert.
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
    public static bool TryParse(string? s, IFormatProvider? provider, out EnergyLabel result)
    {
        result = default;
        var str = s.Unify();

        if (string.IsNullOrEmpty(str))
        {
            return true;
        }
        else if (Qowaiv.Unknown.IsUnknown(str, provider as CultureInfo))
        {
            result = Unknown;
            return true;
        }
        else if (str[0] >= 'A' && str[0] <= 'G')
        {
            var value = 'H' - str[0];

            if (str.Length == 1 || IsAPlus(str))
            {
                result = new(value + str.Count(ch => ch == '+'));
                return true;
            }
        }
        return false;

        static bool IsAPlus(string str)
            => str.Length <= MaxPlusses + 1
            && str[0] == 'A'
            && str[1..].All(ch => ch == '+');
    }
}
