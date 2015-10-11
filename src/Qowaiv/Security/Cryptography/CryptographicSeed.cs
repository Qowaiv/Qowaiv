using Qowaiv.Conversion.Security.Cryptography;
using Qowaiv.Formatting;
using Qowaiv.Json;
using Qowaiv.Text;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv.Security.Cryptography
{
	/// <summary>Represents a cryptographic seed.</summary>
	[DebuggerDisplay("{DebuggerDisplay}")]
	[SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "The < and > operators have no meaning for a cryptographic seed.")]
	[Serializable, SingleValueObject(SingleValueStaticOptions.AllExcludingCulture ^ SingleValueStaticOptions.HasUnknownValue, typeof(Byte[]))]
	[TypeConverter(typeof(CryptographicSeedTypeConverter))]
	public struct CryptographicSeed : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IComparable, IComparable<CryptographicSeed>, IEquatable<CryptographicSeed>
	{
		/// <summary>Represents an empty/not set cryptographic seed.</summary>
		public static readonly CryptographicSeed Empty = default(CryptographicSeed);

		#region Properties

		/// <summary>The inner value of the cryptographic seed.</summary>
		private Byte[] m_Value;

		/// <summary>Gets the length of the cryptographic seed.</summary>
		public int Length { get { return m_Value == null ? 0 : m_Value.Length; } }

		#endregion

		#region Methods

		/// <summary>Returns true if the cryptographic seed is empty, otherwise false.</summary>
		public bool IsEmpty() { return m_Value == default(System.Byte[]); }

		/// <summary>Returns a byte array that contains the value of this instance.</summary>
		public Byte[] ToByteArray()
		{
			var clone = new byte[Length];

			if (Length > 0)
			{
				Array.Copy(m_Value, clone, clone.Length);
			}
			return clone;
		}

		#endregion

		#region (XML) (De)serialization

		/// <summary>Initializes a new instance of cryptographic seed based on the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		private CryptographicSeed(SerializationInfo info, StreamingContext context)
		{
			Guard.NotNull(info, "info");
			m_Value = (Byte[])info.GetValue("Value", typeof(Byte[]));
		}

		/// <summary>Adds the underlying property of cryptographic seed to the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			Guard.NotNull(info, "info");
			info.AddValue("Value", m_Value);
		}

		/// <summary>Gets the xml schema to (de) xml serialize a cryptographic seed.</summary>
		/// <remarks>
		/// Returns null as no schema is required.
		/// </remarks>
		XmlSchema IXmlSerializable.GetSchema() { return null; }

		/// <summary>Reads the cryptographic seed from an xml writer.</summary>
		/// <remarks>
		/// Uses the string parse function of cryptographic seed.
		/// </remarks>
		/// <param name="reader">An xml reader.</param>
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			Guard.NotNull(reader, "reader");
			var s = reader.ReadElementString();
			var val = Parse(s);
			m_Value = val.m_Value;
		}

		/// <summary>Writes the cryptographic seed to an xml writer.</summary>
		/// <remarks>
		/// Uses the string representation of cryptographic seed.
		/// </remarks>
		/// <param name="writer">An xml writer.</param>
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			Guard.NotNull(writer, "writer");
			writer.WriteString(ToString());
		}

		#endregion

		#region (JSON) (De)serialization

		/// <summary>Generates a cryptographic seed from a JSON null object representation.</summary>
		void IJsonSerializable.FromJson() { m_Value = null; }

		/// <summary>Generates a cryptographic seed from a JSON string representation.</summary>
		/// <param name="jsonString">
		/// The JSON string that represents the cryptographic seed.
		/// </param>
		void IJsonSerializable.FromJson(String jsonString)
		{
			m_Value = Parse(jsonString).m_Value;
		}

		/// <summary>Generates a cryptographic seed from a JSON integer representation.</summary>
		/// <param name="jsonInteger">
		/// The JSON integer that represents the cryptographic seed.
		/// </param>
		void IJsonSerializable.FromJson(Int64 jsonInteger) { throw new NotSupportedException(QowaivMessages.JsonSerialization_Int64NotSupported); }

		/// <summary>Generates a cryptographic seed from a JSON number representation.</summary>
		/// <param name="jsonNumber">
		/// The JSON number that represents the cryptographic seed.
		/// </param>
		void IJsonSerializable.FromJson(Double jsonNumber) { throw new NotSupportedException(QowaivMessages.JsonSerialization_DoubleNotSupported); }

		/// <summary>Generates a cryptographic seed from a JSON date representation.</summary>
		/// <param name="jsonDate">
		/// The JSON Date that represents the cryptographic seed.
		/// </param>
		void IJsonSerializable.FromJson(DateTime jsonDate) { throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported); }

		/// <summary>Converts a cryptographic seed into its JSON object representation.</summary>
		object IJsonSerializable.ToJson() { return Length == 0 ? null : ToString(); }

		#endregion

		#region IFormattable / ToString

		/// <summary>Returns a System.String that represents the current cryptographic seed for debug purposes.</summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Debugger.")]
		private string DebuggerDisplay
		{
			get
			{
				if (IsEmpty()) { return "{empty}"; }
				return Base64.ToString(m_Value);
			}
		}

		/// <summary>Returns a System.String that represents the current cryptographic seed.</summary>
		public override string ToString()
		{
			return ToString(CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted System.String that represents the current cryptographic seed.</summary>
		/// <param name="format">
		/// The format that this describes the formatting.
		/// </param>
		public string ToString(string format)
		{
			return ToString(format, CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted System.String that represents the current cryptographic seed.</summary>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		public string ToString(IFormatProvider formatProvider)
		{
			return ToString("", formatProvider);
		}

		/// <summary>Returns a formatted System.String that represents the current cryptographic seed.</summary>
		/// <param name="format">
		/// The format that this describes the formatting.
		/// </param>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			string formatted;
			if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out formatted))
			{
				return formatted;
			}
			return Base64.ToString(m_Value);
		}

		#endregion

		#region IEquatable

		/// <summary>Returns true if this instance and the other cryptographic seed are equal, otherwise false.</summary>
		/// <param name="other">The other cryptographic seed.</param>
		public bool Equals(CryptographicSeed other)
		{
			if (Length == other.Length)
			{
				for (var i = 0; i < Length; i++)
				{
					if (!m_Value[i].Equals(other.m_Value[i])) { return false; }
				}
				return true;
			}
			return false;
		}

		/// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
		/// <param name="obj">An object to compare with.</param>
		public override bool Equals(object obj)
		{
			if (object.ReferenceEquals(obj, null) || !(obj is CryptographicSeed)) { return false; }
			return Equals((CryptographicSeed)obj);
		}

		/// <summary>Returns the hash code for this cryptographic seed.</summary>
		/// <returns>
		/// A 32-bit signed integer hash code.
		/// </returns>
		public override int GetHashCode()
		{
			var hash = 0;
			for (var i = 0; i < Length; i++)
			{
				hash ^= m_Value[i] << (i % 24);
			}
			return hash;
		}

		/// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator ==(CryptographicSeed left, CryptographicSeed right)
		{
			return left.Equals(right);
		}

		/// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator !=(CryptographicSeed left, CryptographicSeed right)
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
		/// An object that evaluates to a cryptographic seed.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.Value
		/// Condition Less than zero This instance precedes value. Zero This instance
		/// has the same position in the sort order as value. Greater than zero This
		/// instance follows value.-or- value is null.
		/// </returns>
		/// <exception cref="System.ArgumentException">
		/// value is not a cryptographic seed.
		/// </exception>
		public int CompareTo(object obj)
		{
			if (obj is CryptographicSeed)
			{
				return CompareTo((CryptographicSeed)obj);
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.AgrumentException_Must, "a cryptographic seed"), "obj");
		}

		/// <summary>Compares this instance with a specified cryptographic seed and indicates
		/// whether this instance precedes, follows, or appears in the same position
		/// in the sort order as the specified cryptographic seed.
		/// </summary>
		/// <param name="other">
		/// The cryptographic seed to compare with this instance.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.
		/// </returns>
		public int CompareTo(CryptographicSeed other)
		{
			var minLength = Math.Min(Length, other.Length);

			for (var i = 0; i < minLength; i++)
			{
				var compare = m_Value[i].CompareTo(other.m_Value[i]);
				if (compare != 0) { return compare; }
			}
			return Length.CompareTo(other.Length);
		}

		#endregion

		#region (Explicit) casting

		/// <summary>Casts a cryptographic seed to a System.String.</summary>
		public static explicit operator string(CryptographicSeed val) { return val.ToString(); }
		/// <summary>Casts a System.String to a cryptographic seed.</summary>
		public static explicit operator CryptographicSeed(string str) { return CryptographicSeed.Parse(str); }

		/// <summary>Casts a cryptographic seed to a System.Byte[].</summary>
		public static explicit operator Byte[](CryptographicSeed val) { return val.ToByteArray(); }
		/// <summary>Casts a System.Byte[] to a cryptographic seed.</summary>
		public static implicit operator CryptographicSeed(Byte[] bytes) { return CryptographicSeed.Create(bytes); }

		#endregion

		#region Factory methods

		/// <summary>Converts the string to a cryptographic seed.</summary>
		/// <param name="s">
		/// A string containing a cryptographic seed to convert.
		/// </param>
		/// <returns>
		/// A cryptographic seed.
		/// </returns>
		/// <exception cref="System.FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static CryptographicSeed Parse(string s)
		{
			CryptographicSeed val;
			if (CryptographicSeed.TryParse(s, out val))
			{
				return val;
			}
			throw new FormatException(QowaivMessages.FormatExceptionCryptographicSeed);
		}

		/// <summary>Converts the string to a cryptographic seed.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a cryptographic seed to convert.
		/// </param>
		/// <returns>
		/// The cryptographic seed if the string was converted successfully, otherwise CryptographicSeed.Empty.
		/// </returns>
		[SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Qowaiv.Security.Cryptography.CryptographicSeed.TryParse(System.String,Qowaiv.Security.Cryptography.CryptographicSeed@)",
			Justification = "If the parsing succeeded or not, is ignored on purpose.")]
		public static CryptographicSeed TryParse(string s)
		{
			var result = CryptographicSeed.Empty;
			TryParse(s, out result);
			return result;
		}

		/// <summary>Converts the string to a cryptographic seed.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a cryptographic seed to convert.
		/// </param>
		/// <param name="result">
		/// The result of the parsing.
		/// </param>
		/// <returns>
		/// True if the string was converted successfully, otherwise false.
		/// </returns>
		public static bool TryParse(string s, out CryptographicSeed result)
		{
			result = CryptographicSeed.Empty;
			if (string.IsNullOrEmpty(s))
			{
				return true;
			}

			byte[] bytes;

			if (Base64.TryGetBytes(s, out bytes))
			{
				result = CryptographicSeed.Create(bytes);
				return true;
			}
			return false;
		}

		/// <summary>Creates a cryptographic seed from a Byte[]. </summary >
		/// <param name="val" >
		/// A byte array describing a cryptographic seed.
		/// </param >
		public static CryptographicSeed Create(Byte[] val)
		{
			if (val == null || val.Length == 0) { return CryptographicSeed.Empty; }

			var bytes = new byte[val.Length];
			Array.Copy(val, bytes, val.Length);

			return new CryptographicSeed() { m_Value = bytes };
		}

		/// <summary>Creates a cryptographic seed from a GUID.</summary >
		/// <param name="id" >
		/// A GUID describing a cryptographic seed.
		/// </param >
		public static CryptographicSeed Create(Guid id)
		{
			return Create(id.ToByteArray());
		}

		/// <summary>Creates a cryptographic seed from a UUID.</summary >
		/// <param name="id" >
		/// A UUID describing a cryptographic seed.
		/// </param >
		public static CryptographicSeed Create(Uuid id)
		{
			return Create(id.ToByteArray());
		}
		#endregion

		#region Validation

		/// <summary>Returns true if the val represents a valid cryptographic seed, otherwise false.</summary>
		public static bool IsValid(string val)
		{
			byte[] bytes;
			return !String.IsNullOrEmpty(val) && Base64.TryGetBytes(val, out bytes);
		}

		#endregion
	}
}