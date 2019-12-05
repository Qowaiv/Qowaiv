﻿#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

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
    [OpenApiDataType(description: "Full-date notation as defined by ISO 8601, for example, 1997-W14-6.", type: "string", format: "date-weekbased")]
    [TypeConverter(typeof(WeekDateTypeConverter))]
    public partial struct WeekDate : ISerializable, IXmlSerializable, IFormattable, IEquatable<WeekDate>, IComparable, IComparable<WeekDate>
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
            if (day < 1 || day > DaysPerWeek)
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
        public int Year => GetDatePart(DatePartYear);

        /// <summary>Gets the week component of the week date represented by this instance.</summary>
        public int Week => GetDatePart(DatePartWeek);

        /// <summary>Gets the day component represented by this instance.</summary>
        public int Day => DayOfWeek == DayOfWeek.Sunday ? DaysPerWeek : (int)DayOfWeek;

        /// <summary>Gets the day of the week represented by this instance.</summary>
        public DayOfWeek DayOfWeek => m_Value.DayOfWeek;

        /// <summary>Gets the day of the year represented by this instance.</summary>
        public int DayOfYear => GetDatePart(DatePartDayOfYear);

        /// <summary>Gets the date time component of this instance.</summary>
        public Date Date => m_Value;

        #endregion

        #region Methods

        /// <summary>Gets the year of week part of the week date.</summary>
        private int GetDatePart(int part)
        {
            int year = m_Value.Year;

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
            var week = dayofyear / DaysPerWeek + 1;
            return week;
        }

        private const int DaysPerWeek = 7;
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

        #region (De)serialization

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

        /// <summary>Serializes the week date to a JSON node.</summary>
        /// <returns>
        /// The serialized JSON string.
        /// </returns>
        public string ToJson() => ToString(CultureInfo.InvariantCulture);

        #endregion

        /// <summary>Returns a <see cref="string"/> that represents the current week date for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => ToString(CultureInfo.InvariantCulture);

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
        private static readonly Dictionary<char, Func<WeekDate, IFormatProvider, string>> FormatTokens = new Dictionary<char, Func<WeekDate, IFormatProvider, string>>
        {
            { 'y', (svo, provider) => svo.Year.ToString("0000", provider) },
            { 'w', (svo, provider) => svo.Week.ToString("00", provider) },
            { 'W', (svo, provider) => svo.Week.ToString("0", provider) },
            { 'd', (svo, provider) => svo.Day.ToString("0", provider) },
        };

        /// <summary>Gets an XML string representation of the week date.</summary>
        private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

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
                var year = int.Parse(match.Groups["year"].Value, formatProvider);
                var week = int.Parse(match.Groups["week"].Value, formatProvider);
                var day = int.Parse(match.Groups["day"].Value, formatProvider);

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
            dt = default;

            // Year 0 is not preserved by the regex.
            if (year < 1 ||
                // year 9999 can lead to overflow.
                year == 9999 && ((week == 52 && day > 5) || week == 53))
            {
                return false;
            }
            dt = GetFirstDayOfFirstWeekOfYear(year);

            // Zero-based.
            int dayofyear = (week - 1) * 7 + (day - 1);

            // Set date.
            dt = dt.AddDays(dayofyear);

            // Week 53 can be non-existent.
            if (week == 53 && GetFirstDayOfFirstWeekOfYear(year + 1) <= dt)
            {
                dt = default;
                return false;
            }
            return true;
        }
    }
}
