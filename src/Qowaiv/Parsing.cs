﻿using Qowaiv.Formatting;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Qowaiv
{
    internal static class Parsing
    {
        private static Regex SpacePattern = new Regex(@"\s", RegexOptions.Compiled);
        private static Regex SpaceAndMarkupPattern = new Regex(@"[\s\.\-_]", RegexOptions.Compiled);

        /// <summary>Clears spacing.</summary>
        public static string ClearSpacing(string str)
        {
            return SpacePattern.Replace(str, "");
        }
        /// <summary>Clears spacing and uppercases.</summary>
        public static string ClearSpacingToUpper(string str)
        {
            return ClearSpacing(str).ToUpperInvariant();
        }
        /// <summary>Clears spacing.</summary>
        public static string ClearSpacingAndMarkup(string str)
        {
            return SpaceAndMarkupPattern.Replace(str, "");
        }
        /// <summary>Clears spacing and uppercases.</summary>
        public static string ClearSpacingAndMarkupToUpper(string str)
        {
            return ClearSpacingAndMarkup(str).ToUpperInvariant();
        }

        /// <summary>Applies <see cref="IFormattable.ToString(string, IFormatProvider)"/> 
        /// when possible, otherwise <see cref="object.ToString()"/>.
        /// </summary>
        public static string ToInvariant(object obj)
        {
            if(obj is null)
            {
                return null;
            }
            if(obj is IFormattable f)
            {
                return f.ToString(null, CultureInfo.InvariantCulture);
            }
            return obj.ToString();
        }

        /// <summary>Unifies the string.</summary>
        public static string ToUnified(string str)
        {
            return (StringFormatter.ToNonDiacritic(str) ?? string.Empty).ToUpperInvariant().Replace(" ", "");
        }
    }
}
