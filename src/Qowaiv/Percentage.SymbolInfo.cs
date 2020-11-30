using Qowaiv.Text;
using System;
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

        internal readonly struct SymbolInfo : IEquatable<SymbolInfo>
        {
            private static SymbolInfo Invalid => new SymbolInfo(SymbolPosition.Invalid, default);

            private SymbolInfo(SymbolPosition symbol, CharBuffer buffer)
            {
                Symbol = symbol;
                Buffer = buffer;
            }

            public SymbolPosition Symbol { get;}
            public CharBuffer Buffer { get;  }
            public override string ToString() => Buffer;

            public bool Equals(SymbolInfo other) => base.Equals(other);

            private static bool NotNone(SymbolPosition symbol) => symbol != SymbolPosition.None;

            public static SymbolInfo Resolve(CharBuffer buffer, NumberFormatInfo numberInfo)
            {
                var symbol = SymbolPosition.None;

                if (buffer.StartsWith(numberInfo.PercentSymbol))
                {
                    buffer.RemoveFromStart(numberInfo.PercentSymbol.Length);
                    symbol = SymbolPosition.PercentBefore;
                }
                else if (buffer.StartsWith(numberInfo.PerMilleSymbol))
                {
                    buffer.RemoveFromStart(numberInfo.PerMilleSymbol.Length);
                    symbol = SymbolPosition.PerMilleBefore;
                }
                else if (buffer.StartsWith(PercentSymbol))
                {
                    buffer.RemoveFromStart(1);
                    symbol = SymbolPosition.PercentBefore;
                }
                else if (buffer.StartsWith(PerMilleSymbol))
                {
                    buffer.RemoveFromStart(1);
                    symbol = SymbolPosition.PerMilleBefore;
                }
                else if (buffer.StartsWith(PerTenThousandSymbol))
                {
                    buffer.RemoveFromStart(1);
                    symbol = SymbolPosition.PerTenThousandBefore;
                }

                if (buffer.EndsWith(numberInfo.PercentSymbol))
                {
                    if (NotNone(symbol)) { return Invalid; }
                    buffer.RemoveFromEnd(numberInfo.PercentSymbol.Length);
                    symbol = SymbolPosition.PercentAfter;
                }
                else if (buffer.EndsWith(numberInfo.PerMilleSymbol))
                {
                    if (NotNone(symbol)) { return Invalid; }
                    buffer.RemoveFromEnd(numberInfo.PerMilleSymbol.Length);
                    symbol = SymbolPosition.PerMilleAfter;
                }
                else if (buffer.EndsWith(PercentSymbol))
                {
                    if (NotNone(symbol)) { return Invalid; }
                    buffer.RemoveFromEnd(1);
                    symbol = SymbolPosition.PercentAfter;
                }
                else if (buffer.EndsWith(PerMilleSymbol))
                {
                    if (NotNone(symbol)) { return Invalid; }
                    buffer.RemoveFromEnd(1);
                    symbol = SymbolPosition.PerMilleAfter;
                }
                else if (buffer.EndsWith(PerTenThousandSymbol))
                {
                    if (NotNone(symbol)) { return Invalid; }
                    buffer.RemoveFromEnd(1);
                    symbol = SymbolPosition.PerTenThousandAfter;
                }

                return buffer.Contains(PercentSymbol) ||
                    buffer.Contains(PerMilleSymbol) ||
                    buffer.Contains(PerTenThousandSymbol) ||
                    buffer.Contains(numberInfo.PercentSymbol) ||
                    buffer.Contains(numberInfo.PerMilleSymbol)
                    ? Invalid
                    : new SymbolInfo(symbol, buffer);
            }
        }
    }
}
