# Qowaiv exensions on FluentValidation.NET
![FluentValidation.NET](https://raw.githubusercontent.com/JeremySkinner/FluentValidation/master/docs/assets/images/logo/fluent-validation-logo.png)

## Validators

### NotUnknown validator
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

### PostalCode validator
The `PostalCodeValidator` validates that a `PostalCode` value is valid for
a specific `Country`, both static and via another property. Accessible via the
fluent syntax.

``` C#
public class CustomValidator : AbstractValidator<Model>
{
    public CustomValidator()
    {
        RuleFor(m => m.PostalCod).ValidFor(m => m.Country);
    }
}
```
