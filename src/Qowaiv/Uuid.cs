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
	/// <summary>Represents a UUID (Universally unique identifier) aka GUID (Globally unique identifier).</summary>
	/// <remarks>
	/// The main difference between this UUID and the default System.GUID is 
	/// the default string representation. For this, that is a 22 char long
	/// Base64 representation.
	/// 
	/// The reason not to call this a GUID but an UUID it just to prevent users of 
	/// Qowaiv to be forced to specify the namespace of there GUID of choice 
	/// everywhere.
	/// </remarks>
	[DebuggerDisplay("{DebuggerDisplay}")]
	// [SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "The < and > operators have no meaning for a GUID.")]
	[Serializable, SingleValueObject(SingleValueStaticOptions.AllExcludingCulture ^ SingleValueStaticOptions.HasUnknownValue, typeof(Guid))]
	[TypeConverter(typeof(UuidTypeConverter))]
	public struct Uuid : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IComparable, IComparable<Uuid>
	{
		/// <summary>Represents the pattern of a (potential) valid GUID.</summary>
		public static readonly Regex Pattern = new Regex(@"^[a-zA-Z0-9_-]{22}(=){0,2}$", RegexOptions.Compiled);

		/// <summary>Represents an empty/not set GUID.</summary>
		public static readonly Uuid Empty = default(Uuid);

		/// <summary>Initializes a new instance of a GUID.</summary>
		private Uuid(Guid id)
		{
			m_Value = id;
		}

		#region Properties

		/// <summary>The inner value of the GUID.</summary>
		private Guid m_Value;

		#endregion

		#region Methods

		/// <summary>Returns true if the GUID is empty, otherwise false.</summary>
		public bool IsEmpty() { return m_Value == default(System.Guid); }

		/// <summary>Returns a 16-element byte array that contains the value of this instance.</summary>
		public byte[] ToByteArray()
		{
			return m_Value.ToByteArray();
		}

		#endregion

		#region (XML) (De)serialization

		/// <summary>Initializes a new instance of GUID based on the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		private Uuid(SerializationInfo info, StreamingContext context)
		{
			Guard.NotNull(info, "info");
			m_Value = (Guid)info.GetValue("Value", typeof(Guid));
		}

		/// <summary>Adds the underlying propererty of GUID to the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			Guard.NotNull(info, "info");
			info.AddValue("Value", m_Value);
		}

		/// <summary>Gets the xml schema to (de) xml serialize a GUID.</summary>
		/// <remarks>
		/// Returns null as no schema is required.
		/// </remarks>
		XmlSchema IXmlSerializable.GetSchema() { return null; }

		/// <summary>Reads the GUID from an xml writer.</summary>
		/// <remarks>
		/// Uses the string parse function of GUID.
		/// </remarks>
		/// <param name="reader">An xml reader.</param>
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			var s = reader.ReadElementString();
			var val = Parse(s);
			m_Value = val.m_Value;
		}

		/// <summary>Writes the GUID to an xml writer.</summary>
		/// <remarks>
		/// Uses the string representation of GUID.
		/// </remarks>
		/// <param name="writer">An xml writer.</param>
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			writer.WriteString(ToString(CultureInfo.InvariantCulture));
		}

		#endregion

		#region (JSON) (De)serialization

		/// <summary>Generates a GUID from a JSON null object representation.</summary>
		void IJsonSerializable.FromJson()
		{
			m_Value = default(Guid);
		}

		/// <summary>Generates a GUID from a JSON string representation.</summary>
		/// <param name="jsonString">
		/// The JSON string that represents the GUID.
		/// </param>
		void IJsonSerializable.FromJson(String jsonString)
		{
			m_Value = Parse(jsonString).m_Value;
		}

		/// <summary>Generates a GUID from a JSON integer representation.</summary>
		/// <param name="jsonInteger">
		/// The JSON integer that represents the GUID.
		/// </param>
		void IJsonSerializable.FromJson(Int64 jsonInteger) { throw new NotSupportedException(QowaivMessages.JsonSerialization_Int64NotSupported); }
		//{
		//    m_Value = Create(jsonInteger).m_Value;
		//}

		/// <summary>Generates a GUID from a JSON number representation.</summary>
		/// <param name="jsonNumber">
		/// The JSON number that represents the GUID.
		/// </param>
		void IJsonSerializable.FromJson(Double jsonNumber) { throw new NotSupportedException(QowaivMessages.JsonSerialization_DoubleNotSupported); }
		//{
		//    m_Value = Create(jsonNumber).m_Value;
		//}

		/// <summary>Generates a GUID from a JSON date representation.</summary>
		/// <param name="jsonDate">
		/// The JSON Date that represents the GUID.
		/// </param>
		void IJsonSerializable.FromJson(DateTime jsonDate) { throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported); }
		//{
		//    m_Value = Create(jsonDate).m_Value;
		//}

		/// <summary>Converts a GUID into its JSON object representation.</summary>
		object IJsonSerializable.ToJson()
		{
			return m_Value == default(Guid) ? null : ToString(CultureInfo.InvariantCulture);
		}

		#endregion

		#region IFormattable / ToString

		/// <summary>Returns a System.String that represents the current GUID for debug purposes.</summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string DebuggerDisplay { get { return ToString(); } }

		/// <summary>Returns a System.String that represents the current GUID.</summary>
		public override string ToString()
		{
			return ToString(CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted System.String that represents the current GUID.</summary>
		/// <param name="format">
		/// The format that this describes the formatting.
		/// </param>
		public string ToString(string format)
		{
			return ToString(format, CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted System.String that represents the current GUID.</summary>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		public string ToString(IFormatProvider formatProvider)
		{
			return ToString("", formatProvider);
		}

		/// <summary>Returns a formatted System.String that represents the current GUID.</summary>
		/// <param name="format">
		/// The format that this describes the formatting.
		/// </param>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		/// <remarks>
		/// S
		/// 22 base64 chars:
		/// 0123465798aAbBcCdDeE_-
		/// N
		/// 32 digits:
		/// 00000000000000000000000000000000
		/// D
		/// 32 digits separated by hyphens:
		/// 00000000-0000-0000-0000-000000000000
		/// B
		/// 32 digits separated by hyphens, enclosed in braces:
		/// {00000000-0000-0000-0000-000000000000}
		/// P
		/// 32 digits separated by hyphens, enclosed in parentheses:
		/// (00000000-0000-0000-0000-000000000000)
		/// X
		/// Four hexadecimal values enclosed in braces, where the fourth value is a subset of eight hexadecimal values that is also enclosed in braces:
		/// {0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}}
		/// 
		/// the lowercase formats are lowercase (except the the 's').
		/// </remarks>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			string formatted;
			if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out formatted))
			{
				return formatted;
			}

			switch (format)
			{
				case null:
				case "":
				case "s":
				case "S":
					// avoid invalid URL characters
					return Convert.ToBase64String(ToByteArray()).Replace('+', '-').Replace('/', '_').Substring(0, 22);
				case "N":
				case "D":
				case "B":
				case "P": return m_Value.ToString(format, formatProvider).ToUpperInvariant();
				case "X": return m_Value.ToString(format, formatProvider).ToUpperInvariant().Replace('X', 'x');

				case "n":
				case "d":
				case "b":
				case "p":
				case "x":
					return m_Value.ToString(format, formatProvider);

				default: throw new FormatException(QowaivMessages.FormatException_InvalidFormat);
			}
		}

		#endregion

		#region IEquatable

		/// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
		/// <param name="obj">An object to compair with.</param>
		public override bool Equals(object obj) { return base.Equals(obj); }

		/// <summary>Returns the hash code for this GUID.</summary>
		/// <returns>
		/// A 32-bit signed integer hash code.
		/// </returns>
		public override int GetHashCode() { return m_Value.GetHashCode(); }

		/// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator ==(Uuid left, Uuid right)
		{
			return left.Equals(right);
		}

		/// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator !=(Uuid left, Uuid right)
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
		/// An object that evaluates to a GUID.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.Value
		/// Condition Less than zero This instance precedes value. Zero This instance
		/// has the same position in the sort order as value. Greater than zero This
		/// instance follows value.-or- value is null.
		/// </returns>
		/// <exception cref="System.ArgumentException">
		/// value is not a GUID.
		/// </exception>
		public int CompareTo(object obj)
		{
			if (obj is Uuid || obj is Guid)
			{
				return CompareTo((Uuid)obj);
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.AgrumentException_Must, "a GUID"), "obj");
		}

		/// <summary>Compares this instance with a specified GUID and indicates
		/// whether this instance precedes, follows, or appears in the same position
		/// in the sort order as the specified GUID.
		/// </summary>
		/// <param name="other">
		/// The GUID to compare with this instance.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.
		/// </returns>
		public int CompareTo(Uuid other) { return m_Value.CompareTo(other.m_Value); }

		#endregion

		#region (Explicit) casting

		/// <summary>Casts a GUID to a System.String.</summary>
		public static explicit operator string(Uuid val) { return val.ToString(CultureInfo.CurrentCulture); }
		/// <summary>Casts a System.String to a GUID.</summary>
		public static explicit operator Uuid(string str) { return Uuid.Parse(str); }

		/// <summary>Casts a Qowaiv.GUID to a System.GUID.</summary>
		public static implicit operator Guid(Uuid val) { return val.m_Value; }
		/// <summary>Casts a System.GUID to a Qowaiv.GUID.</summary>
		public static implicit operator Uuid(Guid val) { return new Uuid(val); }

		#endregion

		#region Factory methods

		/// <summary>Initializes a new instance of a GUID.</summary>
		public static Uuid NewGuid()
		{
			return new Uuid(Guid.NewGuid());
		}

		/// <summary>Converts the string to a GUID.</summary>
		/// <param name="s">
		/// A string containing a GUID to convert.
		/// </param>
		/// <returns>
		/// A GUID.
		/// </returns>
		/// <exception cref="System.FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static Uuid Parse(string s)
		{
			Uuid val;
			if (Uuid.TryParse(s, out val))
			{
				return val;
			}
			throw new FormatException(QowaivMessages.FormatExceptionQGuid);
		}

		/// <summary>Converts the string to a GUID.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a GUID to convert.
		/// </param>
		/// <returns>
		/// The GUID if the string was converted successfully, otherwise QGuid.Empty.
		/// </returns>
		public static Uuid TryParse(string s)
		{
			Uuid val;
			if (Uuid.TryParse(s, out val))
			{
				return val;
			}
			return Uuid.Empty;
		}

		/// <summary>Converts the string to a GUID.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a GUID to convert.
		/// </param>
		/// <param name="result">
		/// The result of the parsing.
		/// </param>
		/// <returns>
		/// True if the string was converted successfully, otherwise false.
		/// </returns>
		public static bool TryParse(string s, out Uuid result)
		{
			result = Uuid.Empty;
			if (string.IsNullOrEmpty(s))
			{
				return true;
			}

			if (Pattern.IsMatch(s))
			{
				var bytes = Convert.FromBase64String(s.Replace('-', '+').Replace('_', '/').Substring(0, 22) + "==");
				result = new Uuid(new Guid(bytes));
				return true;
			}

			Guid id;
			if (Guid.TryParse(s, out id))
			{
				result = new Uuid(id);
				return true;
			}
			return false;
		}

		#endregion

		#region Validation

		/// <summary>Returns true if the value represents a valid GUID, otherwise false.</summary>
		public static bool IsValid(string val)
		{
			Guid id;
			return Pattern.IsMatch(val ?? string.Empty) || Guid.TryParse(val, out id);
		}

		#endregion
	}
}