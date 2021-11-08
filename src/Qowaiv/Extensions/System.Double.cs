using Qowaiv;
using System.Diagnostics.Contracts;

namespace System
{
    /// <summary>Extensions on <see cref="double"/>.</summary>
    public static class QowaivDoubleExtensions
    {
        /// <summary>Adds the specified percentage to the <see cref="double"/>.</summary>
        /// <param name="d">
        /// The value to add a percentage to.
        /// </param>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        [Pure]
        public static double Add(this double d, Percentage p) => d + d.Multiply(p);

        /// <summary>Subtracts the specified percentage to the <see cref="double"/>.</summary>
        /// <param name="d">
        /// The value to Subtract a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage to Subtract.
        /// </param>
        [Pure]
        public static double Subtract(this double d, Percentage p) => d - d.Multiply(p);

        /// <summary>Gets the specified percentage of the <see cref="double"/>.</summary>
        /// <param name="d">
        /// The value to get a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage.
        /// </param>
        [Pure]
        public static double Multiply(this double d, Percentage p) => (double)((decimal)d).Multiply(p);

        /// <summary>Divides the <see cref="double"/> by the specified percentage.</summary>
        /// <param name="d">
        /// The value to divide.
        /// </param>
        /// <param name="p">
        /// The percentage to divide to.
        /// </param>
        [Pure]
        public static double Divide(this double d, Percentage p) => (double)((decimal)d).Divide(p);
    }
}
