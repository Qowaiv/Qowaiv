namespace ConquerClub.Domain.Commands;

public sealed record AutoAttack(
    CountryId Attacker,
    CountryId Defender,
    GameId Game,
    int ExpectedVersion)
    : Command(Game, ExpectedVersion);
