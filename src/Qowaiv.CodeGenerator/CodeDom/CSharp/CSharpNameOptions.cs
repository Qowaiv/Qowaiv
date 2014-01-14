using System;

namespace Qowaiv.CodeGenerator.CodeDom.CSharp
{
    /// <summary>The options for a C# name.</summary>
    [Flags]
    public enum CSharpNameOptions
    {
        /// <summary>No options.</summary>
        None = 0,
        /// <summary>Use aliases for primative types.</summary>
        AliasForPrimitives = 1,
        /// <summary>Use the full name for non primative types.</summary>
        UseFullName = 2,
    }
}
