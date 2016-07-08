using Qowaiv.Conversion;
using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv
{
	/// <summary>The Bank Identifier Code (BIC) is a standard format of Business Identifier Codes
	/// approved by the International Organization for Standardization (ISO) as ISO 9362.
	/// It is a unique identification code for both financial and non-financial institutions.
	/// </summary>
	/// <remarks>
	/// When assigned to a non-financial institution, a code may also be known
	/// as a Business Entity Identifier or BEI.
	/// 
	/// These codes are used when transferring money between banks, particularly
	/// for international wire transfers, and also for the exchange of other
	/// messages between banks. The codes can sometimes be found on account
	/// statements.
	/// </remarks>
	[DebuggerDisplay("{DebuggerDisplay}")]
	[SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "The < and > operators have no meaning for a BIC.")]
	[Serializable, SingleValueObject(SingleValueStaticOptions.All, typeof(string))]
	[TypeConverter(typeof(BankIdentifierCodeTypeConverter))]
	public struct BankIdentifierCode : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IComparable, IComparable<BankIdentifierCode>
	{
		/// <remarks>
		/// http://www.codeproject.com/KB/recipes/bicRegexValidator.aspx
		/// </remarks>
		public static readonly Regex Pattern = new Regex(@"^[A-Z]{6}[A-Z0-9]{2}([A-Z0-9]{3})?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		/// <summary>Represents an empty/not set BIC.</summary>
		public static readonly BankIdentifierCode Empty = default(BankIdentifierCode);

		/// <summary>Represents an unknown (but set) BIC.</summary>
		public static readonly BankIdentifierCode Unknown = new BankIdentifierCode() { m_Value = "ZZZZZZZZZZZ" };

		#region Properties

		/// <summary>The inner value of the BIC.</summary>
		private string m_Value;

		/// <summary>Gets the number of characters of BIC.</summary>
		public int Length { get { return IsEmptyOrUnknown() ? 0 : m_Value.Length; } }

		/// <summary>Gets the institution code or bank code.</summary>
		public string BankCode { get { return IsEmptyOrUnknown() ? string.Empty : m_Value.Substring(0, 4); } }

		/// <summary>Gets the country code.</summary>
		public string CountryCode { get { return IsEmptyOrUnknown() ? string.Empty : m_Value.Substring(4, 2); } }

		/// <summary>Gets the country info of the country code.</summary>
		public Country Country
		{
			get
			{
				if (IsEmpty()) { return Country.Empty; }
				if (IsUnknown()) { return Country.Unknown; }
				return Country.Parse(this.CountryCode, CultureInfo.InvariantCulture);
			}
		}

		/// <summary>Gets the location code.</summary>
		public string LocationCode { get { return IsEmptyOrUnknown() ? string.Empty : m_Value.Substring(6, 2); } }

		/// <summary>Gets the branch code.</summary>
		/// <remarks>
		/// Is optional, XXX for primary office.
		/// </remarks>
		public string BranchCode { get { return this.Length != 11 ? string.Empty : m_Value.Substring(8); } }

		#endregion

		#region Methods

		/// <summary>Returns true if the BIC is empty, otherwise false.</summary>
		public bool IsEmpty() { return m_Value == default(string); }

		/// <summary>Returns true if the BIC is unknown, otherwise false.</summary>
		public bool IsUnknown() { return m_Value == BankIdentifierCode.Unknown.m_Value; }

		/// <summary>Returns true if the BIC is empty or unknown, otherwise false.</summary>
		public bool IsEmptyOrUnknown() { return IsEmpty() || IsUnknown(); }

		#endregion

		#region (XML) (De)serialization

		/// <summary>Initializes a new instance of BIC based on the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		private BankIdentifierCode(SerializationInfo info, StreamingContext context)
		{
			Guard.NotNull(info, "info");
			m_Value = info.GetString("Value");
		}

		/// <summary>Adds the underlying property of BIC to the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			Guard.NotNull(info, "info");
			info.AddValue("Value", m_Value);
		}

		/// <summary>Gets the xml schema to (de) xml serialize a BIC.</summary>
		/// <remarks>
		/// Returns null as no schema is required.
		/// </remarks>
		XmlSchema IXmlSerializable.GetSchema() { return null; }

		/// <summary>Reads the BIC from an xml writer.</summary>
		/// <remarks>
		/// Uses the string parse function of BIC.
		/// </remarks>
		/// <param name="reader">An xml reader.</param>
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			Guard.NotNull(reader, "reader");
			var s = reader.ReadElementString();
			var val = Parse(s, CultureInfo.InvariantCulture);
			m_Value = val.m_Value;
		}

		/// <summary>Writes the BIC to an xml writer.</summary>
		/// <remarks>
		/// Uses the string representation of BIC.
		/// </remarks>
		/// <param name="writer">An xml writer.</param>
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			Guard.NotNull(writer, "writer");
			writer.WriteString(ToString(CultureInfo.InvariantCulture));
		}

		#endregion

		#region (JSON) (De)serialization

		/// <summary>Generates a BIC from a JSON null object representation.</summary>
		void IJsonSerializable.FromJson()
		{
			m_Value = default(string);
		}

		/// <summary>Generates a BIC from a JSON string representation.</summary>
		/// <param name="jsonString">
		/// The JSON string that represents the BIC.
		/// </param>
		void IJsonSerializable.FromJson(string jsonString)
		{
			m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;
		}

		/// <summary>Generates a BIC from a JSON integer representation.</summary>
		/// <param name="jsonInteger">
		/// The JSON integer that represents the BIC.
		/// </param>
		void IJsonSerializable.FromJson(Int64 jsonInteger) { throw new NotSupportedException(QowaivMessages.JsonSerialization_Int64NotSupported); }

		/// <summary>Generates a BIC from a JSON number representation.</summary>
		/// <param name="jsonNumber">
		/// The JSON number that represents the BIC.
		/// </param>
		void IJsonSerializable.FromJson(Double jsonNumber) { throw new NotSupportedException(QowaivMessages.JsonSerialization_DoubleNotSupported); }

		/// <summary>Generates a BIC from a JSON date representation.</summary>
		/// <param name="jsonDate">
		/// The JSON Date that represents the BIC.
		/// </param>
		void IJsonSerializable.FromJson(DateTime jsonDate) { throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported); }

		/// <summary>Converts a BIC into its JSON object representation.</summary>
		object IJsonSerializable.ToJson()
		{
			return m_Value == default(string) ? null : ToString(CultureInfo.InvariantCulture);
		}

		#endregion

		#region IFormattable / ToString

		/// <summary>Returns a <see cref="string"/> that represents the current BIC for debug purposes.</summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Debugger.")]
		private string DebuggerDisplay
		{
			get
			{
				if (IsEmpty()) { return "BankIdentifierCode: (empty)"; }
				if (IsUnknown()) { return "BankIdentifierCode: (unknown)"; }
				return string.Format(CultureInfo.InvariantCulture, "BankIdentifierCode: {0}", m_Value);
			}
		}

		/// <summary>Returns a <see cref="string"/> that represents the current BIC.</summary>
		public override string ToString()
		{
			return ToString(CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted <see cref="string"/> that represents the current BIC.</summary>
		/// <param name="format">
		/// The format that this describes the formatting.
		/// </param>
		public string ToString(string format)
		{
			return ToString(format, CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted <see cref="string"/> that represents the current BIC.</summary>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		public string ToString(IFormatProvider formatProvider)
		{
			return ToString("", formatProvider);
		}

		/// <summary>Returns a formatted <see cref="string"/> that represents the current BIC.</summary>
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
			if (IsEmpty()) { return string.Empty; }
			if (IsUnknown()) { return "?"; }
			return m_Value;
		}

		#endregion

		#region IEquatable

		/// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
		/// <param name="obj">An object to compare with.</param>
		public override bool Equals(object obj) { return base.Equals(obj); }

		/// <summary>Returns the hash code for this BIC.</summary>
		/// <returns>
		/// A 32-bit signed integer hash code.
		/// </returns>
		public override int GetHashCode() { return m_Value == null ? 0 : m_Value.GetHashCode(); }

		/// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator ==(BankIdentifierCode left, BankIdentifierCode right)
		{
			return left.Equals(right);
		}

		/// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator !=(BankIdentifierCode left, BankIdentifierCode right)
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
		/// An object that evaluates to a BIC.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.Value
		/// Condition Less than zero This instance precedes value. Zero This instance
		/// has the same position in the sort order as value. Greater than zero This
		/// instance follows value.-or- value is null.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// value is not a BIC.
		/// </exception>
		public int CompareTo(object obj)
		{
			if (obj is BankIdentifierCode)
			{
				return CompareTo((BankIdentifierCode)obj);
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.ArgumentException_Must, "a BIC"), "obj");
		}

		/// <summary>Compares this instance with a specified BIC and indicates
		/// whether this instance precedes, follows, or appears in the same position
		/// in the sort order as the specified BIC.
		/// </summary>
		/// <param name="other">
		/// The BIC to compare with this instance.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.
		/// </returns>
		public int CompareTo(BankIdentifierCode other) { return String.Compare(m_Value, other.m_Value, StringComparison.Ordinal); }

		#endregion

		#region (Explicit) casting

		/// <summary>Casts a BIC to a <see cref="string"/>.</summary>
		public static explicit operator string(BankIdentifierCode val) { return val.ToString(CultureInfo.CurrentCulture); }
		/// <summary>Casts a <see cref="string"/> to a BIC.</summary>
		public static explicit operator BankIdentifierCode(string str) { return BankIdentifierCode.Parse(str, CultureInfo.CurrentCulture); }


		#endregion

		#region Factory methods

		/// <summary>Converts the string to a BIC.</summary>
		/// <param name="s">
		/// A string containing a BIC to convert.
		/// </param>
		/// <returns>
		/// A BIC.
		/// </returns>
		/// <exception cref="FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static BankIdentifierCode Parse(string s)
		{
			return Parse(s, CultureInfo.CurrentCulture);
		}

		/// <summary>Converts the string to a BIC.</summary>
		/// <param name="s">
		/// A string containing a BIC to convert.
		/// </param>
		/// <param name="formatProvider">
		/// The specified format provider.
		/// </param>
		/// <returns>
		/// A BIC.
		/// </returns>
		/// <exception cref="FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static BankIdentifierCode Parse(string s, IFormatProvider formatProvider)
		{
			BankIdentifierCode val;
			if (BankIdentifierCode.TryParse(s, formatProvider, out val))
			{
				return val;
			}
			throw new FormatException(QowaivMessages.FormatExceptionBankIdentifierCode);
		}

		/// <summary>Converts the string to a BIC.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a BIC to convert.
		/// </param>
		/// <returns>
		/// The BIC if the string was converted successfully, otherwise BankIdentifierCode.Empty.
		/// </returns>
		public static BankIdentifierCode TryParse(string s)
		{
			BankIdentifierCode val;
			if (BankIdentifierCode.TryParse(s, out val))
			{
				return val;
			}
			return BankIdentifierCode.Empty;
		}

		/// <summary>Converts the string to a BIC.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a BIC to convert.
		/// </param>
		/// <param name="result">
		/// The result of the parsing.
		/// </param>
		/// <returns>
		/// True if the string was converted successfully, otherwise false.
		/// </returns>
		public static bool TryParse(string s, out BankIdentifierCode result)
		{
			return TryParse(s, CultureInfo.CurrentCulture, out result);
		}

		/// <summary>Converts the string to a BIC.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a BIC to convert.
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
		public static bool TryParse(string s, IFormatProvider formatProvider, out BankIdentifierCode result)
		{
			result = BankIdentifierCode.Empty;
			if (string.IsNullOrEmpty(s))
			{
				return true;
			}
			if (Qowaiv.Unknown.IsUnknown(s, formatProvider as CultureInfo))
			{
				result = BankIdentifierCode.Unknown;
				return true;
			}
			if (IsValid(s, formatProvider))
			{
				result = new BankIdentifierCode() { m_Value = Parsing.ClearSpacingAndMarkupToUpper(s) };
				return true;
			}
			return false;
		}

		#endregion

		#region Validation

		/// <summary>Returns true if the val represents a valid BIC, otherwise false.</summary>
		public static bool IsValid(string val)
		{
			return IsValid(val, CultureInfo.CurrentCulture);
		}

		/// <summary>Returns true if the val represents a valid BIC, otherwise false.</summary>
		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0",
			Justification = "formatProvider is validated by Country.IsValid().")]
		public static bool IsValid(string val, IFormatProvider formatProvider)
		{
			return Pattern.IsMatch(val ?? string.Empty) && Country.IsValid(val.Substring(4, 2), formatProvider);
		}
		#endregion
	}
}
