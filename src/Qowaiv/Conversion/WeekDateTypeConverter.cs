using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Qowaiv.Conversion
{
    /// <summary>Provides a conversion for a week date.</summary>
    public class WeekDateTypeConverter : DateTypeConverter<WeekDate>
    {
        #region From's

        /// <inheritdoc />
        protected override WeekDate FromString(string str, CultureInfo culture) => WeekDate.Parse(str, culture);

        /// <inheritdoc />
        protected override WeekDate FromDate(Date date) => date;

        /// <inheritdoc />
        protected override WeekDate FromDateTime(DateTime dateTime) => (Date)dateTime;

        /// <inheritdoc />
        protected override WeekDate FromDateTimeOffset(DateTimeOffset offset) => FromDateTime(offset.Date);

        /// <inheritdoc />
        protected override WeekDate FromLocalDateTime(LocalDateTime local) => FromDateTime(local);

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        protected override WeekDate FromWeekDate(WeekDate weekDate) => throw new NotSupportedException();

        #endregion

        #region To's

        /// <inheritdoc />
        protected override DateTime ToDateTime(WeekDate date) => date;
        /// <inheritdoc />
        protected override DateTimeOffset ToDateTimeOffset(WeekDate date) => new DateTimeOffset(date, TimeSpan.Zero);

        /// <inheritdoc />
        protected override LocalDateTime ToLocalDateTime(WeekDate date) => date;

        /// <inheritdoc />
        protected override Date ToDate(WeekDate date) => date;

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        protected override WeekDate ToWeekDate(WeekDate date) => throw new NotSupportedException();

        #endregion
    }
}
