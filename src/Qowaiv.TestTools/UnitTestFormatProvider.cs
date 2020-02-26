using System;
using System.Globalization;

namespace Qowaiv.TestTools
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
            return (typeof(ICustomFormatter).IsAssignableFrom(formatType)) ? this : null;
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
            var str = "Unit Test Formatter, value: '{0:" + format + "}', format: '{1}'";
            return string.Format(CultureInfo.InvariantCulture, str, arg, format);
        }
    }
}
