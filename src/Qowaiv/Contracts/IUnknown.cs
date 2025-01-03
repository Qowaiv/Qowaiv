namespace Qowaiv;

/// <summary>
/// Defines that an SVO has both an empty, a known, and an unknown state.
/// </summary>
/// <remarks>
/// The unknown state can be used to explicitly state that we know a value not
/// to be empty, but currently do not know what value it should have.
/// </remarks>
/// <typeparam name="TSelf">
/// The type of the SVO.
/// </typeparam>
public interface IUnknown<TSelf> : IEmpty<TSelf> where TSelf : struct, IEmpty<TSelf>
{
#if NET8_0_OR_GREATER
    /// <summary>Represents an unknown (but set) <typeparamref name="TSelf" />.</summary>
    static abstract TSelf Unknown { get; }
#endif

    /// <summary>False if <typeparamref name="TSelf" /> is empty or unknown, otherwise true.</summary>
    bool IsKnown { get; }
}
