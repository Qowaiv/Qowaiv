using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv
{
	/// <summary>Represents a collection of unique email addresses.</summary>
	/// <remarks>
	/// Empty and unknown email addresses can not be added due to a special 
	/// equality comparer that always assumes that they are already added.
	/// </remarks>
	[Serializable]
	public class EmailAddressCollection : ISet<EmailAddress>, ISerializable, IXmlSerializable, IJsonSerializable, IFormattable
	{
		/// <summary>The email address separator is a comma.</summary>
		/// <remarks>
		/// According to RFC 6068: http://tools.ietf.org/html/rfc6068.
		/// </remarks>
		public const string Separator = ",";
		/// <summary>The separators used for parsing.</summary>
		/// <remarks>
		/// Both comma and semicolon are supported.
		/// </remarks>
		private static readonly char[] Separators = new char[] { ',', ';' };

		/// <summary>The underlying hash set.</summary>
		/// <remarks>
		/// The only proper way to block the adding of empty and unknown 
		/// email addresses was by overriding Add, which is not allowed if
		/// derived from HasSet&lt;EmailAddress&gt;.
		/// 
		/// So this construction is required.
		/// </remarks>
		private HashSet<EmailAddress> hashset = new HashSet<EmailAddress>();

		/// <summary>Initiates a new collection of email addresses.</summary>
		public EmailAddressCollection() { }

		/// <summary>Initiates a new collection of email addresses.</summary>
		/// <param name="emails">
		/// An array of email addresses.
		/// </param>
		public EmailAddressCollection(params EmailAddress[] emails)
			: this((IEnumerable<EmailAddress>)emails) { }

		/// <summary>Initiates a new collection of email addresses.</summary>
		/// <param name="emails">
		/// An enumeration of email addresses.
		/// </param>
		public EmailAddressCollection(IEnumerable<EmailAddress> emails)
			: this()
		{
			AddRange(emails);
		}

		#region Methods

		/// <summary>Adds an email address to the current collection and returns
		/// a value to indicate if the email address was successfully added.
		/// </summary>
		/// <param name="item">
		/// The email address to add.
		/// </param>
		public bool Add(EmailAddress item)
		{
			if (item.IsEmptyOrUnknown()) { return false; }
			return hashset.Add(item);
		}

		/// <summary>Keeps only the distinct set of email addresses in the collection.</summary>
		/// <returns>
		/// The current (cleaned) instance of the collection.
		/// </returns>
		public void AddRange(IEnumerable<EmailAddress> emails)
		{
			Guard.NotNull(emails, "emails");
			foreach (var email in emails) { Add(email); }
		}

		#endregion

		#region ICollection

		/// <summary>Gets the number of email addresses in the collection.</summary>
		[ExcludeFromCodeCoverage]
		public int Count { get { return hashset.Count; } }

		/// <summary>Returns false as this collection is not read only.</summary>
		[ExcludeFromCodeCoverage]
		public bool IsReadOnly { get { return false; } }

		/// <summary>Adds an email address to the current collection.</summary>
		[ExcludeFromCodeCoverage]
		void ICollection<EmailAddress>.Add(EmailAddress email) { Add(email); }

		/// <summary>Clears all email addresses From current collection.</summary>
		[ExcludeFromCodeCoverage]
		public void Clear() { hashset.Clear(); }

		/// <summary>Returns true if the collection contains the specified email address.</summary>
		[ExcludeFromCodeCoverage]
		public bool Contains(EmailAddress item) { return hashset.Contains(item); }

		/// <summary>Copies the email addresses of the collection to an
		/// System.Array, starting at a particular System.Array index.
		/// </summary>
		[ExcludeFromCodeCoverage]
		public void CopyTo(EmailAddress[] array, int arrayIndex) { hashset.CopyTo(array, arrayIndex); }

		/// <summary>Removes the email address from the collection.</summary>
		[ExcludeFromCodeCoverage]
		public bool Remove(EmailAddress item) { return hashset.Remove(item); }

		/// <summary>Gets an enumerator to loop through all email addresses of the collection.</summary>
		[ExcludeFromCodeCoverage]
		public IEnumerator<EmailAddress> GetEnumerator() { return hashset.GetEnumerator(); }

		/// <summary>Gets an enumerator to loop through all email addresses of the collection.</summary>
		[ExcludeFromCodeCoverage]
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

		#endregion

		#region ISet

		/// <summary>Removes all elements in the specified collection from the current set.</summary>
		[ExcludeFromCodeCoverage]
		public void ExceptWith(IEnumerable<EmailAddress> other) { hashset.ExceptWith(other); }

		/// <summary>Modifies the current set so that it contains only elements that are also in a specified collection.</summary>
		[ExcludeFromCodeCoverage]
		public void IntersectWith(IEnumerable<EmailAddress> other) { hashset.IntersectWith(other); }

		/// <summary>Determines whether the current set is a proper (strict) subset of a specified collection.</summary>
		[ExcludeFromCodeCoverage]
		public bool IsProperSubsetOf(IEnumerable<EmailAddress> other) { return IsProperSubsetOf(other); }

		/// <summary>Determines whether the current set is a proper (strict) superset of a specified collection.</summary>
		[ExcludeFromCodeCoverage]
		public bool IsProperSupersetOf(IEnumerable<EmailAddress> other) { return IsProperSupersetOf(other); }

		/// <summary>Determines whether a set is a subset of a specified collection.</summary>
		[ExcludeFromCodeCoverage]
		public bool IsSubsetOf(IEnumerable<EmailAddress> other) { return IsSubsetOf(other); }

		/// <summary>Determines whether a set is a superset of a specified collection.</summary>
		[ExcludeFromCodeCoverage]
		public bool IsSupersetOf(IEnumerable<EmailAddress> other) { return IsSupersetOf(other); }

		/// <summary>Determines whether the current set overlaps with the specified collection.</summary>
		[ExcludeFromCodeCoverage]
		public bool Overlaps(IEnumerable<EmailAddress> other) { return Overlaps(other); }

		/// <summary>Determines whether the current set and the specified collection contain the same elements.</summary>
		[ExcludeFromCodeCoverage]
		public bool SetEquals(IEnumerable<EmailAddress> other) { return SetEquals(other); }

		/// <summary>Modifies the current set so that it contains only elements that are present
		/// either in the current set or in the specified collection, but not both.
		/// </summary>
		[ExcludeFromCodeCoverage]
		public void SymmetricExceptWith(IEnumerable<EmailAddress> other) { SymmetricExceptWith(other); }

		/// <summary>Modifies the current set so that it contains all elements that are present
		/// in either the current set or the specified collection.
		/// </summary>
		[ExcludeFromCodeCoverage]
		public void UnionWith(IEnumerable<EmailAddress> other) { UnionWith(other); }

		#endregion

		#region (XML) (De)serialization

		/// <summary>Initializes a new instance of email address based on the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		protected EmailAddressCollection(SerializationInfo info, StreamingContext context)
			: this()
		{
			Guard.NotNull(info, "info");
			AddRange(Parse(info.GetString("Value"), CultureInfo.InvariantCulture));
		}

		/// <summary>Adds the underlying property of email address to the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) { GetObjectData(info, context); }

		/// <summary>Adds the underlying property of email address to the serialization info.</summary>
		/// <remarks>
		/// this is used by ISerializable.GetObjectData() so that it can be changed by derived classes.
		/// </remarks>
		protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			Guard.NotNull(info, "info");
			info.AddValue("Value", ToString());
		}

		/// <summary>Gets the xml schema to (de) xml serialize an email address.</summary>
		/// <remarks>
		/// Returns null as no schema is required.
		/// </remarks>
		XmlSchema IXmlSerializable.GetSchema() { return GetSchema(); }

		/// <summary>Gets the xml schema to (de) xml serialize an email address.</summary>
		/// <remarks>
		/// this is used by IXmlSerializable.GetSchema() so that it can be changed by derived classes.
		/// </remarks>
		protected virtual XmlSchema GetSchema() { return null; }

		/// <summary>Reads the email address from an xml writer.</summary>
		/// <param name="reader">An xml reader.</param>
		void IXmlSerializable.ReadXml(XmlReader reader) { ReadXml(reader); }

		/// <summary>Reads the email address from an xml writer.</summary>
		/// <param name="reader">An xml reader.</param>
		/// <remarks>
		/// this is used by IXmlSerializable.ReadXml() so that it can be changed by derived classes.
		/// </remarks>
		protected virtual void ReadXml(XmlReader reader)
		{
			Guard.NotNull(reader, "reader");
			var s = reader.ReadElementString();
			var val = Parse(s, CultureInfo.InvariantCulture);
			AddRange(val);
		}

		/// <summary>Writes the email address to an xml writer.</summary>
		/// <param name="writer">An xml writer.</param>
		void IXmlSerializable.WriteXml(XmlWriter writer) { WriteXml(writer); }
		/// <summary>Writes the email address to an xml writer.</summary>
		/// <remarks>
		/// this is used by IXmlSerializable.WriteXml() so that it can be
		/// changed by derived classes.
		/// </remarks>
		protected virtual void WriteXml(XmlWriter writer)
		{
			Guard.NotNull(writer, "writer");
			writer.WriteString(ToString(CultureInfo.InvariantCulture));
		}

		#endregion

		#region (JSON) (De)serialization

		/// <summary>Generates an email address collection from a JSON null object representation.</summary>
		/// <remarks>
		/// As an email address collection is a reference type, this method
		/// should never be called. Instead, the read will return null.
		/// </remarks>
		[ExcludeFromCodeCoverage]
		void IJsonSerializable.FromJson() { }

		/// <summary>Generates an email address from a JSON string representation.</summary>
		/// <param name="jsonString">
		/// The JSON string that represents the email address.
		/// </param>
		void IJsonSerializable.FromJson(string jsonString)
		{
			AddRange(Parse(jsonString, CultureInfo.InvariantCulture));
		}

		/// <summary>Generates an email address collection from a JSON integer representation.</summary>
		/// <param name="jsonInteger">
		/// The JSON integer that represents the email address.
		/// </param>
		void IJsonSerializable.FromJson(Int64 jsonInteger) { FromJson(jsonInteger); }
		/// <summary>Generates an email address collection from a JSON integer representation.</summary>
		/// <remarks>
		/// this is used by IJsonSerializable.FromJson() so that it can be changed by derived classes.
		/// </remarks>
		protected virtual void FromJson(Int64 jsonInteger) { throw new NotSupportedException(QowaivMessages.JsonSerialization_Int64NotSupported); }

		/// <summary>Generates an email address from a JSON number representation.</summary>
		/// <param name="jsonNumber">
		/// The JSON number that represents the email address.
		/// </param>
		void IJsonSerializable.FromJson(Double jsonNumber) { FromJson(jsonNumber); }
		/// <summary>Generates an email address from a JSON number representation.</summary>
		/// <remarks>
		/// this is used by IJsonSerializable.FromJson() so that it can be changed by derived classes.
		/// </remarks>
		protected virtual void FromJson(Double jsonNumber) { throw new NotSupportedException(QowaivMessages.JsonSerialization_DoubleNotSupported); }

		/// <summary>Generates an email address from a JSON date representation.</summary>
		/// <param name="jsonDate">
		/// The JSON Date that represents the email address.
		/// </param>
		void IJsonSerializable.FromJson(DateTime jsonDate) { FromJson(jsonDate); }
		/// <summary>Generates an email address from a JSON date representation.</summary>
		/// <remarks>
		/// this is used by IJsonSerializable.FromJson() so that it can be changed by derived classes.
		/// </remarks>
		protected virtual void FromJson(DateTime jsonDate) { throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported); }

		/// <summary>Converts an email address into its JSON object representation.</summary>
		object IJsonSerializable.ToJson() { return ToJson(); }
		/// <summary>Converts an email address into its JSON object representation.</summary>
		/// <remarks>
		/// this is used by IJsonSerializable.FromJson() so that it can be changed by derived classes.
		/// </remarks>
		protected virtual object ToJson()
		{
			return Count == 0 ? null : ToString(CultureInfo.InvariantCulture);
		}
		#endregion

		#region IFormattable / ToString

		/// <summary>Returns a <see cref="string"/> that represents the current email address collection.</summary>
		public override string ToString()
		{
			return ToString(CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted <see cref="string"/> that represents the current email address collection.</summary>
		/// <param name="format">
		/// The format that this describes the formatting.
		/// </param>
		public string ToString(string format)
		{
			return ToString(format, CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted <see cref="string"/> that represents the current email address collection.</summary>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		public string ToString(IFormatProvider formatProvider)
		{
			return ToString("", formatProvider);
		}

		/// <summary>Returns a formatted <see cref="string"/> that represents the current email address collection.</summary>
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

			return String.Join(Separator, this.Select(emailaddress => emailaddress.ToString(format, formatProvider)));
		}

		#endregion

		#region Factory methods

		/// <summary>Converts the string to an email address collection.</summary>
		/// <param name="s">
		/// A string containing an email address to convert.
		/// </param>
		/// <returns>
		/// An email address.
		/// </returns>
		/// <exception cref="FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static EmailAddressCollection Parse(string s)
		{
			return Parse(s, CultureInfo.CurrentCulture);
		}

		/// <summary>Converts the string to an email address collection.</summary>
		/// <param name="s">
		/// A string containing an email address to convert.
		/// </param>
		/// <param name="formatProvider">
		/// The specified format provider.
		/// </param>
		/// <returns>
		/// An email address.
		/// </returns>
		/// <exception cref="FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static EmailAddressCollection Parse(string s, IFormatProvider formatProvider)
		{
			EmailAddressCollection val;
			if (EmailAddressCollection.TryParse(s, formatProvider, out val))
			{
				return val;
			}
			throw new FormatException(QowaivMessages.FormatExceptionEmailAddressCollection);
		}

		/// <summary>Converts the string to an email address collection.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing an email address to convert.
		/// </param>
		/// <returns>
		/// The email address if the string was converted successfully, otherwise an empty EmailAddressCollection().
		/// </returns>
		public static EmailAddressCollection TryParse(string s)
		{
			EmailAddressCollection val;
			if (EmailAddressCollection.TryParse(s, out val))
			{
				return val;
			}
			return new EmailAddressCollection();
		}

		/// <summary>Converts the string to an email address collection.
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
		public static bool TryParse(string s, out EmailAddressCollection result)
		{
			return TryParse(s, CultureInfo.CurrentCulture, out result);
		}

		/// <summary>Converts the string to an email address collection.
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
		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "2",
			Justification = "result is set when called Add().")]
		public static bool TryParse(string s, IFormatProvider formatProvider, out EmailAddressCollection result)
		{
			result = new EmailAddressCollection();
			if (string.IsNullOrEmpty(s))
			{
				return true;
			}
			var strs = s.Split(Separators, StringSplitOptions.RemoveEmptyEntries).Select(str => str.Trim());

			foreach (var str in strs)
			{
				EmailAddress email;
				if (EmailAddress.TryParse(str, formatProvider, out email))
				{
					result.Add(email);
				}
				else
				{
					result.Clear();
					return false;
				}
			}
			return result.Any();
		}

		#endregion
	}
}
