using System;
using System.ComponentModel;
using System.Diagnostics;
using NUnit.Framework;

namespace Qowaiv.UnitTests.TestTools
{
    /// <summary>Verifies conditions in unit tests for type converters.</summary>
    public static class TypeConverterAssert
    {
        /// <summary>Asserts that the type converter exists for the specfied type.</summary>
        [DebuggerStepThrough]
        public static void ConverterExists(Type type)
        {
            var converter = TypeDescriptor.GetConverter(type);
            Assert.IsNotNull(converter, "No TypeConverter could be resolved for '{0}'.", type);
        }

        /// <summary>Asserts that the type converter for the specfied type can convert from string.</summary>
        [DebuggerStepThrough]
        public static void CanConvertFromString(Type type)
        {
            var converter = TypeDescriptor.GetConverter(type);

            Assert.IsTrue(converter.CanConvertFrom(typeof(string)), "The TypeConverter for '{0}' cannot covert from System.String.", type);
        }

        /// <summary>Asserts that the type converter for the specfied type can convert to string.</summary>
        [DebuggerStepThrough]
        public static void CanConvertToString(Type type)
        {
            var converter = TypeDescriptor.GetConverter(type);

            Assert.IsTrue(converter.CanConvertTo(typeof(string)), "The TypeConverter for '{0}' cannot covert to System.String.", type);
        }

        /// <summary>Asserts that the type converter for the specfied type can not convert from string.</summary>
        [DebuggerStepThrough]
        public static void CanNotConvertFrom(Type type, Type source)
        {
            var converter = TypeDescriptor.GetConverter(type);

            Assert.IsFalse(converter.CanConvertFrom(source), "The TypeConverter for '{0}' can covert from {1}.", type, source);
        }

        /// <summary>Asserts that the type converter for the specfied type can not convert to string.</summary>
        [DebuggerStepThrough]
        public static void CanNotConvertTo(Type type, Type target)
        {
            var converter = TypeDescriptor.GetConverter(type);

            Assert.IsFalse(converter.CanConvertTo(target), "The TypeConverter for '{0}' can covert to {1}.", type, target);
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
    }
}