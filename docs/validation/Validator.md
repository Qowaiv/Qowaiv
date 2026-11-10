# Qowaiv Validation IValidator&lt;T&gt;

The `Qowaiv.Validation.Abstractions.IValidator<TModel>` interface decouples
model validation from the validation framework of choice. It validates a model
and communicates the outcome as a [`Result<TModel>`](Result.md):

``` C#
public interface IValidator<TModel>
{
    Result<TModel> Validate(TModel model);
}
```

## Implementations

Multiple packages ship an implementation:

| Package                                       | Implementation                    |
| --------------------------------------------- | --------------------------------- |
| [DataAnnotations](data-annotation/README.md)  | `AnnotatedModelValidator<TModel>` |
| [Fluent](Fluent.md)                           | `ModelValidator<TModel>`          |
| [Xml](Xsd.md)                                 | `SchemaValidator<TModel>`         |

[Qowaiv.DomainModel](../domain-model/README.md) does not ship a validator
itself, but its `Aggregate<TAggregate>` takes one to guarantee that the
aggregate stays valid.

As the interface is deliberately small, wrapping a validator of another
library, or writing rules in plain C#, is straightforward:

``` C#
public sealed class AdultValidator : IValidator<Customer>
{
    public Result<Customer> Validate(Customer model)
        => model.Age >= 18
        ? Result.For(model)
        : Result.For(model, ValidationMessage.Error("Customers must be adults", nameof(Customer.Age)));
}
```

## Validator

The static `Validator` class helps to create validators.

### Composite

Multiple validators can be combined into a composite. All validators run, and
their messages are merged. The result is only valid when none of the
validators reports an error:

``` C#
IValidator<Customer> validator = Validator.Composite(
    new CustomerValidator(),
    new CustomerAddressValidator());

Result<Customer> result = validator.Validate(customer);
```

A composite requires at least one validator. Instead of separate validators, it
also accepts any enumerable of validators.

### Empty

A validator without rules that accepts every model:

``` C#
IValidator<Customer> validator = Validator.Empty<Customer>();

Result<Customer> result = validator.Validate(customer); // always valid
```

## Testing

To validate a model with a validator of choice, and assert the outcome of
that, see [`Qowaiv.Validation.TestTools`](TestTools.md):

``` C#
model.ValidateWith(new CustomerValidator()).Should().BeValid();
```
