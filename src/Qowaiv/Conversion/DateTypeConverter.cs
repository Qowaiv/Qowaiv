using System;
using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for a Date.</summary>
    public class DateTypeConverter : DateTypeConverter<Date>
    {
        #region From's

        /// <inheritdoc />
        protected override Date FromString(string str, CultureInfo culture) => Date.Parse(str, culture);

        /// <inheritdoc />
        protected override Date FromDate(Date date) => date;

        /// <inheritdoc />
        protected override Date FromDateTime(DateTime dateTime) => (Date)dateTime;
        
        /// <inheritdoc />
        protected override Date FromDateTimeOffset(DateTimeOffset offset) => FromDateTime(offset.Date);

        /// <inheritdoc />
        protected override Date FromLocalDateTime(LocalDateTime local) => FromDateTime(local);

        /// <inheritdoc />
        protected override Date FromWeekDate(WeekDate weekDate) => weekDate;

        #endregion

        #region To's

        /// <inheritdoc />
        protected override Date ToDate(Date date) => date;

        /// <inheritdoc />
        protected override DateTime ToDateTime(Date date) => date;

        /// <inheritdoc />
        protected override DateTimeOffset ToDateTimeOffset(Date date) => ToDateTime(date);

        /// <inheritdoc />
        protected override LocalDateTime ToLocalDateTime(Date date) => ToDateTime(date);

        /// <inheritdoc />
        protected override WeekDate ToWeekDate(Date date) => date;

        #endregion
    }
}
