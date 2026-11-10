namespace ConquerClub.Domain.Commands;

public sealed record Start(
    Country[] Countries,
    Continent[] Continents,
    int RoundLimit,
    int Players,
    GameId Game)
    : Command(Game, 0);

public sealed record Country(string Name, CountryId[] Borders);

public sealed record Continent(string Name, int Bonus, CountryId[] Territories);
