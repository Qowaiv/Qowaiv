namespace ConquerClub.Domain;

public sealed partial class Game : Aggregate<Game, GameId>
{
    [Pure]
    public static Result<Game> Start(Start start, RandomSource rnd)
        => new Game(start.Game).ApplyEvents(
            new MapInitialized(
                Continents: [.. start.Continents.Select(c => new ContinentInitialized(c.Name, c.Bonus, [.. c.Territories]))],
                Countries: [.. start.Countries.Select(c => new CountryInitialized(c.Name, [.. c.Borders]))]),
            new SettingsInitialized(start.Players, start.RoundLimit, false),
            new ArmiesInitialized([.. RndArmies(start.Players, start.Countries.Length, rnd)]))
        | (g => g.ApplyEvent(g.StartTurn(Player.P1)));

    [Pure]
    public Result<Game> Deploy(CountryId country, Army army)
        => Must.BeInPhase(GamePhase.Deploy)
        | (g => g.Must.BeActivePlayer(army.Owner))
        | (g => g.Must.Exist(country))
        | (g => g.Must.BeOwnedBy(country, army.Owner))
        | (g => g.Must.NotExceedArmyBuffer(army))
        | (g => g.ApplyEvent(new Deployed(country, army)));

    [Pure]
    public Result<Game> Attack(
        CountryId attacker,
        CountryId defender,
        RandomSource rnd)
        => Must.BeInPhase(GamePhase.Attack)
        | (g => g.Must.Exist(attacker))
        | (g => g.Must.Exist(defender))
        | (g => g.Must.BeOwnedBy(attacker, ActivePlayer))
        | (g => g.Must.NotBeOwnedBy(defender, ActivePlayer))
        | (g => g.Must.BeReachable(defender, by: attacker))
        | (g => g.Must.HaveArmiesToAttack(attacker))
        | (g => g.Attack(attacker, defender, Dice
            .Attack(
                Countries.ById(attacker).Army,
                Countries.ById(defender).Army,
                rnd)));

    [Pure]
    public Result<Game> AutoAttack(
        CountryId attacker,
        CountryId defender,
        RandomSource rnd)
        => Must.BeInPhase(GamePhase.Attack)
        | (g => g.Must.Exist(attacker))
        | (g => g.Must.Exist(defender))
        | (g => g.Must.BeOwnedBy(attacker, ActivePlayer))
        | (g => g.Must.NotBeOwnedBy(defender, ActivePlayer))
        | (g => g.Must.BeReachable(defender, by: attacker))
        | (g => g.Must.HaveArmiesToAttack(attacker))
        | (g => g.Attack(attacker, defender, Dice
           .AutoAttack(
               Countries.ById(attacker).Army,
               Countries.ById(defender).Army,
               rnd)));

    [Pure]
    private Result<Game> Attack(
        CountryId attacker,
        CountryId defender,
        AttackResult result)
    {
        var events = Events.Add(result.IsSuccess
            ? new Conquered(attacker, defender)
            : new Attacked(attacker, defender, result));

        return result.IsSuccess && ConquerCountryWillKillPlayer(defender) && KillPlayerWillFinishGame
            ? Apply(events.Add(new Finished()))
            : Apply(events);
    }

    [Pure]
    public Result<Game> Advance(Army to)
        => Must.BeInPhase(GamePhase.Advance)
        | (g => g.Must.BeActivePlayer(to.Owner))
        | (g => g.Must.BeOwnedBy(To!.Id, to.Owner))
        | (g => g.Must.NotExceedArmyBuffer(to))
        | (g => g.ApplyEvent(new Advanced(to)));

    [Pure]
    public Result<Game> Reinforce(CountryId from, CountryId to, Army army)
        => Must.BeInPhase(GamePhase.Reinforce)
        | (g => g.Must.Exist(from))
        | (g => g.Must.Exist(to))
        | (g => g.Must.BeOwnedBy(from, army.Owner))
        | (g => g.Must.BeOwnedBy(to, army.Owner))
        | (g => g.Must.BeReachable(to, by: from))
        | (g => g.ApplyEvent(new Reinforced(from, to, army)));

    [Pure]
    public Result<Game> Resign()
        => Apply(Events
            .Add(new Resigned(ActivePlayer))
            .If(KillPlayerWillFinishGame)
                .Then(() => Events.Add(new Finished()))
            .Else(() => StartTurn(NextPlayer)));

    [Pure]
    private bool ConquerCountryWillKillPlayer(CountryId country) => Countries.Count(c => c.Owner == Countries.ById(country).Owner) == 1;
    
    private bool KillPlayerWillFinishGame => Countries.ActivePlayers().Count() == 2;

    internal void When(MapInitialized @event)
    {
        Continents = [.. @event.Continents.Select((c, id) => new Continent(ContinentId.Create(id), c.Name, c.Bonus))];

        Countries = [.. @event.Countries.Select((c, id) => new Country(CountryId.Create(id), c.Name))];

        LinkNeighborCountries(@event.Countries);
        LinkContinentsToCountries(@event.Continents);
        LinkCountriesToContent();
    }

    internal void When(SettingsInitialized @event)
        => Settings = new Settings(@event.Players, @event.RoundLimit, @event.FogOfWar);

    internal void When(ArmiesInitialized @event)
    {
        foreach (var data in @event.Armies.Select((army, index) => new
        {
            Army = army,
            Id = CountryId.Create(index)
        }))
        {
            Countries.ById(data.Id).Army = data.Army;
        }
        Phase = GamePhase.Deploy;
    }

    internal void When(ArmyInitiated @event)
        => Countries.ById(@event.Country).Army = @event.Army;

    internal void When(TurnStarted @event)
    {
        ActivePlayer = @event.Deployments.Owner;
        ArmyBuffer = @event.Deployments;
        Phase = GamePhase.Deploy;
    }

    internal void When(Deployed @event)
    {
        Countries.ById(@event.Country).Army += @event.Army;
        ArmyBuffer -= @event.Army;
        Phase = ArmyBuffer.Size > 0 ? GamePhase.Deploy : GamePhase.Attack;
    }

    internal void When(Attacked @event)
    {
        From = Countries.ById(@event.Attacker);
        To = Countries.ById(@event.Defender);
        From.Army = @event.Result.Attacker;
        To.Army = @event.Result.Defender;
    }

    internal void When(Conquered @event)
    {
        From = Countries.ById(@event.Attacker);
        To = Countries.ById(@event.Defender);
        ArmyBuffer = From.Army - 2;
        From.Army = From.Army.Owner.Army(1);
        To.Army = From.Army.Owner.Army(1);
        Phase = GamePhase.Advance;
    }

    internal void When(Advanced @event)
    {
        From!.Army += ArmyBuffer - @event.To;
        To!.Army += @event.To;

        ArmyBuffer = Army.None;
        From = null;
        To = null;
        Phase = GamePhase.Attack;
    }

    internal void When(Reinforced @event)
    {
        Countries.ById(@event.From).Army -= @event.Army;
        Countries.ById(@event.To).Army += @event.Army;
    }

    internal void When(Resigned @event)
    {
        foreach (var region in Countries.Where(r => r.Owner == @event.Player))
        {
            region.Army = Player.Neutral.Army(region.Army.Size);
        }
    }

    internal void When(Finished _) => Phase = GamePhase.Finished;

    private void LinkNeighborCountries(IEnumerable<CountryInitialized> countries)
    {
        foreach (var data in countries.Select((c, id) => new
        {
            Country = CountryId.Create(id),
            c.Borders,
        }))
        {
            var country = Countries.ById(data.Country);
            country.Borders = [.. data.Borders.Select(Countries.ById)];
        }
    }

    private void LinkContinentsToCountries(IEnumerable<ContinentInitialized> continents)
    {
        foreach (var data in continents.Select((c, id) => new { Continent = ContinentId.Create(id), Countries = c.Territories }))
        {
            var continent = Continents.ById(data.Continent);

            foreach (var cid in data.Countries)
            {
                var country = Countries.ById(cid);
                country.Continent = continent;
            }
        }
    }

    private void LinkCountriesToContent()
    {
        foreach (var continent in Continents)
        {
            continent.Countries = [.. Countries.Where(c => c.Continent == continent)];
        }
    }

    [Pure]
    private TurnStarted StartTurn(Player player)
    {
        var countries = Countries.Count(c => c.Owner == player);
        var deploy = (Math.Max(3, countries / 3));
        deploy += Continents.Where(c => c.Owner == player).Sum(c => c.Bonus);
        return new TurnStarted(Deployments: player.Army(deploy));
    }

    [Pure]
    private static IEnumerable<Army> RndArmies(int players, int countries, RandomSource rnd)
    {
        var perCountry = countries / Math.Min(players, 3);

        return Enumerable
            .Range(0, countries)
            .Select(index =>
            {
                var cid = 1 + (index / perCountry);
                return cid == 3 && players == 2 || cid > players ? 0 : cid;
            })
            .Select(id => id == 0 ? Player.Neutral : new Player((byte)id))
            .Select(player => player.Army(3))
            .OrderBy(army => rnd.Next());
    }
}
