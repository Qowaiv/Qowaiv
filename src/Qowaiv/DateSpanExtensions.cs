using System;
using System.Diagnostics.Contracts;

namespace Qowaiv
{
    /// <summary>Extensions for <see cref="DateSpan"/>.</summary>
    public static class DateSpanExtensions
    {
        /// <summary>Returns a new local date time that adds the value of the specified <see cref="DateSpan"/>
        /// to the value of this instance.
        /// </summary>
        /// <param name="d">
        /// The date time to add a <see cref="DateSpan"/> to.
        /// </param>
        /// <param name="value">
        /// A <see cref="DateSpan"/> object that represents a positive or negative time interval.
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
        public static DateTime Add(this DateTime d, DateSpan value) => d.Add(value, false);

        /// <summary>Returns a new local date time that adds the value of the specified <see cref="DateSpan"/>
        /// to the value of this instance.
        /// </summary>
        /// <param name="d">
        /// The date time to add a <see cref="DateSpan"/> to.
        /// </param>
        /// <param name="value">
        /// A <see cref="DateSpan"/> object that represents a positive or negative time interval.
        /// </param>
        /// <param name="daysFirst">
        /// If true, days are added first, otherwise months are added first.
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
        public static DateTime Add(this DateTime d, DateSpan value, bool daysFirst)
            => daysFirst
            ? d.AddDays(value.Days).AddMonths(value.TotalMonths)
            : d.AddMonths(value.TotalMonths).AddDays(value.Days);
    }
}
