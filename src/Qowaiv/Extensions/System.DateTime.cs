using Qowaiv;

namespace Qowaiv.Extensions;

/// <summary>Extensions on <see cref="DateTime" />.</summary>
public static class QowaivDateTime
{
    extension(DateTime date)
    {
        /// <summary>Returns a new date time that adds the value of the specified <see cref="DateSpan" />
        /// to the value of this instance.
        /// </summary>
        /// <param name="value">
        /// A <see cref="DateSpan" /> object that represents a positive or negative time interval.
        /// </param>
        /// <returns>
        /// A new date whose value is the sum of the date and time represented
        /// by this instance and the time interval represented by value.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The resulting date is less than <see cref="DateTime.MinValue" /> or greater
        /// than <see cref="DateTime.MaxValue" />.
        /// </exception>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTime Add(DateSpan value) => date.Add(value, DateSpanSettings.Default);

        /// <summary>Returns a new date time that adds the value of the specified <see cref="DateSpan" />
        /// to the value of this instance.
        /// </summary>
        /// <param name="value">
        /// A <see cref="DateSpan" /> object that represents a positive or negative time interval.
        /// </param>
        /// <param name="settings">
        /// If <see cref="DateSpanSettings.DaysFirst" /> days are added first, if <see cref="DateSpanSettings.Default" /> days are added second.
        /// </param>
        /// <returns>
        /// A new date whose value is the sum of the date represented
        /// by this instance and the time interval represented by value.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The resulting date is less than <see cref="DateTime.MinValue" /> or greater
        /// than <see cref="DateTime.MaxValue" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The provided settings have different value then <see cref="DateSpanSettings.DaysFirst" /> or <see cref="DateSpanSettings.Default" />.
        /// </exception>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTime Add(DateSpan value, DateSpanSettings settings) => settings switch
        {
            DateSpanSettings.DaysFirst => date.AddDays(value.Days).AddMonths(value.TotalMonths),
            DateSpanSettings.Default => date.AddMonths(value.TotalMonths).AddDays(value.Days),
            _ => throw new ArgumentOutOfRangeException(nameof(settings), QowaivMessages.ArgumentOutOfRangeException_AddDateSpan),
        };

        /// <summary>Returns a new date time that adds the value of the specified <see cref="MonthSpan" />
        /// to the value of this instance.
        /// </summary>
        /// <param name="value">
        /// A <see cref="MonthSpan" /> object that represents a positive or negative time interval.
        /// </param>
        /// <returns>
        /// A new date whose value is the sum of the date and time represented
        /// by this instance and the time interval represented by value.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The resulting date is less than <see cref="DateTime.MinValue" /> or greater
        /// than <see cref="DateTime.MaxValue" />.
        /// </exception>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTime Add(MonthSpan value) => date.AddMonths(value.TotalMonths);

        /// <summary>Returns a new date time that adds the value of the specified <see cref="YearSpan" />
        /// to the value of this instance.
        /// </summary>
        /// <param name="value">
        /// A <see cref="YearSpan" /> object that represents a positive or negative time interval.
        /// </param>
        /// <returns>
        /// A new date whose value is the sum of the date and time represented
        /// by this instance and the time interval represented by value.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The resulting date is less than <see cref="DateTime.MinValue" /> or greater
        /// than <see cref="DateTime.MaxValue" />.
        /// </exception>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTime Add(YearSpan value) => date.AddYears((int)value);

        /// <summary>Returns true if the date is in the specified month, otherwise false.</summary>
        /// <param name="month">
        /// The <see cref="Month" /> the date should be in.
        /// </param>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsIn(Month month) => !month.IsEmptyOrUnknown() && date.Month == (int)month;

        /// <summary>Returns true if the date is in the specified year, otherwise false.</summary>
        /// <param name="year">
        /// The <see cref="Year" /> the date should be in.
        /// </param>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsIn(Year year) => !year.IsEmptyOrUnknown() && date.Year == (int)year;
    }
}
