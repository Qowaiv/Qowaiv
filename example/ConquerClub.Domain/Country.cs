namespace ConquerClub.Domain;

/// <summary>Represents a region/country/territory.</summary>
[Mutable]
public sealed class Country
{
    internal Country(CountryId id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <summary>Gets the continent of the region.</summary>
    public Continent? Continent { get; internal set; }

    public IReadOnlyList<Country> Borders { get; internal set; } = [];

    /// <summary>Gets the identifier of the region.</summary>
    public CountryId Id { get; }

    /// <summary>Gets the name of the region.</summary>
    public string Name { get; }

    /// <summary>Gets the owner of the region.</summary>
    public Player Owner => Army.Owner;

    /// <summary>Gets or sets the army occupying the region.</summary>
    public Army Army { get; internal set; }

    /// <inheritdoc/>
    [Pure]
    public override string ToString() => $"{Name} ({Id}), Army: {Army}";
}
