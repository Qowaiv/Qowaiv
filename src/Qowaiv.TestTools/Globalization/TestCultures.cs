using System.Globalization;

namespace Qowaiv.TestTools.Globalization
{
    /// <summary>Contains <see cref="CultureInfo"/>'s for test purposes.</summary>
    public static class TestCultures
    {
        /// <summary>Gets the Britsh (en-GB) <see cref="CultureInfo"/>.</summary>
        public static CultureInfo En_GB => new CultureInfo("en-GB");

        /// <summary>Gets the Iranian (fa-IR) <see cref="CultureInfo"/>.</summary>
        public static CultureInfo Fa_IR
        {
            get
            {
                var culture = new CultureInfo("fa-IR");
                culture.NumberFormat.PercentSymbol = "٪";
                culture.NumberFormat.PercentDecimalSeparator = ",";
                return culture;
            }
        }

        /// <summary>Gets the Dutch (nl-NL) <see cref="CultureInfo"/>.</summary>
        public static CultureInfo Nl_NL => new CultureInfo("nl-NL");

        /// <summary>Gets the Flemish (nl-BE) <see cref="CultureInfo"/>.</summary>
        public static CultureInfo Nl_BE
        {
            get
            {
                var culture = new CultureInfo("nl-BE");
                culture.DateTimeFormat.AbbreviatedMonthNames = new[] 
                {
                    "jan.",
                    "feb.",
                    "mrt.",
                    "apr.",
                    "mei",
                    "juni",
                    "juli",
                    "aug.",
                    "sep.",
                    "okt.",
                    "nov.",
                    "dec.",
                    ""
                };

                return culture;
            }
        }
    }
}
