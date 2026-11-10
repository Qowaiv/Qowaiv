# Qowaiv Validation Messages
This package contains `Qowaiv.Validation.Abstractions.IValidationMessage` implementations
that are also exceptions.

## Access Denied
A specific type of `System.Security.SecurityException`.

## Concurrency Issue
To communicate a version mismatch:
``` csharp
var message = ConcurrencyIssue.VersionMismatch(expectedVersion: 42, actualVersion: 43);
```

To communicate a mid-air collision:
``` csharp
var message = ConcurrencyIssue.MidAirCollision();
```

## Entity Not Found
To communicate an entity was not found:

``` csharp
var message = EntityNotFound.ForId(Uuid.Parse("Zj5sozHNSIapCiGm7YqJbQ"));
```

or:

``` csharp
var message = EntityNotFound.For<SomeEntity>(id: 42);
```
## Payment Required
To communicate that a payment is required to proceed (HTTP 402).
A specific type of `System.InvalidOperationException`:

``` csharp
var message = new PaymentRequired();
```

## Service Unavailable
To communicate which (dependent) service was unavailable:

``` csharp
var message = ServiceUnavailable.WithName("Some service");
```
