# Qowaiv extensions on [*Fluent Validation*](https://fluentvalidation.net)
Qowaiv provides a set of extensions on top of the FluentValidation library. It
allows using FluentValidation in combination with Qowaiv's `Result<T>`.

## ModelValidator base class
The `ModelValidator<TModel>` is a base class that bridges a FluentValidation
`AbstractValidator<TModel>` to the `Qowaiv.Validation.Abstractions.IValidator<TModel>`
interface. Extend it to create validators that return `Result<TModel>`:

``` C#
public class CustomerValidator : ModelValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(c => c.Name).Required();
        RuleFor(c => c.Email).NotEmptyOrUnknown();
        RuleFor(c => c.Age).IsPositive();
    }
}

// Usage:
IValidator<Customer> validator = new CustomerValidator();
Result<Customer> result = validator.Validate(customer);
```

## Validators
There is a set of general-purpose validators to validate properties of a model.

### Required
The `RequiredValidation` validates that a required property has a set value. If
specified, an unknown value is considered set; by default it is not.

``` C#
public class CustomValidator : AbstractValidator<Model>
{
    public CustomValidator()
    {
        RuleFor(m => m.Email).Required();
        RuleFor(m => m.Iban).Required(allowUnknown: true);
    }
}
```

### Not unknown
The `UnknownValidation` validates that a value does not equal the Unknown
value (if one exists). Accessible via the fluent syntax.

``` C#
public class CustomValidator : AbstractValidator<Model>
{
    public CustomValidator()
    {
        RuleFor(m => m.Email).NotEmptyOrUnknown();
        RuleFor(m => m.Iban).NotUnknown();
    }
}
```

### (Not) positive/negative
The `NumberValidation` validates that numbers are greater/smaller than, or equal
to zero.

``` C#
public class CustomValidator : AbstractValidator<Model>
{
    public CustomValidator()
    {
        RuleFor(m => m.Number).IsPositive();
        RuleFor(m => m.Number).IsNegative();
        RuleFor(m => m.Number).IsNotPositive();
        RuleFor(m => m.Number).IsNotNegative();
    }
}
```

### (Not) before and (not) after
Using the phrasing `'{PropertyName}' should be after {Value}` instead of
`'{PropertyName}' should be greater than {Value}` makes sense for a wide range
of property types, including date (time) related values.

``` C#
public class CustomValidator : AbstractValidator<Model>
{
    public CustomValidator()
    {
        RuleFor(m => m.EndDate).After(m => Clock.Today().AddYears(20));
        RuleFor(m => m.EndDate).NotAfter(m => Clock.Today().AddYears(20));
        RuleFor(m => m.EndDate).Before(m => Clock.Today().AddYears(20));
        RuleFor(m => m.EndDate).NotBefore(m => Clock.Today().AddYears(20));
    }
}
```

### Relative to the clock

The `ClockValidation` validates if a date (time) is in the past, or future.
It supports `Date`, `DateTime`, `Date?`, and `DateTime?`, and the provision
of custom date (time) provider. By Default, `Clock.UtcNow()` and `Clock.Today()`
are used.

``` C#
public class CustomValidator : AbstractValidator<Model>
{
    public CustomValidator()
    {
        RuleFor(m => m.Date1).InFuture();
        RuleFor(m => m.Date2).InPast();
        RuleFor(m => m.Date3).NotInFuture();
        RuleFor(m => m.Date4).NotInPast(() => CustomDateProvider());
    }
}
```

### Postal code valid for a specific country
The `PostalCodeValidation` validates that a `PostalCode` value is valid for
a specific `Country`, both static and via another property.

``` C#
public class CustomValidator : AbstractValidator<Model>
{
    public CustomValidator()
    {
        RuleFor(m => m.PostalCode).ValidFor(m => m.Country);
    }
}
```

### Finite floating points
The `FloatingPointValidation` validates that numbers are finite.

``` C#
public class CustomValidator : AbstractValidator<Model>
{
    public CustomValidator()
    {
        RuleFor(m => m.Number).IsFinite();
    }
}
```
### Email address should not be IP-based
The `EmailAddressValidation` validates that an `EmailAddress`
does not have an IP-based domain.

``` C#
public class CustomValidator : AbstractValidator<Model>
{
    public CustomValidator()
    {
        RuleFor(m => m.Email).NotIPBased();
    }
}
```
