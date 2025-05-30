namespace @Namespace
{
    [global::System.Diagnostics.DebuggerDisplay("{DebuggerDisplay}")]
    [global::System.ComponentModel.TypeConverter(typeof(SvoTypeConverter))]
#if NET6_0_OR_GREATER
    [global::System.Text.Json.Serialization.JsonConverter(typeof(SvoJsonConverter))]
#endif
    public readonly partial struct @Svo
        : global::System.Xml.Serialization.IXmlSerializable
        , global::System.IFormattable
        , global::System.IEquatable<@Svo>
        , global::System.IComparable
        , global::System.IComparable<@Svo>
        , global::Qowaiv.IUnknown<@Svo>
#if NET8_0_OR_GREATER
        , global::System.Numerics.IEqualityOperators<@Svo, @Svo, bool>
        , global::System.IParsable<@Svo>
#endif
    {
        /// <summary>An singleton instance that deals with the identifier specific behavior.</summary>
        [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
        private static readonly @Behavior behavior = new();

        /// <summary>Represents an empty/not set Single Value Object.</summary>
        public static @Svo Empty { get { return default; } }

        /// <summary>Represents an unknown (but set) Single Value Object.</summary>
        public static @Svo Unknown { get { return new @Svo("\uFFFF"); } }

        /// <summary>Initializes a new instance of the <see cref="@Svo" /> struct.</summary>
        private @Svo(string? value) { m_Value = value; }

        /// <summary>The inner value of the Single Value Object.</summary>
        [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly string? m_Value;

        /// <summary>Returns true if the Single Value Object is set.</summary>
        [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
        public bool HasValue { get { return m_Value is { }; } }

        /// <summary>False if theSingle Value Object is empty or unknown, otherwise false.</summary>
        [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
        public bool IsKnown { get { return m_Value is { } && m_Value != "\uFFFF"; } }

        /// <summary>Gets the number of characters of Single Value Object.</summary>
        public int Length { get { return IsKnown ? behavior.Length(m_Value!) : 0; } }

        /// <inheritdoc />
        [global::System.Diagnostics.Contracts.Pure]
        public int CompareTo(object? obj)
        {
            if (obj is null) return +1;
            else if (obj is @Svo other) return CompareTo(other);
            else throw new global::System.ArgumentException($"Argument must be {GetType().Name}.", nameof(obj));
        }

        /// <inheritdoc />
        [global::System.Diagnostics.Contracts.Pure]
#if NET6_0_OR_GREATER
        public int CompareTo(@Svo other) { return behavior.Compare(m_Value, other.m_Value); }
#else
        public int CompareTo(@Svo other)
        {
            // Comparing with char.max value does not work as expected in older versions of .NET
            if (IsKnown && other.IsKnown)
            {
                return behavior.Compare(m_Value, other.m_Value);
            }
            else if (IsKnown)
            {
                return -1;
            }
            else return other.IsKnown ? +1 : 0;
        }
#endif

        /// <inheritdoc />
        [global::System.Diagnostics.Contracts.Pure]
        public override bool Equals([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] object? obj)
        {
            return obj is @Svo other && Equals(other);
        }

        /// <summary>Returns true if this instance and the other Single Value Object are equal, otherwise false.</summary>
        /// <param name="other">The <see cref="@Svo" /> to compare with.</param>
        [global::System.Diagnostics.Contracts.Pure]
        public bool Equals(@Svo other) { return m_Value == other.m_Value; }

        /// <inheritdoc />
        [global::System.Diagnostics.Contracts.Pure]
        public override int GetHashCode() { return global::Qowaiv.Hashing.Hash.Code(m_Value); }

        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        public static bool operator ==(@Svo left, @Svo right) { return left.Equals(right); }

        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        public static bool operator !=(@Svo left, @Svo right) { return !(left == right); }

        /// <summary>Returns a <see cref="string" /> that represents the Single Value Object for DEBUG purposes.</summary>
        [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get
            {
                if (IsKnown) return ToString("F");
                else if (HasValue) return "{unknown}";
                else return "{empty}";
            }
        }

        /// <summary>Returns a <see cref="string" /> that represents the Single Value Object.</summary>
        [global::System.Diagnostics.Contracts.Pure]
        public override string ToString() { return ToString(provider: null); }

        /// <summary>Returns a formatted <see cref="string" /> that represents the Single Value Object.</summary>
        /// <param name="format">
        /// The format that describes the formatting.
        /// </param>
        [global::System.Diagnostics.Contracts.Pure]
        public string ToString(string? format) { return ToString(format, formatProvider: null); }

        /// <summary>Returns a formatted <see cref="string" /> that represents the Single Value Object.</summary>
        /// <param name="provider">
        /// The format provider.
        /// </param>
        [global::System.Diagnostics.Contracts.Pure]
        public string ToString(global::System.IFormatProvider? provider) => ToString(format: null, provider);

        /// <summary>Returns a formatted <see cref="string" /> that represents the Single Value Object.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        [global::System.Diagnostics.Contracts.Pure]
        public string ToString(string? format, global::System.IFormatProvider? formatProvider)
        {
            return behavior.ToString(m_Value, format, formatProvider);
        }

        /// <summary>Gets the <see href="XmlSchema" /> to XML (de)serialize the Single Value Object.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        [global::System.Diagnostics.Contracts.Pure]
        global::System.Xml.Schema.XmlSchema? global::System.Xml.Serialization.IXmlSerializable.GetSchema()
        {
            return null;
        }

        /// <summary>Reads the Single Value Object from an <see href="XmlReader" />.</summary>
        /// <param name="reader">An XML reader.</param>
        void global::System.Xml.Serialization.IXmlSerializable.ReadXml(global::System.Xml.XmlReader reader)
        {
#if NET6_0_OR_GREATER
            global::System.ArgumentNullException.ThrowIfNull(reader);
#else
            if (reader is null) throw new global::System.ArgumentNullException(nameof(reader));
#endif
            var xml = reader.ReadElementString();
            global::System.Runtime.CompilerServices.Unsafe.AsRef(in this) = Parse(xml, global::System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>Writes the Single Value Object to an <see href="XmlWriter" />.</summary>
        /// <remarks>
        /// Uses <see cref="ToXmlString()" />.
        /// </remarks>
        /// <param name="writer">An XML writer.</param>
        void global::System.Xml.Serialization.IXmlSerializable.WriteXml(global::System.Xml.XmlWriter writer)
        {
#if NET6_0_OR_GREATER
            global::System.ArgumentNullException.ThrowIfNull(writer);
#else
            if (writer is null) throw new global::System.ArgumentNullException(nameof(writer));
#endif
            writer.WriteString(ToXmlString());
        }

        /// <summary>Gets an XML string representation of the Single Value Object.</summary>
        [global::System.Diagnostics.Contracts.Pure]
        private string? ToXmlString() { return behavior.ToXml(m_Value); }

        /// <summary>Creates the Single Value Object from a JSON string.</summary>
        /// <param name="json">
        /// The JSON string to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized Single Value Object.
        /// </returns>
        [global::System.Diagnostics.Contracts.Pure]
        public static @Svo FromJson(string? json) { return Parse(json, global::System.Globalization.CultureInfo.InvariantCulture); }

        /// <summary>Serializes the Single Value Object to a JSON node.</summary>
        /// <returns>
        /// The serialized JSON string.
        /// </returns>
        [global::System.Diagnostics.Contracts.Pure]
        public string? ToJson() { return behavior.ToJson(m_Value); }

        /// <summary>Casts the Single Value Object to a <see cref="string" />.</summary>
        public static explicit operator string(@Svo val)
        {
            return val.ToString(global::System.Globalization.CultureInfo.CurrentCulture);
        }

        /// <summary>Casts a <see cref="string" /> to a Single Value Object.</summary>
        public static explicit operator @Svo(string str)
        {
            return Parse(str, global::System.Globalization.CultureInfo.CurrentCulture);
        }

        /// <summary>Converts the <see cref="string" /> to <see cref="@Svo" />.</summary>
        /// <param name="s">
        /// A string containing the Single Value Object to convert.
        /// </param>
        /// <returns>
        /// The parsed Single Value Object.
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="s" /> is not in the correct format.
        /// </exception>
        [global::System.Diagnostics.Contracts.Pure]
        public static @Svo Parse(string? s) { return Parse(s, null); }

        /// <summary>Converts the <see cref="string" /> to <see cref="@Svo" />.</summary>
        /// <param name="s">
        /// A string containing the Single Value Object to convert.
        /// </param>
        /// <param name="provider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// The parsed Single Value Object.
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="s" /> is not in the correct format.
        /// </exception>
        [global::System.Diagnostics.Contracts.Pure]
        public static @Svo Parse(string? s, global::System.IFormatProvider? provider)
        {
            return TryParse(s, provider)
                ?? throw behavior.InvalidFormat(s, provider);
        }

        /// <summary>Converts the <see cref="string" /> to <see cref="@Svo" />.</summary>
        /// <param name="s">
        /// A string containing the Single Value Object to convert.
        /// </param>
        /// <returns>
        /// The Single Value Object if the string was converted successfully, otherwise default.
        /// </returns>
        [global::System.Diagnostics.Contracts.Pure]
        public static @Svo? TryParse(string? s) { return TryParse(s, null); }

        /// <summary>Converts the <see cref="string" /> to <see cref="@Svo" />.</summary>
        /// <param name="s">
        /// A string containing the Single Value Object to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// The Single Value Object if the string was converted successfully, otherwise default.
        /// </returns>
        [global::System.Diagnostics.Contracts.Pure]
        public static @Svo? TryParse(string? s, global::System.IFormatProvider? formatProvider)
        {
            return TryParse(s, formatProvider, out var val)
                ? val
                : default(@Svo?);
        }

        /// <summary>Converts the <see cref="string" /> to <see cref="@Svo" />.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing the Single Value Object to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        [global::System.Diagnostics.Contracts.Pure]
        public static bool TryParse(string? s, out @Svo result)
        {
            return TryParse(s, null, out result);
        }

        /// <summary>Converts the <see cref="string" /> to <see cref="@Svo" />.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing the Single Value Object to convert.
        /// </param>
        /// <param name="provider">
        /// The specified format provider.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        [global::System.Diagnostics.Contracts.Pure]
        public static bool TryParse(string? s, global::System.IFormatProvider? provider, out @Svo result)
        {
            var success = behavior.TryParse(s, provider, out var parsed);
            result = new @Svo(parsed);
            return success;
        }
        private sealed class SvoTypeConverter : global::Qowaiv.Conversion.SvoTypeConverter<@Svo>
        {
            /// <inheritdoc />
            [global::System.Diagnostics.Contracts.Pure]
            protected override @Svo FromString(string? str, global::System.Globalization.CultureInfo? culture)
            {
                return Parse(str, culture);
            }
        }

#if NET6_0_OR_GREATER
        private sealed class SvoJsonConverter : global::Qowaiv.Json.SvoJsonConverter<@Svo>
        {
            /// <inheritdoc />
            [global::System.Diagnostics.Contracts.Pure]
            protected override @Svo FromJson(string? json) => @Svo.FromJson(json);

            /// <inheritdoc />
            [global::System.Diagnostics.Contracts.Pure]
            protected override object? ToJson(@Svo svo) => svo.ToJson();
        }
#endif
    }
}
