namespace Qowaiv;

public partial struct Percentage
{
    internal enum SymbolPosition
    {
        None = 0,
        PercentBefore,
        PercentAfter,
        PerMilleBefore,
        PerMilleAfter,
        PerTenThousandBefore,
        PerTenThousandAfter,
        Invalid,
    }

    internal readonly struct SymbolInfo : IEquatable<SymbolInfo>
    {
        private static SymbolInfo Invalid => new(SymbolPosition.Invalid, CharBuffer.Empty(0));

        private SymbolInfo(SymbolPosition symbol, CharBuffer buffer)
        {
            Symbol = symbol;
            Buffer = buffer;
        }

        public SymbolPosition Symbol { get; }
        public CharBuffer Buffer { get; }

        [Pure]
        public override string ToString() => Buffer;

        [Pure]
        public override bool Equals(object? obj) => obj is SymbolInfo other && Equals(other);

        [Pure]
        public bool Equals(SymbolInfo other) => base.Equals(other);

        [Pure]
        public override int GetHashCode() => Hash.NotSupportedBy<SymbolInfo>();

        [FluentSyntax]
        public static SymbolInfo Get(CharBuffer buffer, NumberFormatInfo numberInfo)
        {
            var start = StartsWith(buffer, numberInfo);
            var end = EndsWith(buffer, numberInfo);

            var symbol = start == default ? end : start;
            return (NotNone(start) && NotNone(end)) || Contains(buffer, numberInfo)
                ? Invalid
                : new SymbolInfo(symbol, buffer);
        }

        [Pure]
        private static bool NotNone(SymbolPosition symbol) => symbol != SymbolPosition.None;

        [FluentSyntax]
        private static SymbolPosition StartsWith(CharBuffer buffer, NumberFormatInfo numberInfo)
        {
            if (buffer.StartsWith(numberInfo.PercentSymbol))
            {
                buffer.RemoveFromStart(numberInfo.PercentSymbol.Length);
                return SymbolPosition.PercentBefore;
            }
            else if (buffer.StartsWith(numberInfo.PerMilleSymbol))
            {
                buffer.RemoveFromStart(numberInfo.PerMilleSymbol.Length);
                return SymbolPosition.PerMilleBefore;
            }
            else if (buffer.StartsWith(PercentSymbol))
            {
                buffer.RemoveFromStart(1);
                return SymbolPosition.PercentBefore;
            }
            else if (buffer.StartsWith(PerMilleSymbol))
            {
                buffer.RemoveFromStart(1);
                return SymbolPosition.PerMilleBefore;
            }
            else if (buffer.StartsWith(PerTenThousandSymbol))
            {
                buffer.RemoveFromStart(1);
                return SymbolPosition.PerTenThousandBefore;
            }
            return default;
        }

        [FluentSyntax]
        private static SymbolPosition EndsWith(CharBuffer buffer, NumberFormatInfo numberInfo)
        {
            if (buffer.EndsWith(numberInfo.PercentSymbol))
            {
                buffer.RemoveFromEnd(numberInfo.PercentSymbol.Length);
                return SymbolPosition.PercentAfter;
            }
            else if (buffer.EndsWith(numberInfo.PerMilleSymbol))
            {
                buffer.RemoveFromEnd(numberInfo.PerMilleSymbol.Length);
                return SymbolPosition.PerMilleAfter;
            }
            else if (buffer.EndsWith(PercentSymbol))
            {
                buffer.RemoveFromEnd(1);
                return SymbolPosition.PercentAfter;
            }
            else if (buffer.EndsWith(PerMilleSymbol))
            {
                buffer.RemoveFromEnd(1);
                return SymbolPosition.PerMilleAfter;
            }
            else if (buffer.EndsWith(PerTenThousandSymbol))
            {
                buffer.RemoveFromEnd(1);
                return SymbolPosition.PerTenThousandAfter;
            }
            return default;
        }

        [Pure]
        private static bool Contains(CharBuffer buffer, NumberFormatInfo numberInfo)
#pragma warning disable S1067 // Expression is not that complex
            => buffer.Contains(PercentSymbol)
            || buffer.Contains(PerMilleSymbol)
            || buffer.Contains(PerTenThousandSymbol)
            || buffer.Contains(numberInfo.PercentSymbol)
            || buffer.Contains(numberInfo.PerMilleSymbol);
    }
}
