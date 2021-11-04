using System;
using System.Diagnostics.Contracts;

namespace Qowaiv
{
    /// <summary>Contains extensions for Percentage.</summary>
    public static class PercentageExtensions
    {
        #region Add extensions

        /// <summary>Adds the specified percentage to the Decimal.</summary>
        /// <param name="d">
        /// The value to add a percentage to.
        /// </param>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        [Pure]
        public static Decimal Add(this Decimal d, Percentage p) => d + d.Multiply(p);

        /// <summary>Adds the specified percentage to the Double.</summary>
        /// <param name="d">
        /// The value to add a percentage to.
        /// </param>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        [Pure]
        public static Double Add(this Double d, Percentage p) => d + d.Multiply(p);

        /// <summary>Adds the specified percentage to the Single.</summary>
        /// <param name="d">
        /// The value to add a percentage to.
        /// </param>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        [Pure]
        public static Single Add(this Single d, Percentage p) => d + d.Multiply(p);


        /// <summary>Adds the specified percentage to the Int16.</summary>
        /// <param name="d">
        /// The value to add a percentage to.
        /// </param>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        [Pure]
        public static Int64 Add(this Int64 d, Percentage p) => d + d.Multiply(p);

        /// <summary>Adds the specified percentage to the Int32.</summary>
        /// <param name="d">
        /// The value to add a percentage to.
        /// </param>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        [Pure]
        public static Int32 Add(this Int32 d, Percentage p) => d + d.Multiply(p);

        /// <summary>Adds the specified percentage to the Int64.</summary>
        /// <param name="d">
        /// The value to add a percentage to.
        /// </param>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        [Pure]
        public static Int16 Add(this Int16 d, Percentage p) => (Int16)(d + d.Multiply(p));

        /// <summary>Adds the specified percentage to the UInt16.</summary>
        /// <param name="d">
        /// The value to add a percentage to.
        /// </param>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        [CLSCompliant(false)]
        [Pure]
        public static UInt64 Add(this UInt64 d, Percentage p) => d + d.Multiply(p);

        /// <summary>Adds the specified percentage to the UInt32.</summary>
        /// <param name="d">
        /// The value to add a percentage to.
        /// </param>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        [CLSCompliant(false)]
        [Pure]
        public static UInt32 Add(this UInt32 d, Percentage p) => d + d.Multiply(p);

        /// <summary>Adds the specified percentage to the UInt64.</summary>
        /// <param name="d">
        /// The value to add a percentage to.
        /// </param>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        [CLSCompliant(false)]
        [Pure]
        public static UInt16 Add(this UInt16 d, Percentage p) => (UInt16)(d + d.Multiply(p));

        #endregion

        #region Subtract extensions

        /// <summary>Subtracts the specified percentage to the Decimal.</summary>
        /// <param name="d">
        /// The value to Subtract a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage to Subtract.
        /// </param>
        [Pure]
        public static Decimal Subtract(this Decimal d, Percentage p) => d - d.Multiply(p);

        /// <summary>Subtracts the specified percentage to the Double.</summary>
        /// <param name="d">
        /// The value to Subtract a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage to Subtract.
        /// </param>
        [Pure]
        public static Double Subtract(this Double d, Percentage p) => d - d.Multiply(p);

        /// <summary>Subtracts the specified percentage to the Single.</summary>
        /// <param name="d">
        /// The value to Subtract a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage to Subtract.
        /// </param>
        [Pure]
        public static Single Subtract(this Single d, Percentage p) => d - d.Multiply(p);


        /// <summary>Subtracts the specified percentage to the Int16.</summary>
        /// <param name="d">
        /// The value to Subtract a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage to Subtract.
        /// </param>
        [Pure]
        public static Int64 Subtract(this Int64 d, Percentage p) => d - d.Multiply(p);

        /// <summary>Subtracts the specified percentage to the Int32.</summary>
        /// <param name="d">
        /// The value to Subtract a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage to Subtract.
        /// </param>
        [Pure]
        public static Int32 Subtract(this Int32 d, Percentage p) => d - d.Multiply(p);

        /// <summary>Subtracts the specified percentage to the Int64.</summary>
        /// <param name="d">
        /// The value to Subtract a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage to Subtract.
        /// </param>
        [Pure]
        public static Int16 Subtract(this Int16 d, Percentage p) => (Int16)(d - d.Multiply(p));

        /// <summary>Subtracts the specified percentage to the UInt16.</summary>
        /// <param name="d">
        /// The value to Subtract a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage to Subtract.
        /// </param>
        [CLSCompliant(false)]
        [Pure]
        public static UInt64 Subtract(this UInt64 d, Percentage p) => d - d.Multiply(p);

        /// <summary>Subtracts the specified percentage to the UInt32.</summary>
        /// <param name="d">
        /// The value to Subtract a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage to Subtract.
        /// </param>
        [CLSCompliant(false)]
        [Pure]
        public static UInt32 Subtract(this UInt32 d, Percentage p) => d - d.Multiply(p);

        /// <summary>Subtracts the specified percentage to the UInt64.</summary>
        /// <param name="d">
        /// The value to Subtract a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage to Subtract.
        /// </param>
        [CLSCompliant(false)]
        [Pure]
        public static UInt16 Subtract(this UInt16 d, Percentage p) => (UInt16)(d - d.Multiply(p));

        #endregion

        #region Multiply extensions

        /// <summary>Gets the specified percentage of the Decimal.</summary>
        /// <param name="d">
        /// The value to get a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage.
        /// </param>
        [Pure]
        public static Decimal Multiply(this Decimal d, Percentage p) => d * (Decimal)p;

        /// <summary>Gets the specified percentage of the Double.</summary>
        /// <param name="d">
        /// The value to get a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage.
        /// </param>
        [Pure]
        public static Double Multiply(this Double d, Percentage p) => (Double)((Decimal)d).Multiply(p);

        /// <summary>Gets the specified percentage of the Single.</summary>
        /// <param name="d">
        /// The value to get a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage.
        /// </param>
        [Pure]
        public static Single Multiply(this Single d, Percentage p) => (Single)((Decimal)d).Multiply(p);

        /// <summary>Gets the specified percentage of the Int64.</summary>
        /// <param name="d">
        /// The value to get a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage.
        /// </param>
        [Pure]
        public static Int64 Multiply(this Int64 d, Percentage p) => (Int64)((Decimal)d).Multiply(p);

        /// <summary>Gets the specified percentage of the Int32.</summary>
        /// <param name="d">
        /// The value to get a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage.
        /// </param>
        [Pure]
        public static Int32 Multiply(this Int32 d, Percentage p) => (Int32)((Decimal)d).Multiply(p);

        /// <summary>Gets the specified percentage of the Int16.</summary>
        /// <param name="d">
        /// The value to get a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage.
        /// </param>
        [Pure]
        public static Int16 Multiply(this Int16 d, Percentage p) => (Int16)((Decimal)d).Multiply(p);

        /// <summary>Gets the specified percentage of the UInt64.</summary>
        /// <param name="d">
        /// The value to get a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage.
        /// </param>
        [CLSCompliant(false)]
        [Pure]
        public static UInt64 Multiply(this UInt64 d, Percentage p) => (UInt64)((Decimal)d).Multiply(p);

        /// <summary>Gets the specified percentage of the UInt32.</summary>
        /// <param name="d">
        /// The value to get a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage.
        /// </param>
        [CLSCompliant(false)]
        [Pure]
        public static UInt32 Multiply(this UInt32 d, Percentage p) => (UInt32)((Decimal)d).Multiply(p);

        /// <summary>Gets the specified percentage of the UInt16.</summary>
        /// <param name="d">
        /// The value to get a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage.
        /// </param>
        [CLSCompliant(false)]
        [Pure]
        public static UInt16 Multiply(this UInt16 d, Percentage p) => (UInt16)((Decimal)d).Multiply(p);

        #endregion

        #region Divide extensions

        /// <summary>Divides the Decimal by the specified percentage.</summary>
        /// <param name="d">
        /// The value to divide.
        /// </param>
        /// <param name="p">
        /// The percentage to divide to.
        /// </param>
        [Pure]
        public static Decimal Divide(this Decimal d, Percentage p) => d / (Decimal)p;

        /// <summary>Divides the Double by the specified percentage.</summary>
        /// <param name="d">
        /// The value to divide.
        /// </param>
        /// <param name="p">
        /// The percentage to divide to.
        /// </param>
        [Pure]
        public static Double Divide(this Double d, Percentage p) => (Double)((Decimal)d).Divide(p);

        /// <summary>Divides the Single by the specified percentage.</summary>
        /// <param name="d">
        /// The value to divide.
        /// </param>
        /// <param name="p">
        /// The percentage to divide to.
        /// </param>
        [Pure]
        public static Single Divide(this Single d, Percentage p) => (Single)((Decimal)d).Divide(p);


        /// <summary>Divides the Int64 by the specified percentage.</summary>
        /// <param name="d">
        /// The value to divide.
        /// </param>
        /// <param name="p">
        /// The percentage to divide to.
        /// </param>
        [Pure]
        public static Int64 Divide(this Int64 d, Percentage p) => (Int64)((Decimal)d).Divide(p);

        /// <summary>Divides the Int32 by the specified percentage.</summary>
        /// <param name="d">
        /// The value to divide.
        /// </param>
        /// <param name="p">
        /// The percentage to divide to.
        /// </param>
        [Pure]
        public static Int32 Divide(this Int32 d, Percentage p) => (Int32)((Decimal)d).Divide(p);

        /// <summary>Divides the Int16 by the specified percentage.</summary>
        /// <param name="d">
        /// The value to divide.
        /// </param>
        /// <param name="p">
        /// The percentage to divide to.
        /// </param>
        [Pure]
        public static Int16 Divide(this Int16 d, Percentage p) => (Int16)((Decimal)d).Divide(p);


        /// <summary>Divides the UInt64 by the specified percentage.</summary>
        /// <param name="d">
        /// The value to divide.
        /// </param>
        /// <param name="p">
        /// The percentage to divide to.
        /// </param>
        [CLSCompliant(false)]
        [Pure]
        public static UInt64 Divide(this UInt64 d, Percentage p) => (UInt64)((Decimal)d).Divide(p);

        /// <summary>Divides the UInt32 by the specified percentage.</summary>
        /// <param name="d">
        /// The value to divide.
        /// </param>
        /// <param name="p">
        /// The percentage to divide to.
        /// </param>
        [CLSCompliant(false)]
        [Pure]
        public static UInt32 Divide(this UInt32 d, Percentage p) => (UInt32)((Decimal)d).Divide(p);

        /// <summary>Divides the UInt16 by the specified percentage.</summary>
        /// <param name="d">
        /// The value to divide.
        /// </param>
        /// <param name="p">
        /// The percentage to divide to.
        /// </param>
        [CLSCompliant(false)]
        [Pure]
        public static UInt16 Divide(this UInt16 d, Percentage p) => (UInt16)((Decimal)d).Divide(p);

        #endregion

        #region Percent()

        /// <summary>Interprets the <see cref="int"/> if it was written with a '%' sign.</summary>
        [Pure]
        public static Percentage Percent(this int number) => number * 0.01m;

        /// <summary>Interprets the <see cref="double"/> if it was written with a '%' sign.</summary>
        [Pure]
        public static Percentage Percent(this double number) => number * 0.01;

        /// <summary>Interprets the <see cref="decimal"/> if it was written with a '%' sign.</summary>
        [Pure]
        public static Percentage Percent(this decimal number) => number * 0.01m;

        #endregion
    }
}
