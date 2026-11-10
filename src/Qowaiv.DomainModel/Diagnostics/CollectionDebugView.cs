namespace Qowaiv.DomainModel.Diagnostics;

/// <summary>Allows the debugger to display collections.</summary>
[ExcludeFromCodeCoverage]
internal sealed class CollectionDebugView(IEnumerable enumeration)
{
    /// <summary>The array that is shown by the debugger.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
#pragma warning disable S2365 // Properties should not make collection or array copies
    // Every time the enumeration is shown in the debugger, a new array is created.
    // By doing this, it is always in sync with the current state of the enumeration.
    public object[] Items => enumeration.Cast<object>().ToArray();
#pragma warning restore S2365 // Properties should not make collection or array copies

    /// <summary>A reference to the enumeration to display.</summary>
    private readonly IEnumerable enumeration = enumeration;
}
