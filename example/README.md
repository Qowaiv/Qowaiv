# Conquer Club 
### Event sourced DDD example

On the [Conquer Club](https://www.conquerclub.com/) website you can play a game
that is inspired by the board game RISK™. In this demo, (most of) the rules of
that game have been implemented using Qowaiv's Aggregate, to give an
impression on how a (not trivial) event sourced DDD solution could look like.

## Always valid
The validness of the game (state) is guarded by a validator that is injected
via the constructor:
``` C#
 public Game(GameId id) : base(id, new GameValidator()) { }
```

The `[GameValidator](ConquerClub.Domain/Validation/GameValidator.cs)` uses
[Qowaiv.Validation.Fluent](https://github.com/Qowaiv/qowaiv-validation), an
extension on [FluentValadation](https://fluentvalidation.net).

``` C#
internal class GameValidator : ModelValidator<Game>
{
    public GameValidator()
    {
        RuleFor(g => g.Settings).Required();
        RuleFor(g => g.Countries).Required();
        RuleFor(g => g.Phase).IsInEnum().NotEmpty();
        RuleFor(g => g.Round).LessThanOrEqualTo(game => game.Settings?.RoundLimit);
        RuleFor(g => g.ActivePlayer).NotEmptyOrUnknown();

        RuleForEach(g => g.Countries).SetValidator(new CountryValidator());
    }
}
```

So, for any action that changes the game state, all these rules are checked. If
any of them fail, the change is not applied.

## Pre-conditions
Some conditions should be checked before changing the state. There are two major
reasons to that (instead of defining the constraints in a validator):
1. the constraint can be checked afterwards.
2. preventing code from crashing before the state is rejected.
```C#
Result<Game> Deploy(CountryId country, Army army) =>
    MustBeInPhase(GamePhase.Deploy)
    | (g => g.Must.BeActivePlayer(army.Owner))
    | (g => g.Must.Exist(country))
    | (g => g.Must.BeOwnedBy(country, army.Owner))
    | (g => g.Must.NotExeedArmyBuffer(army))
    | (g => g.ApplyEvent(new Deployed(country, army)));
```
The `Qowaiv.Validation.Abstractions.Result<T>` allows us to use a | operator
(as a short for `.Act<T>(Func<T, Result<T>>)`). In short, if any of these lines
returns a result that is not (longer) valid, the execution is ended, and the 
error(s) are returned.

## Conditional events
To keep the replay of events as straightforward as possible, it can be a good
thing to conditionally apply (different) events based on the current state.

In the code below, a successful attack is represented by a `Conquered` event,
and a unsuccessful one (the country was not conquered) by an `Attacked` event.

``` C#
Result<Game> Attack(
    CountryId attacker,
    CountryId defender,
    AttackResult result)
    => Apply(Events
        .If(result.IsSuccess)
            .Then(() => new Conquered(attacker, defender))
        .Else(() => new Attacked(attacker, defender, result)));
```

With this distinction, both `When` replay methods are simple:

``` C#
void When(Attacked @event)
{
    From = Countries.ById(@event.Attacker);
    To = Countries.ById(@event.Defender);
    From.Army = @event.Result.Attacker;
    To.Army = @event.Result.Defender;
}
void When(Conquered @event)
{
    From = Countries.ById(@event.Attacker);
    To = Countries.ById(@event.Defender);
    ArmyBuffer = From.Army - 2;
    From.Army = From.Army.Owner.Army(1);
    To.Army = From.Army.Owner.Army(1);
    Phase = GamePhase.Advance;
}
```
