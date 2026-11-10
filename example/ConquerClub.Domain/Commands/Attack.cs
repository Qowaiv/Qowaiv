namespace ConquerClub.Domain.Commands;

public sealed record Attack(
    CountryId Attacker,
    CountryId Defender,
    GameId Game,
    int ExpectedVersion)
    : Command(Game, ExpectedVersion);
