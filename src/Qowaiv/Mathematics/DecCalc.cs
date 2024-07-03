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

    private uint flags => negative
        ? (uint)((byte)scale << 16) | SignMask
        : (uint)((byte)scale << 16);

    /// <summary>
    /// Removes its trailing zero's.
    /// </summary>
    public void Trim()
    {
        var modulo = 10U;
        var factor = 1U;

        while (lo % modulo == 0 && factor < DecimalMath.Powers10[DecimalMath.MaxInt32Scale])
        {
            factor = modulo;
            modulo *= 10;
            scale--;
        }
        if (factor != 1)
        {
            Divide(factor);
        }
    }

    /// <summary>Multiplies the decimal with an <see cref="uint"/> factor.</summary>
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

    /// <summary>Divides the decimal with an <see cref="uint"/> divisor.</summary>
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

    /// <summary>Adds an <see cref="uint"/> to the decimal.</summary>
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

    /// <summary>Maps to a <see cref="decimal"/> with Big Endian architecture.</summary>
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

    /// <summary>Maps to a <see cref="decimal"/> with Little Endian architecture.</summary>
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
