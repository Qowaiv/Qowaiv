namespace ConquerClub.Domain.Handlers;

public interface CommandHandler<in TCommand>
{
    [Impure]
    Result Handle(TCommand command);
}
