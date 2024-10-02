namespace Qowaiv;

/// <summary>
/// Defines that the default of an SVO represents the empty/not set state.
/// </summary>
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
