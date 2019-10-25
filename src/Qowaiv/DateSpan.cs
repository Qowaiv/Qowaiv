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
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv
{
    /// <summary>Represents a date span.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.Continuous, typeof(ulong))]
    [OpenApiDataType(description: "Date span, specified in years, months and days, for example 1Y+10M+16D.", type: "string", format: "date-span", pattern: @"[+-]?[0-9]+Y)[+-][0-9]+M[+-][0-9]+D")]
    [TypeConverter(typeof(DateSpanTypeConverter))]
    public struct DateSpan : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<DateSpan>, IComparable, IComparable<DateSpan>
    {
        /// <summary>Represents the pattern of a (potential) valid year.</summary>
        public static readonly Regex Pattern = new Regex(@"^(?<Years>([+-]?[0-9]{1,4}))Y(?<Months>([+-][0-9]{1,6}))M(?<Days>([+-][0-9]{1,7}))D$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>Represents the zero date span.</summary>
        public static readonly DateSpan Zero;

        /// <summary>Represents the maximum value of the date span.</summary>
        public static readonly DateSpan MaxValue = new DateSpan { m_Value = AsUInt64(MonthsPerYear * +9998 + 11, +30) };

        /// <summary>Represents the minimum value of the date span.</summary>
        public static readonly DateSpan MinValue = new DateSpan { m_Value = AsUInt64(MonthsPerYear * -9998 - 11, -30) };

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
        public DateSpan(int months, int days)
        {
            m_Value = AsUInt64(months, days);

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

        #region Properties

        /// <summary>The inner value of the date span.</summary>
        private ulong m_Value;

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

        #endregion

        #region Operations

        /// <summary>Unary plus the date span.</summary>
        /// <returns></returns>
        internal DateSpan Plus() => this;

        /// <summary>Negates the date span.</summary>
        public DateSpan Negate() => new DateSpan { m_Value = AsUInt64(-TotalMonths, -Days) };

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
            return new DateSpan { m_Value = AsUInt64(months, days) };
        }

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
            return string.Format(formatProvider, "{0}Y{1:+0;-0;+0}M{2:+0;-0;+0}D", Years, Months, Days);
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

        /// <summary>Unary plus the date span.</summary>
        public static DateSpan operator +(DateSpan span) => span.Plus();

        /// <summary>Negates the date span.</summary>
        public static DateSpan operator -(DateSpan span) => span.Negate();

        /// <summary>Adds two date spans.</summary>
        public static DateSpan operator +(DateSpan l, DateSpan r) => l.Add(r);

        /// <summary>Subtracts two date spans.</summary>
        public static DateSpan operator -(DateSpan l, DateSpan r) => l.Subtract(r);

        #region Factory methods

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

            var match = Pattern.Match(s);

            if (match.Success)
            {
                var y = IntFromGroup(match, nameof(Years));
                var m = IntFromGroup(match, nameof(Months));
                var d = IntFromGroup(match, nameof(Days));

                var months = y * 12 + m;
                var totalDays = d + months * DaysPerMonth;

                if (!IsOutOfRange(months, d, totalDays))
                {
                    result = new DateSpan { m_Value = AsUInt64(months, d) };
                    return true;
                }
            }
            return false;
        }

        private static int IntFromGroup(Match match, string group)
        {
            var str = match.Groups[group].Value;
            return int.Parse(str);
        }

        #endregion

        #region Validation

        /// <summary>Returns true if the val represents a valid date span, otherwise false.</summary>
        public static bool IsValid(string val) => IsValid(val, CultureInfo.CurrentCulture);

        /// <summary>Returns true if the val represents a valid date span, otherwise false.</summary>
        public static bool IsValid(string val, IFormatProvider formatProvider) => TryParse(val, formatProvider, out _);

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

        #endregion
    }
}
