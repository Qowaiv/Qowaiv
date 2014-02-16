using System;
using System.Globalization;

namespace Qowaiv.UnitTests.TestTools.Formatting
{
    /// <summary>Represents the unit test format provider.</summary>
    public class UnitTestFormatProvider : IFormatProvider, ICustomFormatter
    {
        /// <summary>Returns an object that provides formatting services for the specified type.</summary>
        /// <param name="formatType">
        /// The type of format object to return.
        /// </param>
        /// <remarks>
        /// Supports ICustomFormatter.
        /// </remarks>
        public object GetFormat(Type formatType)
        {
            if (typeof(ICustomFormatter).IsAssignableFrom(formatType))
            {
                return this;
            }
            else
            {
                return null;
            }
        }

        /// <summary>Formats the object as String.</summary>
        /// <param name="format">
        /// The format to use.
        /// </param>
        /// <param name="arg">
        /// The object to format.
        /// </param>
        /// <param name="formatProvider">
        /// The specified formatProvider.
        /// </param>
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            return String.Format(CultureInfo.InvariantCulture, "Unit Test Formatter, value: '{0}', format: '{1}'", arg, format);
        }
    }
}
