﻿using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv
{
    /// <summary>Represents a month span.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable]
    [OpenApiDataType(description: "month span", type: "MonthSpan", format: "MonthSpan")]
    [TypeConverter(typeof(Conversion.MonthSpanTypeConverter))]
    public partial struct MonthSpan : ISerializable, IXmlSerializable, IFormattable, IEquatable<MonthSpan>, IComparable, IComparable<MonthSpan>
    {
        /// <summary>Represents a month span with a zero duration.</summary>
        public static readonly MonthSpan Zero;

        /// <summary>Gets the minimum month span (-9998 years).</summary>
        public static readonly MonthSpan MinValue = new MonthSpan(-9998 * 12);

        /// <summary>Gets the maximum month span (+9998 years).</summary>
        public static readonly MonthSpan MaxValue = new MonthSpan(+9998 * 12);

        /// <summary>Gets the total of months.</summary>
        public int TotalMonths => m_Value;

        /// <summary>Gets the years component of the month span.</summary>
        public int Years => TotalMonths / DateSpan.MonthsPerYear;

        /// <summary>Gets the months component of the month span.</summary>
        public int Months => TotalMonths % DateSpan.MonthsPerYear;


        /// <summary>Returns a <see cref = "string "/> that represents the month span for DEBUG purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => ToString("F", CultureInfo.InvariantCulture);

        /// <summary>Returns a formatted <see cref = "string "/> that represents the month span.</summary>
        /// <param name = "format">
        /// The format that this describes the formatting.
        /// </param>
        /// <param name = "formatProvider">
        /// The format provider.
        /// </param>
        /// <remarks>
        /// The formats:
        /// 
        /// F: as 0Y+0D.
        /// All others format the total months.
        /// </remarks>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
            {
                return formatted;
            }

            if (string.IsNullOrEmpty(format) || format == "F")
            {
                return string.Format(formatProvider, "{0}Y{1:+0;-0;+0}M", Years, Months);
            }

            return m_Value.ToString(format, formatProvider);
        }

        /// <summary>Gets an XML string representation of the month span.</summary>
        private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

        /// <summary>Serializes the month span to a JSON node.</summary>
        /// <returns>
        /// The serialized JSON number.
        /// </returns>
        public long ToJson() => m_Value;

        /// <summary>Deserializes the month span from a JSON number.</summary>
        /// <param name = "json">
        /// The JSON number to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized month span.
        /// </returns>
        public static MonthSpan FromJson(long json) => FromMonths(Cast.ToInt<MonthSpan>(json));

        #region (Explicit) casting

        /// <summary>Casts the month span to a <see cref = "string"/>.</summary>
        public static explicit operator string(MonthSpan val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref = "string "/> to a month span.</summary>
        public static explicit operator MonthSpan(string str) => Parse(str, CultureInfo.CurrentCulture);

        /// <summary>Casts the month span to a <see cref = "int"/>.</summary>
        public static explicit operator int(MonthSpan val) => val.m_Value;
        /// <summary>Casts a <see cref = "int "/> to a month span.</summary>
        public static explicit operator MonthSpan(int val) => FromMonths(val);

        /// <summary>Casts the month span to a <see cref="DateSpan"/>.</summary>
        public static implicit operator DateSpan(MonthSpan val) => DateSpan.FromMonths(val.m_Value);
        /// <summary>Casts a <see cref = "DateSpan"/> to a month span.</summary>
        public static explicit operator MonthSpan(DateSpan val) => MonthSpan.FromMonths(val.TotalMonths);

        #endregion

        /// <summary>Represents the underlying value as <see cref = "IConvertible"/>.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IConvertible Convertable => m_Value;
        /// <inheritdoc/>
        TypeCode IConvertible.GetTypeCode() => TypeCode.Int32;

        /// <summary>Converts the <see cref = "string "/> to <see cref = "MonthSpan"/>.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name = "s">
        /// A string containing the month span to convert.
        /// </param>
        /// <param name = "formatProvider">
        /// The specified format provider.
        /// </param>
        /// <param name = "result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, IFormatProvider formatProvider, out MonthSpan result)
        {
            result = default;
            if (string.IsNullOrEmpty(s))
            {
                return true;
            }
            if (int.TryParse(s, NumberStyles.Integer, formatProvider, out var months)
                && TryCreate(months, out result))
            {
                return true;
            }

            if (DateSpan.TryParse(s, formatProvider, out var dateSpan))
            {
                result = FromMonths(dateSpan.TotalMonths);
                return true;
            }
            return false;
        }

        /// <summary>Creates a date span from months only.</summary>
        public static MonthSpan FromMonths(int months)
        {
            if (TryCreate(months, out var monthSpan))
            {
                return monthSpan;
            }
            throw new ArgumentOutOfRangeException("months", QowaivMessages.FormatExceptionMonthSpan);
        }

        /// <summary>Tries to Create a date span from months only.</summary>
        public static bool TryCreate(int months, out MonthSpan monthSpan)
        {
            monthSpan = default;
            if (months >= MinValue.TotalMonths && months <= MaxValue.TotalMonths)
            {
                monthSpan = new MonthSpan(months);
                return true;
            }
            return false;
        }
    }
}
