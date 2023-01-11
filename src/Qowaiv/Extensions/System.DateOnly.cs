#if NET6_0_OR_GREATER

using Qowaiv;

namespace System;

/// <summary>Extensions on <see cref="DateOnly"/>.</summary>
public static class QowaivDateOnlyExtensions
{
    /// <summary>Returns a new date that adds the value of the specified <see cref="DateSpan"/>
    /// to the value of this instance.
    /// </summary>
    /// <param name="d">
    /// The date time to add a <see cref="DateSpan"/> to.
    /// </param>
    /// <param name="value">
    /// A <see cref="DateSpan"/> object that represents a positive or negative time interval.
    /// </param>
    /// <returns>
    /// A new date whose value is the sum of the date represented
    /// by this instance and the time interval represented by value.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The resulting date is less than <see cref="DateOnly.MinValue"/> or greater
    /// than <see cref="DateOnly.MaxValue"/>.
    /// </exception>
    [Pure]
    public static DateOnly Add(this DateOnly d, DateSpan value) => d.Add(value, false);

    /// <summary>Returns a new local date time that adds the value of the specified <see cref="DateSpan"/>
    /// to the value of this instance.
    /// </summary>
    /// <param name="d">
    /// The date to add a <see cref="DateSpan"/> to.
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
    /// The resulting date is less than <see cref="DateOnly.MinValue"/> or greater
    /// than <see cref="DateOnly.MaxValue"/>.
    /// </exception>
    [Pure]
    public static DateOnly Add(this DateOnly d, DateSpan value, bool daysFirst)
        => daysFirst
        ? d.AddDays(value.Days).AddMonths(value.TotalMonths)
        : d.AddMonths(value.TotalMonths).AddDays(value.Days);

    /// <summary>Returns a new local date time that adds the value of the specified <see cref="MonthSpan"/>
    /// to the value of this instance.
    /// </summary>
    /// <param name="d">
    /// The date to add a <see cref="MonthSpan"/> to.
    /// </param>
    /// <param name="value">
    /// A <see cref="MonthSpan"/> object that represents a positive or negative time interval.
    /// </param>
    /// <returns>
    /// A new date whose value is the sum of the date and time represented
    /// by this instance and the time interval represented by value.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The resulting date is less than <see cref="DateOnly.MinValue"/> or greater
    /// than <see cref="DateOnly.MaxValue"/>.
    /// </exception>
    [Pure]
    public static DateOnly Add(this DateOnly d, MonthSpan value) => d.AddMonths(value.TotalMonths);

    /// <summary>Returns true if the date is in the specified month, otherwise false.</summary>
    /// <param name="d">
    /// The date to check.
    /// </param>
    /// <param name="month">
    /// The <see cref="Month"/> the date should be in.
    /// </param>
    [Pure]
    public static bool InMonth(this DateOnly d, Month month) => !month.IsEmptyOrUnknown() && d.Month == (int)month;

    /// <summary>Returns true if the date is in the specified year, otherwise false.</summary>
    /// <param name="d">
    /// The date to check.
    /// </param>
    /// <param name="year">
    /// The <see cref="Year"/> the date should be in.
    /// </param>
    [Pure]
    public static bool InYear(this DateOnly d, Year year) => !year.IsEmptyOrUnknown() && d.Year == (int)year;
}

#endif
