using Qowaiv.Diagnostics.Contracts;
using System;
using System.Diagnostics;

namespace Qowaiv.TestTools.Diagnostics.Contracts
{
    /// <summary>To mark a method explicitly as impure.</summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    [Conditional("CONTRACTS_FULL")]
    public sealed class AssertionAttribute : ImpureAttribute { }
}
