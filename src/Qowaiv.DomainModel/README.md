# Qowaiv Domain Model
`Qowaiv.DomainModel` is a package that provides building blocks for applying
Domain-Driven Design. The starting-point of the modeled entities is that the
concept of *always valid* is achieved by implicitly triggered validation.

## Aggregate Roots & Entities
Aggregate Roots and Entities are complex DDD objects that are identifiable by
their ID. Qowaiv provides a generic base class for both types. It comes with
a mechanism to ensure always valid by a Validator.

``` C#
public sealed class Person : Entity<Person>
{
    internal Person(ChangeTracker tracker) : base(tracker) { }

    public Date DateOfBirth
    {
        get => GetProperty<Date>();
        internal set => SetProperty(value);
    }
    public decimal Length
    {
        get => GetProperty<Date>();
        internal set => SetProperty(value);
    }
}

public sealed class Family : AggregateRoot<Family>
{
    public Family(): base(new FamilyValidator()) 
    { 
        Members = new ChildCollection<Person>(Tracker);
    }

    public ChildCollection<Person> Members { get; }

    public void AddMember1(Date dateOfBirth, decimal length)
    {
        TrackChanges((m) =>
        {
            m.Members.Add(
            new Person(m.Tracker) 
            {
                DateOfBirth = dateOfBirth,
                Length = length,
            });
        }).ThrowIfInvalid();
    }

    public Result<Family> AddMember2(Date dateOfBirth, decimal length)
    {
        return TrackChanges((m) =>
        {
            m.Members.Add(
            new Person(m.Tracker) 
            {
                DateOfBirth = dateOfBirth,
                Length = length,
            });
        });
    }
}

```

And a validator might look like this:

``` C#
public FamilyValidator : FluentModelValidator<Family>
{
    public FamilyValidator()
    {
        RuleFor(m => m.Members).NotEmpty();
        RuleForEach(m => m.Members).ChildRules(member => 
        {
            member.RuleFor(m => m.DateOfBirth)
                .NotEmpty();
            member.RuleFor(m => m.Length)
                .Must(l => l > 0.5)
                .WithMessage("Short people got no reason to live");
        });
    }
}
```

You could argue that some of the rules specified in the validator should be handled
as part of the *anti-corruption layer*, but that is another topic. The point is
that by defining those constraints, you can no longer add any item to `Family.Members`
that is short than 50 cms, or has `default` as `DateOfBirth`, nor could you remove
a member if `Family.Members` contains a single item.

If you want to throw an exception, or deal with a `Result<TAggegate>` is up to
the developer.

### Event Sourcing
Event Sourcing (often as part of Command and Query Responsibility Segregation aka CQRS)
is a concept in which changes on the domain are described (and communicated) as
messages/events. Qowaiv has support for such scenarios.

``` C#
public sealed class Family : EventSourcedAggregateRoot<Family>
{
    public Family(): base(new FamilyValidator()) 
    { 
        Members = new ChildCollection<Person>(Tracker);
    }

    public ChildCollection<Person> Members { get; }

    public Result<Family> AddMember(Date dateOfBirth, decimal length)
    {
        // Works on a name based convention
        return ApplyChange(new FamilyMemberAdded { DateOfBirth = dateOfBirth, Length = Length });
    }
    
    // Matches the name based convention.
    private void Apply(FamilyMemberAdded @event)
    {
        Members.Add(
        new Person(m.Tracker) 
        {
            DateOfBirth = @event.DateOfBirth,
            Length = @event.Length,
        });
    }
}
```

Interacting with an Event Source based aggregate root is commonly done via a
command handler. Whatever mechanism (and with which technology) you choice,
saving the new events and publishing them, is done here.

``` C#
public class CommandHandler
{
    public Result<Family> Handle(AddFamilyMember cmd)
    {
        var family = repository.ById(cmd.Id);
        var result = family.AddMember(cmd.DateOfBirth, cmd.Length);
        repository.Save(family);
        eventbus.Publish(family.EventStream.GetUncommitted());        
        family.EventStream.MarkAllAsCommitted();
        return result;
    }
}
```

The main idea is that if you should have an `Apply(EventyType @event)`
method that changes the state based on the data in the event. This one is called
via `ApplyChange(object @event)`. If applying the event to the aggregate 
lead to an invalid state, the changes are not applied, and the event is not
added to the event stream.

## Complex Value Objects
Complex Value Objects - opposed to Single Value Objects (SVO's), those who can
be represented by a single scalar - are represented by multiple (immutable)
scalars. Qowaiv provides a generic base class helping to achieve that.

``` C#
public sealed class Address : ValueObject<Address>
{
    public Address(string street, HouseNumber number, PostalCode code, Country country)
    {
        Street = street;
        HouseNumber = number;
        PostalCode = code;
        Country = country;
    }

    public string Street { get; }
    public HouseNumber HouseNumber { get; }
    public PostalCode PostalCode { get; }
    public Country Country { get; }

    public override bool Equals(AddressValueObject other)
    {
        if (AreSame(other)) { return true; }
        return NotNull(other)
            && Street == other.Street
            && HouseNumber == other.HouseNumber
            && PostalCode == other.PostalCode
            && Country == other.Country;
    }

    protected override int Hash()
    {
        return QowaivHash.HashObject(Street)
            ^ QowaivHash.Hash(HouseNumber, 3)
            ^ QowaivHash.Hash(PostalCode, 5)
            ^ QowaivHash.Hash(Country, 13);
    }   
}
```
