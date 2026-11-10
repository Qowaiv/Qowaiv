# Qowaiv Validation Guarding

Fluent pre-condition guards for domain logic. Uses the `.Must()` extension
method to create guard expressions that return `Result<TSubject>` to
communicate the outcome.

## Must

The `.Must()` extension creates a `Must<TSubject>` guard builder on any object.

### Be / NotBe

Guard that a condition is true or false:

``` C#
Result<Game> result = game.Must().Be(
    game.Phase == GamePhase.Started,
    "Game must be started");

Result<Game> result = game.Must().NotBe(
    game.Phase == GamePhase.Finished,
    "Game must not be finished");
```

Returns a valid `Result<Game>` if the condition is met, otherwise an invalid
result with the specified error message.

### Exist

Guard that a related entity exists by ID. Useful in command handlers:

``` C#
Result<PlaceOrder> result = command.Must().Exist(
    command.CustomerId,
    (cmd, id) => customerRepository.Find(id));
```

Returns a valid result if the entity is found, otherwise an invalid result
with an `EntityNotFound` message.

You can also provide a custom error message:

``` C#
Result<PlaceOrder> result = command.Must().Exist(
    command.CustomerId,
    (cmd, id) => customerRepository.Find(id),
    ValidationMessage.Error("Customer not found"));
```

## Custom guards

Extend `Must<TSubject>` with custom extension methods for domain-specific
guarding:

``` C#
public static class MustOrderExtensions
{
    public static Result<Order> MustBePlaced(this Order order)
        => order.Must().Be(
            order.Status == OrderStatus.Placed,
            "Order must be in Placed status");
}
```
