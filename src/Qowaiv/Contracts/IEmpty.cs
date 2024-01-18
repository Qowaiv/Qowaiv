namespace Qowaiv;

/// <summary>
/// Defines that the default of an SVO represents the empty/not set state.
/// </summary>
/// <typeparam name="TSelf">
/// The type of the SVO.
/// </typeparam>
public interface IEmpty<TSelf> where TSelf : struct, IEmpty<TSelf>
{
#if NET8_0_OR_GREATER
    /// <summary>Represents an empty/not set <typeparamref name="TSelf"/>.</summary>
    static abstract TSelf Empty { get; }
#endif

    /// <summary>True if <typeparamref name="TSelf"/> is empty, otherwise true.</summary>
    bool HasValue { get; }
}
