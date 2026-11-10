# Qowaiv Domain Model
Qowaiv Domain Model is a library containing the (abstract) building blocks to
set up a Domain-Driven application.

## Event Sourcing
Within Qowaiv Domain Model, the choice has been made to only support DDD via
Event Sourcing. In short: Event Sourcing describes the state of an aggregate
(root) by the (domain) events that occurred within the domain. Getting the
current state of an aggregate can always be achieved by replaying these
events. 

## Always Valid
The aggregate should always be valid according to the boundaries of its domain.
There are multiple ways to achieve this, but within Qowaiv Domain Model this is
guaranteed via an implicitly triggered validator.

When a public method is called that would lead to a new aggregate state, the
events describing the change are only added to the event buffer
associated with the aggregate if the new state is valid according to the
rules specified in the validator.

## Aggregate
An aggregate is a cluster of associated objects that we treat as a unit for the
purpose of data changes. When implementing an aggregate there are several steps
that have to be taken.

First, an aggregate should have a corresponding
[IValidator<TAggragate>](../validation/Validator.md)
implementation of choice. This implementation will safeguard any post conditions
on the aggregate.

Secondly, the actual class representing the aggregate should be created. This class
should inherit from one of two possible base classes:

- `Aggregate<TAggregate>`
- `Aggregate<TAggregate, TId>`

`Aggregate<TAggregate>` is a low level base class that provides a framework
for handling the application of events. It has no event store of its own. Implementations
of this class should override the `protected abstract void AddEventsToBuffer(params object[] events)`
method to achieve persistence of events.

The second option, `Aggregate<TAggregate, TId>`, inherits from
`Aggregate<TAggregate>`. It has built-in identity support and systems for
storing events in the integrated `EventBuffer<TId>`. Events that should be
persisted are added to this buffer, and it can return both the committed as
well as the uncommitted events it contains.

### Immutability
As immutability comes with tons of benefits in DDD scenarios, the
`Aggregate<TAggregate>` is designed to be immutable; that is,
if you apply all your changes via the `Apply`, and `ApplyEvents` methods
(as you should), it will create an updated copy that represents the new state,
leaving the initial instance unchanged.

It is possible to override the `TAggregate Clone()` method, and implement this
behavior, for instance to use one of the many _deep clone_ packages to do the
trick. This might be beneficial for aggregate roots that have a lot of events
(think thousands, or more).

## Event dispatcher
Each aggregate has an event dispatcher that by default uses compiled
expressions to execute matching non-public `When(@event)` methods. This is
extremely fast, but if desired, by implementing your own `EventDispatcher`, the
behavior can be changed.

## Example 1
A (simplified) real life example of a financial entry, using `Aggregate<TAggregate, TId>`:
``` C#
public sealed class FinancialEntry : Aggregate<FinancialEntry, Guid>
{
    // Note that FinancialEntryValidator is passed in for validation purposes.
    // See the implementation of this class below.
    public FinancialEntry(Guid id) : base(id, new FinancialEntryValidator()) { }

     public IReadOnlyCollection<EntryLine> Lines => enties;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<EntryLine> enties = new List<EntryLine>();

    public Result<FinancialEntry> AddLines(params FinancialEntryLine[] lines)
    {
        return ApplyEvents(lines.Select(line => new EntryLineAdded
        {
            GlAccount = line.GlAccount,
            Amount = line.Amount,
            Date = line.Date,
            Description = line.Description,
            AccountId = line.AccountId,
        }).ToArray());
    }
    
    // The method that is triggered when an EntryLineAdded
    // is processed by the ApplyEvents method called from AddLines above.
    internal void When(EntryLineAdded @event)
    {
        enties.Add(new EntryLine
        {
            GlAccount = @event.GlAccount,
            Date = @event.Date,
            Amount = @event.Amount,
            Description = @event.Description,
            AccountId = @event.AccountId,
        });
    }
}
```

And the implementation of the validator for `FinancialEntry` might look like this:

``` C#
public class FinancialEntryValidator : FluentModelValidator<FinancialEntry>
{
    public FinancialEntryValidator()
    {
        RuleFor(entry => entry.Lines).NotEmpty().Custom(BeBalanced);
        RuleFor(entry => entry.Report).NotEmpty();
        RuleForEach(entry => entry.Lines).SetValidator(new EntryLineValidator());
    }

    private void BeBalanced(IReadOnlyCollection<EntryLine> lines, CustomContext context)
    {
        if (lines.Select(line => line.Amount.Currency).Distinct().Count() > 1)
        {
            context.AddFailure(nameof(FinancialEntry.Lines), "Multiple currencies.");
            // return as we can not sum the amounts.
            return;
        }
        var sum = lines.Select(line => line.Amount).Sum();
        if (sum.Currency.IsEmptyOrUnknown())
        {
            context.AddFailure(nameof(FinancialEntry.Lines), "Unknown currency.");
        }

        if (sum.Amount != Amount.Zero)
        {
            context.AddFailure(nameof(FinancialEntry.Lines), "The lines are note balanced.");
        }
    }
}
```

You could argue that some of the rules specified in the validator should be
handled as part of the *anti-corruption layer*, but that is another topic. The
point is that by defining those constraints, you can no longer add any entry
line with any other currency than the lines already added, nor add any financial
entries that are not balanced. Both extremely important within this particular
domain.

Note that the decision to throw an exception, or to deal with a
`Result<TAggregate>` gradually, is up to the developer.

## Example 2
An advanced example (implementing ConquerClub) can be found at [here](ConquerClub.md).

## Conditional Events
When applying changes to an aggregate, you might want to apply a different type
and number of events, based on the current state of the aggregate. This use-case
is supported in the following way:

``` C#
public Result<Game> Attack(Country attacker, Country defender, AttackResult result)
=> Apply(Events
    .If(result.IsSuccess)
        .Then(() => new Conquered
        {
            From = result.Attacker,
            To = result.Defender,
            Armies = result.Attacker,
        })
    .Else(() => new Attacked
    {
        Attacker = attacker,
        Defender = defender,
        Result = result,
    })
    .If(result.IsSuccess && Countries(defender).Single())
        .Then(() => new PlayerEliminated 
        {
            Player = Countries(defender).Owner 
        }));
```

Alternatively:

``` C#
public Result<Game> Attack(Country attacker, Country defender, AttackResult result)
{
    var events = Events.

    if (result.IsSuccess)
    {
        events = events.Add(new Conquered
        {
            From = result.Attacker,
            To = result.Defender,
            Armies = result.Attacker,
        });
    }
    else
    {
        events = events.Add(new Attacked
        {
            Attacker = attacker,
            Defender = defender,
            Result = result,
        });
    }

    if (result.IsSuccess && Countries(defender).Single())
    {
        events = events.Add(new PlayerEliminated 
        {
            Player = Countries(defender).Owner 
        }));
    }

    return Apply(events);
}
```

## Event Buffer
The `EventBuffer<TId>`, as used by `Aggregate<TAggregate, TId>` is an
immutable collection with the following API:

``` C#
// Creation
var id = NewId();
var buffer = EventBuffer.Empty(id, version: 5); // version optional.
var stored = EventBuffer.FromStorage(id, version: 5, storedEvents, (e) => Convert(e));

// Extending, returning a new instance.
var updated = buffer.Add(events); // excepts arrays, enumerables or a single event.

// Export for storage
var export = buffer.SelectUncommitted((id, verion, e) => Export(id, version, e));

// After successful export
var updated = buffer.MarkAllAsCommitted();
```

## Command Processor
An approach often used when applying event sourcing is the [command pattern](https://en.wikipedia.org/wiki/Command_pattern);
every change on the domain(s) is triggered by a command, which is handled by 
a single command handler.

As it should be irrelevant to the sender of the command which handler handles
the command, a command processor can be useful to do just that: ensure that
the registered handler handles the command at hand.

The (abstract) `Qowaiv.DomainModel.Commands.CommandProcessor<TReturnType>`
orchestrates command handlers to command (types). To reduce the usage of reflection,
the actual call is a (one-time) compiled expression, stored in a dictionary. It
can deal with any (except `void`, including both sync and async) return type,
and any (generic) command handler interface of preference.

If a method with an additional `System.Threading.CancelationToken` parameter is
available, that method is preferred. If needed, an default token is added, or - if
provided but not available - ignored.

A typical implementation, using Microsoft's `System.IServiceProvider`,
looks like this:

``` C#
class CommandProcessor : CommandProcessor<Task<Result>>
{
    CommandProcessor(IServiceProvider provider) => Provider = provider;
    protected override Type GenericHandlerType => typeof(CommandHandler<>);
    protected override string HandlerMethod => nameof(CommandHandler<object>.Handle);
    protected override object GetHandler(Type handlerType) => Provider.GetService(handlerType);
    private readonly IServiceProvider Provider;
}
```
