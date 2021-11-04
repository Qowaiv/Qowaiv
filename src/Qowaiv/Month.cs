using Qowaiv.Conversion;
using Qowaiv.Diagnostics;
using Qowaiv.Formatting;
using Qowaiv.Json;
using Qowaiv.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Qowaiv
{
    /// <summary>Represents a month.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.All, typeof(byte))]
    [OpenApiDataType(description: "Month(-only) notation.", example: "Jun", type: "string", format: "month", nullable: true, @enum: "Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec,?")]
    [TypeConverter(typeof(MonthTypeConverter))]
    public partial struct Month : ISerializable, IXmlSerializable, IFormattable, IEquatable<Month>, IComparable, IComparable<Month>
    {
        /// <summary>Represents an empty/not set month.</summary>
        public static readonly Month Empty;

        /// <summary>Represents an unknown (but set) month.</summary>
        public static readonly Month Unknown = new(byte.MaxValue);

        /// <summary>Represents January (01).</summary>
        public static readonly Month January /*  */ = new(01);
        /// <summary>Represents February (02).</summary>
        public static readonly Month February /* */ = new(02);
        /// <summary>Represents March (03).</summary>
        public static readonly Month March /*    */ = new(03);
        /// <summary>Represents April (04).</summary>
        public static readonly Month April /*    */ = new(04);
        /// <summary>Represents May (05).</summary>
        public static readonly Month May /*      */ = new(05);
        /// <summary>Represents June (06).</summary>
        public static readonly Month June /*     */ = new(06);
        /// <summary>Represents July (07).</summary>
        public static readonly Month July /*     */ = new(07);
        /// <summary>Represents August (08).</summary>
        public static readonly Month August /*   */ = new(08);
        /// <summary>Represents September (09).</summary>
        public static readonly Month September /**/ = new(09);
        /// <summary>Represents October (10).</summary>
        public static readonly Month October /*  */ = new(10);
        /// <summary>Represents November (11).</summary>
        public static readonly Month November /* */ = new(11);
        /// <summary>Represents December (12).</summary>
        public static readonly Month December /* */ = new(12);

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
        public string FullName => GetFullName(null);

        /// <summary>Gets the short name of the month.</summary>
        public string ShortName => GetShortName(null);

        /// <summary>Gets the full name of the month.</summary>
        [Pure]
        public string GetFullName(IFormatProvider formatProvider)
            => IsEmptyOrUnknown()
            ? ToDefaultString()
            : (formatProvider as CultureInfo ?? CultureInfo.CurrentCulture).DateTimeFormat.GetMonthName(m_Value);

        /// <summary>Gets the short name of the month.</summary>
        [Pure]
        public string GetShortName(IFormatProvider formatProvider)
            => IsEmptyOrUnknown()
            ? ToDefaultString()
            : (formatProvider as CultureInfo ?? CultureInfo.CurrentCulture).DateTimeFormat.GetAbbreviatedMonthName(m_Value);

        /// <summary>Returns the number of days for the month.</summary>
        /// <param name="year">
        /// The year to ask the number of days for.
        /// </param>
        /// <remarks>
        /// If the year of month is empty or unknown -1 is returned.
        /// </remarks>
        [Pure]
        public int Days(Year year)
            => year.IsEmptyOrUnknown() || IsEmptyOrUnknown()
            ? -1
            : DateTime.DaysInMonth((int)year, m_Value);

        /// <summary>Deserializes the month from a JSON number.</summary>
        /// <param name="json">
        /// The JSON number to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized month.
        /// </returns>
        [Pure]
        public static Month FromJson(double json) => Create((int)json);

        /// <summary>Deserializes the month from a JSON number.</summary>
        /// <param name="json">
        /// The JSON number to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized month.
        /// </returns>
        [Pure]
        public static Month FromJson(long json) => Create((int)json);

        /// <summary>Serializes the month to a JSON node.</summary>
        /// <returns>
        /// The serialized JSON string.
        /// </returns>
        [Pure]
        public string ToJson() => m_Value == default ? null : ToString("s", CultureInfo.InvariantCulture);

        /// <summary>Returns a <see cref="string"/> that represents the current month for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => this.DebuggerDisplay("{0:f (m)}");

        /// <summary>Returns a formatted <see cref="string"/> that represents the current month.</summary>
        /// <param name="format">
        /// The format that describes the formatting.
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
        [Pure]
        public string ToString(string format, IFormatProvider formatProvider)
            => StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted)
            ? formatted
            : StringFormatter.Apply(this, format.WithDefault("f"), formatProvider, FormatTokens);

        /// <summary>Gets an XML string representation of the month.</summary>
        [Pure]
        private string ToXmlString() => ToString("s", CultureInfo.InvariantCulture);

        [Pure]
        private string ToDefaultString() => IsUnknown() ? "?" : string.Empty;

        /// <summary>The format token instructions.</summary>
        private static readonly Dictionary<char, Func<Month, IFormatProvider, string>> FormatTokens = new()
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

        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(Month l, Month r) => HaveValue(l, r) && l.CompareTo(r) < 0;

        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator >(Month l, Month r) => HaveValue(l, r) && l.CompareTo(r) > 0;

        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(Month l, Month r) => HaveValue(l, r) && l.CompareTo(r) <= 0;

        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(Month l, Month r) => HaveValue(l, r) && l.CompareTo(r) >= 0;

        [Pure]
        private static bool HaveValue(Month l, Month r) => !l.IsEmptyOrUnknown() && !r.IsEmptyOrUnknown();

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
            var buffer = s.Buffer().Unify();
            if (buffer.IsEmpty())
            {
                return true;
            }
            else if (buffer.IsUnknown(formatProvider))
            {
                result = Unknown;
                return true;
            }
            else if (byte.TryParse(s, NumberStyles.None, formatProvider, out var n) && IsValid(n))
            {
                result = new Month(n);
                return true;
            }
            else
            {
                var culture = formatProvider as CultureInfo ?? CultureInfo.InvariantCulture;
                AddCulture(culture);
                var str = buffer.ToString();

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
        [Pure]
        public static Month Create(int? val)
            => TryCreate(val, out Month result)
            ? result
            : throw new ArgumentOutOfRangeException(nameof(val), QowaivMessages.FormatExceptionMonth);

        /// <summary>Creates a month from a Byte.
        /// A return value indicates whether the conversion succeeded.
        /// </summary >
        /// <param name="val" >
        /// A decimal describing a month.
        /// </param >
        /// <returns >
        /// A month if the creation was successfully, otherwise Month.Empty.
        /// </returns >
        [Pure]
        public static Month TryCreate(int? val)
            => TryCreate(val, out Month result)
            ? result
            : default;

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

            if (!val.HasValue) { return true; }
            else if (IsValid(val.Value))
            {
                result = new Month((byte)val.Value);
                return true;
            }
            else { return false; }
        }

        /// <summary>Returns true if the val represents a valid month, otherwise false.</summary>
        [Pure]
        public static bool IsValid(int? val)
            => val.HasValue
            && val >= January.m_Value
            && val <= December.m_Value;

        private static void AddCulture(CultureInfo culture)
        {
            lock (locker)
            {
                if (culture == null || Parsings.ContainsKey(culture)) { return; }

                Parsings[culture] = new Dictionary<string, byte>();

                foreach(var month in All)
                {
                    var m = (byte)month;
                    var full = culture.DateTimeFormat.GetAbbreviatedMonthName(m).Buffer().Unify().ToString();
                    var shrt = culture.DateTimeFormat.GetMonthName(m).Buffer().Unify().ToString();

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
        private static readonly Dictionary<CultureInfo, Dictionary<string, byte>> Parsings = new()
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
        private static readonly object locker = new();
    }
}
