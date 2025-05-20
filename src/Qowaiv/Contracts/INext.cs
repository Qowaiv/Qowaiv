namespace Qowaiv;

/// <summary>
/// Defines that a SVO/ID has a factory method that creates a new (random) instance.
/// </summary>
/// <typeparam name="TSelf">
/// The type of the SVO/ID.
/// </typeparam>
public interface INext<TSelf> where TSelf : struct, INext<TSelf>
{
#if NET8_0_OR_GREATER
    /// <summary>Creates the next (random) instance.</summary>
    [Pure]
    static abstract TSelf Next();
#endif
}
