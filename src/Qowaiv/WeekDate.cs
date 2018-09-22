#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion;
using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv
{
    /// <summary>The ISO week date system is a leap week calendar system that is part of
    /// the ISO 8601 date and time standard. The system is used (mainly) in
    /// government and business for fiscal years, as well as in timekeeping.
    /// The system specifies a week year atop the Gregorian calendar by
    /// defining a notation for ordinal weeks of the year.
    /// </summary>
    /// <remarks>
    /// The Gregorian leap cycle, which has 97 leap days spread across 400 years,
    /// contains a whole number of weeks (20871). In every cycle there are 71
    /// years with an additional 53rd week. An average year is exactly 52.1775
    /// weeks long, months average at 4.348125 weeks.
    /// 
    /// An ISO week-numbering year (also called ISO year informally) has 52 or
    /// 53 full weeks. That is 364 or 371 days instead of the usual 365 or 366
    /// days. The extra week is referred to here as a leap week, although
    /// ISO 8601 does not use this term. Weeks start with Monday. The first week
    /// of a year is the week that contains the first Thursday (and, hence,
    /// 4 January) of the year. ISO week year numbering therefore slightly
    /// deviates from the Gregorian for some days close to 1 January.
    /// 
    /// A date is specified by the ISO week-numbering year in the format YYYY,
    /// a week number in the format ww prefixed by the letter 'W', and the
    /// weekday number, a digit d from 1 through 7, beginning with Monday and
    /// ending with Sunday. For example, the Gregorian date 31 December 2006
    /// corresponds to the Sunday of the 52nd week of 2006, and is written
    /// 2006-W52-7 (extended form) or 2006W527 (compact form).
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.All ^ SingleValueStaticOptions.HasEmptyValue ^ SingleValueStaticOptions.HasUnknownValue, typeof(Date))]
    [TypeConverter(typeof(WeekDateTypeConverter))]
    public struct WeekDate : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<WeekDate>, IComparable, IComparable<WeekDate>
    {
        /// <summary>Represents the pattern of a (potential) valid week date.</summary>
        public static readonly Regex Pattern = new Regex(@"^(?<year>[0-9]{1,4})[ -]?W?(?<week>(0?[1-9]|[1-4][0-9]|5[0-3]))[ -]?(?<day>[1-7])$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>Represents the minimum value of the week date.</summary>
        public static readonly WeekDate MinValue = new WeekDate { m_Value = Date.MinValue };

        /// <summary>Represents the maximum value of the week date.</summary>
        public static readonly WeekDate MaxValue = new WeekDate { m_Value = Date.MaxValue };

        /// <summary>Creates a date based on Week Year, week number, and day of the week.</summary>
        public WeekDate(int year, int week, int day)
        {
            if (year < 1 || year > 9999)
            {
                throw new ArgumentOutOfRangeException("year", "Year should be in range [1,9999].");
            }
            if (week < 1 || week > 53)
            {
                throw new ArgumentOutOfRangeException("week", "Week should be in range [1,53].");
            }
            if (day < 1 || day > 7)
            {
                throw new ArgumentOutOfRangeException("day", "Day should be in range [1,7].");
            }

            if (!TryCreate(year, week, day, out Date dt))
            {
                throw new ArgumentOutOfRangeException("Year, Week, and Day parameters describe an un-representable Date.", (Exception)null);
            }
            m_Value = dt;
        }

        #region Properties

        /// <summary>The inner value of the week date.</summary>
        private Date m_Value;

        /// <summary>Gets the year component represented by this instance.</summary>
        /// <remarks>
        /// The week date year component can differ from the year component of a date.
        /// </remarks>
        public int Year { get { return GetDatePart(DatePartYear); } }

        /// <summary>Gets the week component of the week date represented by this instance.</summary>
        public int Week { get { return GetDatePart(DatePartWeek); } }

        /// <summary>Gets the day component represented by this instance.</summary>
        public Int32 Day
        {
            get { return DayOfWeek == DayOfWeek.Sunday ? 7 : (Int32)DayOfWeek; }
        }

        /// <summary>Gets the day of the week represented by this instance.</summary>
        public DayOfWeek DayOfWeek { get { return m_Value.DayOfWeek; } }

        /// <summary>Gets the day of the year represented by this instance.</summary>
        public int DayOfYear { get { return GetDatePart(DatePartDayOfYear); } }

        /// <summary>Gets the date time component of this instance.</summary>
        public Date Date { get { return m_Value; } }

        #endregion

        #region Methods

        /// <summary>Gets the year of week part of the week date.</summary>
        private Int32 GetDatePart(int part)
        {
            int year = m_Value.Year;
            int week = 0;

            // Now the week number.
            DateTime startdate = GetFirstDayOfFirstWeekOfYear(year);
            // No overflow please.
            DateTime enddate = year < 9999 ? GetFirstDayOfFirstWeekOfYear(year + 1) : DateTime.MaxValue;

            // The date is member of a week in the next year.
            if (m_Value >= enddate)
            {
                startdate = enddate;
                year++;
            }
            // The date is member of a week in the previous year.
            if (m_Value < startdate)
            {
                startdate = GetFirstDayOfFirstWeekOfYear(year - 1);
                year--;
            }
            if (part == DatePartYear) { return year; }
            // Day of the week.
            int dayofyear = (m_Value - startdate).Days;

            if (part == DatePartDayOfYear) { return dayofyear; }

            // The week number is not zero based.
            week = dayofyear / 7 + 1;
            return week;
        }

        private const int DatePartYear = 0;
        private const int DatePartDayOfYear = 1;
        private const int DatePartWeek = 2;

        /// <summary>Gets the date of the first day of the first week of the year.</summary>
        /// <remarks>
        /// Source: http://en.wikipedia.org/wiki/ISO_8601
        /// 
        /// There are mutually equivalent descriptions of week 01:
        /// - the week with the year's first Thursday in it (the formal ISO definition),
        /// - the week with 4 January in it,
        /// - the first week with the majority (four or more) of its days in the starting year,
        /// - the week starting with the Monday in the period 29 December – 4 January.
        /// </remarks>
        public static Date GetFirstDayOfFirstWeekOfYear(int year)
        {
            var start = new Date(year, 01, 04);
            var adddays = ((int)start.DayOfWeek + 6) % 7;
            return start.AddDays(-adddays);
        }

        #endregion

        #region (XML) (De)serialization

        /// <summary>Initializes a new instance of week date based on the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private WeekDate(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = (Date)info.GetDateTime("Value");
        }

        /// <summary>Adds the underlying property of week date to the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            info.AddValue("Value", m_Value);
        }

        /// <summary>Gets the <see href="XmlSchema"/> to (de) XML serialize a week date.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <summary>Reads the week date from an <see href="XmlReader"/>.</summary>
        /// <remarks>
        /// Uses the string parse function of week date.
        /// </remarks>
        /// <param name="reader">An XML reader.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Guard.NotNull(writer, nameof(writer));
            var s = reader.ReadElementString();
            var val = Parse(s, CultureInfo.InvariantCulture);
            m_Value = val.m_Value;
        }

        /// <summary>Writes the week date to an <see href="XmlWriter"/>.</summary>
        /// <remarks>
        /// Uses the string representation of week date.
        /// </remarks>
        /// <param name="writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            Guard.NotNull(writer, nameof(writer));
            writer.WriteString(ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        #region (JSON) (De)serialization

        /// <summary>Generates a week date from a JSON null object representation.</summary>
        void IJsonSerializable.FromJson() => throw new NotSupportedException(QowaivMessages.JsonSerialization_NullNotSupported);

        /// <summary>Generates a week date from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the week date.
        /// </param>
        void IJsonSerializable.FromJson(string jsonString)
        {
            m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;
        }

        /// <summary>Generates a week date from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the week date.
        /// </param>
        void IJsonSerializable.FromJson(Int64 jsonInteger) => new NotSupportedException(QowaivMessages.JsonSerialization_Int64NotSupported);

        /// <summary>Generates a week date from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the week date.
        /// </param>
        void IJsonSerializable.FromJson(Double jsonNumber) => new NotSupportedException(QowaivMessages.JsonSerialization_DoubleNotSupported);

        /// <summary>Generates a week date from a JSON date representation.</summary>
        /// <param name="jsonDate">
        /// The JSON Date that represents the week date.
        /// </param>
        void IJsonSerializable.FromJson(DateTime jsonDate)
        {
            m_Value = (Date)jsonDate;
        }

        /// <summary>Converts a week date into its JSON object representation.</summary>
        object IJsonSerializable.ToJson()
        {
            return ToString(CultureInfo.InvariantCulture);
        }

        #endregion

        #region IFormattable / ToString

        /// <summary>Returns a <see cref="string"/> that represents the current week date for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Debugger.")]
        private string DebuggerDisplay
        {
            get { return ToString(CultureInfo.InvariantCulture); }
        }

        /// <summary>Returns a <see cref="string"/> that represents the current week date.</summary>
        public override string ToString()
        {
            return ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current week date.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current week date.</summary>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider formatProvider)
        {
            return ToString("", formatProvider);
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current week date.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        /// <remarks>
        /// The formats:
        /// 
        /// y: as year.
        /// w: as week with leading zero.
        /// W: as week without leading zero.
        /// d: as day.
        /// </remarks>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
            {
                return formatted;
            }

            // If no format specified, use the default format.
            if (string.IsNullOrEmpty(format)) { format = @"y-\Ww-d"; }

            // Apply the format.
            return StringFormatter.Apply(this, format, formatProvider ?? CultureInfo.InvariantCulture, FormatTokens);
        }

        /// <summary>The format token instructions.</summary>
        private static readonly Dictionary<char, Func<WeekDate, IFormatProvider, string>> FormatTokens = new Dictionary<char, Func<WeekDate, IFormatProvider, string>>()
        {
            { 'y', (svo, provider) => svo.Year.ToString("0000", provider) },
            { 'w', (svo, provider) => svo.Week.ToString("00", provider) },
            { 'W', (svo, provider) => svo.Week.ToString("0", provider) },
            { 'd', (svo, provider) => svo.Day.ToString("0", provider) },
        };


        #endregion

        #region IEquatable

        /// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
        /// <param name="obj">An object to compare with.</param>
        public override bool Equals(object obj) { return obj is WeekDate && Equals((WeekDate)obj); }

        /// <summary>Returns true if this instance and the other <see cref="WeekDate"/> are equal, otherwise false.</summary>
        /// <param name="other">The <see cref="WeekDate"/> to compare with.</param>
        public bool Equals(WeekDate other) => m_Value == other.m_Value;

        /// <summary>Returns the hash code for this week date.</summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => m_Value.GetHashCode();

        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator ==(WeekDate left, WeekDate right)
        {
            return left.Equals(right);
        }

        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator !=(WeekDate left, WeekDate right)
        {
            return !(left == right);
        }

        #endregion

        #region IComparable

        /// <summary>Compares this instance with a specified System.Object and indicates whether
        /// this instance precedes, follows, or appears in the same position in the sort
        /// order as the specified System.Object.
        /// </summary>
        /// <param name="obj">
        /// An object that evaluates to a week date.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.Value
        /// Condition Less than zero This instance precedes value. Zero This instance
        /// has the same position in the sort order as value. Greater than zero This
        /// instance follows value.-or- value is null.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// value is not a week date.
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj is WeekDate)
            {
                return CompareTo((WeekDate)obj);
            }
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.ArgumentException_Must, "a week date"), "obj");
        }

        /// <summary>Compares this instance with a specified week date and indicates
        /// whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified week date.
        /// </summary>
        /// <param name="other">
        /// The week date to compare with this instance.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(WeekDate other) => m_Value.CompareTo(other.m_Value);


        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(WeekDate l, WeekDate r) => l.CompareTo(r) < 0;

        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator >(WeekDate l, WeekDate r) => l.CompareTo(r) > 0;

        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(WeekDate l, WeekDate r) => l.CompareTo(r) <= 0;

        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(WeekDate l, WeekDate r) => l.CompareTo(r) >= 0;

        #endregion

        #region (Explicit) casting

        /// <summary>Casts a week date to a <see cref="string"/>.</summary>
        public static explicit operator string(WeekDate val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a week date to a date time.</summary>
        public static implicit operator DateTime(WeekDate val) => val.m_Value;

        /// <summary>Casts a <see cref="string"/> to a week date.</summary>
        public static explicit operator WeekDate(string str) => Parse(str, CultureInfo.CurrentCulture);
        /// <summary>Casts a date time to a week date.</summary>
        public static explicit operator WeekDate(DateTime val) { return Create((Date)val); }

        /// <summary>Casts a date to a week date.</summary>
        public static implicit operator WeekDate(Date val) => Create(val);
        /// <summary>Casts a local date time to a week date.</summary>
        public static explicit operator WeekDate(LocalDateTime val) { return Create(val.Date); }

        #endregion

        #region Factory methods

        /// <summary>Converts the string to a week date.</summary>
        /// <param name="s">
        /// A string containing a week date to convert.
        /// </param>
        /// <returns>
        /// A week date.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static WeekDate Parse(string s)
        {
            return Parse(s, CultureInfo.CurrentCulture);
        }

        /// <summary>Converts the string to a week date.</summary>
        /// <param name="s">
        /// A string containing a week date to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// A week date.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static WeekDate Parse(string s, IFormatProvider formatProvider)
        {
            if (TryParse(s, formatProvider, out WeekDate val))
            {
                return val;
            }
            throw new FormatException(QowaivMessages.FormatExceptionWeekDate);
        }

        /// <summary>Converts the string to a week date.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a week date to convert.
        /// </param>
        /// <returns>
        /// The week date if the string was converted successfully, otherwise Empty.
        /// </returns>
        public static WeekDate TryParse(string s)
        {
            if (TryParse(s, out WeekDate val))
            {
                return val;
            }
            return MinValue;
        }

        /// <summary>Converts the string to a week date.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a week date to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out WeekDate result)
        {
            return TryParse(s, CultureInfo.CurrentCulture, out result);
        }

        /// <summary>Converts the string to a week date.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a week date to convert.
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
        public static bool TryParse(string s, IFormatProvider formatProvider, out WeekDate result)
        {
            result = MinValue;
            var match = Pattern.Match(s ?? string.Empty);
            if (match.Success)
            {
                var year = Int32.Parse(match.Groups["year"].Value, formatProvider);
                var week = Int32.Parse(match.Groups["week"].Value, formatProvider);
                var day = Int32.Parse(match.Groups["day"].Value, formatProvider);

                if (TryCreate(year, week, day, out Date dt))
                {
                    result = new WeekDate { m_Value = dt };
                    return true;
                }
            }
            return false;
        }

        /// <summary>Creates a week date from a date time.</summary >
        /// <param name="val" >
        /// A decimal describing a week date.
        /// </param >
        public static WeekDate Create(Date val)
        {
            return new WeekDate { m_Value = val };
        }

        private static bool TryCreate(int year, int week, int day, out Date dt)
        {
            dt = Date.MinValue;

            // Year 0 is not preserved by the regex.
            if (year < 1 ||
                // year 9999 can lead to overflow.
                year == 9999 && ((week == 52 && day > 5) || week == 53))
            {
                return false;
            }
            dt = GetFirstDayOfFirstWeekOfYear(year);

            // Zerobased.
            int dayofyear = (week - 1) * 7 + (day - 1);

            // Set date.
            dt = dt.AddDays(dayofyear);

            // Week 53 can be non-existent.
            if (week == 53 && GetFirstDayOfFirstWeekOfYear(year + 1) <= dt)
            {
                dt = Date.MinValue;
                return false;
            };
            return true;
        }

        #endregion

        #region Validation

        /// <summary>Returns true if the val represents a valid week date, otherwise false.</summary>
        public static bool IsValid(string val)
        {
            return IsValid(val, CultureInfo.CurrentCulture);
        }

        /// <summary>Returns true if the val represents a valid week date, otherwise false.</summary>
        public static bool IsValid(string val, IFormatProvider formatProvider)
        {
            return TryParse(val, formatProvider, out WeekDate wd);
        }

        #endregion
    }
}
