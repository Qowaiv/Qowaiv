using Qowaiv.Text;
using System.Globalization;

namespace Qowaiv
{
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

        internal ref struct SymbolInfo
        {
            private static SymbolInfo Invalid => new SymbolInfo { Symbol = SymbolPosition.Invalid };

            public SymbolPosition Symbol { get; private set; }
            public CharBuffer Buffer { get; private set; }
            public override string ToString() => Buffer;

            private static bool IsInvalid(SymbolPosition symbol) => symbol == SymbolPosition.Invalid;

            public static SymbolInfo Resolve(CharBuffer buffer, NumberFormatInfo numberInfo)
            {
                var symbol = SymbolPosition.None;

                if (buffer.StartsWith(PercentSymbol))
                {
                    buffer.RemoveFromStart(1);
                    symbol = SymbolPosition.PercentBefore;
                }
                else if (buffer.StartsWith(numberInfo.PercentSymbol))
                {
                    buffer.RemoveFromStart(numberInfo.PercentSymbol.Length);
                    symbol = SymbolPosition.PercentBefore;
                }
                else if (buffer.StartsWith(PerMilleSymbol))
                {
                    buffer.RemoveFromStart(1);
                    symbol = SymbolPosition.PerMilleBefore;
                }
                else if (buffer.StartsWith(numberInfo.PerMilleSymbol))
                {
                    buffer.RemoveFromStart(numberInfo.PerMilleSymbol.Length);
                    symbol = SymbolPosition.PerMilleBefore;
                }
                else if (buffer.StartsWith(PerTenThousandSymbol))
                {
                    buffer.RemoveFromStart(1);
                    symbol = SymbolPosition.PerTenThousandAfter;
                }

                if (buffer.EndsWith(PercentSymbol))
                {
                    if (IsInvalid(symbol)) { return Invalid; }
                    buffer.RemoveFromEnd(1);
                    symbol = SymbolPosition.PercentAfter;
                }
                else if (buffer.EndsWith(numberInfo.PercentSymbol))
                {
                    if (IsInvalid(symbol)) { return Invalid; }
                    buffer.RemoveFromEnd(numberInfo.PercentSymbol.Length);
                    symbol = SymbolPosition.PercentAfter;
                }
                else if (buffer.EndsWith(PerMilleSymbol))
                {
                    if (IsInvalid(symbol)) { return Invalid; }
                    buffer.RemoveFromEnd(1);
                    symbol = SymbolPosition.PerMilleAfter;
                }
                else if (buffer.EndsWith(numberInfo.PerMilleSymbol))
                {
                    if (IsInvalid(symbol)) { return Invalid; }
                    buffer.RemoveFromEnd(numberInfo.PerMilleSymbol.Length);
                    symbol = SymbolPosition.PerMilleAfter;
                }
                else if (buffer.EndsWith(PerTenThousandSymbol))
                {
                    if (IsInvalid(symbol)) { return Invalid; }
                    buffer.RemoveFromEnd(1);
                    symbol = SymbolPosition.PerTenThousandAfter;
                }

                return buffer.Contains(PercentSymbol) ||
                    buffer.Contains(PerMilleSymbol) ||
                    buffer.Contains(PerTenThousandSymbol) ||
                    buffer.Contains(numberInfo.PercentSymbol) ||
                    buffer.Contains(numberInfo.PerMilleSymbol)
                    ? Invalid
                    : new SymbolInfo { Buffer = buffer, Symbol = symbol };
            }
        }
    }
}
