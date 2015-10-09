using Qowaiv.Conversion;
using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv
{
	/// <summary>Represents a Percentage.</summary>
	[DebuggerDisplay("{DebuggerDisplay}")]
	[Serializable, SingleValueObject(SingleValueStaticOptions.All ^ SingleValueStaticOptions.HasEmptyValue ^ SingleValueStaticOptions.HasUnknownValue, typeof(Decimal))]
	[TypeConverter(typeof(PercentageTypeConverter))]
	public struct Percentage : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IComparable, IComparable<Percentage>
	{
		/// <summary>The percentage mark (%).</summary>
		public const string PercentageMark = "%";
		/// <summary>The per mille mark (‰).</summary>
		public const string PerMilleMark = "‰";
		/// <summary>The per ten thousand mark (‱).</summary>
		public const string PerTenThousendMark = "‱";

		/// <summary>Represents 0 percent.</summary>
		public static readonly Percentage Zero = Percentage.Create(0.0m);
		/// <summary>Represents 1 percent.</summary>
		public static readonly Percentage One = Percentage.Create(0.01m);
		/// <summary>Represents 100 percent.</summary>
		public static readonly Percentage Hundred = Percentage.Create(1m);

		/// <summary>Gets the minimum value of a percentage.</summary>
		public static readonly Percentage MinValue = Decimal.MinValue;

		/// <summary>Gets the maximum value of a percentage.</summary>
		public static readonly Percentage MaxValue = Decimal.MaxValue;

		#region Properties

		/// <summary>The inner value of the Percentage.</summary>
		private Decimal m_Value;

		#endregion

		#region Percentage manipulation

		/// <summary>Increases the percentage with one percent.</summary>
		public Percentage Increment()
		{
			return this.Add(Percentage.One);
		}
		/// <summary>Decreases the percentage with one percent.</summary>
		public Percentage Decrement()
		{
			return this.Subtract(Percentage.One);
		}

		/// <summary>Pluses the percentage.</summary>
		public Percentage Plus()
		{
			return Percentage.Create(+m_Value);
		}
		/// <summary>Negates the percentage.</summary>
		public Percentage Negate()
		{
			return Percentage.Create(-m_Value);
		}

		/// <summary>Gets a percentage of the current percentage.</summary>
		/// <param name="p">
		/// The percentage to get.
		/// </param>
		public Percentage Multiply(Percentage p) { return m_Value * p.m_Value; }

		/// <summary>Divides the current percentage by a specified percentage.</summary>
		/// <param name="p">
		/// The percentage to devides to..
		/// </param>
		public Percentage Divide(Percentage p) { return m_Value / p.m_Value; }

		/// <summary>Adds a percentage to the current percentage.
		/// </summary>
		/// <param name="p">
		/// The percentage to add.
		/// </param>
		public Percentage Add(Percentage p) { return m_Value + p.m_Value; }

		/// <summary>Subtracts a percentage from the current percentage.
		/// </summary>
		/// <param name="p">
		/// The percentage to Subtract.
		/// </param>
		public Percentage Subtract(Percentage p) { return m_Value - p.m_Value; }

		#region Multiply

		/// <summary>Multiplies the percentage with a specified factor.
		/// </summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public Percentage Multiply(Decimal factor) { return m_Value * factor; }

		/// <summary>Multiplies the percentage with a specified factor.
		/// </summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public Percentage Multiply(Double factor) { return Multiply((Decimal)factor); }

		/// <summary>Multiplies the percentage with a specified factor.
		/// </summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public Percentage Multiply(Single factor) { return Multiply((Decimal)factor); }


		/// <summary>Multiplies the percentage with a specified factor.
		/// </summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public Percentage Multiply(Int64 factor) { return Multiply((Decimal)factor); }

		/// <summary>Multiplies the percentage with a specified factor.
		/// </summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public Percentage Multiply(Int32 factor) { return Multiply((Decimal)factor); }

		/// <summary>Multiplies the percentage with a specified factor.
		/// </summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public Percentage Multiply(Int16 factor) { return Multiply((Decimal)factor); }


		/// <summary>Multiplies the percentage with a specified factor.
		/// </summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		[CLSCompliant(false)]
		public Percentage Multiply(UInt64 factor) { return Multiply((Decimal)factor); }

		/// <summary>Multiplies the percentage with a specified factor.
		/// </summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		[CLSCompliant(false)]
		public Percentage Multiply(UInt32 factor) { return Multiply((Decimal)factor); }

		/// <summary>Multiplies the percentage with a specified factor.
		/// </summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		[CLSCompliant(false)]
		public Percentage Multiply(UInt16 factor) { return Multiply((Decimal)factor); }

		#endregion

		#region Divide

		/// <summary>Divide the percentage by a specified factor.
		/// </summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public Percentage Divide(Decimal factor) { return m_Value / factor; }

		/// <summary>Divide the percentage by a specified factor.
		/// </summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public Percentage Divide(Double factor) { return Divide((Decimal)factor); }

		/// <summary>Divide the percentage by a specified factor.
		/// </summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public Percentage Divide(Single factor) { return Divide((Decimal)factor); }


		/// <summary>Divide the percentage by a specified factor.
		/// </summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public Percentage Divide(Int64 factor) { return Divide((Decimal)factor); }

		/// <summary>Divide the percentage by a specified factor.
		/// </summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public Percentage Divide(Int32 factor) { return Divide((Decimal)factor); }

		/// <summary>Divide the percentage by a specified factor.
		/// </summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		public Percentage Divide(Int16 factor) { return Divide((Decimal)factor); }


		/// <summary>Divide the percentage by a specified factor.
		/// </summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		[CLSCompliant(false)]
		public Percentage Divide(UInt64 factor) { return Divide((Decimal)factor); }

		/// <summary>Divide the percentage by a specified factor.
		/// </summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		[CLSCompliant(false)]
		public Percentage Divide(UInt32 factor) { return Divide((Decimal)factor); }

		/// <summary>Divide the percentage by a specified factor.
		/// </summary>
		/// <param name="factor">
		/// The factor to multiply with.
		/// </param>
		[CLSCompliant(false)]
		public Percentage Divide(UInt16 factor) { return Divide((Decimal)factor); }

		#endregion

		/// <summary>Increases the percentage with one percent.</summary>
		public static Percentage operator ++(Percentage p) { return p.Increment(); }
		/// <summary>Decreases the percentage with one percent.</summary>
		public static Percentage operator --(Percentage p) { return p.Decrement(); }

		/// <summary>Unitary plusses the percentage.</summary>
		public static Percentage operator +(Percentage p) { return p.Plus(); }
		/// <summary>Negates the percentage.</summary>
		public static Percentage operator -(Percentage p) { return p.Negate(); }

		/// <summary>Multiplies the left and the right percentage.</summary>
		public static Percentage operator *(Percentage l, Percentage r) { return l.Multiply(r); }
		/// <summary>Divides the left by the right percentage.</summary>
		public static Percentage operator /(Percentage l, Percentage r) { return l.Divide(r); }
		/// <summary>Adds the left and the right percentage.</summary>
		public static Percentage operator +(Percentage l, Percentage r) { return l.Add(r); }
		/// <summary>Subtracts the right from the left percentage.</summary>
		public static Percentage operator -(Percentage l, Percentage r) { return l.Subtract(r); }

		/// <summary>Multiplies the percentage with the factor.</summary>
		public static Percentage operator *(Percentage p, Decimal factor) { return p.Multiply(factor); }
		/// <summary>Multiplies the percentage with the factor.</summary>
		public static Percentage operator *(Percentage p, Double factor) { return p.Multiply(factor); }
		/// <summary>Multiplies the percentage with the factor.</summary>
		public static Percentage operator *(Percentage p, Single factor) { return p.Multiply(factor); }

		/// <summary>Multiplies the percentage with the factor.</summary>
		public static Percentage operator *(Percentage p, Int64 factor) { return p.Multiply(factor); }
		/// <summary>Multiplies the percentage with the factor.</summary>
		public static Percentage operator *(Percentage p, Int32 factor) { return p.Multiply(factor); }
		/// <summary>Multiplies the percentage with the factor.</summary>
		public static Percentage operator *(Percentage p, Int16 factor) { return p.Multiply(factor); }

		/// <summary>Multiplies the percentage with the factor.</summary>
		[CLSCompliant(false)]
		public static Percentage operator *(Percentage p, UInt64 factor) { return p.Multiply(factor); }
		/// <summary>Multiplies the percentage with the factor.</summary>
		[CLSCompliant(false)]
		public static Percentage operator *(Percentage p, UInt32 factor) { return p.Multiply(factor); }
		/// <summary>Multiplies the percentage with the factor.</summary>
		[CLSCompliant(false)]
		public static Percentage operator *(Percentage p, UInt16 factor) { return p.Multiply(factor); }

		/// <summary>Divides the percentage by the factor.</summary>
		public static Percentage operator /(Percentage p, Decimal factor) { return p.Divide(factor); }
		/// <summary>Divides the percentage by the factor.</summary>
		public static Percentage operator /(Percentage p, Double factor) { return p.Divide(factor); }
		/// <summary>Divides the percentage by the factor.</summary>
		public static Percentage operator /(Percentage p, Single factor) { return p.Divide(factor); }

		/// <summary>Divides the percentage by the factor.</summary>
		public static Percentage operator /(Percentage p, Int64 factor) { return p.Divide(factor); }
		/// <summary>Divides the percentage by the factor.</summary>
		public static Percentage operator /(Percentage p, Int32 factor) { return p.Divide(factor); }
		/// <summary>Divides the percentage by the factor.</summary>
		public static Percentage operator /(Percentage p, Int16 factor) { return p.Divide(factor); }

		/// <summary>Divides the percentage by the factor.</summary>
		[CLSCompliant(false)]
		public static Percentage operator /(Percentage p, UInt64 factor) { return p.Divide(factor); }
		/// <summary>Divides the percentage by the factor.</summary>
		[CLSCompliant(false)]
		public static Percentage operator /(Percentage p, UInt32 factor) { return p.Divide(factor); }
		/// <summary>Divides the percentage by the factor.</summary>
		[CLSCompliant(false)]
		public static Percentage operator /(Percentage p, UInt16 factor) { return p.Divide(factor); }

		#endregion

		#region Number manipulation

		/// <summary>Gets the percentage of the Decimal.</summary>
		public static Decimal operator *(Decimal d, Percentage p) { return d.Multiply(p); }
		/// <summary>Gets the percentage of the Double.</summary>
		public static Double operator *(Double d, Percentage p) { return d.Multiply(p); }
		/// <summary>Gets the percentage of the Single.</summary>
		public static Single operator *(Single d, Percentage p) { return d.Multiply(p); }

		/// <summary>Gets the percentage of the Int64.</summary>
		public static Int64 operator *(Int64 d, Percentage p) { return d.Multiply(p); }
		/// <summary>Gets the percentage of the Int32.</summary>
		public static Int32 operator *(Int32 d, Percentage p) { return d.Multiply(p); }
		/// <summary>Gets the percentage of the Int16.</summary>
		public static Int16 operator *(Int16 d, Percentage p) { return d.Multiply(p); }

		/// <summary>Gets the percentage of the UInt64.</summary>
		[CLSCompliant(false)]
		public static UInt64 operator *(UInt64 d, Percentage p) { return d.Multiply(p); }
		/// <summary>Gets the percentage of the UInt32.</summary>
		[CLSCompliant(false)]
		public static UInt32 operator *(UInt32 d, Percentage p) { return d.Multiply(p); }
		/// <summary>Gets the percentage of the UInt16.</summary>
		[CLSCompliant(false)]
		public static UInt16 operator *(UInt16 d, Percentage p) { return d.Multiply(p); }

		/// <summary>Divides the Decimal by the percentage.</summary>
		public static Decimal operator /(Decimal d, Percentage p) { return d.Divide(p); }
		/// <summary>Divides the Double by the percentage.</summary>
		public static Double operator /(Double d, Percentage p) { return d.Divide(p); }
		/// <summary>Divides the Single by the percentage.</summary>
		public static Single operator /(Single d, Percentage p) { return d.Divide(p); }

		/// <summary>Divides the Int64 by the percentage.</summary>
		public static Int64 operator /(Int64 d, Percentage p) { return d.Divide(p); }
		/// <summary>Divides the Int32 by the percentage.</summary>
		public static Int32 operator /(Int32 d, Percentage p) { return d.Divide(p); }
		/// <summary>Divides the Int16 by the percentage.</summary>
		public static Int16 operator /(Int16 d, Percentage p) { return d.Divide(p); }

		/// <summary>Divides the UInt64 by the percentage.</summary>
		[CLSCompliant(false)]
		public static UInt64 operator /(UInt64 d, Percentage p) { return d.Divide(p); }
		/// <summary>Divides the UInt32 by the percentage.</summary>
		[CLSCompliant(false)]
		public static UInt32 operator /(UInt32 d, Percentage p) { return d.Divide(p); }
		/// <summary>Divides the UInt16 by the percentage.</summary>
		[CLSCompliant(false)]
		public static UInt16 operator /(UInt16 d, Percentage p) { return d.Divide(p); }

		/// <summary>Adds the percentage to the Decimal.</summary>
		public static Decimal operator +(Decimal d, Percentage p) { return d.Add(p); }
		/// <summary>Adds the percentage to the Double.</summary>
		public static Double operator +(Double d, Percentage p) { return d.Add(p); }
		/// <summary>Adds the percentage to the Single.</summary>
		public static Single operator +(Single d, Percentage p) { return d.Add(p); }

		/// <summary>Adds the percentage to the Int64.</summary>
		public static Int64 operator +(Int64 d, Percentage p) { return d.Add(p); }
		/// <summary>Adds the percentage to the Int32.</summary>
		public static Int32 operator +(Int32 d, Percentage p) { return d.Add(p); }
		/// <summary>Adds the percentage to the Int16.</summary>
		public static Int16 operator +(Int16 d, Percentage p) { return d.Add(p); }

		/// <summary>Adds the percentage to the UInt64.</summary>
		[CLSCompliant(false)]
		public static UInt64 operator +(UInt64 d, Percentage p) { return d.Add(p); }
		/// <summary>Adds the percentage to the UInt32.</summary>
		[CLSCompliant(false)]
		public static UInt32 operator +(UInt32 d, Percentage p) { return d.Add(p); }
		/// <summary>Adds the percentage to the UInt16.</summary>
		[CLSCompliant(false)]
		public static UInt16 operator +(UInt16 d, Percentage p) { return d.Add(p); }

		/// <summary>Subtracts the percentage to the Decimal.</summary>
		public static Decimal operator -(Decimal d, Percentage p) { return d.Subtract(p); }
		/// <summary>Subtracts the percentage to the Double.</summary>
		public static Double operator -(Double d, Percentage p) { return d.Subtract(p); }
		/// <summary>Subtracts the percentage to the Single.</summary>
		public static Single operator -(Single d, Percentage p) { return d.Subtract(p); }

		/// <summary>Subtracts the percentage to the Int64.</summary>
		public static Int64 operator -(Int64 d, Percentage p) { return d.Subtract(p); }
		/// <summary>Subtracts the percentage to the Int32.</summary>
		public static Int32 operator -(Int32 d, Percentage p) { return d.Subtract(p); }
		/// <summary>Subtracts the percentage to the Int16.</summary>
		public static Int16 operator -(Int16 d, Percentage p) { return d.Subtract(p); }

		/// <summary>Subtracts the percentage to the UInt64.</summary>
		[CLSCompliant(false)]
		public static UInt64 operator -(UInt64 d, Percentage p) { return d.Subtract(p); }
		/// <summary>Subtracts the percentage to the UInt32.</summary>
		[CLSCompliant(false)]
		public static UInt32 operator -(UInt32 d, Percentage p) { return d.Subtract(p); }
		/// <summary>Subtracts the percentage to the UInt16.</summary>
		[CLSCompliant(false)]
		public static UInt16 operator -(UInt16 d, Percentage p) { return d.Subtract(p); }

		#endregion

		#region (XML) (De)serialization

		/// <summary>Initializes a new instance of Percentage based on the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		private Percentage(SerializationInfo info, StreamingContext context)
		{
			Guard.NotNull(info, "info");
			m_Value = info.GetDecimal("Value");
		}

		/// <summary>Adds the underlying property of Percentage to the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			Guard.NotNull(info, "info");
			info.AddValue("Value", m_Value);
		}

		/// <summary>Gets the xml schema to (de) xml serialize a Percentage.</summary>
		/// <remarks>
		/// Returns null as no schema is required.
		/// </remarks>
		XmlSchema IXmlSerializable.GetSchema() { return null; }

		/// <summary>Reads the Percentage from an xml writer.</summary>
		/// <remarks>
		/// Uses the string parse function of Percentage.
		/// </remarks>
		/// <param name="reader">An xml reader.</param>
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			Guard.NotNull(reader, "reader");
			var s = reader.ReadElementString();
			var val = Parse(s, CultureInfo.InvariantCulture);
			m_Value = val.m_Value;
		}

		/// <summary>Writes the Percentage to an xml writer.</summary>
		/// <remarks>
		/// Uses the string representation of Percentage.
		/// </remarks>
		/// <param name="writer">An xml writer.</param>
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			Guard.NotNull(writer, "writer");
			writer.WriteString(ToString("0.############################%", CultureInfo.InvariantCulture));
		}

		#endregion

		#region (JSON) (De)serialization

		/// <summary>Generates a Percentage from a JSON null object representation.</summary>
		void IJsonSerializable.FromJson() { throw new NotSupportedException(QowaivMessages.JsonSerialization_NullNotSupported); }

		/// <summary>Generates a Percentage from a JSON string representation.</summary>
		/// <param name="jsonString">
		/// The JSON string that represents the Percentage.
		/// </param>
		void IJsonSerializable.FromJson(String jsonString)
		{
			m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;
		}

		/// <summary>Generates a Percentage from a JSON integer representation.</summary>
		/// <param name="jsonInteger">
		/// The JSON integer that represents the Percentage.
		/// </param>
		void IJsonSerializable.FromJson(Int64 jsonInteger) { throw new NotSupportedException(QowaivMessages.JsonSerialization_Int64NotSupported); }

		/// <summary>Generates a Percentage from a JSON number representation.</summary>
		/// <param name="jsonNumber">
		/// The JSON number that represents the Percentage.
		/// </param>
		void IJsonSerializable.FromJson(Double jsonNumber)
		{
			m_Value = Create((Decimal)jsonNumber).m_Value;
		}

		/// <summary>Generates a Percentage from a JSON date representation.</summary>
		/// <param name="jsonDate">
		/// The JSON Date that represents the Percentage.
		/// </param>
		void IJsonSerializable.FromJson(DateTime jsonDate) { throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported); }

		/// <summary>Converts a Percentage into its JSON object representation.</summary>
		object IJsonSerializable.ToJson()
		{
			return ToString("0.############################%", CultureInfo.InvariantCulture);
		}

		#endregion

		#region IFormattable / ToString

		/// <summary>Returns a System.String that represents the current Percentage for debug purposes.</summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Debugger.")]
		private string DebuggerDisplay
		{
			get { return ToString("0.00##########################%", CultureInfo.InvariantCulture); }
		}

		/// <summary>Returns a System.String that represents the current Percentage formatted with a per ten thousend mark.</summary>
		public string ToPerTenThousendMarkString()
		{
			return ToString("0.############################‱", CultureInfo.InvariantCulture);
		}
		/// <summary>Returns a System.String that represents the current Percentage formatted with a per mille mark.</summary>
		public string ToPerMilleString()
		{
			return ToString("0.############################‰", CultureInfo.InvariantCulture);
		}

		/// <summary>Returns a System.String that represents the current Percentage.</summary>
		public override string ToString()
		{
			return ToString(CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted System.String that represents the current Percentage.</summary>
		/// <param name="format">
		/// The format that this describes the formatting.
		/// </param>
		public string ToString(string format)
		{
			return ToString(format, CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted System.String that represents the current Percentage.</summary>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		public string ToString(IFormatProvider formatProvider)
		{
			string format;
			DefaultFormats.TryGetValue(formatProvider ?? CultureInfo.CurrentCulture, out format);
			format = format ?? "0.############################%";

			return ToString(format, formatProvider);
		}

		/// <summary>Gets the default format for diferent countries.</summary>
		private static Dictionary<IFormatProvider, string> DefaultFormats = new Dictionary<IFormatProvider, string>()
		{
			{ new CultureInfo("fr-FR"), "%0.############################" },
			{ new CultureInfo("fa-IR"), "%0.############################" },
		};


		/// <summary>Returns a formatted System.String that represents the current Percentage.</summary>
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

			var marker = GetMarkerType(format ?? String.Empty);
			if (marker == PercentageMarkerType.Invalid)
			{
				throw new FormatException(QowaivMessages.FormatException_InvalidFormat);
			}
			var decimalVal = m_Value / Dividers[marker];

			var str = decimalVal.ToString(RemoveMarks(format ?? String.Empty), formatProvider);
			switch (marker)
			{
				case PercentageMarkerType.PercentageBefore: str = PercentageMark + str; break;
				case PercentageMarkerType.PercentageAfter: str = str + PercentageMark; break;
				case PercentageMarkerType.PerMilleBefore: str = PerMilleMark + str; break;
				case PercentageMarkerType.PerMilleAfter: str = str + PerMilleMark; break;
				case PercentageMarkerType.PerTenThousendBefore: str = PerTenThousendMark + str; break;
				case PercentageMarkerType.PerTenThousendAfter: str = str + PerTenThousendMark; break;
			}
			return str;
		}

		#endregion

		#region IEquatable

		/// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
		/// <param name="obj">An object to compare with.</param>
		public override bool Equals(object obj) { return base.Equals(obj); }

		/// <summary>Returns the hash code for this Percentage.</summary>
		/// <returns>
		/// A 32-bit signed integer hash code.
		/// </returns>
		public override int GetHashCode() { return m_Value.GetHashCode(); }

		/// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator ==(Percentage left, Percentage right)
		{
			return left.Equals(right);
		}

		/// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator !=(Percentage left, Percentage right)
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
		/// An object that evaluates to a Percentage.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.Value
		/// Condition Less than zero This instance precedes value. Zero This instance
		/// has the same position in the sort order as value. Greater than zero This
		/// instance follows value.-or- value is null.
		/// </returns>
		/// <exception cref="System.ArgumentException">
		/// value is not a Percentage.
		/// </exception>
		public int CompareTo(object obj)
		{
			if (obj is Percentage)
			{
				return CompareTo((Percentage)obj);
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.AgrumentException_Must, "a percentage"), "obj");
		}

		/// <summary>Compares this instance with a specified Percentage and indicates
		/// whether this instance precedes, follows, or appears in the same position
		/// in the sort order as the specified Percentage.
		/// </summary>
		/// <param name="other">
		/// The Percentage to compare with this instance.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.
		/// </returns>
		public int CompareTo(Percentage other) { return m_Value.CompareTo(other.m_Value); }


		/// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
		public static bool operator <(Percentage l, Percentage r) { return l.CompareTo(r) < 0; }

		/// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
		public static bool operator >(Percentage l, Percentage r) { return l.CompareTo(r) > 0; }

		/// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
		public static bool operator <=(Percentage l, Percentage r) { return l.CompareTo(r) <= 0; }

		/// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
		public static bool operator >=(Percentage l, Percentage r) { return l.CompareTo(r) >= 0; }

		#endregion

		#region (Explicit) casting

		/// <summary>Casts a Percentage to a System.String.</summary>
		public static explicit operator string(Percentage val) { return val.ToString(CultureInfo.CurrentCulture); }
		/// <summary>Casts a System.String to a Percentage.</summary>
		public static explicit operator Percentage(string str) { return Percentage.Parse(str, CultureInfo.CurrentCulture); }


		/// <summary>Casts a decimal a Percentage.</summary>
		public static implicit operator Percentage(decimal val) { return Percentage.Create(val); }
		/// <summary>Casts a decimal a Percentage.</summary>
		public static implicit operator Percentage(double val) { return Percentage.Create(val); }

		/// <summary>Casts a Percentage to a decimal.</summary>
		public static explicit operator decimal(Percentage val) { return val.m_Value; }
		/// <summary>Casts a Percentage to a double.</summary>
		public static explicit operator double(Percentage val) { return (double)val.m_Value; }

		#endregion

		#region Factory methods

		/// <summary>Converts the string to a Percentage.</summary>
		/// <param name="s">
		/// A string containing a Percentage to convert.
		/// </param>
		/// <returns>
		/// A Percentage.
		/// </returns>
		/// <exception cref="System.FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static Percentage Parse(string s)
		{
			return Parse(s, CultureInfo.CurrentCulture);
		}

		/// <summary>Converts the string to a Percentage.</summary>
		/// <param name="s">
		/// A string containing a Percentage to convert.
		/// </param>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		/// <returns>
		/// A Percentage.
		/// </returns>
		/// <exception cref="System.FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static Percentage Parse(string s, IFormatProvider formatProvider)
		{
			Percentage val;
			if (Percentage.TryParse(s, formatProvider, out val))
			{
				return val;
			}
			throw new FormatException(QowaivMessages.FormatExceptionPercentage);
		}

		/// <summary>Converts the string to a Percentage.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a Percentage to convert.
		/// </param>
		/// <param name="result">
		/// The result of the parsing.
		/// </param>
		/// <returns>
		/// True if the string was converted successfully, otherwise false.
		/// </returns>
		public static bool TryParse(string s, out Percentage result)
		{
			return TryParse(s, CultureInfo.CurrentCulture, out result);
		}

		/// <summary>Converts the string to a Percentage.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a Percentage to convert.
		/// </param>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		/// <param name="result">
		/// The result of the parsing.
		/// </param>
		/// <returns>
		/// True if the string was converted successfully, otherwise false.
		/// </returns>
		public static bool TryParse(string s, IFormatProvider formatProvider, out Percentage result)
		{
			result = Percentage.Zero;

			if (!string.IsNullOrEmpty(s))
			{
				var marker = GetMarkerType(s);
				Decimal dec;
				s = RemoveMarks(s);
				if (marker != PercentageMarkerType.Invalid && Decimal.TryParse(s, NumberStyles.Number, formatProvider, out dec))
				{
					dec *= Dividers[marker];
					result = Percentage.Create(dec);
					return true;
				}
			}
			return false;
		}

		/// <summary>Creates a Percentage from a Decimal. </summary >
		/// <param name="val" >
		/// A decimal describing a Percentage.
		/// </param >
		public static Percentage Create(Decimal val) { return new Percentage() { m_Value = val }; }

		/// <summary>Creates a Percentage from a Double. </summary >
		/// <param name="val" >
		/// A decimal describing a Percentage.
		/// </param >
		public static Percentage Create(Double val) { return new Percentage() { m_Value = (Decimal)val }; }

		#endregion

		#region Parsing Helpers

		internal enum PercentageMarkerType
		{
			None = 0,
			PercentageBefore,
			PercentageAfter,
			PerMilleBefore,
			PerMilleAfter,
			PerTenThousendBefore,
			PerTenThousendAfter,
			Invalid,
		}

		internal static readonly Dictionary<PercentageMarkerType, Decimal> Dividers = new Dictionary<PercentageMarkerType, Decimal>()
		{
			{ PercentageMarkerType.None, 0.01m },
			{ PercentageMarkerType.PercentageBefore, 0.01m },
			{ PercentageMarkerType.PercentageAfter, 0.01m },
			{ PercentageMarkerType.PerMilleBefore, 0.001m },
			{ PercentageMarkerType.PerMilleAfter, 0.001m },
			{ PercentageMarkerType.PerTenThousendBefore, 0.0001m },
			{ PercentageMarkerType.PerTenThousendAfter, 0.0001m },
		};

		internal static PercentageMarkerType GetMarkerType(string str)
		{
			if (str.Count(c => c == PercentageMark[0] || c == PerMilleMark[0] || c == PerTenThousendMark[0]) > 1) { return PercentageMarkerType.Invalid; }

			if (str.EndsWith(PercentageMark)) { return PercentageMarkerType.PercentageAfter; }
			if (str.StartsWith(PercentageMark)) { return PercentageMarkerType.PercentageBefore; }
			if (str.Contains(PercentageMark)) { return PercentageMarkerType.Invalid; }

			if (str.EndsWith(PerMilleMark)) { return PercentageMarkerType.PerMilleAfter; }
			if (str.StartsWith(PerMilleMark)) { return PercentageMarkerType.PerMilleBefore; }
			if (str.Contains(PerMilleMark)) { return PercentageMarkerType.Invalid; }

			if (str.EndsWith(PerTenThousendMark)) { return PercentageMarkerType.PerTenThousendAfter; }
			if (str.StartsWith(PerTenThousendMark)) { return PercentageMarkerType.PerTenThousendBefore; }
			if (str.Contains(PerTenThousendMark)) { return PercentageMarkerType.Invalid; }
			return PercentageMarkerType.None;
		}
		internal static string RemoveMarks(string str)
		{
			return str.Replace(PercentageMark, "").Replace(PerMilleMark, "").Replace(PerTenThousendMark, "");
		}

		#endregion

		#region Validation

		/// <summary>Returns true if the val represents a valid Percentage, otherwise false.</summary>
		public static bool IsValid(string val)
		{
			return IsValid(val, CultureInfo.CurrentCulture);
		}

		/// <summary>Returns true if the val represents a valid Percentage, otherwise false.</summary>
		public static bool IsValid(string val, IFormatProvider formatProvider)
		{
			Percentage p;
			return TryParse(val, formatProvider, out p);
		}

		#endregion
	}
}