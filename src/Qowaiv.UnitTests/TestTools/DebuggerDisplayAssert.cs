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

			Assert.AreEqual("{DebuggerDisplay}", act.Value, "DebuggerDisplay attribute value is not '{DebuggerDisplay}'.");
		}

		public static void HasResult(string expected, object value)
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
