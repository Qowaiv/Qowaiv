#pragma warning disable S2365 // Properties should not make collection or array copies
// but here, it is the only purpose of this debug view.

namespace Qowaiv.Diagnostics;

/// <summary>Allows the debugger to display collections.</summary>
[ExcludeFromCodeCoverage]
internal sealed class CollectionDebugView
{
    /// <summary>Initializes a new instance of the <see cref="CollectionDebugView"/> class.</summary>
    /// <param name="enumeration">
    /// The collection to show the elements of.
    /// </param>
    public CollectionDebugView(IEnumerable enumeration) => Enumeration = enumeration;

    /// <summary>The array that is shown by the debugger.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public object[] Items => Enumeration.Cast<object>().ToArray();

    /// <summary>A reference to the enumeration to display.</summary>
    private readonly IEnumerable Enumeration;
}
