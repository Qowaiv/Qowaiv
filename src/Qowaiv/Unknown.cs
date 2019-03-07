﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Qowaiv
{
    /// <summary>Helps handling the unknown status of Single Value Objects.</summary>
    /// <remarks>
    /// The 'unknown' case differences from the 'empty' case. Where 'empty' just means:
    /// Not set (yet), this is an unset (default) value, 'unknown' means that the user
    /// has set the value, saying, there must be some value, but I just don't know
    /// which value it should be.
    /// 
    /// Note that not all scenario's that support 'empty' support 'unknown' too.
    /// </remarks>
    public static class Unknown
    {
        /// <summary>Returns true if the string represents unknown, otherwise false.</summary>
        /// <param name="val">
        /// The string value to test.
        /// </param>
        public static bool IsUnknown(string val)
        {
            return IsUnknown(val, CultureInfo.CurrentCulture);
        }

        /// <summary>Returns true if the string represents unknown, otherwise false.</summary>
        /// <param name="val">
        /// The string value to test.
        /// </param>
        /// <param name="culture">
        /// The culture to test for.
        /// </param>
        public static bool IsUnknown(string val, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(val)) { return false; }

            var c = culture ?? CultureInfo.InvariantCulture;

            if (!StringValues.TryGetValue(c, out string[] values))
            {
                lock (addCulturelocker)
                {
                    if (!StringValues.TryGetValue(c, out values))
                    {
                        values = ResourceManager
                            .GetString("Values", c)
                            .Split(';')
                            .Select(v => v.ToUpper(c))
                            .ToArray();

                        StringValues[c] = values;
                    }
                }
            }
            return
                values.Contains(val.ToUpper(c)) ||
                (
                    !c.Equals(CultureInfo.InvariantCulture) &&
                    StringValues[CultureInfo.InvariantCulture].Contains(val.ToUpperInvariant())
                );
        }

        /// <summary>Gets the value that represents set but unknown.</summary>
        /// <param name="type">
        /// The type that should could have an unknown value.
        /// </param>
        /// <returns>
        /// null, if not defined, otherwise the unknown value for a type.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// if type is not a value type.
        /// </exception>
        /// <remarks>
        /// The unknown value is expected to be static field or property of the type with the name "Unknown".
        /// </remarks>
        public static object Value(Type type)
        {
            if (UnknownValues.TryGetValue(type, out object unknown))
            {
                return unknown;
            }
            lock (addUknownValuelocker)
            {
                if (!UnknownValues.TryGetValue(type, out unknown))
                {
                    var field = type.GetField(nameof(Unknown), BindingFlags.Public | BindingFlags.Static);
                    if (field?.FieldType == type)
                    {
                        unknown = field.GetValue(null);
                    }
                    else
                    {
                        var property = type.GetProperty(nameof(Unknown), BindingFlags.Public | BindingFlags.Static);
                        if (property?.PropertyType == type)
                        {
                            unknown = property.GetValue(null);
                        }
                    }
                    UnknownValues[type] = unknown;
                }
            }
            return unknown;
        }

        /// <summary>The resource manager managing the culture based string values.</summary>
        private readonly static Dictionary<CultureInfo, string[]> StringValues = new Dictionary<CultureInfo, string[]>()
        {
            { CultureInfo.InvariantCulture, new []{ "?", "UNKNOWN", "NOT KNOWN", "NOTKNOWN" } },
        };

        private static readonly Dictionary<Type, object> UnknownValues = new Dictionary<Type, object>();

        /// <summary>The resource manager managing the culture based string values.</summary>
        private static readonly ResourceManager ResourceManager = new ResourceManager("Qowaiv.UnknownLabels", typeof(Unknown).Assembly);

        /// <summary>The locker for adding a culture.</summary>
        private static readonly object addCulturelocker = new object();
        private static readonly object addUknownValuelocker = new object();
    }
}
