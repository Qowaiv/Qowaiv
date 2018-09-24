using NUnit.Framework;
using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;

namespace Qowaiv.TestTools
{
    /// <summary>Verifies conditions in unit tests for type converters.</summary>
    public static class TypeConverterAssert
    {
        /// <summary>Asserts that the type converter exists for the specified type.</summary>
        [DebuggerStepThrough]
        public static void ConverterExists(Type type)
        {
            var converter = TypeDescriptor.GetConverter(type);
            Assert.IsNotNull(converter, "No TypeConverter could be resolved for '{0}'.", type);
        }

        /// <summary>Asserts that the type converter for the specified type can convert from string.</summary>
        [DebuggerStepThrough]
        public static void CanConvertFromString(Type type)
        {
            var converter = TypeDescriptor.GetConverter(type);

            Assert.IsTrue(converter.CanConvertFrom(typeof(string)), "The TypeConverter for '{0}' cannot covert from System.String.", type);
        }

        /// <summary>Asserts that the type converter for the specified type can convert to string.</summary>
        [DebuggerStepThrough]
        public static void CanConvertToString(Type type)
        {
            var converter = TypeDescriptor.GetConverter(type);

            Assert.IsTrue(converter.CanConvertTo(typeof(string)), "The TypeConverter for '{0}' cannot covert to System.String.", type);
        }

        /// <summary>Asserts that the type converter for the specified type can not convert from string.</summary>
        [DebuggerStepThrough]
        public static void CanNotConvertFrom(Type type, Type source)
        {
            var converter = TypeDescriptor.GetConverter(type);

            Assert.IsFalse(converter.CanConvertFrom(source), "The TypeConverter for '{0}' can covert from {1}.", type, source);
        }

        /// <summary>Asserts that the type converter for the specified type can not convert to string.</summary>
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

        /// <summary>Asserts that the type converter converts an instance descriptior to a guid.</summary>
        /// <remarks>
        /// Tests the call to the base implementation.
        /// </remarks>
        [DebuggerStepThrough]
        public static void ConvertFromInstanceDescriptor(Type type)
        {
            var expected = Guid.Parse("34adf67e-47d7-4cee-81fb-89be27aaf77c");

            var ctor = typeof(Guid).GetConstructor(new Type[] { typeof(string) });
            var args = new object[] { expected.ToString() };
            var descriptor = new InstanceDescriptor(ctor, args);
            var converter = TypeDescriptor.GetConverter(type);

            var actual = (Guid)converter.ConvertFrom(descriptor);

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
