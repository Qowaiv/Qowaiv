namespace Qowaiv;

/// <summary>
/// Defines that a SVO has both an empty, a known, and an unknown state.
/// </summary>
/// <remarks>
/// The unknown state can be used to explicitly state that we know a value not
/// to be empty, but currently do not know what value it should have.
/// </remarks>
public interface IUnknown : IEmpty
{
    /// <summary>False if the SVO is empty or unknown, otherwise true.</summary>
    bool IsKnown { get; }
}

/// <inheritdoc cref="IUnknown" />
/// <typeparam name="TSelf">
/// The type of the SVO.
/// </typeparam>
public interface IUnknown<TSelf> : IEmpty<TSelf>, IUnknown where TSelf : struct, IEmpty<TSelf>
{
#if !NETSTANDARD2_0
    /// <summary>Represents an unknown (but set) <typeparamref name="TSelf" />.</summary>
    static abstract TSelf Unknown { get; }
#endif
}
