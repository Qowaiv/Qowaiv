using System;
using System.Diagnostics;

namespace Qowaiv.Diagnostics.Contracts
{
    /// <summary>To mark a method explicitly as impure.</summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    [Conditional("CONTRACTS_FULL")]
    public class ImpureAttribute : Attribute { }
}
