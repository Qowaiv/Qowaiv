namespace Qowaiv.DomainModel.Collections;

/// <summary>Represents the start of an (logical) if-statement.</summary>
[DebuggerDisplay("If: {State}")]
public sealed class If
{
    /// <summary>Initializes a new instance of the <see cref="If"/> class.</summary>
    internal If(bool condition, AppendOnlyCollection collection)
        : this(condition ? IfState.True : IfState.False, collection) { }

    /// <summary>Initializes a new instance of the <see cref="If"/> class.</summary>
    internal If(IfState state, AppendOnlyCollection collection)
    {
        State = state;
        Collection = collection;
    }

    /// <summary>The state of the if-branch.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    internal IfState State { get; }

    /// <summary>The parent <see cref="ImmutableCollection"/>.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    internal AppendOnlyCollection Collection { get; }

    /// <summary>Creates a new <see cref="ImmutableCollection"/> with the added item(s)
    /// if the condition is met.</summary>
    /// <remarks>
    /// Null, and null items are ignored.
    /// </remarks>
    [Pure]
    public Then Then(Func<object> item) => State switch
    {
        IfState.True => new Then(true, Collection.Add(item())),
        IfState.False => new Then(false, Collection),
        _ => new Then(true, Collection),
    };
}
