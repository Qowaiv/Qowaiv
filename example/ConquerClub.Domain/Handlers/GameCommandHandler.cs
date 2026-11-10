using Qowaiv.Validation.Guarding;

namespace ConquerClub.Domain.Handlers;

public class GameCommandHandler(
    RandomSource rnd,
    Func<GameId, Result<Game>> load,
    Func<Game, Result> save) :

    CommandHandler<Start>,
    CommandHandler<Deploy>,
    CommandHandler<AutoAttack>,
    CommandHandler<Attack>,
    CommandHandler<Advance>,
    CommandHandler<Reinforce>,
    CommandHandler<Resign>
{
    protected RandomSource Rnd { get; } = rnd;
    public Func<GameId, Result<Game>> Load { get; } = load;
    public Func<Game, Result> Save { get; } = save;

    [Impure]
    public Result Handle(Start command)
        => Game.Start(command, Rnd)
            | (g => Save(g));

    [Impure]
    public Result Handle(Deploy command)
         => ExecuteAndSave(command, g => g.Deploy(
            command.Country,
            command.Army));

    [Impure]
    public Result Handle(AutoAttack command)
        => ExecuteAndSave(command, g => g.AutoAttack(
            command.Attacker,
            command.Defender,
            Rnd));

    [Impure]
    public Result Handle(Attack command)
        => ExecuteAndSave(command, g => g.Attack(
            command.Attacker,
            command.Defender,
            Rnd));

    [Impure]
    public Result Handle(Advance command)
        => ExecuteAndSave(command, g => g.Advance(command.To));

    [Impure]
    public Result Handle(Reinforce command)
        => ExecuteAndSave(command, g => g.Reinforce(
            command.From,
            command.To,
            command.Army));

    [Impure]
    public Result Handle(Resign command)
        => ExecuteAndSave(command, g => g.Resign());

    [Impure]
    private Result ExecuteAndSave<TCommand>(TCommand command, Func<Game, Result<Game>> act) where TCommand : Command
        => Load(command.Game)
        | (g => g.Must().HaveVersion(command.ExpectedVersion))
        | (g => act(g))
        | (g => Save(g));
}
