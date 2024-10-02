using Qowaiv.Mathematics;

namespace Qowaiv;

public readonly partial struct Percentage
{
    internal enum Position
    {
        None,
        Before,
        After,
        Contains,
    }

    internal enum Symbol
    {
        None = 0,
        Percent,
        PerMille,
        PerTenThousand,
    }

    internal readonly struct FormatInfo(string format, NumberFormatInfo provider, Symbol symbol, Position position)
    {
        public readonly string Format = format;

        public readonly NumberFormatInfo Provider = provider;

        public readonly Symbol Symbol = symbol;

        public readonly Position Position = position;

        public int ScaleShift => Symbol switch
        {
            Symbol.PerMille => -3,
            Symbol.PerTenThousand => -4,
            _ => -2,
        };

        private static readonly string[] Befores = ["fr-FR", "fa-IR"];

        [Pure]
        public string ToString(decimal value)
        {
            var sb = new StringBuilder();

            if (Position == Position.Before)
            {
                sb.Append(ToString(Symbol, Provider));
            }

            sb.Append(DecimalMath.ChangeScale(value, -ScaleShift).ToString(Format, Provider));

            if (Position == Position.After)
            {
                sb.Append(ToString(Symbol, Provider));
            }
            return sb.ToString();
        }

        [Pure]
        private static string ToString(Symbol symbol, NumberFormatInfo numberFormat)
            => symbol switch
            {
                Symbol.Percent => numberFormat.PercentSymbol,
                Symbol.PerMille => numberFormat.PerMilleSymbol,
                Symbol.PerTenThousand => PerTenThousandSymbol,
                _ => string.Empty,
            };

        [Pure]
#pragma warning disable S3218 // Inner class members should not shadow outer class "static" or type members
        // This is the only proper name for this function.
        public static bool TryParse(string? format, IFormatProvider? formatProvider, out FormatInfo info)
#pragma warning restore S3218 // Inner class members should not shadow outer class "static" or type members
        {
            format = WithDefault(format, formatProvider as CultureInfo);

            var pos = Position.None;
            var sym = Symbol.None;
            var prov = NumberFormat(formatProvider);

            Scan(ref format, ref pos, ref sym, prov, Symbol.Percent);
            Scan(ref format, ref pos, ref sym, prov, Symbol.PerMille);
            Scan(ref format, ref pos, ref sym, prov, Symbol.PerTenThousand);

            if (pos != Position.Contains)
            {
                info = new(format, prov, sym, pos);
                return true;
            }
            else
            {
                info = default;
                return false;
            }
        }

        [Pure]
        private static string WithDefault(string? str, CultureInfo? culture)
        {
            var before = Befores.Contains(culture?.Name);
            return str switch
            {
                null or "" => before ? PercentSymbol + Percentage.DefaultFormat : Percentage.DefaultFormat + PercentSymbol,
                "PM" => before ? PerMilleSymbol + Percentage.DefaultFormat : Percentage.DefaultFormat + PerMilleSymbol,
                "PT" => before ? PerTenThousandSymbol + Percentage.DefaultFormat : Percentage.DefaultFormat + PerTenThousandSymbol,
                _ => str,
            };
        }

        [Pure]
        private static NumberFormatInfo NumberFormat(IFormatProvider? formatProvider)
        {
            var info = NumberFormatInfo.GetInstance(formatProvider ?? CultureInfo.CurrentCulture);
            info = (NumberFormatInfo)info.Clone();
            info.NumberDecimalDigits = info.PercentDecimalDigits;
            info.NumberDecimalSeparator = info.PercentDecimalSeparator;
            info.NumberGroupSeparator = info.PercentGroupSeparator;
            info.NumberGroupSizes = info.PercentGroupSizes;
            return info;
        }

        [Impure]
        private static void Scan(ref string format, ref Position position, ref Symbol symbol, NumberFormatInfo provider, Symbol match)
        {
            var str = ToString(match, provider);
            if (position == Position.None)
            {
                if (format.StartsWith(str))
                {
                    position = Position.Before;
                    format = format[str.Length..];
                    symbol = match;
                }
                else if (format.EndsWith(str))
                {
                    position = Position.After;
                    format = format[..^str.Length];
                    symbol = match;
                }
            }
            if (format.Contains(str))
            {
                position = Position.Contains;
            }
        }
    }
}
