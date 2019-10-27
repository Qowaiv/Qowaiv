﻿#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion.Financial;
using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv.Financial
{
    /// <summary>Represents </summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.Continuous, typeof(decimal))]
    [OpenApiDataType(description: "Combined currency and amount notation as defined by ISO 4217, for example, EUR 12.47.", type: "string", format: "money", pattern: @"[A-Z]{3} -?[0-9]+(\.[0-9]+)?")]
    [TypeConverter(typeof(MoneyTypeConverter))]
    public struct Money : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<Money>, IComparable, IComparable<Money>
    {
        /// <summary>Represents an Amount of zero.</summary>
        public static readonly Money Zero;
        /// <summary>Represents the smallest possible value of an </summary>
        public static readonly Money MinValue = decimal.MinValue + Currency.Empty;
        /// <summary>Represents the biggest possible value of an </summary>
        public static readonly Money MaxValue = decimal.MaxValue + Currency.Empty;

        #region Properties

        /// <summary>The inner value of the </summary>
        private decimal m_Value;
        private Currency m_Currency;

        /// <summary>Gets the currency of the money.</summary>
        public Currency Currency => m_Currency;

        #endregion

        #region Methods

        /// <summary>Returns the absolute value of the money.</summary>
        public Money Abs() => Math.Abs(m_Value) + Currency;

        /// <summary>Pluses the money.</summary>
        internal Money Plus() => +m_Value + Currency;

        /// <summary>Negates the money.</summary>
        internal Money Negate() => -m_Value + Currency;

        /// <summary>Increases the money with one (of the current currency).</summary>
        internal Money Increment() => (m_Value + 1) + Currency;

        /// <summary>Decreases the money with one (of the current currency).</summary>
        internal Money Decrement() => (m_Value - 1) + Currency;

        /// <summary>Decreases the amount with one.</summary>
        /// <summary>Adds a amount to the current amount.</summary>
        /// <param name="money">
        /// The money to add.
        /// </param>
        public Money Add(Money money) => (m_Value + money.m_Value) + HaveSameCurrency(this, money, "addition");

        /// <summary>Adds the specified percentage to the amount.</summary>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        public Money Add(Percentage p) => m_Value.Add(p) + Currency;

        /// <summary>Subtracts a amount from the current amount.</summary>
        /// <param name="money">
        /// The money to Subtract.
        /// </param>
        public Money Subtract(Money money) => (m_Value - money.m_Value) + HaveSameCurrency(this, money, "subtraction");

        /// <summary>AddsSubtract the specified percentage from the amount.</summary>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        public Money Subtract(Percentage p) => m_Value.Subtract(p) + Currency;

        /// <summary>Gets a percentage of the money.</summary>
        /// <param name="p">
        /// The percentage to get.
        /// </param>
        public Money Multiply(Percentage p) => (m_Value * p) + Currency;

        /// <summary>Multiplies the money with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Money Multiply(decimal factor) => (m_Value * factor) + Currency;

        /// <summary>Multiplies the money with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Money Multiply(double factor) => Multiply((decimal)factor);

        /// <summary>Multiplies the money with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Money Multiply(float factor) => Multiply((decimal)factor);


        /// <summary>Multiplies the money with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Money Multiply(long factor) => Multiply((decimal)factor);

        /// <summary>Multiplies the money with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Money Multiply(int factor) => Multiply((decimal)factor);

        /// <summary>Multiplies the money with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Money Multiply(short factor) => Multiply((decimal)factor);


        /// <summary>Multiplies the money with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        [CLSCompliant(false)]
        public Money Multiply(ulong factor) => Multiply((decimal)factor);

        /// <summary>Multiplies the money with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        [CLSCompliant(false)]
        public Money Multiply(uint factor) => Multiply((decimal)factor);

        /// <summary>Multiplies the money with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        [CLSCompliant(false)]
        public Money Multiply(ushort factor) => Multiply((decimal)factor);


        /// <summary>Divides the money by a specified money.</summary>
        /// <param name="p">
        /// The money to devides to..
        /// </param>
        public Money Divide(Percentage p) => (m_Value / p) + Currency;

        /// <summary>Divides the money by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Money Divide(decimal factor) => (m_Value / factor) + Currency;

        /// <summary>Divides the money by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Money Divide(double factor) => Divide((decimal)factor);

        /// <summary>Divides the money by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Money Divide(float factor) => Divide((decimal)factor);


        /// <summary>Divides the money by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Money Divide(long factor) => Divide((decimal)factor);

        /// <summary>Divides the money by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Money Divide(int factor) => Divide((decimal)factor);

        /// <summary>Divides the money by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Money Divide(short factor) => Divide((decimal)factor);


        /// <summary>Divides the money by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        [CLSCompliant(false)]
        public Money Divide(ulong factor) => Divide((decimal)factor);

        /// <summary>Divides the money by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        [CLSCompliant(false)]
        public Money Divide(uint factor) => Divide((decimal)factor);

        /// <summary>Divides the money by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        [CLSCompliant(false)]
        public Money Divide(ushort factor) => Divide((decimal)factor);

        /// <summary>Rounds the money value to the preferred number decimal places, based on its currency.</summary>
        public Money Round() => Round(Currency.Digits);

        /// <summary>Rounds the money value to a specified number of decimal places.</summary>
        /// <param name="decimals">
        /// A value from -28 to 28 that specifies the number of decimal places to round to.
        /// </param>
        /// <remarks>
        /// A negative value for <paramref name="decimals"/> lowers precision to tenfold, hundredfold, and bigger.
        /// </remarks>
        public Money Round(int decimals) => Round(decimals, DecimalRounding.BankersRound);

        /// <summary>Rounds the money value to a specified number of decimal places.</summary>
        /// <param name="decimals">
        /// A value from -28 to 28 that specifies the number of decimal places to round to.
        /// </param>
        /// <param name="mode">
        /// The mode of rounding applied.
        /// </param>
        /// <remarks>
        /// A negative value for <paramref name="decimals"/> lowers precision to tenfold, hundredfold, and bigger.
        /// </remarks>
        public Money Round(int decimals, DecimalRounding mode) => Create(m_Value.Round(decimals, mode), Currency);

        /// <summary>Rounds the money value to the closed number that is a multiple of the specified factor.</summary>
        /// <param name="multipleOf">
        /// The factor of which the number should be multiple of.
        /// </param>
        public Money RoundToMultiple(decimal multipleOf) => RoundToMultiple(multipleOf, DecimalRounding.BankersRound);

        /// <summary>Rounds the money value to the closed number that is a multiple of the specified factor.</summary>
        /// <param name="multipleOf">
        /// The factor of which the number should be multiple of.
        /// </param>
        /// <param name="mode">
        /// The rounding method used to determine the closed by number.
        /// </param>
        public Money RoundToMultiple(decimal multipleOf, DecimalRounding mode) => Create(m_Value.RoundToMultiple(multipleOf, mode), Currency);

        /// <summary>Increases the money with one (of the current currency).</summary>
        public static Money operator ++(Money money) => money.Increment();
        /// <summary>Decreases the money with one (of the current currency).</summary>
        public static Money operator --(Money money) => money.Decrement();

        /// <summary>Unitary plusses the money.</summary>
        public static Money operator +(Money money) => money.Plus();
        /// <summary>Negates the money.</summary>
        public static Money operator -(Money money) => money.Negate();

        /// <summary>Adds money.</summary>
        /// <param name="l">The left operand.</param>
        /// <param name="r">The right operand</param>
        public static Money operator +(Money l, Money r) => l.Add(r);
        
        /// <summary>Adds the percentage to the money.</summary>
        public static Money operator +(Money money, Percentage p) => (money.m_Value + p) + money.Currency;

        /// <summary>Adds money.</summary>
        /// <param name="l">The left operand.</param>
        /// <param name="r">The right operand</param>
        public static Money operator -(Money l, Money r) => l.Subtract(r);

        /// <summary>Subtracts the percentage from the money.</summary>
        public static Money operator -(Money money, Percentage p) => (money.m_Value - p) + money.Currency;

        /// <summary>Multiplies the money with the factor.</summary>
        public static Money operator *(Money money, Percentage factor) => money.Multiply(factor);

        /// <summary>Multiplies the money with the factor.</summary>
        public static Money operator *(Money money, decimal factor) => money.Multiply(factor);
        /// <summary>Multiplies the money with the factor.</summary>
        public static Money operator *(Money money, double factor) => money.Multiply(factor);
        /// <summary>Multiplies the money with the factor.</summary>
        public static Money operator *(Money money, float factor) => money.Multiply(factor);

        /// <summary>Multiplies the money with the factor.</summary>
        public static Money operator *(Money money, long factor) => money.Multiply(factor);
        /// <summary>Multiplies the money with the factor.</summary>
        public static Money operator *(Money money, int factor) => money.Multiply(factor);
        /// <summary>Multiplies the money with the factor.</summary>
        public static Money operator *(Money money, short factor) => money.Multiply(factor);

        /// <summary>Multiplies the money with the factor.</summary>
        [CLSCompliant(false)]
        public static Money operator *(Money money, ulong factor) => money.Multiply(factor);
        /// <summary>Multiplies the money with the factor.</summary>
        [CLSCompliant(false)]
        public static Money operator *(Money money, uint factor) => money.Multiply(factor);
        /// <summary>Multiplies the money with the factor.</summary>
        [CLSCompliant(false)]
        public static Money operator *(Money money, ushort factor) => money.Multiply(factor);


        /// <summary>Divides the money by the percentage.</summary>
        public static Money operator /(Money money, Percentage p) => money.Divide(p);
        /// <summary>Divides the money by the factor.</summary>
        public static Money operator /(Money money, decimal factor) => money.Divide(factor);
        /// <summary>Divides the money by the factor.</summary>
        public static Money operator /(Money money, double factor) => money.Divide(factor);
        /// <summary>Divides the money by the factor.</summary>
        public static Money operator /(Money money, float factor) => money.Divide(factor);

        /// <summary>Divides the money by the factor.</summary>
        public static Money operator /(Money money, long factor) => money.Divide(factor);
        /// <summary>Divides the money by the factor.</summary>
        public static Money operator /(Money money, int factor) => money.Divide(factor);
        /// <summary>Divides the money by the factor.</summary>
        public static Money operator /(Money money, short factor) => money.Divide(factor);

        /// <summary>Divides the money by the factor.</summary>
        [CLSCompliant(false)]
        public static Money operator /(Money money, ulong factor) => money.Divide(factor);
        /// <summary>Divides the money by the factor.</summary>
        [CLSCompliant(false)]
        public static Money operator /(Money money, uint factor) => money.Divide(factor);
        /// <summary>Divides the money by the factor.</summary>
        [CLSCompliant(false)]
        public static Money operator /(Money money, ushort factor) => money.Divide(factor);



        [DebuggerStepThrough]
        private static Currency HaveSameCurrency(Money l, Money r, string operation)
        {
            if (l.Currency != r.Currency)
            {
                throw new CurrencyMismatchException(l.Currency, r.Currency, operation);
            }
            return l.Currency;
        }
        #endregion

        #region (XML) (De)serialization

        /// <summary>Initializes a new instance of Money based on the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private Money(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = info.GetDecimal("Value");
            m_Currency = Currency.Parse(info.GetString("Currency"));
        }

        /// <summary>Adds the underlying property of Money to the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            info.AddValue("Value", m_Value);
            info.AddValue("Currency", m_Currency.Name);
        }

        /// <summary>Gets the <see href="XmlSchema"/> to (de) XML serialize Money.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <summary>Reads the Money from an <see href="XmlReader"/>.</summary>
        /// <remarks>
        /// Uses the string parse function of Money.
        /// </remarks>
        /// <param name="reader">An XML reader.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Guard.NotNull(reader, nameof(reader));
            var s = reader.ReadElementString();
            var val = Parse(s, CultureInfo.InvariantCulture);
            m_Value = val.m_Value;
            m_Currency = val.m_Currency;
        }

        /// <summary>Writes the Money to an <see href="XmlWriter"/>.</summary>
        /// <remarks>
        /// Uses the string representation of Money.
        /// </remarks>
        /// <param name="writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            Guard.NotNull(writer, nameof(writer));
            writer.WriteString(Currency.Name);
            writer.WriteString(m_Value.ToString("", CultureInfo.InvariantCulture));
        }

        #endregion

        #region (JSON) (De)serialization

        /// <summary>Generates Money from a JSON null object representation.</summary>
        void IJsonSerializable.FromJson() => throw new NotSupportedException(QowaivMessages.JsonSerialization_NullNotSupported);

        /// <summary>Generates Money from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the 
        /// </param>
        void IJsonSerializable.FromJson(string jsonString)
        {
            var money = Parse(jsonString, CultureInfo.InvariantCulture);
            m_Value = money.m_Value;
            m_Currency = money.m_Currency;
        }

        /// <summary>Generates Money from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the 
        /// </param>
        void IJsonSerializable.FromJson(long jsonInteger)
        {
            var money = Create(jsonInteger, Currency.Empty);
            m_Value = money.m_Value;
            m_Currency = money.m_Currency;
        }

        /// <summary>Generates Money from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the 
        /// </param>
        void IJsonSerializable.FromJson(double jsonNumber)
        {
            var money = Create((decimal)jsonNumber, Currency.Empty);
            m_Value = money.m_Value;
            m_Currency = money.m_Currency;
        }

        /// <summary>Generates Money from a JSON date representation.</summary>
        /// <param name="jsonDate">
        /// The JSON Date that represents the 
        /// </param>
        void IJsonSerializable.FromJson(DateTime jsonDate) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported);

        /// <summary>Converts Money into its JSON object representation.</summary>
        object IJsonSerializable.ToJson()
        {
            return ToString(CultureInfo.InvariantCulture);
        }

        #endregion

        #region IFormattable / ToString

        /// <summary>Returns a <see cref="string"/> that represents the current Money for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Debugger.")]
        private string DebuggerDisplay
        {
            get => string.Format(CultureInfo.InvariantCulture, "{0} {1}", m_Currency, m_Value);
        }

        /// <summary>Returns a <see cref="string"/> that represents the current </summary>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);


        /// <summary>Returns a formatted <see cref="string"/> that represents the current </summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);


        /// <summary>Returns a formatted <see cref="string"/> that represents the current </summary>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider formatProvider) => ToString(null, formatProvider);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current </summary>
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
            var numberFormatInfo = Currency.GetNumberFormatInfo(formatProvider);
            return m_Value.ToString(format ?? "C", numberFormatInfo);
        }

        #endregion

        #region IEquatable

        /// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
        /// <param name="obj">An object to compare with.</param>
        public override bool Equals(object obj) => obj is Money && Equals((Money)obj);

        /// <summary>Returns true if this instance and the other <see cref="Money"/> are equal, otherwise false.</summary>
        /// <param name="other">The <see cref="Money"/> to compare with.</param>
        public bool Equals(Money other) => m_Value == other.m_Value && m_Currency == other.m_Currency;

        /// <summary>Returns the hash code for this </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => m_Value.GetHashCode() ^ m_Currency.GetHashCode();

        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator ==(Money left, Money right) => left.Equals(right);

        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator !=(Money left, Money right) => !(left == right);

        #endregion

        #region IComparable

        /// <summary>Compares this instance with a specified System.Object and indicates whether
        /// this instance precedes, follows, or appears in the same position in the sort
        /// order as the specified System.Object.
        /// </summary>
        /// <param name="obj">
        /// An object that evaluates to a 
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.Value
        /// Condition Less than zero This instance precedes value. Zero This instance
        /// has the same position in the sort order as value. Greater than zero This
        /// instance follows value.-or- value is null.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// value is not a 
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj is Money)
            {
                return CompareTo((Money)obj);
            }
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.ArgumentException_Must, "Money"), "obj");
        }

        /// <summary>Compares this instance with a specified Money and indicates
        /// whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified 
        /// </summary>
        /// <param name="other">
        /// The Money to compare with this instance.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(Money other)
        {
            var compare = m_Currency.CompareTo(other.m_Currency);
            if (compare == 0)
            {
                compare = m_Value.CompareTo(other.m_Value);
            }
            return compare;
        }


        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(Money l, Money r) => l.CompareTo(r) < 0;

        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator >(Money l, Money r) => l.CompareTo(r) > 0;

        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(Money l, Money r) => l.CompareTo(r) <= 0;

        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(Money l, Money r) => l.CompareTo(r) >= 0;

        #endregion

        #region (Explicit) casting

        /// <summary>Casts Money to a <see cref="string"/>.</summary>
        public static explicit operator string(Money val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a </summary>
        public static explicit operator Money(string str) => Parse(str, CultureInfo.CurrentCulture);

        /// <summary>Casts an Amount to Money.</summary>
        public static implicit operator Money(Amount val) => Create((decimal)val);
        /// <summary>Casts a decimal to Money.</summary>
        public static implicit operator Money(decimal val) => Create(val);
        /// <summary>Casts a double to Money.</summary>
        public static implicit operator Money(double val) => Create((decimal)val);
        /// <summary>Casts a double to Money.</summary>
        public static implicit operator Money(int val) => Create(val);

        /// <summary>Casts Money to a decimal.</summary>
        public static explicit operator Amount(Money val) => val.m_Value;
        /// <summary>Casts Money to a decimal.</summary>
        public static explicit operator decimal(Money val) => val.m_Value;
        /// <summary>Casts Money to a double.</summary>
        public static explicit operator double(Money val) => (double)val.m_Value;

        #endregion

        #region Factory methods

        /// <summary>Converts the string to </summary>
        /// <param name="s">
        /// A string containing Money to convert.
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static Money Parse(string s) => Parse(s, CultureInfo.CurrentCulture);

        /// <summary>Converts the string to </summary>
        /// <param name="s">
        /// A string containing Money to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static Money Parse(string s, IFormatProvider formatProvider)
        {
            if (TryParse(s, formatProvider, out Money val))
            {
                return val;
            }
            throw new FormatException(QowaivMessages.FormatExceptionMoney);
        }

        /// <summary>Converts the string to 
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing Money to convert.
        /// </param>
        /// <returns>
        /// The Money if the string was converted successfully, otherwise Empty.
        /// </returns>
        public static Money TryParse(string s)
        {
            if (TryParse(s, out Money val))
            {
                return val;
            }
            return Zero;
        }

        /// <summary>Converts the string to 
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing Money to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out Money result)
        {
            return TryParse(s, CultureInfo.CurrentCulture, out result);
        }

        /// <summary>Converts the string to 
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing Money to convert.
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
        public static bool TryParse(string s, IFormatProvider formatProvider, out Money result)
        {
            result = Zero;
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            string s_num;
            string s_cur;

            var culture = formatProvider as CultureInfo ?? CultureInfo.InvariantCulture;

            var buffer = new List<char>(s.Length);
            var min = culture.NumberFormat.NegativeSign[0];
            var pls = culture.NumberFormat.PositiveSign[0];

            foreach (var ch in s)
            {
                if (ch == min || ch == pls || (ch >= '0' && ch <= '9')) { break; }
                buffer.Add(ch);
            }
            if (buffer.Count > 0)
            {
                s_num = s.Substring(buffer.Count);
                s_cur = new string(buffer.ToArray());
            }
            else
            {
                for (var pos = s.Length - 1; pos >= 0; pos--)
                {
                    var ch = s[pos];
                    if (ch == min || ch == pls || (ch >= '0' && ch <= '9')) { break; }
                    buffer.Insert(0, ch);
                }

                s_num = s.Substring(0, s.Length - buffer.Count);
                s_cur = new string(buffer.ToArray());
            }

            Currency currency = Currency.Empty;
            if ((s_cur == "" || Currency.TryParse(s_cur, out currency)) &&
                decimal.TryParse(s_num, NumberStyles.Number, GetNumberFormatInfo(formatProvider), out decimal value))
            {
                result = Create(value, currency);
                return true;
            }
            return false;
        }

        /// <summary >Creates Money from a Decimal. </summary >
        /// <param name="val" >
        /// The amount.
        /// </param>
        public static Money Create(decimal val)
        {
            return Create(val, Currency.Current);
        }

        /// <summary >Creates Money from a Decimal. </summary >
        /// <param name="val" >
        /// A decimal describing the amount.
        /// </param >
        /// <param name="currency">
        /// The currency of the amount.
        /// </param>
        public static Money Create(decimal val, Currency currency)
        {
            return new Money { m_Value = val, m_Currency = currency };
        }

        #endregion

        #region Validation

        /// <summary>Returns true if the val represents a valid Money, otherwise false.</summary>
        public static bool IsValid(string val) => IsValid(val, CultureInfo.CurrentCulture);

        /// <summary>Returns true if the val represents a valid Money, otherwise false.</summary>
        public static bool IsValid(string val, IFormatProvider formatProvider)
        {
            return TryParse(val, formatProvider, out _);
        }

        #endregion

        /// <summary>Gets a <see cref="NumberFormatInfo"/> based on the <see cref="IFormatProvider"/>.</summary>
        /// <remarks>
        /// Because the options for formatting and parsing currencies as provided 
        /// by the .NET framework are not sufficient, internally we use number
        /// settings. For parsing and formatting however we like to use the
        /// currency properties of the <see cref="NumberFormatInfo"/> instead of
        /// the number properties, so we copy them for desired behavior.
        /// </remarks>
        internal static NumberFormatInfo GetNumberFormatInfo(IFormatProvider formatProvider)
        {
            var info = NumberFormatInfo.GetInstance(formatProvider);
            info = (NumberFormatInfo)info.Clone();
            info.NumberDecimalDigits = info.CurrencyDecimalDigits;
            info.NumberDecimalSeparator = info.CurrencyDecimalSeparator;
            info.NumberGroupSeparator = info.CurrencyGroupSeparator;
            info.NumberGroupSizes = info.CurrencyGroupSizes;
            return info;
        }
    }
}
