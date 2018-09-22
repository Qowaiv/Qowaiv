#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion.IO;
using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv.IO
{
    /// <summary>Represents a stream size.</summary>
    /// <remarks>
    /// A stream size measures the size of a computer file or stream. Typically it is 
    /// measured in bytes with an SI prefix. The actual amount of disk space consumed by
    /// the file depends on the file system. The maximum stream size a file system
    /// supports depends on the number of bits reserved to store size information
    /// and the total size of the file system. This value type can not represent
    /// stream sizes bigger than Int64.MaxValue.
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.Continuous, typeof(Int64))]
    [TypeConverter(typeof(StreamSizeTypeConverter))]
    public struct StreamSize : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<StreamSize>, IComparable, IComparable<StreamSize>
    {
        /// <summary>Represents an empty/not set stream size.</summary>
        public static readonly StreamSize Zero;

        /// <summary>Represents 1 Byte.</summary>
        public static readonly StreamSize Byte = new StreamSize { m_Value = 1L };

        /// <summary>Represents 1 kilobyte (1,000 byte).</summary>
        public static readonly StreamSize KB = new StreamSize { m_Value = 1000L };

        /// <summary>Represents 1 Megabyte (1,000,000 byte).</summary>
        public static readonly StreamSize MB = new StreamSize { m_Value = 1000000L };

        /// <summary>Represents 1 Gigabyte (1,000,000,000 byte).</summary>
        public static readonly StreamSize GB = new StreamSize { m_Value = 1000000000L };

        /// <summary>Represents 1 Terabyte (1,000,000,000,000 byte).</summary>
        public static readonly StreamSize TB = new StreamSize { m_Value = 1000000000000L };

        /// <summary>Represents 1 Petabyte (1,000,000,000,000,000 byte).</summary>
        public static readonly StreamSize PB = new StreamSize { m_Value = 1000000000000000L };


        /// <summary>Represents 1 kibibyte (1,024 byte).</summary>
        public static readonly StreamSize KiB = new StreamSize { m_Value = 1L << 10 };

        /// <summary>Represents 1 Mebibyte (1,048,576 byte).</summary>
        public static readonly StreamSize MiB = new StreamSize { m_Value = 1L << 20 };

        /// <summary>Represents 1 Gibibyte (1,073,741,824 byte).</summary>
        public static readonly StreamSize GiB = new StreamSize { m_Value = 1L << 30 };

        /// <summary>Represents 1 Tebibyte (1,099,511,627,776 byte).</summary>
        public static readonly StreamSize TiB = new StreamSize { m_Value = 1L << 40 };

        /// <summary>Represents 1 Petabyte (1,125,899,906,842,624 byte).</summary>
        public static readonly StreamSize PiB = new StreamSize { m_Value = 1L << 50 };

        /// <summary>Represents the minimum stream size that can be represented.</summary>
        public static readonly StreamSize MinValue = new StreamSize { m_Value = long.MinValue };

        /// <summary>Represents the maximum stream size that can be represented.</summary>
        public static readonly StreamSize MaxValue = new StreamSize { m_Value = long.MaxValue };

        /// <summary>Initializes a new instance of a stream size.</summary>
        /// <param name="size">
        /// The number of bytes.
        /// </param>
        public StreamSize(long size)
        {
            m_Value = size;
        }

        #region Properties

        /// <summary>The inner value of the stream size.</summary>
        private long m_Value;

        #endregion

        #region StreamSize manipulation

        /// <summary>Increases the stream size with one percent.</summary>
        public StreamSize Increment()
        {
            return this.Add(StreamSize.Byte);
        }
        /// <summary>Decreases the stream size with one percent.</summary>
        public StreamSize Decrement()
        {
            return this.Subtract(StreamSize.Byte);
        }

        /// <summary>Pluses the stream size.</summary>
        public StreamSize Plus()
        {
            return new StreamSize(+m_Value);
        }
        /// <summary>Negates the stream size.</summary>
        public StreamSize Negate()
        {
            return new StreamSize(-m_Value);
        }

        /// <summary>Adds a stream size to the current stream size.</summary>
        /// <param name="streamSize">
        /// The stream size to add.
        /// </param>
        public StreamSize Add(StreamSize streamSize) { return m_Value + streamSize.m_Value; }

        /// <summary>Adds the specified percentage to the stream size.</summary>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        public StreamSize Add(Percentage p) { return m_Value.Add(p); }

        /// <summary>Subtracts a stream size from the current stream size.</summary>
        /// <param name="streamSize">
        /// The stream size to Subtract.
        /// </param>
        public StreamSize Subtract(StreamSize streamSize) { return m_Value - streamSize.m_Value; }

        /// <summary>AddsSubtract the specified percentage from the stream size.</summary>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        public StreamSize Subtract(Percentage p) { return m_Value.Subtract(p); }

        #region Multiply

        /// <summary>Multiplies the stream size with a specified factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public StreamSize Multiply(Decimal factor) { return (StreamSize)(m_Value * factor); }

        /// <summary>Multiplies the stream size with a specified factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public StreamSize Multiply(Double factor) { return Multiply((Decimal)factor); }

        /// <summary>Multiplies the stream size with a specified factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public StreamSize Multiply(Single factor) { return Multiply((Decimal)factor); }

        /// <summary>Multiplies the stream size with a specified factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public StreamSize Multiply(Percentage factor) { return Multiply((Decimal)factor); }


        /// <summary>Multiplies the stream size with a specified factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public StreamSize Multiply(Int64 factor) { return Multiply((Decimal)factor); }

        /// <summary>Multiplies the stream size with a specified factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public StreamSize Multiply(Int32 factor) { return Multiply((Decimal)factor); }

        /// <summary>Multiplies the stream size with a specified factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public StreamSize Multiply(Int16 factor) { return Multiply((Decimal)factor); }


        /// <summary>Multiplies the stream size with a specified factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        [CLSCompliant(false)]
        public StreamSize Multiply(UInt64 factor) { return Multiply((Decimal)factor); }

        /// <summary>Multiplies the stream size with a specified factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        [CLSCompliant(false)]
        public StreamSize Multiply(UInt32 factor) { return Multiply((Decimal)factor); }

        /// <summary>Multiplies the stream size with a specified factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        [CLSCompliant(false)]
        public StreamSize Multiply(UInt16 factor) { return Multiply((Decimal)factor); }

        #endregion

        #region Divide

        /// <summary>Divide the stream size by a specified factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public StreamSize Divide(Decimal factor) { return (StreamSize)(m_Value / factor); }

        /// <summary>Divide the stream size by a specified factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public StreamSize Divide(Double factor) { return Divide((Decimal)factor); }

        /// <summary>Divide the stream size by a specified factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public StreamSize Divide(Single factor) { return Divide((Decimal)factor); }

        /// <summary>Divide the stream size by a specified factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public StreamSize Divide(Percentage factor) { return Divide((Decimal)factor); }


        /// <summary>Divide the stream size by a specified factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public StreamSize Divide(Int64 factor) { return Divide((Decimal)factor); }

        /// <summary>Divide the stream size by a specified factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public StreamSize Divide(Int32 factor) { return Divide((Decimal)factor); }

        /// <summary>Divide the stream size by a specified factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public StreamSize Divide(Int16 factor) { return Divide((Decimal)factor); }


        /// <summary>Divide the stream size by a specified factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        [CLSCompliant(false)]
        public StreamSize Divide(UInt64 factor) { return Divide((Decimal)factor); }

        /// <summary>Divide the stream size by a specified factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        [CLSCompliant(false)]
        public StreamSize Divide(UInt32 factor) { return Divide((Decimal)factor); }

        /// <summary>Divide the stream size by a specified factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        [CLSCompliant(false)]
        public StreamSize Divide(UInt16 factor) { return Divide((Decimal)factor); }

        #endregion

        /// <summary>Increases the stream size with one percent.</summary>
        public static StreamSize operator ++(StreamSize streamSize) { return streamSize.Increment(); }
        /// <summary>Decreases the stream size with one percent.</summary>
        public static StreamSize operator --(StreamSize streamSize) { return streamSize.Decrement(); }

        /// <summary>Unitary plusses the stream size.</summary>
        public static StreamSize operator +(StreamSize streamSize) { return streamSize.Plus(); }
        /// <summary>Negates the stream size.</summary>
        public static StreamSize operator -(StreamSize streamSize) { return streamSize.Negate(); }

        /// <summary>Adds the left and the right stream size.</summary>
        public static StreamSize operator +(StreamSize l, StreamSize r) { return l.Add(r); }
        /// <summary>Subtracts the right from the left stream size.</summary>
        public static StreamSize operator -(StreamSize l, StreamSize r) { return l.Subtract(r); }

        /// <summary>Adds the percentage to the stream size.</summary>
        public static StreamSize operator +(StreamSize streamSize, Percentage p) { return streamSize.Add(p); }
        /// <summary>Subtracts the percentage from the stream size.</summary>
        public static StreamSize operator -(StreamSize streamSize, Percentage p) { return streamSize.Subtract(p); }

        /// <summary>Multiplies the stream size with the factor.</summary>
        public static StreamSize operator *(StreamSize streamSize, Decimal factor) { return streamSize.Multiply(factor); }
        /// <summary>Multiplies the stream size with the factor.</summary>
        public static StreamSize operator *(StreamSize streamSize, Double factor) { return streamSize.Multiply(factor); }
        /// <summary>Multiplies the stream size with the factor.</summary>
        public static StreamSize operator *(StreamSize streamSize, Single factor) { return streamSize.Multiply(factor); }
        /// <summary>Multiplies the stream size with the factor.</summary>
        public static StreamSize operator *(StreamSize streamSize, Percentage factor) { return streamSize.Multiply(factor); }

        /// <summary>Multiplies the stream size with the factor.</summary>
        public static StreamSize operator *(StreamSize streamSize, Int64 factor) { return streamSize.Multiply(factor); }
        /// <summary>Multiplies the stream size with the factor.</summary>
        public static StreamSize operator *(StreamSize streamSize, Int32 factor) { return streamSize.Multiply(factor); }
        /// <summary>Multiplies the stream size with the factor.</summary>
        public static StreamSize operator *(StreamSize streamSize, Int16 factor) { return streamSize.Multiply(factor); }

        /// <summary>Multiplies the stream size with the factor.</summary>
        [CLSCompliant(false)]
        public static StreamSize operator *(StreamSize streamSize, UInt64 factor) { return streamSize.Multiply(factor); }
        /// <summary>Multiplies the stream size with the factor.</summary>
        [CLSCompliant(false)]
        public static StreamSize operator *(StreamSize streamSize, UInt32 factor) { return streamSize.Multiply(factor); }
        /// <summary>Multiplies the stream size with the factor.</summary>
        [CLSCompliant(false)]
        public static StreamSize operator *(StreamSize streamSize, UInt16 factor) { return streamSize.Multiply(factor); }

        /// <summary>Divides the stream size by the factor.</summary>
        public static StreamSize operator /(StreamSize streamSize, Decimal factor) { return streamSize.Divide(factor); }
        /// <summary>Divides the stream size by the factor.</summary>
        public static StreamSize operator /(StreamSize streamSize, Double factor) { return streamSize.Divide(factor); }
        /// <summary>Divides the stream size by the factor.</summary>
        public static StreamSize operator /(StreamSize streamSize, Single factor) { return streamSize.Divide(factor); }
        /// <summary>Divides the stream size by the factor.</summary>
        public static StreamSize operator /(StreamSize streamSize, Percentage factor) { return streamSize.Divide(factor); }

        /// <summary>Divides the stream size by the factor.</summary>
        public static StreamSize operator /(StreamSize streamSize, Int64 factor) { return streamSize.Divide(factor); }
        /// <summary>Divides the stream size by the factor.</summary>
        public static StreamSize operator /(StreamSize streamSize, Int32 factor) { return streamSize.Divide(factor); }
        /// <summary>Divides the stream size by the factor.</summary>
        public static StreamSize operator /(StreamSize streamSize, Int16 factor) { return streamSize.Divide(factor); }

        /// <summary>Divides the stream size by the factor.</summary>
        [CLSCompliant(false)]
        public static StreamSize operator /(StreamSize streamSize, UInt64 factor) { return streamSize.Divide(factor); }
        /// <summary>Divides the stream size by the factor.</summary>
        [CLSCompliant(false)]
        public static StreamSize operator /(StreamSize streamSize, UInt32 factor) { return streamSize.Divide(factor); }
        /// <summary>Divides the stream size by the factor.</summary>
        [CLSCompliant(false)]
        public static StreamSize operator /(StreamSize streamSize, UInt16 factor) { return streamSize.Divide(factor); }

        #endregion

        #region (XML) (De)serialization

        /// <summary>Initializes a new instance of stream size based on the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private StreamSize(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = info.GetInt64("Value");
        }

        /// <summary>Adds the underlying property of stream size to the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            info.AddValue("Value", m_Value);
        }

        /// <summary>Gets the <see href="XmlSchema"/> to (de) XML serialize a stream size.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <summary>Reads the stream size from an <see href="XmlReader"/>.</summary>
        /// <remarks>
        /// Uses the string parse function of stream size.
        /// </remarks>
        /// <param name="reader">An XML reader.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Guard.NotNull(writer, nameof(writer));
            var s = reader.ReadElementString();
            var val = Parse(s, CultureInfo.InvariantCulture);
            m_Value = val.m_Value;
        }

        /// <summary>Writes the stream size to an <see href="XmlWriter"/>.</summary>
        /// <remarks>
        /// Uses the string representation of stream size.
        /// </remarks>
        /// <param name="writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            Guard.NotNull(writer, nameof(writer));
            writer.WriteString(ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        #region (JSON) (De)serialization

        /// <summary>Generates a stream size from a JSON null object representation.</summary>
        void IJsonSerializable.FromJson()
        {
            m_Value = default(long);
        }

        /// <summary>Generates a stream size from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the stream size.
        /// </param>
        void IJsonSerializable.FromJson(string jsonString)
        {
            m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;
        }

        /// <summary>Generates a stream size from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the stream size.
        /// </param>
        void IJsonSerializable.FromJson(long jsonInteger)
        {
            m_Value = new StreamSize(jsonInteger).m_Value;
        }

        /// <summary>Generates a stream size from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the stream size.
        /// </param>
        void IJsonSerializable.FromJson(double jsonNumber)
        {
            m_Value = new StreamSize((Int64)jsonNumber).m_Value;
        }

        /// <summary>Generates a stream size from a JSON date representation.</summary>
        /// <param name="jsonDate">
        /// The JSON Date that represents the stream size.
        /// </param>
        void IJsonSerializable.FromJson(DateTime jsonDate) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported);

        /// <summary>Converts a stream size into its JSON object representation.</summary>
        object IJsonSerializable.ToJson()
        {
            return m_Value;
        }

        #endregion

        #region IFormattable / ToString

        /// <summary>Returns a <see cref="string"/> that represents the current stream size for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Debugger.")]
        private string DebuggerDisplay { get { return ToString(" F", CultureInfo.InvariantCulture); } }

        /// <summary>Returns a <see cref="string"/> that represents the current stream size.</summary>
        public override string ToString()
        {
            return ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current stream size.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current stream size.</summary>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider formatProvider)
        {
            return ToString("0 byte", formatProvider);
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current stream size.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        /// <remarks>
        /// There are basically two ways to format the stream size. The first one is
        /// automatic. Based on the size the extension is chosen (byte, kB, MB, GB, ect.).
        /// This can be specified by a s/S (short notation) and a f/F (full notation).
        /// 
        /// The other option is to specify the extension explicitly. So Megabyte,
        /// kB, ect. No extension is also possible.
        /// 
        /// Short notation:
        /// 8900.ToString("s") => 8900b
        /// 238900.ToString("s") => 238.9kb
        /// 238900.ToString(" S") => 238.9 kB
        /// 238900.ToString("0000.00 S") => 0238.90 kB
        ///
        /// Full notation:
        /// 8900.ToString("0.0 f") => 8900.0 byte
        /// 238900.ToString("0 f") => 234 kilobyte
        /// 1238900.ToString("0.00 F") => 1.24 Megabyte
        /// 
        /// Custom:
        /// 8900.ToString("0.0 kb") => 8.9 kb
        /// 238900.ToString("0.0 MB") => 0.2 MB
        /// 1238900.ToString("#,##0.00 Kilobyte") => 1,239.00 Kilobyte
        /// 1238900.ToString("#,##0") => 1,238,900
        /// </remarks>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
            {
                return formatted;
            }

            var match = FormattedPattern.Match(format ?? string.Empty);
            if (match.Success)
            {
                return ToFormattedString(formatProvider, match);
            }

            var streamSizeMarker = GetStreamSizeMarker(format);
            var decimalFormat = GetWithoutStreamSizeMarker(format, streamSizeMarker);
            var mp = GetMultiplier(streamSizeMarker);

            decimal size = (decimal)m_Value / (decimal)mp;

            return size.ToString(decimalFormat, formatProvider) + streamSizeMarker;
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase",
            Justification = "This is not about normalization but formatting.")]
        private string ToFormattedString(IFormatProvider formatProvider, Match match)
        {
            var format = match.Groups["format"].Value;
            var streamSizeMarker = match.Groups["streamSizeMarker"].Value;

            var isKibi = streamSizeMarker.Contains("i");

            var sb = new StringBuilder();
            if (m_Value < 0)
            {
                var cultureInfo = formatProvider as CultureInfo;
                var sign = cultureInfo == null ? "-" : cultureInfo.NumberFormat.NegativeSign;
                sb.Append(sign);
            }

            decimal size = Math.Abs(m_Value);
            var order = 0;
            if (size > 9999)
            {
                if (string.IsNullOrEmpty(format)) { format = "0.0"; }

                // Rounding would potential lead to 1000.
                while (size >= 999.5m)
                {
                    order++;
                    size /= isKibi ? 1024 : 1000;
                }
            }

            sb.Append(size.ToString(format, formatProvider));

            if (streamSizeMarker[0] == ' ')
            {
                sb.Append(' ');
                streamSizeMarker = streamSizeMarker.Substring(1);
            }

            switch (streamSizeMarker)
            {
                case "s": sb.Append(ShortLabels[order].ToLowerInvariant()); break;
                case "S": sb.Append(ShortLabels[order]); break;
                case "f": sb.Append(FullLabels[order].ToLowerInvariant()); break;
                case "F": sb.Append(FullLabels[order]); break;

                case "si": sb.Append(ShortLabels1024[order].ToLowerInvariant()); break;
                case "Si": sb.Append(ShortLabels1024[order]); break;
                case "fi": sb.Append(FullLabels1024[order].ToLowerInvariant()); break;
                case "Fi": sb.Append(FullLabels1024[order]); break;
            }
            return sb.ToString();
        }

        private static readonly Regex FormattedPattern = new Regex("^(?<format>.*)(?<streamSizeMarker> ?[sSfF]i?)$", RegexOptions.Compiled | RegexOptions.RightToLeft);
        private static readonly string[] ShortLabels = { "B", "kB", "MB", "GB", "TB", "PB", "EB" };
        private static readonly string[] FullLabels = { "byte", "kilobyte", "Megabyte", "Gigabyte", "Terabyte", "Petabyte", "Exabyte" };

        private static readonly string[] ShortLabels1024 = { "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB" };
        private static readonly string[] FullLabels1024 = { "byte", "kibibyte", "Mebibyte", "Gibibyte", "Tebibyte", "Pebibyte", "Exbibyte" };

        #endregion

        #region IEquatable

        /// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
        /// <param name="obj">An object to compare with.</param>
        public override bool Equals(object obj) { return obj is StreamSize && Equals((StreamSize)obj); }

        /// <summary>Returns true if this instance and the other <see cref="StreamSize"/> are equal, otherwise false.</summary>
        /// <param name="other">The <see cref="StreamSize"/> to compare with.</param>
        public bool Equals(StreamSize other) => m_Value == other.m_Value;

        /// <summary>Returns the hash code for this stream size.</summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => m_Value.GetHashCode();

        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator ==(StreamSize left, StreamSize right)
        {
            return left.Equals(right);
        }

        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator !=(StreamSize left, StreamSize right)
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
        /// An object that evaluates to a stream size.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.Value
        /// Condition Less than zero This instance precedes value. Zero This instance
        /// has the same position in the sort order as value. Greater than zero This
        /// instance follows value.-or- value is null.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// value is not a stream size.
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj is StreamSize)
            {
                return CompareTo((StreamSize)obj);
            }
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.ArgumentException_Must, "a stream size"), "obj");
        }

        /// <summary>Compares this instance with a specified stream size and indicates
        /// whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified stream size.
        /// </summary>
        /// <param name="other">
        /// The stream size to compare with this instance.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(StreamSize other) => m_Value.CompareTo(other.m_Value);


        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(StreamSize l, StreamSize r) => l.CompareTo(r) < 0;

        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator >(StreamSize l, StreamSize r) => l.CompareTo(r) > 0;

        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(StreamSize l, StreamSize r) => l.CompareTo(r) <= 0;

        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(StreamSize l, StreamSize r) => l.CompareTo(r) >= 0;

        #endregion

        #region (Explicit) casting

        /// <summary>Casts a stream size to a <see cref="string"/>.</summary>
        public static explicit operator string(StreamSize val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a stream size.</summary>
        public static explicit operator StreamSize(string str) { return StreamSize.Parse(str, CultureInfo.CurrentCulture); }


        /// <summary>Casts a stream size to a System.Int32.</summary>
        public static explicit operator Int32(StreamSize val) { return (Int32)val.m_Value; }
        /// <summary>Casts an System.Int32 to a stream size.</summary>
        public static implicit operator StreamSize(Int32 val) { return new StreamSize(val); }

        /// <summary>Casts a stream size to a System.Int64.</summary>
        public static explicit operator Int64(StreamSize val) { return (Int64)val.m_Value; }
        /// <summary>Casts a System.Int64 to a stream size.</summary>
        public static implicit operator StreamSize(Int64 val) { return new StreamSize(val); }

        /// <summary>Casts a stream size to a System.Double.</summary>
        public static explicit operator Double(StreamSize val) => (double)val.m_Value;
        /// <summary>Casts a System.Double to a stream size.</summary>
        public static explicit operator StreamSize(Double val) { return new StreamSize((Int64)val); }

        /// <summary>Casts a stream size to a System.Decimal.</summary>
        public static explicit operator Decimal(StreamSize val) { return (Decimal)val.m_Value; }
        /// <summary>Casts a System.DoubleDecimal to a stream size.</summary>
        public static explicit operator StreamSize(Decimal val) { return new StreamSize((Int64)val); }

        #endregion

        #region Factory methods

        /// <summary>Converts the string to a stream size.</summary>
        /// <param name="s">
        /// A string containing a stream size to convert.
        /// </param>
        /// <returns>
        /// A stream size.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static StreamSize Parse(string s)
        {
            return Parse(s, CultureInfo.CurrentCulture);
        }

        /// <summary>Converts the string to a stream size.</summary>
        /// <param name="s">
        /// A string containing a stream size to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// A stream size.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static StreamSize Parse(string s, IFormatProvider formatProvider)
        {
            StreamSize val;
            if (StreamSize.TryParse(s, formatProvider, out val))
            {
                return val;
            }
            throw new FormatException(QowaivMessages.FormatExceptionStreamSize);
        }

        /// <summary>Converts the string to a stream size.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a stream size to convert.
        /// </param>
        /// <returns>
        /// The stream size if the string was converted successfully, otherwise StreamSize.Empty.
        /// </returns>
        public static StreamSize TryParse(string s)
        {
            StreamSize val;
            if (StreamSize.TryParse(s, out val))
            {
                return val;
            }
            return StreamSize.Zero;
        }

        /// <summary>Converts the string to a stream size.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a stream size to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out StreamSize result)
        {
            return TryParse(s, CultureInfo.CurrentCulture, out result);
        }

        /// <summary>Converts the string to a stream size.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a stream size to convert.
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
        public static bool TryParse(string s, IFormatProvider formatProvider, out StreamSize result)
        {
            result = StreamSize.Zero;
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            var streamSizeMarker = GetStreamSizeMarker(s);
            var size = GetWithoutStreamSizeMarker(s, streamSizeMarker);
            var factor = GetMultiplier(streamSizeMarker);

            Int64 sizeInt64;

            if (Int64.TryParse(size, NumberStyles.Number, formatProvider, out sizeInt64) &&
                sizeInt64 <= Int64.MaxValue / factor &&
                sizeInt64 >= Int64.MinValue / factor)
            {
                result = new StreamSize(sizeInt64 * factor);
                return true;
            }

            Decimal sizeDecimal;

            if (Decimal.TryParse(size, NumberStyles.Number, formatProvider, out sizeDecimal))
            {
                if (sizeDecimal <= Decimal.MaxValue / factor &&
                    sizeDecimal >= Decimal.MinValue / factor)
                {
                    sizeDecimal *= factor;

                    if (sizeDecimal <= Int64.MaxValue && sizeDecimal >= Int64.MinValue)
                    {
                        result = new StreamSize((Int64)sizeDecimal);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>Creates a stream size based on the size in kilobytes.</summary>
        /// <param name="size">
        /// The size in kilobytes.
        /// </param>
        public static StreamSize FromKilobytes(double size) { return KB * size; }

        /// <summary>Creates a stream size based on the size in megabytes.</summary>
        /// <param name="size">
        /// The size in megabytes.
        /// </param>
        public static StreamSize FromMegabytes(double size) { return MB * size; }

        /// <summary>Creates a stream size based on the size in gigabytes.</summary>
        /// <param name="size">
        /// The size in gigabytes.
        /// </param>
        public static StreamSize FromGigabytes(double size) { return GB * size; }

        /// <summary>Creates a stream size based on the size in terabytes.</summary>
        /// <param name="size">
        /// The size in terabytes.
        /// </param>
        public static StreamSize FromTerabytes(double size) { return TB * size; }


        /// <summary>Creates a stream size based on the size in kibibytes.</summary>
        /// <param name="size">
        /// The size in kilobytes.
        /// </param>
        public static StreamSize FromKibibytes(double size) { return KiB * size; }

        /// <summary>Creates a stream size based on the size in mebibytes.</summary>
        /// <param name="size">
        /// The size in megabytes.
        /// </param>
        public static StreamSize FromMebibytes(double size) { return MiB * size; }

        /// <summary>Creates a stream size based on the size in gigabytes.</summary>
        /// <param name="size">
        /// The size in gigabytes.
        /// </param>
        public static StreamSize FromGibibytes(double size) { return GiB * size; }

        /// <summary>Creates a stream size based on the size in tebibytes.</summary>
        /// <param name="size">
        /// The size in terabytes.
        /// </param>
        public static StreamSize FromTebibytes(double size) { return TiB * size; }


        /// <summary>Creates a stream size from a file info.</summary>
        public static StreamSize FromByteArray(byte[] bytes)
        {
            Guard.NotNull(bytes, nameof(bytes));
            return new StreamSize(bytes.Length);
        }

        /// <summary>Creates a stream size from a file info.</summary>
        public static StreamSize FromFileInfo(FileInfo fileInfo)
        {
            Guard.NotNull(fileInfo, nameof(fileInfo));
            return new StreamSize(fileInfo.Length);
        }

        /// <summary>Creates a stream size from a stream.</summary>
        public static StreamSize FromStream(Stream stream)
        {
            Guard.NotNull(stream, nameof(stream));
            return new StreamSize(stream.Length);
        }

        #endregion

        #region Validation

        /// <summary>Returns true if the val represents a valid stream size, otherwise false.</summary>
        public static bool IsValid(string val)
        {
            return IsValid(val, CultureInfo.CurrentCulture);
        }

        /// <summary>Returns true if the val represents a valid stream size, otherwise false.</summary>
        public static bool IsValid(string val, IFormatProvider formatProvider)
        {
            StreamSize size;
            return TryParse(val, formatProvider, out size);
        }

        #endregion

        private static string GetStreamSizeMarker(string input)
        {
            if (string.IsNullOrEmpty(input)) { return string.Empty; }

            var length = input.Length;

            foreach (var marker in MultiplierLookup.Keys)
            {
                if (input.ToUpperInvariant().EndsWith(' ' + marker, StringComparison.Ordinal))
                {
                    return input.Substring(length - marker.Length - 1);
                }
                if (input.ToUpperInvariant().EndsWith(marker, StringComparison.Ordinal))
                {
                    return input.Substring(length - marker.Length);
                }
            }
            return string.Empty;
        }
        private static string GetWithoutStreamSizeMarker(string input, string streamSizeMarker)
        {
            if (string.IsNullOrEmpty(streamSizeMarker)) { return input; }
            return input.Substring(0, input.Length - streamSizeMarker.Length);
        }
        private static long GetMultiplier(string streamSizeMarker)
        {
            if (string.IsNullOrEmpty(streamSizeMarker)) { return 1; }
            return MultiplierLookup[streamSizeMarker.ToUpperInvariant().Trim()];
        }

        private static readonly Dictionary<string, long> MultiplierLookup = new Dictionary<string, long>()
        {
            { "KILOBYTE", 1000L },
            { "MEGABYTE", 1000000L },
            { "GIGABYTE", 1000000000L },
            { "TERABYTE", 1000000000000L },
            { "PETABYTE", 1000000000000000L },
            { "EXABYTE",  1000000000000000000L },

            { "KB", 1000L },
            { "MB", 1000000L },
            { "GB", 1000000000L },
            { "TB", 1000000000000L },
            { "PB", 1000000000000000L },
            { "EB", 1000000000000000000L },

            { "KIBIBYTE", 1L << 10 },
            { "MEBIBYTE", 1L << 20 },
            { "GIBIBYTE", 1L << 30 },
            { "TEBIBYTE", 1L << 40 },
            { "PEBIBYTE", 1L << 50 },
            { "EXBIBYTE", 1L << 60 },


            { "KIB", 1L << 10 },
            { "MIB", 1L << 20 },
            { "GIB", 1L << 30 },
            { "TIB", 1L << 40 },
            { "PIB", 1L << 50 },
            { "EIB", 1L << 60 },

            { "BYTE", 1 },
            { "B", 1 },
        };
    }
}
