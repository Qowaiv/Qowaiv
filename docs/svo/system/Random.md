# System.Random
The [`Random`](https://learn.microsoft.com/dotnet/api/system.random) class
can be used to generate [_pseudo_-random](https://en.wikipedia.org/wiki/Pseudorandomness)
data. The generation of the following SVOs is supported:

## Sequence
To simplify sequences of random data:

``` C#
// Calls `Next()` an infinite times.
foreach (var value in rnd.NextSequance(r => r.Next())
{
    // ...
}
```

## [International Bank Account Number](../InternationalBankAccountNumber.md)
``` C#
// Generates an IBAN for a random supported country.
var iban = rnd.NextInternationalBankAccountNumber();

// Generates an IBAN for the given country.
var iban = rnd.NextInternationalBankAccountNumber(Country.NL);
```

## [Burgerservicenummer](../TODO.md)
``` C#
var bsn = rnd.NextBurgerservicenummer();
```

## [RSIN](../TODO.md)
``` C#
var rsin = rnd.NextRsin();
```
