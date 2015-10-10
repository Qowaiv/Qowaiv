using Qowaiv.Conversion;
using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.Collections.Generic;
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
	/// <summary>Represents an email address.</summary>
	[DebuggerDisplay("{DebuggerDisplay}")]
	[SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "The < and > operators have no meaning for an email address.")]
	[Serializable, SingleValueObject(SingleValueStaticOptions.All, typeof(String))]
	[TypeConverter(typeof(EmailAddressTypeConverter))]
	public struct EmailAddress : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IComparable, IComparable<EmailAddress>
	{
		/// <summary>Represents the pattern of a (potential) valid email address.</summary>
		/// <remarks>
		/// http://www.codeproject.com/KB/recipes/EmailRegexValidator.aspx
		/// </remarks>
		public static readonly Regex Pattern = new Regex(
			@"
				^
					[\w{}|/%$&#~!?*`'^=+-]+(\.[\w{}|/%$&#~!?*`'^=+-]+)*
				@ 
				(
					(
						(\[(?=.*]$))?
						(( [0-9] | [1-9][0-9] | 1[0-9]{2} | 2[0-4][0-9] | 25[0-5] )\.){3}
						 ( [0-9] | [1-9][0-9] | 1[0-9]{2} | 2[0-4][0-9] | 25[0-5] )
						((?<=@\[.*)])?
					)
				|
					(\w+([-]*\w+)*\.)*
					[a-z]{2,}
				)
				$
			", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

		/// <summary>Represents an empty/not set email address.</summary>
		public static readonly EmailAddress Empty = default(EmailAddress);

		/// <summary>Represents an unknown (but set) email address.</summary>
		public static readonly EmailAddress Unknown = new EmailAddress() { m_Value = "?" };

		#region Properties

		/// <summary>The inner value of the email address.</summary>
		private String m_Value;

		/// <summary>Gets the number of characters of email address.</summary>
		public int Length { get { return IsEmptyOrUnknown() ? 0 : m_Value.Length; } }

		/// <summary>Gets the local part of the Email Address.</summary>
		public string Local { get { return IsEmptyOrUnknown() ? string.Empty : m_Value.Substring(0, m_Value.IndexOf('@')); } }

		/// <summary>Gets the domain part of the Email Address.</summary>
		public string Domain { get { return IsEmptyOrUnknown() ? string.Empty : m_Value.Substring(m_Value.IndexOf('@') + 1); } }

		#endregion

		#region Methods

		/// <summary>Returns true if the email address is empty, otherwise false.</summary>
		public bool IsEmpty() { return m_Value == default(String); }

		/// <summary>Returns true if the email address is unknown, otherwise false.</summary>
		public bool IsUnknown() { return m_Value == EmailAddress.Unknown.m_Value; }

		/// <summary>Returns true if the email address is empty or unknown, otherwise false.</summary>
		public bool IsEmptyOrUnknown() { return IsEmpty() || IsUnknown(); }

		#endregion

		#region (XML) (De)serialization

		/// <summary>Initializes a new instance of email address based on the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		private EmailAddress(SerializationInfo info, StreamingContext context)
		{
			Guard.NotNull(info, "info");
			m_Value = info.GetString("Value");
		}

		/// <summary>Adds the underlying property of email address to the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			Guard.NotNull(info, "info");
			info.AddValue("Value", m_Value);
		}

		/// <summary>Gets the xml schema to (de) xml serialize an email address.</summary>
		/// <remarks>
		/// Returns null as no schema is required.
		/// </remarks>
		XmlSchema IXmlSerializable.GetSchema() { return null; }

		/// <summary>Reads the email address from an xml writer.</summary>
		/// <remarks>
		/// Uses the string parse function of email address.
		/// </remarks>
		/// <param name="reader">An xml reader.</param>
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			Guard.NotNull(reader, "reader");
			var s = reader.ReadElementString();
			var val = Parse(s, CultureInfo.InvariantCulture);
			m_Value = val.m_Value;
		}

		/// <summary>Writes the email address to an xml writer.</summary>
		/// <remarks>
		/// Uses the string representation of email address.
		/// </remarks>
		/// <param name="writer">An xml writer.</param>
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			Guard.NotNull(writer, "writer");
			writer.WriteString(ToString(CultureInfo.InvariantCulture));
		}

		#endregion

		#region (JSON) (De)serialization

		/// <summary>Generates an email address from a JSON null object representation.</summary>
		void IJsonSerializable.FromJson()
		{
			m_Value = default(String);
		}

		/// <summary>Generates an email address from a JSON string representation.</summary>
		/// <param name="jsonString">
		/// The JSON string that represents the email address.
		/// </param>
		void IJsonSerializable.FromJson(String jsonString)
		{
			m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;
		}

		/// <summary>Generates an email address from a JSON integer representation.</summary>
		/// <param name="jsonInteger">
		/// The JSON integer that represents the email address.
		/// </param>
		void IJsonSerializable.FromJson(Int64 jsonInteger) { throw new NotSupportedException(QowaivMessages.JsonSerialization_Int64NotSupported); }

		/// <summary>Generates an email address from a JSON number representation.</summary>
		/// <param name="jsonNumber">
		/// The JSON number that represents the email address.
		/// </param>
		void IJsonSerializable.FromJson(Double jsonNumber) { throw new NotSupportedException(QowaivMessages.JsonSerialization_DoubleNotSupported); }

		/// <summary>Generates an email address from a JSON date representation.</summary>
		/// <param name="jsonDate">
		/// The JSON Date that represents the email address.
		/// </param>
		void IJsonSerializable.FromJson(DateTime jsonDate) { throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported); }

		/// <summary>Converts an email address into its JSON object representation.</summary>
		object IJsonSerializable.ToJson()
		{
			return m_Value == default(String) ? null : ToString(CultureInfo.InvariantCulture);
		}

		#endregion

		#region IFormattable / ToString

		/// <summary>Returns a System.String that represents the current email address for debug purposes.</summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Debugger.")]
		private string DebuggerDisplay
		{
			get
			{
				if (IsEmpty()) { return "EmailAddress: (empty)"; }
				if (IsUnknown()) { return "EmailAddress: (unknown)"; }
				return String.Format(CultureInfo.InvariantCulture, "EmailAddress: {0}", m_Value);
			}
		}

		/// <summary>Returns a System.String that represents the current email address.</summary>
		public override string ToString()
		{
			return ToString(CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted System.String that represents the current email address.</summary>
		/// <param name="format">
		/// The format that this describes the formatting.
		/// </param>
		public string ToString(string format)
		{
			return ToString(format, CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted System.String that represents the current email address.</summary>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		public string ToString(IFormatProvider formatProvider)
		{
			return ToString("", formatProvider);
		}

		/// <summary>Returns a formatted System.String that represents the current email address.</summary>
		/// <param name="format">
		/// The format that this describes the formatting.
		/// </param>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		/// <remarks>
		/// The formats:
		/// 
		/// f: as formatted email address.
		/// U: as full email address uppercased.
		/// l: as local name.
		/// L: as local name uppercased.
		/// d: as domain.
		/// D: as domain uppercased.
		/// </remarks>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			string formatted;
			if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out formatted))
			{
				return formatted;
			}

			// If no format specified, use the default format.
			if (String.IsNullOrEmpty(format)) { return m_Value ?? String.Empty; }

			// Apply the format.
			return StringFormatter.Apply(this, format, formatProvider, FormatTokens);
		}

		/// <summary>The format token instructions.</summary>
		private static readonly Dictionary<char, Func<EmailAddress, IFormatProvider, string>> FormatTokens = new Dictionary<char, Func<EmailAddress, IFormatProvider, string>>()
		{
			{ 'U', (svo, provider) => svo.m_Value.ToUpperInvariant() },
			{ 'l', (svo, provider) => svo.Local },
			{ 'L', (svo, provider) => svo.Local.ToUpperInvariant() },
			{ 'd', (svo, provider) => svo.Domain },
			{ 'D', (svo, provider) => svo.Domain.ToUpperInvariant() },
			{ 'f', (svo, provider) => svo.m_Value ?? String.Empty },    
		};

		#endregion

		#region IEquatable

		/// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
		/// <param name="obj">An object to compare with.</param>
		public override bool Equals(object obj) { return base.Equals(obj); }

		/// <summary>Returns the hash code for this email address.</summary>
		/// <returns>
		/// A 32-bit signed integer hash code.
		/// </returns>
		public override int GetHashCode() { return m_Value == null ? 0 : m_Value.GetHashCode(); }

		/// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator ==(EmailAddress left, EmailAddress right)
		{
			return left.Equals(right);
		}

		/// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator !=(EmailAddress left, EmailAddress right)
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
		/// An object that evaluates to a email address.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.Value
		/// Condition Less than zero This instance precedes value. Zero This instance
		/// has the same position in the sort order as value. Greater than zero This
		/// instance follows value.-or- value is null.
		/// </returns>
		/// <exception cref="System.ArgumentException">
		/// value is not a email address.
		/// </exception>
		public int CompareTo(object obj)
		{
			if (obj is EmailAddress)
			{
				return CompareTo((EmailAddress)obj);
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.AgrumentException_Must, "an email address"), "obj");
		}

		/// <summary>Compares this instance with a specified email address and indicates
		/// whether this instance precedes, follows, or appears in the same position
		/// in the sort order as the specified email address.
		/// </summary>
		/// <param name="other">
		/// The email address to compare with this instance.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.
		/// </returns>
		public int CompareTo(EmailAddress other) { return String.Compare(m_Value, other.m_Value, StringComparison.Ordinal); }

		#endregion

		#region (Explicit) casting

		/// <summary>Casts an email address to a System.String.</summary>
		public static explicit operator string(EmailAddress val) { return val.ToString(CultureInfo.CurrentCulture); }
		/// <summary>Casts a System.String to a email address.</summary>
		public static explicit operator EmailAddress(string str) { return EmailAddress.Parse(str, CultureInfo.CurrentCulture); }


		#endregion

		#region Factory methods

		/// <summary>Converts the string to an email address.</summary>
		/// <param name="s">
		/// A string containing an email address to convert.
		/// </param>
		/// <returns>
		/// An email address.
		/// </returns>
		/// <exception cref="System.FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static EmailAddress Parse(string s)
		{
			return Parse(s, CultureInfo.CurrentCulture);
		}

		/// <summary>Converts the string to an email address.</summary>
		/// <param name="s">
		/// A string containing an email address to convert.
		/// </param>
		/// <param name="formatProvider">
		/// The specified format provider.
		/// </param>
		/// <returns>
		/// An email address.
		/// </returns>
		/// <exception cref="System.FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static EmailAddress Parse(string s, IFormatProvider formatProvider)
		{
			EmailAddress val;
			if (EmailAddress.TryParse(s, formatProvider, out val))
			{
				return val;
			}
			throw new FormatException(QowaivMessages.FormatExceptionEmailAddress);
		}

		/// <summary>Converts the string to an email address.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing an email address to convert.
		/// </param>
		/// <returns>
		/// The email address if the string was converted successfully, otherwise EmailAddress.Empty.
		/// </returns>
		public static EmailAddress TryParse(string s)
		{
			EmailAddress val;
			if (EmailAddress.TryParse(s, out val))
			{
				return val;
			}
			return EmailAddress.Empty;
		}

		/// <summary>Converts the string to an email address.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing an email address to convert.
		/// </param>
		/// <param name="result">
		/// The result of the parsing.
		/// </param>
		/// <returns>
		/// True if the string was converted successfully, otherwise false.
		/// </returns>
		public static bool TryParse(string s, out EmailAddress result)
		{
			return TryParse(s, CultureInfo.CurrentCulture, out result);
		}

		/// <summary>Converts the string to an email address.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing an email address to convert.
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
		[SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase",
			Justification = "Email addresses are displayed in lowercase by default.")]
		public static bool TryParse(string s, IFormatProvider formatProvider, out EmailAddress result)
		{
			result = EmailAddress.Empty;
			if (string.IsNullOrEmpty(s))
			{
				return true;
			}
			var culture = formatProvider as CultureInfo ?? CultureInfo.InvariantCulture;
			if (Qowaiv.Unknown.IsUnknown(s, culture))
			{
				result = EmailAddress.Unknown;
				return true;
			}
			if (IsValid(s, formatProvider))
			{
				result = new EmailAddress() { m_Value = s.ToLowerInvariant() };
				return true;
			}
			return false;
		}

		#endregion

		#region Validation

		/// <summary>Returns true if the val represents a valid email address, otherwise false.</summary>
		public static bool IsValid(string val)
		{
			return IsValid(val, CultureInfo.CurrentCulture);
		}

		/// <summary>Returns true if the val represents a valid email address, otherwise false.</summary>
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "formatProvider",
			Justification = "Satisfies the static Qowaiv SVO contract.")]
		public static bool IsValid(string val, IFormatProvider formatProvider)
		{
			return Pattern.IsMatch(val ?? string.Empty);
		}
		#endregion
	}
}