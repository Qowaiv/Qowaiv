using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Qowaiv.UnitTests.TestTools
{
    public static class DebuggerDisplayAssert
    {
        public static void HasAttribute(Type type)
        {
            Assert.IsNotNull(type, "The supplied type should not be null.");

            var act = (DebuggerDisplayAttribute)type.GetCustomAttributes(typeof(DebuggerDisplayAttribute), false).FirstOrDefault();
            Assert.IsNotNull(act, "The type '{0}' has no DebuggerDisplay attribute.", type);

            Assert.AreEqual("{DebugToString()}", act.Value, "DebuggerDisplay attribute value is not '{DebugToString()}'.");
        }

        public static void HasResult(string expected, object value)
        {
            Assert.IsNotNull(value, "The supplied value should not be null.");

            var type = value.GetType();

            var method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(m => m.Name == "DebugToString" &&
                    m.ReturnType == typeof(String) &&
                    m.GetParameters().Length == 0);

            Assert.IsNotNull(method, "The type '{0}' does not contain a non public method { string DebugToString() }.", type);

            var actual = method.Invoke(value, new object[0]);

            Assert.AreEqual(expected, actual);
        }
    }
}
