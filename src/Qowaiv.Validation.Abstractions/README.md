# Qowaiv validation abstractions
There are multiple ways to support validation within .NET. Most notable are
* [System.ComponentModel](https://www.nuget.org/packages/System.ComponentModel)
* [FluentValidation.NET](https://fluentvalidation.net)

To prevent a vendor lock-in, `Qowaiv.Validation.Abstractions` has been introduced.
To achieve that the following is added:

### IValidator&lt;TModel&gt;
The main interface. Validates a model and returns the validation result.
``` C#
namespace Qowaiv.Validation.Abstractions
{
    public interface IValidator<TModel>
    {
        Result<TModel> Validate(TModel model);
    }
}
```

### Result and Result&lt;TModel&gt;
A (validation) result, containing validation messages. Creation is only via
factory methods.
``` C#
namespace Qowaiv.Validation.Abstractions
{
    public class Result
    {
        public IReadOnlyList<IValidationMessage> Messages { get; }
        public bool IsValid => !Errors.Any();

        public IEnumerable<IValidationMessage> Errors => Messages.GetErrors();
        public IEnumerable<IValidationMessage> Warnings => Messages.GetWarnings();
        public IEnumerable<IValidationMessage> Infos => Messages.GetInfos();

        public static Result OK => new Result(Enumerable.Empty<IValidationMessage>());

        public static Result<T> For<T>(T data, params IValidationMessage[] messages) => new Result<T>(data, messages);
        public static Result WithMessages(params IValidationMessage[] messages) => new Result(messages);
        public static Result<T> WithMessages<T>(params IValidationMessage[] messages) => new Result<T>(default, messages);
    }
}
```

The generic result contains also the value/validated model. This `Value` is
only accessible when the model is valid, otherwise, an `InvalidModelException`
is thrown. This exception contains the validation messages.

``` C#
namespace Qowaiv.Validation.Abstractions
{
    public sealed class Result<T> : Result
    {
        public T Value => IsValid
            ? _value
            : throw InvalidModelException.For<T>(Errors);
    }
}
```

Typical use cases are:

``` C#
Result<DataType> result = Result.For(data);
Result<DataType> resultWithMessages = Result.For(data, messages);
```

### IValidationMessage
The common ground of validation messages.
``` C#
namespace Qowaiv.Validation.Abstractions
{
    public interface IValidationMessage
    {
        ValidationSeverity Severity { get; }
        string PropertyName { get; }
        string Message { get; }
    }
}
```
