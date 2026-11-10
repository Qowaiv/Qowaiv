namespace ConquerClub.Domain.Commands;

public abstract record Command(GameId Game, int ExpectedVersion);
