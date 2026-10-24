namespace Qowaiv;

/// <summary>
/// Defines that the default of an SVO represents the empty/not set state.
/// </summary>
public interface IEmpty
{
    /// <summary>False if the SVO is empty, otherwise true.</summary>
    bool HasValue { get; }
}

/// <inheritdoc cref="IEmpty" />
/// <typeparam name="TSelf">
/// The type of the SVO.
/// </typeparam>
public interface IEmpty<TSelf> : IEmpty where TSelf : struct, IEmpty<TSelf>
{
#if !NETSTANDARD2_0
    /// <summary>Represents an empty/not set <typeparamref name="TSelf" />.</summary>
    static abstract TSelf Empty { get; }
#endif
}
