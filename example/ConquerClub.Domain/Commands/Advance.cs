namespace ConquerClub.Domain.Commands;

public sealed record Advance(Army To, GameId Game, int ExpectedVersion)
    : Command(Game, ExpectedVersion);
