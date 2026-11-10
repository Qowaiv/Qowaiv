using Qowaiv.Customization;

namespace ConquerClub.Domain;

[Id<StringIdBehavior, string>]
public readonly partial struct GameId;

[Id<Int32IdBehavior, int>]
public readonly partial struct ContinentId;

[Id<Int32IdBehavior, int>]
public readonly partial struct CountryId;
