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
using System.Resources;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv
{
	/// <summary>Represents a Gender.</summary>
	/// <remarks>
	/// Defines a representation of human sexes through a language-neutral 
	/// single-digit code. International standard ISO 5218, titled Information
	/// technology - Codes for the representation of human sexes.
	/// 
	/// It can be used in information systems such as database applications.
	/// 
	/// The four codes specified in ISO/IEC 5218 are:
	/// 0 = not known,
	/// 1 = male,
	/// 2 = female,
	/// 9 = not applicable.
	/// The standard specifies that its use may be referred to by the
	/// designator "SEX".
	/// </remarks>
	[DebuggerDisplay("{DebuggerDisplay}")]
	[Serializable, SingleValueObject(SingleValueStaticOptions.All, typeof(Byte))]
	[TypeConverter(typeof(GenderTypeConverter))]
	public struct Gender : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IComparable, IComparable<Gender>
	{
		/// <summary>Represents an empty/not set Gender.</summary>
		public static readonly Gender Empty = default(Gender);

		/// <summary>Represents a not known/unknown gender.</summary>
		public static readonly Gender Unknown = new Gender() { m_Value = 1 };
		/// <summary>Represents a male.</summary>
		public static readonly Gender Male = new Gender() { m_Value = 2 };
		/// <summary>Represents a female.</summary>
		public static readonly Gender Female = new Gender() { m_Value = 4 };
		/// <summary>Represents a not applicable gender.</summary>
		public static readonly Gender NotApplicable = new Gender() { m_Value = 18 };

		/// <summary>Contains not known, male, female, not applicable.</summary>
		public static readonly ReadOnlyCollection<Gender> All = new ReadOnlyCollection<Gender>(new List<Gender>() { Gender.Male, Gender.Female, Gender.NotApplicable, Gender.Unknown });

		/// <summary>Contains male and female.</summary>
		public static readonly ReadOnlyCollection<Gender> MaleAndFemale = new ReadOnlyCollection<Gender>(new List<Gender>() { Gender.Male, Gender.Female });

		/// <summary>Contains male, female, not applicable.</summary>
		public static readonly ReadOnlyCollection<Gender> MaleFemaleAndNotApplicable = new ReadOnlyCollection<Gender>(new List<Gender>() { Gender.Male, Gender.Female, Gender.NotApplicable });

		#region Properties

		/// <summary>The inner value of the Gender.</summary>
		private Byte m_Value;

		/// <summary>Gets the displayname.</summary>
		public string DisplayName { get { return GetDisplayName(CultureInfo.CurrentCulture); } }

		#endregion

		#region Methods

		/// <summary>Returns true if the Gender is empty, otherwise false.</summary>
		public bool IsEmpty() { return m_Value == default(Byte); }

		/// <summary>Returns true if the Gender is unknown, otherwise false.</summary>
		public bool IsUnknown() { return m_Value == Gender.Unknown.m_Value; }

		/// <summary>Returns true if the Gender is empty or unknown, otherwise false.</summary>
		public bool IsEmptyOrUnknown() { return IsEmpty() || IsUnknown(); }

		/// <summary>Returns true if the Gender is male or female, otherwise false.</summary>
		public bool IsMaleOrFemale() { return this == Gender.Male || this == Gender.Female; }

		/// <summary>Gets the displayname for a specified culture.</summary>
		/// <param name="culture">
		/// The culture of the display name.
		/// </param>
		/// <returns></returns>
		public string GetDisplayName(CultureInfo culture) { return GetResourceString("", culture); }
	   
		/// <summary>Converts the Gender to an Int32.</summary>
		private Int32 ToInt32() { return m_Value >> 1; }
		
		/// <summary>Converts the Gender to an Int32.</summary>
		private Int32? ToNullableInt32() { return ToNullableInt32s[m_Value]; }

		#endregion

		#region (XML) (De)serialization

		/// <summary>Initializes a new instance of Gender based on the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		private Gender(SerializationInfo info, StreamingContext context)
		{
			Guard.NotNull(info, "info");
			m_Value = info.GetByte("Value");
		}

		/// <summary>Adds the underlying property of Gender to the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			Guard.NotNull(info, "info");
			info.AddValue("Value", m_Value);
		}

		/// <summary>Gets the xml schema to (de) xml serialize a Gender.</summary>
		/// <remarks>
		/// Returns null as no schema is required.
		/// </remarks>
		XmlSchema IXmlSerializable.GetSchema() { return null; }

		/// <summary>Reads the Gender from an xml writer.</summary>
		/// <remarks>
		/// Uses the string parse function of Gender.
		/// </remarks>
		/// <param name="reader">An xml reader.</param>
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			var s = reader.ReadElementString();
			var val = Parse(s, CultureInfo.InvariantCulture);
			m_Value = val.m_Value;
		}

		/// <summary>Writes the Gender to an xml writer.</summary>
		/// <remarks>
		/// Uses the string representation of Gender.
		/// </remarks>
		/// <param name="writer">An xml writer.</param>
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			writer.WriteString(GenderLabels[m_Value]);
		}

		#endregion
		
		#region (JSON) (De)serialization

		/// <summary>Generates a Gender from a JSON null object representation.</summary>
		void IJsonSerializable.FromJson()
		{
			m_Value = default(Byte); 
		}

		/// <summary>Generates a Gender from a JSON string representation.</summary>
		/// <param name="jsonString">
		/// The JSON string that represents the Gender.
		/// </param>
		void IJsonSerializable.FromJson(String jsonString)
		{
			m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;
		}

		/// <summary>Generates a Gender from a JSON integer representation.</summary>
		/// <param name="jsonInteger">
		/// The JSON integer that represents the Gender.
		/// </param>
		void IJsonSerializable.FromJson(Int64 jsonInteger)
		{
			m_Value = Create((Int32)jsonInteger).m_Value;
		}

		/// <summary>Generates a Gender from a JSON number representation.</summary>
		/// <param name="jsonNumber">
		/// The JSON number that represents the Gender.
		/// </param>
		void IJsonSerializable.FromJson(Double jsonNumber) { throw new NotSupportedException(QowaivMessages.JsonSerialization_DoubleNotSupported); }

		/// <summary>Generates a Gender from a JSON date representation.</summary>
		/// <param name="jsonDate">
		/// The JSON Date that represents the Gender.
		/// </param>
		void IJsonSerializable.FromJson(DateTime jsonDate) { throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported); }
		
		/// <summary>Converts a Gender into its JSON object representation.</summary>
		object IJsonSerializable.ToJson()
		{
			return GenderLabels[m_Value];
		}

		#endregion

		#region IFormattable / ToString

		/// <summary>Returns a System.String that represents the current Gender for debug purposes.</summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Debugger.")]
		private string DebuggerDisplay
		{
			get
			{
				if (m_Value == default(Byte)) { return "Gender: (empty)"; }
				return "Gender: " + GetDisplayName(CultureInfo.InvariantCulture);
			}
		}

		 /// <summary>Returns a System.String that represents the current Gender.</summary>
		public override string ToString()
		{
			return ToString(CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted System.String that represents the current Gender.</summary>
		/// <param name="format">
		/// The format that this describes the formatting.
		/// </param>
		public string ToString(string format)
		{
			return ToString(format, CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted System.String that represents the current Gender.</summary>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		public string ToString(IFormatProvider formatProvider)
		{
			return ToString("", formatProvider);
		}

		/// <summary>Returns a formatted System.String that represents the current Gender.</summary>
		/// <param name="format">
		/// The format that this describes the formatting.
		/// </param>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		/// <remarks>
		/// The formats:
		/// 
		/// i: as integer.
		/// c: as single character.
		/// h: as Honorific.
		/// s: as Symbol.
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
			if (String.IsNullOrEmpty(format)) { format = "f"; }

			// Apply the format.
			return StringFormatter.Apply(this, format, formatProvider ?? CultureInfo.CurrentCulture, FormatTokens);
		}

		/// <summary>The format token instructions.</summary>
		private static readonly Dictionary<char, Func<Gender, IFormatProvider, string>> FormatTokens = new Dictionary<char, Func<Gender, IFormatProvider, string>>()
		{
			{ 'i', (svo, provider) => svo.GetResourceString("int_", provider) },
			{ 'c', (svo, provider) => svo.GetResourceString("char_", provider) },
			{ 'h', (svo, provider) => svo.GetResourceString("honorific_", provider) },
			{ 's', (svo, provider) => svo.GetResourceString("symbol_", provider) },
			{ 'f', (svo, provider) => svo.GetResourceString("", provider) },
		};

		#endregion
		
		#region IEquatable

		/// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
		/// <param name="obj">An object to compare with.</param>
		public override bool Equals(object obj){ return base.Equals(obj); }

		/// <summary>Returns the hash code for this Gender.</summary>
		/// <returns>
		/// A 32-bit signed integer hash code.
		/// </returns>
		public override int GetHashCode() { return m_Value.GetHashCode(); }

		/// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator ==(Gender left, Gender right)
		{
			return left.Equals(right);
		}

		/// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator !=(Gender left, Gender right)
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
		/// An object that evaluates to a Gender.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.Value
		/// Condition Less than zero This instance precedes value. Zero This instance
		/// has the same position in the sort order as value. Greater than zero This
		/// instance follows value.-or- value is null.
		/// </returns>
		/// <exception cref="System.ArgumentException">
		/// value is not a Gender.
		/// </exception>
		public int CompareTo(object obj)
		{
			if (obj is Gender)
			{
				return CompareTo((Gender)obj);
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.AgrumentException_Must, "a gender"), "obj");
		}

		/// <summary>Compares this instance with a specified Gender and indicates
		/// whether this instance precedes, follows, or appears in the same position
		/// in the sort order as the specified Gender.
		/// </summary>
		/// <param name="other">
		/// The Gender to compare with this instance.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.
		/// </returns>
		/// <remarks>
		/// Gender.Empty follows all other Gender.
		/// </remarks>
		public int CompareTo(Gender other) { return m_Value.CompareTo(other.m_Value); }

		/// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
		public static bool operator <(Gender l, Gender r) { return l.m_Value.CompareTo(r.m_Value) < 0; }

		/// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
		public static bool operator >(Gender l, Gender r) { return l.m_Value.CompareTo(r.m_Value) > 0; }

		/// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
		public static bool operator <=(Gender l, Gender r) { return l.m_Value.CompareTo(r.m_Value) <= 0; }

		/// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
		public static bool operator >=(Gender l, Gender r) { return l.m_Value.CompareTo(r.m_Value) >= 0; }

		#endregion
	   
		#region (Explicit) casting

		/// <summary>Casts a Gender to a System.String.</summary>
		public static explicit operator string(Gender val) { return val.ToString(CultureInfo.CurrentCulture); }
		/// <summary>Casts a System.String to a Gender.</summary>
		public static explicit operator Gender(string str) { return Gender.Parse(str, CultureInfo.CurrentCulture); }

		/// <summary>Casts a Gender to a System.Int32.</summary>
		public static explicit operator Int32(Gender val) { return  val.ToInt32(); }
		/// <summary>Casts an System.Int32 to a Gender.</summary>
		public static implicit operator Gender(Int32 val) { return Gender.Create(val); }

		/// <summary>Casts a Gender to a System.Int32.</summary>
		public static explicit operator Int32?(Gender val) { return val.ToNullableInt32(); }
		/// <summary>Casts an System.Int32 to a Gender.</summary>
		public static implicit operator Gender(Int32? val) { return Gender.Create(val); }

		#endregion

		#region Factory methods

		/// <summary>Converts the string to a Gender.</summary>
		/// <param name="s">
		/// A string containing a Gender to convert.
		/// </param>
		/// <returns>
		/// A Gender.
		/// </returns>
		/// <exception cref="System.FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static Gender Parse(string s)
		{
		   return Parse(s, CultureInfo.CurrentCulture);
		}

		/// <summary>Converts the string to a Gender.</summary>
		/// <param name="s">
		/// A string containing a Gender to convert.
		/// </param>
		/// <param name="formatProvider">
		/// The specified format provider.
		/// </param>
		/// <returns>
		/// A Gender.
		/// </returns>
		/// <exception cref="System.FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static Gender Parse(string s, IFormatProvider formatProvider)
		{
			Gender val;
			if (Gender.TryParse(s, formatProvider, out val))
			{
				return val;
			}
			throw new FormatException(QowaivMessages.FormatExceptionGender);
		}

		/// <summary>Converts the string to a Gender.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a Gender to convert.
		/// </param>
		/// <returns>
		/// The Gender if the string was converted successfully, otherwise Gender.Empty.
		/// </returns>
		public static Gender TryParse(string s)
		{
			Gender val;
			if (Gender.TryParse(s, out val))
			{
				return val;
			}
			return Gender.Empty;
		}

		/// <summary>Converts the string to a Gender.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a Gender to convert.
		/// </param>
		/// <param name="result">
		/// The result of the parsing.
		/// </param>
		/// <returns>
		/// True if the string was converted successfully, otherwise false.
		/// </returns>
		public static bool TryParse(string s, out Gender result)
		{
			return TryParse(s, CultureInfo.CurrentCulture, out result);
		}

		/// <summary>Converts the string to a Gender.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a Gender to convert.
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
		public static bool TryParse(string s, IFormatProvider formatProvider, out Gender result)
		{
			result = Gender.Empty;
			if (string.IsNullOrEmpty(s))
			{
				return true;
			}
			var c = formatProvider as CultureInfo ?? CultureInfo.InvariantCulture;

			AddCulture(c);

			var str = Parsing.ToUnified(s);
			byte val;

			if (Parsings[c].TryGetValue(str, out val) || Parsings[CultureInfo.InvariantCulture].TryGetValue(str, out val))
			{
				result = new Gender() { m_Value = val };
				return true;
			}
			return false;
		}

		/// <summary>Creates a Gender from a Int32.</summary>
		/// <param name="val">
		/// A decimal describing a Gender.
		/// </param>
		/// <exception cref="System.FormatException">
		/// val is not a valid Gender.
		/// </exception>
		public static Gender Create(Int32? val)
		{
			Gender result;
			if (Gender.TryCreate(val, out result))
			{
				return result;
			}
			throw new ArgumentOutOfRangeException("val", QowaivMessages.FormatExceptionGender);
		}

		/// <summary>Creates a Gender from a Int32.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="val">
		/// A decimal describing a Gender.
		/// </param>
		/// <returns>
		/// A Gender if the creation was successfully, otherwise Gender.Empty.
		/// </returns>
		public static Gender TryCreate(Int32? val)
		{
			Gender result;
			if(TryCreate(val, out result))
			{
				return result;
			}
			return Gender.Empty;
		}

		/// <summary>Creates a Gender from a Int32.
		/// A return value indicates whether the creation succeeded.
		/// </summary>
		/// <param name="val">
		/// A Int32 describing a Gender.
		/// </param>
		/// <param name="result">
		/// The result of the creation.
		/// </param>
		/// <returns>
		/// True if a Gender was created successfully, otherwise false.
		/// </returns>
		public static bool TryCreate(Int32? val, out Gender result)
		{
			result = Gender.Empty;

			Byte b = 0;

			if (!val.HasValue || FromInt32s.TryGetValue(val.Value, out b))
			{
				result = new Gender() { m_Value = b };
				return true;
			}
			return false;
		}

		#endregion

		#region Validation

		/// <summary>Returns true if the val represents a valid Gender, otherwise false.</summary>
		public static bool IsValid(string val)
		{
			return IsValid(val, CultureInfo.CurrentCulture);
		}

		/// <summary>Returns true if the val represents a valid Gender, otherwise false.</summary>
		public static bool IsValid(string val, IFormatProvider formatProvider)
		{
			if (string.IsNullOrWhiteSpace(val)) { return false; }

			var culture = formatProvider as CultureInfo ?? CultureInfo.InvariantCulture;

			AddCulture(culture);

			return
				Parsings[culture].ContainsKey(val.ToLower(culture)) ||
				Parsings[CultureInfo.InvariantCulture].ContainsKey(val.ToLowerInvariant());
		}

		/// <summary>Returns true if the val represents a valid Gender, otherwise false.</summary>
		public static bool IsValid(Int32? val)
		{
			return val.HasValue && FromInt32s.ContainsKey(val.Value);
		}

		#endregion

		#region Resources

		private static ResourceManager ResourceManager = new ResourceManager("Qowaiv.GenderLabels", typeof(Gender).Assembly);

		/// <summary>Get resource string.</summary>
		/// <param name="prefix">
		/// The prefix of the resource key.
		/// </param>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		private string GetResourceString(string prefix, IFormatProvider formatProvider)
		{
			if (IsEmpty()) { return string.Empty; }
			return ResourceManager.GetString(prefix + GenderLabels[m_Value], formatProvider as CultureInfo);
		}

		#endregion

		#region Lookup

		/// <summary>Gets the valid values.</summary>
		private static Dictionary<Int32, Byte> FromInt32s = new Dictionary<Int32, Byte>()
		{
			{ 0, 1 },
			{ 1, 2 },
			{ 2, 4 },
			{ 9,  18 },
		};


		private static Dictionary<Byte, Int32?> ToNullableInt32s = new Dictionary<byte, Int32?>()
		{
			{ 0, null },
			{ 1, 0 },
			{ 2, 1 },
			{ 4, 2 },
			{ 18, 9 },
		};

		/// <summary>Gets the gender labels.</summary>
		/// <remarks>
		/// Used for both serialization and resource lookups.
		/// </remarks>
		private static readonly Dictionary<Byte, string> GenderLabels = new Dictionary<byte, string>()
		{
			{ 0, null },
			{ 1, "NotKnown" },
			{ 2, "Male" },
			{ 4, "Female" },
			{ 18, "NotApplicable" }
		};

		/// <summary>Adds a culture to the parsings.</summary>
		/// <param name="culture">
		/// The culture to add.
		/// </param>
		private static void AddCulture(CultureInfo culture)
		{
			lock (locker)
			{
				if (Parsings.ContainsKey(culture)) { return; }

				Parsings[culture] = new Dictionary<string, byte>();

				foreach (var gender in Gender.All)
				{
					var longname = gender.ToString("", culture).ToLower(culture);
					var shortname = gender.ToString("c", culture).ToLower(culture);

					Parsings[culture][longname] = gender.m_Value;
					Parsings[culture][shortname] = gender.m_Value;
				}
			}
		}

		/// <summary>Represents the parsing keys.</summary>
		private static readonly Dictionary<CultureInfo, Dictionary<string, byte>> Parsings = new Dictionary<CultureInfo, Dictionary<string, byte>>()
		{
			{
				CultureInfo.InvariantCulture, new Dictionary<string, byte>()
				{
					{ "", 0 },
					{ "0", 1 },
					{ "1", 2 },
					{ "2", 4 },
					{ "9", 18 },
					{ "?", 1 },
					{ "m", 2 },
					{ "f", 4 },
					{ "x", 18 },
					{ "♂", 2 },
					{ "♀", 4 },
					{ "notknown", 1 },
					{ "not known", 1 },
					{ "unknown", 1 },
					{ "male", 2 },
					{ "female", 4 },
					{ "notapplicable", 18 },
					{ "not applicable", 18 }
				}
			}
		};

		/// <summary>The locker for adding a culture.</summary>
		private static volatile object locker = new object();

		#endregion
	}
}