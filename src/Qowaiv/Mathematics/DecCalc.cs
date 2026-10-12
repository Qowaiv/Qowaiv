using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Qowaiv.Mathematics;

[DebuggerDisplay("{Value()}, Scale = {scale}, [{hi}, {mi}, {lo}]")]
[StructLayout(LayoutKind.Auto)]
internal ref struct DecCalc
{
    private const uint SignMask = 0x_8000_0000;

    public uint lo;
    public uint mi;
    public uint hi;
    public int scale;
    public bool negative;

    private readonly uint flags => negative
        ? (uint)((byte)scale << 16) | SignMask
        : (uint)((byte)scale << 16);

    /// <summary>Multiplies the decimal with an <see cref="uint" /> factor.</summary>
    public void Multiply(uint factor)
    {
        unchecked
        {
            ulong f = factor;
            ulong n = lo * f;
            lo = (uint)n;
            n = (n >> 32) + (mi * f);
            mi = (uint)n;
            n = (n >> 32) + (hi * f);
            hi = (uint)n;

            if ((n >> 32) != 0)
            {
                throw new OverflowException(QowaivMessages.OverflowException_DecimalRound);
            }
        }
    }

    /// <summary>Divides the decimal with an <see cref="uint" /> divisor.</summary>
    [Impure]
    public uint Divide(uint divisor)
    {
        unchecked
        {
            ulong remainder = 0;
            ulong n;

            if (hi != 0)
            {
                (hi, remainder) = Math.DivRem(hi, divisor);
            }

            n = mi | (remainder << 32);

            if (n != 0)
            {
                (n, remainder) = Math.DivRem(n, divisor);
                mi = (uint)n;
            }

            n = lo | (remainder << 32);

            if (n != 0)
            {
                (n, remainder) = Math.DivRem(n, divisor);
                lo = (uint)n;
            }
            return (uint)remainder;
        }
    }

    /// <summary>Adds an <see cref="uint" /> to the decimal.</summary>
    public void Add(uint addition)
    {
        unchecked
        {
            ulong n = lo + addition;
            lo = (uint)n;
            n = (n >> 32) + mi;
            mi = (uint)n;
            n = (n >> 32) + hi;
            hi = (uint)n;

            if ((n >> 32) != 0)
            {
                throw new OverflowException(QowaivMessages.OverflowException_DecimalRound);
            }
        }
    }

    /// <summary>Removes its trailing zero's.</summary>
    public void RemoveTrailingZeros()
    {
        // 2^32 is even, so an odd low part means the value is odd.
        if ((lo & 1) is not 0) return;

        // Never trim beyond a scale of zero: that would only have to be undone.
        var max = scale < DecimalMath.MaxInt32Scale ? scale : DecimalMath.MaxInt32Scale;
        var removed = 0;

        // Fast path: fits in 32 bits.
        if ((hi | mi) is 0)
        {
            while (removed < max && Math.DivRem(lo, 10U) is { Remainder: 0, Quotient: var quotient })
            {
                lo = quotient;
                removed++;
            }
            scale -= removed;
            return;
        }

        // Binary decomposition of the number of zeros (max 9): 8, 4, 2, 1.
        for (var step = 8; step > 0; step >>= 1)
        {
            if (removed + step <= max)
            {
                // Divide a copy, so a non-zero remainder does not alter this instance.
                var copy = this;
                if (copy.Divide(DecimalMath.Powers10[step]) is 0)
                {
                    this = copy;
                    removed += step;
                }
            }
        }
        scale -= removed;
    }

    [Pure]
    public decimal Value()
    {
        if (BitConverter.IsLittleEndian)
        {
            var lit = new EndianLittle()
            {
                uflags = flags,
                ulo = lo,
                umid = mi,
                uhi = hi,
            };
            return Unsafe.As<EndianLittle, decimal>(ref lit);
        }
        else
        {
            var big = new EndianBig()
            {
                uflags = flags,
                ulo = lo,
                umid = mi,
                uhi = hi,
            };
            return Unsafe.As<EndianBig, decimal>(ref big);
        }
    }

    [Pure]
    public static DecCalc New(decimal d)
        => BitConverter.IsLittleEndian
        ? Unsafe.As<decimal, EndianLittle>(ref d).ToCalc()
        : Unsafe.As<decimal, EndianBig>(ref d).ToCalc();

    /// <summary>Maps to a <see cref="decimal" /> with Big Endian architecture.</summary>
    [StructLayout(LayoutKind.Explicit)]
    private struct EndianBig
    {
        [FieldOffset(0)]
        public uint uflags;
        [FieldOffset(4)]
        public uint uhi;
        [FieldOffset(8)]
        public uint umid;
        [FieldOffset(12)]
        public uint ulo;

        [Pure]
        public DecCalc ToCalc() => new()
        {
            negative = (uflags & SignMask) != 0,
            scale = (byte)(uflags >> 16),
            lo = ulo,
            mi = umid,
            hi = uhi,
        };
    }

#if NETSTANDARD2_0
    private static class Math
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static (uint Quotient, uint Remainder) DivRem(uint left, uint right)
        {
            var quotient = left / right;
            return (quotient, left - (quotient * right));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static (ulong Quotient, ulong Remainder) DivRem(ulong left, ulong right)
        {
            var quotient = left / right;
            return (quotient, left - (quotient * right));
        }
    }
#endif

    /// <summary>Maps to a <see cref="decimal" /> with Little Endian architecture.</summary>
    [StructLayout(LayoutKind.Explicit)]
    private struct EndianLittle
    {
        [FieldOffset(0)]
        public uint uflags;
        [FieldOffset(4)]
        public uint uhi;
        [FieldOffset(8)]
        public uint ulo;
        [FieldOffset(12)]
        public uint umid;

        [Pure]
        public DecCalc ToCalc() => new()
        {
            negative = (uflags & SignMask) != 0,
            scale = (byte)(uflags >> 16),
            lo = ulo,
            mi = umid,
            hi = uhi,
        };
    }
}
