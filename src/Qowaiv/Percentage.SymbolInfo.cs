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
            private int Start { get; set; }
            private int Position { get; set; }
            public SymbolPosition Symbol { get; private set; }
            public CharBuffer Buffer { get; private set; }

            private string Sub() => Buffer.Substring(Start, Position);
            private bool IsStart() => Position == 0;
            private bool IsLast() => Position == Buffer.Length - 1;
            private bool IsInvalid()
                => Symbol != SymbolPosition.None
                || (!IsStart() && !IsLast());
            private void Trim()
            {
                if (IsStart())
                {
                    Buffer.RemoveFromStart(Position);
                    Start = 0;
                    Position = 0;
                }
                else
                {
                    Buffer.RemoveFromEnd(Position - Start);
                    Position = Start;
                }
            }

            public static SymbolInfo Resolve(CharBuffer buffer, NumberFormatInfo numberInfo)
            {
                var info = new SymbolInfo { Buffer = buffer };

                while (!info.IsLast())
                {
                    info.Position++;
                    var sub = info.Sub();
                    if (sub == PercentSymbol || sub == numberInfo.PercentSymbol)
                    {
                        if (info.IsInvalid()) { return Invalid; }
                        else
                        {
                            info.Symbol = info.IsStart() ? SymbolPosition.PercentBefore : SymbolPosition.PercentAfter;
                            info.Trim();
                        }
                    }
                    else if (sub == PerMilleSymbol || sub == numberInfo.PerMilleSymbol)
                    {
                        if (info.IsInvalid()) { return Invalid; }
                        else
                        {
                            info.Symbol = info.IsStart() ? SymbolPosition.PerMilleBefore : SymbolPosition.PerMilleAfter;
                            info.Trim();
                        }
                    }
                    else if (sub == PerTenThousandSymbol)
                    {
                        if (info.IsInvalid()) { return Invalid; }
                        else
                        {
                            info.Symbol = info.IsStart() ? SymbolPosition.PerTenThousandBefore : SymbolPosition.PerTenThousandAfter;
                            info.Trim();
                        }
                    }
                    else { info.Start = info.Position; }
                }

                return info;
            }
        }
    }
}
