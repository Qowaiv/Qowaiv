using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Qowaiv.TestTools
{
    public static class DebuggerDisplayAssert
    {
        public static void HasAttribute(Type type)
        {
            Assert.IsNotNull(type, "The supplied type should not be null.");

            var act = type.GetCustomAttribute<DebuggerDisplayAttribute>(false);
            Assert.IsNotNull(act, "The type '{0}' has no DebuggerDisplay attribute.", type);

            Assert.AreEqual("{DebuggerDisplay}", act.Value, "DebuggerDisplay attribute value is not '{DebuggerDisplay}'.");
        }

        public static void HasResult(object expected, object value)
        {
            Assert.IsNotNull(value, "The supplied value should not be null.");

            var type = value.GetType();

            var prop = type.GetProperty("DebuggerDisplay", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.IsNotNull(prop, "The type '{0}' does not contain a non-public property DebuggerDisplay.", type);

            var actual = prop.GetValue(value);

            Assert.AreEqual(expected, actual);
        }
    }
}
