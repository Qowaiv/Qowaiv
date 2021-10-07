﻿#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion;
using Qowaiv.Diagnostics;
using Qowaiv.Formatting;
using Qowaiv.Globalization;
using Qowaiv.Json;
using Qowaiv.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv
{
    /// <summary>Represents a yes-no.</summary>
    /// <remarks>
    /// A yes-no is a (bi-)polar that obviously has the values "yes" and "no". It also
    /// has an "empty"(unset) and "unknown" value.It maps easily with a <see cref="bool"/>, but
    /// Supports all kind of formatting(and both empty and unknown) that can not be
    /// achieved when modeling a property as <see cref="bool"/> instead of an <see cref="YesNo"/>.
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.All, typeof(byte))]
    [OpenApiDataType(description: "Yes-No notation.", example: "yes", type: "string", format: "yes-no", nullable: true, @enum: "yes,no,?")]
    [TypeConverter(typeof(YesNoTypeConverter))]
    public partial struct YesNo : ISerializable, IXmlSerializable, IFormattable, IEquatable<YesNo>, IComparable, IComparable<YesNo>
    {
        /// <summary>Represents an empty/not set yes-no.</summary>
        public static readonly YesNo Empty;

        /// <summary>Represents an unknown (but set) yes-no.</summary>
        public static readonly YesNo No = new YesNo(1);

        /// <summary>Represents an unknown (but set) yes-no.</summary>
        public static readonly YesNo Yes = new YesNo(2);

        /// <summary>Represents an unknown (but set) yes-no.</summary>
        public static readonly YesNo Unknown = new YesNo(3);

        /// <summary>Contains yes and no.</summary>
        public static readonly IReadOnlyCollection<YesNo> YesAndNo = new[] { Yes, No };

        /// <summary>Returns true if the yes-no value represents no, otherwise false.</summary>
        public bool IsNo() => m_Value == No.m_Value;

        /// <summary>Returns true if the yes-no value represents yes, otherwise false.</summary>
        public bool IsYes() => m_Value == Yes.m_Value;

        /// <summary>Returns true if the yes-no value represents yes or no.</summary>
        public bool IsYesOrNo() => IsYes() || IsNo();

        /// <summary>Deserializes the gender from a JSON number.</summary>
        /// <param name="json">
        /// The JSON number to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized gender.
        /// </returns>
        public static YesNo FromJson(double json) => FromJson<double>((int)json);

        /// <summary>Deserializes the yes-no from a JSON number.</summary>
        /// <param name="json">
        /// The JSON number to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized yes-no.
        /// </returns>
        public static YesNo FromJson(long json) => FromJson<long>(json);

        /// <summary>Deserializes the yes-no from a JSON boolean.</summary>
        /// <param name="json">
        /// The JSON boolean to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized yes-no.
        /// </returns>
        public static YesNo FromJson(bool json) => json ? Yes : No;

        private static YesNo FromJson<TFrom>(long val)
        => val switch
        {
            0 => No,
            1 => Yes,
            byte.MaxValue => Unknown,
            short.MaxValue => Unknown,
            int.MaxValue => Unknown,
            long.MaxValue => Unknown,
            _ => throw Exceptions.InvalidCast<TFrom, YesNo>(),
        };

        /// <summary>Serializes the yes-no to a JSON node.</summary>
        /// <returns>
        /// The serialized JSON string.
        /// </returns>
        public string ToJson() => SerializationValues[m_Value];

        /// <summary>Returns a <see cref="string"/> that represents the current yes-no for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => this.DebuggerDisplay("{0:f}");

        /// <summary>Returns a formatted <see cref="string"/> that represents the current yes-no.</summary>
        /// <param name="format">
        /// The format that describes the formatting.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        /// <remarks>
        /// The formats:
        /// 
        /// i: as integer (note, unknown is a question mark sign).
        /// c/C: as single character (y/n/?) (Upper cased).
        /// f/F: as formatted string (Title cased).
        /// b/B: as boolean (true/false) (Title cased).
        /// </remarks>
        public string ToString(string format, IFormatProvider formatProvider)
            => StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted)
            ? formatted
            : StringFormatter.Apply(this, format.WithDefault("f"), formatProvider, FormatTokens);

        /// <summary>The format token instructions.</summary>
        private static readonly Dictionary<char, Func<YesNo, IFormatProvider, string>> FormatTokens = new Dictionary<char, Func<YesNo, IFormatProvider, string>>
        {
            { 'c', (svo, provider) => svo.GetResourceString("ch_", provider) },
            { 'C', (svo, provider) => svo.GetResourceString("ch_", provider).ToUpper(provider) },
            { 'i', (svo, provider) => svo.GetResourceString("int_", provider) },
            { 'f', (svo, provider) => svo.GetResourceString("f_", provider) },
            { 'F', (svo, provider) => svo.GetResourceString("f_", provider).ToTitleCase(provider) },
            { 'b', (svo, provider) => svo.GetResourceString("b_", provider) },
            { 'B', (svo, provider) => svo.GetResourceString("b_", provider).ToTitleCase(provider) },
        };

        /// <summary>Gets an XML string representation of the yes-no.</summary>
        private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

        /// <summary>Casts a yes-no to a <see cref="string"/>.</summary>
        public static explicit operator string(YesNo val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a yes-no.</summary>
        public static explicit operator YesNo(string str) => Cast.String<YesNo>(TryParse, str);

        /// <summary>Casts a yes-no to a nullable <see cref="bool"/>.</summary>
        public static explicit operator bool?(YesNo val) => BooleanValues[val.m_Value];

        /// <summary>Casts a yes-no to a <see cref="bool"/>.</summary>
        public static implicit operator bool(YesNo val) => val.IsYes();

        /// <summary>Casts a nullable <see cref="bool"/> to a yes-no.</summary>
        public static explicit operator YesNo(bool? val)
        {
            if (val.HasValue)
            {
                return val.Value ? Yes : No;
            }
            return Empty;
        }

        /// <summary>Casts a <see cref="bool"/> to a yes-no.</summary>
        public static explicit operator YesNo(bool val) => val ? Yes : No;

        private static readonly bool?[] BooleanValues = new bool?[] { null, false, true, null };

        /// <summary>Converts the string to a yes-no.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a yes-no to convert.
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
        public static bool TryParse(string s, IFormatProvider formatProvider, out YesNo result)
        {
            result = Empty;
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
            else if (ParseValues.TryGetValue(formatProvider, buffer.ToString(), out var val))
            {
                result = new YesNo(val);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static readonly YesNoValues ParseValues = new();

        /// <summary>Gets the yes-no labels.</summary>
        /// <remarks>
        /// Used for both serialization and resource lookups.
        /// </remarks>
        private static readonly string[] LookupSuffix = { null, "No", "Yes", "Unknown" };
        private static readonly string[] SerializationValues = { null, "no", "yes", "?" };
        
        private sealed class YesNoValues : LocalizedValues<byte>
        {
            public YesNoValues() : base(new Dictionary<string, byte>
            {
                { "", 0 },
                { "0", 1 },
                { "1", 2 },
                { "FALSE", 1 },
                { "TRUE", 2 },
                { "NO", 1 },
                { "YES", 2 },
                { "N", 1 },
                { "Y", 2 },
            }) { }

            protected override void AddCulture(CultureInfo culture)
            {
                foreach (var value in YesAndNo)
                {
                    var label = value.ToString("", culture).ToUpper(culture);
                    var @char = value.ToString("c", culture).ToUpper(culture);
                    var @bool = value.ToString("b", culture).ToUpper(culture);

                    this[culture][label] = value.m_Value;
                    this[culture][@char] = value.m_Value;
                    this[culture][@bool] = value.m_Value;
                }
            }
        }

        private static readonly ResourceManager ResourceManager = new ResourceManager("Qowaiv.YesNoLabels", typeof(YesNo).Assembly);

        /// <summary>Get resource string.</summary>
        /// <param name="prefix">
        /// The prefix of the resource key.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        private string GetResourceString(string prefix, IFormatProvider formatProvider)
            => IsEmpty()
            ? string.Empty
            : ResourceManager.Localized(formatProvider, prefix, LookupSuffix[m_Value]);

    }
}
