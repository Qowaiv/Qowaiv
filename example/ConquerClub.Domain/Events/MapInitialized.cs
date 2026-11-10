namespace ConquerClub.Domain.Events;

public sealed record MapInitialized(
    IReadOnlyCollection<ContinentInitialized> Continents,
    IReadOnlyCollection<CountryInitialized> Countries);

public sealed record CountryInitialized(string Name, IReadOnlyCollection<CountryId> Borders);

public sealed record ContinentInitialized(string Name, int Bonus, IReadOnlyCollection<CountryId> Territories);
