namespace Specs;

public static class Rnd
{
    /// <summary>Creates a new randomizer with a fixed seed.</summary>
    /// <param name="seed">
    /// The seed to initialize the state of the randomizer.
    /// </param>
    [Pure]
    public static Random New(int seed = 42) => new(seed);

#if NETSTANDARD2_0
#pragma warning disable S2245 // Pseudorandom number generators (PRNGs) should not be used in security contexts
    
    /// <summary>Provides a thread-safe randomizer.</summary>
    public static Random Shared {get;} = new();
#pragma warning restore S2245 // Pseudorandom number generators (PRNGs) should not be used in security contexts
#else
    /// <summary>Provides a thread-safe randomizer.</summary>
    public static Random Shared => Random.Shared;
#endif
}
