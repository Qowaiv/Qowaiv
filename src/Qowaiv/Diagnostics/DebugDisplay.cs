using System;
using System.Globalization;
using System.Reflection;

namespace Qowaiv.Diagnostics
{
    internal static class DebugDisplay
    {
        public const string Empty = "{empty}";
        public const string Unknown = "{unknown}";

        public static string DebuggerDisplay<Svo>(this Svo svo, string format) where Svo : struct, IFormattable
        {
            if (svo.Equals(default(Svo)) && HasEmptyValue<Svo>())
            {
                return Empty;
            }
            else if (svo.Equals(Qowaiv.Unknown.Value(typeof(Svo))))
            {
                return Unknown;
            }
            else
            {
                return string.Format(CultureInfo.InvariantCulture, format, svo);
            }
        }

        private static bool HasEmptyValue<Svo>()
            => typeof(Svo)
            .GetCustomAttribute<SingleValueObjectAttribute>()
            .StaticOptions.HasFlag(SingleValueStaticOptions.HasEmptyValue);
    }
}
