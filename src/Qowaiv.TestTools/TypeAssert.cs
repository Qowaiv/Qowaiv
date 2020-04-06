using System;
using System.Diagnostics;
using System.Linq;

namespace Qowaiv.TestTools
{
    /// <summary>Contains <see cref="Type"/> specific assertions.</summary>
    public static class TypeAssert
    {
        /// <summary>Asserts that the type implements the interface, and throws if it does not.</summary>
        [DebuggerStepThrough]
        public static void ImplementsInterface(Type type, Type interfaceType)
        {
            Assert.IsNotNull(type);
            Assert.IsTrue(type.GetInterfaces().Contains(interfaceType), $"The type {type}, should implement the interface {interfaceType}.");
        }
    }
}
