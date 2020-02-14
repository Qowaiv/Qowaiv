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
    [Serializable]
    [SingleValueObject(SingleValueStaticOptions.Continuous, typeof(double))]
    [OpenApiDataType(description: "Elo rating system notation.", type: "number", format: "elo")]
    [TypeConverter(typeof(EloTypeConverter))]
    public partial struct Elo : ISerializable, IXmlSerializable, IFormattable, IEquatable<Elo>, IComparable, IComparable<Elo>
    {
        /// <summary>Represents the zero value of an Elo.</summary>
        public static readonly Elo Zero;

        /// <summary>Represents the minimum value of an Elo.</summary>
        public static readonly Elo MinValue = new Elo(double.MinValue);

        /// <summary>Represents the maximum value of an Elo.</summary>
        public static readonly Elo MaxValue = new Elo(double.MaxValue);

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

        /// <summary>Deserializes the Elo from a JSON number.</summary>
        /// <param name="json">
        /// The JSON number to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized Elo.
        /// </returns>
        public static Elo FromJson(double json) => Create(json);

        /// <summary>Deserializes the Elo from a JSON number.</summary>
        /// <param name="json">
        /// The JSON number to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized Elo.
        /// </returns>
        public static Elo FromJson(long json) => Create(json);

        /// <summary>Serializes the Elo to a JSON node.</summary>
        /// <returns>
        /// The serialized JSON number.
        /// </returns>
        public double ToJson() => m_Value;

        /// <summary>Returns a <see cref="string"/> that represents the current Elo for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double DebuggerDisplay => m_Value;

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

        /// <summary>Gets an XML string representation of the Elo.</summary>
        private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

        /// <summary>Casts an Elo to a <see cref="string"/>.</summary>
        public static explicit operator string(Elo val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a Elo.</summary>
        public static explicit operator Elo(string str) => Parse(str, CultureInfo.CurrentCulture);

        /// <summary>Casts a decimal to an Elo.</summary>
        public static implicit operator Elo(decimal val) => new Elo((double)val);
        /// <summary>Casts a decimal to an Elo.</summary>
        public static implicit operator Elo(double val) => new Elo(val);
        /// <summary>Casts an integer to an Elo.</summary>
        public static implicit operator Elo(int val) => new Elo(val);

        /// <summary>Casts an Elo to a decimal.</summary>
        public static explicit operator decimal(Elo val) => (decimal)val.m_Value;
        /// <summary>Casts an Elo to a double.</summary>
        public static explicit operator double(Elo val) => val.m_Value;
        /// <summary>Casts an Elo to an integer.</summary>
        public static explicit operator int(Elo val) => (int)Math.Round(val.m_Value);

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

                if (double.TryParse(str, NumberStyles.Number, formatProvider, out var d) && !double.IsNaN(d) && !double.IsInfinity(d))
                {
                    result = new Elo { m_Value = d };
                    return true;
                }
            }
            return false;
        }

        /// <summary>Creates an <see cref="Elo"/> from a <see cref="double"/>.</summary>
        /// <param name="val">
        /// A decimal describing an <see cref="Elo"/>.
        /// </param>
        /// <exception cref="ArgumentException">
        /// val is not a valid <see cref="Elo"/>.
        /// </exception>
        public static Elo Create(double val)
        {
            if (double.IsNaN(val) || double.IsInfinity(val))
            {
                throw new ArgumentOutOfRangeException(nameof(val), QowaivMessages.ArgumentOutOfRange_Elo);
            }
            return new Elo(val);
        }
    }
}
