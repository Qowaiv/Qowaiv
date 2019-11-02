#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion.Statistics;
using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv.Statistics
{
    /// <summary>Represents an Elo.</summary>
    /// <remarks>
    /// The Elo rating system is a method for calculating the relative skill levels
    /// of players in competitor-versus-competitor games such as chess. It is named
    /// after its creator Arpad Elo, a Hungarian-born American physics professor.
    /// 
    /// The Elo system was originally invented as an improved chess rating system
    /// but is also used as a rating system for multiplayer competition in a number
    /// of video games, association football, gridiron football, basketball, Major
    /// League Baseball, competitive programming, and other games.
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.Continuous, typeof(Double))]
    [OpenApiDataType(description: "Elo rating system notation.", type: "number", format: "elo")]
    [TypeConverter(typeof(EloTypeConverter))]
    public struct Elo : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<Elo>, IComparable, IComparable<Elo>
    {
        /// <summary>Represents the zero value of an Elo.</summary>
        public static readonly Elo Zero = new Elo { m_Value = default };

        /// <summary>Represents the minimum value of an Elo.</summary>
        public static readonly Elo MinValue = new Elo { m_Value = Double.MinValue };

        /// <summary>Represents the maximum value of an Elo.</summary>
        public static readonly Elo MaxValue = new Elo { m_Value = Double.MaxValue };

        #region Properties

        /// <summary>The inner value of the Elo.</summary>
        private Double m_Value;

        #endregion

        #region Methods

        /// <summary>Gets an z-score based on the two Elo's.</summary>
        /// <param name="elo0">
        /// The first Elo.
        /// </param>
        /// <param name="elo1">
        /// The second Elo.
        /// </param>
        public static double GetZScore(Elo elo0, Elo elo1)
        {
            var elo_div = elo1 - elo0;
            var z = 1 / (1 + Math.Pow(10.0, (double)elo_div / 400.0));

            return z;
        }

        #endregion

        #region Elo manipulation

        /// <summary>Increases the Elo with one.</summary>
        public Elo Increment() => Add(1d);

        /// <summary>Decreases the Elo with one.</summary>
        public Elo Decrement() => Subtract(1d);


        /// <summary>Pluses the Elo.</summary>
        public Elo Plus() => Create(+m_Value);

        /// <summary>Negates the Elo.</summary>
        public Elo Negate() => Create(-m_Value);


        /// <summary>Multiplies the current Elo with a factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Elo Multiply(double factor) => m_Value * factor;

        /// <summary>Divides the current Elo by a factor.</summary>
        /// <param name="factor">
        /// The factor to divides by.
        /// </param>
        public Elo Divide(double factor) => m_Value / factor;

        /// <summary>Adds Elo to the current Elo.
        /// </summary>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        public Elo Add(Elo p) => m_Value + p.m_Value;

        /// <summary>Subtracts Elo from the current Elo.
        /// </summary>
        /// <param name="p">
        /// The percentage to Subtract.
        /// </param>
        public Elo Subtract(Elo p) => m_Value - p.m_Value;


        /// <summary>Increases the Elo with one.</summary>
        public static Elo operator ++(Elo elo) => elo.Increment();
        /// <summary>Decreases the Elo with one.</summary>
        public static Elo operator --(Elo elo) => elo.Decrement();

        /// <summary>Unitary plusses the Elo.</summary>
        public static Elo operator +(Elo elo) => elo.Plus();
        /// <summary>Negates the Elo.</summary>
        public static Elo operator -(Elo elo) => elo.Negate();

        /// <summary>Multiplies the Elo with the factor.</summary>
        public static Elo operator *(Elo elo, double factor) => elo.Multiply(factor);
        /// <summary>Divides the Elo by the factor.</summary>
        public static Elo operator /(Elo elo, double factor) => elo.Divide(factor);
        /// <summary>Adds the left and the right Elo.</summary>
        public static Elo operator +(Elo l, Elo r) => l.Add(r);
        /// <summary>Subtracts the right from the left Elo.</summary>
        public static Elo operator -(Elo l, Elo r) => l.Subtract(r);

        #endregion

        #region (XML) (De)serialization

        /// <summary>Initializes a new instance of Elo based on the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private Elo(SerializationInfo info, StreamingContext context)
        {
            if (info == null) { throw new ArgumentNullException("info"); }
            m_Value = info.GetDouble("Value");
        }

        /// <summary>Adds the underlying property of Elo to the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) { throw new ArgumentNullException("info"); }
            info.AddValue("Value", m_Value);
        }

        /// <summary>Gets the <see href="XmlSchema"/> to (de) XML serialize an Elo.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <summary>Reads the Elo from an <see href="XmlReader"/>.</summary>
        /// <remarks>
        /// Uses the string parse function of Elo.
        /// </remarks>
        /// <param name="reader">An XML reader.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            var s = reader.ReadElementString();
            var val = Parse(s, CultureInfo.InvariantCulture);
            m_Value = val.m_Value;
        }

        /// <summary>Writes the Elo to an <see href="XmlWriter"/>.</summary>
        /// <remarks>
        /// Uses the string representation of Elo.
        /// </remarks>
        /// <param name="writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteString(ToString("", CultureInfo.InvariantCulture));
        }

        #endregion

        #region (JSON) (De)serialization

        /// <summary>Generates an Elo from a JSON null object representation.</summary>
        void IJsonSerializable.FromJson() => throw new NotSupportedException(QowaivMessages.JsonSerialization_NullNotSupported);

        /// <summary>Generates an Elo from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the Elo.
        /// </param>
        void IJsonSerializable.FromJson(string jsonString)
        {
            m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;
        }

        /// <summary>Generates an Elo from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the Elo.
        /// </param>
        void IJsonSerializable.FromJson(Int64 jsonInteger)
        {
            m_Value = Create(jsonInteger).m_Value;
        }

        /// <summary>Generates an Elo from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the Elo.
        /// </param>
        void IJsonSerializable.FromJson(Double jsonNumber)
        {
            m_Value = Create(jsonNumber).m_Value;
        }

        /// <summary>Generates an Elo from a JSON date representation.</summary>
        /// <param name="jsonDate">
        /// The JSON Date that represents the Elo.
        /// </param>
        void IJsonSerializable.FromJson(DateTime jsonDate) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported);

        /// <summary>Converts an Elo into its JSON object representation.</summary>
        object IJsonSerializable.ToJson() { return m_Value; }

        #endregion

        #region IFormattable / ToString

        /// <summary>Returns a <see cref="string"/> that represents the current Elo for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Double DebuggerDisplay { get { return m_Value; } }

        /// <summary>Returns a <see cref="string"/> that represents the current Elo.</summary>
        public override string ToString()
        {
            return ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current Elo.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current Elo.</summary>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider formatProvider)
        {
            return ToString("", formatProvider);
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current Elo.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
            {
                return formatted;
            }
            return m_Value.ToString(format, formatProvider);
        }

        #endregion

        #region IEquatable

        /// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
        /// <param name="obj">An object to compare with.</param>
        public override bool Equals(object obj) => obj is Elo && Equals((Elo)obj);

        /// <summary>Returns true if this instance and the other <see cref="Elo"/> are equal, otherwise false.</summary>
        /// <param name="other">The <see cref="Elo"/> to compare with.</param>
        public bool Equals(Elo other) => m_Value.Equals(other.m_Value);

        /// <summary>Returns the hash code for this Elo.</summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => m_Value.GetHashCode();

        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator ==(Elo left, Elo right)
        {
            return left.Equals(right);
        }

        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator !=(Elo left, Elo right)
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
        /// An object that evaluates to a Elo.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.Value
        /// Condition Less than zero This instance precedes value. Zero This instance
        /// has the same position in the sort order as value. Greater than zero This
        /// instance follows value.-or- value is null.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// value is not a Elo.
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj is Elo)
            {
                return CompareTo((Elo)obj);
            }
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.ArgumentException_Must, "an Elo."), "obj");
        }

        /// <summary>Compares this instance with a specified Elo and indicates
        /// whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified Elo.
        /// </summary>
        /// <param name="other">
        /// The Elo to compare with this instance.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(Elo other) => m_Value.CompareTo(other.m_Value);


        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(Elo l, Elo r) => l.CompareTo(r) < 0;

        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator >(Elo l, Elo r) => l.CompareTo(r) > 0;

        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(Elo l, Elo r) => l.CompareTo(r) <= 0;

        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(Elo l, Elo r) => l.CompareTo(r) >= 0;

        #endregion

        #region (Explicit) casting

        /// <summary>Casts an Elo to a <see cref="string"/>.</summary>
        public static explicit operator string(Elo val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a Elo.</summary>
        public static explicit operator Elo(string str) { return Elo.Parse(str, CultureInfo.CurrentCulture); }


        /// <summary>Casts a decimal to an Elo.</summary>
        public static implicit operator Elo(decimal val) { return new Elo { m_Value = (double)val }; }
        /// <summary>Casts a decimal to an Elo.</summary>
        public static implicit operator Elo(double val) { return new Elo { m_Value = val }; }
        /// <summary>Casts an integer to an Elo.</summary>
        public static implicit operator Elo(Int32 val) { return new Elo { m_Value = val }; }

        /// <summary>Casts an Elo to a decimal.</summary>
        public static explicit operator decimal(Elo val) { return (decimal)val.m_Value; }
        /// <summary>Casts an Elo to a double.</summary>
        public static explicit operator double(Elo val) => val.m_Value;
        /// <summary>Casts an Elo to an integer.</summary>
        public static explicit operator Int32(Elo val) { return (Int32)Math.Round(val.m_Value); }

        #endregion

        #region Factory methods

        /// <summary>Converts the string to an Elo.</summary>
        /// <param name="s">
        /// A string containing an Elo to convert.
        /// </param>
        /// <returns>
        /// An Elo.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static Elo Parse(string s)
        {
            return Parse(s, CultureInfo.CurrentCulture);
        }

        /// <summary>Converts the string to an Elo.</summary>
        /// <param name="s">
        /// A string containing an Elo to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        /// <returns>
        /// An Elo.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static Elo Parse(string s, IFormatProvider formatProvider)
        {
            if (TryParse(s, formatProvider, out Elo val))
            {
                return val;
            }
            throw new FormatException(QowaivMessages.FormatExceptionElo);
        }

        /// <summary>Converts the string to an Elo.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing an Elo to convert.
        /// </param>
        /// <returns>
        /// The Elo if the string was converted successfully, otherwise Elo.Empty.
        /// </returns>
        public static Elo TryParse(string s)
        {
            if (TryParse(s, out Elo val))
            {
                return val;
            }
            return Zero;
        }

        /// <summary>Converts the string to an Elo.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing an Elo to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out Elo result)
        {
            return TryParse(s, CultureInfo.CurrentCulture, out result);
        }

        /// <summary>Converts the string to an Elo.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing an Elo to convert.
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
        public static bool TryParse(string s, IFormatProvider formatProvider, out Elo result)
        {
            result = Zero;
            if (!string.IsNullOrEmpty(s))
            {
                var str = s.EndsWith("*", StringComparison.InvariantCultureIgnoreCase) ? s.Substring(0, s.Length - 1) : s;
                if (double.TryParse(str, NumberStyles.Number, formatProvider, out double d))
                {
                    result = new Elo { m_Value = d };
                    return true;
                }
            }
            return false;
        }

        /// <summary >Creates an Elo from a Double. </summary >
        /// <param name="val" >
        /// A decimal describing an Elo.
        /// </param >
        /// <exception cref="FormatException" >
        /// val is not a valid Elo.
        /// </exception >
        public static Elo Create(Double val) => new Elo { m_Value = val };

        #endregion

        #region Validation

        /// <summary>Returns true if the val represents a valid Elo, otherwise false.</summary>
        public static bool IsValid(string val) => IsValid(val, CultureInfo.CurrentCulture);

        /// <summary>Returns true if the val represents a valid Elo, otherwise false.</summary>
        public static bool IsValid(string val, IFormatProvider formatProvider) => TryParse(val, formatProvider, out _);

        #endregion
    }
}
