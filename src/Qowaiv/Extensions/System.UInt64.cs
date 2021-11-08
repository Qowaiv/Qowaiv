using Qowaiv;
using System.Diagnostics.Contracts;

namespace System
{
    /// <summary>Extensions on <see cref="ulong"/>.</summary>
    [CLSCompliant(false)]
    public static class Qowaivulong64Extensions
    {
        /// <summary>Adds the specified percentage to the <see cref="ulong"/>.</summary>
        /// <param name="num">
        /// The value to add a percentage to.
        /// </param>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        [Pure]
        public static ulong Add(this ulong num, Percentage p) => num + num.Multiply(p);

        /// <summary>Subtracts the specified percentage to the <see cref="ulong"/>.</summary>
        /// <param name="num">
        /// The value to Subtract a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage to Subtract.
        /// </param>
        [Pure]
        public static ulong Subtract(this ulong num, Percentage p) => num - num.Multiply(p);

        /// <summary>Gets the specified percentage of the <see cref="ulong"/>.</summary>
        /// <param name="num">
        /// The value to get a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage.
        /// </param>
        [Pure]
        public static ulong Multiply(this ulong num, Percentage p) => (ulong)((decimal)num).Multiply(p);

        /// <summary>Divides the <see cref="ulong"/> by the specified percentage.</summary>
        /// <param name="num">
        /// The value to divide.
        /// </param>
        /// <param name="p">
        /// The percentage to divide to.
        /// </param>
        [Pure]
        public static ulong Divide(this ulong num, Percentage p) => (ulong)((decimal)num).Divide(p);
    }
}
