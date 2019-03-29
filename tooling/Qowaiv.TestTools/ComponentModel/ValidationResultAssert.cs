using NUnit.Framework;
using Qowaiv.ComponentModel;
using System.Diagnostics;

namespace Qowaiv.TestTools.ComponentModel
{
    /// <summary>Assertion helper class on <see cref="Result"/> and/or <see cref="Result{T}"/>.</summary>
    public static class ValidationResultAssert
    {
        /// <summary>Asserts that the result is valid, throws if not.</summary>
        [DebuggerStepThrough]
        public static void IsValid(Result result)
        {
            var actualMessages = result.Errors.ForAssertion();
            Assert.AreEqual(new ValidationTestMessage[0], actualMessages);
        }

        /// <summary>Asserts that result contains expected messages. Throws if not.</summary>
        [DebuggerStepThrough]
        public static void WithErrors(Result result, params ValidationTestMessage[] expectedMessages)
        {
            var actualMessages = result.Errors.ForAssertion();
            Assert.AreEqual(expectedMessages, actualMessages);
        }
    }
}
