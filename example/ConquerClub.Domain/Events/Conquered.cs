namespace ConquerClub.Domain.Events;

public sealed record Conquered(CountryId Attacker, CountryId Defender);
