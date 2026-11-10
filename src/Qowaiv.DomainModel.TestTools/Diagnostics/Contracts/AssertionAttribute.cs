namespace Qowaiv.DomainModel.TestTools.Diagnostics.Contracts;

/// <summary>To mark a method explicitly as impure.</summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
[Conditional("CONTRACTS_FULL")]
internal sealed class AssertionAttribute : ImpureAttribute { }
