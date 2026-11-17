# Qowaiv validation with Microsoft.Extensions.Validation

`Qowaiv.Validation.Microsoft` bridges validation metadata from
`Microsoft.Extensions.Validation` to Qowaiv's `Result<T>`.

## ModelValidator&lt;TModel&gt;

`ModelValidator<TModel>` implements `Qowaiv.Validation.Abstractions.IValidator<TModel>`.
It resolves the validatable type information for `TModel` from the supplied
`ValidationOptions`, validates the model, and maps the reported errors to Qowaiv
validation messages.

``` C#
services.AddValidation();

using var provider = services.BuildServiceProvider();
var options = provider.GetRequiredService<IOptions<ValidationOptions>>().Value;

var validator = new ModelValidator<Customer>(options, provider);
Result<Customer> result = validator.Validate(customer);
```

When no validatable type information is registered for `TModel`, the validator
returns a valid result. Validation errors use their full validation path as the
Qowaiv message's property name.
