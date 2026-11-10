namespace ConquerClub.Domain.Commands;

public sealed record Reinforce(
    CountryId From,
    CountryId To,
    Army Army,
    GameId Game,
    int ExpectedVersion)
    : Command(Game, ExpectedVersion);
