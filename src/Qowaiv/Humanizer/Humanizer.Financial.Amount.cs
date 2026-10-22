using Qowaiv.Financial;

namespace Qowaiv.Humanizer;

/// <summary>Extensions to create <see cref="Financial.Amount" />s, inspired by Humanizer.NET.</summary>
public static class NumberToAmountExtensions
{
    extension(decimal number)
    {
        /// <summary>Converts the <see cref="decimal" /> to a <see cref="Financial.Amount" />.</summary>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Amount Amount() => Financial.Amount.Create(number);
    }

    extension(double number)
    {
        /// <summary>Converts the <see cref="double" /> to a <see cref="Financial.Amount" />.</summary>
        [Pure]
        [Obsolete("Use decimals instead.")]
        public Amount Amount() => Financial.Amount.Create(number);
    }

    extension(int number)
    {
        /// <summary>Converts the <see cref="double" /> to a <see cref="Financial.Amount" />.</summary>
        [Pure]
        public Amount Amount() => Financial.Amount.Create((decimal)number);
    }
}
