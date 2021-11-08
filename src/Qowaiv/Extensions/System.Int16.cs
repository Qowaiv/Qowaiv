using Qowaiv;
using System.Diagnostics.Contracts;

namespace System
{
    /// <summary>Extensions on <see cref="short"/>.</summary>
    public static class Qowaivshort16Extensions
    {
        /// <summary>Adds the specified percentage to the <see cref="short"/>.</summary>
        /// <param name="num">
        /// The value to add a percentage to.
        /// </param>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        [Pure]
        public static short Add(this short num, Percentage p) => (short)(num + num.Multiply(p));

        /// <summary>Subtracts the specified percentage to the <see cref="short"/>.</summary>
        /// <param name="num">
        /// The value to Subtract a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage to Subtract.
        /// </param>
        [Pure]
        public static short Subtract(this short num, Percentage p) => (short)(num - num.Multiply(p));

        /// <summary>Gets the specified percentage of the <see cref="short"/>.</summary>
        /// <param name="num">
        /// The value to get a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage.
        /// </param>
        [Pure]
        public static short Multiply(this short num, Percentage p) => (short)((decimal)num).Multiply(p);

        /// <summary>Divides the <see cref="short"/> by the specified percentage.</summary>
        /// <param name="num">
        /// The value to divide.
        /// </param>
        /// <param name="p">
        /// The percentage to divide to.
        /// </param>
        [Pure]
        public static short Divide(this short num, Percentage p) => (short)((decimal)num).Divide(p);
    }
}
