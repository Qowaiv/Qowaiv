# System.Random
The [`Random`](https://learn.microsoft.com/dotnet/api/system.random) class
can be used to generate [_pseudo_-random](https://en.wikipedia.org/wiki/Pseudorandomness)
data. The generation of the following SVOs is supported:

## [International Bank Account Number](../InternationalBankAccountNumber.md)
``` C#
// Generates an IBAN for a random supported country.
var iban = rnd.NextInternationalBankAccountNumber();

// Generates an IBAN for the given country.
var iban = rnd.NextInternationalBankAccountNumber(Country.NL);
```
