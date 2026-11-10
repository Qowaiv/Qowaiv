# Result.Act
Using [`Result<T>`](Result.md) allows to apply _[Monadic](https://en.wikipedia.org/wiki/Monad_(functional_programming))
Composition/Railway Oriented Programming_.  A chain of actions is executed while
the result is valid, and messages accumulate:

``` C#
Result<DataType> result = GetModel()
    .Act(m => ValidateName(m))
    .Act(m => ValidateAge(m));
```

## Asynchronous

Async versions for all `Act` overloads. They return a `Task<Result<TModel>>`,
which can be awaited to obtain the resulting `Result<TModel>`:

``` C#
Task<Result<DataType>> promise = Result.For(model)
    .ActAsync(m => FetchDetailsAsync(m))
    .ActAsync(m => SaveAsync(m));
```

## Act with a transform

An action can also return a plain value, transforming the model (or a related
value) without producing messages. A `null` transform result makes the result
throw a `NoValue` exception:

``` C#
Result<Name> name = GetPerson()
    .Act(p => p.Name);         // TOut is Name
```

## Pipe operator

The `|` operator provides an alternative syntax for `Act`:

``` C#
Result<DataType> result = GetModel()
    | ValidateName
    | ValidateAge;

// where
Result<DataType> ValidateName(DataType data) => ...
Result<DataType> ValidateAge(DataType data) => ...
```

The limitation is that `TIn` and `TOut` must be the same type.

## Act with context update

For mutable or immutable shared context patterns:

``` C#
// Mutable update
Result<Context> ctx = NewContext()
    .Act(c => Service.GetValue(), (c, value) => c.Value = value);

// Immutable update
Result<Context> ctx = NewContext()
    .Act(c => Service.GetValue(), (c, value) => c.Update(value));
```
