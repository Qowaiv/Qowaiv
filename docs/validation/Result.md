# Result and Result&lt;TModel&gt;

A validation result containing messages. Created only via factory methods.

``` C#
Result ok = Result.OK;
Result ok_ = Result.WithMessages(ValidationMessage.Warn("Check"));
Result<DataType> withValue = Result.For(value);
Result<DataType> withErrors = Result.For(value, ValidationMessage.Error("Bad"));
Result<DataType> nullResult = Result.Null<DataType>();
Result<DataType> withMessages = Result.WithMessages<DataType>(ValidationMessage.Error("Check"));
Task<Result<DataType>> asyncValue = Result.Async(value);
```
A `Result<TModel>` can be turned into a `Task<Result<TModel>>` with `Async()`.

## Properties

* `Messages` — all [validation messages](ValidationMessage).
* `IsValid` — `true` when there are no error-severity messages.
* `Errors` / `Warnings` / `Infos` — filtered views by severity.

## Value access

The generic `Result<TModel>` wraps a `Value` that is only accessible when
valid. Accessing `Value` on an invalid result throws [`InvalidModel`](InvalidModel.md).

``` C#
Result<MyModel> result = Validate(model);
if (result.IsValid)
{
    MyModel value = result.Value; // safe
}
```

## Throw if invalid

When you want to fail fast:

``` C#
result.ThrowIfInvalid(); // throws InvalidModel if invalid
```

## Implicit and explicit casts

``` C#
// Implicit: valid values auto-wrap into Result<T>
Result<MyModel> implicit = validModel;

// Explicit: extracts the value, throws if invalid
MyModel explicit = (MyModel)result;

// Cast: changes the wrapped type (must be a subclass)
Result<DerivedModel> casted = result.Cast<DerivedModel>();
```

## Combining Results
Both collections of `Result` and collections of `Task<Result>` can be combined:

``` C#
Result[] f = [..];
Result[] s = [..];

Result combined = Results.Combine([f, s]);
```

And async:

``` C#
Task<Result>[] f = [..];
Task<Result>[] s = [..];

Result combined = await Results.Combine([f, s]);
```
