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
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Qowaiv
{
    /// <summary>Represents a date span.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.Continuous, typeof(ulong))]
    [OpenApiDataType(description: "Date span, specified in years, months and days, for example 1Y+10M+16D.", type: "string", format: "date-span", pattern: @"[+-]?[0-9]+Y[+-][0-9]+M[+-][0-9]+D")]
    [TypeConverter(typeof(DateSpanTypeConverter))]
    public partial struct DateSpan : ISerializable, IXmlSerializable, IFormattable, IEquatable<DateSpan>, IComparable, IComparable<DateSpan>
    {
        /// <summary>Represents the pattern of a (potential) valid year.</summary>
        public static readonly Regex Pattern = new Regex(@"^(?<Years>([+-]?[0-9]{1,4}))Y(?<Months>([+-][0-9]{1,6}))M(?<Days>([+-][0-9]{1,7}))D$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>Represents the zero date span.</summary>
        public static readonly DateSpan Zero;

        /// <summary>Represents the maximum value of the date span.</summary>
        public static readonly DateSpan MaxValue = new DateSpan(AsUInt64(MonthsPerYear * +9998 + 11, +30));

        /// <summary>Represents the minimum value of the date span.</summary>
        public static readonly DateSpan MinValue = new DateSpan(AsUInt64(MonthsPerYear * -9998 - 11, -30) );

        /// <summary>The average amount of days per month, taken leap years into account.</summary>
        internal const double DaysPerMonth = 30.421625;

        /// <summary>The total of days, that can not be applied on a <see cref="Date"/> or <see cref="DateTime"/>.</summary>
        internal const int MaxDays = (int)(DaysPerMonth * 120000);

        /// <summary>12 months per year.</summary>
        internal const int MonthsPerYear = 12;
        
        /// <summary>365 days per year.</summary>
        internal const int DaysPerYear = 365;
        
        /// <summary>366 days per leap year.</summary>
        internal const int DaysPerLeapYear = 366;

        /// <summary>The shift position of the total months in the value.</summary>
        internal const int MonthShift = 32;

        /// <summary>Creates a new instance of a <see cref="DateSpan"/>.</summary>
        /// <param name="months">
        /// Number of months.
        /// </param>
        /// <param name="days">
        /// Number of days.
        /// </param>
        public DateSpan(int months, int days): this(AsUInt64(months, days))
        {
            if (IsOutOfRange(months, days, TotalDays))
            {
                throw new ArgumentOutOfRangeException(QowaivMessages.ArgumentOutOfRangeException_DateSpan, (Exception)null);
            }
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
            : this(years * MonthsPerYear + months, days) { }

        /// <summary>Converts the combination of months and days to a <see cref="ulong"/>.</summary>
        private static ulong AsUInt64(long months, long days) => (uint)days | ((ulong)months << MonthShift);

        /// <summary>Gets the total of months.</summary>
        public int TotalMonths => (int)(m_Value >> MonthShift);

        /// <summary>Gets the years component of the date span.</summary>
        public int Years => TotalMonths / MonthsPerYear;

        /// <summary>Gets the months component of the date span.</summary>
        public int Months => TotalMonths % MonthsPerYear;

        /// <summary>Gets the days component of the date span.</summary>
        public int Days => (int)m_Value;

        /// <summary>Gets a (approximate) value to sort the date spans by.</summary>
        internal double TotalDays => Days + TotalMonths * DaysPerMonth;

        #region Operations

        /// <summary>Unary plus the date span.</summary>
        /// <returns></returns>
        internal DateSpan Plus() => this;

        /// <summary>Negates the date span.</summary>
        public DateSpan Negate() => new DateSpan(AsUInt64(-TotalMonths, -Days));

        /// <summary>Returns a new date span whose value is the sum of the specified date span and this instance.</summary>
        ///<param name="other">
        /// The date span to add.
        ///</param>
        ///<exception cref="OverflowException">
        /// The resulting time span is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
        ///</exception>
        public DateSpan Add(DateSpan other)
        {
            long days = (long)Days + other.Days;
            long months = (long)TotalMonths + other.TotalMonths;
            return Mutate(months, days);
        }

        /// <summary>Returns a new date span whose value is the subtraction of the specified date span and this instance.</summary>
        ///<param name="other">
        /// The date span to subtract.
        ///</param>
        ///<exception cref="OverflowException">
        /// The resulting time span is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
        ///</exception>
        public DateSpan Subtract(DateSpan other)
        {
            long days = (long)Days - other.Days;
            long months = (long)TotalMonths - other.TotalMonths;
            return Mutate(months, days);
        }

        /// <summary>Returns a new date span whose value is the sum of the days to add this instance.</summary>
        ///<param name="days">
        /// The days to add.
        ///</param>
        ///<exception cref="OverflowException">
        /// The resulting time span is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
        ///</exception>
        public DateSpan AddDays(int days) => Mutate(TotalMonths, Days + (long)days);

        /// <summary>Returns a new date span whose value is the sum of the months to add this instance.</summary>
        ///<param name="months">
        /// The months to add.
        ///</param>
        ///<exception cref="OverflowException">
        /// The resulting time span is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
        ///</exception>
        public DateSpan AddMonths(int months) => Mutate(TotalMonths + (long)months, Days);

        /// <summary>Returns a new date span whose value is the sum of the years to add this instance.</summary>
        ///<param name="years">
        /// The years to add.
        ///</param>
        ///<exception cref="OverflowException">
        /// The resulting time span is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
        ///</exception>
        public DateSpan AddYears(int years) => Mutate(TotalMonths + years * (long)MonthsPerYear, Days);

        /// <summary>Mutates the months and days.</summary>
        ///<exception cref="OverflowException">
        /// The resulting time span is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
        ///</exception>
        private static DateSpan Mutate(long months, long days)
        {
            var totalDays = months * DaysPerMonth + days;

            if (IsOutOfRange(months, days, totalDays))
            {
                throw new OverflowException(QowaivMessages.OverflowException_DateSpan);
            }
            return new DateSpan(AsUInt64(months, days));
        }

        #endregion

        /// <summary>Deserializes the date span from a JSON number.</summary>
        /// <param name="json">
        /// The JSON number to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized date span.
        /// </returns>
        public static DateSpan FromJson(long json) => FromDays((int)json);

        /// <summary>Serializes the date span to a JSON node.</summary>
        /// <returns>
        /// The serialized JSON string.
        /// </returns>
        public string ToJson() => ToString(CultureInfo.InvariantCulture);

        /// <summary>Returns a <see cref="string" /> that represents the current date span for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => string.Format(CultureInfo.InvariantCulture, "{0:#,###0} Years, {1:#,###0} Months, {2:#,###0} Days", Years, Months, Days);

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
            return string.Format(formatProvider, "{0}Y{1:+0;-0;+0}M{2:+0;-0;+0}D", Years, Months, Days);
        }

        /// <summary>Gets an XML string representation of the date span.</summary>
        private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

        /// <inheritdoc/>
        public bool Equals(DateSpan other) => m_Value == other.m_Value;

        /// <inheritdoc/>
        public int CompareTo(DateSpan other) => TotalDays.CompareTo(other.TotalDays);

        /// <inheritdoc/>
        public override int GetHashCode() => m_Value.GetHashCode();

        /// <summary>Unary plus the date span.</summary>
        public static DateSpan operator +(DateSpan span) => span.Plus();

        /// <summary>Negates the date span.</summary>
        public static DateSpan operator -(DateSpan span) => span.Negate();

        /// <summary>Adds two date spans.</summary>
        public static DateSpan operator +(DateSpan l, DateSpan r) => l.Add(r);

        /// <summary>Subtracts two date spans.</summary>
        public static DateSpan operator -(DateSpan l, DateSpan r) => l.Subtract(r);

        /// <summary>Creates a date span from days only.</summary>
        public static DateSpan FromDays(int days) => new DateSpan(0, 0, days);

        /// <summary>Creates a date span from months only.</summary>
        public static DateSpan FromMonths(int months) => new DateSpan(0, months, 0);

        /// <summary>Creates a date span from months only.</summary>
        public static DateSpan FromYears(int years) => new DateSpan(years, 0, 0);

        /// <summary>Calculates the age (in years and days) for a given date for today.</summary>
        /// <param name="t1">
        /// The date to get the age for.
        /// </param>
        /// <returns>
        /// The age defined in years and days.
        /// </returns>
        public static DateSpan Age(Date t1) => Age(t1, Clock.Today());

        /// <summary>Calculates the age (in years and days) for a given date for the reference date.</summary>
        /// <param name="t1">
        /// The date to get the age for.
        /// </param>
        /// <param name="reference">
        /// The reference date.
        /// </param>
        /// <returns>
        /// The age defined in years and days.
        /// </returns>
        public static DateSpan Age(Date t1, Date reference) => Subtract(reference, t1, DateSpanSettings.WithoutMonths);

        /// <summary>Creates a date span on by subtracting t2 from t1.</summary>
        /// <param name="t1">
        /// The date to subtract from.
        /// </param>
        /// <param name="t2">
        /// The date to subtract.
        /// </param>
        /// <returns>
        /// Returns a date span describing the duration between t1 and t2.
        /// </returns>
        public static DateSpan Subtract(Date t1, Date t2) => Subtract(t1, t2, DateSpanSettings.Default);

        /// <summary>Creates a date span on by subtracting t2 from t1.</summary>
        /// <param name="t1">
        /// The date to subtract from.
        /// </param>
        /// <param name="t2">
        /// The date to subtract.
        /// </param>
        /// <param name="settings">
        /// The settings to apply.
        /// </param>
        /// <returns>
        /// Returns a date span describing the duration between t1 and t2.
        /// </returns>
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
            var months = withMonths ? max.Month - min.Month : 0;
            var days = withMonths
                ? max.Day - min.Day
                : max.DayOfYear - min.DayOfYear;

            if (days < 0 && noMixedSings)
            {
                if (withMonths)
                {
                    months--;
                    var sub = daysFirst ? min : max.AddMonths(-1);
                    days += DateTime.DaysInMonth(sub.Year, sub.Month);
                }
                else
                {
                    years--;
                    var sub = daysFirst ? min : max.AddYears(-1);
                    days += DateTime.IsLeapYear(sub.Year) ? DaysPerLeapYear : DaysPerYear;
                }
            }

            return negative
                ? new DateSpan(-years, -months, -days)
                : new DateSpan(+years, +months, +days);
        }

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

            var match = Pattern.Match(s);

            if (match.Success)
            {
                var y = IntFromGroup(match, nameof(Years), formatProvider);
                var m = IntFromGroup(match, nameof(Months), formatProvider);
                var d = IntFromGroup(match, nameof(Days), formatProvider);

                var months = y * 12 + m;
                var totalDays = d + months * DaysPerMonth;

                if (!IsOutOfRange(months, d, totalDays))
                {
                    result = new DateSpan(AsUInt64(months, d));
                    return true;
                }
            }
            return false;
        }

        private static int IntFromGroup(Match match, string group, IFormatProvider formatProvider)
        {
            var str = match.Groups[group].Value;
            return int.Parse(str, formatProvider);
        }

        /// <summary>Returns true if the combination of months and days can not be processed.</summary>
        private static bool IsOutOfRange(long months, long days, double totalDays)
        {
            return months > MaxValue.TotalMonths
                || months < MinValue.TotalMonths
                || totalDays > MaxValue.TotalDays
                || totalDays < MinValue.TotalDays
                || days > +MaxDays
                || days < -MaxDays;
        }
    }
}
