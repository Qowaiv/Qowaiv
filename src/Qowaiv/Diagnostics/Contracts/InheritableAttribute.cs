﻿namespace Qowaiv.Diagnostics.Contracts;

/// <summary>Indicates the a class is designed to be inheritable.</summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
[Conditional("CONTRACTS_FULL")]
public class InheritableAttribute : Attribute
{
    /// <summary>Creates a new instance of the <see cref="InheritableAttribute"/> class.</summary>
    public InheritableAttribute(string? message = null) => _ = message;
}
