﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for a Date.</summary>
    public class DateTypeConverter : DateTypeConverter<Date>
    {
        #region From's

        /// <inheritdoc />
        [Pure]
        protected override Date FromString(string str, CultureInfo culture) => Date.Parse(str, culture);

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [Pure]
        protected override Date FromDate(Date date) => throw new NotSupportedException();

        /// <inheritdoc />
        [Pure]
        protected override Date FromDateTime(DateTime dateTime) => (Date)dateTime;

        /// <inheritdoc />
        [Pure]
        protected override Date FromDateTimeOffset(DateTimeOffset offset) => FromDateTime(offset.Date);

        /// <inheritdoc />
        [Pure]
        protected override Date FromLocalDateTime(LocalDateTime local) => FromDateTime(local);

        /// <inheritdoc />
        [Pure]
        protected override Date FromWeekDate(WeekDate weekDate) => weekDate;

        #endregion

        #region To's

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [Pure]
        protected override Date ToDate(Date date) => throw new NotSupportedException();

        /// <inheritdoc />
        [Pure]
        protected override DateTime ToDateTime(Date date) => date;

        /// <inheritdoc />
        [Pure]
        protected override DateTimeOffset ToDateTimeOffset(Date date) => new DateTimeOffset(date, TimeSpan.Zero);

        /// <inheritdoc />
        [Pure]
        protected override LocalDateTime ToLocalDateTime(Date date) => ToDateTime(date);

        /// <inheritdoc />
        [Pure]
        protected override WeekDate ToWeekDate(Date date) => date;

        #endregion
    }
}
