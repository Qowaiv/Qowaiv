# Qowaiv exensions on FluentValidation.NET
![FluentValidation.NET](https://raw.githubusercontent.com/JeremySkinner/FluentValidation/master/docs/assets/images/logo/fluent-validation-logo.png)

## Validators

### Not unknown
The `NotUnknownValidator` validates that a value does not equal the Unknown
value (if existing of course). Accessible via the fluent syntax.

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

### Email address should be IP-based
The `NoIPBasedEmailAddressValidator` validates that an `EmailAddress`
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

### PostalCode valid for specific country
The `PostalCodeValidator` validates that a `PostalCode` value is valid for
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
