using System.Globalization;

namespace Qowaiv.UnitTests.Globalization
{
    /// <summary>Represents customized test <see cref="CultureInfo"/>'s.</summary>
    public static class TestCulture
    {
        /// <summary>Gets an Iranian <see cref="CultureInfo"/>.</summary>
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

        public static CultureInfo Nl_NL => new CultureInfo("nl-NL");

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
                    "april",
                    "mei",
                    "juni",
                    "juli",
                    "aug.",
                    "sep.",
                    "okt.",
                    "nov.",
                    "dec."
                };

                return culture;
            }
        }
    }
}
