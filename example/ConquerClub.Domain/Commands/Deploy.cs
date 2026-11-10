namespace ConquerClub.Domain.Commands;

public sealed record Deploy(
    CountryId Country,
    Army Army,
    GameId Game,
    int ExpectedVersion)
    : Command(Game, ExpectedVersion);
