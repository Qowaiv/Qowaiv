#if NET8_0_OR_GREATER

using Qowaiv;

namespace System;

/// <summary>Extensions on <see cref="DateOnly" />.</summary>
public static class QowaivDateOnlyExtensions
{
    /// <summary>Returns a new date that adds the value of the specified <see cref="DateSpan" />
    /// to the value of this instance.
    /// </summary>
    /// <param name="d">
    /// The date to add a <see cref="DateSpan" /> to.
    /// </param>
    /// <param name="value">
    /// A <see cref="DateSpan" /> object that represents a positive or negative time interval.
    /// </param>
    /// <returns>
    /// A new date whose value is the sum of the date represented
    /// by this instance and the time interval represented by value.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The resulting date is less than <see cref="DateOnly.MinValue" /> or greater
    /// than <see cref="DateOnly.MaxValue" />.
    /// </exception>
    [Pure]
    public static DateOnly Add(this DateOnly d, DateSpan value) => d.Add(value, DateSpanSettings.Default);

    /// <summary>Returns a new date that adds the value of the specified <see cref="DateSpan" />
    /// to the value of this instance.
    /// </summary>
    /// <param name="d">
    /// The date to add a <see cref="DateSpan" /> to.
    /// </param>
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
    /// The resulting date is less than <see cref="DateOnly.MinValue" /> or greater
    /// than <see cref="DateOnly.MaxValue" />.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The provided settings have different value then <see cref="DateSpanSettings.DaysFirst" /> or <see cref="DateSpanSettings.Default" />.
    /// </exception>
    [Pure]
    public static DateOnly Add(this DateOnly d, DateSpan value, DateSpanSettings settings) => settings switch
    {
        DateSpanSettings.DaysFirst => d.AddDays(value.Days).AddMonths(value.TotalMonths),
        DateSpanSettings.Default => d.AddMonths(value.TotalMonths).AddDays(value.Days),
        _ => throw new ArgumentOutOfRangeException(nameof(settings), QowaivMessages.ArgumentOutOfRangeException_AddDateSpan),
    };

    /// <summary>Returns a new date that adds the value of the specified <see cref="MonthSpan" />
    /// to the value of this instance.
    /// </summary>
    /// <param name="d">
    /// The date to add a <see cref="MonthSpan" /> to.
    /// </param>
    /// <param name="value">
    /// A <see cref="MonthSpan" /> object that represents a positive or negative time interval.
    /// </param>
    /// <returns>
    /// A new date whose value is the sum of the date and time represented
    /// by this instance and the time interval represented by value.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The resulting date is less than <see cref="DateOnly.MinValue" /> or greater
    /// than <see cref="DateOnly.MaxValue" />.
    /// </exception>
    [Pure]
    public static DateOnly Add(this DateOnly d, MonthSpan value) => d.AddMonths(value.TotalMonths);

    /// <summary>Returns true if the date is in the specified month, otherwise false.</summary>
    /// <param name="d">
    /// The date to check.
    /// </param>
    /// <param name="month">
    /// The <see cref="Month" /> the date should be in.
    /// </param>
    [Pure]
    public static bool IsIn(this DateOnly d, Month month) => !month.IsEmptyOrUnknown() && d.Month == (int)month;

    /// <summary>Returns true if the date is in the specified year, otherwise false.</summary>
    /// <param name="d">
    /// The date to check.
    /// </param>
    /// <param name="year">
    /// The <see cref="Year" /> the date should be in.
    /// </param>
    [Pure]
    public static bool IsIn(this DateOnly d, Year year) => !year.IsEmptyOrUnknown() && d.Year == (int)year;
}

#endif
