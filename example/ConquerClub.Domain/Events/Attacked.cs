namespace ConquerClub.Domain.Events;

public sealed record Attacked(
    CountryId Attacker,
    CountryId Defender,
    AttackResult Result);
