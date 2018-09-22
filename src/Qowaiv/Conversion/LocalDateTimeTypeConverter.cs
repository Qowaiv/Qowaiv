﻿using System;
using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for a local date time.</summary>
    public class LocalDateTimeTypeConverter : DateTypeConverter<LocalDateTime>
    {
        #region From's

        /// <inheritdoc />
        protected override LocalDateTime FromString(string str, CultureInfo culture) => LocalDateTime.Parse(str, culture);

        /// <inheritdoc />
        protected override LocalDateTime FromDate(Date date) => (DateTime)date;

        /// <inheritdoc />
        protected override LocalDateTime FromDateTime(DateTime dateTime) => dateTime;

        /// <inheritdoc />
        protected override LocalDateTime FromDateTimeOffset(DateTimeOffset offset) => FromDateTime(offset.DateTime);

        /// <inheritdoc />
        protected override LocalDateTime FromLocalDateTime(LocalDateTime local) => local;

        /// <inheritdoc />
        protected override LocalDateTime FromWeekDate(WeekDate weekDate) => weekDate;

        #endregion

        #region To's

        /// <inheritdoc />
        protected override DateTime ToDateTime(LocalDateTime date) => date;

        /// <inheritdoc />
        protected override DateTimeOffset ToDateTimeOffset(LocalDateTime date) => new DateTimeOffset(date);

        /// <inheritdoc />
        protected override LocalDateTime ToLocalDateTime(LocalDateTime date) => date;

        /// <inheritdoc />
        protected override Date ToDate(LocalDateTime date) => (Date)ToDateTime(date);

        /// <inheritdoc />
        protected override WeekDate ToWeekDate(LocalDateTime date) => ToDate(date);

        #endregion
    }
}

