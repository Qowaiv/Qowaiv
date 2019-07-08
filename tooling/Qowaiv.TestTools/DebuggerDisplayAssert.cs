using System;
using System.Diagnostics;
using System.Reflection;

namespace Qowaiv.TestTools
{
    /// <summary>Assertions related to the <see cref="DebuggerDisplayAttribute"/>.</summary>
    public static class DebuggerDisplayAssert
    {
        /// <summary>Verifies that a certain type is decorated with a <see cref="DebuggerDisplayAttribute"/>.</summary>
        public static void HasAttribute(Type type)
        {
            Assert.IsNotNull(type, "The supplied type should not be null.");

            var act = type.GetCustomAttribute<DebuggerDisplayAttribute>(false);
            Assert.IsNotNull(act, $"The type '{type}' has no DebuggerDisplay attribute.");

            Assert.AreEqual("{DebuggerDisplay}", act.Value, "DebuggerDisplay attribute value is not '{DebuggerDisplay}'.");
        }

        /// <summary>Verifies the outcome of the <see cref="DebuggerDisplayAttribute"/> of a certain <see cref="object"/>.</summary>
        public static void HasResult(object expected, object value)
        {
            Assert.IsNotNull(value, "The supplied value should not be null.");

            var type = value.GetType();

            var prop = type.GetProperty("DebuggerDisplay", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.IsNotNull(prop, $"The type '{type}' does not contain a non-public property DebuggerDisplay.");

            var actual = prop.GetValue(value);

            Assert.AreEqual(expected, actual);
        }
    }
}
