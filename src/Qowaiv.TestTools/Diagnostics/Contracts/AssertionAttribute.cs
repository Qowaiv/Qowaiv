using Qowaiv.Diagnostics.Contracts;
using System;
using System.Diagnostics;

namespace Qowaiv.TestTools.Diagnostics.Contracts
{
    /// <summary>To mark a method explicitly as impure. Methods decorated with
    /// this attribute do an assertion. The returned value allows continuation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    [Conditional("CONTRACTS_FULL")]
    public sealed class AssertionAttribute : ImpureAttribute { }
}
