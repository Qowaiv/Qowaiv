using System.Globalization;

namespace Qowaiv.UnitTests.Globalization
{
    /// <summary>Represents customized test <see cref="CultureInfo"/>'s.</summary>
    public static class TestCulture
    {
        /// <summary>Gets an Iranian <see cref="CultureInfo"/>.</summary>
        public static CultureInfo FaIR
        {
            get
            {
                var culture = new CultureInfo("fa-IR");
                culture.NumberFormat.PercentSymbol = "٪";
                culture.NumberFormat.PercentDecimalSeparator = ",";
                return culture;
            }
        }

    }
}
