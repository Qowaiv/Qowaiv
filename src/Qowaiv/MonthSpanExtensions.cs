using System;
using System.Diagnostics.Contracts;

namespace Qowaiv
{
    /// <summary>Extensions for <see cref="MonthSpan"/>.</summary>
    public static class MonthSpanExtensions
    {
        /// <summary>Returns a new local date time that adds the value of the specified <see cref="MonthSpan"/>
        /// to the value of this instance.
        /// </summary>
        /// <param name="d">
        /// The date time to add a <see cref="MonthSpan"/> to.
        /// </param>
        /// <param name="value">
        /// A <see cref="MonthSpan"/> object that represents a positive or negative time interval.
        /// </param>
        /// <returns>
        /// A new date whose value is the sum of the date and time represented
        /// by this instance and the time interval represented by value.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The resulting date is less than <see cref="DateTime.MinValue"/> or greater
        /// than <see cref="DateTime.MaxValue"/>.
        /// </exception>
        [Pure]
        public static DateTime Add(this DateTime d, MonthSpan value) => d.AddMonths(value.TotalMonths);
    }
}
