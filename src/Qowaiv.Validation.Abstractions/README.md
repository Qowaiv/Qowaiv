# Qowaiv Validation Abstractions

Core abstractions for model validation. Provides a vendor-lock-in-free
`IValidator<TModel>` / `Result<TModel>` abstraction that DataAnnotations,
FluentValidation, and XML validators all implement.

## IValidator&lt;TModel&gt;

The main interface. Validates a model and returns the validation result.

``` C#
public interface IValidator<TModel>
{
    Result<TModel> Validate(TModel model);
}
```

### Validator.Composite

To combine validators you can create a composite validator:

``` C#
var composite = Validator.Composite(new SpecialValidator(), new CustomValidator());
```

### Validator.Empty

To create a validator that always returns a valid result:

``` C#
IValidator<MyModel> validator = Validator.Empty<MyModel>();
Result<MyModel> result = validator.Validate(model); // always valid
```

## Result and Result&lt;TModel&gt;

A validation result containing messages. Created only via factory methods.

``` C#
Result<DataType> ok = Result.OK;
Result<DataType> withValue = Result.For(value);
Result<DataType> withErrors = Result.For(value, ValidationMessage.Error("Bad"));
Result<DataType> nullResult = Result.Null<DataType>();
Result<DataType> withMessages = Result.WithMessages<DataType>(ValidationMessage.Warn("Check"));
AsyncResult<DataType> asyncValue = Result.Async(value);
```

The `Async<T>()` factory creates an [`AsyncResult<T>`](#asyncresulttmodel).
An existing `Result<TModel>` can be turned into an `AsyncResult<TModel>` with
`Async()` (the older `AsTask()` is obsolete).

### Properties

- `Messages` — all validation messages
- `IsValid` — `true` when there are no error-severity messages
- `Errors` / `Warnings` / `Infos` — filtered views by severity

### Value access

The generic `Result<TModel>` wraps a `Value` that is only accessible when
valid. Accessing `Value` on an invalid result throws `InvalidModel`.

``` C#
Result<MyModel> result = Validate(model);
if (result.IsValid)
{
    MyModel value = result.Value; // safe
}
```

### Throw if invalid

When you want to fail fast:

``` C#
result.ThrowIfInvalid(); // throws InvalidModel if invalid
```

### Implicit and explicit casts

``` C#
// Implicit: valid values auto-wrap into Result<T>
Result<MyModel> implicit = validModel;

// Explicit: extracts the value, throws if invalid
MyModel explicit = (MyModel)result;

// Cast: changes the wrapped type (must be a subclass)
Result<DerivedModel> casted = result.Cast<DerivedModel>();
```

## Result composition

### Act

Chain multiple validation/transformation steps. Subsequent actions execute
only while the result is valid, and messages accumulate:

``` C#
Result<DataType> result = GetModel()
    .Act(m => ValidateName(m))
    .Act(m => ValidateAge(m));
```

### ActAsync

Async versions for all `Act` overloads. They return an `AsyncResult<TModel>`,
which can be awaited to obtain the resulting `Result<TModel>`:

``` C#
AsyncResult<DataType> async = Result.For(model)
    .ActAsync(m => FetchDetailsAsync(m))
    .ActAsync(m => SaveAsync(m));

Result<DataType> result = await async;
```

The invoked actions may return `Result<TOut>`, `AsyncResult<TOut>`, or
`AsyncResult` (see [`AsyncResult<TModel>`](#asyncresulttmodel)).

### Act with a transform

An action can also return a plain value, transforming the model (or a related
value) without producing messages. A `null` transform result makes the result
throw `NoValue`:

``` C#
Result<Name> name = GetPerson()
    .Act(p => p.Name);         // TOut is Name
```

### Pipe operator

The `|` operator provides an alternative syntax for `Act`:

``` C#
Result<DataType> result = GetModel()
    | ValidateName
    | ValidateAge;
```

It also supports a pure `TModel`-to-`TModel` transform on both `Result<TModel>`
and `AsyncResult<TModel>`:

``` C#
Result<DataType> result = GetModel() | Normalize;
```

### Act with context update

For mutable or immutable shared context patterns:

``` C#
// Mutable update
Result<Context> ctx = NewContext()
    .Act(c => Service.GetValue(), (c, value) => c.Value = value);

// Immutable update
Result<Context> ctx = NewContext()
    .Act(c => Service.GetValue(), (c, value) => c.Update(value));
```

## AsyncResult&lt;TModel&gt;

`AsyncResult<TModel>` represents an operation that produces a `Result<TModel>`
at some time in the future. It is a lightweight readonly struct that wraps a
`Task<Result<TModel>>`, is awaitable, and exposes the same composition
(`ActAsync` and `|`) as `Result<TModel>`.

### Creating

An `AsyncResult<TModel>` is created from a `Result<TModel>` via `Async()`, via
the `Result.Async<T>()` factory, or implicitly from a `Task<Result<TModel>>`:

``` C#
AsyncResult<MyModel> fromResult = Result.For(model).Async();
AsyncResult<MyModel> fromFactory = Result.Async(model);
AsyncResult<MyModel> fromFactoryWithMessages = Result.Async(model, ValidationMessage.Warn("Check"));
AsyncResult<MyModel> fromTask = Task.FromResult(Result.For(model));
```

### Awaiting

Because `AsyncResult<TModel>` is awaitable, awaiting it yields the underlying
`Result<TModel>`. It also mirrors the underlying task via `IsCompleted` and
`IsCanceled`:

``` C#
Result<MyModel> result = await Result.Async(model);
```

### Composition

`ActAsync` runs the next step only while the result is valid and merges the
messages, exactly like `Act` on `Result<TModel>`:

``` C#
Result<DataType> result = await Result.Async(model)
    .ActAsync(m => FetchDetailsAsync(m))
    .ActAsync(m => SaveAsync(m));
```

The `|` operator works on `AsyncResult<TModel>` as well:

``` C#
Result<DataType> result = await (Result.Async(model)
    | ValidateName
    | SaveAsync);
```

## IValidationMessage

The common interface for all validation messages.

``` C#
public interface IValidationMessage
{
    ValidationSeverity Severity { get; }
    string? PropertyName { get; }
    string? Message { get; }
}
```

## ValidationMessage

A concrete, sealed implementation with factory methods:

``` C#
IValidationMessage error = ValidationMessage.Error("Name is required", "Name");
IValidationMessage warn = ValidationMessage.Warn("Deprecated field");
IValidationMessage info = ValidationMessage.Info("Auto-corrected");
IValidationMessage none = ValidationMessage.None; // null
```

## ValidationSeverity

``` C#
public enum ValidationSeverity
{
    Info = 0,
    Warning = 1,
    Error = 2,
}
```
