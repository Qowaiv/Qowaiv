namespace ConquerClub.Domain.Commands;

public sealed record Resign(GameId Game, int ExpectedVersion)
    : Command(Game, ExpectedVersion);
