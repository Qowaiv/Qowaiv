using ConquerClub.Domain.Diagnostics;

namespace ConquerClub.Domain;

/// <summary>Represents a continent/super-region.</summary>
[DebuggerTypeProxy(typeof(CollectionDebugView))]
[Mutable]
public sealed class Continent : IEnumerable<Country>
{
    internal Continent(ContinentId id, string name, int bonus)
    {
        Id = id;
        Name = name;
        Bonus = bonus;
    }

    /// <summary>Gets the identifier of the continent.</summary>
    public ContinentId Id { get; }

    /// <summary>Gets the name of the continent.</summary>
    public string Name { get; }

    /// <summary>Gets the bonus for controlling the full continent.</summary>
    public int Bonus { get; }

    /// <summary>Gets the owner of the continent, and <see cref="Player.Neutral"/> if nobody owns the continent.</summary>
    public Player Owner
        => Countries.All(r => r.Owner == Countries[0].Owner) ? Countries[0].Owner : Player.Neutral;

    /// <summary>Gets the number of regions on the continent.</summary>
    public IReadOnlyList<Country> Countries { get; internal set; } = [];

    /// <inheritdoc/>
    [Pure]
    public override string ToString() => $"{Name} ({Id}), bonus {Bonus}, {Countries.Count} countries";

    /// <inheritdoc/>
    [Pure] 
    public IEnumerator<Country> GetEnumerator() => Countries.GetEnumerator();

    /// <inheritdoc/>
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public static class ContinentExtensions
{
    [Pure]
    public static Continent? ById(this IEnumerable<Continent> continents, ContinentId id)
        => continents.FirstOrDefault(c => c.Id == id);
}
