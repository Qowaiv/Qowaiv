using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Qowaiv.TestTools
{
    /// <summary>Verifies conditions in unit tests for type converters.</summary>
    public static class TypeConverterAssert
    {
        /// <summary>Asserts that the type converter for the specified type can convert from string.</summary>
        [DebuggerStepThrough]
        public static void CanConvertFromString(Type type)
        {
            var converter = TypeDescriptor.GetConverter(type);

            Assert.IsTrue(converter.CanConvertFrom(typeof(string)), $"The TypeConverter for '{type}' cannot covert from System.String.");
        }

        /// <summary>Asserts that the type converter for the specified type can convert to string.</summary>
        [DebuggerStepThrough]
        public static void CanConvertToString(Type type)
        {
            var converter = TypeDescriptor.GetConverter(type);

            Assert.IsTrue(converter.CanConvertTo(typeof(string)), $"The TypeConverter for '{type}' cannot covert to System.String.");
        }

        /// <summary>Asserts that the type converter for the specified type can not convert from string.</summary>
        [DebuggerStepThrough]
        public static void CanNotConvertFrom(Type type, Type source)
        {
            var converter = TypeDescriptor.GetConverter(type);

            Assert.IsTrue(!converter.CanConvertFrom(source), $"The TypeConverter for '{type}' can covert from {source}.");
        }

        /// <summary>Asserts that the type converter for the specified type can not convert to string.</summary>
        [DebuggerStepThrough]
        public static void CanNotConvertTo(Type type, Type target)
        {
            var converter = TypeDescriptor.GetConverter(type);

            Assert.IsTrue(!converter.CanConvertTo(target), $"The TypeConverter for '{type}' can covert to {target}.");
        }


        /// <summary>Asserts that the type converter converts the input string to the expected value.</summary>
        [DebuggerStepThrough]
        public static void ConvertFromEquals<T>(T expected, object input)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            var actual = (T)converter.ConvertFrom(input);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>Asserts that the TypeConverter converts the input value to the expected string.</summary>
        [DebuggerStepThrough]
        public static void ConvertToStringEquals<T>(string expected, T input)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            var actual = (string)converter.ConvertTo(input, typeof(string));

            Assert.AreEqual(expected, actual);
        }

        /// <summary>Asserts that the TypeConverter converts the input value to the expected string.</summary>
        [DebuggerStepThrough]
        public static void ConvertToEquals<TExp, TAct>(TExp expected, TAct input)
        {
            var converter = TypeDescriptor.GetConverter(typeof(TAct));
            var actual = (TExp)converter.ConvertTo(input, typeof(TExp));

            Assert.AreEqual(expected, actual);
        }
    }
}
