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

    internal readonly struct FormatInfo
    {
        private static readonly string[] Befores = ["fr-FR", "fa-IR"];

        public FormatInfo(string format, NumberFormatInfo provider, Symbol symbol, Position position)
        {
            Format = format;
            Provider = provider;
            Symbol = symbol;
            Position = position;
        }

        public string Format { get; }

        public NumberFormatInfo Provider { get; }

        public Symbol Symbol { get; }

        public Position Position { get; }

        public decimal Factor => Symbol switch
        {
            Symbol.PerMille => 0.001m,
            Symbol.PerTenThousand => 0.0001m,
            _ => 0.01m,
        };

        [Pure]
        public string ToString(decimal value)
        {
            var sb = new StringBuilder();

            if (Position == Position.Before)
            {
                sb.Append(ToString(Symbol, Provider));
            }

            sb.Append((value / Factor).ToString(Format, Provider));

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

            var position = Position.None;
            var symbol = Symbol.None;
            var provider = NumberFormat(formatProvider);

            Scan(ref format, ref position, ref symbol, provider, Symbol.Percent);
            Scan(ref format, ref position, ref symbol, provider, Symbol.PerMille);
            Scan(ref format, ref position, ref symbol, provider, Symbol.PerTenThousand);

            if (position != Position.Contains)
            {
                info = new(format, provider, symbol, position);
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
