#pragma warning disable CA1822 // Mark members as static, methods are used for tests and need to be instance-based
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static

using Qowaiv.DomainModel.Reflection;

namespace Relection.Expression_compiling_event_dispatcher_specs;

public class Support_when_methods
{
    [Test]
    public void With_single_argument()
        => new ExpressionCompilingEventDispatcher<WhenWithSingleArgument>(new())
            .SupportedEventTypes.Should().ContainSingle();

    [Test]
    public void With_return_statement()
        => new ExpressionCompilingEventDispatcher<WhenWithReturnStatement>(new())
            .SupportedEventTypes.Should().ContainSingle();
    
    [Test]
    public void Non_public_methods()
    {
        var dispatcher = new ExpressionCompilingEventDispatcher<WhenWithSingleArgument>(new WhenWithSingleArgument());
        dispatcher.Invoking(d => d.When(new DummyEvent()))
            .Should().Throw<Detected>();
    }

    internal class WhenWithSingleArgument
    {
        internal void When(DummyEvent _) => throw new Detected();
    }

    internal class WhenWithReturnStatement
    {
        internal int When(DummyEvent _) => throw new Detected();
    }

    [EmptyTestClass]
    internal class DummyEvent { }

    public class Detected : Exception { }
}

public class Not_supported_when_methods
{
    [Test]
    public void Object_argument()
        => new ExpressionCompilingEventDispatcher<WhenWithObjectParameter>(new())
            .SupportedEventTypes.Should().BeEmpty();

    
    [Test]
    public void Primitive_arguments()
         => new ExpressionCompilingEventDispatcher<WhenWithPrimitiveParameters>(new())
            .SupportedEventTypes.Should().BeEmpty();

    [Test]
    public void Enum_arguments()
        => new ExpressionCompilingEventDispatcher<WhenWithEnumParameters>(new())
            .SupportedEventTypes.Should().BeEmpty();

    [Test]
    public void No_arguments()
        => new ExpressionCompilingEventDispatcher<WhenNoParameterEvent>(new())
            .SupportedEventTypes.Should().BeEmpty();

    [Test]
    public void Multiple_arguments()
        => new ExpressionCompilingEventDispatcher<WhenWithMultipleParameters>(new())
            .SupportedEventTypes.Should().BeEmpty();


    [Test]
    public void Ignores_unknown_event()
    {
        var dispatcher = new ExpressionCompilingEventDispatcher<WhenWithMultipleParameters>(new WhenWithMultipleParameters());
        dispatcher.SupportedEventTypes.Should().BeEmpty();

        dispatcher.Invoking(d => d.When(new NotRegisteredEvent()))
            .Should().NotThrow();
    }

    [Test]
    public void Ignores_null_event()
    {
        var dispatcher = new ExpressionCompilingEventDispatcher<WhenWithMultipleParameters>(new WhenWithMultipleParameters());
        dispatcher.SupportedEventTypes.Should().BeEmpty();

        dispatcher.Invoking(d => d.When(null))
            .Should().NotThrow();
    }

    internal class WhenWithMultipleParameters
    {
        internal void When(DummyEvent @event, int other) => throw new NotSupportedException($"{@event} + {other}.");
    }

    internal class WhenNoParameterEvent
    {
        internal void When() => throw new NotSupportedException();
    }

    internal class WhenWithObjectParameter
    {
        internal void When(object @event) => throw new NotSupportedException($"Not a valid event: {@event}.");
    }
    internal class WhenWithPrimitiveParameters
    {
        internal void When(int @event) => throw new NotSupportedException($"Not a valid event: {@event}.");
        internal void When(long @event) => throw new NotSupportedException($"Not a valid event: {@event}.");
        internal void When(bool @event) => throw new NotSupportedException($"Not a valid event: {@event}.");
    }
    internal class WhenWithEnumParameters
    {
        internal void When(Base64FormattingOptions @event) => throw new NotSupportedException($"Not a valid event: {@event}.");
        internal void When(EnvironmentVariableTarget @event) => throw new NotSupportedException($"Not a valid event: {@event}.");
    }

    [EmptyTestClass]
    internal class DummyEvent { }

    [EmptyTestClass]
    internal class NotRegisteredEvent { }

    public class Detected : Exception { }
}
