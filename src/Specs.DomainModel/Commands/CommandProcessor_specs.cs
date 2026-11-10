namespace Commands.CommandProcessor_specs;

internal interface CommandHandler<TCommand> { Task<Result<string>> Handle(TCommand command); }
internal interface CancelableCommandHandler<TCommand> { Task<Result<string>> Handle(TCommand command, CancellationToken token); }
internal interface SyncCommandHandler<in TCommand> { string Handle(TCommand command); }

public class Cancelation_token
{
    [Test]
    public async Task passed_through_when_applicable()
        => (await new CancelableCommandProcessor(new AsyncCancelableCommandHandler())
        .Send(new EmptyCommand(), token: default))
        .Should().BeValid()
        .And.Value.Should().Be("AsyncCancelableCommandHandler.Handle(token)");

    [Test]
    public async Task ignored_when_not_in_contract()
        => (await new AsyncCommandProcessor(new AsyncCommandHandler())
        .Send(new EmptyCommand(), token: default))
        .Should().BeValid()
        .And.Value.Should().Be("AsyncCommandHandler.Handle()");

    [Test]
    public async Task added_when_method_with_token_is_available()
        => (await new CancelableCommandProcessor(new AsyncCancelableCommandHandler())
        .Send(new EmptyCommand()))
        .Should().BeValid()
        .And.Value.Should().Be("AsyncCancelableCommandHandler.Handle(token)");

    [Test]
    public async Task not_added_when_not_available()
        => (await new AsyncCommandProcessor(new AsyncCommandHandler())
        .Send(new EmptyCommand()))
        .Should().BeValid()
        .And.Value.Should().Be("AsyncCommandHandler.Handle()");
}

public class Throws_when
{
    [Test]
    public void hander_could_not_be_resolved()
    {
        Action send = () => new InvalidReturnTypeProcessor(null!).Send(new EmptyCommand());
        send.Should().Throw<UnresolvedCommandHandler>().WithMessage(
            "The command handler Commands.CommandProcessor_specs.SyncCommandHandler`1[Commands.CommandProcessor_specs.EmptyCommand] could not be resolved.");
    }

    [Test]
    public void method_could_not_be_resolved()
    {
        Action send = () => new InvalidReturnTypeProcessor(new AsyncCommandHandler()).Send(new EmptyCommand());

        send.Should().Throw<UnresolvedCommandHandler>().WithMessage(
            "The command handler method System.Int32 Commands.CommandProcessor_specs.SyncCommandHandler`1[Commands.CommandProcessor_specs.EmptyCommand].Handle(Commands.CommandProcessor_specs.EmptyCommand, token?) could not be resolved.");
    }
}

public class Caches
{
    [Test]
    public async Task supported_command_types()
    {
        var processor = new AsyncCommandProcessor(new AsyncCommandHandler());
        processor.CommandTypes.Should().BeEmpty(because: "not called yet.");
        (await processor.Send(new EmptyCommand())).Should().BeValid();
        processor.CommandTypes.Should().BeEquivalentTo([typeof(EmptyCommand)], because: "empty command is supported.");
    }
}

internal class AsyncCommandProcessor(object handler) : CommandProcessor<Task<Result<string>>>
{
    private readonly object handler = handler;
    protected override Type GenericHandlerType => typeof(CommandHandler<>);
    protected override string HandlerMethod => nameof(CommandHandler<>.Handle);
    protected override object GetHandler(Type handlerType) => handler;
}

internal class CancelableCommandProcessor(object handler) : CommandProcessor<Task<Result<string>>>
{
    private readonly object handler = handler;
    protected override Type GenericHandlerType => typeof(CancelableCommandHandler<>);
    protected override string HandlerMethod => nameof(CancelableCommandHandler<>.Handle);
    protected override object GetHandler(Type handlerType) => handler;
}
internal class SyncCommandProcessor(object handler) : CommandProcessor<string>
{
    private readonly object handler = handler;
    protected override Type GenericHandlerType => typeof(SyncCommandHandler<>);
    protected override string HandlerMethod => nameof(SyncCommandHandler<>.Handle);
    protected override object GetHandler(Type handlerType) => handler;
}

internal class InvalidReturnTypeProcessor(object handler) : CommandProcessor<int>
{
    private readonly object handler = handler;
    protected override Type GenericHandlerType => typeof(SyncCommandHandler<>);
    protected override string HandlerMethod => nameof(SyncCommandHandler<>.Handle);
    protected override object GetHandler(Type handlerType) => handler;
}

[EmptyTestClass]
internal record EmptyCommand();

internal class AsyncCommandHandler : CommandHandler<EmptyCommand>
{
    public Task<Result<string>> Handle(EmptyCommand command) => Result.For("AsyncCommandHandler.Handle()").Async();
}

#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
internal class AsyncCancelableCommandHandler : CancelableCommandHandler<EmptyCommand>
{
    public Task<Result<string>> Handle(EmptyCommand command) => throw new NotSupportedException("Use overload with token");
    public Task<Result<string>> Handle(EmptyCommand command, CancellationToken token) => Result.For("AsyncCancelableCommandHandler.Handle(token)").Async();
}
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
