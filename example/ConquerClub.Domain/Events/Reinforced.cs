namespace ConquerClub.Domain.Events;

public sealed record Reinforced(CountryId From, CountryId To, Army Army);
