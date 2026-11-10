using Qowaiv.Validation.Messages;

namespace ConquerClub.Domain;

public static class CountryExtensions
{
    [Pure]
    public static Country? TryById(this IEnumerable<Country> countries, CountryId id)
        => countries.FirstOrDefault(c => c.Id == id);

    [Pure]
    public static Country ById(this IEnumerable<Country> countries, CountryId id)
        => countries.TryById(id)
        ?? throw EntityNotFound.ForId(id);

    [Pure]
    public static IEnumerable<Player> ActivePlayers(this IEnumerable<Country> countries)
        => countries.Select(c => c.Owner)
        .Distinct()
        .Where(p => p != Player.Neutral)
        .OrderBy(p => p);
}
