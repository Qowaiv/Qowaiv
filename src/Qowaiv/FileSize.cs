using Qowaiv.Conversion;
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
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv
{
	/// <summary>Represents a file size.</summary>
	/// <remarks>
	/// A file size measures the size of a computer file. Typically it is measured
	/// in bytes with an SI prefix. The actual amount of disk space consumed by
	/// the file depends on the file system. The maximum file size a file system
	/// supports depends on the number of bits reserved to store size information
	/// and the total size of the file system. This value type can not represent
	/// file sizes bigger than Int64.MaxValue.
	/// </remarks>
	[DebuggerDisplay("{DebuggerDisplay}")]
	[Serializable, SingleValueObject(SingleValueStaticOptions.Continuous, typeof(Int64))]
	[TypeConverter(typeof(FileSizeTypeConverter))]
	public struct FileSize : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IComparable, IComparable<FileSize>
	{
		/// <summary>Represents an empty/not set file size.</summary>
		public static readonly FileSize Zero = new FileSize() { m_Value = 0L };

		/// <summary>Represents 1 Byte.</summary>
		public static readonly FileSize Byte = new FileSize() { m_Value = 1L };

		/// <summary>Represents 1 kilobyte (1,024 byte).</summary>
		public static readonly FileSize KB = new FileSize() { m_Value = 1L << 10 };

		/// <summary>Represents 1 Megabyte (1,048,576 byte).</summary>
		public static readonly FileSize MB = new FileSize() { m_Value = 1L << 20 };

		/// <summary>Represents 1 Gigabyte (1,073,741,824 byte).</summary>
		public static readonly FileSize GB = new FileSize() { m_Value = 1L << 30 };

		/// <summary>Represents 1 Terabyte (1,099,511,627,776 byte).</summary>
		public static readonly FileSize TB = new FileSize() { m_Value = 1L << 40 };

		/// <summary>Represents 1 Petabyte (1,125,899,906,842,624 byte).</summary>
		public static readonly FileSize PB = new FileSize() { m_Value = 1L << 50 };

		/// <summary>Represents the minimum file size that can be represented.</summary>
		public static readonly FileSize MinValue = new FileSize() { m_Value = Int64.MinValue };

		/// <summary>Represents the maximum file size that can be represented.</summary>
		public static readonly FileSize MaxValue = new FileSize() { m_Value = Int64.MaxValue };

		/// <summary>Initializes a new instance of a file size.</summary>
		/// <param name="size">
		/// The number of bytes.
		/// </param>
		public FileSize(Int64 size)
		{
			m_Value = size;
		}

		#region Properties

		/// <summary>The inner value of the file size.</summary>
		private Int64 m_Value;

		#endregion

		#region FileSize manipulation

		/// <summary>Increases the file size with one percent.</summary>
		public FileSize Increment()
		{
			return this.Add(FileSize.Byte);
		}
		/// <summary>Decreases the file size with one percent.</summary>
		public FileSize Decrement()
		{
			return this.Subtract(FileSize.Byte);
		}

		/// <summary>Pluses the file size.</summary>
		public FileSize Plus()
		{
			return new FileSize(+m_Value);
		}
		/// <summary>Negates the file size.</summary>
		public FileSize Negate()
		{
			return new FileSize(-m_Value);
		}

		/// <summary>Adds a file size to the current file size.</summary>
		/// <param name="fileSize">
		/// The file size to add.
		/// </param>
		public FileSize Add(FileSize fileSize) { return m_Value + fileSize.m_Value; }

		/// <summary>Adds the specified percentage to the file size.</summary>
		/// <param name="p">
		/// The percentage to add.
		/// </param>
		public FileSize Add(Percentage p) { return m_Value.Add(p); }

		/// <summary>Subtracts a file size from the current file size.</summary>
		/// <param name="fileSize">
		/// The file size to Subtract.
		/// </param>
		public FileSize Subtract(FileSize fileSize) { return m_Value - fileSize.m_Value; }

		/// <summary>AddsSubtract the specified percentage from the file size.</summary>
		/// <param name="p">
		/// The percentage to add.
		/// </param>
		public FileSize Subtract(Percentage p) { return m_Value.Subtract(p); }

		#region Multiply

		/// <summary>Multiplies the file size with a specified factor.</summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public FileSize Multiply(Decimal factor) { return (FileSize)(m_Value * factor); }

		/// <summary>Multiplies the file size with a specified factor.</summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public FileSize Multiply(Double factor) { return Multiply((Decimal)factor); }

		/// <summary>Multiplies the file size with a specified factor.</summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public FileSize Multiply(Single factor) { return Multiply((Decimal)factor); }

		/// <summary>Multiplies the file size with a specified factor.</summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public FileSize Multiply(Percentage factor) { return Multiply((Decimal)factor); }


		/// <summary>Multiplies the file size with a specified factor.</summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public FileSize Multiply(Int64 factor) { return Multiply((Decimal)factor); }

		/// <summary>Multiplies the file size with a specified factor.</summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public FileSize Multiply(Int32 factor) { return Multiply((Decimal)factor); }

		/// <summary>Multiplies the file size with a specified factor.</summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public FileSize Multiply(Int16 factor) { return Multiply((Decimal)factor); }


		/// <summary>Multiplies the file size with a specified factor.</summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		[CLSCompliant(false)]
		public FileSize Multiply(UInt64 factor) { return Multiply((Decimal)factor); }

		/// <summary>Multiplies the file size with a specified factor.</summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		[CLSCompliant(false)]
		public FileSize Multiply(UInt32 factor) { return Multiply((Decimal)factor); }

		/// <summary>Multiplies the file size with a specified factor.</summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		[CLSCompliant(false)]
		public FileSize Multiply(UInt16 factor) { return Multiply((Decimal)factor); }

		#endregion

		#region Divide

		/// <summary>Divide the file size by a specified factor.</summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public FileSize Divide(Decimal factor) { return (FileSize)(m_Value / factor); }

		/// <summary>Divide the file size by a specified factor.</summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public FileSize Divide(Double factor) { return Divide((Decimal)factor); }

		/// <summary>Divide the file size by a specified factor.</summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public FileSize Divide(Single factor) { return Divide((Decimal)factor); }

		/// <summary>Divide the file size by a specified factor.</summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public FileSize Divide(Percentage factor) { return Divide((Decimal)factor); }


		/// <summary>Divide the file size by a specified factor.</summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public FileSize Divide(Int64 factor) { return Divide((Decimal)factor); }

		/// <summary>Divide the file size by a specified factor.</summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public FileSize Divide(Int32 factor) { return Divide((Decimal)factor); }

		/// <summary>Divide the file size by a specified factor.</summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public FileSize Divide(Int16 factor) { return Divide((Decimal)factor); }


		/// <summary>Divide the file size by a specified factor.</summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		[CLSCompliant(false)]
		public FileSize Divide(UInt64 factor) { return Divide((Decimal)factor); }

		/// <summary>Divide the file size by a specified factor.</summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		[CLSCompliant(false)]
		public FileSize Divide(UInt32 factor) { return Divide((Decimal)factor); }

		/// <summary>Divide the file size by a specified factor.</summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		[CLSCompliant(false)]
		public FileSize Divide(UInt16 factor) { return Divide((Decimal)factor); }

		#endregion

		/// <summary>Increases the file size with one percent.</summary>
		public static FileSize operator ++(FileSize fileSize) { return fileSize.Increment(); }
		/// <summary>Decreases the file size with one percent.</summary>
		public static FileSize operator --(FileSize fileSize) { return fileSize.Decrement(); }

		/// <summary>Unitary plusses the file size.</summary>
		public static FileSize operator +(FileSize fileSize) { return fileSize.Plus(); }
		/// <summary>Negates the file size.</summary>
		public static FileSize operator -(FileSize fileSize) { return fileSize.Negate(); }

		/// <summary>Adds the left and the right file size.</summary>
		public static FileSize operator +(FileSize l, FileSize r) { return l.Add(r); }
		/// <summary>Subtracts the right from the left file size.</summary>
		public static FileSize operator -(FileSize l, FileSize r) { return l.Subtract(r); }

		/// <summary>Adds the percentage to the file size.</summary>
		public static FileSize operator +(FileSize fileSize, Percentage p) { return fileSize.Add(p); }
		/// <summary>Subtracts the percentage from the file size.</summary>
		public static FileSize operator -(FileSize fileSize, Percentage p) { return fileSize.Subtract(p); }

		/// <summary>Multiplies the file size with the factor.</summary>
		public static FileSize operator *(FileSize fileSize, Decimal factor) { return fileSize.Multiply(factor); }
		/// <summary>Multiplies the file size with the factor.</summary>
		public static FileSize operator *(FileSize fileSize, Double factor) { return fileSize.Multiply(factor); }
		/// <summary>Multiplies the file size with the factor.</summary>
		public static FileSize operator *(FileSize fileSize, Single factor) { return fileSize.Multiply(factor); }
		/// <summary>Multiplies the file size with the factor.</summary>
		public static FileSize operator *(FileSize fileSize, Percentage factor) { return fileSize.Multiply(factor); }

		/// <summary>Multiplies the file size with the factor.</summary>
		public static FileSize operator *(FileSize fileSize, Int64 factor) { return fileSize.Multiply(factor); }
		/// <summary>Multiplies the file size with the factor.</summary>
		public static FileSize operator *(FileSize fileSize, Int32 factor) { return fileSize.Multiply(factor); }
		/// <summary>Multiplies the file size with the factor.</summary>
		public static FileSize operator *(FileSize fileSize, Int16 factor) { return fileSize.Multiply(factor); }

		/// <summary>Multiplies the file size with the factor.</summary>
		[CLSCompliant(false)]
		public static FileSize operator *(FileSize fileSize, UInt64 factor) { return fileSize.Multiply(factor); }
		/// <summary>Multiplies the file size with the factor.</summary>
		[CLSCompliant(false)]
		public static FileSize operator *(FileSize fileSize, UInt32 factor) { return fileSize.Multiply(factor); }
		/// <summary>Multiplies the file size with the factor.</summary>
		[CLSCompliant(false)]
		public static FileSize operator *(FileSize fileSize, UInt16 factor) { return fileSize.Multiply(factor); }

		/// <summary>Divides the file size by the factor.</summary>
		public static FileSize operator /(FileSize fileSize, Decimal factor) { return fileSize.Divide(factor); }
		/// <summary>Divides the file size by the factor.</summary>
		public static FileSize operator /(FileSize fileSize, Double factor) { return fileSize.Divide(factor); }
		/// <summary>Divides the file size by the factor.</summary>
		public static FileSize operator /(FileSize fileSize, Single factor) { return fileSize.Divide(factor); }
		/// <summary>Divides the file size by the factor.</summary>
		public static FileSize operator /(FileSize fileSize, Percentage factor) { return fileSize.Divide(factor); }

		/// <summary>Divides the file size by the factor.</summary>
		public static FileSize operator /(FileSize fileSize, Int64 factor) { return fileSize.Divide(factor); }
		/// <summary>Divides the file size by the factor.</summary>
		public static FileSize operator /(FileSize fileSize, Int32 factor) { return fileSize.Divide(factor); }
		/// <summary>Divides the file size by the factor.</summary>
		public static FileSize operator /(FileSize fileSize, Int16 factor) { return fileSize.Divide(factor); }

		/// <summary>Divides the file size by the factor.</summary>
		[CLSCompliant(false)]
		public static FileSize operator /(FileSize fileSize, UInt64 factor) { return fileSize.Divide(factor); }
		/// <summary>Divides the file size by the factor.</summary>
		[CLSCompliant(false)]
		public static FileSize operator /(FileSize fileSize, UInt32 factor) { return fileSize.Divide(factor); }
		/// <summary>Divides the file size by the factor.</summary>
		[CLSCompliant(false)]
		public static FileSize operator /(FileSize fileSize, UInt16 factor) { return fileSize.Divide(factor); }

		#endregion

		#region (XML) (De)serialization

		/// <summary>Initializes a new instance of file size based on the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		private FileSize(SerializationInfo info, StreamingContext context)
		{
			Guard.NotNull(info, "info");
			m_Value = info.GetInt64("Value");
		}

		/// <summary>Adds the underlying property of file size to the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			Guard.NotNull(info, "info");
			info.AddValue("Value", m_Value);
		}

		/// <summary>Gets the xml schema to (de) xml serialize a file size.</summary>
		/// <remarks>
		/// Returns null as no schema is required.
		/// </remarks>
		XmlSchema IXmlSerializable.GetSchema() { return null; }

		/// <summary>Reads the file size from an xml writer.</summary>
		/// <remarks>
		/// Uses the string parse function of file size.
		/// </remarks>
		/// <param name="reader">An xml reader.</param>
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			Guard.NotNull(reader, "reader");
			var s = reader.ReadElementString();
			var val = Parse(s, CultureInfo.InvariantCulture);
			m_Value = val.m_Value;
		}

		/// <summary>Writes the file size to an xml writer.</summary>
		/// <remarks>
		/// Uses the string representation of file size.
		/// </remarks>
		/// <param name="writer">An xml writer.</param>
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			Guard.NotNull(writer, "writer");
			writer.WriteString(ToString(CultureInfo.InvariantCulture));
		}

		#endregion

		#region (JSON) (De)serialization

		/// <summary>Generates a file size from a JSON null object representation.</summary>
		void IJsonSerializable.FromJson()
		{
			m_Value = default(Int32);
		}

		/// <summary>Generates a file size from a JSON string representation.</summary>
		/// <param name="jsonString">
		/// The JSON string that represents the file size.
		/// </param>
		void IJsonSerializable.FromJson(String jsonString)
		{
			m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;
		}

		/// <summary>Generates a file size from a JSON integer representation.</summary>
		/// <param name="jsonInteger">
		/// The JSON integer that represents the file size.
		/// </param>
		void IJsonSerializable.FromJson(Int64 jsonInteger)
		{
			m_Value = new FileSize(jsonInteger).m_Value;
		}

		/// <summary>Generates a file size from a JSON number representation.</summary>
		/// <param name="jsonNumber">
		/// The JSON number that represents the file size.
		/// </param>
		void IJsonSerializable.FromJson(Double jsonNumber)
		{
			m_Value = new FileSize((Int64)jsonNumber).m_Value;
		}

		/// <summary>Generates a file size from a JSON date representation.</summary>
		/// <param name="jsonDate">
		/// The JSON Date that represents the file size.
		/// </param>
		void IJsonSerializable.FromJson(DateTime jsonDate) { throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported); }

		/// <summary>Converts a file size into its JSON object representation.</summary>
		object IJsonSerializable.ToJson()
		{
			return m_Value;
		}

		#endregion

		#region IFormattable / ToString

		/// <summary>Returns a System.String that represents the current file size for debug purposes.</summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Debugger.")]
		private string DebuggerDisplay
		{
			get { return ToString(" F", CultureInfo.InvariantCulture); }
		}

		/// <summary>Returns a System.String that represents the current file size.</summary>
		public override string ToString()
		{
			return ToString(CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted System.String that represents the current file size.</summary>
		/// <param name="format">
		/// The format that this describes the formatting.
		/// </param>
		public string ToString(string format)
		{
			return ToString(format, CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted System.String that represents the current file size.</summary>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		public string ToString(IFormatProvider formatProvider)
		{
			return ToString("0 byte", formatProvider);
		}

		/// <summary>Returns a formatted System.String that represents the current file size.</summary>
		/// <param name="format">
		/// The format that this describes the formatting.
		/// </param>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		/// <remarks>
		/// There are basically two ways to format the file size. The first one is
		/// automatic. Based on the size the extension is chosen (byte, kB, MB, GB, ect.).
		/// This can be specified by a s/S (short notation) and a f/F (full notation).
		/// 
		/// The other option is to specify the extension explicitly. So Megabyte,
		/// kB, ect. No extension is also possible.
		/// 
		/// Short notation:
		/// 8900.ToString("s") => 8900b
		/// 238900.ToString("s") => 233.3kb
		/// 238900.ToString(" S") => 233.3 kB
		/// 238900.ToString("0000.00 S") => 0233.30 kB
		///
		/// Full notation:
		/// 8900.ToString("0.0 f") => 8900.0 byte
		/// 238900.ToString("0 f") => 233 kilobyte
		/// 1238900.ToString("0.00 F") => 1.18 Megabyte
		/// 
		/// Custom:
		/// 8900.ToString("0.0 kb") => 8.7 kb
		/// 238900.ToString("0.0 MB") => 0.2 MB
		/// 1238900.ToString("#,##0.00 Kilobyte") => 1,209.86 Kilobyte
		/// 1238900.ToString("#,##0") => 1,238,900
		/// </remarks>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			string formatted;
			if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out formatted))
			{
				return formatted;
			}

			var match = FormattedPattern.Match(format ?? String.Empty);
			if (match.Success)
			{
				return ToFormattedString(formatProvider, match);
			}

			var fileSizeMarker = GetFileSizeMarker(format);
			var decimalFormat = GetWithoutFileSizeMarker(format, fileSizeMarker);
			var shift = GetShift(fileSizeMarker);

			Decimal size = (Decimal)m_Value / (1L << shift);

			return size.ToString(decimalFormat, formatProvider) + fileSizeMarker;
		}

		private string ToFormattedString(IFormatProvider formatProvider, Match match)
		{
			var format = match.Groups["format"].Value;
			var fileSizeMarker = match.Groups["fileSizeMarker"].Value;

			Decimal size = m_Value;
			var order = 0;
			if (m_Value > 9999)
			{
				if (String.IsNullOrEmpty(format)) { format = "0.0"; }

				// Rounding would potential lead to 1000.
				while (size >= 999.5m)
				{
					order++;
					size /= 1024;
				}
			}
			var str = size.ToString(format, formatProvider);

			if (fileSizeMarker[0] == ' ')
			{
				str += ' ';
				fileSizeMarker = fileSizeMarker.Substring(1);
			}

			switch (fileSizeMarker)
			{
				case "s": str += ShortLabels[order].ToLowerInvariant(); break;
				case "S": str += ShortLabels[order]; break;
				case "f": str += FullLabels[order].ToLowerInvariant(); break;
				case "F": str += FullLabels[order]; break;
			}
			return str;
		}

		private static readonly Regex FormattedPattern = new Regex("^(?<format>.*)(?<fileSizeMarker> ?[sSfF])$", RegexOptions.Compiled | RegexOptions.RightToLeft);
		private static readonly string[] ShortLabels = { "B", "kB", "MB", "GB", "TB", "PB", "EB" };
		private static readonly string[] FullLabels = { "byte", "kilobyte", "Megabyte", "Gigabyte", "Terabyte", "Petabyte", "Exabyte" };

		#endregion

		#region IEquatable

		/// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
		/// <param name="obj">An object to compare with.</param>
		public override bool Equals(object obj) { return base.Equals(obj); }

		/// <summary>Returns the hash code for this file size.</summary>
		/// <returns>
		/// A 32-bit signed integer hash code.
		/// </returns>
		public override int GetHashCode() { return m_Value.GetHashCode(); }

		/// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator ==(FileSize left, FileSize right)
		{
			return left.Equals(right);
		}

		/// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator !=(FileSize left, FileSize right)
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
		/// An object that evaluates to a file size.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.Value
		/// Condition Less than zero This instance precedes value. Zero This instance
		/// has the same position in the sort order as value. Greater than zero This
		/// instance follows value.-or- value is null.
		/// </returns>
		/// <exception cref="System.ArgumentException">
		/// value is not a file size.
		/// </exception>
		public int CompareTo(object obj)
		{
			if (obj is FileSize)
			{
				return CompareTo((FileSize)obj);
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.AgrumentException_Must, "a file size"), "obj");
		}

		/// <summary>Compares this instance with a specified file size and indicates
		/// whether this instance precedes, follows, or appears in the same position
		/// in the sort order as the specified file size.
		/// </summary>
		/// <param name="other">
		/// The file size to compare with this instance.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.
		/// </returns>
		public int CompareTo(FileSize other) { return m_Value.CompareTo(other.m_Value); }


		/// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
		public static bool operator <(FileSize l, FileSize r) { return l.CompareTo(r) < 0; }

		/// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
		public static bool operator >(FileSize l, FileSize r) { return l.CompareTo(r) > 0; }

		/// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
		public static bool operator <=(FileSize l, FileSize r) { return l.CompareTo(r) <= 0; }

		/// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
		public static bool operator >=(FileSize l, FileSize r) { return l.CompareTo(r) >= 0; }

		#endregion

		#region (Explicit) casting

		/// <summary>Casts a file size to a System.String.</summary>
		public static explicit operator string(FileSize val) { return val.ToString(CultureInfo.CurrentCulture); }
		/// <summary>Casts a System.String to a file size.</summary>
		public static explicit operator FileSize(string str) { return FileSize.Parse(str, CultureInfo.CurrentCulture); }


		/// <summary>Casts a file size to a System.Int32.</summary>
		public static explicit operator Int32(FileSize val) { return (Int32)val.m_Value; }
		/// <summary>Casts an System.Int32 to a file size.</summary>
		public static implicit operator FileSize(Int32 val) { return new FileSize(val); }

		/// <summary>Casts a file size to a System.Int64.</summary>
		public static explicit operator Int64(FileSize val) { return (Int64)val.m_Value; }
		/// <summary>Casts a System.Int64 to a file size.</summary>
		public static implicit operator FileSize(Int64 val) { return new FileSize(val); }

		/// <summary>Casts a file size to a System.Double.</summary>
		public static explicit operator Double(FileSize val) { return (Double)val.m_Value; }
		/// <summary>Casts a System.Double to a file size.</summary>
		public static explicit operator FileSize(Double val) { return new FileSize((Int64)val); }

		/// <summary>Casts a file size to a System.Decimal.</summary>
		public static explicit operator Decimal(FileSize val) { return (Decimal)val.m_Value; }
		/// <summary>Casts a System.DoubleDecimal to a file size.</summary>
		public static explicit operator FileSize(Decimal val) { return new FileSize((Int64)val); }

		#endregion

		#region Factory methods

		/// <summary>Converts the string to a file size.</summary>
		/// <param name="s">
		/// A string containing a file size to convert.
		/// </param>
		/// <returns>
		/// A file size.
		/// </returns>
		/// <exception cref="System.FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static FileSize Parse(string s)
		{
			return Parse(s, CultureInfo.CurrentCulture);
		}

		/// <summary>Converts the string to a file size.</summary>
		/// <param name="s">
		/// A string containing a file size to convert.
		/// </param>
		/// <param name="formatProvider">
		/// The specified format provider.
		/// </param>
		/// <returns>
		/// A file size.
		/// </returns>
		/// <exception cref="System.FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static FileSize Parse(string s, IFormatProvider formatProvider)
		{
			FileSize val;
			if (FileSize.TryParse(s, formatProvider, out val))
			{
				return val;
			}
			throw new FormatException(QowaivMessages.FormatExceptionFileSize);
		}

		/// <summary>Converts the string to a file size.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a file size to convert.
		/// </param>
		/// <returns>
		/// The file size if the string was converted successfully, otherwise FileSize.Empty.
		/// </returns>
		public static FileSize TryParse(string s)
		{
			FileSize val;
			if (FileSize.TryParse(s, out val))
			{
				return val;
			}
			return FileSize.Zero;
		}

		/// <summary>Converts the string to a file size.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a file size to convert.
		/// </param>
		/// <param name="result">
		/// The result of the parsing.
		/// </param>
		/// <returns>
		/// True if the string was converted successfully, otherwise false.
		/// </returns>
		public static bool TryParse(string s, out FileSize result)
		{
			return TryParse(s, CultureInfo.CurrentCulture, out result);
		}

		/// <summary>Converts the string to a file size.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a file size to convert.
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
		public static bool TryParse(string s, IFormatProvider formatProvider, out FileSize result)
		{
			result = FileSize.Zero;
			if (string.IsNullOrEmpty(s))
			{
				return false;
			}

			var fileSizeMarker = GetFileSizeMarker(s);
			var size = GetWithoutFileSizeMarker(s, fileSizeMarker);
			var shift = GetShift(fileSizeMarker);

			Int64 sizeInt64;

			if (Int64.TryParse(size, NumberStyles.Number, formatProvider, out sizeInt64) &&
				sizeInt64 <= (Int64.MaxValue >> shift) &&
				sizeInt64 >= (Int64.MinValue >> shift))
			{
				result = new FileSize(sizeInt64 << shift);
				return true;
			}

			Decimal sizeDecimal;

			if (Decimal.TryParse(size, NumberStyles.Number, formatProvider, out sizeDecimal))
			{
				long factor = 1L << shift;
				if (sizeDecimal <= Decimal.MaxValue / factor &&
					sizeDecimal >= Decimal.MinValue / factor)
				{
					sizeDecimal *= factor;

					if (sizeDecimal <= Int64.MaxValue && sizeDecimal >= Int64.MinValue)
					{
						result = new FileSize((Int64)sizeDecimal);
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>Creates a file size from a file info.</summary>
		public static FileSize FromFileInfo(FileInfo fileInfo)
		{
			Guard.NotNull(fileInfo, "fileInfo");
			return new FileSize(fileInfo.Length);
		}

		/// <summary>Creates a file size from a stream.</summary>
		public static FileSize FromStream(Stream stream)
		{
			Guard.NotNull(stream, "stream");
			return new FileSize(stream.Length);
		}

		#endregion

		#region Validation

		/// <summary>Returns true if the val represents a valid file size, otherwise false.</summary>
		public static bool IsValid(string val)
		{
			return IsValid(val, CultureInfo.CurrentCulture);
		}

		/// <summary>Returns true if the val represents a valid file size, otherwise false.</summary>
		public static bool IsValid(string val, IFormatProvider formatProvider)
		{
			FileSize size;
			return TryParse(val, formatProvider, out size);
		}

		#endregion

		private static string GetFileSizeMarker(string input)
		{
			if (string.IsNullOrEmpty(input)) { return String.Empty; }

			var length = input.Length;

			foreach (var marker in ShiftLookup.Keys)
			{
				if (input.ToLowerInvariant().EndsWith(' ' + marker, StringComparison.Ordinal))
				{
					return input.Substring(length - marker.Length - 1);
				}
				if (input.ToLowerInvariant().EndsWith(marker, StringComparison.Ordinal))
				{
					return input.Substring(length - marker.Length);
				}
			}
			return String.Empty;
		}
		private static string GetWithoutFileSizeMarker(string input, string fileSizeMarker)
		{
			if (string.IsNullOrEmpty(fileSizeMarker)) { return input; }
			return input.Substring(0, input.Length - fileSizeMarker.Length);
		}
		private static int GetShift(string fileSizeMarker)
		{
			if (string.IsNullOrEmpty(fileSizeMarker)) { return 0; }
			return ShiftLookup[fileSizeMarker.ToLowerInvariant().Trim()];
		}

		private static readonly Dictionary<string, int> ShiftLookup = new Dictionary<string, int>()
		{
			{ "kilobyte", 10 },
			{ "megabyte", 20 },
			{ "gigabyte", 30 },
			{ "terabyte", 40 },
			{ "petabyte", 50 },
			{ "exabyte",  60 },

			{ "kb", 10 },
			{ "mb", 20 },
			{ "gb", 30 },
			{ "tb", 40 },
			{ "pb", 50 },
			{ "eb", 60 },

			{ "byte", 0 },
			{ "b", 0 },
		};

	}
}