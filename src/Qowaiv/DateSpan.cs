#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion;
using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv
{
    /// <summary>Represents a date span.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.Continuous, typeof(ulong))]
    [OpenApiDataType(description: "Date span, specified in years, months and days, for example +1Y+10M+16D.", type: "string", format: "date-span", pattern: @"([+-][0-9]+Y)?([+-][0-9]+M)?([+-][0-9]+D)?")]
    [TypeConverter(typeof(DateSpanTypeConverter))]
    public struct DateSpan : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<DateSpan>, IComparable, IComparable<DateSpan>
    {
        /// <summary>Represents the pattern of a (potential) valid year.</summary>
        internal static readonly Regex Pattern = new Regex(@"^(?<Years>([+-][0-9]{1,4})Y)?(?<Months>([+-][0-9]{1,5})M)?(?<Days>([+-][0-9]{1,9})D)?$", RegexOptions.Compiled|RegexOptions.IgnoreCase);

        /// <summary>The average amount of days per month (taken leap years into account.</summary>
        internal const double DaysPerMonth = 30.421625;

        /// <summary>Represents an empty/not set date span.</summary>
        public static readonly DateSpan Zero;

        /// <summary>Creates a new instance of a <see cref="DateSpan"/>.</summary>
        /// <param name="months">
        /// Number of months.
        /// </param>
        /// <param name="days">
        /// Number of days.
        /// </param>
        public DateSpan(int months, int days)
        {
            m_Value = (uint)days | ((ulong)months << 32);
        }

        /// <summary>Creates a new instance of a <see cref="DateSpan"/>.</summary>
        /// <param name="years">
        /// Number of years.
        /// </param>
        /// <param name="months">
        /// Number of months.
        /// </param>
        /// <param name="days">
        /// Number of days.
        /// </param>
        public DateSpan(int years, int months, int days) 
            : this(years * 12 + months, days) { }

        #region Properties

        /// <summary>The inner value of the date span.</summary>
        private ulong m_Value;

        /// <summary>Gets the total of months.</summary>
        public int TotalMonths => (int)(m_Value >> 32);

        /// <summary>Gets the years component of the date span.</summary>
        public int Years => TotalMonths / 12;

        /// <summary>Gets the months component of the date span.</summary>
        public int Months => TotalMonths % 12;

        /// <summary>Gets the days component of the date span.</summary>
        public int Days => (int)m_Value;

        /// <summary>Gets a (approximate) value to sort the date spans by.</summary>
        internal double TotalDays => Days + TotalMonths * DaysPerMonth;

        #endregion

        #region (XML) (De)serialization

        /// <summary>Initializes a new instance of date span based on the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private DateSpan(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = info.GetUInt64("Value");
        }

        /// <summary>Adds the underlying property of date span to the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            info.AddValue("Value", m_Value);
        }

        /// <summary>Gets the <see href="XmlSchema" /> to (de) XML serialize a date span.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <summary>Reads the date span from an <see href="XmlReader" />.</summary>
        /// <remarks>
        /// Uses the string parse function of date span.
        /// </remarks>
        /// <param name="reader">An XML reader.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Guard.NotNull(reader, nameof(reader));
            var s = reader.ReadElementString();
            var val = Parse(s, CultureInfo.InvariantCulture);
            m_Value = val.m_Value;
        }

        /// <summary>Writes the date span to an <see href="XmlWriter" />.</summary>
        /// <remarks>
        /// Uses the string representation of date span.
        /// </remarks>
        /// <param name="writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            Guard.NotNull(writer, nameof(writer));
            writer.WriteString(ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        #region (JSON) (De)serialization

        /// <summary>Generates a date span from a JSON null object representation.</summary>
        void IJsonSerializable.FromJson() => throw new NotSupportedException(QowaivMessages.JsonSerialization_NullNotSupported);

        /// <summary>Generates a date span from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the date span.
        /// </param>
        void IJsonSerializable.FromJson(string jsonString) => m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;

        /// <summary>Generates a date span from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the date span.
        /// </param>
        void IJsonSerializable.FromJson(long jsonInteger) => m_Value = new DateSpan(0, (int)jsonInteger).m_Value;

        /// <summary>Generates a date span from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the date span.
        /// </param>
        void IJsonSerializable.FromJson(double jsonNumber) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DoubleNotSupported);

        /// <summary>Generates a date span from a JSON date representation.</summary>
        /// <param name="jsonDate">
        /// The JSON Date that represents the date span.
        /// </param>
        void IJsonSerializable.FromJson(DateTime jsonDate) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported);

        /// <summary>Converts a date span into its JSON object representation.</summary>
        object IJsonSerializable.ToJson() => ToString(CultureInfo.InvariantCulture);

        #endregion

        #region IFormattable / ToString

        /// <summary>Returns a <see cref="string" /> that represents the current date span for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => string.Format(CultureInfo.InvariantCulture, "{0:#,###0} Years, {1:#,###0} Months, {2:#,###0} Days", Years, Months, Days);

        /// <summary>Returns a <see cref="string" /> that represents the current date span.</summary>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref="string" /> that represents the current date span.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref="string" /> that represents the current date span.</summary>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider formatProvider) => ToString("", formatProvider);

        /// <summary>Returns a formatted <see cref="string" /> that represents the current date span.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
            {
                return formatted;
            }
            
            if(m_Value == 0)
            {
                return "0D";
            }

            var sb = new StringBuilder(16);

            if(Years != 0)
            {
                sb.AppendFormat(formatProvider, "{0:+0;-0;#}Y", Years);
            }
            if (Months != 0)
            {
                sb.AppendFormat(formatProvider, "{0:+0;-0;#}M", Months);
            }
            if (Days != 0)
            {
                sb.AppendFormat(formatProvider, "{0:+0;-0;#}D", Days);
            }
            return sb.ToString();
        }
        #endregion

        #region IEquatable

        /// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
        /// <param name="obj">An object to compare with.</param>
        public override bool Equals(object obj) => obj is DateSpan && Equals((DateSpan)obj);

        /// <summary>Returns true if this instance and the other <see cref="DateSpan" /> are equal, otherwise false.</summary>
        /// <param name="other">The <see cref="DateSpan" /> to compare with.</param>
        public bool Equals(DateSpan other) => m_Value == other.m_Value;

        /// <summary>Returns the hash code for this date span.</summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => m_Value.GetHashCode();

        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator ==(DateSpan left, DateSpan right) => left.Equals(right);

        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator !=(DateSpan left, DateSpan right) => !(left == right);

        #endregion

        #region IComparable

        /// <summary>Compares this instance with a specified System.Object and indicates whether
        /// this instance precedes, follows, or appears in the same position in the sort
        /// order as the specified System.Object.
        /// </summary>
        /// <param name="obj">
        /// An object that evaluates to a date span.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.Value
        /// Condition Less than zero This instance precedes value. Zero This instance
        /// has the same position in the sort order as value. Greater than zero This
        /// instance follows value.-or- value is null.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// value is not a date span.
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj is DateSpan)
            {
                return CompareTo((DateSpan)obj);
            }
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.ArgumentException_Must, "a date span"), "obj");
        }

        /// <summary>Compares this instance with a specified date span and indicates
        /// whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified date span.
        /// </summary>
        /// <param name="other">
        /// The date span to compare with this instance.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(DateSpan other) => TotalDays.CompareTo(other.TotalDays);


        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(DateSpan l, DateSpan r) => l.CompareTo(r) < 0;

        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator >(DateSpan l, DateSpan r) => l.CompareTo(r) > 0;

        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(DateSpan l, DateSpan r) => l.CompareTo(r) <= 0;

        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(DateSpan l, DateSpan r) => l.CompareTo(r) >= 0;

        #endregion

        #region Factory methods

        /// <summary>Creates a date span from days only.</summary>
        public static DateSpan FromDays(int days) => new DateSpan(0, 0, days);

        public static DateSpan Age(Date reference) => Age(reference, DateSpanSettings.WithoutMonths);
        public static DateSpan Age(Date reference, DateSpanSettings settings) => Subtract(Clock.Today(), reference, settings);

        public static DateSpan Subtract(Date t1, Date t2) => Subtract(t1, t2, DateSpanSettings.Default);
        public static DateSpan Subtract(Date t1, Date t2, DateSpanSettings settings)
        {
            var withYears = (settings & DateSpanSettings.WithoutYears) == default;
            var withMonths = (settings & DateSpanSettings.WithoutMonths) == default;
            var daysOnly = !withYears && !withMonths;

            if (daysOnly)
            {
                var totalDays = (int)(t1 - t2).TotalDays;
                return FromDays(totalDays);
            }

            var noMixedSings = (settings & DateSpanSettings.MixedSigns) == default;
            var daysFirst = (settings & DateSpanSettings.DaysFirst) != default;

            var max = t1;
            var min = t2;

            var negative = t1 < t2;

            if (negative)
            {
                max = t2;
                min = t1;
            }

            var years = max.Year - min.Year;
            var months = withMonths ? max.Month - min.Month: 0;
            var days = max.Day - min.Day;

            if (days < 0 && noMixedSings)
            {
                if(withMonths)
                {
                    months--;
                    var sub = daysFirst ? min : max.AddMonths(-1);
                    days += DateTime.DaysInMonth(sub.Year, sub.Month);
                }
                else
                {
                    years--;
                    var sub = daysFirst ? min : max.AddYears(-1);
                    days += DateTime.IsLeapYear(sub.Year) ? 366 : 365;
                }
            }

            return negative
                ? new DateSpan(-years, -months, -days)
                : new DateSpan(+years, +months, +days);
        }

        /// <summary>Converts the string to a date span.</summary>
        /// <param name="s">
        /// A string containing a date span to convert.
        /// </param>
        /// <returns>
        /// A date span.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static DateSpan Parse(string s) => Parse(s, CultureInfo.CurrentCulture);

        /// <summary>Converts the string to a date span.</summary>
        /// <param name="s">
        /// A string containing a date span to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// A date span.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static DateSpan Parse(string s, IFormatProvider formatProvider)
        {
            if (TryParse(s, formatProvider, out DateSpan val))
            {
                return val;
            }
            throw new FormatException(QowaivMessages.FormatExceptionDateSpan);
        }

        /// <summary>Converts the string to a date span.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a date span to convert.
        /// </param>
        /// <returns>
        /// The date span if the string was converted successfully, otherwise Empty.
        /// </returns>
        public static DateSpan TryParse(string s)
        {
            if (TryParse(s, out DateSpan val))
            {
                return val;
            }
            return Zero;
        }

        /// <summary>Converts the string to a date span.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a date span to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out DateSpan result) => TryParse(s, CultureInfo.CurrentCulture, out result);

        /// <summary>Converts the string to a date span.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a date span to convert.
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
        public static bool TryParse(string s, IFormatProvider formatProvider, out DateSpan result)
        {
            result = default;

            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
            if (s == "0D")
            {
                return true;
            }

            var match = Pattern.Match(s);

            if (match.Success)
            {
                var y = IntFromGroup(match, nameof(Years));
                var m = IntFromGroup(match, nameof(Months));
                var d = IntFromGroup(match, nameof(Days)); 
                result = new DateSpan(y, m, d);

                return true;
            }
            return false;
        }

        private static int IntFromGroup(Match match, string group)
        {
            if (match.Groups[group].Length != 0)
            {
                var str = match.Groups[group].Value;
                return int.Parse(str.Substring(0, str.Length -1));
            }
            return 0;
        }

        #endregion
    }
}
