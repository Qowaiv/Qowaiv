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
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
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
    [Serializable, SingleValueObject(SingleValueStaticOptions.Continuous, typeof(long))]
    [OpenApiDataType(description: "Stream size notation (in byte).", type: "integer", format: "stream-size")]
    [TypeConverter(typeof(StreamSizeTypeConverter))]
    public partial struct StreamSize : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<StreamSize>, IComparable, IComparable<StreamSize>
    {
        /// <summary>Represents an empty/not set stream size.</summary>
        public static readonly StreamSize Zero;

        /// <summary>Represents 1 Byte.</summary>
        public static readonly StreamSize Byte = new StreamSize(1L);

        /// <summary>Represents 1 kilobyte (1,000 byte).</summary>
        public static readonly StreamSize KB = new StreamSize(1_000L);

        /// <summary>Represents 1 Megabyte (1,000,000 byte).</summary>
        public static readonly StreamSize MB = new StreamSize(1_000_000L);

        /// <summary>Represents 1 Gigabyte (1,000,000,000 byte).</summary>
        public static readonly StreamSize GB = new StreamSize(1_000_000_000L);

        /// <summary>Represents 1 Terabyte (1,000,000,000,000 byte).</summary>
        public static readonly StreamSize TB = new StreamSize(1_000_000_000_000L);

        /// <summary>Represents 1 Petabyte (1,000,000,000,000,000 byte).</summary>
        public static readonly StreamSize PB = new StreamSize(1_000_000_000_000_000L);


        /// <summary>Represents 1 kibibyte (1,024 byte).</summary>
        public static readonly StreamSize KiB = new StreamSize(1L << 10);

        /// <summary>Represents 1 Mebibyte (1,048,576 byte).</summary>
        public static readonly StreamSize MiB = new StreamSize(1L << 20);

        /// <summary>Represents 1 Gibibyte (1,073,741,824 byte).</summary>
        public static readonly StreamSize GiB = new StreamSize(1L << 30);

        /// <summary>Represents 1 Tebibyte (1,099,511,627,776 byte).</summary>
        public static readonly StreamSize TiB = new StreamSize(1L << 40);

        /// <summary>Represents 1 Petabyte (1,125,899,906,842,624 byte).</summary>
        public static readonly StreamSize PiB = new StreamSize(1L << 50);

        /// <summary>Represents the minimum stream size that can be represented.</summary>
        public static readonly StreamSize MinValue = new StreamSize(long.MinValue);

        /// <summary>Represents the maximum stream size that can be represented.</summary>
        public static readonly StreamSize MaxValue = new StreamSize(long.MaxValue);

        /// <summary>Initializes a new instance of a stream size.</summary>
        /// <param name="size">
        /// The number of bytes.
        /// </param>
        public StreamSize(long size) => m_Value = size;

        /// <summary>The inner value of the stream size.</summary>
        private long m_Value;

        #region StreamSize manipulation

        /// <summary>Returns the absolute value of stream size.</summary>
        public StreamSize Abs() => Math.Abs(m_Value);

        /// <summary>Increases the stream size with one byte.</summary>
        internal StreamSize Increment() => Add(Byte);

        /// <summary>Decreases the stream size with one percent.</summary>
        internal StreamSize Decrement() => Subtract(Byte);

        /// <summary>Pluses the stream size.</summary>
        internal StreamSize Plus() => +m_Value;

        /// <summary>Negates the stream size.</summary>
        internal StreamSize Negate() => -m_Value;

        /// <summary>Adds a stream size to the current stream size.</summary>
        /// <param name="streamSize">
        /// The stream size to add.
        /// </param>
        public StreamSize Add(StreamSize streamSize) => m_Value + streamSize.m_Value;

        /// <summary>Adds the specified percentage to the stream size.</summary>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        public StreamSize Add(Percentage p) => m_Value.Add(p);

        /// <summary>Subtracts a stream size from the current stream size.</summary>
        /// <param name="streamSize">
        /// The stream size to Subtract.
        /// </param>
        public StreamSize Subtract(StreamSize streamSize) => m_Value - streamSize.m_Value;

        /// <summary>AddsSubtract the specified percentage from the stream size.</summary>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        public StreamSize Subtract(Percentage p) => m_Value.Subtract(p);

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

        /// <summary>Increases the stream size with one byte.</summary>
        public static StreamSize operator ++(StreamSize streamSize) => streamSize.Increment();
        /// <summary>Decreases the stream size with one byte.</summary>
        public static StreamSize operator --(StreamSize streamSize) => streamSize.Decrement();

        /// <summary>Unitary plusses the stream size.</summary>
        public static StreamSize operator +(StreamSize streamSize) { return streamSize.Plus(); }
        /// <summary>Negates the stream size.</summary>
        public static StreamSize operator -(StreamSize streamSize) { return streamSize.Negate(); }

        /// <summary>Adds the left and the right stream size.</summary>
        public static StreamSize operator +(StreamSize l, StreamSize r) => l.Add(r);
        /// <summary>Subtracts the right from the left stream size.</summary>
        public static StreamSize operator -(StreamSize l, StreamSize r) => l.Subtract(r);

        /// <summary>Adds the percentage to the stream size.</summary>
        public static StreamSize operator +(StreamSize streamSize, Percentage p) => streamSize.Add(p);
        /// <summary>Subtracts the percentage from the stream size.</summary>
        public static StreamSize operator -(StreamSize streamSize, Percentage p) => streamSize.Subtract(p);

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

        private void FromJson(object json)
        {
            if (json is double dec)
            {
                m_Value = (long)dec;
            }
            else if (json is long num)
            {
                m_Value = num;
            }
            else
            {
                m_Value = Parse(Parsing.ToInvariant(json), CultureInfo.InvariantCulture).m_Value;
            }
        }

        /// <inheritdoc />
        object IJsonSerializable.ToJson() => m_Value;

        /// <summary>Returns a <see cref="string"/> that represents the current stream size for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => ToString(" F", CultureInfo.InvariantCulture);

        /// <summary>Returns a <see cref="string"/> that represents the current stream size.</summary>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current stream size.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current stream size.</summary>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider formatProvider) => ToString("0 byte", formatProvider);

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


        /// <summary>Gets an XML string representation of the stream size.</summary>
        private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

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


        /// <summary>Casts a stream size to a <see cref="string"/>.</summary>
        public static explicit operator string(StreamSize val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a stream size.</summary>
        public static explicit operator StreamSize(string str) => Parse(str, CultureInfo.CurrentCulture);


        /// <summary>Casts a stream size to a System.Int32.</summary>
        public static explicit operator int(StreamSize val) => (int)val.m_Value;
        /// <summary>Casts an System.Int32 to a stream size.</summary>
        public static implicit operator StreamSize(int val) => new StreamSize(val);

        /// <summary>Casts a stream size to a System.Int64.</summary>
        public static explicit operator long(StreamSize val) => val.m_Value;
        /// <summary>Casts a System.Int64 to a stream size.</summary>
        public static implicit operator StreamSize(long val) => new StreamSize(val);

        /// <summary>Casts a stream size to a System.Double.</summary>
        public static explicit operator double(StreamSize val) => val.m_Value;
        /// <summary>Casts a System.Double to a stream size.</summary>
        public static explicit operator StreamSize(double val) => new StreamSize((long)val);

        /// <summary>Casts a stream size to a System.Decimal.</summary>
        public static explicit operator decimal(StreamSize val) => val.m_Value;
        /// <summary>Casts a System.DoubleDecimal to a stream size.</summary>
        public static explicit operator StreamSize(decimal val) => new StreamSize((long)val);

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
            result = default;
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            var streamSizeMarker = GetStreamSizeMarker(s);
            var size = GetWithoutStreamSizeMarker(s, streamSizeMarker);
            var factor = GetMultiplier(streamSizeMarker);

            if (long.TryParse(size, NumberStyles.Number, formatProvider, out long sizeInt64) &&
                sizeInt64 <= long.MaxValue / factor &&
                sizeInt64 >= long.MinValue / factor)
            {
                result = new StreamSize(sizeInt64 * factor);
                return true;
            }

            if (decimal.TryParse(size, NumberStyles.Number, formatProvider, out decimal sizeDecimal) &&
                sizeDecimal <= decimal.MaxValue / factor &&
                sizeDecimal >= decimal.MinValue / factor)
            {
                sizeDecimal *= factor;

                if (sizeDecimal <= long.MaxValue && sizeDecimal >= long.MinValue)
                {
                    result = new StreamSize((long)sizeDecimal);
                    return true;
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
        private static readonly Dictionary<string, long> MultiplierLookup = new Dictionary<string, long>
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
