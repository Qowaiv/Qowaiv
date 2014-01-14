using System;

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
        public static Decimal Add(this Decimal d, Percentage p) { return d + d.Multiply(p); }
        
        /// <summary>Adds the specified percentage to the Double.</summary>
        /// <param name="d">
        /// The value to add a percentage to.
        /// </param>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        public static Double Add(this Double d, Percentage p) { return d + d.Multiply(p); }
        
        /// <summary>Adds the specified percentage to the Single.</summary>
        /// <param name="d">
        /// The value to add a percentage to.
        /// </param>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        public static Single Add(this Single d, Percentage p) { return d + d.Multiply(p); }


        /// <summary>Adds the specified percentage to the Int16.</summary>
        /// <param name="d">
        /// The value to add a percentage to.
        /// </param>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        public static Int64 Add(this Int64 d, Percentage p) { return d + d.Multiply(p); }

        /// <summary>Adds the specified percentage to the Int32.</summary>
        /// <param name="d">
        /// The value to add a percentage to.
        /// </param>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        public static Int32 Add(this Int32 d, Percentage p) { return d + d.Multiply(p); }

        /// <summary>Adds the specified percentage to the Int64.</summary>
        /// <param name="d">
        /// The value to add a percentage to.
        /// </param>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        public static Int16 Add(this Int16 d, Percentage p) { return (Int16)(d + d.Multiply(p)); }

        /// <summary>Adds the specified percentage to the UInt16.</summary>
        /// <param name="d">
        /// The value to add a percentage to.
        /// </param>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        [CLSCompliant(false)]
        public static UInt64 Add(this UInt64 d, Percentage p) { return d + d.Multiply(p); }

        /// <summary>Adds the specified percentage to the UInt32.</summary>
        /// <param name="d">
        /// The value to add a percentage to.
        /// </param>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        [CLSCompliant(false)]
        public static UInt32 Add(this UInt32 d, Percentage p) { return d + d.Multiply(p); }

        /// <summary>Adds the specified percentage to the UInt64.</summary>
        /// <param name="d">
        /// The value to add a percentage to.
        /// </param>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        [CLSCompliant(false)]
        public static UInt16 Add(this UInt16 d, Percentage p) { return (UInt16)(d + d.Multiply(p)); }

        #endregion

        #region Subtract extensions

        /// <summary>Subtracts the specified percentage to the Decimal.</summary>
        /// <param name="d">
        /// The value to Subtract a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage to Subtract.
        /// </param>
        public static Decimal Subtract(this Decimal d, Percentage p) { return d - d.Multiply(p); }

        /// <summary>Subtracts the specified percentage to the Double.</summary>
        /// <param name="d">
        /// The value to Subtract a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage to Subtract.
        /// </param>
        public static Double Subtract(this Double d, Percentage p) { return d - d.Multiply(p); }

        /// <summary>Subtracts the specified percentage to the Single.</summary>
        /// <param name="d">
        /// The value to Subtract a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage to Subtract.
        /// </param>
        public static Single Subtract(this Single d, Percentage p) { return d - d.Multiply(p); }


        /// <summary>Subtracts the specified percentage to the Int16.</summary>
        /// <param name="d">
        /// The value to Subtract a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage to Subtract.
        /// </param>
        public static Int64 Subtract(this Int64 d, Percentage p) { return d - d.Multiply(p); }

        /// <summary>Subtracts the specified percentage to the Int32.</summary>
        /// <param name="d">
        /// The value to Subtract a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage to Subtract.
        /// </param>
        public static Int32 Subtract(this Int32 d, Percentage p) { return d - d.Multiply(p); }

        /// <summary>Subtracts the specified percentage to the Int64.</summary>
        /// <param name="d">
        /// The value to Subtract a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage to Subtract.
        /// </param>
        public static Int16 Subtract(this Int16 d, Percentage p) { return (Int16)(d - d.Multiply(p)); }

        /// <summary>Subtracts the specified percentage to the UInt16.</summary>
        /// <param name="d">
        /// The value to Subtract a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage to Subtract.
        /// </param>
        [CLSCompliant(false)]
        public static UInt64 Subtract(this UInt64 d, Percentage p) { return d - d.Multiply(p); }

        /// <summary>Subtracts the specified percentage to the UInt32.</summary>
        /// <param name="d">
        /// The value to Subtract a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage to Subtract.
        /// </param>
        [CLSCompliant(false)]
        public static UInt32 Subtract(this UInt32 d, Percentage p) { return d - d.Multiply(p); }

        /// <summary>Subtracts the specified percentage to the UInt64.</summary>
        /// <param name="d">
        /// The value to Subtract a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage to Subtract.
        /// </param>
        [CLSCompliant(false)]
        public static UInt16 Subtract(this UInt16 d, Percentage p) { return (UInt16)(d - d.Multiply(p)); }

        #endregion

        #region Multiply extensions

        /// <summary>Gets the specified percentage of the Decimal.</summary>
        /// <param name="d">
        /// The value to get a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage.
        /// </param>
        public static Decimal Multiply(this Decimal d, Percentage p) { return d * (Decimal)p; }

        /// <summary>Gets the specified percentage of the Double.</summary>
        /// <param name="d">
        /// The value to get a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage.
        /// </param>
        public static Double Multiply(this Double d, Percentage p) { return (Double)((Decimal)d).Multiply(p); }

        /// <summary>Gets the specified percentage of the Single.</summary>
        /// <param name="d">
        /// The value to get a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage.
        /// </param>
        public static Single Multiply(this Single d, Percentage p) { return (Single)((Decimal)d).Multiply(p); }

        /// <summary>Gets the specified percentage of the Int64.</summary>
        /// <param name="d">
        /// The value to get a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage.
        /// </param>
        public static Int64 Multiply(this Int64 d, Percentage p) { return (Int64)((Decimal)d).Multiply(p); }

        /// <summary>Gets the specified percentage of the Int32.</summary>
        /// <param name="d">
        /// The value to get a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage.
        /// </param>
        public static Int32 Multiply(this Int32 d, Percentage p) { return (Int32)((Decimal)d).Multiply(p); }

        /// <summary>Gets the specified percentage of the Int16.</summary>
        /// <param name="d">
        /// The value to get a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage.
        /// </param>
        public static Int16 Multiply(this Int16 d, Percentage p) { return (Int16)((Decimal)d).Multiply(p); }

        /// <summary>Gets the specified percentage of the UInt64.</summary>
        /// <param name="d">
        /// The value to get a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage.
        /// </param>
        [CLSCompliant(false)]
        public static UInt64 Multiply(this UInt64 d, Percentage p) { return (UInt64)((Decimal)d).Multiply(p); }

        /// <summary>Gets the specified percentage of the UInt32.</summary>
        /// <param name="d">
        /// The value to get a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage.
        /// </param>
        [CLSCompliant(false)]
        public static UInt32 Multiply(this UInt32 d, Percentage p) { return (UInt32)((Decimal)d).Multiply(p); }

        /// <summary>Gets the specified percentage of the UInt16.</summary>
        /// <param name="d">
        /// The value to get a percentage from.
        /// </param>
        /// <param name="p">
        /// The percentage.
        /// </param>
        [CLSCompliant(false)]
        public static UInt16 Multiply(this UInt16 d, Percentage p) { return (UInt16)((Decimal)d).Multiply(p); }

        #endregion

        #region Divide extensions

        /// <summary>Divides the Decimal by the specified percentage.</summary>
        /// <param name="d">
        /// The value to divide.
        /// </param>
        /// <param name="p">
        /// The percentage to divide to.
        /// </param>
        public static Decimal Divide(this Decimal d, Percentage p) { return d / (Decimal)p; }

        /// <summary>Divides the Double by the specified percentage.</summary>
        /// <param name="d">
        /// The value to divide.
        /// </param>
        /// <param name="p">
        /// The percentage to divide to.
        /// </param>
        public static Double Divide(this Double d, Percentage p) { return (Double)((Decimal)d).Divide(p); }

        /// <summary>Divides the Single by the specified percentage.</summary>
        /// <param name="d">
        /// The value to divide.
        /// </param>
        /// <param name="p">
        /// The percentage to divide to.
        /// </param>
        public static Single Divide(this Single d, Percentage p) { return (Single)((Decimal)d).Divide(p); }


        /// <summary>Divides the Int64 by the specified percentage.</summary>
        /// <param name="d">
        /// The value to divide.
        /// </param>
        /// <param name="p">
        /// The percentage to divide to.
        /// </param>
        public static Int64 Divide(this Int64 d, Percentage p) { return (Int64)((Decimal)d).Divide(p); }

        /// <summary>Divides the Int32 by the specified percentage.</summary>
        /// <param name="d">
        /// The value to divide.
        /// </param>
        /// <param name="p">
        /// The percentage to divide to.
        /// </param>
        public static Int32 Divide(this Int32 d, Percentage p) { return (Int32)((Decimal)d).Divide(p); }

        /// <summary>Divides the Int16 by the specified percentage.</summary>
        /// <param name="d">
        /// The value to divide.
        /// </param>
        /// <param name="p">
        /// The percentage to divide to.
        /// </param>
        public static Int16 Divide(this Int16 d, Percentage p) { return (Int16)((Decimal)d).Divide(p); }


        /// <summary>Divides the UInt64 by the specified percentage.</summary>
        /// <param name="d">
        /// The value to divide.
        /// </param>
        /// <param name="p">
        /// The percentage to divide to.
        /// </param>
        [CLSCompliant(false)]
        public static UInt64 Divide(this UInt64 d, Percentage p) { return (UInt64)((Decimal)d).Divide(p); }

        /// <summary>Divides the UInt32 by the specified percentage.</summary>
        /// <param name="d">
        /// The value to divide.
        /// </param>
        /// <param name="p">
        /// The percentage to divide to.
        /// </param>
        [CLSCompliant(false)]
        public static UInt32 Divide(this UInt32 d, Percentage p) { return (UInt32)((Decimal)d).Divide(p); }

        /// <summary>Divides the UInt16 by the specified percentage.</summary>
        /// <param name="d">
        /// The value to divide.
        /// </param>
        /// <param name="p">
        /// The percentage to divide to.
        /// </param>
        [CLSCompliant(false)]
        public static UInt16 Divide(this UInt16 d, Percentage p) { return (UInt16)((Decimal)d).Divide(p); }

        #endregion
    }
}
