# Validation message
The common `Qowaiv.Validation.Abstractions.IValidationMessage` interface for
validation messages, with `Severity`, `PropertyName`, and `Message`
properties. Implementations are available in each package.

``` C#
public interface IValidationMessage
{
    ValidationSeverity Severity { get; }
    string? PropertyName { get; }
    string? Message { get; }
}
```

It maps to:
* [`FluentValidation.Results.ValidationFailure`](https://docs.fluentvalidation.net)
* [`System.ComponentModel.DataAnnotations.ValidationResult`](https://learn.microsoft.com/dotnet/api/system.componentmodel.dataannotations.validationresult)
* [`System.Xml.Schema.ValidationEventArgs`](https://learn.microsoft.com/dotnet/api/system.xml.schema.validationeventargs)

## Severity
The `ValidationSeverity` of a message is one of:

``` C#
public enum ValidationSeverity
{
    None = -1,
    Info = 0,
    Warning = 1,
    Error = 2,
}
```

The values are ordered by impact: a higher value is worse. Only rarely used
severities, such as `None`, are negative. This also keeps `Info`, the least
severe severity that is actually reported, at `0`, which makes
`ValidationMessages` sorting by severity (descending) straightforward.

`None` is basically no issue: it represents a successful message (so no
severity). Messages with this severity are never part of a `ValidationMessages`
collection.

It exists because the validation frameworks that are wrapped communicate "no
issue" as well, and not in a uniform way. `DataAnnotations`, for example,
communicates it with both `ValidationResult.Success` and `null`. Both are
treated as "nothing to report" (`ValidationMessage.None` represents
`ValidationResult.Success`, and `null` messages are ignored), so a validator does
not have to deal with them separately.

## Factory methods
`ValidationMessage` is a concrete, sealed implementation of `IValidationMessage`
that is created via factory methods:

``` C#
IValidationMessage error = ValidationMessage.Error("Name is required", "Name");
IValidationMessage warn = ValidationMessage.Warn("Deprecated field");
IValidationMessage info = ValidationMessage.Info("Auto-corrected");
IValidationMessage none = ValidationMessage.None;
```

The `PropertyName` is optional. To create a message for a severity that is only
known at runtime, use `ValidationMessage.For()`. Severities that are not `None`,
`Info`, or `Warning` result in an error message:

``` C#
IValidationMessage message = ValidationMessage.For(severity, "Something is off", "Name");
```

## Validation messages
The [append-only](https://en.wikipedia.org/wiki/Append-only) `Qowaiv.Validation.Abstractions.ValidationMessages`
validation message collection. It is grouped per severity: messages are ordered
from the most (Error) to the least (Info) severe, and `null` messages and
messages with severity `None` are ignored.

``` C#
ValidationMessages messages = [ValidationMessage.Info("Auto-corrected"), ValidationMessage.Error("Bad")];

messages.HasErrors; // true
messages.Errors;    // only the error messages
messages.Warnings;  // only the warning messages
messages.Infos;     // only the info messages
```

## Messages that are exceptions
The `Qowaiv.Validation.Messages` package provides `IValidationMessage`
implementations that are also exceptions. See the
[Messages README](../../src/Qowaiv.Validation.Messages/README.md) for details.

| Message              | Derives from                      | HTTP status                |
|----------------------|-----------------------------------|----------------------------|
| `PaymentRequired`    | `InvalidOperationException`       | 402 (`PaymentRequired`)    |
| `AccessDenied`       | `SecurityException`               | 403 (`Forbidden`)          |
| `ConcurrencyIssue`   | `InvalidOperationException`       | 409 (`Conflict`)           |
| `EntityNotFound`     | `InvalidOperationException`       | 404 (`NotFound`)           |
| `ServiceUnavailable` | `Exception`                       | 503 (`ServiceUnavailable`) |

## HTTP status
A validation message that can be bound to an [HTTP status](https://en.wikipedia.org/wiki/List_of_HTTP_status_codes)
implements `IHttpStatusMessage`, an `IValidationMessage` that also has a
`System.Net.HttpStatusCode`. This is a separate interface because most messages
(DataAnnotations, FluentValidation, XML) have no HTTP meaning.

``` C#
public interface IHttpStatusMessage : IValidationMessage
{
    HttpStatusCode StatusCode { get; }
}
```

This allows a web layer to determine the status without knowing the concrete
message type:

``` C#
if (message is IHttpStatusMessage { StatusCode: var status })
{
    return status;
}
```
