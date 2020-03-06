using System;
using System.Globalization;

namespace Qowaiv
{
    /// <summary>Helper for creating <see cref="Exception"/>'s.</summary>
    internal static class Exceptions
    {
        public static InvalidCastException InvalidCast(Type from, Type to) 
        {
            return new InvalidCastException(string.Format(
                CultureInfo.CurrentCulture,
                QowaivMessages.InvalidCastException_FromTo,
                from,
                to));
        }
    }
}
