namespace ConquerClub.Domain;

[Mutable]
public sealed partial class Game(GameId id) : Aggregate<Game, GameId>(id, new GameValidator())
{
    public Game() : this(GameId.Next()) { }

    public Settings Settings { get; private set; } = new(0, 0, false);

    public IReadOnlyList<Continent> Continents { get; private set; } = [];

    public IReadOnlyList<Country> Countries { get; private set; } = [];

    /// <summary>Gets the current round.</summary>
    public int Round { get; private set; } = 1;

    /// <summary>Gets the phase the round is in.</summary>
    public GamePhase Phase { get; private set; }

    public IEnumerable<Player> ActivePlayers => Countries.ActivePlayers();

    /// <summary>Gets or sets the active player.</summary>
    public Player ActivePlayer { get; private set; } = Player.P1;

    public Player NextPlayer
    {
        get
        {
            var players = ActivePlayers.ToList();
            var active = players.IndexOf(ActivePlayer);
            return players[(active + 1) % players.Count];
        }
    }

    /// <summary>Gets or sets the last from country.</summary>
    public Country? From { get; private set; }

    /// <summary>Gets or sets the last to country.</summary>
    public Country? To { get; private set; }

    /// <summary>Gets or sets the armies to <see cref="Commands.Deploy"/> and <see cref="Commands.Advance"/>.</summary>
    public Army ArmyBuffer { get; private set; }
}
