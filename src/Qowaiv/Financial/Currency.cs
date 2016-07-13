using Qowaiv.Conversion.Financial;
using Qowaiv.Formatting;
using Qowaiv.Globalization;
using Qowaiv.Json;
using Qowaiv.Threading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv.Financial
{
	/// <summary>Represents a currency.</summary>
	/// <remarks>
	/// A currency in the most specific use of the word refers to money in any
	/// form when in actual use or circulation as a medium of exchange,
	/// especially circulating banknotes and coins.
	///
	/// A much more general use of the word currency is anything that is used
	/// in any circumstances, as a medium of exchange. In this use, "currency"
	/// is a synonym for the concept of money.
	/// </remarks>
	[DebuggerDisplay("{DebuggerDisplay}")]
	[SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "The < and > operators have no meaning for a currency.")]
	[Serializable, SingleValueObject(SingleValueStaticOptions.All, typeof(string))]
	[TypeConverter(typeof(CurrencyTypeConverter))]
	public partial struct Currency : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IFormatProvider, IComparable, IComparable<Currency>
	{
		/// <summary>Represents an empty/not set currency.</summary>
		public static readonly Currency Empty;

		/// <summary>Represents an unknown (but set) currency.</summary>
		public static readonly Currency Unknown = new Currency() { m_Value = "ZZZ" };

		/// <summary>Gets a currency based on the current thread.</summary>
		public static Currency Current { get { return Thread.CurrentThread.GetValue<Currency>(); } }

		#region Properties

		/// <summary>The inner value of the currency.</summary>
		private string m_Value;

		/// <summary>Gets the name of the currency.</summary>
		public string Name { get { return IsUnknown() ? "?" : m_Value ?? string.Empty; } }

		/// <summary>Gets the display name.</summary>
		[SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods",
			Justification = "Property DisplayName is a shortcut for GetDisplayName(CultureInfo.CurrentCulture).")]
		public string DisplayName { get { return GetDisplayName(CultureInfo.CurrentCulture); } }

		/// <summary>Gets the full name of the currency in English.</summary>
		/// <returns>
		/// The full name of the currency in English.
		/// </returns>
		public string EnglishName { get { return GetDisplayName(CultureInfo.InvariantCulture); } }

		///<summary>Gets the code defined in ISO 4217 for the currency.</summary>
		public string IsoCode { get { return GetResourceString("ISO", CultureInfo.InvariantCulture); } }

		///<summary>Gets the numeric code defined in ISO 4217 for the currency.</summary>
		public int IsoNumericCode { get { return m_Value == default(string) ? 0 : XmlConvert.ToInt32(GetResourceString("Num", CultureInfo.InvariantCulture)); } }

		///<summary>Gets the symbol for a currency.</summary>
		public string Symbol { get { return m_Value == default(string) ? "" : GetResourceString("Symbol", CultureInfo.InvariantCulture); } }

		///<summary>Gets the number of after the decimal separator.</summary>
		public int Digits { get { return m_Value == default(string) ? 0 : XmlConvert.ToInt32(GetResourceString("Digits", CultureInfo.InvariantCulture)); } }

		/// <summary>Gets the start date from witch the currency exists.</summary>
		public Date StartDate { get { return m_Value == default(string) ? Date.MinValue : (Date)XmlConvert.ToDateTime(GetResourceString("StartDate", CultureInfo.InvariantCulture), "yyyy-MM-dd"); } }

		/// <summary>If the currency does not exist anymore, the end date is given, otherwise null.</summary>
		public Date? EndDate
		{
			get
			{
				var val = GetResourceString("EndDate", CultureInfo.InvariantCulture);
				return string.IsNullOrEmpty(val) ? null : (Date?)XmlConvert.ToDateTime(val, "yyyy-MM-dd");
			}
		}

		#endregion

		#region Methods

		/// <summary>Returns true if the currency is empty, otherwise false.</summary>
		public bool IsEmpty() { return m_Value == default(string); }

		/// <summary>Returns true if the currency is unknown, otherwise false.</summary>
		public bool IsUnknown() { return m_Value == Unknown.m_Value; }

		/// <summary>Returns true if the currency is empty or unknown, otherwise false.</summary>
		public bool IsEmptyOrUnknown() { return IsEmpty() || IsUnknown(); }

		/// <summary>Gets the display name for a specified culture.</summary>
		/// <param name="culture">
		/// The culture of the display name.
		/// </param>
		/// <returns></returns>
		public string GetDisplayName(CultureInfo culture) { return GetResourceString("DisplayName", culture); }

		/// <summary>Returns true if the currency exists at the given date, otherwise false.</summary>
		/// <param name="measurement">
		/// The date of existence.
		/// </param>
		public bool ExistsOnDate(Date measurement)
		{
			return this.StartDate <= measurement && (!this.EndDate.HasValue || this.EndDate.Value >= measurement);
		}

		/// <summary>Gets the countries using this currency at the given date.</summary>
		/// <param name="measurement">
		/// The date of measurement.
		/// </param>
		public IEnumerable<Country> GetCountries(Date measurement)
		{
			var currency = this;
			return Country.GetExisting(measurement)
				.Where(country => country.GetCurrency(measurement) == currency);
		}

		#endregion

		#region (XML) (De)serialization

		/// <summary>Initializes a new instance of currency based on the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		private Currency(SerializationInfo info, StreamingContext context)
		{
			Guard.NotNull(info, "info");
			m_Value = info.GetString("Value");
		}

		/// <summary>Adds the underlying property of currency to the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			Guard.NotNull(info, "info");
			info.AddValue("Value", m_Value);
		}

		/// <summary>Gets the xml schema to (de) xml serialize a currency.</summary>
		/// <remarks>
		/// Returns null as no schema is required.
		/// </remarks>
		XmlSchema IXmlSerializable.GetSchema() { return null; }

		/// <summary>Reads the currency from an xml writer.</summary>
		/// <remarks>
		/// Uses the string parse function of currency.
		/// </remarks>
		/// <param name="reader">An xml reader.</param>
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			Guard.NotNull(reader, "reader");
			var s = reader.ReadElementString();
			var val = Parse(s, CultureInfo.InvariantCulture);
			m_Value = val.m_Value;
		}

		/// <summary>Writes the currency to an xml writer.</summary>
		/// <remarks>
		/// Uses the string representation of currency.
		/// </remarks>
		/// <param name="writer">An xml writer.</param>
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			Guard.NotNull(writer, "writer");
			writer.WriteString(ToString(CultureInfo.InvariantCulture));
		}

		#endregion

		#region (JSON) (De)serialization

		/// <summary>Generates a currency from a JSON null object representation.</summary>
		void IJsonSerializable.FromJson()
		{
			m_Value = default(string);
		}

		/// <summary>Generates a currency from a JSON string representation.</summary>
		/// <param name="jsonString">
		/// The JSON string that represents the currency.
		/// </param>
		void IJsonSerializable.FromJson(string jsonString)
		{
			m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;
		}

		/// <summary>Generates a currency from a JSON integer representation.</summary>
		/// <param name="jsonInteger">
		/// The JSON integer that represents the currency.
		/// </param>
		void IJsonSerializable.FromJson(Int64 jsonInteger)
		{
			m_Value = Parse(jsonInteger.ToString("000", CultureInfo.InvariantCulture), CultureInfo.InvariantCulture).m_Value;
		}

		/// <summary>Generates a currency from a JSON number representation.</summary>
		/// <param name="jsonNumber">
		/// The JSON number that represents the currency.
		/// </param>
		void IJsonSerializable.FromJson(Double jsonNumber) { throw new NotSupportedException(QowaivMessages.JsonSerialization_DoubleNotSupported); }

		/// <summary>Generates a currency from a JSON date representation.</summary>
		/// <param name="jsonDate">
		/// The JSON Date that represents the currency.
		/// </param>
		void IJsonSerializable.FromJson(DateTime jsonDate) { throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported); }

		/// <summary>Converts a currency into its JSON object representation.</summary>
		object IJsonSerializable.ToJson()
		{
			return m_Value == default(string) ? null : ToString(CultureInfo.InvariantCulture);
		}

		#endregion

		#region IFormattable / ToString

		/// <summary>Returns a <see cref="string"/> that represents the current currency for debug purposes.</summary>
		private string DebuggerDisplay
		{
			get
			{
				if (IsEmpty()) { return "Currency: (empty)"; }
				if (IsUnknown()) { return "Currency: (unknown)"; }

				return string.Format(
				  CultureInfo.InvariantCulture,
				  "Currency: {0} ({1}/{2:000})",
				  EnglishName,
				  IsoCode,
				  IsoNumericCode
			  );
			}
		}

		/// <summary>Returns a <see cref="string"/> that represents the current currency.</summary>
		public override string ToString()
		{
			return ToString(CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted <see cref="string"/> that represents the current currency.</summary>
		/// <param name="format">
		/// The format that this describes the formatting.
		/// </param>
		public string ToString(string format)
		{
			return ToString(format, CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted <see cref="string"/> that represents the current currency.</summary>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		public string ToString(IFormatProvider formatProvider)
		{
			return ToString("", formatProvider);
		}

		/// <summary>Returns a formatted <see cref="string"/> that represents the current currency.</summary>
		/// <param name="format">
		/// The format that this describes the formatting.
		/// </param>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		/// <remarks>
		/// The formats:
		/// 
		/// n: as Name.
		/// i: as ISO Code.
		/// 0: as ISO Numeric.
		/// s/$: as symbol.
		/// e: as English name.
		/// f: as formatted/display name.
		/// </remarks>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			string formatted;
			if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out formatted))
			{
				return formatted;
			}

			// If no format specified, use the default format.
			if (string.IsNullOrEmpty(format)) { return this.Name; }

			// Apply the format.
			return StringFormatter.Apply(this, format, formatProvider, FormatTokens);
		}

		/// <summary>The format token instructions.</summary>
		private static readonly Dictionary<char, Func<Currency, IFormatProvider, string>> FormatTokens = new Dictionary<char, Func<Currency, IFormatProvider, string>>()
		{
			{ 'n', (svo, provider) => svo.Name },
			{ 'i', (svo, provider) => svo.IsoCode },
			{ '0', (svo, provider) => svo.IsoNumericCode.ToString("000", provider) },
			{ 's', (svo, provider) => svo.Symbol },
			{ '$', (svo, provider) => svo.Symbol },
			{ 'e', (svo, provider) => svo.EnglishName },
			{ 'f', (svo, provider) => svo.GetResourceString("DisplayName", provider) },
		};

		#endregion

		#region IFormatter
				
		object IFormatProvider.GetFormat(Type formatType)
		{
			if (formatType != typeof(NumberFormatInfo)) { return null; }
			return GetNumberFormatInfo(CultureInfo.CurrentCulture);
		}

		/// <summary>Gets a <see cref="NumberFormatInfo"/> based on the <see cref="IFormatProvider"/>.</summary>
		/// <remarks>
		/// Because the options for formatting and parsing currencies as provided 
		/// by the .NET framework are not sufficient, internally we use number
		/// settings. For parsing and formatting however we like to use the
		/// currency properties of the <see cref="NumberFormatInfo"/> instead of
		/// the number properties, so we copy them for desired behavior.
		/// </remarks>
		internal NumberFormatInfo GetNumberFormatInfo(IFormatProvider formatProvider)
		{
			var info = NumberFormatInfo.GetInstance(formatProvider);
			info = (NumberFormatInfo)info.Clone();
			info.CurrencySymbol = string.IsNullOrEmpty(Symbol) ? IsoCode : Symbol;
			info.CurrencyDecimalDigits = Digits;
			return info;
		}

		#endregion

		#region IEquatable

		/// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
		/// <param name="obj">An object to compare with.</param>
		public override bool Equals(object obj) { return base.Equals(obj); }

		/// <summary>Returns the hash code for this currency.</summary>
		/// <returns>
		/// A 32-bit signed integer hash code.
		/// </returns>
		public override int GetHashCode() { return m_Value == null ? 0 : m_Value.GetHashCode(); }

		/// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator ==(Currency left, Currency right)
		{
			return left.Equals(right);
		}

		/// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator !=(Currency left, Currency right)
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
		/// An object that evaluates to a currency.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.Value
		/// Condition Less than zero This instance precedes value. Zero This instance
		/// has the same position in the sort order as value. Greater than zero This
		/// instance follows value.-or- value is null.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// value is not a currency.
		/// </exception>
		public int CompareTo(object obj)
		{
			if (obj is Currency)
			{
				return CompareTo((Currency)obj);
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.ArgumentException_Must, "a currency"), "obj");
		}

		/// <summary>Compares this instance with a specified currency and indicates
		/// whether this instance precedes, follows, or appears in the same position
		/// in the sort order as the specified currency.
		/// </summary>
		/// <param name="other">
		/// The currency to compare with this instance.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.
		/// </returns>
		public int CompareTo(Currency other) { return String.Compare(m_Value, other.m_Value, StringComparison.Ordinal); }

		#endregion

		#region (Explicit) casting

		/// <summary>Casts a currency to a <see cref="string"/>.</summary>
		public static explicit operator string(Currency val) { return val.ToString(CultureInfo.CurrentCulture); }
		/// <summary>Casts a <see cref="string"/> to a currency.</summary>
		public static explicit operator Currency(string str) { return Currency.Parse(str, CultureInfo.CurrentCulture); }

		/// <summary>Casts a currency to a <see cref="string"/>.</summary>
		public static explicit operator int(Currency val) { return val.IsoNumericCode; }

		/// <summary>Casts a <see cref="string"/> to a currency.</summary>
		public static explicit operator Currency(int val) { return AllCurrencies.FirstOrDefault(c => c.IsoNumericCode == val); }

		#endregion

		#region Factory methods

		/// <summary>Converts the string to a currency.</summary>
		/// <param name="s">
		/// A string containing a currency to convert.
		/// </param>
		/// <returns>
		/// A currency.
		/// </returns>
		/// <exception cref="FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static Currency Parse(string s)
		{
			return Parse(s, CultureInfo.CurrentCulture);
		}

		/// <summary>Converts the string to a currency.</summary>
		/// <param name="s">
		/// A string containing a currency to convert.
		/// </param>
		/// <param name="formatProvider">
		/// The specified format provider.
		/// </param>
		/// <returns>
		/// A currency.
		/// </returns>
		/// <exception cref="FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static Currency Parse(string s, IFormatProvider formatProvider)
		{
			Currency val;
			if (TryParse(s, formatProvider, out val))
			{
				return val;
			}
			throw new FormatException(QowaivMessages.FormatExceptionCurrency);
		}

		/// <summary>Converts the string to a currency.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a currency to convert.
		/// </param>
		/// <returns>
		/// The currency if the string was converted successfully, otherwise Currency.Empty.
		/// </returns>
		public static Currency TryParse(string s)
		{
			Currency val;
			if (TryParse(s, out val))
			{
				return val;
			}
			return Empty;
		}

		/// <summary>Converts the string to a currency.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a currency to convert.
		/// </param>
		/// <param name="result">
		/// The result of the parsing.
		/// </param>
		/// <returns>
		/// True if the string was converted successfully, otherwise false.
		/// </returns>
		public static bool TryParse(string s, out Currency result)
		{
			return TryParse(s, CultureInfo.CurrentCulture, out result);
		}

		/// <summary>Converts the string to a currency.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a currency to convert.
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
		public static bool TryParse(string s, IFormatProvider formatProvider, out Currency result)
		{
			result = Empty;
			if (string.IsNullOrEmpty(s))
			{
				return true;
			}
			var culture = formatProvider as CultureInfo ?? CultureInfo.InvariantCulture;
			if (Qowaiv.Unknown.IsUnknown(s, culture) || s == Unknown.Symbol)
			{
				result = Unknown;
				return true;
			}
			AddCulture(culture);

			var str = Parsing.ToUnified(s);
			string val;
			
			if (Parsings[culture].TryGetValue(str, out val) || Parsings[CultureInfo.InvariantCulture].TryGetValue(str, out val))
			{
				result = new Currency() { m_Value = val };
				return true;
			}
			foreach (var currency in AllCurrencies.Where(c => !string.IsNullOrEmpty(c.Symbol)))
			{
				if (currency.Symbol == str)
				{
					result = currency;
					return true;
				}
			}
			return false;
		}

		#endregion

		#region Validation

		/// <summary>Returns true if the val represents a valid currency, otherwise false.</summary>
		public static bool IsValid(string val)
		{
			return IsValid(val, CultureInfo.CurrentCulture);
		}

		/// <summary>Returns true if the val represents a valid currency, otherwise false.</summary>
		public static bool IsValid(string val, IFormatProvider formatProvider)
		{
			var culture = formatProvider as CultureInfo ?? CultureInfo.InvariantCulture;

			if (string.IsNullOrWhiteSpace(val) || Qowaiv.Unknown.IsUnknown(val, culture)) { return false; }

			AddCulture(culture);

			var str = Parsing.ToUnified(val);
			return Parsings[culture].ContainsKey(str) || Parsings[CultureInfo.InvariantCulture].ContainsKey(str);
		}
		#endregion

		#region Get currencies

		/// <summary>Gets all existing currencies.</summary>
		/// <returns>
		/// A list of existing currencies.
		/// </returns>
		public static IEnumerable<Currency> GetExisting()
		{
			return GetExisting(Date.Today);
		}

		/// <summary>Gets all countries existing on the specified measurement date.</summary>
		/// <param name="measurement">
		/// The measurement date.
		/// </param>
		/// <returns>
		/// A list of existing countries.
		/// </returns>
		public static IEnumerable<Currency> GetExisting(Date measurement)
		{
			return AllCurrencies.Where(currency => currency.ExistsOnDate(measurement));
		}

		/// <summary>Gets a collection of all country info's.</summary>
		/// <remarks>
		/// We'd like to call this All, but because of CLS-compliance, we can not,
		/// because ALL exists.
		/// </remarks>
		[SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes",
			Justification = "ReadOnlyCollection<T> is immutable.")]
		public static readonly ReadOnlyCollection<Currency> AllCurrencies = new ReadOnlyCollection<Currency>(
			ResourceManager
				.GetString("All")
				.Split(';')
				.Select(str => new Currency() { m_Value = str })
				.ToList());

		#endregion

		#region Resources

		/// <summary>This is done so that it will be available when called by another initialization.</summary>
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (s_ResourceManager == null)
				{
					ResourceManager temp = new ResourceManager("Qowaiv.Financial.CurrencyLabels", typeof(Currency).Assembly);
					s_ResourceManager = temp;
				}
				return s_ResourceManager;
			}
		}
		private static ResourceManager s_ResourceManager;


		/// <summary>Get resource string.</summary>
		/// <param name="postfix">
		/// The prefix of the resource key.
		/// </param>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		internal string GetResourceString(string postfix, IFormatProvider formatProvider)
		{
			return GetResourceString(postfix, formatProvider as CultureInfo);
		}

		/// <summary>Get resource string.</summary>
		/// <param name="postfix">
		/// The prefix of the resource key.
		/// </param>
		/// <param name="culture">
		/// The culture.
		/// </param>
		internal string GetResourceString(string postfix, CultureInfo culture)
		{
			if (m_Value == default(string)) { return string.Empty; }
			return ResourceManager.GetString(m_Value + '_' + postfix, culture ?? CultureInfo.CurrentCulture) ?? string.Empty;
		}

		#endregion

		#region Lookup

		/// <summary>Initializes the country lookup.</summary>
		[SuppressMessage("Microsoft.Performance", "CA1809:AvoidExcessiveLocals",
			Justification = "Those constants are the hole point of this class.")]
		[SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode",
			Justification = "Due to generated constants.")]
		[SuppressMessage("Microsoft.Usage", "CA2207:InitializeValueTypeStaticFieldsInline",
			Justification = "Complex initialization, this approach is better understandable.")]
		static Currency()
		{
			foreach (var country in AllCurrencies)
			{
				Parsings[CultureInfo.InvariantCulture][country.IsoCode.ToUpperInvariant()] = country.m_Value;
				Parsings[CultureInfo.InvariantCulture][country.IsoNumericCode.ToString("000", CultureInfo.InvariantCulture)] = country.m_Value;
				Parsings[CultureInfo.InvariantCulture][Parsing.ToUnified(country.GetDisplayName(CultureInfo.InvariantCulture))] = country.m_Value;
			}
		}

		/// <summary>Adds a culture to the parsings.</summary>
		/// <param name="culture">
		/// The culture to add.
		/// </param>
		private static void AddCulture(CultureInfo culture)
		{
			lock (locker)
			{
				if (Parsings.ContainsKey(culture)) { return; }

				Parsings[culture] = new Dictionary<string, string>();

				Parsings[culture][Unknown.GetDisplayName(culture)] = Unknown.m_Value;

				foreach (var country in Currency.AllCurrencies)
				{
					Parsings[culture][Parsing.ToUnified(country.GetDisplayName(culture))] = country.m_Value;
				}
			}
		}

		/// <summary>Represents the parsing keys.</summary>
		private static readonly Dictionary<CultureInfo, Dictionary<string, string>> Parsings = new Dictionary<CultureInfo, Dictionary<string, string>>()
		{
			{
				CultureInfo.InvariantCulture, new Dictionary<string, string>()
				{
					{ "ZZZ", "ZZZ" },
					{ "999", "ZZZ" },
					{ "unknown", "ZZZ" },
				}
			},
		};

		/// <summary>The locker for adding a culture.</summary>
		private static volatile object locker = new object();

		#endregion

		#region Money creation operators

		/// <summary>Creates money based on the amount and the currency.</summary>
		public static Money operator +(Amount val, Currency currency) { return Money.Create(val, currency); }
		/// <summary>Creates money based on the amount and the currency.</summary>
		public static Money operator +(decimal val, Currency currency) { return Money.Create(val, currency); }
		/// <summary>Creates money based on the amount and the currency.</summary>
		public static Money operator +(double val, Currency currency) { return Money.Create(val, currency); }
		/// <summary>Creates money based on the amount and the currency.</summary>
		public static Money operator +(int val, Currency currency) { return Money.Create(val, currency); }

		#endregion
	}
}