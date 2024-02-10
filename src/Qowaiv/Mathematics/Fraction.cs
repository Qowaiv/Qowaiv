using Qowaiv.Conversion.Mathematics;
using System.Runtime.InteropServices;

namespace Qowaiv.Mathematics;

/// <summary>Represents a fraction.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.Continuous, typeof(Tuple<long, long>))]
[OpenApiDataType(description: "Faction", type: "string", format: "faction", pattern: "-?[0-9]+(/[0-9]+)?", example: "13/42")]
[TypeConverter(typeof(FractionTypeConverter))]
#if NET6_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.Mathematics.FractionJsonConverter))]
#endif
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct Fraction : IXmlSerializable, IFormattable, IEquatable<Fraction>, IComparable, IComparable<Fraction>
#if NET8_0_OR_GREATER
#else
, ISerializable
#endif
{
    /// <summary>Represents the zero (0) <see cref="Fraction"/> value.</summary>
    /// <remarks>
    /// Is the default value of <see cref="Fraction"/>.
    /// </remarks>
    public static Fraction Zero => default;

    /// <summary>Represents the one (1) <see cref="Fraction"/> value.</summary>
    public static Fraction One => New(1, 1);

    /// <summary>Represents the smallest positive <see cref="Fraction"/> value that is greater than zero.</summary>
    public static readonly Fraction Epsilon = New(1, long.MaxValue);

    /// <summary>Represents the largest possible value of a <see cref="Fraction"/>.</summary>
    public static Fraction MaxValue => New(+long.MaxValue, 1);

    /// <summary>Represents the smallest possible value of a <see cref="Fraction"/>.</summary>
    public static Fraction MinValue => New(-long.MaxValue, 1);

    internal static partial class Formatting
    {
        public const string SuperScript = "⁰¹²³⁴⁵⁶⁷⁸⁹";
        public const string SubScript = "₀₁₂₃₄₅₆₇₈₉";

        public const char Slash = '/';
        public const char Colon = ':';
        public const char DivisionSign = '÷';
        public const char FractionSlash = (char)0x2044;
        public const char DivisionSlash = (char)0x2215;
        public const char ShortSlash = (char)0x337;
        public const char LongSlash = (char)0x338;

        /// <summary>The different supported fraction bar characters.</summary>
        /// <remarks>
        /// name           | c | code
        /// ---------------|---|------
        /// slash          | / |   5C
        /// colon          | : |   3A
        /// division sign  | ÷ |   F7
        /// fraction slash | ⁄ | 2044
        /// division slash | ∕ | 2215
        /// short slash    | ̷  |  337
        /// long slash     | ̸  |  338.
        /// </remarks>
        public static readonly string FractionBars = new(
        [
            Slash,
            Colon,
            DivisionSign,
            FractionSlash,
            DivisionSlash,
            ShortSlash,
            LongSlash,
        ]);

        public static readonly Regex Pattern = GetPattern();

#if NET8_0_OR_GREATER
        [GeneratedRegex(@"^(\[(?<Whole>.+)\] ?)?(?<Numerator>.+?)(?<FractionBars>[/:÷⁄∕̷̸])(?<Denominator>.+)$", RegOptions.Default, RegOptions.TimeoutMilliseconds)]
        private static partial Regex GetPattern();
#else
        [Pure]
        private static Regex GetPattern() => new(
            @"^(\[(?<Whole>.+)\] ?)?(?<Numerator>.+?)(?<FractionBars>[/:÷⁄∕̷̸])(?<Denominator>.+)$",
            RegOptions.Default,
            RegOptions.Timeout);
#endif

        /// <summary>Returns true if the <see cref="char"/> is a supported fraction bar.</summary>
        [Pure]
        public static bool IsFractionBar(char ch) => FractionBars.Contains(ch);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly long numerator;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly long denominator;

    /// <summary>Initializes a new instance of the <see cref="Fraction"/> struct.</summary>
    /// <param name="numerator">
    /// The numerator part of the fraction.
    /// </param>
    /// <param name="denominator">
    /// The denominator part of the fraction.
    /// </param>
    /// <exception cref="DivideByZeroException">
    /// if the <paramref name="denominator"/> is zero.
    /// </exception>
    /// <remarks>
    /// If the fraction is negative, the numerator 'carries' the sign.
    /// Furthermore, the numerator and denominator are reduced (so 2/4
    /// becomes 1/2).
    /// </remarks>
    public Fraction(long numerator, long denominator)
        : this(new Data(numerator, denominator).Guard().Simplify()) { }

    /// <summary>Initializes a new instance of the <see cref="Fraction"/> struct.</summary>
    private Fraction(Data data)
    {
        numerator = data.numerator;
        denominator = data.denominator;
    }

    /// <summary>Gets the numerator of the fraction.</summary>
    public long Numerator => numerator;

    /// <summary>Gets the denominator of the fraction.</summary>
    public long Denominator => IsZero() ? 1 : denominator;

    /// <summary>Get whole of the fraction.</summary>
    public long Whole => IsZero() ? 0 : numerator / denominator;

    /// <summary>Gets the remainder of the fraction.</summary>
    /// <remarks>
    /// The remainder is expressed as an absolute value.
    /// </remarks>
    public long Remainder => IsZero() ? 0 : (numerator % denominator).Abs();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0:super⁄sub} = {0:0.########}");

    /// <summary>Returns true if the faction is zero.</summary>
    [Pure]
    public bool IsZero() => numerator == 0;

    /// <summary>Gets the sign of the fraction.</summary>
    [Pure]
    public int Sign() => Math.Sign(numerator);

    /// <summary>Returns the absolute value of the fraction.</summary>
    [Pure]
    public Fraction Abs() => New(numerator.Abs(), denominator);

    /// <summary>Gets the inverse of a faction.</summary>
    /// <exception cref="DivideByZeroException">
    /// When the fraction is <see cref="Zero"/>.
    /// </exception>
    [Pure]
    public Fraction Inverse()
        => IsZero()
        ? throw new DivideByZeroException()
        : New(Sign() * denominator, numerator.Abs());

    /// <summary>Returns a formatted <see cref = "string "/> that represents the fraction.</summary>
    /// <param name = "format">
    /// The format that this describes the formatting.
    /// </param>
    /// <param name = "formatProvider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
        {
            return formatted;
        }
        else if (Formatting.Pattern.Match(format.WithDefault("0/0")) is { Success: true } match)
        {
            return ToString(formatProvider, match);
        }

        // if no fraction bar character has been provided, format as a decimal.
        else if (!format.WithDefault().Any(Formatting.IsFractionBar))
        {
            return ToDecimal().ToString(format, formatProvider);
        }
        else throw new FormatException(QowaivMessages.FormatException_InvalidFormat);
    }

    [Pure]
    private string ToString(IFormatProvider? formatProvider, Match match)
    {
        var sb = new StringBuilder();
        var remainder = AppendWhole(sb, match.Groups[nameof(Whole)].Value, formatProvider);
        AppendNumerator(sb, remainder, match.Groups[nameof(Numerator)].Value, formatProvider);
        sb.Append(match.Groups[nameof(Formatting.FractionBars)].Value);
        AppendDenominator(sb, match.Groups[nameof(Denominator)].Value, formatProvider);
        return sb.ToString();
    }

    [Impure]
    private long AppendWhole(StringBuilder sb, string format, IFormatProvider? formatProvider)
    {
        if (!string.IsNullOrEmpty(format))
        {
            sb.Append(Whole.ToString(format, formatProvider));

            // For -0 n/d
            if (Whole == 0 && Sign() == -1 && sb.Length != 0 && !sb.ToString().Contains(formatProvider.NegativeSign()))
            {
                sb.Insert(0, formatProvider.NegativeSign());
            }
            return Remainder;
        }
        else return numerator;
    }

    private void AppendNumerator(StringBuilder sb, long remainder, string format, IFormatProvider? formatProvider)
    {
        if (format == "super")
        {
            if (sb.Length == 0 && Sign() == -1)
            {
                sb.Append(formatProvider.NegativeSign());
            }

            // use invariant as we want to convert to superscript.
            var super = remainder.Abs().ToString(CultureInfo.InvariantCulture).Select(ch => Formatting.SuperScript[ASCII.Number(ch)]).ToArray();
            sb.Append(super);
        }
        else
        {
            if (sb.Length != 0 && sb[^1] != ' ')
            {
                sb.Append(' ');
            }
            sb.Append(remainder.ToString(format, formatProvider));
        }
    }

    private void AppendDenominator(StringBuilder sb, string format, IFormatProvider? formatProvider)
    {
        if (format == "sub")
        {
            // use invariant as we want to convert to superscript.
            var super = Denominator.ToString(CultureInfo.InvariantCulture).Select(ch => Formatting.SubScript[ASCII.Number(ch)]).ToArray();
            sb.Append(super);
        }
        else
        {
            sb.Append(Denominator.ToString(format, formatProvider));
        }
    }

    /// <inheritdoc/>
    [Pure]
    public bool Equals(Fraction other)
    {
        // to deal with zero (default should be zero).
        if (IsZero() && other.IsZero())
        {
            return true;
        }
        return denominator == other.denominator
            && numerator == other.numerator;
    }

    /// <inheritdoc/>
    [Pure]
    public override int GetHashCode() => Hash.Code(denominator).And(numerator);

    /// <inheritdoc/>
    [Pure]
    public int CompareTo(Fraction other)
    {
        if (denominator == other.denominator || IsZero() || other.IsZero())
        {
            return numerator.CompareTo(other.numerator);
        }

        // To prevent overflows, normalize the numerators as double only.
        var self = (double)numerator * other.denominator;
        var othr = (double)other.numerator * denominator;

        return self.CompareTo(othr);
    }

#if NET8_0_OR_GREATER
#else
    /// <summary>Initializes a new instance of the <see cref="Fraction"/> struct.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    private Fraction(SerializationInfo info, StreamingContext context)
    {
        Guard.NotNull(info);

        var n = info.GetInt64(nameof(numerator));
        var d = info.GetInt64(nameof(denominator));

        if (n != default || d != default)
        {
            var data = new Data(n, d).Guard().Simplify();
            numerator = data.numerator;
            denominator = data.denominator;
        }
    }

    /// <summary>Adds the underlying property of the fraction to the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
        Guard.NotNull(info);
        info.AddValue(nameof(numerator), numerator);
        info.AddValue(nameof(denominator), denominator);
    }
#endif

    /// <summary>Gets an XML string representation of the fraction.</summary>
    [Pure]
    private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Gets an JSON representation of the fraction.</summary>
    [Pure]
    public string ToJson() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Deserializes the fraction from a JSON number.</summary>
    /// <param name = "json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized fraction.
    /// </returns>
    [Pure]
    public static Fraction FromJson(double json) => Cast(json);

    /// <summary>Deserializes the fraction from a JSON number.</summary>
    /// <param name = "json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized fraction.
    /// </returns>
    [Pure]
    public static Fraction FromJson(long json) => New(json, 1);

    /// <summary>Casts the fraction to a <see cref="decimal"/>.</summary>
    [Pure]
    private decimal ToDecimal() => IsZero() ? decimal.Zero : numerator / (decimal)denominator;

    /// <summary>Casts the fraction to a <see cref="double"/>.</summary>
    [Pure]
    private double ToDouble() => IsZero() ? 0d : numerator / (double)denominator;

    /// <summary>Casts a <see cref="decimal"/> to a fraction.</summary>
    [Pure]
    private static Fraction Cast(decimal number)
        => number < MinValue.numerator || number > MaxValue.numerator
        ? throw new OverflowException(QowaivMessages.OverflowException_Fraction)
        : Create(number, MinimumError);

    /// <summary>Casts a <see cref="double"/> to a fraction.</summary>
    [Pure]
    private static Fraction Cast(double number)
        => number < MinValue.numerator || number > MaxValue.numerator
        ? throw new OverflowException(QowaivMessages.OverflowException_Fraction)
        : Create((decimal)number, MinimumError);

    /// <summary>Casts a <see cref="decimal"/> to a <see cref="Fraction"/>.</summary>
    public static explicit operator Fraction(decimal number) => Cast(number);

    /// <summary>Casts a <see cref="decimal"/> to a <see cref="Fraction"/>.</summary>
    public static explicit operator decimal(Fraction fraction) => fraction.ToDecimal();

    /// <summary>Casts a <see cref="Percentage"/> to a <see cref="Fraction"/>.</summary>
    public static explicit operator Fraction(Percentage number) => Cast((decimal)number);

    /// <summary>Casts a <see cref="Percentage"/> to a <see cref="Fraction"/>.</summary>
    public static explicit operator Percentage(Fraction fraction) => Percentage.Create(fraction.ToDecimal());

    /// <summary>Casts a <see cref="double"/> to a <see cref="Fraction"/>.</summary>
    public static explicit operator Fraction(double number) => Cast(number);

    /// <summary>Casts a <see cref="double"/> to a <see cref="Fraction"/>.</summary>
    public static explicit operator double(Fraction fraction) => fraction.ToDouble();

    /// <summary>Casts a <see cref="long"/> to a <see cref="Fraction"/>.</summary>
    public static explicit operator Fraction(long number) => Create(number);

    /// <summary>Casts a <see cref="long"/> to a <see cref="Fraction"/>.</summary>
    public static explicit operator long(Fraction fraction) => fraction.Whole;

    /// <summary>Converts the <see cref = "string "/> to <see cref = "Fraction"/>.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name = "s">
    /// A string containing the fraction to convert.
    /// </param>
    /// <param name = "provider">
    /// The specified format provider.
    /// </param>
    /// <param name = "result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out Fraction result)
    {
        if (s is { Length: > 0 } && FractionParser.Parse(s, provider) is { } fraction)
        {
            result = fraction;
            return true;
        }
        else
        {
            result = Zero;
            return false;
        }
    }

    /// <summary>Creates a fraction based on a <see cref="decimal"/>.</summary>
    /// <param name="number">
    /// The decimal value to represent as a fraction.
    /// </param>
    [Pure]
    public static Fraction Create(decimal number) => Create(number, MinimumError);

    /// <summary>Creates a fraction based on a <see cref="double"/>.</summary>
    /// <param name="number">
    /// The double value to represent as a fraction.
    /// </param>
    [Pure]
    public static Fraction Create(double number)
        => number < (double)decimal.MinValue || number > (double)decimal.MaxValue
        ? throw new ArgumentOutOfRangeException(nameof(number), QowaivMessages.OverflowException_Fraction)
        : Create((decimal)number, MinimumError);

    /// <summary>Creates a fraction based on a <see cref="long"/>.</summary>
    /// <param name="number">
    /// The long value to represent as a fraction.
    /// </param>
    [Pure]
    public static Fraction Create(long number) => number == 0 ? Zero : New(number, 1);

    /// <summary>Initializes a new instance of the <see cref="Fraction"/> class.</summary>
    /// <exception cref="OverflowException">
    /// If the numerator is <see cref="long.MinValue"/>.
    /// </exception>
    /// <remarks>
    /// This pseudo constructor differs from the public constructor that it
    /// does not reduce any value, nor checks the denominator.
    /// </remarks>
    [Pure]
    private static Fraction New(long n, long d)
        => n == long.MinValue
        ? throw new OverflowException(QowaivMessages.OverflowException_Fraction)
        : new(new Data(numerator: n, denominator: d));

    /// <summary>Reduce the numbers based on the greatest common divisor.</summary>
    private static void Reduce(ref long a, ref long b)
    {
        var gcd = Gcd(a, b);
        a /= gcd;
        b /= gcd;
    }

    /// <summary>Gets the Greatest Common Divisor.</summary>
    /// <remarks>
    /// See: https://en.wikipedia.org/wiki/Greatest_common_divisor.
    /// </remarks>
    [Pure]
    private static long Gcd(long a, long b)
    {
        long even = 1;

        // while both are even.
        while ((a & 1) == 0 && (b & 1) == 0)
        {
            a >>= 1;
            b >>= 1;
            even <<= 1;
        }
        while (b != 0)
        {
            (a, b) = (b, a % b);
        }
        return a * even;
    }

    /// <remarks>
    /// to pass the two components around during creation.
    /// </remarks>
    private ref struct Data(long numerator, long denominator)
    {
        public long numerator = numerator;
        public long denominator = denominator;

        [FluentSyntax]
        public readonly Data Guard()
        {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one
            if (numerator == long.MinValue)
            {
                throw new ArgumentOutOfRangeException(nameof(numerator), QowaivMessages.OverflowException_Fraction);
            }
            if (denominator == 0 || denominator == long.MinValue)
            {
                throw new ArgumentOutOfRangeException(nameof(denominator), QowaivMessages.OverflowException_Fraction);
            }
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one
            return this;
        }

        /// <summary>Reduce the numbers based on the greatest common divisor.</summary>
        [FluentSyntax]
        public Data Simplify()
        {
            var negative = numerator < 0 ^ denominator < 0;
            numerator = Math.Abs(numerator);
            denominator = Math.Abs(denominator);

            Reduce(ref numerator, ref denominator);

            if (negative)
            {
                numerator = -numerator;
            }
            return this;
        }
    }
}
