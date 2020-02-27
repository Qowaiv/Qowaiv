using System;

namespace Qowaiv.Mathematics
{
    /// <summary><see cref="Math"/> extensions.</summary>
    internal static class MathExtensions
    {
        /// <summary>Gets the absolute value of the  <paramref name="number"/>.</summary>
        public static long Abs(this long number) => Math.Abs(number);

        /// <summary>Gets the absolute value of the <paramref name="number"/>.</summary>
        public static decimal Abs(this decimal number) => Math.Abs(number);

        /// <summary>Gets the sign of the <paramref name="number"/>.</summary>
        public static int Sign(this long number) => Math.Sign(number);

        /// <summary>Gets the sign of the <paramref name="number"/>.</summary>
        public static int Sign(this decimal number) => Math.Sign(number);
    }
}
