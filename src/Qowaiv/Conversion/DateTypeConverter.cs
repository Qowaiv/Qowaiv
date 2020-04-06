using System;
using System.Diagnostics.CodeAnalysis;
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
        [ExcludeFromCodeCoverage]
        protected override Date FromDate(Date date) => throw new NotSupportedException();

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
        [ExcludeFromCodeCoverage]
        protected override Date ToDate(Date date) => throw new NotSupportedException();

        /// <inheritdoc />
        protected override DateTime ToDateTime(Date date) => date;

        /// <inheritdoc />
        protected override DateTimeOffset ToDateTimeOffset(Date date) => new DateTimeOffset(date, TimeSpan.Zero);

        /// <inheritdoc />
        protected override LocalDateTime ToLocalDateTime(Date date) => ToDateTime(date);

        /// <inheritdoc />
        protected override WeekDate ToWeekDate(Date date) => date;

        #endregion
    }
}
