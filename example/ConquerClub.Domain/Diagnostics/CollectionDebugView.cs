#pragma warning disable S2365
// Properties should not make collection or array copies
// Required for debugging purposes.

namespace ConquerClub.Domain.Diagnostics;

/// <summary>Allows the debugger to display collections.</summary>
internal sealed class CollectionDebugView(IEnumerable enumeration)
{
    /// <summary>A reference to the enumeration to display.</summary>
    private readonly IEnumerable collection = enumeration;

    /// <summary>Gets the array that is shown by the debugger.</summary>
    /// <remarks>
    /// Every time the enumeration is shown in the debugger, a new array is created.
    /// By doing this, it is always in sync with the current state of the enumeration.
    /// </remarks>
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public object[] Items => [.. collection.Cast<object>()];
}
