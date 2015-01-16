using Qowaiv.Conversion;
using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	/// <summary>Represents a month.</summary>
	[DebuggerDisplay("{DebuggerDisplay}")]
	[SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "The < and > operators have no meaning for a month.")]
	[Serializable, SingleValueObject(SingleValueStaticOptions.All, typeof(Byte))]
	[TypeConverter(typeof(MonthTypeConverter))]
	public struct Month : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IComparable, IComparable<Month>
	{
		/// <summary>Represents the pattern of a (potential) valid month.</summary>
		public static readonly Regex Pattern = new Regex(@"^(0?[1-9]|10|11|12)$", RegexOptions.Compiled);

		/// <summary>Represents an empty/not set month.</summary>
		public static readonly Month Empty = default(Month);

		/// <summary>Represents an unknown (but set) month.</summary>
		public static readonly Month Unknown = new Month() { m_Value = Byte.MaxValue };

		/// <summary>Represents January (01).</summary>
		public static readonly Month January = new Month() { m_Value = 1 };
		/// <summary>Represents February (02).</summary>
		public static readonly Month February = new Month() { m_Value = 2 };
		/// <summary>Represents March (03).</summary>
		public static readonly Month March = new Month() { m_Value = 3 };
		/// <summary>Represents April (04).</summary>
		public static readonly Month April = new Month() { m_Value = 4 };
		/// <summary>Represents May (05).</summary>
		public static readonly Month May = new Month() { m_Value = 5 };
		/// <summary>Represents June (06).</summary>
		public static readonly Month June = new Month() { m_Value = 6 };
		/// <summary>Represents July (07).</summary>
		public static readonly Month July = new Month() { m_Value = 7 };
		/// <summary>Represents August (08).</summary>
		public static readonly Month August = new Month() { m_Value = 8 };
		/// <summary>Represents September (09).</summary>
		public static readonly Month September = new Month() { m_Value = 9 };
		/// <summary>Represents October (10).</summary>
		public static readonly Month October = new Month() { m_Value = 10 };
		/// <summary>Represents November (11).</summary>
		public static readonly Month November = new Month() { m_Value = 11 };
		/// <summary>Represents December (12).</summary>
		public static readonly Month December = new Month() { m_Value = 12 };

		/// <summary>Represents all months (January till December).</summary>
		public static readonly ReadOnlyCollection<Month> All = new ReadOnlyCollection<Month>(
			new List<Month>()
			{
				Month.January,
				Month.February,
				Month.March,
				Month.April,
				Month.May,
				Month.June,
				Month.July,
				Month.August,
				Month.September,
				Month.October,
				Month.November,
				Month.December,
			});

		#region Properties

		/// <summary>The inner value of the month.</summary>
		private Byte m_Value;

		/// <summary>Gets the full name of the month.</summary>
		public string FullName { get { return GetFullName(CultureInfo.CurrentCulture); } }

		/// <summary>Gets the short name of the month.</summary>
		public string ShortName { get { return GetShortName(CultureInfo.CurrentCulture); } }

		#endregion

		#region Methods

		/// <summary>Returns true if the month is empty, otherwise false.</summary>
		public bool IsEmpty() { return m_Value == default(System.Byte); }

		/// <summary>Returns true if the month is unknown, otherwise false.</summary>
		public bool IsUnknown() { return m_Value == Month.Unknown.m_Value; }

		/// <summary>Returns true if the month is empty or unknown, otherwise false.</summary>
		public bool IsEmptyOrUnknown() { return IsEmpty() || IsUnknown(); }



		/// <summary>Gets the full name of the month.</summary>
		public string GetFullName(IFormatProvider formatProvider)
		{
			return IsEmptyOrUnknown() ? ToDefaultString() : (formatProvider as CultureInfo ?? CultureInfo.InvariantCulture).DateTimeFormat.GetMonthName(m_Value);
		}

		/// <summary>Gets the short name of the month.</summary>
		public string GetShortName(IFormatProvider formatProvider)
		{
			return IsEmptyOrUnknown() ? ToDefaultString() : (formatProvider as CultureInfo ?? CultureInfo.InvariantCulture).DateTimeFormat.GetAbbreviatedMonthName(m_Value);
		}

		#endregion

		#region (XML) (De)serialization

		/// <summary>Initializes a new instance of month based on the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		private Month(SerializationInfo info, StreamingContext context)
		{
			if (info == null) { throw new ArgumentNullException("info"); }
			m_Value = info.GetByte("Value");
		}

		/// <summary>Adds the underlying propererty of month to the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null) { throw new ArgumentNullException("info"); }
			info.AddValue("Value", m_Value);
		}

		/// <summary>Gets the xml schema to (de) xml serialize a month.</summary>
		/// <remarks>
		/// Returns null as no schema is required.
		/// </remarks>
		XmlSchema IXmlSerializable.GetSchema() { return null; }

		/// <summary>Reads the month from an xml writer.</summary>
		/// <remarks>
		/// Uses the string parse function of month.
		/// </remarks>
		/// <param name="reader">An xml reader.</param>
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			var s = reader.ReadElementString();
			var val = Parse(s, CultureInfo.InvariantCulture);
			m_Value = val.m_Value;
		}

		/// <summary>Writes the month to an xml writer.</summary>
		/// <remarks>
		/// Uses the string representation of month.
		/// </remarks>
		/// <param name="writer">An xml writer.</param>
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			writer.WriteString(ToString(CultureInfo.InvariantCulture));
		}

		#endregion

		#region (JSON) (De)serialization

		/// <summary>Generates a month from a JSON null object representation.</summary>
		void IJsonSerializable.FromJson()
		{
			m_Value = default(Byte);
		}

		/// <summary>Generates a month from a JSON string representation.</summary>
		/// <param name="jsonString">
		/// The JSON string that represents the month.
		/// </param>
		void IJsonSerializable.FromJson(String jsonString)
		{
			m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;
		}

		/// <summary>Generates a month from a JSON integer representation.</summary>
		/// <param name="jsonInteger">
		/// The JSON integer that represents the month.
		/// </param>
		void IJsonSerializable.FromJson(Int64 jsonInteger)
		{
			m_Value = Create((Int32)jsonInteger).m_Value;
		}

		/// <summary>Generates a month from a JSON number representation.</summary>
		/// <param name="jsonNumber">
		/// The JSON number that represents the month.
		/// </param>
		void IJsonSerializable.FromJson(Double jsonNumber)
		{
			m_Value = Create((Int32)jsonNumber).m_Value;
		}

		/// <summary>Generates a month from a JSON date representation.</summary>
		/// <param name="jsonDate">
		/// The JSON Date that represents the month.
		/// </param>
		void IJsonSerializable.FromJson(DateTime jsonDate) { throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported); }

		/// <summary>Converts a month into its JSON object representation.</summary>
		object IJsonSerializable.ToJson()
		{
			return m_Value == default(Byte) ? null : ToString(CultureInfo.InvariantCulture);
		}

		#endregion

		#region IFormattable / ToString

		/// <summary>Returns a System.String that represents the current month for debug purposes.</summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string DebuggerDisplay
		{
			get
			{
				if (IsEmpty()) { return "Month: (empty)"; }
				if (IsUnknown()) { return "Month: (unknown)"; }
				return string.Format(CultureInfo.InvariantCulture, "Month: {0:f} ({0:m})", this);
			}
		}

		/// <summary>Returns a System.String that represents the current month.</summary>
		public override string ToString()
		{
			return ToString(CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted System.String that represents the current month.</summary>
		/// <param name="format">
		/// The format that this describes the formatting.
		/// </param>
		public string ToString(string format)
		{
			return ToString(format, CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted System.String that represents the current month.</summary>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		public string ToString(IFormatProvider formatProvider)
		{
			return ToString("", formatProvider);
		}

		/// <summary>Returns a formatted System.String that represents the current month.</summary>
		/// <param name="format">
		/// The format that this describes the formatting.
		/// </param>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		/// <remarks>
		/// The formats:
		/// 
		/// f: as full name.
		/// s: as short name.
		/// M: as number with leading zero.
		/// m: as number without leading zero.
		/// </remarks>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			string formatted;
			if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out formatted))
			{
				return formatted;
			}
			// Apply the format.
			return StringFormatter.Apply(this, String.IsNullOrEmpty(format) ? "M" : format, formatProvider ?? CultureInfo.CurrentCulture, FormatTokens);
		}

		private string ToDefaultString() { return IsUnknown() ? "?" : String.Empty; }

		/// <summary>The format token instructions.</summary>
		private static readonly Dictionary<char, Func<Month, IFormatProvider, string>> FormatTokens = new Dictionary<char, Func<Month, IFormatProvider, string>>()
		{
			{ 'f', (svo, provider) => svo.GetFullName(provider) },
			{ 's', (svo, provider) => svo.GetShortName(provider) },
			{ 'M', (svo, provider) => svo.IsEmptyOrUnknown() ? svo.ToDefaultString() : svo.m_Value.ToString("0", provider) },
			{ 'm', (svo, provider) => svo.IsEmptyOrUnknown() ? svo.ToDefaultString() : svo.m_Value.ToString("00", provider) },
		};

		#endregion

		#region IEquatable

		/// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
		/// <param name="obj">An object to compair with.</param>
		public override bool Equals(object obj) { return base.Equals(obj); }

		/// <summary>Returns the hash code for this month.</summary>
		/// <returns>
		/// A 32-bit signed integer hash code.
		/// </returns>
		public override int GetHashCode() { return m_Value.GetHashCode(); }

		/// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator ==(Month left, Month right)
		{
			return left.Equals(right);
		}

		/// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator !=(Month left, Month right)
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
		/// An object that evaluates to a month.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.Value
		/// Condition Less than zero This instance precedes value. Zero This instance
		/// has the same position in the sort order as value. Greater than zero This
		/// instance follows value.-or- value is null.
		/// </returns>
		/// <exception cref="System.ArgumentException">
		/// value is not a month.
		/// </exception>
		public int CompareTo(object obj)
		{
			if (obj is Month)
			{
				return CompareTo((Month)obj);
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.AgrumentException_Must, "a month"), "obj");
		}

		/// <summary>Compares this instance with a specified month and indicates
		/// whether this instance precedes, follows, or appears in the same position
		/// in the sort order as the specified month.
		/// </summary>
		/// <param name="other">
		/// The month to compare with this instance.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.
		/// </returns>
		public int CompareTo(Month other) { return m_Value.CompareTo(other.m_Value); }


		/// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
		public static bool operator <(Month l, Month r) { return l.CompareTo(r) < 0; }

		/// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
		public static bool operator >(Month l, Month r) { return l.m_Value > r.m_Value; }

		/// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
		public static bool operator <=(Month l, Month r) { return l.m_Value <= r.m_Value; }

		/// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
		public static bool operator >=(Month l, Month r) { return l.m_Value >= r.m_Value; }

		#endregion

		#region (Explicit) casting

		/// <summary>Casts a month to a System.String.</summary>
		public static explicit operator string(Month val) { return val.ToString(CultureInfo.CurrentCulture); }
		/// <summary>Casts a System.String to a month.</summary>
		public static explicit operator Month(string str) { return Month.Parse(str, CultureInfo.CurrentCulture); }



		/// <summary>Casts a month to a System.Int32.</summary>
		public static explicit operator Int32(Month val) { return val.m_Value; }
		/// <summary>Casts an System.Int32 to a month.</summary>
		public static implicit operator Month(Int32 val) { return Month.Create(val); }

		#endregion

		#region Factory methods

		/// <summary>Converts the string to a month.</summary>
		/// <param name="s">
		/// A string containing a month to convert.
		/// </param>
		/// <returns>
		/// A month.
		/// </returns>
		/// <exception cref="System.FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static Month Parse(string s)
		{
			return Parse(s, CultureInfo.CurrentCulture);
		}

		/// <summary>Converts the string to a month.</summary>
		/// <param name="s">
		/// A string containing a month to convert.
		/// </param>
		/// <param name="formatProvider">
		/// The specified format provider.
		/// </param>
		/// <returns>
		/// A month.
		/// </returns>
		/// <exception cref="System.FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static Month Parse(string s, IFormatProvider formatProvider)
		{
			Month val;
			if (Month.TryParse(s, formatProvider, out val))
			{
				return val;
			}
			throw new FormatException(QowaivMessages.FormatExceptionMonth);
		}

		/// <summary>Converts the string to a month.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a month to convert.
		/// </param>
		/// <returns>
		/// The month if the string was converted successfully, otherwise Month.Empty.
		/// </returns>
		public static Month TryParse(string s)
		{
			Month val;
			if (Month.TryParse(s, out val))
			{
				return val;
			}
			return Month.Empty;
		}

		/// <summary>Converts the string to a month.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a month to convert.
		/// </param>
		/// <param name="result">
		/// The result of the parsing.
		/// </param>
		/// <returns>
		/// True if the string was converted successfully, otherwise false.
		/// </returns>
		public static bool TryParse(string s, out Month result)
		{
			return TryParse(s, CultureInfo.CurrentCulture, out result);
		}

		/// <summary>Converts the string to a month.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a month to convert.
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
		public static bool TryParse(string s, IFormatProvider formatProvider, out Month result)
		{
			result = Month.Empty;
			if (string.IsNullOrEmpty(s))
			{
				return true;
			}
			if (Qowaiv.Unknown.IsUnknown(s))
			{
				result = Month.Unknown;
				return true;
			}
			if (Pattern.IsMatch(s))
			{
				result = new Month() { m_Value = System.Byte.Parse(s, formatProvider) };
				return true;
			}
			else
			{
				var culture = formatProvider as CultureInfo ?? CultureInfo.InvariantCulture;
				AddCulture(culture);

				Byte m;
				var str = Parsing.ToUnified(s);
				if (Parsings[culture].TryGetValue(str, out m) ||
					Parsings[CultureInfo.InvariantCulture].TryGetValue(str, out m))
				{
					result = new Month() { m_Value = m };
					return true;
				}
			}
			return false;
		}

		///  <summary >Creates a month from a Byte. </summary >
		///  <param name="val" >
		/// A decimal describing a month.
		///  </param >
		///  <exception cref="System.FormatException" >
		/// val is not a valid month.
		///  </exception >
		public static Month Create(Int32? val)
		{
			Month result;
			if (Month.TryCreate(val, out result))
			{
				return result;
			}
			throw new ArgumentOutOfRangeException("val", QowaivMessages.FormatExceptionMonth);
		}

		///  <summary >Creates a month from a Byte.
		/// A return value indicates whether the conversion succeeded.
		///  </summary >
		///  <param name="val" >
		/// A decimal describing a month.
		///  </param >
		///  <returns >
		/// A month if the creation was successfully, otherwise Month.Empty.
		///  </returns >
		public static Month TryCreate(Int32? val)
		{
			Month result;
			if (TryCreate(val, out result))
			{
				return result;
			}
			return Month.Empty;
		}

		///  <summary >Creates a month from a Byte.
		/// A return value indicates whether the creation succeeded.
		///  </summary >
		///  <param name="val" >
		/// A Byte describing a month.
		///  </param >
		///  <param name="result" >
		/// The result of the creation.
		///  </param >
		///  <returns >
		/// True if a month was created successfully, otherwise false.
		///  </returns >
		public static bool TryCreate(Int32? val, out Month result)
		{
			result = Month.Empty;

			if (!val.HasValue)
			{
				return true;
			}
			if (IsValid(val.Value))
			{
				result = new Month() { m_Value = (Byte)val.Value };
				return true;
			}
			return false;
		}

		#endregion

		#region Validation

		/// <summary>Returns true if the val represents a valid month, otherwise false.</summary>
		public static bool IsValid(string val)
		{
			return IsValid(val, CultureInfo.CurrentCulture);
		}

		/// <summary>Returns true if the val represents a valid month, otherwise false.</summary>
		public static bool IsValid(string val, IFormatProvider formatProvider)
		{
			AddCulture(formatProvider as CultureInfo);

			var str = Parsing.ToUnified(val);
			var culture = formatProvider as CultureInfo;
			return
				Pattern.IsMatch(val ?? string.Empty) ||
				(culture != null && Parsings[culture].ContainsKey(str)) ||
				Parsings[CultureInfo.InvariantCulture].ContainsKey(str);
		}

		/// <summary>Returns true if the val represents a valid month, otherwise false.</summary>
		public static bool IsValid(Int32? val)
		{
			if (!val.HasValue) { return false; }
			return val.Value >= January.m_Value && val.Value <= December.m_Value;
		}
		#endregion

		#region Lookup

		private static void AddCulture(CultureInfo culture)
		{
			lock (locker)
			{
				if (culture == null || Parsings.ContainsKey(culture)) { return; }

				Parsings[culture] = new Dictionary<string, byte>();

				for (byte m = 1; m <= 12; m++)
				{
					var full = Parsing.ToUnified(culture.DateTimeFormat.GetAbbreviatedMonthName(m));
					var shrt = Parsing.ToUnified(culture.DateTimeFormat.GetMonthName(m));

					if (!Parsings[CultureInfo.InvariantCulture].ContainsKey(full))
					{
						Parsings[culture][full] = m;
					}
					if (!Parsings[CultureInfo.InvariantCulture].ContainsKey(shrt))
					{
						Parsings[culture][shrt] = m;
					}
				}
			}
		}

		/// <summary>Represents the parsing keys.</summary>
		private static readonly Dictionary<CultureInfo, Dictionary<string, byte>> Parsings = new Dictionary<CultureInfo, Dictionary<string, byte>>()
		{
			{
				CultureInfo.InvariantCulture, new Dictionary<string, byte>()
				{
					{"january", 1},
					{"february", 2},
					{"march", 3},
					{"april", 4},
					{"may", 5},
					{"june", 6},
					{"july", 7},
					{"august", 8},
					{"september", 9},
					{"october", 10},
					{"november", 11}, 
					{"december", 12}, 
					{"jan", 1},
					{"feb", 2},
					{"mar", 3},
					{"apr", 4},
					{"jun", 6},
					{"jul", 7},
					{"aug", 8},
					{"sep", 9},
					{"oct", 10},
					{"nov", 11},
					{"dec", 12},
				}
			}
		};

		/// <summary>The locker for adding a culture.</summary>
		private static volatile object locker = new object();

		#endregion
	}
}
