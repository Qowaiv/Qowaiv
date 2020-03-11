#pragma warning disable S2328 // "GetHashCode" should not reference mutable fields
// To support XML serialization, the underlying value has to be mutable but the struct is inmutable.

using Qowaiv.Conversion;
using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Qowaiv
{
    /// <summary>Represents a month.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.All, typeof(byte))]
    [OpenApiDataType(description: "Month(-only) notation.", type: "string", format: "month", nullable: true, @enum: "Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec,?")]
    [TypeConverter(typeof(MonthTypeConverter))]
    public partial struct Month : ISerializable, IXmlSerializable, IFormattable, IEquatable<Month>, IComparable, IComparable<Month>
    {
        /// <summary>Represents the pattern of a (potential) valid month.</summary>
        public static readonly Regex Pattern = new Regex(@"^(0?[1-9]|10|11|12)$", RegexOptions.Compiled);

        /// <summary>Represents an empty/not set month.</summary>
        public static readonly Month Empty;

        /// <summary>Represents an unknown (but set) month.</summary>
        public static readonly Month Unknown = new Month(byte.MaxValue);

        /// <summary>Represents January (01).</summary>
        public static readonly Month January /*  */ = new Month(01);
        /// <summary>Represents February (02).</summary>
        public static readonly Month February /* */ = new Month(02);
        /// <summary>Represents March (03).</summary>
        public static readonly Month March /*    */ = new Month(03);
        /// <summary>Represents April (04).</summary>
        public static readonly Month April /*    */ = new Month(04);
        /// <summary>Represents May (05).</summary>
        public static readonly Month May /*      */ = new Month(05);
        /// <summary>Represents June (06).</summary>
        public static readonly Month June /*     */ = new Month(06);
        /// <summary>Represents July (07).</summary>
        public static readonly Month July /*     */ = new Month(07);
        /// <summary>Represents August (08).</summary>
        public static readonly Month August /*   */ = new Month(08);
        /// <summary>Represents September (09).</summary>
        public static readonly Month September /**/ = new Month(09);
        /// <summary>Represents October (10).</summary>
        public static readonly Month October /*  */ = new Month(10);
        /// <summary>Represents November (11).</summary>
        public static readonly Month November /* */ = new Month(11);
        /// <summary>Represents December (12).</summary>
        public static readonly Month December /* */ = new Month(12);

        /// <summary>Represents all months (January till December).</summary>
        public static readonly IReadOnlyList<Month> All = new []
        {
            January,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December,
        };

        /// <summary>Gets the full name of the month.</summary>
        public string FullName => GetFullName(CultureInfo.CurrentCulture);

        /// <summary>Gets the short name of the month.</summary>
        public string ShortName => GetShortName(CultureInfo.CurrentCulture);

        /// <summary>Gets the full name of the month.</summary>
        public string GetFullName(IFormatProvider formatProvider)
        {
            return IsEmptyOrUnknown()
                ? ToDefaultString()
                : (formatProvider as CultureInfo ?? CultureInfo.InvariantCulture).DateTimeFormat.GetMonthName(m_Value);
        }

        /// <summary>Gets the short name of the month.</summary>
        public string GetShortName(IFormatProvider formatProvider)
        {
            return IsEmptyOrUnknown()
                ? ToDefaultString()
                : (formatProvider as CultureInfo ?? CultureInfo.InvariantCulture).DateTimeFormat.GetAbbreviatedMonthName(m_Value);
        }

        /// <summary>Returns the number of days for the month.</summary>
        /// <param name="year">
        /// The year to ask the number of days for.
        /// </param>
        /// <remarks>
        /// If the year of month is empty or unknown -1 is returned.
        /// </remarks>
        public int Days(Year year)
        {
            return
                year.IsEmptyOrUnknown() || IsEmptyOrUnknown()
                ? -1
                : DateTime.DaysInMonth((int)year, m_Value);
        }

        /// <summary>Deserializes the month from a JSON number.</summary>
        /// <param name="json">
        /// The JSON number to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized month.
        /// </returns>
        public static Month FromJson(double json) => Create((int)json);

        /// <summary>Deserializes the month from a JSON number.</summary>
        /// <param name="json">
        /// The JSON number to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized month.
        /// </returns>
        public static Month FromJson(long json) => Create((int)json);

        /// <summary>Serializes the month to a JSON node.</summary>
        /// <returns>
        /// The serialized JSON string.
        /// </returns>
        public string ToJson() => m_Value == default ? null : ToString("s", CultureInfo.InvariantCulture);

        /// <summary>Returns a <see cref="string"/> that represents the current month for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get
            {
                if (IsEmpty()) { return "Month: (empty)"; }
                if (IsUnknown()) { return "Month: (unknown)"; }
                return string.Format(CultureInfo.InvariantCulture, "Month: {0:f} ({0:m})", this);
            }
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current month.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        /// <remarks>
        /// The formats:
        /// 
        /// f: as full name.
        /// s: as short name.
        /// M: as number with leading zero.
        /// m: as number without leading zero.
        /// </remarks>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
            {
                return formatted;
            }
            // Apply the format.
            return StringFormatter.Apply
            (
                this, string.IsNullOrEmpty(format) ? "f" : format,
                formatProvider ?? CultureInfo.CurrentCulture, FormatTokens
            );
        }


        /// <summary>Gets an XML string representation of the month.</summary>
        private string ToXmlString() => ToString("s", CultureInfo.InvariantCulture);

        private string ToDefaultString() => IsUnknown() ? "?" : string.Empty;

        /// <summary>Represents the underlying value as <see cref="IConvertible"/>.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IConvertible Convertable => m_Value;

        /// <inheritdoc/>
        TypeCode IConvertible.GetTypeCode() => TypeCode.Byte;

        /// <summary>The format token instructions.</summary>
        private static readonly Dictionary<char, Func<Month, IFormatProvider, string>> FormatTokens = new Dictionary<char, Func<Month, IFormatProvider, string>>
        {
            { 'f', (svo, provider) => svo.GetFullName(provider) },
            { 's', (svo, provider) => svo.GetShortName(provider) },
            { 'M', (svo, provider) => svo.IsEmptyOrUnknown() ? svo.ToDefaultString() : svo.m_Value.ToString("0", provider) },
            { 'm', (svo, provider) => svo.IsEmptyOrUnknown() ? svo.ToDefaultString() : svo.m_Value.ToString("00", provider) },
        };

        /// <summary>Casts a month to a <see cref="string"/>.</summary>
        public static explicit operator string(Month val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a month.</summary>
        public static explicit operator Month(string str) => Cast.String<Month>(TryParse, str);


        /// <summary>Casts a month to a System.Int32.</summary>
        public static explicit operator int(Month val) => val.m_Value;
        /// <summary>Casts an System.Int32 to a month.</summary>
        public static implicit operator Month(int val) => Cast.Primitive<int, Month>(TryCreate, val);

        /// <summary>Converts the string to a month.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a month to convert.
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
        public static bool TryParse(string s, IFormatProvider formatProvider, out Month result)
        {
            result = default;
            if (string.IsNullOrEmpty(s))
            {
                return true;
            }
            var culture = formatProvider as CultureInfo ?? CultureInfo.InvariantCulture;
            if (Qowaiv.Unknown.IsUnknown(s, culture))
            {
                result = Unknown;
                return true;
            }
            if (Pattern.IsMatch(s))
            {
                result = new Month(byte.Parse(s, formatProvider));
                return true;
            }
            else
            {
                AddCulture(culture);

                var str = Parsing.ToUnified(s);
                if (Parsings[culture].TryGetValue(str, out byte m) ||
                    Parsings[CultureInfo.InvariantCulture].TryGetValue(str, out m))
                {
                    result = new Month(m);
                    return true;
                }
            }
            return false;
        }

        /// <summary>Creates a month from a Byte.</summary>
        /// <param name="val" >
        /// A decimal describing a month.
        /// </param >
        /// <exception cref="FormatException">
        /// val is not a valid month.
        /// </exception>
        public static Month Create(int? val)
        {
            if (TryCreate(val, out Month result))
            {
                return result;
            }
            throw new ArgumentOutOfRangeException("val", QowaivMessages.FormatExceptionMonth);
        }

        /// <summary>Creates a month from a Byte.
        /// A return value indicates whether the conversion succeeded.
        /// </summary >
        /// <param name="val" >
        /// A decimal describing a month.
        /// </param >
        /// <returns >
        /// A month if the creation was successfully, otherwise Month.Empty.
        /// </returns >
        public static Month TryCreate(int? val)
        {
            if (TryCreate(val, out Month result))
            {
                return result;
            }
            return Empty;
        }

        /// <summary>Creates a month from a Byte.
        /// A return value indicates whether the creation succeeded.
        /// </summary >
        /// <param name="val" >
        /// A Byte describing a month.
        /// </param >
        /// <param name="result" >
        /// The result of the creation.
        /// </param >
        /// <returns >
        /// True if a month was created successfully, otherwise false.
        /// </returns >
        public static bool TryCreate(int? val, out Month result)
        {
            result = Empty;

            if (!val.HasValue)
            {
                return true;
            }
            if (IsValid(val.Value))
            {
                result = new Month((byte)val.Value);
                return true;
            }
            return false;
        }

        /// <summary>Returns true if the val represents a valid month, otherwise false.</summary>
        public static bool IsValid(int? val)
        {
            return val.HasValue
                && val.Value >= January.m_Value
                && val.Value <= December.m_Value;
        }

        #region Lookup

        private static void AddCulture(CultureInfo culture)
        {
            lock (locker)
            {
                if (culture == null || Parsings.ContainsKey(culture)) { return; }

                Parsings[culture] = new Dictionary<string, byte>();

                for (byte m = 1; m <= 12; m++)
                {
                    var full = Parsing.ToUnified(culture.DateTimeFormat.GetAbbreviatedMonthName(m));
                    var shrt = Parsing.ToUnified(culture.DateTimeFormat.GetMonthName(m));

                    if (!Parsings[CultureInfo.InvariantCulture].ContainsKey(full))
                    {
                        Parsings[culture][full] = m;
                    }
                    if (!Parsings[CultureInfo.InvariantCulture].ContainsKey(shrt))
                    {
                        Parsings[culture][shrt] = m;
                    }
                }
            }
        }

        /// <summary>Represents the parsing keys.</summary>
        private static readonly Dictionary<CultureInfo, Dictionary<string, byte>> Parsings = new Dictionary<CultureInfo, Dictionary<string, byte>>
        {
            {
                CultureInfo.InvariantCulture, new Dictionary<string, byte>
                {
                    {"JANUARY", 1},
                    {"FEBRUARY", 2},
                    {"MARCH", 3},
                    {"APRIL", 4},
                    {"MAY", 5},
                    {"JUNE", 6},
                    {"JULY", 7},
                    {"AUGUST", 8},
                    {"SEPTEMBER", 9},
                    {"OCTOBER", 10},
                    {"NOVEMBER", 11},
                    {"DECEMBER", 12},
                    {"JAN", 1},
                    {"FEB", 2},
                    {"MAR", 3},
                    {"APR", 4},
                    {"JUN", 6},
                    {"JUL", 7},
                    {"AUG", 8},
                    {"SEP", 9},
                    {"OCT", 10},
                    {"NOV", 11},
                    {"DEC", 12},
                }
            }
        };

        /// <summary>The locker for adding a culture.</summary>
        private static readonly object locker = new object();

        #endregion
    }
}
